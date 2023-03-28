// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// A <see cref="JsonConverter"/> for a <see cref="ReleaseVersion"/> object.
    /// </summary>
    internal class ReleaseVersionConverter : JsonConverter<ReleaseVersion>
    {
        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ReleaseVersion value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString());

        /// <inheritdoc />
        public override ReleaseVersion Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            ReleaseVersion.Parse(reader.GetString());
    }
}
