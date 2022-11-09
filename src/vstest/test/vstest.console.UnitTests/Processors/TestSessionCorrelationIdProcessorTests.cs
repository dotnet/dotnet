// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class TestSessionCorrelationIdProcessorTests
{
    [TestMethod]
    public void ProcessorExecutorInitialize_ShouldFailIfNullCommandOption() =>
        Assert.ThrowsException<ArgumentNullException>(() => new TestSessionCorrelationIdProcessorModeProcessorExecutor(null!));

    [TestMethod]
    public void ProcessorExecutorInitialize_ShouldFailIfNullSession()
    {
        TestSessionCorrelationIdProcessorModeProcessorExecutor testSessionCorrelationIdProcessor = new(new CommandLineOptions());
        Assert.ThrowsException<CommandLineException>(() => testSessionCorrelationIdProcessor.Initialize(null));
    }

    [TestMethod]
    public void ProcessorExecutorInitialize_ShouldSetCommandOption()
    {
        var commandOptions = new CommandLineOptions();
        TestSessionCorrelationIdProcessorModeProcessorExecutor testSessionCorrelationIdProcessor = new(commandOptions);
        testSessionCorrelationIdProcessor.Initialize("sessionId");
        Assert.IsNotNull(commandOptions.TestSessionCorrelationId);
    }

    [TestMethod]
    public void ProcessorCapabilities()
    {
        TestSessionCorrelationIdProcessorCapabilities processorCapabilities = new();
        Assert.IsNull(processorCapabilities.HelpContentResourceName);
        Assert.AreEqual(ArgumentProcessorPriority.CliRunSettings, processorCapabilities.Priority);
        Assert.AreEqual(HelpContentPriority.None, processorCapabilities.HelpPriority);
        Assert.AreEqual("/TestSessionCorrelationId", processorCapabilities.CommandName);
    }
}
