// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.SettingsMigrator.UnitTests;

[TestClass]
public class MigratorTests
{
    private const string InvalidSettings = "<InvalidSettings>";

    private const string OldRunSettings = "<RunSettings>" +
                                          "<MSTest>" +
                                          "<ForcedLegacyMode>true</ForcedLegacyMode>" +
                                          "<SettingsFile></SettingsFile>" +
                                          "</MSTest>" +
                                          "</RunSettings>";

    private readonly Migrator _migrator;
    private readonly string _newRunsettingsPath;

    private string _oldTestsettingsPath;
    private string _oldRunsettingsPath;

    public MigratorTests()
    {
        _migrator = new Migrator();
        _newRunsettingsPath = Path.Combine(Path.GetTempPath(), "generatedRunsettings.runsettings");
        _oldTestsettingsPath = Path.GetFullPath(Path.Combine(".", "oldTestsettings.testsettings"));
        _oldRunsettingsPath = Path.Combine(Path.GetTempPath(), "oldRunsettings.runsettings");
    }

    [TestCleanup]
    public void TestCleanup()
    {
        if (File.Exists(_newRunsettingsPath))
        {
            File.Delete(_newRunsettingsPath);
        }
    }

    [TestMethod]
    public void NonRootedPathIsNotMigrated()
    {
        _migrator.Migrate("asda", _newRunsettingsPath);

        Assert.IsFalse(File.Exists(_newRunsettingsPath), "Run settings should not be generated.");
    }

    [TestMethod]
    public void MigratorGeneratesCorrectRunsettingsForEmbeddedTestSettings()
    {
        var doc = new XmlDocument();
        doc.LoadXml(OldRunSettings);
        Assert.IsNotNull(doc.DocumentElement);
        var settingsnode = doc.DocumentElement.SelectSingleNode(@"/RunSettings/MSTest/SettingsFile");
        Assert.IsNotNull(settingsnode);
        settingsnode.InnerText = _oldTestsettingsPath;
        File.WriteAllText(_oldRunsettingsPath, doc.InnerXml);

        _migrator.Migrate(_oldRunsettingsPath, _newRunsettingsPath);

        Validate(_newRunsettingsPath);

        File.Delete(_oldRunsettingsPath);
    }

    [TestMethod]
    public void MigratorGeneratesCorrectRunsettingsForEmbeddedTestSettingsOfRelativePath()
    {
        _oldRunsettingsPath = Path.GetFullPath(Path.Combine(".", "oldRunSettingsWithEmbeddedSettings.runSEttings"));

        _migrator.Migrate(_oldRunsettingsPath, _newRunsettingsPath);
        Validate(_newRunsettingsPath);
    }

    [TestMethod]
    public void MigratorGeneratesCorrectRunsettingsWithDc()
    {
        _oldRunsettingsPath = Path.GetFullPath(Path.Combine(".", "oldRunSettingsWithDataCollector.runsettings"));

        _migrator.Migrate(_oldRunsettingsPath, _newRunsettingsPath);

        using XmlTextReader reader = new(_newRunsettingsPath);
        reader.Namespaces = false;
        var document = new XmlDocument();
        document.Load(reader);
        var root = document.DocumentElement;
        Assert.IsNotNull(root);
        var dataCollectorNode = root.SelectNodes(@"/RunSettings/DataCollectionRunSettings/DataCollectors/DataCollector");
        Assert.IsNotNull(dataCollectorNode);
        Assert.AreEqual(2, dataCollectorNode.Count, "Data collector is missing");
    }

    [TestMethod]
    public void MigratorGeneratesCorrectRunsettingsForTestSettings()
    {
        _migrator.Migrate(_oldTestsettingsPath, _newRunsettingsPath);

        Validate(_newRunsettingsPath);
    }

    [TestMethod]
    [ExpectedException(typeof(XmlException))]
    public void InvalidSettingsThrowsException()
    {
        _oldTestsettingsPath = Path.Combine(Path.GetTempPath(), "oldTestsettings.testsettings");

        File.WriteAllText(_oldTestsettingsPath, InvalidSettings);
        File.WriteAllText(_newRunsettingsPath, string.Empty);

        _migrator.Migrate(_oldTestsettingsPath, _newRunsettingsPath);

        File.Delete(_oldTestsettingsPath);
    }

    [TestMethod]
    // On some systems this throws file not found, on some it throws directory not found,
    // I don't know why and it does not matter for the test. As long as it throws.
    [ExpectedException(typeof(IOException), AllowDerivedTypes = true)]
    public void InvalidPathThrowsException()
    {
        string oldTestsettingsPath = @"X:\generatedRun,settings.runsettings";

        _migrator.Migrate(oldTestsettingsPath, _newRunsettingsPath);
    }

    private static void Validate(string newRunsettingsPath)
    {
        Assert.IsTrue(File.Exists(newRunsettingsPath), "Run settings should be generated.");

        using XmlTextReader reader = new(newRunsettingsPath);
        reader.Namespaces = false;

        var document = new XmlDocument();
        document.Load(reader);
        var root = document.DocumentElement;
        Assert.IsNotNull(root);

        Assert.IsNotNull(root.SelectSingleNode(@"/RunSettings/WebTestRunConfiguration/Browser/Headers/Header"), "There should be a WebTestRunConfiguration node");
        Assert.IsNotNull(root.SelectSingleNode(@"/RunSettings/LegacySettings"), "There should be a LegacySettings node");
        Assert.IsNotNull(root.SelectSingleNode(@"/RunSettings/LegacySettings/Deployment/DeploymentItem"), "There should be a DeploymentItem node");

        var scriptNode = root.SelectSingleNode(@"/RunSettings/LegacySettings/Scripts");
        Assert.IsNotNull(scriptNode, "There should be a WebTestRunConfiguration node");
        Assert.IsNotNull(scriptNode.Attributes);
        Assert.AreEqual(".\\setup.bat", scriptNode.Attributes["setupScript"]!.Value, "setupScript does not match.");
        Assert.AreEqual(".\\cleanup.bat", scriptNode.Attributes["cleanupScript"]!.Value, "cleanupScript does not match.");

        var forcedLegacyNode = root.SelectSingleNode(@"/RunSettings/MSTest/ForcedLegacyMode");
        Assert.IsNotNull(forcedLegacyNode, "ForcedLegacy node should be present");
        Assert.AreEqual("true", forcedLegacyNode.InnerText, "Forced legacy should be true");

        var executionNode = root.SelectSingleNode(@" / RunSettings/LegacySettings/Execution");
        Assert.IsNotNull(executionNode, "There should be a Execution node");
        Assert.IsNotNull(executionNode.Attributes);
        Assert.AreEqual("2", executionNode.Attributes["parallelTestCount"]!.Value, "parallelTestCount value does not match.");
        Assert.AreEqual("MSIL", executionNode.Attributes["hostProcessPlatform"]!.Value, "hostProcessPlatform value does not match.");

        Assert.IsNotNull(root.SelectSingleNode(@"/RunSettings/LegacySettings/Execution/Hosts"), "There should be a Hosts node");

        var timeoutNode = root.SelectSingleNode(@"/RunSettings/LegacySettings/Execution/Timeouts");
        Assert.IsNotNull(timeoutNode, "There should be a Timeouts node");
        Assert.IsNotNull(timeoutNode.Attributes);
        Assert.AreEqual("120000", timeoutNode.Attributes["testTimeout"]!.Value, "testTimeout value does not match.");

        Assert.IsNotNull(root.SelectSingleNode(@"/RunSettings/LegacySettings/Execution/TestTypeSpecific/UnitTestRunConfig/AssemblyResolution/TestDirectory"), "There should be a Assembly resolution node");

        var testSessionTimeoutNode = root.SelectSingleNode(@"/RunSettings/RunConfiguration/TestSessionTimeout");
        Assert.IsNotNull(testSessionTimeoutNode, "There should be a TestSessionTimeout node");
        Assert.AreEqual("60000", testSessionTimeoutNode.InnerText, "Timeout value does not match.");

        var dataCollectorNode = root.SelectSingleNode(@"/RunSettings/DataCollectionRunSettings/DataCollectors/DataCollector");
        Assert.IsNotNull(dataCollectorNode, "There should be a DataCollector node");
        Assert.IsNotNull(dataCollectorNode.Attributes);
        Assert.AreEqual("Event Log", dataCollectorNode.Attributes["friendlyName"]!.Value, "Data collector does not match.");
    }
}
