// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.RegularExpressions;

namespace Microsoft.TestTemplates.Acceptance.Tests;

public record ExecutionResult(string Arguments, string StandardOutput, string StandardError, int ExitCode);

public static class ExecutionResultExtensions
{
    // this output is specific to the version of TP included with the runtime you are using to run the tests
    // if you see all tests failed, chances are that the output changed after you upgraded to latest version of dotnet
    private const string TestSummaryStatusMessageFormat = "Passed! - Failed: 0, Passed: {1}, Skipped: 0, Total: {0},";

    /// <summary>
    /// Validate if the overall test count and results are matching.
    /// </summary>
    /// <param name="passedTestsCount">Passed test count</param>
    /// <param name="failedTestsCount">Failed test count</param>
    /// <param name="skippedTestsCount">Skipped test count</param>
    public static void ValidateSummaryStatus(this ExecutionResult executionResult, int passedTestsCount, int failedTestsCount, int skippedTestsCount)
    {
        var totalTestCount = passedTestsCount + failedTestsCount + skippedTestsCount;
        if (totalTestCount == 0)
        {
            // No test should be found/run
            var summaryStatus = string.Format(TestSummaryStatusMessageFormat, @"\d+", @"\d+", @"\d+", @"\d+");
            StringAssert.DoesNotMatch(
                executionResult.StandardOutput,
                new Regex(summaryStatus),
                "Excepted: There should not be test summary{2}Actual: {0}{2}Standard Error: {1}{2}Arguments: {3}{2}",
                executionResult.StandardOutput,
                executionResult.StandardError,
                Environment.NewLine,
                executionResult.Arguments);
        }
        else
        {
            var summaryStatus = string.Format(TestSummaryStatusMessageFormat, totalTestCount, passedTestsCount, failedTestsCount, skippedTestsCount);
            StringAssert.Contains(
                executionResult.StandardOutput,
                summaryStatus,
                "The Test summary does not match.{3}Expected summary: {1}{3}Test Output: {0}{3}Standard Error: {2}{3}Arguments: {4}{3}",
                executionResult.StandardOutput,
                summaryStatus,
                executionResult.StandardError,
                Environment.NewLine,
                executionResult.Arguments);
        }
    }
}
