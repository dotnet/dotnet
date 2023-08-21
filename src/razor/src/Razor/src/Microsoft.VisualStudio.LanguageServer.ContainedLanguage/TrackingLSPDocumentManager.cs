// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

internal abstract class TrackingLSPDocumentManager : LSPDocumentManager
{
    public abstract void TrackDocument(ITextBuffer buffer);

    public abstract void UntrackDocument(ITextBuffer buffer);

    public abstract void UpdateVirtualDocument<TVirtualDocument>(
        Uri hostDocumentUri,
        IReadOnlyList<ITextChange> changes,
        int hostDocumentVersion,
        object? state) where TVirtualDocument : VirtualDocument;

    public virtual void UpdateVirtualDocument<TVirtualDocument>(
        Uri hostDocumentUri,
        Uri virtualDocumentUri,
        IReadOnlyList<ITextChange> changes,
        int hostDocumentVersion,
        object? state) where TVirtualDocument : VirtualDocument
    {
        // This is only virtual to prevent a binary breaking change. We don't expect anyone to call this method, without also implementing it
        throw new NotImplementedException();
    }
}
