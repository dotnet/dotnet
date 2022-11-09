// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class HelpArgumentProcessorTests
{
    /// <summary>
    /// The help argument processor get metadata should return help argument processor capabilities.
    /// </summary>
    [TestMethod]
    public void GetMetadataShouldReturnHelpArgumentProcessorCapabilities()
    {
        HelpArgumentProcessor processor = new();
        Assert.IsTrue(processor.Metadata.Value is HelpArgumentProcessorCapabilities);
    }

    /// <summary>
    /// The help argument processor get executer should return help argument processor capabilities.
    /// </summary>
    [TestMethod]
    public void GetExecuterShouldReturnHelpArgumentProcessorCapabilities()
    {
        HelpArgumentProcessor processor = new();
        Assert.IsTrue(processor.Executor!.Value is HelpArgumentExecutor);
    }

    #region HelpArgumentProcessorCapabilitiesTests

    [TestMethod]
    public void CapabilitiesShouldAppropriateProperties()
    {
        HelpArgumentProcessorCapabilities capabilities = new();
        Assert.AreEqual("/Help", capabilities.CommandName);
        Assert.AreEqual("-?|--Help|/?|/Help" + Environment.NewLine + "      Display this usage message.", capabilities.HelpContentResourceName);

        Assert.AreEqual(HelpContentPriority.HelpArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Help, capabilities.Priority);

        Assert.IsTrue(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    [TestMethod]
    public void ExecuterExecuteReturnArgumentProcessorResultAbort()
    {
        HelpArgumentExecutor executor = new();
        var result = executor.Execute();
        Assert.AreEqual(ArgumentProcessorResult.Abort, result);
    }

    [TestMethod]
    public void ExecuterExecuteWritesAppropriateDataToConsole()
    {
        HelpArgumentExecutor executor = new();
        var output = new DummyConsoleOutput();
        executor.Output = output;
        _ = executor.Execute();
        Assert.IsTrue(output.Lines.Contains("Usage: vstest.console.exe [Arguments] [Options] [[--] <RunSettings arguments>...]]"));
        Assert.IsTrue(output.Lines.Contains("Arguments:"));
        Assert.IsTrue(output.Lines.Contains("Options:"));
        Assert.IsTrue(output.Lines.Contains("Description: Runs tests from the specified files."));
        Assert.IsTrue(output.Lines.Contains("  To run tests:" + Environment.NewLine + "    >vstest.console.exe tests.dll " + Environment.NewLine + "  To run tests with additional settings such as  data collectors:" + Environment.NewLine + "    >vstest.console.exe  tests.dll /Settings:Local.RunSettings"));
    }
}

internal class DummyConsoleOutput : IOutput
{
    /// <summary>
    /// The lines.
    /// </summary>
    internal List<string?> Lines;

    public DummyConsoleOutput()
    {
        Lines = new List<string?>();
    }

    public void WriteLine(string? message, OutputLevel level)
    {
        Lines.Add(message);
    }

    public void Write(string? message, OutputLevel level)
    {
        throw new NotImplementedException();
    }
}
