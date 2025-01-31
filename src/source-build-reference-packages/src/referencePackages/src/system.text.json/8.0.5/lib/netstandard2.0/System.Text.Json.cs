// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = ".NET Standard 2.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Text.Json")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides high-performance and low-allocating types that serialize objects to JavaScript Object Notation (JSON) text and deserialize JSON text to objects, with UTF-8 support built-in. Also provides types to read and write JSON text encoded as UTF-8, and to create an in-memory document object model (DOM), that is read-only, for random access of the JSON elements within a structured view of the data.\r\n\r\nThe System.Text.Json library is built-in as part of the shared framework in .NET Runtime. The package can be installed when you need to use it in other target frameworks.")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.1024.46610")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.10+81cabf2857a01351e5ab578947c7403a5b128ad1")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Text.Json")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
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

        public static bool TryParseValue(ref Utf8JsonReader reader, out JsonDocument? document) { throw null; }

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

        public readonly string? GetString() { throw null; }

        [CLSCompliant(false)]
        public readonly ushort GetUInt16() { throw null; }

        [CLSCompliant(false)]
        public readonly uint GetUInt32() { throw null; }

        [CLSCompliant(false)]
        public readonly ulong GetUInt64() { throw null; }

        public static JsonElement ParseValue(ref Utf8JsonReader reader) { throw null; }

        public override readonly string ToString() { throw null; }

        public readonly bool TryGetByte(out byte value) { throw null; }

        public readonly bool TryGetBytesFromBase64(out byte[]? value) { throw null; }

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

        public static bool TryParseValue(ref Utf8JsonReader reader, out JsonElement? element) { throw null; }

        public readonly bool ValueEquals(ReadOnlySpan<byte> utf8Text) { throw null; }

        public readonly bool ValueEquals(ReadOnlySpan<char> text) { throw null; }

        public readonly bool ValueEquals(string? text) { throw null; }

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

        public string Value { get { throw null; } }

        public static JsonEncodedText Encode(ReadOnlySpan<byte> utf8Value, Encodings.Web.JavaScriptEncoder? encoder = null) { throw null; }

        public static JsonEncodedText Encode(ReadOnlySpan<char> value, Encodings.Web.JavaScriptEncoder? encoder = null) { throw null; }

        public static JsonEncodedText Encode(string value, Encodings.Web.JavaScriptEncoder? encoder = null) { throw null; }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(JsonEncodedText other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public override readonly string ToString() { throw null; }
    }

    public partial class JsonException : Exception
    {
        public JsonException() { }

        protected JsonException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public JsonException(string? message, Exception? innerException) { }

        public JsonException(string? message, string? path, long? lineNumber, long? bytePositionInLine, Exception? innerException) { }

        public JsonException(string? message, string? path, long? lineNumber, long? bytePositionInLine) { }

        public JsonException(string? message) { }

        public long? BytePositionInLine { get { throw null; } }

        public long? LineNumber { get { throw null; } }

        public override string Message { get { throw null; } }

        public string? Path { get { throw null; } }

        public override void GetObjectData(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }
    }

    public abstract partial class JsonNamingPolicy
    {
        public static JsonNamingPolicy CamelCase { get { throw null; } }

        public static JsonNamingPolicy KebabCaseLower { get { throw null; } }

        public static JsonNamingPolicy KebabCaseUpper { get { throw null; } }

        public static JsonNamingPolicy SnakeCaseLower { get { throw null; } }

        public static JsonNamingPolicy SnakeCaseUpper { get { throw null; } }

        public abstract string ConvertName(string name);
    }

    public readonly partial struct JsonProperty
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public string Name { get { throw null; } }

        public JsonElement Value { get { throw null; } }

        public readonly bool NameEquals(ReadOnlySpan<byte> utf8Text) { throw null; }

        public readonly bool NameEquals(ReadOnlySpan<char> text) { throw null; }

        public readonly bool NameEquals(string? text) { throw null; }

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
        public static bool IsReflectionEnabledByDefault { get { throw null; } }

        public static object? Deserialize(IO.Stream utf8Json, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static object? Deserialize(IO.Stream utf8Json, Type returnType, JsonSerializerOptions? options = null) { throw null; }

        public static object? Deserialize(IO.Stream utf8Json, Type returnType, Serialization.JsonSerializerContext context) { throw null; }

        public static object? Deserialize(ReadOnlySpan<byte> utf8Json, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static object? Deserialize(ReadOnlySpan<byte> utf8Json, Type returnType, JsonSerializerOptions? options = null) { throw null; }

        public static object? Deserialize(ReadOnlySpan<byte> utf8Json, Type returnType, Serialization.JsonSerializerContext context) { throw null; }

        public static object? Deserialize(ReadOnlySpan<char> json, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static object? Deserialize(ReadOnlySpan<char> json, Type returnType, JsonSerializerOptions? options = null) { throw null; }

        public static object? Deserialize(ReadOnlySpan<char> json, Type returnType, Serialization.JsonSerializerContext context) { throw null; }

        public static object? Deserialize(string json, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static object? Deserialize(string json, Type returnType, JsonSerializerOptions? options = null) { throw null; }

        public static object? Deserialize(string json, Type returnType, Serialization.JsonSerializerContext context) { throw null; }

        public static object? Deserialize(this JsonDocument document, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static object? Deserialize(this JsonDocument document, Type returnType, JsonSerializerOptions? options = null) { throw null; }

        public static object? Deserialize(this JsonDocument document, Type returnType, Serialization.JsonSerializerContext context) { throw null; }

        public static object? Deserialize(this JsonElement element, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static object? Deserialize(this JsonElement element, Type returnType, JsonSerializerOptions? options = null) { throw null; }

        public static object? Deserialize(this JsonElement element, Type returnType, Serialization.JsonSerializerContext context) { throw null; }

        public static object? Deserialize(this Nodes.JsonNode? node, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static object? Deserialize(this Nodes.JsonNode? node, Type returnType, JsonSerializerOptions? options = null) { throw null; }

        public static object? Deserialize(this Nodes.JsonNode? node, Type returnType, Serialization.JsonSerializerContext context) { throw null; }

        public static object? Deserialize(ref Utf8JsonReader reader, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static object? Deserialize(ref Utf8JsonReader reader, Type returnType, JsonSerializerOptions? options = null) { throw null; }

        public static object? Deserialize(ref Utf8JsonReader reader, Type returnType, Serialization.JsonSerializerContext context) { throw null; }

        public static TValue? Deserialize<TValue>(IO.Stream utf8Json, JsonSerializerOptions? options = null) { throw null; }

        public static TValue? Deserialize<TValue>(IO.Stream utf8Json, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static TValue? Deserialize<TValue>(ReadOnlySpan<byte> utf8Json, JsonSerializerOptions? options = null) { throw null; }

        public static TValue? Deserialize<TValue>(ReadOnlySpan<byte> utf8Json, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static TValue? Deserialize<TValue>(ReadOnlySpan<char> json, JsonSerializerOptions? options = null) { throw null; }

        public static TValue? Deserialize<TValue>(ReadOnlySpan<char> json, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static TValue? Deserialize<TValue>(string json, JsonSerializerOptions? options = null) { throw null; }

        public static TValue? Deserialize<TValue>(string json, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static TValue? Deserialize<TValue>(this JsonDocument document, JsonSerializerOptions? options = null) { throw null; }

        public static TValue? Deserialize<TValue>(this JsonDocument document, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static TValue? Deserialize<TValue>(this JsonElement element, JsonSerializerOptions? options = null) { throw null; }

        public static TValue? Deserialize<TValue>(this JsonElement element, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static TValue? Deserialize<TValue>(this Nodes.JsonNode? node, JsonSerializerOptions? options = null) { throw null; }

        public static TValue? Deserialize<TValue>(this Nodes.JsonNode? node, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static TValue? Deserialize<TValue>(ref Utf8JsonReader reader, JsonSerializerOptions? options = null) { throw null; }

        public static TValue? Deserialize<TValue>(ref Utf8JsonReader reader, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static Threading.Tasks.ValueTask<object?> DeserializeAsync(IO.Stream utf8Json, Serialization.Metadata.JsonTypeInfo jsonTypeInfo, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Threading.Tasks.ValueTask<object?> DeserializeAsync(IO.Stream utf8Json, Type returnType, JsonSerializerOptions? options = null, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Threading.Tasks.ValueTask<object?> DeserializeAsync(IO.Stream utf8Json, Type returnType, Serialization.JsonSerializerContext context, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Threading.Tasks.ValueTask<TValue?> DeserializeAsync<TValue>(IO.Stream utf8Json, JsonSerializerOptions? options = null, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Threading.Tasks.ValueTask<TValue?> DeserializeAsync<TValue>(IO.Stream utf8Json, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Collections.Generic.IAsyncEnumerable<TValue?> DeserializeAsyncEnumerable<TValue>(IO.Stream utf8Json, JsonSerializerOptions? options = null, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Collections.Generic.IAsyncEnumerable<TValue?> DeserializeAsyncEnumerable<TValue>(IO.Stream utf8Json, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static void Serialize(IO.Stream utf8Json, object? value, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { }

        public static void Serialize(IO.Stream utf8Json, object? value, Type inputType, JsonSerializerOptions? options = null) { }

        public static void Serialize(IO.Stream utf8Json, object? value, Type inputType, Serialization.JsonSerializerContext context) { }

        public static string Serialize(object? value, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static string Serialize(object? value, Type inputType, JsonSerializerOptions? options = null) { throw null; }

        public static string Serialize(object? value, Type inputType, Serialization.JsonSerializerContext context) { throw null; }

        public static void Serialize(Utf8JsonWriter writer, object? value, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { }

        public static void Serialize(Utf8JsonWriter writer, object? value, Type inputType, JsonSerializerOptions? options = null) { }

        public static void Serialize(Utf8JsonWriter writer, object? value, Type inputType, Serialization.JsonSerializerContext context) { }

        public static string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null) { throw null; }

        public static string Serialize<TValue>(TValue value, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static void Serialize<TValue>(IO.Stream utf8Json, TValue value, JsonSerializerOptions? options = null) { }

        public static void Serialize<TValue>(IO.Stream utf8Json, TValue value, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { }

        public static void Serialize<TValue>(Utf8JsonWriter writer, TValue value, JsonSerializerOptions? options = null) { }

        public static void Serialize<TValue>(Utf8JsonWriter writer, TValue value, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { }

        public static Threading.Tasks.Task SerializeAsync(IO.Stream utf8Json, object? value, Serialization.Metadata.JsonTypeInfo jsonTypeInfo, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Threading.Tasks.Task SerializeAsync(IO.Stream utf8Json, object? value, Type inputType, JsonSerializerOptions? options = null, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Threading.Tasks.Task SerializeAsync(IO.Stream utf8Json, object? value, Type inputType, Serialization.JsonSerializerContext context, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Threading.Tasks.Task SerializeAsync<TValue>(IO.Stream utf8Json, TValue value, JsonSerializerOptions? options = null, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static Threading.Tasks.Task SerializeAsync<TValue>(IO.Stream utf8Json, TValue value, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo, Threading.CancellationToken cancellationToken = default) { throw null; }

        public static JsonDocument SerializeToDocument(object? value, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static JsonDocument SerializeToDocument(object? value, Type inputType, JsonSerializerOptions? options = null) { throw null; }

        public static JsonDocument SerializeToDocument(object? value, Type inputType, Serialization.JsonSerializerContext context) { throw null; }

        public static JsonDocument SerializeToDocument<TValue>(TValue value, JsonSerializerOptions? options = null) { throw null; }

        public static JsonDocument SerializeToDocument<TValue>(TValue value, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static JsonElement SerializeToElement(object? value, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static JsonElement SerializeToElement(object? value, Type inputType, JsonSerializerOptions? options = null) { throw null; }

        public static JsonElement SerializeToElement(object? value, Type inputType, Serialization.JsonSerializerContext context) { throw null; }

        public static JsonElement SerializeToElement<TValue>(TValue value, JsonSerializerOptions? options = null) { throw null; }

        public static JsonElement SerializeToElement<TValue>(TValue value, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static Nodes.JsonNode? SerializeToNode(object? value, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static Nodes.JsonNode? SerializeToNode(object? value, Type inputType, JsonSerializerOptions? options = null) { throw null; }

        public static Nodes.JsonNode? SerializeToNode(object? value, Type inputType, Serialization.JsonSerializerContext context) { throw null; }

        public static Nodes.JsonNode? SerializeToNode<TValue>(TValue value, JsonSerializerOptions? options = null) { throw null; }

        public static Nodes.JsonNode? SerializeToNode<TValue>(TValue value, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }

        public static byte[] SerializeToUtf8Bytes(object? value, Serialization.Metadata.JsonTypeInfo jsonTypeInfo) { throw null; }

        public static byte[] SerializeToUtf8Bytes(object? value, Type inputType, JsonSerializerOptions? options = null) { throw null; }

        public static byte[] SerializeToUtf8Bytes(object? value, Type inputType, Serialization.JsonSerializerContext context) { throw null; }

        public static byte[] SerializeToUtf8Bytes<TValue>(TValue value, JsonSerializerOptions? options = null) { throw null; }

        public static byte[] SerializeToUtf8Bytes<TValue>(TValue value, Serialization.Metadata.JsonTypeInfo<TValue> jsonTypeInfo) { throw null; }
    }

    public enum JsonSerializerDefaults
    {
        General = 0,
        Web = 1
    }

    public sealed partial class JsonSerializerOptions
    {
        public JsonSerializerOptions() { }

        public JsonSerializerOptions(JsonSerializerDefaults defaults) { }

        public JsonSerializerOptions(JsonSerializerOptions options) { }

        public bool AllowTrailingCommas { get { throw null; } set { } }

        public Collections.Generic.IList<Serialization.JsonConverter> Converters { get { throw null; } }

        public static JsonSerializerOptions Default { get { throw null; } }

        public int DefaultBufferSize { get { throw null; } set { } }

        public Serialization.JsonIgnoreCondition DefaultIgnoreCondition { get { throw null; } set { } }

        public JsonNamingPolicy? DictionaryKeyPolicy { get { throw null; } set { } }

        public Encodings.Web.JavaScriptEncoder? Encoder { get { throw null; } set { } }

        public bool IgnoreNullValues { get { throw null; } set { } }

        public bool IgnoreReadOnlyFields { get { throw null; } set { } }

        public bool IgnoreReadOnlyProperties { get { throw null; } set { } }

        public bool IncludeFields { get { throw null; } set { } }

        public bool IsReadOnly { get { throw null; } }

        public int MaxDepth { get { throw null; } set { } }

        public Serialization.JsonNumberHandling NumberHandling { get { throw null; } set { } }

        public Serialization.JsonObjectCreationHandling PreferredObjectCreationHandling { get { throw null; } set { } }

        public bool PropertyNameCaseInsensitive { get { throw null; } set { } }

        public JsonNamingPolicy? PropertyNamingPolicy { get { throw null; } set { } }

        public JsonCommentHandling ReadCommentHandling { get { throw null; } set { } }

        public Serialization.ReferenceHandler? ReferenceHandler { get { throw null; } set { } }

        public Serialization.Metadata.IJsonTypeInfoResolver? TypeInfoResolver { get { throw null; } set { } }

        public Collections.Generic.IList<Serialization.Metadata.IJsonTypeInfoResolver> TypeInfoResolverChain { get { throw null; } }

        public Serialization.JsonUnknownTypeHandling UnknownTypeHandling { get { throw null; } set { } }

        public Serialization.JsonUnmappedMemberHandling UnmappedMemberHandling { get { throw null; } set { } }

        public bool WriteIndented { get { throw null; } set { } }

        public void AddContext<TContext>()
            where TContext : Serialization.JsonSerializerContext, new() { }

        public Serialization.JsonConverter GetConverter(Type typeToConvert) { throw null; }

        public Serialization.Metadata.JsonTypeInfo GetTypeInfo(Type type) { throw null; }

        public void MakeReadOnly() { }

        public void MakeReadOnly(bool populateMissingResolver) { }

        public bool TryGetTypeInfo(Type type, out Serialization.Metadata.JsonTypeInfo? typeInfo) { throw null; }
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
        public Encodings.Web.JavaScriptEncoder? Encoder { get { throw null; } set { } }

        public bool Indented { get { throw null; } set { } }

        public int MaxDepth { get { throw null; } set { } }

        public bool SkipValidation { get { throw null; } set { } }
    }

    public ref partial struct Utf8JsonReader
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

        public bool ValueIsEscaped { get { throw null; } }

        public Buffers.ReadOnlySequence<byte> ValueSequence { get { throw null; } }

        public ReadOnlySpan<byte> ValueSpan { get { throw null; } }

        public readonly int CopyString(Span<byte> utf8Destination) { throw null; }

        public readonly int CopyString(Span<char> destination) { throw null; }

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

        public string? GetString() { throw null; }

        [CLSCompliant(false)]
        public ushort GetUInt16() { throw null; }

        [CLSCompliant(false)]
        public uint GetUInt32() { throw null; }

        [CLSCompliant(false)]
        public ulong GetUInt64() { throw null; }

        public bool Read() { throw null; }

        public void Skip() { }

        public bool TryGetByte(out byte value) { throw null; }

        public bool TryGetBytesFromBase64(out byte[]? value) { throw null; }

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

        public readonly bool ValueTextEquals(ReadOnlySpan<byte> utf8Text) { throw null; }

        public readonly bool ValueTextEquals(ReadOnlySpan<char> text) { throw null; }

        public readonly bool ValueTextEquals(string? text) { throw null; }
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

        public void WriteRawValue(Buffers.ReadOnlySequence<byte> utf8Json, bool skipInputValidation = false) { }

        public void WriteRawValue(ReadOnlySpan<byte> utf8Json, bool skipInputValidation = false) { }

        public void WriteRawValue(ReadOnlySpan<char> json, bool skipInputValidation = false) { }

        public void WriteRawValue(string json, bool skipInputValidation = false) { }

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

        public void WriteString(ReadOnlySpan<byte> utf8PropertyName, string? value) { }

        public void WriteString(ReadOnlySpan<byte> utf8PropertyName, JsonEncodedText value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, DateTime value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, DateTimeOffset value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, Guid value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> utf8Value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, string? value) { }

        public void WriteString(ReadOnlySpan<char> propertyName, JsonEncodedText value) { }

        public void WriteString(string propertyName, DateTime value) { }

        public void WriteString(string propertyName, DateTimeOffset value) { }

        public void WriteString(string propertyName, Guid value) { }

        public void WriteString(string propertyName, ReadOnlySpan<byte> utf8Value) { }

        public void WriteString(string propertyName, ReadOnlySpan<char> value) { }

        public void WriteString(string propertyName, string? value) { }

        public void WriteString(string propertyName, JsonEncodedText value) { }

        public void WriteString(JsonEncodedText propertyName, DateTime value) { }

        public void WriteString(JsonEncodedText propertyName, DateTimeOffset value) { }

        public void WriteString(JsonEncodedText propertyName, Guid value) { }

        public void WriteString(JsonEncodedText propertyName, ReadOnlySpan<byte> utf8Value) { }

        public void WriteString(JsonEncodedText propertyName, ReadOnlySpan<char> value) { }

        public void WriteString(JsonEncodedText propertyName, string? value) { }

        public void WriteString(JsonEncodedText propertyName, JsonEncodedText value) { }

        public void WriteStringValue(DateTime value) { }

        public void WriteStringValue(DateTimeOffset value) { }

        public void WriteStringValue(Guid value) { }

        public void WriteStringValue(ReadOnlySpan<byte> utf8Value) { }

        public void WriteStringValue(ReadOnlySpan<char> value) { }

        public void WriteStringValue(string? value) { }

        public void WriteStringValue(JsonEncodedText value) { }
    }
}

namespace System.Text.Json.Nodes
{
    public sealed partial class JsonArray : JsonNode, Collections.Generic.IList<JsonNode?>, Collections.Generic.ICollection<JsonNode?>, Collections.Generic.IEnumerable<JsonNode?>, Collections.IEnumerable
    {
        public JsonArray(JsonNodeOptions? options = null) { }

        public JsonArray(params JsonNode?[] items) { }

        public JsonArray(JsonNodeOptions options, params JsonNode?[] items) { }

        public int Count { get { throw null; } }

        bool Collections.Generic.ICollection<JsonNode>.IsReadOnly { get { throw null; } }

        public void Add(JsonNode? item) { }

        public void Add<T>(T? value) { }

        public void Clear() { }

        public bool Contains(JsonNode? item) { throw null; }

        public static JsonArray? Create(JsonElement element, JsonNodeOptions? options = null) { throw null; }

        public Collections.Generic.IEnumerator<JsonNode?> GetEnumerator() { throw null; }

        public Collections.Generic.IEnumerable<T> GetValues<T>() { throw null; }

        public int IndexOf(JsonNode? item) { throw null; }

        public void Insert(int index, JsonNode? item) { }

        public bool Remove(JsonNode? item) { throw null; }

        public void RemoveAt(int index) { }

        void Collections.Generic.ICollection<JsonNode>.CopyTo(JsonNode[] array, int index) { }

        Collections.IEnumerator? Collections.IEnumerable.GetEnumerator() { throw null; }

        public override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions? options = null) { }
    }

    public abstract partial class JsonNode
    {
        internal JsonNode() { }

        public JsonNode? this[int index] { get { throw null; } set { } }

        public JsonNode? this[string propertyName] { get { throw null; } set { } }

        public JsonNodeOptions? Options { get { throw null; } }

        public JsonNode? Parent { get { throw null; } }

        public JsonNode Root { get { throw null; } }

        public JsonArray AsArray() { throw null; }

        public JsonObject AsObject() { throw null; }

        public JsonValue AsValue() { throw null; }

        public JsonNode DeepClone() { throw null; }

        public static bool DeepEquals(JsonNode? node1, JsonNode? node2) { throw null; }

        public int GetElementIndex() { throw null; }

        public string GetPath() { throw null; }

        public string GetPropertyName() { throw null; }

        public virtual T GetValue<T>() { throw null; }

        public JsonValueKind GetValueKind() { throw null; }

        public static explicit operator bool(JsonNode value) { throw null; }

        public static explicit operator byte(JsonNode value) { throw null; }

        public static explicit operator char(JsonNode value) { throw null; }

        public static explicit operator DateTime(JsonNode value) { throw null; }

        public static explicit operator DateTimeOffset(JsonNode value) { throw null; }

        public static explicit operator decimal(JsonNode value) { throw null; }

        public static explicit operator double(JsonNode value) { throw null; }

        public static explicit operator Guid(JsonNode value) { throw null; }

        public static explicit operator short(JsonNode value) { throw null; }

        public static explicit operator int(JsonNode value) { throw null; }

        public static explicit operator long(JsonNode value) { throw null; }

        public static explicit operator bool?(JsonNode? value) { throw null; }

        public static explicit operator byte?(JsonNode? value) { throw null; }

        public static explicit operator char?(JsonNode? value) { throw null; }

        public static explicit operator DateTime?(JsonNode? value) { throw null; }

        public static explicit operator DateTimeOffset?(JsonNode? value) { throw null; }

        public static explicit operator decimal?(JsonNode? value) { throw null; }

        public static explicit operator double?(JsonNode? value) { throw null; }

        public static explicit operator Guid?(JsonNode? value) { throw null; }

        public static explicit operator short?(JsonNode? value) { throw null; }

        public static explicit operator int?(JsonNode? value) { throw null; }

        public static explicit operator long?(JsonNode? value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator sbyte?(JsonNode? value) { throw null; }

        public static explicit operator float?(JsonNode? value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator ushort?(JsonNode? value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator uint?(JsonNode? value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator ulong?(JsonNode? value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator sbyte(JsonNode value) { throw null; }

        public static explicit operator float(JsonNode value) { throw null; }

        public static explicit operator string?(JsonNode? value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator ushort(JsonNode value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator uint(JsonNode value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator ulong(JsonNode value) { throw null; }

        public static implicit operator JsonNode(bool value) { throw null; }

        public static implicit operator JsonNode(byte value) { throw null; }

        public static implicit operator JsonNode(char value) { throw null; }

        public static implicit operator JsonNode(DateTime value) { throw null; }

        public static implicit operator JsonNode(DateTimeOffset value) { throw null; }

        public static implicit operator JsonNode(decimal value) { throw null; }

        public static implicit operator JsonNode(double value) { throw null; }

        public static implicit operator JsonNode(Guid value) { throw null; }

        public static implicit operator JsonNode(short value) { throw null; }

        public static implicit operator JsonNode(int value) { throw null; }

        public static implicit operator JsonNode(long value) { throw null; }

        public static implicit operator JsonNode?(bool? value) { throw null; }

        public static implicit operator JsonNode?(byte? value) { throw null; }

        public static implicit operator JsonNode?(char? value) { throw null; }

        public static implicit operator JsonNode?(DateTime? value) { throw null; }

        public static implicit operator JsonNode?(DateTimeOffset? value) { throw null; }

        public static implicit operator JsonNode?(decimal? value) { throw null; }

        public static implicit operator JsonNode?(double? value) { throw null; }

        public static implicit operator JsonNode?(Guid? value) { throw null; }

        public static implicit operator JsonNode?(short? value) { throw null; }

        public static implicit operator JsonNode?(int? value) { throw null; }

        public static implicit operator JsonNode?(long? value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator JsonNode?(sbyte? value) { throw null; }

        public static implicit operator JsonNode?(float? value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator JsonNode?(ushort? value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator JsonNode?(uint? value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator JsonNode?(ulong? value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator JsonNode(sbyte value) { throw null; }

        public static implicit operator JsonNode(float value) { throw null; }

        public static implicit operator JsonNode?(string? value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator JsonNode(ushort value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator JsonNode(uint value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator JsonNode(ulong value) { throw null; }

        public static JsonNode? Parse(IO.Stream utf8Json, JsonNodeOptions? nodeOptions = null, JsonDocumentOptions documentOptions = default) { throw null; }

        public static JsonNode? Parse(ReadOnlySpan<byte> utf8Json, JsonNodeOptions? nodeOptions = null, JsonDocumentOptions documentOptions = default) { throw null; }

        public static JsonNode? Parse(string json, JsonNodeOptions? nodeOptions = null, JsonDocumentOptions documentOptions = default) { throw null; }

        public static JsonNode? Parse(ref Utf8JsonReader reader, JsonNodeOptions? nodeOptions = null) { throw null; }

        public static Threading.Tasks.Task<JsonNode?> ParseAsync(IO.Stream utf8Json, JsonNodeOptions? nodeOptions = null, JsonDocumentOptions documentOptions = default, Threading.CancellationToken cancellationToken = default) { throw null; }

        public void ReplaceWith<T>(T value) { }

        public string ToJsonString(JsonSerializerOptions? options = null) { throw null; }

        public override string ToString() { throw null; }

        public abstract void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions? options = null);
    }

    public partial struct JsonNodeOptions
    {
        private int _dummyPrimitive;
        public bool PropertyNameCaseInsensitive { get { throw null; } set { } }
    }

    public sealed partial class JsonObject : JsonNode, Collections.Generic.IDictionary<string, JsonNode?>, Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, JsonNode?>>, Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, JsonNode?>>, Collections.IEnumerable
    {
        public JsonObject(Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<string, JsonNode?>> properties, JsonNodeOptions? options = null) { }

        public JsonObject(JsonNodeOptions? options = null) { }

        public int Count { get { throw null; } }

        bool Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, JsonNode>>.IsReadOnly { get { throw null; } }

        Collections.Generic.ICollection<string> Collections.Generic.IDictionary<string, JsonNode>.Keys { get { throw null; } }

        Collections.Generic.ICollection<JsonNode?> Collections.Generic.IDictionary<string, JsonNode>.Values { get { throw null; } }

        public void Add(Collections.Generic.KeyValuePair<string, JsonNode?> property) { }

        public void Add(string propertyName, JsonNode? value) { }

        public void Clear() { }

        public bool ContainsKey(string propertyName) { throw null; }

        public static JsonObject? Create(JsonElement element, JsonNodeOptions? options = null) { throw null; }

        public Collections.Generic.IEnumerator<Collections.Generic.KeyValuePair<string, JsonNode?>> GetEnumerator() { throw null; }

        public bool Remove(string propertyName) { throw null; }

        bool Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, JsonNode>>.Contains(Collections.Generic.KeyValuePair<string, JsonNode> item) { throw null; }

        void Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, JsonNode>>.CopyTo(Collections.Generic.KeyValuePair<string, JsonNode>[] array, int index) { }

        bool Collections.Generic.ICollection<Collections.Generic.KeyValuePair<string, JsonNode>>.Remove(Collections.Generic.KeyValuePair<string, JsonNode> item) { throw null; }

        bool Collections.Generic.IDictionary<string, JsonNode>.TryGetValue(string propertyName, out JsonNode jsonNode) { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public bool TryGetPropertyValue(string propertyName, out JsonNode? jsonNode) { throw null; }

        public override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions? options = null) { }
    }

    public abstract partial class JsonValue : JsonNode
    {
        internal JsonValue() { }

        public static JsonValue Create(bool value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(byte value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(char value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(DateTime value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(DateTimeOffset value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(decimal value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(double value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(Guid value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(short value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(int value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(long value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(bool? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(byte? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(char? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(DateTime? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(DateTimeOffset? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(decimal? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(double? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(Guid? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(short? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(int? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(long? value, JsonNodeOptions? options = null) { throw null; }

        [CLSCompliant(false)]
        public static JsonValue? Create(sbyte? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(float? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(JsonElement? value, JsonNodeOptions? options = null) { throw null; }

        [CLSCompliant(false)]
        public static JsonValue? Create(ushort? value, JsonNodeOptions? options = null) { throw null; }

        [CLSCompliant(false)]
        public static JsonValue? Create(uint? value, JsonNodeOptions? options = null) { throw null; }

        [CLSCompliant(false)]
        public static JsonValue? Create(ulong? value, JsonNodeOptions? options = null) { throw null; }

        [CLSCompliant(false)]
        public static JsonValue Create(sbyte value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue Create(float value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(string? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create(JsonElement value, JsonNodeOptions? options = null) { throw null; }

        [CLSCompliant(false)]
        public static JsonValue Create(ushort value, JsonNodeOptions? options = null) { throw null; }

        [CLSCompliant(false)]
        public static JsonValue Create(uint value, JsonNodeOptions? options = null) { throw null; }

        [CLSCompliant(false)]
        public static JsonValue Create(ulong value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create<T>(T? value, JsonNodeOptions? options = null) { throw null; }

        public static JsonValue? Create<T>(T? value, Serialization.Metadata.JsonTypeInfo<T> jsonTypeInfo, JsonNodeOptions? options = null) { throw null; }

        public abstract bool TryGetValue<T>(out T? value);
    }
}

namespace System.Text.Json.Serialization
{
    public partial interface IJsonOnDeserialized
    {
        void OnDeserialized();
    }

    public partial interface IJsonOnDeserializing
    {
        void OnDeserializing();
    }

    public partial interface IJsonOnSerialized
    {
        void OnSerialized();
    }

    public partial interface IJsonOnSerializing
    {
        void OnSerializing();
    }

    public abstract partial class JsonAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
    public sealed partial class JsonConstructorAttribute : JsonAttribute
    {
    }

    public abstract partial class JsonConverter
    {
        internal JsonConverter() { }

        public abstract Type? Type { get; }

        public abstract bool CanConvert(Type typeToConvert);
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false)]
    public partial class JsonConverterAttribute : JsonAttribute
    {
        protected JsonConverterAttribute() { }

        public JsonConverterAttribute(Type converterType) { }

        public Type? ConverterType { get { throw null; } }

        public virtual JsonConverter? CreateConverter(Type typeToConvert) { throw null; }
    }

    public abstract partial class JsonConverterFactory : JsonConverter
    {
        protected JsonConverterFactory() { }

        public sealed override Type? Type { get { throw null; } }

        public abstract JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options);
    }

    public abstract partial class JsonConverter<T> : JsonConverter
    {
        protected internal JsonConverter() { }

        public virtual bool HandleNull { get { throw null; } }

        public sealed override Type Type { get { throw null; } }

        public override bool CanConvert(Type typeToConvert) { throw null; }

        public abstract T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);
        public virtual T ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) { throw null; }

        public abstract void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
        public virtual void WriteAsPropertyName(Utf8JsonWriter writer, T value, JsonSerializerOptions options) { }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public partial class JsonDerivedTypeAttribute : JsonAttribute
    {
        public JsonDerivedTypeAttribute(Type derivedType, int typeDiscriminator) { }

        public JsonDerivedTypeAttribute(Type derivedType, string typeDiscriminator) { }

        public JsonDerivedTypeAttribute(Type derivedType) { }

        public Type DerivedType { get { throw null; } }

        public object? TypeDiscriminator { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed partial class JsonExtensionDataAttribute : JsonAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed partial class JsonIgnoreAttribute : JsonAttribute
    {
        public JsonIgnoreCondition Condition { get { throw null; } set { } }
    }

    public enum JsonIgnoreCondition
    {
        Never = 0,
        Always = 1,
        WhenWritingDefault = 2,
        WhenWritingNull = 3
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed partial class JsonIncludeAttribute : JsonAttribute
    {
    }

    public enum JsonKnownNamingPolicy
    {
        Unspecified = 0,
        CamelCase = 1,
        SnakeCaseLower = 2,
        SnakeCaseUpper = 3,
        KebabCaseLower = 4,
        KebabCaseUpper = 5
    }

    public sealed partial class JsonNumberEnumConverter<TEnum> : JsonConverterFactory where TEnum : struct, Enum
    {
        public override bool CanConvert(Type typeToConvert) { throw null; }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) { throw null; }
    }

    [Flags]
    public enum JsonNumberHandling
    {
        Strict = 0,
        AllowReadingFromString = 1,
        WriteAsString = 2,
        AllowNamedFloatingPointLiterals = 4
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed partial class JsonNumberHandlingAttribute : JsonAttribute
    {
        public JsonNumberHandlingAttribute(JsonNumberHandling handling) { }

        public JsonNumberHandling Handling { get { throw null; } }
    }

    public enum JsonObjectCreationHandling
    {
        Replace = 0,
        Populate = 1
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false)]
    public sealed partial class JsonObjectCreationHandlingAttribute : JsonAttribute
    {
        public JsonObjectCreationHandlingAttribute(JsonObjectCreationHandling handling) { }

        public JsonObjectCreationHandling Handling { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed partial class JsonPolymorphicAttribute : JsonAttribute
    {
        public bool IgnoreUnrecognizedTypeDiscriminators { get { throw null; } set { } }

        public string? TypeDiscriminatorPropertyName { get { throw null; } set { } }

        public JsonUnknownDerivedTypeHandling UnknownDerivedTypeHandling { get { throw null; } set { } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed partial class JsonPropertyNameAttribute : JsonAttribute
    {
        public JsonPropertyNameAttribute(string name) { }

        public string Name { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed partial class JsonPropertyOrderAttribute : JsonAttribute
    {
        public JsonPropertyOrderAttribute(int order) { }

        public int Order { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed partial class JsonRequiredAttribute : JsonAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed partial class JsonSerializableAttribute : JsonAttribute
    {
        public JsonSerializableAttribute(Type type) { }

        public JsonSourceGenerationMode GenerationMode { get { throw null; } set { } }

        public string? TypeInfoPropertyName { get { throw null; } set { } }
    }

    public abstract partial class JsonSerializerContext : Metadata.IJsonTypeInfoResolver
    {
        protected JsonSerializerContext(JsonSerializerOptions? options) { }

        protected abstract JsonSerializerOptions? GeneratedSerializerOptions { get; }

        public JsonSerializerOptions Options { get { throw null; } }

        public abstract Metadata.JsonTypeInfo? GetTypeInfo(Type type);
        Metadata.JsonTypeInfo Metadata.IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options) { throw null; }
    }

    [Flags]
    public enum JsonSourceGenerationMode
    {
        Default = 0,
        Metadata = 1,
        Serialization = 2
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed partial class JsonSourceGenerationOptionsAttribute : JsonAttribute
    {
        public JsonSourceGenerationOptionsAttribute() { }

        public JsonSourceGenerationOptionsAttribute(JsonSerializerDefaults defaults) { }

        public bool AllowTrailingCommas { get { throw null; } set { } }

        public Type[]? Converters { get { throw null; } set { } }

        public int DefaultBufferSize { get { throw null; } set { } }

        public JsonIgnoreCondition DefaultIgnoreCondition { get { throw null; } set { } }

        public JsonKnownNamingPolicy DictionaryKeyPolicy { get { throw null; } set { } }

        public JsonSourceGenerationMode GenerationMode { get { throw null; } set { } }

        public bool IgnoreReadOnlyFields { get { throw null; } set { } }

        public bool IgnoreReadOnlyProperties { get { throw null; } set { } }

        public bool IncludeFields { get { throw null; } set { } }

        public int MaxDepth { get { throw null; } set { } }

        public JsonNumberHandling NumberHandling { get { throw null; } set { } }

        public JsonObjectCreationHandling PreferredObjectCreationHandling { get { throw null; } set { } }

        public bool PropertyNameCaseInsensitive { get { throw null; } set { } }

        public JsonKnownNamingPolicy PropertyNamingPolicy { get { throw null; } set { } }

        public JsonCommentHandling ReadCommentHandling { get { throw null; } set { } }

        public JsonUnknownTypeHandling UnknownTypeHandling { get { throw null; } set { } }

        public JsonUnmappedMemberHandling UnmappedMemberHandling { get { throw null; } set { } }

        public bool UseStringEnumConverter { get { throw null; } set { } }

        public bool WriteIndented { get { throw null; } set { } }
    }

    public partial class JsonStringEnumConverter : JsonConverterFactory
    {
        public JsonStringEnumConverter() { }

        public JsonStringEnumConverter(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true) { }

        public sealed override bool CanConvert(Type typeToConvert) { throw null; }

        public sealed override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) { throw null; }
    }

    public partial class JsonStringEnumConverter<TEnum> : JsonConverterFactory where TEnum : struct, Enum
    {
        public JsonStringEnumConverter() { }

        public JsonStringEnumConverter(JsonNamingPolicy? namingPolicy = null, bool allowIntegerValues = true) { }

        public sealed override bool CanConvert(Type typeToConvert) { throw null; }

        public sealed override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) { throw null; }
    }

    public enum JsonUnknownDerivedTypeHandling
    {
        FailSerialization = 0,
        FallBackToBaseType = 1,
        FallBackToNearestAncestor = 2
    }

    public enum JsonUnknownTypeHandling
    {
        JsonElement = 0,
        JsonNode = 1
    }

    public enum JsonUnmappedMemberHandling
    {
        Skip = 0,
        Disallow = 1
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public partial class JsonUnmappedMemberHandlingAttribute : JsonAttribute
    {
        public JsonUnmappedMemberHandlingAttribute(JsonUnmappedMemberHandling unmappedMemberHandling) { }

        public JsonUnmappedMemberHandling UnmappedMemberHandling { get { throw null; } }
    }

    public abstract partial class ReferenceHandler
    {
        public static ReferenceHandler IgnoreCycles { get { throw null; } }

        public static ReferenceHandler Preserve { get { throw null; } }

        public abstract ReferenceResolver CreateResolver();
    }

    public sealed partial class ReferenceHandler<T> : ReferenceHandler where T : ReferenceResolver, new()
    {
        public override ReferenceResolver CreateResolver() { throw null; }
    }

    public abstract partial class ReferenceResolver
    {
        public abstract void AddReference(string referenceId, object value);
        public abstract string GetReference(object value, out bool alreadyExists);
        public abstract object ResolveReference(string referenceId);
    }
}

namespace System.Text.Json.Serialization.Metadata
{
    public partial class DefaultJsonTypeInfoResolver : IJsonTypeInfoResolver
    {
        public Collections.Generic.IList<Action<JsonTypeInfo>> Modifiers { get { throw null; } }

        public virtual JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options) { throw null; }
    }

    public partial interface IJsonTypeInfoResolver
    {
        JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options);
    }

    public sealed partial class JsonCollectionInfoValues<TCollection>
    {
        public JsonTypeInfo ElementInfo { get { throw null; } init { } }

        public JsonTypeInfo? KeyInfo { get { throw null; } init { } }

        public JsonNumberHandling NumberHandling { get { throw null; } init { } }

        public Func<TCollection>? ObjectCreator { get { throw null; } init { } }

        public Action<Utf8JsonWriter, TCollection>? SerializeHandler { get { throw null; } init { } }
    }

    public readonly partial struct JsonDerivedType
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public JsonDerivedType(Type derivedType, int typeDiscriminator) { }

        public JsonDerivedType(Type derivedType, string typeDiscriminator) { }

        public JsonDerivedType(Type derivedType) { }

        public Type DerivedType { get { throw null; } }

        public object? TypeDiscriminator { get { throw null; } }
    }

    public static partial class JsonMetadataServices
    {
        public static JsonConverter<bool> BooleanConverter { get { throw null; } }

        public static JsonConverter<byte[]?> ByteArrayConverter { get { throw null; } }

        public static JsonConverter<byte> ByteConverter { get { throw null; } }

        public static JsonConverter<char> CharConverter { get { throw null; } }

        public static JsonConverter<DateTime> DateTimeConverter { get { throw null; } }

        public static JsonConverter<DateTimeOffset> DateTimeOffsetConverter { get { throw null; } }

        public static JsonConverter<decimal> DecimalConverter { get { throw null; } }

        public static JsonConverter<double> DoubleConverter { get { throw null; } }

        public static JsonConverter<Guid> GuidConverter { get { throw null; } }

        public static JsonConverter<short> Int16Converter { get { throw null; } }

        public static JsonConverter<int> Int32Converter { get { throw null; } }

        public static JsonConverter<long> Int64Converter { get { throw null; } }

        public static JsonConverter<Nodes.JsonArray?> JsonArrayConverter { get { throw null; } }

        public static JsonConverter<JsonDocument?> JsonDocumentConverter { get { throw null; } }

        public static JsonConverter<JsonElement> JsonElementConverter { get { throw null; } }

        public static JsonConverter<Nodes.JsonNode?> JsonNodeConverter { get { throw null; } }

        public static JsonConverter<Nodes.JsonObject?> JsonObjectConverter { get { throw null; } }

        public static JsonConverter<Nodes.JsonValue?> JsonValueConverter { get { throw null; } }

        public static JsonConverter<Memory<byte>> MemoryByteConverter { get { throw null; } }

        public static JsonConverter<object?> ObjectConverter { get { throw null; } }

        public static JsonConverter<ReadOnlyMemory<byte>> ReadOnlyMemoryByteConverter { get { throw null; } }

        [CLSCompliant(false)]
        public static JsonConverter<sbyte> SByteConverter { get { throw null; } }

        public static JsonConverter<float> SingleConverter { get { throw null; } }

        public static JsonConverter<string?> StringConverter { get { throw null; } }

        public static JsonConverter<TimeSpan> TimeSpanConverter { get { throw null; } }

        [CLSCompliant(false)]
        public static JsonConverter<ushort> UInt16Converter { get { throw null; } }

        [CLSCompliant(false)]
        public static JsonConverter<uint> UInt32Converter { get { throw null; } }

        [CLSCompliant(false)]
        public static JsonConverter<ulong> UInt64Converter { get { throw null; } }

        public static JsonConverter<Uri?> UriConverter { get { throw null; } }

        public static JsonConverter<Version?> VersionConverter { get { throw null; } }

        public static JsonTypeInfo<TElement[]> CreateArrayInfo<TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TElement[]> collectionInfo) { throw null; }

        public static JsonTypeInfo<TCollection> CreateConcurrentQueueInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Concurrent.ConcurrentQueue<TElement> { throw null; }

        public static JsonTypeInfo<TCollection> CreateConcurrentStackInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Concurrent.ConcurrentStack<TElement> { throw null; }

        public static JsonTypeInfo<TCollection> CreateDictionaryInfo<TCollection, TKey, TValue>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.Dictionary<TKey, TValue> { throw null; }

        public static JsonTypeInfo<TCollection> CreateIAsyncEnumerableInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.IAsyncEnumerable<TElement> { throw null; }

        public static JsonTypeInfo<TCollection> CreateICollectionInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.ICollection<TElement> { throw null; }

        public static JsonTypeInfo<TCollection> CreateIDictionaryInfo<TCollection>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.IDictionary { throw null; }

        public static JsonTypeInfo<TCollection> CreateIDictionaryInfo<TCollection, TKey, TValue>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.IDictionary<TKey, TValue> { throw null; }

        public static JsonTypeInfo<TCollection> CreateIEnumerableInfo<TCollection>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.IEnumerable { throw null; }

        public static JsonTypeInfo<TCollection> CreateIEnumerableInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.IEnumerable<TElement> { throw null; }

        public static JsonTypeInfo<TCollection> CreateIListInfo<TCollection>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.IList { throw null; }

        public static JsonTypeInfo<TCollection> CreateIListInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.IList<TElement> { throw null; }

        public static JsonTypeInfo<TCollection> CreateImmutableDictionaryInfo<TCollection, TKey, TValue>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo, Func<Collections.Generic.IEnumerable<Collections.Generic.KeyValuePair<TKey, TValue>>, TCollection> createRangeFunc)
            where TCollection : Collections.Generic.IReadOnlyDictionary<TKey, TValue> { throw null; }

        public static JsonTypeInfo<TCollection> CreateImmutableEnumerableInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo, Func<Collections.Generic.IEnumerable<TElement>, TCollection> createRangeFunc)
            where TCollection : Collections.Generic.IEnumerable<TElement> { throw null; }

        public static JsonTypeInfo<TCollection> CreateIReadOnlyDictionaryInfo<TCollection, TKey, TValue>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.IReadOnlyDictionary<TKey, TValue> { throw null; }

        public static JsonTypeInfo<TCollection> CreateISetInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.ISet<TElement> { throw null; }

        public static JsonTypeInfo<TCollection> CreateListInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.List<TElement> { throw null; }

        public static JsonTypeInfo<Memory<TElement>> CreateMemoryInfo<TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<Memory<TElement>> collectionInfo) { throw null; }

        public static JsonTypeInfo<T> CreateObjectInfo<T>(JsonSerializerOptions options, JsonObjectInfoValues<T> objectInfo) { throw null; }

        public static JsonPropertyInfo CreatePropertyInfo<T>(JsonSerializerOptions options, JsonPropertyInfoValues<T> propertyInfo) { throw null; }

        public static JsonTypeInfo<TCollection> CreateQueueInfo<TCollection>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo, Action<TCollection, object?> addFunc)
            where TCollection : Collections.IEnumerable { throw null; }

        public static JsonTypeInfo<TCollection> CreateQueueInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.Queue<TElement> { throw null; }

        public static JsonTypeInfo<ReadOnlyMemory<TElement>> CreateReadOnlyMemoryInfo<TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<ReadOnlyMemory<TElement>> collectionInfo) { throw null; }

        public static JsonTypeInfo<TCollection> CreateStackInfo<TCollection>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo, Action<TCollection, object?> addFunc)
            where TCollection : Collections.IEnumerable { throw null; }

        public static JsonTypeInfo<TCollection> CreateStackInfo<TCollection, TElement>(JsonSerializerOptions options, JsonCollectionInfoValues<TCollection> collectionInfo)
            where TCollection : Collections.Generic.Stack<TElement> { throw null; }

        public static JsonTypeInfo<T> CreateValueInfo<T>(JsonSerializerOptions options, JsonConverter converter) { throw null; }

        public static JsonConverter<T> GetEnumConverter<T>(JsonSerializerOptions options)
            where T : struct, Enum { throw null; }

        public static JsonConverter<T?> GetNullableConverter<T>(JsonSerializerOptions options)
            where T : struct { throw null; }

        public static JsonConverter<T?> GetNullableConverter<T>(JsonTypeInfo<T> underlyingTypeInfo)
            where T : struct { throw null; }

        public static JsonConverter<T> GetUnsupportedTypeConverter<T>() { throw null; }
    }

    public sealed partial class JsonObjectInfoValues<T>
    {
        public Func<JsonParameterInfoValues[]>? ConstructorParameterMetadataInitializer { get { throw null; } init { } }

        public JsonNumberHandling NumberHandling { get { throw null; } init { } }

        public Func<T>? ObjectCreator { get { throw null; } init { } }

        public Func<object[], T>? ObjectWithParameterizedConstructorCreator { get { throw null; } init { } }

        public Func<JsonSerializerContext, JsonPropertyInfo[]>? PropertyMetadataInitializer { get { throw null; } init { } }

        public Action<Utf8JsonWriter, T>? SerializeHandler { get { throw null; } init { } }
    }

    public sealed partial class JsonParameterInfoValues
    {
        public object? DefaultValue { get { throw null; } init { } }

        public bool HasDefaultValue { get { throw null; } init { } }

        public string Name { get { throw null; } init { } }

        public Type ParameterType { get { throw null; } init { } }

        public int Position { get { throw null; } init { } }
    }

    public partial class JsonPolymorphismOptions
    {
        public Collections.Generic.IList<JsonDerivedType> DerivedTypes { get { throw null; } }

        public bool IgnoreUnrecognizedTypeDiscriminators { get { throw null; } set { } }

        public string TypeDiscriminatorPropertyName { get { throw null; } set { } }

        public JsonUnknownDerivedTypeHandling UnknownDerivedTypeHandling { get { throw null; } set { } }
    }

    public abstract partial class JsonPropertyInfo
    {
        internal JsonPropertyInfo() { }

        public System.Reflection.ICustomAttributeProvider? AttributeProvider { get { throw null; } set { } }

        public JsonConverter? CustomConverter { get { throw null; } set { } }

        public Func<object, object?>? Get { get { throw null; } set { } }

        public bool IsExtensionData { get { throw null; } set { } }

        public bool IsRequired { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public JsonNumberHandling? NumberHandling { get { throw null; } set { } }

        public JsonObjectCreationHandling? ObjectCreationHandling { get { throw null; } set { } }

        public JsonSerializerOptions Options { get { throw null; } }

        public int Order { get { throw null; } set { } }

        public Type PropertyType { get { throw null; } }

        public Action<object, object?>? Set { get { throw null; } set { } }

        public Func<object, object?, bool>? ShouldSerialize { get { throw null; } set { } }
    }

    public sealed partial class JsonPropertyInfoValues<T>
    {
        public JsonConverter<T>? Converter { get { throw null; } init { } }

        public Type DeclaringType { get { throw null; } init { } }

        public Func<object, T?>? Getter { get { throw null; } init { } }

        public bool HasJsonInclude { get { throw null; } init { } }

        public JsonIgnoreCondition? IgnoreCondition { get { throw null; } init { } }

        public bool IsExtensionData { get { throw null; } init { } }

        public bool IsProperty { get { throw null; } init { } }

        public bool IsPublic { get { throw null; } init { } }

        public bool IsVirtual { get { throw null; } init { } }

        public string? JsonPropertyName { get { throw null; } init { } }

        public JsonNumberHandling? NumberHandling { get { throw null; } init { } }

        public string PropertyName { get { throw null; } init { } }

        public JsonTypeInfo PropertyTypeInfo { get { throw null; } init { } }

        public Action<object, T?>? Setter { get { throw null; } init { } }
    }

    public abstract partial class JsonTypeInfo
    {
        internal JsonTypeInfo() { }

        public JsonConverter Converter { get { throw null; } }

        public Func<object>? CreateObject { get { throw null; } set { } }

        public bool IsReadOnly { get { throw null; } }

        public JsonTypeInfoKind Kind { get { throw null; } }

        public JsonNumberHandling? NumberHandling { get { throw null; } set { } }

        public Action<object>? OnDeserialized { get { throw null; } set { } }

        public Action<object>? OnDeserializing { get { throw null; } set { } }

        public Action<object>? OnSerialized { get { throw null; } set { } }

        public Action<object>? OnSerializing { get { throw null; } set { } }

        public JsonSerializerOptions Options { get { throw null; } }

        public IJsonTypeInfoResolver? OriginatingResolver { get { throw null; } set { } }

        public JsonPolymorphismOptions? PolymorphismOptions { get { throw null; } set { } }

        public JsonObjectCreationHandling? PreferredPropertyObjectCreationHandling { get { throw null; } set { } }

        public Collections.Generic.IList<JsonPropertyInfo> Properties { get { throw null; } }

        public Type Type { get { throw null; } }

        public JsonUnmappedMemberHandling? UnmappedMemberHandling { get { throw null; } set { } }

        public JsonPropertyInfo CreateJsonPropertyInfo(Type propertyType, string name) { throw null; }

        public static JsonTypeInfo CreateJsonTypeInfo(Type type, JsonSerializerOptions options) { throw null; }

        public static JsonTypeInfo<T> CreateJsonTypeInfo<T>(JsonSerializerOptions options) { throw null; }

        public void MakeReadOnly() { }
    }

    public enum JsonTypeInfoKind
    {
        None = 0,
        Object = 1,
        Enumerable = 2,
        Dictionary = 3
    }

    public static partial class JsonTypeInfoResolver
    {
        public static IJsonTypeInfoResolver Combine(params IJsonTypeInfoResolver?[] resolvers) { throw null; }

        public static IJsonTypeInfoResolver WithAddedModifier(this IJsonTypeInfoResolver resolver, Action<JsonTypeInfo> modifier) { throw null; }
    }

    public sealed partial class JsonTypeInfo<T> : JsonTypeInfo
    {
        internal JsonTypeInfo() { }

        public new Func<T>? CreateObject { get { throw null; } set { } }

        public Action<Utf8JsonWriter, T>? SerializeHandler { get { throw null; } }
    }
}