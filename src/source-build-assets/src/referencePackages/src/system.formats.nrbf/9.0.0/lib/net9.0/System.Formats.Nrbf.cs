// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Diagnostics.CodeAnalysis.Experimental("SYSLIB5005", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v9.0", FrameworkDisplayName = ".NET 9.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Formats.Nrbf")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides a safe reader for .NET Remoting Binary Format (NRBF) payloads.")]
[assembly: System.Reflection.AssemblyFileVersion("9.0.24.52809")]
[assembly: System.Reflection.AssemblyInformationalVersion("9.0.0+9d5a6a9aa463d6d10b0b0ba6d5982cc82f363dc3")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Formats.Nrbf")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("9.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Formats.Nrbf
{
    public abstract partial class ArrayRecord : SerializationRecord
    {
        internal ArrayRecord() { }
        public override SerializationRecordId Id { get { throw null; } }
        public abstract ReadOnlySpan<int> Lengths { get; }
        public int Rank { get { throw null; } }

        [Diagnostics.CodeAnalysis.RequiresDynamicCode("The code for an array of the specified type might not be available.")]
        public Array GetArray(Type expectedArrayType, bool allowNulls = true) { throw null; }
    }

    public abstract partial class ClassRecord : SerializationRecord
    {
        internal ClassRecord() { }
        public override SerializationRecordId Id { get { throw null; } }
        public Collections.Generic.IEnumerable<string> MemberNames { get { throw null; } }
        public override Reflection.Metadata.TypeName TypeName { get { throw null; } }

        public ArrayRecord? GetArrayRecord(string memberName) { throw null; }
        public bool GetBoolean(string memberName) { throw null; }
        public byte GetByte(string memberName) { throw null; }
        public char GetChar(string memberName) { throw null; }
        public ClassRecord? GetClassRecord(string memberName) { throw null; }
        public DateTime GetDateTime(string memberName) { throw null; }
        public decimal GetDecimal(string memberName) { throw null; }
        public double GetDouble(string memberName) { throw null; }
        public short GetInt16(string memberName) { throw null; }
        public int GetInt32(string memberName) { throw null; }
        public long GetInt64(string memberName) { throw null; }
        public object? GetRawValue(string memberName) { throw null; }
        public sbyte GetSByte(string memberName) { throw null; }
        public SerializationRecord? GetSerializationRecord(string memberName) { throw null; }
        public float GetSingle(string memberName) { throw null; }
        public string? GetString(string memberName) { throw null; }
        public TimeSpan GetTimeSpan(string memberName) { throw null; }
        public ushort GetUInt16(string memberName) { throw null; }
        public uint GetUInt32(string memberName) { throw null; }
        public ulong GetUInt64(string memberName) { throw null; }
        public bool HasMember(string memberName) { throw null; }
    }

    public static partial class NrbfDecoder
    {
        public static SerializationRecord Decode(IO.Stream payload, out Collections.Generic.IReadOnlyDictionary<SerializationRecordId, SerializationRecord> recordMap, PayloadOptions? options = null, bool leaveOpen = false) { throw null; }
        public static SerializationRecord Decode(IO.Stream payload, PayloadOptions? options = null, bool leaveOpen = false) { throw null; }
        public static ClassRecord DecodeClassRecord(IO.Stream payload, PayloadOptions? options = null, bool leaveOpen = false) { throw null; }
        public static bool StartsWithPayloadHeader(IO.Stream stream) { throw null; }
        public static bool StartsWithPayloadHeader(ReadOnlySpan<byte> bytes) { throw null; }
    }
    public sealed partial class PayloadOptions
    {
        public Reflection.Metadata.TypeNameParseOptions? TypeNameParseOptions { get { throw null; } set { } }
        public bool UndoTruncatedTypeNames { get { throw null; } set { } }
    }
    public abstract partial class PrimitiveTypeRecord : SerializationRecord
    {
        internal PrimitiveTypeRecord() { }
        public object Value { get { throw null; } }
    }

    public abstract partial class PrimitiveTypeRecord<T> : PrimitiveTypeRecord
    {
        internal PrimitiveTypeRecord() { }
        public override Reflection.Metadata.TypeName TypeName { get { throw null; } }
        public new T Value { get { throw null; } }
    }

    public abstract partial class SerializationRecord
    {
        internal SerializationRecord() { }
        public abstract SerializationRecordId Id { get; }
        public abstract SerializationRecordType RecordType { get; }
        public abstract Reflection.Metadata.TypeName TypeName { get; }

        public bool TypeNameMatches(Type type) { throw null; }
    }
    public readonly partial struct SerializationRecordId : IEquatable<SerializationRecordId>
    {
        private readonly int _dummyPrimitive;
        public readonly bool Equals(SerializationRecordId other) { throw null; }
        public override readonly bool Equals(object? obj) { throw null; }
        public override readonly int GetHashCode() { throw null; }
    }

    public enum SerializationRecordType
    {
        SerializedStreamHeader = 0,
        ClassWithId = 1,
        SystemClassWithMembers = 2,
        ClassWithMembers = 3,
        SystemClassWithMembersAndTypes = 4,
        ClassWithMembersAndTypes = 5,
        BinaryObjectString = 6,
        BinaryArray = 7,
        MemberPrimitiveTyped = 8,
        MemberReference = 9,
        ObjectNull = 10,
        MessageEnd = 11,
        BinaryLibrary = 12,
        ObjectNullMultiple256 = 13,
        ObjectNullMultiple = 14,
        ArraySinglePrimitive = 15,
        ArraySingleObject = 16,
        ArraySingleString = 17,
        MethodCall = 21,
        MethodReturn = 22
    }

    public abstract partial class SZArrayRecord<T> : ArrayRecord
    {
        internal SZArrayRecord() { }
        public int Length { get { throw null; } }
        public override ReadOnlySpan<int> Lengths { get { throw null; } }

        public abstract T?[] GetArray(bool allowNulls = true);
    }
}