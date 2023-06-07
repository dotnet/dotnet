// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.DataCollector;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

using Moq;
using System.Globalization;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.UnitTests;

[TestClass]
public class DataCollectorMainTests
{
    private readonly string[] _args = { "--port", "1025", "--parentprocessid", "100", "--diag", "abc.txt", "--tracelevel", "3" };
    private readonly string[] _argsWithEmptyDiagArg = { "--port", "1025", "--parentprocessid", "100", "--diag", "", "--tracelevel", "3" };
    private readonly string[] _argsWithInvalidTraceLevel = { "--port", "1025", "--parentprocessid", "100", "--diag", "abc.txt", "--tracelevel", "5" };

    private static readonly string TimeoutErrorMessage =
        "datacollector process failed to connect to vstest.console process after 90 seconds. This may occur due to machine slowness, please set environment variable VSTEST_CONNECTION_TIMEOUT to increase timeout.";
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<IEnvironment> _mockEnvironment;
    private readonly Mock<IDataCollectionRequestHandler> _mockDataCollectionRequestHandler;
    private readonly DataCollectorMain _dataCollectorMain;

    public DataCollectorMainTests()
    {
        _mockProcessHelper = new Mock<IProcessHelper>();
        _mockEnvironment = new Mock<IEnvironment>();
        _mockDataCollectionRequestHandler = new Mock<IDataCollectionRequestHandler>();
        _dataCollectorMain = new DataCollectorMain(_mockProcessHelper.Object, _mockEnvironment.Object, _mockDataCollectionRequestHandler.Object, new());
        _mockDataCollectionRequestHandler.Setup(rh => rh.WaitForRequestSenderConnection(It.IsAny<int>())).Returns(true);
    }

    [TestCleanup]
    public void CleanUp()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "");
    }

    [TestMethod]
    public void RunShouldTimeoutBasedOnEnvVariable()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, "10");
        _dataCollectorMain.Run(_args);
        _mockDataCollectionRequestHandler.Verify(rh => rh.WaitForRequestSenderConnection(10 * 1000));
    }

    [TestMethod]
    public void RunShouldTimeoutBasedDefaultValueIfEnvVariableNotSet()
    {
        _dataCollectorMain.Run(_args);

        _mockDataCollectionRequestHandler.Verify(rh => rh.WaitForRequestSenderConnection(EnvironmentHelper.DefaultConnectionTimeout * 1000));
    }

    [TestMethod]
    public void RunShouldInitializeTraceWithTraceLevelOffIfDiagArgIsEmpty()
    {
        // Setting EqtTrace.TraceLevel to a value other than info.
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Verbose;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Verbose;
#endif
        // Action
        _dataCollectorMain.Run(_argsWithEmptyDiagArg); // Passing tracelevel as info and diag file path is empty.

        // Verify
        Assert.AreEqual(TraceLevel.Off, (TraceLevel)EqtTrace.TraceLevel);
    }

    [TestMethod]
    public void RunShouldInitializeTraceWithVerboseTraceLevelIfInvalidTraceLevelPassed()
    {
        // Setting EqtTrace.TraceLevel to a value other than info.
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Info;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Info;
#endif
        // Action
        _dataCollectorMain.Run(_argsWithInvalidTraceLevel);

        // Verify
        Assert.AreEqual(TraceLevel.Verbose, (TraceLevel)EqtTrace.TraceLevel);
    }

    [TestMethod]
    public void RunShouldInitializeTraceWithCorrectVerboseTraceLevel()
    {
        // Setting EqtTrace.TraceLevel to a value other than info.
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Verbose;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Verbose;
#endif
        // Action
        _dataCollectorMain.Run(_args); // Trace level is set as info in args.

        // Verify
        Assert.AreEqual(TraceLevel.Info, (TraceLevel)EqtTrace.TraceLevel);
    }

    [TestMethod]
    public void RunShouldThrowIfTimeoutOccured()
    {
        _mockDataCollectionRequestHandler.Setup(rh => rh.WaitForRequestSenderConnection(It.IsAny<int>())).Returns(false);
        var message = Assert.ThrowsException<TestPlatformException>(() => _dataCollectorMain.Run(_args)).Message;
        Assert.AreEqual(TimeoutErrorMessage, message);
    }

    [TestMethod]
    public void RunWhenCliUiLanguageIsSetChangesCultureAndFlowsOverride()
    {
        // Arrange
        var culture = new CultureInfo("fr-fr");
        var envVarMock = new Mock<IEnvironmentVariableHelper>();
        envVarMock.Setup(x => x.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE")).Returns(culture.Name);

        bool threadCultureWasSet = false;
        var dataCollectorMain = new DataCollectorMain(_mockProcessHelper.Object, _mockEnvironment.Object, _mockDataCollectionRequestHandler.Object,
            new(envVarMock.Object, lang => threadCultureWasSet = lang.Equals(culture)));

        // Act
        dataCollectorMain.Run(_args);

        // Assert
        Assert.IsTrue(threadCultureWasSet, "DefaultThreadCurrentUICulture was not set");
        envVarMock.Verify(x => x.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE"), Times.Exactly(2));
        envVarMock.Verify(x => x.GetEnvironmentVariable("VSLANG"), Times.Once);
        envVarMock.Verify(x => x.SetEnvironmentVariable("VSLANG", culture.LCID.ToString(CultureInfo.InvariantCulture)), Times.Once);
        envVarMock.Verify(x => x.GetEnvironmentVariable("PreferredUILang"), Times.Once);
        envVarMock.Verify(x => x.SetEnvironmentVariable("PreferredUILang", culture.Name), Times.Once);
    }

    [TestMethod]
    public void RunWhenVsLangIsSetChangesCultureAndFlowsOverride()
    {
        // Arrange
        var culture = new CultureInfo("fr-fr");
        var envVarMock = new Mock<IEnvironmentVariableHelper>();
        envVarMock.Setup(x => x.GetEnvironmentVariable("VSLANG")).Returns(culture.LCID.ToString(CultureInfo.InvariantCulture));

        bool threadCultureWasSet = false;
        var dataCollectorMain = new DataCollectorMain(_mockProcessHelper.Object, _mockEnvironment.Object, _mockDataCollectionRequestHandler.Object,
            new(envVarMock.Object, lang => threadCultureWasSet = lang.Equals(culture)));

        // Act
        dataCollectorMain.Run(_args);

        // Assert
        Assert.IsTrue(threadCultureWasSet, "DefaultThreadCurrentUICulture was not set");
        envVarMock.Verify(x => x.GetEnvironmentVariable("VSLANG"), Times.Exactly(2));
        envVarMock.Verify(x => x.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE"), Times.Exactly(2));
        envVarMock.Verify(x => x.SetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", culture.Name), Times.Once);
        envVarMock.Verify(x => x.GetEnvironmentVariable("PreferredUILang"), Times.Once);
        envVarMock.Verify(x => x.SetEnvironmentVariable("PreferredUILang", culture.Name), Times.Once);
    }

    [TestMethod]
    public void RunWhenNoCultureEnvVarSetDoesNotChangeCultureNorFlowsOverride()
    {
        // Arrange
        var envVarMock = new Mock<IEnvironmentVariableHelper>();
        envVarMock.Setup(x => x.GetEnvironmentVariable(It.IsAny<string>())).Returns(default(string));

        bool threadCultureWasSet = false;
        var dataCollectorMain = new DataCollectorMain(_mockProcessHelper.Object, _mockEnvironment.Object, _mockDataCollectionRequestHandler.Object,
            new(envVarMock.Object, lang => threadCultureWasSet = true));

        // Act
        dataCollectorMain.Run(_args);

        // Assert
        Assert.IsFalse(threadCultureWasSet, "DefaultThreadCurrentUICulture was set");
        envVarMock.Verify(x => x.GetEnvironmentVariable("VSLANG"), Times.Once);
        envVarMock.Verify(x => x.SetEnvironmentVariable("VSLANG", It.IsAny<string>()), Times.Never);
        envVarMock.Verify(x => x.GetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE"), Times.Once);
        envVarMock.Verify(x => x.SetEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", It.IsAny<string>()), Times.Never);
        envVarMock.Verify(x => x.GetEnvironmentVariable("PreferredUILang"), Times.Never);
        envVarMock.Verify(x => x.SetEnvironmentVariable("PreferredUILang", It.IsAny<string>()), Times.Never);
    }
}
