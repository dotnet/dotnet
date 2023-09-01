// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text.Adornments;

namespace Microsoft.AspNetCore.Razor.LanguageServer.FindAllReferences;

[LanguageServerEndpoint(Methods.TextDocumentReferencesName)]
internal sealed class FindAllReferencesEndpoint : AbstractRazorDelegatingEndpoint<ReferenceParams, VSInternalReferenceItem[]>, ICapabilitiesProvider
{
    private readonly FilePathService _filePathService;
    private readonly IRazorDocumentMappingService _documentMappingService;

    public FindAllReferencesEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        IRazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        ILoggerFactory loggerFactory,
        FilePathService filePathService)
        : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<FindAllReferencesEndpoint>())
    {
        _filePathService = filePathService ?? throw new ArgumentNullException(nameof(filePathService));
        _documentMappingService = documentMappingService ?? throw new ArgumentNullException(nameof(documentMappingService));
    }

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        serverCapabilities.ReferencesProvider = new ReferenceOptions()
        {
            // https://github.com/dotnet/razor/issues/8033
            WorkDoneProgress = false,
        };
    }

    protected override string CustomMessageTarget => CustomMessageNames.RazorReferencesEndpointName;

    protected override bool PreferCSharpOverHtmlIfPossible => true;

    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferAttributeNameDocumentPositionInfoStrategy.Instance;

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(ReferenceParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        // HTML doesn't need to do FAR
        if (positionInfo.LanguageKind != RazorLanguageKind.CSharp)
        {
            return Task.FromResult<IDelegatedParams?>(null);
        }

        var documentContext = requestContext.GetRequiredDocumentContext();
        return Task.FromResult<IDelegatedParams?>(new DelegatedPositionParams(
                documentContext.Identifier,
                positionInfo.Position,
                positionInfo.LanguageKind));
    }

    protected override async Task<VSInternalReferenceItem[]> HandleDelegatedResponseAsync(VSInternalReferenceItem[] delegatedResponse, ReferenceParams originalRequest, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var remappedLocations = new List<VSInternalReferenceItem>();

        foreach (var referenceItem in delegatedResponse)
        {
            if (referenceItem?.Location is null || referenceItem.Text is null)
            {
                continue;
            }

            // Temporary fix for codebehind leaking through
            // Revert when https://github.com/dotnet/aspnetcore/issues/22512 is resolved
            referenceItem.DefinitionText = FilterReferenceDisplayText(referenceItem.DefinitionText);
            referenceItem.Text = FilterReferenceDisplayText(referenceItem.Text);

            // Indicates the reference item is directly available in the code
            referenceItem.Origin = VSInternalItemOrigin.Exact;

            if (!_filePathService.IsVirtualCSharpFile(referenceItem.Location.Uri) &&
                !_filePathService.IsVirtualHtmlFile(referenceItem.Location.Uri))
            {
                // This location doesn't point to a virtual file. No need to remap.
                remappedLocations.Add(referenceItem);
                continue;
            }

            var (itemUri, mappedRange) = await _documentMappingService.MapToHostDocumentUriAndRangeAsync(referenceItem.Location.Uri, referenceItem.Location.Range, cancellationToken).ConfigureAwait(false);

            referenceItem.Location.Uri = itemUri;
            referenceItem.DisplayPath = itemUri.AbsolutePath;
            referenceItem.Location.Range = mappedRange;

            remappedLocations.Add(referenceItem);
        }

        return remappedLocations.ToArray();
    }

    /// <summary>
    /// If the reference text is showing a generated identifier (such as "__o =") this
    /// fixes it to be what the actual reference display would look like to a user.
    /// See https://github.com/dotnet/razor/issues/4611 for more details on what this fixes
    /// </summary>
    private static object? FilterReferenceDisplayText(object? referenceText)
    {
        const string CodeBehindObjectPrefix = "__o = ";
        const string CodeBehindBackingFieldSuffix = "k__BackingField";

        if (referenceText is string text)
        {
            if (text.StartsWith(CodeBehindObjectPrefix, StringComparison.Ordinal))
            {
                return text
                    .Substring(CodeBehindObjectPrefix.Length, text.Length - CodeBehindObjectPrefix.Length - 1); // -1 for trailing `;`
            }

            return text.Replace(CodeBehindBackingFieldSuffix, string.Empty);
        }

        if (referenceText is ClassifiedTextElement textElement &&
            FilterReferenceClassifiedRuns(textElement.Runs.ToArray()))
        {
            var filteredRuns = textElement.Runs.Skip(4); // `__o`, ` `, `=`, ` `
            filteredRuns = filteredRuns.Take(filteredRuns.Count() - 1); // Trailing `;`
            return new ClassifiedTextElement(filteredRuns);
        }

        return referenceText;
    }

    private static bool FilterReferenceClassifiedRuns(IReadOnlyList<ClassifiedTextRun> runs)
    {
        if (runs.Count < 5)
        {
            return false;
        }

        return VerifyRunMatches(runs[0], "field name", "__o") &&
            VerifyRunMatches(runs[1], "text", " ") &&
            VerifyRunMatches(runs[2], "operator", "=") &&
            VerifyRunMatches(runs[3], "text", " ") &&
            VerifyRunMatches(runs[runs.Count - 1], "punctuation", ";");

        static bool VerifyRunMatches(ClassifiedTextRun run, string expectedClassificationType, string expectedText)
        {
            return run.ClassificationTypeName == expectedClassificationType &&
                run.Text == expectedText;
        }
    }
}
