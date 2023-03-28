// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Custom converter for <see cref="SupportPhase"/> enumeration.
    /// </summary>
    internal class SupportPhaseConverter : JsonConverter<SupportPhase>
    {
        /// <inheritdoc />
        public override SupportPhase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            Enum.TryParse(reader.GetString().Replace("-", ""), ignoreCase: true, out SupportPhase result) ? result : SupportPhase.Unknown;

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, SupportPhase value, JsonSerializerOptions options) =>
            throw new NotImplementedException();
    }
}
