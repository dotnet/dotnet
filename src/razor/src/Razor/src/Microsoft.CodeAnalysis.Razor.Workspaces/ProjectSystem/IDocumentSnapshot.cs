// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
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

    Task<SourceText> GetTextAsync();
    Task<VersionStamp> GetTextVersionAsync();
    Task<RazorCodeDocument> GetGeneratedOutputAsync();

    /// <summary>
    /// Gets the Roslyn syntax tree for the generated C# for this Razor document
    /// </summary>
    /// <remarks>Using this from the LSP server side of things is not ideal. Use sparingly :)</remarks>
    Task<SyntaxTree> GetCSharpSyntaxTreeAsync(CancellationToken cancellationToken);

    bool TryGetText([NotNullWhen(true)] out SourceText? result);
    bool TryGetTextVersion(out VersionStamp result);
    bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result);

    IDocumentSnapshot WithText(SourceText text);
}
