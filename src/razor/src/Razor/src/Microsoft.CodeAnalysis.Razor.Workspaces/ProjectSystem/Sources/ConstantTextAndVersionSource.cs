// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
