// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class DataCollectionTests : AcceptanceTestBase
{
    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void ExecuteTestsWithDataCollection(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("SimpleTestProject2.dll");
        string runSettings = GetRunsettingsFilePath(TempDirectory.Path);
        string diagFileName = Path.Combine(TempDirectory.Path, "diaglog.txt");
        var extensionsPath = Path.GetDirectoryName(GetTestDllForFramework("OutOfProcDataCollector.dll", "netstandard2.0"));
        var arguments = PrepareArguments(assemblyPaths, null, runSettings, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, $" /Diag:{diagFileName}", $" /TestAdapterPath:{extensionsPath}");

        var env = new Dictionary<string, string?>
        {
            ["TEST_ASSET_SAMPLE_COLLECTOR_PATH"] = TempDirectory.Path,
        };

        InvokeVsTest(arguments, env);

        ValidateSummaryStatus(1, 1, 1);
        VaildateDataCollectorOutput(TempDirectory.Path);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void ExecuteTestsWithDataCollectionUsingCollectArgument(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("SimpleTestProject2.dll");
        string diagFileName = Path.Combine(TempDirectory.Path, "diaglog.txt");
        var extensionsPath = Path.GetDirectoryName(GetTestDllForFramework("OutOfProcDataCollector.dll", "netstandard2.0"));

        var arguments = PrepareArguments(assemblyPaths, null, null, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        arguments = string.Concat(arguments, $" /Diag:{diagFileName}", $" /Collect:SampleDataCollector", $" /TestAdapterPath:{extensionsPath}");

        var env = new Dictionary<string, string?>
        {
            ["TEST_ASSET_SAMPLE_COLLECTOR_PATH"] = TempDirectory.Path,
        };

        InvokeVsTest(arguments, env);

        ValidateSummaryStatus(1, 1, 1);
        VaildateDataCollectorOutput(TempDirectory.Path);
    }

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
    public void DataCollectorAssemblyLoadingShouldNotThrowErrorForNetCore(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetTestDllForFramework("AppDomainGetAssembliesTestProject.dll", DEFAULT_HOST_NETCORE), string.Empty, string.Empty, FrameworkArgValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
    public void DataCollectorAssemblyLoadingShouldNotThrowErrorForFullFramework(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetAssetFullPath("AppDomainGetAssembliesTestProject.dll"), string.Empty, string.Empty, FrameworkArgValue, resultsDirectory: TempDirectory.Path);

        InvokeVsTest(arguments);
        ValidateSummaryStatus(1, 0, 0);
    }

    [TestMethod]
    [NetFullTargetFrameworkDataSource]
    [NetCoreTargetFrameworkDataSource]
    public void DataCollectorAttachmentProcessor(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetAssetFullPath("SimpleTestProject.dll");
        var secondAssemblyPath = GetAssetFullPath("SimpleTestProject2.dll");
        string runSettings = GetRunsettingsFilePath(TempDirectory.Path);
        string diagFileName = Path.Combine(TempDirectory.Path, "diaglog.txt");
        var extensionsPath = Path.GetDirectoryName(GetTestDllForFramework("AttachmentProcessorDataCollector.dll", "netstandard2.0"));
        var arguments = PrepareArguments([assemblyPath, secondAssemblyPath], null, runSettings, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, $" /Diag:{diagFileName}", $" /TestAdapterPath:{extensionsPath}");

        XElement runSettingsXml = XElement.Load(runSettings);

        // Today we merge only in the case of ParallelProxyExecutionManager executor, that is chosen if:
        // (parallelLevel > 1 || !testHostManager.Shared) -> "src\Microsoft.TestPlatform.CrossPlatEngine\TestEngine.cs" line ~248
        // So we'll merge always in case of DotnetTestHostManager(Shared = false) or in case of DefaultTestHostManager(DisableAppDomain = true) or if MaxCpuCount > 1
        // For NetFull test we need to have more than one test library and MaxCpuCount > 1
        runSettingsXml.Add(new XElement("RunConfiguration", new XElement("MaxCpuCount", 2)));

        // Set datacollector parameters
        runSettingsXml.Element("DataCollectionRunSettings")!
            .Element("DataCollectors")!
            .Element("DataCollector")!
            .Add(new XElement("Configuration", new XElement("MergeFile", "MergedFile.txt")));
        runSettingsXml.Save(runSettings);

        var env = new Dictionary<string, string?>
        {
            ["SampleDataCollectorTempPath"] = TempDirectory.Path,
        };

        InvokeVsTest(arguments, env);

        ValidateSummaryStatus(2, 2, 2);

        string mergedFile = Directory.GetFiles(TempDirectory.Path, "MergedFile.txt", SearchOption.AllDirectories).Single();
        var fileContent = new List<string>();
        using (var streamReader = new StreamReader(mergedFile))
        {
            while (!streamReader.EndOfStream)
            {
                string? line = streamReader.ReadLine();
                Assert.IsTrue(line!.StartsWith("SessionEnded_Handler_"));
                fileContent.Add(line);
            }
        }

        Assert.AreEqual(2, fileContent.Distinct().Count());

        var dataCollectorsLogs = Directory.GetFiles(TempDirectory.Path, "*.datacollector.*", SearchOption.TopDirectoryOnly);
        Assert.AreEqual(2, dataCollectorsLogs.Distinct().Count());
        foreach (var dataCollectorLogFile in dataCollectorsLogs)
        {
            string dataCollectorLog = File.ReadAllText(dataCollectorLogFile);
            Assert.IsTrue(dataCollectorLog.Contains("MetadataReaderExtensionsHelper: Valid extension found: extension type 'DataCollector' identifier 'my://sample/datacollector' implementation 'AttachmentProcessorDataCollector.SampleDataCollectorV1' version '1'"));
            Assert.IsTrue(dataCollectorLog.Contains("MetadataReaderExtensionsHelper: Valid extension found: extension type 'DataCollector' identifier 'my://sample/datacollector' implementation 'AttachmentProcessorDataCollector.SampleDataCollectorV2' version '2'"));
            Assert.IsTrue(Regex.IsMatch(dataCollectorLog, @"GetTestExtensionFromType: Discovered multiple test extensions with identifier data 'my://sample/datacollector' and type 'AttachmentProcessorDataCollector\.SampleDataCollectorV1, AttachmentProcessorDataCollector, Version=.*, Culture=neutral, PublicKeyToken=null' inside file '.*AttachmentProcessorDataCollector\.dll'; keeping the first one 'AttachmentProcessorDataCollector\.SampleDataCollectorV2, AttachmentProcessorDataCollector, Version=.*, Culture=neutral, PublicKeyToken=null'\."));
        }
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

    private void VaildateDataCollectorOutput(string resultsDir)
    {
        // Output of datacollection attachment.
        StdOutputContains("filename.txt");
        StdOutputContains("TestCaseStarted");
        StdOutputContains("TestCaseEnded");
        StdOutputContains("SampleUnitTestProject2.UnitTest1.PassingTest2");
        StdOutputContains("SampleUnitTestProject2.UnitTest1.FailingTest2");
        StdOutputContains("Data collector 'SampleDataCollector' message: SessionStarted");
        StdOutputContains("Data collector 'SampleDataCollector' message: TestHostLaunched");
        StdOutputContains("Data collector 'SampleDataCollector' message: SessionEnded");
        StdOutputContains("Data collector 'SampleDataCollector' message: my warning");
        StdErrorContains("Data collector 'SampleDataCollector' message: Data collector caught an exception of type 'System.Exception': 'my exception'. More details:");
        StdOutputContains("Data collector 'SampleDataCollector' message: Dispose called.");

        // Verify attachments
        var isTestRunLevelAttachmentFound = false;
        var testCaseLevelAttachmentsCount = 0;
        var diaglogsFileCount = 0;

        var resultFiles = Directory.GetFiles(resultsDir, "*.txt", SearchOption.AllDirectories);

        foreach (var file in resultFiles)
        {
            // Test Run level attachments are logged in standard output.
            if (file.Contains("filename.txt"))
            {
                StdOutputContains(file);
                isTestRunLevelAttachmentFound = true;
            }

            if (file.Contains("testcasefilename"))
            {
                testCaseLevelAttachmentsCount++;
            }

            if (file.Contains("diaglog"))
            {
                diaglogsFileCount++;
            }
        }

        Assert.IsTrue(isTestRunLevelAttachmentFound);
        Assert.AreEqual(3, testCaseLevelAttachmentsCount);
        Assert.AreEqual(3, diaglogsFileCount);
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
}
