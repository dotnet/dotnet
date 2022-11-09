// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using vstest.console.UnitTests.Processors;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class ResultsDirectoryArgumentProcessorTests
{
    private readonly ResultsDirectoryArgumentExecutor _executor;
    private readonly TestableRunSettingsProvider _runSettingsProvider;

    public ResultsDirectoryArgumentProcessorTests()
    {
        _runSettingsProvider = new TestableRunSettingsProvider();
        _executor = new ResultsDirectoryArgumentExecutor(CommandLineOptions.Instance, _runSettingsProvider);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        CommandLineOptions.Reset();
    }

    [TestMethod]
    public void GetMetadataShouldReturnResultsDirectoryArgumentProcessorCapabilities()
    {
        var processor = new ResultsDirectoryArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is ResultsDirectoryArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnResultsDirectoryArgumentExecutor()
    {
        var processor = new ResultsDirectoryArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is ResultsDirectoryArgumentExecutor);
    }

    #region ResultsDirectoryArgumentProcessorCapabilities tests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new ResultsDirectoryArgumentProcessorCapabilities();
        Assert.AreEqual("/ResultsDirectory", capabilities.CommandName);
        var expected = "--ResultsDirectory|/ResultsDirectory\r\n      Test results directory will be created in specified path if not exists.\r\n      Example  /ResultsDirectory:<pathToResultsDirectory>";
        Assert.AreEqual(expected.NormalizeLineEndings().ShowWhiteSpace(), capabilities.HelpContentResourceName.NormalizeLineEndings().ShowWhiteSpace());

        Assert.AreEqual(HelpContentPriority.ResultsDirectoryArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.AutoUpdateRunSettings, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    #region ResultsDirectoryArgumentExecutor Initialize tests

    [TestMethod]
    public void InitializeShouldThrowIfArgumentIsNull()
    {
        string? folder = null;
        var message =
            @"The /ResultsDirectory parameter requires a value, where the test results should be saved. Example:  /ResultsDirectory:c:\MyTestResultsDirectory";
        InitializeExceptionTestTemplate(folder, message);
    }

    [TestMethod]
    public void InitializeShouldThrowIfArgumentIsAWhiteSpace()
    {
        var folder = " ";
        var message =
            @"The /ResultsDirectory parameter requires a value, where the test results should be saved. Example:  /ResultsDirectory:c:\MyTestResultsDirectory";
        InitializeExceptionTestTemplate(folder, message);
    }

    [TestMethod]
    public void InitializeShouldThrowIfGivenPathIsIllegal()
    {
        // the internal code uses IsPathRooted which does not consider this rooted on Linux
        // so we need to convert the path, and use char that is invalid on the current platform
        var invalidChar = Path.GetInvalidPathChars()[0];

        var folder = TranslatePath($@"c:\som{invalidChar}\illegal\path\");
        // The error varies based on the runtime and OS, just checking that we detect
        // incorrect path should be enough and not so flaky
        // you might get
        // - The filename, directory name, or volume label syntax is incorrect
        // - Illegal characters in path.
        // - etc.

        var message = $"The path '{folder}' specified in the 'ResultsDirectory' is invalid. Error:";

        InitializeExceptionTestTemplate(folder, message);
    }

    private void InitializeExceptionTestTemplate(string? folder, string message)
    {
        var isExceptionThrown = false;

        try
        {
            _executor.Initialize(folder);
        }
        catch (Exception ex)
        {
            isExceptionThrown = true;
            Assert.IsTrue(ex is CommandLineException, "ex is CommandLineException");
            StringAssert.StartsWith(ex.Message, message);
        }

        Assert.IsTrue(isExceptionThrown, "isExceptionThrown");
    }

    [TestMethod]
    public void InitializeShouldSetCommandLineOptionsAndRunSettingsForRelativePathValue()
    {
        var relativePath = TranslatePath(@".\relative\path");
        var absolutePath = Path.GetFullPath(relativePath);
        _executor.Initialize(relativePath);
        Assert.AreEqual(absolutePath, CommandLineOptions.Instance.ResultsDirectory);
        Assert.AreEqual(absolutePath, _runSettingsProvider.QueryRunSettingsNode(ResultsDirectoryArgumentExecutor.RunSettingsPath));
    }

    [TestMethod]
    public void InitializeShouldSetCommandLineOptionsAndRunSettingsForAbsolutePathValue()
    {
        var absolutePath = TranslatePath(@"c:\random\someone\testresults");
        _executor.Initialize(absolutePath);
        Assert.AreEqual(absolutePath, CommandLineOptions.Instance.ResultsDirectory);
        Assert.AreEqual(absolutePath, _runSettingsProvider.QueryRunSettingsNode(ResultsDirectoryArgumentExecutor.RunSettingsPath));
    }

    #endregion

    #region ResultsDirectoryArgumentExecutor Execute tests

    [TestMethod]
    public void ExecuteShouldReturnSuccess()
    {
        Assert.AreEqual(ArgumentProcessorResult.Success, _executor.Execute());
    }

    #endregion

    private static string TranslatePath(string path)
    {
        // RuntimeInformation has conflict when used
        if (Environment.OSVersion.Platform.ToString().StartsWith("Win"))
            return path;

        var prefix = Path.GetTempPath();

        return Regex.Replace(path.Replace("\\", "/"), @"(\w)\:/", $@"{prefix}$1/");
    }
}
