// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollection;

/// <summary>
/// Payload object that is used to exchange data between datacollector process and runner process.
/// </summary>
[DataContract]
public class BeforeTestRunStartResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BeforeTestRunStartResult"/> class.
    /// </summary>
    /// <param name="environmentVariables">
    /// The environment variables.
    /// </param>
    /// <param name="dataCollectionEventsPort">
    /// The data Collection Events Port.
    /// </param>
    public BeforeTestRunStartResult(IDictionary<string, string?> environmentVariables, int dataCollectionEventsPort)
    {
        EnvironmentVariables = environmentVariables;
        DataCollectionEventsPort = dataCollectionEventsPort;
    }

    /// <summary>
    /// Gets the environment variable dictionary.
    /// </summary>
    [DataMember]
    public IDictionary<string, string?> EnvironmentVariables { get; private set; }

    /// <summary>
    /// Gets the data collection events port.
    /// </summary>
    [DataMember]
    public int DataCollectionEventsPort { get; private set; }
}
