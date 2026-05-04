// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json.Serialization;

namespace OpenTelemetry.Sampler.AWS;

internal class SamplingStatisticsDocument
{
    public SamplingStatisticsDocument(string clientID, string ruleName, long requestCount, long sampledCount, long borrowCount, double timestamp)
    {
        this.ClientID = clientID;
        this.RuleName = ruleName;
        this.RequestCount = requestCount;
        this.SampledCount = sampledCount;
        this.BorrowCount = borrowCount;
        this.Timestamp = timestamp;
    }

    [JsonPropertyName("ClientID")]
    public string ClientID { get; set; }

    [JsonPropertyName("RuleName")]
    public string RuleName { get; set; }

    [JsonPropertyName("RequestCount")]
    public long RequestCount { get; set; }

    [JsonPropertyName("SampledCount")]
    public long SampledCount { get; set; }

    [JsonPropertyName("BorrowCount")]
    public long BorrowCount { get; set; }

    [JsonPropertyName("Timestamp")]
    public double Timestamp { get; set; }
}
