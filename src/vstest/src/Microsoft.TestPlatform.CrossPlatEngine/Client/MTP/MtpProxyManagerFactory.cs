// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;

/// <summary>
/// Creates the Microsoft.Testing.Platform (MTP) discovery/execution proxy managers.
/// </summary>
/// <remarks>
/// The concrete proxy managers (<see cref="MtpProxyDiscoveryManager"/> and <see cref="MtpProxyExecutionManager"/>)
/// are internal to this assembly. The MTP runtime provider that drives them lives in a separate assembly
/// (<c>Microsoft.TestPlatform.TestHostRuntimeProvider</c>), so it cannot instantiate them directly; it goes
/// through this small public factory instead. This keeps the MTP proxy implementations internal while giving the
/// out-of-assembly provider a single, intentional public seam to create them.
/// </remarks>
public static class MtpProxyManagerFactory
{
    /// <summary>
    /// Creates the discovery manager that drives discovery for Microsoft.Testing.Platform sources.
    /// </summary>
    public static IProxyDiscoveryManager CreateDiscoveryManager()
        => new MtpProxyDiscoveryManager();

    /// <summary>
    /// Creates the execution manager that drives execution for Microsoft.Testing.Platform sources.
    /// </summary>
    /// <param name="dataCollectionManager">
    /// The data collection manager to wire in when data collectors are enabled, or <see langword="null"/>
    /// when data collection is off.
    /// </param>
    public static IProxyExecutionManager CreateExecutionManager(IProxyDataCollectionManager? dataCollectionManager)
        => dataCollectionManager is null
            ? new MtpProxyExecutionManager()
            : new MtpProxyExecutionManager(dataCollectionManager);
}
