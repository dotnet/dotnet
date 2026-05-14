// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollectorUnitTests;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Moq.Protected;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.UnitTests;

[TestClass]
public class DataCollectorInformationTests
{
    private readonly DataCollectorInformation _dataCollectorInfo;

    private readonly List<KeyValuePair<string, string>> _envVarList;

    private readonly Mock<DataCollector2> _mockDataCollector;

    private readonly Mock<ITelemetryReporter> _telemetryReporter;

    public DataCollectorInformationTests()
    {
        _envVarList = new List<KeyValuePair<string, string>>();
        _mockDataCollector = new Mock<DataCollector2>();
        _mockDataCollector.As<ITestExecutionEnvironmentSpecifier>().Setup(x => x.GetTestExecutionEnvironmentVariables()).Returns(_envVarList);
        _mockDataCollector.Protected().Setup("Dispose", false, true);
        var mockMessageSink = new Mock<IMessageSink>();
        _dataCollectorInfo = new DataCollectorInformation(
            _mockDataCollector.Object,
            null,
            new DataCollectorConfig(typeof(CustomDataCollector)),
            null,
            new Mock<IDataCollectionAttachmentManager>().Object,
            new TestPlatformDataCollectionEvents(),
            mockMessageSink.Object,
            string.Empty);
        _telemetryReporter = new Mock<ITelemetryReporter>();
    }

    [TestMethod]
    public void InitializeDataCollectorShouldInitializeDataCollector()
    {
        _envVarList.Add(new KeyValuePair<string, string>("key", "value"));

        _dataCollectorInfo.InitializeDataCollector(_telemetryReporter.Object);
        _dataCollectorInfo.SetTestExecutionEnvironmentVariables();

        CollectionAssert.AreEqual(_envVarList, _dataCollectorInfo.TestExecutionEnvironmentVariables!.ToList());
    }

    [TestMethod]
    public void DisposeShouldInvokeDisposeOfDatacollector()
    {
        _dataCollectorInfo.InitializeDataCollector(_telemetryReporter.Object);
        _dataCollectorInfo.DisposeDataCollector();

        _mockDataCollector.Protected().Verify("Dispose", Times.Once(), false, true);
    }
}
