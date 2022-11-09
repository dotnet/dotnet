// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CommandLine;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace vstest.console.UnitTests.Processors;

[TestClass]
public class UseVsixExtensionsArgumentProcessorTests
{
    private const string DeprecationMessage = @"/UseVsixExtensions is getting deprecated. Please use /TestAdapterPath instead.";
    private readonly Mock<ITestRequestManager> _testRequestManager;
    private readonly Mock<IVSExtensionManager> _extensionManager;
    private readonly Mock<IOutput> _output;
    private readonly UseVsixExtensionsArgumentExecutor _executor;

    public UseVsixExtensionsArgumentProcessorTests()
    {
        _testRequestManager = new Mock<ITestRequestManager>();
        _extensionManager = new Mock<IVSExtensionManager>();
        _output = new Mock<IOutput>();
        _executor = new UseVsixExtensionsArgumentExecutor(CommandLineOptions.Instance, _testRequestManager.Object, _extensionManager.Object, _output.Object);
    }

    [TestMethod]
    public void GetMetadataShouldReturnUseVsixExtensionsArgumentProcessorCapabilities()
    {
        var processor = new UseVsixExtensionsArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is UseVsixExtensionsArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnUseVsixExtensionsArgumentProcessorCapabilities()
    {
        var processor = new UseVsixExtensionsArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is UseVsixExtensionsArgumentExecutor);
    }

    #region UseVsixExtensionsArgumentProcessorCapabilities tests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new UseVsixExtensionsArgumentProcessorCapabilities();

        Assert.AreEqual("/UseVsixExtensions", capabilities.CommandName);
        Assert.IsNull(capabilities.HelpContentResourceName);
        Assert.AreEqual(HelpContentPriority.None, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.AutoUpdateRunSettings, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    #region UseVsixExtensionsArgumentExecutor tests

    [TestMethod]
    public void InitializeShouldThrowExceptionIfArgumentIsNull()
    {
        var message = Assert.ThrowsException<CommandLineException>(() => _executor.Initialize(null)).Message;
        Assert.AreEqual(@"The /UseVsixExtensions parameter requires a value. If 'true', the installed VSIX extensions (if any) will be used in the test run. If false, they will be ignored.   Example:  /UseVsixExtensions:true", message);
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfArgumentIsInvalid()
    {
        var invalidArg = "Foo";

        var message = Assert.ThrowsException<CommandLineException>(() => _executor.Initialize(invalidArg)).Message;
        Assert.AreEqual(@"Argument Foo is not expected in the 'UseVsixExtensions' command. Specify the command indicating whether the vsix extensions should be used or skipped (Example: vstest.console.exe myTests.dll /UseVsixExtensions:true) and try again.", message);
    }

    [TestMethod]
    public void InitializeForArgumentEqualTrueShouldCallTestRequestManagerInitializeExtensions()
    {
        var extensions = new List<string> { "T1.dll", "T2.dll" };
        _extensionManager.Setup(em => em.GetUnitTestExtensions()).Returns(extensions);

        _executor.Initialize("true");

        _output.Verify(o => o.WriteLine(DeprecationMessage, OutputLevel.Warning), Times.Once);
        _extensionManager.Verify(em => em.GetUnitTestExtensions(), Times.Once);
        _testRequestManager.Verify(trm => trm.InitializeExtensions(extensions, true), Times.Once);
    }

    [TestMethod]
    public void InitializeForArgumentEqualfalseShouldNotCallTestRequestManagerInitializeExtensions()
    {
        _executor.Initialize("false");

        _output.Verify(o => o.WriteLine(DeprecationMessage, OutputLevel.Warning), Times.Once);
        _extensionManager.Verify(em => em.GetUnitTestExtensions(), Times.Never);
        _testRequestManager.Verify(trm => trm.InitializeExtensions(It.IsAny<IEnumerable<string>>(), true), Times.Never);
    }

    #endregion
}
