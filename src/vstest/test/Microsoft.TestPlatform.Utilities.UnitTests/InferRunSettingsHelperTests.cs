// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OMResources = Microsoft.VisualStudio.TestPlatform.ObjectModel.Resources.CommonResources;

namespace Microsoft.TestPlatform.Utilities.UnitTests;

[TestClass]
public class InferRunSettingsHelperTests
{
    private readonly IDictionary<string, Architecture> _sourceArchitectures;
    private readonly IDictionary<string, Framework> _sourceFrameworks;
    private readonly Framework _frameworkNet45 = Framework.FromString(".NETFramework,Version=4.5")!;
    private readonly Framework _frameworkNet46 = Framework.FromString(".NETFramework,Version=4.6")!;
    private readonly Framework _frameworkNet47 = Framework.FromString(".NETFramework,Version=4.7")!;
    private const string MultiTargettingForwardLink = @"https://aka.ms/tp/vstest/multitargetingdoc?view=vs-2019";

    public InferRunSettingsHelperTests()
    {
        _sourceArchitectures = new Dictionary<string, Architecture>();
        _sourceFrameworks = new Dictionary<string, Framework>();
    }

    [TestMethod]
    public void UpdateRunSettingsShouldThrowIfRunSettingsNodeDoesNotExist()
    {
        var settings = @"<RandomSettings></RandomSettings>";
        var xmlDocument = GetXmlDocument(settings);

        Action action = () => InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X86, Framework.DefaultFramework, "temp");

        var exception = Assert.ThrowsExactly<XmlException>(action);
        Assert.AreEqual("An error occurred while loading the settings.  Error: Could not find 'RunSettings' node..", exception.Message);
    }

    [TestMethod]
    public void UpdateRunSettingsShouldThrowIfPlatformNodeIsInvalid()
    {
        var settings = @"<RunSettings><RunConfiguration><TargetPlatform>foo</TargetPlatform></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        Action action = () => InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X86, Framework.DefaultFramework, "temp");

        var exception = Assert.ThrowsExactly<XmlException>(action);
        Assert.AreEqual("An error occurred while loading the settings.  Error: Invalid setting 'RunConfiguration'. Invalid value 'foo' specified for 'TargetPlatform'..", exception.Message);
    }

    [TestMethod]
    public void UpdateRunSettingsShouldThrowIfFrameworkNodeIsInvalid()
    {
        var settings = @"<RunSettings><RunConfiguration><TargetFrameworkVersion>foo</TargetFrameworkVersion></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        Action action = () => InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X86, Framework.DefaultFramework, "temp");

        var exception = Assert.ThrowsExactly<XmlException>(action);
        Assert.AreEqual("An error occurred while loading the settings.  Error: Invalid setting 'RunConfiguration'. Invalid value 'foo' specified for 'TargetFrameworkVersion'..", exception.Message);
    }

    [TestMethod]
    public void UpdateRunSettingsShouldUpdateWithPlatformSettings()
    {
        var settings = @"<RunSettings></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X86, Framework.DefaultFramework, "temp");

        var xml = xmlDocument.OuterXml;

        StringAssert.Contains(xml, "<TargetPlatform>X86</TargetPlatform>");
    }

    [TestMethod]
    public void UpdateRunSettingsShouldUpdateWithFrameworkSettings()
    {
        var settings = @"<RunSettings></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X86, Framework.DefaultFramework, "temp");

        var xml = xmlDocument.OuterXml;

        StringAssert.Contains(xml, $"<TargetFrameworkVersion>{Framework.DefaultFramework.Name}</TargetFrameworkVersion>");
    }

    [TestMethod]
    public void UpdateRunSettingsShouldUpdateWithResultsDirectorySettings()
    {
        var settings = @"<RunSettings></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X86, Framework.DefaultFramework, "temp");

        var xml = xmlDocument.OuterXml;

        StringAssert.Contains(xml, "<ResultsDirectory>temp</ResultsDirectory>");
    }

    [TestMethod]
    public void UpdateRunSettingsShouldNotUpdatePlatformIfRunSettingsAlreadyHasIt()
    {
        var settings = @"<RunSettings><RunConfiguration><TargetPlatform>X86</TargetPlatform></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X64, Framework.DefaultFramework, "temp");

        var xml = xmlDocument.OuterXml;

        StringAssert.Contains(xml, "<TargetPlatform>X86</TargetPlatform>");
    }

    [TestMethod]
    public void UpdateRunSettingsShouldNotUpdateFrameworkIfRunSettingsAlreadyHasIt()
    {
        var settings = @"<RunSettings><RunConfiguration><TargetFrameworkVersion>Framework40</TargetFrameworkVersion></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X64, Framework.DefaultFramework, "temp");

        var xml = xmlDocument.OuterXml;

        StringAssert.Contains(xml, "<TargetFrameworkVersion>.NETFramework,Version=v4.0</TargetFrameworkVersion>");
    }
    //TargetFrameworkMoniker

    [TestMethod]
    public void UpdateRunSettingsShouldAllowTargetFrameworkMonikerValue()
    {

        var settings = @"<RunSettings><RunConfiguration><TargetFrameworkVersion>.NETFramework,Version=v4.0</TargetFrameworkVersion></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X64, Framework.DefaultFramework, "temp");

        var xml = xmlDocument.OuterXml;

        StringAssert.Contains(xml, "<TargetFrameworkVersion>.NETFramework,Version=v4.0</TargetFrameworkVersion>");
    }

    [TestMethod]
    public void UpdateRunSettingsShouldNotUpdateResultsDirectoryIfRunSettingsAlreadyHasIt()
    {
        var settings = @"<RunSettings><RunConfiguration><ResultsDirectory>someplace</ResultsDirectory></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X64, Framework.DefaultFramework, "temp");

        var xml = xmlDocument.OuterXml;

        StringAssert.Contains(xml, "<ResultsDirectory>someplace</ResultsDirectory>");
    }

    [TestMethod]
    public void UpdateRunSettingsShouldNotUpdatePlatformOrFrameworkOrResultsDirectoryIfRunSettingsAlreadyHasIt()
    {
        var settings = @"<RunSettings><RunConfiguration><TargetPlatform>X86</TargetPlatform><TargetFrameworkVersion>Framework40</TargetFrameworkVersion><ResultsDirectory>someplace</ResultsDirectory></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X64, Framework.DefaultFramework, "temp");

        var xml = xmlDocument.OuterXml;

        StringAssert.Contains(xml, "<TargetPlatform>X86</TargetPlatform>");
        StringAssert.Contains(xml, "<TargetFrameworkVersion>Framework40</TargetFrameworkVersion>");
        StringAssert.Contains(xml, "<ResultsDirectory>someplace</ResultsDirectory>");
    }

    [TestMethod]
    public void UpdateRunSettingsWithAnEmptyRunSettingsShouldAddValuesSpecifiedInRunConfiguration()
    {
        var settings = @"<RunSettings></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X64, Framework.DefaultFramework, "temp");

        var xml = xmlDocument.OuterXml;

        StringAssert.Contains(xml, "<TargetPlatform>X64</TargetPlatform>");
        StringAssert.Contains(xml, $"<TargetFrameworkVersion>{Framework.DefaultFramework.Name}</TargetFrameworkVersion>");
        StringAssert.Contains(xml, "<ResultsDirectory>temp</ResultsDirectory>");
    }

    [TestMethod]
    public void UpdateRunSettingsShouldReturnBackACompleteRunSettings()
    {
        var settings = @"<RunSettings></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateRunSettingsWithUserProvidedSwitches(xmlDocument, Architecture.X64, Framework.DefaultFramework, "temp");

        var xml = xmlDocument.OuterXml;
        var expectedRunSettings = $"<RunSettings><RunConfiguration><ResultsDirectory>temp</ResultsDirectory><TargetPlatform>X64</TargetPlatform><TargetFrameworkVersion>{Framework.DefaultFramework.Name}</TargetFrameworkVersion></RunConfiguration></RunSettings>";

        Assert.AreEqual(expectedRunSettings, xml);
    }

    [TestMethod]
    public void UpdateDesignModeOrCsiShouldNotModifyXmlIfNodeIsAlreadyPresent()
    {
        var settings = @"<RunSettings><RunConfiguration><DesignMode>False</DesignMode><CollectSourceInformation>False</CollectSourceInformation></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateDesignMode(xmlDocument, true);
        InferRunSettingsHelper.UpdateCollectSourceInformation(xmlDocument, true);

        Assert.AreEqual("False", GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/DesignMode"));
        Assert.AreEqual("False", GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/CollectSourceInformation"));
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void UpdateDesignModeOrCsiShouldModifyXmlToValueProvided(bool val)
    {
        var settings = @"<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateDesignMode(xmlDocument, val);
        InferRunSettingsHelper.UpdateCollectSourceInformation(xmlDocument, val);

        Assert.AreEqual(val.ToString(), GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/DesignMode"));
        Assert.AreEqual(val.ToString(), GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/CollectSourceInformation"));
    }

    [TestMethod]
    public void MakeRunsettingsCompatibleShouldDeleteNewlyAddedRunConfigurationNode()
    {
        var settings = @"<RunSettings><RunConfiguration><DesignMode>False</DesignMode><CollectSourceInformation>False</CollectSourceInformation></RunConfiguration></RunSettings>";

        var result = InferRunSettingsHelper.MakeRunsettingsCompatible(settings)!;

        Assert.IsTrue(result.IndexOf("DesignMode", StringComparison.OrdinalIgnoreCase) < 0);
    }

    [TestMethod]
    public void MakeRunsettingsCompatibleShouldNotDeleteOldRunConfigurationNode()
    {
        var settings = @"<RunSettings>
                                <RunConfiguration>
                                    <DesignMode>False</DesignMode>
                                    <CollectSourceInformation>False</CollectSourceInformation>
                                    <TargetPlatform>x86</TargetPlatform>
                                    <TargetFrameworkVersion>net46</TargetFrameworkVersion>
                                    <TestAdaptersPaths>dummypath</TestAdaptersPaths>
                                    <ResultsDirectory>dummypath</ResultsDirectory>
                                    <SolutionDirectory>dummypath</SolutionDirectory>
                                    <MaxCpuCount>2</MaxCpuCount>
                                    <DisableParallelization>False</DisableParallelization>
                                    <DisableAppDomain>False</DisableAppDomain>
                                </RunConfiguration>
                            </RunSettings>";

        var result = InferRunSettingsHelper.MakeRunsettingsCompatible(settings)!;

        Assert.IsTrue(result.IndexOf("TargetPlatform", StringComparison.OrdinalIgnoreCase) > 0);
        Assert.IsTrue(result.IndexOf("TargetFrameworkVersion", StringComparison.OrdinalIgnoreCase) > 0);
        Assert.IsTrue(result.IndexOf("TestAdaptersPaths", StringComparison.OrdinalIgnoreCase) > 0);
        Assert.IsTrue(result.IndexOf("ResultsDirectory", StringComparison.OrdinalIgnoreCase) > 0);
        Assert.IsTrue(result.IndexOf("SolutionDirectory", StringComparison.OrdinalIgnoreCase) > 0);
        Assert.IsTrue(result.IndexOf("MaxCpuCount", StringComparison.OrdinalIgnoreCase) > 0);
        Assert.IsTrue(result.IndexOf("DisableParallelization", StringComparison.OrdinalIgnoreCase) > 0);
        Assert.IsTrue(result.IndexOf("DisableAppDomain", StringComparison.OrdinalIgnoreCase) > 0);
    }

    [TestMethod]
    public void UpdateTargetDeviceValueFromOldMsTestSettings()
    {
        var settings = @"<RunSettings>
                                <RunConfiguration>
                                    <MaxCpuCount>2</MaxCpuCount>
                                    <DisableParallelization>False</DisableParallelization>
                                    <DisableAppDomain>False</DisableAppDomain>
                                </RunConfiguration>
                                <MSPhoneTest>
                                  <TargetDevice>169.254.193.190</TargetDevice>
                                </MSPhoneTest>
                            </RunSettings>";

        var xmlDocument = GetXmlDocument(settings);

        var result = InferRunSettingsHelper.TryGetDeviceXml(xmlDocument.CreateNavigator()!, out string? deviceXml);
        Assert.IsTrue(result);
        Assert.IsNotNull(deviceXml);

        InferRunSettingsHelper.UpdateTargetDevice(xmlDocument, deviceXml);
        Assert.AreEqual(deviceXml.ToString(), GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/TargetDevice"));
    }

    [TestMethod]
    public void UpdateTargetPlatformShouldNotModifyXmlIfNodeIsAlreadyPresentForOverwriteFalse()
    {
        var settings = @"<RunSettings><RunConfiguration><TargetPlatform>x86</TargetPlatform></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateTargetPlatform(xmlDocument, "X64", overwrite: false);

        Assert.AreEqual("x86", GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/TargetPlatform"));
    }

    [TestMethod]
    public void UpdateTargetPlatformShouldModifyXmlIfNodeIsAlreadyPresentForOverwriteTrue()
    {
        var settings = @"<RunSettings><RunConfiguration><TargetPlatform>x86</TargetPlatform></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateTargetPlatform(xmlDocument, "X64", overwrite: true);

        Assert.AreEqual("X64", GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/TargetPlatform"));
    }

    [TestMethod]
    public void UpdateTargetPlatformShouldAddPlatformXmlNodeIfNotPresent()
    {
        var settings = @"<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateTargetPlatform(xmlDocument, "X64");

        Assert.AreEqual("X64", GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/TargetPlatform"));
    }

    [TestMethod]
    public void UpdateTargetFrameworkShouldNotModifyXmlIfNodeIsAlreadyPresentForOverwriteFalse()
    {
        var settings = @"<RunSettings><RunConfiguration><TargetFrameworkVersion>.NETFramework,Version=v4.5</TargetFrameworkVersion></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateTargetFramework(xmlDocument, ".NETCoreApp,Version=v1.0", overwrite: false);

        Assert.AreEqual(".NETFramework,Version=v4.5", GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/TargetFrameworkVersion"));
    }

    [TestMethod]
    public void UpdateTargetFrameworkShouldModifyXmlIfNodeIsAlreadyPresentForOverwriteTrue()
    {
        var settings = @"<RunSettings><RunConfiguration><TargetFrameworkVersion>.NETFramework,Version=v4.5</TargetFrameworkVersion></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateTargetFramework(xmlDocument, ".NETCoreApp,Version=v1.0", overwrite: true);

        Assert.AreEqual(".NETCoreApp,Version=v1.0", GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/TargetFrameworkVersion"));
    }

    [TestMethod]
    public void UpdateTargetFrameworkShouldAddFrameworkXmlNodeIfNotPresent()
    {
        var settings = @"<RunSettings><RunConfiguration></RunConfiguration></RunSettings>";
        var xmlDocument = GetXmlDocument(settings);

        InferRunSettingsHelper.UpdateTargetFramework(xmlDocument, ".NETCoreApp,Version=v1.0");

        Assert.AreEqual(".NETCoreApp,Version=v1.0", GetValueOf(xmlDocument, "/RunSettings/RunConfiguration/TargetFrameworkVersion"));
    }

    [TestMethod]
    public void FilterCompatibleSourcesShouldIdentifyIncomaptiableSourcesAndConstructWarningMessage()
    {
        #region Arrange
        _sourceArchitectures["AnyCPU1net46.dll"] = Architecture.AnyCPU;
        _sourceArchitectures["x64net47.exe"] = Architecture.X64;
        _sourceArchitectures["x86net45.dll"] = Architecture.X86;

        _sourceFrameworks["AnyCPU1net46.dll"] = _frameworkNet46;
        _sourceFrameworks["x64net47.exe"] = _frameworkNet47;
        _sourceFrameworks["x86net45.dll"] = _frameworkNet45;

        StringBuilder sb = new();
        sb.AppendLine();
        sb.AppendLine(GetSourceIncompatibleMessage("AnyCPU1net46.dll"));
        sb.AppendLine(GetSourceIncompatibleMessage("x64net47.exe"));
        sb.AppendLine(GetSourceIncompatibleMessage("x86net45.dll"));

        var expected = string.Format(CultureInfo.CurrentCulture, OMResources.DisplayChosenSettings, _frameworkNet45, Constants.DefaultPlatform, sb.ToString(), MultiTargettingForwardLink);


        #endregion
        var compatibleSources = InferRunSettingsHelper.FilterCompatibleSources(Constants.DefaultPlatform, Constants.DefaultPlatform, _frameworkNet45, _sourceArchitectures, _sourceFrameworks, out string warningMessage);

        // None of the DLLs passed are compatible to the chosen settings
        Assert.AreEqual(0, compatibleSources.Count());
        Assert.AreEqual(expected, warningMessage);
    }

    [TestMethod]
    public void FilterCompatibleSourcesShouldIdentifyCompatibleSources()
    {
        _sourceArchitectures["x64net45.exe"] = Architecture.X64;
        _sourceArchitectures["x86net45.dll"] = Architecture.X86;

        _sourceFrameworks["x64net45.exe"] = _frameworkNet45;
        _sourceFrameworks["x86net45.dll"] = _frameworkNet45;

        StringBuilder sb = new();
        sb.AppendLine();
        sb.AppendLine(GetSourceIncompatibleMessage("x86net45.dll"));

        var expected = string.Format(CultureInfo.CurrentCulture, OMResources.DisplayChosenSettings, _frameworkNet45, Constants.DefaultPlatform, sb.ToString(), MultiTargettingForwardLink);

        var compatibleSources = InferRunSettingsHelper.FilterCompatibleSources(Constants.DefaultPlatform, Constants.DefaultPlatform, _frameworkNet45, _sourceArchitectures, _sourceFrameworks, out string warningMessage);

        // only "x64net45.exe" is the compatible source
        Assert.AreEqual(1, compatibleSources.Count());
        Assert.AreEqual(expected, warningMessage);
    }

    [TestMethod]
    public void FilterCompatibleSourcesShouldNotComposeWarningIfSettingsAreCorrect()
    {
        _sourceArchitectures["x86net45.dll"] = Architecture.X86;
        _sourceFrameworks["x86net45.dll"] = _frameworkNet45;

        var compatibleSources = InferRunSettingsHelper.FilterCompatibleSources(Architecture.X86, Constants.DefaultPlatform, _frameworkNet45, _sourceArchitectures, _sourceFrameworks, out string warningMessage);

        // only "x86net45.dll" is the compatible source
        Assert.AreEqual(1, compatibleSources.Count());
        Assert.IsTrue(string.IsNullOrEmpty(warningMessage));
    }

    [TestMethod]
    public void FilterCompatibleSourcesShouldRetrunWarningMessageIfNoConflict()
    {
        _sourceArchitectures["x64net45.exe"] = Architecture.X64;
        _sourceFrameworks["x64net45.exe"] = _frameworkNet45;
        _ = InferRunSettingsHelper.FilterCompatibleSources(Constants.DefaultPlatform, Constants.DefaultPlatform, _frameworkNet45, _sourceArchitectures, _sourceFrameworks, out string warningMessage);

        Assert.IsTrue(string.IsNullOrEmpty(warningMessage));
    }

    [TestMethod]
    public void IsTestSettingsEnabledShouldReturnTrueIfRunsettingsHasTestSettings()
    {
        string runsettingsString = @"<RunSettings>
                                        <MSTest>
                                            <SettingsFile>C:\temp.testsettings</SettingsFile>
                                            <ForcedLegacyMode>true</ForcedLegacyMode>
                                        </MSTest>
                                    </RunSettings>";

        Assert.IsTrue(InferRunSettingsHelper.IsTestSettingsEnabled(runsettingsString));
    }

    [TestMethod]
    public void IsTestSettingsEnabledShouldReturnFalseIfRunsettingsDoesnotHaveTestSettings()
    {
        string runsettingsString = @"<RunSettings>
                                        <MSTest>
                                            <ForcedLegacyMode>true</ForcedLegacyMode>
                                        </MSTest>
                                    </RunSettings>";

        Assert.IsFalse(InferRunSettingsHelper.IsTestSettingsEnabled(runsettingsString));
    }

    [TestMethod]
    public void TryGetLegacySettingsForRunSettingsWithoutLegacySettingsShouldReturnFalse()
    {
        string runSettingsXml = @"<RunSettings>
                                      </RunSettings>";
        Assert.IsFalse(InferRunSettingsHelper.TryGetLegacySettingElements(runSettingsXml, out _));
    }

    [TestMethod]
    public void TryGetLegacySettingsForRunSettingsWithInvalidLegacySettingsShouldReturnFalse()
    {
        string runSettingsXml = @"<RunSettings>
                                        <LegacySettings>
                                            <Foo>
                                        </LegacySettings>
                                      </RunSettings>";
        Assert.IsFalse(InferRunSettingsHelper.TryGetLegacySettingElements(runSettingsXml, out _));
    }

    [TestMethod]
    public void TryGetLegacySettingsForRunSettingsWithEmptyLegacySettingsShouldReturnTrueAndEmptyListForLegacySettingElements()
    {
        string runSettingsXml = @"<RunSettings>
                                        <LegacySettings>
                                        </LegacySettings>
                                      </RunSettings>";

        Assert.IsTrue(InferRunSettingsHelper.TryGetLegacySettingElements(runSettingsXml, out Dictionary<string, string> legacySettings));
        Assert.AreEqual(0, legacySettings.Count);
    }

    [TestMethod]
    public void TryGetLegacySettingsForRunSettingsWithValidLegacySettingsShouldReturnTrueAndListForLegacySettingElements()
    {
        string runSettingsXml = @"<RunSettings>
                                       <LegacySettings>
                                            <Deployment enabled=""true"" deploySatelliteAssemblies=""true"" >
                                                <DeploymentItem filename="".\test.txt"" />
                                            </Deployment>
                                            <Scripts setupScript="".\setup.bat"" cleanupScript="".\cleanup.bat"" />
                                            <Execution hostProcessPlatform=""MSIL"" parallelTestCount=""4"">
                                                <Timeouts testTimeout=""120"" />
                                                <TestTypeSpecific>
                                                    <UnitTestRunConfig>
                                                        <AssemblyResolution />
                                                    </UnitTestRunConfig>
                                                </TestTypeSpecific>
                                                <Hosts />
                                            </Execution>
                                       </LegacySettings>
                                      </RunSettings>";

        var expectedElements = "Deployment, Scripts, Execution, AssemblyResolution, Timeouts, Hosts";
        var expectedDeploymentAttributes = "enabled, deploySatelliteAssemblies";
        var expectedExecutionAttributes = "hostProcessPlatform, parallelTestCount";

        Assert.IsTrue(InferRunSettingsHelper.TryGetLegacySettingElements(runSettingsXml, out Dictionary<string, string> legacySettings));
        Assert.AreEqual(3, legacySettings.Count, "count does not match");
        Assert.AreEqual(expectedElements, legacySettings["Elements"]);
        Assert.AreEqual(expectedDeploymentAttributes, legacySettings["DeploymentAttributes"]);
        Assert.AreEqual(expectedExecutionAttributes, legacySettings["ExecutionAttributes"]);
    }

    [TestMethod]
    public void GetEnvironmentVariablesWithValidValuesInRunSettingsShouldReturnValidDictionary()
    {
        string runSettingsXml = @"<RunSettings>
                                       <RunConfiguration>
                                          <EnvironmentVariables>
                                             <RANDOM_PATH>C:\temp</RANDOM_PATH>
                                             <RANDOM_PATH2>C:\temp2</RANDOM_PATH2>
                                          </EnvironmentVariables>
                                       </RunConfiguration>
                                      </RunSettings>";

        var envVars = InferRunSettingsHelper.GetEnvironmentVariables(runSettingsXml)!;

        Assert.AreEqual(2, envVars.Count);
        Assert.AreEqual(@"C:\temp", envVars["RANDOM_PATH"]);
        Assert.AreEqual(@"C:\temp2", envVars["RANDOM_PATH2"]);
    }

    [TestMethod]
    public void GetEnvironmentVariablesWithDuplicateEnvValuesInRunSettingsShouldReturnValidDictionary()
    {
        string runSettingsXml = @"<RunSettings>
                                       <RunConfiguration>
                                          <EnvironmentVariables>
                                             <RANDOM_PATH>C:\temp</RANDOM_PATH>
                                             <RANDOM_PATH>C:\temp2</RANDOM_PATH>
                                          </EnvironmentVariables>
                                       </RunConfiguration>
                                      </RunSettings>";

        var envVars = InferRunSettingsHelper.GetEnvironmentVariables(runSettingsXml)!;

        Assert.AreEqual(1, envVars.Count);
        Assert.AreEqual(@"C:\temp", envVars["RANDOM_PATH"]);
    }

    [TestMethod]
    public void GetEnvironmentVariablesWithEmptyVariablesInRunSettingsShouldReturnEmptyDictionary()
    {
        string runSettingsXml = @"<RunSettings>
                                       <RunConfiguration>
                                         <EnvironmentVariables>
                                         </EnvironmentVariables>
                                       </RunConfiguration>
                                      </RunSettings>";

        var envVars = InferRunSettingsHelper.GetEnvironmentVariables(runSettingsXml)!;
        Assert.AreEqual(0, envVars.Count);
    }

    [TestMethod]
    public void GetEnvironmentVariablesWithInvalidValuesInRunSettingsShouldReturnNull()
    {
        string runSettingsXml = @"<RunSettings>
                                       <RunConfiguration>
                                         <EnvironmentVariables>
                                            <Foo>
                                         </EnvironmentVariables>
                                       </RunConfiguration>
                                      </RunSettings>";

        var envVars = InferRunSettingsHelper.GetEnvironmentVariables(runSettingsXml);
        Assert.IsNull(envVars);
    }

    [TestMethod]
    public void GetEnvironmentVariablesWithoutEnvVarNodeInRunSettingsShouldReturnNull()
    {
        string runSettingsXml = @"<RunSettings>
                                       <RunConfiguration>
                                       </RunConfiguration>
                                      </RunSettings>";

        var envVars = InferRunSettingsHelper.GetEnvironmentVariables(runSettingsXml);
        Assert.IsNull(envVars);
    }

    #region RunSettingsIncompatibeWithTestSettings Tests

    [TestMethod]
    public void RunSettingsWithCodeCoverageAndInlineTestSettingsXml()
    {
        // Setup
        var runSettingsWithCodeCoverageAndInlineTestSettingsXml = @"
                    <RunSettings>
                      <RunConfiguration>
                        <TargetFrameworkVersion>Framework45</TargetFrameworkVersion>
                        <ResultsDirectory>C:\TestProject1\TestResults</ResultsDirectory>
                        <SolutionDirectory>C:\TestProject1\</SolutionDirectory>
                        <TargetPlatform>X86</TargetPlatform>
                      </RunConfiguration>
                      <MSTest>
                        <SettingsFile>C:\TestProject1\TestSettings1.testsettings</SettingsFile>
                        <ForcedLegacyMode>true</ForcedLegacyMode>
                        <IgnoreTestImpact>true</IgnoreTestImpact>
                      </MSTest>
                      <DataCollectionRunSettings>
                        <DataCollectors>
                          <DataCollector friendlyName=""Code Coverage"" uri=""datacollector://Microsoft/CodeCoverage/2.0"" assemblyQualifiedName=""DynamicCoverageDataCollector"">
                            <Configuration>
                              <CoverageFileName>DevBox 2018-01-07 20_42_30.coverage</CoverageFileName>
                              <CodeCoverage>
                              </CodeCoverage>
                            </Configuration>
                          </DataCollector>
                        </DataCollectors>
                      </DataCollectionRunSettings>
                    </RunSettings>";

        // Act and validate
        Assert.IsFalse(InferRunSettingsHelper.AreRunSettingsCollectorsIncompatibleWithTestSettings(
            runSettingsWithCodeCoverageAndInlineTestSettingsXml), "Invalid response");
        Assert.IsTrue(InferRunSettingsHelper.AreRunSettingsCollectorsIncompatibleWithTestSettings(
            ConvertOutOfProcToInProcDataCollectionSettings(runSettingsWithCodeCoverageAndInlineTestSettingsXml)), "Invalid response");
    }

    [TestMethod]
    public void RunSettingsWithFakesAndCodeCoverageAndInlineTestSettingsXml()
    {
        var runSettingsWithFakesAndCodeCoverageAndInlineTestSettingsXml = @"
                <RunSettings>
                  <RunConfiguration>
                    <TargetFrameworkVersion>Framework45</TargetFrameworkVersion>
                    <ResultsDirectory>C:\TestProject1\TestResults</ResultsDirectory>
                    <SolutionDirectory>C:\TestProject1\</SolutionDirectory>
                    <TargetPlatform>X86</TargetPlatform>
                  </RunConfiguration>
                  <MSTest>
                    <SettingsFile>C:\TestProject1\TestSettings1.testsettings</SettingsFile>
                    <ForcedLegacyMode>true</ForcedLegacyMode>
                    <IgnoreTestImpact>true</IgnoreTestImpact>
                  </MSTest>
                  <DataCollectionRunSettings>
                    <DataCollectors>
                      <DataCollector friendlyName=""Code Coverage"" uri=""datacollector://Microsoft/CodeCoverage/2.0"" assemblyQualifiedName=""DynamicCoverageDataCollector"">
                      </DataCollector>
                      <DataCollector friendlyName=""UnitTestIsolation"" uri=""datacollector://Microsoft/unittestisolation/1.0"" assemblyQualifiedName=""DynamicCoverageDataCollector"">
                      </DataCollector>
                    </DataCollectors>
                  </DataCollectionRunSettings>
                </RunSettings>";

        // Act and validate
        Assert.IsFalse(InferRunSettingsHelper.AreRunSettingsCollectorsIncompatibleWithTestSettings(
            runSettingsWithFakesAndCodeCoverageAndInlineTestSettingsXml), "Invalid response");
        Assert.IsTrue(InferRunSettingsHelper.AreRunSettingsCollectorsIncompatibleWithTestSettings(
            ConvertOutOfProcToInProcDataCollectionSettings(runSettingsWithFakesAndCodeCoverageAndInlineTestSettingsXml)), "Invalid response");
    }

    [TestMethod]
    public void RunSettingsWithEnabledAndDisabledCollectorAndNoEmbeddedTestSettingsXml()
    {
        var runSettingsWithEnabledAndDisabledCollectorAndInlineTestSettingsXml = @"
                <RunSettings>
                    <RunConfiguration>
                        <TargetFrameworkVersion>Framework45</TargetFrameworkVersion>
                        <ResultsDirectory>C:\TestProject1\TestResults</ResultsDirectory>
                        <SolutionDirectory>C:\TestProject1\</SolutionDirectory>
                        <TargetPlatform>X86</TargetPlatform>
                    </RunConfiguration>
                    <DataCollectionRunSettings>
                    <DataCollectors>
                        <DataCollector friendlyName=""Video"" uri=""datacollector://Microsoft/Video/2.0"" assemblyQualifiedName=""VideoCollector"">
                        </DataCollector>
                    </DataCollectors>
                    <DataCollectors>
                        <DataCollector friendlyName=""EventLog"" uri=""datacollector://Microsoft/Log/2.0"" enabled=""false"" assemblyQualifiedName=""LogCollector"">
                        </DataCollector>
                    </DataCollectors>
                    </DataCollectionRunSettings>
                </RunSettings>";

        // Act and validate
        Assert.IsFalse(InferRunSettingsHelper.AreRunSettingsCollectorsIncompatibleWithTestSettings(
            runSettingsWithEnabledAndDisabledCollectorAndInlineTestSettingsXml), "Invalid response");
        Assert.IsFalse(InferRunSettingsHelper.AreRunSettingsCollectorsIncompatibleWithTestSettings(
            ConvertOutOfProcToInProcDataCollectionSettings(runSettingsWithEnabledAndDisabledCollectorAndInlineTestSettingsXml)), "Invalid response");
    }

    [TestMethod]
    public void RunSettingsWithEnabledAndDisabledCollectorAndInlineTestSettingsXml()
    {
        var runSettingsWithEnabledAndDisabledCollectorAndInlineTestSettingsXml = @"
                <RunSettings>
                    <RunConfiguration>
                        <TargetFrameworkVersion>Framework45</TargetFrameworkVersion>
                        <ResultsDirectory>C:\TestProject1\TestResults</ResultsDirectory>
                        <SolutionDirectory>C:\TestProject1\</SolutionDirectory>
                        <TargetPlatform>X86</TargetPlatform>
                    </RunConfiguration>
                    <MSTest>
                        <SettingsFile>C:\TestProject1\TestSettings1.testsettings</SettingsFile>
                        <ForcedLegacyMode>true</ForcedLegacyMode>
                        <IgnoreTestImpact>true</IgnoreTestImpact>
                    </MSTest>
                    <DataCollectionRunSettings>
                    <DataCollectors>
                        <DataCollector friendlyName=""Video"" uri=""datacollector://Microsoft/Video/2.0"" assemblyQualifiedName=""VideoCollector"">
                        </DataCollector>
                    </DataCollectors>
                    <DataCollectors>
                        <DataCollector friendlyName=""EventLog"" uri=""datacollector://Microsoft/Log/2.0"" enabled=""false"" assemblyQualifiedName=""LogCollector"">
                        </DataCollector>
                    </DataCollectors>
                    </DataCollectionRunSettings>
                </RunSettings>";

        // Act and validate
        Assert.IsTrue(InferRunSettingsHelper.AreRunSettingsCollectorsIncompatibleWithTestSettings(
            runSettingsWithEnabledAndDisabledCollectorAndInlineTestSettingsXml), "Invalid response");
        Assert.IsTrue(InferRunSettingsHelper.AreRunSettingsCollectorsIncompatibleWithTestSettings(
            ConvertOutOfProcToInProcDataCollectionSettings(runSettingsWithEnabledAndDisabledCollectorAndInlineTestSettingsXml)), "Invalid response");
    }

    [TestMethod]
    public void RunSettingsWithDisabledCollectionSettingsAndInlineTestSettingsXml()
    {
        var runSettingsWithDisabledCollectionSettingsAndInlineTestSettingsXml = @"
                <RunSettings>
                  <RunConfiguration>
                    <TargetFrameworkVersion>Framework45</TargetFrameworkVersion>
                    <ResultsDirectory>C:\TestProject1\TestResults</ResultsDirectory>
                    <SolutionDirectory>C:\TestProject1\</SolutionDirectory>
                    <TargetPlatform>X86</TargetPlatform>
                  </RunConfiguration>
                  <MSTest>
                    <SettingsFile>C:\TestProject1\TestSettings1.testsettings</SettingsFile>
                    <ForcedLegacyMode>true</ForcedLegacyMode>
                    <IgnoreTestImpact>true</IgnoreTestImpact>
                  </MSTest>
                  <DataCollectionRunSettings>
                    <DataCollectors>
                      <DataCollector friendlyName=""Video"" uri=""datacollector://Microsoft/Video/2.0"" enabled=""false"" assemblyQualifiedName=""VideoCollector"">
                      </DataCollector>
                    </DataCollectors>
                    <DataCollectors>
                      <DataCollector friendlyName=""EventLog"" uri=""datacollector://Microsoft/Log/2.0"" enabled=""false"" assemblyQualifiedName=""LogCollector"">
                      </DataCollector>
                    </DataCollectors>
                  </DataCollectionRunSettings>
                </RunSettings>";

        // Act and validate
        Assert.IsFalse(InferRunSettingsHelper.AreRunSettingsCollectorsIncompatibleWithTestSettings(
            runSettingsWithDisabledCollectionSettingsAndInlineTestSettingsXml), "Invalid response");
        Assert.IsFalse(InferRunSettingsHelper.AreRunSettingsCollectorsIncompatibleWithTestSettings(
            ConvertOutOfProcToInProcDataCollectionSettings(runSettingsWithDisabledCollectionSettingsAndInlineTestSettingsXml)), "Invalid response");
    }

    #endregion

    #region Private Methods

    private string GetSourceIncompatibleMessage(string source)
    {
        return string.Format(CultureInfo.CurrentCulture, OMResources.SourceIncompatible, source, _sourceFrameworks[source].Name, _sourceArchitectures[source]);
    }

    private static XmlDocument GetXmlDocument(string settingsXml)
    {
        var doc = new XmlDocument();
        doc.LoadXml(settingsXml);

        return doc;
    }

    private static string GetValueOf(XmlDocument xmlDocument, string xpath)
    {
        return xmlDocument.SelectSingleNode(xpath)!.InnerText;
    }

    private static string ConvertOutOfProcToInProcDataCollectionSettings(string settings)
    {
        return settings.Replace("DataCollectionRunSettings", "InProcDataCollectionRunSettings")
            .Replace("<DataCollectors>", "<InProcDataCollectors>")
            .Replace("</DataCollectors>", "</InProcDataCollectors>")
            .Replace("<DataCollector ", "<InProcDataCollector ")
            .Replace("</DataCollector>", "</InProcDataCollector>");
    }

    #endregion
}
