// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Model;
using NuGet.Versioning;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update
{
    internal interface IVersionChooser
    {
        Task<NuGetVersion?> GetLatestVersionAsync(
            string packageId,
            ILogger logger,
            CancellationToken cancellationToken);

        Task<NuGetVersion?> GetNonVulnerableAsync(
            string packageId,
            NuGetVersion minVersion,
            ILogger logger,
            IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<PackageVulnerabilityInfo>>> knownVulnerabilities,
            CancellationToken cancellationToken);
    }
}
