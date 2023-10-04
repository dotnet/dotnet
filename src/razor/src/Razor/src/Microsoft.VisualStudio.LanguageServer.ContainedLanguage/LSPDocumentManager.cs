// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

public abstract class LSPDocumentManager
{
    public abstract bool TryGetDocument(Uri uri, [NotNullWhen(returnValue: true)] out LSPDocumentSnapshot? lspDocumentSnapshot);

    /// <summary>
    /// Tells each <see cref="LSPDocument" /> to try and refresh the number of virtual documents it contains
    /// if necessary.
    /// </summary>
    public virtual void RefreshVirtualDocuments()
    {
        // No-op in the default implementation.
    }
}
