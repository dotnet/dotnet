// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Formatting;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Razor.AutoInsert;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer.AutoInsert;

[RazorLanguageServerEndpoint(VSInternalMethods.OnAutoInsertName)]
internal class OnAutoInsertEndpoint(
    LanguageServerFeatureOptions languageServerFeatureOptions,
    IDocumentMappingService documentMappingService,
    IClientConnection clientConnection,
    IAutoInsertService autoInsertService,
    RazorLSPOptionsMonitor optionsMonitor,
    IRazorFormattingService razorFormattingService,
    ILoggerFactory loggerFactory)
    : AbstractRazorDelegatingEndpoint<VSInternalDocumentOnAutoInsertParams, VSInternalDocumentOnAutoInsertResponseItem?>(languageServerFeatureOptions, documentMappingService, clientConnection, loggerFactory.GetOrCreateLogger<OnAutoInsertEndpoint>()), ICapabilitiesProvider
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions = languageServerFeatureOptions;
    private readonly RazorLSPOptionsMonitor _optionsMonitor = optionsMonitor;
    private readonly IRazorFormattingService _razorFormattingService = razorFormattingService;
    private readonly IAutoInsertService _autoInsertService = autoInsertService;

    protected override string CustomMessageTarget => CustomMessageNames.RazorOnAutoInsertEndpointName;

    /// <summary>
    /// Used to to send request to Html even when it is in a Razor context, for example
    /// for component attributes that are a Razor context, but we want to treat them as Html for auto-inserting quotes
    /// after typing equals for attribute values.
    /// </summary>
    protected override IDocumentPositionInfoStrategy DocumentPositionInfoStrategy => PreferHtmlInAttributeValuesDocumentPositionInfoStrategy.Instance;

    public void ApplyCapabilities(VSInternalServerCapabilities serverCapabilities, VSInternalClientCapabilities clientCapabilities)
    {
        var triggerCharacters = _autoInsertService.TriggerCharacters;

        if (_languageServerFeatureOptions.SingleServerSupport)
        {
            triggerCharacters = [
                .. triggerCharacters,
                .. AutoInsertService.HtmlAllowedAutoInsertTriggerCharacters,
                .. AutoInsertService.CSharpAllowedAutoInsertTriggerCharacters];
        }

        serverCapabilities.EnableOnAutoInsert(triggerCharacters);
    }

    protected override async Task<VSInternalDocumentOnAutoInsertResponseItem?> TryHandleAsync(VSInternalDocumentOnAutoInsertParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        var codeDocument = await documentContext.GetCodeDocumentAsync(cancellationToken).ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        var character = request.Character;

        if (_autoInsertService.TryResolveInsertion(
                codeDocument,
                request.Position,
                character,
                _optionsMonitor.CurrentValue.AutoClosingTags,
                out var insertTextEdit))
        {
            return new VSInternalDocumentOnAutoInsertResponseItem()
            {
                TextEdit = insertTextEdit.TextEdit,
                TextEditFormat = insertTextEdit.TextEditFormat,
            };
        }

        // No provider could handle the text edit.
        return null;
    }

    protected override Task<IDelegatedParams?> CreateDelegatedParamsAsync(VSInternalDocumentOnAutoInsertParams request, RazorRequestContext requestContext, DocumentPositionInfo positionInfo, CancellationToken cancellationToken)
    {
        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return SpecializedTasks.Null<IDelegatedParams>();
        }

        if (positionInfo.LanguageKind == RazorLanguageKind.Html)
        {
            if (!AutoInsertService.HtmlAllowedAutoInsertTriggerCharacters.Contains(request.Character))
            {
                Logger.LogInformation($"Inapplicable HTML trigger char {request.Character}.");
                return SpecializedTasks.Null<IDelegatedParams>();
            }

            if (!_optionsMonitor.CurrentValue.AutoInsertAttributeQuotes && request.Character == "=")
            {
                // Use Razor setting for auto insert attribute quotes. HTML Server doesn't have a way to pass that
                // information along so instead we just don't delegate the request.
                Logger.LogTrace($"Not delegating to HTML completion because AutoInsertAttributeQuotes is disabled");
                return SpecializedTasks.Null<IDelegatedParams>();
            }
        }
        else if (positionInfo.LanguageKind == RazorLanguageKind.CSharp)
        {
            if (!AutoInsertService.CSharpAllowedAutoInsertTriggerCharacters.Contains(request.Character))
            {
                Logger.LogInformation($"Inapplicable C# trigger char {request.Character}.");
                return SpecializedTasks.Null<IDelegatedParams>();
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
            if (!_optionsMonitor.CurrentValue.Formatting.IsOnTypeEnabled())
            {
                Logger.LogInformation($"Formatting on type disabled, so auto insert is a no-op for C#.");
                return SpecializedTasks.Null<IDelegatedParams>();
            }
        }

        return Task.FromResult<IDelegatedParams?>(new DelegatedOnAutoInsertParams(
            documentContext.GetTextDocumentIdentifierAndVersion(),
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

        var documentContext = requestContext.DocumentContext;
        if (documentContext is null)
        {
            return null;
        }

        // For Html we just return the edit as is
        if (positionInfo.LanguageKind == RazorLanguageKind.Html)
        {
            return delegatedResponse;
        }

        // For C# we run the edit through our formatting engine
        Debug.Assert(positionInfo.LanguageKind == RazorLanguageKind.CSharp);

        var options = RazorFormattingOptions.From(originalRequest.Options, _optionsMonitor.CurrentValue.CodeBlockBraceOnNextLine);

        var csharpSourceText = await documentContext.GetCSharpSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var textChange = csharpSourceText.GetTextChange(delegatedResponse.TextEdit);

        var mappedChange = delegatedResponse.TextEditFormat == InsertTextFormat.Snippet
            ? await _razorFormattingService.TryGetCSharpSnippetFormattingEditAsync(documentContext, [textChange], options, cancellationToken).ConfigureAwait(false)
            : await _razorFormattingService.TryGetSingleCSharpEditAsync(documentContext, textChange, options, cancellationToken).ConfigureAwait(false);
        if (mappedChange is not { } change)
        {
            return null;
        }

        var sourceText = await documentContext.GetSourceTextAsync(cancellationToken).ConfigureAwait(false);
        var mappedEdit = sourceText.GetTextEdit(change);

        return new VSInternalDocumentOnAutoInsertResponseItem()
        {
            TextEdit = mappedEdit,
            TextEditFormat = delegatedResponse.TextEditFormat,
        };
    }
}
