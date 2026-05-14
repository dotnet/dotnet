// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;

/// <summary>
/// The data collection parameters.
/// </summary>
public class DataCollectionParameters
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionParameters"/> class.
    /// </summary>
    /// <param name="areTestCaseLevelEventsRequired">
    /// The are test case level events required.
    /// </param>
    /// <param name="environmentVariables">
    /// The environment variables.
    /// </param>
    /// <param name="dataCollectionEventsPort">
    /// The data Collection Events Port.
    /// </param>
    public DataCollectionParameters(
        bool areTestCaseLevelEventsRequired,
        IDictionary<string, string?>? environmentVariables,
        int dataCollectionEventsPort)
    {
        AreTestCaseLevelEventsRequired = areTestCaseLevelEventsRequired;
        EnvironmentVariables = environmentVariables;
        DataCollectionEventsPort = dataCollectionEventsPort;
    }

    /// <summary>
    /// Gets a value indicating whether any of the enabled data collectors
    /// registered for test case level events
    /// </summary>
    public bool AreTestCaseLevelEventsRequired { get; private set; }

    /// <summary>
    /// Gets BeforeTestRunStart Call on the DataCollectors can yield/return a set of environment variables
    /// </summary>
    public IDictionary<string, string?>? EnvironmentVariables { get; private set; }

    /// <summary>
    /// Gets the data collection events port.
    /// </summary>
    public int DataCollectionEventsPort { get; private set; }

    /// <summary>
    /// The create default parameter instance.
    /// </summary>
    /// <returns>
    /// The <see cref="DataCollectionParameters"/>.
    /// </returns>
    public static DataCollectionParameters CreateDefaultParameterInstance()
    {
        return new DataCollectionParameters(areTestCaseLevelEventsRequired: false, environmentVariables: null, dataCollectionEventsPort: 0);
    }
}
