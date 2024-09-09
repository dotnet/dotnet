// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Razor.Formatting;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal static class LspInitializationHelpers
{
    public static void EnableInlayHints(this VSInternalServerCapabilities serverCapabilities)
    {
        serverCapabilities.InlayHintOptions = new InlayHintOptions
        {
            ResolveProvider = true,
            WorkDoneProgress = false
        };
    }

    public static void EnableSemanticTokens(this VSInternalServerCapabilities serverCapabilities, ISemanticTokensLegendService legend)
    {
        serverCapabilities.SemanticTokensOptions = new SemanticTokensOptions().EnableSemanticTokens(legend);
    }

    public static SemanticTokensOptions EnableSemanticTokens(this SemanticTokensOptions options, ISemanticTokensLegendService legend)
    {
        options.Full = false;
        options.Legend = new SemanticTokensLegend
        {
            TokenModifiers = legend.TokenModifiers.All,
            TokenTypes = legend.TokenTypes.All
        };
        options.Range = true;

        return options;
    }

    public static void EnableSignatureHelp(this VSInternalServerCapabilities serverCapabilities)
    {
        serverCapabilities.SignatureHelpProvider = new SignatureHelpOptions().EnableSignatureHelp();
    }

    public static SignatureHelpOptions EnableSignatureHelp(this SignatureHelpOptions options)
    {
        options.TriggerCharacters = ["(", ",", "<"];
        options.RetriggerCharacters = [">", ")"];

        return options;
    }

    public static void EnableHoverProvider(this VSInternalServerCapabilities serverCapabilities)
    {
        serverCapabilities.HoverProvider = new HoverOptions()
        {
            WorkDoneProgress = false,
        };
    }

    public static void EnableValidateBreakpointRange(this VSInternalServerCapabilities serverCapabilities)
    {
        serverCapabilities.BreakableRangeProvider = true;
    }

    public static void EnableMapCodeProvider(this VSInternalServerCapabilities serverCapabilities)
    {
        serverCapabilities.MapCodeProvider = true;
    }

    public static void EnableOnAutoInsert(
        this VSInternalServerCapabilities serverCapabilities,
        IEnumerable<string> triggerCharacters)
    {
        serverCapabilities.OnAutoInsertProvider = new VSInternalDocumentOnAutoInsertOptions()
            .EnableOnAutoInsert(triggerCharacters);
    }

    public static VSInternalDocumentOnAutoInsertOptions EnableOnAutoInsert(
        this VSInternalDocumentOnAutoInsertOptions options,
        IEnumerable<string> triggerCharacters)
    {
        options.TriggerCharacters = triggerCharacters.Distinct().ToArray();

        return options;
    }

    public static DocumentOnTypeFormattingOptions EnableOnTypeFormattingTriggerCharacters(this DocumentOnTypeFormattingOptions options)
    {
        options.FirstTriggerCharacter = RazorFormattingService.FirstTriggerCharacter;
        options.MoreTriggerCharacter = RazorFormattingService.MoreTriggerCharacters.ToArray();

        return options;
    }
}
