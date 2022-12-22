// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class EnableLoggersArgumentProcessorTests
{
    [TestInitialize]
    public void Initialize()
    {
        RunTestsArgumentProcessorTests.SetupMockExtensions();
    }

    [TestMethod]
    public void GetMetadataShouldReturnEnableLoggerArgumentProcessorCapabilities()
    {
        EnableLoggerArgumentProcessor processor = new();
        Assert.IsTrue(processor.Metadata.Value is EnableLoggerArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnEnableLoggerArgumentExecutor()
    {
        EnableLoggerArgumentProcessor processor = new();
        Assert.IsTrue(processor.Executor!.Value is EnableLoggerArgumentExecutor);
    }

    [TestMethod]
    public void CapabilitiesShouldAppropriateProperties()
    {
        EnableLoggerArgumentProcessorCapabilities capabilities = new();
        Assert.AreEqual("/Logger", capabilities.CommandName);
#if NETFRAMEWORK
        Assert.AreEqual("--logger|/logger:<Logger Uri/FriendlyName>" + Environment.NewLine + "      Specify a logger for test results. For example, to log results into a " + Environment.NewLine + "      Visual Studio Test Results File (TRX) use /logger:trx[;LogFileName=<Defaults to unique file name>]" + Environment.NewLine + "      Creates file in TestResults directory with given LogFileName." + Environment.NewLine + "" + Environment.NewLine + "      Change the verbosity level in log messages for console logger as shown below" + Environment.NewLine + "      Example: /logger:console;verbosity=<Defaults to \"normal\">" + Environment.NewLine + "      Allowed values for verbosity: quiet, minimal, normal and detailed." + Environment.NewLine + "" + Environment.NewLine + "      Change the diagnostic level prefix for console logger as shown below" + Environment.NewLine + "      Example: /logger:console;prefix=<Defaults to \"false\">" + Environment.NewLine + "      More info on Console Logger here : https://aka.ms/console-logger", capabilities.HelpContentResourceName);
#else
        Assert.AreEqual("--logger|/logger:<Logger Uri/FriendlyName>" + Environment.NewLine + "      Specify a logger for test results. For example, to log results into a " + Environment.NewLine + "      Visual Studio Test Results File (TRX) use /logger:trx[;LogFileName=<Defaults to unique file name>]" + Environment.NewLine + "      Creates file in TestResults directory with given LogFileName." + Environment.NewLine + "" + Environment.NewLine + "      Change the verbosity level in log messages for console logger as shown below" + Environment.NewLine + "      Example: /logger:console;verbosity=<Defaults to \"minimal\">" + Environment.NewLine + "      Allowed values for verbosity: quiet, minimal, normal and detailed." + Environment.NewLine + "" + Environment.NewLine + "      Change the diagnostic level prefix for console logger as shown below" + Environment.NewLine + "      Example: /logger:console;prefix=<Defaults to \"false\">" + Environment.NewLine + "      More info on Console Logger here : https://aka.ms/console-logger", capabilities.HelpContentResourceName);
#endif

        Assert.AreEqual(HelpContentPriority.EnableLoggerArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Logging, capabilities.Priority);

        Assert.IsTrue(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    [TestMethod]
    [DataRow("  ")]
    [DataRow(null)]
    [DataRow("TestLoggerExtension;==;;;Collection=http://localhost:8080/tfs/DefaultCollection;TeamProject=MyProject;BuildName=DailyBuild_20121130.1")]
    public void ExectorInitializeShouldThrowExceptionIfInvalidArgumentIsPassed(string argument)
    {
        var executor = new EnableLoggerArgumentExecutor(RunSettingsManager.Instance);
        try
        {
            executor.Initialize(argument);
        }
        catch (Exception e)
        {
            string exceptionMessage = string.Format(CultureInfo.CurrentCulture, CommandLineResources.LoggerUriInvalid, argument);
            Assert.IsTrue(e.GetType().Equals(typeof(CommandLineException)));
            Assert.IsTrue(e.Message.Contains(exceptionMessage));
        }
    }

    [TestMethod]
    public void ExecutorExecuteShouldReturnArgumentProcessorResultSuccess()
    {
        var executor = new EnableLoggerArgumentExecutor(RunSettingsManager.Instance);
        var result = executor.Execute();
        Assert.AreEqual(ArgumentProcessorResult.Success, result);
    }

    [TestMethod]
    public void ExecutorInitializeShouldAddLoggerWithFriendlyNameInRunSettingsIfNamePresentInArg()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <DataCollectionRunSettings>
                    <DataCollectors>
                      <DataCollector friendlyName=""Code Coverage"">
                      </DataCollector>
                    </DataCollectors>
                  </DataCollectionRunSettings>
                </RunSettings>";

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(settingsXml);
        RunSettingsManager.Instance.SetActiveRunSettings(runSettings);

        var executor = new EnableLoggerArgumentExecutor(RunSettingsManager.Instance);
        executor.Initialize("DummyLoggerExtension");

        string expectedSettingsXml =
            @"<?xml version=""1.0"" encoding=""utf-16""?>
<RunSettings>
  <RunConfiguration>
  </RunConfiguration>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName=""Code Coverage"">
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
  <LoggerRunSettings>
    <Loggers>
      <Logger friendlyName=""DummyLoggerExtension"" enabled=""True"" />
    </Loggers>
  </LoggerRunSettings>
</RunSettings>";

        Assert.AreEqual(expectedSettingsXml, RunSettingsManager.Instance.ActiveRunSettings?.SettingsXml);
    }

    [TestMethod]
    public void ExecutorInitializeShouldAddLoggerWithUriInRunSettingsIfUriPresentInArg()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <DataCollectionRunSettings>
                    <DataCollectors>
                      <DataCollector friendlyName=""Code Coverage"">
                      </DataCollector>
                    </DataCollectors>
                  </DataCollectionRunSettings>
                </RunSettings>";

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(settingsXml);
        RunSettingsManager.Instance.SetActiveRunSettings(runSettings);

        var executor = new EnableLoggerArgumentExecutor(RunSettingsManager.Instance);
        executor.Initialize("logger://DummyLoggerUri");

        string expectedSettingsXml =
            @"<?xml version=""1.0"" encoding=""utf-16""?>
<RunSettings>
  <RunConfiguration>
  </RunConfiguration>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName=""Code Coverage"">
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
  <LoggerRunSettings>
    <Loggers>
      <Logger uri=""logger://dummyloggeruri/"" enabled=""True"" />
    </Loggers>
  </LoggerRunSettings>
</RunSettings>";

        Assert.AreEqual(expectedSettingsXml, RunSettingsManager.Instance.ActiveRunSettings?.SettingsXml);
    }

    [TestMethod]
    public void ExecutorInitializeShouldCorrectlyAddLoggerParametersInRunSettings()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <DataCollectionRunSettings>
                    <DataCollectors>
                      <DataCollector friendlyName=""Code Coverage"">
                      </DataCollector>
                    </DataCollectors>
                  </DataCollectionRunSettings>
                </RunSettings>";

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(settingsXml);
        RunSettingsManager.Instance.SetActiveRunSettings(runSettings);

        var executor = new EnableLoggerArgumentExecutor(RunSettingsManager.Instance);
        executor.Initialize("logger://DummyLoggerUri;Collection=http://localhost:8080/tfs/DefaultCollection;TeamProject=MyProject;BuildName=DailyBuild_20121130.1");

        string expectedSettingsXml =
            @"<?xml version=""1.0"" encoding=""utf-16""?>
<RunSettings>
  <RunConfiguration>
  </RunConfiguration>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName=""Code Coverage"">
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
  <LoggerRunSettings>
    <Loggers>
      <Logger uri=""logger://dummyloggeruri/"" enabled=""True"">
        <Configuration>
          <Collection>http://localhost:8080/tfs/DefaultCollection</Collection>
          <TeamProject>MyProject</TeamProject>
          <BuildName>DailyBuild_20121130.1</BuildName>
        </Configuration>
      </Logger>
    </Loggers>
  </LoggerRunSettings>
</RunSettings>";

        Assert.AreEqual(expectedSettingsXml, RunSettingsManager.Instance.ActiveRunSettings?.SettingsXml);
    }

    [TestMethod]
    public void ExecutorInitializeShouldCorrectlyAddLoggerWhenRunSettingsNotPassed()
    {
        RunSettingsManager.Instance.SetActiveRunSettings(new RunSettings());

        var executor = new EnableLoggerArgumentExecutor(RunSettingsManager.Instance);
        executor.Initialize("logger://DummyLoggerUri;Collection=http://localhost:8080/tfs/DefaultCollection;TeamProject=MyProject;BuildName=DailyBuild_20121130.1");

        string expectedSettingsXml =
            @"
  <LoggerRunSettings>
    <Loggers>
      <Logger uri=""logger://dummyloggeruri/"" enabled=""True"">
        <Configuration>
          <Collection>http://localhost:8080/tfs/DefaultCollection</Collection>
          <TeamProject>MyProject</TeamProject>
          <BuildName>DailyBuild_20121130.1</BuildName>
        </Configuration>
      </Logger>
    </Loggers>
  </LoggerRunSettings>";
        Assert.IsTrue(RunSettingsManager.Instance.ActiveRunSettings!.SettingsXml!.Contains(expectedSettingsXml));
    }

    [TestMethod]
    public void ExecutorInitializeShouldCorrectlyAddLoggerInRunSettingsWhenOtherLoggersAlreadyPresent()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <DataCollectionRunSettings>
                    <DataCollectors>
                      <DataCollector friendlyName=""Code Coverage"">
                      </DataCollector>
                    </DataCollectors>
                  </DataCollectionRunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger uri=""logger://dummyloggeruriTemp/"" enabled=""False"">
                        <Configuration>
                          <Collection1>http://localhost:8080/tfs/DefaultCollection1</Collection1>
                          <TeamProject>MyProject</TeamProject>
                          <BuildName1>DailyBuild_20121130.11</BuildName1>
                        </Configuration>
                      </Logger>
                      <Logger friendlyName=""tempLogger1"" />
                      <Logger friendlyName=""tempLogger2"" enabled=""False"">
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(settingsXml);
        RunSettingsManager.Instance.SetActiveRunSettings(runSettings);

        var executor = new EnableLoggerArgumentExecutor(RunSettingsManager.Instance);
        executor.Initialize("logger://DummyLoggerUri;Collection=http://localhost:8080/tfs/DefaultCollection;TeamProject=MyProject;BuildName=DailyBuild_20121130.1");

        string expectedSettingsXml =
            @"<?xml version=""1.0"" encoding=""utf-16""?>
<RunSettings>
  <RunConfiguration>
  </RunConfiguration>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName=""Code Coverage"">
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
  <LoggerRunSettings>
    <Loggers>
      <Logger uri=""logger://dummyloggeruritemp/"" enabled=""False"">
        <Configuration>
          <Collection1>http://localhost:8080/tfs/DefaultCollection1</Collection1>
          <TeamProject>MyProject</TeamProject>
          <BuildName1>DailyBuild_20121130.11</BuildName1>
        </Configuration>
      </Logger>
      <Logger friendlyName=""tempLogger1"" enabled=""True"" />
      <Logger friendlyName=""tempLogger2"" enabled=""False"" />
      <Logger uri=""logger://dummyloggeruri/"" enabled=""True"">
        <Configuration>
          <Collection>http://localhost:8080/tfs/DefaultCollection</Collection>
          <TeamProject>MyProject</TeamProject>
          <BuildName>DailyBuild_20121130.1</BuildName>
        </Configuration>
      </Logger>
    </Loggers>
  </LoggerRunSettings>
</RunSettings>";

        Assert.AreEqual(expectedSettingsXml, RunSettingsManager.Instance.ActiveRunSettings?.SettingsXml);
    }

    [TestMethod]
    public void ExecutorInitializeShouldPreferCommandLineLoggerOverRunSettingsLoggerIfSame()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <DataCollectionRunSettings>
                    <DataCollectors>
                      <DataCollector friendlyName=""Code Coverage"">
                      </DataCollector>
                    </DataCollectors>
                  </DataCollectionRunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger uri=""logger://dummyloggeruriTemp/"" enabled=""False"">
                        <Configuration>
                          <Collection1>http://localhost:8080/tfs/DefaultCollection1</Collection1>
                          <TeamProject>MyProject</TeamProject>
                          <BuildName1>DailyBuild_20121130.11</BuildName1>
                        </Configuration>
                      </Logger>
                      <Logger friendlyName=""tempLogger1"" />
                      <Logger friendlyName=""tempLogger2"" enabled=""False"">
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(settingsXml);
        RunSettingsManager.Instance.SetActiveRunSettings(runSettings);

        var executor = new EnableLoggerArgumentExecutor(RunSettingsManager.Instance);
        executor.Initialize("tempLogger2");

        string expectedSettingsXml =
            @"<?xml version=""1.0"" encoding=""utf-16""?>
<RunSettings>
  <RunConfiguration>
  </RunConfiguration>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName=""Code Coverage"">
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
  <LoggerRunSettings>
    <Loggers>
      <Logger uri=""logger://dummyloggeruritemp/"" enabled=""False"">
        <Configuration>
          <Collection1>http://localhost:8080/tfs/DefaultCollection1</Collection1>
          <TeamProject>MyProject</TeamProject>
          <BuildName1>DailyBuild_20121130.11</BuildName1>
        </Configuration>
      </Logger>
      <Logger friendlyName=""tempLogger1"" enabled=""True"" />
      <Logger friendlyName=""tempLogger2"" enabled=""True"" />
    </Loggers>
  </LoggerRunSettings>
</RunSettings>";

        Assert.AreEqual(expectedSettingsXml, RunSettingsManager.Instance.ActiveRunSettings?.SettingsXml);
    }

    [TestMethod]
    public void ExecutorInitializeShouldPreferCommandLineLoggerOverRunSettingsLoggerEvenIfCaseMismatch()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <DataCollectionRunSettings>
                    <DataCollectors>
                      <DataCollector friendlyName=""Code Coverage"">
                      </DataCollector>
                    </DataCollectors>
                  </DataCollectionRunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger uri=""logger://dummyloggeruriTemp/"" enabled=""False"">
                        <Configuration>
                          <Collection1>http://localhost:8080/tfs/DefaultCollection1</Collection1>
                          <TeamProject>MyProject</TeamProject>
                          <BuildName1>DailyBuild_20121130.11</BuildName1>
                        </Configuration>
                      </Logger>
                      <Logger friendlyName=""tempLogger1"" />
                      <Logger FRiendlyName=""tEMPLogger2"" enabled=""FaLse"">
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(settingsXml);
        RunSettingsManager.Instance.SetActiveRunSettings(runSettings);

        var executor = new EnableLoggerArgumentExecutor(RunSettingsManager.Instance);
        executor.Initialize("tempLoggER2");

        string expectedSettingsXml =
            @"<?xml version=""1.0"" encoding=""utf-16""?>
<RunSettings>
  <RunConfiguration>
  </RunConfiguration>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName=""Code Coverage"">
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
  <LoggerRunSettings>
    <Loggers>
      <Logger uri=""logger://dummyloggeruritemp/"" enabled=""False"">
        <Configuration>
          <Collection1>http://localhost:8080/tfs/DefaultCollection1</Collection1>
          <TeamProject>MyProject</TeamProject>
          <BuildName1>DailyBuild_20121130.11</BuildName1>
        </Configuration>
      </Logger>
      <Logger friendlyName=""tempLogger1"" enabled=""True"" />
      <Logger friendlyName=""tempLoggER2"" enabled=""True"" />
    </Loggers>
  </LoggerRunSettings>
</RunSettings>";

        Assert.AreEqual(expectedSettingsXml, RunSettingsManager.Instance.ActiveRunSettings?.SettingsXml);
    }

    [TestMethod]
    public void ExecutorInitializeShouldPreferCommandLineLoggerWithParamsOverRunSettingsLoggerIfSame()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <DataCollectionRunSettings>
                    <DataCollectors>
                      <DataCollector friendlyName=""Code Coverage"">
                      </DataCollector>
                    </DataCollectors>
                  </DataCollectionRunSettings>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger uri=""logger://dummyloggeruriTemp/"" enabled=""False"">
                        <Configuration>
                          <Collection1>http://localhost:8080/tfs/DefaultCollection1</Collection1>
                          <TeamProject>MyProject</TeamProject>
                          <BuildName1>DailyBuild_20121130.11</BuildName1>
                        </Configuration>
                      </Logger>
                      <Logger friendlyName=""tempLogger1"" />
                      <Logger friendlyName=""tempLogger2"" enabled=""False"">
                      </Logger>
                      <Logger uri=""logger://dummyloggeruri/"" enabled=""True"">
                        <Configuration>
                          <Collection>http://localhost:8080/tfs/DefaultCollection</Collection>
                          <TeamProject>MyProject</TeamProject>
                          <BuildName>DailyBuild_20121130.1</BuildName>
                          <NewAttr>value</NewAttr>
                        </Configuration>
                      </Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(settingsXml);
        RunSettingsManager.Instance.SetActiveRunSettings(runSettings);

        var executor = new EnableLoggerArgumentExecutor(RunSettingsManager.Instance);
        executor.Initialize("logger://DummyLoggerUri;Collection=http://localhost:8080/tfs/DefaultCollectionOverride;TeamProjectOverride=MyProject;BuildName=DailyBuild_20121130.1Override;NewAttr=value");

        string expectedSettingsXml =
            @"<?xml version=""1.0"" encoding=""utf-16""?>
<RunSettings>
  <RunConfiguration>
  </RunConfiguration>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName=""Code Coverage"">
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
  <LoggerRunSettings>
    <Loggers>
      <Logger uri=""logger://dummyloggeruritemp/"" enabled=""False"">
        <Configuration>
          <Collection1>http://localhost:8080/tfs/DefaultCollection1</Collection1>
          <TeamProject>MyProject</TeamProject>
          <BuildName1>DailyBuild_20121130.11</BuildName1>
        </Configuration>
      </Logger>
      <Logger friendlyName=""tempLogger1"" enabled=""True"" />
      <Logger friendlyName=""tempLogger2"" enabled=""False"" />
      <Logger uri=""logger://dummyloggeruri/"" enabled=""True"">
        <Configuration>
          <Collection>http://localhost:8080/tfs/DefaultCollectionOverride</Collection>
          <TeamProjectOverride>MyProject</TeamProjectOverride>
          <BuildName>DailyBuild_20121130.1Override</BuildName>
          <NewAttr>value</NewAttr>
        </Configuration>
      </Logger>
    </Loggers>
  </LoggerRunSettings>
</RunSettings>";

        Assert.AreEqual(expectedSettingsXml, RunSettingsManager.Instance.ActiveRunSettings?.SettingsXml);
    }
}
