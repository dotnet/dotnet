// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Text.Json")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Text.Json")]
[assembly: System.Reflection.AssemblyFileVersion("4.700.19.46214")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.0.0+4ac4c0367003fe3973a3648eb0715ddb0e3bbcea")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Core")]
[assembly: System.Reflection.AssemblyTitle("System.Text.Json")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Text.Json
{
    public enum JsonCommentHandling : byte
    {
        Disallow = 0,
        Skip = 1,
        Allow = 2
    }

    public sealed partial class JsonDocument : IDisposable
    {
        internal JsonDocument() { }

        public JsonElement RootElement { get { throw null; } }

        public void Dispose() { }

        public static JsonDocument Parse(Buffers.ReadOnlySequence<byte> utf8Json, JsonDocumentOptions options = default) { throw null; }

        public static JsonDocument Parse(IO.Stream utf8Json, JsonDocumentOptions options = default) { throw null; }

        public static JsonDocument Parse(ReadOnlyMemory<byte> utf8Json, JsonDocumentOptions options = default) { throw null; }

        public static JsonDocument Parse(ReadOnlyMemory<char> json, JsonDocumentOptions options = default) { throw null; }

        public static JsonDocument Parse(string json, JsonDocumentOptions options = default) { throw null; }

        public static Threading.Tasks.Task<JsonDocument> ParseAsync(IO.Stream utf8Json, JsonDocumentOptions options = default, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static JsonDocument ParseValue(ref Utf8JsonReader reader) { throw null; }

        public static bool TryParseValue(ref Utf8JsonReader reader, out JsonDocument document) { throw null; }

        public void WriteTo(Utf8JsonWriter writer) { }
    }

    public partial struct JsonDocumentOptions
    {
        private int _dummyPrimitive;
        public bool AllowTrailingCommas { get { throw null; } set { } }

        public JsonCommentHandling CommentHandling { get { throw null; } set { } }

        public int MaxDepth { get { throw null; } set { } }
    }

    public readonly partial struct JsonElement
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public JsonElement this[int index] { get { throw null; } }

        public JsonValueKind ValueKind { get { throw null; } }

        public readonly JsonElement Clone() { throw null; }

        public readonly ArrayEnumerator EnumerateArray() { throw null; }

        public readonly ObjectEnumerator EnumerateObject() { throw null; }

        public readonly int GetArrayLength() { throw null; }

        public readonly bool GetBoolean() { throw null; }

        public readonly byte GetByte() { throw null; }

        public readonly byte[] GetBytesFromBase64() { throw null; }

        public readonly DateTime GetDateTime() { throw null; }

        public readonly DateTimeOffset GetDateTimeOffset() { throw null; }

        public readonly decimal GetDecimal() { throw null; }

        public readonly double GetDouble() { throw null; }

        public readonly Guid GetGuid() { throw null; }

        public readonly short GetInt16() { throw null; }

        public readonly int GetInt32() { throw null; }

        public readonly long GetInt64() { throw null; }

        public readonly JsonElement GetProperty(ReadOnlySpan<byte> utf8PropertyName) { throw null; }

        public readonly JsonElement GetProperty(ReadOnlySpan<char> propertyName) { throw null; }

        public readonly JsonElement GetProperty(string propertyName) { throw null; }

        public readonly string GetRawText() { throw null; }

        [CLSCompliant(false)]
        public readonly sbyte GetSByte() { throw null; }

        public readonly float GetSingle() { throw null; }

        public readonly string GetString() { throw null; }

        [CLSCompliant(false)]
        public readonly ushort GetUInt16() { throw null; }

        [CLSCompliant(false)]
        public readonly uint GetUInt32() { throw null; }

        [CLSCompliant(false)]
        public readonly ulong GetUInt64() { throw null; }

        public override readonly string ToString() { throw null; }

        public readonly bool TryGetByte(out byte value) { throw null; }

        public readonly bool TryGetBytesFromBase64(out byte[] value) { throw null; }

        public readonly bool TryGetDateTime(out DateTime value) { throw null; }

        public readonly bool TryGetDateTimeOffset(out DateTimeOffset value) { throw null; }

        public readonly bool TryGetDecimal(out decimal value) { throw null; }

        public readonly bool TryGetDouble(out double value) { throw null; }

        public readonly bool TryGetGuid(out Guid value) { throw null; }

        public readonly bool TryGetInt16(out short value) { throw null; }

        public readonly bool TryGetInt32(out int value) { throw null; }

        public readonly bool TryGetInt64(out long value) { throw null; }

        public readonly bool TryGetProperty(ReadOnlySpan<byte> utf8PropertyName, out JsonElement value) { throw null; }

        public readonly bool TryGetProperty(ReadOnlySpan<char> propertyName, out JsonElement value) { throw null; }

        public readonly bool TryGetProperty(string propertyName, out JsonElement value) { throw null; }

        [CLSCompliant(false)]
        public readonly bool TryGetSByte(out sbyte value) { throw null; }

        public readonly bool TryGetSingle(out float value) { throw null; }

        [CLSCompliant(false)]
        public readonly bool TryGetUInt16(out ushort value) { throw null; }

        [CLSCompliant(false)]
        public readonly bool TryGetUInt32(out uint value) { throw null; }

        [CLSCompliant(false)]
        public readonly bool TryGetUInt64(out ulong value) { throw null; }

        public readonly bool ValueEquals(ReadOnlySpan<byte> utf8Text) { throw null; }

        public readonly bool ValueEquals(ReadOnlySpan<char> text) { throw null; }

        public readonly bool ValueEquals(string text) { throw null; }

        public readonly void WriteTo(Utf8JsonWriter writer) { }

        public partial struct ArrayEnumerator : Collections.Generic.IEnumerable<JsonElement>, Collections.IEnumerable, Collections.Generic.IEnumerator<JsonElement>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public JsonElement Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public ArrayEnumerator GetEnumerator() { throw null; }

            public bool MoveNext() { throw null; }

            public void Reset() { }

            Collections.Generic.IEnumerator<JsonElement> Collections.Generic.IEnumerable<JsonElement>.GetEnumerator() { throw null; }

            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
        }

        public partial struct ObjectEnumerator : Collections.Generic.IEnumerable<JsonProperty>, Collections.IEnumerable, Collections.Generic.IEnumerator<JsonProperty>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public JsonProperty Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public void Dispose() { }

            public ObjectEnumerator GetEnumerator() { throw null; }

            public bool MoveNext() { throw null; }

            public void Reset() { }

            Collections.Generic.IEnumerator<JsonProperty> Collections.Generic.IEnumerable<JsonProperty>.GetEnumerator() { throw null; }

            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
        }
    }

    public readonly partial struct JsonEncodedText : IEquatable<JsonEncodedText>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ReadOnlySpan<byte> EncodedUtf8Bytes { get { throw null; } }

        public static JsonEncodedText Encode(ReadOnlySpan<byte> utf8Value, Encodings.Web.JavaScriptEncoder encoder = null) { throw null; }

        public static JsonEncodedText Encode(ReadOnlySpan<char> value, Encodings.Web.JavaScriptEncoder encoder = null) { throw null; }

        public static JsonEncodedText Encode(string value, Encodings.Web.JavaScriptEncoder encoder = null) { throw null; }

        public override readonly bool Equals(object obj) { throw null; }

        public readonly bool Equals(JsonEncodedText other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public override readonly string ToString() { throw null; }
    }

    public partial class JsonException : Exception
    {
        public JsonException() { }

        protected JsonException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public JsonException(string message, Exception innerException) { }

        public JsonException(string message, string path, long? lineNumber, long? bytePositionInLine, Exception innerException) { }

        public JsonException(string message, string path, long? lineNumber, long? bytePositionInLine) { }

        public JsonException(string message) { }

        public long? BytePositionInLine { get { throw null; } }

        public long? LineNumber { get { throw null; } }

        public override string Message { get { throw null; } }

        public string Path { get { throw null; } }

        public override void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }
    }

    public abstract partial class JsonNamingPolicy
    {
        public static JsonNamingPolicy CamelCase { get { throw null; } }

        public abstract string ConvertName(string name);
    }

    public readonly partial struct JsonProperty
    {
        public string Name { get { throw null; } }

        public JsonElement Value { get { throw null; } }

        public readonly bool NameEquals(ReadOnlySpan<byte> utf8Text) { throw null; }

        public readonly bool NameEquals(ReadOnlySpan<char> text) { throw null; }

        public readonly bool NameEquals(string text) { throw null; }

        public override readonly string ToString() { throw null; }

        public readonly void WriteTo(Utf8JsonWriter writer) { }
    }

    public partial struct JsonReaderOptions
    {
        private int _dummyPrimitive;
        public bool AllowTrailingCommas { get { throw null; } set { } }

        public JsonCommentHandling CommentHandling { get { throw null; } set { } }

        public int MaxDepth { get { throw null; } set { } }
    }

    public partial struct JsonReaderState
    {
        private int _dummyPrimitive;
        public JsonReaderState(JsonReaderOptions options = default) { }

        public JsonReaderOptions Options { get { throw null; } }
    }

    public static partial class JsonSerializer
    {
        public static object Deserialize(ReadOnlySpan<byte> utf8Json, Type returnType, JsonSerializerOptions options = null) { throw null; }

        public static object Deserialize(string json, Type returnType, JsonSerializerOptions options = null) { throw null; }

        public static object Deserialize(ref Utf8JsonReader reader, Type returnType, JsonSerializerOptions options = null) { throw null; }

        public static TValue Deserialize<TValue>(ReadOnlySpan<byte> utf8Json, JsonSerializerOptions options = null) { throw null; }

        public static TValue Deserialize<TValue>(string json, JsonSerializerOptions options = null) { throw null; }

        public static TValue Deserialize<TValue>(ref Utf8JsonReader reader, JsonSerializerOptions options = null) { throw null; }

        public static Threading.Tasks.ValueTask<object> DeserializeAsync(IO.Stream utf8Json, Type returnType, JsonSerializerOptions options = null, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Threading.Tasks.ValueTask<TValue> DeserializeAsync<TValue>(IO.Stream utf8Json, JsonSerializerOptions options = null, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static string Serialize(object value, Type inputType, JsonSerializerOptions options = null) { throw null; }

        public static void Serialize(Utf8JsonWriter writer, object value, Type inputType, JsonSerializerOptions options = null) { }

        public static string Serialize<TValue>(TValue value, JsonSerializerOptions options = null) { throw null; }

        public static void Serialize<TValue>(Utf8JsonWriter writer, TValue value, JsonSerializerOptions options = null) { }

        public static Threading.Tasks.Task SerializeAsync(IO.Stream utf8Json, object value, Type inputType, JsonSerializerOptions options = null, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Threading.Tasks.Task SerializeAsync<TValue>(IO.Stream utf8Json, TValue value, JsonSerializerOptions options = null, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static byte[] SerializeToUtf8Bytes(object value, Type inputType, JsonSerializerOptions options = null) { throw null; }

        public static byte[] SerializeToUtf8Bytes<TValue>(TValue value, JsonSerializerOptions options = null) { throw null; }
    }

    public sealed partial class JsonSerializerOptions
    {
        public bool AllowTrailingCommas { get { throw null; } set { } }

        public Collections.Generic.IList<Serialization.JsonConverter> Converters { get { throw null; } }

        public int DefaultBufferSize { get { throw null; } set { } }

        public JsonNamingPolicy DictionaryKeyPolicy { get { throw null; } set { } }

        public Encodings.Web.JavaScriptEncoder Encoder { get { throw null; } set { } }

        public bool IgnoreNullValues { get { throw null; } set { } }

        public bool IgnoreReadOnlyProperties { get { throw null; } set { } }

        public int MaxDepth { get { throw null; } set { } }

        public bool PropertyNameCaseInsensitive { get { throw null; } set { } }

        public JsonNamingPolicy PropertyNamingPolicy { get { throw null; } set { } }

        public JsonCommentHandling ReadCommentHandling { get { throw null; } set { } }

        public bool WriteIndented { get { throw null; } set { } }

        public Serialization.JsonConverter GetConverter(Type typeToConvert) { throw null; }
    }

    public enum JsonTokenType : byte
    {
        None = 0,
        StartObject = 1,
        EndObject = 2,
        StartArray = 3,
        EndArray = 4,
        PropertyName = 5,
        Comment = 6,
        String = 7,
        Number = 8,
        True = 9,
        False = 10,
        Null = 11
    }

    public enum JsonValueKind : byte
    {
        Undefined = 0,
        Object = 1,
        Array = 2,
        String = 3,
        Number = 4,
        True = 5,
        False = 6,
        Null = 7
    }

    public partial struct JsonWriterOptions
    {
        private object _dummy;
        private int _dummyPrimitive;
        public Encodings.Web.JavaScriptEncoder Encoder { get { throw null; } set { } }

        public bool Indented { get { throw null; } set { } }

        public bool SkipValidation { get { throw null; } set { } }
    }

    public partial struct Utf8JsonReader
    {
        private object _dummy;
        private int _dummyPrimitive;
        public Utf8JsonReader(Buffers.ReadOnlySequence<byte> jsonData, bool isFinalBlock, JsonReaderState state) { }

        public Utf8JsonReader(Buffers.ReadOnlySequence<byte> jsonData, JsonReaderOptions options = default) { }

        public Utf8JsonReader(ReadOnlySpan<byte> jsonData, bool isFinalBlock, JsonReaderState state) { }

        public Utf8JsonReader(ReadOnlySpan<byte> jsonData, JsonReaderOptions options = default) { }

        public long BytesConsumed { get { throw null; } }

        public int CurrentDepth { get { throw null; } }

        public JsonReaderState CurrentState { get { throw null; } }

        public bool HasValueSequence { get { throw null; } }

        public bool IsFinalBlock { get { throw null; } }

        public SequencePosition Position { get { throw null; } }

        public long TokenStartIndex { get { throw null; } }

        public JsonTokenType TokenType { get { throw null; } }

        public Buffers.ReadOnlySequence<byte> ValueSequence { get { throw null; } }

        public ReadOnlySpan<byte> ValueSpan { get { throw null; } }

        public bool GetBoolean() { throw null; }

        public byte GetByte() { throw null; }

        public byte[] GetBytesFromBase64() { throw null; }

        public string GetComment() { throw null; }

        public DateTime GetDateTime() { throw null; }

        public DateTimeOffset GetDateTimeOffset() { throw null; }

        public decimal GetDecimal() { throw null; }

        public double GetDouble() { throw null; }

        public Guid GetGuid() { throw null; }

        public short GetInt16() { throw null; }

        public int GetInt32() { throw null; }

        public long GetInt64() { throw null; }

        [CLSCompliant(false)]
        public sbyte GetSByte() { throw null; }

        public float GetSingle() { throw null; }

        public string GetString() { throw null; }

        [CLSCompliant(false)]
        public ushort GetUInt16() { throw null; }

        [CLSCompliant(false)]
        public uint GetUInt32() { throw null; }

        [CLSCompliant(false)]
        public ulong GetUInt64() { throw null; }

        public bool Read() { throw null; }

        public void Skip() { }

        public bool TryGetByte(out byte value) { throw null; }

        public bool TryGetBytesFromBase64(out byte[] value) { throw null; }

        public bool TryGetDateTime(out DateTime value) { throw null; }

        public bool TryGetDateTimeOffset(out DateTimeOffset value) { throw null; }

        public bool TryGetDecimal(out decimal value) { throw null; }

        public bool TryGetDouble(out double value) { throw null; }

        public bool TryGetGuid(out Guid value) { throw null; }

        public bool TryGetInt16(out short value) { throw null; }

        public bool TryGetInt32(out int value) { throw null; }

        public bool TryGetInt64(out long value) { throw null; }

        [CLSCompliant(false)]
        public bool TryGetSByte(out sbyte value) { throw null; }

        public bool TryGetSingle(out float value) { throw null; }

        [CLSCompliant(false)]
        public bool TryGetUInt16(out ushort value) { throw null; }

        [CLSCompliant(false)]
        public bool TryGetUInt32(out uint value) { throw null; }

        [CLSCompliant(false)]
        public bool TryGetUInt64(out ulong value) { throw null; }

        public bool TrySkip() { throw null; }

        public bool ValueTextEquals(ReadOnlySpan<byte> utf8Text) { throw null; }

        public bool ValueTextEquals(ReadOnlySpan<char> text) { throw null; }

        public bool ValueTextEquals(string text) { throw null; }
    }

    public sealed partial class Utf8JsonWriter : IDisposable, IAsyncDisposable
    {
        public Utf8JsonWriter(Buffers.IBufferWriter<byte> bufferWriter, JsonWriterOptions options = default) { }

        public Utf8JsonWriter(IO.Stream utf8Json, JsonWriterOptions options = default) { }

        public long BytesCommitted { get { throw null; } }

        public int BytesPending { get { throw null; } }

        public int CurrentDepth { get { throw null; } }

        public JsonWriterOptions Options { get { throw null; } }

        public void Dispose() { }

        public Threading.Tasks.ValueTask DisposeAsync() { throw null; }

        public void Flush() { }

        public Threading.Tasks.Task FlushAsync(Threading.CancellationToken cancellationToken = default) { throw null; }

        public void Reset() { }

        public void Reset(Buffers.IBufferWriter<byte> bufferWriter) { }

        public void Reset(IO.Stream utf8Json) { }

        public void WriteBase64String(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<byte> bytes) { }

        public void WriteBase64String(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> bytes) { }

        public void WriteBase64String(string propertyName, ReadOnlySpan<byte> bytes) { }

        public void WriteBase64String(JsonEncodedText propertyName, ReadOnlySpan<byte> bytes) { }

        public void WriteBase64StringValue(ReadOnlySpan<byte> bytes) { }

        public void WriteBoolean(ReadOnlySpan<byte> utf8PropertyName, bool value) { }

        public void WriteBoolean(ReadOnlySpan<char> propertyName, bool value) { }

        public void WriteBoolean(string propertyName, bool value) { }

        public void WriteBoolean(JsonEncodedText propertyName, bool value) { }

        public void WriteBooleanValue(bool value) { }

        public void WriteCommentValue(ReadOnlySpan<byte> utf8Value) { }

        public void WriteCommentValue(ReadOnlySpan<char> value) { }

        public void WriteCommentValue(string value) { }

        public void WriteEndArray() { }

        public void WriteEndObject() { }

        public void WriteNull(ReadOnlySpan<byte> utf8PropertyName) { }

        public void WriteNull(ReadOnlySpan<char> propertyName) { }

        public void WriteNull(string propertyName) { }

        public void WriteNull(JsonEncodedText propertyName) { }

        public void WriteNullValue() { }

        public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, decimal value) { }

        public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, double value) { }

        public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, int value) { }

        public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, long value) { }

        public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, float value) { }

        [CLSCompliant(false)]
        public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, uint value) { }

        [CLSCompliant(false)]
        public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, ulong value) { }

        public void WriteNumber(ReadOnlySpan<char> propertyName, decimal value) { }

        public void WriteNumber(ReadOnlySpan<char> propertyName, double value) { }

        public void WriteNumber(ReadOnlySpan<char> propertyName, int value) { }

        public void WriteNumber(ReadOnlySpan<char> propertyName, long value) { }

        public void WriteNumber(ReadOnlySpan<char> propertyName, float value) { }

        [CLSCompliant(false)]
        public void WriteNumber(ReadOnlySpan<char> propertyName, uint value) { }

        [CLSCompliant(false)]
        public void WriteNumber(ReadOnlySpan<char> propertyName, ulong value) { }

        public void WriteNumber(string propertyName, decimal value) { }

        public void WriteNumber(string propertyName, double value) { }

        public void WriteNumber(string propertyName, int value) { }

        public void WriteNumber(string propertyName, long value) { }

        public void WriteNumber(string propertyName, float value) { }

        [CLSCompliant(false)]
        public void WriteNumber(string propertyName, uint value) { }

        [CLSCompliant(false)]
        public void WriteNumber(string propertyName, ulong value) { }

        public void WriteNumber(JsonEncodedText propertyName, decimal value) { }

        public void WriteNumber(JsonEncodedText propertyName, double value) { }

        public void WriteNumber(JsonEncodedText propertyName, int value) { }

        public void WriteNumber(JsonEncodedText propertyName, long value) { }

        public void WriteNumber(JsonEncodedText propertyName, float value) { }

        [CLSCompliant(false)]
        public void WriteNumber(JsonEncodedText propertyName, uint value) { }

        [CLSCompliant(false)]
        public void WriteNumber(JsonEncodedText propertyName, ulong value) { }

        public void WriteNumberValue(decimal value) { }

        public void WriteNumberValue(double value) { }

        public void WriteNumberValue(int value) { }

        public void WriteNumberValue(long value) { }

        public void WriteNumberValue(float value) { }

        [CLSCompliant(false)]
        public void WriteNumberValue(uint value) { }

        [CLSCompliant(false)]
        public void WriteNumberValue(ulong value) { }

        public void WritePropertyName(ReadOnlySpan<byte> utf8PropertyName) { }

        public void WritePropertyName(ReadOnlySpan<char> propertyName) { }

        public void WritePropertyName(string propertyName) { }

        public void WritePropertyName(JsonEncodedText propertyName) { }

        public void WriteStartArray() { }

        public void WriteStartArray(ReadOnlySpan<byte> utf8PropertyName) { }

        public void WriteStartArray(ReadOnlySpan<char> propertyName) { }

        public void WriteStartArray(string propertyName) { }

        public void WriteStartArray(JsonEncodedText propertyName) { }

        public void WriteStartObject() { }

        public void WriteStartObject(ReadOnlySpan<byte> utf8PropertyName) { }

        public void WriteStartObject(ReadOnlySpan<char> propertyName) { }

        public void WriteStartObject(string propertyName) { }

        public void WriteStartObject(JsonEncodedText propertyName) { }

        public void WriteString(ReadOnlySpan<byte> utf8PropertyName, DateTime value) { }

        public void WriteString(ReadOnlySpan<byte> utf8PropertyName, DateTimeOffset value) { }

        public void WriteString(ReadOnlySpan<byte> utf8PropertyName, Guid value) { }

        public void WriteString(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<byte> utf8Value) { }

        public void WriteString(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<char> value) { }

        public void WriteString(ReadOnlySpan<byte> utf8PropertyName, string value) { }

        public void WriteString(ReadOnlySpan<byte> utf8PropertyName, JsonEncodedText value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, DateTime value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, DateTimeOffset value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, Guid value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> utf8Value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, string value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, JsonEncodedText value) { }

        public void WriteString(string propertyName, DateTime value) { }

        public void WriteString(string propertyName, DateTimeOffset value) { }

        public void WriteString(string propertyName, Guid value) { }

        public void WriteString(string propertyName, ReadOnlySpan<byte> utf8Value) { }

        public void WriteString(string propertyName, ReadOnlySpan<char> value) { }

        public void WriteString(string propertyName, string value) { }

        public void WriteString(string propertyName, JsonEncodedText value) { }

        public void WriteString(JsonEncodedText propertyName, DateTime value) { }

        public void WriteString(JsonEncodedText propertyName, DateTimeOffset value) { }

        public void WriteString(JsonEncodedText propertyName, Guid value) { }

        public void WriteString(JsonEncodedText propertyName, ReadOnlySpan<byte> utf8Value) { }

        public void WriteString(JsonEncodedText propertyName, ReadOnlySpan<char> value) { }

        public void WriteString(JsonEncodedText propertyName, string value) { }

        public void WriteString(JsonEncodedText propertyName, JsonEncodedText value) { }

        public void WriteStringValue(DateTime value) { }

        public void WriteStringValue(DateTimeOffset value) { }

        public void WriteStringValue(Guid value) { }

        public void WriteStringValue(ReadOnlySpan<byte> utf8Value) { }

        public void WriteStringValue(ReadOnlySpan<char> value) { }

        public void WriteStringValue(string value) { }

        public void WriteStringValue(JsonEncodedText value) { }
    }
}

namespace System.Text.Json.Serialization
{
    public abstract partial class JsonAttribute : Attribute
    {
    }

    public abstract partial class JsonConverter
    {
        internal JsonConverter() { }

        public abstract bool CanConvert(Type typeToConvert);
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property, AllowMultiple = false)]
    public partial class JsonConverterAttribute : JsonAttribute
    {
        protected JsonConverterAttribute() { }

        public JsonConverterAttribute(Type converterType) { }

        public Type ConverterType { get { throw null; } }

        public virtual JsonConverter CreateConverter(Type typeToConvert) { throw null; }
    }

    public abstract partial class JsonConverterFactory : JsonConverter
    {
        protected JsonConverterFactory() { }

        public abstract JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options);
    }

    public abstract partial class JsonConverter<T> : JsonConverter
    {
        protected internal JsonConverter() { }

        public override bool CanConvert(Type typeToConvert) { throw null; }

        public abstract T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);
        public abstract void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed partial class JsonExtensionDataAttribute : JsonAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed partial class JsonIgnoreAttribute : JsonAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed partial class JsonPropertyNameAttribute : JsonAttribute
    {
        public JsonPropertyNameAttribute(string name) { }

        public string Name { get { throw null; } }
    }

    public sealed partial class JsonStringEnumConverter : JsonConverterFactory
    {
        public JsonStringEnumConverter() { }

        public JsonStringEnumConverter(JsonNamingPolicy namingPolicy = null, bool allowIntegerValues = true) { }

        public override bool CanConvert(Type typeToConvert) { throw null; }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) { throw null; }
    }
}