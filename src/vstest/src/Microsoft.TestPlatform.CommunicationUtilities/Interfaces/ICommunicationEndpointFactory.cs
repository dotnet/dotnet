// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

public interface ICommunicationEndpointFactory
{
    /// <summary>
    /// Create communication endpoint.
    /// </summary>
    /// <param name="role" cref="ConnectionRole">Endpoint role.</param>
    /// <returns cref="ICommunicationEndPoint">Return communication endpoint object.</returns>
    ICommunicationEndPoint Create(ConnectionRole role);
}
