// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.UnitTests;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollectorUnitTests;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.DataCollector.UnitTests;

[TestClass]
public class DataCollectionTelemetryManagerTests
{
    private readonly Mock<IRequestData> _mockRequestData;
    private readonly Mock<IMetricsCollection> _mockMetricsCollection;
    private readonly DataCollectionTelemetryManager _telemetryManager;
    private readonly DataCollectorInformation _dataCollectorInformation;

    public DataCollectionTelemetryManagerTests()
    {
        _mockRequestData = new Mock<IRequestData>();
        _mockMetricsCollection = new Mock<IMetricsCollection>();
        _mockRequestData.Setup(m => m.MetricsCollection).Returns(_mockMetricsCollection.Object);

        var dataCollectorMock = new Mock<CodeCoverageDataCollector>();
        var evnVariablesMock = dataCollectorMock.As<ITestExecutionEnvironmentSpecifier>();
        evnVariablesMock.Setup(a => a.GetTestExecutionEnvironmentVariables()).Returns(new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("MicrosoftInstrumentationEngine_ConfigPath32_VanguardInstrumentationProfiler", "path1"),
            new KeyValuePair<string, string>("MicrosoftInstrumentationEngine_ConfigPath64_VanguardInstrumentationProfiler", "path2")
        });

        _dataCollectorInformation = new DataCollectorInformation(
            dataCollectorMock.Object,
            null,
            new DataCollectorConfig(typeof(CustomDataCollector)),
            null,
            new Mock<IDataCollectionAttachmentManager>().Object,
            new TestPlatformDataCollectionEvents(),
            new Mock<IMessageSink>().Object,
            string.Empty);

        _telemetryManager = new DataCollectionTelemetryManager(_mockRequestData.Object);
    }

    [TestMethod]
    public void RecordEnvironmentVariableAddition_ShouldDoNothing_IfNotProfilerVariable()
    {
        // act
        _telemetryManager.RecordEnvironmentVariableAddition(_dataCollectorInformation, "key", "value");

        // assert
        _mockMetricsCollection.Verify(c => c.Add(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
    }

    [TestMethod]
    public void RecordEnvironmentVariableConflict_ShouldDoNothing_IfNotProfilerVariable_ValuesSame()
    {
        // act
        _telemetryManager.RecordEnvironmentVariableConflict(_dataCollectorInformation, "key", "value", "value");

        // assert
        _mockMetricsCollection.Verify(c => c.Add(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
    }

    [TestMethod]
    public void RecordEnvironmentVariableConflict_ShouldDoNothing_IfNotProfilerVariable_ValuesDifferent()
    {
        // act
        _telemetryManager.RecordEnvironmentVariableConflict(_dataCollectorInformation, "key", "value", "othervalue");

        // assert
        _mockMetricsCollection.Verify(c => c.Add(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
    }

    [TestMethod]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-C11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{324F817A-7420-4E6D-B3C1-143FBED6D855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{9317ae81-bcd8-47b7-aaa1-a28062e41c71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71}", "aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-c11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{9317AE81-bcd8-47b7-AAA1-A28062E41C71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("AAAAAAAAAAAAA", "00000000-0000-0000-0000-000000000000")]
    public void RecordEnvironmentVariableAddition_ShouldCollectTelemetry_IfCorProfilerVariable(string profilerGuid, string profilerName)
    {
        // act
        _telemetryManager.RecordEnvironmentVariableAddition(_dataCollectorInformation, "COR_PROFILER", profilerGuid);

        // assert
        _mockMetricsCollection.Verify(c => c.Add($"VS.TestPlatform.DataCollector.CorProfiler.{_dataCollectorInformation.DataCollectorConfig.TypeUri}", profilerName), Times.Once);
    }

    [TestMethod]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-C11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{324F817A-7420-4E6D-B3C1-143FBED6D855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{9317ae81-bcd8-47b7-aaa1-a28062e41c71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71}", "aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-c11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{9317AE81-bcd8-47b7-AAA1-A28062E41C71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("AAAAAAAAAAAAA", "00000000-0000-0000-0000-000000000000")]
    public void RecordEnvironmentVariableAddition_ShouldCollectTelemetry_IfCoreClrProfilerVariable(string profilerGuid, string profilerName)
    {
        // act
        _telemetryManager.RecordEnvironmentVariableAddition(_dataCollectorInformation, "CORECLR_PROFILER", profilerGuid);

        // assert
        _mockMetricsCollection.Verify(c => c.Add($"VS.TestPlatform.DataCollector.CoreClrProfiler.{_dataCollectorInformation.DataCollectorConfig.TypeUri}", profilerName), Times.Once);
    }

    [TestMethod]
    [DataRow("{0f8fad5b-d9cb-469f-a165-70867728950e}", "{E5F256DC-7959-4DD6-8E4F-C11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{0f8fad5b-d9cb-469f-a165-70867728950e}", "{324F817A-7420-4E6D-B3C1-143FBED6D855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{0f8fad5b-d9cb-469f-a165-70867728950e}", "{9317ae81-bcd8-47b7-aaa1-a28062e41c71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{0f8fad5b-d9cb-469f-a165-70867728950e}", "{aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71}", "aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "{E5F256DC-7959-4DD6-8E4F-c11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{0f8fad5b-d9cb-469f-a165-70867728950e}", "{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "{9317AE81-bcd8-47b7-AAA1-A28062E41C71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "AAAAAAAAAAAAA", "00000000-0000-0000-0000-000000000000")]
    public void RecordEnvironmentVariableConflict_ShouldCollectOverwrittenTelemetry_IfCorProfilerVariable(string existingProfilerGuid, string profilerGuid, string expectedOverwrittenProfiler)
    {
        // act
        _telemetryManager.RecordEnvironmentVariableConflict(_dataCollectorInformation, "COR_PROFILER", profilerGuid, existingProfilerGuid);

        // assert
        _mockMetricsCollection.Verify(c => c.Add($"VS.TestPlatform.DataCollector.CorProfiler.{_dataCollectorInformation.DataCollectorConfig.TypeUri}", $"{Guid.Parse(existingProfilerGuid)}(overwritten:{expectedOverwrittenProfiler})"), Times.Once);
    }

    [TestMethod]
    [DataRow("{0f8fad5b-d9cb-469f-a165-70867728950e}", "{E5F256DC-7959-4DD6-8E4F-C11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{0f8fad5b-d9cb-469f-a165-70867728950e}", "{324F817A-7420-4E6D-B3C1-143FBED6D855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{0f8fad5b-d9cb-469f-a165-70867728950e}", "{9317ae81-bcd8-47b7-aaa1-a28062e41c71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{0f8fad5b-d9cb-469f-a165-70867728950e}", "{aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71}", "aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "{E5F256DC-7959-4DD6-8E4F-c11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{0f8fad5b-d9cb-469f-a165-70867728950e}", "{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "{9317AE81-bcd8-47b7-AAA1-A28062E41C71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "AAAAAAAAAAAAA", "00000000-0000-0000-0000-000000000000")]
    public void RecordEnvironmentVariableConflict_ShouldCollectOverwrittenTelemetry_IfCoreClrProfilerVariable(string existingProfilerGuid, string profilerGuid, string expectedOverwrittenProfiler)
    {
        // act
        _telemetryManager.RecordEnvironmentVariableConflict(_dataCollectorInformation, "CORECLR_PROFILER", profilerGuid, existingProfilerGuid);

        // assert
        _mockMetricsCollection.Verify(c => c.Add($"VS.TestPlatform.DataCollector.CoreClrProfiler.{_dataCollectorInformation.DataCollectorConfig.TypeUri}", $"{Guid.Parse(existingProfilerGuid)}(overwritten:{expectedOverwrittenProfiler})"), Times.Once);
    }

    [TestMethod]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-C11150AB28E0}")]
    [DataRow("{324F817A-7420-4E6D-B3C1-143FBED6D855}")]
    [DataRow("{9317ae81-bcd8-47b7-aaa1-a28062e41c71}")]
    [DataRow("{aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71}")]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-c11150AB28E0}")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}")]
    [DataRow("{9317AE81-bcd8-47b7-AAA1-A28062E41C71}")]
    [DataRow("AAAAAAAAAAAAA")]
    public void RecordEnvironmentVariableConflict_ShouldCollectClrIeTelemetry_IfCorProfilerVariableAndCollectorSpecifiesClrIeProfile(string profilerGuid)
    {
        // arrange
        _dataCollectorInformation.SetTestExecutionEnvironmentVariables();

        // act
        _telemetryManager.RecordEnvironmentVariableConflict(_dataCollectorInformation, "COR_PROFILER", profilerGuid, "{324F817A-7420-4E6D-B3C1-143FBED6D855}");

        // assert
        _mockMetricsCollection.Verify(c => c.Add($"VS.TestPlatform.DataCollector.CorProfiler.{_dataCollectorInformation.DataCollectorConfig.TypeUri}", "324f817a-7420-4e6d-b3c1-143fbed6d855"), Times.Once);
    }

    [TestMethod]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-C11150AB28E0}")]
    [DataRow("{324F817A-7420-4E6D-B3C1-143FBED6D855}")]
    [DataRow("{9317ae81-bcd8-47b7-aaa1-a28062e41c71}")]
    [DataRow("{aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71}")]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-c11150AB28E0}")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}")]
    [DataRow("{9317AE81-bcd8-47b7-AAA1-A28062E41C71}")]
    [DataRow("AAAAAAAAAAAAA")]
    public void RecordEnvironmentVariableConflict_ShouldCollectClrIeTelemetry_IfCoreClrProfilerVariableAndCollectorSpecifiesClrIeProfile(string profilerGuid)
    {
        // arrange
        _dataCollectorInformation.SetTestExecutionEnvironmentVariables();

        // act
        _telemetryManager.RecordEnvironmentVariableConflict(_dataCollectorInformation, "CORECLR_PROFILER", profilerGuid, "{324F817A-7420-4E6D-B3C1-143FBED6D855}");

        // assert
        _mockMetricsCollection.Verify(c => c.Add($"VS.TestPlatform.DataCollector.CoreClrProfiler.{_dataCollectorInformation.DataCollectorConfig.TypeUri}", "324f817a-7420-4e6d-b3c1-143fbed6d855"), Times.Once);
    }

    [TestMethod]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-C11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{324F817A-7420-4E6D-B3C1-143FBED6D855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{9317ae81-bcd8-47b7-aaa1-a28062e41c71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71}", "aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-c11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{9317AE81-bcd8-47b7-AAA1-A28062E41C71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("AAAAAAAAAAAAA", "00000000-0000-0000-0000-000000000000")]
    public void RecordEnvironmentVariableConflict_ShouldCollectTelemetry_IfCorProfilerVariableAndBothValuesSame(string profilerGuid, string profilerName)
    {
        // act
        _telemetryManager.RecordEnvironmentVariableConflict(_dataCollectorInformation, "COR_PROFILER", profilerGuid, profilerGuid.ToLower(CultureInfo.InvariantCulture));

        // assert
        _mockMetricsCollection.Verify(c => c.Add($"VS.TestPlatform.DataCollector.CorProfiler.{_dataCollectorInformation.DataCollectorConfig.TypeUri}", profilerName), Times.Once);
    }

    [TestMethod]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-C11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{324F817A-7420-4E6D-B3C1-143FBED6D855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{9317ae81-bcd8-47b7-aaa1-a28062e41c71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71}", "aaaaaaaa-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("{E5F256DC-7959-4DD6-8E4F-c11150AB28E0}", "e5f256dc-7959-4dd6-8e4f-c11150ab28e0")]
    [DataRow("{324f817a-7420-4e6d-b3c1-143fbEd6d855}", "324f817a-7420-4e6d-b3c1-143fbed6d855")]
    [DataRow("{9317AE81-bcd8-47b7-AAA1-A28062E41C71}", "9317ae81-bcd8-47b7-aaa1-a28062e41c71")]
    [DataRow("AAAAAAAAAAAAA", "00000000-0000-0000-0000-000000000000")]
    public void RecordEnvironmentVariableConflict_ShouldCollectTelemetry_IfCoreClrProfilerVariableAndBothValuesSame(string profilerGuid, string profilerName)
    {
        // act
        _telemetryManager.RecordEnvironmentVariableConflict(_dataCollectorInformation, "CORECLR_PROFILER", profilerGuid, profilerGuid.ToUpper(CultureInfo.InvariantCulture));

        // assert
        _mockMetricsCollection.Verify(c => c.Add($"VS.TestPlatform.DataCollector.CoreClrProfiler.{_dataCollectorInformation.DataCollectorConfig.TypeUri}", profilerName), Times.Once);
    }
}
