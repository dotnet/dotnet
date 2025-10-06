// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem.Sources;

internal interface ITextAndVersionSource
{
    TextLoader? TextLoader { get; }

    bool TryGetValue([NotNullWhen(true)] out TextAndVersion? result);
    ValueTask<TextAndVersion> GetValueAsync(CancellationToken cancellationToken);
}
