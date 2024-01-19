// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed partial class DocumentVersionCache
{
    private readonly struct DocumentEntry(IDocumentSnapshot document, int version)
    {
        public WeakReference<IDocumentSnapshot> Document { get; } = new WeakReference<IDocumentSnapshot>(document);

        public int Version { get; } = version;
    }
}
