// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update
{
    internal record PackageUpdateArgs
    {
        public required string Project { get; init; }

        public required IReadOnlyList<PackageWithVersionRange> Packages { get; init; }

        public required bool Interactive { get; init; }

        public required LogLevel LogLevel { get; init; }

        public required bool Vulnerable { get; init; }
    }
}
