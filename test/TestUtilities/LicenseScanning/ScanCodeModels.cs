// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.DotNet.SourceBuild.LicenseScanning;

public sealed class LicenseScanDocument
{
    [JsonPropertyName("files")]
    public List<LicenseScanFile> Files { get; init; } = [];
}

public sealed class LicenseScanFile
{
    [JsonPropertyName("path")]
    public string Path { get; init; } = "";

    [JsonPropertyName("detected_license_expression")]
    public string? LicenseExpression { get; init; }
}

public sealed class ScanCodeDocument
{
    [JsonPropertyName("files")]
    public List<ScanCodeFile> Files { get; init; } = [];
}

public sealed class ScanCodeFile
{
    [JsonPropertyName("path")]
    public string? Path { get; init; }

    [JsonPropertyName("detected_license_expression")]
    public string? LicenseExpression { get; init; }

    [JsonPropertyName("license_detections")]
    public List<ScanCodeLicenseDetection> LicenseDetections { get; init; } = [];
}

public sealed class ScanCodeLicenseDetection
{
    [JsonPropertyName("license_expression")]
    public string? LicenseExpression { get; init; }

    [JsonPropertyName("matches")]
    public List<ScanCodeLicenseMatch> Matches { get; init; } = [];
}

public sealed class ScanCodeLicenseMatch
{
    [JsonPropertyName("license_expression")]
    public string? LicenseExpression { get; init; }

    [JsonPropertyName("rule_identifier")]
    public string? RuleIdentifier { get; init; }

    [JsonPropertyName("matcher")]
    public string? Matcher { get; init; }

    [JsonPropertyName("score")]
    public decimal? Score { get; init; }

    [JsonPropertyName("start_line")]
    public int? StartLine { get; init; }

    [JsonPropertyName("end_line")]
    public int? EndLine { get; init; }

    [JsonPropertyName("matched_text")]
    public string? MatchedText { get; init; }
}
