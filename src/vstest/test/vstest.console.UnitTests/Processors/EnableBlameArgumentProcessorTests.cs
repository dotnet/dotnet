// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CommandLine;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace vstest.console.UnitTests.Processors;

[TestClass]
public class EnableBlameArgumentProcessorTests
{
    private readonly Mock<IEnvironment> _mockEnvronment;
    private readonly Mock<IOutput> _mockOutput;
    private readonly TestableRunSettingsProvider _settingsProvider;
    private readonly EnableBlameArgumentExecutor _executor;
    private readonly string _defaultRunSettings = string.Join(Environment.NewLine,
        "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
        "<RunSettings>",
        "  <DataCollectionRunSettings>",
        "    <DataCollectors ></DataCollectors>",
        "  </DataCollectionRunSettings>",
        "  <RunConfiguration><ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory></RunConfiguration>",
        "  </RunSettings>");

    public EnableBlameArgumentProcessorTests()
    {
        _settingsProvider = new TestableRunSettingsProvider();
        _mockEnvronment = new Mock<IEnvironment>();
        _mockOutput = new Mock<IOutput>();

        _executor = new TestableEnableBlameArgumentExecutor(_settingsProvider, _mockEnvronment.Object, _mockOutput.Object);
        CollectArgumentExecutor.EnabledDataCollectors.Clear();
    }

    [TestMethod]
    public void GetMetadataShouldReturnEnableBlameArgumentProcessorCapabilities()
    {
        var processor = new EnableBlameArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is EnableBlameArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnEnableBlameArgumentProcessorCapabilities()
    {
        var processor = new EnableBlameArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is EnableBlameArgumentExecutor);
    }

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new EnableBlameArgumentProcessorCapabilities();

        Assert.AreEqual("/Blame", capabilities.CommandName);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Logging, capabilities.Priority);
        Assert.AreEqual(HelpContentPriority.EnableDiagArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.AreEqual(CommandLineResources.EnableBlameUsage, capabilities.HelpContentResourceName);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    [TestMethod]
    public void InitializeShouldCreateEntryForBlameInRunSettingsIfNotAlreadyPresent()
    {
        _ = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _executor.Initialize("");

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine,
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
            "<RunSettings>",
            "  <DataCollectionRunSettings>",
            "    <DataCollectors>",
            "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
            "        <Configuration>",
            "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
            "        </Configuration>",
            "      </DataCollector>",
            "    </DataCollectors>",
            "  </DataCollectionRunSettings>",
            "  <RunConfiguration>",
            "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
            "  </RunConfiguration>",
            "  <LoggerRunSettings>",
            "    <Loggers>",
            "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
            "    </Loggers>",
            "  </LoggerRunSettings>",
            "</RunSettings>"
        ), _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldOverwriteEntryForBlameInRunSettingsIfAlreadyPresent()
    {
        _ = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(string.Join(Environment.NewLine,
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
            "<RunSettings>",
            "  <DataCollectionRunSettings>",
            "    <DataCollectors>",
            "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
            "        <Configuration>",
            "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
            "          <CollectDump DumpType=\"mini\" CollectAlways=\"false\" />",
            "        </Configuration>",
            "      </DataCollector>",
            "    </DataCollectors>",
            "  </DataCollectionRunSettings>",
            "  <RunConfiguration>",
            "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
            "  </RunConfiguration>",
            "  <LoggerRunSettings>",
            "    <Loggers>",
            "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
            "    </Loggers>",
            "  </LoggerRunSettings>",
            "</RunSettings>"));
        _settingsProvider.SetActiveRunSettings(runsettings);

        _executor.Initialize("CollectDump;DumpType=full;CollectAlways=true");

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors>",
                "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
                "        <Configuration>",
                "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "          <CollectDump DumpType=\"full\" CollectAlways=\"true\" />",
                "        </Configuration>",
                "      </DataCollector>",
                "    </DataCollectors>",
                "  </DataCollectionRunSettings>",
                "  <RunConfiguration>",
                "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "  </RunConfiguration>",
                "  <LoggerRunSettings>",
                "    <Loggers>",
                "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
                "    </Loggers>",
                "  </LoggerRunSettings>",
                "</RunSettings>"),
            _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldWarnIfIncorrectParameterIsSpecifiedForCollectDumpOption()
    {
        var invalidParameter = "CollectDumpXX";
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _mockEnvronment.Setup(x => x.OperatingSystem)
            .Returns(PlatformOperatingSystem.Windows);
        _mockEnvronment.Setup(x => x.Architecture)
            .Returns(PlatformArchitecture.X64);

        _executor.Initialize(invalidParameter);
        _mockOutput.Verify(x => x.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.BlameIncorrectOption, invalidParameter), OutputLevel.Warning));

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors>",
                "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
                "        <Configuration>",
                "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "        </Configuration>",
                "      </DataCollector>",
                "    </DataCollectors>",
                "  </DataCollectionRunSettings>",
                "  <RunConfiguration>",
                "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "  </RunConfiguration>",
                "  <LoggerRunSettings>",
                "    <Loggers>",
                "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
                "    </Loggers>",
                "  </LoggerRunSettings>",
                "</RunSettings>"),
            _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    [ExpectedException(typeof(CommandLineException))]
    public void InitializeShouldThrowIfInvalidParameterFormatIsSpecifiedForCollectDumpOption()
    {
        var invalidString = "CollectDump;sdf=sdg;;as;a=";
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _mockEnvronment.Setup(x => x.OperatingSystem)
            .Returns(PlatformOperatingSystem.Windows);
        _mockEnvronment.Setup(x => x.Architecture)
            .Returns(PlatformArchitecture.X64);

        _executor.Initialize(invalidString);
        _mockOutput.Verify(x => x.WriteLine(string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidBlameArgument, invalidString), OutputLevel.Warning));

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors>",
                "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
                "        <Configuration>",
                "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "        </Configuration>",
                "      </DataCollector>",
                "    </DataCollectors>",
                "  </DataCollectionRunSettings>",
                "  <RunConfiguration>",
                "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "  </RunConfiguration>",
                "  <LoggerRunSettings>",
                "    <Loggers>",
                "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
                "    </Loggers>",
                "  </LoggerRunSettings>",
                "</RunSettings>"),
            _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldCreateEntryForBlameAlongWithCollectDumpEntryIfEnabled()
    {
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _mockEnvronment.Setup(x => x.OperatingSystem)
            .Returns(PlatformOperatingSystem.Windows);
        _mockEnvronment.Setup(x => x.Architecture)
            .Returns(PlatformArchitecture.X64);

        _executor.Initialize("CollectDump");

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors>",
                "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
                "        <Configuration>",
                "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "          <CollectDump DumpType=\"Full\" />",
                "        </Configuration>",
                "      </DataCollector>",
                "    </DataCollectors>",
                "  </DataCollectionRunSettings>",
                "  <RunConfiguration>",
                "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "  </RunConfiguration>",
                "  <LoggerRunSettings>",
                "    <Loggers>",
                "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
                "    </Loggers>",
                "  </LoggerRunSettings>",
                "</RunSettings>"),
            _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldCreateEntryForBlameAlongWithCollectDumpParametersIfEnabled()
    {
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _mockEnvronment.Setup(x => x.OperatingSystem)
            .Returns(PlatformOperatingSystem.Windows);
        _mockEnvronment.Setup(x => x.Architecture)
            .Returns(PlatformArchitecture.X64);

        _executor.Initialize("CollectDump;DumpType=full;CollectAlways=true");

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors>",
                "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
                "        <Configuration>",
                "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "          <CollectDump DumpType=\"full\" CollectAlways=\"true\" />",
                "        </Configuration>",
                "      </DataCollector>",
                "    </DataCollectors>",
                "  </DataCollectionRunSettings>",
                "  <RunConfiguration>",
                "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "  </RunConfiguration>",
                "  <LoggerRunSettings>",
                "    <Loggers>",
                "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
                "    </Loggers>",
                "  </LoggerRunSettings>",
                "</RunSettings>"),
            _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldCreateEntryForBlameAlongWithCollectHangDumpEntryIfEnabled()
    {
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _mockEnvronment.Setup(x => x.OperatingSystem)
            .Returns(PlatformOperatingSystem.Windows);
        _mockEnvronment.Setup(x => x.Architecture)
            .Returns(PlatformArchitecture.X64);

        _executor.Initialize("CollectHangDump");

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(
            string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors>",
                "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
                "        <Configuration>",
                "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "          <CollectDumpOnTestSessionHang TestTimeout=\"3600000\" HangDumpType=\"Full\" />",
                "        </Configuration>",
                "      </DataCollector>",
                "    </DataCollectors>",
                "  </DataCollectionRunSettings>",
                "  <RunConfiguration>",
                "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "  </RunConfiguration>",
                "  <LoggerRunSettings>",
                "    <Loggers>",
                "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
                "    </Loggers>",
                "  </LoggerRunSettings>",
                "</RunSettings>"),
            _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldCreateEntryForBlameAlongWithCollectHangDumpParametersIfEnabled()
    {
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _mockEnvronment.Setup(x => x.OperatingSystem)
            .Returns(PlatformOperatingSystem.Windows);
        _mockEnvronment.Setup(x => x.Architecture)
            .Returns(PlatformArchitecture.X64);

        _executor.Initialize("CollectHangDump;TestTimeout=10min;HangDumpType=Mini");

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors>",
                "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
                "        <Configuration>",
                "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "          <CollectDumpOnTestSessionHang TestTimeout=\"10min\" HangDumpType=\"Mini\" />",
                "        </Configuration>",
                "      </DataCollector>",
                "    </DataCollectors>",
                "  </DataCollectionRunSettings>",
                "  <RunConfiguration>",
                "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "  </RunConfiguration>",
                "  <LoggerRunSettings>",
                "    <Loggers>",
                "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
                "    </Loggers>",
                "  </LoggerRunSettings>",
                "</RunSettings>"),
            _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    public void InitializeMonitorPostmortemDebuggerShouldGenerateCorrectConfiguration()
    {
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _mockEnvronment.Setup(x => x.OperatingSystem)
            .Returns(PlatformOperatingSystem.Windows);
        _mockEnvronment.Setup(x => x.Architecture)
            .Returns(PlatformArchitecture.X64);

        _executor.Initialize("MonitorPostmortemDebugger;DumpDirectoryPath=c:\\DumpDirectoryPath");
        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors>",
                "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
                "        <Configuration>",
                "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "          <MonitorPostmortemDebugger DumpDirectoryPath=\"c:\\DumpDirectoryPath\" />",
                "        </Configuration>",
                "      </DataCollector>",
                "    </DataCollectors>",
                "  </DataCollectionRunSettings>",
                "  <RunConfiguration>",
                "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "  </RunConfiguration>",
                "  <LoggerRunSettings>",
                "    <Loggers>",
                "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
                "    </Loggers>",
                "  </LoggerRunSettings>",
                "</RunSettings>"),
            _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    public void InitializeMonitorPostmortemDebuggerShouldGenerateCorrectConfigurationAlsoIfIncomplete()
    {
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _mockEnvronment.Setup(x => x.OperatingSystem)
            .Returns(PlatformOperatingSystem.Windows);
        _mockEnvronment.Setup(x => x.Architecture)
            .Returns(PlatformArchitecture.X64);

        _executor.Initialize("MonitorPostmortemDebugger;");
        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine,
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors>",
                "      <DataCollector friendlyName=\"blame\" enabled=\"True\">",
                "        <Configuration>",
                "          <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "          <MonitorPostmortemDebugger DumpDirectoryPath=\"\" />",
                "        </Configuration>",
                "      </DataCollector>",
                "    </DataCollectors>",
                "  </DataCollectionRunSettings>",
                "  <RunConfiguration>",
                "    <ResultsDirectory>C:\\dir\\TestResults</ResultsDirectory>",
                "  </RunConfiguration>",
                "  <LoggerRunSettings>",
                "    <Loggers>",
                "      <Logger friendlyName=\"blame\" enabled=\"True\" />",
                "    </Loggers>",
                "  </LoggerRunSettings>",
                "</RunSettings>"),
            _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    internal class TestableEnableBlameArgumentExecutor : EnableBlameArgumentExecutor
    {
        internal TestableEnableBlameArgumentExecutor(IRunSettingsProvider runSettingsManager, IEnvironment environment, IOutput output)
            : base(runSettingsManager, environment, new Mock<IFileHelper>().Object)
        {
            Output = output;
        }
    }
}
