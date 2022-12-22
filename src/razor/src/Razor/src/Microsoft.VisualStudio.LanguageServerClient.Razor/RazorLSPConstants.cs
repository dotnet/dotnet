// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal static class RazorLSPConstants
{
    public const string RazorCSharpLanguageServerName = "Razor C# Language Server Client";

    public const string RazorLanguageServerName = "Razor Language Server Client";

    public const string HtmlLanguageServerName = "HtmlDelegationLanguageServerClient";

    public const string CSHTMLFileExtension = ".cshtml";

    public const string RazorFileExtension = ".razor";

    public const string CSharpFileExtension = ".cs";

    public const string CSharpContentTypeName = "CSharp";

    public const string HtmlLSPDelegationContentTypeName = "html-delegation";

    public const string HtmlLSPContentTypeName = "htmlLSPClient";

    public const string CssLSPContentTypeName = "cssLSPClient";

    public const string TypeScriptLSPContentTypeName = "JavaScript";

    public static readonly Guid RazorActiveUIContextGuid = new("3c5ded8f-72c7-4b1f-af2d-099ceeb935b8");
}
