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
    private readonly Mock<IFileHelper> _fileHelper;
    private readonly FilePatternParser _filePatternParser;
    private readonly string _currentDirectory = @"C:\\Temp";

    public CommandLineOptionsTests()
    {
        _fileHelper = new Mock<IFileHelper>();
        _filePatternParser = new FilePatternParser(new Mock<Matcher>().Object, _fileHelper.Object);
        CommandLineOptions.Reset();
        CommandLineOptions.Instance.FileHelper = _fileHelper.Object;
        CommandLineOptions.Instance.FilePatternParser = _filePatternParser;
        _fileHelper.Setup(fh => fh.GetCurrentDirectory()).Returns(_currentDirectory);
    }

    [TestMethod]
    public void CommandLineOptionsDefaultBatchSizeIsTen()
    {
        Assert.AreEqual(10, CommandLineOptions.Instance.BatchSize);
    }

    [TestMethod]
    public void CommandLineOptionsDiscoveryDefaultBatchSizeIsThousand()
    {
        Assert.AreEqual(1000, CommandLineOptions.DefaultDiscoveryBatchSize);
    }

    [TestMethod]
    public void CommandLineOptionsDefaultTestRunStatsEventTimeoutIsOnePointFiveSec()
    {
        var timeout = new TimeSpan(0, 0, 0, 1, 500);
        Assert.AreEqual(timeout, CommandLineOptions.Instance.TestStatsEventTimeout);
    }

    [TestMethod]
    public void CommandLineOptionsGetForSourcesPropertyShouldReturnReadonlySourcesEnumerable()
    {
        Assert.IsTrue(CommandLineOptions.Instance.Sources is ReadOnlyCollection<string>);
    }

    [TestMethod]
    public void CommandLineOptionsGetForHasPhoneContextPropertyIfTargetDeviceIsSetReturnsTrue()
    {
        Assert.IsFalse(CommandLineOptions.Instance.HasPhoneContext);

        // Set some not null value
        CommandLineOptions.Instance.TargetDevice = "TargetDevice";
        Assert.IsTrue(CommandLineOptions.Instance.HasPhoneContext);
    }

    [TestMethod]
    public void CommandLineOptionsAddSourceShouldThrowCommandLineExceptionForNullSource()
    {
        Assert.ThrowsException<TestSourceException>(() => CommandLineOptions.Instance.AddSource(null!));
    }

    [TestMethod]
    public void CommandLineOptionsAddSourceShouldConvertRelativePathToAbsolutePath()
    {
        string relativeTestFilePath = "DummyTestFile.txt";
        var absolutePath = Path.Combine(_currentDirectory, relativeTestFilePath);
        _fileHelper.Setup(fh => fh.Exists(absolutePath)).Returns(true);

        // Pass relative path
        CommandLineOptions.Instance.AddSource(relativeTestFilePath);
        Assert.IsTrue(CommandLineOptions.Instance.Sources.Contains(absolutePath));
    }

    [TestMethod]
    public void CommandLineOptionsAddSourceShouldThrowCommandLineExceptionForInvalidSource()
    {
        Assert.ThrowsException<TestSourceException>(() => CommandLineOptions.Instance.AddSource("DummySource"));
    }

    [TestMethod]
    public void CommandLineOptionsAddSourceShouldAddSourceForValidSource()
    {
        string testFilePath = Path.Combine(Path.GetTempPath(), "DummyTestFile.txt");
        _fileHelper.Setup(fh => fh.Exists(testFilePath)).Returns(true);

        CommandLineOptions.Instance.AddSource(testFilePath);

        Assert.IsTrue(CommandLineOptions.Instance.Sources.Contains(testFilePath));
    }
}
