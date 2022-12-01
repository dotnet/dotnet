// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

[Shared]
[Export(typeof(LSPDocumentFactory))]
internal class DefaultLSPDocumentFactory : LSPDocumentFactory
{
    private readonly FileUriProvider _fileUriProvider;
    private readonly IEnumerable<Lazy<VirtualDocumentFactory, IContentTypeMetadata>> _virtualDocumentFactories;

    [ImportingConstructor]
    public DefaultLSPDocumentFactory(
        FileUriProvider fileUriProvider,
        [ImportMany] IEnumerable<Lazy<VirtualDocumentFactory, IContentTypeMetadata>> virtualBufferFactories)
    {
        if (fileUriProvider is null)
        {
            throw new ArgumentNullException(nameof(fileUriProvider));
        }

        if (virtualBufferFactories is null)
        {
            throw new ArgumentNullException(nameof(virtualBufferFactories));
        }

        _fileUriProvider = fileUriProvider;
        _virtualDocumentFactories = virtualBufferFactories;
    }

    public override LSPDocument Create(ITextBuffer buffer)
    {
        if (buffer is null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        var uri = _fileUriProvider.GetOrCreate(buffer);
        var virtualDocuments = CreateVirtualDocuments(buffer);
        var lspDocument = new DefaultLSPDocument(uri, buffer, virtualDocuments);

        return lspDocument;
    }

    private IReadOnlyList<VirtualDocument> CreateVirtualDocuments(ITextBuffer hostDocumentBuffer)
    {
        var virtualDocuments = new List<VirtualDocument>();
        foreach (var factory in _virtualDocumentFactories)
        {
            if (factory.Metadata.ContentTypes.Any(ct => hostDocumentBuffer.ContentType.IsOfType(ct)))
            {
                if (factory.Value.TryCreateFor(hostDocumentBuffer, out var virtualDocument))
                {
                    virtualDocuments.Add(virtualDocument);
                }
            }
        }

        return virtualDocuments;
    }
}
