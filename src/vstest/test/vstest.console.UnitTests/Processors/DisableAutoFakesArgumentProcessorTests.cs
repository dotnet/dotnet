// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class DisableAutoFakesArgumentProcessorTests
{
    private readonly DisableAutoFakesArgumentProcessor _disableAutoFakesArgumentProcessor;

    public DisableAutoFakesArgumentProcessorTests()
    {
        _disableAutoFakesArgumentProcessor = new DisableAutoFakesArgumentProcessor();
    }

    [TestMethod]
    public void DisableAutoFakesArgumentProcessorMetadataShouldProvideAppropriateCapabilities()
    {
        Assert.IsFalse(_disableAutoFakesArgumentProcessor.Metadata.Value.AllowMultiple);
        Assert.IsFalse(_disableAutoFakesArgumentProcessor.Metadata.Value.AlwaysExecute);
        Assert.IsFalse(_disableAutoFakesArgumentProcessor.Metadata.Value.IsAction);
        Assert.IsFalse(_disableAutoFakesArgumentProcessor.Metadata.Value.IsSpecialCommand);
        Assert.AreEqual(DisableAutoFakesArgumentProcessor.CommandName, _disableAutoFakesArgumentProcessor.Metadata.Value.CommandName);
        Assert.IsNull(_disableAutoFakesArgumentProcessor.Metadata.Value.ShortCommandName);
        Assert.AreEqual(ArgumentProcessorPriority.Normal, _disableAutoFakesArgumentProcessor.Metadata.Value.Priority);
        Assert.AreEqual(HelpContentPriority.DisableAutoFakesArgumentProcessorHelpPriority, _disableAutoFakesArgumentProcessor.Metadata.Value.HelpPriority);
    }


    [TestMethod]
    public void DisableAutoFakesArgumentProcessorExecutorShouldThrowIfArgumentIsNullOrEmpty()
    {
        Assert.ThrowsException<CommandLineException>(() => _disableAutoFakesArgumentProcessor.Executor!.Value.Initialize(string.Empty));
        Assert.ThrowsException<CommandLineException>(() => _disableAutoFakesArgumentProcessor.Executor!.Value.Initialize(" "));
    }

    [TestMethod]
    public void DisableAutoFakesArgumentProcessorExecutorShouldThrowIfArgumentIsNotBooleanString()
    {
        Assert.ThrowsException<CommandLineException>(() => _disableAutoFakesArgumentProcessor.Executor!.Value.Initialize("DisableAutoFakes"));
    }

    [TestMethod]
    public void DisableAutoFakesArgumentProcessorExecutorShouldSetCommandLineDisableAutoFakeValueAsPerArgumentProvided()
    {
        _disableAutoFakesArgumentProcessor.Executor!.Value.Initialize("true");
        Assert.IsTrue(CommandLineOptions.Instance.DisableAutoFakes);
    }
}
