// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Remote.Razor;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

internal sealed class TestServiceBroker : IRazorServiceBroker
{
    private Solution? _solution;

    public void UpdateSolution(Solution solution)
    {
        _solution = solution;
    }

    public ValueTask RunServiceAsync(Func<CancellationToken, ValueTask> implementation, CancellationToken cancellationToken)
    {
        return implementation(cancellationToken);
    }

    public ValueTask<T> RunServiceAsync<T>(RazorPinnedSolutionInfoWrapper solutionInfo, Func<Solution, ValueTask<T>> implementation, CancellationToken cancellationToken)
    {
        return implementation(_solution.AssumeNotNull());
    }

    public void Dispose()
    {
    }
}
