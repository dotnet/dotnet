// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Common.Interfaces;

/// <summary>
/// Metadata that is available from data collectors.
/// </summary>
public interface IDataCollectorCapabilities : ITestExtensionCapabilities
{
    /// <summary>
    /// Gets the friendly name corresponding to the data collector.
    /// </summary>
    string FriendlyName { get; }

    /// <summary>
    /// Check if the data collector has got attachment processor registered.
    /// </summary>
    bool HasAttachmentProcessor { get; }
}
