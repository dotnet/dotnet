// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text;
using OpenTelemetry.Resources;
using Xunit;

namespace OpenTelemetry.Exporter.OneCollector.Tests;

// Suppressed because the underscores help with readability. That and artistic
// license!
#pragma warning disable CA1707 // Identifiers should not contain underscores

public class WriteDirectlyToTransportSinkTests
{
    [Fact]
    public void FullDataWrittenToTransportTest()
    {
        var transport = new TestTransport();

        using var sink = new WriteDirectlyToTransportSink<string>(
            new TestSerializer(),
            transport);

        var items = new string[]
        {
            "item1",
            "item2",
            "item3",
        };

        var numberOfRecordsWritten = sink.Write(Resource.Empty, new(items, items.Length));

        Assert.Equal(3, numberOfRecordsWritten);

        var data = transport.ExportedData[0];

        Assert.NotNull(data);

        Assert.Equal("\"item1\"\n\"item2\"\n\"item3\"\n", Encoding.ASCII.GetString(data));

        Assert.Equal(0, sink.Buffer.Length);
    }

    [Fact]
    public void PartialDataWrittenToTransport_MaxItemsPerPayload_Test()
    {
        var transport = new TestTransport();

        using var sink = new WriteDirectlyToTransportSink<string>(
            new TestSerializer(maxNumberOfItemsPerPayload: 2),
            transport);

        var items = new string[]
        {
            "item1",
            "item2",
            "item3",
        };

        var numberOfRecordsWritten = sink.Write(Resource.Empty, new(items, items.Length));

        Assert.Equal(0, sink.Buffer.Length);
        Assert.Equal(3, numberOfRecordsWritten);
        Assert.Equal(2, transport.ExportedData.Count);

        var data = transport.ExportedData[0];

        Assert.NotNull(data);
        Assert.Equal("\"item1\"\n\"item2\"\n", Encoding.ASCII.GetString(data));

        data = transport.ExportedData[1];

        Assert.NotNull(data);
        Assert.Equal("\"item3\"\n", Encoding.ASCII.GetString(data));
    }

    [Fact]
    public void PartialDataWrittenToTransport_MaxPayloadSize_Test()
    {
        var expectedPayloadSizeInBytes = "\"item1\"\n\"item2\"\n".Length;

        var transport = new TestTransport();

        using var sink = new WriteDirectlyToTransportSink<string>(
            new TestSerializer(maxPayloadSizeInBytes: expectedPayloadSizeInBytes + 1),
            transport);

        var items = new string[]
        {
            "item1",
            "item2",
            "item3",
        };

        var numberOfRecordsWritten = sink.Write(Resource.Empty, new(items, items.Length));

        Assert.Equal(0, sink.Buffer.Length);
        Assert.Equal(3, numberOfRecordsWritten);
        Assert.Equal(2, transport.ExportedData.Count);

        var data = transport.ExportedData[0];

        Assert.NotNull(data);
        Assert.Equal(expectedPayloadSizeInBytes, data.Length);
        Assert.Equal("\"item1\"\n\"item2\"\n", Encoding.ASCII.GetString(data));

        data = transport.ExportedData[1];

        Assert.NotNull(data);
        Assert.NotEqual(expectedPayloadSizeInBytes, data.Length);
        Assert.Equal("\"item3\"\n", Encoding.ASCII.GetString(data));

        transport.ExportedData.Clear();

        items =
        [
            "item4",
            "item5"
        ];

        numberOfRecordsWritten = sink.Write(Resource.Empty, new(items, items.Length));

        Assert.Equal(0, sink.Buffer.Length);
        Assert.Equal(2, numberOfRecordsWritten);
        Assert.Single(transport.ExportedData);

        data = transport.ExportedData[0];

        Assert.NotNull(data);
        Assert.Equal(expectedPayloadSizeInBytes, data.Length);
        Assert.Equal("\"item4\"\n\"item5\"\n", Encoding.ASCII.GetString(data));
    }

    [Fact]
    public void SingleItem_MaxPayloadSize_Test()
    {
        var maxPayloadSizeInBytes = 100;

        var transport = new TestTransport();

        using var sink = new WriteDirectlyToTransportSink<string>(
            new TestSerializer(maxPayloadSizeInBytes),
            transport);

        var items = new string[]
        {
            new('a', maxPayloadSizeInBytes - 3), // 3 characters added for "s and \n in JSON
        };

        var numberOfRecordsWritten = sink.Write(Resource.Empty, new(items, items.Length));

        Assert.Equal(0, sink.Buffer.Length);
        Assert.Equal(0, numberOfRecordsWritten);
        Assert.Empty(transport.ExportedData);
    }

    [Fact]
    public void MultipleItems_MaxPayloadSize_Test()
    {
        var expectedPayloadSizeInBytes = "\"item1\"\n\"item2\"\n".Length;

        var maxPayloadSizeInBytes = 100;

        var transport = new TestTransport();

        using var sink = new WriteDirectlyToTransportSink<string>(
            new TestSerializer(maxPayloadSizeInBytes),
            transport);

        var items = new string[]
        {
            "item1",
            new('a', maxPayloadSizeInBytes - 3), // 3 characters added for "s and \n in JSON
            "item3",
        };

        var numberOfRecordsWritten = sink.Write(Resource.Empty, new(items, items.Length));

        Assert.Equal(0, sink.Buffer.Length);
        Assert.Equal(2, numberOfRecordsWritten);
        Assert.Single(transport.ExportedData);

        var data = transport.ExportedData[0];

        Assert.NotNull(data);
        Assert.Equal(expectedPayloadSizeInBytes, data.Length);
        Assert.Equal("\"item1\"\n\"item3\"\n", Encoding.ASCII.GetString(data));
    }

    private sealed class TestSerializer : CommonSchemaJsonSerializer<string>
    {
        public TestSerializer(
            int maxPayloadSizeInBytes = int.MaxValue,
            int maxNumberOfItemsPerPayload = int.MaxValue)
            : base("tenant-token", maxPayloadSizeInBytes, maxNumberOfItemsPerPayload)
        {
        }

        public override string Description => nameof(TestSerializer);

        protected override void SerializeItemToJson(Resource resource, string item, CommonSchemaJsonSerializationState serializationState)
        {
            serializationState.Writer.WriteStringValue(item);
        }
    }

    private sealed class TestTransport : ITransport
    {
        public string Description => nameof(TestTransport);

        public List<byte[]> ExportedData { get; } = [];

        public IDisposable RegisterPayloadTransmittedCallback(OneCollectorExporterPayloadTransmittedCallbackAction callback, bool includeFailures)
        {
            throw new NotImplementedException();
        }

        public bool Send(in TransportSendRequest sendRequest)
        {
            var stream = new MemoryStream();

            sendRequest.ItemStream.CopyTo(stream);

            this.ExportedData.Add(stream.ToArray());

            return true;
        }
    }
}
