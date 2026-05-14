// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal class VulnerableDatabaseCapability : VulnerableCapabilityBase
    {
        private readonly IPackageVulnerabilityService _vulnerabilityService;
        private readonly PackageIdentity _packageIdentity;

        public VulnerableDatabaseCapability(IPackageVulnerabilityService vulnerabilityService, PackageIdentity packageIdentity)
        {
            _vulnerabilityService = vulnerabilityService ?? throw new ArgumentNullException(nameof(vulnerabilityService));
            _packageIdentity = packageIdentity ?? throw new ArgumentNullException(nameof(packageIdentity));
        }

        public override async Task PopulateDataAsync(CancellationToken cancellationToken)
        {
            Vulnerabilities = await _vulnerabilityService.GetVulnerabilityInfoAsync(_packageIdentity, cancellationToken);
        }
    }
}
