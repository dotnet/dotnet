// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Class used to define the DiscoveryRequestPayload sent by the Vstest.console translation layers into design mode
/// </summary>
[DataContract]
public class DiscoveryRequestPayload
{
    /// <summary>
    /// Settings used for the discovery request.
    /// </summary>
    [DataMember]
    public IEnumerable<string>? Sources { get; set; }

    /// <summary>
    /// Settings used for the discovery request.
    /// </summary>
    [DataMember]
    public string? RunSettings { get; set; }

    /// <summary>
    /// Gets or sets the test platform options
    /// </summary>
    [DataMember]
    public TestPlatformOptions? TestPlatformOptions
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the test session info.
    /// </summary>
    [DataMember]
    public TestSessionInfo? TestSessionInfo { get; set; }
}
