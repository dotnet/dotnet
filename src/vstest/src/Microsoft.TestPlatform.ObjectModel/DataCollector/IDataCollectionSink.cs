// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Class used by data collectors to send data to up-stream components
/// (agent, controller, client, etc).
/// </summary>
public interface IDataCollectionSink
{
    /// <summary>
    /// The send data will send the data collection data to upstream applications.
    /// Key will be set as TestProperty in TestResult and value as corresponding value.
    /// </summary>
    /// <param name="dataCollectionContext">
    /// The data collection context.
    /// </param>
    /// <param name="key">
    /// The key should be unique for a data collector.
    /// </param>
    /// <param name="value">
    /// The value should be a string or an object serialized into a JSON string.
    /// </param>
    void SendData(DataCollectionContext dataCollectionContext, string key, string value);

}
