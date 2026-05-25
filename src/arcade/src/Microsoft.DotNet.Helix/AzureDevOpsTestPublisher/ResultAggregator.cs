// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher;

public enum AggregationType
{
    Single = 0,
    Rerun = 1,
    DataDriven = 2,
}

public sealed class AggregatedResult(
    AggregationType aggregationType,
    string name,
    double durationSeconds,
    string result,
    IReadOnlyList<AggregatedResult>? subResults = null,
    IReadOnlyList<TestResultAttachment>? attachments = null,
    string? failureMessage = null,
    string? stackTrace = null,
    string? skipReason = null,
    bool isFlaky = false,
    int? attemptId = null)
{
    public AggregationType AggregationType { get; } = aggregationType;

    public string Name { get; } = name ?? string.Empty;

    public double DurationSeconds { get; } = durationSeconds;

    public string Result { get; } = result ?? string.Empty;

    public IReadOnlyList<TestResultAttachment> Attachments { get; } = attachments ?? [];

    public IReadOnlyList<AggregatedResult> SubResults { get; } = subResults ?? [];

    public string? FailureMessage { get; } = failureMessage ?? skipReason;

    public string? StackTrace { get; } = stackTrace;

    public int? AttemptId { get; } = attemptId;

    public bool IsFlaky { get; } = isFlaky;
}

public sealed class ResultAggregator
{
    public IReadOnlyList<AggregatedResult> Aggregate(IEnumerable<IEnumerable<TestResult>>? results)
    {
        if (results is null)
        {
            return [];
        }

        string GetResult(TestResult test)
        {
            if (test.Ignored && string.Equals(test.Result, "Fail", StringComparison.Ordinal))
            {
                return "NotApplicable";
            }

            return test.Result switch
            {
                "Pass" => "Passed",
                "Fail" => "Failed",
                "Skip" => "NotExecuted",
                _ => "None",
            };
        }

        static string ParseBasicName(string name)
        {
            int separatorIndex = name.IndexOf('(');
            return separatorIndex >= 0 ? name[..separatorIndex] : name;
        }

        AggregatedResult CreateResultFromTest(TestResult result, int? attemptId = null)
        {
            return new AggregatedResult(
                AggregationType.Single,
                attemptId is null ? result.Name : $"Attempt #{attemptId} - {result.Name}",
                result.DurationSeconds,
                GetResult(result),
                [],
                result.Attachments,
                result.FailureMessage,
                result.StackTrace,
                result.SkipReason,
                attemptId: attemptId);
        }

        string GetDataDrivenResult(IReadOnlyList<TestResult> groupedResults)
        {
            if (groupedResults.Count == 0)
            {
                return "None";
            }

            if (groupedResults.Any(static r => !r.Ignored && r.Result == "Fail"))
            {
                return "Failed";
            }

            if (groupedResults.Any(static r => r.Result == "Pass"))
            {
                return "Passed";
            }

            return GetResult(groupedResults[0]);
        }

        (bool IsFlaky, string Outcome) GetRerunResult(IReadOnlyList<TestResult> groupedResults)
        {
            if (groupedResults.Count == 0)
            {
                return (false, "None");
            }

            bool anyPass = groupedResults.Any(static r => r.Result == "Pass");
            bool anyFail = groupedResults.Any(static r => !r.Ignored && r.Result == "Fail");
            bool isFlaky = anyPass && anyFail;

            if (anyPass)
            {
                return (isFlaky, "Passed");
            }

            if (anyFail)
            {
                return (isFlaky, "Failed");
            }

            return (false, GetResult(groupedResults[0]));
        }

        AggregatedResult ProcessNamedTest(string name, IReadOnlyList<List<TestResult>> byIterationThenName)
        {
            if (byIterationThenName.Count == 1)
            {
                List<TestResult> singleRun = byIterationThenName[0];
                if (singleRun.Count == 1)
                {
                    return CreateResultFromTest(singleRun[0]);
                }

                return new AggregatedResult(
                    AggregationType.DataDriven,
                    name,
                    singleRun.Sum(static r => r.DurationSeconds),
                    GetDataDrivenResult(singleRun),
                    [.. singleRun.Select(testResult => CreateResultFromTest(testResult))]);
            }

            bool hasDataDriven = byIterationThenName.Any(static x => x.Count > 1);

            if (hasDataDriven)
            {
                var dataDrivenByFullName = new Dictionary<string, List<TestResult>>(StringComparer.Ordinal);
                foreach (List<TestResult> iteration in byIterationThenName)
                {
                    foreach (TestResult test in iteration)
                    {
                        if (!dataDrivenByFullName.TryGetValue(test.Name, out List<TestResult>? list))
                        {
                            list = [];
                            dataDrivenByFullName[test.Name] = list;
                        }

                        list.Add(test);
                    }
                }

                var subResults = new List<AggregatedResult>();
                double totalDuration = 0;

                foreach (KeyValuePair<string, List<TestResult>> pair in dataDrivenByFullName)
                {
                    List<TestResult> dataDrivenTests = pair.Value;
                    if (dataDrivenTests.Count == 1)
                    {
                        subResults.Add(CreateResultFromTest(dataDrivenTests[0]));
                        totalDuration += dataDrivenTests[0].DurationSeconds;
                        continue;
                    }

                    (bool isFlaky, string? aggregateResult) = GetRerunResult(dataDrivenTests);
                    double partialDuration = dataDrivenTests.Sum(static r => r.DurationSeconds);
                    totalDuration += partialDuration;
                    subResults.Add(new AggregatedResult(
                        AggregationType.Rerun,
                        pair.Key,
                        partialDuration,
                        aggregateResult,
                        [.. dataDrivenTests.Select((r, index) => CreateResultFromTest(r, index + 1))],
                        isFlaky: isFlaky));
                }

                string aggregateOutcome = "Inconclusive";
                if (dataDrivenByFullName.Values.Any(rerunSet => rerunSet.Where(static r => !r.Ignored).All(static r => r.Result == "Fail")))
                {
                    aggregateOutcome = "Failed";
                }
                else if (dataDrivenByFullName.Values.All(rerunSet => rerunSet.All(static r => r.Result == "Skip")))
                {
                    aggregateOutcome = "NotExecuted";
                }
                else if (dataDrivenByFullName.Values.All(rerunSet => rerunSet.Any(static r => r.Result == "Pass")))
                {
                    aggregateOutcome = "Passed";
                }

                return new AggregatedResult(
                    AggregationType.DataDriven,
                    name,
                    totalDuration,
                    aggregateOutcome,
                    subResults);
            }

            var reruns = byIterationThenName.Select(static run => run[0]).ToList();
            (bool rerunIsFlaky, string? rerunOutcome) = GetRerunResult(reruns);
            return new AggregatedResult(
                AggregationType.Rerun,
                name,
                reruns.Sum(static r => r.DurationSeconds),
                rerunOutcome,
                [.. reruns.Select((r, index) => CreateResultFromTest(r, index + 1))],
                failureMessage: reruns[0].FailureMessage,
                stackTrace: reruns[0].StackTrace,
                isFlaky: rerunIsFlaky);
        }

        AggregatedResult ReduceSimpleResult(AggregatedResult result)
        {
            if (result.SubResults.Count == 0)
            {
                return result;
            }

            if (result.AggregationType == AggregationType.Rerun)
            {
                int distinctOutcomes = result.SubResults
                    .Select(static r => r.Result)
                    .Distinct(StringComparer.Ordinal)
                    .Count();

                if (distinctOutcomes == 1)
                {
                    AggregatedResult single = result.SubResults[0];
                    return new AggregatedResult(
                        AggregationType.Single,
                        result.Name,
                        single.DurationSeconds,
                        single.Result,
                        attachments: single.Attachments,
                        failureMessage: single.FailureMessage,
                        stackTrace: single.StackTrace);
                }

                return result;
            }

            return new AggregatedResult(
                result.AggregationType,
                result.Name,
                result.DurationSeconds,
                result.Result,
                [.. result.SubResults.Select(ReduceSimpleResult)],
                result.Attachments,
                result.FailureMessage,
                result.StackTrace,
                isFlaky: result.IsFlaky,
                attemptId: result.AttemptId);
        }

        var partials = new List<Dictionary<string, List<TestResult>>>();
        foreach (IEnumerable<TestResult> resultSet in results)
        {
            var perAttempt = new Dictionary<string, List<TestResult>>(StringComparer.Ordinal);
            foreach (TestResult result in resultSet)
            {
                string basicName = ParseBasicName(result.Name);
                if (!perAttempt.TryGetValue(basicName, out List<TestResult>? list))
                {
                    list = [];
                    perAttempt[basicName] = list;
                }

                list.Add(result);
            }

            partials.Add(perAttempt);
        }

        partials = [.. partials.Where(run => run.Count > 0)];

        if (partials.Count == 0)
        {
            return [];
        }

        var aggregate = new List<AggregatedResult>();
        foreach (Dictionary<string, List<TestResult>> run in partials)
        {
            foreach (KeyValuePair<string, List<TestResult>> pair in run.ToList())
            {
                if (!run.Remove(pair.Key, out List<TestResult>? currentSet))
                {
                    continue;
                }

                var fullSet = new List<List<TestResult>> { currentSet };
                foreach (Dictionary<string, List<TestResult>> otherRun in partials)
                {
                    if (ReferenceEquals(otherRun, run))
                    {
                        continue;
                    }

                    if (otherRun.Remove(pair.Key, out List<TestResult>? otherSet))
                    {
                        fullSet.Add(otherSet);
                    }
                }

                aggregate.Add(ProcessNamedTest(pair.Key, fullSet));
            }
        }

        return [.. aggregate.Select(ReduceSimpleResult)];
    }
}
