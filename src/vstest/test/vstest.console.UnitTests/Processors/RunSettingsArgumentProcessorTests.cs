// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using vstest.console.UnitTests.Processors;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class RunSettingsArgumentProcessorTests
{
    private readonly TestableRunSettingsProvider _settingsProvider;

    public RunSettingsArgumentProcessorTests()
    {
        _settingsProvider = new TestableRunSettingsProvider();
    }

    [TestCleanup]
    public void TestCleanup()
    {
        CommandLineOptions.Reset();
    }

    [TestMethod]
    public void GetMetadataShouldReturnRunSettingsArgumentProcessorCapabilities()
    {
        var processor = new RunSettingsArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is RunSettingsArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnRunSettingsArgumentExecutor()
    {
        var processor = new RunSettingsArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is RunSettingsArgumentExecutor);
    }

    #region RunSettingsArgumentProcessorCapabilities tests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new RunSettingsArgumentProcessorCapabilities();
        Assert.AreEqual("/Settings", capabilities.CommandName);
        var expected = "--Settings|/Settings:<Settings File>\r\n      Settings to use when running tests.";
        Assert.AreEqual(expected.NormalizeLineEndings().ShowWhiteSpace(), capabilities.HelpContentResourceName.NormalizeLineEndings().ShowWhiteSpace());

        Assert.AreEqual(HelpContentPriority.RunSettingsArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.RunSettings, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    #region RunSettingsArgumentExecutor tests

    [TestMethod]
    public void InitializeShouldThrowExceptionIfArgumentIsNull()
    {
        Action action = () => new RunSettingsArgumentExecutor(CommandLineOptions.Instance, null!).Initialize(null);

        ExceptionUtilities.ThrowsException<CommandLineException>(
            action,
            "The /Settings parameter requires a settings file to be provided.");
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfArgumentIsWhiteSpace()
    {
        Action action = () => new RunSettingsArgumentExecutor(CommandLineOptions.Instance, null!).Initialize("  ");

        ExceptionUtilities.ThrowsException<CommandLineException>(
            action,
            "The /Settings parameter requires a settings file to be provided.");
    }

    [TestMethod]
    public void InitializeShouldThrowExceptionIfFileDoesNotExist()
    {
        var fileName = "C:\\Imaginary\\nonExistentFile.txt";

        var executor = new RunSettingsArgumentExecutor(CommandLineOptions.Instance, null!);
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(false);

        executor.FileHelper = mockFileHelper.Object;

        ExceptionUtilities.ThrowsException<CommandLineException>(
            () => executor.Initialize(fileName),
            "The Settings file '{0}' could not be found.",
            fileName);
    }

    [TestMethod]
    public void InitializeShouldThrowIfRunSettingsSchemaDoesNotMatch()
    {
        // Arrange.
        var fileName = "C:\\temp\\r.runsettings";
        var settingsXml = "<BadRunSettings></BadRunSettings>";

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            settingsXml);

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);

        executor.FileHelper = mockFileHelper.Object;

        // Act and Assert.
        ExceptionUtilities.ThrowsException<SettingsException>(
            () => executor.Initialize(fileName),
            "Settings file provided does not conform to required format.");
    }

    [TestMethod]
    public void InitializeShouldSetActiveRunSettings()
    {
        // Arrange.
        var fileName = "C:\\temp\\r.runsettings";
        var settingsXml = "<RunSettings></RunSettings>";

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            settingsXml);

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        executor.FileHelper = mockFileHelper.Object;

        // Act.
        executor.Initialize(fileName);

        // Assert.
        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(fileName, CommandLineOptions.Instance.SettingsFile);
    }

    [TestMethod]
    public void InitializeShouldSetSettingsFileForCommandLineOptions()
    {
        // Arrange.
        var fileName = "C:\\temp\\r.runsettings";
        var settingsXml = "<RunSettings></RunSettings>";

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            settingsXml);

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        executor.FileHelper = mockFileHelper.Object;

        // Act.
        executor.Initialize(fileName);

        // Assert.
        Assert.AreEqual(fileName, CommandLineOptions.Instance.SettingsFile);
    }

    [TestMethod]
    public void InitializeShouldAddDefaultSettingsIfNotPresent()
    {
        // Arrange.
        var fileName = "C:\\temp\\r.runsettings";
        var settingsXml = "<RunSettings></RunSettings>";

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            settingsXml);

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        executor.FileHelper = mockFileHelper.Object;

        // Act.
        executor.Initialize(fileName);

        // Assert.
        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        RunConfiguration runConfiguration =
            XmlRunSettingsUtilities.GetRunConfigurationNode(_settingsProvider.ActiveRunSettings.SettingsXml);
        Assert.AreEqual(runConfiguration.ResultsDirectory, Constants.DefaultResultsDirectory);
        Assert.AreEqual(runConfiguration.TargetFramework!.ToString(), Framework.DefaultFramework.ToString());
        Assert.AreEqual(runConfiguration.TargetPlatform, Constants.DefaultPlatform);

    }

    [TestMethod]
    public void InitializeShouldSetActiveRunSettingsForTestSettingsFiles()
    {
        // Arrange.
        var fileName = "C:\\temp\\r.testsettings";
        var settingsXml = "<TestSettings></TestSettings>";

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            settingsXml);

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        executor.FileHelper = mockFileHelper.Object;

        // Act.
        executor.Initialize(fileName);


        // Assert.
        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);

        var expected = string.Join(Environment.NewLine,
            $"<?xml version=\"1.0\" encoding=\"utf-16\"?>",
            $"<RunSettings>",
            $"  <RunConfiguration>",
            $"    <ResultsDirectory>{Constants.DefaultResultsDirectory}</ResultsDirectory>",
            $"    <TargetPlatform>{Constants.DefaultPlatform}</TargetPlatform>",
            $"    <TargetFrameworkVersion>{Framework.DefaultFramework.Name}</TargetFrameworkVersion>",
            $"  </RunConfiguration>",
            $"  <MSTest>",
            $"    <SettingsFile>C:\\temp\\r.testsettings</SettingsFile>",
            $"    <ForcedLegacyMode>true</ForcedLegacyMode>",
            $"  </MSTest>",
            $"  <DataCollectionRunSettings>",
            $"    <DataCollectors />",
            $"  </DataCollectionRunSettings>",
            $"</RunSettings>");
        StringAssert.Contains(_settingsProvider.ActiveRunSettings.SettingsXml, expected);
    }


    [TestMethod]
    public void InitializeShouldUpdateCommandLineOptionsArchitectureAndFxIfProvided()
    {
        // Arrange.
        var fileName = "C:\\temp\\r.runsettings";
        var settingsXml = $"<RunSettings><RunConfiguration><TargetPlatform>{nameof(Architecture.X64)}</TargetPlatform><TargetFrameworkVersion>{Constants.DotNetFramework46}</TargetFrameworkVersion></RunConfiguration></RunSettings>";

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            settingsXml);

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        executor.FileHelper = mockFileHelper.Object;

        // Act.
        executor.Initialize(fileName);

        // Assert.
        Assert.IsTrue(CommandLineOptions.Instance.ArchitectureSpecified);
        Assert.IsTrue(CommandLineOptions.Instance.FrameworkVersionSpecified);
        Assert.AreEqual(Architecture.X64, CommandLineOptions.Instance.TargetArchitecture);
        Assert.AreEqual(Constants.DotNetFramework46, CommandLineOptions.Instance.TargetFrameworkVersion.Name);
    }

    [TestMethod]
    public void InitializeShouldNotUpdateCommandLineOptionsArchitectureAndFxIfNotProvided()
    {
        // Arrange.
        var fileName = "C:\\temp\\r.runsettings";
        var settingsXml = "<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            settingsXml);

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        executor.FileHelper = mockFileHelper.Object;

        // Act.
        executor.Initialize(fileName);

        // Assert.
        Assert.IsFalse(CommandLineOptions.Instance.ArchitectureSpecified);
        Assert.IsFalse(CommandLineOptions.Instance.FrameworkVersionSpecified);
    }

    [TestMethod]
    public void InitializeShouldPreserveActualJapaneseString()
    {
        var runsettingsFile = Path.Combine(Path.GetTempPath(), "InitializeShouldPreserveActualJapaneseString.runsettings");
        var settingsXml = @"<RunSettings><RunConfiguration><ResultsDirectory>C:\新しいフォルダー</ResultsDirectory></RunConfiguration></RunSettings>";

        File.WriteAllText(runsettingsFile, settingsXml, Encoding.UTF8);

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            null);

        executor.Initialize(runsettingsFile);
        Assert.IsTrue(_settingsProvider.ActiveRunSettings!.SettingsXml!.Contains(@"C:\新しいフォルダー"));
        File.Delete(runsettingsFile);
    }

    [TestMethod]
    public void InitializeShouldSetInIsolataionToTrueIfEnvironmentVariablesSpecified()
    {
        var settingsXml = @"<RunSettings><RunConfiguration><EnvironmentVariables><RANDOM_PATH>C:\temp</RANDOM_PATH></EnvironmentVariables></RunConfiguration></RunSettings>";

        // Arrange.
        var fileName = "C:\\temp\\r.runsettings";

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            settingsXml);

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        executor.FileHelper = mockFileHelper.Object;

        // Act.
        executor.Initialize(fileName);

        // Assert.
        Assert.IsTrue(CommandLineOptions.Instance.InIsolation);
        Assert.AreEqual("true", _settingsProvider.QueryRunSettingsNode(InIsolationArgumentExecutor.RunSettingsPath));
    }

    [TestMethod]
    public void InitializeShouldNotSetInIsolataionToTrueIfEnvironmentVariablesNotSpecified()
    {
        var settingsXml = @"<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";

        /// Arrange.
        var fileName = "C:\\temp\\r.runsettings";

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            settingsXml);

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        executor.FileHelper = mockFileHelper.Object;

        // Act.
        executor.Initialize(fileName);

        // Assert.
        Assert.IsFalse(CommandLineOptions.Instance.InIsolation);
        Assert.IsNull(_settingsProvider.QueryRunSettingsNode(InIsolationArgumentExecutor.RunSettingsPath));
    }

    [TestMethod]
    public void InitializeShouldUpdateTestCaseFilterIfProvided()
    {
        // Arrange.
        var fileName = "C:\\temp\\r.runsettings";
        var filter = "TestCategory=Included";
        var settingsXml = $"<RunSettings><RunConfiguration><TestCaseFilter>{filter}</TestCaseFilter></RunConfiguration></RunSettings>";

        var executor = new TestableRunSettingsArgumentExecutor(
            CommandLineOptions.Instance,
            _settingsProvider,
            settingsXml);

        // Setup mocks.
        var mockFileHelper = new Mock<IFileHelper>();
        mockFileHelper.Setup(fh => fh.Exists(It.IsAny<string>())).Returns(true);
        executor.FileHelper = mockFileHelper.Object;

        // Act.
        executor.Initialize(fileName);

        // Assert.
        Assert.AreEqual(filter, CommandLineOptions.Instance.TestCaseFilterValue);
    }
    #endregion

    #region Testable Implementations

    private class TestableRunSettingsArgumentExecutor : RunSettingsArgumentExecutor
    {
        private readonly string? _runSettingsString;

        internal TestableRunSettingsArgumentExecutor(
            CommandLineOptions commandLineOptions,
            IRunSettingsProvider runSettingsManager,
            string? runSettings)
            : base(commandLineOptions, runSettingsManager)

        {
            _runSettingsString = runSettings;
        }

        protected override XmlReader GetReaderForFile(string runSettingsFile)
        {
            if (_runSettingsString == null)
            {
                return base.GetReaderForFile(runSettingsFile);
            }

            var reader = new StringReader(_runSettingsString);
            var xmlReader = XmlReader.Create(reader, XmlRunSettingsUtilities.ReaderSettings);

            return xmlReader;
        }
    }

    #endregion
}
