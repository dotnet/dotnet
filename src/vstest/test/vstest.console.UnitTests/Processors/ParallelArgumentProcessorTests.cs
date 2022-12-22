// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using vstest.console.UnitTests.Processors;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class ParallelArgumentProcessorTests
{
    private readonly ParallelArgumentExecutor _executor;
    private readonly TestableRunSettingsProvider _runSettingsProvider;

    public ParallelArgumentProcessorTests()
    {
        _runSettingsProvider = new TestableRunSettingsProvider();
        _executor = new ParallelArgumentExecutor(CommandLineOptions.Instance, _runSettingsProvider);
    }
    [TestCleanup]
    public void TestCleanup()
    {
        CommandLineOptions.Reset();
    }

    [TestMethod]
    public void GetMetadataShouldReturnParallelArgumentProcessorCapabilities()
    {
        var processor = new ParallelArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is ParallelArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnParallelArgumentExecutor()
    {
        var processor = new ParallelArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is ParallelArgumentExecutor);
    }

    #region ParallelArgumentProcessorCapabilities tests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new ParallelArgumentProcessorCapabilities();
        Assert.AreEqual("/Parallel", capabilities.CommandName);
        var expected = "--Parallel|/Parallel\r\n      Specifies that the tests be executed in parallel. By default up\r\n      to all available cores on the machine may be used.\r\n      The number of cores to use may be configured using a settings file.";
        Assert.AreEqual(expected.NormalizeLineEndings().ShowWhiteSpace(), capabilities.HelpContentResourceName.NormalizeLineEndings().ShowWhiteSpace());

        Assert.AreEqual(HelpContentPriority.ParallelArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.AutoUpdateRunSettings, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    #region ParallelArgumentExecutor Initialize tests

    [TestMethod]
    public void InitializeShouldThrowIfArgumentIsNonNull()
    {

        // Parallel should not have any values or arguments
        ExceptionUtilities.ThrowsException<CommandLineException>(
            () => _executor.Initialize("123"),
            "Argument " + 123 + " is not expected in the 'Parallel' command. Specify the command without the argument (Example: vstest.console.exe myTests.dll /Parallel) and try again.");
    }

    [TestMethod]
    public void InitializeShouldSetParallelValue()
    {
        _executor.Initialize(null);
        Assert.IsTrue(CommandLineOptions.Instance.Parallel, "Parallel option must be set to true.");
        Assert.AreEqual("0", _runSettingsProvider.QueryRunSettingsNode(ParallelArgumentExecutor.RunSettingsPath));
    }

    #endregion

    #region ParallelArgumentExecutor Execute tests

    [TestMethod]
    public void ExecuteShouldReturnSuccess()
    {
        Assert.AreEqual(ArgumentProcessorResult.Success, _executor.Execute());
    }

    #endregion
}
