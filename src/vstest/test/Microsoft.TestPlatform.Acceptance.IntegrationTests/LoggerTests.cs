// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class LoggerTests : AcceptanceTestBase
{
    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void TrxLoggerWithFriendlyNameShouldProperlyOverwriteFile(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        var trxFileName = "TestResults.trx";
        arguments = string.Concat(arguments, $" /logger:\"trx;LogFileName={trxFileName}\"");
        InvokeVsTest(arguments);

        arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"trx;LogFileName={trxFileName}\"");
        arguments = string.Concat(arguments, " /testcasefilter:Name~Pass");
        InvokeVsTest(arguments);

        var trxFilePath = Path.Combine(TempDirectory.Path, trxFileName);
        Assert.IsTrue(IsValidXml(trxFilePath), "Invalid content in Trx log file");
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void HtmlLoggerWithFriendlyNameShouldProperlyOverwriteFile(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        var htmlFileName = "TestResults.html";
        arguments = string.Concat(arguments, $" /logger:\"html;LogFileName={htmlFileName}\"");
        InvokeVsTest(arguments);

        arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"html;LogFileName={htmlFileName}\"");
        arguments = string.Concat(arguments, " /testcasefilter:Name~Pass");
        InvokeVsTest(arguments);

        var htmlLogFilePath = Path.Combine(TempDirectory.Path, htmlFileName);
        IsFileAndContentEqual(htmlLogFilePath);
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void HtmlLoggerWithFriendlyNameContainsExpectedContent(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        var htmlFileName = "TestResults.html";
        arguments = string.Concat(arguments, $" /logger:\"html;LogFileName={htmlFileName}\"");
        InvokeVsTest(arguments);

        var htmlLogFilePath = Path.Combine(TempDirectory.Path, htmlFileName);
        XmlDocument report = LoadReport(htmlLogFilePath);

        AssertExpectedHtml(report.DocumentElement!);
    }

    private static XmlDocument LoadReport(string htmlLogFilePath)
    {
        // XML reader cannot handle <br> tags because they are not closed, and hence are not valid XML.
        // They are correct HTML though, so we patch it here.
        var text = File.ReadAllText(htmlLogFilePath).Replace("<br>", "<br/>");
        var report = new XmlDocument();
        report.Load(new StringReader(text));
        return report;
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void TrxLoggerWithExecutorUriShouldProperlyOverwriteFile(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        var trxFileName = "TestResults.trx";
        arguments = string.Concat(arguments, $" /logger:\"logger://Microsoft/TestPlatform/TrxLogger/v1;LogFileName={trxFileName}\"");
        InvokeVsTest(arguments);

        arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"logger://Microsoft/TestPlatform/TrxLogger/v1;LogFileName={trxFileName}\"");
        arguments = string.Concat(arguments, " /testcasefilter:Name~Pass");
        InvokeVsTest(arguments);

        var trxLogFilePath = Path.Combine(TempDirectory.Path, trxFileName);
        Assert.IsTrue(IsValidXml(trxLogFilePath), "Invalid content in Trx log file");
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void TrxLoggerWithLogFilePrefixShouldGenerateMultipleTrx(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);
        var trxFileNamePattern = "TestResults";

        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"logger://Microsoft/TestPlatform/TrxLogger/v1;LogFilePrefix={trxFileNamePattern}\"");
        InvokeVsTest(arguments);

        arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"logger://Microsoft/TestPlatform/TrxLogger/v1;LogFilePrefix={trxFileNamePattern}\"");
        arguments = string.Concat(arguments, " /testcasefilter:Name~Pass");
        InvokeVsTest(arguments);

        var trxFilePaths = Directory.EnumerateFiles(TempDirectory.Path, trxFileNamePattern + "_net*.trx");
        Assert.IsGreaterThan(1, trxFilePaths.Count());
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void HtmlLoggerWithExecutorUriShouldProperlyOverwriteFile(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        var htmlFileName = "TestResults.html";
        arguments = string.Concat(arguments, $" /logger:\"logger://Microsoft/TestPlatform/htmlLogger/v1;LogFileName={htmlFileName}\"");
        InvokeVsTest(arguments);

        arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"logger://Microsoft/TestPlatform/htmlLogger/v1;LogFileName={htmlFileName}\"");
        arguments = string.Concat(arguments, " /testcasefilter:Name~Pass");
        InvokeVsTest(arguments);

        var htmlLogFilePath = Path.Combine(TempDirectory.Path, htmlFileName);
        IsFileAndContentEqual(htmlLogFilePath);
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void TrxLoggerResultSummaryOutcomeValueShouldBeFailedIfNoTestsExecutedAndTreatNoTestsAsErrorIsTrue(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue);
        var trxFilePath = Path.Combine(TempDirectory.Path, "TrxLogger.trx");

        arguments = string.Concat(arguments, $" /logger:\"trx;LogFileName={trxFilePath}\"");

        // Setting /TestCaseFilter to the test name, which does not exists in the assembly, so we will have 0 tests executed
        arguments = string.Concat(arguments, " /TestCaseFilter:TestNameThatMatchesNoTestInTheAssembly");
        arguments = string.Concat(arguments, " -- RunConfiguration.TreatNoTestsAsError=true");

        InvokeVsTest(arguments);

        string? outcomeValue = GetElementAttributeValueFromTrx(trxFilePath, "ResultSummary", "outcome");

        Assert.AreEqual("Failed", outcomeValue);
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void TrxLoggerResultSummaryOutcomeValueShouldNotChangeIfNoTestsExecutedAndTreatNoTestsAsErrorIsFalse(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var arguments = PrepareArguments(GetSampleTestAssembly(), GetTestAdapterPath(), string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue);
        var trxFilePath = Path.Combine(TempDirectory.Path, "TrxLogger.trx");

        arguments = string.Concat(arguments, $" /logger:\"trx;LogFileName={trxFilePath}\"");

        // Setting /TestCaseFilter to the test name, which does not exists in the assembly, so we will have 0 tests executed
        arguments = string.Concat(arguments, " /TestCaseFilter:TestNameThatMatchesNoTestInTheAssembly");
        arguments = string.Concat(arguments, " -- RunConfiguration.TreatNoTestsAsError=false");

        InvokeVsTest(arguments);

        string? outcomeValue = GetElementAttributeValueFromTrx(trxFilePath, "ResultSummary", "outcome");

        Assert.AreEqual("Completed", outcomeValue);
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void TrxLoggerResultSummaryOutcomeValueShouldBeFailedWhenDataCollectorLogsError(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetSampleTestAssembly();
        var extensionsPath = Path.GetDirectoryName(GetTestDllForFramework("OutOfProcDataCollector.dll", "netstandard2.0"));
        var trxFilePath = Path.Combine(TempDirectory.Path, "TrxLogger.trx");

        var arguments = PrepareArguments(assemblyPath, null, null, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        arguments = string.Concat(arguments, $" /TestCaseFilter:PassingTest");
        arguments = string.Concat(arguments, $" /Collect:SampleDataCollector");
        arguments = string.Concat(arguments, $" /TestAdapterPath:{extensionsPath}");
        arguments = string.Concat(arguments, $" /logger:\"trx;LogFileName={trxFilePath}\"");

        var env = new Dictionary<string, string?>
        {
            ["TEST_ASSET_SAMPLE_COLLECTOR_PATH"] = TempDirectory.Path,
        };

        InvokeVsTest(arguments, env);

        string? outcomeValue = GetElementAttributeValueFromTrx(trxFilePath, "ResultSummary", "outcome");

        Assert.AreEqual("Failed", outcomeValue);
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void TrxLoggerResultSummaryOutcomeValueShouldBeCompletedWhenDataCollectorLogsErrorAndTreatErrorMessagesAsWarningsIsTrue(RunnerInfo runnerInfo)
    {
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPath = GetSampleTestAssembly();
        var extensionsPath = Path.GetDirectoryName(GetTestDllForFramework("OutOfProcDataCollector.dll", "netstandard2.0"));
        var trxFilePath = Path.Combine(TempDirectory.Path, "TrxLogger.trx");

        var arguments = PrepareArguments(assemblyPath, null, null, FrameworkArgValue, runnerInfo.InIsolationValue, TempDirectory.Path);
        arguments = string.Concat(arguments, $" /TestCaseFilter:PassingTest");
        arguments = string.Concat(arguments, $" /Collect:SampleDataCollector");
        arguments = string.Concat(arguments, $" /TestAdapterPath:{extensionsPath}");
        arguments = string.Concat(arguments, $" /logger:\"trx;LogFileName={trxFilePath};TreatErrorMessagesAsWarnings=true\"");

        var env = new Dictionary<string, string?>
        {
            ["TEST_ASSET_SAMPLE_COLLECTOR_PATH"] = TempDirectory.Path,
        };

        InvokeVsTest(arguments, env);

        string? outcomeValue = GetElementAttributeValueFromTrx(trxFilePath, "ResultSummary", "outcome");

        Assert.AreEqual("Completed", outcomeValue);
    }

    private static void AssertExpectedHtml(XmlElement root)
    {
        XmlNodeList elementList = root.GetElementsByTagName("details");
        Assert.HasCount(2, elementList);

        foreach (XmlElement element in elementList)
        {
            Assert.AreEqual("summary", element.FirstChild?.Name);
            if (element.HasAttributes)
            {
                Assert.AreEqual("open", element.Attributes[0].Name);
            }
        }
    }

    private static bool IsValidXml(string xmlFilePath)
    {
        try
        {
            using var file = File.OpenRead(xmlFilePath);
            using var reader = XmlReader.Create(file);
            while (reader.Read())
            {
            }

            return true;
        }
        catch (XmlException)
        {
            return false;
        }
    }

    private static void IsFileAndContentEqual(string filePath)
    {
        StringBuilder sb = new();
        using (var sr = new StreamReader(filePath))
        {
            sb.Append(sr.ReadToEnd());
        }

        string filePathContent = sb.ToString();
        string[] divs = ["Total tests", "Passed", "Failed", "Skipped", "Run duration", "Pass percentage", "PassingTest"];
        foreach (string str in divs)
        {
            Assert.Contains(str, filePathContent);
        }
    }

    private static string? GetElementAttributeValueFromTrx(string trxFileName, string fieldName, string attributeName)
    {
        using FileStream file = File.OpenRead(trxFileName);
        using XmlReader reader = XmlReader.Create(file);
        while (reader.Read())
        {
            if (reader.Name.Equals(fieldName) && reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
            {
                return reader.GetAttribute(attributeName);
            }
        }

        return null;
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void TrxLoggerShouldNotDoubleCountDataDrivenTestResults(RunnerInfo runnerInfo)
    {
        // Regression test for https://github.com/microsoft/vstest/issues/15643
        // DataDriven (DataRow) test results were double-counted in TRX ResultSummary:
        // both the parent container and each inner data row result were counted.
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("DataDrivenTestProject.dll");
        var trxFilePath = Path.Combine(TempDirectory.Path, "DataDriven.trx");
        var arguments = PrepareArguments(assemblyPaths, null, string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"trx;LogFileName={trxFilePath}\"");

        InvokeVsTest(arguments);

        ValidateSummaryStatus(4, 0, 0);

        // Parse the TRX file and verify Counters reflect actual test executions (4),
        // not inflated by parent container results.
        var totalAttr = GetElementAttributeValueFromTrx(trxFilePath, "Counters", "total");
        var passedAttr = GetElementAttributeValueFromTrx(trxFilePath, "Counters", "passed");

        Assert.IsNotNull(totalAttr, "TRX Counters element should have a 'total' attribute.");
        Assert.IsNotNull(passedAttr, "TRX Counters element should have a 'passed' attribute.");
        // DataDrivenTestProject has: 3 DataRow rows + 1 SimpleTest = 4 test executions.
        // Before the fix, total would be 5 (parent container counted as extra).
        Assert.AreEqual("4", totalAttr, "TRX total count should reflect actual test executions, not include parent containers.");
        Assert.AreEqual("4", passedAttr, "TRX passed count should reflect actual passed tests.");
    }

    [TestMethod]
    [TestMatrix(testHost: Net)]
    public void TrxLoggerShouldPlaceTrxFileInSubdirectoryWhenLogFileNameContainsPath(RunnerInfo runnerInfo)
    {
        // Regression test for https://github.com/microsoft/vstest/issues/15271
        // When LogFileName contains a subdirectory (e.g. "subdir/results.trx"),
        // the TRX file and its attachments should be placed under that subdirectory.
        SetTestEnvironment(_testEnvironment, runnerInfo);

        var assemblyPaths = GetAssetFullPath("SimpleTestProject2.dll");
        var subDir = "custom-subdir";
        var trxFileName = "results.trx";
        var logFileName = Path.Combine(subDir, trxFileName);
        var arguments = PrepareArguments(assemblyPaths, null, string.Empty, FrameworkArgValue, runnerInfo.InIsolationValue, resultsDirectory: TempDirectory.Path);
        arguments = string.Concat(arguments, $" /logger:\"trx;LogFileName={logFileName}\"");

        InvokeVsTest(arguments);

        ValidateSummaryStatus(1, 1, 1);

        // Verify the TRX file is in the expected subdirectory
        var expectedTrxPath = Path.Combine(TempDirectory.Path, subDir, trxFileName);
        Assert.IsTrue(File.Exists(expectedTrxPath),
            $"Expected TRX file at '{expectedTrxPath}' but it was not found. " +
            $"Files in results dir: {string.Join(", ", Directory.GetFiles(TempDirectory.Path, "*.trx", SearchOption.AllDirectories))}");
        Assert.IsTrue(IsValidXml(expectedTrxPath), "TRX file content should be valid XML.");
    }
}
