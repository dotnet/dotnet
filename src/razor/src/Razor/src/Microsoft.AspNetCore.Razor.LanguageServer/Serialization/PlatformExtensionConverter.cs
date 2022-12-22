// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Serialization;

internal class PlatformExtensionConverter<TBase, TExtension> : JsonConverter
    where TExtension : TBase
{
    private readonly bool _captureJson;

    public PlatformExtensionConverter()
    {
        if (typeof(ICaptureJson).IsAssignableFrom(typeof(TExtension)))
        {
            _captureJson = true;
        }
    }

    /// <inheritdoc/>
    public override bool CanWrite => false;

    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TBase);
    }

    /// <inheritdoc/>
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (_captureJson)
        {
            var jtoken = JToken.ReadFrom(reader);
            var extension = jtoken.ToObject<TExtension>(serializer);

            if (extension != null)
            {
                var captureJson = (ICaptureJson)extension;
                captureJson.Json = jtoken;
            }

            return extension;
        }
        else
        {
            return serializer.Deserialize<TExtension>(reader);
        }
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
