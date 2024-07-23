// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal partial class DocumentState
{
    internal record struct ImportItem(string? FilePath, VersionStamp Version, IDocumentSnapshot Document)
    {
        public readonly string? FileKind => Document.FileKind;
    }
}
