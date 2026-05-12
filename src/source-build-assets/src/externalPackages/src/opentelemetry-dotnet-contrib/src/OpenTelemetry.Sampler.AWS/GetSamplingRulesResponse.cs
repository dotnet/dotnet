// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json.Serialization;

namespace OpenTelemetry.Sampler.AWS;

internal sealed class GetSamplingRulesResponse
{
    [JsonPropertyName("NextToken")]
    public string? NextToken { get; set; }

    [JsonPropertyName("SamplingRuleRecords")]
    public List<SamplingRuleRecord>? SamplingRuleRecords { get; set; }
}
