// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection;
// <inheritdoc />
internal class InProcDataCollectionSink : IDataCollectionSink
{
    private readonly IDictionary<Guid, TestCaseDataCollectionData> _testCaseDataCollectionDataMap;

    /// <summary>
    /// In process data collection sink
    /// </summary>
    public InProcDataCollectionSink()
    {
        _testCaseDataCollectionDataMap = new Dictionary<Guid, TestCaseDataCollectionData>();
    }

    // <inheritdoc />
    public void SendData(DataCollectionContext dataCollectionContext, string key, string value)
    {
        ValidateArg.NotNullOrEmpty(key, nameof(key));
        ValidateArg.NotNullOrEmpty(value, nameof(value));
        ValidateArg.NotNullOrEmpty(dataCollectionContext?.TestCase?.Id.ToString(), "dataCollectionContext.TestCase.Id");

        var testCaseId = dataCollectionContext!.TestCase!.Id;
        AddKeyValuePairToDictionary(testCaseId, key, value);
    }

    /// <summary>
    /// Gets the data collection data stored in the in process data collection sink
    /// </summary>
    /// <param name="testCaseId">valid test case id</param>
    /// <returns>test data collection dictionary </returns>
    public IDictionary<string, string> GetDataCollectionDataSetForTestCase(Guid testCaseId)
    {
        if (!_testCaseDataCollectionDataMap.TryGetValue(testCaseId, out TestCaseDataCollectionData? testCaseDataCollection))
        {
            EqtTrace.Warning("No DataCollection Data set for the test case {0}", testCaseId);
            return new Dictionary<string, string>();
        }
        else
        {
            _testCaseDataCollectionDataMap.Remove(testCaseId);
            return testCaseDataCollection.CollectionData;
        }
    }

    private void AddKeyValuePairToDictionary(Guid testCaseId, string key, string value)
    {
        if (!_testCaseDataCollectionDataMap.ContainsKey(testCaseId))
        {
            var testCaseCollectionData = new TestCaseDataCollectionData();
            testCaseCollectionData.AddOrUpdateData(key, value);
            _testCaseDataCollectionDataMap[testCaseId] = testCaseCollectionData;
        }
        else
        {
            _testCaseDataCollectionDataMap[testCaseId].AddOrUpdateData(key, value);
        }
    }

    private class TestCaseDataCollectionData
    {
        public TestCaseDataCollectionData()
        {
            CollectionData = new Dictionary<string, string>();
        }

        internal IDictionary<string, string> CollectionData { get; private set; }

        internal void AddOrUpdateData(string key, string value)
        {
            if (!CollectionData.ContainsKey(key))
            {
                CollectionData[key] = value;
            }
            else
            {
                EqtTrace.Warning("The data for in-proc data collector with key {0} has already been set. Will be reset with new value", key);
                CollectionData[key] = value;
            }
        }
    }
}
