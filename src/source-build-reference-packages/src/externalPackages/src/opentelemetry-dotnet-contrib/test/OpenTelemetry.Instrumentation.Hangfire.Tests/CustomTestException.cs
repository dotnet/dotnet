// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Instrumentation.Hangfire.Tests;

/// <summary>
/// Custom exception type for testing error.type differentiation.
/// </summary>
internal class CustomTestException : Exception
{
    public CustomTestException(string message)
        : base(message)
    {
    }
}
