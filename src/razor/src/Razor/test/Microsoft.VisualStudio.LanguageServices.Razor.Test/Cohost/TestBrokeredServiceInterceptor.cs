// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Remote.Razor;
using Xunit;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal sealed class TestBrokeredServiceInterceptor : IRazorBrokeredServiceInterceptor
{
    private readonly TestSolutionStore _solutionStore = new();

    public Task<RazorPinnedSolutionInfoWrapper> GetSolutionInfoAsync(Solution solution, CancellationToken cancellationToken)
        => _solutionStore.AddAsync(solution, cancellationToken);

    public ValueTask RunServiceAsync(
        Func<CancellationToken, ValueTask> implementation,
        CancellationToken cancellationToken)
        => implementation(cancellationToken);

    public ValueTask<T> RunServiceAsync<T>(
        RazorPinnedSolutionInfoWrapper solutionInfo,
        Func<Solution, ValueTask<T>> implementation,
        CancellationToken cancellationToken)
    {
        var solution = _solutionStore.Get(solutionInfo);

        Assert.NotNull(solution);

        return implementation(solution);
    }
}
