// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;

/// <summary>
/// Class used to define the start test session payload sent by the vstest.console translation
/// layers into design mode.
/// </summary>
[DataContract]
public class StartTestSessionPayload
{
    /// <summary>
    /// Gets or sets the sources used for starting the test session.
    /// </summary>
    [DataMember]
    public IList<string>? Sources { get; set; }

    /// <summary>
    /// Gets or sets the run settings used for starting the test session.
    /// </summary>
    [DataMember]
    public string? RunSettings { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if debugging is enabled.
    /// </summary>
    [DataMember]
    public bool IsDebuggingEnabled { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if a custom host launcher should be used.
    /// </summary>
    [DataMember]
    public bool HasCustomHostLauncher { get; set; }

    /// <summary>
    /// Gets or sets the test platform options.
    /// </summary>
    [DataMember]
    public TestPlatformOptions? TestPlatformOptions { get; set; }
}
