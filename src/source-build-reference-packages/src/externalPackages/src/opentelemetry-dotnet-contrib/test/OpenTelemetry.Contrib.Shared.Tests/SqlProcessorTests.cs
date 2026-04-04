// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.Instrumentation.Tests;

public class SqlProcessorTests
{
    private readonly ITestOutputHelper output;

    public SqlProcessorTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    public static TheoryData<SqlProcessorTestCases.TestCase> TestData => SqlProcessorTestCases.GetSemanticConventionsTestCases();

    [SkippableTheory]
    [MemberData(nameof(TestData))]
    public void TestGetSanitizedSql(SqlProcessorTestCases.TestCase testCase)
    {
        Skip.IfNot(string.IsNullOrWhiteSpace(testCase.Skip), testCase.Skip);

        this.output.WriteLine($"Input: {testCase.Input.Query}");

        var sqlStatementInfo = SqlProcessor.GetSanitizedSql(testCase.Input.Query);

        var succeeded = false;
        foreach (var sanitizedQueryText in testCase.Expected.SanitizedQueryText)
        {
            if (sqlStatementInfo.SanitizedSql.Equals(sanitizedQueryText))
            {
                succeeded = true;
                break;
            }
        }

        this.output.WriteLine($"Sanitized: {sqlStatementInfo.SanitizedSql}");
        this.output.WriteLine($"Summary: {sqlStatementInfo.DbQuerySummary}");

        Assert.True(
            succeeded,
            $"Expected one of the sanitized query texts to match: {string.Join(", ", testCase.Expected.SanitizedQueryText)} but got: {sqlStatementInfo.SanitizedSql}");

        Assert.Equal(testCase.Expected.Summary, sqlStatementInfo.DbQuerySummary);
    }
}
