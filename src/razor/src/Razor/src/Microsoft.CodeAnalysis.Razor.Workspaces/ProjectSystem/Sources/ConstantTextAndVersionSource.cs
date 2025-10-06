// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem.Sources;

internal sealed class ConstantTextAndVersionSource(SourceText text, VersionStamp version) : ITextAndVersionSource
{
    private readonly TextAndVersion _textAndVersion = TextAndVersion.Create(text, version);

    public TextLoader? TextLoader => null;

    public ValueTask<TextAndVersion> GetValueAsync(CancellationToken cancellationToken)
        => new(_textAndVersion);

    public bool TryGetValue([NotNullWhen(true)] out TextAndVersion? result)
    {
        result = _textAndVersion;
        return true;
    }
}
