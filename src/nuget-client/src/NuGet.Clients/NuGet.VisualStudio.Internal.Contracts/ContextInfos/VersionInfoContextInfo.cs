// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public sealed class VersionInfoContextInfo
    {
        public VersionInfoContextInfo(NuGetVersion version) : this(version, new long?())
        {
        }

        public VersionInfoContextInfo(NuGetVersion version, long? downloadCount)
        {
            Version = version;
            DownloadCount = downloadCount;
        }

        public NuGetVersion Version { get; }
        public long? DownloadCount { get; }

        public PackageDeprecationMetadataContextInfo? PackageDeprecationMetadata { get; internal set; }

        public PackageSearchMetadataContextInfo? PackageSearchMetadata { get; internal set; }

        public static async ValueTask<VersionInfoContextInfo> CreateAsync(VersionInfo versionInfo)
        {
            Assumes.NotNull(versionInfo);

            var versionContextInfo = new VersionInfoContextInfo(versionInfo.Version, versionInfo.DownloadCount);

            if (versionInfo.PackageSearchMetadata != null)
            {
                var packageSearchMetadataContextInfo = PackageSearchMetadataContextInfo.Create(versionInfo.PackageSearchMetadata);
                versionContextInfo.PackageSearchMetadata = packageSearchMetadataContextInfo;
                PackageDeprecationMetadata? packageDeprecationMetadata = await versionInfo.PackageSearchMetadata.GetDeprecationMetadataAsync();
                if (packageDeprecationMetadata != null)
                {
                    versionContextInfo.PackageDeprecationMetadata = PackageDeprecationMetadataContextInfo.Create(packageDeprecationMetadata);
                }
            }

            return versionContextInfo;
        }
    }
}
