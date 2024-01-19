// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text.Json.Serialization;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine.XPlat
{
    internal class DetailedVerbosityPackage : NormalVerbosityPackage
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("vulnerable")]
        public bool? Vulnerable { get; set; }

        [JsonPropertyName("deprecation")]
        public string Deprecation { get; set; }

        [JsonPropertyName("projectUrl")]
        public Uri ProjectUrl { get; set; }

        public DetailedVerbosityPackage() : base()
        {
        }

        public DetailedVerbosityPackage(IPackageSearchMetadata packageSearchMetadata, string deprecation) : base(packageSearchMetadata)
        {
            Description = packageSearchMetadata.Description;

            if (packageSearchMetadata.Vulnerabilities != null && packageSearchMetadata.Vulnerabilities.Any())
            {
                Vulnerable = true;
            }

            Deprecation = deprecation;
            ProjectUrl = packageSearchMetadata.ProjectUrl;
        }
    }
}
