// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal partial class DocumentState
{
    private readonly record struct ImportItem(
        string? FilePath,
        string? FileKind,
        SourceText Text,
        VersionStamp Version)
    {
        // Note: The default import does not have file path, file kind, or version stamp.
        public static ImportItem CreateDefault(SourceText text)
            => new(FilePath: null, FileKind: null, text, Version: default);
    }
}
