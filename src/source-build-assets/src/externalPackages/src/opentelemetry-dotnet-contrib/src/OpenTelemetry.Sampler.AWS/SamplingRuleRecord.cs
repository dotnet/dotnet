// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json.Serialization;

namespace OpenTelemetry.Sampler.AWS;

internal class SamplingRuleRecord
{
    [JsonPropertyName("CreatedAt")]
    public double CreatedAt { get; set; }

    [JsonPropertyName("ModifiedAt")]
    public double ModifiedAt { get; set; }

    [JsonPropertyName("SamplingRule")]
    public SamplingRule? SamplingRule { get; set; }
}
