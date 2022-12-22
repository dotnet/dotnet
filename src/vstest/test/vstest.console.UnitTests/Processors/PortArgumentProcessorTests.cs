// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
#if !NET5_0_OR_GREATER
using System.Diagnostics;
#endif
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Client.DesignMode;
using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class PortArgumentProcessorTests
{
    private readonly Mock<IProcessHelper> _mockProcessHelper;
    private readonly Mock<IDesignModeClient> _testDesignModeClient;
    private readonly Mock<ITestRequestManager> _testRequestManager;
    private PortArgumentExecutor _executor;

    public PortArgumentProcessorTests()
    {
        _mockProcessHelper = new Mock<IProcessHelper>();
        _testDesignModeClient = new Mock<IDesignModeClient>();
        _testRequestManager = new Mock<ITestRequestManager>();
        _executor = new PortArgumentExecutor(CommandLineOptions.Instance, _testRequestManager.Object);
    }

    [TestMethod]
    public void GetMetadataShouldReturnPortArgumentProcessorCapabilities()
    {
        var processor = new PortArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is PortArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecutorShouldReturnPortArgumentProcessorCapabilities()
    {
        var processor = new PortArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is PortArgumentExecutor);
    }

    #region PortArgumentProcessorCapabilitiesTests

    [TestMethod]
    public void CapabilitiesShouldAppropriateProperties()
    {
        var capabilities = new PortArgumentProcessorCapabilities();
        Assert.AreEqual("/Port", capabilities.CommandName);
        Assert.AreEqual("--Port|/Port:<Port>" + Environment.NewLine + "      The Port for socket connection and receiving the event messages.", capabilities.HelpContentResourceName);

        Assert.AreEqual(HelpContentPriority.PortArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.DesignMode, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    [TestMethod]
    public void ExecutorInitializeWithNullOrEmptyPortShouldThrowCommandLineException()
    {
        try
        {
            _executor.Initialize(null);
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex is CommandLineException);
            Assert.AreEqual("The --Port|/Port argument requires the port number which is an integer. Specify the port for socket connection and receiving the event messages.", ex.Message);
        }
    }

    [TestMethod]
    public void ExecutorInitializeWithInvalidPortShouldThrowCommandLineException()
    {
        try
        {
            _executor.Initialize("Foo");
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex is CommandLineException);
            Assert.AreEqual("The --Port|/Port argument requires the port number which is an integer. Specify the port for socket connection and receiving the event messages.", ex.Message);
        }
    }

    [TestMethod]
    public void ExecutorInitializeWithValidPortShouldAddPortToCommandLineOptionsAndInitializeDesignModeManager()
    {
        int port = 2345;
        CommandLineOptions.Instance.ParentProcessId = 0;

        _executor.Initialize(port.ToString(CultureInfo.InvariantCulture));

        Assert.AreEqual(port, CommandLineOptions.Instance.Port);
        Assert.IsNotNull(DesignModeClient.Instance);
    }

    [TestMethod]
    public void ExecutorInitializeShouldSetDesignMode()
    {
        int port = 2345;
        CommandLineOptions.Instance.ParentProcessId = 0;

        _executor.Initialize(port.ToString(CultureInfo.InvariantCulture));

        Assert.IsTrue(CommandLineOptions.Instance.IsDesignMode);
    }

    [TestMethod]
    public void ExecutorInitializeShouldSetProcessExitCallback()
    {
        _executor = new PortArgumentExecutor(CommandLineOptions.Instance, _testRequestManager.Object, _mockProcessHelper.Object);
        int port = 2345;
#if NET5_0_OR_GREATER
        var pid = Environment.ProcessId;
#else
        int pid;
        using (var p = Process.GetCurrentProcess())
            pid = p.Id;
#endif
        CommandLineOptions.Instance.ParentProcessId = pid;

        _executor.Initialize(port.ToString(CultureInfo.InvariantCulture));

        _mockProcessHelper.Verify(ph => ph.SetExitCallback(pid, It.IsAny<Action<object?>>()), Times.Once);
    }

    [TestMethod]
    public void ExecutorExecuteForValidConnectionReturnsArgumentProcessorResultSuccess()
    {
        _executor = new PortArgumentExecutor(CommandLineOptions.Instance, _testRequestManager.Object,
            (parentProcessId, ph) => _testDesignModeClient.Object, _mockProcessHelper.Object);

        int port = 2345;
        _executor.Initialize(port.ToString(CultureInfo.InvariantCulture));
        var result = _executor.Execute();

        _testDesignModeClient.Verify(td =>
            td.ConnectToClientAndProcessRequests(port, _testRequestManager.Object), Times.Once);

        Assert.AreEqual(ArgumentProcessorResult.Success, result);
    }

    [TestMethod]
    public void ExecutorExecuteForFailedConnectionShouldThrowCommandLineException()
    {
        _executor = new PortArgumentExecutor(CommandLineOptions.Instance, _testRequestManager.Object,
            (parentProcessId, ph) => _testDesignModeClient.Object, _mockProcessHelper.Object);

        _testDesignModeClient.Setup(td => td.ConnectToClientAndProcessRequests(It.IsAny<int>(),
            It.IsAny<ITestRequestManager>())).Callback(() => throw new TimeoutException());

        int port = 2345;
        _executor.Initialize(port.ToString(CultureInfo.InvariantCulture));
        Assert.ThrowsException<CommandLineException>(() => _executor.Execute());

        _testDesignModeClient.Verify(td => td.ConnectToClientAndProcessRequests(port, _testRequestManager.Object), Times.Once);
    }


    [TestMethod]
    public void ExecutorExecuteSetsParentProcessIdOnDesignModeInitializer()
    {
        var parentProcessId = 2346;
        var parentProcessIdArgumentExecutor = new ParentProcessIdArgumentExecutor(CommandLineOptions.Instance);
        parentProcessIdArgumentExecutor.Initialize(parentProcessId.ToString(CultureInfo.InvariantCulture));

        int actualParentProcessId = -1;
        _executor = new PortArgumentExecutor(CommandLineOptions.Instance,
            _testRequestManager.Object,
            (ppid, ph) =>
            {
                actualParentProcessId = ppid;
                return _testDesignModeClient.Object;
            },
            _mockProcessHelper.Object
        );

        int port = 2345;
        _executor.Initialize(port.ToString(CultureInfo.InvariantCulture));
        var result = _executor.Execute();

        _testDesignModeClient.Verify(td =>
            td.ConnectToClientAndProcessRequests(port, _testRequestManager.Object), Times.Once);

        Assert.AreEqual(parentProcessId, actualParentProcessId, "Parent process Id must be set correctly on design mode initializer");

        Assert.AreEqual(ArgumentProcessorResult.Success, result);
    }
}
