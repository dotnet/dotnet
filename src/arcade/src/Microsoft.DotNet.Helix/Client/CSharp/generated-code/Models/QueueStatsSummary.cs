// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using Newtonsoft.Json;

namespace Microsoft.DotNet.Helix.Client.Models;

public partial class QueueStatsSummary
{
    public QueueStatsSummary()
    {
    }

    [JsonProperty("Organization")]
    public string Organization { get; set; }

    [JsonProperty("Project")]
    public string Project { get; set; }

    [JsonProperty("QueueName")]
    public string QueueName { get; set; }

    [JsonProperty("Depth")]
    public int? Depth { get; set; }

    [JsonProperty("AverageRunDuration")]
    public string AverageRunDuration { get; set; }

    [JsonProperty("P50RunDuration")]
    public string P50RunDuration { get; set; }

    [JsonProperty("P95RunDuration")]
    public string P95RunDuration { get; set; }

    [JsonProperty("P50Wait")]
    public string P50Wait { get; set; }

    [JsonProperty("P95Wait")]
    public string P95Wait { get; set; }

    [JsonProperty("EstimatedWait")]
    public string EstimatedWait { get; set; }

    [JsonProperty("EstimatedWaitMethod")]
    public string EstimatedWaitMethod { get; set; }

    [JsonProperty("EstimatedWaitConfidence")]
    public int? EstimatedWaitConfidence { get; set; }

    [JsonProperty("OnlineMachineCount")]
    public int? OnlineMachineCount { get; set; }

    [JsonProperty("BusyMachineCount")]
    public int? BusyMachineCount { get; set; }

    [JsonProperty("StaleMachineCount")]
    public int? StaleMachineCount { get; set; }

    [JsonProperty("HealthyCores")]
    public double? HealthyCores { get; set; }

    [JsonProperty("ConfiguredMaxCapacity")]
    public double? ConfiguredMaxCapacity { get; set; }

    [JsonProperty("MaxMachines")]
    public int? MaxMachines { get; set; }

    [JsonProperty("ExpectedScaleUpTime")]
    public string ExpectedScaleUpTime { get; set; }

    [JsonProperty("WorkItemStatsSampleCount")]
    public int? WorkItemStatsSampleCount { get; set; }

    [JsonProperty("HeartbeatAge")]
    public string HeartbeatAge { get; set; }

    [JsonProperty("LiveSnapshotAge")]
    public string LiveSnapshotAge { get; set; }

    [JsonProperty("WorkItemStatsAge")]
    public string WorkItemStatsAge { get; set; }

    [JsonProperty("AutoscalerConfigAge")]
    public string AutoscalerConfigAge { get; set; }

    [JsonProperty("HealthSeverity")]
    public int? HealthSeverity { get; set; }

    [JsonProperty("HealthSummary")]
    public string HealthSummary { get; set; }

    [JsonProperty("ObserverUiUrl")]
    public string ObserverUiUrl { get; set; }

    [JsonProperty("ObserverUiPath")]
    public string ObserverUiPath { get; set; }

    [JsonProperty("GeneratedAt")]
    public DateTimeOffset? GeneratedAt { get; set; }
}
