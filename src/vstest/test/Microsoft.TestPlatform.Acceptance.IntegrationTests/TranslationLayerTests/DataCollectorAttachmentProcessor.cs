// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests.TranslationLayerTests;

[TestClass]
[TestCategory("Windows-Review")]
public class DataCollectorAttachmentProcessor : AcceptanceTestBase
{
    private readonly IVsTestConsoleWrapper _vstestConsoleWrapper;
    private readonly RunEventHandler _runEventHandler;
    private readonly TestRunAttachmentsProcessingEventHandler _testRunAttachmentsProcessingEventHandler;

    public DataCollectorAttachmentProcessor()
    {
        _vstestConsoleWrapper = GetVsTestConsoleWrapper();
        _runEventHandler = new RunEventHandler();
        _testRunAttachmentsProcessingEventHandler = new TestRunAttachmentsProcessingEventHandler();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _vstestConsoleWrapper?.EndSession();
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public async Task AttachmentProcessorDataCollector_ExtensionFileNotLocked(RunnerInfo runnerInfo)
    {
        // arrange
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var originalExtensionsPath = Path.GetDirectoryName(GetTestDllForFramework("AttachmentProcessorDataCollector.dll", "netstandard2.0"));

        string extensionPath = Path.Combine(TempDirectory.Path, "AttachmentProcessorDataCollector");
        Directory.CreateDirectory(extensionPath);
        TempDirectory.CopyDirectory(new DirectoryInfo(originalExtensionsPath!), new DirectoryInfo(extensionPath));

        string runSettings = GetRunsettingsFilePath(TempDirectory.Path);
        XElement runSettingsXml = XElement.Load(runSettings);
        runSettingsXml.Add(new XElement("RunConfiguration", new XElement("TestAdaptersPaths", extensionPath)));
        // Set datacollector parameters
        runSettingsXml!.Element("DataCollectionRunSettings")!
            .Element("DataCollectors")!
            .Element("DataCollector")!
            .Add(new XElement("Configuration", new XElement("MergeFile", "MergedFile.txt")));
        runSettingsXml.Save(runSettings);

        // act
        _vstestConsoleWrapper.RunTests(GetTestAssemblies(), File.ReadAllText(runSettings), new TestPlatformOptions(), _runEventHandler);
        _vstestConsoleWrapper.RunTests(GetTestAssemblies(), File.ReadAllText(runSettings), new TestPlatformOptions(), _runEventHandler);
        await _vstestConsoleWrapper.ProcessTestRunAttachmentsAsync(_runEventHandler.Attachments, _runEventHandler.InvokedDataCollectors, File.ReadAllText(runSettings), true, false, _testRunAttachmentsProcessingEventHandler, CancellationToken.None);

        // assert
        // Extension path is not locked, we can remove it.
        Directory.Delete(extensionPath, true);

        // Ensure we ran the extension.
        using var logFile = new FileStream(Path.Combine(TempDirectory.Path, "log.txt"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(logFile);
        string logFileContent = streamReader.ReadToEnd();
        Assert.IsTrue(Regex.IsMatch(logFileContent, $@"DataCollectorAttachmentsProcessorsFactory: Collector attachment processor 'AttachmentProcessorDataCollector\.SampleDataCollectorAttachmentProcessor, AttachmentProcessorDataCollector, Version=.*, Culture=neutral, PublicKeyToken=null' from file '{extensionPath.Replace(@"\", @"\\")}\\AttachmentProcessorDataCollector.dll' added to the 'run list'"));
        Assert.IsTrue(Regex.IsMatch(logFileContent, @"Invocation of data collector attachment processor AssemblyQualifiedName: 'Microsoft\.VisualStudio\.TestPlatform\.CrossPlatEngine\.TestRunAttachmentsProcessing\.DataCollectorAttachmentProcessorAppDomain, Microsoft\.TestPlatform\.CrossPlatEngine, Version=.*, Culture=neutral, PublicKeyToken=.*' FriendlyName: 'SampleDataCollector'"));
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

    private IList<string> GetTestAssemblies()
        => new List<string> { "SimpleTestProject.dll", "SimpleTestProject2.dll" }.Select(p => GetAssetFullPath(p)).ToList();
}
