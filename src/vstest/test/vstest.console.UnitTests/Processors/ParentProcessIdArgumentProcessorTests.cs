// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class ParentProcessIdArgumentProcessorTests
{
    [TestMethod]
    public void GetMetadataShouldReturnParentProcessIdArgumentProcessorCapabilities()
    {
        var processor = new ParentProcessIdArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is ParentProcessIdArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecutorShouldReturnParentProcessIdArgumentProcessorCapabilities()
    {
        var processor = new ParentProcessIdArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is ParentProcessIdArgumentExecutor);
    }

    [TestMethod]
    public void CapabilitiesShouldHaveHigherPriorityThanPortCapabilities()
    {
        var parentProcessIdCapabilities = new ParentProcessIdArgumentProcessorCapabilities();
        var portCapabilities = new PortArgumentProcessorCapabilities();

        // Less the number, high the priority
        Assert.IsTrue(parentProcessIdCapabilities.Priority == portCapabilities.Priority, "ParentProcessId must have higher priority than Port.");
    }

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new ParentProcessIdArgumentProcessorCapabilities();
        Assert.AreEqual("/ParentProcessId", capabilities.CommandName);
        var expected = "--ParentProcessId|/ParentProcessId:<ParentProcessId>\r\n      Process Id of the Parent Process responsible for launching current process.";
        Assert.AreEqual(expected.NormalizeLineEndings().ShowWhiteSpace(), capabilities.HelpContentResourceName.NormalizeLineEndings().ShowWhiteSpace());

        Assert.AreEqual(HelpContentPriority.ParentProcessIdArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.DesignMode, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    [TestMethod]
    public void ExecutorInitializeWithNullOrEmptyParentProcessIdShouldThrowCommandLineException()
    {
        var executor = new ParentProcessIdArgumentExecutor(CommandLineOptions.Instance);
        try
        {
            executor.Initialize(null);
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex is CommandLineException);
            Assert.AreEqual("The --ParentProcessId|/ParentProcessId argument requires the process id which is an integer. Specify the process id of the parent process that launched this process.", ex.Message);
        }
    }

    [TestMethod]
    public void ExecutorInitializeWithInvalidParentProcessIdShouldThrowCommandLineException()
    {
        var executor = new ParentProcessIdArgumentExecutor(CommandLineOptions.Instance);
        try
        {
            executor.Initialize("Foo");
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex is CommandLineException);
            Assert.AreEqual("The --ParentProcessId|/ParentProcessId argument requires the process id which is an integer. Specify the process id of the parent process that launched this process.", ex.Message);
        }
    }

    [TestMethod]
    public void ExecutorInitializeWithValidPortShouldAddParentProcessIdToCommandLineOptions()
    {
        var executor = new ParentProcessIdArgumentExecutor(CommandLineOptions.Instance);
        int parentProcessId = 2345;
        executor.Initialize(parentProcessId.ToString(CultureInfo.InvariantCulture));
        Assert.AreEqual(parentProcessId, CommandLineOptions.Instance.ParentProcessId);
    }

    [TestMethod]
    public void ExecutorExecuteReturnsArgumentProcessorResultSuccess()
    {
        var executor = new ParentProcessIdArgumentExecutor(CommandLineOptions.Instance);

        int parentProcessId = 2345;
        executor.Initialize(parentProcessId.ToString(CultureInfo.InvariantCulture));
        var result = executor.Execute();

        Assert.AreEqual(ArgumentProcessorResult.Success, result);
    }
}
