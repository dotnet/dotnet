// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;

/// <summary>
/// Class used to define the stop test session payload sent by the vstest.console translation
/// layers into design mode.
/// </summary>
[DataContract]
public class StopTestSessionPayload
{
    /// <summary>
    /// Gets or sets the test session info.
    /// </summary>
    [DataMember]
    public TestSessionInfo? TestSessionInfo { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if metrics should be collected.
    /// </summary>
    [DataMember]
    public bool CollectMetrics { get; set; }
}
