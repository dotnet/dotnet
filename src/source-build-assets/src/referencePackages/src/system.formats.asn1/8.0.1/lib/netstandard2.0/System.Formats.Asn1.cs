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
[assembly: System.Reflection.AssemblyDefaultAlias("System.Formats.Asn1")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("IsTrimmable", "True")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides classes that can read and write the ASN.1 BER, CER, and DER data formats.\r\n\r\nCommonly Used Types:\r\nSystem.Formats.Asn1.AsnReader\r\nSystem.Formats.Asn1.AsnWriter")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.724.31311")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.7+2aade6beb02ea367fd97c4070a4198802fe61c03")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Formats.Asn1")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Formats.Asn1
{
    public readonly partial struct Asn1Tag : IEquatable<Asn1Tag>
    {
        private readonly int _dummyPrimitive;
        public static readonly Asn1Tag Boolean;
        public static readonly Asn1Tag ConstructedBitString;
        public static readonly Asn1Tag ConstructedOctetString;
        public static readonly Asn1Tag Enumerated;
        public static readonly Asn1Tag GeneralizedTime;
        public static readonly Asn1Tag Integer;
        public static readonly Asn1Tag Null;
        public static readonly Asn1Tag ObjectIdentifier;
        public static readonly Asn1Tag PrimitiveBitString;
        public static readonly Asn1Tag PrimitiveOctetString;
        public static readonly Asn1Tag Sequence;
        public static readonly Asn1Tag SetOf;
        public static readonly Asn1Tag UtcTime;
        public Asn1Tag(TagClass tagClass, int tagValue, bool isConstructed = false) { }

        public Asn1Tag(UniversalTagNumber universalTagNumber, bool isConstructed = false) { }

        public bool IsConstructed { get { throw null; } }

        public TagClass TagClass { get { throw null; } }

        public int TagValue { get { throw null; } }

        public readonly Asn1Tag AsConstructed() { throw null; }

        public readonly Asn1Tag AsPrimitive() { throw null; }

        public readonly int CalculateEncodedSize() { throw null; }

        public static Asn1Tag Decode(ReadOnlySpan<byte> source, out int bytesConsumed) { throw null; }

        public readonly int Encode(Span<byte> destination) { throw null; }

        public readonly bool Equals(Asn1Tag other) { throw null; }

        public override readonly bool Equals(object? obj) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public readonly bool HasSameClassAndValue(Asn1Tag other) { throw null; }

        public static bool operator ==(Asn1Tag left, Asn1Tag right) { throw null; }

        public static bool operator !=(Asn1Tag left, Asn1Tag right) { throw null; }

        public override readonly string ToString() { throw null; }

        public static bool TryDecode(ReadOnlySpan<byte> source, out Asn1Tag tag, out int bytesConsumed) { throw null; }

        public readonly bool TryEncode(Span<byte> destination, out int bytesWritten) { throw null; }
    }

    public partial class AsnContentException : Exception
    {
        public AsnContentException() { }

        protected AsnContentException(Runtime.Serialization.SerializationInfo info, Runtime.Serialization.StreamingContext context) { }

        public AsnContentException(string? message, Exception? inner) { }

        public AsnContentException(string? message) { }
    }

    public static partial class AsnDecoder
    {
        public static byte[] ReadBitString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int unusedBitCount, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static bool ReadBoolean(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static string ReadCharacterString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, UniversalTagNumber encodingType, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static Asn1Tag ReadEncodedValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int contentOffset, out int contentLength, out int bytesConsumed) { throw null; }

        public static ReadOnlySpan<byte> ReadEnumeratedBytes(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static Enum ReadEnumeratedValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Type enumType, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static TEnum ReadEnumeratedValue<TEnum>(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
            where TEnum : Enum { throw null; }

        public static DateTimeOffset ReadGeneralizedTime(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static Numerics.BigInteger ReadInteger(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static ReadOnlySpan<byte> ReadIntegerBytes(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static Collections.BitArray ReadNamedBitList(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static Enum ReadNamedBitListValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Type flagsEnumType, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static TFlagsEnum ReadNamedBitListValue<TFlagsEnum>(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null)
            where TFlagsEnum : Enum { throw null; }

        public static void ReadNull(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static string ReadObjectIdentifier(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static byte[] ReadOctetString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static void ReadSequence(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int contentOffset, out int contentLength, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static void ReadSetOf(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int contentOffset, out int contentLength, out int bytesConsumed, bool skipSortOrderValidation = false, Asn1Tag? expectedTag = null) { throw null; }

        public static DateTimeOffset ReadUtcTime(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int bytesConsumed, int twoDigitYearMax = 2049, Asn1Tag? expectedTag = null) { throw null; }

        public static bool TryReadBitString(ReadOnlySpan<byte> source, Span<byte> destination, AsnEncodingRules ruleSet, out int unusedBitCount, out int bytesConsumed, out int bytesWritten, Asn1Tag? expectedTag = null) { throw null; }

        public static bool TryReadCharacterString(ReadOnlySpan<byte> source, Span<char> destination, AsnEncodingRules ruleSet, UniversalTagNumber encodingType, out int bytesConsumed, out int charsWritten, Asn1Tag? expectedTag = null) { throw null; }

        public static bool TryReadCharacterStringBytes(ReadOnlySpan<byte> source, Span<byte> destination, AsnEncodingRules ruleSet, Asn1Tag expectedTag, out int bytesConsumed, out int bytesWritten) { throw null; }

        public static bool TryReadEncodedValue(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out Asn1Tag tag, out int contentOffset, out int contentLength, out int bytesConsumed) { throw null; }

        public static bool TryReadInt32(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int value, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static bool TryReadInt64(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out long value, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static bool TryReadOctetString(ReadOnlySpan<byte> source, Span<byte> destination, AsnEncodingRules ruleSet, out int bytesConsumed, out int bytesWritten, Asn1Tag? expectedTag = null) { throw null; }

        public static bool TryReadPrimitiveBitString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out int unusedBitCount, out ReadOnlySpan<byte> value, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        public static bool TryReadPrimitiveCharacterStringBytes(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, Asn1Tag expectedTag, out ReadOnlySpan<byte> value, out int bytesConsumed) { throw null; }

        public static bool TryReadPrimitiveOctetString(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out ReadOnlySpan<byte> value, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        [CLSCompliant(false)]
        public static bool TryReadUInt32(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out uint value, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }

        [CLSCompliant(false)]
        public static bool TryReadUInt64(ReadOnlySpan<byte> source, AsnEncodingRules ruleSet, out ulong value, out int bytesConsumed, Asn1Tag? expectedTag = null) { throw null; }
    }

    public enum AsnEncodingRules
    {
        BER = 0,
        CER = 1,
        DER = 2
    }

    public partial class AsnReader
    {
        public AsnReader(ReadOnlyMemory<byte> data, AsnEncodingRules ruleSet, AsnReaderOptions options = default) { }

        public bool HasData { get { throw null; } }

        public AsnEncodingRules RuleSet { get { throw null; } }

        public AsnReader Clone() { throw null; }

        public ReadOnlyMemory<byte> PeekContentBytes() { throw null; }

        public ReadOnlyMemory<byte> PeekEncodedValue() { throw null; }

        public Asn1Tag PeekTag() { throw null; }

        public byte[] ReadBitString(out int unusedBitCount, Asn1Tag? expectedTag = null) { throw null; }

        public bool ReadBoolean(Asn1Tag? expectedTag = null) { throw null; }

        public string ReadCharacterString(UniversalTagNumber encodingType, Asn1Tag? expectedTag = null) { throw null; }

        public ReadOnlyMemory<byte> ReadEncodedValue() { throw null; }

        public ReadOnlyMemory<byte> ReadEnumeratedBytes(Asn1Tag? expectedTag = null) { throw null; }

        public Enum ReadEnumeratedValue(Type enumType, Asn1Tag? expectedTag = null) { throw null; }

        public TEnum ReadEnumeratedValue<TEnum>(Asn1Tag? expectedTag = null)
            where TEnum : Enum { throw null; }

        public DateTimeOffset ReadGeneralizedTime(Asn1Tag? expectedTag = null) { throw null; }

        public Numerics.BigInteger ReadInteger(Asn1Tag? expectedTag = null) { throw null; }

        public ReadOnlyMemory<byte> ReadIntegerBytes(Asn1Tag? expectedTag = null) { throw null; }

        public Collections.BitArray ReadNamedBitList(Asn1Tag? expectedTag = null) { throw null; }

        public Enum ReadNamedBitListValue(Type flagsEnumType, Asn1Tag? expectedTag = null) { throw null; }

        public TFlagsEnum ReadNamedBitListValue<TFlagsEnum>(Asn1Tag? expectedTag = null)
            where TFlagsEnum : Enum { throw null; }

        public void ReadNull(Asn1Tag? expectedTag = null) { }

        public string ReadObjectIdentifier(Asn1Tag? expectedTag = null) { throw null; }

        public byte[] ReadOctetString(Asn1Tag? expectedTag = null) { throw null; }

        public AsnReader ReadSequence(Asn1Tag? expectedTag = null) { throw null; }

        public AsnReader ReadSetOf(bool skipSortOrderValidation, Asn1Tag? expectedTag = null) { throw null; }

        public AsnReader ReadSetOf(Asn1Tag? expectedTag = null) { throw null; }

        public DateTimeOffset ReadUtcTime(int twoDigitYearMax, Asn1Tag? expectedTag = null) { throw null; }

        public DateTimeOffset ReadUtcTime(Asn1Tag? expectedTag = null) { throw null; }

        public void ThrowIfNotEmpty() { }

        public bool TryReadBitString(Span<byte> destination, out int unusedBitCount, out int bytesWritten, Asn1Tag? expectedTag = null) { throw null; }

        public bool TryReadCharacterString(Span<char> destination, UniversalTagNumber encodingType, out int charsWritten, Asn1Tag? expectedTag = null) { throw null; }

        public bool TryReadCharacterStringBytes(Span<byte> destination, Asn1Tag expectedTag, out int bytesWritten) { throw null; }

        public bool TryReadInt32(out int value, Asn1Tag? expectedTag = null) { throw null; }

        public bool TryReadInt64(out long value, Asn1Tag? expectedTag = null) { throw null; }

        public bool TryReadOctetString(Span<byte> destination, out int bytesWritten, Asn1Tag? expectedTag = null) { throw null; }

        public bool TryReadPrimitiveBitString(out int unusedBitCount, out ReadOnlyMemory<byte> value, Asn1Tag? expectedTag = null) { throw null; }

        public bool TryReadPrimitiveCharacterStringBytes(Asn1Tag expectedTag, out ReadOnlyMemory<byte> contents) { throw null; }

        public bool TryReadPrimitiveOctetString(out ReadOnlyMemory<byte> contents, Asn1Tag? expectedTag = null) { throw null; }

        [CLSCompliant(false)]
        public bool TryReadUInt32(out uint value, Asn1Tag? expectedTag = null) { throw null; }

        [CLSCompliant(false)]
        public bool TryReadUInt64(out ulong value, Asn1Tag? expectedTag = null) { throw null; }
    }

    public partial struct AsnReaderOptions
    {
        private int _dummyPrimitive;
        public bool SkipSetSortOrderVerification { get { throw null; } set { } }

        public int UtcTimeTwoDigitYearMax { get { throw null; } set { } }
    }

    public sealed partial class AsnWriter
    {
        public AsnWriter(AsnEncodingRules ruleSet, int initialCapacity) { }

        public AsnWriter(AsnEncodingRules ruleSet) { }

        public AsnEncodingRules RuleSet { get { throw null; } }

        public void CopyTo(AsnWriter destination) { }

        public byte[] Encode() { throw null; }

        public int Encode(Span<byte> destination) { throw null; }

        public bool EncodedValueEquals(AsnWriter other) { throw null; }

        public bool EncodedValueEquals(ReadOnlySpan<byte> other) { throw null; }

        public int GetEncodedLength() { throw null; }

        public void PopOctetString(Asn1Tag? tag = null) { }

        public void PopSequence(Asn1Tag? tag = null) { }

        public void PopSetOf(Asn1Tag? tag = null) { }

        public Scope PushOctetString(Asn1Tag? tag = null) { throw null; }

        public Scope PushSequence(Asn1Tag? tag = null) { throw null; }

        public Scope PushSetOf(Asn1Tag? tag = null) { throw null; }

        public void Reset() { }

        public bool TryEncode(Span<byte> destination, out int bytesWritten) { throw null; }

        public void WriteBitString(ReadOnlySpan<byte> value, int unusedBitCount = 0, Asn1Tag? tag = null) { }

        public void WriteBoolean(bool value, Asn1Tag? tag = null) { }

        public void WriteCharacterString(UniversalTagNumber encodingType, ReadOnlySpan<char> str, Asn1Tag? tag = null) { }

        public void WriteCharacterString(UniversalTagNumber encodingType, string value, Asn1Tag? tag = null) { }

        public void WriteEncodedValue(ReadOnlySpan<byte> value) { }

        public void WriteEnumeratedValue(Enum value, Asn1Tag? tag = null) { }

        public void WriteEnumeratedValue<TEnum>(TEnum value, Asn1Tag? tag = null)
            where TEnum : Enum { }

        public void WriteGeneralizedTime(DateTimeOffset value, bool omitFractionalSeconds = false, Asn1Tag? tag = null) { }

        public void WriteInteger(long value, Asn1Tag? tag = null) { }

        public void WriteInteger(Numerics.BigInteger value, Asn1Tag? tag = null) { }

        public void WriteInteger(ReadOnlySpan<byte> value, Asn1Tag? tag = null) { }

        [CLSCompliant(false)]
        public void WriteInteger(ulong value, Asn1Tag? tag = null) { }

        public void WriteIntegerUnsigned(ReadOnlySpan<byte> value, Asn1Tag? tag = null) { }

        public void WriteNamedBitList(Collections.BitArray value, Asn1Tag? tag = null) { }

        public void WriteNamedBitList(Enum value, Asn1Tag? tag = null) { }

        public void WriteNamedBitList<TEnum>(TEnum value, Asn1Tag? tag = null)
            where TEnum : Enum { }

        public void WriteNull(Asn1Tag? tag = null) { }

        public void WriteObjectIdentifier(ReadOnlySpan<char> oidValue, Asn1Tag? tag = null) { }

        public void WriteObjectIdentifier(string oidValue, Asn1Tag? tag = null) { }

        public void WriteOctetString(ReadOnlySpan<byte> value, Asn1Tag? tag = null) { }

        public void WriteUtcTime(DateTimeOffset value, int twoDigitYearMax, Asn1Tag? tag = null) { }

        public void WriteUtcTime(DateTimeOffset value, Asn1Tag? tag = null) { }

        public readonly partial struct Scope : IDisposable
        {
            private readonly object _dummy;
            private readonly int _dummyPrimitive;
            public readonly void Dispose() { }
        }
    }

    public enum TagClass
    {
        Universal = 0,
        Application = 64,
        ContextSpecific = 128,
        Private = 192
    }

    public enum UniversalTagNumber
    {
        EndOfContents = 0,
        Boolean = 1,
        Integer = 2,
        BitString = 3,
        OctetString = 4,
        Null = 5,
        ObjectIdentifier = 6,
        ObjectDescriptor = 7,
        External = 8,
        InstanceOf = 8,
        Real = 9,
        Enumerated = 10,
        Embedded = 11,
        UTF8String = 12,
        RelativeObjectIdentifier = 13,
        Time = 14,
        Sequence = 16,
        SequenceOf = 16,
        Set = 17,
        SetOf = 17,
        NumericString = 18,
        PrintableString = 19,
        T61String = 20,
        TeletexString = 20,
        VideotexString = 21,
        IA5String = 22,
        UtcTime = 23,
        GeneralizedTime = 24,
        GraphicString = 25,
        ISO646String = 26,
        VisibleString = 26,
        GeneralString = 27,
        UniversalString = 28,
        UnrestrictedCharacterString = 29,
        BMPString = 30,
        Date = 31,
        TimeOfDay = 32,
        DateTime = 33,
        Duration = 34,
        ObjectIdentifierIRI = 35,
        RelativeObjectIdentifierIRI = 36
    }
}