// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.CommandLine;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace vstest.console.UnitTests.Processors;

[TestClass]
public class CliRunSettingsArgumentProcessorTests
{
    private readonly TestableRunSettingsProvider _settingsProvider;
    private readonly CliRunSettingsArgumentExecutor _executor;
    private readonly CommandLineOptions _commandLineOptions;
    private readonly string _defaultRunSettings = string.Join(Environment.NewLine,
        "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
        "<RunSettings>",
        "  <DataCollectionRunSettings>",
        "    <DataCollectors />",
        "  </DataCollectionRunSettings>",
        "</RunSettings>");

    private readonly string _runSettingsWithDeploymentDisabled = string.Join(Environment.NewLine,
        "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
        "<RunSettings>",
        "  <DataCollectionRunSettings>",
        "    <DataCollectors />",
        "  </DataCollectionRunSettings>",
        "  <MSTest>",
        "    <DeploymentEnabled>False</DeploymentEnabled>",
        "  </MSTest>",
        "</RunSettings>");

    private readonly string _runSettingsWithDeploymentEnabled = string.Join(Environment.NewLine,
        "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
        "<RunSettings>",
        "  <DataCollectionRunSettings>",
        "    <DataCollectors />",
        "  </DataCollectionRunSettings>",
        "  <MSTest>",
        "    <DeploymentEnabled>True</DeploymentEnabled>",
        "  </MSTest>",
        "</RunSettings>");

    public CliRunSettingsArgumentProcessorTests()
    {
        _commandLineOptions = CommandLineOptions.Instance;
        _settingsProvider = new TestableRunSettingsProvider();
        _executor = new CliRunSettingsArgumentExecutor(_settingsProvider, _commandLineOptions);
    }

    [TestCleanup]
    public void Cleanup()
    {
        CommandLineOptions.Reset();
    }

    [TestMethod]
    public void GetMetadataShouldReturnRunSettingsArgumentProcessorCapabilities()
    {
        var processor = new CliRunSettingsArgumentProcessor();
        Assert.IsTrue(processor.Metadata.Value is CliRunSettingsArgumentProcessorCapabilities);
    }

    [TestMethod]
    public void GetExecuterShouldReturnRunSettingsArgumentProcessorCapabilities()
    {
        var processor = new CliRunSettingsArgumentProcessor();
        Assert.IsTrue(processor.Executor!.Value is CliRunSettingsArgumentExecutor);
    }

    #region CLIRunSettingsArgumentProcessorCapabilities tests

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateProperties()
    {
        var capabilities = new CliRunSettingsArgumentProcessorCapabilities();

        Assert.AreEqual("--", capabilities.CommandName);
        var expected = "RunSettings arguments:\r\n      Arguments to pass runsettings configurations through commandline. Arguments may be specified as name-value pair of the form [name]=[value] after \"-- \". Note the space after --. \r\n      Use a space to separate multiple [name]=[value].\r\n      More info on RunSettings arguments support: https://aka.ms/vstest-runsettings-arguments";
        Assert.AreEqual(expected.NormalizeLineEndings().ShowWhiteSpace(), capabilities.HelpContentResourceName.NormalizeLineEndings().ShowWhiteSpace());

        Assert.AreEqual(HelpContentPriority.CliRunSettingsArgumentProcessorHelpPriority, capabilities.HelpPriority);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.CliRunSettings, capabilities.Priority);

        Assert.IsFalse(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.AlwaysExecute);
        Assert.IsFalse(capabilities.IsSpecialCommand);
    }

    #endregion

    #region CLIRunSettingsArgumentExecutor tests

    [TestMethod]
    public void InitializeShouldNotThrowExceptionIfArgumentIsNull()
    {
        _executor.Initialize((string[]?)null);

        Assert.IsNull(_settingsProvider.ActiveRunSettings);
    }

    [TestMethod]
    public void InitializeShouldNotThrowExceptionIfArgumentIsEmpty()
    {
        _executor.Initialize(Array.Empty<string>());

        Assert.IsNull(_settingsProvider.ActiveRunSettings);
    }

    [TestMethod]
    public void InitializeShouldCreateEmptyRunSettingsIfArgumentsHasOnlyWhiteSpace()
    {
        _executor.Initialize(new string[] { " " });

        Assert.IsNull(_settingsProvider.ActiveRunSettings);
    }

    [TestMethod]
    public void InitializeShouldSetValueInRunSettings()
    {
        var args = new string[] { "MSTest.DeploymentEnabled=False" };

        _executor.Initialize(args);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(_runSettingsWithDeploymentDisabled, _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldIgnoreKeyIfValueIsNotPassed()
    {
        var args = new string[] { "MSTest.DeploymentEnabled=False", "MSTest1" };

        _executor.Initialize(args);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(_runSettingsWithDeploymentDisabled, _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [DataRow("Testameter.Parameter(name=\"asf\",value=\"rgq\")")]
    [DataRow("TestRunParameter.Parameter(name=\"asf\",value=\"rgq\")")]
    [TestMethod]
    public void InitializeShouldThrowErrorIfArgumentIsInValid(string arg)
    {
        var args = new string[] { arg };
        var str = CommandLineResources.MalformedRunSettingsKey;

        CommandLineException ex = Assert.ThrowsException<CommandLineException>(() => _executor.Initialize(args));

        Assert.AreEqual(str, ex.Message);
    }

    [TestMethod]
    public void InitializeShouldIgnoreWhiteSpaceInBeginningOrEndOfKey()
    {
        var args = new string[] { " MSTest.DeploymentEnabled =False" };

        _executor.Initialize(args);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(_runSettingsWithDeploymentDisabled, _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldIgnoreThrowExceptionIfKeyHasWhiteSpace()
    {
        var args = new string[] { "MST est.DeploymentEnabled=False" };

        Action action = () => _executor.Initialize(args);

        ExceptionUtilities.ThrowsException<CommandLineException>(
            action,
            "One or more runsettings provided contain invalid token");
    }

    [TestMethod]
    public void InitializeShouldEncodeXmlIfInvalidXmlCharsArePresent()
    {
        var args = new string[] { "MSTest.DeploymentEnabled=F>a><l<se" };

        _executor.Initialize(args);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine, "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
            "<RunSettings>",
            "  <DataCollectionRunSettings>",
            "    <DataCollectors />",
            "  </DataCollectionRunSettings>",
            "  <MSTest>",
            "    <DeploymentEnabled>F&gt;a&gt;&lt;l&lt;se</DeploymentEnabled>",
            "  </MSTest>",
            "</RunSettings>"), _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldIgnoreIfKeyIsNotPassed()
    {
        var args = new string[] { "MSTest.DeploymentEnabled=False", "=value" };

        _executor.Initialize(args);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(_runSettingsWithDeploymentDisabled, _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldIgnoreIfEmptyValueIsPassed()
    {

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runSettings);

        var args = new string[] { "MSTest.DeploymentEnabled=" };
        _executor.Initialize(args);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(_defaultRunSettings, _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldOverwriteValueIfNodeAlreadyExists()
    {

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runSettings);

        var args = new string[] { "MSTest.DeploymentEnabled=True" };
        _executor.Initialize(args);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(_runSettingsWithDeploymentEnabled, _settingsProvider.ActiveRunSettings.SettingsXml);
    }


    [TestMethod]
    public void InitializeShouldOverwriteValueIfWhitSpaceIsPassedAndNodeAlreadyExists()
    {

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runSettings);

        var args = new string[] { "MSTest.DeploymentEnabled= " };
        _executor.Initialize(args);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(string.Join(Environment.NewLine, "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
            "<RunSettings>",
            "  <DataCollectionRunSettings>",
            "    <DataCollectors />",
            "  </DataCollectionRunSettings>",
            "  <MSTest>",
            "    <DeploymentEnabled>",
            "    </DeploymentEnabled>",
            "  </MSTest>",
            "</RunSettings>"), _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [TestMethod]
    public void InitializeShouldUpdateCommandLineOptionsFrameworkIfProvided()
    {

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runSettings);

        var args = new string[] { $"RunConfiguration.TargetFrameworkVersion={Constants.DotNetFramework46}" };
        _executor.Initialize(args);

        Assert.IsTrue(_commandLineOptions.FrameworkVersionSpecified);
        Assert.AreEqual(Constants.DotNetFramework46, _commandLineOptions.TargetFrameworkVersion.Name);
    }

    [TestMethod]
    public void InitializeShouldUpdateCommandLineOptionsArchitectureIfProvided()
    {

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runSettings);

        var args = new string[] { $"RunConfiguration.TargetPlatform={nameof(Architecture.ARM)}" };
        _executor.Initialize(args);

        Assert.IsTrue(_commandLineOptions.ArchitectureSpecified);
        Assert.AreEqual(Architecture.ARM, _commandLineOptions.TargetArchitecture);
    }

    [TestMethod]
    public void InitializeShouldNotUpdateCommandLineOptionsArchitectureAndFxIfNotProvided()
    {

        var runSettings = new RunSettings();
        runSettings.LoadSettingsXml(_defaultRunSettings);
        _settingsProvider.SetActiveRunSettings(runSettings);

        var args = Array.Empty<string>();
        _executor.Initialize(args);

        Assert.IsFalse(_commandLineOptions.ArchitectureSpecified);
        Assert.IsFalse(_commandLineOptions.FrameworkVersionSpecified);
    }

    [DynamicData(nameof(TestRunParameterArgValidTestCases), DynamicDataSourceType.Method)]
    [DataTestMethod]
    public void InitializeShouldValidateTestRunParameter(string arg, string runSettingsWithTestRunParameters)
    {
        var args = new string[] { arg };

        _executor.Initialize(args);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(runSettingsWithTestRunParameters, _settingsProvider.ActiveRunSettings.SettingsXml);
    }

    [DynamicData(nameof(TestRunParameterArgInvalidTestCases), DynamicDataSourceType.Method)]
    [DataTestMethod]
    public void InitializeShouldThrowErrorIfTestRunParameterNodeIsInValid(string arg)
    {
        var args = new string[] { arg };
        var str = string.Format(CultureInfo.CurrentCulture, CommandLineResources.InvalidTestRunParameterArgument, arg);

        CommandLineException ex = Assert.ThrowsException<CommandLineException>(() => _executor.Initialize(args));

        Assert.AreEqual(str, ex.Message);
    }

    public static IEnumerable<object[]> TestRunParameterArgInvalidTestCases()
    {
        return InvalidTestCases;
    }

    private static readonly List<object[]> InvalidTestCases = new()
    {
        new object[] { "TestRunParameters.Parameter(name=asf,value=rgq)" },
        new object[] { "TestRunParameters.Parameter(name=\"asf\",value=\"rgq\" )" },
        new object[] { "TestRunParameters.Parameter( name=\"asf\",value=\"rgq\")" },
        new object[] { "TestRunParametersParameter(name=\"asf\",value=\"rgq\")" },
        new object[] { "TestRunParameters.Paramete(name=\"asf\",value=\"rgq\")" },
        new object[] { "TestRunParameters.Parametername=\"asf\",value=\"rgq\")" },
        new object[] { "TestRunParameters.Parameter(ame=\"asf\",value=\"rgq\")" },
        new object[] { "TestRunParameters.Parameter(name\"asf\",value=\"rgq\")" },
        new object[] { "TestRunParameters.Parameter(name=\"asf\" value=\"rgq\")" },
        new object[] { "TestRunParameters.Parameter(name=\"asf\",alue=\"rgq\")" },
        new object[] { "TestRunParameters.Parameter(name=\"asf\",value\"rgq\")" },
        new object[] { "TestRunParameters.Parameter(name=\"asf\",value=\"rgq\"" },
        new object[] { "TestRunParameters.Parameter(name=\"asf\",value=\"rgq\")wfds" },
        new object[] { "TestRunParameters.Parameter(name=\"\",value=\"rgq\")" },
        new object[] { "TestRunParameters.Parameter(name=\"asf\",value=\"\")" },
        new object[] { "TestRunParameters.Parameter(name=asf\",value=\"rgq\")" },
        new object[] { "TestRunParameters.Parameter(name=\"asf,value=\"rgq\")" },
        new object[] { "TestRunParameters.Parameter(name=\"asf\",value=rgq\")" },
        new object[] { "TestRunParameters.Parameter(name=\"asf\",value=\"rgq)" },
        new object[] { "TestRunParameters.Parameter(name=\"asf@#!\",value=\"rgq\")" },
        new object[] { "TestRunParameters.Parameter(name=\"\",value=\"fgf\")" },
        new object[] { "TestRunParameters.Parameter(name=\"gag\",value=\"\")" },
        new object[] { "TestRunParameters.Parameter(name=\"gag\")" }
    };

    public static IEnumerable<object[]> TestRunParameterArgValidTestCases()
    {
        return ValidTestCases;
    }

    private static readonly List<object[]> ValidTestCases = new()
    {
        new object[] { "TestRunParameters.Parameter(name=\"weburl\",value=\"&><\")" ,
            string.Join(Environment.NewLine, "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors />",
                "  </DataCollectionRunSettings>",
                "  <TestRunParameters>",
                "    <Parameter name=\"weburl\" value=\"&amp;&gt;&lt;\" />",
                "  </TestRunParameters>",
                "</RunSettings>")
        },
        new object[] { "TestRunParameters.Parameter(name=\"weburl\",value=\"http://localhost//abc\")" ,
            string.Join(Environment.NewLine, "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors />",
                "  </DataCollectionRunSettings>",
                "  <TestRunParameters>",
                "    <Parameter name=\"weburl\" value=\"http://localhost//abc\" />",
                "  </TestRunParameters>",
                "</RunSettings>")
        },
        new object[] { "TestRunParameters.Parameter(name= \"a_sf123_12\",value= \"2324346a!@#$%^*()_+-=':;.,/?{}[]|\")" ,
            string.Join(Environment.NewLine, "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors />",
                "  </DataCollectionRunSettings>",
                "  <TestRunParameters>",
                "    <Parameter name=\"a_sf123_12\" value=\"2324346a!@#$%^*()_+-=':;.,/?{}[]|\" />",
                "  </TestRunParameters>",
                "</RunSettings>")
        },
        new object[] { "TestRunParameters.Parameter(name = \"weburl\" , value = \"http://localhost//abc\")" ,
            string.Join(Environment.NewLine, "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                "<RunSettings>",
                "  <DataCollectionRunSettings>",
                "    <DataCollectors />",
                "  </DataCollectionRunSettings>",
                "  <TestRunParameters>",
                "    <Parameter name=\"weburl\" value=\"http://localhost//abc\" />",
                "  </TestRunParameters>",
                "</RunSettings>")
        },
    };
    #endregion

    [TestMethod]
    public void InitializeShouldMergeTestRunParametersWithSpaces()
    {
        // in powershell call: ConsoleApp1.exe --% --TestRunParameters.Parameter(name =\"myParam\", value=\"myValue\")
        // args:
        //--
        //TestRunParameters.Parameter(name = "myParam",
        //value = "myValue")

        // in cmd: ConsoleApp1.exe -- TestRunParameters.Parameter(name=\"myParam\", value=\"myValue\")
        // args:
        //--
        //TestRunParameters.Parameter(name = "myParam",
        //value = "myValue")

        // in ubuntu wsl without escaping the space: ConsoleApp1.exe-- TestRunParameters.Parameter\(name =\"myParam\", value=\"myValue\"\)
        // args:
        //--
        //TestRunParameters.Parameter(name = "myParam",
        //value = "myValue")

        // in ubuntu wsl with escaping the space: ConsoleApp1.exe-- TestRunParameters.Parameter\(name =\"myParam\",\ value=\"myValue\"\)
        // args:
        //--
        //TestRunParameters.Parameter(name = "myParam", value = "myValue")

        var args = new string[] {
            "--",
            "TestRunParameters.Parameter(name=\"myParam\",",
            "value=\"myValue\")",
            "TestRunParameters.Parameter(name=\"myParam2\",",
            "value=\"myValue 2\")",
        };

        var runsettings = string.Join(Environment.NewLine, new[]{
            "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
            "<RunSettings>",
            "  <DataCollectionRunSettings>",
            "    <DataCollectors />",
            "  </DataCollectionRunSettings>",
            "  <TestRunParameters>",
            "    <Parameter name=\"myParam\" value=\"myValue\" />",
            "    <Parameter name=\"myParam2\" value=\"myValue 2\" />",
            "  </TestRunParameters>",
            "</RunSettings>"});

        _executor.Initialize(args);

        Assert.IsNotNull(_settingsProvider.ActiveRunSettings);
        Assert.AreEqual(runsettings, _settingsProvider.ActiveRunSettings.SettingsXml);
    }
}
