// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Custom converter for <see cref="ReleaseType"/> enumeration.
    /// </summary>
    internal class ReleaseTypeConverter : JsonConverter<ReleaseType>
    {
        /// <inheritdoc />
        public override ReleaseType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            Enum.TryParse(reader.GetString(), ignoreCase: true, out ReleaseType result) ? result : ReleaseType.Unknown;

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ReleaseType value, JsonSerializerOptions options) =>
            throw new NotImplementedException();
    }
}
