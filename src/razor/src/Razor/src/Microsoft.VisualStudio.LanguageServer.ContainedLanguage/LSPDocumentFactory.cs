// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

internal abstract class LSPDocumentFactory
{
    public abstract LSPDocument Create(ITextBuffer buffer);

    internal virtual bool TryRefreshVirtualDocuments(LSPDocument document)
    {
        // No-op in the default implementation.
        return false;
    }
}
