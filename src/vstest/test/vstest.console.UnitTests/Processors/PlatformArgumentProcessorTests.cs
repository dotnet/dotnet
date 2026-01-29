// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using vstest.console.UnitTests.Processors;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class PlatformArgumentProcessorTests
{
    private readonly PlatformArgumentExecutor _executor;
    private readonly TestableRunSettingsProvider _runSettingsProvider;

    public PlatformArgumentProcessorTests()
    {
        _runSettingsProvider = new TestableRunSettingsProvider();
        _executor = new PlatformArgumentExecutor(CommandLineOptions.Instance, _runSettingsProvider);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        CommandLineOptions.Reset();
    }

    [TestMethod]
    public void GetMetadataShouldReturnPlatformArgumentProcessorCapabilities()
    {
        var processor = new PlatformArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is PlatformArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnPlatformArgumentExecutor()
    {
        var processor = new PlatformArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is PlatformArgumentExecutor);
    }

    #region PlatformArgumentProcessorCapabilities tests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new PlatformArgumentProcessorCapabilities();
        Assert.AreEqual("/Platform", capabilities.CommandName);
        var expected = "--Platform|/Platform:<Platform type>\r\n      Target platform architecture to be used for test execution. \r\n      Valid values are x86, x64 and ARM.";
        Assert.AreEqual(expected: expected.NormalizeLineEndings().ShowWhiteSpace(), capabilities.HelpContentResourceName.NormalizeLineEndings().ShowWhiteSpace());

        Assert.AreEqual(HelpContentPriority.PlatformArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.AutoUpdateRunSettings, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    #region PlatformArgumentExecutor Initialize tests

    [TestMethod]
    public void InitializeShouldThrowIfArgumentIsNull()
    {
        ExceptionUtilities.ThrowsException<CommandLineException>(
            () => _executor.Initialize(null),
            "The /Platform argument requires the target platform type for the test run to be provided.   Example:  /Platform:x86");
    }

    [TestMethod]
    public void InitializeShouldThrowIfArgumentIsEmpty()
    {
        ExceptionUtilities.ThrowsException<CommandLineException>(
            () => _executor.Initialize("  "),
            "The /Platform argument requires the target platform type for the test run to be provided.   Example:  /Platform:x86");
    }

    [TestMethod]
    public void InitializeShouldThrowIfArgumentIsNotAnArchitecture()
    {
        ExceptionUtilities.ThrowsException<CommandLineException>(
            () => _executor.Initialize("foo"),
            "Invalid platform type: {0}. Valid platform types are X86, X64, ARM, ARM64, S390x, Ppc64le, RiscV64, LoongArch64.",
            "foo");
    }

    [TestMethod]
    public void InitializeShouldThrowIfArgumentIsNotASupportedArchitecture()
    {
        ExceptionUtilities.ThrowsException<CommandLineException>(
            () => _executor.Initialize("AnyCPU"),
            "Invalid platform type: {0}. Valid platform types are X86, X64, ARM, ARM64, S390x, Ppc64le, RiscV64, LoongArch64.",
            "AnyCPU");
    }

    [TestMethod]
    public void InitializeShouldSetCommandLineOptionsArchitecture()
    {
        _executor.Initialize("x64");
        Assert.AreEqual(ObjectModel.Architecture.X64, CommandLineOptions.Instance.TargetArchitecture);
        Assert.AreEqual(nameof(ObjectModel.Architecture.X64), _runSettingsProvider.QueryRunSettingsNode(PlatformArgumentExecutor.RunSettingsPath));
    }

    [TestMethod]
    public void InitializeShouldNotConsiderCaseSensitivityOfTheArgumentPassed()
    {
        _executor.Initialize("ArM");
        Assert.AreEqual(ObjectModel.Architecture.ARM, CommandLineOptions.Instance.TargetArchitecture);
        Assert.AreEqual(nameof(ObjectModel.Architecture.ARM), _runSettingsProvider.QueryRunSettingsNode(PlatformArgumentExecutor.RunSettingsPath));
    }

    #endregion

    #region PlatformArgumentExecutor Execute tests

    [TestMethod]
    public void ExecuteShouldReturnSuccess()
    {
        Assert.AreEqual(ArgumentProcessorResult.Success, _executor.Execute());
    }

    #endregion
}
