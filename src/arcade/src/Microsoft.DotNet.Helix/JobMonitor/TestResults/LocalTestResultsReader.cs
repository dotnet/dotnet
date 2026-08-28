// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml;
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
        if (fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
        {
            fileName = fileName[..^4];
        }

        return fileName.EndsWith(".trx", StringComparison.OrdinalIgnoreCase)
            || fileName.EndsWith("testResults.xml", StringComparison.OrdinalIgnoreCase)
            || fileName.EndsWith("test-results.xml", StringComparison.OrdinalIgnoreCase)
            || fileName.EndsWith("test_results.xml", StringComparison.OrdinalIgnoreCase)
            || fileName.EndsWith("junit-results.xml", StringComparison.OrdinalIgnoreCase)
            || fileName.EndsWith("junitresults.xml", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<IReadOnlyList<TestResult>> ReadResultFileAsync(
        string filePath,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var results = new List<TestResult>();
            await foreach (TestResult result in ReadResultsAsync(filePath, cancellationToken))
            {
                results.Add(result);
            }

            return results;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to parse test results file '{Path}'.", filePath);
            return [];
        }
    }

    private async IAsyncEnumerable<TestResult> ReadResultsAsync(
        string filePath,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string rootName = await ReadRootNameAsync(filePath, cancellationToken);
        string workItemName = new DirectoryInfo(Path.GetDirectoryName(filePath) ?? string.Empty).Name;

        switch (rootName)
        {
            case "assemblies":
            case "assembly":
                await foreach (TestResult result in ReadElementsAsync(
                    filePath,
                    "test",
                    ReadXunitResult,
                    cancellationToken))
                {
                    yield return result;
                }
                break;

            case "testsuites":
            case "testsuite":
                await foreach (TestResult result in ReadElementsAsync(
                    filePath,
                    "testcase",
                    element => ReadJUnitResult(element, workItemName),
                    cancellationToken))
                {
                    yield return result;
                }
                break;

            case "TestRun":
                Dictionary<string, TestDefinition> definitions =
                    await ReadTrxDefinitionsAsync(filePath, cancellationToken);
                await foreach (TestResult result in ReadElementsAsync(
                    filePath,
                    "UnitTestResult",
                    element => ReadTrxResult(element, workItemName, definitions),
                    cancellationToken))
                {
                    yield return result;
                }
                break;

            default:
                _logger.LogWarning(
                    "Test result file '{Path}' has unsupported root element '{RootElement}' and will be skipped.",
                    filePath,
                    rootName);
                break;
        }
    }

    private static async Task<string> ReadRootNameAsync(string filePath, CancellationToken cancellationToken)
    {
        using XmlReader reader = CreateReader(filePath);
        while (await reader.ReadAsync())
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (reader.NodeType == XmlNodeType.Element)
            {
                return reader.LocalName;
            }
        }

        return string.Empty;
    }

    private static async IAsyncEnumerable<TestResult> ReadElementsAsync(
        string filePath,
        string elementName,
        Func<XElement, TestResult> convert,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using XmlReader reader = CreateReader(filePath);
        while (await reader.ReadAsync())
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (reader.NodeType != XmlNodeType.Element || reader.LocalName != elementName)
            {
                continue;
            }

            using XmlReader subtree = reader.ReadSubtree();
            await subtree.ReadAsync();
            yield return convert(XElement.Load(subtree, LoadOptions.PreserveWhitespace));
        }
    }

    private static async Task<Dictionary<string, TestDefinition>> ReadTrxDefinitionsAsync(
        string filePath,
        CancellationToken cancellationToken)
    {
        var definitions = new Dictionary<string, TestDefinition>(StringComparer.OrdinalIgnoreCase);
        using XmlReader reader = CreateReader(filePath);
        while (await reader.ReadAsync())
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "UnitTest")
            {
                continue;
            }

            using XmlReader subtree = reader.ReadSubtree();
            await subtree.ReadAsync();
            XElement unitTest = XElement.Load(subtree);
            string? id = GetAttribute(unitTest, "id");
            XElement? method = unitTest.Descendants().FirstOrDefault(static x => x.Name.LocalName == "TestMethod");
            if (!string.IsNullOrEmpty(id))
            {
                definitions[id] = new TestDefinition(
                    GetAttribute(method, "className"),
                    GetAttribute(method, "name"));
            }
        }

        return definitions;
    }

    private TestResult ReadXunitResult(XElement test)
    {
        XElement? failure = test.Elements().FirstOrDefault(static x => x.Name.LocalName == "failure");
        string? output = test.Elements().FirstOrDefault(static x => x.Name.LocalName == "output")?.Value?.Trim();
        string typeName = GetAttribute(test, "type") ?? string.Empty;
        string method = GetAttribute(test, "method") ?? string.Empty;
        string name = GetAttribute(test, "name")
            ?? (!string.IsNullOrEmpty(typeName) && !string.IsNullOrEmpty(method) ? $"{typeName}.{method}" : method);
        string outcome = NormalizeOutcome(GetAttribute(test, "result"));

        List<TestResultAttachment> attachments = [];
        AddAttachmentIfEnabled(attachments, "output.txt", output, outcome);

        return new TestResult(
            name,
            "xunit",
            typeName,
            method,
            ParseDouble(GetAttribute(test, "time")),
            outcome,
            GetAttribute(failure, "exception-type"),
            failure?.Elements().FirstOrDefault(static x => x.Name.LocalName == "message")?.Value?.Trim(),
            failure?.Elements().FirstOrDefault(static x => x.Name.LocalName == "stack-trace")?.Value?.Trim(),
            test.Elements().FirstOrDefault(static x => x.Name.LocalName == "reason")?.Value?.Trim(),
            attachments);
    }

    private TestResult ReadJUnitResult(XElement test, string workItemName)
    {
        XElement? failure = test.Elements().FirstOrDefault(static x => x.Name.LocalName is "failure" or "error");
        XElement? skipped = test.Elements().FirstOrDefault(static x => x.Name.LocalName == "skipped");
        string className = GetAttribute(test, "classname") ?? workItemName;
        string method = GetAttribute(test, "name") ?? string.Empty;
        string outcome = skipped is not null ? "Skip" : failure is not null ? "Fail" : "Pass";

        List<TestResultAttachment> attachments = [];
        AddAttachmentIfEnabled(
            attachments,
            "stdout.txt",
            test.Elements().FirstOrDefault(static x => x.Name.LocalName == "system-out")?.Value?.Trim(),
            outcome);
        AddAttachmentIfEnabled(
            attachments,
            "stderr.txt",
            test.Elements().FirstOrDefault(static x => x.Name.LocalName == "system-err")?.Value?.Trim(),
            outcome);

        return new TestResult(
            !string.IsNullOrEmpty(className) ? $"{className}.{method}" : method,
            "junit",
            className,
            method,
            ParseDouble(GetAttribute(test, "time")),
            outcome,
            null,
            failure?.Value?.Trim(),
            null,
            skipped?.Value?.Trim(),
            attachments);
    }

    private TestResult ReadTrxResult(
        XElement result,
        string workItemName,
        IReadOnlyDictionary<string, TestDefinition> definitions)
    {
        string testId = GetAttribute(result, "testId") ?? string.Empty;
        definitions.TryGetValue(testId, out TestDefinition? definition);
        string className = definition?.ClassName ?? workItemName;
        string method = definition?.Method ?? GetAttribute(result, "testName") ?? string.Empty;
        string displayName = GetAttribute(result, "testName")
            ?? (!string.IsNullOrEmpty(className) ? $"{className}.{method}" : method);

        XElement? output = result.Descendants().FirstOrDefault(static x => x.Name.LocalName == "Output");
        string? failureMessage = output?.Descendants().FirstOrDefault(static x => x.Name.LocalName == "Message")?.Value?.Trim();
        string outcome = NormalizeOutcome(GetAttribute(result, "outcome"));

        List<TestResultAttachment> attachments = [];
        AddAttachmentIfEnabled(
            attachments,
            "stdout.txt",
            output?.Descendants().FirstOrDefault(static x => x.Name.LocalName == "StdOut")?.Value?.Trim(),
            outcome);
        AddAttachmentIfEnabled(
            attachments,
            "stderr.txt",
            output?.Descendants().FirstOrDefault(static x => x.Name.LocalName == "StdErr")?.Value?.Trim(),
            outcome);

        return new TestResult(
            displayName,
            "trx",
            className,
            method,
            ParseDuration(GetAttribute(result, "duration")),
            outcome,
            null,
            failureMessage,
            output?.Descendants().FirstOrDefault(static x => x.Name.LocalName == "StackTrace")?.Value?.Trim(),
            string.Equals(outcome, "Skip", StringComparison.Ordinal) ? failureMessage : null,
            attachments);
    }

    private static XmlReader CreateReader(string filePath)
        => XmlReader.Create(File.OpenRead(filePath), new XmlReaderSettings
        {
            Async = true,
            CloseInput = true,
            DtdProcessing = DtdProcessing.Prohibit,
            IgnoreComments = true,
        });

    private static string? GetAttribute(XElement? element, string name)
        => element?.Attribute(name)?.Value;

    private static double ParseDouble(string? value)
        => double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double result) ? result : 0;

    private static double ParseDuration(string? value)
        => TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out TimeSpan result)
            ? result.TotalSeconds
            : ParseDouble(value);

    private static string NormalizeOutcome(string? value)
        => value?.Trim().ToLowerInvariant() switch
        {
            "pass" or "passed" or "success" or "succeeded" => "Pass",
            "skip" or "skipped" or "notexecuted" or "notrun" => "Skip",
            "fail" or "failed" or "error" or "timeout" or "aborted" => "Fail",
            _ => "None",
        };

    private void AddAttachmentIfEnabled(
        List<TestResultAttachment> attachments,
        string name,
        string? text,
        string normalizedOutcome)
    {
        bool includeAttachment = _attachmentMode switch
        {
            TestResultAttachmentMode.All => true,
            TestResultAttachmentMode.Failed => normalizedOutcome == "Fail",
            _ => false,
        };

        if (includeAttachment && !string.IsNullOrWhiteSpace(text))
        {
            attachments.Add(new TestResultAttachment(name, text));
        }
    }

    private sealed record TestDefinition(string? ClassName, string? Method);
}
