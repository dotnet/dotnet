// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal interface IVulnerableCapable
    {
        public IReadOnlyList<PackageVulnerabilityMetadataContextInfo>? Vulnerabilities { get; }

        public bool IsVulnerable { get; }

        public PackageVulnerabilitySeverity VulnerabilityMaxSeverity { get; }

        public Task PopulateDataAsync(CancellationToken cancellationToken);
    }
}
