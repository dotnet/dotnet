// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.Common.Hosting;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;

namespace vstest.ProgrammerTests.Fakes;

internal class FakeTestRuntimeProviderManager : ITestRuntimeProviderManager
{
    public FakeTestRuntimeProviderManager(FakeErrorAggregator fakeErrorAggregator)
    {
        FakeErrorAggregator = fakeErrorAggregator;
    }

    public List<FakeTestRuntimeProvider> TestRuntimeProviders { get; } = new();
    public Queue<FakeTestRuntimeProvider> TestRuntimeProvidersByOrder { get; } = new();
    public List<ActionRecord<FakeTestRuntimeProvider>> UsedTestRuntimeProviders { get; } = new();

    public FakeErrorAggregator FakeErrorAggregator { get; }

    public void AddTestRuntimeProviders(params FakeTestRuntimeProvider[] runtimeProviders)
    {
        TestRuntimeProviders.AddRange(runtimeProviders);

        // In cases where we don't have multi tfm run, we will be asked for
        // a provider with multiple sources. In that case we don't know exactly which one to provide
        // so we need to go by order. We also do this resolve twice for each source in parallel run
        // because we first need to know if the provider is shared. So we add to the queue twice.
        // This is brittle, but there is no way around this :(
        foreach (var provider in runtimeProviders)
        {
            TestRuntimeProvidersByOrder.Enqueue(provider);
            TestRuntimeProvidersByOrder.Enqueue(provider);
        }
    }

    public ITestRuntimeProvider? GetTestHostManagerByRunConfiguration(string? _, List<string>? sources)
    {
        var allMatchingProviders = TestRuntimeProviders
            .Where(r => r.TestDlls.Select(dll => dll.Path)
            .Any(path => sources?.Contains(path) == true))
            .ToList();

        if (allMatchingProviders.Count == 0)
        {
            throw new InvalidOperationException($"There are no FakeTestRuntimeProviders associated with any of the incoming sources, make sure your testhost fixture has at least one dll: {sources?.JoinByComma()}");
        }

        if (allMatchingProviders.Count > 1)
        {
            // This is a single tfm run, or multiple dlls in the run have the same tfm. We need to provide
            // providers by order.
            if (!TestRuntimeProvidersByOrder.TryDequeue(out var provider))
            {
                throw new InvalidOperationException("There are no more FakeTestRuntimeProviders to be provided.");
            }

            UsedTestRuntimeProviders.Add(new ActionRecord<FakeTestRuntimeProvider>(provider));
            return provider;
        }

        var single = allMatchingProviders.Single();
        UsedTestRuntimeProviders.Add(new ActionRecord<FakeTestRuntimeProvider>(single));
        return single;
    }

    public ITestRuntimeProvider GetTestHostManagerByUri(string hostUri)
    {
        throw new NotImplementedException();
    }
}
