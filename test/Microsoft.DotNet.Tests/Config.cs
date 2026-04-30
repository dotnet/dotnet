// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.Versioning;

namespace Microsoft.DotNet.Tests;

internal static class Config
{
    private const string ConfigSwitchPrefix = "Microsoft.DotNet.Tests.";

    public static string? BinariesReportFile => (string)AppContext.GetData(ConfigSwitchPrefix + nameof(BinariesReportFile))!;
    public static string LogsDirectory => (string)AppContext.GetData(ConfigSwitchPrefix + nameof(LogsDirectory))! ?? throw new InvalidOperationException("Logs directory must be specified");
    public static string? RepoRoot => (string)AppContext.GetData(ConfigSwitchPrefix + nameof(RepoRoot))!;
    public static string? SourceTarballPath => (string)AppContext.GetData(ConfigSwitchPrefix + nameof(SourceTarballPath))!;

    public static string? MsftSdkTarballPath1 => (string?)AppContext.GetData(ConfigSwitchPrefix + nameof(MsftSdkTarballPath1));
    public static string? MsftSdkTarballPath2 => (string?)AppContext.GetData(ConfigSwitchPrefix + nameof(MsftSdkTarballPath2));

    public static string? SourceBuiltSdkTarballPath1 => (string?)AppContext.GetData(ConfigSwitchPrefix + nameof(SourceBuiltSdkTarballPath1));
    public static string? SourceBuiltSdkTarballPath2 => (string?)AppContext.GetData(ConfigSwitchPrefix + nameof(SourceBuiltSdkTarballPath2));
}
