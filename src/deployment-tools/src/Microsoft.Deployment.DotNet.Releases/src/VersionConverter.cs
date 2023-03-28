// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Fault tolerant version converter that trims out leading and trailing white-space before parsing version numbers.
    /// </summary>
    internal class VersionConverter : JsonConverter<Version>
    {
        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Version value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());

        /// <inheritdoc />
        public override Version Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            Version.Parse(reader.GetString().Trim());
    }
}
