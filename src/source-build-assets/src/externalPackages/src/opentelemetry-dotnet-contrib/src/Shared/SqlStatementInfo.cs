// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation;

internal readonly struct SqlStatementInfo
{
    public SqlStatementInfo(string sanitizedSql, string dbQuerySummaryText)
    {
        this.SanitizedSql = sanitizedSql;
        this.DbQuerySummary = dbQuerySummaryText;
    }

    public string SanitizedSql { get; }

    public string DbQuerySummary { get; }
}
