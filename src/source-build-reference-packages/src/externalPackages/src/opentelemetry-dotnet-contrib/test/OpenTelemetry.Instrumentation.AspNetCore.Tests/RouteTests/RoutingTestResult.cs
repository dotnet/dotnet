// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json;
using System.Text.Json.Serialization;
using RouteTests.TestApplication;

namespace RouteTests;

internal abstract class RoutingTestResult
{
    internal static readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

    private readonly string anchorPrefix;

    protected RoutingTestResult(TestCase testCase, string anchorPrefix)
    {
        this.anchorPrefix = anchorPrefix;
        this.TestCase = testCase;
        this.DetailsAnchor = this.GenerateAnchor();
    }

    public string? IdealHttpRoute { get; set; }

    public RouteInfo RouteInfo { get; set; } = new();

    [JsonIgnore]
    public TestCase TestCase { get; private set; }

    [JsonIgnore]
    public string? DetailsAnchor { get; protected set; }

    private string GenerateAnchor()
    {
        var chars = this.TestCase.Name.ToCharArray()
            .Where(c => (!char.IsPunctuation(c) && c != '`') || c == '-')
            .Select(c => c switch
            {
                '-' or ' ' => '-',
                _ => char.ToLowerInvariant(c),
            })
            .ToArray();

#pragma warning disable CA1308 // Normalize strings to uppercase
        return $"{this.anchorPrefix}__{this.TestCase.TestApplicationScenario.ToString().ToLowerInvariant()}-{new string(chars)}";
#pragma warning restore CA1308 // Normalize strings to uppercase
    }
}

#pragma warning disable SA1402 // File may only contain a single type
internal class MetricRoutingTestResult : RoutingTestResult
{
    public MetricRoutingTestResult(TestCase testCase)
        : base(testCase, "metrics")
    {
    }

    public string? MetricHttpRoute { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, JsonSerializerOptions);
    }
}

internal class ActivityRoutingTestResult : RoutingTestResult
{
    public ActivityRoutingTestResult(TestCase testCase)
        : base(testCase, "activity")
    {
    }

    public string ActivityDisplayName { get; set; } = string.Empty;

    public string? ActivityHttpRoute { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, JsonSerializerOptions);
    }
}
#pragma warning restore SA1402 // File may only contain a single type
