// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuGet.Protocol.Converters
{
    /// <remarks>
    /// NSJ equivalent: <see cref="SafeUriConverter"/>.
    /// Used by: <see cref="PackageSearchMetadata.LicenseUrl"/>, <see cref="PackageSearchMetadata.ProjectUrl"/>,
    /// <see cref="PackageSearchMetadata.ReadmeUrl"/>, <see cref="PackageVulnerabilityMetadata.AdvisoryUrl"/>.
    /// </remarks>
    internal sealed class SafeUriStjConverter : JsonConverter<Uri>
    {
        public override Uri? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                Uri.TryCreate(reader.GetString()?.Trim(), UriKind.Absolute, out Uri? uri);
                return uri;
            }

            reader.Skip();
            return null;
        }

        public override void Write(Utf8JsonWriter writer, Uri value, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }
    }
}
