// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;

internal static class DiscoveryCriteriaFactory
{
    public static DiscoveryCriteria Create(
        Dictionary<string, IEnumerable<string>> adapterSourceMap,
        long frequencyOfDiscoveredTestsEvent,
        TimeSpan discoveredTestEventTimeout,
        string? runSettings,
        TestSessionInfo? testSessionInfo)
    {
        var sources = adapterSourceMap.Values.SelectMany(adapterSources => adapterSources).ToArray();
        var criteria = new DiscoveryCriteria(
            sources.Length == 0 ? [string.Empty] : sources,
            frequencyOfDiscoveredTestsEvent,
            discoveredTestEventTimeout,
            runSettings,
            testSessionInfo);

        criteria.AdapterSourceMap.Clear();
        foreach (var adapterSources in adapterSourceMap)
        {
            criteria.AdapterSourceMap.Add(adapterSources.Key, adapterSources.Value);
        }

        return criteria;
    }
}
