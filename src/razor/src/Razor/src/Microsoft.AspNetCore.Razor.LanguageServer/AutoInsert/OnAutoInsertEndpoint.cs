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
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

[LanguageServerEndpoint(VSInternalMethods.OnAutoInsertName)]
internal class OnAutoInsertEndpoint : AbstractRazorDelegatingEndpoint<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem?>, IRegistrationExtension
{
    private static readonly HashSet<string> s_htmlAllowedTriggerCharacters = new(StringComparer.Ordinal) { "=", };
    private static readonly HashSet<string> s_cSharpAllowedTriggerCharacters = new(StringComparer.Ordinal) { "'", "/", "\n" };

    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;
    private readonly IOptionsMonitor<RazorLSPOptions> _optionsMonitor;
    private readonly IReadOnlyList<IOnAutoInsertProvider> _onAutoInsertProviders;

    public OnAutoInsertEndpoint(
        LanguageServerFeatureOptions languageServerFeatureOptions,
        RazorDocumentMappingService documentMappingService,
        ClientNotifierServiceBase languageServer,
        IEnumerable<IOnAutoInsertProvider> onAutoInsertProvider,
        IOptionsMonitor<RazorLSPOptions> optionsMonitor,
        ILoggerFactory loggerFactory)
        : base(languageServerFeatureOptions, documentMappingService, languageServer, loggerFactory.CreateLogger<OnAutoInsertEndpoint>())
    {
        _languageServerFeatureOptions = languageServerFeatureOptions ?? throw new ArgumentNullException(nameof(languageServerFeatureOptions));
        _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
        _onAutoInsertProviders = onAutoInsertProvider?.ToList() ?? throw new ArgumentNullException(nameof(onAutoInsertProvider));
    }

    protected override string CustomMessageTarget => RazorLanguageServerCustomMessageTargets.RazorOnAutoInsertEndpointName;

    public RegistrationExtensionResult GetRegistration(VSInternalClientCapabilities clientCapabilities)
    {
        const string AssociatedServerCapability = "_vs_onAutoInsertProvider";

        var triggerCharacters = _onAutoInsertProviders.Select(provider => provider.TriggerCharacter);

        if (_languageServerFeatureOptions.SingleServerSupport)
        {
            triggerCharacters = triggerCharacters.Concat(s_htmlAllowedTriggerCharacters).Concat(s_cSharpAllowedTriggerCharacters);
        }

        var registrationOptions = new VSInternalDocumentOnAutoInsertOptions()
        {
            TriggerCharacters = triggerCharacters.Distinct().ToArray()
        };

        return new RegistrationExtensionResult(AssociatedServerCapability, registrationOptions);
    }

    protected override async Task<VSInternalDocumentOnAutoInsertResponseItem?> TryHandleAsync(VSInternalDocumentOnAutoInsertParams request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
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

        var workspaceFactory = requestContext.GetRequiredService<AdhocWorkspaceFactory>();
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

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(VSInternalDocumentOnAutoInsertParams request, RazorRequestContext requestContext, Projection projection, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.GetRequiredDocumentContext();
        if (projection.LanguageKind == RazorLanguageKind.Html &&
           !s_htmlAllowedTriggerCharacters.Contains(request.Character))
        {
            Logger.LogInformation("Inapplicable HTML trigger char {request.Character}.", request.Character);
            return Task.FromResult<IDelegatedParams?>(null);
        }
        else if (projection.LanguageKind == RazorLanguageKind.CSharp)
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
                requestContext.Logger.LogInformation("Formatting on type disabled, so auto insert is a no-op for C#.");
                return Task.FromResult<IDelegatedParams?>(null);
            }
        }

        return Task.FromResult<IDelegatedParams?>(new DelegatedOnAutoInsertParams(
            documentContext.Identifier,
            projection.Position,
            projection.LanguageKind,
            request.Character,
            request.Options));
    }

    protected override async Task<VSInternalDocumentOnAutoInsertResponseItem?> HandleDelegatedResponseAsync(
        VSInternalDocumentOnAutoInsertResponseItem? delegatedResponse,
        VSInternalDocumentOnAutoInsertParams originalRequest,
        RazorRequestContext requestContext,
        Projection projection,
        CancellationToken cancellationToken)
    {
        if (delegatedResponse is null)
        {
            return null;
        }

        var documentContext = requestContext.GetRequiredDocumentContext();

        // For Html we just return the edit as is
        if (projection.LanguageKind == RazorLanguageKind.Html)
        {
            return delegatedResponse;
        }

        // For C# we run the edit through our formatting engine
        var edits = new[] { delegatedResponse.TextEdit };

        var razorFormattingService = requestContext.GetRequiredService<IRazorFormattingService>();
        TextEdit[] mappedEdits;
        if (delegatedResponse.TextEditFormat == InsertTextFormat.Snippet)
        {
            mappedEdits = await razorFormattingService.FormatSnippetAsync(documentContext, projection.LanguageKind, edits, originalRequest.Options, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            mappedEdits = await razorFormattingService.FormatOnTypeAsync(documentContext, projection.LanguageKind, edits, originalRequest.Options, hostDocumentIndex: 0, triggerCharacter: '\0', cancellationToken).ConfigureAwait(false);
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
