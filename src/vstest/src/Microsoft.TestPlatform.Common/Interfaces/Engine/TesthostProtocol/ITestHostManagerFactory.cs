// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.TesthostProtocol;

/// <summary>
/// The factory that provides discovery and execution managers to the test host.
/// </summary>
public interface ITestHostManagerFactory
{
    /// <summary>
    /// The discovery manager instance for any discovery related operations inside of the test host.
    /// </summary>
    /// <returns>The discovery manager.</returns>
    IDiscoveryManager GetDiscoveryManager();

    /// <summary>
    /// The execution manager instance for any discovery related operations inside of the test host.
    /// </summary>
    /// <returns>The execution manager.</returns>
    IExecutionManager GetExecutionManager();
}
