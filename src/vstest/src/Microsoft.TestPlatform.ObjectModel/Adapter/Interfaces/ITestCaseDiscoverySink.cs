// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

/// <summary>
/// TestCaseDiscovery sink is used by discovery extensions to communicate test cases as they are being discovered,
/// and various discovery related events.
/// </summary>
public interface ITestCaseDiscoverySink
{
    /// <summary>
    /// Callback used by discovery extensions to send back testcases as they are being discovered.
    /// </summary>
    /// <param name="discoveredTest">New test discovered since last invocation.</param>
    void SendTestCase(TestCase discoveredTest);

}
