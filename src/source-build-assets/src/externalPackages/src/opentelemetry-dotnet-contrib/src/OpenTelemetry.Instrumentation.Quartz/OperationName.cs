// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Quartz;

/// <summary>
/// Quartz diagnostic source operation name constants.
/// </summary>
public static class OperationName
{
    /// <summary>
    /// Quartz Job category constants.
    /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
    public static class Job
#pragma warning restore CA1034 // Nested types should not be visible
    {
        /// <summary>
        /// Quartz job execute diagnostic source operation name.
        /// </summary>
        public const string Execute = "Quartz.Job.Execute";

        /// <summary>
        /// Quartz job veto diagnostic source operation name.
        /// </summary>
        public const string Veto = "Quartz.Job.Veto";
    }
}
