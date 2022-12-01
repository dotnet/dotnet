// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

internal abstract class LSPDocumentChangeListener
{
    public abstract void Changed(
        LSPDocumentSnapshot? old,
        LSPDocumentSnapshot? @new,
        VirtualDocumentSnapshot? virtualOld,
        VirtualDocumentSnapshot? virtualNew,
        LSPDocumentChangeKind kind);
}
