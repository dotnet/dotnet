// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

[Export(typeof(VirtualDocumentFactory))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
internal class CSharpVirtualDocumentFactory : VirtualDocumentFactoryBase
{
    public static readonly string CSharpClientName = "RazorCSharp";
    private static readonly IReadOnlyDictionary<object, object> s_languageBufferProperties = new Dictionary<object, object>
    {
        { LanguageClientConstants.ClientNamePropertyKey, CSharpClientName }
    };

    private static IContentType? s_csharpContentType;
    private readonly LanguageServerFeatureOptions _languageServerFeatureOptions;

    [ImportingConstructor]
    public CSharpVirtualDocumentFactory(
        IContentTypeRegistryService contentTypeRegistry,
        ITextBufferFactoryService textBufferFactory,
        ITextDocumentFactoryService textDocumentFactory,
        FileUriProvider fileUriProvider,
        LanguageServerFeatureOptions languageServerFeatureOptions)
        : base(contentTypeRegistry, textBufferFactory, textDocumentFactory, fileUriProvider)
    {
        _languageServerFeatureOptions = languageServerFeatureOptions;
    }

    protected override IContentType LanguageContentType
    {
        get
        {
            if (s_csharpContentType is null)
            {
                var contentType = ContentTypeRegistry.GetContentType(RazorLSPConstants.CSharpContentTypeName);
                s_csharpContentType = new RemoteContentDefinitionType(contentType);
            }

            return s_csharpContentType;
        }
    }

    protected override string HostDocumentContentTypeName => RazorConstants.RazorLSPContentTypeName;
    protected override string LanguageFileNameSuffix => _languageServerFeatureOptions.CSharpVirtualDocumentSuffix;
    protected override IReadOnlyDictionary<object, object>? LanguageBufferProperties => s_languageBufferProperties;
    protected override VirtualDocument CreateVirtualDocument(Uri uri, ITextBuffer textBuffer) => new CSharpVirtualDocument(uri, textBuffer);

    private class RemoteContentDefinitionType : IContentType
    {
        private static readonly IReadOnlyList<string> s_extendedBaseContentTypes = new[]
        {
            "code-languageserver-base",
            CodeRemoteContentDefinition.CodeRemoteContentTypeName
        };

        private readonly IContentType _innerContentType;

        internal RemoteContentDefinitionType(IContentType innerContentType)
        {
            if (innerContentType is null)
            {
                throw new ArgumentNullException(nameof(innerContentType));
            }

            _innerContentType = innerContentType;
            TypeName = innerContentType.TypeName;
            DisplayName = innerContentType.DisplayName;
        }

        public string TypeName { get; }

        public string DisplayName { get; }

        public IEnumerable<IContentType> BaseTypes => _innerContentType.BaseTypes;

        public bool IsOfType(string type)
        {
            return s_extendedBaseContentTypes.Contains(type) || _innerContentType.IsOfType(type);
        }
    }
}
