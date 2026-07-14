// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommandLineUtilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.CommandLine;

[TestClass]
public class GenerateFakesUtilitiesTests
{
    private readonly CommandLineOptions _commandLineOptions = new();
    private readonly Mock<IFileHelper> _fileHelper;
    private readonly string _currentDirectory = @"C:\\Temp";
    private readonly string _runSettings = string.Empty;

    public GenerateFakesUtilitiesTests()
    {
        _fileHelper = new Mock<IFileHelper>();
        _commandLineOptions.FileHelper = _fileHelper.Object;
        _fileHelper.Setup(fh => fh.GetCurrentDirectory()).Returns(_currentDirectory);
        _runSettings = @"<RunSettings><RunConfiguration><TargetFrameworkVersion>.netstandard,Version=5.0</TargetFrameworkVersion></RunConfiguration ></RunSettings>";
    }

    [TestMethod]
    public void CommandLineOptionsDefaultDisableAutoFakesIsFalse()
    {
        Assert.IsFalse(_commandLineOptions.DisableAutoFakes);
    }

    [TestMethod]
    public void FakesShouldNotBeGeneratedIfDisableAutoFakesSetToTrue()
    {
        _commandLineOptions.DisableAutoFakes = true;
        string runSettingsXml = @"<RunSettings><RunConfiguration><TargetFrameworkVersion>.netstandard,Version=5.0</TargetFrameworkVersion></RunConfiguration ></RunSettings>";

        runSettingsXml = GenerateFakesUtilities.GenerateFakesSettings(_commandLineOptions, [], runSettingsXml);
        Assert.AreEqual(runSettingsXml, _runSettings);
    }

}
