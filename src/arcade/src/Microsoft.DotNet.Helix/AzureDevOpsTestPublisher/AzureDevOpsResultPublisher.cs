// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;
using Microsoft.Extensions.Logging;

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;

public sealed class AzureDevOpsResultPublisher : IDisposable
{
    private const int TestListBuckets = 32;
    private static readonly TimeSpan s_httpClientTimeout = TimeSpan.FromMinutes(5);
    private static readonly JsonSerializerOptions s_serializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false,
    };

    private readonly AsyncLocal<string> _lastSendContent = new();
    private string s_lastSendContent
    {
        get => _lastSendContent.Value ?? string.Empty;
        set => _lastSendContent.Value = value;
    }

    private readonly AzureDevOpsReportingParameters _azdoParameters;
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public AzureDevOpsResultPublisher(
        AzureDevOpsReportingParameters azdoParameters,
        ILogger logger)
    {
        _azdoParameters = azdoParameters;
        _httpClient = CreateHttpClient(azdoParameters.AccessToken);
        _logger = logger;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    public async Task<TestResultUploadSummary> UploadTestResultsWithSummaryAsync(List<string> testResultFiles, object resultMetadata, CancellationToken cancellationToken = default)
    {
        var testResultReader = new LocalTestResultsReader(_logger);

        Task<IReadOnlyList<TestResult>>[] parseTasks = [.. testResultFiles.Select(file => testResultReader.ReadResultFileAsync(file, cancellationToken))];
        IReadOnlyList<TestResult>[] parsedResults = await Task.WhenAll(parseTasks);
        if (parsedResults.Length == 0)
        {
            _logger.LogWarning("No test result files were provided for upload");
            return new TestResultUploadSummary(true, 0);
        }

        IReadOnlyList<AggregatedResult> aggregatedResults = new ResultAggregator().Aggregate(parsedResults);
        if (aggregatedResults.Count == 0)
        {
            _logger.LogDebug("Test results were discovered but none could be aggregated");
            return new TestResultUploadSummary(true, 0);
        }

        long uploadedCount = await UploadTestResultsWithCountAsync(aggregatedResults, resultMetadata, cancellationToken);
        return new TestResultUploadSummary(
            aggregatedResults.All(result => result.Result != "Failed"), // TODO: maybe there's a better way to find out if a test failed? Is this extensive enough?
            uploadedCount);
    }

    public async Task<long> UploadTestResultsWithCountAsync(IEnumerable<AggregatedResult> results, object resultMetadata, CancellationToken cancellationToken = default)
    {
        try
        {
            long publishedTestCount = 0;
            IReadOnlyList<AggregatedResult> resultList = results as IReadOnlyList<AggregatedResult> ?? results.ToList();
            var converted = ConvertResults(resultList, resultMetadata).ToList();
            foreach (List<ConvertedResult> batch in Batch(converted, 1000, static t => Size(t.Converted)))
            {
                IReadOnlyList<PublishedTestCase> publishedTests = await PublishResultsAsync(batch, cancellationToken);
                publishedTestCount += publishedTests.Count;
            }

            _logger.LogDebug("Uploaded {Count} results", publishedTestCount);

            // TODO - Do we need this?
            // await SendMetadataAsync(resultList, cancellationToken);
            return publishedTestCount;
        }
        catch (TerminalError ex)
        {
            _logger.LogError(ex, "Failed to upload test results to Azure DevOps.");
            throw;
        }
    }

    private static async Task SendMetadataAsync(
        IEnumerable<AggregatedResult> allTestResults,
        CancellationToken cancellationToken)
    {
        var partitionedResults = new Dictionary<int, List<TestListRow>>();
        var resultCounts = new Dictionary<string, int>(StringComparer.Ordinal);

        void ProcessResultForMetadata(AggregatedResult result)
        {
            resultCounts[result.Result] = resultCounts.TryGetValue(result.Result, out int count) ? count + 1 : 1;
            if (!string.Equals(result.Result, "Passed", StringComparison.Ordinal))
            {
                return;
            }

            string name = result.Name;
            string? argumentHash = null;
            string partitionKey = name;
            int parenthesisIndex = name.IndexOf('(');
            if (parenthesisIndex >= 0)
            {
                string argumentList = name[(parenthesisIndex + 1)..].TrimEnd(')');
                name = name[..parenthesisIndex];
                argumentHash = Convert.ToBase64String(SHA1.HashData(Encoding.UTF8.GetBytes(argumentList)));
                partitionKey = name + argumentHash;
            }

            int bucket = SHA1.HashData(Encoding.UTF8.GetBytes(partitionKey))[0] % TestListBuckets;
            if (!partitionedResults.TryGetValue(bucket, out List<TestListRow>? testNames))
            {
                testNames = [];
                partitionedResults[bucket] = testNames;
            }

            testNames.Add(new TestListRow(name, argumentHash));
        }

        void ProcessTestForMetadata(AggregatedResult result)
        {
            if (result.AggregationType == AggregationType.DataDriven && result.SubResults.Count > 0)
            {
                foreach (AggregatedResult subResult in result.SubResults)
                {
                    ProcessTestForMetadata(subResult);
                }
            }
            else if (result.AggregationType == AggregationType.Single)
            {
                ProcessResultForMetadata(result);
            }
        }

        foreach (AggregatedResult result in allTestResults)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ProcessTestForMetadata(result);
        }
        /*
        var uploadedUrls = new Dictionary<int, string>();
        foreach ((int key, List<TestListRow>? testNames) in partitionedResults)
        {
            byte[] csvBytes = CreateCompressedCsv(testNames);
            string fileName = $"{Guid.NewGuid():N}.csv.gz";
            uploadedUrls[key] = await _uploadClient.UploadAsync(csvBytes, fileName, "application/gzip", cancellationToken);
        }

        var dataModel = new
        {
            version = 2,
            rerun_tests = backChannelCases,
            test_lists = uploadedUrls,
            partitions = TestListBuckets,
            result_counts = resultCounts,
        };

        byte[] rawBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(dataModel, s_serializerOptions));
        byte[] compressedBytes = Compress(rawBytes);
        string base64Data = Convert.ToBase64String(compressedBytes);
        string fileNameBase = $"__helix_metadata_{Guid.NewGuid():N}.json.gz";

        await SendWithRetryAsync(
            HttpMethod.Post,
            $"{_azdoParameters.TeamProject}/_apis/test/runs/{_azdoParameters.TestRunId}/attachments?api-version=7.1-preview.1",
            new TestRunAttachmentRequest(fileNameBase, base64Data),
            cancellationToken);

        string metadataUrl = await _uploadClient.UploadAsync(compressedBytes, fileNameBase, "application/gzip", cancellationToken);
        await _eventClient.SendAsync(
            new
            {
                Type = "AzureDevOpsTestRunMetadata",
                TestRunProject = _azdoParameters.TeamProject,
                TestRunId = _azdoParameters.TestRunId,
                Url = metadataUrl,
            },
            cancellationToken);
        */
    }

    private async Task<IReadOnlyList<PublishedTestCase>> PublishResultsAsync(
        IReadOnlyList<ConvertedResult> converted,
        CancellationToken cancellationToken)
    {
        var testCaseResults = converted.Select(static c => c.Converted).ToList();
        var originalList = converted.Select(static c => c.Aggregated).ToList();

        using HttpResponseMessage response = await SendWithRetryAsync(
            HttpMethod.Post,
            $"{_azdoParameters.TeamProject}/_apis/test/runs/{_azdoParameters.TestRunId}/results?api-version=7.1-preview.6",
            testCaseResults,
            cancellationToken);

        IReadOnlyList<PublishedTestCaseResultReference> publishedResults = await ReadPublishedResultsAsync(response, cancellationToken);
        if (publishedResults.Count == 0)
        {
            _logger.LogWarning("The test run appears to have been closed, aborting test result uploads.");
            return [];
        }

        List<PublishedTestCase> publishedTestCases = [];

        foreach ((PublishedTestCaseResultReference published, AggregatedResult original, PublishedTestCase testCase) in publishedResults.Zip(originalList, testCaseResults))
        {
            if (published.Id == -1)
            {
                _logger.LogWarning("Azure DevOps test ID returned -1, unable to attach files.");
                continue;
            }

            async Task IterateSubResultsAsync(
                IReadOnlyList<PublishedSubResultReference>? publishedSubResults,
                IReadOnlyList<AggregatedResult> originalSubResults,
                long testId)
            {
                if (publishedSubResults is null || publishedSubResults.Count == 0)
                {
                    if (originalSubResults.Count > 0)
                    {
                        _logger.LogError("Published results do not include sub-results, attachments lost.");
                    }

                    return;
                }

                if (publishedSubResults.Count != originalSubResults.Count)
                {
                    _logger.LogError("Published sub-result counts do not match uploaded attachments. Attachments lost.");
                    return;
                }

                foreach ((PublishedSubResultReference publishedSubResult, AggregatedResult originalSubResult) subTriplet in publishedSubResults.Zip(originalSubResults, (publishedSubResult, originalSubResult) => (publishedSubResult, originalSubResult)))
                {
                    foreach (TestResultAttachment attachment in subTriplet.originalSubResult.Attachments)
                    {
                        await SendAttachmentAsync(attachment, testId, subTriplet.publishedSubResult.Id, cancellationToken);
                    }

                    await IterateSubResultsAsync(subTriplet.publishedSubResult.SubResults, subTriplet.originalSubResult.SubResults, testId);
                }
            }

            foreach (TestResultAttachment attachment in original.Attachments)
            {
                await SendAttachmentAsync(attachment, published.Id, null, cancellationToken);
            }

            await IterateSubResultsAsync(published.SubResults, original.SubResults, published.Id);

            publishedTestCases.Add(testCase);
        }

        return publishedTestCases;
    }

    private async Task SendAttachmentAsync(
        TestResultAttachment attachment,
        long testId,
        long? subResultId,
        CancellationToken cancellationToken)
    {
        var request = new TestRunAttachmentRequest(
            attachment.Name,
            Convert.ToBase64String(Encoding.UTF8.GetBytes(attachment.Text)));

        string path = subResultId is long subId
            ? $"{_azdoParameters.TeamProject}/_apis/test/runs/{_azdoParameters.TestRunId}/results/{testId}/attachments?testSubResultId={subId}&api-version=7.1-preview.1"
            : $"{_azdoParameters.TeamProject}/_apis/test/runs/{_azdoParameters.TestRunId}/results/{testId}/attachments?api-version=7.1-preview.1";

        using HttpResponseMessage response = await SendWithRetryAsync(HttpMethod.Post, path, request, cancellationToken);
        _ = response;
    }

    private IEnumerable<ConvertedResult> ConvertResults(IEnumerable<AggregatedResult> results, object resultMetadata)
    {
        static string GetResultGroupType(AggregationType aggregationType)
        {
            return aggregationType switch
            {
                AggregationType.Single => "None",
                AggregationType.DataDriven => "dataDriven",
                AggregationType.Rerun => "rerun",
                _ => "None",
            };
        }

        string comment = JsonSerializer.Serialize(resultMetadata) ?? string.Empty;

        PublishedSubResult ConvertToSubTest(AggregatedResult result)
        {
            var customFields = new List<CustomField>();
            if (result.IsFlaky)
            {
                customFields.Add(new CustomField("IsTestResultFlaky", true));
            }

            if ((result.AttemptId ?? 0) > 1)
            {
                customFields.Add(new CustomField("AttemptId", result.AttemptId!.Value - 1));
            }

            return new PublishedSubResult
            {
                Comment = comment,
                CustomFields = customFields,
                DisplayName = result.Name,
                Outcome = result.Result,
                DurationInMs = result.DurationSeconds * 1000.0,
                StackTrace = result.StackTrace,
                ErrorMessage = result.FailureMessage,
                SubResults = result.SubResults.Count == 0 ? null : [.. result.SubResults.Select(ConvertToSubTest)],
                ResultGroupType = GetResultGroupType(result.AggregationType),
            };
        }

        ConvertedResult ConvertResult(AggregatedResult result)
        {
            var customFields = new List<CustomField>();
            if (result.IsFlaky)
            {
                customFields.Add(new CustomField("IsTestResultFlaky", true));
            }

            if (result.AggregationType == AggregationType.Rerun && result.SubResults.Count > 1)
            {
                customFields.Add(new CustomField("AttemptId", result.SubResults.Count - 1));
            }

            return new ConvertedResult(
                new PublishedTestCase
                {
                    TestCaseTitle = result.Name,
                    AutomatedTestName = result.Name,
                    AutomatedTestType = "helix",
                    AutomatedTestStorage = comment, // TODO: This was workitem ID
                    Priority = 1,
                    DurationInMs = result.DurationSeconds * 1000.0,
                    Outcome = result.Result,
                    State = "Completed",
                    Comment = comment,
                    StackTrace = result.StackTrace,
                    ErrorMessage = result.FailureMessage,
                    SubResults = result.SubResults.Count == 0 ? null : [.. result.SubResults.Select(ConvertToSubTest)],
                    ResultGroupType = GetResultGroupType(result.AggregationType),
                    CustomFields = customFields,
                },
                result);
        }

        var converted = results.Select(ConvertResult).ToList();
        foreach (ConvertedResult? result in converted)
        {
            foreach (ConvertedResult chunk in Chunk(result, 950))
            {
                yield return chunk;
            }
        }
    }

    private static IEnumerable<ConvertedResult> Chunk(ConvertedResult test, int limit)
    {
        if (Size(test.Converted) <= limit)
        {
            yield return test;
            yield break;
        }

        IEnumerable<ChunkPair> zippedSubTests = (test.Converted.SubResults ?? [])
            .Zip(test.Aggregated.SubResults, (converted, aggregated) => new ChunkPair(converted, aggregated));

        foreach (List<ChunkPair> zippedBatch in Batch(zippedSubTests, limit, static pair => Size(pair.Converted)))
        {
            yield return new ConvertedResult(
                test.Converted with { SubResults = [.. zippedBatch.Select(static x => x.Converted)], Id = null },
                new AggregatedResult(
                    test.Aggregated.AggregationType,
                    test.Aggregated.Name,
                    test.Aggregated.DurationSeconds,
                    test.Aggregated.Result,
                    [.. zippedBatch.Select(static x => x.Aggregated)],
                    test.Aggregated.Attachments,
                    test.Aggregated.FailureMessage,
                    test.Aggregated.StackTrace,
                    isFlaky: test.Aggregated.IsFlaky,
                    attemptId: test.Aggregated.AttemptId));
        }
    }

    private static int Size(PublishedTestCase test)
    {
        return 1 + (test.SubResults?.Sum(Size) ?? 0);
    }

    private static int Size(PublishedSubResult test)
    {
        return 1 + (test.SubResults?.Sum(Size) ?? 0);
    }

    private static IEnumerable<List<T>> Batch<T>(IEnumerable<T> items, int limit, Func<T, int> getSize)
    {
        var currentBatch = new List<T>();
        int currentSize = 0;

        foreach (T? item in items)
        {
            int size = getSize(item);
            if (size > limit)
            {
                throw new InvalidOperationException("Cannot split a result larger than the batching limit.");
            }

            if (currentSize + size > limit && currentBatch.Count > 0)
            {
                yield return currentBatch;
                currentBatch = [];
                currentSize = 0;
            }

            currentBatch.Add(item);
            currentSize += size;
        }

        if (currentBatch.Count > 0)
        {
            yield return currentBatch;
        }
    }

    private static HttpClient CreateHttpClient(string? accessToken)
    {
        var client = new HttpClient
        {
            Timeout = s_httpClientTimeout
        };
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            string basicToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($":{accessToken}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicToken);
        }

        return client;
    }

    private async Task<HttpResponseMessage> SendWithRetryAsync(
        HttpMethod method,
        string relativePath,
        object? payload,
        CancellationToken cancellationToken)
    {
        int triesLeft = 10;
        string? body = payload is null ? null : JsonSerializer.Serialize(payload, s_serializerOptions);
        if (!string.IsNullOrEmpty(body))
        {
            s_lastSendContent = body;
        }

        while (true)
        {
            Uri baseUri = _azdoParameters.CollectionUri.AbsoluteUri.EndsWith('/')
                ? _azdoParameters.CollectionUri
                : new Uri(_azdoParameters.CollectionUri.AbsoluteUri + '/', UriKind.Absolute);

            using var request = new HttpRequestMessage(method, new Uri(baseUri, relativePath));
            if (body is not null)
            {
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            bool isServerError = (int)response.StatusCode >= 500;
            if ((isServerError || response.StatusCode == HttpStatusCode.TooManyRequests) && triesLeft > 0)
            {
                TimeSpan retryDelay = GetRetryDelay(response) ?? TimeSpan.FromSeconds(response.StatusCode == HttpStatusCode.TooManyRequests ? 30 : 3);
                response.Dispose();
                triesLeft--;
                _logger.LogDebug("Hit HTTP {StatusCode} from Azure DevOps. Waiting {DelaySeconds:0.###} seconds and trying again ({TriesLeft} retries left).",
                    (int)response.StatusCode,
                    retryDelay.TotalSeconds,
                    triesLeft);
                await Task.Delay(retryDelay, cancellationToken);
                continue;
            }

            if (responseBody.Contains("It may have been deleted", StringComparison.OrdinalIgnoreCase)
                || responseBody.Contains("not authorized to access this resource", StringComparison.OrdinalIgnoreCase)
                || responseBody.Contains("cannot be added or updated for a test run which is in Completed state", StringComparison.OrdinalIgnoreCase)
                || response.StatusCode == HttpStatusCode.Forbidden
                || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Dispose();
                throw new TerminalError(responseBody);
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(s_lastSendContent))
                {
                    /* TODO
                    await _uploadClient.UploadAsync(
                        Encoding.UTF8.GetBytes(s_lastSendContent),
                        "__failed_azdo_request_content.json",
                        "text/plain; charset=UTF-8",
                        cancellationToken);
                    */
                }
            }
            catch (Exception uploadException)
            {
                _logger.LogError(uploadException, "Failed to upload failed request payload.");
            }

            response.Dispose();
            throw new AzureDevOpsReportingError($"Azure DevOps request failed with status code {(int)response.StatusCode}: {responseBody}");
        }
    }

    private static TimeSpan? GetRetryDelay(HttpResponseMessage response)
    {
        RetryConditionHeaderValue? retryAfter = response.Headers.RetryAfter;
        if (retryAfter?.Delta is { } delta && delta > TimeSpan.Zero)
        {
            return delta;
        }

        if (retryAfter?.Date is { } date)
        {
            TimeSpan delay = date - DateTimeOffset.UtcNow;
            if (delay > TimeSpan.Zero)
            {
                return delay;
            }
        }

        return null;
    }

    private static async Task<IReadOnlyList<PublishedTestCaseResultReference>> ReadPublishedResultsAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        string content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(content))
        {
            return [];
        }

        using var document = JsonDocument.Parse(content);
        JsonElement root = document.RootElement;
        if (root.ValueKind == JsonValueKind.Array)
        {
            return [.. root.EnumerateArray().Select(ParsePublishedResult)];
        }

        if (root.TryGetProperty("value", out JsonElement value) && value.ValueKind == JsonValueKind.Array)
        {
            return [.. value.EnumerateArray().Select(ParsePublishedResult)];
        }

        return [];
    }

    private static PublishedTestCaseResultReference ParsePublishedResult(JsonElement element)
    {
        var subResults = new List<PublishedSubResultReference>();
        if (element.TryGetProperty("subResults", out JsonElement subResultElement) && subResultElement.ValueKind == JsonValueKind.Array)
        {
            subResults.AddRange(subResultElement.EnumerateArray().Select(ParsePublishedSubResult));
        }

        return new PublishedTestCaseResultReference(
            element.TryGetProperty("id", out JsonElement idElement) ? idElement.GetInt64() : -1,
            subResults);
    }

    private static PublishedSubResultReference ParsePublishedSubResult(JsonElement element)
    {
        var subResults = new List<PublishedSubResultReference>();
        if (element.TryGetProperty("subResults", out JsonElement subResultElement) && subResultElement.ValueKind == JsonValueKind.Array)
        {
            subResults.AddRange(subResultElement.EnumerateArray().Select(ParsePublishedSubResult));
        }

        return new PublishedSubResultReference(
            element.TryGetProperty("id", out JsonElement idElement) ? idElement.GetInt64() : -1,
            subResults);
    }

    private static byte[] CreateCompressedCsv(IEnumerable<TestListRow> rows)
    {
        var builder = new StringBuilder();
        foreach (TestListRow row in rows)
        {
            builder.Append(EscapeCsv(row.TestName));
            builder.Append(',');
            builder.Append(EscapeCsv(row.ArgumentHash));
            builder.AppendLine();
        }

        return Compress(Encoding.UTF8.GetBytes(builder.ToString()));
    }

    private static string EscapeCsv(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        if (!value.Contains('"') && !value.Contains(',') && !value.Contains('\n') && !value.Contains('\r'))
        {
            return value;
        }

        return $"\"{value.Replace("\"", "\"\"")}\"";
    }

    private static byte[] Compress(ReadOnlySpan<byte> rawBytes)
    {
        using var target = new MemoryStream();
        using (var gzip = new GZipStream(target, CompressionLevel.SmallestSize, leaveOpen: true))
        {
            gzip.Write(rawBytes);
        }

        return target.ToArray();
    }

    private sealed record ConvertedResult(PublishedTestCase Converted, AggregatedResult Aggregated);

    private sealed record ChunkPair(PublishedSubResult Converted, AggregatedResult Aggregated);

    private sealed record TestListRow(string TestName, string? ArgumentHash);

    private sealed record TestRunAttachmentRequest(string FileName, string Stream);

    private sealed record CustomField(string FieldName, object Value);

    private sealed record PublishedTestCase
    {
        public long? Id { get; init; }

        public string TestCaseTitle { get; init; } = string.Empty;

        public string AutomatedTestName { get; init; } = string.Empty;

        public string AutomatedTestType { get; init; } = string.Empty;

        public string AutomatedTestStorage { get; init; } = string.Empty;

        public int Priority { get; init; }

        public double DurationInMs { get; init; }

        public string Outcome { get; init; } = string.Empty;

        public string State { get; init; } = string.Empty;

        public string Comment { get; init; } = string.Empty;

        public string? StackTrace { get; init; }

        public string? ErrorMessage { get; init; }

        public List<PublishedSubResult>? SubResults { get; init; }

        public string ResultGroupType { get; init; } = string.Empty;

        public List<CustomField>? CustomFields { get; init; }
    }

    private sealed record PublishedSubResult
    {
        public long? Id { get; init; }

        public string Comment { get; init; } = string.Empty;

        public List<CustomField>? CustomFields { get; init; }

        public string DisplayName { get; init; } = string.Empty;

        public string Outcome { get; init; } = string.Empty;

        public double DurationInMs { get; init; }

        public string? StackTrace { get; init; }

        public string? ErrorMessage { get; init; }

        public List<PublishedSubResult>? SubResults { get; init; }

        public string ResultGroupType { get; init; } = string.Empty;
    }

    private sealed record PublishedTestCaseResultReference(long Id, IReadOnlyList<PublishedSubResultReference> SubResults);

    private sealed record PublishedSubResultReference(long Id, IReadOnlyList<PublishedSubResultReference> SubResults);
}
