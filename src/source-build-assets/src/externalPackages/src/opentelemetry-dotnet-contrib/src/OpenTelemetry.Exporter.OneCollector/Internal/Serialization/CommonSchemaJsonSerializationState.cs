// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
#if NET
using System.Runtime.InteropServices;
#endif
using System.Text.Json;

namespace OpenTelemetry.Exporter.OneCollector;

internal sealed class CommonSchemaJsonSerializationState
{
    public const int MaxNumberOfExtensionKeys = 64;
    public const int MaxNumberOfExtensionValuesPerKey = 16;
    private readonly Dictionary<string, int> keys = new(4, StringComparer.OrdinalIgnoreCase);
    private readonly List<KeyValuePair<ExtensionFieldInformation, object?>> allValues = new(16);
    private string itemType;
    private KeyValueLookup[] keysToAllValuesLookup = new KeyValueLookup[4];
    private int[] valueIndices = new int[4 * MaxNumberOfExtensionValuesPerKey];

    public CommonSchemaJsonSerializationState(string itemType, Utf8JsonWriter writer)
    {
        this.itemType = itemType;
        this.Writer = writer;
    }

    public Utf8JsonWriter Writer { get; private set; }

    public int ExtensionPropertyCount { get; private set; }

    public int ExtensionAttributeCount => this.allValues.Count;

    public void AddExtensionAttribute(KeyValuePair<string, object?> attribute)
    {
        if (!ExtensionFieldInformationManager.SharedCache.TryResolveExtensionFieldInformation(
            attribute.Key,
            out var fieldInformation))
        {
            OneCollectorExporterEventSource.Log.AttributeDropped(this.itemType, attribute.Key, "Invalid extension field name");
            return;
        }

        Debug.Assert(fieldInformation?.ExtensionName != null, "fieldInformation.ExtensionName was null");
        Debug.Assert(fieldInformation?.EncodedExtensionName.EncodedUtf8Bytes.Length > 0, "fieldInformation.EncodedExtensionName was empty");
        Debug.Assert(fieldInformation?.FieldName != null, "fieldInformation.FieldName was null");
        Debug.Assert(fieldInformation?.EncodedFieldName.EncodedUtf8Bytes.Length > 0, "fieldInformation.EncodedFieldName was empty");

#pragma warning disable IDE0370 // Remove unnecessary suppression
        var extensionName = fieldInformation!.ExtensionName!;
#pragma warning restore IDE0370 // Remove unnecessary suppression

#if NET
        ref var lookupIndexRef = ref CollectionsMarshal.GetValueRefOrAddDefault(this.keys, extensionName, out var existed);
        var lookupIndex = lookupIndexRef;
        if (!existed)
        {
            this.AssignNewExtensionToLookupIndex(ref lookupIndexRef);
            lookupIndex = lookupIndexRef;

            if (lookupIndex == -1)
            {
                this.keys.Remove(extensionName);
            }
        }
#else
#pragma warning disable IDE0370 // Suppression is unnecessary
        if (!this.keys.TryGetValue(extensionName, out var lookupIndex))
        {
            this.AssignNewExtensionToLookupIndex(ref lookupIndex);
            if (lookupIndex != -1)
            {
                this.keys[extensionName] = lookupIndex;
            }
        }
#pragma warning restore IDE0370 // Suppression is unnecessary
#endif

        if (lookupIndex == -1)
        {
            OneCollectorExporterEventSource.Log.AttributeDropped(this.itemType, attribute.Key, "Extension limit reached");
            return;
        }

        ref var keyLookup = ref this.keysToAllValuesLookup[lookupIndex];

        if (keyLookup.Count >= MaxNumberOfExtensionValuesPerKey)
        {
            OneCollectorExporterEventSource.Log.AttributeDropped(this.itemType, attribute.Key, "Extension value limit reached");
            return;
        }

        var index = this.allValues.Count;
        this.allValues.Add(new KeyValuePair<ExtensionFieldInformation, object?>(fieldInformation, attribute.Value));
        this.valueIndices[(lookupIndex * MaxNumberOfExtensionValuesPerKey) + keyLookup.Count++] = index;
    }

    public void SerializeExtensionPropertiesToJson(bool writeExtensionObjectEnvelope)
    {
        var writer = this.Writer;

        if (writeExtensionObjectEnvelope)
        {
            writer.WriteStartObject(CommonSchemaJsonSerializationHelper.ExtensionsProperty);
        }

#if NET
        var allValues = CollectionsMarshal.AsSpan(this.allValues);
#else
        var allValues = this.allValues;
#endif

        foreach (var extensionPropertyKey in this.keys)
        {
            var wroteStartObject = false;

            var lookupIndex = extensionPropertyKey.Value;
            if (lookupIndex < 0)
            {
                continue;
            }

            ref var keyLookup = ref this.keysToAllValuesLookup[lookupIndex];
            var valueIndicesOffset = lookupIndex * MaxNumberOfExtensionValuesPerKey;

            for (var i = 0; i < keyLookup.Count; i++)
            {
#if NET
                ref var attribute = ref allValues[this.valueIndices[valueIndicesOffset + i]];
#else
                var attribute = allValues[this.valueIndices[valueIndicesOffset + i]];
#endif
                var fieldInformation = attribute.Key;

                if (!wroteStartObject)
                {
                    writer.WriteStartObject(fieldInformation.EncodedExtensionName);
                    wroteStartObject = true;
                }

                writer.WritePropertyName(fieldInformation.EncodedFieldName);
                CommonSchemaJsonSerializationHelper.SerializeValueToJson(attribute.Value, writer);
            }

            if (wroteStartObject)
            {
                writer.WriteEndObject();
            }
        }

        if (writeExtensionObjectEnvelope)
        {
            writer.WriteEndObject();
        }
    }

    public void Reset(string itemType, Utf8JsonWriter writer)
    {
        this.itemType = itemType;
        this.Writer = writer;
    }

    public void BeginItem()
    {
        if (this.allValues.Count <= 0)
        {
            return;
        }

        for (var i = 0; i < this.ExtensionPropertyCount; i++)
        {
            ref var lookup = ref this.keysToAllValuesLookup[i];
            lookup.Count = 0;
        }

        this.ExtensionPropertyCount = 0;

        this.keys.Clear();

        this.allValues.Clear();
    }

    private void AssignNewExtensionToLookupIndex(ref int lookupIndex)
    {
        lookupIndex = this.ExtensionPropertyCount;

        if (lookupIndex >= this.keysToAllValuesLookup.Length)
        {
            if (this.keysToAllValuesLookup.Length >= MaxNumberOfExtensionKeys)
            {
                lookupIndex = -1;
                return;
            }

            var newLength = this.keysToAllValuesLookup.Length * 2;
            var newKeysToAllValuesLookup = new KeyValueLookup[newLength];
            this.keysToAllValuesLookup.CopyTo(newKeysToAllValuesLookup, 0);
            this.keysToAllValuesLookup = newKeysToAllValuesLookup;

            var newValueIndices = new int[newLength * MaxNumberOfExtensionValuesPerKey];
            this.valueIndices.CopyTo(newValueIndices, 0);
            this.valueIndices = newValueIndices;
        }

        this.ExtensionPropertyCount++;
    }

    private struct KeyValueLookup
    {
        public int Count;
    }
}
