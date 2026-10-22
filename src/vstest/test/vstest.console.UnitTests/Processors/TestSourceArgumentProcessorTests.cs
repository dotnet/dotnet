// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using vstest.console.Internal;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;
// <summary>
// Tests for TestSourceArgumentProcessor
// </summary>
[TestClass]
public class TestSourceArgumentProcessorTests
{
    /// <summary>
    /// The help argument processor get metadata should return help argument processor capabilities.
    /// </summary>
    [TestMethod]
    public void GetMetadataShouldReturnTestSourceArgumentProcessorCapabilities()
    {
        TestSourceArgumentProcessor processor = new();
        Assert.IsTrue(processor.Metadata.Value is TestSourceArgumentProcessorCapabilities);
    }

    /// <summary>
    /// The help argument processor get executer should return help argument processor capabilities.
    /// </summary>
    [TestMethod]
    public void GetExecuterShouldReturnTestSourceArgumentProcessorCapabilities()
    {
        TestSourceArgumentProcessor processor = new();
        Assert.IsTrue(processor.Executor!.Value is TestSourceArgumentExecutor);
    }

    #region TestSourceArgumentProcessorCapabilitiesTests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        TestSourceArgumentProcessorCapabilities capabilities = new();
        Assert.AreEqual("/TestSource", capabilities.CommandName);
        Assert.IsNull(capabilities.HelpContentResourceName);

        Assert.AreEqual(HelpContentPriority.None, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Normal, capabilities.Priority);

        Assert.IsTrue(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsTrue(capabilities.IsSpecialCommand);
    }

    #endregion

    #region TestSourceArgumentExecutorTests

    [TestMethod]
    public void ExecuterInitializeWithInvalidSourceShouldThrowCommandLineException()
    {
        var options = CommandLineOptions.Instance;
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(x => x.GetCurrentDirectory()).Returns("");
        options.FileHelper = mockFileHelper.Object;
        TestSourceArgumentExecutor executor = new(options);

        // This path is invalid
        string testFilePath = "TestFile.txt";

        try
        {
            executor.Initialize(testFilePath);
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex is TestSourceException);
            StringAssert.StartsWith(ex.Message, "The test source file \"");
            StringAssert.EndsWith(ex.Message, testFilePath + "\" provided was not found.");
        }
    }

    [TestMethod]
    public void ExecuterInitializeWithValidSourceShouldAddItToTestSources()
    {
        var testFilePath = "DummyTestFile.txt";
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(testFilePath)).Returns(true);
        mockFileHelper.Setup(x => x.GetCurrentDirectory()).Returns("");

        var options = CommandLineOptions.Instance;
        CommandLineOptions.Reset();
        options.FileHelper = mockFileHelper.Object;
        options.FilePatternParser = new FilePatternParser(new Mock<Matcher>().Object, mockFileHelper.Object);
        var executor = new TestSourceArgumentExecutor(options);

        executor.Initialize(testFilePath);

        // Check if the testsource is present in the TestSources
        Assert.IsTrue(options.Sources.Contains(testFilePath));
    }

    [TestMethod]
    public void ExecutorExecuteReturnArgumentProcessorResultSuccess()
    {
        var options = CommandLineOptions.Instance;
        var executor = new TestSourceArgumentExecutor(options);
        var result = executor.Execute();
        Assert.AreEqual(ArgumentProcessorResult.Success, result);
    }

    #endregion
}
