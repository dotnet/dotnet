// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

[Export(typeof(FallbackCapabilitiesFilterResolver))]
internal class DefaultFallbackCapabilitiesFilterResolver : FallbackCapabilitiesFilterResolver
{
    public override Func<JToken, bool> Resolve(string lspRequestMethodName)
    {
        if (lspRequestMethodName is null)
        {
            throw new ArgumentNullException(nameof(lspRequestMethodName));
        }

        return lspRequestMethodName switch
        {
            // Standard LSP capabilities
            Methods.TextDocumentImplementationName => CheckImplementationCapabilities,
            Methods.TextDocumentTypeDefinitionName => CheckTypeDefinitionCapabilities,
            Methods.TextDocumentReferencesName => CheckFindAllReferencesCapabilities,
            Methods.TextDocumentRenameName => CheckRenameCapabilities,
            Methods.TextDocumentSignatureHelpName => CheckSignatureHelpCapabilities,
            Methods.TextDocumentWillSaveName => CheckWillSaveCapabilities,
            Methods.TextDocumentWillSaveWaitUntilName => CheckWillSaveWaitUntilCapabilities,
            Methods.TextDocumentRangeFormattingName => CheckRangeFormattingCapabilities,
            Methods.WorkspaceSymbolName => CheckWorkspaceSymbolCapabilities,
            Methods.TextDocumentOnTypeFormattingName => CheckOnTypeFormattingCapabilities,
            Methods.TextDocumentFormattingName => CheckFormattingCapabilities,
            Methods.TextDocumentHoverName => CheckHoverCapabilities,
            Methods.TextDocumentCodeActionName => CheckCodeActionCapabilities,
            Methods.TextDocumentCodeLensName => CheckCodeLensCapabilities,
            Methods.TextDocumentCompletionName => CheckCompletionCapabilities,
            Methods.TextDocumentCompletionResolveName => CheckCompletionResolveCapabilities,
            Methods.TextDocumentDefinitionName => CheckDefinitionCapabilities,
            Methods.TextDocumentDocumentHighlightName => CheckHighlightCapabilities,
            "textDocument/semanticTokens" or Methods.TextDocumentSemanticTokensFullName or Methods.TextDocumentSemanticTokensFullDeltaName or Methods.TextDocumentSemanticTokensRangeName => CheckSemanticTokensCapabilities,
            Methods.TextDocumentLinkedEditingRangeName => CheckLinkedEditingRangeCapabilities,
            Methods.CodeActionResolveName => CheckCodeActionResolveCapabilities,
            Methods.TextDocumentDocumentColorName => CheckDocumentColorCapabilities,
            // VS LSP Expansion capabilities
            VSMethods.GetProjectContextsName => CheckProjectContextsCapabilities,
            VSInternalMethods.DocumentReferencesName => CheckMSReferencesCapabilities,
            VSInternalMethods.OnAutoInsertName => CheckOnAutoInsertCapabilities,
            VSInternalMethods.DocumentPullDiagnosticName or VSInternalMethods.WorkspacePullDiagnosticName => CheckPullDiagnosticCapabilities,
            VSInternalMethods.TextDocumentTextPresentationName => CheckTextPresentationCapabilities,
            VSInternalMethods.TextDocumentUriPresentationName => CheckUriPresentationCapabilities,
            _ => FallbackCheckCapabilties,
        };
    }

    private bool CheckDocumentColorCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.DocumentColorProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckSemanticTokensCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<VSServerCapabilities>();

        return serverCapabilities?.SemanticTokensOptions is not null;
    }

    private static bool CheckImplementationCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.ImplementationProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private bool CheckTypeDefinitionCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.TypeDefinitionProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckFindAllReferencesCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.ReferencesProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckRenameCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.RenameProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckSignatureHelpCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.SignatureHelpProvider is not null;
    }

    private static bool CheckWillSaveCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.TextDocumentSync?.WillSave == true;
    }

    private static bool CheckWillSaveWaitUntilCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.TextDocumentSync?.WillSaveWaitUntil == true;
    }

    private static bool CheckRangeFormattingCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.DocumentRangeFormattingProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckWorkspaceSymbolCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.WorkspaceSymbolProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckOnTypeFormattingCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.DocumentOnTypeFormattingProvider is not null;
    }

    private static bool CheckFormattingCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.DocumentFormattingProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckHoverCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.HoverProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckCodeActionCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.CodeActionProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckCodeLensCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.CodeLensProvider is not null;
    }

    private static bool CheckCompletionCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.CompletionProvider is not null;
    }

    private static bool CheckCompletionResolveCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.CompletionProvider?.ResolveProvider == true;
    }

    private static bool CheckDefinitionCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.DefinitionProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckHighlightCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.DocumentHighlightProvider?.Match(
            boolValue => boolValue,
            options => options is not null) ?? false;
    }

    private static bool CheckMSReferencesCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<VSInternalServerCapabilities>();

        return serverCapabilities?.MSReferencesProvider == true;
    }

    private static bool CheckProjectContextsCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<VSInternalServerCapabilities>();

        return serverCapabilities?.ProjectContextProvider == true;
    }

    private static bool CheckCodeActionResolveCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        var resolvesCodeActions = serverCapabilities?.CodeActionProvider?.Match(
            boolValue => false,
            options => options.ResolveProvider) ?? false;

        return resolvesCodeActions;
    }

    private static bool CheckOnAutoInsertCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<VSInternalServerCapabilities>();

        return serverCapabilities?.OnAutoInsertProvider is not null;
    }

    private static bool CheckTextPresentationCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<VSInternalServerCapabilities>();

        return serverCapabilities?.TextPresentationProvider == true;
    }
    private static bool CheckUriPresentationCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<VSInternalServerCapabilities>();

        return serverCapabilities?.UriPresentationProvider == true;
    }

    private static bool CheckLinkedEditingRangeCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<ServerCapabilities>();

        return serverCapabilities?.LinkedEditingRangeProvider?.Match(
          boolValue => boolValue,
          options => options is not null) ?? false;
    }

    private static bool CheckPullDiagnosticCapabilities(JToken token)
    {
        var serverCapabilities = token.ToObject<VSInternalServerCapabilities>();

        return serverCapabilities?.SupportsDiagnosticRequests == true;
    }

    private bool FallbackCheckCapabilties(JToken token)
    {
        // Fallback is to assume present

        return true;
    }
}
