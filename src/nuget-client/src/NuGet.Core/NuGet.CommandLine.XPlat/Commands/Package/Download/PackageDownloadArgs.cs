// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat.Commands.Package.PackageDownload;

internal record PackageDownloadArgs
{
    public IReadOnlyList<PackageWithNuGetVersion>? Packages { get; set; }
    public IList<string>? Sources { get; set; }
    public string? OutputDirectory { get; set; }
    public string? ConfigFile { get; set; }
    public bool IncludePrerelease { get; set; }
    public bool AllowInsecureConnections { get; set; }
    public bool Interactive { get; set; }
    public LogLevel LogLevel { get; set; }
}
