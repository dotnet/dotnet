// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

# nullable disable

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class PostProcessingTests : AcceptanceTestBase
{
    [TestMethod]
    public void DotnetSDKSimulation_PostProcessing()
    {
        var extensionsPath = Path.GetDirectoryName(GetTestDllForFramework("AttachmentProcessorDataCollector.dll", "netstandard2.0"));
        _testEnvironment.RunnerFramework = CoreRunnerFramework;

        string runSettings = GetRunsettingsFilePath(TempDirectory.Path);
        string correlationSessionId = Guid.NewGuid().ToString();

        // Set datacollector parameters
        XElement runSettingsXml = XElement.Load(runSettings);
        runSettingsXml.Element("DataCollectionRunSettings")
            .Element("DataCollectors")
            .Element("DataCollector")
            .Add(new XElement("Configuration", new XElement("MergeFile", "MergedFile.txt")));
        runSettingsXml.Save(runSettings);

        // Build and run tests like msbuild
        Parallel.For(0, 5, i =>
        {
            string projectFolder = Path.Combine(TempDirectory.Path, i.ToString(CultureInfo.InvariantCulture));
            ExecuteApplication(GetConsoleRunnerPath(), $"new mstest -o {projectFolder}", out string stdOut, out string stdError, out int exitCode);
            Assert.AreEqual(exitCode, 0);
            ExecuteApplication(GetConsoleRunnerPath(), $"build {projectFolder} -c release", out stdOut, out stdError, out exitCode);
            Assert.AreEqual(exitCode, 0);

            string testContainer = Directory.GetFiles(Path.Combine(projectFolder, "bin"), $"{i}.dll", SearchOption.AllDirectories).Single();

            ExecuteVsTestConsole($"{testContainer} --Collect:\"SampleDataCollector\" --TestAdapterPath:\"{extensionsPath}\" --ResultsDirectory:\"{Path.GetDirectoryName(testContainer)}\" --Settings:\"{runSettings}\" --ArtifactsProcessingMode-Collect --TestSessionCorrelationId:\"{correlationSessionId}\" --Diag:\"{TempDirectory.Path + '/'}\"", out stdOut, out stdError, out exitCode);
            Assert.AreEqual(exitCode, 0);
        });

        // Post process artifacts
        ExecuteVsTestConsole($"--ArtifactsProcessingMode-PostProcess --TestSessionCorrelationId:\"{correlationSessionId}\" --Diag:\"{TempDirectory.Path + "/mergeLog/"}\"", out string stdOut, out string stdError, out int exitCode);
        Assert.AreEqual(exitCode, 0);

        using StringReader reader = new(stdOut);
        Assert.AreEqual(string.Empty, reader.ReadLine().Trim());
        Assert.AreEqual("Attachments:", reader.ReadLine().Trim());
        string mergedFile = reader.ReadLine().Trim();
        Assert.AreEqual(string.Empty, reader.ReadLine().Trim());
        Assert.IsNull(reader.ReadLine());

        var fileContent = new List<string>();
        using var streamReader = new StreamReader(mergedFile);
        while (!streamReader.EndOfStream)
        {
            string line = streamReader.ReadLine();
            Assert.IsTrue(line.StartsWith("SessionEnded_Handler_"));
            fileContent.Add(line);
        }

        Assert.AreEqual(5, fileContent.Distinct().Count());
    }

    private static string GetRunsettingsFilePath(string resultsDir)
    {
        var runsettingsPath = Path.Combine(resultsDir, "test_" + Guid.NewGuid() + ".runsettings");
        var dataCollectionAttributes = new Dictionary<string, string>
        {
            { "friendlyName", "SampleDataCollector" },
            { "uri", "my://sample/datacollector" }
        };

        CreateDataCollectionRunSettingsFile(runsettingsPath, dataCollectionAttributes);
        return runsettingsPath;
    }

    private static void CreateDataCollectionRunSettingsFile(string destinationRunsettingsPath, Dictionary<string, string> dataCollectionAttributes)
    {
        var doc = new XmlDocument();
        var xmlDeclaration = doc.CreateNode(XmlNodeType.XmlDeclaration, string.Empty, string.Empty);

        doc.AppendChild(xmlDeclaration);
        var runSettingsNode = doc.CreateElement(Constants.RunSettingsName);
        doc.AppendChild(runSettingsNode);
        var dcConfigNode = doc.CreateElement(Constants.DataCollectionRunSettingsName);
        runSettingsNode.AppendChild(dcConfigNode);
        var dataCollectorsNode = doc.CreateElement(Constants.DataCollectorsSettingName);
        dcConfigNode.AppendChild(dataCollectorsNode);
        var dataCollectorNode = doc.CreateElement(Constants.DataCollectorSettingName);
        dataCollectorsNode.AppendChild(dataCollectorNode);

        foreach (var kvp in dataCollectionAttributes)
        {
            dataCollectorNode.SetAttribute(kvp.Key, kvp.Value);
        }

        using var stream = new FileHelper().GetStream(destinationRunsettingsPath, FileMode.Create);
        doc.Save(stream);
    }
}
