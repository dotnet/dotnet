// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Acceptance.IntegrationTests;

[TestClass]
public static class Build
{
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext)
    {
        // Increase the ThreadPool minimum threads to avoid starvation during parallel test
        // execution. Each test class spawns vstest.console and testhost processes — the async
        // process management and I/O redirection callbacks need ThreadPool threads to complete
        // promptly.
        var additionalThreadsCount = System.Environment.ProcessorCount * 4;
        System.Threading.ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
        System.Threading.ThreadPool.SetMinThreads(workerThreads + additionalThreadsCount, completionPortThreads + additionalThreadsCount);

        IntegrationTestBuild.BuildTestAssetsForIntegrationTests(testContext);
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        IntegrationTestBuild.CleanupTestAssets();
    }
}
