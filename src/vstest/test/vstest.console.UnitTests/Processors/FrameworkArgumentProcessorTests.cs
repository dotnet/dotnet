// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using vstest.console.UnitTests.Processors;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class FrameworkArgumentProcessorTests
{
    private readonly FrameworkArgumentExecutor _executor;
    private readonly TestableRunSettingsProvider _runSettingsProvider;

    public FrameworkArgumentProcessorTests()
    {
        _runSettingsProvider = new TestableRunSettingsProvider();
        _executor = new FrameworkArgumentExecutor(CommandLineOptions.Instance, _runSettingsProvider);
    }
    [TestCleanup]
    public void TestCleanup()
    {
        CommandLineOptions.Reset();
    }

    [TestMethod]
    public void GetMetadataShouldReturnFrameworkArgumentProcessorCapabilities()
    {
        var processor = new FrameworkArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is FrameworkArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnFrameworkArgumentExecutor()
    {
        var processor = new FrameworkArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is FrameworkArgumentExecutor);
    }

    #region FrameworkArgumentProcessorCapabilities tests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new FrameworkArgumentProcessorCapabilities();
        Assert.AreEqual("/Framework", capabilities.CommandName);
        StringAssert.Contains(capabilities.HelpContentResourceName, "Valid values are \".NETFramework,Version=v4.5.1\", \".NETCoreApp,Version=v1.0\"");

        Assert.AreEqual(HelpContentPriority.FrameworkArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.AutoUpdateRunSettings, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    #region FrameworkArgumentExecutor Initialize tests

    [TestMethod]
    public void InitializeShouldThrowIfArgumentIsNull()
    {

        ExceptionUtilities.ThrowsException<CommandLineException>(
            () => _executor.Initialize(null),
            "The /Framework argument requires the target .Net Framework version for the test run.   Example:  /Framework:\".NETFramework,Version=v4.5.1\"");
    }

    [TestMethod]
    public void InitializeShouldThrowIfArgumentIsEmpty()
    {
        ExceptionUtilities.ThrowsException<CommandLineException>(
            () => _executor.Initialize("  "),
            "The /Framework argument requires the target .Net Framework version for the test run.   Example:  /Framework:\".NETFramework,Version=v4.5.1\"");
    }

    [TestMethod]
    public void InitializeShouldThrowIfArgumentIsInvalid()
    {
        ExceptionUtilities.ThrowsException<CommandLineException>(
            () => _executor.Initialize("foo"),
            "Invalid .Net Framework version:{0}. Please give the fullname of the TargetFramework(Example: .NETCoreApp,Version=v2.0). Other supported .Net Framework versions are Framework40, Framework45, FrameworkCore10 and FrameworkUap10.",
            "foo");
    }

    [TestMethod]
    public void InitializeShouldSetCommandLineOptionsAndRunSettingsFramework()
    {
        _executor.Initialize(".NETCoreApp,Version=v1.0");
        Assert.AreEqual(".NETCoreApp,Version=v1.0", CommandLineOptions.Instance.TargetFrameworkVersion!.Name);
        Assert.AreEqual(".NETCoreApp,Version=v1.0", _runSettingsProvider.QueryRunSettingsNode(FrameworkArgumentExecutor.RunSettingsPath));
    }

    [TestMethod]
    public void InitializeShouldSetCommandLineOptionsFrameworkForOlderFrameworks()
    {
        _executor.Initialize("Framework35");
        Assert.AreEqual(".NETFramework,Version=v3.5", CommandLineOptions.Instance.TargetFrameworkVersion!.Name);
        Assert.AreEqual(".NETFramework,Version=v3.5", _runSettingsProvider.QueryRunSettingsNode(FrameworkArgumentExecutor.RunSettingsPath));
    }

    [TestMethod]
    public void InitializeShouldSetCommandLineOptionsFrameworkForCaseInsensitiveFramework()
    {
        _executor.Initialize(".netcoreApp,Version=v1.0");
        Assert.AreEqual(".NETCoreApp,Version=v1.0", CommandLineOptions.Instance.TargetFrameworkVersion!.Name);
        Assert.AreEqual(".NETCoreApp,Version=v1.0", _runSettingsProvider.QueryRunSettingsNode(FrameworkArgumentExecutor.RunSettingsPath));
    }

    [TestMethod]
    public void InitializeShouldNotSetFrameworkIfSettingsFileIsLegacy()
    {
        _runSettingsProvider.UpdateRunSettingsNode(FrameworkArgumentExecutor.RunSettingsPath, nameof(FrameworkVersion.Framework45));
        CommandLineOptions.Instance.SettingsFile = @"c:\tmp\settings.testsettings";
        _executor.Initialize(".NETFramework,Version=v3.5");
        Assert.AreEqual(".NETFramework,Version=v3.5", CommandLineOptions.Instance.TargetFrameworkVersion!.Name);
        Assert.AreEqual(nameof(FrameworkVersion.Framework45), _runSettingsProvider.QueryRunSettingsNode(FrameworkArgumentExecutor.RunSettingsPath));
    }

    #endregion

    #region FrameworkArgumentExecutor Execute tests

    [TestMethod]
    public void ExecuteShouldReturnSuccess()
    {
        Assert.AreEqual(ArgumentProcessorResult.Success, _executor.Execute());
    }

    #endregion

}
