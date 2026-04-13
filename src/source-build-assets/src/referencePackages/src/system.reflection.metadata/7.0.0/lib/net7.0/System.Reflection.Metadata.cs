// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("System.Reflection.Metadata.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001004b86c4cb78549b34bab61a3b1800e23bfeb5b3ec390074041536a7e3cbd97f5f04cf0f857155a8928eaa29ebfd11cfbbad3ba70efea7bda3226c6a8d370a4cd303f714486b6ebc225985a638471e6ef571cc92a4613c00b8fa65d61ccee0cbe5f36330c9a01f4183559f1bef24cc2917c6d913e3a541333a1d05d9bed22b38cb")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection.Metadata")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("This package provides a low-level .NET (ECMA-335) metadata reader and writer. It's geared for performance and is the ideal choice for building higher-level libraries that intend to provide their own object model, such as compilers. The metadata format is defined by the ECMA-335 - Common Language Infrastructure (CLI) specification.\r\n\r\nThe System.Reflection.Metadata library is built-in as part of the shared framework in .NET Runtime. The package can be installed when you need to use it in other target frameworks.")]
[assembly: System.Reflection.AssemblyFileVersion("7.0.22.51805")]
[assembly: System.Reflection.AssemblyInformationalVersion("7.0.0+d099f075e45d2aa6007a22b71b45a08758559f80")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Reflection.Metadata")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("7.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Reflection
{
    [Flags]
    public enum AssemblyFlags
    {
        PublicKey = 1,
        Retargetable = 256,
        WindowsRuntime = 512,
        ContentTypeMask = 3584,
        DisableJitCompileOptimizer = 16384,
        EnableJitCompileTracking = 32768
    }

    public enum AssemblyHashAlgorithm
    {
        None = 0,
        MD5 = 32771,
        Sha1 = 32772,
        Sha256 = 32780,
        Sha384 = 32781,
        Sha512 = 32782
    }

    public enum DeclarativeSecurityAction : short
    {
        None = 0,
        Demand = 2,
        Assert = 3,
        Deny = 4,
        PermitOnly = 5,
        LinkDemand = 6,
        InheritanceDemand = 7,
        RequestMinimum = 8,
        RequestOptional = 9,
        RequestRefuse = 10
    }

    [Flags]
    public enum ManifestResourceAttributes
    {
        Public = 1,
        Private = 2,
        VisibilityMask = 7
    }

    [Flags]
    public enum MethodImportAttributes : short
    {
        None = 0,
        ExactSpelling = 1,
        CharSetAnsi = 2,
        CharSetUnicode = 4,
        CharSetAuto = 6,
        CharSetMask = 6,
        BestFitMappingEnable = 16,
        BestFitMappingDisable = 32,
        BestFitMappingMask = 48,
        SetLastError = 64,
        CallingConventionWinApi = 256,
        CallingConventionCDecl = 512,
        CallingConventionStdCall = 768,
        CallingConventionThisCall = 1024,
        CallingConventionFastCall = 1280,
        CallingConventionMask = 1792,
        ThrowOnUnmappableCharEnable = 4096,
        ThrowOnUnmappableCharDisable = 8192,
        ThrowOnUnmappableCharMask = 12288
    }

    [Flags]
    public enum MethodSemanticsAttributes
    {
        Setter = 1,
        Getter = 2,
        Other = 4,
        Adder = 8,
        Remover = 16,
        Raiser = 32
    }
}

namespace System.Reflection.Metadata
{
    public readonly partial struct ArrayShape
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ArrayShape(int rank, Collections.Immutable.ImmutableArray<int> sizes, Collections.Immutable.ImmutableArray<int> lowerBounds) { }

        public Collections.Immutable.ImmutableArray<int> LowerBounds { get { throw null; } }

        public int Rank { get { throw null; } }

        public Collections.Immutable.ImmutableArray<int> Sizes { get { throw null; } }
    }

    public readonly partial struct AssemblyDefinition
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public StringHandle Culture { get { throw null; } }

        public AssemblyFlags Flags { get { throw null; } }

        public AssemblyHashAlgorithm HashAlgorithm { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public BlobHandle PublicKey { get { throw null; } }

        public Version Version { get { throw null; } }

        public readonly AssemblyName GetAssemblyName() { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public readonly DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes() { throw null; }
    }

    public readonly partial struct AssemblyDefinitionHandle : IEquatable<AssemblyDefinitionHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(AssemblyDefinitionHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(AssemblyDefinitionHandle left, AssemblyDefinitionHandle right) { throw null; }

        public static explicit operator AssemblyDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator AssemblyDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(AssemblyDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(AssemblyDefinitionHandle handle) { throw null; }

        public static bool operator !=(AssemblyDefinitionHandle left, AssemblyDefinitionHandle right) { throw null; }
    }

    public readonly partial struct AssemblyFile
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public bool ContainsMetadata { get { throw null; } }

        public BlobHandle HashValue { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct AssemblyFileHandle : IEquatable<AssemblyFileHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(AssemblyFileHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(AssemblyFileHandle left, AssemblyFileHandle right) { throw null; }

        public static explicit operator AssemblyFileHandle(EntityHandle handle) { throw null; }

        public static explicit operator AssemblyFileHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(AssemblyFileHandle handle) { throw null; }

        public static implicit operator Handle(AssemblyFileHandle handle) { throw null; }

        public static bool operator !=(AssemblyFileHandle left, AssemblyFileHandle right) { throw null; }
    }

    public readonly partial struct AssemblyFileHandleCollection : Collections.Generic.IReadOnlyCollection<AssemblyFileHandle>, Collections.Generic.IEnumerable<AssemblyFileHandle>, Collections.IEnumerable
    {
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<AssemblyFileHandle> Collections.Generic.IEnumerable<AssemblyFileHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<AssemblyFileHandle>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public AssemblyFileHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct AssemblyReference
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public StringHandle Culture { get { throw null; } }

        public AssemblyFlags Flags { get { throw null; } }

        public BlobHandle HashValue { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public BlobHandle PublicKeyOrToken { get { throw null; } }

        public Version Version { get { throw null; } }

        public readonly AssemblyName GetAssemblyName() { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct AssemblyReferenceHandle : IEquatable<AssemblyReferenceHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(AssemblyReferenceHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(AssemblyReferenceHandle left, AssemblyReferenceHandle right) { throw null; }

        public static explicit operator AssemblyReferenceHandle(EntityHandle handle) { throw null; }

        public static explicit operator AssemblyReferenceHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(AssemblyReferenceHandle handle) { throw null; }

        public static implicit operator Handle(AssemblyReferenceHandle handle) { throw null; }

        public static bool operator !=(AssemblyReferenceHandle left, AssemblyReferenceHandle right) { throw null; }
    }

    public readonly partial struct AssemblyReferenceHandleCollection : Collections.Generic.IReadOnlyCollection<AssemblyReferenceHandle>, Collections.Generic.IEnumerable<AssemblyReferenceHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<AssemblyReferenceHandle> Collections.Generic.IEnumerable<AssemblyReferenceHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<AssemblyReferenceHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public AssemblyReferenceHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct Blob
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public bool IsDefault { get { throw null; } }

        public int Length { get { throw null; } }

        public readonly ArraySegment<byte> GetBytes() { throw null; }
    }

    public partial class BlobBuilder
    {
        public BlobBuilder(int capacity = 256) { }

        protected internal int ChunkCapacity { get { throw null; } }

        public int Count { get { throw null; } }

        protected int FreeBytes { get { throw null; } }

        public void Align(int alignment) { }

        protected virtual BlobBuilder AllocateChunk(int minimalSize) { throw null; }

        public void Clear() { }

        public bool ContentEquals(BlobBuilder other) { throw null; }

        protected void Free() { }

        protected virtual void FreeChunk() { }

        public Blobs GetBlobs() { throw null; }

        public void LinkPrefix(BlobBuilder prefix) { }

        public void LinkSuffix(BlobBuilder suffix) { }

        public void PadTo(int position) { }

        public Blob ReserveBytes(int byteCount) { throw null; }

        public byte[] ToArray() { throw null; }

        public byte[] ToArray(int start, int byteCount) { throw null; }

        public Collections.Immutable.ImmutableArray<byte> ToImmutableArray() { throw null; }

        public Collections.Immutable.ImmutableArray<byte> ToImmutableArray(int start, int byteCount) { throw null; }

        public int TryWriteBytes(IO.Stream source, int byteCount) { throw null; }

        public void WriteBoolean(bool value) { }

        public void WriteByte(byte value) { }

        public void WriteBytes(byte value, int byteCount) { }

        public void WriteBytes(byte[] buffer, int start, int byteCount) { }

        public void WriteBytes(byte[] buffer) { }

        public unsafe void WriteBytes(byte* buffer, int byteCount) { }

        public void WriteBytes(Collections.Immutable.ImmutableArray<byte> buffer, int start, int byteCount) { }

        public void WriteBytes(Collections.Immutable.ImmutableArray<byte> buffer) { }

        public void WriteCompressedInteger(int value) { }

        public void WriteCompressedSignedInteger(int value) { }

        public void WriteConstant(object? value) { }

        public void WriteContentTo(IO.Stream destination) { }

        public void WriteContentTo(BlobBuilder destination) { }

        public void WriteContentTo(ref BlobWriter destination) { }

        public void WriteDateTime(DateTime value) { }

        public void WriteDecimal(decimal value) { }

        public void WriteDouble(double value) { }

        public void WriteGuid(Guid value) { }

        public void WriteInt16(short value) { }

        public void WriteInt16BE(short value) { }

        public void WriteInt32(int value) { }

        public void WriteInt32BE(int value) { }

        public void WriteInt64(long value) { }

        public void WriteReference(int reference, bool isSmall) { }

        public void WriteSByte(sbyte value) { }

        public void WriteSerializedString(string? value) { }

        public void WriteSingle(float value) { }

        public void WriteUInt16(ushort value) { }

        public void WriteUInt16BE(ushort value) { }

        public void WriteUInt32(uint value) { }

        public void WriteUInt32BE(uint value) { }

        public void WriteUInt64(ulong value) { }

        public void WriteUserString(string value) { }

        public void WriteUTF16(char[] value) { }

        public void WriteUTF16(string value) { }

        public void WriteUTF8(string value, bool allowUnpairedSurrogates = true) { }

        public partial struct Blobs : Collections.Generic.IEnumerable<Blob>, Collections.IEnumerable, Collections.Generic.IEnumerator<Blob>, Collections.IEnumerator, IDisposable
        {
            public Blob Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public Blobs GetEnumerator() { throw null; }

            public bool MoveNext() { throw null; }

            public void Reset() { }

            Collections.Generic.IEnumerator<Blob> Collections.Generic.IEnumerable<Blob>.GetEnumerator() { throw null; }

            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct BlobContentId : IEquatable<BlobContentId>
    {
        private readonly int _dummyPrimitive;
        public BlobContentId(byte[] id) { }

        public BlobContentId(Collections.Immutable.ImmutableArray<byte> id) { }

        public BlobContentId(Guid guid, uint stamp) { }

        public Guid Guid { get { throw null; } }

        public bool IsDefault { get { throw null; } }

        public uint Stamp { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(BlobContentId other) { throw null; }

        public static BlobContentId FromHash(byte[] hashCode) { throw null; }

        public static BlobContentId FromHash(Collections.Immutable.ImmutableArray<byte> hashCode) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static Func<Collections.Generic.IEnumerable<Blob>, BlobContentId> GetTimeBasedProvider() { throw null; }

        public static bool operator ==(BlobContentId left, BlobContentId right) { throw null; }

        public static bool operator !=(BlobContentId left, BlobContentId right) { throw null; }
    }

    public readonly partial struct BlobHandle : IEquatable<BlobHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(BlobHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(BlobHandle left, BlobHandle right) { throw null; }

        public static explicit operator BlobHandle(Handle handle) { throw null; }

        public static implicit operator Handle(BlobHandle handle) { throw null; }

        public static bool operator !=(BlobHandle left, BlobHandle right) { throw null; }
    }

    public partial struct BlobReader
    {
        private object _dummy;
        private int _dummyPrimitive;
        public unsafe BlobReader(byte* buffer, int length) { }

        public unsafe byte* CurrentPointer { get { throw null; } }

        public int Length { get { throw null; } }

        public int Offset { get { throw null; } set { } }

        public int RemainingBytes { get { throw null; } }

        public unsafe byte* StartPointer { get { throw null; } }

        public void Align(byte alignment) { }

        public int IndexOf(byte value) { throw null; }

        public BlobHandle ReadBlobHandle() { throw null; }

        public bool ReadBoolean() { throw null; }

        public byte ReadByte() { throw null; }

        public void ReadBytes(int byteCount, byte[] buffer, int bufferOffset) { }

        public byte[] ReadBytes(int byteCount) { throw null; }

        public char ReadChar() { throw null; }

        public int ReadCompressedInteger() { throw null; }

        public int ReadCompressedSignedInteger() { throw null; }

        public object? ReadConstant(ConstantTypeCode typeCode) { throw null; }

        public DateTime ReadDateTime() { throw null; }

        public decimal ReadDecimal() { throw null; }

        public double ReadDouble() { throw null; }

        public Guid ReadGuid() { throw null; }

        public short ReadInt16() { throw null; }

        public int ReadInt32() { throw null; }

        public long ReadInt64() { throw null; }

        public sbyte ReadSByte() { throw null; }

        public SerializationTypeCode ReadSerializationTypeCode() { throw null; }

        public string? ReadSerializedString() { throw null; }

        public SignatureHeader ReadSignatureHeader() { throw null; }

        public SignatureTypeCode ReadSignatureTypeCode() { throw null; }

        public float ReadSingle() { throw null; }

        public EntityHandle ReadTypeHandle() { throw null; }

        public ushort ReadUInt16() { throw null; }

        public uint ReadUInt32() { throw null; }

        public ulong ReadUInt64() { throw null; }

        public string ReadUTF16(int byteCount) { throw null; }

        public string ReadUTF8(int byteCount) { throw null; }

        public void Reset() { }

        public bool TryReadCompressedInteger(out int value) { throw null; }

        public bool TryReadCompressedSignedInteger(out int value) { throw null; }
    }

    public partial struct BlobWriter
    {
        private object _dummy;
        private int _dummyPrimitive;
        public BlobWriter(byte[] buffer, int start, int count) { }

        public BlobWriter(byte[] buffer) { }

        public BlobWriter(int size) { }

        public BlobWriter(Blob blob) { }

        public Blob Blob { get { throw null; } }

        public int Length { get { throw null; } }

        public int Offset { get { throw null; } set { } }

        public int RemainingBytes { get { throw null; } }

        public void Align(int alignment) { }

        public void Clear() { }

        public bool ContentEquals(BlobWriter other) { throw null; }

        public void PadTo(int offset) { }

        public byte[] ToArray() { throw null; }

        public byte[] ToArray(int start, int byteCount) { throw null; }

        public Collections.Immutable.ImmutableArray<byte> ToImmutableArray() { throw null; }

        public Collections.Immutable.ImmutableArray<byte> ToImmutableArray(int start, int byteCount) { throw null; }

        public void WriteBoolean(bool value) { }

        public void WriteByte(byte value) { }

        public void WriteBytes(byte value, int byteCount) { }

        public void WriteBytes(byte[] buffer, int start, int byteCount) { }

        public void WriteBytes(byte[] buffer) { }

        public unsafe void WriteBytes(byte* buffer, int byteCount) { }

        public void WriteBytes(Collections.Immutable.ImmutableArray<byte> buffer, int start, int byteCount) { }

        public void WriteBytes(Collections.Immutable.ImmutableArray<byte> buffer) { }

        public int WriteBytes(IO.Stream source, int byteCount) { throw null; }

        public void WriteBytes(BlobBuilder source) { }

        public void WriteCompressedInteger(int value) { }

        public void WriteCompressedSignedInteger(int value) { }

        public void WriteConstant(object? value) { }

        public void WriteDateTime(DateTime value) { }

        public void WriteDecimal(decimal value) { }

        public void WriteDouble(double value) { }

        public void WriteGuid(Guid value) { }

        public void WriteInt16(short value) { }

        public void WriteInt16BE(short value) { }

        public void WriteInt32(int value) { }

        public void WriteInt32BE(int value) { }

        public void WriteInt64(long value) { }

        public void WriteReference(int reference, bool isSmall) { }

        public void WriteSByte(sbyte value) { }

        public void WriteSerializedString(string? str) { }

        public void WriteSingle(float value) { }

        public void WriteUInt16(ushort value) { }

        public void WriteUInt16BE(ushort value) { }

        public void WriteUInt32(uint value) { }

        public void WriteUInt32BE(uint value) { }

        public void WriteUInt64(ulong value) { }

        public void WriteUserString(string value) { }

        public void WriteUTF16(char[] value) { }

        public void WriteUTF16(string value) { }

        public void WriteUTF8(string value, bool allowUnpairedSurrogates) { }
    }

    public readonly partial struct Constant
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public EntityHandle Parent { get { throw null; } }

        public ConstantTypeCode TypeCode { get { throw null; } }

        public BlobHandle Value { get { throw null; } }
    }

    public readonly partial struct ConstantHandle : IEquatable<ConstantHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(ConstantHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ConstantHandle left, ConstantHandle right) { throw null; }

        public static explicit operator ConstantHandle(EntityHandle handle) { throw null; }

        public static explicit operator ConstantHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ConstantHandle handle) { throw null; }

        public static implicit operator Handle(ConstantHandle handle) { throw null; }

        public static bool operator !=(ConstantHandle left, ConstantHandle right) { throw null; }
    }

    public enum ConstantTypeCode : byte
    {
        Invalid = 0,
        Boolean = 2,
        Char = 3,
        SByte = 4,
        Byte = 5,
        Int16 = 6,
        UInt16 = 7,
        Int32 = 8,
        UInt32 = 9,
        Int64 = 10,
        UInt64 = 11,
        Single = 12,
        Double = 13,
        String = 14,
        NullReference = 18
    }

    public readonly partial struct CustomAttribute
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public EntityHandle Constructor { get { throw null; } }

        public EntityHandle Parent { get { throw null; } }

        public BlobHandle Value { get { throw null; } }

        public readonly CustomAttributeValue<TType> DecodeValue<TType>(ICustomAttributeTypeProvider<TType> provider) { throw null; }
    }

    public readonly partial struct CustomAttributeHandle : IEquatable<CustomAttributeHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(CustomAttributeHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(CustomAttributeHandle left, CustomAttributeHandle right) { throw null; }

        public static explicit operator CustomAttributeHandle(EntityHandle handle) { throw null; }

        public static explicit operator CustomAttributeHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(CustomAttributeHandle handle) { throw null; }

        public static implicit operator Handle(CustomAttributeHandle handle) { throw null; }

        public static bool operator !=(CustomAttributeHandle left, CustomAttributeHandle right) { throw null; }
    }

    public readonly partial struct CustomAttributeHandleCollection : Collections.Generic.IReadOnlyCollection<CustomAttributeHandle>, Collections.Generic.IEnumerable<CustomAttributeHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<CustomAttributeHandle> Collections.Generic.IEnumerable<CustomAttributeHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<CustomAttributeHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public CustomAttributeHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public enum CustomAttributeNamedArgumentKind : byte
    {
        Field = 83,
        Property = 84
    }

    public readonly partial struct CustomAttributeNamedArgument<TType>
    {
        private readonly TType _Type_k__BackingField;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public CustomAttributeNamedArgument(string? name, CustomAttributeNamedArgumentKind kind, TType type, object? value) { }

        public CustomAttributeNamedArgumentKind Kind { get { throw null; } }

        public string? Name { get { throw null; } }

        public TType Type { get { throw null; } }

        public object? Value { get { throw null; } }
    }

    public readonly partial struct CustomAttributeTypedArgument<TType>
    {
        private readonly TType _Type_k__BackingField;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public CustomAttributeTypedArgument(TType type, object? value) { }

        public TType Type { get { throw null; } }

        public object? Value { get { throw null; } }
    }

    public readonly partial struct CustomAttributeValue<TType>
    {
        private readonly Collections.Immutable.ImmutableArray<CustomAttributeTypedArgument<TType>> _FixedArguments_k__BackingField;
        private readonly Collections.Immutable.ImmutableArray<CustomAttributeNamedArgument<TType>> _NamedArguments_k__BackingField;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public CustomAttributeValue(Collections.Immutable.ImmutableArray<CustomAttributeTypedArgument<TType>> fixedArguments, Collections.Immutable.ImmutableArray<CustomAttributeNamedArgument<TType>> namedArguments) { }

        public Collections.Immutable.ImmutableArray<CustomAttributeTypedArgument<TType>> FixedArguments { get { throw null; } }

        public Collections.Immutable.ImmutableArray<CustomAttributeNamedArgument<TType>> NamedArguments { get { throw null; } }
    }

    public readonly partial struct CustomDebugInformation
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public GuidHandle Kind { get { throw null; } }

        public EntityHandle Parent { get { throw null; } }

        public BlobHandle Value { get { throw null; } }
    }

    public readonly partial struct CustomDebugInformationHandle : IEquatable<CustomDebugInformationHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(CustomDebugInformationHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(CustomDebugInformationHandle left, CustomDebugInformationHandle right) { throw null; }

        public static explicit operator CustomDebugInformationHandle(EntityHandle handle) { throw null; }

        public static explicit operator CustomDebugInformationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(CustomDebugInformationHandle handle) { throw null; }

        public static implicit operator Handle(CustomDebugInformationHandle handle) { throw null; }

        public static bool operator !=(CustomDebugInformationHandle left, CustomDebugInformationHandle right) { throw null; }
    }

    public readonly partial struct CustomDebugInformationHandleCollection : Collections.Generic.IReadOnlyCollection<CustomDebugInformationHandle>, Collections.Generic.IEnumerable<CustomDebugInformationHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<CustomDebugInformationHandle> Collections.Generic.IEnumerable<CustomDebugInformationHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<CustomDebugInformationHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public CustomDebugInformationHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public sealed partial class DebugMetadataHeader
    {
        internal DebugMetadataHeader() { }

        public MethodDefinitionHandle EntryPoint { get { throw null; } }

        public Collections.Immutable.ImmutableArray<byte> Id { get { throw null; } }

        public int IdStartOffset { get { throw null; } }
    }

    public readonly partial struct DeclarativeSecurityAttribute
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public DeclarativeSecurityAction Action { get { throw null; } }

        public EntityHandle Parent { get { throw null; } }

        public BlobHandle PermissionSet { get { throw null; } }
    }

    public readonly partial struct DeclarativeSecurityAttributeHandle : IEquatable<DeclarativeSecurityAttributeHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(DeclarativeSecurityAttributeHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(DeclarativeSecurityAttributeHandle left, DeclarativeSecurityAttributeHandle right) { throw null; }

        public static explicit operator DeclarativeSecurityAttributeHandle(EntityHandle handle) { throw null; }

        public static explicit operator DeclarativeSecurityAttributeHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(DeclarativeSecurityAttributeHandle handle) { throw null; }

        public static implicit operator Handle(DeclarativeSecurityAttributeHandle handle) { throw null; }

        public static bool operator !=(DeclarativeSecurityAttributeHandle left, DeclarativeSecurityAttributeHandle right) { throw null; }
    }

    public readonly partial struct DeclarativeSecurityAttributeHandleCollection : Collections.Generic.IReadOnlyCollection<DeclarativeSecurityAttributeHandle>, Collections.Generic.IEnumerable<DeclarativeSecurityAttributeHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<DeclarativeSecurityAttributeHandle> Collections.Generic.IEnumerable<DeclarativeSecurityAttributeHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<DeclarativeSecurityAttributeHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public DeclarativeSecurityAttributeHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct Document
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public BlobHandle Hash { get { throw null; } }

        public GuidHandle HashAlgorithm { get { throw null; } }

        public GuidHandle Language { get { throw null; } }

        public DocumentNameBlobHandle Name { get { throw null; } }
    }

    public readonly partial struct DocumentHandle : IEquatable<DocumentHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(DocumentHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(DocumentHandle left, DocumentHandle right) { throw null; }

        public static explicit operator DocumentHandle(EntityHandle handle) { throw null; }

        public static explicit operator DocumentHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(DocumentHandle handle) { throw null; }

        public static implicit operator Handle(DocumentHandle handle) { throw null; }

        public static bool operator !=(DocumentHandle left, DocumentHandle right) { throw null; }
    }

    public readonly partial struct DocumentHandleCollection : Collections.Generic.IReadOnlyCollection<DocumentHandle>, Collections.Generic.IEnumerable<DocumentHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<DocumentHandle> Collections.Generic.IEnumerable<DocumentHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<DocumentHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public DocumentHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct DocumentNameBlobHandle : IEquatable<DocumentNameBlobHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(DocumentNameBlobHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(DocumentNameBlobHandle left, DocumentNameBlobHandle right) { throw null; }

        public static explicit operator DocumentNameBlobHandle(BlobHandle handle) { throw null; }

        public static implicit operator BlobHandle(DocumentNameBlobHandle handle) { throw null; }

        public static bool operator !=(DocumentNameBlobHandle left, DocumentNameBlobHandle right) { throw null; }
    }

    public readonly partial struct EntityHandle : IEquatable<EntityHandle>
    {
        private readonly int _dummyPrimitive;
        public static readonly AssemblyDefinitionHandle AssemblyDefinition;
        public static readonly ModuleDefinitionHandle ModuleDefinition;
        public bool IsNil { get { throw null; } }

        public HandleKind Kind { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(EntityHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(EntityHandle left, EntityHandle right) { throw null; }

        public static explicit operator EntityHandle(Handle handle) { throw null; }

        public static implicit operator Handle(EntityHandle handle) { throw null; }

        public static bool operator !=(EntityHandle left, EntityHandle right) { throw null; }
    }

    public readonly partial struct EventAccessors
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public MethodDefinitionHandle Adder { get { throw null; } }

        public Collections.Immutable.ImmutableArray<MethodDefinitionHandle> Others { get { throw null; } }

        public MethodDefinitionHandle Raiser { get { throw null; } }

        public MethodDefinitionHandle Remover { get { throw null; } }
    }

    public readonly partial struct EventDefinition
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public EventAttributes Attributes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public EntityHandle Type { get { throw null; } }

        public readonly EventAccessors GetAccessors() { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct EventDefinitionHandle : IEquatable<EventDefinitionHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(EventDefinitionHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(EventDefinitionHandle left, EventDefinitionHandle right) { throw null; }

        public static explicit operator EventDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator EventDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(EventDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(EventDefinitionHandle handle) { throw null; }

        public static bool operator !=(EventDefinitionHandle left, EventDefinitionHandle right) { throw null; }
    }

    public readonly partial struct EventDefinitionHandleCollection : Collections.Generic.IReadOnlyCollection<EventDefinitionHandle>, Collections.Generic.IEnumerable<EventDefinitionHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<EventDefinitionHandle> Collections.Generic.IEnumerable<EventDefinitionHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<EventDefinitionHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public EventDefinitionHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct ExceptionRegion
    {
        private readonly int _dummyPrimitive;
        public EntityHandle CatchType { get { throw null; } }

        public int FilterOffset { get { throw null; } }

        public int HandlerLength { get { throw null; } }

        public int HandlerOffset { get { throw null; } }

        public ExceptionRegionKind Kind { get { throw null; } }

        public int TryLength { get { throw null; } }

        public int TryOffset { get { throw null; } }
    }

    public enum ExceptionRegionKind : ushort
    {
        Catch = 0,
        Filter = 1,
        Finally = 2,
        Fault = 4
    }

    public readonly partial struct ExportedType
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public TypeAttributes Attributes { get { throw null; } }

        public EntityHandle Implementation { get { throw null; } }

        public bool IsForwarder { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public StringHandle Namespace { get { throw null; } }

        public NamespaceDefinitionHandle NamespaceDefinition { get { throw null; } }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct ExportedTypeHandle : IEquatable<ExportedTypeHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(ExportedTypeHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ExportedTypeHandle left, ExportedTypeHandle right) { throw null; }

        public static explicit operator ExportedTypeHandle(EntityHandle handle) { throw null; }

        public static explicit operator ExportedTypeHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ExportedTypeHandle handle) { throw null; }

        public static implicit operator Handle(ExportedTypeHandle handle) { throw null; }

        public static bool operator !=(ExportedTypeHandle left, ExportedTypeHandle right) { throw null; }
    }

    public readonly partial struct ExportedTypeHandleCollection : Collections.Generic.IReadOnlyCollection<ExportedTypeHandle>, Collections.Generic.IEnumerable<ExportedTypeHandle>, Collections.IEnumerable
    {
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<ExportedTypeHandle> Collections.Generic.IEnumerable<ExportedTypeHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<ExportedTypeHandle>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public ExportedTypeHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct FieldDefinition
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public FieldAttributes Attributes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }

        public readonly TType DecodeSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext) { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public readonly TypeDefinitionHandle GetDeclaringType() { throw null; }

        public readonly ConstantHandle GetDefaultValue() { throw null; }

        public readonly BlobHandle GetMarshallingDescriptor() { throw null; }

        public readonly int GetOffset() { throw null; }

        public readonly int GetRelativeVirtualAddress() { throw null; }
    }

    public readonly partial struct FieldDefinitionHandle : IEquatable<FieldDefinitionHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(FieldDefinitionHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(FieldDefinitionHandle left, FieldDefinitionHandle right) { throw null; }

        public static explicit operator FieldDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator FieldDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(FieldDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(FieldDefinitionHandle handle) { throw null; }

        public static bool operator !=(FieldDefinitionHandle left, FieldDefinitionHandle right) { throw null; }
    }

    public readonly partial struct FieldDefinitionHandleCollection : Collections.Generic.IReadOnlyCollection<FieldDefinitionHandle>, Collections.Generic.IEnumerable<FieldDefinitionHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<FieldDefinitionHandle> Collections.Generic.IEnumerable<FieldDefinitionHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<FieldDefinitionHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public FieldDefinitionHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct GenericParameter
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public GenericParameterAttributes Attributes { get { throw null; } }

        public int Index { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public EntityHandle Parent { get { throw null; } }

        public readonly GenericParameterConstraintHandleCollection GetConstraints() { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct GenericParameterConstraint
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public GenericParameterHandle Parameter { get { throw null; } }

        public EntityHandle Type { get { throw null; } }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct GenericParameterConstraintHandle : IEquatable<GenericParameterConstraintHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(GenericParameterConstraintHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(GenericParameterConstraintHandle left, GenericParameterConstraintHandle right) { throw null; }

        public static explicit operator GenericParameterConstraintHandle(EntityHandle handle) { throw null; }

        public static explicit operator GenericParameterConstraintHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(GenericParameterConstraintHandle handle) { throw null; }

        public static implicit operator Handle(GenericParameterConstraintHandle handle) { throw null; }

        public static bool operator !=(GenericParameterConstraintHandle left, GenericParameterConstraintHandle right) { throw null; }
    }

    public readonly partial struct GenericParameterConstraintHandleCollection : Collections.Generic.IReadOnlyList<GenericParameterConstraintHandle>, Collections.Generic.IEnumerable<GenericParameterConstraintHandle>, Collections.IEnumerable, Collections.Generic.IReadOnlyCollection<GenericParameterConstraintHandle>
    {
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public GenericParameterConstraintHandle this[int index] { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<GenericParameterConstraintHandle> Collections.Generic.IEnumerable<GenericParameterConstraintHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<GenericParameterConstraintHandle>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public GenericParameterConstraintHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct GenericParameterHandle : IEquatable<GenericParameterHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(GenericParameterHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(GenericParameterHandle left, GenericParameterHandle right) { throw null; }

        public static explicit operator GenericParameterHandle(EntityHandle handle) { throw null; }

        public static explicit operator GenericParameterHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(GenericParameterHandle handle) { throw null; }

        public static implicit operator Handle(GenericParameterHandle handle) { throw null; }

        public static bool operator !=(GenericParameterHandle left, GenericParameterHandle right) { throw null; }
    }

    public readonly partial struct GenericParameterHandleCollection : Collections.Generic.IReadOnlyList<GenericParameterHandle>, Collections.Generic.IEnumerable<GenericParameterHandle>, Collections.IEnumerable, Collections.Generic.IReadOnlyCollection<GenericParameterHandle>
    {
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public GenericParameterHandle this[int index] { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<GenericParameterHandle> Collections.Generic.IEnumerable<GenericParameterHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<GenericParameterHandle>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public GenericParameterHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct GuidHandle : IEquatable<GuidHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(GuidHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(GuidHandle left, GuidHandle right) { throw null; }

        public static explicit operator GuidHandle(Handle handle) { throw null; }

        public static implicit operator Handle(GuidHandle handle) { throw null; }

        public static bool operator !=(GuidHandle left, GuidHandle right) { throw null; }
    }

    public readonly partial struct Handle : IEquatable<Handle>
    {
        private readonly int _dummyPrimitive;
        public static readonly AssemblyDefinitionHandle AssemblyDefinition;
        public static readonly ModuleDefinitionHandle ModuleDefinition;
        public bool IsNil { get { throw null; } }

        public HandleKind Kind { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(Handle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(Handle left, Handle right) { throw null; }

        public static bool operator !=(Handle left, Handle right) { throw null; }
    }

    public sealed partial class HandleComparer : Collections.Generic.IEqualityComparer<Handle>, Collections.Generic.IComparer<Handle>, Collections.Generic.IEqualityComparer<EntityHandle>, Collections.Generic.IComparer<EntityHandle>
    {
        internal HandleComparer() { }

        public static HandleComparer Default { get { throw null; } }

        public int Compare(EntityHandle x, EntityHandle y) { throw null; }

        public int Compare(Handle x, Handle y) { throw null; }

        public bool Equals(EntityHandle x, EntityHandle y) { throw null; }

        public bool Equals(Handle x, Handle y) { throw null; }

        public int GetHashCode(EntityHandle obj) { throw null; }

        public int GetHashCode(Handle obj) { throw null; }
    }

    public enum HandleKind : byte
    {
        ModuleDefinition = 0,
        TypeReference = 1,
        TypeDefinition = 2,
        FieldDefinition = 4,
        MethodDefinition = 6,
        Parameter = 8,
        InterfaceImplementation = 9,
        MemberReference = 10,
        Constant = 11,
        CustomAttribute = 12,
        DeclarativeSecurityAttribute = 14,
        StandaloneSignature = 17,
        EventDefinition = 20,
        PropertyDefinition = 23,
        MethodImplementation = 25,
        ModuleReference = 26,
        TypeSpecification = 27,
        AssemblyDefinition = 32,
        AssemblyReference = 35,
        AssemblyFile = 38,
        ExportedType = 39,
        ManifestResource = 40,
        GenericParameter = 42,
        MethodSpecification = 43,
        GenericParameterConstraint = 44,
        Document = 48,
        MethodDebugInformation = 49,
        LocalScope = 50,
        LocalVariable = 51,
        LocalConstant = 52,
        ImportScope = 53,
        CustomDebugInformation = 55,
        UserString = 112,
        Blob = 113,
        Guid = 114,
        String = 120,
        NamespaceDefinition = 124
    }

    public partial interface IConstructedTypeProvider<TType> : ISZArrayTypeProvider<TType>
    {
        TType GetArrayType(TType elementType, ArrayShape shape);
        TType GetByReferenceType(TType elementType);
        TType GetGenericInstantiation(TType genericType, Collections.Immutable.ImmutableArray<TType> typeArguments);
        TType GetPointerType(TType elementType);
    }

    public partial interface ICustomAttributeTypeProvider<TType> : ISimpleTypeProvider<TType>, ISZArrayTypeProvider<TType>
    {
        TType GetSystemType();
        TType GetTypeFromSerializedName(string name);
        PrimitiveTypeCode GetUnderlyingEnumType(TType type);
        bool IsSystemType(TType type);
    }

    public enum ILOpCode : ushort
    {
        Nop = 0,
        Break = 1,
        Ldarg_0 = 2,
        Ldarg_1 = 3,
        Ldarg_2 = 4,
        Ldarg_3 = 5,
        Ldloc_0 = 6,
        Ldloc_1 = 7,
        Ldloc_2 = 8,
        Ldloc_3 = 9,
        Stloc_0 = 10,
        Stloc_1 = 11,
        Stloc_2 = 12,
        Stloc_3 = 13,
        Ldarg_s = 14,
        Ldarga_s = 15,
        Starg_s = 16,
        Ldloc_s = 17,
        Ldloca_s = 18,
        Stloc_s = 19,
        Ldnull = 20,
        Ldc_i4_m1 = 21,
        Ldc_i4_0 = 22,
        Ldc_i4_1 = 23,
        Ldc_i4_2 = 24,
        Ldc_i4_3 = 25,
        Ldc_i4_4 = 26,
        Ldc_i4_5 = 27,
        Ldc_i4_6 = 28,
        Ldc_i4_7 = 29,
        Ldc_i4_8 = 30,
        Ldc_i4_s = 31,
        Ldc_i4 = 32,
        Ldc_i8 = 33,
        Ldc_r4 = 34,
        Ldc_r8 = 35,
        Dup = 37,
        Pop = 38,
        Jmp = 39,
        Call = 40,
        Calli = 41,
        Ret = 42,
        Br_s = 43,
        Brfalse_s = 44,
        Brtrue_s = 45,
        Beq_s = 46,
        Bge_s = 47,
        Bgt_s = 48,
        Ble_s = 49,
        Blt_s = 50,
        Bne_un_s = 51,
        Bge_un_s = 52,
        Bgt_un_s = 53,
        Ble_un_s = 54,
        Blt_un_s = 55,
        Br = 56,
        Brfalse = 57,
        Brtrue = 58,
        Beq = 59,
        Bge = 60,
        Bgt = 61,
        Ble = 62,
        Blt = 63,
        Bne_un = 64,
        Bge_un = 65,
        Bgt_un = 66,
        Ble_un = 67,
        Blt_un = 68,
        Switch = 69,
        Ldind_i1 = 70,
        Ldind_u1 = 71,
        Ldind_i2 = 72,
        Ldind_u2 = 73,
        Ldind_i4 = 74,
        Ldind_u4 = 75,
        Ldind_i8 = 76,
        Ldind_i = 77,
        Ldind_r4 = 78,
        Ldind_r8 = 79,
        Ldind_ref = 80,
        Stind_ref = 81,
        Stind_i1 = 82,
        Stind_i2 = 83,
        Stind_i4 = 84,
        Stind_i8 = 85,
        Stind_r4 = 86,
        Stind_r8 = 87,
        Add = 88,
        Sub = 89,
        Mul = 90,
        Div = 91,
        Div_un = 92,
        Rem = 93,
        Rem_un = 94,
        And = 95,
        Or = 96,
        Xor = 97,
        Shl = 98,
        Shr = 99,
        Shr_un = 100,
        Neg = 101,
        Not = 102,
        Conv_i1 = 103,
        Conv_i2 = 104,
        Conv_i4 = 105,
        Conv_i8 = 106,
        Conv_r4 = 107,
        Conv_r8 = 108,
        Conv_u4 = 109,
        Conv_u8 = 110,
        Callvirt = 111,
        Cpobj = 112,
        Ldobj = 113,
        Ldstr = 114,
        Newobj = 115,
        Castclass = 116,
        Isinst = 117,
        Conv_r_un = 118,
        Unbox = 121,
        Throw = 122,
        Ldfld = 123,
        Ldflda = 124,
        Stfld = 125,
        Ldsfld = 126,
        Ldsflda = 127,
        Stsfld = 128,
        Stobj = 129,
        Conv_ovf_i1_un = 130,
        Conv_ovf_i2_un = 131,
        Conv_ovf_i4_un = 132,
        Conv_ovf_i8_un = 133,
        Conv_ovf_u1_un = 134,
        Conv_ovf_u2_un = 135,
        Conv_ovf_u4_un = 136,
        Conv_ovf_u8_un = 137,
        Conv_ovf_i_un = 138,
        Conv_ovf_u_un = 139,
        Box = 140,
        Newarr = 141,
        Ldlen = 142,
        Ldelema = 143,
        Ldelem_i1 = 144,
        Ldelem_u1 = 145,
        Ldelem_i2 = 146,
        Ldelem_u2 = 147,
        Ldelem_i4 = 148,
        Ldelem_u4 = 149,
        Ldelem_i8 = 150,
        Ldelem_i = 151,
        Ldelem_r4 = 152,
        Ldelem_r8 = 153,
        Ldelem_ref = 154,
        Stelem_i = 155,
        Stelem_i1 = 156,
        Stelem_i2 = 157,
        Stelem_i4 = 158,
        Stelem_i8 = 159,
        Stelem_r4 = 160,
        Stelem_r8 = 161,
        Stelem_ref = 162,
        Ldelem = 163,
        Stelem = 164,
        Unbox_any = 165,
        Conv_ovf_i1 = 179,
        Conv_ovf_u1 = 180,
        Conv_ovf_i2 = 181,
        Conv_ovf_u2 = 182,
        Conv_ovf_i4 = 183,
        Conv_ovf_u4 = 184,
        Conv_ovf_i8 = 185,
        Conv_ovf_u8 = 186,
        Refanyval = 194,
        Ckfinite = 195,
        Mkrefany = 198,
        Ldtoken = 208,
        Conv_u2 = 209,
        Conv_u1 = 210,
        Conv_i = 211,
        Conv_ovf_i = 212,
        Conv_ovf_u = 213,
        Add_ovf = 214,
        Add_ovf_un = 215,
        Mul_ovf = 216,
        Mul_ovf_un = 217,
        Sub_ovf = 218,
        Sub_ovf_un = 219,
        Endfinally = 220,
        Leave = 221,
        Leave_s = 222,
        Stind_i = 223,
        Conv_u = 224,
        Arglist = 65024,
        Ceq = 65025,
        Cgt = 65026,
        Cgt_un = 65027,
        Clt = 65028,
        Clt_un = 65029,
        Ldftn = 65030,
        Ldvirtftn = 65031,
        Ldarg = 65033,
        Ldarga = 65034,
        Starg = 65035,
        Ldloc = 65036,
        Ldloca = 65037,
        Stloc = 65038,
        Localloc = 65039,
        Endfilter = 65041,
        Unaligned = 65042,
        Volatile = 65043,
        Tail = 65044,
        Initobj = 65045,
        Constrained = 65046,
        Cpblk = 65047,
        Initblk = 65048,
        Rethrow = 65050,
        Sizeof = 65052,
        Refanytype = 65053,
        Readonly = 65054
    }

    public static partial class ILOpCodeExtensions
    {
        public static int GetBranchOperandSize(this ILOpCode opCode) { throw null; }

        public static ILOpCode GetLongBranch(this ILOpCode opCode) { throw null; }

        public static ILOpCode GetShortBranch(this ILOpCode opCode) { throw null; }

        public static bool IsBranch(this ILOpCode opCode) { throw null; }
    }

    public partial class ImageFormatLimitationException : Exception
    {
        public ImageFormatLimitationException() { }

        protected ImageFormatLimitationException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public ImageFormatLimitationException(string? message, Exception? innerException) { }

        public ImageFormatLimitationException(string? message) { }
    }

    public readonly partial struct ImportDefinition
    {
        private readonly int _dummyPrimitive;
        public BlobHandle Alias { get { throw null; } }

        public ImportDefinitionKind Kind { get { throw null; } }

        public AssemblyReferenceHandle TargetAssembly { get { throw null; } }

        public BlobHandle TargetNamespace { get { throw null; } }

        public EntityHandle TargetType { get { throw null; } }
    }

    public readonly partial struct ImportDefinitionCollection : Collections.Generic.IEnumerable<ImportDefinition>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<ImportDefinition> Collections.Generic.IEnumerable<ImportDefinition>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<ImportDefinition>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public ImportDefinition Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            public void Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public enum ImportDefinitionKind
    {
        ImportNamespace = 1,
        ImportAssemblyNamespace = 2,
        ImportType = 3,
        ImportXmlNamespace = 4,
        ImportAssemblyReferenceAlias = 5,
        AliasAssemblyReference = 6,
        AliasNamespace = 7,
        AliasAssemblyNamespace = 8,
        AliasType = 9
    }

    public readonly partial struct ImportScope
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public BlobHandle ImportsBlob { get { throw null; } }

        public ImportScopeHandle Parent { get { throw null; } }

        public readonly ImportDefinitionCollection GetImports() { throw null; }
    }

    public readonly partial struct ImportScopeCollection : Collections.Generic.IReadOnlyCollection<ImportScopeHandle>, Collections.Generic.IEnumerable<ImportScopeHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<ImportScopeHandle> Collections.Generic.IEnumerable<ImportScopeHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<ImportScopeHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public ImportScopeHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct ImportScopeHandle : IEquatable<ImportScopeHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(ImportScopeHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ImportScopeHandle left, ImportScopeHandle right) { throw null; }

        public static explicit operator ImportScopeHandle(EntityHandle handle) { throw null; }

        public static explicit operator ImportScopeHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ImportScopeHandle handle) { throw null; }

        public static implicit operator Handle(ImportScopeHandle handle) { throw null; }

        public static bool operator !=(ImportScopeHandle left, ImportScopeHandle right) { throw null; }
    }

    public readonly partial struct InterfaceImplementation
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public EntityHandle Interface { get { throw null; } }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct InterfaceImplementationHandle : IEquatable<InterfaceImplementationHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(InterfaceImplementationHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(InterfaceImplementationHandle left, InterfaceImplementationHandle right) { throw null; }

        public static explicit operator InterfaceImplementationHandle(EntityHandle handle) { throw null; }

        public static explicit operator InterfaceImplementationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(InterfaceImplementationHandle handle) { throw null; }

        public static implicit operator Handle(InterfaceImplementationHandle handle) { throw null; }

        public static bool operator !=(InterfaceImplementationHandle left, InterfaceImplementationHandle right) { throw null; }
    }

    public readonly partial struct InterfaceImplementationHandleCollection : Collections.Generic.IReadOnlyCollection<InterfaceImplementationHandle>, Collections.Generic.IEnumerable<InterfaceImplementationHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<InterfaceImplementationHandle> Collections.Generic.IEnumerable<InterfaceImplementationHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<InterfaceImplementationHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public InterfaceImplementationHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public partial interface ISignatureTypeProvider<TType, TGenericContext> : ISimpleTypeProvider<TType>, IConstructedTypeProvider<TType>, ISZArrayTypeProvider<TType>
    {
        TType GetFunctionPointerType(MethodSignature<TType> signature);
        TType GetGenericMethodParameter(TGenericContext genericContext, int index);
        TType GetGenericTypeParameter(TGenericContext genericContext, int index);
        TType GetModifiedType(TType modifier, TType unmodifiedType, bool isRequired);
        TType GetPinnedType(TType elementType);
        TType GetTypeFromSpecification(MetadataReader reader, TGenericContext genericContext, TypeSpecificationHandle handle, byte rawTypeKind);
    }

    public partial interface ISimpleTypeProvider<TType>
    {
        TType GetPrimitiveType(PrimitiveTypeCode typeCode);
        TType GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, byte rawTypeKind);
        TType GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, byte rawTypeKind);
    }

    public partial interface ISZArrayTypeProvider<TType>
    {
        TType GetSZArrayType(TType elementType);
    }

    public readonly partial struct LocalConstant
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public StringHandle Name { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }
    }

    public readonly partial struct LocalConstantHandle : IEquatable<LocalConstantHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(LocalConstantHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(LocalConstantHandle left, LocalConstantHandle right) { throw null; }

        public static explicit operator LocalConstantHandle(EntityHandle handle) { throw null; }

        public static explicit operator LocalConstantHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(LocalConstantHandle handle) { throw null; }

        public static implicit operator Handle(LocalConstantHandle handle) { throw null; }

        public static bool operator !=(LocalConstantHandle left, LocalConstantHandle right) { throw null; }
    }

    public readonly partial struct LocalConstantHandleCollection : Collections.Generic.IReadOnlyCollection<LocalConstantHandle>, Collections.Generic.IEnumerable<LocalConstantHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<LocalConstantHandle> Collections.Generic.IEnumerable<LocalConstantHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<LocalConstantHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public LocalConstantHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct LocalScope
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int EndOffset { get { throw null; } }

        public ImportScopeHandle ImportScope { get { throw null; } }

        public int Length { get { throw null; } }

        public MethodDefinitionHandle Method { get { throw null; } }

        public int StartOffset { get { throw null; } }

        public readonly LocalScopeHandleCollection.ChildrenEnumerator GetChildren() { throw null; }

        public readonly LocalConstantHandleCollection GetLocalConstants() { throw null; }

        public readonly LocalVariableHandleCollection GetLocalVariables() { throw null; }
    }

    public readonly partial struct LocalScopeHandle : IEquatable<LocalScopeHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(LocalScopeHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(LocalScopeHandle left, LocalScopeHandle right) { throw null; }

        public static explicit operator LocalScopeHandle(EntityHandle handle) { throw null; }

        public static explicit operator LocalScopeHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(LocalScopeHandle handle) { throw null; }

        public static implicit operator Handle(LocalScopeHandle handle) { throw null; }

        public static bool operator !=(LocalScopeHandle left, LocalScopeHandle right) { throw null; }
    }

    public readonly partial struct LocalScopeHandleCollection : Collections.Generic.IReadOnlyCollection<LocalScopeHandle>, Collections.Generic.IEnumerable<LocalScopeHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<LocalScopeHandle> Collections.Generic.IEnumerable<LocalScopeHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct ChildrenEnumerator : Collections.Generic.IEnumerator<LocalScopeHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public LocalScopeHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }

        public partial struct Enumerator : Collections.Generic.IEnumerator<LocalScopeHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public LocalScopeHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct LocalVariable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public LocalVariableAttributes Attributes { get { throw null; } }

        public int Index { get { throw null; } }

        public StringHandle Name { get { throw null; } }
    }

    [Flags]
    public enum LocalVariableAttributes
    {
        None = 0,
        DebuggerHidden = 1
    }

    public readonly partial struct LocalVariableHandle : IEquatable<LocalVariableHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(LocalVariableHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(LocalVariableHandle left, LocalVariableHandle right) { throw null; }

        public static explicit operator LocalVariableHandle(EntityHandle handle) { throw null; }

        public static explicit operator LocalVariableHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(LocalVariableHandle handle) { throw null; }

        public static implicit operator Handle(LocalVariableHandle handle) { throw null; }

        public static bool operator !=(LocalVariableHandle left, LocalVariableHandle right) { throw null; }
    }

    public readonly partial struct LocalVariableHandleCollection : Collections.Generic.IReadOnlyCollection<LocalVariableHandle>, Collections.Generic.IEnumerable<LocalVariableHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<LocalVariableHandle> Collections.Generic.IEnumerable<LocalVariableHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<LocalVariableHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public LocalVariableHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct ManifestResource
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ManifestResourceAttributes Attributes { get { throw null; } }

        public EntityHandle Implementation { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public long Offset { get { throw null; } }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct ManifestResourceHandle : IEquatable<ManifestResourceHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(ManifestResourceHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ManifestResourceHandle left, ManifestResourceHandle right) { throw null; }

        public static explicit operator ManifestResourceHandle(EntityHandle handle) { throw null; }

        public static explicit operator ManifestResourceHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ManifestResourceHandle handle) { throw null; }

        public static implicit operator Handle(ManifestResourceHandle handle) { throw null; }

        public static bool operator !=(ManifestResourceHandle left, ManifestResourceHandle right) { throw null; }
    }

    public readonly partial struct ManifestResourceHandleCollection : Collections.Generic.IReadOnlyCollection<ManifestResourceHandle>, Collections.Generic.IEnumerable<ManifestResourceHandle>, Collections.IEnumerable
    {
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<ManifestResourceHandle> Collections.Generic.IEnumerable<ManifestResourceHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<ManifestResourceHandle>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public ManifestResourceHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct MemberReference
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public StringHandle Name { get { throw null; } }

        public EntityHandle Parent { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }

        public readonly TType DecodeFieldSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext) { throw null; }

        public readonly MethodSignature<TType> DecodeMethodSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext) { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public readonly MemberReferenceKind GetKind() { throw null; }
    }

    public readonly partial struct MemberReferenceHandle : IEquatable<MemberReferenceHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(MemberReferenceHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(MemberReferenceHandle left, MemberReferenceHandle right) { throw null; }

        public static explicit operator MemberReferenceHandle(EntityHandle handle) { throw null; }

        public static explicit operator MemberReferenceHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(MemberReferenceHandle handle) { throw null; }

        public static implicit operator Handle(MemberReferenceHandle handle) { throw null; }

        public static bool operator !=(MemberReferenceHandle left, MemberReferenceHandle right) { throw null; }
    }

    public readonly partial struct MemberReferenceHandleCollection : Collections.Generic.IReadOnlyCollection<MemberReferenceHandle>, Collections.Generic.IEnumerable<MemberReferenceHandle>, Collections.IEnumerable
    {
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<MemberReferenceHandle> Collections.Generic.IEnumerable<MemberReferenceHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<MemberReferenceHandle>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public MemberReferenceHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public enum MemberReferenceKind
    {
        Method = 0,
        Field = 1
    }

    public enum MetadataKind
    {
        Ecma335 = 0,
        WindowsMetadata = 1,
        ManagedWindowsMetadata = 2
    }

    public sealed partial class MetadataReader
    {
        public unsafe MetadataReader(byte* metadata, int length, MetadataReaderOptions options, MetadataStringDecoder? utf8Decoder) { }

        public unsafe MetadataReader(byte* metadata, int length, MetadataReaderOptions options) { }

        public unsafe MetadataReader(byte* metadata, int length) { }

        public AssemblyFileHandleCollection AssemblyFiles { get { throw null; } }

        public AssemblyReferenceHandleCollection AssemblyReferences { get { throw null; } }

        public CustomAttributeHandleCollection CustomAttributes { get { throw null; } }

        public CustomDebugInformationHandleCollection CustomDebugInformation { get { throw null; } }

        public DebugMetadataHeader? DebugMetadataHeader { get { throw null; } }

        public DeclarativeSecurityAttributeHandleCollection DeclarativeSecurityAttributes { get { throw null; } }

        public DocumentHandleCollection Documents { get { throw null; } }

        public EventDefinitionHandleCollection EventDefinitions { get { throw null; } }

        public ExportedTypeHandleCollection ExportedTypes { get { throw null; } }

        public FieldDefinitionHandleCollection FieldDefinitions { get { throw null; } }

        public ImportScopeCollection ImportScopes { get { throw null; } }

        public bool IsAssembly { get { throw null; } }

        public LocalConstantHandleCollection LocalConstants { get { throw null; } }

        public LocalScopeHandleCollection LocalScopes { get { throw null; } }

        public LocalVariableHandleCollection LocalVariables { get { throw null; } }

        public ManifestResourceHandleCollection ManifestResources { get { throw null; } }

        public MemberReferenceHandleCollection MemberReferences { get { throw null; } }

        public MetadataKind MetadataKind { get { throw null; } }

        public int MetadataLength { get { throw null; } }

        public unsafe byte* MetadataPointer { get { throw null; } }

        public string MetadataVersion { get { throw null; } }

        public MethodDebugInformationHandleCollection MethodDebugInformation { get { throw null; } }

        public MethodDefinitionHandleCollection MethodDefinitions { get { throw null; } }

        public MetadataReaderOptions Options { get { throw null; } }

        public PropertyDefinitionHandleCollection PropertyDefinitions { get { throw null; } }

        public MetadataStringComparer StringComparer { get { throw null; } }

        public TypeDefinitionHandleCollection TypeDefinitions { get { throw null; } }

        public TypeReferenceHandleCollection TypeReferences { get { throw null; } }

        public MetadataStringDecoder UTF8Decoder { get { throw null; } }

        public AssemblyDefinition GetAssemblyDefinition() { throw null; }

        public AssemblyFile GetAssemblyFile(AssemblyFileHandle handle) { throw null; }

        public static AssemblyName GetAssemblyName(string assemblyFile) { throw null; }

        public AssemblyReference GetAssemblyReference(AssemblyReferenceHandle handle) { throw null; }

        public byte[] GetBlobBytes(BlobHandle handle) { throw null; }

        public Collections.Immutable.ImmutableArray<byte> GetBlobContent(BlobHandle handle) { throw null; }

        public BlobReader GetBlobReader(BlobHandle handle) { throw null; }

        public BlobReader GetBlobReader(StringHandle handle) { throw null; }

        public Constant GetConstant(ConstantHandle handle) { throw null; }

        public CustomAttribute GetCustomAttribute(CustomAttributeHandle handle) { throw null; }

        public CustomAttributeHandleCollection GetCustomAttributes(EntityHandle handle) { throw null; }

        public CustomDebugInformation GetCustomDebugInformation(CustomDebugInformationHandle handle) { throw null; }

        public CustomDebugInformationHandleCollection GetCustomDebugInformation(EntityHandle handle) { throw null; }

        public DeclarativeSecurityAttribute GetDeclarativeSecurityAttribute(DeclarativeSecurityAttributeHandle handle) { throw null; }

        public Document GetDocument(DocumentHandle handle) { throw null; }

        public EventDefinition GetEventDefinition(EventDefinitionHandle handle) { throw null; }

        public ExportedType GetExportedType(ExportedTypeHandle handle) { throw null; }

        public FieldDefinition GetFieldDefinition(FieldDefinitionHandle handle) { throw null; }

        public GenericParameter GetGenericParameter(GenericParameterHandle handle) { throw null; }

        public GenericParameterConstraint GetGenericParameterConstraint(GenericParameterConstraintHandle handle) { throw null; }

        public Guid GetGuid(GuidHandle handle) { throw null; }

        public ImportScope GetImportScope(ImportScopeHandle handle) { throw null; }

        public InterfaceImplementation GetInterfaceImplementation(InterfaceImplementationHandle handle) { throw null; }

        public LocalConstant GetLocalConstant(LocalConstantHandle handle) { throw null; }

        public LocalScope GetLocalScope(LocalScopeHandle handle) { throw null; }

        public LocalScopeHandleCollection GetLocalScopes(MethodDebugInformationHandle handle) { throw null; }

        public LocalScopeHandleCollection GetLocalScopes(MethodDefinitionHandle handle) { throw null; }

        public LocalVariable GetLocalVariable(LocalVariableHandle handle) { throw null; }

        public ManifestResource GetManifestResource(ManifestResourceHandle handle) { throw null; }

        public MemberReference GetMemberReference(MemberReferenceHandle handle) { throw null; }

        public MethodDebugInformation GetMethodDebugInformation(MethodDebugInformationHandle handle) { throw null; }

        public MethodDebugInformation GetMethodDebugInformation(MethodDefinitionHandle handle) { throw null; }

        public MethodDefinition GetMethodDefinition(MethodDefinitionHandle handle) { throw null; }

        public MethodImplementation GetMethodImplementation(MethodImplementationHandle handle) { throw null; }

        public MethodSpecification GetMethodSpecification(MethodSpecificationHandle handle) { throw null; }

        public ModuleDefinition GetModuleDefinition() { throw null; }

        public ModuleReference GetModuleReference(ModuleReferenceHandle handle) { throw null; }

        public NamespaceDefinition GetNamespaceDefinition(NamespaceDefinitionHandle handle) { throw null; }

        public NamespaceDefinition GetNamespaceDefinitionRoot() { throw null; }

        public Parameter GetParameter(ParameterHandle handle) { throw null; }

        public PropertyDefinition GetPropertyDefinition(PropertyDefinitionHandle handle) { throw null; }

        public StandaloneSignature GetStandaloneSignature(StandaloneSignatureHandle handle) { throw null; }

        public string GetString(DocumentNameBlobHandle handle) { throw null; }

        public string GetString(NamespaceDefinitionHandle handle) { throw null; }

        public string GetString(StringHandle handle) { throw null; }

        public TypeDefinition GetTypeDefinition(TypeDefinitionHandle handle) { throw null; }

        public TypeReference GetTypeReference(TypeReferenceHandle handle) { throw null; }

        public TypeSpecification GetTypeSpecification(TypeSpecificationHandle handle) { throw null; }

        public string GetUserString(UserStringHandle handle) { throw null; }
    }

    [Flags]
    public enum MetadataReaderOptions
    {
        None = 0,
        ApplyWindowsRuntimeProjections = 1,
        Default = 1
    }

    public sealed partial class MetadataReaderProvider : IDisposable
    {
        internal MetadataReaderProvider() { }

        public void Dispose() { }

        public static unsafe MetadataReaderProvider FromMetadataImage(byte* start, int size) { throw null; }

        public static MetadataReaderProvider FromMetadataImage(Collections.Immutable.ImmutableArray<byte> image) { throw null; }

        public static MetadataReaderProvider FromMetadataStream(IO.Stream stream, MetadataStreamOptions options = MetadataStreamOptions.Default, int size = 0) { throw null; }

        public static unsafe MetadataReaderProvider FromPortablePdbImage(byte* start, int size) { throw null; }

        public static MetadataReaderProvider FromPortablePdbImage(Collections.Immutable.ImmutableArray<byte> image) { throw null; }

        public static MetadataReaderProvider FromPortablePdbStream(IO.Stream stream, MetadataStreamOptions options = MetadataStreamOptions.Default, int size = 0) { throw null; }

        public MetadataReader GetMetadataReader(MetadataReaderOptions options = MetadataReaderOptions.ApplyWindowsRuntimeProjections, MetadataStringDecoder? utf8Decoder = null) { throw null; }
    }

    [Flags]
    public enum MetadataStreamOptions
    {
        Default = 0,
        LeaveOpen = 1,
        PrefetchMetadata = 2
    }

    public readonly partial struct MetadataStringComparer
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public readonly bool Equals(DocumentNameBlobHandle handle, string value, bool ignoreCase) { throw null; }

        public readonly bool Equals(DocumentNameBlobHandle handle, string value) { throw null; }

        public readonly bool Equals(NamespaceDefinitionHandle handle, string value, bool ignoreCase) { throw null; }

        public readonly bool Equals(NamespaceDefinitionHandle handle, string value) { throw null; }

        public readonly bool Equals(StringHandle handle, string value, bool ignoreCase) { throw null; }

        public readonly bool Equals(StringHandle handle, string value) { throw null; }

        public readonly bool StartsWith(StringHandle handle, string value, bool ignoreCase) { throw null; }

        public readonly bool StartsWith(StringHandle handle, string value) { throw null; }
    }

    public partial class MetadataStringDecoder
    {
        public MetadataStringDecoder(Text.Encoding encoding) { }

        public static MetadataStringDecoder DefaultUTF8 { get { throw null; } }

        public Text.Encoding Encoding { get { throw null; } }

        public virtual unsafe string GetString(byte* bytes, int byteCount) { throw null; }
    }

    public sealed partial class MethodBodyBlock
    {
        internal MethodBodyBlock() { }

        public Collections.Immutable.ImmutableArray<ExceptionRegion> ExceptionRegions { get { throw null; } }

        public StandaloneSignatureHandle LocalSignature { get { throw null; } }

        public bool LocalVariablesInitialized { get { throw null; } }

        public int MaxStack { get { throw null; } }

        public int Size { get { throw null; } }

        public static MethodBodyBlock Create(BlobReader reader) { throw null; }

        public byte[]? GetILBytes() { throw null; }

        public Collections.Immutable.ImmutableArray<byte> GetILContent() { throw null; }

        public BlobReader GetILReader() { throw null; }
    }

    public readonly partial struct MethodDebugInformation
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public DocumentHandle Document { get { throw null; } }

        public StandaloneSignatureHandle LocalSignature { get { throw null; } }

        public BlobHandle SequencePointsBlob { get { throw null; } }

        public readonly SequencePointCollection GetSequencePoints() { throw null; }

        public readonly MethodDefinitionHandle GetStateMachineKickoffMethod() { throw null; }
    }

    public readonly partial struct MethodDebugInformationHandle : IEquatable<MethodDebugInformationHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(MethodDebugInformationHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(MethodDebugInformationHandle left, MethodDebugInformationHandle right) { throw null; }

        public static explicit operator MethodDebugInformationHandle(EntityHandle handle) { throw null; }

        public static explicit operator MethodDebugInformationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(MethodDebugInformationHandle handle) { throw null; }

        public static implicit operator Handle(MethodDebugInformationHandle handle) { throw null; }

        public static bool operator !=(MethodDebugInformationHandle left, MethodDebugInformationHandle right) { throw null; }

        public readonly MethodDefinitionHandle ToDefinitionHandle() { throw null; }
    }

    public readonly partial struct MethodDebugInformationHandleCollection : Collections.Generic.IReadOnlyCollection<MethodDebugInformationHandle>, Collections.Generic.IEnumerable<MethodDebugInformationHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<MethodDebugInformationHandle> Collections.Generic.IEnumerable<MethodDebugInformationHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<MethodDebugInformationHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public MethodDebugInformationHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct MethodDefinition
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public MethodAttributes Attributes { get { throw null; } }

        public MethodImplAttributes ImplAttributes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public int RelativeVirtualAddress { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }

        public readonly MethodSignature<TType> DecodeSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext) { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public readonly DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes() { throw null; }

        public readonly TypeDefinitionHandle GetDeclaringType() { throw null; }

        public readonly GenericParameterHandleCollection GetGenericParameters() { throw null; }

        public readonly MethodImport GetImport() { throw null; }

        public readonly ParameterHandleCollection GetParameters() { throw null; }
    }

    public readonly partial struct MethodDefinitionHandle : IEquatable<MethodDefinitionHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(MethodDefinitionHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(MethodDefinitionHandle left, MethodDefinitionHandle right) { throw null; }

        public static explicit operator MethodDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator MethodDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(MethodDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(MethodDefinitionHandle handle) { throw null; }

        public static bool operator !=(MethodDefinitionHandle left, MethodDefinitionHandle right) { throw null; }

        public readonly MethodDebugInformationHandle ToDebugInformationHandle() { throw null; }
    }

    public readonly partial struct MethodDefinitionHandleCollection : Collections.Generic.IReadOnlyCollection<MethodDefinitionHandle>, Collections.Generic.IEnumerable<MethodDefinitionHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<MethodDefinitionHandle> Collections.Generic.IEnumerable<MethodDefinitionHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<MethodDefinitionHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public MethodDefinitionHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct MethodImplementation
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public EntityHandle MethodBody { get { throw null; } }

        public EntityHandle MethodDeclaration { get { throw null; } }

        public TypeDefinitionHandle Type { get { throw null; } }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct MethodImplementationHandle : IEquatable<MethodImplementationHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(MethodImplementationHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(MethodImplementationHandle left, MethodImplementationHandle right) { throw null; }

        public static explicit operator MethodImplementationHandle(EntityHandle handle) { throw null; }

        public static explicit operator MethodImplementationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(MethodImplementationHandle handle) { throw null; }

        public static implicit operator Handle(MethodImplementationHandle handle) { throw null; }

        public static bool operator !=(MethodImplementationHandle left, MethodImplementationHandle right) { throw null; }
    }

    public readonly partial struct MethodImplementationHandleCollection : Collections.Generic.IReadOnlyCollection<MethodImplementationHandle>, Collections.Generic.IEnumerable<MethodImplementationHandle>, Collections.IEnumerable
    {
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<MethodImplementationHandle> Collections.Generic.IEnumerable<MethodImplementationHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<MethodImplementationHandle>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public MethodImplementationHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct MethodImport
    {
        private readonly int _dummyPrimitive;
        public MethodImportAttributes Attributes { get { throw null; } }

        public ModuleReferenceHandle Module { get { throw null; } }

        public StringHandle Name { get { throw null; } }
    }

    public readonly partial struct MethodSignature<TType>
    {
        private readonly TType _ReturnType_k__BackingField;
        private readonly Collections.Immutable.ImmutableArray<TType> _ParameterTypes_k__BackingField;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public MethodSignature(SignatureHeader header, TType returnType, int requiredParameterCount, int genericParameterCount, Collections.Immutable.ImmutableArray<TType> parameterTypes) { }

        public int GenericParameterCount { get { throw null; } }

        public SignatureHeader Header { get { throw null; } }

        public Collections.Immutable.ImmutableArray<TType> ParameterTypes { get { throw null; } }

        public int RequiredParameterCount { get { throw null; } }

        public TType ReturnType { get { throw null; } }
    }

    public readonly partial struct MethodSpecification
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public EntityHandle Method { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }

        public readonly Collections.Immutable.ImmutableArray<TType> DecodeSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext) { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct MethodSpecificationHandle : IEquatable<MethodSpecificationHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(MethodSpecificationHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(MethodSpecificationHandle left, MethodSpecificationHandle right) { throw null; }

        public static explicit operator MethodSpecificationHandle(EntityHandle handle) { throw null; }

        public static explicit operator MethodSpecificationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(MethodSpecificationHandle handle) { throw null; }

        public static implicit operator Handle(MethodSpecificationHandle handle) { throw null; }

        public static bool operator !=(MethodSpecificationHandle left, MethodSpecificationHandle right) { throw null; }
    }

    public readonly partial struct ModuleDefinition
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public GuidHandle BaseGenerationId { get { throw null; } }

        public int Generation { get { throw null; } }

        public GuidHandle GenerationId { get { throw null; } }

        public GuidHandle Mvid { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct ModuleDefinitionHandle : IEquatable<ModuleDefinitionHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(ModuleDefinitionHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ModuleDefinitionHandle left, ModuleDefinitionHandle right) { throw null; }

        public static explicit operator ModuleDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator ModuleDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ModuleDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(ModuleDefinitionHandle handle) { throw null; }

        public static bool operator !=(ModuleDefinitionHandle left, ModuleDefinitionHandle right) { throw null; }
    }

    public readonly partial struct ModuleReference
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public StringHandle Name { get { throw null; } }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct ModuleReferenceHandle : IEquatable<ModuleReferenceHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(ModuleReferenceHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ModuleReferenceHandle left, ModuleReferenceHandle right) { throw null; }

        public static explicit operator ModuleReferenceHandle(EntityHandle handle) { throw null; }

        public static explicit operator ModuleReferenceHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ModuleReferenceHandle handle) { throw null; }

        public static implicit operator Handle(ModuleReferenceHandle handle) { throw null; }

        public static bool operator !=(ModuleReferenceHandle left, ModuleReferenceHandle right) { throw null; }
    }

    public partial struct NamespaceDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public Collections.Immutable.ImmutableArray<ExportedTypeHandle> ExportedTypes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public Collections.Immutable.ImmutableArray<NamespaceDefinitionHandle> NamespaceDefinitions { get { throw null; } }

        public NamespaceDefinitionHandle Parent { get { throw null; } }

        public Collections.Immutable.ImmutableArray<TypeDefinitionHandle> TypeDefinitions { get { throw null; } }
    }

    public readonly partial struct NamespaceDefinitionHandle : IEquatable<NamespaceDefinitionHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(NamespaceDefinitionHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(NamespaceDefinitionHandle left, NamespaceDefinitionHandle right) { throw null; }

        public static explicit operator NamespaceDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator Handle(NamespaceDefinitionHandle handle) { throw null; }

        public static bool operator !=(NamespaceDefinitionHandle left, NamespaceDefinitionHandle right) { throw null; }
    }

    public readonly partial struct Parameter
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ParameterAttributes Attributes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public int SequenceNumber { get { throw null; } }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public readonly ConstantHandle GetDefaultValue() { throw null; }

        public readonly BlobHandle GetMarshallingDescriptor() { throw null; }
    }

    public readonly partial struct ParameterHandle : IEquatable<ParameterHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(ParameterHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(ParameterHandle left, ParameterHandle right) { throw null; }

        public static explicit operator ParameterHandle(EntityHandle handle) { throw null; }

        public static explicit operator ParameterHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ParameterHandle handle) { throw null; }

        public static implicit operator Handle(ParameterHandle handle) { throw null; }

        public static bool operator !=(ParameterHandle left, ParameterHandle right) { throw null; }
    }

    public readonly partial struct ParameterHandleCollection : Collections.Generic.IReadOnlyCollection<ParameterHandle>, Collections.Generic.IEnumerable<ParameterHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<ParameterHandle> Collections.Generic.IEnumerable<ParameterHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<ParameterHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public ParameterHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public static partial class PEReaderExtensions
    {
        public static MetadataReader GetMetadataReader(this PortableExecutable.PEReader peReader, MetadataReaderOptions options, MetadataStringDecoder? utf8Decoder) { throw null; }

        public static MetadataReader GetMetadataReader(this PortableExecutable.PEReader peReader, MetadataReaderOptions options) { throw null; }

        public static MetadataReader GetMetadataReader(this PortableExecutable.PEReader peReader) { throw null; }

        public static MethodBodyBlock GetMethodBody(this PortableExecutable.PEReader peReader, int relativeVirtualAddress) { throw null; }
    }

    public enum PrimitiveSerializationTypeCode : byte
    {
        Boolean = 2,
        Char = 3,
        SByte = 4,
        Byte = 5,
        Int16 = 6,
        UInt16 = 7,
        Int32 = 8,
        UInt32 = 9,
        Int64 = 10,
        UInt64 = 11,
        Single = 12,
        Double = 13,
        String = 14
    }

    public enum PrimitiveTypeCode : byte
    {
        Void = 1,
        Boolean = 2,
        Char = 3,
        SByte = 4,
        Byte = 5,
        Int16 = 6,
        UInt16 = 7,
        Int32 = 8,
        UInt32 = 9,
        Int64 = 10,
        UInt64 = 11,
        Single = 12,
        Double = 13,
        String = 14,
        TypedReference = 22,
        IntPtr = 24,
        UIntPtr = 25,
        Object = 28
    }

    public readonly partial struct PropertyAccessors
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public MethodDefinitionHandle Getter { get { throw null; } }

        public Collections.Immutable.ImmutableArray<MethodDefinitionHandle> Others { get { throw null; } }

        public MethodDefinitionHandle Setter { get { throw null; } }
    }

    public readonly partial struct PropertyDefinition
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public PropertyAttributes Attributes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }

        public readonly MethodSignature<TType> DecodeSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext) { throw null; }

        public readonly PropertyAccessors GetAccessors() { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public readonly ConstantHandle GetDefaultValue() { throw null; }
    }

    public readonly partial struct PropertyDefinitionHandle : IEquatable<PropertyDefinitionHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(PropertyDefinitionHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(PropertyDefinitionHandle left, PropertyDefinitionHandle right) { throw null; }

        public static explicit operator PropertyDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator PropertyDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(PropertyDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(PropertyDefinitionHandle handle) { throw null; }

        public static bool operator !=(PropertyDefinitionHandle left, PropertyDefinitionHandle right) { throw null; }
    }

    public readonly partial struct PropertyDefinitionHandleCollection : Collections.Generic.IReadOnlyCollection<PropertyDefinitionHandle>, Collections.Generic.IEnumerable<PropertyDefinitionHandle>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<PropertyDefinitionHandle> Collections.Generic.IEnumerable<PropertyDefinitionHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<PropertyDefinitionHandle>, Collections.IEnumerator, IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public PropertyDefinitionHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct ReservedBlob<THandle>
        where THandle : struct
    {
        private readonly THandle _Handle_k__BackingField;
        public Blob Content { get { throw null; } }

        public THandle Handle { get { throw null; } }

        public readonly BlobWriter CreateWriter() { throw null; }
    }

    public readonly partial struct SequencePoint : IEquatable<SequencePoint>
    {
        private readonly int _dummyPrimitive;
        public const int HiddenLine = 16707566;
        public DocumentHandle Document { get { throw null; } }

        public int EndColumn { get { throw null; } }

        public int EndLine { get { throw null; } }

        public bool IsHidden { get { throw null; } }

        public int Offset { get { throw null; } }

        public int StartColumn { get { throw null; } }

        public int StartLine { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(SequencePoint other) { throw null; }

        public override readonly int GetHashCode() { throw null; }
    }

    public readonly partial struct SequencePointCollection : Collections.Generic.IEnumerable<SequencePoint>, Collections.IEnumerable
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<SequencePoint> Collections.Generic.IEnumerable<SequencePoint>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<SequencePoint>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public SequencePoint Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            public void Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public enum SerializationTypeCode : byte
    {
        Invalid = 0,
        Boolean = 2,
        Char = 3,
        SByte = 4,
        Byte = 5,
        Int16 = 6,
        UInt16 = 7,
        Int32 = 8,
        UInt32 = 9,
        Int64 = 10,
        UInt64 = 11,
        Single = 12,
        Double = 13,
        String = 14,
        SZArray = 29,
        Type = 80,
        TaggedObject = 81,
        Enum = 85
    }

    [Flags]
    public enum SignatureAttributes : byte
    {
        None = 0,
        Generic = 16,
        Instance = 32,
        ExplicitThis = 64
    }

    public enum SignatureCallingConvention : byte
    {
        Default = 0,
        CDecl = 1,
        StdCall = 2,
        ThisCall = 3,
        FastCall = 4,
        VarArgs = 5,
        Unmanaged = 9
    }

    public partial struct SignatureHeader : IEquatable<SignatureHeader>
    {
        private int _dummyPrimitive;
        public const byte CallingConventionOrKindMask = 15;
        public SignatureHeader(byte rawValue) { }

        public SignatureHeader(SignatureKind kind, SignatureCallingConvention convention, SignatureAttributes attributes) { }

        public SignatureAttributes Attributes { get { throw null; } }

        public SignatureCallingConvention CallingConvention { get { throw null; } }

        public bool HasExplicitThis { get { throw null; } }

        public bool IsGeneric { get { throw null; } }

        public bool IsInstance { get { throw null; } }

        public SignatureKind Kind { get { throw null; } }

        public byte RawValue { get { throw null; } }

        public override bool Equals(object? obj) { throw null; }

        public bool Equals(SignatureHeader other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(SignatureHeader left, SignatureHeader right) { throw null; }

        public static bool operator !=(SignatureHeader left, SignatureHeader right) { throw null; }

        public override string ToString() { throw null; }
    }

    public enum SignatureKind : byte
    {
        Method = 0,
        Field = 6,
        LocalVariables = 7,
        Property = 8,
        MethodSpecification = 10
    }

    public enum SignatureTypeCode : byte
    {
        Invalid = 0,
        Void = 1,
        Boolean = 2,
        Char = 3,
        SByte = 4,
        Byte = 5,
        Int16 = 6,
        UInt16 = 7,
        Int32 = 8,
        UInt32 = 9,
        Int64 = 10,
        UInt64 = 11,
        Single = 12,
        Double = 13,
        String = 14,
        Pointer = 15,
        ByReference = 16,
        GenericTypeParameter = 19,
        Array = 20,
        GenericTypeInstance = 21,
        TypedReference = 22,
        IntPtr = 24,
        UIntPtr = 25,
        FunctionPointer = 27,
        Object = 28,
        SZArray = 29,
        GenericMethodParameter = 30,
        RequiredModifier = 31,
        OptionalModifier = 32,
        TypeHandle = 64,
        Sentinel = 65,
        Pinned = 69
    }

    public enum SignatureTypeKind : byte
    {
        Unknown = 0,
        ValueType = 17,
        Class = 18
    }

    public readonly partial struct StandaloneSignature
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public BlobHandle Signature { get { throw null; } }

        public readonly Collections.Immutable.ImmutableArray<TType> DecodeLocalSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext) { throw null; }

        public readonly MethodSignature<TType> DecodeMethodSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext) { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public readonly StandaloneSignatureKind GetKind() { throw null; }
    }

    public readonly partial struct StandaloneSignatureHandle : IEquatable<StandaloneSignatureHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(StandaloneSignatureHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(StandaloneSignatureHandle left, StandaloneSignatureHandle right) { throw null; }

        public static explicit operator StandaloneSignatureHandle(EntityHandle handle) { throw null; }

        public static explicit operator StandaloneSignatureHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(StandaloneSignatureHandle handle) { throw null; }

        public static implicit operator Handle(StandaloneSignatureHandle handle) { throw null; }

        public static bool operator !=(StandaloneSignatureHandle left, StandaloneSignatureHandle right) { throw null; }
    }

    public enum StandaloneSignatureKind
    {
        Method = 0,
        LocalVariables = 1
    }

    public readonly partial struct StringHandle : IEquatable<StringHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(StringHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(StringHandle left, StringHandle right) { throw null; }

        public static explicit operator StringHandle(Handle handle) { throw null; }

        public static implicit operator Handle(StringHandle handle) { throw null; }

        public static bool operator !=(StringHandle left, StringHandle right) { throw null; }
    }

    public readonly partial struct TypeDefinition
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public TypeAttributes Attributes { get { throw null; } }

        public EntityHandle BaseType { get { throw null; } }

        public bool IsNested { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public StringHandle Namespace { get { throw null; } }

        public NamespaceDefinitionHandle NamespaceDefinition { get { throw null; } }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public readonly DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes() { throw null; }

        public readonly TypeDefinitionHandle GetDeclaringType() { throw null; }

        public readonly EventDefinitionHandleCollection GetEvents() { throw null; }

        public readonly FieldDefinitionHandleCollection GetFields() { throw null; }

        public readonly GenericParameterHandleCollection GetGenericParameters() { throw null; }

        public readonly InterfaceImplementationHandleCollection GetInterfaceImplementations() { throw null; }

        public readonly TypeLayout GetLayout() { throw null; }

        public readonly MethodImplementationHandleCollection GetMethodImplementations() { throw null; }

        public readonly MethodDefinitionHandleCollection GetMethods() { throw null; }

        public readonly Collections.Immutable.ImmutableArray<TypeDefinitionHandle> GetNestedTypes() { throw null; }

        public readonly PropertyDefinitionHandleCollection GetProperties() { throw null; }
    }

    public readonly partial struct TypeDefinitionHandle : IEquatable<TypeDefinitionHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(TypeDefinitionHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(TypeDefinitionHandle left, TypeDefinitionHandle right) { throw null; }

        public static explicit operator TypeDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator TypeDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(TypeDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(TypeDefinitionHandle handle) { throw null; }

        public static bool operator !=(TypeDefinitionHandle left, TypeDefinitionHandle right) { throw null; }
    }

    public readonly partial struct TypeDefinitionHandleCollection : Collections.Generic.IReadOnlyCollection<TypeDefinitionHandle>, Collections.Generic.IEnumerable<TypeDefinitionHandle>, Collections.IEnumerable
    {
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<TypeDefinitionHandle> Collections.Generic.IEnumerable<TypeDefinitionHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<TypeDefinitionHandle>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public TypeDefinitionHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct TypeLayout
    {
        private readonly int _dummyPrimitive;
        public TypeLayout(int size, int packingSize) { }

        public bool IsDefault { get { throw null; } }

        public int PackingSize { get { throw null; } }

        public int Size { get { throw null; } }
    }

    public readonly partial struct TypeReference
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public StringHandle Name { get { throw null; } }

        public StringHandle Namespace { get { throw null; } }

        public EntityHandle ResolutionScope { get { throw null; } }
    }

    public readonly partial struct TypeReferenceHandle : IEquatable<TypeReferenceHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(TypeReferenceHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(TypeReferenceHandle left, TypeReferenceHandle right) { throw null; }

        public static explicit operator TypeReferenceHandle(EntityHandle handle) { throw null; }

        public static explicit operator TypeReferenceHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(TypeReferenceHandle handle) { throw null; }

        public static implicit operator Handle(TypeReferenceHandle handle) { throw null; }

        public static bool operator !=(TypeReferenceHandle left, TypeReferenceHandle right) { throw null; }
    }

    public readonly partial struct TypeReferenceHandleCollection : Collections.Generic.IReadOnlyCollection<TypeReferenceHandle>, Collections.Generic.IEnumerable<TypeReferenceHandle>, Collections.IEnumerable
    {
        private readonly int _dummyPrimitive;
        public int Count { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        readonly Collections.Generic.IEnumerator<TypeReferenceHandle> Collections.Generic.IEnumerable<TypeReferenceHandle>.GetEnumerator() { throw null; }

        readonly Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public partial struct Enumerator : Collections.Generic.IEnumerator<TypeReferenceHandle>, Collections.IEnumerator, IDisposable
        {
            private int _dummyPrimitive;
            public TypeReferenceHandle Current { get { throw null; } }

            object Collections.IEnumerator.Current { get { throw null; } }

            public bool MoveNext() { throw null; }

            void Collections.IEnumerator.Reset() { }

            void IDisposable.Dispose() { }
        }
    }

    public readonly partial struct TypeSpecification
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public BlobHandle Signature { get { throw null; } }

        public readonly TType DecodeSignature<TType, TGenericContext>(ISignatureTypeProvider<TType, TGenericContext> provider, TGenericContext genericContext) { throw null; }

        public readonly CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public readonly partial struct TypeSpecificationHandle : IEquatable<TypeSpecificationHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(TypeSpecificationHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(TypeSpecificationHandle left, TypeSpecificationHandle right) { throw null; }

        public static explicit operator TypeSpecificationHandle(EntityHandle handle) { throw null; }

        public static explicit operator TypeSpecificationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(TypeSpecificationHandle handle) { throw null; }

        public static implicit operator Handle(TypeSpecificationHandle handle) { throw null; }

        public static bool operator !=(TypeSpecificationHandle left, TypeSpecificationHandle right) { throw null; }
    }

    public readonly partial struct UserStringHandle : IEquatable<UserStringHandle>
    {
        private readonly int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(UserStringHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(UserStringHandle left, UserStringHandle right) { throw null; }

        public static explicit operator UserStringHandle(Handle handle) { throw null; }

        public static implicit operator Handle(UserStringHandle handle) { throw null; }

        public static bool operator !=(UserStringHandle left, UserStringHandle right) { throw null; }
    }
}

namespace System.Reflection.Metadata.Ecma335
{
    public readonly partial struct ArrayShapeEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ArrayShapeEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly void Shape(int rank, Collections.Immutable.ImmutableArray<int> sizes, Collections.Immutable.ImmutableArray<int> lowerBounds) { }
    }

    public readonly partial struct BlobEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public BlobEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly void CustomAttributeSignature(Action<FixedArgumentsEncoder> fixedArguments, Action<CustomAttributeNamedArgumentsEncoder> namedArguments) { }

        public readonly void CustomAttributeSignature(out FixedArgumentsEncoder fixedArguments, out CustomAttributeNamedArgumentsEncoder namedArguments) { throw null; }

        public readonly FieldTypeEncoder Field() { throw null; }

        public readonly SignatureTypeEncoder FieldSignature() { throw null; }

        public readonly LocalVariablesEncoder LocalVariableSignature(int variableCount) { throw null; }

        public readonly MethodSignatureEncoder MethodSignature(SignatureCallingConvention convention = SignatureCallingConvention.Default, int genericParameterCount = 0, bool isInstanceMethod = false) { throw null; }

        public readonly GenericTypeArgumentsEncoder MethodSpecificationSignature(int genericArgumentCount) { throw null; }

        public readonly NamedArgumentsEncoder PermissionSetArguments(int argumentCount) { throw null; }

        public readonly PermissionSetEncoder PermissionSetBlob(int attributeCount) { throw null; }

        public readonly MethodSignatureEncoder PropertySignature(bool isInstanceProperty = false) { throw null; }

        public readonly SignatureTypeEncoder TypeSpecificationSignature() { throw null; }
    }

    public static partial class CodedIndex
    {
        public static int CustomAttributeType(EntityHandle handle) { throw null; }

        public static int HasConstant(EntityHandle handle) { throw null; }

        public static int HasCustomAttribute(EntityHandle handle) { throw null; }

        public static int HasCustomDebugInformation(EntityHandle handle) { throw null; }

        public static int HasDeclSecurity(EntityHandle handle) { throw null; }

        public static int HasFieldMarshal(EntityHandle handle) { throw null; }

        public static int HasSemantics(EntityHandle handle) { throw null; }

        public static int Implementation(EntityHandle handle) { throw null; }

        public static int MemberForwarded(EntityHandle handle) { throw null; }

        public static int MemberRefParent(EntityHandle handle) { throw null; }

        public static int MethodDefOrRef(EntityHandle handle) { throw null; }

        public static int ResolutionScope(EntityHandle handle) { throw null; }

        public static int TypeDefOrRef(EntityHandle handle) { throw null; }

        public static int TypeDefOrRefOrSpec(EntityHandle handle) { throw null; }

        public static int TypeOrMethodDef(EntityHandle handle) { throw null; }
    }

    public sealed partial class ControlFlowBuilder
    {
        public void AddCatchRegion(LabelHandle tryStart, LabelHandle tryEnd, LabelHandle handlerStart, LabelHandle handlerEnd, EntityHandle catchType) { }

        public void AddFaultRegion(LabelHandle tryStart, LabelHandle tryEnd, LabelHandle handlerStart, LabelHandle handlerEnd) { }

        public void AddFilterRegion(LabelHandle tryStart, LabelHandle tryEnd, LabelHandle handlerStart, LabelHandle handlerEnd, LabelHandle filterStart) { }

        public void AddFinallyRegion(LabelHandle tryStart, LabelHandle tryEnd, LabelHandle handlerStart, LabelHandle handlerEnd) { }

        public void Clear() { }
    }

    public readonly partial struct CustomAttributeArrayTypeEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public CustomAttributeArrayTypeEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly CustomAttributeElementTypeEncoder ElementType() { throw null; }

        public readonly void ObjectArray() { }
    }

    public readonly partial struct CustomAttributeElementTypeEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public CustomAttributeElementTypeEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly void Boolean() { }

        public readonly void Byte() { }

        public readonly void Char() { }

        public readonly void Double() { }

        public readonly void Enum(string enumTypeName) { }

        public readonly void Int16() { }

        public readonly void Int32() { }

        public readonly void Int64() { }

        public readonly void PrimitiveType(PrimitiveSerializationTypeCode type) { }

        public readonly void SByte() { }

        public readonly void Single() { }

        public readonly void String() { }

        public readonly void SystemType() { }

        public readonly void UInt16() { }

        public readonly void UInt32() { }

        public readonly void UInt64() { }
    }

    public readonly partial struct CustomAttributeNamedArgumentsEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public CustomAttributeNamedArgumentsEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly NamedArgumentsEncoder Count(int count) { throw null; }
    }

    public readonly partial struct CustomModifiersEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public CustomModifiersEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly CustomModifiersEncoder AddModifier(EntityHandle type, bool isOptional) { throw null; }
    }

    public readonly partial struct EditAndContinueLogEntry : IEquatable<EditAndContinueLogEntry>
    {
        private readonly int _dummyPrimitive;
        public EditAndContinueLogEntry(EntityHandle handle, EditAndContinueOperation operation) { }

        public EntityHandle Handle { get { throw null; } }

        public EditAndContinueOperation Operation { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(EditAndContinueLogEntry other) { throw null; }

        public override readonly int GetHashCode() { throw null; }
    }

    public enum EditAndContinueOperation
    {
        Default = 0,
        AddMethod = 1,
        AddField = 2,
        AddParameter = 3,
        AddProperty = 4,
        AddEvent = 5
    }

    public readonly partial struct ExceptionRegionEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public BlobBuilder Builder { get { throw null; } }

        public bool HasSmallFormat { get { throw null; } }

        public readonly ExceptionRegionEncoder Add(ExceptionRegionKind kind, int tryOffset, int tryLength, int handlerOffset, int handlerLength, EntityHandle catchType = default, int filterOffset = 0) { throw null; }

        public readonly ExceptionRegionEncoder AddCatch(int tryOffset, int tryLength, int handlerOffset, int handlerLength, EntityHandle catchType) { throw null; }

        public readonly ExceptionRegionEncoder AddFault(int tryOffset, int tryLength, int handlerOffset, int handlerLength) { throw null; }

        public readonly ExceptionRegionEncoder AddFilter(int tryOffset, int tryLength, int handlerOffset, int handlerLength, int filterOffset) { throw null; }

        public readonly ExceptionRegionEncoder AddFinally(int tryOffset, int tryLength, int handlerOffset, int handlerLength) { throw null; }

        public static bool IsSmallExceptionRegion(int startOffset, int length) { throw null; }

        public static bool IsSmallRegionCount(int exceptionRegionCount) { throw null; }
    }

    public static partial class ExportedTypeExtensions
    {
        public static int GetTypeDefinitionId(this ExportedType exportedType) { throw null; }
    }

    public readonly partial struct FieldTypeEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public FieldTypeEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly CustomModifiersEncoder CustomModifiers() { throw null; }

        public readonly SignatureTypeEncoder Type(bool isByRef = false) { throw null; }

        public readonly void TypedReference() { }
    }

    public readonly partial struct FixedArgumentsEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public FixedArgumentsEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly LiteralEncoder AddArgument() { throw null; }
    }

    public enum FunctionPointerAttributes
    {
        None = 0,
        HasThis = 32,
        HasExplicitThis = 96
    }

    public readonly partial struct GenericTypeArgumentsEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public GenericTypeArgumentsEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly SignatureTypeEncoder AddArgument() { throw null; }
    }

    public enum HeapIndex
    {
        UserString = 0,
        String = 1,
        Blob = 2,
        Guid = 3
    }

    public readonly partial struct InstructionEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public InstructionEncoder(BlobBuilder codeBuilder, ControlFlowBuilder? controlFlowBuilder = null) { }

        public BlobBuilder CodeBuilder { get { throw null; } }

        public ControlFlowBuilder? ControlFlowBuilder { get { throw null; } }

        public int Offset { get { throw null; } }

        public readonly void Branch(ILOpCode code, LabelHandle label) { }

        public readonly void Call(EntityHandle methodHandle) { }

        public readonly void Call(MemberReferenceHandle methodHandle) { }

        public readonly void Call(MethodDefinitionHandle methodHandle) { }

        public readonly void Call(MethodSpecificationHandle methodHandle) { }

        public readonly void CallIndirect(StandaloneSignatureHandle signature) { }

        public readonly LabelHandle DefineLabel() { throw null; }

        public readonly void LoadArgument(int argumentIndex) { }

        public readonly void LoadArgumentAddress(int argumentIndex) { }

        public readonly void LoadConstantI4(int value) { }

        public readonly void LoadConstantI8(long value) { }

        public readonly void LoadConstantR4(float value) { }

        public readonly void LoadConstantR8(double value) { }

        public readonly void LoadLocal(int slotIndex) { }

        public readonly void LoadLocalAddress(int slotIndex) { }

        public readonly void LoadString(UserStringHandle handle) { }

        public readonly void MarkLabel(LabelHandle label) { }

        public readonly void OpCode(ILOpCode code) { }

        public readonly void StoreArgument(int argumentIndex) { }

        public readonly void StoreLocal(int slotIndex) { }

        public readonly void Token(int token) { }

        public readonly void Token(EntityHandle handle) { }
    }

    public readonly partial struct LabelHandle : IEquatable<LabelHandle>
    {
        private readonly int _dummyPrimitive;
        public int Id { get { throw null; } }

        public bool IsNil { get { throw null; } }

        public override readonly bool Equals(object? obj) { throw null; }

        public readonly bool Equals(LabelHandle other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(LabelHandle left, LabelHandle right) { throw null; }

        public static bool operator !=(LabelHandle left, LabelHandle right) { throw null; }
    }

    public readonly partial struct LiteralEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public LiteralEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly ScalarEncoder Scalar() { throw null; }

        public readonly void TaggedScalar(Action<CustomAttributeElementTypeEncoder> type, Action<ScalarEncoder> scalar) { }

        public readonly void TaggedScalar(out CustomAttributeElementTypeEncoder type, out ScalarEncoder scalar) { throw null; }

        public readonly void TaggedVector(Action<CustomAttributeArrayTypeEncoder> arrayType, Action<VectorEncoder> vector) { }

        public readonly void TaggedVector(out CustomAttributeArrayTypeEncoder arrayType, out VectorEncoder vector) { throw null; }

        public readonly VectorEncoder Vector() { throw null; }
    }

    public readonly partial struct LiteralsEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public LiteralsEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly LiteralEncoder AddLiteral() { throw null; }
    }

    public readonly partial struct LocalVariablesEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public LocalVariablesEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly LocalVariableTypeEncoder AddVariable() { throw null; }
    }

    public readonly partial struct LocalVariableTypeEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public LocalVariableTypeEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly CustomModifiersEncoder CustomModifiers() { throw null; }

        public readonly SignatureTypeEncoder Type(bool isByRef = false, bool isPinned = false) { throw null; }

        public readonly void TypedReference() { }
    }

    public sealed partial class MetadataAggregator
    {
        public MetadataAggregator(Collections.Generic.IReadOnlyList<int>? baseTableRowCounts, Collections.Generic.IReadOnlyList<int>? baseHeapSizes, Collections.Generic.IReadOnlyList<MetadataReader>? deltaReaders) { }

        public MetadataAggregator(MetadataReader baseReader, Collections.Generic.IReadOnlyList<MetadataReader> deltaReaders) { }

        public Handle GetGenerationHandle(Handle handle, out int generation) { throw null; }
    }

    public sealed partial class MetadataBuilder
    {
        public MetadataBuilder(int userStringHeapStartOffset = 0, int stringHeapStartOffset = 0, int blobHeapStartOffset = 0, int guidHeapStartOffset = 0) { }

        public AssemblyDefinitionHandle AddAssembly(StringHandle name, Version version, StringHandle culture, BlobHandle publicKey, AssemblyFlags flags, AssemblyHashAlgorithm hashAlgorithm) { throw null; }

        public AssemblyFileHandle AddAssemblyFile(StringHandle name, BlobHandle hashValue, bool containsMetadata) { throw null; }

        public AssemblyReferenceHandle AddAssemblyReference(StringHandle name, Version version, StringHandle culture, BlobHandle publicKeyOrToken, AssemblyFlags flags, BlobHandle hashValue) { throw null; }

        public ConstantHandle AddConstant(EntityHandle parent, object? value) { throw null; }

        public CustomAttributeHandle AddCustomAttribute(EntityHandle parent, EntityHandle constructor, BlobHandle value) { throw null; }

        public CustomDebugInformationHandle AddCustomDebugInformation(EntityHandle parent, GuidHandle kind, BlobHandle value) { throw null; }

        public DeclarativeSecurityAttributeHandle AddDeclarativeSecurityAttribute(EntityHandle parent, DeclarativeSecurityAction action, BlobHandle permissionSet) { throw null; }

        public DocumentHandle AddDocument(BlobHandle name, GuidHandle hashAlgorithm, BlobHandle hash, GuidHandle language) { throw null; }

        public void AddEncLogEntry(EntityHandle entity, EditAndContinueOperation code) { }

        public void AddEncMapEntry(EntityHandle entity) { }

        public EventDefinitionHandle AddEvent(EventAttributes attributes, StringHandle name, EntityHandle type) { throw null; }

        public void AddEventMap(TypeDefinitionHandle declaringType, EventDefinitionHandle eventList) { }

        public ExportedTypeHandle AddExportedType(TypeAttributes attributes, StringHandle @namespace, StringHandle name, EntityHandle implementation, int typeDefinitionId) { throw null; }

        public FieldDefinitionHandle AddFieldDefinition(FieldAttributes attributes, StringHandle name, BlobHandle signature) { throw null; }

        public void AddFieldLayout(FieldDefinitionHandle field, int offset) { }

        public void AddFieldRelativeVirtualAddress(FieldDefinitionHandle field, int offset) { }

        public GenericParameterHandle AddGenericParameter(EntityHandle parent, GenericParameterAttributes attributes, StringHandle name, int index) { throw null; }

        public GenericParameterConstraintHandle AddGenericParameterConstraint(GenericParameterHandle genericParameter, EntityHandle constraint) { throw null; }

        public ImportScopeHandle AddImportScope(ImportScopeHandle parentScope, BlobHandle imports) { throw null; }

        public InterfaceImplementationHandle AddInterfaceImplementation(TypeDefinitionHandle type, EntityHandle implementedInterface) { throw null; }

        public LocalConstantHandle AddLocalConstant(StringHandle name, BlobHandle signature) { throw null; }

        public LocalScopeHandle AddLocalScope(MethodDefinitionHandle method, ImportScopeHandle importScope, LocalVariableHandle variableList, LocalConstantHandle constantList, int startOffset, int length) { throw null; }

        public LocalVariableHandle AddLocalVariable(LocalVariableAttributes attributes, int index, StringHandle name) { throw null; }

        public ManifestResourceHandle AddManifestResource(ManifestResourceAttributes attributes, StringHandle name, EntityHandle implementation, uint offset) { throw null; }

        public void AddMarshallingDescriptor(EntityHandle parent, BlobHandle descriptor) { }

        public MemberReferenceHandle AddMemberReference(EntityHandle parent, StringHandle name, BlobHandle signature) { throw null; }

        public MethodDebugInformationHandle AddMethodDebugInformation(DocumentHandle document, BlobHandle sequencePoints) { throw null; }

        public MethodDefinitionHandle AddMethodDefinition(MethodAttributes attributes, MethodImplAttributes implAttributes, StringHandle name, BlobHandle signature, int bodyOffset, ParameterHandle parameterList) { throw null; }

        public MethodImplementationHandle AddMethodImplementation(TypeDefinitionHandle type, EntityHandle methodBody, EntityHandle methodDeclaration) { throw null; }

        public void AddMethodImport(MethodDefinitionHandle method, MethodImportAttributes attributes, StringHandle name, ModuleReferenceHandle module) { }

        public void AddMethodSemantics(EntityHandle association, MethodSemanticsAttributes semantics, MethodDefinitionHandle methodDefinition) { }

        public MethodSpecificationHandle AddMethodSpecification(EntityHandle method, BlobHandle instantiation) { throw null; }

        public ModuleDefinitionHandle AddModule(int generation, StringHandle moduleName, GuidHandle mvid, GuidHandle encId, GuidHandle encBaseId) { throw null; }

        public ModuleReferenceHandle AddModuleReference(StringHandle moduleName) { throw null; }

        public void AddNestedType(TypeDefinitionHandle type, TypeDefinitionHandle enclosingType) { }

        public ParameterHandle AddParameter(ParameterAttributes attributes, StringHandle name, int sequenceNumber) { throw null; }

        public PropertyDefinitionHandle AddProperty(PropertyAttributes attributes, StringHandle name, BlobHandle signature) { throw null; }

        public void AddPropertyMap(TypeDefinitionHandle declaringType, PropertyDefinitionHandle propertyList) { }

        public StandaloneSignatureHandle AddStandaloneSignature(BlobHandle signature) { throw null; }

        public void AddStateMachineMethod(MethodDefinitionHandle moveNextMethod, MethodDefinitionHandle kickoffMethod) { }

        public TypeDefinitionHandle AddTypeDefinition(TypeAttributes attributes, StringHandle @namespace, StringHandle name, EntityHandle baseType, FieldDefinitionHandle fieldList, MethodDefinitionHandle methodList) { throw null; }

        public void AddTypeLayout(TypeDefinitionHandle type, ushort packingSize, uint size) { }

        public TypeReferenceHandle AddTypeReference(EntityHandle resolutionScope, StringHandle @namespace, StringHandle name) { throw null; }

        public TypeSpecificationHandle AddTypeSpecification(BlobHandle signature) { throw null; }

        public BlobHandle GetOrAddBlob(byte[] value) { throw null; }

        public BlobHandle GetOrAddBlob(Collections.Immutable.ImmutableArray<byte> value) { throw null; }

        public BlobHandle GetOrAddBlob(BlobBuilder value) { throw null; }

        public BlobHandle GetOrAddBlobUTF16(string value) { throw null; }

        public BlobHandle GetOrAddBlobUTF8(string value, bool allowUnpairedSurrogates = true) { throw null; }

        public BlobHandle GetOrAddConstantBlob(object? value) { throw null; }

        public BlobHandle GetOrAddDocumentName(string value) { throw null; }

        public GuidHandle GetOrAddGuid(Guid guid) { throw null; }

        public StringHandle GetOrAddString(string value) { throw null; }

        public UserStringHandle GetOrAddUserString(string value) { throw null; }

        public int GetRowCount(TableIndex table) { throw null; }

        public Collections.Immutable.ImmutableArray<int> GetRowCounts() { throw null; }

        public ReservedBlob<GuidHandle> ReserveGuid() { throw null; }

        public ReservedBlob<UserStringHandle> ReserveUserString(int length) { throw null; }

        public void SetCapacity(HeapIndex heap, int byteCount) { }

        public void SetCapacity(TableIndex table, int rowCount) { }
    }

    public static partial class MetadataReaderExtensions
    {
        public static Collections.Generic.IEnumerable<EditAndContinueLogEntry> GetEditAndContinueLogEntries(this MetadataReader reader) { throw null; }

        public static Collections.Generic.IEnumerable<EntityHandle> GetEditAndContinueMapEntries(this MetadataReader reader) { throw null; }

        public static int GetHeapMetadataOffset(this MetadataReader reader, HeapIndex heapIndex) { throw null; }

        public static int GetHeapSize(this MetadataReader reader, HeapIndex heapIndex) { throw null; }

        public static BlobHandle GetNextHandle(this MetadataReader reader, BlobHandle handle) { throw null; }

        public static StringHandle GetNextHandle(this MetadataReader reader, StringHandle handle) { throw null; }

        public static UserStringHandle GetNextHandle(this MetadataReader reader, UserStringHandle handle) { throw null; }

        public static int GetTableMetadataOffset(this MetadataReader reader, TableIndex tableIndex) { throw null; }

        public static int GetTableRowCount(this MetadataReader reader, TableIndex tableIndex) { throw null; }

        public static int GetTableRowSize(this MetadataReader reader, TableIndex tableIndex) { throw null; }

        public static Collections.Generic.IEnumerable<TypeDefinitionHandle> GetTypesWithEvents(this MetadataReader reader) { throw null; }

        public static Collections.Generic.IEnumerable<TypeDefinitionHandle> GetTypesWithProperties(this MetadataReader reader) { throw null; }

        public static SignatureTypeKind ResolveSignatureTypeKind(this MetadataReader reader, EntityHandle typeHandle, byte rawTypeKind) { throw null; }
    }

    public sealed partial class MetadataRootBuilder
    {
        public MetadataRootBuilder(MetadataBuilder tablesAndHeaps, string? metadataVersion = null, bool suppressValidation = false) { }

        public string MetadataVersion { get { throw null; } }

        public MetadataSizes Sizes { get { throw null; } }

        public bool SuppressValidation { get { throw null; } }

        public void Serialize(BlobBuilder builder, int methodBodyStreamRva, int mappedFieldDataStreamRva) { }
    }

    public sealed partial class MetadataSizes
    {
        internal MetadataSizes() { }

        public Collections.Immutable.ImmutableArray<int> ExternalRowCounts { get { throw null; } }

        public Collections.Immutable.ImmutableArray<int> HeapSizes { get { throw null; } }

        public Collections.Immutable.ImmutableArray<int> RowCounts { get { throw null; } }

        public int GetAlignedHeapSize(HeapIndex index) { throw null; }
    }

    public static partial class MetadataTokens
    {
        public static readonly int HeapCount;
        public static readonly int TableCount;
        public static AssemblyFileHandle AssemblyFileHandle(int rowNumber) { throw null; }

        public static AssemblyReferenceHandle AssemblyReferenceHandle(int rowNumber) { throw null; }

        public static BlobHandle BlobHandle(int offset) { throw null; }

        public static ConstantHandle ConstantHandle(int rowNumber) { throw null; }

        public static CustomAttributeHandle CustomAttributeHandle(int rowNumber) { throw null; }

        public static CustomDebugInformationHandle CustomDebugInformationHandle(int rowNumber) { throw null; }

        public static DeclarativeSecurityAttributeHandle DeclarativeSecurityAttributeHandle(int rowNumber) { throw null; }

        public static DocumentHandle DocumentHandle(int rowNumber) { throw null; }

        public static DocumentNameBlobHandle DocumentNameBlobHandle(int offset) { throw null; }

        public static EntityHandle EntityHandle(int token) { throw null; }

        public static EntityHandle EntityHandle(TableIndex tableIndex, int rowNumber) { throw null; }

        public static EventDefinitionHandle EventDefinitionHandle(int rowNumber) { throw null; }

        public static ExportedTypeHandle ExportedTypeHandle(int rowNumber) { throw null; }

        public static FieldDefinitionHandle FieldDefinitionHandle(int rowNumber) { throw null; }

        public static GenericParameterConstraintHandle GenericParameterConstraintHandle(int rowNumber) { throw null; }

        public static GenericParameterHandle GenericParameterHandle(int rowNumber) { throw null; }

        public static int GetHeapOffset(BlobHandle handle) { throw null; }

        public static int GetHeapOffset(GuidHandle handle) { throw null; }

        public static int GetHeapOffset(Handle handle) { throw null; }

        public static int GetHeapOffset(this MetadataReader reader, Handle handle) { throw null; }

        public static int GetHeapOffset(StringHandle handle) { throw null; }

        public static int GetHeapOffset(UserStringHandle handle) { throw null; }

        public static int GetRowNumber(EntityHandle handle) { throw null; }

        public static int GetRowNumber(this MetadataReader reader, EntityHandle handle) { throw null; }

        public static int GetToken(EntityHandle handle) { throw null; }

        public static int GetToken(Handle handle) { throw null; }

        public static int GetToken(this MetadataReader reader, EntityHandle handle) { throw null; }

        public static int GetToken(this MetadataReader reader, Handle handle) { throw null; }

        public static GuidHandle GuidHandle(int offset) { throw null; }

        public static Handle Handle(int token) { throw null; }

        public static EntityHandle Handle(TableIndex tableIndex, int rowNumber) { throw null; }

        public static ImportScopeHandle ImportScopeHandle(int rowNumber) { throw null; }

        public static InterfaceImplementationHandle InterfaceImplementationHandle(int rowNumber) { throw null; }

        public static LocalConstantHandle LocalConstantHandle(int rowNumber) { throw null; }

        public static LocalScopeHandle LocalScopeHandle(int rowNumber) { throw null; }

        public static LocalVariableHandle LocalVariableHandle(int rowNumber) { throw null; }

        public static ManifestResourceHandle ManifestResourceHandle(int rowNumber) { throw null; }

        public static MemberReferenceHandle MemberReferenceHandle(int rowNumber) { throw null; }

        public static MethodDebugInformationHandle MethodDebugInformationHandle(int rowNumber) { throw null; }

        public static MethodDefinitionHandle MethodDefinitionHandle(int rowNumber) { throw null; }

        public static MethodImplementationHandle MethodImplementationHandle(int rowNumber) { throw null; }

        public static MethodSpecificationHandle MethodSpecificationHandle(int rowNumber) { throw null; }

        public static ModuleReferenceHandle ModuleReferenceHandle(int rowNumber) { throw null; }

        public static ParameterHandle ParameterHandle(int rowNumber) { throw null; }

        public static PropertyDefinitionHandle PropertyDefinitionHandle(int rowNumber) { throw null; }

        public static StandaloneSignatureHandle StandaloneSignatureHandle(int rowNumber) { throw null; }

        public static StringHandle StringHandle(int offset) { throw null; }

        public static bool TryGetHeapIndex(HandleKind type, out HeapIndex index) { throw null; }

        public static bool TryGetTableIndex(HandleKind type, out TableIndex index) { throw null; }

        public static TypeDefinitionHandle TypeDefinitionHandle(int rowNumber) { throw null; }

        public static TypeReferenceHandle TypeReferenceHandle(int rowNumber) { throw null; }

        public static TypeSpecificationHandle TypeSpecificationHandle(int rowNumber) { throw null; }

        public static UserStringHandle UserStringHandle(int offset) { throw null; }
    }

    [Flags]
    public enum MethodBodyAttributes
    {
        None = 0,
        InitLocals = 1
    }

    public readonly partial struct MethodBodyStreamEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public MethodBodyStreamEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly MethodBody AddMethodBody(int codeSize, int maxStack = 8, int exceptionRegionCount = 0, bool hasSmallExceptionRegions = true, StandaloneSignatureHandle localVariablesSignature = default, MethodBodyAttributes attributes = MethodBodyAttributes.InitLocals, bool hasDynamicStackAllocation = false) { throw null; }

        public readonly MethodBody AddMethodBody(int codeSize, int maxStack, int exceptionRegionCount, bool hasSmallExceptionRegions, StandaloneSignatureHandle localVariablesSignature, MethodBodyAttributes attributes) { throw null; }

        public readonly int AddMethodBody(InstructionEncoder instructionEncoder, int maxStack = 8, StandaloneSignatureHandle localVariablesSignature = default, MethodBodyAttributes attributes = MethodBodyAttributes.InitLocals, bool hasDynamicStackAllocation = false) { throw null; }

        public readonly int AddMethodBody(InstructionEncoder instructionEncoder, int maxStack, StandaloneSignatureHandle localVariablesSignature, MethodBodyAttributes attributes) { throw null; }

        public readonly partial struct MethodBody
        {
            private readonly int _dummyPrimitive;
            public ExceptionRegionEncoder ExceptionRegions { get { throw null; } }

            public Blob Instructions { get { throw null; } }

            public int Offset { get { throw null; } }
        }
    }

    public readonly partial struct MethodSignatureEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public MethodSignatureEncoder(BlobBuilder builder, bool hasVarArgs) { }

        public BlobBuilder Builder { get { throw null; } }

        public bool HasVarArgs { get { throw null; } }

        public readonly void Parameters(int parameterCount, Action<ReturnTypeEncoder> returnType, Action<ParametersEncoder> parameters) { }

        public readonly void Parameters(int parameterCount, out ReturnTypeEncoder returnType, out ParametersEncoder parameters) { throw null; }
    }

    public readonly partial struct NamedArgumentsEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public NamedArgumentsEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly void AddArgument(bool isField, Action<NamedArgumentTypeEncoder> type, Action<NameEncoder> name, Action<LiteralEncoder> literal) { }

        public readonly void AddArgument(bool isField, out NamedArgumentTypeEncoder type, out NameEncoder name, out LiteralEncoder literal) { throw null; }
    }

    public readonly partial struct NamedArgumentTypeEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public NamedArgumentTypeEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly void Object() { }

        public readonly CustomAttributeElementTypeEncoder ScalarType() { throw null; }

        public readonly CustomAttributeArrayTypeEncoder SZArray() { throw null; }
    }

    public readonly partial struct NameEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public NameEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly void Name(string name) { }
    }

    public readonly partial struct ParametersEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ParametersEncoder(BlobBuilder builder, bool hasVarArgs = false) { }

        public BlobBuilder Builder { get { throw null; } }

        public bool HasVarArgs { get { throw null; } }

        public readonly ParameterTypeEncoder AddParameter() { throw null; }

        public readonly ParametersEncoder StartVarArgs() { throw null; }
    }

    public readonly partial struct ParameterTypeEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ParameterTypeEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly CustomModifiersEncoder CustomModifiers() { throw null; }

        public readonly SignatureTypeEncoder Type(bool isByRef = false) { throw null; }

        public readonly void TypedReference() { }
    }

    public readonly partial struct PermissionSetEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public PermissionSetEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly PermissionSetEncoder AddPermission(string typeName, Collections.Immutable.ImmutableArray<byte> encodedArguments) { throw null; }

        public readonly PermissionSetEncoder AddPermission(string typeName, BlobBuilder encodedArguments) { throw null; }
    }

    public sealed partial class PortablePdbBuilder
    {
        public PortablePdbBuilder(MetadataBuilder tablesAndHeaps, Collections.Immutable.ImmutableArray<int> typeSystemRowCounts, MethodDefinitionHandle entryPoint, Func<Collections.Generic.IEnumerable<Blob>, BlobContentId>? idProvider = null) { }

        public ushort FormatVersion { get { throw null; } }

        public Func<Collections.Generic.IEnumerable<Blob>, BlobContentId> IdProvider { get { throw null; } }

        public string MetadataVersion { get { throw null; } }

        public BlobContentId Serialize(BlobBuilder builder) { throw null; }
    }

    public readonly partial struct ReturnTypeEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ReturnTypeEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly CustomModifiersEncoder CustomModifiers() { throw null; }

        public readonly SignatureTypeEncoder Type(bool isByRef = false) { throw null; }

        public readonly void TypedReference() { }

        public readonly void Void() { }
    }

    public readonly partial struct ScalarEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ScalarEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly void Constant(object? value) { }

        public readonly void NullArray() { }

        public readonly void SystemType(string? serializedTypeName) { }
    }

    public readonly partial struct SignatureDecoder<TType, TGenericContext>
    {
        private readonly ISignatureTypeProvider<TType, TGenericContext> _provider;
        private readonly TGenericContext _genericContext;
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public SignatureDecoder(ISignatureTypeProvider<TType, TGenericContext> provider, MetadataReader metadataReader, TGenericContext genericContext) { }

        public readonly TType DecodeFieldSignature(ref BlobReader blobReader) { throw null; }

        public readonly Collections.Immutable.ImmutableArray<TType> DecodeLocalSignature(ref BlobReader blobReader) { throw null; }

        public readonly MethodSignature<TType> DecodeMethodSignature(ref BlobReader blobReader) { throw null; }

        public readonly Collections.Immutable.ImmutableArray<TType> DecodeMethodSpecificationSignature(ref BlobReader blobReader) { throw null; }

        public readonly TType DecodeType(ref BlobReader blobReader, bool allowTypeSpecifications = false) { throw null; }
    }

    public readonly partial struct SignatureTypeEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public SignatureTypeEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly void Array(Action<SignatureTypeEncoder> elementType, Action<ArrayShapeEncoder> arrayShape) { }

        public readonly void Array(out SignatureTypeEncoder elementType, out ArrayShapeEncoder arrayShape) { throw null; }

        public readonly void Boolean() { }

        public readonly void Byte() { }

        public readonly void Char() { }

        public readonly CustomModifiersEncoder CustomModifiers() { throw null; }

        public readonly void Double() { }

        public readonly MethodSignatureEncoder FunctionPointer(SignatureCallingConvention convention = SignatureCallingConvention.Default, FunctionPointerAttributes attributes = FunctionPointerAttributes.None, int genericParameterCount = 0) { throw null; }

        public readonly GenericTypeArgumentsEncoder GenericInstantiation(EntityHandle genericType, int genericArgumentCount, bool isValueType) { throw null; }

        public readonly void GenericMethodTypeParameter(int parameterIndex) { }

        public readonly void GenericTypeParameter(int parameterIndex) { }

        public readonly void Int16() { }

        public readonly void Int32() { }

        public readonly void Int64() { }

        public readonly void IntPtr() { }

        public readonly void Object() { }

        public readonly SignatureTypeEncoder Pointer() { throw null; }

        public readonly void PrimitiveType(PrimitiveTypeCode type) { }

        public readonly void SByte() { }

        public readonly void Single() { }

        public readonly void String() { }

        public readonly SignatureTypeEncoder SZArray() { throw null; }

        public readonly void Type(EntityHandle type, bool isValueType) { }

        public readonly void UInt16() { }

        public readonly void UInt32() { }

        public readonly void UInt64() { }

        public readonly void UIntPtr() { }

        public readonly void VoidPointer() { }
    }

    public enum TableIndex : byte
    {
        Module = 0,
        TypeRef = 1,
        TypeDef = 2,
        FieldPtr = 3,
        Field = 4,
        MethodPtr = 5,
        MethodDef = 6,
        ParamPtr = 7,
        Param = 8,
        InterfaceImpl = 9,
        MemberRef = 10,
        Constant = 11,
        CustomAttribute = 12,
        FieldMarshal = 13,
        DeclSecurity = 14,
        ClassLayout = 15,
        FieldLayout = 16,
        StandAloneSig = 17,
        EventMap = 18,
        EventPtr = 19,
        Event = 20,
        PropertyMap = 21,
        PropertyPtr = 22,
        Property = 23,
        MethodSemantics = 24,
        MethodImpl = 25,
        ModuleRef = 26,
        TypeSpec = 27,
        ImplMap = 28,
        FieldRva = 29,
        EncLog = 30,
        EncMap = 31,
        Assembly = 32,
        AssemblyProcessor = 33,
        AssemblyOS = 34,
        AssemblyRef = 35,
        AssemblyRefProcessor = 36,
        AssemblyRefOS = 37,
        File = 38,
        ExportedType = 39,
        ManifestResource = 40,
        NestedClass = 41,
        GenericParam = 42,
        MethodSpec = 43,
        GenericParamConstraint = 44,
        Document = 48,
        MethodDebugInformation = 49,
        LocalScope = 50,
        LocalVariable = 51,
        LocalConstant = 52,
        ImportScope = 53,
        StateMachineMethod = 54,
        CustomDebugInformation = 55
    }

    public readonly partial struct VectorEncoder
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public VectorEncoder(BlobBuilder builder) { }

        public BlobBuilder Builder { get { throw null; } }

        public readonly LiteralsEncoder Count(int count) { throw null; }
    }
}

namespace System.Reflection.PortableExecutable
{
    [Flags]
    public enum Characteristics : ushort
    {
        RelocsStripped = 1,
        ExecutableImage = 2,
        LineNumsStripped = 4,
        LocalSymsStripped = 8,
        AggressiveWSTrim = 16,
        LargeAddressAware = 32,
        BytesReversedLo = 128,
        Bit32Machine = 256,
        DebugStripped = 512,
        RemovableRunFromSwap = 1024,
        NetRunFromSwap = 2048,
        System = 4096,
        Dll = 8192,
        UpSystemOnly = 16384,
        BytesReversedHi = 32768
    }

    public readonly partial struct CodeViewDebugDirectoryData
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Age { get { throw null; } }

        public Guid Guid { get { throw null; } }

        public string Path { get { throw null; } }
    }

    public sealed partial class CoffHeader
    {
        internal CoffHeader() { }

        public Characteristics Characteristics { get { throw null; } }

        public Machine Machine { get { throw null; } }

        public short NumberOfSections { get { throw null; } }

        public int NumberOfSymbols { get { throw null; } }

        public int PointerToSymbolTable { get { throw null; } }

        public short SizeOfOptionalHeader { get { throw null; } }

        public int TimeDateStamp { get { throw null; } }
    }

    [Flags]
    public enum CorFlags
    {
        ILOnly = 1,
        Requires32Bit = 2,
        ILLibrary = 4,
        StrongNameSigned = 8,
        NativeEntryPoint = 16,
        TrackDebugData = 65536,
        Prefers32Bit = 131072
    }

    public sealed partial class CorHeader
    {
        internal CorHeader() { }

        public DirectoryEntry CodeManagerTableDirectory { get { throw null; } }

        public int EntryPointTokenOrRelativeVirtualAddress { get { throw null; } }

        public DirectoryEntry ExportAddressTableJumpsDirectory { get { throw null; } }

        public CorFlags Flags { get { throw null; } }

        public ushort MajorRuntimeVersion { get { throw null; } }

        public DirectoryEntry ManagedNativeHeaderDirectory { get { throw null; } }

        public DirectoryEntry MetadataDirectory { get { throw null; } }

        public ushort MinorRuntimeVersion { get { throw null; } }

        public DirectoryEntry ResourcesDirectory { get { throw null; } }

        public DirectoryEntry StrongNameSignatureDirectory { get { throw null; } }

        public DirectoryEntry VtableFixupsDirectory { get { throw null; } }
    }

    public sealed partial class DebugDirectoryBuilder
    {
        public void AddCodeViewEntry(string pdbPath, Metadata.BlobContentId pdbContentId, ushort portablePdbVersion, int age) { }

        public void AddCodeViewEntry(string pdbPath, Metadata.BlobContentId pdbContentId, ushort portablePdbVersion) { }

        public void AddEmbeddedPortablePdbEntry(Metadata.BlobBuilder debugMetadata, ushort portablePdbVersion) { }

        public void AddEntry(DebugDirectoryEntryType type, uint version, uint stamp) { }

        public void AddEntry<TData>(DebugDirectoryEntryType type, uint version, uint stamp, TData data, Action<Metadata.BlobBuilder, TData> dataSerializer) { }

        public void AddPdbChecksumEntry(string algorithmName, Collections.Immutable.ImmutableArray<byte> checksum) { }

        public void AddReproducibleEntry() { }
    }

    public readonly partial struct DebugDirectoryEntry
    {
        private readonly int _dummyPrimitive;
        public DebugDirectoryEntry(uint stamp, ushort majorVersion, ushort minorVersion, DebugDirectoryEntryType type, int dataSize, int dataRelativeVirtualAddress, int dataPointer) { }

        public int DataPointer { get { throw null; } }

        public int DataRelativeVirtualAddress { get { throw null; } }

        public int DataSize { get { throw null; } }

        public bool IsPortableCodeView { get { throw null; } }

        public ushort MajorVersion { get { throw null; } }

        public ushort MinorVersion { get { throw null; } }

        public uint Stamp { get { throw null; } }

        public DebugDirectoryEntryType Type { get { throw null; } }
    }

    public enum DebugDirectoryEntryType
    {
        Unknown = 0,
        Coff = 1,
        CodeView = 2,
        Reproducible = 16,
        EmbeddedPortablePdb = 17,
        PdbChecksum = 19
    }

    public readonly partial struct DirectoryEntry
    {
        public readonly int RelativeVirtualAddress;
        public readonly int Size;
        public DirectoryEntry(int relativeVirtualAddress, int size) { }
    }

    [Flags]
    public enum DllCharacteristics : ushort
    {
        ProcessInit = 1,
        ProcessTerm = 2,
        ThreadInit = 4,
        ThreadTerm = 8,
        HighEntropyVirtualAddressSpace = 32,
        DynamicBase = 64,
        NxCompatible = 256,
        NoIsolation = 512,
        NoSeh = 1024,
        NoBind = 2048,
        AppContainer = 4096,
        WdmDriver = 8192,
        TerminalServerAware = 32768
    }

    public enum Machine : ushort
    {
        Unknown = 0,
        I386 = 332,
        WceMipsV2 = 361,
        Alpha = 388,
        SH3 = 418,
        SH3Dsp = 419,
        SH3E = 420,
        SH4 = 422,
        SH5 = 424,
        Arm = 448,
        Thumb = 450,
        ArmThumb2 = 452,
        AM33 = 467,
        PowerPC = 496,
        PowerPCFP = 497,
        IA64 = 512,
        MIPS16 = 614,
        Alpha64 = 644,
        MipsFpu = 870,
        MipsFpu16 = 1126,
        Tricore = 1312,
        Ebc = 3772,
        LoongArch32 = 25138,
        LoongArch64 = 25188,
        Amd64 = 34404,
        M32R = 36929,
        Arm64 = 43620
    }

    public partial class ManagedPEBuilder : PEBuilder
    {
        public const int ManagedResourcesDataAlignment = 8;
        public const int MappedFieldDataAlignment = 8;
        public ManagedPEBuilder(PEHeaderBuilder header, Metadata.Ecma335.MetadataRootBuilder metadataRootBuilder, Metadata.BlobBuilder ilStream, Metadata.BlobBuilder? mappedFieldData = null, Metadata.BlobBuilder? managedResources = null, ResourceSectionBuilder? nativeResources = null, DebugDirectoryBuilder? debugDirectoryBuilder = null, int strongNameSignatureSize = 128, Metadata.MethodDefinitionHandle entryPoint = default, CorFlags flags = CorFlags.ILOnly, Func<Collections.Generic.IEnumerable<Metadata.Blob>, Metadata.BlobContentId>? deterministicIdProvider = null) : base(default!, default) { }

        protected override Collections.Immutable.ImmutableArray<Section> CreateSections() { throw null; }

        protected internal override PEDirectoriesBuilder GetDirectories() { throw null; }

        protected override Metadata.BlobBuilder SerializeSection(string name, SectionLocation location) { throw null; }

        public void Sign(Metadata.BlobBuilder peImage, Func<Collections.Generic.IEnumerable<Metadata.Blob>, byte[]> signatureProvider) { }
    }

    public readonly partial struct PdbChecksumDebugDirectoryData
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public string AlgorithmName { get { throw null; } }

        public Collections.Immutable.ImmutableArray<byte> Checksum { get { throw null; } }
    }

    public abstract partial class PEBuilder
    {
        protected PEBuilder(PEHeaderBuilder header, Func<Collections.Generic.IEnumerable<Metadata.Blob>, Metadata.BlobContentId>? deterministicIdProvider) { }

        public PEHeaderBuilder Header { get { throw null; } }

        public Func<Collections.Generic.IEnumerable<Metadata.Blob>, Metadata.BlobContentId> IdProvider { get { throw null; } }

        public bool IsDeterministic { get { throw null; } }

        protected abstract Collections.Immutable.ImmutableArray<Section> CreateSections();
        protected internal abstract PEDirectoriesBuilder GetDirectories();
        protected Collections.Immutable.ImmutableArray<Section> GetSections() { throw null; }

        public Metadata.BlobContentId Serialize(Metadata.BlobBuilder builder) { throw null; }

        protected abstract Metadata.BlobBuilder SerializeSection(string name, SectionLocation location);
        protected readonly partial struct Section
        {
            public readonly SectionCharacteristics Characteristics;
            public readonly string Name;
            public Section(string name, SectionCharacteristics characteristics) { }
        }
    }

    public sealed partial class PEDirectoriesBuilder
    {
        public int AddressOfEntryPoint { get { throw null; } set { } }

        public DirectoryEntry BaseRelocationTable { get { throw null; } set { } }

        public DirectoryEntry BoundImportTable { get { throw null; } set { } }

        public DirectoryEntry CopyrightTable { get { throw null; } set { } }

        public DirectoryEntry CorHeaderTable { get { throw null; } set { } }

        public DirectoryEntry DebugTable { get { throw null; } set { } }

        public DirectoryEntry DelayImportTable { get { throw null; } set { } }

        public DirectoryEntry ExceptionTable { get { throw null; } set { } }

        public DirectoryEntry ExportTable { get { throw null; } set { } }

        public DirectoryEntry GlobalPointerTable { get { throw null; } set { } }

        public DirectoryEntry ImportAddressTable { get { throw null; } set { } }

        public DirectoryEntry ImportTable { get { throw null; } set { } }

        public DirectoryEntry LoadConfigTable { get { throw null; } set { } }

        public DirectoryEntry ResourceTable { get { throw null; } set { } }

        public DirectoryEntry ThreadLocalStorageTable { get { throw null; } set { } }
    }

    public sealed partial class PEHeader
    {
        internal PEHeader() { }

        public int AddressOfEntryPoint { get { throw null; } }

        public int BaseOfCode { get { throw null; } }

        public int BaseOfData { get { throw null; } }

        public DirectoryEntry BaseRelocationTableDirectory { get { throw null; } }

        public DirectoryEntry BoundImportTableDirectory { get { throw null; } }

        public DirectoryEntry CertificateTableDirectory { get { throw null; } }

        public uint CheckSum { get { throw null; } }

        public DirectoryEntry CopyrightTableDirectory { get { throw null; } }

        public DirectoryEntry CorHeaderTableDirectory { get { throw null; } }

        public DirectoryEntry DebugTableDirectory { get { throw null; } }

        public DirectoryEntry DelayImportTableDirectory { get { throw null; } }

        public DllCharacteristics DllCharacteristics { get { throw null; } }

        public DirectoryEntry ExceptionTableDirectory { get { throw null; } }

        public DirectoryEntry ExportTableDirectory { get { throw null; } }

        public int FileAlignment { get { throw null; } }

        public DirectoryEntry GlobalPointerTableDirectory { get { throw null; } }

        public ulong ImageBase { get { throw null; } }

        public DirectoryEntry ImportAddressTableDirectory { get { throw null; } }

        public DirectoryEntry ImportTableDirectory { get { throw null; } }

        public DirectoryEntry LoadConfigTableDirectory { get { throw null; } }

        public PEMagic Magic { get { throw null; } }

        public ushort MajorImageVersion { get { throw null; } }

        public byte MajorLinkerVersion { get { throw null; } }

        public ushort MajorOperatingSystemVersion { get { throw null; } }

        public ushort MajorSubsystemVersion { get { throw null; } }

        public ushort MinorImageVersion { get { throw null; } }

        public byte MinorLinkerVersion { get { throw null; } }

        public ushort MinorOperatingSystemVersion { get { throw null; } }

        public ushort MinorSubsystemVersion { get { throw null; } }

        public int NumberOfRvaAndSizes { get { throw null; } }

        public DirectoryEntry ResourceTableDirectory { get { throw null; } }

        public int SectionAlignment { get { throw null; } }

        public int SizeOfCode { get { throw null; } }

        public int SizeOfHeaders { get { throw null; } }

        public ulong SizeOfHeapCommit { get { throw null; } }

        public ulong SizeOfHeapReserve { get { throw null; } }

        public int SizeOfImage { get { throw null; } }

        public int SizeOfInitializedData { get { throw null; } }

        public ulong SizeOfStackCommit { get { throw null; } }

        public ulong SizeOfStackReserve { get { throw null; } }

        public int SizeOfUninitializedData { get { throw null; } }

        public Subsystem Subsystem { get { throw null; } }

        public DirectoryEntry ThreadLocalStorageTableDirectory { get { throw null; } }
    }

    public sealed partial class PEHeaderBuilder
    {
        public PEHeaderBuilder(Machine machine = Machine.Unknown, int sectionAlignment = 8192, int fileAlignment = 512, ulong imageBase = 4194304, byte majorLinkerVersion = 48, byte minorLinkerVersion = 0, ushort majorOperatingSystemVersion = 4, ushort minorOperatingSystemVersion = 0, ushort majorImageVersion = 0, ushort minorImageVersion = 0, ushort majorSubsystemVersion = 4, ushort minorSubsystemVersion = 0, Subsystem subsystem = Subsystem.WindowsCui, DllCharacteristics dllCharacteristics = DllCharacteristics.DynamicBase | DllCharacteristics.NxCompatible | DllCharacteristics.NoSeh | DllCharacteristics.TerminalServerAware, Characteristics imageCharacteristics = Characteristics.Dll, ulong sizeOfStackReserve = 1048576, ulong sizeOfStackCommit = 4096, ulong sizeOfHeapReserve = 1048576, ulong sizeOfHeapCommit = 4096) { }

        public DllCharacteristics DllCharacteristics { get { throw null; } }

        public int FileAlignment { get { throw null; } }

        public ulong ImageBase { get { throw null; } }

        public Characteristics ImageCharacteristics { get { throw null; } }

        public Machine Machine { get { throw null; } }

        public ushort MajorImageVersion { get { throw null; } }

        public byte MajorLinkerVersion { get { throw null; } }

        public ushort MajorOperatingSystemVersion { get { throw null; } }

        public ushort MajorSubsystemVersion { get { throw null; } }

        public ushort MinorImageVersion { get { throw null; } }

        public byte MinorLinkerVersion { get { throw null; } }

        public ushort MinorOperatingSystemVersion { get { throw null; } }

        public ushort MinorSubsystemVersion { get { throw null; } }

        public int SectionAlignment { get { throw null; } }

        public ulong SizeOfHeapCommit { get { throw null; } }

        public ulong SizeOfHeapReserve { get { throw null; } }

        public ulong SizeOfStackCommit { get { throw null; } }

        public ulong SizeOfStackReserve { get { throw null; } }

        public Subsystem Subsystem { get { throw null; } }

        public static PEHeaderBuilder CreateExecutableHeader() { throw null; }

        public static PEHeaderBuilder CreateLibraryHeader() { throw null; }
    }

    public sealed partial class PEHeaders
    {
        public PEHeaders(IO.Stream peStream, int size, bool isLoadedImage) { }

        public PEHeaders(IO.Stream peStream, int size) { }

        public PEHeaders(IO.Stream peStream) { }

        public CoffHeader CoffHeader { get { throw null; } }

        public int CoffHeaderStartOffset { get { throw null; } }

        public CorHeader? CorHeader { get { throw null; } }

        public int CorHeaderStartOffset { get { throw null; } }

        public bool IsCoffOnly { get { throw null; } }

        public bool IsConsoleApplication { get { throw null; } }

        public bool IsDll { get { throw null; } }

        public bool IsExe { get { throw null; } }

        public int MetadataSize { get { throw null; } }

        public int MetadataStartOffset { get { throw null; } }

        public PEHeader? PEHeader { get { throw null; } }

        public int PEHeaderStartOffset { get { throw null; } }

        public Collections.Immutable.ImmutableArray<SectionHeader> SectionHeaders { get { throw null; } }

        public int GetContainingSectionIndex(int relativeVirtualAddress) { throw null; }

        public bool TryGetDirectoryOffset(DirectoryEntry directory, out int offset) { throw null; }
    }

    public enum PEMagic : ushort
    {
        PE32 = 267,
        PE32Plus = 523
    }

    public readonly partial struct PEMemoryBlock
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public int Length { get { throw null; } }

        public unsafe byte* Pointer { get { throw null; } }

        public readonly Collections.Immutable.ImmutableArray<byte> GetContent() { throw null; }

        public readonly Collections.Immutable.ImmutableArray<byte> GetContent(int start, int length) { throw null; }

        public readonly Metadata.BlobReader GetReader() { throw null; }

        public readonly Metadata.BlobReader GetReader(int start, int length) { throw null; }
    }

    public sealed partial class PEReader : IDisposable
    {
        public unsafe PEReader(byte* peImage, int size, bool isLoadedImage) { }

        public unsafe PEReader(byte* peImage, int size) { }

        public PEReader(Collections.Immutable.ImmutableArray<byte> peImage) { }

        public PEReader(IO.Stream peStream, PEStreamOptions options, int size) { }

        public PEReader(IO.Stream peStream, PEStreamOptions options) { }

        public PEReader(IO.Stream peStream) { }

        public bool HasMetadata { get { throw null; } }

        public bool IsEntireImageAvailable { get { throw null; } }

        public bool IsLoadedImage { get { throw null; } }

        public PEHeaders PEHeaders { get { throw null; } }

        public void Dispose() { }

        public PEMemoryBlock GetEntireImage() { throw null; }

        public PEMemoryBlock GetMetadata() { throw null; }

        public PEMemoryBlock GetSectionData(int relativeVirtualAddress) { throw null; }

        public PEMemoryBlock GetSectionData(string sectionName) { throw null; }

        public CodeViewDebugDirectoryData ReadCodeViewDebugDirectoryData(DebugDirectoryEntry entry) { throw null; }

        public Collections.Immutable.ImmutableArray<DebugDirectoryEntry> ReadDebugDirectory() { throw null; }

        public Metadata.MetadataReaderProvider ReadEmbeddedPortablePdbDebugDirectoryData(DebugDirectoryEntry entry) { throw null; }

        public PdbChecksumDebugDirectoryData ReadPdbChecksumDebugDirectoryData(DebugDirectoryEntry entry) { throw null; }

        public bool TryOpenAssociatedPortablePdb(string peImagePath, Func<string, IO.Stream?> pdbFileStreamProvider, out Metadata.MetadataReaderProvider? pdbReaderProvider, out string? pdbPath) { throw null; }
    }

    [Flags]
    public enum PEStreamOptions
    {
        Default = 0,
        LeaveOpen = 1,
        PrefetchMetadata = 2,
        PrefetchEntireImage = 4,
        IsLoadedImage = 8
    }

    public abstract partial class ResourceSectionBuilder
    {
        protected internal abstract void Serialize(Metadata.BlobBuilder builder, SectionLocation location);
    }

    [Flags]
    public enum SectionCharacteristics : uint
    {
        MemWrite = 2147483648U,
        TypeReg = 0U,
        TypeDSect = 1U,
        TypeNoLoad = 2U,
        TypeGroup = 4U,
        TypeNoPad = 8U,
        TypeCopy = 16U,
        ContainsCode = 32U,
        ContainsInitializedData = 64U,
        ContainsUninitializedData = 128U,
        LinkerOther = 256U,
        LinkerInfo = 512U,
        TypeOver = 1024U,
        LinkerRemove = 2048U,
        LinkerComdat = 4096U,
        MemProtected = 16384U,
        NoDeferSpecExc = 16384U,
        GPRel = 32768U,
        MemFardata = 32768U,
        MemSysheap = 65536U,
        Mem16Bit = 131072U,
        MemPurgeable = 131072U,
        MemLocked = 262144U,
        MemPreload = 524288U,
        Align1Bytes = 1048576U,
        Align2Bytes = 2097152U,
        Align4Bytes = 3145728U,
        Align8Bytes = 4194304U,
        Align16Bytes = 5242880U,
        Align32Bytes = 6291456U,
        Align64Bytes = 7340032U,
        Align128Bytes = 8388608U,
        Align256Bytes = 9437184U,
        Align512Bytes = 10485760U,
        Align1024Bytes = 11534336U,
        Align2048Bytes = 12582912U,
        Align4096Bytes = 13631488U,
        Align8192Bytes = 14680064U,
        AlignMask = 15728640U,
        LinkerNRelocOvfl = 16777216U,
        MemDiscardable = 33554432U,
        MemNotCached = 67108864U,
        MemNotPaged = 134217728U,
        MemShared = 268435456U,
        MemExecute = 536870912U,
        MemRead = 1073741824U
    }

    public readonly partial struct SectionHeader
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public string Name { get { throw null; } }

        public ushort NumberOfLineNumbers { get { throw null; } }

        public ushort NumberOfRelocations { get { throw null; } }

        public int PointerToLineNumbers { get { throw null; } }

        public int PointerToRawData { get { throw null; } }

        public int PointerToRelocations { get { throw null; } }

        public SectionCharacteristics SectionCharacteristics { get { throw null; } }

        public int SizeOfRawData { get { throw null; } }

        public int VirtualAddress { get { throw null; } }

        public int VirtualSize { get { throw null; } }
    }

    public readonly partial struct SectionLocation
    {
        private readonly int _dummyPrimitive;
        public SectionLocation(int relativeVirtualAddress, int pointerToRawData) { }

        public int PointerToRawData { get { throw null; } }

        public int RelativeVirtualAddress { get { throw null; } }
    }

    public enum Subsystem : ushort
    {
        Unknown = 0,
        Native = 1,
        WindowsGui = 2,
        WindowsCui = 3,
        OS2Cui = 5,
        PosixCui = 7,
        NativeWindows = 8,
        WindowsCEGui = 9,
        EfiApplication = 10,
        EfiBootServiceDriver = 11,
        EfiRuntimeDriver = 12,
        EfiRom = 13,
        Xbox = 14,
        WindowsBootApplication = 16
    }
}