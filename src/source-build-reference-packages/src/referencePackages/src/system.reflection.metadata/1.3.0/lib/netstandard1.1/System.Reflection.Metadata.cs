// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("System.Reflection.Metadata.Tests, PublicKey=002400000480000094000000060200000024000052534131000400000100010015c01ae1f50e8cc09ba9eac9147cf8fd9fce2cfe9f8dce4f7301c4132ca9fb50ce8cbf1df4dc18dd4d210e4345c744ecb3365ed327efdbc52603faa5e21daa11234c8c4a73e51f03bf192544581ebe107adee3a34928e39d04e524a9ce729d5090bfd7dad9d10c722c0def9ccc08ff0a03790e48bcd1f9b6c476063e1966a1c4")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyTitle("System.Reflection.Metadata")]
[assembly: System.Reflection.AssemblyDescription("System.Reflection.Metadata")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection.Metadata")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.3.0.0")]
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
    public partial struct AssemblyDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public StringHandle Culture { get { throw null; } }

        public AssemblyFlags Flags { get { throw null; } }

        public AssemblyHashAlgorithm HashAlgorithm { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public BlobHandle PublicKey { get { throw null; } }

        public Version Version { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes() { throw null; }
    }

    public partial struct AssemblyDefinitionHandle : IEquatable<AssemblyDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(AssemblyDefinitionHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(AssemblyDefinitionHandle left, AssemblyDefinitionHandle right) { throw null; }

        public static explicit operator AssemblyDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator AssemblyDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(AssemblyDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(AssemblyDefinitionHandle handle) { throw null; }

        public static bool operator !=(AssemblyDefinitionHandle left, AssemblyDefinitionHandle right) { throw null; }
    }

    public partial struct AssemblyFile
    {
        private object _dummy;
        private int _dummyPrimitive;
        public bool ContainsMetadata { get { throw null; } }

        public BlobHandle HashValue { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct AssemblyFileHandle : IEquatable<AssemblyFileHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(AssemblyFileHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(AssemblyFileHandle left, AssemblyFileHandle right) { throw null; }

        public static explicit operator AssemblyFileHandle(EntityHandle handle) { throw null; }

        public static explicit operator AssemblyFileHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(AssemblyFileHandle handle) { throw null; }

        public static implicit operator Handle(AssemblyFileHandle handle) { throw null; }

        public static bool operator !=(AssemblyFileHandle left, AssemblyFileHandle right) { throw null; }
    }

    public partial struct AssemblyFileHandleCollection : Collections.Generic.IReadOnlyCollection<AssemblyFileHandle>, Collections.Generic.IEnumerable<AssemblyFileHandle>, Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<AssemblyFileHandle> Collections.Generic.IEnumerable<AssemblyFileHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct AssemblyReference
    {
        private object _dummy;
        private int _dummyPrimitive;
        public StringHandle Culture { get { throw null; } }

        public AssemblyFlags Flags { get { throw null; } }

        public BlobHandle HashValue { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public BlobHandle PublicKeyOrToken { get { throw null; } }

        public Version Version { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct AssemblyReferenceHandle : IEquatable<AssemblyReferenceHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(AssemblyReferenceHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(AssemblyReferenceHandle left, AssemblyReferenceHandle right) { throw null; }

        public static explicit operator AssemblyReferenceHandle(EntityHandle handle) { throw null; }

        public static explicit operator AssemblyReferenceHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(AssemblyReferenceHandle handle) { throw null; }

        public static implicit operator Handle(AssemblyReferenceHandle handle) { throw null; }

        public static bool operator !=(AssemblyReferenceHandle left, AssemblyReferenceHandle right) { throw null; }
    }

    public partial struct AssemblyReferenceHandleCollection : Collections.Generic.IReadOnlyCollection<AssemblyReferenceHandle>, Collections.Generic.IEnumerable<AssemblyReferenceHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<AssemblyReferenceHandle> Collections.Generic.IEnumerable<AssemblyReferenceHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct BlobHandle : IEquatable<BlobHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(BlobHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

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

        public int Length { get { throw null; } }

        public int Offset { get { throw null; } }

        public int RemainingBytes { get { throw null; } }

        public BlobHandle ReadBlobHandle() { throw null; }

        public bool ReadBoolean() { throw null; }

        public byte ReadByte() { throw null; }

        public byte[] ReadBytes(int byteCount) { throw null; }

        public char ReadChar() { throw null; }

        public int ReadCompressedInteger() { throw null; }

        public int ReadCompressedSignedInteger() { throw null; }

        public object ReadConstant(ConstantTypeCode typeCode) { throw null; }

        public DateTime ReadDateTime() { throw null; }

        public decimal ReadDecimal() { throw null; }

        public double ReadDouble() { throw null; }

        public Guid ReadGuid() { throw null; }

        public short ReadInt16() { throw null; }

        public int ReadInt32() { throw null; }

        public long ReadInt64() { throw null; }

        public sbyte ReadSByte() { throw null; }

        public SerializationTypeCode ReadSerializationTypeCode() { throw null; }

        public string ReadSerializedString() { throw null; }

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

    public partial struct Constant
    {
        private object _dummy;
        private int _dummyPrimitive;
        public EntityHandle Parent { get { throw null; } }

        public ConstantTypeCode TypeCode { get { throw null; } }

        public BlobHandle Value { get { throw null; } }
    }

    public partial struct ConstantHandle : IEquatable<ConstantHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(ConstantHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

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

    public partial struct CustomAttribute
    {
        private object _dummy;
        private int _dummyPrimitive;
        public EntityHandle Constructor { get { throw null; } }

        public EntityHandle Parent { get { throw null; } }

        public BlobHandle Value { get { throw null; } }
    }

    public partial struct CustomAttributeHandle : IEquatable<CustomAttributeHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(CustomAttributeHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(CustomAttributeHandle left, CustomAttributeHandle right) { throw null; }

        public static explicit operator CustomAttributeHandle(EntityHandle handle) { throw null; }

        public static explicit operator CustomAttributeHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(CustomAttributeHandle handle) { throw null; }

        public static implicit operator Handle(CustomAttributeHandle handle) { throw null; }

        public static bool operator !=(CustomAttributeHandle left, CustomAttributeHandle right) { throw null; }
    }

    public partial struct CustomAttributeHandleCollection : Collections.Generic.IReadOnlyCollection<CustomAttributeHandle>, Collections.Generic.IEnumerable<CustomAttributeHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<CustomAttributeHandle> Collections.Generic.IEnumerable<CustomAttributeHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct CustomDebugInformation
    {
        private object _dummy;
        private int _dummyPrimitive;
        public GuidHandle Kind { get { throw null; } }

        public EntityHandle Parent { get { throw null; } }

        public BlobHandle Value { get { throw null; } }
    }

    public partial struct CustomDebugInformationHandle : IEquatable<CustomDebugInformationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(CustomDebugInformationHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(CustomDebugInformationHandle left, CustomDebugInformationHandle right) { throw null; }

        public static explicit operator CustomDebugInformationHandle(EntityHandle handle) { throw null; }

        public static explicit operator CustomDebugInformationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(CustomDebugInformationHandle handle) { throw null; }

        public static implicit operator Handle(CustomDebugInformationHandle handle) { throw null; }

        public static bool operator !=(CustomDebugInformationHandle left, CustomDebugInformationHandle right) { throw null; }
    }

    public partial struct CustomDebugInformationHandleCollection : Collections.Generic.IReadOnlyCollection<CustomDebugInformationHandle>, Collections.Generic.IEnumerable<CustomDebugInformationHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<CustomDebugInformationHandle> Collections.Generic.IEnumerable<CustomDebugInformationHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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
    }

    public partial struct DeclarativeSecurityAttribute
    {
        private object _dummy;
        private int _dummyPrimitive;
        public DeclarativeSecurityAction Action { get { throw null; } }

        public EntityHandle Parent { get { throw null; } }

        public BlobHandle PermissionSet { get { throw null; } }
    }

    public partial struct DeclarativeSecurityAttributeHandle : IEquatable<DeclarativeSecurityAttributeHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(DeclarativeSecurityAttributeHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(DeclarativeSecurityAttributeHandle left, DeclarativeSecurityAttributeHandle right) { throw null; }

        public static explicit operator DeclarativeSecurityAttributeHandle(EntityHandle handle) { throw null; }

        public static explicit operator DeclarativeSecurityAttributeHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(DeclarativeSecurityAttributeHandle handle) { throw null; }

        public static implicit operator Handle(DeclarativeSecurityAttributeHandle handle) { throw null; }

        public static bool operator !=(DeclarativeSecurityAttributeHandle left, DeclarativeSecurityAttributeHandle right) { throw null; }
    }

    public partial struct DeclarativeSecurityAttributeHandleCollection : Collections.Generic.IReadOnlyCollection<DeclarativeSecurityAttributeHandle>, Collections.Generic.IEnumerable<DeclarativeSecurityAttributeHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<DeclarativeSecurityAttributeHandle> Collections.Generic.IEnumerable<DeclarativeSecurityAttributeHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct Document
    {
        private object _dummy;
        private int _dummyPrimitive;
        public BlobHandle Hash { get { throw null; } }

        public GuidHandle HashAlgorithm { get { throw null; } }

        public GuidHandle Language { get { throw null; } }

        public DocumentNameBlobHandle Name { get { throw null; } }
    }

    public partial struct DocumentHandle : IEquatable<DocumentHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(DocumentHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(DocumentHandle left, DocumentHandle right) { throw null; }

        public static explicit operator DocumentHandle(EntityHandle handle) { throw null; }

        public static explicit operator DocumentHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(DocumentHandle handle) { throw null; }

        public static implicit operator Handle(DocumentHandle handle) { throw null; }

        public static bool operator !=(DocumentHandle left, DocumentHandle right) { throw null; }
    }

    public partial struct DocumentHandleCollection : Collections.Generic.IReadOnlyCollection<DocumentHandle>, Collections.Generic.IEnumerable<DocumentHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<DocumentHandle> Collections.Generic.IEnumerable<DocumentHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct DocumentNameBlobHandle : IEquatable<DocumentNameBlobHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(DocumentNameBlobHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(DocumentNameBlobHandle left, DocumentNameBlobHandle right) { throw null; }

        public static explicit operator DocumentNameBlobHandle(BlobHandle handle) { throw null; }

        public static implicit operator BlobHandle(DocumentNameBlobHandle handle) { throw null; }

        public static bool operator !=(DocumentNameBlobHandle left, DocumentNameBlobHandle right) { throw null; }
    }

    public partial struct EntityHandle : IEquatable<EntityHandle>
    {
        private int _dummyPrimitive;
        public static readonly AssemblyDefinitionHandle AssemblyDefinition;
        public static readonly ModuleDefinitionHandle ModuleDefinition;
        public bool IsNil { get { throw null; } }

        public HandleKind Kind { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(EntityHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(EntityHandle left, EntityHandle right) { throw null; }

        public static explicit operator EntityHandle(Handle handle) { throw null; }

        public static implicit operator Handle(EntityHandle handle) { throw null; }

        public static bool operator !=(EntityHandle left, EntityHandle right) { throw null; }
    }

    public partial struct EventAccessors
    {
        private int _dummyPrimitive;
        public MethodDefinitionHandle Adder { get { throw null; } }

        public MethodDefinitionHandle Raiser { get { throw null; } }

        public MethodDefinitionHandle Remover { get { throw null; } }
    }

    public partial struct EventDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public EventAttributes Attributes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public EntityHandle Type { get { throw null; } }

        public EventAccessors GetAccessors() { throw null; }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct EventDefinitionHandle : IEquatable<EventDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(EventDefinitionHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(EventDefinitionHandle left, EventDefinitionHandle right) { throw null; }

        public static explicit operator EventDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator EventDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(EventDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(EventDefinitionHandle handle) { throw null; }

        public static bool operator !=(EventDefinitionHandle left, EventDefinitionHandle right) { throw null; }
    }

    public partial struct EventDefinitionHandleCollection : Collections.Generic.IReadOnlyCollection<EventDefinitionHandle>, Collections.Generic.IEnumerable<EventDefinitionHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<EventDefinitionHandle> Collections.Generic.IEnumerable<EventDefinitionHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct ExceptionRegion
    {
        private int _dummyPrimitive;
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

    public partial struct ExportedType
    {
        private object _dummy;
        private int _dummyPrimitive;
        public TypeAttributes Attributes { get { throw null; } }

        public EntityHandle Implementation { get { throw null; } }

        public bool IsForwarder { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public StringHandle Namespace { get { throw null; } }

        public NamespaceDefinitionHandle NamespaceDefinition { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct ExportedTypeHandle : IEquatable<ExportedTypeHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(ExportedTypeHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(ExportedTypeHandle left, ExportedTypeHandle right) { throw null; }

        public static explicit operator ExportedTypeHandle(EntityHandle handle) { throw null; }

        public static explicit operator ExportedTypeHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ExportedTypeHandle handle) { throw null; }

        public static implicit operator Handle(ExportedTypeHandle handle) { throw null; }

        public static bool operator !=(ExportedTypeHandle left, ExportedTypeHandle right) { throw null; }
    }

    public partial struct ExportedTypeHandleCollection : Collections.Generic.IReadOnlyCollection<ExportedTypeHandle>, Collections.Generic.IEnumerable<ExportedTypeHandle>, Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<ExportedTypeHandle> Collections.Generic.IEnumerable<ExportedTypeHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct FieldDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public FieldAttributes Attributes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public TypeDefinitionHandle GetDeclaringType() { throw null; }

        public ConstantHandle GetDefaultValue() { throw null; }

        public BlobHandle GetMarshallingDescriptor() { throw null; }

        public int GetOffset() { throw null; }

        public int GetRelativeVirtualAddress() { throw null; }
    }

    public partial struct FieldDefinitionHandle : IEquatable<FieldDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(FieldDefinitionHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(FieldDefinitionHandle left, FieldDefinitionHandle right) { throw null; }

        public static explicit operator FieldDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator FieldDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(FieldDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(FieldDefinitionHandle handle) { throw null; }

        public static bool operator !=(FieldDefinitionHandle left, FieldDefinitionHandle right) { throw null; }
    }

    public partial struct FieldDefinitionHandleCollection : Collections.Generic.IReadOnlyCollection<FieldDefinitionHandle>, Collections.Generic.IEnumerable<FieldDefinitionHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<FieldDefinitionHandle> Collections.Generic.IEnumerable<FieldDefinitionHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct GenericParameter
    {
        private object _dummy;
        private int _dummyPrimitive;
        public GenericParameterAttributes Attributes { get { throw null; } }

        public int Index { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public EntityHandle Parent { get { throw null; } }

        public GenericParameterConstraintHandleCollection GetConstraints() { throw null; }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct GenericParameterConstraint
    {
        private object _dummy;
        private int _dummyPrimitive;
        public GenericParameterHandle Parameter { get { throw null; } }

        public EntityHandle Type { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct GenericParameterConstraintHandle : IEquatable<GenericParameterConstraintHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(GenericParameterConstraintHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(GenericParameterConstraintHandle left, GenericParameterConstraintHandle right) { throw null; }

        public static explicit operator GenericParameterConstraintHandle(EntityHandle handle) { throw null; }

        public static explicit operator GenericParameterConstraintHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(GenericParameterConstraintHandle handle) { throw null; }

        public static implicit operator Handle(GenericParameterConstraintHandle handle) { throw null; }

        public static bool operator !=(GenericParameterConstraintHandle left, GenericParameterConstraintHandle right) { throw null; }
    }

    public partial struct GenericParameterConstraintHandleCollection : Collections.Generic.IReadOnlyList<GenericParameterConstraintHandle>, Collections.Generic.IReadOnlyCollection<GenericParameterConstraintHandle>, Collections.Generic.IEnumerable<GenericParameterConstraintHandle>, Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public GenericParameterConstraintHandle this[int index] { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<GenericParameterConstraintHandle> Collections.Generic.IEnumerable<GenericParameterConstraintHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct GenericParameterHandle : IEquatable<GenericParameterHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(GenericParameterHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(GenericParameterHandle left, GenericParameterHandle right) { throw null; }

        public static explicit operator GenericParameterHandle(EntityHandle handle) { throw null; }

        public static explicit operator GenericParameterHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(GenericParameterHandle handle) { throw null; }

        public static implicit operator Handle(GenericParameterHandle handle) { throw null; }

        public static bool operator !=(GenericParameterHandle left, GenericParameterHandle right) { throw null; }
    }

    public partial struct GenericParameterHandleCollection : Collections.Generic.IReadOnlyList<GenericParameterHandle>, Collections.Generic.IReadOnlyCollection<GenericParameterHandle>, Collections.Generic.IEnumerable<GenericParameterHandle>, Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public GenericParameterHandle this[int index] { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<GenericParameterHandle> Collections.Generic.IEnumerable<GenericParameterHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct GuidHandle : IEquatable<GuidHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(GuidHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(GuidHandle left, GuidHandle right) { throw null; }

        public static explicit operator GuidHandle(Handle handle) { throw null; }

        public static implicit operator Handle(GuidHandle handle) { throw null; }

        public static bool operator !=(GuidHandle left, GuidHandle right) { throw null; }
    }

    public partial struct Handle : IEquatable<Handle>
    {
        private int _dummyPrimitive;
        public static readonly AssemblyDefinitionHandle AssemblyDefinition;
        public static readonly ModuleDefinitionHandle ModuleDefinition;
        public bool IsNil { get { throw null; } }

        public HandleKind Kind { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(Handle other) { throw null; }

        public override int GetHashCode() { throw null; }

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

    public partial struct ImportDefinition
    {
        private int _dummyPrimitive;
        public BlobHandle Alias { get { throw null; } }

        public ImportDefinitionKind Kind { get { throw null; } }

        public AssemblyReferenceHandle TargetAssembly { get { throw null; } }

        public BlobHandle TargetNamespace { get { throw null; } }

        public EntityHandle TargetType { get { throw null; } }
    }

    public partial struct ImportDefinitionCollection : Collections.Generic.IEnumerable<ImportDefinition>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<ImportDefinition> Collections.Generic.IEnumerable<ImportDefinition>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct ImportScope
    {
        private object _dummy;
        private int _dummyPrimitive;
        public BlobHandle ImportsBlob { get { throw null; } }

        public ImportScopeHandle Parent { get { throw null; } }

        public ImportDefinitionCollection GetImports() { throw null; }
    }

    public partial struct ImportScopeCollection : Collections.Generic.IReadOnlyCollection<ImportScopeHandle>, Collections.Generic.IEnumerable<ImportScopeHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<ImportScopeHandle> Collections.Generic.IEnumerable<ImportScopeHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct ImportScopeHandle : IEquatable<ImportScopeHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(ImportScopeHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(ImportScopeHandle left, ImportScopeHandle right) { throw null; }

        public static explicit operator ImportScopeHandle(EntityHandle handle) { throw null; }

        public static explicit operator ImportScopeHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ImportScopeHandle handle) { throw null; }

        public static implicit operator Handle(ImportScopeHandle handle) { throw null; }

        public static bool operator !=(ImportScopeHandle left, ImportScopeHandle right) { throw null; }
    }

    public partial struct InterfaceImplementation
    {
        private object _dummy;
        private int _dummyPrimitive;
        public EntityHandle Interface { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct InterfaceImplementationHandle : IEquatable<InterfaceImplementationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(InterfaceImplementationHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(InterfaceImplementationHandle left, InterfaceImplementationHandle right) { throw null; }

        public static explicit operator InterfaceImplementationHandle(EntityHandle handle) { throw null; }

        public static explicit operator InterfaceImplementationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(InterfaceImplementationHandle handle) { throw null; }

        public static implicit operator Handle(InterfaceImplementationHandle handle) { throw null; }

        public static bool operator !=(InterfaceImplementationHandle left, InterfaceImplementationHandle right) { throw null; }
    }

    public partial struct InterfaceImplementationHandleCollection : Collections.Generic.IReadOnlyCollection<InterfaceImplementationHandle>, Collections.Generic.IEnumerable<InterfaceImplementationHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<InterfaceImplementationHandle> Collections.Generic.IEnumerable<InterfaceImplementationHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct LocalConstant
    {
        private object _dummy;
        private int _dummyPrimitive;
        public StringHandle Name { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }
    }

    public partial struct LocalConstantHandle : IEquatable<LocalConstantHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(LocalConstantHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(LocalConstantHandle left, LocalConstantHandle right) { throw null; }

        public static explicit operator LocalConstantHandle(EntityHandle handle) { throw null; }

        public static explicit operator LocalConstantHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(LocalConstantHandle handle) { throw null; }

        public static implicit operator Handle(LocalConstantHandle handle) { throw null; }

        public static bool operator !=(LocalConstantHandle left, LocalConstantHandle right) { throw null; }
    }

    public partial struct LocalConstantHandleCollection : Collections.Generic.IReadOnlyCollection<LocalConstantHandle>, Collections.Generic.IEnumerable<LocalConstantHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<LocalConstantHandle> Collections.Generic.IEnumerable<LocalConstantHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct LocalScope
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int EndOffset { get { throw null; } }

        public ImportScopeHandle ImportScope { get { throw null; } }

        public int Length { get { throw null; } }

        public MethodDefinitionHandle Method { get { throw null; } }

        public int StartOffset { get { throw null; } }

        public LocalScopeHandleCollection.ChildrenEnumerator GetChildren() { throw null; }

        public LocalConstantHandleCollection GetLocalConstants() { throw null; }

        public LocalVariableHandleCollection GetLocalVariables() { throw null; }
    }

    public partial struct LocalScopeHandle : IEquatable<LocalScopeHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(LocalScopeHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(LocalScopeHandle left, LocalScopeHandle right) { throw null; }

        public static explicit operator LocalScopeHandle(EntityHandle handle) { throw null; }

        public static explicit operator LocalScopeHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(LocalScopeHandle handle) { throw null; }

        public static implicit operator Handle(LocalScopeHandle handle) { throw null; }

        public static bool operator !=(LocalScopeHandle left, LocalScopeHandle right) { throw null; }
    }

    public partial struct LocalScopeHandleCollection : Collections.Generic.IReadOnlyCollection<LocalScopeHandle>, Collections.Generic.IEnumerable<LocalScopeHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<LocalScopeHandle> Collections.Generic.IEnumerable<LocalScopeHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct LocalVariable
    {
        private object _dummy;
        private int _dummyPrimitive;
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

    public partial struct LocalVariableHandle : IEquatable<LocalVariableHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(LocalVariableHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(LocalVariableHandle left, LocalVariableHandle right) { throw null; }

        public static explicit operator LocalVariableHandle(EntityHandle handle) { throw null; }

        public static explicit operator LocalVariableHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(LocalVariableHandle handle) { throw null; }

        public static implicit operator Handle(LocalVariableHandle handle) { throw null; }

        public static bool operator !=(LocalVariableHandle left, LocalVariableHandle right) { throw null; }
    }

    public partial struct LocalVariableHandleCollection : Collections.Generic.IReadOnlyCollection<LocalVariableHandle>, Collections.Generic.IEnumerable<LocalVariableHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<LocalVariableHandle> Collections.Generic.IEnumerable<LocalVariableHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct ManifestResource
    {
        private object _dummy;
        private int _dummyPrimitive;
        public ManifestResourceAttributes Attributes { get { throw null; } }

        public EntityHandle Implementation { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public long Offset { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct ManifestResourceHandle : IEquatable<ManifestResourceHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(ManifestResourceHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(ManifestResourceHandle left, ManifestResourceHandle right) { throw null; }

        public static explicit operator ManifestResourceHandle(EntityHandle handle) { throw null; }

        public static explicit operator ManifestResourceHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ManifestResourceHandle handle) { throw null; }

        public static implicit operator Handle(ManifestResourceHandle handle) { throw null; }

        public static bool operator !=(ManifestResourceHandle left, ManifestResourceHandle right) { throw null; }
    }

    public partial struct ManifestResourceHandleCollection : Collections.Generic.IReadOnlyCollection<ManifestResourceHandle>, Collections.Generic.IEnumerable<ManifestResourceHandle>, Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<ManifestResourceHandle> Collections.Generic.IEnumerable<ManifestResourceHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct MemberReference
    {
        private object _dummy;
        private int _dummyPrimitive;
        public StringHandle Name { get { throw null; } }

        public EntityHandle Parent { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public MemberReferenceKind GetKind() { throw null; }
    }

    public partial struct MemberReferenceHandle : IEquatable<MemberReferenceHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(MemberReferenceHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(MemberReferenceHandle left, MemberReferenceHandle right) { throw null; }

        public static explicit operator MemberReferenceHandle(EntityHandle handle) { throw null; }

        public static explicit operator MemberReferenceHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(MemberReferenceHandle handle) { throw null; }

        public static implicit operator Handle(MemberReferenceHandle handle) { throw null; }

        public static bool operator !=(MemberReferenceHandle left, MemberReferenceHandle right) { throw null; }
    }

    public partial struct MemberReferenceHandleCollection : Collections.Generic.IReadOnlyCollection<MemberReferenceHandle>, Collections.Generic.IEnumerable<MemberReferenceHandle>, Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<MemberReferenceHandle> Collections.Generic.IEnumerable<MemberReferenceHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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
        public unsafe MetadataReader(byte* metadata, int length, MetadataReaderOptions options, MetadataStringDecoder utf8Decoder) { }

        public unsafe MetadataReader(byte* metadata, int length, MetadataReaderOptions options) { }

        public unsafe MetadataReader(byte* metadata, int length) { }

        public AssemblyFileHandleCollection AssemblyFiles { get { throw null; } }

        public AssemblyReferenceHandleCollection AssemblyReferences { get { throw null; } }

        public CustomAttributeHandleCollection CustomAttributes { get { throw null; } }

        public CustomDebugInformationHandleCollection CustomDebugInformation { get { throw null; } }

        public DebugMetadataHeader DebugMetadataHeader { get { throw null; } }

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

        public string MetadataVersion { get { throw null; } }

        public MethodDebugInformationHandleCollection MethodDebugInformation { get { throw null; } }

        public MethodDefinitionHandleCollection MethodDefinitions { get { throw null; } }

        public MetadataReaderOptions Options { get { throw null; } }

        public PropertyDefinitionHandleCollection PropertyDefinitions { get { throw null; } }

        public MetadataStringComparer StringComparer { get { throw null; } }

        public TypeDefinitionHandleCollection TypeDefinitions { get { throw null; } }

        public TypeReferenceHandleCollection TypeReferences { get { throw null; } }

        public AssemblyDefinition GetAssemblyDefinition() { throw null; }

        public AssemblyFile GetAssemblyFile(AssemblyFileHandle handle) { throw null; }

        public AssemblyReference GetAssemblyReference(AssemblyReferenceHandle handle) { throw null; }

        public byte[] GetBlobBytes(BlobHandle handle) { throw null; }

        public Collections.Immutable.ImmutableArray<byte> GetBlobContent(BlobHandle handle) { throw null; }

        public BlobReader GetBlobReader(BlobHandle handle) { throw null; }

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

        public MetadataReader GetMetadataReader(MetadataReaderOptions options = MetadataReaderOptions.ApplyWindowsRuntimeProjections, MetadataStringDecoder utf8Decoder = null) { throw null; }
    }

    [Flags]
    public enum MetadataStreamOptions
    {
        Default = 0,
        LeaveOpen = 1,
        PrefetchMetadata = 2
    }

    public partial struct MetadataStringComparer
    {
        private object _dummy;
        private int _dummyPrimitive;
        public bool Equals(DocumentNameBlobHandle handle, string value, bool ignoreCase) { throw null; }

        public bool Equals(DocumentNameBlobHandle handle, string value) { throw null; }

        public bool Equals(NamespaceDefinitionHandle handle, string value, bool ignoreCase) { throw null; }

        public bool Equals(NamespaceDefinitionHandle handle, string value) { throw null; }

        public bool Equals(StringHandle handle, string value, bool ignoreCase) { throw null; }

        public bool Equals(StringHandle handle, string value) { throw null; }

        public bool StartsWith(StringHandle handle, string value, bool ignoreCase) { throw null; }

        public bool StartsWith(StringHandle handle, string value) { throw null; }
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

        public byte[] GetILBytes() { throw null; }

        public Collections.Immutable.ImmutableArray<byte> GetILContent() { throw null; }

        public BlobReader GetILReader() { throw null; }
    }

    public partial struct MethodDebugInformation
    {
        private object _dummy;
        private int _dummyPrimitive;
        public DocumentHandle Document { get { throw null; } }

        public StandaloneSignatureHandle LocalSignature { get { throw null; } }

        public BlobHandle SequencePointsBlob { get { throw null; } }

        public SequencePointCollection GetSequencePoints() { throw null; }

        public MethodDefinitionHandle GetStateMachineKickoffMethod() { throw null; }
    }

    public partial struct MethodDebugInformationHandle : IEquatable<MethodDebugInformationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(MethodDebugInformationHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(MethodDebugInformationHandle left, MethodDebugInformationHandle right) { throw null; }

        public static explicit operator MethodDebugInformationHandle(EntityHandle handle) { throw null; }

        public static explicit operator MethodDebugInformationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(MethodDebugInformationHandle handle) { throw null; }

        public static implicit operator Handle(MethodDebugInformationHandle handle) { throw null; }

        public static bool operator !=(MethodDebugInformationHandle left, MethodDebugInformationHandle right) { throw null; }

        public MethodDefinitionHandle ToDefinitionHandle() { throw null; }
    }

    public partial struct MethodDebugInformationHandleCollection : Collections.Generic.IReadOnlyCollection<MethodDebugInformationHandle>, Collections.Generic.IEnumerable<MethodDebugInformationHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<MethodDebugInformationHandle> Collections.Generic.IEnumerable<MethodDebugInformationHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct MethodDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public MethodAttributes Attributes { get { throw null; } }

        public MethodImplAttributes ImplAttributes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public int RelativeVirtualAddress { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes() { throw null; }

        public TypeDefinitionHandle GetDeclaringType() { throw null; }

        public GenericParameterHandleCollection GetGenericParameters() { throw null; }

        public MethodImport GetImport() { throw null; }

        public ParameterHandleCollection GetParameters() { throw null; }
    }

    public partial struct MethodDefinitionHandle : IEquatable<MethodDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(MethodDefinitionHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(MethodDefinitionHandle left, MethodDefinitionHandle right) { throw null; }

        public static explicit operator MethodDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator MethodDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(MethodDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(MethodDefinitionHandle handle) { throw null; }

        public static bool operator !=(MethodDefinitionHandle left, MethodDefinitionHandle right) { throw null; }

        public MethodDebugInformationHandle ToDebugInformationHandle() { throw null; }
    }

    public partial struct MethodDefinitionHandleCollection : Collections.Generic.IReadOnlyCollection<MethodDefinitionHandle>, Collections.Generic.IEnumerable<MethodDefinitionHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<MethodDefinitionHandle> Collections.Generic.IEnumerable<MethodDefinitionHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct MethodImplementation
    {
        private object _dummy;
        private int _dummyPrimitive;
        public EntityHandle MethodBody { get { throw null; } }

        public EntityHandle MethodDeclaration { get { throw null; } }

        public TypeDefinitionHandle Type { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct MethodImplementationHandle : IEquatable<MethodImplementationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(MethodImplementationHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(MethodImplementationHandle left, MethodImplementationHandle right) { throw null; }

        public static explicit operator MethodImplementationHandle(EntityHandle handle) { throw null; }

        public static explicit operator MethodImplementationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(MethodImplementationHandle handle) { throw null; }

        public static implicit operator Handle(MethodImplementationHandle handle) { throw null; }

        public static bool operator !=(MethodImplementationHandle left, MethodImplementationHandle right) { throw null; }
    }

    public partial struct MethodImplementationHandleCollection : Collections.Generic.IReadOnlyCollection<MethodImplementationHandle>, Collections.Generic.IEnumerable<MethodImplementationHandle>, Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<MethodImplementationHandle> Collections.Generic.IEnumerable<MethodImplementationHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct MethodImport
    {
        private int _dummyPrimitive;
        public MethodImportAttributes Attributes { get { throw null; } }

        public ModuleReferenceHandle Module { get { throw null; } }

        public StringHandle Name { get { throw null; } }
    }

    public partial struct MethodSpecification
    {
        private object _dummy;
        private int _dummyPrimitive;
        public EntityHandle Method { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct MethodSpecificationHandle : IEquatable<MethodSpecificationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(MethodSpecificationHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(MethodSpecificationHandle left, MethodSpecificationHandle right) { throw null; }

        public static explicit operator MethodSpecificationHandle(EntityHandle handle) { throw null; }

        public static explicit operator MethodSpecificationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(MethodSpecificationHandle handle) { throw null; }

        public static implicit operator Handle(MethodSpecificationHandle handle) { throw null; }

        public static bool operator !=(MethodSpecificationHandle left, MethodSpecificationHandle right) { throw null; }
    }

    public partial struct ModuleDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public GuidHandle BaseGenerationId { get { throw null; } }

        public int Generation { get { throw null; } }

        public GuidHandle GenerationId { get { throw null; } }

        public GuidHandle Mvid { get { throw null; } }

        public StringHandle Name { get { throw null; } }
    }

    public partial struct ModuleDefinitionHandle : IEquatable<ModuleDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(ModuleDefinitionHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(ModuleDefinitionHandle left, ModuleDefinitionHandle right) { throw null; }

        public static explicit operator ModuleDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator ModuleDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ModuleDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(ModuleDefinitionHandle handle) { throw null; }

        public static bool operator !=(ModuleDefinitionHandle left, ModuleDefinitionHandle right) { throw null; }
    }

    public partial struct ModuleReference
    {
        private object _dummy;
        private int _dummyPrimitive;
        public StringHandle Name { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct ModuleReferenceHandle : IEquatable<ModuleReferenceHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(ModuleReferenceHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

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

    public partial struct NamespaceDefinitionHandle : IEquatable<NamespaceDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(NamespaceDefinitionHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(NamespaceDefinitionHandle left, NamespaceDefinitionHandle right) { throw null; }

        public static explicit operator NamespaceDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator Handle(NamespaceDefinitionHandle handle) { throw null; }

        public static bool operator !=(NamespaceDefinitionHandle left, NamespaceDefinitionHandle right) { throw null; }
    }

    public partial struct Parameter
    {
        private object _dummy;
        private int _dummyPrimitive;
        public ParameterAttributes Attributes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public int SequenceNumber { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public ConstantHandle GetDefaultValue() { throw null; }

        public BlobHandle GetMarshallingDescriptor() { throw null; }
    }

    public partial struct ParameterHandle : IEquatable<ParameterHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(ParameterHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(ParameterHandle left, ParameterHandle right) { throw null; }

        public static explicit operator ParameterHandle(EntityHandle handle) { throw null; }

        public static explicit operator ParameterHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(ParameterHandle handle) { throw null; }

        public static implicit operator Handle(ParameterHandle handle) { throw null; }

        public static bool operator !=(ParameterHandle left, ParameterHandle right) { throw null; }
    }

    public partial struct ParameterHandleCollection : Collections.Generic.IReadOnlyCollection<ParameterHandle>, Collections.Generic.IEnumerable<ParameterHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<ParameterHandle> Collections.Generic.IEnumerable<ParameterHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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
        public static MetadataReader GetMetadataReader(this PortableExecutable.PEReader peReader, MetadataReaderOptions options, MetadataStringDecoder utf8Decoder) { throw null; }

        public static MetadataReader GetMetadataReader(this PortableExecutable.PEReader peReader, MetadataReaderOptions options) { throw null; }

        public static MetadataReader GetMetadataReader(this PortableExecutable.PEReader peReader) { throw null; }

        public static MethodBodyBlock GetMethodBody(this PortableExecutable.PEReader peReader, int relativeVirtualAddress) { throw null; }
    }

    public partial struct PropertyAccessors
    {
        private int _dummyPrimitive;
        public MethodDefinitionHandle Getter { get { throw null; } }

        public MethodDefinitionHandle Setter { get { throw null; } }
    }

    public partial struct PropertyDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public PropertyAttributes Attributes { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public BlobHandle Signature { get { throw null; } }

        public PropertyAccessors GetAccessors() { throw null; }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public ConstantHandle GetDefaultValue() { throw null; }
    }

    public partial struct PropertyDefinitionHandle : IEquatable<PropertyDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(PropertyDefinitionHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(PropertyDefinitionHandle left, PropertyDefinitionHandle right) { throw null; }

        public static explicit operator PropertyDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator PropertyDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(PropertyDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(PropertyDefinitionHandle handle) { throw null; }

        public static bool operator !=(PropertyDefinitionHandle left, PropertyDefinitionHandle right) { throw null; }
    }

    public partial struct PropertyDefinitionHandleCollection : Collections.Generic.IReadOnlyCollection<PropertyDefinitionHandle>, Collections.Generic.IEnumerable<PropertyDefinitionHandle>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<PropertyDefinitionHandle> Collections.Generic.IEnumerable<PropertyDefinitionHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct SequencePoint : IEquatable<SequencePoint>
    {
        private int _dummyPrimitive;
        public const int HiddenLine = 16707566;
        public DocumentHandle Document { get { throw null; } }

        public int EndColumn { get { throw null; } }

        public int EndLine { get { throw null; } }

        public bool IsHidden { get { throw null; } }

        public int Offset { get { throw null; } }

        public int StartColumn { get { throw null; } }

        public int StartLine { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(SequencePoint other) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public partial struct SequencePointCollection : Collections.Generic.IEnumerable<SequencePoint>, Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<SequencePoint> Collections.Generic.IEnumerable<SequencePoint>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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
        VarArgs = 5
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

        public override bool Equals(object obj) { throw null; }

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

    public partial struct StandaloneSignature
    {
        private object _dummy;
        private int _dummyPrimitive;
        public BlobHandle Signature { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public StandaloneSignatureKind GetKind() { throw null; }
    }

    public partial struct StandaloneSignatureHandle : IEquatable<StandaloneSignatureHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(StandaloneSignatureHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

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

    public partial struct StringHandle : IEquatable<StringHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(StringHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(StringHandle left, StringHandle right) { throw null; }

        public static explicit operator StringHandle(Handle handle) { throw null; }

        public static implicit operator Handle(StringHandle handle) { throw null; }

        public static bool operator !=(StringHandle left, StringHandle right) { throw null; }
    }

    public partial struct TypeDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public TypeAttributes Attributes { get { throw null; } }

        public EntityHandle BaseType { get { throw null; } }

        public StringHandle Name { get { throw null; } }

        public StringHandle Namespace { get { throw null; } }

        public NamespaceDefinitionHandle NamespaceDefinition { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }

        public DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes() { throw null; }

        public TypeDefinitionHandle GetDeclaringType() { throw null; }

        public EventDefinitionHandleCollection GetEvents() { throw null; }

        public FieldDefinitionHandleCollection GetFields() { throw null; }

        public GenericParameterHandleCollection GetGenericParameters() { throw null; }

        public InterfaceImplementationHandleCollection GetInterfaceImplementations() { throw null; }

        public TypeLayout GetLayout() { throw null; }

        public MethodImplementationHandleCollection GetMethodImplementations() { throw null; }

        public MethodDefinitionHandleCollection GetMethods() { throw null; }

        public Collections.Immutable.ImmutableArray<TypeDefinitionHandle> GetNestedTypes() { throw null; }

        public PropertyDefinitionHandleCollection GetProperties() { throw null; }
    }

    public partial struct TypeDefinitionHandle : IEquatable<TypeDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(TypeDefinitionHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(TypeDefinitionHandle left, TypeDefinitionHandle right) { throw null; }

        public static explicit operator TypeDefinitionHandle(EntityHandle handle) { throw null; }

        public static explicit operator TypeDefinitionHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(TypeDefinitionHandle handle) { throw null; }

        public static implicit operator Handle(TypeDefinitionHandle handle) { throw null; }

        public static bool operator !=(TypeDefinitionHandle left, TypeDefinitionHandle right) { throw null; }
    }

    public partial struct TypeDefinitionHandleCollection : Collections.Generic.IReadOnlyCollection<TypeDefinitionHandle>, Collections.Generic.IEnumerable<TypeDefinitionHandle>, Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<TypeDefinitionHandle> Collections.Generic.IEnumerable<TypeDefinitionHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct TypeLayout
    {
        private int _dummyPrimitive;
        public TypeLayout(int size, int packingSize) { }

        public bool IsDefault { get { throw null; } }

        public int PackingSize { get { throw null; } }

        public int Size { get { throw null; } }
    }

    public partial struct TypeReference
    {
        private object _dummy;
        private int _dummyPrimitive;
        public StringHandle Name { get { throw null; } }

        public StringHandle Namespace { get { throw null; } }

        public EntityHandle ResolutionScope { get { throw null; } }
    }

    public partial struct TypeReferenceHandle : IEquatable<TypeReferenceHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(TypeReferenceHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(TypeReferenceHandle left, TypeReferenceHandle right) { throw null; }

        public static explicit operator TypeReferenceHandle(EntityHandle handle) { throw null; }

        public static explicit operator TypeReferenceHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(TypeReferenceHandle handle) { throw null; }

        public static implicit operator Handle(TypeReferenceHandle handle) { throw null; }

        public static bool operator !=(TypeReferenceHandle left, TypeReferenceHandle right) { throw null; }
    }

    public partial struct TypeReferenceHandleCollection : Collections.Generic.IReadOnlyCollection<TypeReferenceHandle>, Collections.Generic.IEnumerable<TypeReferenceHandle>, Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }

        public Enumerator GetEnumerator() { throw null; }

        Collections.Generic.IEnumerator<TypeReferenceHandle> Collections.Generic.IEnumerable<TypeReferenceHandle>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

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

    public partial struct TypeSpecification
    {
        private object _dummy;
        private int _dummyPrimitive;
        public BlobHandle Signature { get { throw null; } }

        public CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }

    public partial struct TypeSpecificationHandle : IEquatable<TypeSpecificationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(TypeSpecificationHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(TypeSpecificationHandle left, TypeSpecificationHandle right) { throw null; }

        public static explicit operator TypeSpecificationHandle(EntityHandle handle) { throw null; }

        public static explicit operator TypeSpecificationHandle(Handle handle) { throw null; }

        public static implicit operator EntityHandle(TypeSpecificationHandle handle) { throw null; }

        public static implicit operator Handle(TypeSpecificationHandle handle) { throw null; }

        public static bool operator !=(TypeSpecificationHandle left, TypeSpecificationHandle right) { throw null; }
    }

    public partial struct UserStringHandle : IEquatable<UserStringHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(UserStringHandle other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(UserStringHandle left, UserStringHandle right) { throw null; }

        public static explicit operator UserStringHandle(Handle handle) { throw null; }

        public static implicit operator Handle(UserStringHandle handle) { throw null; }

        public static bool operator !=(UserStringHandle left, UserStringHandle right) { throw null; }
    }
}

namespace System.Reflection.Metadata.Ecma335
{
    public partial struct EditAndContinueLogEntry : IEquatable<EditAndContinueLogEntry>
    {
        private int _dummyPrimitive;
        public EditAndContinueLogEntry(EntityHandle handle, EditAndContinueOperation operation) { }

        public EntityHandle Handle { get { throw null; } }

        public EditAndContinueOperation Operation { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(EditAndContinueLogEntry other) { throw null; }

        public override int GetHashCode() { throw null; }
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

    public static partial class ExportedTypeExtensions
    {
        public static int GetTypeDefinitionId(this ExportedType exportedType) { throw null; }
    }

    public enum HeapIndex
    {
        UserString = 0,
        String = 1,
        Blob = 2,
        Guid = 3
    }

    public sealed partial class MetadataAggregator
    {
        public MetadataAggregator(Collections.Generic.IReadOnlyList<int> baseTableRowCounts, Collections.Generic.IReadOnlyList<int> baseHeapSizes, Collections.Generic.IReadOnlyList<MetadataReader> deltaReaders) { }

        public MetadataAggregator(MetadataReader baseReader, Collections.Generic.IReadOnlyList<MetadataReader> deltaReaders) { }

        public Handle GetGenerationHandle(Handle handle, out int generation) { throw null; }
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

        public static int GetHeapOffset(Handle handle) { throw null; }

        public static int GetHeapOffset(this MetadataReader reader, Handle handle) { throw null; }

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
}

namespace System.Reflection.PortableExecutable
{
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

    public partial struct CodeViewDebugDirectoryData
    {
        private object _dummy;
        private int _dummyPrimitive;
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

    public partial struct DebugDirectoryEntry
    {
        private int _dummyPrimitive;
        public DebugDirectoryEntry(uint stamp, ushort majorVersion, ushort minorVersion, DebugDirectoryEntryType type, int dataSize, int dataRelativeVirtualAddress, int dataPointer) { }

        public int DataPointer { get { throw null; } }

        public int DataRelativeVirtualAddress { get { throw null; } }

        public int DataSize { get { throw null; } }

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
        Reproducible = 16
    }

    public partial struct DirectoryEntry
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
        Amd64 = 34404,
        M32R = 36929
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

    public sealed partial class PEHeaders
    {
        public PEHeaders(IO.Stream peStream, int size) { }

        public PEHeaders(IO.Stream peStream) { }

        public CoffHeader CoffHeader { get { throw null; } }

        public int CoffHeaderStartOffset { get { throw null; } }

        public CorHeader CorHeader { get { throw null; } }

        public int CorHeaderStartOffset { get { throw null; } }

        public bool IsCoffOnly { get { throw null; } }

        public bool IsConsoleApplication { get { throw null; } }

        public bool IsDll { get { throw null; } }

        public bool IsExe { get { throw null; } }

        public int MetadataSize { get { throw null; } }

        public int MetadataStartOffset { get { throw null; } }

        public PEHeader PEHeader { get { throw null; } }

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

    public partial struct PEMemoryBlock
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Length { get { throw null; } }

        public unsafe byte* Pointer { get { throw null; } }

        public Collections.Immutable.ImmutableArray<byte> GetContent() { throw null; }
    }

    public sealed partial class PEReader : IDisposable
    {
        public unsafe PEReader(byte* peImage, int size) { }

        public PEReader(Collections.Immutable.ImmutableArray<byte> peImage) { }

        public PEReader(IO.Stream peStream, PEStreamOptions options, int size) { }

        public PEReader(IO.Stream peStream, PEStreamOptions options) { }

        public PEReader(IO.Stream peStream) { }

        public bool HasMetadata { get { throw null; } }

        public bool IsEntireImageAvailable { get { throw null; } }

        public PEHeaders PEHeaders { get { throw null; } }

        public void Dispose() { }

        public PEMemoryBlock GetEntireImage() { throw null; }

        public PEMemoryBlock GetMetadata() { throw null; }

        public PEMemoryBlock GetSectionData(int relativeVirtualAddress) { throw null; }

        public CodeViewDebugDirectoryData ReadCodeViewDebugDirectoryData(DebugDirectoryEntry entry) { throw null; }

        public Collections.Immutable.ImmutableArray<DebugDirectoryEntry> ReadDebugDirectory() { throw null; }
    }

    [Flags]
    public enum PEStreamOptions
    {
        Default = 0,
        LeaveOpen = 1,
        PrefetchMetadata = 2,
        PrefetchEntireImage = 4
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

    public partial struct SectionHeader
    {
        private object _dummy;
        private int _dummyPrimitive;
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
        Xbox = 14
    }
}