// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using vstest.console.UnitTests.Processors;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.UnitTests.Processors;

[TestClass]
public class EnvironmentArgumentProcessorTests
{
    private const string DefaultRunSettings =
        "<?xml version=\"1.0\" encoding=\"utf-16\"?><RunSettings></RunSettings>";

    private readonly TestableRunSettingsProvider _settingsProvider;
    private readonly Mock<IOutput> _mockOutput;
    private readonly CommandLineOptions _commandLineOptions;

    public EnvironmentArgumentProcessorTests()
    {
        _commandLineOptions = CommandLineOptions.Instance;
        _settingsProvider = new TestableRunSettingsProvider();
        _settingsProvider.UpdateRunSettings(DefaultRunSettings);
        _mockOutput = new Mock<IOutput>();
    }

    [TestCleanup]
    public void Cleanup()
    {
        CommandLineOptions.Reset();
    }

    [TestMethod]
    public void CapabilitiesShouldReturnAppropriateValues()
    {
        // Arrange & Act
        var capabilities = new EnvironmentArgumentProcessor.ArgumentProcessorCapabilities();

        // Assert
        Assert.AreEqual("/Environment", capabilities.CommandName);
        Assert.AreEqual("/e", capabilities.ShortCommandName);
        Assert.IsTrue(capabilities.AllowMultiple);
        Assert.IsFalse(capabilities.IsAction);
        Assert.AreEqual(ArgumentProcessorPriority.Normal, capabilities.Priority);
        Assert.AreEqual(HelpContentPriority.EnvironmentArgumentProcessorHelpPriority, capabilities.HelpPriority);
    }

    [TestMethod]
    public void AppendsEnvironmentVariableToRunSettings()
    {
        // Arrange
        var executor = GetExecutor();

        // Act
        executor.Initialize("VARIABLE=VALUE");

        // Assert
        var result = ParseSettingsXml(_settingsProvider);
        var environmentVariables = result.Variables;
        var inIsolation = result.InIsolation;

        Assert.IsNotNull(environmentVariables, "Environment variable cannot found in RunSettings.xml.");
        Assert.IsNotNull(inIsolation, "Isolation must be forced, an InIsolation entry was missing!");

        var variables = environmentVariables.Elements().ToArray();
        Assert.AreEqual(1, variables.Length, "Environment variable count mismatched!");

        Assert.AreEqual("true", inIsolation.Value, "Isolation must be forced, InIsolation is not set to true.");
        Assert.AreEqual("VARIABLE", variables[0].Name.LocalName);
        Assert.AreEqual("VALUE", variables[0].Value);

    }

    [TestMethod]
    public void AppendsMultipleEnvironmentVariablesToRunSettings()
    {
        // Arrange
        var executor1 = GetExecutor();
        var executor2 = GetExecutor();
        var executor3 = GetExecutor();

        // Act
        executor1.Initialize("VARIABLE_ONE=VALUE");
        executor2.Initialize("VARIABLE_TWO=VALUE WITH SPACE");
        executor3.Initialize("VARIABLE_THREE=VALUE WITH SPACE;AND SEMICOLON");

        // Assert
        var result = ParseSettingsXml(_settingsProvider);
        var environmentVariables = result.Variables;
        var inIsolation = result.InIsolation;

        Assert.IsNotNull(environmentVariables, "Environment variable cannot found in RunSettings.xml.");
        Assert.IsNotNull(inIsolation, "Isolation must be forced, an InIsolation entry was missing!");

        Assert.AreEqual("true", inIsolation.Value, "Isolation must be forced, InIsolation is not set to true.");
        var variables = environmentVariables.Elements().ToArray();
        Assert.AreEqual(3, variables.Length, "Environment variable count mismatched!");

        Assert.AreEqual("VARIABLE_ONE", variables[0].Name.LocalName);
        Assert.AreEqual("VALUE", variables[0].Value);

        Assert.AreEqual("VARIABLE_TWO", variables[1].Name.LocalName);
        Assert.AreEqual("VALUE WITH SPACE", variables[1].Value);

        Assert.AreEqual("VARIABLE_THREE", variables[2].Name.LocalName);
        Assert.AreEqual("VALUE WITH SPACE;AND SEMICOLON", variables[2].Value);
    }

    [TestMethod]
    public void InIsolationValueShouldBeOverriden()
    {
        // Arrange
        _commandLineOptions.InIsolation = false;
        _settingsProvider.UpdateRunSettingsNode(InIsolationArgumentExecutor.RunSettingsPath, "false");
        var executor = GetExecutor();

        // Act
        executor.Initialize("VARIABLE=VALUE");

        // Assert
        var result = ParseSettingsXml(_settingsProvider);
        var environmentVariables = result.Variables;
        var inIsolation = result.InIsolation;

        Assert.IsNotNull(environmentVariables, "Environment variable cannot found in RunSettings.xml.");
        Assert.IsNotNull(inIsolation, "Isolation must be forced, an InIsolation entry was missing!");

        Assert.AreEqual("true", inIsolation.Value, "Isolation must be forced, InIsolation is overriden to true.");
        var variables = environmentVariables.Elements().ToArray();
        Assert.AreEqual(1, variables.Length, "Environment variable count mismatched!");

        Assert.AreEqual("VARIABLE", variables[0].Name.LocalName);
        Assert.AreEqual("VALUE", variables[0].Value);
    }

    [TestMethod]
    public void ShoudWarnWhenAValueIsOverriden()
    {
        // Arrange
        _settingsProvider.UpdateRunSettingsNode("RunConfiguration.EnvironmentVariables.VARIABLE",
            "Initial value");
        var warningMessage = string.Format(CultureInfo.CurrentCulture, CommandLineResources.CommandLineWarning,
            string.Format(CultureInfo.CurrentCulture, CommandLineResources.EnvironmentVariableXIsOverriden, "VARIABLE")
        );
        _mockOutput.Setup(mock =>
            mock.WriteLine(
                It.Is<string>(message => message == warningMessage),
                It.Is<OutputLevel>(level => level == OutputLevel.Warning)
            )
        ).Verifiable();
        var executor = GetExecutor();

        // Act
        executor.Initialize("VARIABLE=New value");

        // Assert
        _mockOutput.VerifyAll();
    }

    private static XmlParseResult ParseSettingsXml(IRunSettingsProvider provider)
    {
        var document = XDocument.Parse(provider.ActiveRunSettings!.SettingsXml!);

        var runConfiguration = document
            ?.Root
            ?.Element("RunConfiguration");

        var variables = runConfiguration?.Element("EnvironmentVariables");
        var inIsolation = runConfiguration?.Element("InIsolation");

        return new XmlParseResult(variables, inIsolation);
    }

    private IArgumentExecutor GetExecutor()
    {
        return new EnvironmentArgumentProcessor.ArgumentExecutor(
            _commandLineOptions,
            _settingsProvider,
            _mockOutput.Object
        );
    }

    private class XmlParseResult
    {
        public XmlParseResult(XElement? variables, XElement? inIsolation)
        {
            Variables = variables;
            InIsolation = inIsolation;
        }

        internal readonly XElement? Variables;
        internal readonly XElement? InIsolation;
    }
}
