// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.Protocol;

internal static class LanguageServerConstants
{
    public const string RazorLanguageQueryEndpoint = "razor/languageQuery";

    public const string RazorBreakpointSpanEndpoint = "razor/breakpointSpan";

    public const string RazorProximityExpressionsEndpoint = "razor/proximityExpressions";

    public const string RazorLanguageServerName = "Razor Language Server";

    public const string RazorMapToDocumentRangesEndpoint = "razor/mapToDocumentRanges";

    public const string RazorMapToDocumentEditsEndpoint = "razor/mapToDocumentEdits";

    public const string RazorCodeActionRunnerCommand = "razor/runCodeAction";

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
        public const string GenerateEventHandler = "GenerateEventHandler";

        public const string GenerateAsyncEventHandler = "GenerateAsyncEventHandler";

        public const string EditBasedCodeActionCommand = "EditBasedCodeActionCommand";

        public const string ExtractToCodeBehindAction = "ExtractToCodeBehind";

        public const string ExtractToNewComponentAction = "ExtractToNewComponent";

        public const string CreateComponentFromTag = "CreateComponentFromTag";

        public const string AddUsing = "AddUsing";

        public const string FullyQualify = "FullyQualify";

        public const string PromoteUsingDirective = "PromoteUsingDirective";

        public const string CodeActionFromVSCode = "CodeActionFromVSCode";

        public const string WrapAttributes = "WrapAttributes";

        /// <summary>
        /// Remaps without formatting the resolved code action edit
        /// </summary>
        public const string UnformattedRemap = "UnformattedRemap";

        /// <summary>
        /// Remaps and formats the resolved code action edit
        /// </summary>
        public const string Default = "Default";
    }
}
