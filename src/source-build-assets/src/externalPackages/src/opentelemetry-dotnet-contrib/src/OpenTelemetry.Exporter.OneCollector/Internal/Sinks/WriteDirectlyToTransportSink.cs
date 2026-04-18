// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using OpenTelemetry.Internal;
using OpenTelemetry.Resources;

namespace OpenTelemetry.Exporter.OneCollector;

internal sealed class WriteDirectlyToTransportSink<T> : ISink<T>, IDisposable
    where T : class
{
    private readonly string typeName;
    private readonly ISerializer<T> serializer;

    public WriteDirectlyToTransportSink(
        ISerializer<T> serializer,
        ITransport transport,
        int initialBufferCapacity = 8192)
    {
        Guard.ThrowIfNull(serializer);
        Guard.ThrowIfNull(transport);
        Guard.ThrowIfOutOfRange(initialBufferCapacity, min: 0);

        this.typeName = typeof(T).Name;
        this.serializer = serializer;
        this.Transport = transport;
        this.Buffer = new(initialBufferCapacity);
    }

    public string Description => "WriteDirectlyToTransportSink";

    public ITransport Transport { get; }

    internal MemoryStream Buffer { get; }

    public void Dispose()
    {
        (this.serializer as IDisposable)?.Dispose();
        (this.Transport as IDisposable)?.Dispose();
    }

    public int Write(Resource resource, in Batch<T> batch)
    {
        var totalNumberOfItemsSerialized = 0;
        var totalNumberOfItemsDroppedDuringSerialization = 0;
        var totalNumberOfItemsDroppedDueToTransmissionFailure = 0;
        var buffer = this.Buffer;
        ArraySegment<byte> remainingDataFromPreviousTransmission = default;
        var state = new BatchSerializationState<T>(in batch);

        while (true)
        {
            int numberOfItemsToSend;

            if (remainingDataFromPreviousTransmission.Count > 0)
            {
                buffer.Position = 0;
                buffer.Write(
                    remainingDataFromPreviousTransmission.Array!,
                    remainingDataFromPreviousTransmission.Offset,
                    remainingDataFromPreviousTransmission.Count);
                buffer.SetLength(remainingDataFromPreviousTransmission.Count);
                numberOfItemsToSend = 1;
                remainingDataFromPreviousTransmission = default;
            }
            else
            {
                buffer.SetLength(0);
                numberOfItemsToSend = 0;
            }

            this.serializer.SerializeBatchOfItemsToStream(
                resource,
                ref state,
                buffer,
                (int)buffer.Length,
                out var result);

            totalNumberOfItemsDroppedDuringSerialization += result.NumberOfItemsDropped;

            var numberOfItemsSerialized = result.NumberOfItemsSerialized;

            if (numberOfItemsSerialized > 0)
            {
                OneCollectorExporterEventSource.Log.WriteSinkDataWrittenEventIfEnabled(this.typeName, numberOfItemsSerialized, this.Description);

                totalNumberOfItemsSerialized += numberOfItemsSerialized;
                numberOfItemsToSend += numberOfItemsSerialized;
            }
            else if (numberOfItemsToSend <= 0)
            {
                break;
            }

            if (result.PayloadOverflowItemSizeInBytes.HasValue)
            {
                var hasUnderlyingBuffer = buffer.TryGetBuffer(out var underlyingBuffer);
                Debug.Assert(hasUnderlyingBuffer, "Could not access underlying buffer");

                var endPositionOfValidMessages = (int)(result.PayloadSizeInBytes - result.PayloadOverflowItemSizeInBytes);

                remainingDataFromPreviousTransmission = new ArraySegment<byte>(
                    underlyingBuffer.Array!,
                    endPositionOfValidMessages,
                    (int)result.PayloadOverflowItemSizeInBytes.Value);

                buffer.SetLength(endPositionOfValidMessages);

                numberOfItemsToSend--;
            }

            buffer.Position = 0;

            if (!this.Transport.Send(
                new TransportSendRequest
                {
                    ItemType = this.typeName,
                    ItemSerializationFormat = this.serializer.SerializationFormat,
                    ItemStream = buffer,
                    NumberOfItems = numberOfItemsToSend,
                }))
            {
                totalNumberOfItemsDroppedDueToTransmissionFailure += numberOfItemsToSend;
            }
        }

        if (totalNumberOfItemsDroppedDuringSerialization > 0 || totalNumberOfItemsDroppedDueToTransmissionFailure > 0)
        {
            OneCollectorExporterEventSource.Log.DataDropped(
                this.typeName,
                totalNumberOfItemsDroppedDuringSerialization + totalNumberOfItemsDroppedDueToTransmissionFailure,
                totalNumberOfItemsDroppedDuringSerialization,
                totalNumberOfItemsDroppedDueToTransmissionFailure);
        }

        return totalNumberOfItemsSerialized;
    }
}
