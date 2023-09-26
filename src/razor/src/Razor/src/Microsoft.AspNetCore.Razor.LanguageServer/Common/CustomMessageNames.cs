// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

/// <summary>
/// This lists all of the LSP methods that we support  that are not part of the LSP spec, or LSP++
/// </summary>
/// <remarks>
/// Handlers for these methods live in either the RazorCustomMessageTarget class in this repo for VS,
/// or in various TypeScript files in https://github.com/dotnet/vscode-csharp for VS Code.
/// </remarks>
internal static class CustomMessageNames
{
    // VS Windows only
    public const string RazorInlineCompletionEndpoint = "razor/inlineCompletion";
    public const string RazorValidateBreakpointRangeName = "razor/validateBreakpointRange";
    public const string RazorOnAutoInsertEndpointName = "razor/onAutoInsert";
    public const string RazorSemanticTokensRefreshEndpoint = "razor/semanticTokensRefresh";
    public const string RazorTextPresentationEndpoint = "razor/textPresentation";
    public const string RazorUriPresentationEndpoint = "razor/uriPresentation";
    public const string RazorSpellCheckEndpoint = "razor/spellCheck";
    public const string RazorProjectContextsEndpoint = "razor/projectContexts";
    public const string RazorPullDiagnosticEndpointName = "razor/pullDiagnostics";
    public const string RazorProvidePreciseRangeSemanticTokensEndpoint = "razor/provideSemanticTokensRanges";

    // VS Windows and VS Code
    public const string RazorUpdateCSharpBufferEndpoint = "razor/updateCSharpBuffer";
    public const string RazorUpdateHtmlBufferEndpoint = "razor/updateHtmlBuffer";
    public const string RazorProvideCodeActionsEndpoint = "razor/provideCodeActions";
    public const string RazorResolveCodeActionsEndpoint = "razor/resolveCodeActions";
    public const string RazorProvideHtmlColorPresentationEndpoint = "razor/provideHtmlColorPresentation";
    public const string RazorProvideHtmlDocumentColorEndpoint = "razor/provideHtmlDocumentColor";
    public const string RazorProvideSemanticTokensRangeEndpoint = "razor/provideSemanticTokensRange";
    public const string RazorFoldingRangeEndpoint = "razor/foldingRange";
    public const string RazorHtmlFormattingEndpoint = "razor/htmlFormatting";
    public const string RazorHtmlOnTypeFormattingEndpoint = "razor/htmlOnTypeFormatting";
    public const string RazorSimplifyMethodEndpointName = "razor/simplifyMethod";
    public const string RazorFormatNewFileEndpointName = "razor/formatNewFile";

    // VS Windows only at the moment, but could/should be migrated
    public const string RazorDocumentSymbolEndpoint = "razor/documentSymbol";

    public const string RazorRenameEndpointName = "razor/rename";

    public const string RazorHoverEndpointName = "razor/hover";

    public const string RazorDefinitionEndpointName = "razor/definition";

    public const string RazorDocumentHighlightEndpointName = "razor/documentHighlight";

    public const string RazorSignatureHelpEndpointName = "razor/signatureHelp";

    public const string RazorImplementationEndpointName = "razor/implementation";

    public const string RazorReferencesEndpointName = "razor/references";

    // Called to get C# diagnostics from Roslyn when publishing diagnostics for VS Code
    public const string RazorCSharpPullDiagnosticsEndpointName = "razor/csharpPullDiagnostics";
}
