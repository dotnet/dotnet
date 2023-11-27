// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
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
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
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
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
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
    [NetCoreTargetFrameworkDataSource]
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
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource(inIsolation: true, inProcess: true)]
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
        Assert.IsTrue(trxFilePaths.Count() > 1);
    }

    [TestMethod]
    [NetCoreTargetFrameworkDataSource]
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
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
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

        string? outcomeValue = GetElementAtributeValueFromTrx(trxFilePath, "ResultSummary", "outcome");

        Assert.AreEqual("Failed", outcomeValue);
    }

    [TestMethod]
    [TestCategory("Windows-Review")]
    [NetFullTargetFrameworkDataSource]
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

        string? outcomeValue = GetElementAtributeValueFromTrx(trxFilePath, "ResultSummary", "outcome");

        Assert.AreEqual("Completed", outcomeValue);
    }

    private static void AssertExpectedHtml(XmlElement root)
    {
        XmlNodeList elementList = root.GetElementsByTagName("details");
        Assert.AreEqual(2, elementList.Count);

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
        string[] divs = { "Total tests", "Passed", "Failed", "Skipped", "Run duration", "Pass percentage", "PassingTest" };
        foreach (string str in divs)
        {
            StringAssert.Contains(filePathContent, str);
        }
    }

    private static string? GetElementAtributeValueFromTrx(string trxFileName, string fieldName, string attributeName)
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
}
