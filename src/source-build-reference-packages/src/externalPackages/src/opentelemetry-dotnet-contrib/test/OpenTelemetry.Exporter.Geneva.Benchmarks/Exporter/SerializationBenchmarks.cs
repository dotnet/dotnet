// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Text;
using BenchmarkDotNet.Attributes;
using OpenTelemetry.Exporter.Geneva.External;
using OpenTelemetry.Exporter.Geneva.MsgPack;
using OpenTelemetry.Exporter.Geneva.Tld;

/*
BenchmarkDotNet v0.13.10, Windows 11 (10.0.23424.1000)
Intel Core i7-9700 CPU 3.00GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


| Method                         | Mean      | Error     | StdDev    | Allocated |
|------------------------------- |----------:|----------:|----------:|----------:|
| TLD_SerializeUInt8             | 16.721 ns | 0.1541 ns | 0.1441 ns |         - |
| MsgPack_SerializeUInt8         |  7.470 ns | 0.0854 ns | 0.0799 ns |         - |
| TLD_SerializeAsciiString       | 25.828 ns | 0.3779 ns | 0.3535 ns |         - |
| MsgPack_SerializeAsciiString   | 16.779 ns | 0.0421 ns | 0.0328 ns |         - |
| TLD_SerializeUnicodeSubString  | 35.224 ns | 0.2928 ns | 0.2596 ns |         - |
| TLD_SerializeUnicodeString     | 33.503 ns | 0.3786 ns | 0.3541 ns |         - |
| MsgPack_SerializeUnicodeString | 19.173 ns | 0.1042 ns | 0.0924 ns |         - |
| TLD_SerializeDateTime          | 36.896 ns | 0.2391 ns | 0.2119 ns |         - |
| MsgPack_SerializeDateTime      | 30.335 ns | 0.1765 ns | 0.1651 ns |         - |
| TLD_Reset                      |  7.856 ns | 0.0453 ns | 0.0379 ns |         - |
*/

namespace OpenTelemetry.Exporter.Geneva.Benchmarks;

[MemoryDiagnoser]
public class SerializationBenchmarks
{
    private const int StringLengthLimit = (1 << 14) - 1;
    private const string Key = "ext_dt_traceId";
    private const string Value = "e8ea7e9ac72de94e91fabc613f9686b2";
    private readonly EventBuilder eventBuilder = new(UncheckedASCIIEncoding.SharedInstance);
    private readonly byte[] buffer = new byte[65360];

    [Benchmark]
    public void TLD_SerializeUInt8()
    {
        this.eventBuilder.Reset("test");
        this.eventBuilder.AddUInt8("Number", 123);
    }

    [Benchmark]
    public void MsgPack_SerializeUInt8()
    {
        var cursor = MessagePackSerializer.SerializeAsciiString(this.buffer, 0, "Number");
        MessagePackSerializer.SerializeUInt8(this.buffer, cursor, 123);
    }

    [Benchmark]
    public void TLD_SerializeAsciiString()
    {
        this.eventBuilder.Reset("test");
        this.eventBuilder.AddCountedString(Key, Value);
    }

    [Benchmark]
    public void MsgPack_SerializeAsciiString()
    {
        var cursor = MessagePackSerializer.SerializeAsciiString(this.buffer, 0, Key);
        MessagePackSerializer.SerializeAsciiString(this.buffer, cursor, Value);
    }

    [Benchmark]
    public void TLD_SerializeUnicodeSubString()
    {
        this.eventBuilder.Reset("test");
        this.eventBuilder.AddCountedAnsiString(Key, Value, Encoding.UTF8, 0, Math.Min(Value.Length, StringLengthLimit));
    }

    [Benchmark]
    public void TLD_SerializeUnicodeString()
    {
        this.eventBuilder.Reset("test");
        this.eventBuilder.AddCountedAnsiString(Key, Value, Encoding.UTF8);
    }

    [Benchmark]
    public void MsgPack_SerializeUnicodeString()
    {
        var cursor = MessagePackSerializer.SerializeAsciiString(this.buffer, 0, Key);
        MessagePackSerializer.SerializeUnicodeString(this.buffer, cursor, Value);
    }

    [Benchmark]
    public void TLD_SerializeDateTime()
    {
        this.eventBuilder.Reset("test");
        this.eventBuilder.AddFileTime("time", DateTime.UtcNow);
    }

    [Benchmark]
    public void MsgPack_SerializeDateTime()
    {
        var cursor = MessagePackSerializer.SerializeAsciiString(this.buffer, 0, "time");
        MessagePackSerializer.SerializeUtcDateTime(this.buffer, cursor, DateTime.UtcNow);
    }

    [Benchmark]
    public void TLD_Reset()
    {
        this.eventBuilder.Reset("test");
    }
}
