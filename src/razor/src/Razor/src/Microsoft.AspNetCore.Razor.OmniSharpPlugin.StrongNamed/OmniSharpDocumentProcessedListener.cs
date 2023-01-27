// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin.StrongNamed;

internal abstract class OmniSharpDocumentProcessedListener
{
    public abstract void Initialize(OmniSharpProjectSnapshotManager projectManager);

    public abstract void DocumentProcessed(RazorCodeDocument codeDocument, OmniSharpDocumentSnapshot document);
}
