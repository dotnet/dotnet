// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using RouteTests.TestApplication;

namespace RouteTests;

public class TestCase
{
    public string Name { get; set; } = string.Empty;

    public int? MinimumDotnetVersion { get; set; }

    public int? MaximumDotnetVersion { get; set; }

    public TestApplicationScenario TestApplicationScenario { get; set; }

    public string? HttpMethod { get; set; }

    public string Path { get; set; } = string.Empty;

    public int ExpectedStatusCode { get; set; }

    public string? ExpectedHttpRoute { get; set; }

    public string? CurrentHttpRoute { get; set; }

    public string? ExpectedMetricRoute { get; set; }

    public override string ToString()
    {
        // This is used by Visual Studio's test runner to identify the test case.
        return $"{this.TestApplicationScenario}: {this.Name}";
    }
}
