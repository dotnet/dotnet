// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

public interface ITestDiscoveryEventsRegistrar : IBaseTestEventsRegistrar
{
    /// <summary>
    /// Registers to receive discovery events from discovery request.
    /// These events will then be broadcast to any registered loggers.
    /// </summary>
    /// <param name="discoveryRequest">The discovery request to register for events on.</param>
    void RegisterDiscoveryEvents(IDiscoveryRequest discoveryRequest);

    /// <summary>
    /// Unregister the events from the discovery request.
    /// </summary>
    /// <param name="discoveryRequest">The discovery request from which events should be unregistered.</param>
    void UnregisterDiscoveryEvents(IDiscoveryRequest discoveryRequest);
}
