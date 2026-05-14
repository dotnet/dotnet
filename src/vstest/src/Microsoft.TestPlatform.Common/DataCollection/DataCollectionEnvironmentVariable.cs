// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector;

/// <summary>
/// An environment variable requested to be set in the test execution environment by a data collector, including the
/// friendly names of data collectors that requested it.
/// This is needed to find list of environment variables needed for test run after eliminating the duplicate name and keys.
/// For details check DataCollectionPluginManager.AddCollectorEnvironmentVariables() method.
/// </summary>
internal class DataCollectionEnvironmentVariable
{
    /// <summary>
    /// Variable name and requested value
    /// </summary>
    private readonly KeyValuePair<string, string> _variable;

    /// <summary>
    /// Friendly names of data collectors that requested this environment variable
    /// </summary>
    private readonly List<string> _dataCollectorsThatRequested;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataCollectionEnvironmentVariable"/> class.
    /// </summary>
    /// <param name="variable">
    /// Variable name and requested value.
    /// </param>
    /// <param name="requestingDataCollectorFriendlyName">
    /// Friendly name of the data collector requesting it.
    /// </param>
    public DataCollectionEnvironmentVariable(
        KeyValuePair<string, string> variable,
        string requestingDataCollectorFriendlyName)
    {
        ValidateArg.NotNullOrEmpty(variable.Key, nameof(variable.Key));
        ValidateArg.NotNullOrEmpty(requestingDataCollectorFriendlyName, nameof(requestingDataCollectorFriendlyName));

        _variable = variable;
        _dataCollectorsThatRequested = new List<string> { requestingDataCollectorFriendlyName };
    }

    /// <summary>
    /// Gets variable name.
    /// </summary>
    public string Name
    {
        get
        {
            return _variable.Key;
        }
    }

    /// <summary>
    /// Gets requested value
    /// </summary>
    public string Value
    {
        get
        {
            return _variable.Value;
        }
    }

    /// <summary>
    /// Gets friendly name of the first data collector that requested this environment variable
    /// </summary>
    public string FirstDataCollectorThatRequested
    {
        get
        {
            return _dataCollectorsThatRequested[0];
        }
    }

    /// <summary>
    /// Adds the data collector to the list of data collectors that requested this variable.
    /// </summary>
    /// <param name="requestingDataCollectorFriendlyName">Friendly name of requesting data collector.</param>
    public void AddRequestingDataCollector(string requestingDataCollectorFriendlyName)
    {
        ValidateArg.NotNullOrEmpty(requestingDataCollectorFriendlyName, nameof(requestingDataCollectorFriendlyName));
        _dataCollectorsThatRequested.Add(requestingDataCollectorFriendlyName);
    }

}
