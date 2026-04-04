// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Hangfire.Tests;

/// <summary>
/// Test job class that throws a custom exception type.
/// Used for testing error.type tag cardinality.
/// </summary>
internal class TestJobWithCustomException
{
    public void ThrowCustomException()
    {
        throw new CustomTestException("Test custom exception");
    }
}
