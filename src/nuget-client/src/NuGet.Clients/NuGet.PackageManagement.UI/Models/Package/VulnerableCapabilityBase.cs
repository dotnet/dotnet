// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal abstract class VulnerableCapabilityBase : IVulnerableCapable
    {
        private IReadOnlyList<PackageVulnerabilityMetadataContextInfo>? _vulnerabilities;

        public IReadOnlyList<PackageVulnerabilityMetadataContextInfo>? Vulnerabilities
        {
            get => _vulnerabilities;
            protected set
            {
                List<PackageVulnerabilityMetadataContextInfo>? sortedList = null;
                if (value != null)
                {
                    sortedList = [.. value];
                    // Sort the list in descending order.
                    sortedList.Sort((b, a) => a.Severity.CompareTo(b.Severity));
                }
                _vulnerabilities = sortedList;
            }
        }

        public bool IsVulnerable => Vulnerabilities?.Count > 0;

        public PackageVulnerabilitySeverity VulnerabilityMaxSeverity
        {
            get
            {
                if (!IsVulnerable || Vulnerabilities is null)
                {
                    return PackageVulnerabilitySeverity.Unknown;
                }

                // Vulnerabilities are ordered on set so the first element is always the highest severity
                int severity = Vulnerabilities[0].Severity;
                if (Enum.IsDefined(typeof(PackageVulnerabilitySeverity), severity))
                {
                    return (PackageVulnerabilitySeverity)severity;
                }
                else
                {
                    return PackageVulnerabilitySeverity.Unknown;
                }
            }
        }

        public abstract Task PopulateDataAsync(CancellationToken cancellationToken);
    }
}
