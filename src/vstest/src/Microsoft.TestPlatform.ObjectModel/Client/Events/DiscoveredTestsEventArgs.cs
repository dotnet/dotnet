// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Event arguments used to notify the availability of new tests
/// </summary>
public partial class DiscoveredTestsEventArgs : EventArgs
{
    public DiscoveredTestsEventArgs(IEnumerable<TestCase>? discoveredTestCases)
    {
        DiscoveredTestCases = discoveredTestCases;
    }
    /// <summary>
    /// Tests discovered in this discovery request
    /// </summary>
    public IEnumerable<TestCase>? DiscoveredTestCases { get; private set; }
}
