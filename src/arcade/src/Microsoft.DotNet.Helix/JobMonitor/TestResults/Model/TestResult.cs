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

    /// <summary>
    /// Stable, fully qualified test identifier (<c>TypeName.Method</c>) independent of the
    /// framework-provided display name. Used as the AzDO <c>automatedTestName</c> so a test keeps
    /// a consistent identity even when a custom display name is used (xUnit) or the framework only
    /// reports the method name (MSTest). Falls back to the method or display name when type/method
    /// information is unavailable.
    /// </summary>
    public string FullyQualifiedName { get; } =
        !string.IsNullOrEmpty(typeName) && !string.IsNullOrEmpty(method)
            ? $"{typeName}.{method}"
            : !string.IsNullOrEmpty(method)
                ? method
                : name ?? string.Empty;

    public double DurationSeconds { get; } = durationSeconds;

    public string Result { get; } = result ?? string.Empty;

    public string? ExceptionType { get; } = exceptionType;

    public string? FailureMessage { get; } = failureMessage;

    public string? StackTrace { get; } = stackTrace;

    public string? SkipReason { get; } = skipReason;

    public IReadOnlyList<TestResultAttachment> Attachments { get; } = attachments ?? [];

    public bool Ignored { get; set; }
}
