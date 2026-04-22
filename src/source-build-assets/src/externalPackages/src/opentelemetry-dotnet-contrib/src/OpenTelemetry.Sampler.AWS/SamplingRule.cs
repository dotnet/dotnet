// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text.Json.Serialization;

namespace OpenTelemetry.Sampler.AWS;

internal class SamplingRule : IComparable<SamplingRule>
{
    public SamplingRule(
        string ruleName,
        int priority,
        double fixedRate,
        int reservoirSize,
        string host,
        string httpMethod,
        string resourceArn,
        string serviceName,
        string serviceType,
        string urlPath,
        int version,
        Dictionary<string, string> attributes)
    {
        this.RuleName = ruleName;
        this.Priority = priority;
        this.FixedRate = fixedRate;
        this.ReservoirSize = reservoirSize;
        this.Host = host;
        this.HttpMethod = httpMethod;
        this.ResourceArn = resourceArn;
        this.ServiceName = serviceName;
        this.ServiceType = serviceType;
        this.UrlPath = urlPath;
        this.Version = version;
        this.Attributes = attributes;
    }

    [JsonPropertyName("RuleName")]
    public string RuleName { get; set; }

    [JsonPropertyName("Priority")]
    public int Priority { get; set; }

    [JsonPropertyName("FixedRate")]
    public double FixedRate { get; set; }

    [JsonPropertyName("ReservoirSize")]
    public int ReservoirSize { get; set; }

    [JsonPropertyName("Host")]
    public string Host { get; set; }

    [JsonPropertyName("HTTPMethod")]
    public string HttpMethod { get; set; }

    [JsonPropertyName("ResourceARN")]
    public string ResourceArn { get; set; }

    [JsonPropertyName("ServiceName")]
    public string ServiceName { get; set; }

    [JsonPropertyName("ServiceType")]
    public string ServiceType { get; set; }

    [JsonPropertyName("URLPath")]
    public string UrlPath { get; set; }

    [JsonPropertyName("Version")]
    public int Version { get; set; }

    [JsonPropertyName("Attributes")]
    public Dictionary<string, string> Attributes { get; set; }

    public int CompareTo(SamplingRule? other)
    {
        var result = this.Priority.CompareTo(other?.Priority);
        if (result == 0)
        {
            result = string.Compare(this.RuleName, other?.RuleName, StringComparison.Ordinal);
        }

        return result;
    }
}
