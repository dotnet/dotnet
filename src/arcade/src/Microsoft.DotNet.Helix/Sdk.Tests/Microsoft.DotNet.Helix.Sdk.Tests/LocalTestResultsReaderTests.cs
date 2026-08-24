// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    public class LocalTestResultsReaderTests
    {
        public static IEnumerable<object[]> AttachmentModeCases()
        {
            foreach (string format in new[] { "xunit", "junit", "trx" })
            {
                foreach (string outcome in new[] { "Pass", "Skip", "Fail" })
                {
                    yield return [format, outcome, TestResultAttachmentMode.Failed, outcome == "Fail"];
                    yield return [format, outcome, TestResultAttachmentMode.All, true];
                    yield return [format, outcome, TestResultAttachmentMode.None, false];
                    yield return [format, outcome, null, outcome == "Fail"];
                }
            }
        }

        [Theory]
        [MemberData(nameof(AttachmentModeCases))]
        public async Task LocalTestResultsReader_AppliesAttachmentMode(
            string format,
            string outcome,
            TestResultAttachmentMode? mode,
            bool expectsAttachments)
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string workItemDirectory = Path.Combine(tempDirectory, "work-item");
            Directory.CreateDirectory(workItemDirectory);

            try
            {
                string filePath = WriteResultFile(workItemDirectory, format, outcome, includeOutput: true);
                LocalTestResultsReader reader = mode.HasValue
                    ? new LocalTestResultsReader(NullLoggerFactory.Instance.CreateLogger<LocalTestResultsReader>(), mode.Value)
                    : new LocalTestResultsReader(NullLoggerFactory.Instance.CreateLogger<LocalTestResultsReader>());

                TestResult result = Assert.Single(await reader.ReadResultFileAsync(filePath));

                Assert.Equal(outcome, result.Result);
                Assert.Equal(expectsAttachments ? ExpectedAttachmentCount(format) : 0, result.Attachments.Count);
            }
            finally
            {
                Directory.Delete(tempDirectory, recursive: true);
            }
        }

        [Theory]
        [InlineData("xunit")]
        [InlineData("junit")]
        [InlineData("trx")]
        public async Task LocalTestResultsReader_DoesNotAttachEmptyOutput(string format)
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string workItemDirectory = Path.Combine(tempDirectory, "work-item");
            Directory.CreateDirectory(workItemDirectory);

            try
            {
                string filePath = WriteResultFile(workItemDirectory, format, "Fail", includeOutput: false);
                var reader = new LocalTestResultsReader(
                    NullLoggerFactory.Instance.CreateLogger<LocalTestResultsReader>(),
                    TestResultAttachmentMode.All);

                TestResult result = Assert.Single(await reader.ReadResultFileAsync(filePath));

                Assert.Empty(result.Attachments);
            }
            finally
            {
                Directory.Delete(tempDirectory, recursive: true);
            }
        }

        [Fact]
        public async Task LocalTestResultsReader_ReadsXunitFileFromDownloadedResults()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string workItemDirectory = Path.Combine(tempDirectory, "work-item");
            Directory.CreateDirectory(workItemDirectory);

            try
            {
                File.WriteAllText(
                    Path.Combine(workItemDirectory, "testResults.xml"),
                    """
                    <assemblies>
                      <assembly name="Sample.Tests.dll" total="1" passed="1" failed="0" skipped="0">
                        <collection total="1" passed="1" failed="0" skipped="0">
                          <test name="Sample.Tests.Passes" type="Sample.Tests" method="Passes" time="0.125" result="Pass" />
                        </collection>
                      </assembly>
                    </assemblies>
                    """);

                var reader = new LocalTestResultsReader(NullLoggerFactory.Instance.CreateLogger<LocalTestResultsReader>());
                string filePath = Path.Combine(workItemDirectory, "testResults.xml");
                IReadOnlyList<TestResult> resultSets = await reader.ReadResultFileAsync(filePath);
                IReadOnlyList<AggregatedResult> aggregate = new ResultAggregator().Aggregate([resultSets]);
                AggregatedResult result = Assert.Single(aggregate);

                Assert.Equal("Sample.Tests.Passes", result.Name);
                Assert.Equal("Passed", result.Result);
            }
            finally
            {
                Directory.Delete(tempDirectory, recursive: true);
            }
        }

        [Fact]
        public async Task LocalTestResultsReader_CombinesPackedAndXmlResultsAcrossWorkItems()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string packedDirectory = Path.Combine(tempDirectory, "packed-item");
            string xmlDirectory = Path.Combine(tempDirectory, "xml-item");
            Directory.CreateDirectory(packedDirectory);
            Directory.CreateDirectory(xmlDirectory);
            string originalDirectory = Environment.CurrentDirectory;

            try
            {
                Environment.CurrentDirectory = packedDirectory;
                string filePath = Path.Combine(xmlDirectory, "testResults.xml");

                File.WriteAllText(
                    filePath,
                    """
                    <assemblies>
                      <assembly name="Xml.Tests.dll" total="1" passed="1" failed="0" skipped="0">
                        <collection total="1" passed="1" failed="0" skipped="0">
                          <test name="Xml.Tests.Passes" type="Xml.Tests" method="Passes" time="0.250" result="Pass" />
                        </collection>
                      </assembly>
                    </assemblies>
                    """);

                IReadOnlyList<TestResult> resultSets = await new LocalTestResultsReader(NullLoggerFactory.Instance.CreateLogger<LocalTestResultsReader>()).ReadResultFileAsync(filePath);
                IReadOnlyList<AggregatedResult> aggregate = new ResultAggregator().Aggregate([resultSets]);

                Assert.Single(aggregate);
                Assert.Contains(aggregate, static x => x.Name == "Xml.Tests.Passes");
            }
            finally
            {
                Environment.CurrentDirectory = originalDirectory;
                Directory.Delete(tempDirectory, recursive: true);
            }
        }

        [Fact]
        public async Task LocalTestResultsReader_TrxWithUnqualifiedTestName_DerivesFullyQualifiedName()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            string workItemDirectory = Path.Combine(tempDirectory, "work-item");
            Directory.CreateDirectory(workItemDirectory);

            try
            {
                // MSTest emits testName as just the method name; the class lives in TestMethod.className.
                File.WriteAllText(
                    Path.Combine(workItemDirectory, "results.trx"),
                    """
                    <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
                      <Results>
                        <UnitTestResult testId="11111111-1111-1111-1111-111111111111" testName="MyMethod" outcome="Passed" duration="00:00:00.1234567" />
                      </Results>
                      <TestDefinitions>
                        <UnitTest id="11111111-1111-1111-1111-111111111111">
                          <TestMethod className="Ns.MyTests" name="MyMethod" />
                        </UnitTest>
                      </TestDefinitions>
                    </TestRun>
                    """);

                var reader = new LocalTestResultsReader(NullLoggerFactory.Instance.CreateLogger<LocalTestResultsReader>());
                string filePath = Path.Combine(workItemDirectory, "results.trx");
                IReadOnlyList<TestResult> resultSets = await reader.ReadResultFileAsync(filePath);

                TestResult test = Assert.Single(resultSets);
                Assert.Equal("MyMethod", test.Name);
                Assert.Equal("Ns.MyTests.MyMethod", test.FullyQualifiedName);

                AggregatedResult aggregated = Assert.Single(new ResultAggregator().Aggregate([resultSets], useFullyQualifiedName: true));
                Assert.Equal("Ns.MyTests.MyMethod", aggregated.FullyQualifiedName);
                Assert.Equal("Passed", aggregated.Result);
            }
            finally
            {
                Directory.Delete(tempDirectory, recursive: true);
            }
        }

        private static int ExpectedAttachmentCount(string format)
            => format == "xunit" ? 1 : 2;

        private static string WriteResultFile(string directory, string format, string outcome, bool includeOutput)
        {
            string output = includeOutput ? "test output" : "   ";
            string errorOutput = includeOutput ? "test error output" : string.Empty;

            (string FileName, string Content) result = format switch
            {
                "xunit" => (
                    "testResults.xml",
                    $"""
                    <assemblies>
                      <assembly>
                        <collection>
                          <test name="Sample.Tests.Test" type="Sample.Tests" method="Test" result="{outcome}">
                            {(outcome == "Fail" ? "<failure exception-type=\"Exception\"><message>failure</message><stack-trace>stack</stack-trace></failure>" : string.Empty)}
                            {(outcome == "Skip" ? "<reason>skipped</reason>" : string.Empty)}
                            <output>{output}</output>
                          </test>
                        </collection>
                      </assembly>
                    </assemblies>
                    """),
                "junit" => (
                    "junit-results.xml",
                    $"""
                    <testsuites>
                      <testsuite>
                        <testcase classname="Sample.Tests" name="Test">
                          {(outcome == "Fail" ? "<failure>failure</failure>" : string.Empty)}
                          {(outcome == "Skip" ? "<skipped>skipped</skipped>" : string.Empty)}
                          <system-out>{output}</system-out>
                          <system-err>{errorOutput}</system-err>
                        </testcase>
                      </testsuite>
                    </testsuites>
                    """),
                "trx" => (
                    "results.trx",
                    $"""
                    <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
                      <Results>
                        <UnitTestResult testId="11111111-1111-1111-1111-111111111111" testName="Test" outcome="{TrxOutcome(outcome)}">
                          <Output>
                            <StdOut>{output}</StdOut>
                            <StdErr>{errorOutput}</StdErr>
                            {(outcome == "Fail" ? "<ErrorInfo><Message>failure</Message><StackTrace>stack</StackTrace></ErrorInfo>" : string.Empty)}
                          </Output>
                        </UnitTestResult>
                      </Results>
                      <TestDefinitions>
                        <UnitTest id="11111111-1111-1111-1111-111111111111">
                          <TestMethod className="Sample.Tests" name="Test" />
                        </UnitTest>
                      </TestDefinitions>
                    </TestRun>
                    """),
                _ => throw new ArgumentOutOfRangeException(nameof(format)),
            };

            string filePath = Path.Combine(directory, result.FileName);
            File.WriteAllText(filePath, result.Content);
            return filePath;
        }

        private static string TrxOutcome(string outcome)
            => outcome switch
            {
                "Pass" => "Passed",
                "Skip" => "NotExecuted",
                "Fail" => "Failed",
                _ => throw new ArgumentOutOfRangeException(nameof(outcome)),
            };
    }
}
