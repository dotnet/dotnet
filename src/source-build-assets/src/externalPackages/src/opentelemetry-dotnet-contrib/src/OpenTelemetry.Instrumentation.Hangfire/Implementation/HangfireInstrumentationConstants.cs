// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Hangfire.Implementation;

internal static class HangfireInstrumentationConstants
{
    public const string JobIdTag = "job.id";
    public const string JobCreatedAtTag = "job.createdat";

    public const string ActivityName = "JOB";
    public const string ActivityKey = "opentelemetry_activity_key";
    public const string ActivityContextKey = "opentelemetry_activity_context";
}
