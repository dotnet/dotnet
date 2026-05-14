// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

internal static class TestRunCriteriaExtensions
{
    public static TestRunCriteriaWithSources CreateTestRunCriteriaForSources(this TestRunCriteria testRunCriteria, ITestRuntimeProvider? testRuntimeProvider, string? runSettings, TestExecutionContext executionContext, IEnumerable<string>? inputPackages)
    {
        TPDebug.Assert(testRunCriteria.AdapterSourceMap is not null, "testRunCriteria.AdapterSourceMap is null");
        if (TryCheckTestSourceDifferFromPackage(testRuntimeProvider, inputPackages, out IEnumerable<string>? actualTestSources))
        {
            UpdateTestSources(actualTestSources, testRunCriteria.AdapterSourceMap);
        }
        else
        {
            inputPackages = null;
        }

        return new TestRunCriteriaWithSources(testRunCriteria.AdapterSourceMap, inputPackages?.FirstOrDefault(), runSettings, executionContext);
    }

    public static TestRunCriteriaWithTests CreateTestRunCriteriaForTests(this TestRunCriteria testRunCriteria, ITestRuntimeProvider? testRuntimeProvider,
        string? runSettings, TestExecutionContext executionContext, IEnumerable<string>? inputPackages)
    {
        TPDebug.Assert(testRunCriteria.Tests is not null, "testRunCriteria.Tests is null");
        if (TryCheckTestSourceDifferFromPackage(testRuntimeProvider, inputPackages, out IEnumerable<string>? actualTestSources))
        {
            // In UWP scenario TestCase object contains the package as source, which is not actual test source for adapters,
            // so update test case before sending them.
            // We are limiting that a testhost will always run for a single package, A package can contain multiple sources
            foreach (var tc in testRunCriteria.Tests)
            {
                tc.Source = actualTestSources.FirstOrDefault()!;
            }
        }
        else
        {
            inputPackages = null;
        }

        return new TestRunCriteriaWithTests(testRunCriteria.Tests, inputPackages?.FirstOrDefault(), runSettings, executionContext);
    }

    private static bool TryCheckTestSourceDifferFromPackage(ITestRuntimeProvider? testRuntimeProvider,
        IEnumerable<string>? inputPackages, [NotNullWhen(true)] out IEnumerable<string>? actualTestSources)
    {
        actualTestSources = testRuntimeProvider?.GetTestSources(inputPackages!);

        if (inputPackages is null || actualTestSources is null)
        {
            return false;
        }

        // For netcore/fullclr both packages and sources are same thing,
        // For UWP the actual source(exe) differs from input source(.appxrecipe) which we call package.
        // So in such models we check if they differ, then we pass this info to test host to update TestCase source with package info,
        // since this is needed by IDE's to map a TestCase to project.
        return inputPackages.Except(actualTestSources).Any();
    }

    /// <summary>
    /// Update the AdapterSourceMap
    /// </summary>
    /// <param name="sources">actual test sources</param>
    /// <param name="adapterSourceMap">Adapter Source Map</param>
    private static void UpdateTestSources(IEnumerable<string> sources, Dictionary<string, IEnumerable<string>> adapterSourceMap)
    {
        adapterSourceMap.Clear();
        adapterSourceMap.Add(ObjectModel.Constants.UnspecifiedAdapterPath, sources);
    }
}
