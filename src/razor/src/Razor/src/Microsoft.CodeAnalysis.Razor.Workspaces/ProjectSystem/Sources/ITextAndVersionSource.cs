// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
