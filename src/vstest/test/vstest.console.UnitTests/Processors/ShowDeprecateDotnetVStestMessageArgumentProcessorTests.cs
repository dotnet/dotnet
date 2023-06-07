// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace vstest.console.UnitTests.Processors;

[TestClass]
[TestCategory("Windows-Review")]
public class ShowDeprecateDotnetVStestMessageArgumentProcessorTests
{
    [TestMethod]
    public void ShowDeprecateDotnetVStestMessageProcessorCommandName()
    {
        Assert.AreEqual("/ShowDeprecateDotnetVSTestMessage", ShowDeprecateDotnetVStestMessageArgumentProcessor.CommandName);
    }

    [TestMethod]
    public void ShowDeprecateDotnetVStestMessageProcessorCapabilities()
    {
        ShowDeprecateDotnetVStestMessageProcessorCapabilities showDeprecateDotnetVStestMessageProcessorCapabilities = new();
        Assert.IsNull(showDeprecateDotnetVStestMessageProcessorCapabilities.HelpContentResourceName);
        Assert.IsFalse(showDeprecateDotnetVStestMessageProcessorCapabilities.IsAction);
        Assert.IsFalse(showDeprecateDotnetVStestMessageProcessorCapabilities.AllowMultiple);
        Assert.AreEqual(ArgumentProcessorPriority.CliRunSettings, showDeprecateDotnetVStestMessageProcessorCapabilities.Priority);
        Assert.AreEqual(HelpContentPriority.None, showDeprecateDotnetVStestMessageProcessorCapabilities.HelpPriority);
    }

    [TestMethod]
    public void ShowDeprecateDotnetVStestMessageArgumentProcessorReturnsCorrectTypes()
    {
        ShowDeprecateDotnetVStestMessageArgumentProcessor showDeprecateDotnetVStestMessageArgumentProcessor = new();
        Assert.IsInstanceOfType(showDeprecateDotnetVStestMessageArgumentProcessor.Executor!.Value, typeof(ShowDeprecateDotnetVStestMessageProcessorExecutor));
        Assert.IsInstanceOfType(showDeprecateDotnetVStestMessageArgumentProcessor.Metadata!.Value, typeof(ShowDeprecateDotnetVStestMessageProcessorCapabilities));
    }


    [TestMethod]
    public void ShowDeprecateDotnetVStestMessageProcessorExecutor_Succeeded()
    {
        ShowDeprecateDotnetVStestMessageProcessorExecutor showDeprecateDotnetVStestMessageProcessorExecutor = new();
        showDeprecateDotnetVStestMessageProcessorExecutor.Initialize("we will ignore the param");
        Assert.AreEqual(ArgumentProcessorResult.Success, showDeprecateDotnetVStestMessageProcessorExecutor.Execute());
    }
}
