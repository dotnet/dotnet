// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

internal static class DiscoveryCriteriaExtensions
{
    public static void UpdateDiscoveryCriteria(this DiscoveryCriteria discoveryCriteria, ITestRuntimeProvider testRuntimeProvider)
    {
        var actualTestSources = testRuntimeProvider.GetTestSources(discoveryCriteria.Sources);

        // If the actual testSources, & input test sources do differ it means that the User(IDE) actually sent a package.
        // We are limiting that only one package can be sent per session, so picking the first entry in sources
        if (discoveryCriteria.Sources.Except(actualTestSources).Any())
        {
            discoveryCriteria.Package = discoveryCriteria.Sources.FirstOrDefault();

            // Allow TestRuntimeProvider to update source map, this is required for remote scenarios.
            // If we run for specific tests, then we expect the test case object to contain correct source path for remote scenario as well
            UpdateTestSources(actualTestSources, discoveryCriteria.AdapterSourceMap);
        }
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
