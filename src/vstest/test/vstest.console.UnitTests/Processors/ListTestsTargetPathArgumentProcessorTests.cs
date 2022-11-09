// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class ListTestsTargetPathArgumentProcessorTests
{
    [TestMethod]
    public void GetMetadataShouldReturnListTestsTargetPathArgumentProcessorCapabilities()
    {
        ListTestsTargetPathArgumentProcessor processor = new();
        Assert.IsTrue(processor.Metadata.Value is ListTestsTargetPathArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecutorShouldReturnListTestsTargetPathArgumentProcessorCapabilities()
    {
        ListTestsTargetPathArgumentProcessor processor = new();
        Assert.IsTrue(processor.Executor!.Value is ListTestsTargetPathArgumentExecutor);
    }

    #region TestCaseFilterArgumentProcessorCapabilitiesTests

    [TestMethod]
    public void CapabilitiesShouldAppropriateProperties()
    {
        ListTestsTargetPathArgumentProcessorCapabilities capabilities = new();
        Assert.AreEqual("/ListTestsTargetPath", capabilities.CommandName);

        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Normal, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    [TestMethod]
    public void ExecutorInitializeWithNullOrEmptyListTestsTargetPathShouldThrowCommandLineException()
    {
        var options = CommandLineOptions.Instance;
        ListTestsTargetPathArgumentExecutor executor = new(options);

        try
        {
            executor.Initialize(null);
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex is CommandLineException);
            StringAssert.Contains(ex.Message, "ListTestsTargetPath is required with ListFullyQualifiedTests!");
        }
    }

    [TestMethod]
    public void ExecutorInitializeWithValidListTestsTargetPathShouldAddListTestsTargetPathToCommandLineOptions()
    {
        var options = CommandLineOptions.Instance;
        ListTestsTargetPathArgumentExecutor executor = new(options);

        executor.Initialize(@"C:\sample.txt");
        Assert.AreEqual(@"C:\sample.txt", options.ListTestsTargetPath);
    }

    [TestMethod]
    public void ExecutorListTestsTargetPathArgumentProcessorResultSuccess()
    {
        var executor = new ListTestsTargetPathArgumentExecutor(CommandLineOptions.Instance);
        var result = executor.Execute();
        Assert.AreEqual(ArgumentProcessorResult.Success, result);
    }
}
