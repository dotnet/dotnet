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
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Memory")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Memory")]
[assembly: System.Reflection.AssemblyFileVersion("4.600.125.15403")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.1+ab95a1f103b49919ba02577a99f3173405bb50ca")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.Memory")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/maintenance-packages")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.3.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System
{
    public static partial class MemoryExtensions
    {
        public static ReadOnlyMemory<char> AsMemory(this string text, int start, int length) { throw null; }

        public static ReadOnlyMemory<char> AsMemory(this string text, int start) { throw null; }

        public static ReadOnlyMemory<char> AsMemory(this string text) { throw null; }

        public static Memory<T> AsMemory<T>(this T[] array, int start, int length) { throw null; }

        public static Memory<T> AsMemory<T>(this T[] array, int start) { throw null; }

        public static Memory<T> AsMemory<T>(this T[] array) { throw null; }

        public static Memory<T> AsMemory<T>(this ArraySegment<T> segment, int start, int length) { throw null; }

        public static Memory<T> AsMemory<T>(this ArraySegment<T> segment, int start) { throw null; }

        public static Memory<T> AsMemory<T>(this ArraySegment<T> segment) { throw null; }

        public static ReadOnlySpan<char> AsSpan(this string text, int start, int length) { throw null; }

        public static ReadOnlySpan<char> AsSpan(this string text, int start) { throw null; }

        public static ReadOnlySpan<char> AsSpan(this string text) { throw null; }

        public static Span<T> AsSpan<T>(this T[] array, int start, int length) { throw null; }

        public static Span<T> AsSpan<T>(this T[] array, int start) { throw null; }

        public static Span<T> AsSpan<T>(this T[] array) { throw null; }

        public static Span<T> AsSpan<T>(this ArraySegment<T> segment, int start, int length) { throw null; }

        public static Span<T> AsSpan<T>(this ArraySegment<T> segment, int start) { throw null; }

        public static Span<T> AsSpan<T>(this ArraySegment<T> segment) { throw null; }

        public static int BinarySearch<T>(this ReadOnlySpan<T> span, IComparable<T> comparable) { throw null; }

        public static int BinarySearch<T>(this Span<T> span, IComparable<T> comparable) { throw null; }

        public static int BinarySearch<T, TComparer>(this ReadOnlySpan<T> span, T value, TComparer comparer)
            where TComparer : Collections.Generic.IComparer<T> { throw null; }

        public static int BinarySearch<T, TComparable>(this ReadOnlySpan<T> span, TComparable comparable)
            where TComparable : IComparable<T> { throw null; }

        public static int BinarySearch<T, TComparer>(this Span<T> span, T value, TComparer comparer)
            where TComparer : Collections.Generic.IComparer<T> { throw null; }

        public static int BinarySearch<T, TComparable>(this Span<T> span, TComparable comparable)
            where TComparable : IComparable<T> { throw null; }

        public static int CompareTo(this ReadOnlySpan<char> span, ReadOnlySpan<char> other, StringComparison comparisonType) { throw null; }

        public static bool Contains(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType) { throw null; }

        public static void CopyTo<T>(this T[] source, Memory<T> destination) { }

        public static void CopyTo<T>(this T[] source, Span<T> destination) { }

        public static bool EndsWith(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType) { throw null; }

        public static bool EndsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
            where T : IEquatable<T> { throw null; }

        public static bool EndsWith<T>(this Span<T> span, ReadOnlySpan<T> value)
            where T : IEquatable<T> { throw null; }

        public static bool Equals(this ReadOnlySpan<char> span, ReadOnlySpan<char> other, StringComparison comparisonType) { throw null; }

        public static int IndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType) { throw null; }

        public static int IndexOf<T>(this ReadOnlySpan<T> span, T value)
            where T : IEquatable<T> { throw null; }

        public static int IndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
            where T : IEquatable<T> { throw null; }

        public static int IndexOf<T>(this Span<T> span, T value)
            where T : IEquatable<T> { throw null; }

        public static int IndexOf<T>(this Span<T> span, ReadOnlySpan<T> value)
            where T : IEquatable<T> { throw null; }

        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2)
            where T : IEquatable<T> { throw null; }

        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1)
            where T : IEquatable<T> { throw null; }

        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values)
            where T : IEquatable<T> { throw null; }

        public static int IndexOfAny<T>(this Span<T> span, T value0, T value1, T value2)
            where T : IEquatable<T> { throw null; }

        public static int IndexOfAny<T>(this Span<T> span, T value0, T value1)
            where T : IEquatable<T> { throw null; }

        public static int IndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values)
            where T : IEquatable<T> { throw null; }

        public static bool IsWhiteSpace(this ReadOnlySpan<char> span) { throw null; }

        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, T value)
            where T : IEquatable<T> { throw null; }

        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
            where T : IEquatable<T> { throw null; }

        public static int LastIndexOf<T>(this Span<T> span, T value)
            where T : IEquatable<T> { throw null; }

        public static int LastIndexOf<T>(this Span<T> span, ReadOnlySpan<T> value)
            where T : IEquatable<T> { throw null; }

        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2)
            where T : IEquatable<T> { throw null; }

        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1)
            where T : IEquatable<T> { throw null; }

        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values)
            where T : IEquatable<T> { throw null; }

        public static int LastIndexOfAny<T>(this Span<T> span, T value0, T value1, T value2)
            where T : IEquatable<T> { throw null; }

        public static int LastIndexOfAny<T>(this Span<T> span, T value0, T value1)
            where T : IEquatable<T> { throw null; }

        public static int LastIndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values)
            where T : IEquatable<T> { throw null; }

        public static bool Overlaps<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other, out int elementOffset) { throw null; }

        public static bool Overlaps<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other) { throw null; }

        public static bool Overlaps<T>(this Span<T> span, ReadOnlySpan<T> other, out int elementOffset) { throw null; }

        public static bool Overlaps<T>(this Span<T> span, ReadOnlySpan<T> other) { throw null; }

        public static void Reverse<T>(this Span<T> span) { }

        public static int SequenceCompareTo<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other)
            where T : IComparable<T> { throw null; }

        public static int SequenceCompareTo<T>(this Span<T> span, ReadOnlySpan<T> other)
            where T : IComparable<T> { throw null; }

        public static bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other)
            where T : IEquatable<T> { throw null; }

        public static bool SequenceEqual<T>(this Span<T> span, ReadOnlySpan<T> other)
            where T : IEquatable<T> { throw null; }

        public static bool StartsWith(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType) { throw null; }

        public static bool StartsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
            where T : IEquatable<T> { throw null; }

        public static bool StartsWith<T>(this Span<T> span, ReadOnlySpan<T> value)
            where T : IEquatable<T> { throw null; }

        public static int ToLower(this ReadOnlySpan<char> source, Span<char> destination, Globalization.CultureInfo culture) { throw null; }

        public static int ToLowerInvariant(this ReadOnlySpan<char> source, Span<char> destination) { throw null; }

        public static int ToUpper(this ReadOnlySpan<char> source, Span<char> destination, Globalization.CultureInfo culture) { throw null; }

        public static int ToUpperInvariant(this ReadOnlySpan<char> source, Span<char> destination) { throw null; }

        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, char trimChar) { throw null; }

        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars) { throw null; }

        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span) { throw null; }

        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, char trimChar) { throw null; }

        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars) { throw null; }

        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span) { throw null; }

        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, char trimChar) { throw null; }

        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars) { throw null; }

        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span) { throw null; }
    }

    public readonly partial struct Memory<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Memory(T[] array, int start, int length) { }

        public Memory(T[] array) { }

        public static Memory<T> Empty { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public int Length { get { throw null; } }

        public Span<T> Span { get { throw null; } }

        public readonly void CopyTo(Memory<T> destination) { }

        public readonly bool Equals(Memory<T> other) { throw null; }

        public override readonly bool Equals(object obj) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static implicit operator Memory<T>(T[] array) { throw null; }

        public static implicit operator Memory<T>(ArraySegment<T> segment) { throw null; }

        public static implicit operator ReadOnlyMemory<T>(Memory<T> memory) { throw null; }

        public readonly Buffers.MemoryHandle Pin() { throw null; }

        public readonly Memory<T> Slice(int start, int length) { throw null; }

        public readonly Memory<T> Slice(int start) { throw null; }

        public readonly T[] ToArray() { throw null; }

        public override readonly string ToString() { throw null; }

        public readonly bool TryCopyTo(Memory<T> destination) { throw null; }
    }

    public readonly partial struct ReadOnlyMemory<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ReadOnlyMemory(T[] array, int start, int length) { }

        public ReadOnlyMemory(T[] array) { }

        public static ReadOnlyMemory<T> Empty { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public int Length { get { throw null; } }

        public ReadOnlySpan<T> Span { get { throw null; } }

        public readonly void CopyTo(Memory<T> destination) { }

        public override readonly bool Equals(object obj) { throw null; }

        public readonly bool Equals(ReadOnlyMemory<T> other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static implicit operator ReadOnlyMemory<T>(T[] array) { throw null; }

        public static implicit operator ReadOnlyMemory<T>(ArraySegment<T> segment) { throw null; }

        public readonly Buffers.MemoryHandle Pin() { throw null; }

        public readonly ReadOnlyMemory<T> Slice(int start, int length) { throw null; }

        public readonly ReadOnlyMemory<T> Slice(int start) { throw null; }

        public readonly T[] ToArray() { throw null; }

        public override readonly string ToString() { throw null; }

        public readonly bool TryCopyTo(Memory<T> destination) { throw null; }
    }

    public readonly ref partial struct ReadOnlySpan<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ReadOnlySpan(T[] array, int start, int length) { }

        public ReadOnlySpan(T[] array) { }

        [CLSCompliant(false)]
        public unsafe ReadOnlySpan(void* pointer, int length) { }

        public static ReadOnlySpan<T> Empty { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public ref readonly T this[int index] { get { throw null; } }

        public int Length { get { throw null; } }

        public readonly void CopyTo(Span<T> destination) { }

        [Obsolete("Equals() on ReadOnlySpan will always throw an exception. Use == instead.")]
        public override readonly bool Equals(object obj) { throw null; }

        public readonly Enumerator GetEnumerator() { throw null; }

        [Obsolete("GetHashCode() on ReadOnlySpan will always throw an exception.")]
        public override readonly int GetHashCode() { throw null; }

        public readonly ref readonly T GetPinnableReference() { throw null; }

        public static bool operator ==(ReadOnlySpan<T> left, ReadOnlySpan<T> right) { throw null; }

        public static implicit operator ReadOnlySpan<T>(T[] array) { throw null; }

        public static implicit operator ReadOnlySpan<T>(ArraySegment<T> segment) { throw null; }

        public static bool operator !=(ReadOnlySpan<T> left, ReadOnlySpan<T> right) { throw null; }

        public readonly ReadOnlySpan<T> Slice(int start, int length) { throw null; }

        public readonly ReadOnlySpan<T> Slice(int start) { throw null; }

        public readonly T[] ToArray() { throw null; }

        public override readonly string ToString() { throw null; }

        public readonly bool TryCopyTo(Span<T> destination) { throw null; }

        public ref partial struct Enumerator
        {
            private ReadOnlySpan<T> _span;
            private object _dummy;
            private int _dummyPrimitive;
            public ref readonly T Current { get { throw null; } }

            public bool MoveNext() { throw null; }
        }
    }

    public readonly partial struct SequencePosition : IEquatable<SequencePosition>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public SequencePosition(object @object, int integer) { }

        public override readonly bool Equals(object obj) { throw null; }

        public readonly bool Equals(SequencePosition other) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public readonly int GetInteger() { throw null; }

        public readonly object GetObject() { throw null; }
    }

    public readonly ref partial struct Span<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Span(T[] array, int start, int length) { }

        public Span(T[] array) { }

        [CLSCompliant(false)]
        public unsafe Span(void* pointer, int length) { }

        public static Span<T> Empty { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public ref T this[int index] { get { throw null; } }

        public int Length { get { throw null; } }

        public readonly void Clear() { }

        public readonly void CopyTo(Span<T> destination) { }

        [Obsolete("Equals() on Span will always throw an exception. Use == instead.")]
        public override readonly bool Equals(object obj) { throw null; }

        public readonly void Fill(T value) { }

        public readonly Enumerator GetEnumerator() { throw null; }

        [Obsolete("GetHashCode() on Span will always throw an exception.")]
        public override readonly int GetHashCode() { throw null; }

        public readonly ref T GetPinnableReference() { throw null; }

        public static bool operator ==(Span<T> left, Span<T> right) { throw null; }

        public static implicit operator Span<T>(T[] array) { throw null; }

        public static implicit operator Span<T>(ArraySegment<T> segment) { throw null; }

        public static implicit operator ReadOnlySpan<T>(Span<T> span) { throw null; }

        public static bool operator !=(Span<T> left, Span<T> right) { throw null; }

        public readonly Span<T> Slice(int start, int length) { throw null; }

        public readonly Span<T> Slice(int start) { throw null; }

        public readonly T[] ToArray() { throw null; }

        public override readonly string ToString() { throw null; }

        public readonly bool TryCopyTo(Span<T> destination) { throw null; }

        public ref partial struct Enumerator
        {
            private Span<T> _span;
            private object _dummy;
            private int _dummyPrimitive;
            public ref T Current { get { throw null; } }

            public bool MoveNext() { throw null; }
        }
    }
}

namespace System.Buffers
{
    public static partial class BuffersExtensions
    {
        public static void CopyTo<T>(this in ReadOnlySequence<T> source, Span<T> destination) { }

        public static SequencePosition? PositionOf<T>(this in ReadOnlySequence<T> source, T value)
            where T : IEquatable<T> { throw null; }

        public static T[] ToArray<T>(this in ReadOnlySequence<T> sequence) { throw null; }

        public static void Write<T>(this IBufferWriter<T> writer, ReadOnlySpan<T> value) { }
    }

    public partial interface IBufferWriter<T>
    {
        void Advance(int count);
        Memory<T> GetMemory(int sizeHint = 0);
        Span<T> GetSpan(int sizeHint = 0);
    }

    public partial interface IMemoryOwner<T> : IDisposable
    {
        Memory<T> Memory { get; }
    }

    public partial interface IPinnable
    {
        MemoryHandle Pin(int elementIndex);
        void Unpin();
    }

    public partial struct MemoryHandle : IDisposable
    {
        private object _dummy;
        private int _dummyPrimitive;
        [CLSCompliant(false)]
        public unsafe MemoryHandle(void* pointer, Runtime.InteropServices.GCHandle handle = default, IPinnable pinnable = null) { }

        [CLSCompliant(false)]
        public unsafe void* Pointer { get { throw null; } }

        public void Dispose() { }
    }

    public abstract partial class MemoryManager<T> : IMemoryOwner<T>, IDisposable, IPinnable
    {
        public virtual Memory<T> Memory { get { throw null; } }

        protected Memory<T> CreateMemory(int start, int length) { throw null; }

        protected Memory<T> CreateMemory(int length) { throw null; }

        protected abstract void Dispose(bool disposing);
        public abstract Span<T> GetSpan();
        public abstract MemoryHandle Pin(int elementIndex = 0);
        void IDisposable.Dispose() { }

        protected internal virtual bool TryGetArray(out ArraySegment<T> segment) { throw null; }

        public abstract void Unpin();
    }

    public abstract partial class MemoryPool<T> : IDisposable
    {
        public abstract int MaxBufferSize { get; }

        public static MemoryPool<T> Shared { get { throw null; } }

        public void Dispose() { }

        protected abstract void Dispose(bool disposing);
        public abstract IMemoryOwner<T> Rent(int minBufferSize = -1);
    }

    public enum OperationStatus
    {
        Done = 0,
        DestinationTooSmall = 1,
        NeedMoreData = 2,
        InvalidData = 3
    }

    public abstract partial class ReadOnlySequenceSegment<T>
    {
        public ReadOnlyMemory<T> Memory { get { throw null; } protected set { } }

        public ReadOnlySequenceSegment<T> Next { get { throw null; } protected set { } }

        public long RunningIndex { get { throw null; } protected set { } }
    }

    public readonly partial struct ReadOnlySequence<T>
    {
        public static readonly ReadOnlySequence<T> Empty;
        public ReadOnlySequence(T[] array, int start, int length) { }

        public ReadOnlySequence(T[] array) { }

        public ReadOnlySequence(ReadOnlySequenceSegment<T> startSegment, int startIndex, ReadOnlySequenceSegment<T> endSegment, int endIndex) { }

        public ReadOnlySequence(ReadOnlyMemory<T> memory) { }

        public SequencePosition End { get { throw null; } }

        public ReadOnlyMemory<T> First { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public bool IsSingleSegment { get { throw null; } }

        public long Length { get { throw null; } }

        public SequencePosition Start { get { throw null; } }

        public readonly Enumerator GetEnumerator() { throw null; }

        public readonly SequencePosition GetPosition(long offset, SequencePosition origin) { throw null; }

        public readonly SequencePosition GetPosition(long offset) { throw null; }

        public readonly ReadOnlySequence<T> Slice(int start, int length) { throw null; }

        public readonly ReadOnlySequence<T> Slice(int start, SequencePosition end) { throw null; }

        public readonly ReadOnlySequence<T> Slice(long start, long length) { throw null; }

        public readonly ReadOnlySequence<T> Slice(long start, SequencePosition end) { throw null; }

        public readonly ReadOnlySequence<T> Slice(long start) { throw null; }

        public readonly ReadOnlySequence<T> Slice(SequencePosition start, int length) { throw null; }

        public readonly ReadOnlySequence<T> Slice(SequencePosition start, long length) { throw null; }

        public readonly ReadOnlySequence<T> Slice(SequencePosition start, SequencePosition end) { throw null; }

        public readonly ReadOnlySequence<T> Slice(SequencePosition start) { throw null; }

        public override readonly string ToString() { throw null; }

        public readonly bool TryGet(ref SequencePosition position, out ReadOnlyMemory<T> memory, bool advance = true) { throw null; }

        public partial struct Enumerator
        {
            private ReadOnlySequence<T> _sequence;
            private ReadOnlyMemory<T> _currentMemory;
            private int _dummyPrimitive;
            public Enumerator(in ReadOnlySequence<T> sequence) { }

            public ReadOnlyMemory<T> Current { get { throw null; } }

            public bool MoveNext() { throw null; }
        }
    }

    public readonly partial struct StandardFormat : IEquatable<StandardFormat>
    {
        private readonly int _dummyPrimitive;
        public const byte MaxPrecision = 99;
        public const byte NoPrecision = 255;
        public StandardFormat(char symbol, byte precision = 255) { }

        public bool HasPrecision { get { throw null; } }

        public bool IsDefault { get { throw null; } }

        public byte Precision { get { throw null; } }

        public char Symbol { get { throw null; } }

        public readonly bool Equals(StandardFormat other) { throw null; }

        public override readonly bool Equals(object obj) { throw null; }

        public override readonly int GetHashCode() { throw null; }

        public static bool operator ==(StandardFormat left, StandardFormat right) { throw null; }

        public static implicit operator StandardFormat(char symbol) { throw null; }

        public static bool operator !=(StandardFormat left, StandardFormat right) { throw null; }

        public static StandardFormat Parse(ReadOnlySpan<char> format) { throw null; }

        public static StandardFormat Parse(string format) { throw null; }

        public override readonly string ToString() { throw null; }
    }
}

namespace System.Buffers.Binary
{
    public static partial class BinaryPrimitives
    {
        public static short ReadInt16BigEndian(ReadOnlySpan<byte> source) { throw null; }

        public static short ReadInt16LittleEndian(ReadOnlySpan<byte> source) { throw null; }

        public static int ReadInt32BigEndian(ReadOnlySpan<byte> source) { throw null; }

        public static int ReadInt32LittleEndian(ReadOnlySpan<byte> source) { throw null; }

        public static long ReadInt64BigEndian(ReadOnlySpan<byte> source) { throw null; }

        public static long ReadInt64LittleEndian(ReadOnlySpan<byte> source) { throw null; }

        [CLSCompliant(false)]
        public static ushort ReadUInt16BigEndian(ReadOnlySpan<byte> source) { throw null; }

        [CLSCompliant(false)]
        public static ushort ReadUInt16LittleEndian(ReadOnlySpan<byte> source) { throw null; }

        [CLSCompliant(false)]
        public static uint ReadUInt32BigEndian(ReadOnlySpan<byte> source) { throw null; }

        [CLSCompliant(false)]
        public static uint ReadUInt32LittleEndian(ReadOnlySpan<byte> source) { throw null; }

        [CLSCompliant(false)]
        public static ulong ReadUInt64BigEndian(ReadOnlySpan<byte> source) { throw null; }

        [CLSCompliant(false)]
        public static ulong ReadUInt64LittleEndian(ReadOnlySpan<byte> source) { throw null; }

        public static byte ReverseEndianness(byte value) { throw null; }

        public static short ReverseEndianness(short value) { throw null; }

        public static int ReverseEndianness(int value) { throw null; }

        public static long ReverseEndianness(long value) { throw null; }

        [CLSCompliant(false)]
        public static sbyte ReverseEndianness(sbyte value) { throw null; }

        [CLSCompliant(false)]
        public static ushort ReverseEndianness(ushort value) { throw null; }

        [CLSCompliant(false)]
        public static uint ReverseEndianness(uint value) { throw null; }

        [CLSCompliant(false)]
        public static ulong ReverseEndianness(ulong value) { throw null; }

        public static bool TryReadInt16BigEndian(ReadOnlySpan<byte> source, out short value) { throw null; }

        public static bool TryReadInt16LittleEndian(ReadOnlySpan<byte> source, out short value) { throw null; }

        public static bool TryReadInt32BigEndian(ReadOnlySpan<byte> source, out int value) { throw null; }

        public static bool TryReadInt32LittleEndian(ReadOnlySpan<byte> source, out int value) { throw null; }

        public static bool TryReadInt64BigEndian(ReadOnlySpan<byte> source, out long value) { throw null; }

        public static bool TryReadInt64LittleEndian(ReadOnlySpan<byte> source, out long value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryReadUInt16BigEndian(ReadOnlySpan<byte> source, out ushort value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryReadUInt16LittleEndian(ReadOnlySpan<byte> source, out ushort value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryReadUInt32BigEndian(ReadOnlySpan<byte> source, out uint value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryReadUInt32LittleEndian(ReadOnlySpan<byte> source, out uint value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryReadUInt64BigEndian(ReadOnlySpan<byte> source, out ulong value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryReadUInt64LittleEndian(ReadOnlySpan<byte> source, out ulong value) { throw null; }

        public static bool TryWriteInt16BigEndian(Span<byte> destination, short value) { throw null; }

        public static bool TryWriteInt16LittleEndian(Span<byte> destination, short value) { throw null; }

        public static bool TryWriteInt32BigEndian(Span<byte> destination, int value) { throw null; }

        public static bool TryWriteInt32LittleEndian(Span<byte> destination, int value) { throw null; }

        public static bool TryWriteInt64BigEndian(Span<byte> destination, long value) { throw null; }

        public static bool TryWriteInt64LittleEndian(Span<byte> destination, long value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryWriteUInt16BigEndian(Span<byte> destination, ushort value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryWriteUInt16LittleEndian(Span<byte> destination, ushort value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryWriteUInt32BigEndian(Span<byte> destination, uint value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryWriteUInt32LittleEndian(Span<byte> destination, uint value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryWriteUInt64BigEndian(Span<byte> destination, ulong value) { throw null; }

        [CLSCompliant(false)]
        public static bool TryWriteUInt64LittleEndian(Span<byte> destination, ulong value) { throw null; }

        public static void WriteInt16BigEndian(Span<byte> destination, short value) { }

        public static void WriteInt16LittleEndian(Span<byte> destination, short value) { }

        public static void WriteInt32BigEndian(Span<byte> destination, int value) { }

        public static void WriteInt32LittleEndian(Span<byte> destination, int value) { }

        public static void WriteInt64BigEndian(Span<byte> destination, long value) { }

        public static void WriteInt64LittleEndian(Span<byte> destination, long value) { }

        [CLSCompliant(false)]
        public static void WriteUInt16BigEndian(Span<byte> destination, ushort value) { }

        [CLSCompliant(false)]
        public static void WriteUInt16LittleEndian(Span<byte> destination, ushort value) { }

        [CLSCompliant(false)]
        public static void WriteUInt32BigEndian(Span<byte> destination, uint value) { }

        [CLSCompliant(false)]
        public static void WriteUInt32LittleEndian(Span<byte> destination, uint value) { }

        [CLSCompliant(false)]
        public static void WriteUInt64BigEndian(Span<byte> destination, ulong value) { }

        [CLSCompliant(false)]
        public static void WriteUInt64LittleEndian(Span<byte> destination, ulong value) { }
    }
}

namespace System.Buffers.Text
{
    public static partial class Base64
    {
        public static OperationStatus DecodeFromUtf8(ReadOnlySpan<byte> utf8, Span<byte> bytes, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true) { throw null; }

        public static OperationStatus DecodeFromUtf8InPlace(Span<byte> buffer, out int bytesWritten) { throw null; }

        public static OperationStatus EncodeToUtf8(ReadOnlySpan<byte> bytes, Span<byte> utf8, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true) { throw null; }

        public static OperationStatus EncodeToUtf8InPlace(Span<byte> buffer, int dataLength, out int bytesWritten) { throw null; }

        public static int GetMaxDecodedFromUtf8Length(int length) { throw null; }

        public static int GetMaxEncodedToUtf8Length(int length) { throw null; }
    }

    public static partial class Utf8Formatter
    {
        public static bool TryFormat(bool value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(byte value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(DateTime value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(DateTimeOffset value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(decimal value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(double value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(Guid value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(short value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(int value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(long value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        [CLSCompliant(false)]
        public static bool TryFormat(sbyte value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(float value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        public static bool TryFormat(TimeSpan value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        [CLSCompliant(false)]
        public static bool TryFormat(ushort value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        [CLSCompliant(false)]
        public static bool TryFormat(uint value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }

        [CLSCompliant(false)]
        public static bool TryFormat(ulong value, Span<byte> destination, out int bytesWritten, StandardFormat format = default) { throw null; }
    }

    public static partial class Utf8Parser
    {
        public static bool TryParse(ReadOnlySpan<byte> source, out bool value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out byte value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out DateTime value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out DateTimeOffset value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out decimal value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out double value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out Guid value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out short value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out int value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out long value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        [CLSCompliant(false)]
        public static bool TryParse(ReadOnlySpan<byte> source, out sbyte value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out float value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        public static bool TryParse(ReadOnlySpan<byte> source, out TimeSpan value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        [CLSCompliant(false)]
        public static bool TryParse(ReadOnlySpan<byte> source, out ushort value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        [CLSCompliant(false)]
        public static bool TryParse(ReadOnlySpan<byte> source, out uint value, out int bytesConsumed, char standardFormat = '\0') { throw null; }

        [CLSCompliant(false)]
        public static bool TryParse(ReadOnlySpan<byte> source, out ulong value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
    }
}

namespace System.Runtime.InteropServices
{
    public static partial class MemoryMarshal
    {
        public static ReadOnlySpan<byte> AsBytes<T>(ReadOnlySpan<T> span)
            where T : struct { throw null; }

        public static Span<byte> AsBytes<T>(Span<T> span)
            where T : struct { throw null; }

        public static Memory<T> AsMemory<T>(ReadOnlyMemory<T> memory) { throw null; }

        public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(ReadOnlySpan<TFrom> span)
            where TFrom : struct where TTo : struct { throw null; }

        public static Span<TTo> Cast<TFrom, TTo>(Span<TFrom> span)
            where TFrom : struct where TTo : struct { throw null; }

        public static Memory<T> CreateFromPinnedArray<T>(T[] array, int start, int length) { throw null; }

        public static ref T GetReference<T>(ReadOnlySpan<T> span) { throw null; }

        public static ref T GetReference<T>(Span<T> span) { throw null; }

        public static T Read<T>(ReadOnlySpan<byte> source)
            where T : struct { throw null; }

        public static Collections.Generic.IEnumerable<T> ToEnumerable<T>(ReadOnlyMemory<T> memory) { throw null; }

        public static bool TryGetArray<T>(ReadOnlyMemory<T> memory, out ArraySegment<T> segment) { throw null; }

        public static bool TryGetMemoryManager<T, TManager>(ReadOnlyMemory<T> memory, out TManager manager, out int start, out int length)
            where TManager : Buffers.MemoryManager<T> { throw null; }

        public static bool TryGetMemoryManager<T, TManager>(ReadOnlyMemory<T> memory, out TManager manager)
            where TManager : Buffers.MemoryManager<T> { throw null; }

        public static bool TryGetString(ReadOnlyMemory<char> memory, out string text, out int start, out int length) { throw null; }

        public static bool TryRead<T>(ReadOnlySpan<byte> source, out T value)
            where T : struct { throw null; }

        public static bool TryWrite<T>(Span<byte> destination, ref T value)
            where T : struct { throw null; }

        public static void Write<T>(Span<byte> destination, ref T value)
            where T : struct { }
    }

    public static partial class SequenceMarshal
    {
        public static bool TryGetArray<T>(Buffers.ReadOnlySequence<T> sequence, out ArraySegment<T> segment) { throw null; }

        public static bool TryGetReadOnlyMemory<T>(Buffers.ReadOnlySequence<T> sequence, out ReadOnlyMemory<T> memory) { throw null; }

        public static bool TryGetReadOnlySequenceSegment<T>(Buffers.ReadOnlySequence<T> sequence, out Buffers.ReadOnlySequenceSegment<T> startSegment, out int startIndex, out Buffers.ReadOnlySequenceSegment<T> endSegment, out int endIndex) { throw null; }
    }
}