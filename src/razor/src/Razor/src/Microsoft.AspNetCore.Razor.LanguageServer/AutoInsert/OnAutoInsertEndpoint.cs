// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

[LanguageServerEndpoint(VSInternalMethods.OnAutoInsertName)]
internal class OnAutoInsertEndpoint(
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IRazorDocumentMappingService documentMappingService,
    IClientConnection clientConnection,
    IEnumerable<IOnAutoInsertProvider> onAutoInsertProvider,
    IOptionsMonitor<RazorLSPOptions> optionsMonitor,
    IRazorLoggerFactory loggerFactory)
    : AbstractRazorDelegatingEndpoint<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem?>(languageServerFeatureOptions, documentMappingService, clientConnection, loggerFactory.CreateLogger<OnAutoInsertEndpoint>()), ICapabilitiesProvider
{
    private static readonly HashSet<string> s_htmlAllowedTriggerCharacters = new(StringComparer.Ordinal) { "=", };
    private static readonly HashSet<string> s_cSharpAllowedTriggerCharacters = new(StringComparer.Ordinal) { "'", "/", "\n" };

    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
    private readonly IReadOnlyList<IOnAutoInsertProvider> _onAutoInsertProviders = onAutoInsertProvider?.ToList() ?? throw new ArgumentNullException(nameof(onAutoInsertProvider));

    protected override string CustomMessageTarget => CustomMessageNames.RazorOnAutoInsertEndpointName;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        var triggerCharacters = _onAutoInsertProviders.Select(provider => provider.TriggerCharacter);

        if (_languageServerFeatureOptions.SingleServerSupport)
        {
            triggerCharacters = triggerCharacters.Concat(s_htmlAllowedTriggerCharacters).Concat(s_cSharpAllowedTriggerCharacters);
        }

        serverCapabilities.OnAutoInsertProvider = new VSInternalDocumentOnAutoInsertOptions()
        {
            TriggerCharacters = triggerCharacters.Distinct().ToArray()
        };
    }

    protected override async Task<VSInternalDocumentOnAutoInsertResponseItem?> TryHandleAsync(VSInternalDocumentOnAutoInsertParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);
        if (codeDocument.IsUnsupported())
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var character = request.Character;

        var applicableProviders = new List<IOnAutoInsertProvider>();
        for (var i = 0; i < _onAutoInsertProviders.Count; i++)
        {
            var formatOnTypeProvider = _onAutoInsertProviders[i];
            if (formatOnTypeProvider.TriggerCharacter == character)
            {
                applicableProviders.Add(formatOnTypeProvider);
            }
        }

        if (applicableProviders.Count == 0)
        {
            // There's currently a bug in the LSP platform where other language clients OnAutoInsert trigger characters influence every language clients trigger characters.
            // To combat this we need to preemptively return so we don't try having our providers handle characters that they can't.
            return null;
        }

        var uri = request.TextDocument.Uri;
        var position = request.Position;

        var workspaceFactory = requestContext.GetRequiredService<IAdhocWorkspaceFactory>();
        using (var formattingContext = FormattingContext.Create(uri, documentContext.Snapshot, codeDocument, request.Options, workspaceFactory))
        {
            for (var i = 0; i < applicableProviders.Count; i++)
            {
                if (applicableProviders[i].TryResolveInsertion(position, formattingContext, out var textEdit, out var format))
                {
                    return new VSInternalDocumentOnAutoInsertResponseItem()
                    {
                        TextEdit = textEdit,
                        TextEditFormat = format,
                    };
                }
            }
        }

        // No provider could handle the text edit.
        return null;
    }

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(VSInternalDocumentOnAutoInsertParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        if (positionInfo.LanguageKind == RazorLanguageKind.Html)
        {
            if (!s_htmlAllowedTriggerCharacters.Contains(request.Character))
            {
                Logger.LogInformation("Inapplicable HTML trigger char {request.Character}.", request.Character);
                return Task.FromResult<IDelegatedParams?>(null);
            }

            if (!_optionsMonitor.CurrentValue.AutoInsertAttributeQuotes && request.Character == "=")
            {
                // Use Razor setting for autoinsert attribute quotes. HTML Server doesn't have a way to pass that
                // information along so instead we just don't delegate the request.
                Logger.LogTrace("Not delegating to HTML completion because AutoInsertAttributeQuotes is disabled");
                return Task.FromResult<IDelegatedParams?>(null);
            }
        }
        else if (positionInfo.LanguageKind == RazorLanguageKind.CSharp)
        {
            if (!s_cSharpAllowedTriggerCharacters.Contains(request.Character))
            {
                Logger.LogInformation("Inapplicable C# trigger char {request.Character}.", request.Character);
                return Task.FromResult<IDelegatedParams?>(null);
            }

            // Special case for C# where we use AutoInsert for two purposes:
            // 1. For XML documentation comments (filling out the template when typing "///")
            // 2. For "on type formatting" style behaviour, like adjusting indentation when pressing Enter inside empty braces
            //
            // If users have turned off on-type formatting, they don't want the behaviour of number 2, but its impossible to separate
            // that out from number 1. Typing "///" could just as easily adjust indentation on some unrelated code higher up in the
            // file, which is exactly the behaviour users complain about.
            //
            // Therefore we are just going to no-op if the user has turned off on type formatting. Maybe one day we can make this
            // smarter, but at least the user can always turn the setting back on, type their "///", and turn it back off, without
            // having to restart VS. Not the worst compromise (hopefully!)
            if (!_optionsMonitor.CurrentValue.FormatOnType)
            {
                Logger.LogInformation("Formatting on type disabled, so auto insert is a no-op for C#.");
                return Task.FromResult<IDelegatedParams?>(null);
            }
        }

        return Task.FromResult<IDelegatedParams?>(new DelegatedOnAutoInsertParams(
            documentContext.Identifier,
            positionInfo.Position,
            positionInfo.LanguageKind,
            request.Character,
            request.Options));
    }

    protected override async Task<VSInternalDocumentOnAutoInsertResponseItem?> HandleDelegatedResponseAsync(
        VSInternalDocumentOnAutoInsertResponseItem? delegatedResponse,
        VSInternalDocumentOnAutoInsertParams originalRequest,
        RazorRequestContext requestContext,
        DocumentPositionInfo positionInfo,
        CancellationToken cancellationToken)
    {
        if (delegatedResponse is null)
        {
            return null;
        }

        var documentContext = requestContext.GetRequiredDocumentContext();

        // For Html we just return the edit as is
        if (positionInfo.LanguageKind == RazorLanguageKind.Html)
        {
            return delegatedResponse;
        }

        // For C# we run the edit through our formatting engine
        var edits = new[] { delegatedResponse.TextEdit };

        var razorFormattingService = requestContext.GetRequiredService<IRazorFormattingService>();
        TextEdit[] mappedEdits;
        if (delegatedResponse.TextEditFormat == InsertTextFormat.Snippet)
        {
            mappedEdits = await razorFormattingService.FormatSnippetAsync(documentContext, positionInfo.LanguageKind, edits, originalRequest.Options, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            mappedEdits = await razorFormattingService.FormatOnTypeAsync(documentContext, positionInfo.LanguageKind, edits, originalRequest.Options, hostDocumentIndex: 0, triggerCharacter: '\0', cancellationToken).ConfigureAwait(false);
        }

        if (mappedEdits.Length != 1)
        {
            return null;
        }

        return new VSInternalDocumentOnAutoInsertResponseItem()
        {
            TextEdit = mappedEdits[0],
            TextEditFormat = delegatedResponse.TextEditFormat,
        };
    }
}
