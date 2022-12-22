// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Resources;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.ObjectModel.UnitTests.Utilities;

[TestClass]
public class XmlRunSettingsUtilitiesTests
{
    #region Private Variables

    private readonly string _runSettingsXmlWithDataCollectors = @"<RunSettings>
<RunConfiguration>
</RunConfiguration>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName=""Code Coverage"" uri=""datacollector://Microsoft/CodeCoverage/2.0"" assemblyQualifiedName=""Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceCollector, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
        <Configuration>
          <CodeCoverage>
            <ModulePaths>
              <Exclude>
                <ModulePath>.*CPPUnitTestFramework.*</ModulePath>
              </Exclude>
            </ModulePaths>
          </CodeCoverage>
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>";

    private readonly string _runSettingsXmlWithDataCollectorsDisabled = @"<RunSettings>
<RunConfiguration>
</RunConfiguration>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName=""Code Coverage"" uri=""datacollector://Microsoft/CodeCoverage/2.0"" enabled=""false"" assemblyQualifiedName=""Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceCollector, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
        <Configuration>
          <CodeCoverage>
            <ModulePaths>
              <Exclude>
                <ModulePath>.*CPPUnitTestFramework.*</ModulePath>
              </Exclude>
            </ModulePaths>
          </CodeCoverage>
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>";

    private readonly string _runSettingsXmlWithIncorrectDataCollectorSettings = @"<RunSettings>
<RunConfiguration>
</RunConfiguration>
  <DataCollectionRunSettings>
    <DataCollectors attributethatshouldneverexist=""false"">
      <DataCollector friendlyName=""Code Coverage"" uri=""datacollector://Microsoft/CodeCoverage/2.0"" enabled=""false"" assemblyQualifiedName=""Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceCollector, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
        <Configuration>
          <CodeCoverage>
            <ModulePaths>
              <Exclude>
                <ModulePath>.*CPPUnitTestFramework.*</ModulePath>
              </Exclude>
            </ModulePaths>
          </CodeCoverage>
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>";

    private readonly string _emptyRunSettings = "<RunSettings></RunSettings>";

    #endregion

    #region GetTestRunParameters tests

    [TestMethod]
    public void GetTestRunParametersReturnsEmptyDictionaryOnNullRunSettings()
    {
        Dictionary<string, object> trp = XmlRunSettingsUtilities.GetTestRunParameters(null);
        Assert.IsNotNull(trp);
        Assert.AreEqual(0, trp.Count);
    }

    [TestMethod]
    public void GetTestRunParametersReturnsEmptyDictionaryWhenNoTestRunParameters()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <ResultsDirectory>.\TestResults</ResultsDirectory>
                       <TargetPlatform>x86</TargetPlatform>
                       <TargetFrameworkVersion>Framework40</TargetFrameworkVersion>
                     </RunConfiguration>
                </RunSettings>";

        Dictionary<string, object> trp = XmlRunSettingsUtilities.GetTestRunParameters(settingsXml);
        Assert.IsNotNull(trp);
        Assert.AreEqual(0, trp.Count);
    }

    [TestMethod]
    public void GetTestRunParametersReturnsEmptyDictionaryForEmptyTestRunParametersNode()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <ResultsDirectory>.\TestResults</ResultsDirectory>
                       <TargetPlatform>x86</TargetPlatform>
                       <TargetFrameworkVersion>Framework40</TargetFrameworkVersion>
                     </RunConfiguration>
                     <TestRunParameters>
                     </TestRunParameters>
                </RunSettings>";

        Dictionary<string, object> trp = XmlRunSettingsUtilities.GetTestRunParameters(settingsXml);
        Assert.IsNotNull(trp);
        Assert.AreEqual(0, trp.Count);
    }

    [TestMethod]
    public void GetTestRunParametersReturns1EntryOn1TestRunParameter()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <ResultsDirectory>.\TestResults</ResultsDirectory>
                       <TargetPlatform>x86</TargetPlatform>
                       <TargetFrameworkVersion>Framework40</TargetFrameworkVersion>
                     </RunConfiguration>
                     <TestRunParameters>
                        <Parameter name=""webAppUrl"" value=""http://localhost"" />
                     </TestRunParameters>
                </RunSettings>";

        Dictionary<string, object> trp = XmlRunSettingsUtilities.GetTestRunParameters(settingsXml);
        Assert.IsNotNull(trp);
        Assert.AreEqual(1, trp.Count);

        // Verify Parameter Values.
        Assert.IsTrue(trp.ContainsKey("webAppUrl"));
        Assert.AreEqual("http://localhost", trp["webAppUrl"]);
    }

    [TestMethod]
    public void GetTestRunParametersReturns3EntryOn3TestRunParameter()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <ResultsDirectory>.\TestResults</ResultsDirectory>
                       <TargetPlatform>x86</TargetPlatform>
                       <TargetFrameworkVersion>Framework40</TargetFrameworkVersion>
                     </RunConfiguration>
                     <TestRunParameters>
                        <Parameter name=""webAppUrl"" value=""http://localhost"" />
                        <Parameter name=""webAppUserName"" value=""Admin"" />
                        <Parameter name=""webAppPassword"" value=""Password"" />
                     </TestRunParameters>
                </RunSettings>";

        Dictionary<string, object> trp = XmlRunSettingsUtilities.GetTestRunParameters(settingsXml);
        Assert.IsNotNull(trp);
        Assert.AreEqual(3, trp.Count);

        // Verify Parameter Values.
        Assert.IsTrue(trp.ContainsKey("webAppUrl"));
        Assert.AreEqual("http://localhost", trp["webAppUrl"]);
        Assert.IsTrue(trp.ContainsKey("webAppUserName"));
        Assert.AreEqual("Admin", trp["webAppUserName"]);
        Assert.IsTrue(trp.ContainsKey("webAppPassword"));
        Assert.AreEqual("Password", trp["webAppPassword"]);
    }

    [TestMethod]
    public void GetTestRunParametersThrowsWhenTrpNodeHasAttributes()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <ResultsDirectory>.\TestResults</ResultsDirectory>
                       <TargetPlatform>x86</TargetPlatform>
                       <TargetFrameworkVersion>Framework40</TargetFrameworkVersion>
                     </RunConfiguration>
                     <TestRunParameters count=""1"">
                        <Parameter name=""webAppUrl"" value=""http://localhost"" />
                     </TestRunParameters>
                </RunSettings>";

        Assert.ThrowsException<SettingsException>(() => XmlRunSettingsUtilities.GetTestRunParameters(settingsXml));
    }

    [TestMethod]
    public void GetTestRunParametersThrowsWhenTrpNodeHasNonParameterTypeChildNodes()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <ResultsDirectory>.\TestResults</ResultsDirectory>
                       <TargetPlatform>x86</TargetPlatform>
                       <TargetFrameworkVersion>Framework40</TargetFrameworkVersion>
                     </RunConfiguration>
                     <TestRunParameters>
                        <Parameter name=""webAppUrl"" value=""http://localhost"" />
                        <TargetPlatform>x86</TargetPlatform>
                     </TestRunParameters>
                </RunSettings>";

        Assert.ThrowsException<SettingsException>(() => XmlRunSettingsUtilities.GetTestRunParameters(settingsXml));
    }

    [TestMethod]
    public void GetTestRunParametersIgnoresMalformedKeyValues()
    {
        string settingsXml =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                     <RunConfiguration>
                       <ResultsDirectory>.\TestResults</ResultsDirectory>
                       <TargetPlatform>x86</TargetPlatform>
                       <TargetFrameworkVersion>Framework40</TargetFrameworkVersion>
                     </RunConfiguration>
                     <TestRunParameters>
                        <Parameter name=""webAppUrl"" values=""http://localhost"" />
                     </TestRunParameters>
                </RunSettings>";

        Dictionary<string, object> trp = XmlRunSettingsUtilities.GetTestRunParameters(settingsXml);
        Assert.IsNotNull(trp);
        Assert.AreEqual(0, trp.Count);
    }

    [TestMethod]
    public void GetInProcDataCollectionRunSettingsFromSettings()
    {
        string settingsXml = @"<RunSettings>
                                    <InProcDataCollectionRunSettings>
                                        <InProcDataCollectors>
                                            <InProcDataCollector friendlyName='Test Impact' uri='InProcDataCollector://Microsoft/TestImpact/1.0' assemblyQualifiedName='TestImpactListener.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a'  codebase='E:\repos\MSTest\src\managed\TestPlatform\TestImpactListener.Tests\bin\Debug\TestImpactListener.Tests.dll'>
                                                <Configuration>
                                                    <Port>4312</Port>
                                                </Configuration>
                                            </InProcDataCollector>
                                        </InProcDataCollectors>
                                    </InProcDataCollectionRunSettings>
                                </RunSettings>";
        var inProcDcRunSettings = XmlRunSettingsUtilities.GetInProcDataCollectionRunSettings(settingsXml);
        Assert.IsNotNull(inProcDcRunSettings);
        Assert.AreEqual(1, inProcDcRunSettings.DataCollectorSettingsList.Count);
    }

    [TestMethod]
    public void GetInProcDataCollectionRunSettingsThrowsExceptionWhenXmlNotValid()
    {
        string settingsXml = @"<RunSettings>
                                    <InProcDataCollectionRunSettings>
                                        <InProcDataCollectors>
                                            <InProcDataCollector friendlyNames='Test Impact' uris='InProcDataCollector://Microsoft/TestImpact/1.0' assemblyQualifiedName='TestImpactListener.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7ccb7239ffde675a'  codebase='E:\repos\MSTest\src\managed\TestPlatform\TestImpactListener.Tests\bin\Debug\TestImpactListener.Tests.dll'>
                                                <Configuration>
                                                    <Port>4312</Port>
                                                </Configuration>
                                            </InProcDataCollector>
                                        </InProcDataCollectors>
                                    </InProcDataCollectionRunSettings>
                                </RunSettings>";

        Assert.ThrowsException<SettingsException>(
            () => XmlRunSettingsUtilities.GetInProcDataCollectionRunSettings(settingsXml));
    }
    #endregion

    #region CreateDefaultRunSettings tests

    [TestMethod]
    public void CreateDefaultRunSettingsShouldReturnABasicRunSettings()
    {
        var defaultRunSettings = XmlRunSettingsUtilities.CreateDefaultRunSettings().CreateNavigator()!.OuterXml;
        var expectedRunSettings = string.Join(Environment.NewLine,
            "<RunSettings>",
            "  <DataCollectionRunSettings>",
            "    <DataCollectors />",
            "  </DataCollectionRunSettings>",
            "</RunSettings>"
        );

        Assert.AreEqual(expectedRunSettings, defaultRunSettings);
    }

    #endregion

    #region IsDataCollectionEnabled tests

    [TestMethod]
    public void IsDataCollectionEnabledShouldReturnFalseIfRunSettingsIsNull()
    {
        Assert.IsFalse(XmlRunSettingsUtilities.IsDataCollectionEnabled(null));
    }

    [TestMethod]
    public void IsDataCollectionEnabledShouldReturnFalseIfDataCollectionNodeIsNotPresent()
    {
        Assert.IsFalse(XmlRunSettingsUtilities.IsDataCollectionEnabled(_emptyRunSettings));
    }

    [TestMethod]
    public void IsDataCollectionEnabledShouldReturnFalseIfDataCollectionIsDisabled()
    {
        Assert.IsFalse(XmlRunSettingsUtilities.IsDataCollectionEnabled(_runSettingsXmlWithDataCollectorsDisabled));
    }


    [TestMethod]
    public void IsDataCollectionEnabledShouldReturnTrueIfDataCollectionIsEnabled()
    {
        Assert.IsTrue(XmlRunSettingsUtilities.IsDataCollectionEnabled(_runSettingsXmlWithDataCollectors));
    }

    #endregion

    #region IsInProcDataCollectionEnabled tests.

    [TestMethod]
    public void IsInProcDataCollectionEnabledShouldReturnFalseIfRunSettingsIsNull()
    {
        Assert.IsFalse(XmlRunSettingsUtilities.IsInProcDataCollectionEnabled(null));
    }

    [TestMethod]
    public void IsInProcDataCollectionEnabledShouldReturnFalseIfDataCollectionNodeIsNotPresent()
    {
        Assert.IsFalse(XmlRunSettingsUtilities.IsInProcDataCollectionEnabled(_emptyRunSettings));
    }

    [TestMethod]
    public void IsInProcDataCollectionEnabledShouldReturnFalseIfDataCollectionIsDisabled()
    {
        Assert.IsFalse(XmlRunSettingsUtilities.IsInProcDataCollectionEnabled(ConvertOutOfProcDataCollectionSettingsToInProcDataCollectionSettings(_runSettingsXmlWithDataCollectorsDisabled)));
    }

    [TestMethod]
    public void IsInProcDataCollectionEnabledShouldReturnTrueIfDataCollectionIsEnabled()
    {
        Assert.IsTrue(XmlRunSettingsUtilities.IsInProcDataCollectionEnabled(ConvertOutOfProcDataCollectionSettingsToInProcDataCollectionSettings(_runSettingsXmlWithDataCollectors)));
    }

    #endregion

    #region GetLoggerRunsettings tests

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnNullWhenSettingsIsnull()
    {
        Assert.IsNull(XmlRunSettingsUtilities.GetLoggerRunSettings(null));
    }

    [TestMethod]
    public void GetLoggerRunSettingShouldReturnNullWhenNoLoggersPresent()
    {
        string runSettingsXmlWithNoLoggers =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <DataCollectionRunSettings>
                    <DataCollectors>
                      <DataCollector friendlyName=""Code Coverage"" uri=""datacollector://Microsoft/CodeCoverage/2.0"" assemblyQualifiedName=""Microsoft.VisualStudio.Coverage.DynamicCoverageDataCollector, Microsoft.VisualStudio.TraceCollector, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"">
                        <Configuration>
                          <CodeCoverage>
                            <ModulePaths>
                              <Exclude>
                                <ModulePath>.*CPPUnitTestFramework.*</ModulePath>
                              </Exclude>
                            </ModulePaths>
                          </CodeCoverage>
                        </Configuration>
                      </DataCollector>
                    </DataCollectors>
                  </DataCollectionRunSettings>
                </RunSettings>";

        Assert.IsNull(XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsXmlWithNoLoggers));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldNotReturnNullWhenLoggersPresent()
    {
        string runSettingsWithLoggerHavingFriendlyName =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        Assert.IsNotNull(XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingFriendlyName));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLoggerWithCorrectFriendlyName()
    {
        string runSettingsWithLoggerHavingFriendlyName =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingFriendlyName)!;
        Assert.AreEqual("TestLoggerWithParameterExtension", loggerRunSettings.LoggerSettingsList.First().FriendlyName);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLoggerWithCorrectUri()
    {
        string runSettingsWithLoggerHavingUri =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger uri=""testlogger://logger""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingUri)!;
        Assert.IsTrue(new Uri("testlogger://logger").Equals(loggerRunSettings.LoggerSettingsList.First().Uri));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldThrowWhenInvalidUri()
    {
        string runSettingsWithInvalidUri =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger uri=""invalidUri""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var exceptionMessage = string.Empty;

        try
        {
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithInvalidUri);
        }
        catch (SettingsException ex)
        {
            exceptionMessage = ex.Message;
        }

        Assert.IsTrue(exceptionMessage.Contains(
            string.Format(
                CultureInfo.CurrentCulture,
                Resources.InvalidUriInSettings,
                "invalidUri",
                "Logger")));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLoggerWithCorrectAssemblyQualifiedName()
    {
        string runSettingsWithLoggerHavingAssemblyQualifiedName =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger assemblyQualifiedName=""Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingAssemblyQualifiedName)!;
        Assert.AreEqual("Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx", loggerRunSettings.LoggerSettingsList.First().AssemblyQualifiedName);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLoggerWithCorrectCodeBase()
    {
        string runSettingsWithLoggerHavingAssemblyQualifiedName =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger assemblyQualifiedName=""Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx"" CodeBase=""C:\Sample\Sample.TestLogger.dll""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingAssemblyQualifiedName)!;
        Assert.AreEqual(@"C:\Sample\Sample.TestLogger.dll", loggerRunSettings.LoggerSettingsList.First().CodeBase);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLoggerWithCorrectEnabledAttributeValue()
    {
        string runSettingsWithLoggerHavingEnabledAttribute =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"" enabled=""faLSe""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingEnabledAttribute)!;
        Assert.IsFalse(loggerRunSettings.LoggerSettingsList.First().IsEnabled);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLoggerWithEnabledFalseIfInvalidEnabledValue()
    {
        string runSettingsWithLoggerHavingInvalidEnabledValue =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"" enabled=""invalidValue""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingInvalidEnabledValue)!;
        Assert.IsFalse(loggerRunSettings.LoggerSettingsList.First().IsEnabled);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLoggerAsEnabledWhenEnabledAttributeNotPresent()
    {
        string runSettingsWithLoggerHavingEnabledAttribute =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingEnabledAttribute)!;
        Assert.IsTrue(loggerRunSettings.LoggerSettingsList.First().IsEnabled);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldThrowIfDuplicateAttributesPresent()
    {
        string runSettingsWithLoggerHavingDuplicateAttributes =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"" friendlyName=""TestLogger""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var exceptionMessage = string.Empty;

        try
        {
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingDuplicateAttributes);
        }
        catch (SettingsException ex)
        {
            exceptionMessage = ex.Message;
        }

        Assert.IsTrue(exceptionMessage.Contains(CommonResources.MalformedRunSettingsFile));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLoggerWithCorrectValuesWhenMultipleAttributesPresent()
    {
        string runSettingsWithLoggerHavingMultipleAttributes =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"" uri=""testlogger://logger"" assemblyQualifiedName=""Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx"" coDEbase=""C:\Sample\Sample.TestLogger.dll"" enabled=""faLSe""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingMultipleAttributes)!;
        Assert.AreEqual("TestLoggerWithParameterExtension", loggerRunSettings.LoggerSettingsList.First().FriendlyName);
        Assert.IsTrue(new Uri("testlogger://logger").Equals(loggerRunSettings.LoggerSettingsList.First().Uri));
        Assert.AreEqual("Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx", loggerRunSettings.LoggerSettingsList.First().AssemblyQualifiedName);
        Assert.AreEqual(@"C:\Sample\Sample.TestLogger.dll", loggerRunSettings.LoggerSettingsList.First().CodeBase);
        Assert.IsFalse(loggerRunSettings.LoggerSettingsList.First().IsEnabled);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLoggerWithCorrectValuesWhenCaseSensitivityNotMaintained()
    {
        string runSettingsWithLoggerHavingAttributesWithRandomCasing =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <loGGerRunSettings>
                    <LOGgers>
                      <loGGer FRIEndlyNAME=""TestLoggerWithParameterExtension"" uRI=""testlogger://logger"" AssEMBLYQualifiedName=""Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx"" coDEbase=""C:\Sample\Sample.TestLogger.dll"" enaBLED=""faLSe""></loGGer>
                    </LOGgers>
                  </loGGerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithLoggerHavingAttributesWithRandomCasing)!;
        Assert.AreEqual("TestLoggerWithParameterExtension", loggerRunSettings.LoggerSettingsList.First().FriendlyName);
        Assert.IsTrue(new Uri("testlogger://logger").Equals(loggerRunSettings.LoggerSettingsList.First().Uri));
        Assert.AreEqual("Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx", loggerRunSettings.LoggerSettingsList.First().AssemblyQualifiedName);
        Assert.AreEqual(@"C:\Sample\Sample.TestLogger.dll", loggerRunSettings.LoggerSettingsList.First().CodeBase);
        Assert.IsFalse(loggerRunSettings.LoggerSettingsList.First().IsEnabled);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldThrowShouldThrowOnMalformedLoggerSettings()
    {
        string runSettingsXmlWithMalformedLoggerSettings =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers invalidValue>
                      <Logger friendlyName=""TestLoggerWithParameterExtension""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var exceptionMessage = string.Empty;

        try
        {
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsXmlWithMalformedLoggerSettings);
        }
        catch (SettingsException ex)
        {
            exceptionMessage = ex.Message;
        }

        Assert.IsTrue(exceptionMessage.Contains(CommonResources.MalformedRunSettingsFile));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldThrowWhenAttribtuesPresentInLoggerRunSettingsNode()
    {
        string runSettingsWithAttributesPresentInLoggerRunSettingsNode =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings name=""invalidAttribtueValue"">
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var exceptionMessage = string.Empty;

        try
        {
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithAttributesPresentInLoggerRunSettingsNode);
        }
        catch (SettingsException ex)
        {
            exceptionMessage = ex.Message;
        }

        Assert.IsTrue(exceptionMessage.Contains(string.Format(
            CultureInfo.CurrentCulture,
            Resources.InvalidSettingsXmlAttribute,
            "LoggerRunSettings",
            "name")));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnEmptyLoggerRunSettingsWhenLoggerRunSettingsNodeIsEmpty()
    {
        string runSettingsWithEmptyLoggerRunSettingsNode =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithEmptyLoggerRunSettingsNode)!;
        Assert.AreEqual(0, loggerRunSettings.LoggerSettingsList.Count);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnEmptyLoggerRunSettingsWhenLoggerRunSettingsNodeIsSelfEnding()
    {
        string runSettingsWithEmptyLoggerRunSettingsNode =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings />
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithEmptyLoggerRunSettingsNode)!;
        Assert.AreEqual(0, loggerRunSettings.LoggerSettingsList.Count);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldThrowWhenNodeOtherThanLoggersPresentInLoggerRunSettings()
    {
        string runSettingsWithNodeOtherLoggers =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRUNSettings>
                    <LoggersInvalid>
                      <Logger friendlyName=""TestLoggerWithParameterExtension""></Logger>
                    </LoggersInvalid>
                  </LoggerRUNSettings>
                </RunSettings>";

        var exceptionMessage = string.Empty;

        try
        {
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithNodeOtherLoggers);
        }
        catch (SettingsException ex)
        {
            exceptionMessage = ex.Message;
        }

        Assert.IsTrue(exceptionMessage.Contains(string.Format(
            CultureInfo.CurrentCulture,
            Resources.InvalidSettingsXmlElement,
            "LoggerRUNSettings",
            "LoggersInvalid")));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldThrowWhenAttribtuesPresentInLoggersNode()
    {
        string runSettingsWithAttributesPresentInLoggersNode =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers nameAttr=""invalidAttribtueValue"">
                      <Logger friendlyName=""TestLoggerWithParameterExtension""></Logger>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var exceptionMessage = string.Empty;

        try
        {
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithAttributesPresentInLoggersNode);
        }
        catch (SettingsException ex)
        {
            exceptionMessage = ex.Message;
        }

        Assert.IsTrue(exceptionMessage.Contains(string.Format(
            CultureInfo.CurrentCulture,
            Resources.InvalidSettingsXmlAttribute,
            "Loggers",
            "nameAttr")));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnEmptyLoggersWhenLoggersIsEmpty()
    {
        string runSettingsWithEmptyLoggersNode =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers>
                    </Loggers>
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithEmptyLoggersNode)!;
        Assert.AreEqual(0, loggerRunSettings.LoggerSettingsList.Count);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnEmptyLoggersWhenLoggersIsSelfEnding()
    {
        string runSettingsWithEmptyLoggersNode =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRunSettings>
                    <Loggers />
                  </LoggerRunSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithEmptyLoggersNode)!;
        Assert.AreEqual(0, loggerRunSettings.LoggerSettingsList.Count);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldThrowWhenNodeOtherThanLoggerPresentInLoggers()
    {
        string runSettingsWithNodeOtherLoggers =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRUNSettings>
                    <Loggers>
                      <LoggerInvalid friendlyName=""TestLoggerWithParameterExtension""></LoggerInvalid>
                    </Loggers>
                  </LoggerRUNSettings>
                </RunSettings>";

        var exceptionMessage = string.Empty;

        try
        {
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithNodeOtherLoggers);
        }
        catch (SettingsException ex)
        {
            exceptionMessage = ex.Message;
        }

        Assert.IsTrue(exceptionMessage.Contains(string.Format(
            CultureInfo.CurrentCulture,
            Resources.InvalidSettingsXmlElement,
            "Loggers",
            "LoggerInvalid")));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldThrowWhenRequiredAttributesNotPresentInLoggerNode()
    {
        // One among friendlyName, uri and assemblyQualifiedName should be present.
        string runSettingsWithNoneOfRequiredAttributes =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRUNSettings>
                    <Loggers>
                      <LogGer enabled=""tRUe""></LogGer>
                    </Loggers>
                  </LoggerRUNSettings>
                </RunSettings>";

        var exceptionMessage = string.Empty;

        try
        {
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithNoneOfRequiredAttributes);
        }
        catch (SettingsException ex)
        {
            exceptionMessage = ex.Message;
        }

        Assert.IsTrue(exceptionMessage.Contains(string.Format(CultureInfo.CurrentCulture, Resources.MissingLoggerAttributes, "LogGer")));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnConfigurationElementIfPresentInLoggerNode()
    {
        string runSettingsWithConfigurationElementInLoggerNode =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRUNSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"">
                        <ConfiGUration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </ConfiGUration>
                      </Logger>
                    </Loggers>
                  </LoggerRUNSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithConfigurationElementInLoggerNode)!;

        var expectedConfigurationElement = new XmlDocument().CreateElement("ConfiGUration");
        expectedConfigurationElement.InnerXml = "<Key1>Value1</Key1><Key2>Value2</Key2>";
        Assert.AreEqual(expectedConfigurationElement.Name,
            loggerRunSettings.LoggerSettingsList.First().Configuration!.Name);
        Assert.AreEqual(expectedConfigurationElement.InnerXml,
            loggerRunSettings.LoggerSettingsList.First().Configuration!.InnerXml);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldThrowWhenNodeOtherThanConfigurationPresentInLogger()
    {
        string runSettingsWithInvalidConfigurationElement =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRUNSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"">
                        <ConfiGUrationInvalid>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </ConfiGUrationInvalid>
                      </Logger>
                    </Loggers>
                  </LoggerRUNSettings>
                </RunSettings>";

        var exceptionMessage = string.Empty;

        try
        {
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithInvalidConfigurationElement);
        }
        catch (SettingsException ex)
        {
            exceptionMessage = ex.Message;
        }

        Assert.AreEqual(string.Format(
            CultureInfo.CurrentCulture,
            Resources.InvalidSettingsXmlElement,
            "Logger",
            "ConfiGUrationInvalid"), exceptionMessage);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldThrowOnInvalidAttributeInLoggerNode()
    {
        string runSettingsWithInvalidAttributeInLoggerNode =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRUNSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"" invalidAttr=""blabla"">
                        <ConfiGUration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </ConfiGUration>
                      </Logger>
                    </Loggers>
                  </LoggerRUNSettings>
                </RunSettings>";

        var exceptionMessage = string.Empty;

        try
        {
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithInvalidAttributeInLoggerNode);
        }
        catch (SettingsException ex)
        {
            exceptionMessage = ex.Message;
        }

        Assert.AreEqual(string.Format(
            CultureInfo.CurrentCulture,
            Resources.InvalidSettingsXmlAttribute,
            "Logger",
            "invalidAttr"), exceptionMessage);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnMultipleLoggersIfPresent()
    {
        string runSettingsWithMultipleLoggers =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRUNSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"">
                        <ConfiGUration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </ConfiGUration>
                      </Logger>
                      <Logger friendlyName=""TestLogger"" uri=""testlogger://logger"" assemblyQualifiedName=""Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx"" coDEbase=""C:\Sample\Sample.TestLogger.dll"" enabled=""faLSe""></Logger>
                      <Logger uri=""testlogger://loggerTemp"">
                        <ConfiGUration>
                          <Key3>Value3</Key3>
                          <Key4>Value4</Key4>
                        </ConfiGUration>
                      </Logger>
                    </Loggers>
                  </LoggerRUNSettings>
                </RunSettings>";

        var loggerRunSettings =
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithMultipleLoggers)!;

        Assert.AreEqual(3, loggerRunSettings.LoggerSettingsList.Count);

        // 1st logger
        var loggerFirst = loggerRunSettings.LoggerSettingsList[0];
        Assert.AreEqual("TestLoggerWithParameterExtension", loggerFirst.FriendlyName);
        Assert.IsTrue(string.IsNullOrWhiteSpace(loggerFirst.Uri?.ToString()));
        Assert.IsTrue(string.IsNullOrWhiteSpace(loggerFirst.AssemblyQualifiedName));
        Assert.IsTrue(string.IsNullOrWhiteSpace(loggerFirst.CodeBase));
        Assert.IsTrue(loggerFirst.IsEnabled);
        Assert.AreEqual("<Key1>Value1</Key1><Key2>Value2</Key2>", loggerFirst.Configuration!.InnerXml);

        // 2nd logger
        var loggerSecond = loggerRunSettings.LoggerSettingsList[1];
        Assert.AreEqual("TestLogger", loggerSecond.FriendlyName);
        Assert.AreEqual(new Uri("testlogger://logger").ToString(), loggerSecond.Uri!.ToString());
        Assert.AreEqual("Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx", loggerSecond.AssemblyQualifiedName);
        Assert.AreEqual(@"C:\Sample\Sample.TestLogger.dll", loggerSecond.CodeBase);
        Assert.IsFalse(loggerSecond.IsEnabled);
        Assert.IsNull(loggerSecond.Configuration);

        // 3rd logger
        var loggerThird = loggerRunSettings.LoggerSettingsList[2];
        Assert.IsTrue(string.IsNullOrWhiteSpace(loggerThird.FriendlyName));
        Assert.AreEqual(new Uri("testlogger://loggerTemp").ToString(), loggerThird.Uri!.ToString());
        Assert.IsTrue(string.IsNullOrWhiteSpace(loggerThird.AssemblyQualifiedName));
        Assert.IsTrue(string.IsNullOrWhiteSpace(loggerThird.CodeBase));
        Assert.IsTrue(loggerThird.IsEnabled);
        Assert.AreEqual("<Key3>Value3</Key3><Key4>Value4</Key4>", loggerThird.Configuration!.InnerXml);
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLoggersWhenLoggerHasSelfEndingTag()
    {
        string runSettingsWithSelfEndingLoggers =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRUNSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"" />
                      <Logger friendlyName=""TestLogger"" uri=""testlogger://logger"" assemblyQualifiedName=""Sample.Sample.Sample.SampleLogger, Sample.Sample.Logger, Version=0.0.0.0, Culture=neutral, PublicKeyToken=xxxxxxxxxxxxxxxx"" enabled=""faLSe"">
                      </Logger>
                      <Logger uri=""testlogger://loggerTemp"" />
                    </Loggers>
                  </LoggerRUNSettings>
                </RunSettings>";

        var loggerRunSettings =
            XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithSelfEndingLoggers)!;

        Assert.AreEqual(3, loggerRunSettings.LoggerSettingsList.Count);
        Assert.AreEqual("TestLoggerWithParameterExtension", loggerRunSettings.LoggerSettingsList[0].FriendlyName);
        Assert.AreEqual("TestLogger", loggerRunSettings.LoggerSettingsList[1].FriendlyName);
        Assert.AreEqual("TestLogger", loggerRunSettings.LoggerSettingsList[1].FriendlyName);
        Assert.IsTrue(string.IsNullOrWhiteSpace(loggerRunSettings.LoggerSettingsList[2].FriendlyName));
    }

    [TestMethod]
    public void GetLoggerRunSettingsShouldReturnLastConfigurationElementIfMultiplePresent()
    {
        string runSettingsWithMultipleConfigurationElements =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                <RunSettings>
                  <RunConfiguration>
                  </RunConfiguration>
                  <LoggerRUNSettings>
                    <Loggers>
                      <Logger friendlyName=""TestLoggerWithParameterExtension"">
                        <ConfiGUration>
                          <Key1>Value1</Key1>
                          <Key2>Value2</Key2>
                        </ConfiGUration>
                        <ConfiGUration>
                          <Key3>Value3</Key3>
                          <Key4>Value4</Key4>
                        </ConfiGUration>
                      </Logger>
                    </Loggers>
                  </LoggerRUNSettings>
                </RunSettings>";

        var loggerRunSettings = XmlRunSettingsUtilities.GetLoggerRunSettings(runSettingsWithMultipleConfigurationElements)!;

        var expectedConfigurationElement = new XmlDocument().CreateElement("ConfiGUration");
        expectedConfigurationElement.InnerXml = "<Key3>Value3</Key3><Key4>Value4</Key4>";
        Assert.AreEqual(expectedConfigurationElement.Name,
            loggerRunSettings.LoggerSettingsList.First().Configuration!.Name);
        Assert.AreEqual(expectedConfigurationElement.InnerXml,
            loggerRunSettings.LoggerSettingsList.First().Configuration!.InnerXml);
    }

    #endregion

    #region GetDataCollectionRunSettings tests

    [TestMethod]
    public void GetDataCollectionRunSettingsShouldReturnNullIfSettingsIsNull()
    {
        Assert.IsNull(XmlRunSettingsUtilities.GetDataCollectionRunSettings(null));
    }

    [TestMethod]
    public void GetDataCollectionRunSettingsShouldReturnNullOnNoDataCollectorSettings()
    {
        Assert.IsNull(XmlRunSettingsUtilities.GetDataCollectionRunSettings(_emptyRunSettings));
    }

    [TestMethod]
    public void GetDataCollectionRunSettingsShouldReturnDataCollectorRunSettings()
    {
        Assert.IsNotNull(XmlRunSettingsUtilities.GetDataCollectionRunSettings(_runSettingsXmlWithDataCollectors));
    }

    [TestMethod]
    public void GetDataCollectionRunSettingsShouldReturnDataCollectorRunSettingsEvenIfDisabled()
    {
        Assert.IsNotNull(XmlRunSettingsUtilities.GetDataCollectionRunSettings(_runSettingsXmlWithDataCollectorsDisabled));
    }

    [TestMethod]
    public void GetDataCollectionRunSettingsShouldThrowOnMalformedDataCollectorSettings()
    {
        Assert.ThrowsException<SettingsException>(() => XmlRunSettingsUtilities.GetDataCollectionRunSettings(_runSettingsXmlWithIncorrectDataCollectorSettings));
    }

    #endregion

    [TestMethod]
    public void GetDataCollectorsFriendlyNameShouldReturnListOfFriendlyName()
    {
        var settingsXml = @"<RunSettings>
                                    <DataCollectionRunSettings>
                                        <DataCollectors>
                                            <DataCollector friendlyName=""DummyDataCollector1"">
                                            </DataCollector>
                                            <DataCollector friendlyName=""DummyDataCollector2"">
                                            </DataCollector>
                                        </DataCollectors>
                                    </DataCollectionRunSettings>
                                </RunSettings>";

        var friendlyNameList = XmlRunSettingsUtilities.GetDataCollectorsFriendlyName(settingsXml).ToList();

        Assert.AreEqual(2, friendlyNameList.Count, "There should be two friendly name");
        CollectionAssert.AreEqual(friendlyNameList, new List<string> { "DummyDataCollector1", "DummyDataCollector2" });
    }

    private static string ConvertOutOfProcDataCollectionSettingsToInProcDataCollectionSettings(string settings)
    {
        return
            settings.Replace("DataCollectionRunSettings", "InProcDataCollectionRunSettings")
                .Replace("<DataCollectors>", "<InProcDataCollectors>")
                .Replace("</DataCollectors>", "</InProcDataCollectors>")
                .Replace("<DataCollector ", "<InProcDataCollector ")
                .Replace("</DataCollector>", "</InProcDataCollector>");
    }
}
