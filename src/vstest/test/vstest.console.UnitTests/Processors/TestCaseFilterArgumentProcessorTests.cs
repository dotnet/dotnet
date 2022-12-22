// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class TestCaseFilterArgumentProcessorTests
{
    [TestMethod]
    public void GetMetadataShouldReturnTestCaseFilterArgumentProcessorCapabilities()
    {
        TestCaseFilterArgumentProcessor processor = new();
        Assert.IsTrue(processor.Metadata.Value is TestCaseFilterArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecutorShouldReturnTestCaseFilterArgumentProcessorCapabilities()
    {
        TestCaseFilterArgumentProcessor processor = new();
        Assert.IsTrue(processor.Executor!.Value is TestCaseFilterArgumentExecutor);
    }

    #region TestCaseFilterArgumentProcessorCapabilitiesTests

    [TestMethod]
    public void CapabilitiesShouldAppropriateProperties()
    {
        TestCaseFilterArgumentProcessorCapabilities capabilities = new();
        Assert.AreEqual("/TestCaseFilter", capabilities.CommandName);
        StringAssert.Contains(capabilities.HelpContentResourceName, "/TestCaseFilter:<Expression>" + Environment.NewLine + "      Run tests that match the given expression." + Environment.NewLine + "      <Expression> is of the format <property>Operator<value>[|&<Expression>]");

        Assert.AreEqual(HelpContentPriority.TestCaseFilterArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Normal, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    [TestMethod]
    public void ExecutorInitializeWithNullOrEmptyTestCaseFilterShouldThrowCommandLineException()
    {
        var options = CommandLineOptions.Instance;
        TestCaseFilterArgumentExecutor executor = new(options);

        try
        {
            executor.Initialize(null);
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex is CommandLineException);
            StringAssert.Contains(ex.Message, @"The /TestCaseFilter argument requires the filter value.");
        }
    }

    [TestMethod]
    public void ExecutorInitializeWithNullOrEmptyTestCaseFilterShouldNotThrowWhenTestFilterWasSpecifiedByPreviousStep()
    {
        var options = CommandLineOptions.Instance;
        options.TestCaseFilterValue = "Test=FilterFromPreviousStep";
        TestCaseFilterArgumentExecutor executor = new(options);

        executor.Initialize(null);
    }

    [TestMethod]
    public void ExecutorInitializeWithTestCaseFilterShouldMergeWithTheValueProvidedByPreviousStep()
    {
        var options = CommandLineOptions.Instance;
        var defaultValue = "Test=FilterFromPreviousStep";
        options.TestCaseFilterValue = defaultValue;
        Assert.AreEqual(defaultValue, options.TestCaseFilterValue);
        TestCaseFilterArgumentExecutor executor = new(options);

        var value = "Test=NewFilter";
        executor.Initialize(value);

        var expectedValue = $"({defaultValue})&({value})";
        Assert.AreEqual(expectedValue, options.TestCaseFilterValue);
    }

    [TestMethod]
    public void ExecutorExecutoreturnArgumentProcessorResultSuccess()
    {
        var executor = new TestCaseFilterArgumentExecutor(CommandLineOptions.Instance);
        var result = executor.Execute();
        Assert.AreEqual(ArgumentProcessorResult.Success, result);
    }
}
