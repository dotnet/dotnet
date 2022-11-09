// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Reflection.Metadata")]
[assembly: AssemblyDescription("System.Reflection.Metadata")]
[assembly: AssemblyDefaultAlias("System.Reflection.Metadata")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("1.3.0.0")]




namespace System.Reflection
{
    [System.FlagsAttribute]
    public enum AssemblyFlags
    {
        ContentTypeMask = 3584,
        DisableJitCompileOptimizer = 16384,
        EnableJitCompileTracking = 32768,
        PublicKey = 1,
        Retargetable = 256,
        WindowsRuntime = 512,
    }
    public enum AssemblyHashAlgorithm
    {
        MD5 = 32771,
        None = 0,
        Sha1 = 32772,
        Sha256 = 32780,
        Sha384 = 32781,
        Sha512 = 32782,
    }
    public enum DeclarativeSecurityAction : short
    {
        Assert = (short)3,
        Demand = (short)2,
        Deny = (short)4,
        InheritanceDemand = (short)7,
        LinkDemand = (short)6,
        None = (short)0,
        PermitOnly = (short)5,
        RequestMinimum = (short)8,
        RequestOptional = (short)9,
        RequestRefuse = (short)10,
    }
    [System.FlagsAttribute]
    public enum ManifestResourceAttributes
    {
        Private = 2,
        Public = 1,
        VisibilityMask = 7,
    }
    [System.FlagsAttribute]
    public enum MethodImportAttributes : short
    {
        BestFitMappingDisable = (short)32,
        BestFitMappingEnable = (short)16,
        BestFitMappingMask = (short)48,
        CallingConventionCDecl = (short)512,
        CallingConventionFastCall = (short)1280,
        CallingConventionMask = (short)1792,
        CallingConventionStdCall = (short)768,
        CallingConventionThisCall = (short)1024,
        CallingConventionWinApi = (short)256,
        CharSetAnsi = (short)2,
        CharSetAuto = (short)6,
        CharSetMask = (short)6,
        CharSetUnicode = (short)4,
        ExactSpelling = (short)1,
        None = (short)0,
        SetLastError = (short)64,
        ThrowOnUnmappableCharDisable = (short)8192,
        ThrowOnUnmappableCharEnable = (short)4096,
        ThrowOnUnmappableCharMask = (short)12288,
    }
    [System.FlagsAttribute]
    public enum MethodSemanticsAttributes
    {
        Adder = 8,
        Getter = 2,
        Other = 4,
        Raiser = 32,
        Remover = 16,
        Setter = 1,
    }
}
namespace System.Reflection.Metadata
{
    public partial struct AssemblyDefinition
    {
        private object _dummy;
        public System.Reflection.Metadata.StringHandle Culture { get { throw null; } }
        public System.Reflection.AssemblyFlags Flags { get { throw null; } }
        public System.Reflection.AssemblyHashAlgorithm HashAlgorithm { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle PublicKey { get { throw null; } }
        public System.Version Version { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
        public System.Reflection.Metadata.DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes() { throw null; }
    }
    public partial struct AssemblyDefinitionHandle : System.IEquatable<System.Reflection.Metadata.AssemblyDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.AssemblyDefinitionHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.AssemblyDefinitionHandle left, System.Reflection.Metadata.AssemblyDefinitionHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.AssemblyDefinitionHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.AssemblyDefinitionHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.AssemblyDefinitionHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.AssemblyDefinitionHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.AssemblyDefinitionHandle left, System.Reflection.Metadata.AssemblyDefinitionHandle right) { throw null; }
    }
    public partial struct AssemblyFile
    {
        private object _dummy;
        private int _dummyPrimitive;
        public bool ContainsMetadata { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle HashValue { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct AssemblyFileHandle : System.IEquatable<System.Reflection.Metadata.AssemblyFileHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.AssemblyFileHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.AssemblyFileHandle left, System.Reflection.Metadata.AssemblyFileHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.AssemblyFileHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.AssemblyFileHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.AssemblyFileHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.AssemblyFileHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.AssemblyFileHandle left, System.Reflection.Metadata.AssemblyFileHandle right) { throw null; }
    }
    public partial struct AssemblyFileHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.AssemblyFileHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.AssemblyFileHandle>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.AssemblyFileHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.AssemblyFileHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.AssemblyFileHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.AssemblyFileHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.AssemblyFileHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct AssemblyReference
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.StringHandle Culture { get { throw null; } }
        public System.Reflection.AssemblyFlags Flags { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle HashValue { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle PublicKeyOrToken { get { throw null; } }
        public System.Version Version { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct AssemblyReferenceHandle : System.IEquatable<System.Reflection.Metadata.AssemblyReferenceHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.AssemblyReferenceHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.AssemblyReferenceHandle left, System.Reflection.Metadata.AssemblyReferenceHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.AssemblyReferenceHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.AssemblyReferenceHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.AssemblyReferenceHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.AssemblyReferenceHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.AssemblyReferenceHandle left, System.Reflection.Metadata.AssemblyReferenceHandle right) { throw null; }
    }
    public partial struct AssemblyReferenceHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.AssemblyReferenceHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.AssemblyReferenceHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.AssemblyReferenceHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.AssemblyReferenceHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.AssemblyReferenceHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.AssemblyReferenceHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.AssemblyReferenceHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct BlobHandle : System.IEquatable<System.Reflection.Metadata.BlobHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.BlobHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.BlobHandle left, System.Reflection.Metadata.BlobHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.BlobHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.BlobHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.BlobHandle left, System.Reflection.Metadata.BlobHandle right) { throw null; }
    }
    public partial struct BlobReader
    {
        private int _dummyPrimitive;
        public unsafe BlobReader(byte* buffer, int length) { throw null; }
        public int Length { get { throw null; } }
        public int Offset { get { throw null; } }
        public int RemainingBytes { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle ReadBlobHandle() { throw null; }
        public bool ReadBoolean() { throw null; }
        public byte ReadByte() { throw null; }
        public byte[] ReadBytes(int byteCount) { throw null; }
        public char ReadChar() { throw null; }
        public int ReadCompressedInteger() { throw null; }
        public int ReadCompressedSignedInteger() { throw null; }
        public object ReadConstant(System.Reflection.Metadata.ConstantTypeCode typeCode) { throw null; }
        public System.DateTime ReadDateTime() { throw null; }
        public decimal ReadDecimal() { throw null; }
        public double ReadDouble() { throw null; }
        public System.Guid ReadGuid() { throw null; }
        public short ReadInt16() { throw null; }
        public int ReadInt32() { throw null; }
        public long ReadInt64() { throw null; }
        public sbyte ReadSByte() { throw null; }
        public System.Reflection.Metadata.SerializationTypeCode ReadSerializationTypeCode() { throw null; }
        public string ReadSerializedString() { throw null; }
        public System.Reflection.Metadata.SignatureHeader ReadSignatureHeader() { throw null; }
        public System.Reflection.Metadata.SignatureTypeCode ReadSignatureTypeCode() { throw null; }
        public float ReadSingle() { throw null; }
        public System.Reflection.Metadata.EntityHandle ReadTypeHandle() { throw null; }
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
        public System.Reflection.Metadata.EntityHandle Parent { get { throw null; } }
        public System.Reflection.Metadata.ConstantTypeCode TypeCode { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle Value { get { throw null; } }
    }
    public partial struct ConstantHandle : System.IEquatable<System.Reflection.Metadata.ConstantHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.ConstantHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.ConstantHandle left, System.Reflection.Metadata.ConstantHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.ConstantHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.ConstantHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.ConstantHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.ConstantHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.ConstantHandle left, System.Reflection.Metadata.ConstantHandle right) { throw null; }
    }
    public enum ConstantTypeCode : byte
    {
        Boolean = (byte)2,
        Byte = (byte)5,
        Char = (byte)3,
        Double = (byte)13,
        Int16 = (byte)6,
        Int32 = (byte)8,
        Int64 = (byte)10,
        Invalid = (byte)0,
        NullReference = (byte)18,
        SByte = (byte)4,
        Single = (byte)12,
        String = (byte)14,
        UInt16 = (byte)7,
        UInt32 = (byte)9,
        UInt64 = (byte)11,
    }
    public partial struct CustomAttribute
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.EntityHandle Constructor { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle Parent { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle Value { get { throw null; } }
    }
    public partial struct CustomAttributeHandle : System.IEquatable<System.Reflection.Metadata.CustomAttributeHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.CustomAttributeHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.CustomAttributeHandle left, System.Reflection.Metadata.CustomAttributeHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.CustomAttributeHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.CustomAttributeHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.CustomAttributeHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.CustomAttributeHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.CustomAttributeHandle left, System.Reflection.Metadata.CustomAttributeHandle right) { throw null; }
    }
    public partial struct CustomAttributeHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.CustomAttributeHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.CustomAttributeHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.CustomAttributeHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.CustomAttributeHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.CustomAttributeHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.CustomAttributeHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public enum CustomAttributeNamedArgumentKind : byte
    {
        Field = (byte)83,
        Property = (byte)84,
    }
    public partial struct CustomDebugInformation
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.GuidHandle Kind { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle Parent { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle Value { get { throw null; } }
    }
    public partial struct CustomDebugInformationHandle : System.IEquatable<System.Reflection.Metadata.CustomDebugInformationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.CustomDebugInformationHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.CustomDebugInformationHandle left, System.Reflection.Metadata.CustomDebugInformationHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.CustomDebugInformationHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.CustomDebugInformationHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.CustomDebugInformationHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.CustomDebugInformationHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.CustomDebugInformationHandle left, System.Reflection.Metadata.CustomDebugInformationHandle right) { throw null; }
    }
    public partial struct CustomDebugInformationHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.CustomDebugInformationHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.CustomDebugInformationHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.CustomDebugInformationHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.CustomDebugInformationHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.CustomDebugInformationHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.CustomDebugInformationHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.CustomDebugInformationHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public sealed partial class DebugMetadataHeader
    {
        internal DebugMetadataHeader() { }
        public System.Reflection.Metadata.MethodDefinitionHandle EntryPoint { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<byte> Id { get { throw null; } }
    }
    public partial struct DeclarativeSecurityAttribute
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.DeclarativeSecurityAction Action { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle Parent { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle PermissionSet { get { throw null; } }
    }
    public partial struct DeclarativeSecurityAttributeHandle : System.IEquatable<System.Reflection.Metadata.DeclarativeSecurityAttributeHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.DeclarativeSecurityAttributeHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.DeclarativeSecurityAttributeHandle left, System.Reflection.Metadata.DeclarativeSecurityAttributeHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.DeclarativeSecurityAttributeHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.DeclarativeSecurityAttributeHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.DeclarativeSecurityAttributeHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.DeclarativeSecurityAttributeHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.DeclarativeSecurityAttributeHandle left, System.Reflection.Metadata.DeclarativeSecurityAttributeHandle right) { throw null; }
    }
    public partial struct DeclarativeSecurityAttributeHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.DeclarativeSecurityAttributeHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.DeclarativeSecurityAttributeHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.DeclarativeSecurityAttributeHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.DeclarativeSecurityAttributeHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.DeclarativeSecurityAttributeHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.DeclarativeSecurityAttributeHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.DeclarativeSecurityAttributeHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct Document
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.BlobHandle Hash { get { throw null; } }
        public System.Reflection.Metadata.GuidHandle HashAlgorithm { get { throw null; } }
        public System.Reflection.Metadata.GuidHandle Language { get { throw null; } }
        public System.Reflection.Metadata.DocumentNameBlobHandle Name { get { throw null; } }
    }
    public partial struct DocumentHandle : System.IEquatable<System.Reflection.Metadata.DocumentHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.DocumentHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.DocumentHandle left, System.Reflection.Metadata.DocumentHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.DocumentHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.DocumentHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.DocumentHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.DocumentHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.DocumentHandle left, System.Reflection.Metadata.DocumentHandle right) { throw null; }
    }
    public partial struct DocumentHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.DocumentHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.DocumentHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.DocumentHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.DocumentHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.DocumentHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.DocumentHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.DocumentHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct DocumentNameBlobHandle : System.IEquatable<System.Reflection.Metadata.DocumentNameBlobHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.DocumentNameBlobHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.DocumentNameBlobHandle left, System.Reflection.Metadata.DocumentNameBlobHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.DocumentNameBlobHandle (System.Reflection.Metadata.BlobHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.BlobHandle (System.Reflection.Metadata.DocumentNameBlobHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.DocumentNameBlobHandle left, System.Reflection.Metadata.DocumentNameBlobHandle right) { throw null; }
    }
    public partial struct EntityHandle : System.IEquatable<System.Reflection.Metadata.EntityHandle>
    {
        private int _dummyPrimitive;
        public static readonly System.Reflection.Metadata.AssemblyDefinitionHandle AssemblyDefinition;
        public static readonly System.Reflection.Metadata.ModuleDefinitionHandle ModuleDefinition;
        public bool IsNil { get { throw null; } }
        public System.Reflection.Metadata.HandleKind Kind { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.EntityHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.EntityHandle left, System.Reflection.Metadata.EntityHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.EntityHandle left, System.Reflection.Metadata.EntityHandle right) { throw null; }
    }
    public partial struct EventAccessors
    {
        private int _dummyPrimitive;
        public System.Reflection.Metadata.MethodDefinitionHandle Adder { get { throw null; } }
        public System.Reflection.Metadata.MethodDefinitionHandle Raiser { get { throw null; } }
        public System.Reflection.Metadata.MethodDefinitionHandle Remover { get { throw null; } }
    }
    public partial struct EventDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.EventAttributes Attributes { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle Type { get { throw null; } }
        public System.Reflection.Metadata.EventAccessors GetAccessors() { throw null; }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct EventDefinitionHandle : System.IEquatable<System.Reflection.Metadata.EventDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.EventDefinitionHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.EventDefinitionHandle left, System.Reflection.Metadata.EventDefinitionHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.EventDefinitionHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.EventDefinitionHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.EventDefinitionHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.EventDefinitionHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.EventDefinitionHandle left, System.Reflection.Metadata.EventDefinitionHandle right) { throw null; }
    }
    public partial struct EventDefinitionHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.EventDefinitionHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.EventDefinitionHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.EventDefinitionHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.EventDefinitionHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.EventDefinitionHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.EventDefinitionHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.EventDefinitionHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct ExceptionRegion
    {
        private int _dummyPrimitive;
        public System.Reflection.Metadata.EntityHandle CatchType { get { throw null; } }
        public int FilterOffset { get { throw null; } }
        public int HandlerLength { get { throw null; } }
        public int HandlerOffset { get { throw null; } }
        public System.Reflection.Metadata.ExceptionRegionKind Kind { get { throw null; } }
        public int TryLength { get { throw null; } }
        public int TryOffset { get { throw null; } }
    }
    public enum ExceptionRegionKind : ushort
    {
        Catch = (ushort)0,
        Fault = (ushort)4,
        Filter = (ushort)1,
        Finally = (ushort)2,
    }
    public partial struct ExportedType
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.TypeAttributes Attributes { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle Implementation { get { throw null; } }
        public bool IsForwarder { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Namespace { get { throw null; } }
        public System.Reflection.Metadata.NamespaceDefinitionHandle NamespaceDefinition { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct ExportedTypeHandle : System.IEquatable<System.Reflection.Metadata.ExportedTypeHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.ExportedTypeHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.ExportedTypeHandle left, System.Reflection.Metadata.ExportedTypeHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.ExportedTypeHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.ExportedTypeHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.ExportedTypeHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.ExportedTypeHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.ExportedTypeHandle left, System.Reflection.Metadata.ExportedTypeHandle right) { throw null; }
    }
    public partial struct ExportedTypeHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.ExportedTypeHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.ExportedTypeHandle>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.ExportedTypeHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.ExportedTypeHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.ExportedTypeHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.ExportedTypeHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.ExportedTypeHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct FieldDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.FieldAttributes Attributes { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle Signature { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
        public System.Reflection.Metadata.TypeDefinitionHandle GetDeclaringType() { throw null; }
        public System.Reflection.Metadata.ConstantHandle GetDefaultValue() { throw null; }
        public System.Reflection.Metadata.BlobHandle GetMarshallingDescriptor() { throw null; }
        public int GetOffset() { throw null; }
        public int GetRelativeVirtualAddress() { throw null; }
    }
    public partial struct FieldDefinitionHandle : System.IEquatable<System.Reflection.Metadata.FieldDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.FieldDefinitionHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.FieldDefinitionHandle left, System.Reflection.Metadata.FieldDefinitionHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.FieldDefinitionHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.FieldDefinitionHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.FieldDefinitionHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.FieldDefinitionHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.FieldDefinitionHandle left, System.Reflection.Metadata.FieldDefinitionHandle right) { throw null; }
    }
    public partial struct FieldDefinitionHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.FieldDefinitionHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.FieldDefinitionHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.FieldDefinitionHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.FieldDefinitionHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.FieldDefinitionHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.FieldDefinitionHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.FieldDefinitionHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct GenericParameter
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.GenericParameterAttributes Attributes { get { throw null; } }
        public int Index { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle Parent { get { throw null; } }
        public System.Reflection.Metadata.GenericParameterConstraintHandleCollection GetConstraints() { throw null; }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct GenericParameterConstraint
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.GenericParameterHandle Parameter { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle Type { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct GenericParameterConstraintHandle : System.IEquatable<System.Reflection.Metadata.GenericParameterConstraintHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.GenericParameterConstraintHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.GenericParameterConstraintHandle left, System.Reflection.Metadata.GenericParameterConstraintHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.GenericParameterConstraintHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.GenericParameterConstraintHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.GenericParameterConstraintHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.GenericParameterConstraintHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.GenericParameterConstraintHandle left, System.Reflection.Metadata.GenericParameterConstraintHandle right) { throw null; }
    }
    public partial struct GenericParameterConstraintHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.GenericParameterConstraintHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.GenericParameterConstraintHandle>, System.Collections.Generic.IReadOnlyList<System.Reflection.Metadata.GenericParameterConstraintHandle>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.GenericParameterConstraintHandle this[int index] { get { throw null; } }
        public System.Reflection.Metadata.GenericParameterConstraintHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.GenericParameterConstraintHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.GenericParameterConstraintHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.GenericParameterConstraintHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.GenericParameterConstraintHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct GenericParameterHandle : System.IEquatable<System.Reflection.Metadata.GenericParameterHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.GenericParameterHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.GenericParameterHandle left, System.Reflection.Metadata.GenericParameterHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.GenericParameterHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.GenericParameterHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.GenericParameterHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.GenericParameterHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.GenericParameterHandle left, System.Reflection.Metadata.GenericParameterHandle right) { throw null; }
    }
    public partial struct GenericParameterHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.GenericParameterHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.GenericParameterHandle>, System.Collections.Generic.IReadOnlyList<System.Reflection.Metadata.GenericParameterHandle>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.GenericParameterHandle this[int index] { get { throw null; } }
        public System.Reflection.Metadata.GenericParameterHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.GenericParameterHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.GenericParameterHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.GenericParameterHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.GenericParameterHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct GuidHandle : System.IEquatable<System.Reflection.Metadata.GuidHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.GuidHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.GuidHandle left, System.Reflection.Metadata.GuidHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.GuidHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.GuidHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.GuidHandle left, System.Reflection.Metadata.GuidHandle right) { throw null; }
    }
    public partial struct Handle : System.IEquatable<System.Reflection.Metadata.Handle>
    {
        private int _dummyPrimitive;
        public static readonly System.Reflection.Metadata.AssemblyDefinitionHandle AssemblyDefinition;
        public static readonly System.Reflection.Metadata.ModuleDefinitionHandle ModuleDefinition;
        public bool IsNil { get { throw null; } }
        public System.Reflection.Metadata.HandleKind Kind { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.Handle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.Handle left, System.Reflection.Metadata.Handle right) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.Handle left, System.Reflection.Metadata.Handle right) { throw null; }
    }
    public sealed partial class HandleComparer : System.Collections.Generic.IComparer<System.Reflection.Metadata.EntityHandle>, System.Collections.Generic.IComparer<System.Reflection.Metadata.Handle>, System.Collections.Generic.IEqualityComparer<System.Reflection.Metadata.EntityHandle>, System.Collections.Generic.IEqualityComparer<System.Reflection.Metadata.Handle>
    {
        internal HandleComparer() { }
        public static System.Reflection.Metadata.HandleComparer Default { get { throw null; } }
        public int Compare(System.Reflection.Metadata.EntityHandle x, System.Reflection.Metadata.EntityHandle y) { throw null; }
        public int Compare(System.Reflection.Metadata.Handle x, System.Reflection.Metadata.Handle y) { throw null; }
        public bool Equals(System.Reflection.Metadata.EntityHandle x, System.Reflection.Metadata.EntityHandle y) { throw null; }
        public bool Equals(System.Reflection.Metadata.Handle x, System.Reflection.Metadata.Handle y) { throw null; }
        public int GetHashCode(System.Reflection.Metadata.EntityHandle obj) { throw null; }
        public int GetHashCode(System.Reflection.Metadata.Handle obj) { throw null; }
    }
    public enum HandleKind : byte
    {
        AssemblyDefinition = (byte)32,
        AssemblyFile = (byte)38,
        AssemblyReference = (byte)35,
        Blob = (byte)113,
        Constant = (byte)11,
        CustomAttribute = (byte)12,
        CustomDebugInformation = (byte)55,
        DeclarativeSecurityAttribute = (byte)14,
        Document = (byte)48,
        EventDefinition = (byte)20,
        ExportedType = (byte)39,
        FieldDefinition = (byte)4,
        GenericParameter = (byte)42,
        GenericParameterConstraint = (byte)44,
        Guid = (byte)114,
        ImportScope = (byte)53,
        InterfaceImplementation = (byte)9,
        LocalConstant = (byte)52,
        LocalScope = (byte)50,
        LocalVariable = (byte)51,
        ManifestResource = (byte)40,
        MemberReference = (byte)10,
        MethodDebugInformation = (byte)49,
        MethodDefinition = (byte)6,
        MethodImplementation = (byte)25,
        MethodSpecification = (byte)43,
        ModuleDefinition = (byte)0,
        ModuleReference = (byte)26,
        NamespaceDefinition = (byte)124,
        Parameter = (byte)8,
        PropertyDefinition = (byte)23,
        StandaloneSignature = (byte)17,
        String = (byte)120,
        TypeDefinition = (byte)2,
        TypeReference = (byte)1,
        TypeSpecification = (byte)27,
        UserString = (byte)112,
    }
    public partial struct ImportDefinition
    {
        private int _dummyPrimitive;
        public System.Reflection.Metadata.BlobHandle Alias { get { throw null; } }
        public System.Reflection.Metadata.ImportDefinitionKind Kind { get { throw null; } }
        public System.Reflection.Metadata.AssemblyReferenceHandle TargetAssembly { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle TargetNamespace { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle TargetType { get { throw null; } }
    }
    public partial struct ImportDefinitionCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.ImportDefinition>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public System.Reflection.Metadata.ImportDefinitionCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.ImportDefinition> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.ImportDefinition>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.ImportDefinition>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.ImportDefinition Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            public void Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public enum ImportDefinitionKind
    {
        AliasAssemblyNamespace = 8,
        AliasAssemblyReference = 6,
        AliasNamespace = 7,
        AliasType = 9,
        ImportAssemblyNamespace = 2,
        ImportAssemblyReferenceAlias = 5,
        ImportNamespace = 1,
        ImportType = 3,
        ImportXmlNamespace = 4,
    }
    public partial struct ImportScope
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.BlobHandle ImportsBlob { get { throw null; } }
        public System.Reflection.Metadata.ImportScopeHandle Parent { get { throw null; } }
        public System.Reflection.Metadata.ImportDefinitionCollection GetImports() { throw null; }
    }
    public partial struct ImportScopeCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.ImportScopeHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.ImportScopeHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.ImportScopeCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.ImportScopeHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.ImportScopeHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.ImportScopeHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.ImportScopeHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct ImportScopeHandle : System.IEquatable<System.Reflection.Metadata.ImportScopeHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.ImportScopeHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.ImportScopeHandle left, System.Reflection.Metadata.ImportScopeHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.ImportScopeHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.ImportScopeHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.ImportScopeHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.ImportScopeHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.ImportScopeHandle left, System.Reflection.Metadata.ImportScopeHandle right) { throw null; }
    }
    public partial struct InterfaceImplementation
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.EntityHandle Interface { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct InterfaceImplementationHandle : System.IEquatable<System.Reflection.Metadata.InterfaceImplementationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.InterfaceImplementationHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.InterfaceImplementationHandle left, System.Reflection.Metadata.InterfaceImplementationHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.InterfaceImplementationHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.InterfaceImplementationHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.InterfaceImplementationHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.InterfaceImplementationHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.InterfaceImplementationHandle left, System.Reflection.Metadata.InterfaceImplementationHandle right) { throw null; }
    }
    public partial struct InterfaceImplementationHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.InterfaceImplementationHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.InterfaceImplementationHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.InterfaceImplementationHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.InterfaceImplementationHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.InterfaceImplementationHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.InterfaceImplementationHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.InterfaceImplementationHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct LocalConstant
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle Signature { get { throw null; } }
    }
    public partial struct LocalConstantHandle : System.IEquatable<System.Reflection.Metadata.LocalConstantHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.LocalConstantHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.LocalConstantHandle left, System.Reflection.Metadata.LocalConstantHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.LocalConstantHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.LocalConstantHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.LocalConstantHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.LocalConstantHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.LocalConstantHandle left, System.Reflection.Metadata.LocalConstantHandle right) { throw null; }
    }
    public partial struct LocalConstantHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.LocalConstantHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.LocalConstantHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.LocalConstantHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.LocalConstantHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.LocalConstantHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.LocalConstantHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.LocalConstantHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct LocalScope
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int EndOffset { get { throw null; } }
        public System.Reflection.Metadata.ImportScopeHandle ImportScope { get { throw null; } }
        public int Length { get { throw null; } }
        public System.Reflection.Metadata.MethodDefinitionHandle Method { get { throw null; } }
        public int StartOffset { get { throw null; } }
        public System.Reflection.Metadata.LocalScopeHandleCollection.ChildrenEnumerator GetChildren() { throw null; }
        public System.Reflection.Metadata.LocalConstantHandleCollection GetLocalConstants() { throw null; }
        public System.Reflection.Metadata.LocalVariableHandleCollection GetLocalVariables() { throw null; }
    }
    public partial struct LocalScopeHandle : System.IEquatable<System.Reflection.Metadata.LocalScopeHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.LocalScopeHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.LocalScopeHandle left, System.Reflection.Metadata.LocalScopeHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.LocalScopeHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.LocalScopeHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.LocalScopeHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.LocalScopeHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.LocalScopeHandle left, System.Reflection.Metadata.LocalScopeHandle right) { throw null; }
    }
    public partial struct LocalScopeHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.LocalScopeHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.LocalScopeHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.LocalScopeHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.LocalScopeHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.LocalScopeHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct ChildrenEnumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.LocalScopeHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.LocalScopeHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.LocalScopeHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.LocalScopeHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct LocalVariable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.LocalVariableAttributes Attributes { get { throw null; } }
        public int Index { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
    }
    [System.FlagsAttribute]
    public enum LocalVariableAttributes
    {
        DebuggerHidden = 1,
        None = 0,
    }
    public partial struct LocalVariableHandle : System.IEquatable<System.Reflection.Metadata.LocalVariableHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.LocalVariableHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.LocalVariableHandle left, System.Reflection.Metadata.LocalVariableHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.LocalVariableHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.LocalVariableHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.LocalVariableHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.LocalVariableHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.LocalVariableHandle left, System.Reflection.Metadata.LocalVariableHandle right) { throw null; }
    }
    public partial struct LocalVariableHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.LocalVariableHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.LocalVariableHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.LocalVariableHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.LocalVariableHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.LocalVariableHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.LocalVariableHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.LocalVariableHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct ManifestResource
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.ManifestResourceAttributes Attributes { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle Implementation { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public long Offset { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct ManifestResourceHandle : System.IEquatable<System.Reflection.Metadata.ManifestResourceHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.ManifestResourceHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.ManifestResourceHandle left, System.Reflection.Metadata.ManifestResourceHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.ManifestResourceHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.ManifestResourceHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.ManifestResourceHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.ManifestResourceHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.ManifestResourceHandle left, System.Reflection.Metadata.ManifestResourceHandle right) { throw null; }
    }
    public partial struct ManifestResourceHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.ManifestResourceHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.ManifestResourceHandle>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.ManifestResourceHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.ManifestResourceHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.ManifestResourceHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.ManifestResourceHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.ManifestResourceHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct MemberReference
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle Parent { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle Signature { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
        public System.Reflection.Metadata.MemberReferenceKind GetKind() { throw null; }
    }
    public partial struct MemberReferenceHandle : System.IEquatable<System.Reflection.Metadata.MemberReferenceHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.MemberReferenceHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.MemberReferenceHandle left, System.Reflection.Metadata.MemberReferenceHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.MemberReferenceHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.MemberReferenceHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.MemberReferenceHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.MemberReferenceHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.MemberReferenceHandle left, System.Reflection.Metadata.MemberReferenceHandle right) { throw null; }
    }
    public partial struct MemberReferenceHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.MemberReferenceHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.MemberReferenceHandle>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.MemberReferenceHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.MemberReferenceHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.MemberReferenceHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.MemberReferenceHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.MemberReferenceHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public enum MemberReferenceKind
    {
        Field = 1,
        Method = 0,
    }
    public enum MetadataKind
    {
        Ecma335 = 0,
        ManagedWindowsMetadata = 2,
        WindowsMetadata = 1,
    }
    public sealed partial class MetadataReader
    {
        public unsafe MetadataReader(byte* metadata, int length) { }
        public unsafe MetadataReader(byte* metadata, int length, System.Reflection.Metadata.MetadataReaderOptions options) { }
        public unsafe MetadataReader(byte* metadata, int length, System.Reflection.Metadata.MetadataReaderOptions options, System.Reflection.Metadata.MetadataStringDecoder utf8Decoder) { }
        public System.Reflection.Metadata.AssemblyFileHandleCollection AssemblyFiles { get { throw null; } }
        public System.Reflection.Metadata.AssemblyReferenceHandleCollection AssemblyReferences { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection CustomAttributes { get { throw null; } }
        public System.Reflection.Metadata.CustomDebugInformationHandleCollection CustomDebugInformation { get { throw null; } }
        public System.Reflection.Metadata.DebugMetadataHeader DebugMetadataHeader { get { throw null; } }
        public System.Reflection.Metadata.DeclarativeSecurityAttributeHandleCollection DeclarativeSecurityAttributes { get { throw null; } }
        public System.Reflection.Metadata.DocumentHandleCollection Documents { get { throw null; } }
        public System.Reflection.Metadata.EventDefinitionHandleCollection EventDefinitions { get { throw null; } }
        public System.Reflection.Metadata.ExportedTypeHandleCollection ExportedTypes { get { throw null; } }
        public System.Reflection.Metadata.FieldDefinitionHandleCollection FieldDefinitions { get { throw null; } }
        public System.Reflection.Metadata.ImportScopeCollection ImportScopes { get { throw null; } }
        public bool IsAssembly { get { throw null; } }
        public System.Reflection.Metadata.LocalConstantHandleCollection LocalConstants { get { throw null; } }
        public System.Reflection.Metadata.LocalScopeHandleCollection LocalScopes { get { throw null; } }
        public System.Reflection.Metadata.LocalVariableHandleCollection LocalVariables { get { throw null; } }
        public System.Reflection.Metadata.ManifestResourceHandleCollection ManifestResources { get { throw null; } }
        public System.Reflection.Metadata.MemberReferenceHandleCollection MemberReferences { get { throw null; } }
        public System.Reflection.Metadata.MetadataKind MetadataKind { get { throw null; } }
        public string MetadataVersion { get { throw null; } }
        public System.Reflection.Metadata.MethodDebugInformationHandleCollection MethodDebugInformation { get { throw null; } }
        public System.Reflection.Metadata.MethodDefinitionHandleCollection MethodDefinitions { get { throw null; } }
        public System.Reflection.Metadata.MetadataReaderOptions Options { get { throw null; } }
        public System.Reflection.Metadata.PropertyDefinitionHandleCollection PropertyDefinitions { get { throw null; } }
        public System.Reflection.Metadata.MetadataStringComparer StringComparer { get { throw null; } }
        public System.Reflection.Metadata.TypeDefinitionHandleCollection TypeDefinitions { get { throw null; } }
        public System.Reflection.Metadata.TypeReferenceHandleCollection TypeReferences { get { throw null; } }
        public System.Reflection.Metadata.AssemblyDefinition GetAssemblyDefinition() { throw null; }
        public System.Reflection.Metadata.AssemblyFile GetAssemblyFile(System.Reflection.Metadata.AssemblyFileHandle handle) { throw null; }
        public System.Reflection.Metadata.AssemblyReference GetAssemblyReference(System.Reflection.Metadata.AssemblyReferenceHandle handle) { throw null; }
        public byte[] GetBlobBytes(System.Reflection.Metadata.BlobHandle handle) { throw null; }
        public System.Collections.Immutable.ImmutableArray<byte> GetBlobContent(System.Reflection.Metadata.BlobHandle handle) { throw null; }
        public System.Reflection.Metadata.BlobReader GetBlobReader(System.Reflection.Metadata.BlobHandle handle) { throw null; }
        public System.Reflection.Metadata.Constant GetConstant(System.Reflection.Metadata.ConstantHandle handle) { throw null; }
        public System.Reflection.Metadata.CustomAttribute GetCustomAttribute(System.Reflection.Metadata.CustomAttributeHandle handle) { throw null; }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes(System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public System.Reflection.Metadata.CustomDebugInformation GetCustomDebugInformation(System.Reflection.Metadata.CustomDebugInformationHandle handle) { throw null; }
        public System.Reflection.Metadata.CustomDebugInformationHandleCollection GetCustomDebugInformation(System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public System.Reflection.Metadata.DeclarativeSecurityAttribute GetDeclarativeSecurityAttribute(System.Reflection.Metadata.DeclarativeSecurityAttributeHandle handle) { throw null; }
        public System.Reflection.Metadata.Document GetDocument(System.Reflection.Metadata.DocumentHandle handle) { throw null; }
        public System.Reflection.Metadata.EventDefinition GetEventDefinition(System.Reflection.Metadata.EventDefinitionHandle handle) { throw null; }
        public System.Reflection.Metadata.ExportedType GetExportedType(System.Reflection.Metadata.ExportedTypeHandle handle) { throw null; }
        public System.Reflection.Metadata.FieldDefinition GetFieldDefinition(System.Reflection.Metadata.FieldDefinitionHandle handle) { throw null; }
        public System.Reflection.Metadata.GenericParameter GetGenericParameter(System.Reflection.Metadata.GenericParameterHandle handle) { throw null; }
        public System.Reflection.Metadata.GenericParameterConstraint GetGenericParameterConstraint(System.Reflection.Metadata.GenericParameterConstraintHandle handle) { throw null; }
        public System.Guid GetGuid(System.Reflection.Metadata.GuidHandle handle) { throw null; }
        public System.Reflection.Metadata.ImportScope GetImportScope(System.Reflection.Metadata.ImportScopeHandle handle) { throw null; }
        public System.Reflection.Metadata.InterfaceImplementation GetInterfaceImplementation(System.Reflection.Metadata.InterfaceImplementationHandle handle) { throw null; }
        public System.Reflection.Metadata.LocalConstant GetLocalConstant(System.Reflection.Metadata.LocalConstantHandle handle) { throw null; }
        public System.Reflection.Metadata.LocalScope GetLocalScope(System.Reflection.Metadata.LocalScopeHandle handle) { throw null; }
        public System.Reflection.Metadata.LocalScopeHandleCollection GetLocalScopes(System.Reflection.Metadata.MethodDebugInformationHandle handle) { throw null; }
        public System.Reflection.Metadata.LocalScopeHandleCollection GetLocalScopes(System.Reflection.Metadata.MethodDefinitionHandle handle) { throw null; }
        public System.Reflection.Metadata.LocalVariable GetLocalVariable(System.Reflection.Metadata.LocalVariableHandle handle) { throw null; }
        public System.Reflection.Metadata.ManifestResource GetManifestResource(System.Reflection.Metadata.ManifestResourceHandle handle) { throw null; }
        public System.Reflection.Metadata.MemberReference GetMemberReference(System.Reflection.Metadata.MemberReferenceHandle handle) { throw null; }
        public System.Reflection.Metadata.MethodDebugInformation GetMethodDebugInformation(System.Reflection.Metadata.MethodDebugInformationHandle handle) { throw null; }
        public System.Reflection.Metadata.MethodDebugInformation GetMethodDebugInformation(System.Reflection.Metadata.MethodDefinitionHandle handle) { throw null; }
        public System.Reflection.Metadata.MethodDefinition GetMethodDefinition(System.Reflection.Metadata.MethodDefinitionHandle handle) { throw null; }
        public System.Reflection.Metadata.MethodImplementation GetMethodImplementation(System.Reflection.Metadata.MethodImplementationHandle handle) { throw null; }
        public System.Reflection.Metadata.MethodSpecification GetMethodSpecification(System.Reflection.Metadata.MethodSpecificationHandle handle) { throw null; }
        public System.Reflection.Metadata.ModuleDefinition GetModuleDefinition() { throw null; }
        public System.Reflection.Metadata.ModuleReference GetModuleReference(System.Reflection.Metadata.ModuleReferenceHandle handle) { throw null; }
        public System.Reflection.Metadata.NamespaceDefinition GetNamespaceDefinition(System.Reflection.Metadata.NamespaceDefinitionHandle handle) { throw null; }
        public System.Reflection.Metadata.NamespaceDefinition GetNamespaceDefinitionRoot() { throw null; }
        public System.Reflection.Metadata.Parameter GetParameter(System.Reflection.Metadata.ParameterHandle handle) { throw null; }
        public System.Reflection.Metadata.PropertyDefinition GetPropertyDefinition(System.Reflection.Metadata.PropertyDefinitionHandle handle) { throw null; }
        public System.Reflection.Metadata.StandaloneSignature GetStandaloneSignature(System.Reflection.Metadata.StandaloneSignatureHandle handle) { throw null; }
        public string GetString(System.Reflection.Metadata.DocumentNameBlobHandle handle) { throw null; }
        public string GetString(System.Reflection.Metadata.NamespaceDefinitionHandle handle) { throw null; }
        public string GetString(System.Reflection.Metadata.StringHandle handle) { throw null; }
        public System.Reflection.Metadata.TypeDefinition GetTypeDefinition(System.Reflection.Metadata.TypeDefinitionHandle handle) { throw null; }
        public System.Reflection.Metadata.TypeReference GetTypeReference(System.Reflection.Metadata.TypeReferenceHandle handle) { throw null; }
        public System.Reflection.Metadata.TypeSpecification GetTypeSpecification(System.Reflection.Metadata.TypeSpecificationHandle handle) { throw null; }
        public string GetUserString(System.Reflection.Metadata.UserStringHandle handle) { throw null; }
    }
    [System.FlagsAttribute]
    public enum MetadataReaderOptions
    {
        ApplyWindowsRuntimeProjections = 1,
        Default = 1,
        None = 0,
    }
    public sealed partial class MetadataReaderProvider : System.IDisposable
    {
        internal MetadataReaderProvider() { }
        public void Dispose() { }
        public unsafe static System.Reflection.Metadata.MetadataReaderProvider FromMetadataImage(byte* start, int size) { throw null; }
        public static System.Reflection.Metadata.MetadataReaderProvider FromMetadataImage(System.Collections.Immutable.ImmutableArray<byte> image) { throw null; }
        public static System.Reflection.Metadata.MetadataReaderProvider FromMetadataStream(System.IO.Stream stream, System.Reflection.Metadata.MetadataStreamOptions options = System.Reflection.Metadata.MetadataStreamOptions.Default, int size = 0) { throw null; }
        public unsafe static System.Reflection.Metadata.MetadataReaderProvider FromPortablePdbImage(byte* start, int size) { throw null; }
        public static System.Reflection.Metadata.MetadataReaderProvider FromPortablePdbImage(System.Collections.Immutable.ImmutableArray<byte> image) { throw null; }
        public static System.Reflection.Metadata.MetadataReaderProvider FromPortablePdbStream(System.IO.Stream stream, System.Reflection.Metadata.MetadataStreamOptions options = System.Reflection.Metadata.MetadataStreamOptions.Default, int size = 0) { throw null; }
        public System.Reflection.Metadata.MetadataReader GetMetadataReader(System.Reflection.Metadata.MetadataReaderOptions options = System.Reflection.Metadata.MetadataReaderOptions.Default, System.Reflection.Metadata.MetadataStringDecoder utf8Decoder = null) { throw null; }
    }
    [System.FlagsAttribute]
    public enum MetadataStreamOptions
    {
        Default = 0,
        LeaveOpen = 1,
        PrefetchMetadata = 2,
    }
    public partial struct MetadataStringComparer
    {
        private object _dummy;
        public bool Equals(System.Reflection.Metadata.DocumentNameBlobHandle handle, string value) { throw null; }
        public bool Equals(System.Reflection.Metadata.DocumentNameBlobHandle handle, string value, bool ignoreCase) { throw null; }
        public bool Equals(System.Reflection.Metadata.NamespaceDefinitionHandle handle, string value) { throw null; }
        public bool Equals(System.Reflection.Metadata.NamespaceDefinitionHandle handle, string value, bool ignoreCase) { throw null; }
        public bool Equals(System.Reflection.Metadata.StringHandle handle, string value) { throw null; }
        public bool Equals(System.Reflection.Metadata.StringHandle handle, string value, bool ignoreCase) { throw null; }
        public bool StartsWith(System.Reflection.Metadata.StringHandle handle, string value) { throw null; }
        public bool StartsWith(System.Reflection.Metadata.StringHandle handle, string value, bool ignoreCase) { throw null; }
    }
    public partial class MetadataStringDecoder
    {
        public MetadataStringDecoder(System.Text.Encoding encoding) { }
        public static System.Reflection.Metadata.MetadataStringDecoder DefaultUTF8 { get { throw null; } }
        public System.Text.Encoding Encoding { get { throw null; } }
        public unsafe virtual string GetString(byte* bytes, int byteCount) { throw null; }
    }
    public sealed partial class MethodBodyBlock
    {
        internal MethodBodyBlock() { }
        public System.Collections.Immutable.ImmutableArray<System.Reflection.Metadata.ExceptionRegion> ExceptionRegions { get { throw null; } }
        public System.Reflection.Metadata.StandaloneSignatureHandle LocalSignature { get { throw null; } }
        public bool LocalVariablesInitialized { get { throw null; } }
        public int MaxStack { get { throw null; } }
        public int Size { get { throw null; } }
        public static System.Reflection.Metadata.MethodBodyBlock Create(System.Reflection.Metadata.BlobReader reader) { throw null; }
        public byte[] GetILBytes() { throw null; }
        public System.Collections.Immutable.ImmutableArray<byte> GetILContent() { throw null; }
        public System.Reflection.Metadata.BlobReader GetILReader() { throw null; }
    }
    public partial struct MethodDebugInformation
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.DocumentHandle Document { get { throw null; } }
        public System.Reflection.Metadata.StandaloneSignatureHandle LocalSignature { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle SequencePointsBlob { get { throw null; } }
        public System.Reflection.Metadata.SequencePointCollection GetSequencePoints() { throw null; }
        public System.Reflection.Metadata.MethodDefinitionHandle GetStateMachineKickoffMethod() { throw null; }
    }
    public partial struct MethodDebugInformationHandle : System.IEquatable<System.Reflection.Metadata.MethodDebugInformationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.MethodDebugInformationHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.MethodDebugInformationHandle left, System.Reflection.Metadata.MethodDebugInformationHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.MethodDebugInformationHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.MethodDebugInformationHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.MethodDebugInformationHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.MethodDebugInformationHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.MethodDebugInformationHandle left, System.Reflection.Metadata.MethodDebugInformationHandle right) { throw null; }
        public System.Reflection.Metadata.MethodDefinitionHandle ToDefinitionHandle() { throw null; }
    }
    public partial struct MethodDebugInformationHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.MethodDebugInformationHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.MethodDebugInformationHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.MethodDebugInformationHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.MethodDebugInformationHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.MethodDebugInformationHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.MethodDebugInformationHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.MethodDebugInformationHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct MethodDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.MethodAttributes Attributes { get { throw null; } }
        public System.Reflection.MethodImplAttributes ImplAttributes { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public int RelativeVirtualAddress { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle Signature { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
        public System.Reflection.Metadata.DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes() { throw null; }
        public System.Reflection.Metadata.TypeDefinitionHandle GetDeclaringType() { throw null; }
        public System.Reflection.Metadata.GenericParameterHandleCollection GetGenericParameters() { throw null; }
        public System.Reflection.Metadata.MethodImport GetImport() { throw null; }
        public System.Reflection.Metadata.ParameterHandleCollection GetParameters() { throw null; }
    }
    public partial struct MethodDefinitionHandle : System.IEquatable<System.Reflection.Metadata.MethodDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.MethodDefinitionHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.MethodDefinitionHandle left, System.Reflection.Metadata.MethodDefinitionHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.MethodDefinitionHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.MethodDefinitionHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.MethodDefinitionHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.MethodDefinitionHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.MethodDefinitionHandle left, System.Reflection.Metadata.MethodDefinitionHandle right) { throw null; }
        public System.Reflection.Metadata.MethodDebugInformationHandle ToDebugInformationHandle() { throw null; }
    }
    public partial struct MethodDefinitionHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.MethodDefinitionHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.MethodDefinitionHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.MethodDefinitionHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.MethodDefinitionHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.MethodDefinitionHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.MethodDefinitionHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.MethodDefinitionHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct MethodImplementation
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.EntityHandle MethodBody { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle MethodDeclaration { get { throw null; } }
        public System.Reflection.Metadata.TypeDefinitionHandle Type { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct MethodImplementationHandle : System.IEquatable<System.Reflection.Metadata.MethodImplementationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.MethodImplementationHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.MethodImplementationHandle left, System.Reflection.Metadata.MethodImplementationHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.MethodImplementationHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.MethodImplementationHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.MethodImplementationHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.MethodImplementationHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.MethodImplementationHandle left, System.Reflection.Metadata.MethodImplementationHandle right) { throw null; }
    }
    public partial struct MethodImplementationHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.MethodImplementationHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.MethodImplementationHandle>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.MethodImplementationHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.MethodImplementationHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.MethodImplementationHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.MethodImplementationHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.MethodImplementationHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct MethodImport
    {
        private int _dummyPrimitive;
        public System.Reflection.MethodImportAttributes Attributes { get { throw null; } }
        public System.Reflection.Metadata.ModuleReferenceHandle Module { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
    }
    public partial struct MethodSpecification
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.EntityHandle Method { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle Signature { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct MethodSpecificationHandle : System.IEquatable<System.Reflection.Metadata.MethodSpecificationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.MethodSpecificationHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.MethodSpecificationHandle left, System.Reflection.Metadata.MethodSpecificationHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.MethodSpecificationHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.MethodSpecificationHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.MethodSpecificationHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.MethodSpecificationHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.MethodSpecificationHandle left, System.Reflection.Metadata.MethodSpecificationHandle right) { throw null; }
    }
    public partial struct ModuleDefinition
    {
        private object _dummy;
        public System.Reflection.Metadata.GuidHandle BaseGenerationId { get { throw null; } }
        public int Generation { get { throw null; } }
        public System.Reflection.Metadata.GuidHandle GenerationId { get { throw null; } }
        public System.Reflection.Metadata.GuidHandle Mvid { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
    }
    public partial struct ModuleDefinitionHandle : System.IEquatable<System.Reflection.Metadata.ModuleDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.ModuleDefinitionHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.ModuleDefinitionHandle left, System.Reflection.Metadata.ModuleDefinitionHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.ModuleDefinitionHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.ModuleDefinitionHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.ModuleDefinitionHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.ModuleDefinitionHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.ModuleDefinitionHandle left, System.Reflection.Metadata.ModuleDefinitionHandle right) { throw null; }
    }
    public partial struct ModuleReference
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct ModuleReferenceHandle : System.IEquatable<System.Reflection.Metadata.ModuleReferenceHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.ModuleReferenceHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.ModuleReferenceHandle left, System.Reflection.Metadata.ModuleReferenceHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.ModuleReferenceHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.ModuleReferenceHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.ModuleReferenceHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.ModuleReferenceHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.ModuleReferenceHandle left, System.Reflection.Metadata.ModuleReferenceHandle right) { throw null; }
    }
    public partial struct NamespaceDefinition
    {
        private object _dummy;
        public System.Collections.Immutable.ImmutableArray<System.Reflection.Metadata.ExportedTypeHandle> ExportedTypes { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<System.Reflection.Metadata.NamespaceDefinitionHandle> NamespaceDefinitions { get { throw null; } }
        public System.Reflection.Metadata.NamespaceDefinitionHandle Parent { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<System.Reflection.Metadata.TypeDefinitionHandle> TypeDefinitions { get { throw null; } }
    }
    public partial struct NamespaceDefinitionHandle : System.IEquatable<System.Reflection.Metadata.NamespaceDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.NamespaceDefinitionHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.NamespaceDefinitionHandle left, System.Reflection.Metadata.NamespaceDefinitionHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.NamespaceDefinitionHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.NamespaceDefinitionHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.NamespaceDefinitionHandle left, System.Reflection.Metadata.NamespaceDefinitionHandle right) { throw null; }
    }
    public partial struct Parameter
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.ParameterAttributes Attributes { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public int SequenceNumber { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
        public System.Reflection.Metadata.ConstantHandle GetDefaultValue() { throw null; }
        public System.Reflection.Metadata.BlobHandle GetMarshallingDescriptor() { throw null; }
    }
    public partial struct ParameterHandle : System.IEquatable<System.Reflection.Metadata.ParameterHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.ParameterHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.ParameterHandle left, System.Reflection.Metadata.ParameterHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.ParameterHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.ParameterHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.ParameterHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.ParameterHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.ParameterHandle left, System.Reflection.Metadata.ParameterHandle right) { throw null; }
    }
    public partial struct ParameterHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.ParameterHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.ParameterHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.ParameterHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.ParameterHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.ParameterHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.ParameterHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.ParameterHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public static partial class PEReaderExtensions
    {
        public static System.Reflection.Metadata.MetadataReader GetMetadataReader(this System.Reflection.PortableExecutable.PEReader peReader) { throw null; }
        public static System.Reflection.Metadata.MetadataReader GetMetadataReader(this System.Reflection.PortableExecutable.PEReader peReader, System.Reflection.Metadata.MetadataReaderOptions options) { throw null; }
        public static System.Reflection.Metadata.MetadataReader GetMetadataReader(this System.Reflection.PortableExecutable.PEReader peReader, System.Reflection.Metadata.MetadataReaderOptions options, System.Reflection.Metadata.MetadataStringDecoder utf8Decoder) { throw null; }
        public static System.Reflection.Metadata.MethodBodyBlock GetMethodBody(this System.Reflection.PortableExecutable.PEReader peReader, int relativeVirtualAddress) { throw null; }
    }
    public partial struct PropertyAccessors
    {
        private int _dummyPrimitive;
        public System.Reflection.Metadata.MethodDefinitionHandle Getter { get { throw null; } }
        public System.Reflection.Metadata.MethodDefinitionHandle Setter { get { throw null; } }
    }
    public partial struct PropertyDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.PropertyAttributes Attributes { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.BlobHandle Signature { get { throw null; } }
        public System.Reflection.Metadata.PropertyAccessors GetAccessors() { throw null; }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
        public System.Reflection.Metadata.ConstantHandle GetDefaultValue() { throw null; }
    }
    public partial struct PropertyDefinitionHandle : System.IEquatable<System.Reflection.Metadata.PropertyDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.PropertyDefinitionHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.PropertyDefinitionHandle left, System.Reflection.Metadata.PropertyDefinitionHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.PropertyDefinitionHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.PropertyDefinitionHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.PropertyDefinitionHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.PropertyDefinitionHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.PropertyDefinitionHandle left, System.Reflection.Metadata.PropertyDefinitionHandle right) { throw null; }
    }
    public partial struct PropertyDefinitionHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.PropertyDefinitionHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.PropertyDefinitionHandle>, System.Collections.IEnumerable
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.PropertyDefinitionHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.PropertyDefinitionHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.PropertyDefinitionHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.PropertyDefinitionHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public System.Reflection.Metadata.PropertyDefinitionHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct SequencePoint : System.IEquatable<System.Reflection.Metadata.SequencePoint>
    {
        private int _dummyPrimitive;
        public const int HiddenLine = 16707566;
        public System.Reflection.Metadata.DocumentHandle Document { get { throw null; } }
        public int EndColumn { get { throw null; } }
        public int EndLine { get { throw null; } }
        public bool IsHidden { get { throw null; } }
        public int Offset { get { throw null; } }
        public int StartColumn { get { throw null; } }
        public int StartLine { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.SequencePoint other) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public partial struct SequencePointCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.SequencePoint>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public System.Reflection.Metadata.SequencePointCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.SequencePoint> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.SequencePoint>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.SequencePoint>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.SequencePoint Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            public void Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public enum SerializationTypeCode : byte
    {
        Boolean = (byte)2,
        Byte = (byte)5,
        Char = (byte)3,
        Double = (byte)13,
        Enum = (byte)85,
        Int16 = (byte)6,
        Int32 = (byte)8,
        Int64 = (byte)10,
        Invalid = (byte)0,
        SByte = (byte)4,
        Single = (byte)12,
        String = (byte)14,
        SZArray = (byte)29,
        TaggedObject = (byte)81,
        Type = (byte)80,
        UInt16 = (byte)7,
        UInt32 = (byte)9,
        UInt64 = (byte)11,
    }
    [System.FlagsAttribute]
    public enum SignatureAttributes : byte
    {
        ExplicitThis = (byte)64,
        Generic = (byte)16,
        Instance = (byte)32,
        None = (byte)0,
    }
    public enum SignatureCallingConvention : byte
    {
        CDecl = (byte)1,
        Default = (byte)0,
        FastCall = (byte)4,
        StdCall = (byte)2,
        ThisCall = (byte)3,
        VarArgs = (byte)5,
    }
    public partial struct SignatureHeader : System.IEquatable<System.Reflection.Metadata.SignatureHeader>
    {
        private int _dummyPrimitive;
        public const byte CallingConventionOrKindMask = (byte)15;
        public SignatureHeader(byte rawValue) { throw null; }
        public SignatureHeader(System.Reflection.Metadata.SignatureKind kind, System.Reflection.Metadata.SignatureCallingConvention convention, System.Reflection.Metadata.SignatureAttributes attributes) { throw null; }
        public System.Reflection.Metadata.SignatureAttributes Attributes { get { throw null; } }
        public System.Reflection.Metadata.SignatureCallingConvention CallingConvention { get { throw null; } }
        public bool HasExplicitThis { get { throw null; } }
        public bool IsGeneric { get { throw null; } }
        public bool IsInstance { get { throw null; } }
        public System.Reflection.Metadata.SignatureKind Kind { get { throw null; } }
        public byte RawValue { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.SignatureHeader other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.SignatureHeader left, System.Reflection.Metadata.SignatureHeader right) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.SignatureHeader left, System.Reflection.Metadata.SignatureHeader right) { throw null; }
        public override string ToString() { throw null; }
    }
    public enum SignatureKind : byte
    {
        Field = (byte)6,
        LocalVariables = (byte)7,
        Method = (byte)0,
        MethodSpecification = (byte)10,
        Property = (byte)8,
    }
    public enum SignatureTypeCode : byte
    {
        Array = (byte)20,
        Boolean = (byte)2,
        ByReference = (byte)16,
        Byte = (byte)5,
        Char = (byte)3,
        Double = (byte)13,
        FunctionPointer = (byte)27,
        GenericMethodParameter = (byte)30,
        GenericTypeInstance = (byte)21,
        GenericTypeParameter = (byte)19,
        Int16 = (byte)6,
        Int32 = (byte)8,
        Int64 = (byte)10,
        IntPtr = (byte)24,
        Invalid = (byte)0,
        Object = (byte)28,
        OptionalModifier = (byte)32,
        Pinned = (byte)69,
        Pointer = (byte)15,
        RequiredModifier = (byte)31,
        SByte = (byte)4,
        Sentinel = (byte)65,
        Single = (byte)12,
        String = (byte)14,
        SZArray = (byte)29,
        TypedReference = (byte)22,
        TypeHandle = (byte)64,
        UInt16 = (byte)7,
        UInt32 = (byte)9,
        UInt64 = (byte)11,
        UIntPtr = (byte)25,
        Void = (byte)1,
    }
    public partial struct StandaloneSignature
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.BlobHandle Signature { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
        public System.Reflection.Metadata.StandaloneSignatureKind GetKind() { throw null; }
    }
    public partial struct StandaloneSignatureHandle : System.IEquatable<System.Reflection.Metadata.StandaloneSignatureHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.StandaloneSignatureHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.StandaloneSignatureHandle left, System.Reflection.Metadata.StandaloneSignatureHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.StandaloneSignatureHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.StandaloneSignatureHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.StandaloneSignatureHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.StandaloneSignatureHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.StandaloneSignatureHandle left, System.Reflection.Metadata.StandaloneSignatureHandle right) { throw null; }
    }
    public enum StandaloneSignatureKind
    {
        LocalVariables = 1,
        Method = 0,
    }
    public partial struct StringHandle : System.IEquatable<System.Reflection.Metadata.StringHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.StringHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.StringHandle left, System.Reflection.Metadata.StringHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.StringHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.StringHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.StringHandle left, System.Reflection.Metadata.StringHandle right) { throw null; }
    }
    public partial struct TypeDefinition
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.TypeAttributes Attributes { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle BaseType { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Namespace { get { throw null; } }
        public System.Reflection.Metadata.NamespaceDefinitionHandle NamespaceDefinition { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
        public System.Reflection.Metadata.DeclarativeSecurityAttributeHandleCollection GetDeclarativeSecurityAttributes() { throw null; }
        public System.Reflection.Metadata.TypeDefinitionHandle GetDeclaringType() { throw null; }
        public System.Reflection.Metadata.EventDefinitionHandleCollection GetEvents() { throw null; }
        public System.Reflection.Metadata.FieldDefinitionHandleCollection GetFields() { throw null; }
        public System.Reflection.Metadata.GenericParameterHandleCollection GetGenericParameters() { throw null; }
        public System.Reflection.Metadata.InterfaceImplementationHandleCollection GetInterfaceImplementations() { throw null; }
        public System.Reflection.Metadata.TypeLayout GetLayout() { throw null; }
        public System.Reflection.Metadata.MethodImplementationHandleCollection GetMethodImplementations() { throw null; }
        public System.Reflection.Metadata.MethodDefinitionHandleCollection GetMethods() { throw null; }
        public System.Collections.Immutable.ImmutableArray<System.Reflection.Metadata.TypeDefinitionHandle> GetNestedTypes() { throw null; }
        public System.Reflection.Metadata.PropertyDefinitionHandleCollection GetProperties() { throw null; }
    }
    public partial struct TypeDefinitionHandle : System.IEquatable<System.Reflection.Metadata.TypeDefinitionHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.TypeDefinitionHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.TypeDefinitionHandle left, System.Reflection.Metadata.TypeDefinitionHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.TypeDefinitionHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.TypeDefinitionHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.TypeDefinitionHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.TypeDefinitionHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.TypeDefinitionHandle left, System.Reflection.Metadata.TypeDefinitionHandle right) { throw null; }
    }
    public partial struct TypeDefinitionHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.TypeDefinitionHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.TypeDefinitionHandle>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.TypeDefinitionHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.TypeDefinitionHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.TypeDefinitionHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.TypeDefinitionHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.TypeDefinitionHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct TypeLayout
    {
        private int _dummyPrimitive;
        public TypeLayout(int size, int packingSize) { throw null; }
        public bool IsDefault { get { throw null; } }
        public int PackingSize { get { throw null; } }
        public int Size { get { throw null; } }
    }
    public partial struct TypeReference
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.StringHandle Name { get { throw null; } }
        public System.Reflection.Metadata.StringHandle Namespace { get { throw null; } }
        public System.Reflection.Metadata.EntityHandle ResolutionScope { get { throw null; } }
    }
    public partial struct TypeReferenceHandle : System.IEquatable<System.Reflection.Metadata.TypeReferenceHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.TypeReferenceHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.TypeReferenceHandle left, System.Reflection.Metadata.TypeReferenceHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.TypeReferenceHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.TypeReferenceHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.TypeReferenceHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.TypeReferenceHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.TypeReferenceHandle left, System.Reflection.Metadata.TypeReferenceHandle right) { throw null; }
    }
    public partial struct TypeReferenceHandleCollection : System.Collections.Generic.IEnumerable<System.Reflection.Metadata.TypeReferenceHandle>, System.Collections.Generic.IReadOnlyCollection<System.Reflection.Metadata.TypeReferenceHandle>, System.Collections.IEnumerable
    {
        private int _dummyPrimitive;
        public int Count { get { throw null; } }
        public System.Reflection.Metadata.TypeReferenceHandleCollection.Enumerator GetEnumerator() { throw null; }
        System.Collections.Generic.IEnumerator<System.Reflection.Metadata.TypeReferenceHandle> System.Collections.Generic.IEnumerable<System.Reflection.Metadata.TypeReferenceHandle>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<System.Reflection.Metadata.TypeReferenceHandle>, System.Collections.IEnumerator, System.IDisposable
        {
            private int _dummyPrimitive;
            public System.Reflection.Metadata.TypeReferenceHandle Current { get { throw null; } }
            object System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            void System.Collections.IEnumerator.Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
    public partial struct TypeSpecification
    {
        private object _dummy;
        private int _dummyPrimitive;
        public System.Reflection.Metadata.BlobHandle Signature { get { throw null; } }
        public System.Reflection.Metadata.CustomAttributeHandleCollection GetCustomAttributes() { throw null; }
    }
    public partial struct TypeSpecificationHandle : System.IEquatable<System.Reflection.Metadata.TypeSpecificationHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.TypeSpecificationHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.TypeSpecificationHandle left, System.Reflection.Metadata.TypeSpecificationHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.TypeSpecificationHandle (System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static explicit operator System.Reflection.Metadata.TypeSpecificationHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.EntityHandle (System.Reflection.Metadata.TypeSpecificationHandle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.TypeSpecificationHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.TypeSpecificationHandle left, System.Reflection.Metadata.TypeSpecificationHandle right) { throw null; }
    }
    public partial struct UserStringHandle : System.IEquatable<System.Reflection.Metadata.UserStringHandle>
    {
        private int _dummyPrimitive;
        public bool IsNil { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.UserStringHandle other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Metadata.UserStringHandle left, System.Reflection.Metadata.UserStringHandle right) { throw null; }
        public static explicit operator System.Reflection.Metadata.UserStringHandle (System.Reflection.Metadata.Handle handle) { throw null; }
        public static implicit operator System.Reflection.Metadata.Handle (System.Reflection.Metadata.UserStringHandle handle) { throw null; }
        public static bool operator !=(System.Reflection.Metadata.UserStringHandle left, System.Reflection.Metadata.UserStringHandle right) { throw null; }
    }
}
namespace System.Reflection.Metadata.Ecma335
{
    public partial struct EditAndContinueLogEntry : System.IEquatable<System.Reflection.Metadata.Ecma335.EditAndContinueLogEntry>
    {
        private int _dummyPrimitive;
        public EditAndContinueLogEntry(System.Reflection.Metadata.EntityHandle handle, System.Reflection.Metadata.Ecma335.EditAndContinueOperation operation) { throw null; }
        public System.Reflection.Metadata.EntityHandle Handle { get { throw null; } }
        public System.Reflection.Metadata.Ecma335.EditAndContinueOperation Operation { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Metadata.Ecma335.EditAndContinueLogEntry other) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public enum EditAndContinueOperation
    {
        AddEvent = 5,
        AddField = 2,
        AddMethod = 1,
        AddParameter = 3,
        AddProperty = 4,
        Default = 0,
    }
    public static partial class ExportedTypeExtensions
    {
        public static int GetTypeDefinitionId(this System.Reflection.Metadata.ExportedType exportedType) { throw null; }
    }
    public enum HeapIndex
    {
        Blob = 2,
        Guid = 3,
        String = 1,
        UserString = 0,
    }
    public sealed partial class MetadataAggregator
    {
        public MetadataAggregator(System.Collections.Generic.IReadOnlyList<int> baseTableRowCounts, System.Collections.Generic.IReadOnlyList<int> baseHeapSizes, System.Collections.Generic.IReadOnlyList<System.Reflection.Metadata.MetadataReader> deltaReaders) { }
        public MetadataAggregator(System.Reflection.Metadata.MetadataReader baseReader, System.Collections.Generic.IReadOnlyList<System.Reflection.Metadata.MetadataReader> deltaReaders) { }
        public System.Reflection.Metadata.Handle GetGenerationHandle(System.Reflection.Metadata.Handle handle, out int generation) { throw null; }
    }
    public static partial class MetadataReaderExtensions
    {
        public static System.Collections.Generic.IEnumerable<System.Reflection.Metadata.Ecma335.EditAndContinueLogEntry> GetEditAndContinueLogEntries(this System.Reflection.Metadata.MetadataReader reader) { throw null; }
        public static System.Collections.Generic.IEnumerable<System.Reflection.Metadata.EntityHandle> GetEditAndContinueMapEntries(this System.Reflection.Metadata.MetadataReader reader) { throw null; }
        public static int GetHeapMetadataOffset(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.Ecma335.HeapIndex heapIndex) { throw null; }
        public static int GetHeapSize(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.Ecma335.HeapIndex heapIndex) { throw null; }
        public static System.Reflection.Metadata.BlobHandle GetNextHandle(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.BlobHandle handle) { throw null; }
        public static System.Reflection.Metadata.StringHandle GetNextHandle(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.StringHandle handle) { throw null; }
        public static System.Reflection.Metadata.UserStringHandle GetNextHandle(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.UserStringHandle handle) { throw null; }
        public static int GetTableMetadataOffset(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.Ecma335.TableIndex tableIndex) { throw null; }
        public static int GetTableRowCount(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.Ecma335.TableIndex tableIndex) { throw null; }
        public static int GetTableRowSize(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.Ecma335.TableIndex tableIndex) { throw null; }
        public static System.Collections.Generic.IEnumerable<System.Reflection.Metadata.TypeDefinitionHandle> GetTypesWithEvents(this System.Reflection.Metadata.MetadataReader reader) { throw null; }
        public static System.Collections.Generic.IEnumerable<System.Reflection.Metadata.TypeDefinitionHandle> GetTypesWithProperties(this System.Reflection.Metadata.MetadataReader reader) { throw null; }
    }
    public static partial class MetadataTokens
    {
        public static readonly int HeapCount;
        public static readonly int TableCount;
        public static System.Reflection.Metadata.AssemblyFileHandle AssemblyFileHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.AssemblyReferenceHandle AssemblyReferenceHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.BlobHandle BlobHandle(int offset) { throw null; }
        public static System.Reflection.Metadata.ConstantHandle ConstantHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.CustomAttributeHandle CustomAttributeHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.CustomDebugInformationHandle CustomDebugInformationHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.DeclarativeSecurityAttributeHandle DeclarativeSecurityAttributeHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.DocumentHandle DocumentHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.DocumentNameBlobHandle DocumentNameBlobHandle(int offset) { throw null; }
        public static System.Reflection.Metadata.EntityHandle EntityHandle(int token) { throw null; }
        public static System.Reflection.Metadata.EntityHandle EntityHandle(System.Reflection.Metadata.Ecma335.TableIndex tableIndex, int rowNumber) { throw null; }
        public static System.Reflection.Metadata.EventDefinitionHandle EventDefinitionHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.ExportedTypeHandle ExportedTypeHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.FieldDefinitionHandle FieldDefinitionHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.GenericParameterConstraintHandle GenericParameterConstraintHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.GenericParameterHandle GenericParameterHandle(int rowNumber) { throw null; }
        public static int GetHeapOffset(System.Reflection.Metadata.Handle handle) { throw null; }
        public static int GetHeapOffset(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.Handle handle) { throw null; }
        public static int GetRowNumber(System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static int GetRowNumber(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static int GetToken(System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static int GetToken(System.Reflection.Metadata.Handle handle) { throw null; }
        public static int GetToken(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.EntityHandle handle) { throw null; }
        public static int GetToken(this System.Reflection.Metadata.MetadataReader reader, System.Reflection.Metadata.Handle handle) { throw null; }
        public static System.Reflection.Metadata.GuidHandle GuidHandle(int offset) { throw null; }
        public static System.Reflection.Metadata.Handle Handle(int token) { throw null; }
        public static System.Reflection.Metadata.EntityHandle Handle(System.Reflection.Metadata.Ecma335.TableIndex tableIndex, int rowNumber) { throw null; }
        public static System.Reflection.Metadata.ImportScopeHandle ImportScopeHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.InterfaceImplementationHandle InterfaceImplementationHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.LocalConstantHandle LocalConstantHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.LocalScopeHandle LocalScopeHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.LocalVariableHandle LocalVariableHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.ManifestResourceHandle ManifestResourceHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.MemberReferenceHandle MemberReferenceHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.MethodDebugInformationHandle MethodDebugInformationHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.MethodDefinitionHandle MethodDefinitionHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.MethodImplementationHandle MethodImplementationHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.MethodSpecificationHandle MethodSpecificationHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.ModuleReferenceHandle ModuleReferenceHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.ParameterHandle ParameterHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.PropertyDefinitionHandle PropertyDefinitionHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.StandaloneSignatureHandle StandaloneSignatureHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.StringHandle StringHandle(int offset) { throw null; }
        public static bool TryGetHeapIndex(System.Reflection.Metadata.HandleKind type, out System.Reflection.Metadata.Ecma335.HeapIndex index) { throw null; }
        public static bool TryGetTableIndex(System.Reflection.Metadata.HandleKind type, out System.Reflection.Metadata.Ecma335.TableIndex index) { throw null; }
        public static System.Reflection.Metadata.TypeDefinitionHandle TypeDefinitionHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.TypeReferenceHandle TypeReferenceHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.TypeSpecificationHandle TypeSpecificationHandle(int rowNumber) { throw null; }
        public static System.Reflection.Metadata.UserStringHandle UserStringHandle(int offset) { throw null; }
    }
    public enum TableIndex : byte
    {
        Assembly = (byte)32,
        AssemblyOS = (byte)34,
        AssemblyProcessor = (byte)33,
        AssemblyRef = (byte)35,
        AssemblyRefOS = (byte)37,
        AssemblyRefProcessor = (byte)36,
        ClassLayout = (byte)15,
        Constant = (byte)11,
        CustomAttribute = (byte)12,
        CustomDebugInformation = (byte)55,
        DeclSecurity = (byte)14,
        Document = (byte)48,
        EncLog = (byte)30,
        EncMap = (byte)31,
        Event = (byte)20,
        EventMap = (byte)18,
        EventPtr = (byte)19,
        ExportedType = (byte)39,
        Field = (byte)4,
        FieldLayout = (byte)16,
        FieldMarshal = (byte)13,
        FieldPtr = (byte)3,
        FieldRva = (byte)29,
        File = (byte)38,
        GenericParam = (byte)42,
        GenericParamConstraint = (byte)44,
        ImplMap = (byte)28,
        ImportScope = (byte)53,
        InterfaceImpl = (byte)9,
        LocalConstant = (byte)52,
        LocalScope = (byte)50,
        LocalVariable = (byte)51,
        ManifestResource = (byte)40,
        MemberRef = (byte)10,
        MethodDebugInformation = (byte)49,
        MethodDef = (byte)6,
        MethodImpl = (byte)25,
        MethodPtr = (byte)5,
        MethodSemantics = (byte)24,
        MethodSpec = (byte)43,
        Module = (byte)0,
        ModuleRef = (byte)26,
        NestedClass = (byte)41,
        Param = (byte)8,
        ParamPtr = (byte)7,
        Property = (byte)23,
        PropertyMap = (byte)21,
        PropertyPtr = (byte)22,
        StandAloneSig = (byte)17,
        StateMachineMethod = (byte)54,
        TypeDef = (byte)2,
        TypeRef = (byte)1,
        TypeSpec = (byte)27,
    }
}
namespace System.Reflection.PortableExecutable
{
    public enum Characteristics : ushort
    {
        AggressiveWSTrim = (ushort)16,
        Bit32Machine = (ushort)256,
        BytesReversedHi = (ushort)32768,
        BytesReversedLo = (ushort)128,
        DebugStripped = (ushort)512,
        Dll = (ushort)8192,
        ExecutableImage = (ushort)2,
        LargeAddressAware = (ushort)32,
        LineNumsStripped = (ushort)4,
        LocalSymsStripped = (ushort)8,
        NetRunFromSwap = (ushort)2048,
        RelocsStripped = (ushort)1,
        RemovableRunFromSwap = (ushort)1024,
        System = (ushort)4096,
        UpSystemOnly = (ushort)16384,
    }
    public partial struct CodeViewDebugDirectoryData
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Age { get { throw null; } }
        public System.Guid Guid { get { throw null; } }
        public string Path { get { throw null; } }
    }
    public sealed partial class CoffHeader
    {
        internal CoffHeader() { }
        public System.Reflection.PortableExecutable.Characteristics Characteristics { get { throw null; } }
        public System.Reflection.PortableExecutable.Machine Machine { get { throw null; } }
        public short NumberOfSections { get { throw null; } }
        public int NumberOfSymbols { get { throw null; } }
        public int PointerToSymbolTable { get { throw null; } }
        public short SizeOfOptionalHeader { get { throw null; } }
        public int TimeDateStamp { get { throw null; } }
    }
    [System.FlagsAttribute]
    public enum CorFlags
    {
        ILLibrary = 4,
        ILOnly = 1,
        NativeEntryPoint = 16,
        Prefers32Bit = 131072,
        Requires32Bit = 2,
        StrongNameSigned = 8,
        TrackDebugData = 65536,
    }
    public sealed partial class CorHeader
    {
        internal CorHeader() { }
        public System.Reflection.PortableExecutable.DirectoryEntry CodeManagerTableDirectory { get { throw null; } }
        public int EntryPointTokenOrRelativeVirtualAddress { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry ExportAddressTableJumpsDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.CorFlags Flags { get { throw null; } }
        public ushort MajorRuntimeVersion { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry ManagedNativeHeaderDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry MetadataDirectory { get { throw null; } }
        public ushort MinorRuntimeVersion { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry ResourcesDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry StrongNameSignatureDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry VtableFixupsDirectory { get { throw null; } }
    }
    public partial struct DebugDirectoryEntry
    {
        private int _dummyPrimitive;
        public DebugDirectoryEntry(uint stamp, ushort majorVersion, ushort minorVersion, System.Reflection.PortableExecutable.DebugDirectoryEntryType type, int dataSize, int dataRelativeVirtualAddress, int dataPointer) { throw null; }
        public int DataPointer { get { throw null; } }
        public int DataRelativeVirtualAddress { get { throw null; } }
        public int DataSize { get { throw null; } }
        public ushort MajorVersion { get { throw null; } }
        public ushort MinorVersion { get { throw null; } }
        public uint Stamp { get { throw null; } }
        public System.Reflection.PortableExecutable.DebugDirectoryEntryType Type { get { throw null; } }
    }
    public enum DebugDirectoryEntryType
    {
        CodeView = 2,
        Coff = 1,
        Reproducible = 16,
        Unknown = 0,
    }
    public partial struct DirectoryEntry
    {
        public readonly int RelativeVirtualAddress;
        public readonly int Size;
        public DirectoryEntry(int relativeVirtualAddress, int size) { throw null; }
    }
    [System.FlagsAttribute]
    public enum DllCharacteristics : ushort
    {
        AppContainer = (ushort)4096,
        DynamicBase = (ushort)64,
        HighEntropyVirtualAddressSpace = (ushort)32,
        NoBind = (ushort)2048,
        NoIsolation = (ushort)512,
        NoSeh = (ushort)1024,
        NxCompatible = (ushort)256,
        ProcessInit = (ushort)1,
        ProcessTerm = (ushort)2,
        TerminalServerAware = (ushort)32768,
        ThreadInit = (ushort)4,
        ThreadTerm = (ushort)8,
        WdmDriver = (ushort)8192,
    }
    public enum Machine : ushort
    {
        Alpha = (ushort)388,
        Alpha64 = (ushort)644,
        AM33 = (ushort)467,
        Amd64 = (ushort)34404,
        Arm = (ushort)448,
        ArmThumb2 = (ushort)452,
        Ebc = (ushort)3772,
        I386 = (ushort)332,
        IA64 = (ushort)512,
        M32R = (ushort)36929,
        MIPS16 = (ushort)614,
        MipsFpu = (ushort)870,
        MipsFpu16 = (ushort)1126,
        PowerPC = (ushort)496,
        PowerPCFP = (ushort)497,
        SH3 = (ushort)418,
        SH3Dsp = (ushort)419,
        SH3E = (ushort)420,
        SH4 = (ushort)422,
        SH5 = (ushort)424,
        Thumb = (ushort)450,
        Tricore = (ushort)1312,
        Unknown = (ushort)0,
        WceMipsV2 = (ushort)361,
    }
    public sealed partial class PEHeader
    {
        internal PEHeader() { }
        public int AddressOfEntryPoint { get { throw null; } }
        public int BaseOfCode { get { throw null; } }
        public int BaseOfData { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry BaseRelocationTableDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry BoundImportTableDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry CertificateTableDirectory { get { throw null; } }
        public uint CheckSum { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry CopyrightTableDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry CorHeaderTableDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry DebugTableDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry DelayImportTableDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DllCharacteristics DllCharacteristics { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry ExceptionTableDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry ExportTableDirectory { get { throw null; } }
        public int FileAlignment { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry GlobalPointerTableDirectory { get { throw null; } }
        public ulong ImageBase { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry ImportAddressTableDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry ImportTableDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry LoadConfigTableDirectory { get { throw null; } }
        public System.Reflection.PortableExecutable.PEMagic Magic { get { throw null; } }
        public ushort MajorImageVersion { get { throw null; } }
        public byte MajorLinkerVersion { get { throw null; } }
        public ushort MajorOperatingSystemVersion { get { throw null; } }
        public ushort MajorSubsystemVersion { get { throw null; } }
        public ushort MinorImageVersion { get { throw null; } }
        public byte MinorLinkerVersion { get { throw null; } }
        public ushort MinorOperatingSystemVersion { get { throw null; } }
        public ushort MinorSubsystemVersion { get { throw null; } }
        public int NumberOfRvaAndSizes { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry ResourceTableDirectory { get { throw null; } }
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
        public System.Reflection.PortableExecutable.Subsystem Subsystem { get { throw null; } }
        public System.Reflection.PortableExecutable.DirectoryEntry ThreadLocalStorageTableDirectory { get { throw null; } }
    }
    public sealed partial class PEHeaders
    {
        public PEHeaders(System.IO.Stream peStream) { }
        public PEHeaders(System.IO.Stream peStream, int size) { }
        public System.Reflection.PortableExecutable.CoffHeader CoffHeader { get { throw null; } }
        public int CoffHeaderStartOffset { get { throw null; } }
        public System.Reflection.PortableExecutable.CorHeader CorHeader { get { throw null; } }
        public int CorHeaderStartOffset { get { throw null; } }
        public bool IsCoffOnly { get { throw null; } }
        public bool IsConsoleApplication { get { throw null; } }
        public bool IsDll { get { throw null; } }
        public bool IsExe { get { throw null; } }
        public int MetadataSize { get { throw null; } }
        public int MetadataStartOffset { get { throw null; } }
        public System.Reflection.PortableExecutable.PEHeader PEHeader { get { throw null; } }
        public int PEHeaderStartOffset { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<System.Reflection.PortableExecutable.SectionHeader> SectionHeaders { get { throw null; } }
        public int GetContainingSectionIndex(int relativeVirtualAddress) { throw null; }
        public bool TryGetDirectoryOffset(System.Reflection.PortableExecutable.DirectoryEntry directory, out int offset) { throw null; }
    }
    public enum PEMagic : ushort
    {
        PE32 = (ushort)267,
        PE32Plus = (ushort)523,
    }
    public partial struct PEMemoryBlock
    {
        private object _dummy;
        private int _dummyPrimitive;
        public int Length { get { throw null; } }
        public unsafe byte* Pointer { get { throw null; } }
        public System.Collections.Immutable.ImmutableArray<byte> GetContent() { throw null; }
    }
    public sealed partial class PEReader : System.IDisposable
    {
        public unsafe PEReader(byte* peImage, int size) { }
        public PEReader(System.Collections.Immutable.ImmutableArray<byte> peImage) { }
        public PEReader(System.IO.Stream peStream) { }
        public PEReader(System.IO.Stream peStream, System.Reflection.PortableExecutable.PEStreamOptions options) { }
        public PEReader(System.IO.Stream peStream, System.Reflection.PortableExecutable.PEStreamOptions options, int size) { }
        public bool HasMetadata { get { throw null; } }
        public bool IsEntireImageAvailable { get { throw null; } }
        public System.Reflection.PortableExecutable.PEHeaders PEHeaders { get { throw null; } }
        public void Dispose() { }
        public System.Reflection.PortableExecutable.PEMemoryBlock GetEntireImage() { throw null; }
        public System.Reflection.PortableExecutable.PEMemoryBlock GetMetadata() { throw null; }
        public System.Reflection.PortableExecutable.PEMemoryBlock GetSectionData(int relativeVirtualAddress) { throw null; }
        public System.Reflection.PortableExecutable.CodeViewDebugDirectoryData ReadCodeViewDebugDirectoryData(System.Reflection.PortableExecutable.DebugDirectoryEntry entry) { throw null; }
        public System.Collections.Immutable.ImmutableArray<System.Reflection.PortableExecutable.DebugDirectoryEntry> ReadDebugDirectory() { throw null; }
    }
    [System.FlagsAttribute]
    public enum PEStreamOptions
    {
        Default = 0,
        LeaveOpen = 1,
        PrefetchEntireImage = 4,
        PrefetchMetadata = 2,
    }
    [System.FlagsAttribute]
    public enum SectionCharacteristics : uint
    {
        Align1024Bytes = (uint)11534336,
        Align128Bytes = (uint)8388608,
        Align16Bytes = (uint)5242880,
        Align1Bytes = (uint)1048576,
        Align2048Bytes = (uint)12582912,
        Align256Bytes = (uint)9437184,
        Align2Bytes = (uint)2097152,
        Align32Bytes = (uint)6291456,
        Align4096Bytes = (uint)13631488,
        Align4Bytes = (uint)3145728,
        Align512Bytes = (uint)10485760,
        Align64Bytes = (uint)7340032,
        Align8192Bytes = (uint)14680064,
        Align8Bytes = (uint)4194304,
        AlignMask = (uint)15728640,
        ContainsCode = (uint)32,
        ContainsInitializedData = (uint)64,
        ContainsUninitializedData = (uint)128,
        GPRel = (uint)32768,
        LinkerComdat = (uint)4096,
        LinkerInfo = (uint)512,
        LinkerNRelocOvfl = (uint)16777216,
        LinkerOther = (uint)256,
        LinkerRemove = (uint)2048,
        Mem16Bit = (uint)131072,
        MemDiscardable = (uint)33554432,
        MemExecute = (uint)536870912,
        MemFardata = (uint)32768,
        MemLocked = (uint)262144,
        MemNotCached = (uint)67108864,
        MemNotPaged = (uint)134217728,
        MemPreload = (uint)524288,
        MemProtected = (uint)16384,
        MemPurgeable = (uint)131072,
        MemRead = (uint)1073741824,
        MemShared = (uint)268435456,
        MemSysheap = (uint)65536,
        MemWrite = (uint)2147483648,
        NoDeferSpecExc = (uint)16384,
        TypeCopy = (uint)16,
        TypeDSect = (uint)1,
        TypeGroup = (uint)4,
        TypeNoLoad = (uint)2,
        TypeNoPad = (uint)8,
        TypeOver = (uint)1024,
        TypeReg = (uint)0,
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
        public System.Reflection.PortableExecutable.SectionCharacteristics SectionCharacteristics { get { throw null; } }
        public int SizeOfRawData { get { throw null; } }
        public int VirtualAddress { get { throw null; } }
        public int VirtualSize { get { throw null; } }
    }
    public enum Subsystem : ushort
    {
        EfiApplication = (ushort)10,
        EfiBootServiceDriver = (ushort)11,
        EfiRom = (ushort)13,
        EfiRuntimeDriver = (ushort)12,
        Native = (ushort)1,
        NativeWindows = (ushort)8,
        OS2Cui = (ushort)5,
        PosixCui = (ushort)7,
        Unknown = (ushort)0,
        WindowsCEGui = (ushort)9,
        WindowsCui = (ushort)3,
        WindowsGui = (ushort)2,
        Xbox = (ushort)14,
    }
}
