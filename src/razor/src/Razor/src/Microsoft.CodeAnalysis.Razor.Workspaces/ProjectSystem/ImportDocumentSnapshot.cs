// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal sealed class ImportDocumentSnapshot(IProjectSnapshot project, RazorProjectItem item) : IDocumentSnapshot
{
    public IProjectSnapshot Project { get; } = project;

    private readonly RazorProjectItem _importItem = item;
    private SourceText? _text;

    // The default import file does not have a kind or paths.
    public string? FileKind => null;
    public string? FilePath => null;
    public string? TargetPath => null;

    public int Version => 1;

    public ValueTask<SourceText> GetTextAsync(CancellationToken cancellationToken)
    {
        return TryGetText(out var text)
            ? new(text)
            : ReadTextAsync();

        ValueTask<SourceText> ReadTextAsync()
        {
            using var stream = _importItem.Read();
            var sourceText = SourceText.From(stream);

            var result = _text ??= InterlockedOperations.Initialize(ref _text, sourceText);
            return new(result);
        }
    }

    public ValueTask<RazorCodeDocument> GetGeneratedOutputAsync(
        bool forceDesignTimeGeneratedOutput,
        CancellationToken cancellationToken)
        => throw new NotSupportedException();

    public ValueTask<VersionStamp> GetTextVersionAsync(CancellationToken cancellationToken)
        => new(VersionStamp.Default);

    public bool TryGetText([NotNullWhen(true)] out SourceText? result)
    {
        result = _text;
        return result is not null;
    }

    public bool TryGetTextVersion(out VersionStamp result)
    {
        result = VersionStamp.Default;
        return true;
    }

    public bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result)
        => throw new NotSupportedException();

    public IDocumentSnapshot WithText(SourceText text)
        => throw new NotSupportedException();

    public ValueTask<SyntaxTree> GetCSharpSyntaxTreeAsync(CancellationToken cancellationToken)
        => throw new NotSupportedException();
}
