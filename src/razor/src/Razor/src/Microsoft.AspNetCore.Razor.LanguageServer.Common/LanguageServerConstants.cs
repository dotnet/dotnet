// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

public static class LanguageServerConstants
{
    internal const int VSCompletionItemKindOffset = 118115;

    public const string DefaultProjectConfigurationFile = "project.razor.json";

    internal const string RazorSemanticTokensLegendEndpoint = "_vs_/textDocument/semanticTokensLegend";

    internal const string SemanticTokensProviderName = "semanticTokensProvider";

    internal const string RazorLanguageQueryEndpoint = "razor/languageQuery";

    internal const string RazorBreakpointSpanEndpoint = "razor/breakpointSpan";

    internal const string RazorProximityExpressionsEndpoint = "razor/proximityExpressions";

    internal const string RazorMonitorProjectConfigurationFilePathEndpoint = "razor/monitorProjectConfigurationFilePath";

    internal const string RazorMapToDocumentRangesEndpoint = "razor/mapToDocumentRanges";

    internal const string RazorTranslateDiagnosticsEndpoint = "razor/translateDiagnostics";

    internal const string RazorMapToDocumentEditsEndpoint = "razor/mapToDocumentEdits";

    internal const string RazorCodeActionRunnerCommand = "razor/runCodeAction";

    internal const string RazorDocumentFormattingEndpoint = "textDocument/formatting";

    internal const string RazorDocumentOnTypeFormattingEndpoint = "textDocument/onTypeFormatting";

    internal const string RazorCompletionEndpointName = "razor/completion";

    internal const string RazorCompletionResolveEndpointName = "razor/completionItem/resolve";

    internal const string RazorGetFormattingOptionsEndpointName = "razor/formatting/options";

    // This needs to be the same as in Web Tools, that is used by the HTML editor, because
    // we actually respond to the Web Tools "Wrap With Div" command handler, which sends this message
    // to all servers. We then take the message, get the HTML virtual document, and send it
    // straight back to Web Tools for them to do the work.
    internal const string RazorWrapWithTagEndpoint = "textDocument/_vsweb_wrapWithTag";

    internal static class CodeActions
    {
        public const string EditBasedCodeActionCommand = "EditBasedCodeActionCommand";

        public const string ExtractToCodeBehindAction = "ExtractToCodeBehind";

        public const string CreateComponentFromTag = "CreateComponentFromTag";

        public const string AddUsing = "AddUsing";

        public const string CodeActionFromVSCode = "CodeActionFromVSCode";

        /// <summary>
        /// Remaps without formatting the resolved code action edit
        /// </summary>
        public const string UnformattedRemap = "UnformattedRemap";

        /// <summary>
        /// Remaps and formats the resolved code action edit
        /// </summary>
        public const string Default = "Default";

        internal static class Languages
        {
            public const string CSharp = "CSharp";

            public const string Razor = "Razor";

            public const string Html = "Html";
        }
    }
}
