// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.Models.Package
{
    internal class DeprecationPackageMetadataCapability : IDeprecationCapable
    {
        private readonly IPackageMetadataRetrievalAdapter _packageMetadataRetrievalAdapter;
        private PackageDeprecationMetadataContextInfo? _deprecationMetadata;

        public DeprecationPackageMetadataCapability(IPackageMetadataRetrievalAdapter packageMetadataRetrievalAdapter)
        {
            _packageMetadataRetrievalAdapter = packageMetadataRetrievalAdapter ?? throw new ArgumentNullException(nameof(packageMetadataRetrievalAdapter));
        }

        public AlternatePackageMetadataContextInfo? AlternatePackage => _deprecationMetadata?.AlternatePackage;

        public bool IsDeprecated => _deprecationMetadata != null;

        public PackageDeprecationReason PackageDeprecationReasons
        {
            get
            {
                if (_deprecationMetadata?.Reasons == null || _deprecationMetadata.Reasons.Count == 0)
                {
                    return PackageDeprecationReason.Unknown;
                }

                bool hasCriticalBugs = false;
                bool hasLegacy = false;

                foreach (var reason in _deprecationMetadata.Reasons)
                {
                    if (string.Equals(reason, PackageDeprecationReasonConstants.CriticalBugs, StringComparison.OrdinalIgnoreCase))
                    {
                        hasCriticalBugs = true;
                    }
                    else if (string.Equals(reason, PackageDeprecationReasonConstants.Legacy, StringComparison.OrdinalIgnoreCase))
                    {
                        hasLegacy = true;
                    }

                    if (hasCriticalBugs && hasLegacy)
                    {
                        return PackageDeprecationReason.LegacyAndCriticalBugs;
                    }
                }

                if (hasCriticalBugs)
                {
                    return PackageDeprecationReason.CriticalBugs;
                }

                if (hasLegacy)
                {
                    return PackageDeprecationReason.Legacy;
                }

                return PackageDeprecationReason.Unknown;
            }
        }

        public PackageDeprecationMetadataContextInfo? DeprecationMetadata => _deprecationMetadata;

        public async Task PopulateDataAsync(CancellationToken cancellationToken)
        {
            _deprecationMetadata = await _packageMetadataRetrievalAdapter.GetPackageDeprecationInfoAsync(cancellationToken);
        }
    }
}
