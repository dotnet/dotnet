// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class ArtifactProcessingPostProcessModeProcessorTest
{
    private readonly Mock<IArtifactProcessingManager> _artifactProcessingManagerMock = new();
    private readonly Mock<IFeatureFlag> _featureFlagMock = new();

    [TestMethod]
    public void ProcessorExecutorInitialize_ShouldFailIfNullCtor()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new ArtifactProcessingPostProcessModeProcessorExecutor(null!, _artifactProcessingManagerMock.Object));
        Assert.ThrowsException<ArgumentNullException>(() => new ArtifactProcessingPostProcessModeProcessorExecutor(new CommandLineOptions(), null!));
    }

    [TestMethod]
    public void ProcessorExecutorInitialize_ShouldNotFailIfNullArg()
    {
        ArtifactProcessingPostProcessModeProcessorExecutor artifactProcessingPostProcessModeProcessorExecutor = new(new CommandLineOptions(), _artifactProcessingManagerMock.Object);
        artifactProcessingPostProcessModeProcessorExecutor.Initialize(null);
    }

    [TestMethod]
    public void ProcessorExecutorInitialize_ShouldSetCommandOption()
    {
        var commandOptions = new CommandLineOptions();
        ArtifactProcessingPostProcessModeProcessorExecutor artifactProcessingPostProcessModeProcessorExecutor = new(commandOptions, _artifactProcessingManagerMock.Object);
        artifactProcessingPostProcessModeProcessorExecutor.Initialize(null);
        Assert.AreEqual(ArtifactProcessingMode.PostProcess, commandOptions.ArtifactProcessingMode);
    }

    [TestMethod]
    public void ProcessorCapabilities()
    {
        ArtifactProcessingPostProcessModeProcessorCapabilities processorCapabilities = new();
        Assert.IsNull(processorCapabilities.HelpContentResourceName);
        Assert.AreEqual(ArgumentProcessorPriority.Normal, processorCapabilities.Priority);
        Assert.AreEqual(HelpContentPriority.None, processorCapabilities.HelpPriority);
        Assert.AreEqual("/ArtifactsProcessingMode-PostProcess", processorCapabilities.CommandName);
    }

    [TestMethod]
    public void ProcessorExecutorInitialize_ExceptionShouldNotBubbleUp()
    {
        _artifactProcessingManagerMock.Setup(x => x.PostProcessArtifactsAsync()).Callback(() => throw new Exception());
        ArtifactProcessingPostProcessModeProcessorExecutor artifactProcessingPostProcessModeProcessorExecutor = new(new CommandLineOptions(), _artifactProcessingManagerMock.Object);
        artifactProcessingPostProcessModeProcessorExecutor.Initialize(null);
        Assert.AreEqual(ArgumentProcessorResult.Fail, artifactProcessingPostProcessModeProcessorExecutor.Execute());
    }

    [TestMethod]
    public void ArtifactProcessingPostProcessMode_ContainsPostProcessCommand()
    {
        _featureFlagMock.Setup(x => x.IsSet(It.IsAny<string>())).Returns(false);
        Assert.IsTrue(ArtifactProcessingPostProcessModeProcessor.ContainsPostProcessCommand(new string[] { "--artifactsProcessingMode-postprocess" }, _featureFlagMock.Object));
        Assert.IsTrue(ArtifactProcessingPostProcessModeProcessor.ContainsPostProcessCommand(new string[] { "--ARTIfactsProcessingMode-postprocess" }, _featureFlagMock.Object));
        Assert.IsFalse(ArtifactProcessingPostProcessModeProcessor.ContainsPostProcessCommand(new string[] { "-ARTIfactsProcessingMode-postprocess" }, _featureFlagMock.Object));
        Assert.IsFalse(ArtifactProcessingPostProcessModeProcessor.ContainsPostProcessCommand(new string[] { "--ARTIfactsProcessingMode-postproces" }, _featureFlagMock.Object));
        Assert.IsFalse(ArtifactProcessingPostProcessModeProcessor.ContainsPostProcessCommand(null, _featureFlagMock.Object));
    }
}
