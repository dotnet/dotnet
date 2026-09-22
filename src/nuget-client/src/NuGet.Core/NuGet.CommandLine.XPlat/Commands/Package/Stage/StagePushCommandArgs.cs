// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;

namespace NuGet.CommandLine.XPlat.Commands.Package.Stage
{
    internal sealed class StagePushCommandArgs
    {
        public required string PackagePath { get; init; }
        public string? Source { get; init; }
        public string? ApiKey { get; init; }
        public string? GroupId { get; init; }
        public bool NoSymbols { get; init; }
        public string? ConfigFile { get; init; }
        public bool Interactive { get; init; }
        public required ILoggerWithColor Logger { get; init; }
        public required CancellationToken CancellationToken { get; init; }
    }
}
