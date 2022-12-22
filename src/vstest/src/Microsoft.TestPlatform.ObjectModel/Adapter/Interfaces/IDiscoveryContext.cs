// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

/// <summary>
/// Specifies the user specified RunSettings and framework provided context of the discovery.
/// </summary>
public interface IDiscoveryContext
{
    /// <summary>
    /// Runsettings specified for this request.
    /// </summary>
    IRunSettings? RunSettings { get; }
}
