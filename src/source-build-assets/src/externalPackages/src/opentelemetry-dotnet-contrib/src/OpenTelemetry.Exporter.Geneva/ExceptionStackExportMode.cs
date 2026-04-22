// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Exporter.Geneva;

/// <summary>
/// Defines modes for exporting exception stack traces. Currently applicable only to the Logs signal.
/// </summary>
public enum ExceptionStackExportMode
{
    /// <summary>
    /// Exception stack traces are dropped and not exported.
    /// </summary>
    Drop,

    /// <summary>
    /// Exports exception stack traces as a string using the <c>ToString()</c> implementation of the exception.
    /// The output is formatted in a culture-agnostic manner and is primarily designed for human readability.
    /// See <see href="https://learn.microsoft.com/dotnet/api/system.exception.tostring"/>.
    /// </summary>
    /// <remarks>
    /// Typically, <c>ToString()</c> includes information about inner exceptions and additional details,
    /// such as the exception message. However, this behavior is not guaranteed, as custom exceptions
    /// can override <c>ToString()</c> to return arbitrary content.
    /// </remarks>
    ExportAsString,

    /// <summary>
    /// Exports exception stack traces as a string using the <c>StackTrace</c> property of the exception.
    /// See <see href="https://learn.microsoft.com/dotnet/api/system.exception.stacktrace"/>.
    /// </summary>
    /// <remarks>
    /// This represents the raw stack trace and does not include inner exception details.
    /// Note that the <c>StackTrace</c> property can also be overridden in custom exception implementations.
    /// </remarks>
    ExportAsStackTraceString,

    // ExportAsArrayOfStacks - future if stacks can be exported in more structured way
}
