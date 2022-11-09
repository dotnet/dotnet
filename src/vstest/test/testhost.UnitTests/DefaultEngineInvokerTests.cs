// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace testhost.UnitTests;

[TestClass]
public class DefaultEngineInvokerTests
{
    private const int ParentProcessId = 27524;
    private static readonly IDictionary<string, string?> ArgsDictionary = new Dictionary<string, string?>
    {
        { "--port", "21291" },
        { "--endpoint", "127.0.0.1:021291"  },
        { "--role", "client"},
        { "--parentprocessid", ParentProcessId.ToString(CultureInfo.InvariantCulture) },
        { "--diag", "temp.txt"},
        { "--tracelevel", "3"},
        { "--telemetryoptedin", "false"},
        { "--datacollectionport", "21290"}
    };
    private static readonly string TimeoutErrorMessage =
        "testhost process failed to connect to datacollector process after 90 seconds. This may occur due to machine slowness, please set environment variable VSTEST_CONNECTION_TIMEOUT to increase timeout.";

    private readonly Mock<ITestRequestHandler> _mockTestRequestHandler;
    private readonly Mock<IDataCollectionTestCaseEventSender> _mockDataCollectionTestCaseEventSender;
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly DefaultEngineInvoker _engineInvoker;

    public DefaultEngineInvokerTests()
    {
        _mockDataCollectionTestCaseEventSender = new Mock<IDataCollectionTestCaseEventSender>();
        _mockTestRequestHandler = new Mock<ITestRequestHandler>();
        _mockProcessHelper = new Mock<IProcessHelper>();
        _engineInvoker = new DefaultEngineInvoker(
            _mockTestRequestHandler.Object,
            _mockDataCollectionTestCaseEventSender.Object,
            _mockProcessHelper.Object);
        _mockTestRequestHandler.Setup(h => h.WaitForRequestSenderConnection(It.IsAny<int>())).Returns(true);
        _mockDataCollectionTestCaseEventSender.Setup(s => s.WaitForRequestSenderConnection(It.IsAny<int>())).Returns(true);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, string.Empty);
    }

    [TestMethod]
    public void InvokeShouldWaitForDefaultTimeoutIfNoEnvVariableSetDuringDataCollectorConnection()
    {
        _engineInvoker.Invoke(ArgsDictionary);

        _mockDataCollectionTestCaseEventSender.Verify(s => s.WaitForRequestSenderConnection(EnvironmentHelper.DefaultConnectionTimeout * 1000));
    }

    [TestMethod]
    public void InvokeShouldWaitBasedOnTimeoutEnvVariableDuringDataCollectorConnection()
    {
        var timeout = 10;
        Environment.SetEnvironmentVariable(EnvironmentHelper.VstestConnectionTimeout, timeout.ToString(CultureInfo.InvariantCulture));
        _engineInvoker.Invoke(ArgsDictionary);

        _mockDataCollectionTestCaseEventSender.Verify(s => s.WaitForRequestSenderConnection(timeout * 1000));
    }

    [TestMethod]
    public void InvokeShouldThrowExceptionIfDataCollectorConnection()
    {
        _mockDataCollectionTestCaseEventSender.Setup(s => s.WaitForRequestSenderConnection(It.IsAny<int>())).Returns(false);
        var message = Assert.ThrowsException<TestPlatformException>(() => _engineInvoker.Invoke(ArgsDictionary)).Message;

        Assert.AreEqual(message, TimeoutErrorMessage);
    }

    [TestMethod]
    public void InvokeShouldSetParentProcessExistCallback()
    {
        _engineInvoker.Invoke(ArgsDictionary);

        _mockProcessHelper.Verify(h => h.SetExitCallback(ParentProcessId, It.IsAny<Action<object?>>()));
    }

    [TestMethod]
    public void InvokeShouldInitializeTraceWithCorrectTraceLevel()
    {
        // Setting EqtTrace.TraceLevel to a value other than info.
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Verbose;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Verbose;
#endif

        _engineInvoker.Invoke(ArgsDictionary);

        // Verify
        Assert.AreEqual(TraceLevel.Info, (TraceLevel)EqtTrace.TraceLevel);
    }

    [TestMethod]
    public void InvokeShouldInitializeTraceWithVerboseTraceLevelIfInvalidTraceLevelPassed()
    {
        // Setting EqtTrace.TraceLevel to a value other than info.
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Warning;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Warning;
#endif

        try
        {
            ArgsDictionary["--tracelevel"] = "5"; // int value which is not defined in TraceLevel.
            _engineInvoker.Invoke(ArgsDictionary);
        }
        finally
        {
            ArgsDictionary["--tracelevel"] = "3"; // Setting to default value of 3.
        }

        // Verify
        Assert.AreEqual(TraceLevel.Verbose, (TraceLevel)EqtTrace.TraceLevel);
    }
}
