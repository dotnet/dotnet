// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal interface IDocumentSnapshot
{
    string? FileKind { get; }
    string? FilePath { get; }
    string? TargetPath { get; }
    IProjectSnapshot Project { get; }
    bool SupportsOutput { get; }

    ImmutableArray<IDocumentSnapshot> GetImports();

    Task<SourceText> GetTextAsync();
    Task<VersionStamp> GetTextVersionAsync();
    Task<RazorCodeDocument> GetGeneratedOutputAsync();

    bool TryGetText([NotNullWhen(true)] out SourceText? result);
    bool TryGetTextVersion(out VersionStamp result);
    bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result);
}
