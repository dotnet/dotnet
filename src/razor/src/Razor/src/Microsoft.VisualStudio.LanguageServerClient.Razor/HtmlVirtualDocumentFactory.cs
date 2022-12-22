// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(VirtualDocumentFactory))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
internal class HtmlVirtualDocumentFactory : VirtualDocumentFactoryBase
{
    private static IContentType? s_htmlLSPContentType;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    [ImportingConstructor]
    public HtmlVirtualDocumentFactory(
        IContentTypeRegistryService contentTypeRegistry,
        ITextBufferFactoryService textBufferFactory,
        ITextDocumentFactoryService textDocumentFactory,
        FileUriProvider filePathProvider,
        LanguageServerFeatureOptions languageServerFeatureOptions)
        : base(contentTypeRegistry, textBufferFactory, textDocumentFactory, filePathProvider)
    {
        _languageServerFeatureOptions = languageServerFeatureOptions;
    }

    protected override IContentType LanguageContentType
    {
        get
        {
            s_htmlLSPContentType ??= ContentTypeRegistry.GetContentType(RazorLSPConstants.HtmlLSPDelegationContentTypeName);

            return s_htmlLSPContentType;
        }
    }

    protected override string HostDocumentContentTypeName => RazorConstants.RazorLSPContentTypeName;
    protected override string LanguageFileNameSuffix => _languageServerFeatureOptions.HtmlVirtualDocumentSuffix;
    protected override VirtualDocument CreateVirtualDocument(Uri uri, ITextBuffer textBuffer) => new HtmlVirtualDocument(uri, textBuffer);
}
