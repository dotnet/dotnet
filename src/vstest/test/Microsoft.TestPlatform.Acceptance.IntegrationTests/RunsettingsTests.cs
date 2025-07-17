// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
// monitoring the processes does not work correctly
[TestCategory("Windows-Review")]
public class RunsettingsTests : AcceptanceTestBase
{
    #region Runsettings precedence tests
    /// <summary>
    /// Command line run settings should have high precedence among settings file, cli runsettings and cli switches
    /// </summary>
    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void CommandLineRunSettingsShouldWinAmongAllOptions(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var targetPlatform = "x86";
        var testhostProcessName = new[] { "testhost.x86" };

        // We pass 2 dlls in RunTestWithRunSettings, and MaxCpuCount=1 should win,
        // we should see 1 testhost for .NET Framework (we share the host there),
        // and 2 testhosts for .NET, because we don't share hosts there for non-parallel run.
        var expectedNumOfProcessCreated = runnerInfo.IsNetFrameworkTarget ? 1 : 2;

        // passing parallel
        var runConfigurationDictionary = new Dictionary<string, string>
        {
            { "MaxCpuCount", "0" },
            { "TargetFrameworkVersion", GetTargetFrameworkForRunsettings() },
            { "TestAdaptersPaths", GetTestAdapterPath() }
        };
        // passing different platform
        var additionalArgs = "/Platform:x64";

        var runSettingsArgs = string.Join(
            " ",
            [
                "RunConfiguration.MaxCpuCount=1",
                string.Concat("RunConfiguration.TargetPlatform=",targetPlatform),
                string.Concat("RunConfiguration.TargetFrameworkVersion=" , GetTargetFrameworkForRunsettings()),
                string.Concat("RunConfiguration.TestAdaptersPaths=" , GetTestAdapterPath())
            ]);

        RunTestWithRunSettings(runConfigurationDictionary, runSettingsArgs, additionalArgs, testhostProcessName, expectedNumOfProcessCreated);
    }

    /// <summary>
    /// Command line run settings should have high precedence between cli runsettings and cli switches.
    /// </summary>
    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void CLIRunsettingsShouldWinBetweenCLISwitchesAndCLIRunsettings(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var targetPlatform = "x86";
        var testhostProcessName = new[] { "testhost.x86" };

        // We pass 2 dlls in RunTestWithRunSettings, and MaxCpuCount=1 should win,
        // we should see 1 testhost for .NET Framework (we share the host there),
        // and 2 testhosts for .NET, because we don't share hosts there for non-parallel run.
        var expectedNumOfProcessCreated = runnerInfo.IsNetFrameworkTarget ? 1 : 2;

        // Pass parallel
        var additionalArgs = "/Parallel";

        // Pass non parallel
        var runSettingsArgs = string.Join(
            " ",
            [
                "RunConfiguration.MaxCpuCount=1",
                string.Concat("RunConfiguration.TargetPlatform=",targetPlatform),
                string.Concat("RunConfiguration.TargetFrameworkVersion=" , GetTargetFrameworkForRunsettings()),
                string.Concat("RunConfiguration.TestAdaptersPaths=" , GetTestAdapterPath())
            ]);

        RunTestWithRunSettings(null, runSettingsArgs, additionalArgs, testhostProcessName, expectedNumOfProcessCreated);
    }

    /// <summary>
    /// Command line switches should have high precedence if runsetting file and command line switch specified
    /// </summary>
    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void CommandLineSwitchesShouldWinBetweenSettingsFileAndCommandLineSwitches(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testhostProcessName = new[] { "testhost.x86" };

        // We pass 2 dlls in RunTestWithRunSettings, and MaxCpuCount=1 should win,
        // we should see 1 testhost for .NET Framework (we share the host there),
        // and 2 testhosts for .NET, because we don't share hosts there for non-parallel run.
        var expectedNumOfProcessCreated = runnerInfo.IsNetFrameworkTarget ? 1 : 2;

        // passing different platform
        var runConfigurationDictionary = new Dictionary<string, string>
        {
            { "MaxCpuCount", "1" },
            { "TargetPlatform", "x64" },
            { "TargetFrameworkVersion", GetTargetFrameworkForRunsettings() },
            { "TestAdaptersPaths", GetTestAdapterPath() }
        };
        var additionalArgs = "/Platform:x86";

        RunTestWithRunSettings(runConfigurationDictionary, null, additionalArgs, testhostProcessName, expectedNumOfProcessCreated);
    }

    #endregion

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSettingsWithoutParallelAndPlatformX86(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var targetPlatform = "x86";
        var testhostProcessNames = new[] { "testhost.x86" };

        // We pass 2 dlls in RunTestWithRunSettings, and MaxCpuCount=1 should win,
        // we should see 1 testhost for .NET Framework (we share the host there),
        // and 2 testhosts for .NET, because we don't share hosts there for non-parallel run.
        var expectedNumOfProcessCreated = runnerInfo.IsNetFrameworkTarget ? 1 : 2;

        var runConfigurationDictionary = new Dictionary<string, string>
        {
            { "MaxCpuCount", "1" },
            { "TargetPlatform", targetPlatform },
            { "TargetFrameworkVersion", GetTargetFrameworkForRunsettings() },
            { "TestAdaptersPaths", GetTestAdapterPath() }
        };
        RunTestWithRunSettings(runConfigurationDictionary, null, null, testhostProcessNames, expectedNumOfProcessCreated);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSettingsParamsAsArguments(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var targetPlatform = "x86";
        var testhostProcessName = new[] { "testhost.x86" };

        // We pass 2 dlls in RunTestWithRunSettings, and MaxCpuCount=1 should win,
        // we should see 1 testhost for .NET Framework (we share the host there),
        // and 2 testhosts for .NET, because we don't share hosts there for non-parallel run.
        var expectedNumOfProcessCreated = runnerInfo.IsNetFrameworkTarget ? 1 : 2;

        var runSettingsArgs = string.Join(
            " ",
            [
                "RunConfiguration.MaxCpuCount=1",
                string.Concat("RunConfiguration.TargetPlatform=",targetPlatform),
                string.Concat("RunConfiguration.TargetFrameworkVersion=" , GetTargetFrameworkForRunsettings()),
                string.Concat("RunConfiguration.TestAdaptersPaths=" , GetTestAdapterPath())
            ]);

        RunTestWithRunSettings(null, runSettingsArgs, null, testhostProcessName, expectedNumOfProcessCreated);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSettingsAndRunSettingsParamsAsArguments(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var targetPlatform = "x86";
        var testhostProcessName = new[] { "testhost.x86" };

        // We pass 2 dlls in RunTestWithRunSettings, and MaxCpuCount=1 should win,
        // we should see 1 testhost for .NET Framework (we share the host there),
        // and 2 testhosts for .NET, because we don't share hosts there for non-parallel run.
        var expectedNumOfProcessCreated = runnerInfo.IsNetFrameworkTarget ? 1 : 2;

        var runConfigurationDictionary = new Dictionary<string, string>
        {
            { "MaxCpuCount", "2" },
            { "TargetPlatform", targetPlatform },
            { "TargetFrameworkVersion", GetTargetFrameworkForRunsettings() },
            { "TestAdaptersPaths", GetTestAdapterPath() }
        };

        var runSettingsArgs = string.Join(
            " ",
            [
                "RunConfiguration.MaxCpuCount=1",
                string.Concat("RunConfiguration.TargetPlatform=",targetPlatform),
                string.Concat("RunConfiguration.TargetFrameworkVersion=" , GetTargetFrameworkForRunsettings()),
                string.Concat("RunConfiguration.TestAdaptersPaths=" , GetTestAdapterPath())
            ]);

        RunTestWithRunSettings(runConfigurationDictionary, runSettingsArgs, null, testhostProcessName, expectedNumOfProcessCreated);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void RunSettingsWithParallelAndPlatformX64(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var targetPlatform = "x64";
        var testhostProcessName = new[] { "testhost" };
        var expectedProcessCreated = 2;

        var runConfigurationDictionary = new Dictionary<string, string>
        {
            { "MaxCpuCount", "2" },
            { "TargetPlatform", targetPlatform },
            { "TargetFrameworkVersion", GetTargetFrameworkForRunsettings()},
            { "TestAdaptersPaths", GetTestAdapterPath() }
        };
        RunTestWithRunSettings(runConfigurationDictionary, null, null, testhostProcessName, expectedProcessCreated);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void RunSettingsWithInvalidValueShouldLogError(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var runConfigurationDictionary = new Dictionary<string, string>
        {
            { "TargetPlatform", "123" }
        };
        var runsettingsFilePath = GetRunsettingsFilePath(runConfigurationDictionary, TempDirectory);
        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            string.Empty,
            runsettingsFilePath, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        InvokeVsTest(arguments);
        StdErrorContains(@"Settings file provided does not conform to required format. An error occurred while loading the settings. Error: Invalid setting 'RunConfiguration'. Invalid value '123' specified for 'TargetPlatform'.");
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, inProcess: true)]
    [NetCoreTargetFrameworkDataSource]
    public void TestAdapterPathFromRunSettings(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var runConfigurationDictionary = new Dictionary<string, string>
        {
            { "TestAdaptersPaths", GetTestAdapterPath() }
        };
        var runsettingsFilePath = GetRunsettingsFilePath(runConfigurationDictionary, TempDirectory);
        var arguments = PrepareArguments(
            GetSampleTestAssembly(),
            string.Empty,
            runsettingsFilePath, FrameworkArgValue,
            runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 1, 1);
    }

    #region LegacySettings Tests

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, useCoreRunner: false)]
    public void LegacySettingsWithPlatform(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssemblyPath = GetAssetFullPath("LegacySettingsUnitTestProject.dll");
        _ = Path.GetDirectoryName(testAssemblyPath);

        var runsettingsXml = @"<RunSettings>
                                    <MSTest>
                                    <ForcedLegacyMode>true</ForcedLegacyMode>
                                    </MSTest>
                                    <LegacySettings>
                                      <Execution hostProcessPlatform=""x64"">
                                      </Execution>
                                    </LegacySettings>
                                   </RunSettings>";

        var runsettingsFilePath = GetRunsettingsFilePath(null, TempDirectory);
        File.WriteAllText(runsettingsFilePath, runsettingsXml);

        var arguments = PrepareArguments(
           testAssemblyPath,
           string.Empty,
           runsettingsFilePath, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        InvokeVsTest(arguments);
        ValidateSummaryStatus(0, 0, 0);
    }

    [TestMethod]
    [Ignore("Ignore until we have new host available with CUIT removed.")]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, useCoreRunner: false)]
    public void LegacySettingsWithScripts(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssemblyPath = GetAssetFullPath("LegacySettingsUnitTestProject.dll");

        // Create the script files
        var guid = Guid.NewGuid();
        var setupScriptName = "setupScript_" + guid + ".bat";
        var setupScriptPath = Path.Combine(TempDirectory.Path, setupScriptName);
        File.WriteAllText(setupScriptPath, @"echo > %temp%\ScriptTestingFile.txt");

        var cleanupScriptName = "cleanupScript_" + guid + ".bat";
        var cleanupScriptPath = Path.Combine(TempDirectory.Path, cleanupScriptName);
        File.WriteAllText(cleanupScriptPath, @"del %temp%\ScriptTestingFile.txt");

        var runsettingsFormat = @"<RunSettings>
                                    <MSTest>
                                    <ForcedLegacyMode>true</ForcedLegacyMode>
                                    </MSTest>
                                    <LegacySettings>
                                         <Scripts setupScript=""{0}"" cleanupScript=""{1}"" />
                                    </LegacySettings>
                                   </RunSettings>";

        // Scripts have relative paths to temp directory where the runsettings is created.
        var runsettingsXml = string.Format(CultureInfo.CurrentCulture, runsettingsFormat, setupScriptName, cleanupScriptName);
        var runsettingsPath = GetRunsettingsFilePath(null, TempDirectory);
        File.WriteAllText(runsettingsPath, runsettingsXml);

        var arguments = PrepareArguments(
           testAssemblyPath,
           string.Empty,
           runsettingsPath, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:Name=ScriptsTest");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);

        // Validate cleanup script ran
        var scriptPath = Path.Combine(TempDirectory.Path, "ScriptTestingFile.txt");
        Assert.IsFalse(File.Exists(scriptPath));
    }

    [TestMethod]
    [Ignore("Ignore until we have new host available with CUIT removed.")]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, useCoreRunner: false)]
    public void LegacySettingsWithDeploymentItem(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssemblyPath = GetAssetFullPath("LegacySettingsUnitTestProject.dll");
        var testAssemblyDirectory = Path.GetDirectoryName(testAssemblyPath);
        Assert.IsNotNull(testAssemblyDirectory);

        var deploymentItem = Path.Combine(testAssemblyDirectory, "Deployment", "DeploymentFile.xml");

        var runsettingsFormat = @"<RunSettings>
                                    <MSTest>
                                    <ForcedLegacyMode>true</ForcedLegacyMode>
                                    </MSTest>
                                    <LegacySettings>
                                         <Deployment>
                                            <DeploymentItem filename=""{0}"" />
                                         </Deployment>
                                    </LegacySettings>
                                   </RunSettings>";

        var runsettingsXml = string.Format(CultureInfo.CurrentCulture, runsettingsFormat, deploymentItem);
        var runsettingsPath = GetRunsettingsFilePath(null, TempDirectory);
        File.WriteAllText(runsettingsPath, runsettingsXml);

        var arguments = PrepareArguments(
           testAssemblyPath,
           string.Empty,
           runsettingsPath, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:Name=DeploymentItemTest");
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
    }

    [TestMethod]
    [Ignore("Ignore until we have new host available with CUIT removed.")]
    [TestCategory("Windows")]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, useCoreRunner: false)]
    public void LegacySettingsTestTimeout(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssemblyPath = GetAssetFullPath("LegacySettingsUnitTestProject.dll");
        var runsettingsXml = @"<RunSettings>
                                    <MSTest>
                                    <ForcedLegacyMode>true</ForcedLegacyMode>
                                    </MSTest>
                                    <LegacySettings>
                                        <Execution><Timeouts testTimeout=""2000"" />
                                        </Execution>
                                    </LegacySettings>
                                   </RunSettings>";
        var runsettingsPath = GetRunsettingsFilePath(null, TempDirectory);
        File.WriteAllText(runsettingsPath, runsettingsXml);
        var arguments = PrepareArguments(testAssemblyPath, string.Empty, runsettingsPath, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:Name~TimeTest");

        InvokeVsTest(arguments);

        ValidateSummaryStatus(1, 1, 0);
    }

    [TestMethod]
    [Ignore("Ignore until we have new host available with CUIT removed.")]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSourceAttribute(inIsolation: true, useCoreRunner: false)]
    public void LegacySettingsAssemblyResolution(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssemblyPath = GetAssetFullPath("LegacySettingsUnitTestProject.dll");
        var runsettingsFormat = @"<RunSettings>
                                    <MSTest><ForcedLegacyMode>true</ForcedLegacyMode></MSTest>
                                    <LegacySettings>
                                        <Execution>
                                         <TestTypeSpecific>
                                          <UnitTestRunConfig testTypeId=""13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b"">
                                           <AssemblyResolution>
                                              <TestDirectory useLoadContext=""true"" />
                                              <RuntimeResolution>
                                                  <Directory path=""{0}"" includeSubDirectories=""true"" />
                                              </RuntimeResolution>
                                           </AssemblyResolution>
                                          </UnitTestRunConfig>
                                         </TestTypeSpecific>
                                        </Execution>
                                    </LegacySettings>
                                   </RunSettings>";

        var testAssemblyDirectory = Path.Combine(_testEnvironment.TestAssetsPath, "LegacySettingsUnitTestProject", "DependencyAssembly");
        var runsettingsXml = string.Format(CultureInfo.CurrentCulture, runsettingsFormat, testAssemblyDirectory);
        var runsettingsPath = GetRunsettingsFilePath(null, TempDirectory);
        File.WriteAllText(runsettingsPath, runsettingsXml);
        var arguments = PrepareArguments(testAssemblyPath, string.Empty, runsettingsPath, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, " /testcasefilter:Name=DependencyTest");

        InvokeVsTest(arguments);

        ValidateSummaryStatus(1, 0, 0);
    }

    #endregion

    #region RunSettings With EnvironmentVariables Settings Tests

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void EnvironmentVariablesSettingsShouldSetEnvironmentVariables(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var testAssemblyPath = GetAssetFullPath("EnvironmentVariablesTestProject.dll");

        var runsettingsXml = @"<RunSettings>
                                    <RunConfiguration>
                                      <EnvironmentVariables>
                                        <RANDOM_PATH>C:\temp</RANDOM_PATH>
                                      </EnvironmentVariables>
                                    </RunConfiguration>
                                   </RunSettings>";

        var runsettingsPath = GetRunsettingsFilePath(null, TempDirectory);
        File.WriteAllText(runsettingsPath, runsettingsXml);

        var arguments = PrepareArguments(
           testAssemblyPath,
           string.Empty,
           runsettingsPath, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
    }

    #endregion

    #region RunSettings defined in project file
    /// <summary>
    /// RunSettingsFilePath can be specified in .csproj and should be honored by `dotnet test`, this test
    /// checks that the settings were honored by translating an inconclusive test to failed "result", instead of the default "skipped".
    /// This test depends on Microsoft.TestPlatform.Build\Microsoft.TestPlatform.targets being previously copied into the
    /// artifacts/testArtifacts/dotnet folder. This will allow the local copy of dotnet to pickup the VSTest msbuild task.
    /// </summary>
    /// <param name="runnerInfo"></param>
    [TestMethod]
    // patched dotnet is not published on non-windows systems
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSourceAttribute(useDesktopRunner: false)]
    [NetCoreTargetFrameworkDataSourceAttribute(useDesktopRunner: false)]
    public void RunSettingsAreLoadedFromProject(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var projectName = "ProjectFileRunSettingsTestProject.csproj";
        var projectPath = GetIsolatedTestAsset(projectName);
        InvokeDotnetTest($@"{projectPath} /p:VSTestUseMSBuildOutput=false --logger:""Console;Verbosity=normal"" /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}", workingDirectory: Path.GetDirectoryName(projectPath));
        ValidateSummaryStatus(0, 1, 0);

        // make sure that we can revert the project settings back by providing a config from command line
        // keeping this in the same test, because it is easier to see that we are reverting settings that
        // are honored by dotnet test, instead of just using the default, which would produce the same
        // result
        var settingsPath = GetProjectAssetFullPath(projectName, "inconclusive.runsettings");
        InvokeDotnetTest($@"{projectPath} --settings {settingsPath} /p:VSTestUseMSBuildOutput=false --logger:""Console;Verbosity=normal"" /p:PackageVersion={IntegrationTestEnvironment.LatestLocallyBuiltNugetVersion}", workingDirectory: Path.GetDirectoryName(projectPath));
        ValidateSummaryStatus(0, 0, 1);
    }

    #endregion

    private static string GetRunsettingsFilePath(Dictionary<string, string>? runConfigurationDictionary, TempDirectory tempDirectory)
    {
        var runsettingsPath = Path.Combine(tempDirectory.Path, "test_" + Guid.NewGuid() + ".runsettings");
        if (runConfigurationDictionary != null)
        {
            CreateRunSettingsFile(runsettingsPath, runConfigurationDictionary);
        }

        return runsettingsPath;
    }

    private void RunTestWithRunSettings(Dictionary<string, string>? runConfigurationDictionary,
        string? runSettingsArgs, string? additionalArgs, IEnumerable<string> testhostProcessNames, int expectedNumOfProcessCreated)
    {

        var assemblyPaths = BuildMultipleAssemblyPath("SimpleTestProject.dll", "SimpleTestProject2.dll");

        var runsettingsPath = string.Empty;

        if (runConfigurationDictionary != null)
        {
            runsettingsPath = GetRunsettingsFilePath(runConfigurationDictionary, TempDirectory);
        }

        var arguments = PrepareArguments(assemblyPaths, GetTestAdapterPath(), runsettingsPath, FrameworkArgValue, _testEnvironment.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments += GetDiagArg(TempDirectory.Path);

        if (!string.IsNullOrWhiteSpace(additionalArgs))
        {
            arguments = string.Concat(arguments, " ", additionalArgs);
        }

        if (!string.IsNullOrWhiteSpace(runSettingsArgs))
        {
            arguments = string.Concat(arguments, " -- ", runSettingsArgs);
        }

        InvokeVsTest(arguments);

        // assert
        AssertExpectedNumberOfHostProcesses(expectedNumOfProcessCreated, TempDirectory.Path, testhostProcessNames, arguments, GetConsoleRunnerPath());
        ValidateSummaryStatus(2, 2, 2);

        //cleanup
        if (!string.IsNullOrWhiteSpace(runsettingsPath))
        {
            File.Delete(runsettingsPath);
        }
    }
}
