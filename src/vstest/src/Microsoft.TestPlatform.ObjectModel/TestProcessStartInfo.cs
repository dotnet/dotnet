// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// The start info of the test runner
/// </summary>
[DataContract]
public class TestProcessStartInfo
{
    /// <summary>
    /// The name of the test runner exe
    /// </summary>
    [DataMember]
    public string? FileName { get; set; }

    /// <summary>
    /// The arguments for the test runner
    /// </summary>
    [DataMember]
    public string? Arguments { get; set; }

    /// <summary>
    /// The working directory for the test runner
    /// </summary>
    [DataMember]
    public string? WorkingDirectory { get; set; }

    /// <summary>
    /// The associated environment variables
    /// </summary>
    [DataMember]
    public IDictionary<string, string?>? EnvironmentVariables { get; set; }

    /// <summary>
    /// Any additional custom properties that might be required for the launch
    /// For example - emulator ID, remote machine details etc.
    /// </summary>
    [DataMember]
    public IDictionary<string, string>? CustomProperties { get; set; }
}
