// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update
{
    internal class PackageUpdateArgs
    {
        public required string Project { get; init; }

        public IReadOnlyList<string>? Packages { get; init; }
    }
}
