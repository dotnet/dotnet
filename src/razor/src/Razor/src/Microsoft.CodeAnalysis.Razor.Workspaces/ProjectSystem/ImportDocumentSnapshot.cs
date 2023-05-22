// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class ImportDocumentSnapshot : IDocumentSnapshot
{
    // The default import file does not have a kind or paths.
    public string? FileKind => null;
    public string? FilePath => null;
    public string? TargetPath => null;

    public bool SupportsOutput => false;
    public IProjectSnapshot Project => _project;

    private readonly IProjectSnapshot _project;
    private readonly RazorProjectItem _importItem;
    private SourceText? _sourceText;
    private readonly VersionStamp _version;

    public ImportDocumentSnapshot(IProjectSnapshot project, RazorProjectItem item)
    {
        _project = project;
        _importItem = item;
        _version = VersionStamp.Default;
    }

    public Task<RazorCodeDocument> GetGeneratedOutputAsync()
        => throw new NotSupportedException();

    public ImmutableArray<IDocumentSnapshot> GetImports()
        => ImmutableArray<IDocumentSnapshot>.Empty;

    public async Task<SourceText> GetTextAsync()
    {
        using (var stream = _importItem.Read())
        using (var reader = new StreamReader(stream))
        {
            var content = await reader.ReadToEndAsync().ConfigureAwait(false);
            _sourceText = SourceText.From(content);
        }

        return _sourceText;
    }

    public Task<VersionStamp> GetTextVersionAsync()
    {
        return Task.FromResult(_version);
    }

    public bool TryGetText([NotNullWhen(true)] out SourceText? result)
    {
        if (_sourceText is { } sourceText)
        {
            result = sourceText;
            return true;
        }

        result = null;
        return false;
    }

    public bool TryGetTextVersion(out VersionStamp result)
    {
        result = _version;
        return true;
    }

    public bool TryGetGeneratedOutput([NotNullWhen(true)] out RazorCodeDocument? result)
        => throw new NotSupportedException();
}
