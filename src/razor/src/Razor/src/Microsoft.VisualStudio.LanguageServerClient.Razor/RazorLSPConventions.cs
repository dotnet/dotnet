// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Shared]
[Export(typeof(RazorLSPConventions))]
internal class RazorLSPConventions
{
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    [ImportingConstructor]
    public RazorLSPConventions(LanguageServerFeatureOptions languageServerFeatureOptions)
    {
        _languageServerFeatureOptions = languageServerFeatureOptions;
    }

    public bool IsVirtualCSharpFile(Uri uri) => CheckIfFileUriAndExtensionMatch(uri, _languageServerFeatureOptions.CSharpVirtualDocumentSuffix);

    public bool IsVirtualHtmlFile(Uri uri) => CheckIfFileUriAndExtensionMatch(uri, _languageServerFeatureOptions.HtmlVirtualDocumentSuffix);

    public static bool IsRazorFile(Uri uri) => CheckIfFileUriAndExtensionMatch(uri, RazorLSPConstants.RazorFileExtension);

    public static bool IsCSHTMLFile(Uri uri) => CheckIfFileUriAndExtensionMatch(uri, RazorLSPConstants.CSHTMLFileExtension);

    public Uri GetRazorDocumentUri(Uri virtualDocumentUri)
    {
        return _languageServerFeatureOptions.GetRazorDocumentUri(virtualDocumentUri);
    }

    private static bool CheckIfFileUriAndExtensionMatch(Uri uri, string extension)
    {
        if (uri is null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        if (string.IsNullOrEmpty(extension))
        {
            throw new ArgumentNullException(nameof(extension));
        }

        return uri.GetAbsoluteOrUNCPath()?.EndsWith(extension, StringComparison.Ordinal) ?? false;
    }
}
