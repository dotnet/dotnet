// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
#if NETCOREAPP
using System.Text.Json;
#else
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
#endif

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Converts a json representation of <see cref="KeyValuePair{String,String}"/> to an object.
/// </summary>
internal class CustomKeyValueConverter : TypeConverter
{
#if !NETCOREAPP
    private readonly DataContractJsonSerializer _serializer = new(typeof(TraitObject[]));
#endif

    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return value is KeyValuePair<string, string>[] keyValuePairs
            ? keyValuePairs
            : base.ConvertTo(context, culture, value, destinationType);
    }

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        // KeyValuePairs are used for traits.
        if (value is string data)
        {
#if NETCOREAPP
            // DataContractJsonSerializer relies on reflection metadata for TraitObject
            // that is not preserved when the translation layer is published with NativeAOT.
            using var document = JsonDocument.Parse(data);
            if (document.RootElement.ValueKind == JsonValueKind.Null)
            {
                return Array.Empty<KeyValuePair<string?, string?>>();
            }

            var traits = new KeyValuePair<string?, string?>[document.RootElement.GetArrayLength()];
            int index = 0;
            foreach (var trait in document.RootElement.EnumerateArray())
            {
                string? key = trait.TryGetProperty("Key", out var keyElement) ? keyElement.GetString() : null;
                string? traitValue = trait.TryGetProperty("Value", out var valueElement) ? valueElement.GetString() : null;
                traits[index++] = new KeyValuePair<string?, string?>(key, traitValue);
            }

            return traits;
#else
            // PERF: The values returned here can possibly be cached, but the benefits are very small speed wise,
            // and it is unclear how many distinct objects we get, and how much memory this would consume. I was seeing around 100ms improvement on 10k tests.

            using var stream = new MemoryStream(Encoding.Unicode.GetBytes(data));
            // Converting Json data to array of KeyValuePairs with duplicate keys.
            var listOfTraitObjects = _serializer.ReadObject(stream) as TraitObject[];
            return listOfTraitObjects?.Select(trait => new KeyValuePair<string?, string?>(trait.Key, trait.Value)).ToArray() ?? [];
#endif
        }

        return null;
    }

#if !NETCOREAPP
    [System.Runtime.Serialization.DataContract]
    private class TraitObject
    {
        [System.Runtime.Serialization.DataMember(Name = "Key")]
        public string? Key { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Value")]
        public string? Value { get; set; }
    }
#endif
}
