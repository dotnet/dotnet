// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using vstest.console.UnitTests.Processors;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class InIsolationArgumentProcessorTests
{
    private readonly InIsolationArgumentExecutor _executor;
    private readonly TestableRunSettingsProvider _runSettingsProvider;

    public InIsolationArgumentProcessorTests()
    {
        _runSettingsProvider = new TestableRunSettingsProvider();
        _executor = new InIsolationArgumentExecutor(CommandLineOptions.Instance, _runSettingsProvider);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        CommandLineOptions.Reset();
    }

    [TestMethod]
    public void GetMetadataShouldReturnInProcessArgumentProcessorCapabilities()
    {
        var processor = new InIsolationArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is InIsolationArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnInProcessArgumentExecutor()
    {
        var processor = new InIsolationArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is InIsolationArgumentExecutor);
    }

    [TestMethod]
    public void InIsolationArgumentProcessorMetadataShouldProvideAppropriateCapabilities()
    {
        var isolationProcessor = new InIsolationArgumentProcessor();
        Assert.IsFalse(isolationProcessor.Metadata.Value.AllowMultiple);
        Assert.IsFalse(isolationProcessor.Metadata.Value.AlwaysExecute);
        Assert.IsFalse(isolationProcessor.Metadata.Value.IsAction);
        Assert.IsFalse(isolationProcessor.Metadata.Value.IsSpecialCommand);
        Assert.AreEqual(InIsolationArgumentProcessor.CommandName, isolationProcessor.Metadata.Value.CommandName);
        Assert.IsNull(isolationProcessor.Metadata.Value.ShortCommandName);
        Assert.AreEqual(ArgumentProcessorPriority.AutoUpdateRunSettings, isolationProcessor.Metadata.Value.Priority);
        Assert.AreEqual(HelpContentPriority.InIsolationArgumentProcessorHelpPriority, isolationProcessor.Metadata.Value.HelpPriority);
        Assert.AreEqual("--InIsolation|/InIsolation" + Environment.NewLine + "      Runs the tests in an isolated process. This makes vstest.console.exe " + Environment.NewLine + "      process less likely to be stopped on an error in the tests, but tests " + Environment.NewLine + "      may run slower.", isolationProcessor.Metadata.Value.HelpContentResourceName);
    }

    [TestMethod]
    public void InIsolationArgumentProcessorExecutorShouldThrowIfArgumentIsProvided()
    {
        // InProcess should not have any values or arguments
        ExceptionUtilities.ThrowsException<CommandLineException>(
            () => _executor.Initialize("true"),
            "Argument true is not expected in the 'InIsolation' command. Specify the command without the argument (Example: vstest.console.exe myTests.dll /InIsolation) and try again.");
    }

    [TestMethod]
    public void InitializeShouldSetInIsolationValue()
    {
        _executor.Initialize(null);
        Assert.IsTrue(CommandLineOptions.Instance.InIsolation, "InProcess option must be set to true.");
        Assert.AreEqual("true", _runSettingsProvider.QueryRunSettingsNode(InIsolationArgumentExecutor.RunSettingsPath));
    }

    [TestMethod]
    public void ExecuteShouldReturnSuccess()
    {
        Assert.AreEqual(ArgumentProcessorResult.Success, _executor.Execute());
    }
}
