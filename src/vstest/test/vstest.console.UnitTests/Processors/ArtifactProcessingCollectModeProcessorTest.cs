// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class ArtifactProcessingCollectModeProcessorTest
{
    [TestMethod]
    public void ProcessorExecutorInitialize_ShouldFailIfNullCommandOption() =>
        Assert.ThrowsException<ArgumentNullException>(() => new ArtifactProcessingCollectModeProcessorExecutor(null!));

    [TestMethod]
    public void ProcessorExecutorInitialize_ShouldNotFailIfNullArg()
    {
        ArtifactProcessingCollectModeProcessorExecutor artifactProcessingCollectModeProcessorExecutor = new(new CommandLineOptions());
        artifactProcessingCollectModeProcessorExecutor.Initialize(null);
    }

    [TestMethod]
    public void ProcessorExecutorInitialize_ShouldSetCommandOption()
    {
        var commandOptions = new CommandLineOptions();
        ArtifactProcessingCollectModeProcessorExecutor artifactProcessingCollectModeProcessorExecutor = new(commandOptions);
        artifactProcessingCollectModeProcessorExecutor.Initialize(null);
        Assert.AreEqual(ArtifactProcessingMode.Collect, commandOptions.ArtifactProcessingMode);
    }

    [TestMethod]
    public void ProcessorCapabilities()
    {
        ArtifactProcessingCollectModeProcessorCapabilities processorCapabilities = new();
        Assert.IsNull(processorCapabilities.HelpContentResourceName);
        Assert.AreEqual(ArgumentProcessorPriority.CliRunSettings, processorCapabilities.Priority);
        Assert.AreEqual(HelpContentPriority.None, processorCapabilities.HelpPriority);
        Assert.AreEqual("/ArtifactsProcessingMode-Collect", processorCapabilities.CommandName);
    }
}
