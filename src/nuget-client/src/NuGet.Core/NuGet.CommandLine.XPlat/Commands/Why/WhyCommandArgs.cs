// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.Threading;
using Spectre.Console;

namespace NuGet.CommandLine.XPlat.Commands.Why
{
    internal class WhyCommandArgs
    {
        public required string Path { get; init; }
        public required string Package { get; init; }
        public required List<string> Frameworks { get; init; }
        public required IAnsiConsole Logger { get; init; }
        public required CancellationToken CancellationToken { get; init; }
        public required IDotnetVersionChecker DotnetVersionChecker { get; init; }
    }
}
