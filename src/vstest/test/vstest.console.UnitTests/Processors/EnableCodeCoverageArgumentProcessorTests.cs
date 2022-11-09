// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CommandLine;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace vstest.console.UnitTests.Processors;

[TestClass]
public class EnableCodeCoverageArgumentProcessorTests
{
    private readonly TestableRunSettingsProvider _settingsProvider;
    private readonly EnableCodeCoverageArgumentExecutor _executor;

    private readonly string _defaultRunSettings = string.Join(Environment.NewLine,
        "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
        "<RunSettings>",
        "  <DataCollectionRunSettings>",
        "    <DataCollectors >{0}</DataCollectors>",
        "  </DataCollectionRunSettings>",
        "</RunSettings>");

    public EnableCodeCoverageArgumentProcessorTests()
    {
        _settingsProvider = new TestableRunSettingsProvider();
        _executor = new EnableCodeCoverageArgumentExecutor(CommandLineOptions.Instance, _settingsProvider,
            new Mock<IFileHelper>().Object);
        CollectArgumentExecutor.EnabledDataCollectors.Clear();
    }

    [TestMethod]
    public void GetMetadataShouldReturnEnableCodeCoverageArgumentProcessorCapabilities()
    {
        var processor = new EnableCodeCoverageArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is EnableCodeCoverageArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnEnableCodeCoverageArgumentProcessorCapabilities()
    {
        var processor = new EnableCodeCoverageArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is EnableCodeCoverageArgumentExecutor);
    }

    #region EnableCodeCoverageArgumentProcessorCapabilities tests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new EnableCodeCoverageArgumentProcessorCapabilities();

        Assert.AreEqual("/EnableCodeCoverage", capabilities.CommandName);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.AutoUpdateRunSettings, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    #region EnableCodeCoverageArgumentExecutor tests

    [TestMethod]
    public void InitializeShouldSetEnableCodeCoverageOfCommandLineOption()
    {
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(runsettingsString);
        _settingsProvider.SetActiveRunSettings(runsettings);

        CommandLineOptions.Instance.EnableCodeCoverage = false;

        _executor.Initialize(string.Empty);

        Assert.IsTrue(CommandLineOptions.Instance.EnableCodeCoverage,
            "/EnableCoverage should set CommandLineOption.EnableCodeCoverage to true");
    }

    [TestMethod]
    public void InitializeShouldCreateEntryForCodeCoverageInRunSettingsIfNotAlreadyPresent()
    {
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings, "");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(runsettingsString);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _executor.Initialize(string.Empty);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        var dataCollectorsFriendlyNames =
            XmlRunSettingsUtilities.GetDataCollectorsFriendlyName(_settingsProvider.ActiveRunSettings
                .SettingsXml!);
        Assert.IsTrue(dataCollectorsFriendlyNames.Contains("Code Coverage"),
            "Code coverage setting in not available in runsettings");
    }

    [TestMethod]
    public void InitializeShouldEnableCodeCoverageIfDisabledInRunSettings()
    {
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings,
            "<DataCollector friendlyName=\"Code Coverage\" enabled=\"False\" />");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(runsettingsString);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _executor.Initialize(string.Empty);

        Assert.AreEqual(string.Join(Environment.NewLine,
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
            "<RunSettings>",
            "  <DataCollectionRunSettings>",
            "    <DataCollectors>",
            "      <DataCollector friendlyName=\"Code Coverage\" enabled=\"True\" />",
            "    </DataCollectors>",
            "  </DataCollectionRunSettings>",
            "</RunSettings>"), _settingsProvider.ActiveRunSettings!.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldNotDisableOtherDataCollectors()
    {
        CollectArgumentExecutor.EnabledDataCollectors.Add("mydatacollector1");
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings,
            "<DataCollector friendlyName=\"Code Coverage\" enabled=\"False\" /><DataCollector friendlyName=\"MyDataCollector1\" enabled=\"True\" />");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(runsettingsString);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _executor.Initialize(string.Empty);

        Assert.AreEqual(string.Join(Environment.NewLine,
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
            "<RunSettings>",
            "  <DataCollectionRunSettings>",
            "    <DataCollectors>",
            "      <DataCollector friendlyName=\"Code Coverage\" enabled=\"True\" />",
            "      <DataCollector friendlyName=\"MyDataCollector1\" enabled=\"True\" />",
            "    </DataCollectors>",
            "  </DataCollectionRunSettings>",
            "</RunSettings>"), _settingsProvider.ActiveRunSettings!.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldNotEnableOtherDataCollectors()
    {
        var runsettingsString = string.Format(CultureInfo.CurrentCulture, _defaultRunSettings,
            "<DataCollector friendlyName=\"Code Coverage\" enabled=\"False\" /><DataCollector friendlyName=\"MyDataCollector1\" enabled=\"False\" />");
        var runsettings = new RunSettings();
        runsettings.LoadSettingsXml(runsettingsString);
        _settingsProvider.SetActiveRunSettings(runsettings);

        _executor.Initialize(string.Empty);

        Assert.AreEqual(string.Join(Environment.NewLine,
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
            "<RunSettings>",
            "  <DataCollectionRunSettings>",
            "    <DataCollectors>",
            "      <DataCollector friendlyName=\"Code Coverage\" enabled=\"True\" />",
            "      <DataCollector friendlyName=\"MyDataCollector1\" enabled=\"False\" />",
            "    </DataCollectors>",
            "  </DataCollectionRunSettings>",
            "</RunSettings>"), _settingsProvider.ActiveRunSettings!.SettingsXml);
    }

    #endregion
}
