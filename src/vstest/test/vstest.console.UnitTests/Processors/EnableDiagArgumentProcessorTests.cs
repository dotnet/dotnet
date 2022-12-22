// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class EnableDiagArgumentProcessorTests
{
    private readonly string _dummyFilePath = Path.Combine(Path.DirectorySeparatorChar.ToString(), $"{Guid.NewGuid()}", $"{Guid.NewGuid()}.txt");

    private readonly EnableDiagArgumentProcessor _diagProcessor;

    private readonly Mock<IFileHelper> _mockFileHelper;

    private readonly TraceLevel _traceLevel;
    private readonly string? _traceFileName;

    public EnableDiagArgumentProcessorTests()
    {
        _mockFileHelper = new Mock<IFileHelper>();
        _diagProcessor = new TestableEnableDiagArgumentProcessor(_mockFileHelper.Object);

        // Saving the EqtTrace state
#if NETFRAMEWORK
        _traceLevel = EqtTrace.TraceLevel;
        EqtTrace.TraceLevel = TraceLevel.Off;
#else
        _traceLevel = (TraceLevel)EqtTrace.TraceLevel;
        EqtTrace.TraceLevel = (PlatformTraceLevel)TraceLevel.Off;
#endif

        _traceFileName = EqtTrace.LogFile;
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Restoring to initial state for EqtTrace
        EqtTrace.InitializeTrace(_traceFileName, PlatformTraceLevel.Verbose);
#if NETFRAMEWORK
        EqtTrace.TraceLevel = _traceLevel;
#else
        EqtTrace.TraceLevel = (PlatformTraceLevel)_traceLevel;
#endif
    }

    [TestMethod]
    public void EnableDiagArgumentProcessorMetadataShouldProvideAppropriateCapabilities()
    {
        Assert.IsFalse(_diagProcessor.Metadata.Value.AllowMultiple);
        Assert.IsFalse(_diagProcessor.Metadata.Value.AlwaysExecute);
        Assert.IsFalse(_diagProcessor.Metadata.Value.IsAction);
        Assert.IsFalse(_diagProcessor.Metadata.Value.IsSpecialCommand);
        Assert.AreEqual(EnableDiagArgumentProcessor.CommandName, _diagProcessor.Metadata.Value.CommandName);
        Assert.IsNull(_diagProcessor.Metadata.Value.ShortCommandName);
        Assert.AreEqual(ArgumentProcessorPriority.Diag, _diagProcessor.Metadata.Value.Priority);
        Assert.AreEqual(HelpContentPriority.EnableDiagArgumentProcessorHelpPriority, _diagProcessor.Metadata.Value.HelpPriority);
        Assert.AreEqual(CommandLineResources.EnableDiagUsage, _diagProcessor.Metadata.Value.HelpContentResourceName);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("  ")]
    [DataRow("")]
    public void EnableDiagArgumentProcessorExecutorThrowsIfDiagArgumentIsNullOrEmpty(string argument)
    {
        string exceptionMessage = string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidDiagArgument, argument);
        EnableDiagArgumentProcessorExecutorShouldThrowIfInvalidArgument(argument, exceptionMessage);
    }

    [TestMethod]
    public void EnableDiagArgumentProcessorExecutorDoesNotThrowsIfFileDotOpenThrow()
    {
        _mockFileHelper.Setup(fh => fh.DirectoryExists(Path.GetDirectoryName(_dummyFilePath)!)).Returns(true);

        _diagProcessor.Executor!.Value.Initialize(_dummyFilePath);
    }

    [TestMethod]
    [DataRow("abc.txt;verbosity=normal=verbose")] // Multiple '=' in parameter
    [DataRow("abc.txt;verbosity;key1=value1")] // No '=' in parameter
    [DataRow("\"abst;dfsdc.txt\";verbosity=normal")] // Too many parameters
    [DataRow("abs;dfsdc.txt;verbosity=normal")] // Too many parameters
    public void EnableDiagArgumentProcessorExecutorShouldThrowIfInvalidArgument(string argument)
    {
        string exceptionMessage = string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidDiagArgument, argument);
        EnableDiagArgumentProcessorExecutorShouldThrowIfInvalidArgument(argument, exceptionMessage);
    }

    [TestMethod]
    [DataRow("abc.txt")]
    [DataRow("abc.txt;verbosity=normal")]
    [DataRow("abc.txt;tracelevel=info;newkey=newvalue")]
    [DataRow("\"abc.txt\";verbosity=normal;newkey=newvalue")] //escaped double quotes are allowed for file path.
    [DataRow(";;abc.txt;;;;verbosity=normal;;;;")]
    // Files with no extension are totally valid files.
    // When we delegate to host or datacollector we change the name in GetTimestampedLogFile
    // Path.ChangeExtension replaces the curent extension with our new one that is timestamp and
    // the name of the target (e.g. host). When there is no extension it just adds it, so we do either:
    // log.txt -> log.host.21-09-10_12-25-41_68765_5.txt
    // log.my.txt -> log.my.host.21-09-10_12-25-50_55183_5.txt
    // log -> log.host.21-09-10_12-25-27_94286_5
    [DataRow("log")]
    [DataRow("log.log")]
    public void EnableDiagArgumentProcessorExecutorShouldNotThrowIfValidArgument(string argument)
    {
        try
        {
            _diagProcessor.Executor!.Value.Initialize(argument);
        }
        catch (Exception ex)
        {
            Assert.Fail("Expected no exception, but got: " + ex.Message);
        }
    }

    [TestMethod]
    [DataRow("abc.txt;tracelevel=info;newkey=newvalue")]
    [DataRow("abc.txt;tracelevel=info;")]
    [DataRow("abc.txt;tracelevel=INfO")]
    [DataRow("abc.txt;traCELevel=info")]
    [DataRow("abc.txt;traCELevel=INfO")]
    public void EnableDiagArgumentProcessorExecutorShouldInitializeTraceWithCorrectTraceLevel(string argument)
    {
        // Setting any trace level  other than info.
#if NETFRAMEWORK
        EqtTrace.TraceLevel = TraceLevel.Verbose;
#else
        EqtTrace.TraceLevel = PlatformTraceLevel.Verbose;
#endif

        _diagProcessor.Executor!.Value.Initialize(argument);

        Assert.AreEqual(TraceLevel.Info, (TraceLevel)EqtTrace.TraceLevel);
        Assert.IsTrue(EqtTrace.LogFile?.Contains("abc.txt"));
    }

    [TestMethod]
    public void EnableDiagArgumentProcessorExecutorShouldCreateDirectoryOfLogFileIfNotExists()
    {
        _mockFileHelper.Setup(fh => fh.DirectoryExists(Path.GetDirectoryName(_dummyFilePath)!)).Returns(false);

        _diagProcessor.Executor!.Value.Initialize(_dummyFilePath);

        _mockFileHelper.Verify(fh => fh.CreateDirectory(Path.GetDirectoryName(_dummyFilePath)!), Times.Once);
    }

    [TestMethod]
    public void EnableDiagArgumentProcessorExecutorShouldNotCreateDirectoryIfAFileIsProvided()
    {
        _diagProcessor.Executor!.Value.Initialize("log.txt");

        _mockFileHelper.Verify(fh => fh.CreateDirectory(It.IsAny<string>()), Times.Never);
    }

    private class TestableEnableDiagArgumentProcessor : EnableDiagArgumentProcessor
    {
        /// <inheritdoc/>
        public TestableEnableDiagArgumentProcessor(IFileHelper fileHelper)
            : base(fileHelper)
        {
        }
    }

    private void EnableDiagArgumentProcessorExecutorShouldThrowIfInvalidArgument(string argument, string exceptionMessage)
    {
        var e = Assert.ThrowsException<CommandLineException>(() => _diagProcessor.Executor!.Value.Initialize(argument));
        StringAssert.Contains(e.Message, exceptionMessage);
    }
}
