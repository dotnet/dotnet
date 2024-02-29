// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.CommandLine.XPlat
{
    internal class SearchResultPackagesConverter : JsonConverter<IPackageSearchMetadata>
    {
        private readonly PackageSearchVerbosity _verbosity;
        private readonly bool _exactMatch;

        public SearchResultPackagesConverter(PackageSearchVerbosity verbosity, bool exactMatch)
        {
            _verbosity = verbosity;
            _exactMatch = exactMatch;
        }

        public override IPackageSearchMetadata Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IPackageSearchMetadata value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(JsonProperties.PackageId, value.Identity.Id);

            if (_exactMatch)
            {
                writer.WriteString(JsonProperties.Version, value.Identity.Version.ToNormalizedString());
            }
            else
            {
                writer.WriteString(JsonProperties.LatestVersion, value.Identity.Version.ToNormalizedString());
            }

            if (_verbosity == PackageSearchVerbosity.Normal || _verbosity == PackageSearchVerbosity.Detailed)
            {
                if (value.DownloadCount.HasValue)
                {
                    writer.WriteNumber(JsonProperties.DownloadCount, (decimal)value.DownloadCount);
                }

                if (value.Owners is not null)
                {
                    writer.WriteString(JsonProperties.Owners, value.Owners);
                }
            }

            if (_verbosity == PackageSearchVerbosity.Detailed)
            {
                writer.WriteString(JsonProperties.Description, value.Description);

                if (value.Vulnerabilities != null && value.Vulnerabilities.Any())
                {
                    writer.WriteBoolean("vulnerable", true);
                }

                writer.WriteString(JsonProperties.ProjectUrl, value.ProjectUrl.ToString());

                PackageDeprecationMetadata packageDeprecationMetadata = value.GetDeprecationMetadataAsync().Result;

                if (packageDeprecationMetadata != null)
                {
                    writer.WriteString(JsonProperties.Deprecation, packageDeprecationMetadata.Message);
                }
            }

            writer.WriteEndObject();
        }
    }
}
