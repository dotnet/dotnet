// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Razor.Language;

/// <summary>
///  Struct that holds onto either a dictionary or a <see cref="MetadataCollection"/> for
///  a tag helper builder object.
/// </summary>
internal struct MetadataHolder
{
    private Dictionary<string, string?>? _metadataDictionary;
    private MetadataCollection? _metadataCollection;

    public IDictionary<string, string?> MetadataDictionary
    {
        get
        {
            if (_metadataCollection is not null)
            {
                ThrowMixedMetadataException();
            }

            return _metadataDictionary ??= new Dictionary<string, string?>(StringComparer.Ordinal);
        }
    }

    public void SetMetadataCollection(MetadataCollection metadata)
    {
        if (_metadataDictionary is { Count: > 0 })
        {
            ThrowMixedMetadataException();
        }

        _metadataCollection = metadata;
    }

    [DoesNotReturn]
    private static void ThrowMixedMetadataException()
    {
        throw new InvalidOperationException(
            Resources.Format0_and_1_cannot_both_be_used_for_a_single_builder(nameof(SetMetadataCollection), nameof(MetadataDictionary)));
    }

    public readonly bool TryGetMetadataValue(string key, [NotNullWhen(true)] out string? value)
    {
        if (_metadataCollection is { } metadataCollection)
        {
            return metadataCollection.TryGetValue(key, out value);
        }

        if (_metadataDictionary is { } metadataDictionary)
        {
            return metadataDictionary.TryGetValue(key, out value);
        }

        value = null;
        return false;
    }

    public void Clear()
    {
        _metadataDictionary?.Clear();
        _metadataCollection = null;
    }

    public void AddIfMissing(string key, string? value)
    {
        if (_metadataCollection is { } metadataCollection)
        {
            if (!metadataCollection.ContainsKey(key))
            {
                // We need to maintain a semantic for TagHelperDescriptorBuilder that TagHelperMetadata.Runtime.Name
                // is always included in the metadata. However, if the newer SetMetadata APIs are being used, we
                // would need to create a new MetadataCollection to include new metadata. To avoid this allocation,
                // we will require that the SetMetadata APIs *always* include TagHelperMetadata.Runtime.Name, and
                // throw if they don't.
                throw new InvalidOperationException(
                    Resources.FormatCannot_add_item_with_key_0_to_an_existing_1(key, nameof(MetadataCollection)));
            }
        }
        else // _metadataCollection is null
        {
            var metadataDictionary = _metadataDictionary ??= new Dictionary<string, string?>(StringComparer.Ordinal);

            if (!metadataDictionary.ContainsKey(key))
            {
                metadataDictionary.Add(key, value);
            }
        }
    }

    public readonly MetadataCollection GetMetadataCollection()
        => _metadataCollection ?? MetadataCollection.CreateOrEmpty(_metadataDictionary);
}
