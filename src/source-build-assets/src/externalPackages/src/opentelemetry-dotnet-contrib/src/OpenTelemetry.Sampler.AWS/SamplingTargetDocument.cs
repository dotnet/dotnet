// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json.Serialization;

namespace OpenTelemetry.Sampler.AWS;

internal class SamplingTargetDocument
{
    [JsonPropertyName("FixedRate")]
    public double? FixedRate { get; set; }

    [JsonPropertyName("Interval")]
    public long? Interval { get; set; }

    [JsonPropertyName("ReservoirQuota")]
    public long? ReservoirQuota { get; set; }

    [JsonPropertyName("ReservoirQuotaTTL")]
    public double? ReservoirQuotaTTL { get; set; }

    [JsonPropertyName("RuleName")]
    public string? RuleName { get; set; }
}
