// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Text.Json;
using OpenTelemetry.Internal;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.OneCollector;

internal abstract class CommonSchemaJsonSerializer<T> : ISerializer<T>
    where T : class
{
    private readonly int maxPayloadSizeInBytes;
    private readonly int maxNumberOfItemsPerPayload;
    private readonly string itemType = typeof(T).Name;

    protected CommonSchemaJsonSerializer(
        string tenantToken,
        int maxPayloadSizeInBytes = int.MaxValue,
        int maxNumberOfItemsPerPayload = int.MaxValue)
    {
        Debug.Assert(!string.IsNullOrEmpty(tenantToken), "tenantToken was null or empty.");

        this.maxPayloadSizeInBytes = maxPayloadSizeInBytes;
        this.maxNumberOfItemsPerPayload = maxNumberOfItemsPerPayload;

        this.TenantTokenWithTenancySystemSymbol = JsonEncodedText.Encode($"{CommonSchemaJsonSerializationHelper.OneCollectorTenancySymbol}:{tenantToken}");
    }

    public abstract string Description { get; }

    public OneCollectorExporterSerializationFormatType SerializationFormat => OneCollectorExporterSerializationFormatType.CommonSchemaV4JsonStream;

    protected JsonEncodedText TenantTokenWithTenancySystemSymbol { get; }

    public void SerializeBatchOfItemsToStream(
        Resource resource,
        ref BatchSerializationState<T> state,
        Stream stream,
        int initialSizeOfPayloadInBytes,
        out BatchSerializationResult result)
    {
        Guard.ThrowIfNull(stream);

        var numberOfSerializedItems = 0;
        var numberOfDroppedItems = 0;
        long payloadSizeInBytes = initialSizeOfPayloadInBytes;

        var jsonSerializerState = ThreadStorageHelper.GetCommonSchemaJsonSerializationState(this.itemType, stream);

        var writer = jsonSerializerState.Writer;

        while (state.TryGetNextItem(out var item))
        {
            jsonSerializerState.BeginItem();

            this.SerializeItemToJson(resource, item!, jsonSerializerState);

            var currentItemSizeInBytes = writer.BytesCommitted + writer.BytesPending + 1;

            writer.Flush();
            writer.Reset();

            stream.Write(CommonSchemaJsonSerializationHelper.NewLine, 0, 1);

            if (currentItemSizeInBytes >= this.maxPayloadSizeInBytes)
            {
                // Note: If an individual item cannot fit inside the max size it
                // is dropped.
                numberOfDroppedItems++;
                stream.SetLength(stream.Position - currentItemSizeInBytes);
                continue;
            }

            payloadSizeInBytes += currentItemSizeInBytes;

            if (++numberOfSerializedItems >= this.maxNumberOfItemsPerPayload)
            {
                break;
            }

            if (payloadSizeInBytes >= this.maxPayloadSizeInBytes)
            {
                // Note: If the last item written doesn't fit into the max size
                // it is kept in the buffer and becomes the first item in the
                // next transmission.
                result = new BatchSerializationResult
                {
                    NumberOfItemsSerialized = numberOfSerializedItems,
                    NumberOfItemsDropped = numberOfDroppedItems,
                    PayloadSizeInBytes = payloadSizeInBytes,
                    PayloadOverflowItemSizeInBytes = currentItemSizeInBytes,
                };
                return;
            }
        }

        result = new BatchSerializationResult
        {
            NumberOfItemsSerialized = numberOfSerializedItems,
            NumberOfItemsDropped = numberOfDroppedItems,
            PayloadSizeInBytes = payloadSizeInBytes,
        };
    }

    protected static bool AttributeKeyStartWithExtensionPrefix(string attributeKey)
    {
        return attributeKey.StartsWith("ext.", StringComparison.OrdinalIgnoreCase);
    }

    protected static void SerializeResourceToJsonInsideCurrentObject(Resource resource, CommonSchemaJsonSerializationState serializationState)
    {
        Debug.Assert(resource != null, "resource was null");
        Debug.Assert(serializationState != null, "serializationState was null");

        var writer = serializationState!.Writer;

        Debug.Assert(writer != null, "writer was null");

        if (resource!.Attributes is IReadOnlyList<KeyValuePair<string, object?>> resourceAttributeList)
        {
            for (var i = 0; i < resourceAttributeList.Count; i++)
            {
                var resourceAttribute = resourceAttributeList[i];

                if (AttributeKeyStartWithExtensionPrefix(resourceAttribute.Key))
                {
                    serializationState.AddExtensionAttribute(resourceAttribute);
                    continue;
                }

                CommonSchemaJsonSerializationHelper.SerializeKeyValueToJson(resourceAttribute.Key, resourceAttribute.Value, writer!);
            }
        }
        else
        {
            foreach (var resourceAttribute in resource.Attributes)
            {
                if (AttributeKeyStartWithExtensionPrefix(resourceAttribute.Key))
                {
                    serializationState.AddExtensionAttribute(resourceAttribute!);
                    continue;
                }

                CommonSchemaJsonSerializationHelper.SerializeKeyValueToJson(resourceAttribute.Key, resourceAttribute.Value, writer!);
            }
        }
    }

    protected abstract void SerializeItemToJson(Resource resource, T item, CommonSchemaJsonSerializationState serializationState);
}
