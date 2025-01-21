// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Threading;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem.Sources;

internal sealed class LoadableTextAndVersionSource(TextLoader textLoader) : ITextAndVersionSource
{
    public TextLoader? TextLoader => textLoader;

    private static readonly LoadTextOptions s_loadTextOptions = new(SourceHashAlgorithm.Sha256);

    private readonly AsyncLazy<TextAndVersion> _lazy = AsyncLazy.Create(ct => textLoader.LoadTextAndVersionAsync(s_loadTextOptions, ct));

    public ValueTask<TextAndVersion> GetValueAsync(CancellationToken cancellationToken)
        => new(_lazy.GetValueAsync(cancellationToken));

    public bool TryGetValue([NotNullWhen(true)] out TextAndVersion? result)
        => _lazy.TryGetValue(out result);
}
