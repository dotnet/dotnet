// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using vstest.console.Internal;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.CommandLine;

[TestClass]
public class CommandLineOptionsTests
{
    private readonly CommandLineOptions _commandLineOptions = new();
    private readonly Mock<IFileHelper> _fileHelper;
    private readonly FilePatternParser _filePatternParser;
    private readonly string _currentDirectory = @"C:\\Temp";

    public CommandLineOptionsTests()
    {
        _fileHelper = new Mock<IFileHelper>();
        _filePatternParser = new FilePatternParser(new Mock<Matcher>().Object, _fileHelper.Object);
        _commandLineOptions.FileHelper = _fileHelper.Object;
        _commandLineOptions.FilePatternParser = _filePatternParser;
        _fileHelper.Setup(fh => fh.GetCurrentDirectory()).Returns(_currentDirectory);
    }

    [TestMethod]
    public void CommandLineOptionsDefaultBatchSizeIsTen()
    {
        Assert.AreEqual(10, _commandLineOptions.BatchSize);
    }

    [TestMethod]
    public void CommandLineOptionsDiscoveryDefaultBatchSizeIsThousand()
    {
#pragma warning disable MSTEST0025 // Use 'Assert.Fail' instead of an always-failing assert
        Assert.AreEqual(1000, CommandLineOptions.DefaultDiscoveryBatchSize);
#pragma warning restore MSTEST0025 // Use 'Assert.Fail' instead of an always-failing assert
    }

    [TestMethod]
    public void CommandLineOptionsDefaultTestRunStatsEventTimeoutIsOnePointFiveSec()
    {
        var timeout = new TimeSpan(0, 0, 0, 1, 500);
        Assert.AreEqual(timeout, _commandLineOptions.TestStatsEventTimeout);
    }

    [TestMethod]
    public void CommandLineOptionsGetForSourcesPropertyShouldReturnReadonlySourcesEnumerable()
    {
        Assert.IsTrue(_commandLineOptions.Sources is ReadOnlyCollection<string>);
    }

    [TestMethod]
    public void CommandLineOptionsGetForHasPhoneContextPropertyIfTargetDeviceIsSetReturnsTrue()
    {
        Assert.IsFalse(_commandLineOptions.HasPhoneContext);

        // Set some not null value
        _commandLineOptions.TargetDevice = "TargetDevice";
        Assert.IsTrue(_commandLineOptions.HasPhoneContext);
    }

    [TestMethod]
    public void CommandLineOptionsAddSourceShouldThrowCommandLineExceptionForNullSource()
    {
        Assert.ThrowsExactly<TestSourceException>(() => _commandLineOptions.AddSource(null!));
    }

    [TestMethod]
    public void CommandLineOptionsAddSourceShouldConvertRelativePathToAbsolutePath()
    {
        string relativeTestFilePath = "DummyTestFile.txt";
        var absolutePath = Path.Combine(_currentDirectory, relativeTestFilePath);
        _fileHelper.Setup(fh => fh.Exists(absolutePath)).Returns(true);

        // Pass relative path
        _commandLineOptions.AddSource(relativeTestFilePath);
        Assert.IsTrue(_commandLineOptions.Sources.Contains(absolutePath));
    }

    [TestMethod]
    public void CommandLineOptionsAddSourceShouldThrowCommandLineExceptionForInvalidSource()
    {
        Assert.ThrowsExactly<TestSourceException>(() => _commandLineOptions.AddSource("DummySource"));
    }

    [TestMethod]
    public void CommandLineOptionsAddSourceShouldAddSourceForValidSource()
    {
        string testFilePath = Path.Combine(Path.GetTempPath(), "DummyTestFile.txt");
        _fileHelper.Setup(fh => fh.Exists(testFilePath)).Returns(true);

        _commandLineOptions.AddSource(testFilePath);

        Assert.IsTrue(_commandLineOptions.Sources.Contains(testFilePath));
    }
}
