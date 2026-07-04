// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;

/// <summary>
/// Implemented by a runtime provider (an <see cref="ObjectModel.Host.ITestRuntimeProvider"/>) that hosts a
/// run over its own protocol instead of the vstest testhost protocol, and therefore supplies its own
/// discovery/execution proxy managers.
/// </summary>
/// <remarks>
/// The standard flow creates a <c>ProxyDiscoveryManager</c>/<c>ProxyExecutionManager</c> that wrap an
/// <see cref="ObjectModel.Host.ITestRuntimeProvider"/> launching a vstest testhost. Some providers — such as
/// the Microsoft.Testing.Platform provider — are fundamentally a different shape: the test application is its
/// own host and speaks its own JSON-RPC protocol, so it needs a different proxy manager entirely. Rather than
/// teaching <see cref="TestEngine"/> about each such protocol with inline branches, the resolved provider that
/// implements this interface is asked to produce its own proxy managers. This keeps protocol-specific wiring
/// in the provider and out of the engine.
/// <para>
/// This interface is public because the runtime providers that implement it live in a separate assembly
/// (<c>Microsoft.TestPlatform.TestHostRuntimeProvider</c>). The concrete proxy managers stay internal to this
/// assembly; providers create them through the public <see cref="MTP.MtpProxyManagerFactory"/> helper.
/// </para>
/// </remarks>
public interface IProxyManagerFactory
{
    /// <summary>
    /// Creates the discovery manager used to drive discovery for this provider's sources.
    /// </summary>
    IProxyDiscoveryManager CreateDiscoveryManager();

    /// <summary>
    /// Creates the execution manager used to drive execution for this provider's sources.
    /// </summary>
    /// <param name="dataCollectionManager">
    /// The data collection manager to wire in when data collectors are enabled, or <see langword="null"/>
    /// when data collection is off.
    /// </param>
    IProxyExecutionManager CreateExecutionManager(IProxyDataCollectionManager? dataCollectionManager);
}
