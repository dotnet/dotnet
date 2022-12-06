// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.LanguageServer.Common;

    public static class LanguageServerConstants
    {
        public const int VSCompletionItemKindOffset = 118115;

        public const string DefaultProjectConfigurationFile = "project.razor.json";

        public const string RazorSemanticTokensLegendEndpoint = "_vs_/textDocument/semanticTokensLegend";

        public const string SemanticTokensProviderName = "semanticTokensProvider";

        public const string RazorLanguageQueryEndpoint = "razor/languageQuery";

        public const string RazorBreakpointSpanEndpoint = "razor/breakpointSpan";

        public const string RazorProximityExpressionsEndpoint = "razor/proximityExpressions";

        public const string RazorMonitorProjectConfigurationFilePathEndpoint = "razor/monitorProjectConfigurationFilePath";

        public const string RazorMapToDocumentRangesEndpoint = "razor/mapToDocumentRanges";

        public const string RazorTranslateDiagnosticsEndpoint = "razor/translateDiagnostics";

        public const string RazorMapToDocumentEditsEndpoint = "razor/mapToDocumentEdits";

        public const string RazorCodeActionRunnerCommand = "razor/runCodeAction";

        public const string RazorDocumentFormattingEndpoint = "textDocument/formatting";

        public const string RazorDocumentOnTypeFormattingEndpoint = "textDocument/onTypeFormatting";

        public const string RazorCompletionEndpointName = "razor/completion";

        public const string RazorCompletionResolveEndpointName = "razor/completionItem/resolve";

        public const string RazorGetFormattingOptionsEndpointName = "razor/formatting/options";

        // This needs to be the same as in Web Tools, that is used by the HTML editor, because
        // we actually respond to the Web Tools "Wrap With Div" command handler, which sends this message
        // to all servers. We then take the message, get the HTML virtual document, and send it
        // straight back to Web Tools for them to do the work.
        public const string RazorWrapWithTagEndpoint = "textDocument/_vsweb_wrapWithTag";

        public static class CodeActions
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

            public static class Languages
            {
                public const string CSharp = "CSharp";

                public const string Razor = "Razor";

                public const string Html = "Html";
            }
        }
    }
