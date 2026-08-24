// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Xml.Linq;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;

public sealed class LocalTestResultsReader(
    ILogger logger,
    TestResultAttachmentMode attachmentMode = TestResultAttachmentMode.Failed)
{
    private readonly ILogger _logger = logger;
    private readonly TestResultAttachmentMode _attachmentMode = attachmentMode;

    public static bool LooksLikeTestResultFile(string path)
    {
        string fileName = Path.GetFileName(path);
        return fileName.EndsWith(".trx", StringComparison.OrdinalIgnoreCase)
               || fileName.EndsWith("testResults.xml", StringComparison.OrdinalIgnoreCase)
               || fileName.EndsWith("test-results.xml", StringComparison.OrdinalIgnoreCase)
               || fileName.EndsWith("test_results.xml", StringComparison.OrdinalIgnoreCase)
               || fileName.EndsWith("junit-results.xml", StringComparison.OrdinalIgnoreCase)
               || fileName.EndsWith("junitresults.xml", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<IReadOnlyList<TestResult>> ReadResultFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            using FileStream stream = File.OpenRead(filePath);
            XDocument document = await XDocument.LoadAsync(stream, LoadOptions.PreserveWhitespace, cancellationToken);
            string rootName = document.Root?.Name.LocalName ?? string.Empty;
            string workItemName = new DirectoryInfo(Path.GetDirectoryName(filePath) ?? string.Empty).Name;

            return rootName switch
            {
                "assemblies" or "assembly" => ReadXunitResults(document),
                "TestRun" => ReadTrxResults(document, workItemName),
                "testsuites" or "testsuite" => ReadJUnitResults(document, workItemName),
                _ => [],
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to parse test results file '{Path}'.", filePath);
            return [];
        }
    }

    private IReadOnlyList<TestResult> ReadXunitResults(XDocument document)
    {
        return [..
            document.Descendants().Where(static e => e.Name.LocalName == "test").Select(test =>
            {
                XElement? failure = test.Elements().FirstOrDefault(static x => x.Name.LocalName == "failure");
                string? message = failure?.Elements().FirstOrDefault(static x => x.Name.LocalName == "message")?.Value?.Trim();
                string? stackTrace = failure?.Elements().FirstOrDefault(static x => x.Name.LocalName == "stack-trace")?.Value?.Trim();
                string? output = test.Elements().FirstOrDefault(static x => x.Name.LocalName == "output")?.Value?.Trim();
                string? skipReason = test.Elements().FirstOrDefault(static x => x.Name.LocalName == "reason")?.Value?.Trim();

                string typeName = GetAttribute(test, "type") ?? string.Empty;
                string method = GetAttribute(test, "method") ?? string.Empty;
                string name = GetAttribute(test, "name")
                    ?? (!string.IsNullOrEmpty(typeName) && !string.IsNullOrEmpty(method) ? $"{typeName}.{method}" : method);
                string normalizedOutcome = NormalizeOutcome(GetAttribute(test, "result"));

                List<TestResultAttachment> attachments = [];
                AddAttachmentIfEnabled(attachments, "output.txt", output, normalizedOutcome);

                return new TestResult(
                    name,
                    "xunit",
                    typeName,
                    method,
                    ParseDouble(GetAttribute(test, "time")),
                    normalizedOutcome,
                    GetAttribute(failure, "exception-type"),
                    message,
                    stackTrace,
                    skipReason,
                    attachments);
            })];
    }

    private IReadOnlyList<TestResult> ReadJUnitResults(XDocument document, string workItemName)
    {
        return [..
            document.Descendants().Where(static e => e.Name.LocalName == "testcase").Select(test =>
            {
                XElement? failure = test.Elements().FirstOrDefault(static x => x.Name.LocalName is "failure" or "error");
                XElement? skipped = test.Elements().FirstOrDefault(static x => x.Name.LocalName == "skipped");
                string? stdout = test.Elements().FirstOrDefault(static x => x.Name.LocalName == "system-out")?.Value?.Trim();
                string? stderr = test.Elements().FirstOrDefault(static x => x.Name.LocalName == "system-err")?.Value?.Trim();

                string className = GetAttribute(test, "classname") ?? workItemName;
                string method = GetAttribute(test, "name") ?? string.Empty;
                string name = !string.IsNullOrEmpty(className) ? $"{className}.{method}" : method;
                string result = skipped is not null ? "Skip" : failure is not null ? "Fail" : "Pass";

                List<TestResultAttachment> attachments = [];
                AddAttachmentIfEnabled(attachments, "stdout.txt", stdout, result);
                AddAttachmentIfEnabled(attachments, "stderr.txt", stderr, result);

                return new TestResult(
                    name,
                    "junit",
                    className,
                    method,
                    ParseDouble(GetAttribute(test, "time")),
                    result,
                    null,
                    failure?.Value?.Trim(),
                    null,
                    skipped?.Value?.Trim(),
                    attachments);
            })];
    }

    private IReadOnlyList<TestResult> ReadTrxResults(XDocument document, string workItemName)
    {
        Dictionary<string, XElement> unitTestsById = document
            .Descendants()
            .Where(static e => e.Name.LocalName == "UnitTest")
            .Select(static unitTest => (Id: GetAttribute(unitTest, "id"), Element: unitTest))
            .Where(static x => !string.IsNullOrEmpty(x.Id))
            .ToDictionary(static x => x.Id!, static x => x.Element, StringComparer.OrdinalIgnoreCase);

        return [..
            document.Descendants().Where(static e => e.Name.LocalName == "UnitTestResult").Select(result =>
            {
                string testId = GetAttribute(result, "testId") ?? string.Empty;
                unitTestsById.TryGetValue(testId, out XElement? unitTest);
                XElement? testMethod = unitTest?.Descendants().FirstOrDefault(static x => x.Name.LocalName == "TestMethod");

                string className = GetAttribute(testMethod, "className") ?? workItemName;
                string method = GetAttribute(testMethod, "name") ?? GetAttribute(result, "testName") ?? string.Empty;
                string displayName = GetAttribute(result, "testName")
                    ?? (!string.IsNullOrEmpty(className) ? $"{className}.{method}" : method);

                XElement? output = result.Descendants().FirstOrDefault(static x => x.Name.LocalName == "Output");
                string? failureMessage = output?.Descendants().FirstOrDefault(static x => x.Name.LocalName == "Message")?.Value?.Trim();
                string? stackTrace = output?.Descendants().FirstOrDefault(static x => x.Name.LocalName == "StackTrace")?.Value?.Trim();
                string? stdout = output?.Descendants().FirstOrDefault(static x => x.Name.LocalName == "StdOut")?.Value?.Trim();
                string? stderr = output?.Descendants().FirstOrDefault(static x => x.Name.LocalName == "StdErr")?.Value?.Trim();

                string rawOutcome = GetAttribute(result, "outcome") ?? string.Empty;
                string normalizedOutcome = NormalizeOutcome(rawOutcome);
                string? skipReason = string.Equals(normalizedOutcome, "Skip", StringComparison.Ordinal) ? failureMessage : null;

                List<TestResultAttachment> attachments = [];
                AddAttachmentIfEnabled(attachments, "stdout.txt", stdout, normalizedOutcome);
                AddAttachmentIfEnabled(attachments, "stderr.txt", stderr, normalizedOutcome);

                return new TestResult(
                    displayName,
                    "trx",
                    className,
                    method,
                    ParseDuration(GetAttribute(result, "duration")),
                    normalizedOutcome,
                    null,
                    failureMessage,
                    stackTrace,
                    skipReason,
                    attachments);
            })];
    }

    private static string? GetAttribute(XElement? element, string name)
        => element?.Attribute(name)?.Value;

    private static double ParseDouble(string? value)
    {
        return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result)
            ? result
            : 0;
    }

    private static double ParseDuration(string? value)
    {
        return TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out TimeSpan result)
            ? result.TotalSeconds
            : ParseDouble(value);
    }

    private static string NormalizeOutcome(string? value)
    {
        return value?.Trim().ToLowerInvariant() switch
        {
            "pass" or "passed" or "success" or "succeeded" => "Pass",
            "skip" or "skipped" or "notexecuted" or "notrun" => "Skip",
            "fail" or "failed" or "error" or "timeout" or "aborted" => "Fail",
            _ => "None",
        };
    }

    private void AddAttachmentIfEnabled(
        List<TestResultAttachment> attachments,
        string name,
        string? text,
        string normalizedOutcome)
    {
        bool includeAttachment = _attachmentMode switch
        {
            TestResultAttachmentMode.All => true,
            TestResultAttachmentMode.Failed => string.Equals(normalizedOutcome, "Fail", StringComparison.Ordinal),
            TestResultAttachmentMode.None => false,
            _ => false,
        };

        if (includeAttachment && !string.IsNullOrWhiteSpace(text))
        {
            attachments.Add(new TestResultAttachment(name, text));
        }
    }
}
