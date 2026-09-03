// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Helix.AzureDevOpsTestPublisher.Model;

public sealed class TestResult(
    string name,
    string kind,
    string typeName,
    string method,
    double durationSeconds,
    string result,
    string? exceptionType,
    string? failureMessage,
    string? stackTrace,
    string? skipReason,
    IReadOnlyList<TestResultAttachment>? attachments = null)
{
    public string Name { get; } = name ?? string.Empty;

    public string Kind { get; } = kind ?? string.Empty;

    public string TypeName { get; } = typeName ?? string.Empty;

    public string Method { get; } = method ?? string.Empty;

    public double DurationSeconds { get; } = durationSeconds;

    public string Result { get; } = result ?? string.Empty;

    public string? ExceptionType { get; } = exceptionType;

    public string? FailureMessage { get; } = failureMessage;

    public string? StackTrace { get; } = stackTrace;

    public string? SkipReason { get; } = skipReason;

    public IReadOnlyList<TestResultAttachment> Attachments { get; } = attachments ?? [];

    public bool Ignored { get; set; }
}
