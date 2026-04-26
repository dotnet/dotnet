// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json.Serialization;

namespace OpenTelemetry.Sampler.AWS;

internal class UnprocessedStatistic
{
    public UnprocessedStatistic(string? errorCode, string? message, string? ruleName)
    {
        this.ErrorCode = errorCode;
        this.Message = message;
        this.RuleName = ruleName;
    }

    [JsonPropertyName("ErrorCode")]
    public string? ErrorCode { get; set; }

    [JsonPropertyName("Message")]
    public string? Message { get; set; }

    [JsonPropertyName("RuleName")]
    public string? RuleName { get; set; }
}
