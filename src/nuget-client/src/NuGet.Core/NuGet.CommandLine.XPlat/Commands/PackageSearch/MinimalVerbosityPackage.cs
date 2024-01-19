// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.Json.Serialization;
using NuGet.CommandLine.XPlat.Commands.PackageSearch;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine.XPlat
{
    internal class MinimalVerbosityPackage : ISearchResultPackage
    {
        [JsonPropertyName("id")]
        public string PackageId { get; set; }

        [JsonPropertyName("latestVersion")]
        public string LatestVersion { get; set; }

        public MinimalVerbosityPackage() { }

        public MinimalVerbosityPackage(IPackageSearchMetadata packageSearchMetadata)
        {
            PackageId = packageSearchMetadata.Identity.Id;
            LatestVersion = packageSearchMetadata.Identity.Version.ToString();
        }
    }
}
