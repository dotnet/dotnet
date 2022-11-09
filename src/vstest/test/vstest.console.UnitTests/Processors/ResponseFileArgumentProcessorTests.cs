// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class ResponseFileArgumentProcessorTests
{
    [TestCleanup]
    public void TestCleanup()
    {
        CommandLineOptions.Reset();
    }

    [TestMethod]
    public void GetMetadataShouldReturnResponseFileArgumentProcessorCapabilities()
    {
        var processor = new ResponseFileArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is ResponseFileArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnNull()
    {
        var processor = new ResponseFileArgumentProcessor();
        Assert.IsNull(processor.Executor);
    }

    #region ResponseFileArgumentProcessorCapabilities tests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new ResponseFileArgumentProcessorCapabilities();
        Assert.AreEqual("@", capabilities.CommandName);
        StringAssert.Contains(capabilities.HelpContentResourceName, "Read response file for more options");

        Assert.AreEqual(HelpContentPriority.ResponseFileArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Normal, capabilities.Priority);

        Assert.IsTrue(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsTrue(capabilities.IsSpecialCommand);
    }

    #endregion
}
