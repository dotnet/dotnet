// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json.Serialization;

namespace OpenTelemetry.Sampler.AWS;

internal sealed class GetSamplingTargetsResponse
{
    public GetSamplingTargetsResponse(
        double lastRuleModification,
        List<SamplingTargetDocument> samplingTargetDocuments,
        List<UnprocessedStatistic> unprocessedStatistics)
    {
        this.LastRuleModification = lastRuleModification;
        this.SamplingTargetDocuments = samplingTargetDocuments;
        this.UnprocessedStatistics = unprocessedStatistics;
    }

    // This is actually a time in unix seconds.
    [JsonPropertyName("LastRuleModification")]
    public double LastRuleModification { get; set; }

    [JsonPropertyName("SamplingTargetDocuments")]
    public List<SamplingTargetDocument> SamplingTargetDocuments { get; set; }

    [JsonPropertyName("UnprocessedStatistics")]
    public List<UnprocessedStatistic> UnprocessedStatistics { get; set; }
}
