// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.UnitTests;

[TestClass]
public class DataCollectionManagerTests
{
    private readonly DataCollectionManager _dataCollectionManager;
    private readonly string _defaultRunSettings = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<RunSettings>\r\n  <DataCollectionRunSettings>\r\n    <DataCollectors >{0}</DataCollectors>\r\n  </DataCollectionRunSettings>\r\n</RunSettings>";
    private readonly string _defaultDataCollectionSettings = "<DataCollector friendlyName=\"{0}\" uri=\"{1}\" assemblyQualifiedName=\"{2}\" codebase=\"{3}\" {4} />";
    private string _dataCollectorSettings;
    private readonly string _friendlyName;
    private readonly string _uri;
    private readonly Mock<IMessageSink> _mockMessageSink;
    private readonly Mock<DataCollector2> _mockDataCollector;
    private readonly Mock<CodeCoverageDataCollector> _mockCodeCoverageDataCollector;
    private readonly List<KeyValuePair<string, string>> _envVarList;
    private readonly List<KeyValuePair<string, string>> _codeCoverageEnvVarList;
    private readonly Mock<IDataCollectionAttachmentManager> _mockDataCollectionAttachmentManager;
    private readonly Mock<IDataCollectionTelemetryManager> _mockDataCollectionTelemetryManager;
    private readonly Mock<ITelemetryReporter> _mockTelemetryReporter;

    public DataCollectionManagerTests()
    {
        _friendlyName = "CustomDataCollector";
        _uri = "my://custom/datacollector";
        _envVarList = new List<KeyValuePair<string, string>>();
        _codeCoverageEnvVarList = new List<KeyValuePair<string, string>>();
        _mockDataCollector = new Mock<DataCollector2>();
        _mockDataCollector.As<ITestExecutionEnvironmentSpecifier>().Setup(x => x.GetTestExecutionEnvironmentVariables()).Returns(_envVarList);
        _mockCodeCoverageDataCollector = new Mock<CodeCoverageDataCollector>();
        _mockCodeCoverageDataCollector.As<ITestExecutionEnvironmentSpecifier>().Setup(x => x.GetTestExecutionEnvironmentVariables()).Returns(_codeCoverageEnvVarList);
        _dataCollectorSettings = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, _friendlyName, _uri, _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty));
        _mockMessageSink = new Mock<IMessageSink>();
        _mockDataCollectionAttachmentManager = new Mock<IDataCollectionAttachmentManager>();
        _mockDataCollectionAttachmentManager.SetReturnsDefault(new List<AttachmentSet>());
        _mockDataCollectionTelemetryManager = new Mock<IDataCollectionTelemetryManager>();
        _mockTelemetryReporter = new Mock<ITelemetryReporter>();

        _dataCollectionManager = new TestableDataCollectionManager(_mockDataCollectionAttachmentManager.Object, _mockMessageSink.Object, _mockDataCollector.Object, _mockCodeCoverageDataCollector.Object, _mockDataCollectionTelemetryManager.Object, _mockTelemetryReporter.Object);
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldThrowExceptionIfSettingsXmlIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _dataCollectionManager.InitializeDataCollectors(null!));
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldReturnEmptyDictionaryIfDataCollectorsAreNotConfigured()
    {
        var runSettings = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Empty);
        _dataCollectionManager.InitializeDataCollectors(runSettings);

        Assert.AreEqual(0, _dataCollectionManager.RunDataCollectors.Count);
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldLoadDataCollector()
    {
        var dataCollectorSettings = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, _friendlyName, _uri, _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty));
        _dataCollectionManager.InitializeDataCollectors(dataCollectorSettings);

        Assert.IsTrue(_dataCollectionManager.RunDataCollectors.ContainsKey(_mockDataCollector.Object.GetType()));
        Assert.AreEqual(typeof(AttachmentProcessorDataCollector2), _dataCollectionManager.RunDataCollectors[_mockDataCollector.Object.GetType()].DataCollectorConfig.AttachmentsProcessorType);
        Assert.IsTrue(_dataCollectionManager.RunDataCollectors[_mockDataCollector.Object.GetType()].DataCollectorConfig.Metadata.Contains(true));
        _mockDataCollector.Verify(x => x.Initialize(It.IsAny<XmlElement>(), It.IsAny<DataCollectionEvents>(), It.IsAny<DataCollectionSink>(), It.IsAny<DataCollectionLogger>(), It.IsAny<DataCollectionEnvironmentContext>()), Times.Once);
    }

    [TestMethod]
    public void InitializeShouldNotAddDataCollectorIfItIsDisabled()
    {
        var dataCollectorSettingsDisabled = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, _friendlyName, _uri, _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, "enabled=\"false\""));
        _dataCollectionManager.InitializeDataCollectors(dataCollectorSettingsDisabled);

        Assert.AreEqual(0, _dataCollectionManager.RunDataCollectors.Count);
        _mockDataCollector.Verify(x => x.Initialize(It.IsAny<XmlElement>(), It.IsAny<DataCollectionEvents>(), It.IsAny<DataCollectionSink>(), It.IsAny<DataCollectionLogger>(), It.IsAny<DataCollectionEnvironmentContext>()), Times.Never);
    }

    [TestMethod]
    public void InitializeShouldAddDataCollectorIfItIsEnabled()
    {
        var dataCollectorSettingsEnabled = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, _friendlyName, _uri, _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, "enabled=\"true\""));
        _dataCollectionManager.InitializeDataCollectors(dataCollectorSettingsEnabled);

        Assert.IsTrue(_dataCollectionManager.RunDataCollectors.ContainsKey(_mockDataCollector.Object.GetType()));
        _mockDataCollector.Verify(x => x.Initialize(It.IsAny<XmlElement>(), It.IsAny<DataCollectionEvents>(), It.IsAny<DataCollectionSink>(), It.IsAny<DataCollectionLogger>(), It.IsAny<DataCollectionEnvironmentContext>()), Times.Once);
        _mockDataCollector.Verify(x => x.Initialize(_mockTelemetryReporter.Object), Times.Once);
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldLoadDataCollectorIfFriendlyNameIsNotCorrectAndUriIsCorrect()
    {
        var dataCollectorSettingsWithWrongFriendlyName = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, "anyFriendlyName", _uri, _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty));
        _dataCollectionManager.InitializeDataCollectors(dataCollectorSettingsWithWrongFriendlyName);

        Assert.AreEqual(1, _dataCollectionManager.RunDataCollectors.Count);
        _mockDataCollector.Verify(x => x.Initialize(It.IsAny<XmlElement>(), It.IsAny<DataCollectionEvents>(), It.IsAny<DataCollectionSink>(), It.IsAny<DataCollectionLogger>(), It.IsAny<DataCollectionEnvironmentContext>()), Times.Once);
        _mockDataCollector.Verify(x => x.Initialize(_mockTelemetryReporter.Object), Times.Once);
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldLoadDataCollectorIfFriendlyNameIsCorrectAndUriIsNotCorrect()
    {
        var dataCollectorSettingsWithWrongUri = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, _friendlyName, "my://custom/WrongDatacollector", _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty));
        _dataCollectionManager.InitializeDataCollectors(dataCollectorSettingsWithWrongUri);

        Assert.AreEqual(1, _dataCollectionManager.RunDataCollectors.Count);
        _mockDataCollector.Verify(x => x.Initialize(It.IsAny<XmlElement>(), It.IsAny<DataCollectionEvents>(), It.IsAny<DataCollectionSink>(), It.IsAny<DataCollectionLogger>(), It.IsAny<DataCollectionEnvironmentContext>()), Times.Once);
        _mockDataCollector.Verify(x => x.Initialize(_mockTelemetryReporter.Object), Times.Once);
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldLoadDataCollectorIfFriendlyNameIsNullAndUriIsCorrect()
    {
        var dataCollectorSettingsWithNullFriendlyName = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, string.Empty, _uri, _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty).Replace("friendlyName=\"\"", string.Empty));
        _dataCollectionManager.InitializeDataCollectors(dataCollectorSettingsWithNullFriendlyName);

        Assert.AreEqual(1, _dataCollectionManager.RunDataCollectors.Count);
        _mockDataCollector.Verify(x => x.Initialize(It.IsAny<XmlElement>(), It.IsAny<DataCollectionEvents>(), It.IsAny<DataCollectionSink>(), It.IsAny<DataCollectionLogger>(), It.IsAny<DataCollectionEnvironmentContext>()), Times.Once);
        _mockDataCollector.Verify(x => x.Initialize(_mockTelemetryReporter.Object), Times.Once);
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldLoadDataCollectorIfFriendlyNameIsCorrectAndUriIsEmpty()
    {
        var dataCollectorSettingsWithEmptyUri = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, _friendlyName, string.Empty, _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty));
        Assert.ThrowsException<ArgumentNullException>(() => _dataCollectionManager.InitializeDataCollectors(dataCollectorSettingsWithEmptyUri));
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldLoadDataCollectorIfFriendlyNameIsEmptyAndUriIsCorrect()
    {
        var dataCollectorSettingsWithEmptyFriendlyName = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, _friendlyName, string.Empty, _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty));
        Assert.ThrowsException<ArgumentNullException>(() => _dataCollectionManager.InitializeDataCollectors(dataCollectorSettingsWithEmptyFriendlyName));
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldNotLoadDataCollectorIfFriendlyNameIsNotCorrectAndUriIsNotCorrect()
    {
        var dataCollectorSettingsWithWrongFriendlyNameAndWrongUri = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, "anyFriendlyName", "datacollector://data", _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty));
        _dataCollectionManager.InitializeDataCollectors(dataCollectorSettingsWithWrongFriendlyNameAndWrongUri);

        Assert.AreEqual(0, _dataCollectionManager.RunDataCollectors.Count);
        _mockDataCollector.Verify(x => x.Initialize(It.IsAny<XmlElement>(), It.IsAny<DataCollectionEvents>(), It.IsAny<DataCollectionSink>(), It.IsAny<DataCollectionLogger>(), It.IsAny<DataCollectionEnvironmentContext>()), Times.Never);
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldNotAddSameDataCollectorMoreThanOnce()
    {
        var datacollecterSettings = string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, "CustomDataCollector", "my://custom/datacollector", _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, "enabled =\"true\"");
        var runSettings = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, datacollecterSettings + datacollecterSettings);

        _dataCollectionManager.InitializeDataCollectors(runSettings);

        Assert.IsTrue(_dataCollectionManager.RunDataCollectors.ContainsKey(_mockDataCollector.Object.GetType()));
        _mockDataCollector.Verify(x => x.Initialize(It.IsAny<XmlElement>(), It.IsAny<DataCollectionEvents>(), It.IsAny<DataCollectionSink>(), It.IsAny<DataCollectionLogger>(), It.IsAny<DataCollectionEnvironmentContext>()), Times.Once);
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldLoadDataCollectorAndReturnEnvironmentVariables()
    {
        _envVarList.Add(new KeyValuePair<string, string>("key", "value"));

        var result = _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);

        Assert.AreEqual("value", result["key"]);

        _mockDataCollectionTelemetryManager.Verify(tm => tm.RecordEnvironmentVariableAddition(It.IsAny<DataCollectorInformation>(), "key", "value"));
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldLogExceptionToMessageSinkIfInitializationFails()
    {
        _mockDataCollector.Setup(
            x =>
                x.Initialize(
                    It.IsAny<XmlElement>(),
                    It.IsAny<DataCollectionEvents>(),
                    It.IsAny<DataCollectionSink>(),
                    It.IsAny<DataCollectionLogger>(),
                    It.IsAny<DataCollectionEnvironmentContext>())).Throws<Exception>();

        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);

        Assert.AreEqual(0, _dataCollectionManager.RunDataCollectors.Count);
        _mockMessageSink.Verify(x => x.SendMessage(It.IsAny<DataCollectionMessageEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldLogExceptionToMessageSinkIfSetEnvironmentVariableFails()
    {
        _mockDataCollector.As<ITestExecutionEnvironmentSpecifier>().Setup(x => x.GetTestExecutionEnvironmentVariables()).Throws<Exception>();

        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);

        Assert.AreEqual(0, _dataCollectionManager.RunDataCollectors.Count);
        _mockMessageSink.Verify(x => x.SendMessage(It.IsAny<DataCollectionMessageEventArgs>()), Times.Once);
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldReturnFirstEnvironmentVariableIfMoreThanOneVariablesWithSameKeyIsSpecified()
    {
        _envVarList.Add(new KeyValuePair<string, string>("key", "value"));
        _envVarList.Add(new KeyValuePair<string, string>("key", "value1"));

        var result = _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);

        Assert.AreEqual("value", result["key"]);

        _mockDataCollectionTelemetryManager.Verify(tm => tm.RecordEnvironmentVariableAddition(It.IsAny<DataCollectorInformation>(), "key", "value"));
        _mockDataCollectionTelemetryManager.Verify(tm => tm.RecordEnvironmentVariableConflict(It.IsAny<DataCollectorInformation>(), "key", "value1", "value"));
    }

    [TestMethod]
    public void InitializeDataCollectorsShouldReturnOtherThanCodeCoverageEnvironmentVariableIfMoreThanOneVariablesWithSameKeyIsSpecified()
    {
        _envVarList.Add(new KeyValuePair<string, string>("cor_profiler", "clrie"));
        _envVarList.Add(new KeyValuePair<string, string>("same_key", "same_value"));
        _codeCoverageEnvVarList.Add(new KeyValuePair<string, string>("cor_profiler", "direct"));
        _codeCoverageEnvVarList.Add(new KeyValuePair<string, string>("clrie_profiler_vanguard", "path"));
        _codeCoverageEnvVarList.Add(new KeyValuePair<string, string>("same_key", "same_value"));

        _dataCollectorSettings = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings,
            string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, "Code Coverage", "my://custom/ccdatacollector", _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty) +
            string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, _friendlyName, _uri, _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty));

        var result = _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);

        Assert.AreEqual(3, result.Count);
        Assert.AreEqual("clrie", result["cor_profiler"]);
        Assert.AreEqual("path", result["clrie_profiler_vanguard"]);
        Assert.AreEqual("same_value", result["same_key"]);

        _mockDataCollectionTelemetryManager.Verify(tm => tm.RecordEnvironmentVariableAddition(It.Is<DataCollectorInformation>(i => i.DataCollectorConfig.FriendlyName == _friendlyName), "cor_profiler", "clrie"));
        _mockDataCollectionTelemetryManager.Verify(tm => tm.RecordEnvironmentVariableConflict(It.Is<DataCollectorInformation>(i => i.DataCollectorConfig.FriendlyName == "Code Coverage"), "cor_profiler", "direct", "clrie"));
        _mockDataCollectionTelemetryManager.Verify(tm => tm.RecordEnvironmentVariableAddition(It.Is<DataCollectorInformation>(i => i.DataCollectorConfig.FriendlyName == "Code Coverage"), "clrie_profiler_vanguard", "path"));
        _mockDataCollectionTelemetryManager.Verify(tm => tm.RecordEnvironmentVariableAddition(It.Is<DataCollectorInformation>(i => i.DataCollectorConfig.FriendlyName == _friendlyName), "same_key", "same_value"));
        _mockDataCollectionTelemetryManager.Verify(tm => tm.RecordEnvironmentVariableConflict(It.Is<DataCollectorInformation>(i => i.DataCollectorConfig.FriendlyName == "Code Coverage"), "same_key", "same_value", "same_value"));
    }

    [TestMethod]
    public void SessionStartedShouldReturnFalseIfDataCollectionIsNotConfiguredInRunSettings()
    {
        var runSettings = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Empty);
        _dataCollectionManager.InitializeDataCollectors(runSettings);

        var sessionStartEventArgs = new SessionStartEventArgs();
        var result = _dataCollectionManager.SessionStarted(sessionStartEventArgs);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void SessionStartedShouldSendEventToDataCollector()
    {
        var isStartInvoked = false;
        SetupMockDataCollector((XmlElement a, DataCollectionEvents b, DataCollectionSink c, DataCollectionLogger d, DataCollectionEnvironmentContext e) => b.SessionStart += (sender, eventArgs) => isStartInvoked = true);

        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);

        var sessionStartEventArgs = new SessionStartEventArgs();
        var areTestCaseEventsSubscribed = _dataCollectionManager.SessionStarted(sessionStartEventArgs);

        Assert.IsTrue(isStartInvoked);
        Assert.IsFalse(areTestCaseEventsSubscribed);
    }

    [TestMethod]
    public void SessionStartedShouldReturnTrueIfTestCaseStartIsSubscribed()
    {
        SetupMockDataCollector((XmlElement a, DataCollectionEvents b, DataCollectionSink c, DataCollectionLogger d, DataCollectionEnvironmentContext e) => b.TestCaseStart += (sender, eventArgs) => { });

        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);

        var sessionStartEventArgs = new SessionStartEventArgs();
        var areTestCaseEventsSubscribed = _dataCollectionManager.SessionStarted(sessionStartEventArgs);

        Assert.IsTrue(areTestCaseEventsSubscribed);
    }

    [TestMethod]
    public void SessionStartedShouldContinueDataCollectionIfExceptionIsThrownWhileSendingEventsToDataCollector()
    {
        SetupMockDataCollector((XmlElement a, DataCollectionEvents b, DataCollectionSink c, DataCollectionLogger d, DataCollectionEnvironmentContext e) => b.SessionStart += (sender, eventArgs) => throw new Exception());

        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);

        var sessionStartEventArgs = new SessionStartEventArgs();
        var result = _dataCollectionManager.SessionStarted(sessionStartEventArgs);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void SessionStartedShouldReturnFalseIfDataCollectorsAreNotInitialized()
    {
        var sessionStartEventArgs = new SessionStartEventArgs();
        var result = _dataCollectionManager.SessionStarted(sessionStartEventArgs);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void SessionStartedShouldHaveCorrectSessionContext()
    {
        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);

        var sessionStartEventArgs = new SessionStartEventArgs();

        Assert.AreEqual(new SessionId(Guid.Empty), sessionStartEventArgs.Context.SessionId);

        _dataCollectionManager.SessionStarted(sessionStartEventArgs);

        Assert.AreNotEqual(new SessionId(Guid.Empty), sessionStartEventArgs.Context.SessionId);
    }

    [TestMethod]
    public void SessionEndedShouldReturnEmptyCollectionIfDataCollectionIsNotEnabled()
    {
        var runSettings = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Empty);
        _dataCollectionManager.InitializeDataCollectors(runSettings);

        var result = _dataCollectionManager.SessionEnded();

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetInvokedDataCollectorsShouldReturnDataCollector()
    {
        var dataCollectorSettingsWithNullFriendlyName = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, string.Format(CultureInfo.InvariantCulture, _defaultDataCollectionSettings, string.Empty, _uri, _mockDataCollector.Object.GetType().AssemblyQualifiedName, typeof(DataCollectionManagerTests).Assembly.Location, string.Empty).Replace("friendlyName=\"\"", string.Empty));
        _dataCollectionManager.InitializeDataCollectors(dataCollectorSettingsWithNullFriendlyName);
        var invokedDataCollector = _dataCollectionManager.GetInvokedDataCollectors();
        Assert.AreEqual(1, invokedDataCollector.Count);
        Assert.IsTrue(invokedDataCollector[0].HasAttachmentProcessor);
    }

    [TestMethod]
    public void SessionEndedShouldReturnAttachments()
    {
        var attachment = new AttachmentSet(new Uri("my://custom/datacollector"), "CustomDataCollector");
        attachment.Attachments.Add(new UriDataAttachment(new Uri("my://filename.txt"), "filename.txt"));

        _mockDataCollectionAttachmentManager.Setup(x => x.GetAttachments(It.IsAny<DataCollectionContext>())).Returns(new List<AttachmentSet>() { attachment });

        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);
        var sessionStartEventArgs = new SessionStartEventArgs();
        _dataCollectionManager.SessionStarted(sessionStartEventArgs);

        var result = _dataCollectionManager.SessionEnded();

        Assert.IsTrue(result[0].Attachments[0].Uri.ToString().Contains("filename.txt"));
    }

    [TestMethod]
    public void SessionEndedShouldNotReturnAttachmentsIfExceptionIsThrownWhileGettingAttachments()
    {
        _mockDataCollectionAttachmentManager.Setup(x => x.GetAttachments(It.IsAny<DataCollectionContext>())).Throws<Exception>();
        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);

        var result = _dataCollectionManager.SessionEnded();

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SessionEndedShouldContinueDataCollectionIfExceptionIsThrownWhileSendingSessionEndEventToDataCollector()
    {
        var attachment = new AttachmentSet(new Uri("my://custom/datacollector"), "CustomDataCollector");
        attachment.Attachments.Add(new UriDataAttachment(new Uri("my://filename.txt"), "filename.txt"));

        _mockDataCollectionAttachmentManager.Setup(x => x.GetAttachments(It.IsAny<DataCollectionContext>())).Returns(new List<AttachmentSet>() { attachment });

        SetupMockDataCollector((XmlElement a, DataCollectionEvents b, DataCollectionSink c, DataCollectionLogger d, DataCollectionEnvironmentContext e)
            => b.SessionEnd += (sender, ev) =>
            {
                c.SendFileAsync(e.SessionDataCollectionContext, "filename.txt", true);
                throw new Exception();
            });

        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);
        var sessionStartEventArgs = new SessionStartEventArgs();
        _dataCollectionManager.SessionStarted(sessionStartEventArgs);

        var result = _dataCollectionManager.SessionEnded();

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void SessionEndedShouldCancelProcessingAttachmentRequestsIfSessionIsCancelled()
    {
        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);
        var sessionStartEventArgs = new SessionStartEventArgs();
        _dataCollectionManager.SessionStarted(sessionStartEventArgs);

        var result = _dataCollectionManager.SessionEnded(true);

        _mockDataCollectionAttachmentManager.Verify(x => x.Cancel(), Times.Once);
    }

    #region TestCaseEventsTest

    [TestMethod]
    public void TestCaseStartedShouldSendEventToDataCollector()
    {
        var isStartInvoked = false;
        SetupMockDataCollector((XmlElement a, DataCollectionEvents b, DataCollectionSink c, DataCollectionLogger d, DataCollectionEnvironmentContext e) => b.TestCaseStart += (sender, eventArgs) => isStartInvoked = true);

        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);
        var args = new TestCaseStartEventArgs(new TestCase());
        _dataCollectionManager.TestCaseStarted(args);

        Assert.IsTrue(isStartInvoked);
    }

    [TestMethod]
    public void TestCaseStartedShouldNotSendEventToDataCollectorIfDataColletionIsNotEnbled()
    {
        var isStartInvoked = false;
        SetupMockDataCollector((XmlElement a, DataCollectionEvents b, DataCollectionSink c, DataCollectionLogger d, DataCollectionEnvironmentContext e) => b.TestCaseStart += (sender, eventArgs) => isStartInvoked = true);

        var args = new TestCaseStartEventArgs(new TestCase());
        _dataCollectionManager.TestCaseStarted(args);

        Assert.IsFalse(isStartInvoked);
    }

    [TestMethod]
    public void TestCaseEndedShouldSendEventToDataCollector()
    {
        var isEndInvoked = false;
        SetupMockDataCollector((XmlElement a, DataCollectionEvents b, DataCollectionSink c, DataCollectionLogger d, DataCollectionEnvironmentContext e) => b.TestCaseEnd += (sender, eventArgs) => isEndInvoked = true);

        _dataCollectionManager.InitializeDataCollectors(_dataCollectorSettings);
        var args = new TestCaseEndEventArgs();
        args.TestElement = new TestCase();
        _dataCollectionManager.TestCaseEnded(args);

        Assert.IsTrue(isEndInvoked);
    }

    [TestMethod]
    public void TestCaseEndedShouldNotSendEventToDataCollectorIfDataCollectionIsNotEnbled()
    {
        var isEndInvoked = false;
        var runSettings = string.Format(CultureInfo.InvariantCulture, _defaultRunSettings, _dataCollectorSettings);
        SetupMockDataCollector((XmlElement a, DataCollectionEvents b, DataCollectionSink c, DataCollectionLogger d, DataCollectionEnvironmentContext e) => b.TestCaseEnd += (sender, eventArgs) => isEndInvoked = true);

        var args = new TestCaseEndEventArgs();
        Assert.IsFalse(isEndInvoked);
    }

    private void SetupMockDataCollector(Action<XmlElement, DataCollectionEvents, DataCollectionSink, DataCollectionLogger, DataCollectionEnvironmentContext> callback)
    {
        _mockDataCollector.Setup(
            x =>
                x.Initialize(
                    It.IsAny<XmlElement>(),
                    It.IsAny<DataCollectionEvents>(),
                    It.IsAny<DataCollectionSink>(),
                    It.IsAny<DataCollectionLogger>(),
                    It.IsAny<DataCollectionEnvironmentContext>())).Callback<XmlElement, DataCollectionEvents, DataCollectionSink, DataCollectionLogger, DataCollectionEnvironmentContext>(callback.Invoke);
    }

    #endregion
}

internal class TestableDataCollectionManager : DataCollectionManager
{
    private readonly ObjectModel.DataCollection.DataCollector? _dataCollector;
    private readonly ObjectModel.DataCollection.DataCollector? _ccDataCollector;

    public TestableDataCollectionManager(IDataCollectionAttachmentManager datacollectionAttachmentManager, IMessageSink messageSink,
        ObjectModel.DataCollection.DataCollector dataCollector, ObjectModel.DataCollection.DataCollector ccDataCollector,
        IDataCollectionTelemetryManager dataCollectionTelemetryManager, ITelemetryReporter telemetryReporter) : this(datacollectionAttachmentManager, messageSink, dataCollectionTelemetryManager, telemetryReporter)
    {
        _dataCollector = dataCollector;
        _ccDataCollector = ccDataCollector;
    }

    internal TestableDataCollectionManager(IDataCollectionAttachmentManager datacollectionAttachmentManager, IMessageSink messageSink,
        IDataCollectionTelemetryManager dataCollectionTelemetryManager, ITelemetryReporter telemetryReporter) : base(datacollectionAttachmentManager, messageSink, dataCollectionTelemetryManager, telemetryReporter)
    {
    }

    protected override bool TryGetUriFromFriendlyName(string? friendlyName, out string dataCollectorUri)
    {
        if (friendlyName!.Equals("CustomDataCollector"))
        {
            dataCollectorUri = "my://custom/datacollector";
            return true;
        }
        else if (friendlyName.Equals("Code Coverage"))
        {
            dataCollectorUri = "my://custom/ccdatacollector";
            return true;
        }
        else
        {
            dataCollectorUri = string.Empty;
            return false;
        }
    }

    protected override bool IsUriValid(string? uri)
    {
        return string.Equals(uri, "my://custom/datacollector") || string.Equals(uri, "my://custom/ccdatacollector");
    }

    protected override ObjectModel.DataCollection.DataCollector TryGetTestExtension(string extensionUri)
    {
        if (extensionUri.Equals("my://custom/datacollector"))
        {
            return _dataCollector!;
        }

        if (extensionUri.Equals("my://custom/ccdatacollector"))
        {
            return _ccDataCollector!;
        }

        return null!;
    }

    protected override DataCollectorConfig? TryGetDataCollectorConfig(string extensionUri)
    {
        if (extensionUri.Equals("my://custom/datacollector"))
        {
            var dc = new DataCollectorConfig(_dataCollector!.GetType());
            dc.FilePath = Path.GetTempFileName();
            return dc;
        }

        if (extensionUri.Equals("my://custom/ccdatacollector"))
        {
            var dc = new DataCollectorConfig(_ccDataCollector!.GetType());
            dc.FilePath = Path.GetTempFileName();
            return dc;
        }

        return null;
    }
}

[DataCollectorFriendlyName("CustomDataCollector")]
[DataCollectorTypeUri("my://custom/datacollector")]
[DataCollectorAttachmentProcessor(typeof(AttachmentProcessorDataCollector2))]
public abstract class DataCollector2 : ObjectModel.DataCollection.DataCollector, ITelemetryInitializer
{
    public virtual void Initialize(ITelemetryReporter telemetryReporter)
    {
    }
}

[DataCollectorFriendlyName("Code Coverage")]
[DataCollectorTypeUri("my://custom/ccdatacollector")]
public abstract class CodeCoverageDataCollector : ObjectModel.DataCollection.DataCollector
{
}

public class AttachmentProcessorDataCollector2 : IDataCollectorAttachmentProcessor
{
    public bool SupportsIncrementalProcessing => throw new NotImplementedException();

    public IEnumerable<Uri> GetExtensionUris()
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<AttachmentSet>> ProcessAttachmentSetsAsync(XmlElement configurationElement, ICollection<AttachmentSet> attachments, IProgress<int> progressReporter, IMessageLogger logger, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
