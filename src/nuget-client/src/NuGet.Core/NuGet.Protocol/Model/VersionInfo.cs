// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using NuGet.Versioning;

namespace NuGet.Protocol.Core.Types
{
    public class VersionInfo
    {
        public VersionInfo(NuGetVersion version)
            : this(version, new long?())
        {
        }

        public VersionInfo(NuGetVersion version, string? downloadCount)
        {
            Version = version;

            long count;
            if (long.TryParse(downloadCount, out count))
            {
                DownloadCount = count;
            }
        }

        [JsonConstructor]
        public VersionInfo(NuGetVersion version, long? downloadCount)
        {
            Version = version;
            DownloadCount = downloadCount;
        }

        [JsonPropertyName(JsonProperties.Version)]
        public NuGetVersion Version { get; private set; }

        [JsonPropertyName("downloads")]
        public long? DownloadCount { get; private set; }

        /// <summary>
        /// In V2, when finding the list of versions that a package ID has, we also get all of the metadata
        /// associated with each version. It would be wasteful to throw this away, so we store what we have
        /// here. For V3, the metadata property is null. Callers that receive this type need to be able to
        /// fetch this package metadata some other way if this property is null.
        /// </summary>
        [JsonIgnore]
        public IPackageSearchMetadata? PackageSearchMetadata { get; set; }
    }
}
