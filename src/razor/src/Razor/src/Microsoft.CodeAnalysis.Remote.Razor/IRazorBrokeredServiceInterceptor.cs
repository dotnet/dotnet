// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Api;

namespace Microsoft.CodeAnalysis.Remote.Razor;

/// <summary>
/// An abstraction to avoid calling the static <see cref="RazorBrokeredServiceImplementation"/> helper defined in Roslyn.
/// </summary>
internal interface IRazorBrokeredServiceInterceptor
{
    ValueTask RunServiceAsync(
        Func<CancellationToken, ValueTask> implementation,
        CancellationToken cancellationToken);

    ValueTask<T> RunServiceAsync<T>(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        Func<Solution, ValueTask<T>> implementation,
        CancellationToken cancellationToken);
}
