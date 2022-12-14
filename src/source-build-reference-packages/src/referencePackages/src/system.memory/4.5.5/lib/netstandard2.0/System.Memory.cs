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
[assembly: AssemblyTitle("System.Memory")]
[assembly: AssemblyDescription("System.Memory")]
[assembly: AssemblyDefaultAlias("System.Memory")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.31308.01")]
[assembly: AssemblyInformationalVersion("4.6.31308.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.1.2")]




namespace System
{
    public static partial class MemoryExtensions
    {
        public static System.ReadOnlyMemory<char> AsMemory(this string text) { throw null; }
        public static System.ReadOnlyMemory<char> AsMemory(this string text, int start) { throw null; }
        public static System.ReadOnlyMemory<char> AsMemory(this string text, int start, int length) { throw null; }
        public static System.Memory<T> AsMemory<T>(this System.ArraySegment<T> segment) { throw null; }
        public static System.Memory<T> AsMemory<T>(this System.ArraySegment<T> segment, int start) { throw null; }
        public static System.Memory<T> AsMemory<T>(this System.ArraySegment<T> segment, int start, int length) { throw null; }
        public static System.Memory<T> AsMemory<T>(this T[] array) { throw null; }
        public static System.Memory<T> AsMemory<T>(this T[] array, int start) { throw null; }
        public static System.Memory<T> AsMemory<T>(this T[] array, int start, int length) { throw null; }
        public static System.ReadOnlySpan<char> AsSpan(this string text) { throw null; }
        public static System.ReadOnlySpan<char> AsSpan(this string text, int start) { throw null; }
        public static System.ReadOnlySpan<char> AsSpan(this string text, int start, int length) { throw null; }
        public static System.Span<T> AsSpan<T>(this System.ArraySegment<T> segment) { throw null; }
        public static System.Span<T> AsSpan<T>(this System.ArraySegment<T> segment, int start) { throw null; }
        public static System.Span<T> AsSpan<T>(this System.ArraySegment<T> segment, int start, int length) { throw null; }
        public static System.Span<T> AsSpan<T>(this T[] array) { throw null; }
        public static System.Span<T> AsSpan<T>(this T[] array, int start) { throw null; }
        public static System.Span<T> AsSpan<T>(this T[] array, int start, int length) { throw null; }
        public static int BinarySearch<T>(this System.ReadOnlySpan<T> span, System.IComparable<T> comparable) { throw null; }
        public static int BinarySearch<T>(this System.Span<T> span, System.IComparable<T> comparable) { throw null; }
        public static int BinarySearch<T, TComparer>(this System.ReadOnlySpan<T> span, T value, TComparer comparer) where TComparer : System.Collections.Generic.IComparer<T> { throw null; }
        public static int BinarySearch<T, TComparable>(this System.ReadOnlySpan<T> span, TComparable comparable) where TComparable : System.IComparable<T> { throw null; }
        public static int BinarySearch<T, TComparer>(this System.Span<T> span, T value, TComparer comparer) where TComparer : System.Collections.Generic.IComparer<T> { throw null; }
        public static int BinarySearch<T, TComparable>(this System.Span<T> span, TComparable comparable) where TComparable : System.IComparable<T> { throw null; }
        public static int CompareTo(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> other, System.StringComparison comparisonType) { throw null; }
        public static bool Contains(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> value, System.StringComparison comparisonType) { throw null; }
        public static void CopyTo<T>(this T[] source, System.Memory<T> destination) { }
        public static void CopyTo<T>(this T[] source, System.Span<T> destination) { }
        public static bool EndsWith(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> value, System.StringComparison comparisonType) { throw null; }
        public static bool EndsWith<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> value) where T : System.IEquatable<T> { throw null; }
        public static bool EndsWith<T>(this System.Span<T> span, System.ReadOnlySpan<T> value) where T : System.IEquatable<T> { throw null; }
        public static bool Equals(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> other, System.StringComparison comparisonType) { throw null; }
        public static int IndexOf(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> value, System.StringComparison comparisonType) { throw null; }
        public static int IndexOfAny<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> values) where T : System.IEquatable<T> { throw null; }
        public static int IndexOfAny<T>(this System.ReadOnlySpan<T> span, T value0, T value1) where T : System.IEquatable<T> { throw null; }
        public static int IndexOfAny<T>(this System.ReadOnlySpan<T> span, T value0, T value1, T value2) where T : System.IEquatable<T> { throw null; }
        public static int IndexOfAny<T>(this System.Span<T> span, System.ReadOnlySpan<T> values) where T : System.IEquatable<T> { throw null; }
        public static int IndexOfAny<T>(this System.Span<T> span, T value0, T value1) where T : System.IEquatable<T> { throw null; }
        public static int IndexOfAny<T>(this System.Span<T> span, T value0, T value1, T value2) where T : System.IEquatable<T> { throw null; }
        public static int IndexOf<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> value) where T : System.IEquatable<T> { throw null; }
        public static int IndexOf<T>(this System.ReadOnlySpan<T> span, T value) where T : System.IEquatable<T> { throw null; }
        public static int IndexOf<T>(this System.Span<T> span, System.ReadOnlySpan<T> value) where T : System.IEquatable<T> { throw null; }
        public static int IndexOf<T>(this System.Span<T> span, T value) where T : System.IEquatable<T> { throw null; }
        public static bool IsWhiteSpace(this System.ReadOnlySpan<char> span) { throw null; }
        public static int LastIndexOfAny<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> values) where T : System.IEquatable<T> { throw null; }
        public static int LastIndexOfAny<T>(this System.ReadOnlySpan<T> span, T value0, T value1) where T : System.IEquatable<T> { throw null; }
        public static int LastIndexOfAny<T>(this System.ReadOnlySpan<T> span, T value0, T value1, T value2) where T : System.IEquatable<T> { throw null; }
        public static int LastIndexOfAny<T>(this System.Span<T> span, System.ReadOnlySpan<T> values) where T : System.IEquatable<T> { throw null; }
        public static int LastIndexOfAny<T>(this System.Span<T> span, T value0, T value1) where T : System.IEquatable<T> { throw null; }
        public static int LastIndexOfAny<T>(this System.Span<T> span, T value0, T value1, T value2) where T : System.IEquatable<T> { throw null; }
        public static int LastIndexOf<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> value) where T : System.IEquatable<T> { throw null; }
        public static int LastIndexOf<T>(this System.ReadOnlySpan<T> span, T value) where T : System.IEquatable<T> { throw null; }
        public static int LastIndexOf<T>(this System.Span<T> span, System.ReadOnlySpan<T> value) where T : System.IEquatable<T> { throw null; }
        public static int LastIndexOf<T>(this System.Span<T> span, T value) where T : System.IEquatable<T> { throw null; }
        public static bool Overlaps<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> other) { throw null; }
        public static bool Overlaps<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> other, out int elementOffset) { throw null; }
        public static bool Overlaps<T>(this System.Span<T> span, System.ReadOnlySpan<T> other) { throw null; }
        public static bool Overlaps<T>(this System.Span<T> span, System.ReadOnlySpan<T> other, out int elementOffset) { throw null; }
        public static void Reverse<T>(this System.Span<T> span) { }
        public static int SequenceCompareTo<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> other) where T : System.IComparable<T> { throw null; }
        public static int SequenceCompareTo<T>(this System.Span<T> span, System.ReadOnlySpan<T> other) where T : System.IComparable<T> { throw null; }
        public static bool SequenceEqual<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> other) where T : System.IEquatable<T> { throw null; }
        public static bool SequenceEqual<T>(this System.Span<T> span, System.ReadOnlySpan<T> other) where T : System.IEquatable<T> { throw null; }
        public static bool StartsWith(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> value, System.StringComparison comparisonType) { throw null; }
        public static bool StartsWith<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> value) where T : System.IEquatable<T> { throw null; }
        public static bool StartsWith<T>(this System.Span<T> span, System.ReadOnlySpan<T> value) where T : System.IEquatable<T> { throw null; }
        public static int ToLower(this System.ReadOnlySpan<char> source, System.Span<char> destination, System.Globalization.CultureInfo culture) { throw null; }
        public static int ToLowerInvariant(this System.ReadOnlySpan<char> source, System.Span<char> destination) { throw null; }
        public static int ToUpper(this System.ReadOnlySpan<char> source, System.Span<char> destination, System.Globalization.CultureInfo culture) { throw null; }
        public static int ToUpperInvariant(this System.ReadOnlySpan<char> source, System.Span<char> destination) { throw null; }
        public static System.ReadOnlySpan<char> Trim(this System.ReadOnlySpan<char> span) { throw null; }
        public static System.ReadOnlySpan<char> Trim(this System.ReadOnlySpan<char> span, char trimChar) { throw null; }
        public static System.ReadOnlySpan<char> Trim(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> trimChars) { throw null; }
        public static System.ReadOnlySpan<char> TrimEnd(this System.ReadOnlySpan<char> span) { throw null; }
        public static System.ReadOnlySpan<char> TrimEnd(this System.ReadOnlySpan<char> span, char trimChar) { throw null; }
        public static System.ReadOnlySpan<char> TrimEnd(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> trimChars) { throw null; }
        public static System.ReadOnlySpan<char> TrimStart(this System.ReadOnlySpan<char> span) { throw null; }
        public static System.ReadOnlySpan<char> TrimStart(this System.ReadOnlySpan<char> span, char trimChar) { throw null; }
        public static System.ReadOnlySpan<char> TrimStart(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> trimChars) { throw null; }
    }
    public readonly partial struct Memory<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Memory(T[] array) { throw null; }
        public Memory(T[] array, int start, int length) { throw null; }
        public static System.Memory<T> Empty { get { throw null; } }
        public bool IsEmpty { get { throw null; } }
        public int Length { get { throw null; } }
        public System.Span<T> Span { get { throw null; } }
        public void CopyTo(System.Memory<T> destination) { }
        public bool Equals(System.Memory<T> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static implicit operator System.Memory<T> (System.ArraySegment<T> segment) { throw null; }
        public static implicit operator System.ReadOnlyMemory<T> (System.Memory<T> memory) { throw null; }
        public static implicit operator System.Memory<T> (T[] array) { throw null; }
        public System.Buffers.MemoryHandle Pin() { throw null; }
        public System.Memory<T> Slice(int start) { throw null; }
        public System.Memory<T> Slice(int start, int length) { throw null; }
        public T[] ToArray() { throw null; }
        public override string ToString() { throw null; }
        public bool TryCopyTo(System.Memory<T> destination) { throw null; }
    }
    public readonly partial struct ReadOnlyMemory<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ReadOnlyMemory(T[] array) { throw null; }
        public ReadOnlyMemory(T[] array, int start, int length) { throw null; }
        public static System.ReadOnlyMemory<T> Empty { get { throw null; } }
        public bool IsEmpty { get { throw null; } }
        public int Length { get { throw null; } }
        public System.ReadOnlySpan<T> Span { get { throw null; } }
        public void CopyTo(System.Memory<T> destination) { }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.ReadOnlyMemory<T> other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static implicit operator System.ReadOnlyMemory<T> (System.ArraySegment<T> segment) { throw null; }
        public static implicit operator System.ReadOnlyMemory<T> (T[] array) { throw null; }
        public System.Buffers.MemoryHandle Pin() { throw null; }
        public System.ReadOnlyMemory<T> Slice(int start) { throw null; }
        public System.ReadOnlyMemory<T> Slice(int start, int length) { throw null; }
        public T[] ToArray() { throw null; }
        public override string ToString() { throw null; }
        public bool TryCopyTo(System.Memory<T> destination) { throw null; }
    }
    public readonly ref partial struct ReadOnlySpan<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        [System.CLSCompliantAttribute(false)]
        public unsafe ReadOnlySpan(void* pointer, int length) { throw null; }
        public ReadOnlySpan(T[] array) { throw null; }
        public ReadOnlySpan(T[] array, int start, int length) { throw null; }
        public static System.ReadOnlySpan<T> Empty { get { throw null; } }
        public bool IsEmpty { get { throw null; } }
        public ref readonly T this[int index] { get { throw null; } }
        public int Length { get { throw null; } }
        public void CopyTo(System.Span<T> destination) { }
        [System.ObsoleteAttribute("Equals() on ReadOnlySpan will always throw an exception. Use == instead.")]
        public override bool Equals(object obj) { throw null; }
        public System.ReadOnlySpan<T>.Enumerator GetEnumerator() { throw null; }
        [System.ObsoleteAttribute("GetHashCode() on ReadOnlySpan will always throw an exception.")]
        public override int GetHashCode() { throw null; }
        public ref readonly T GetPinnableReference() { throw null; }
        public static bool operator ==(System.ReadOnlySpan<T> left, System.ReadOnlySpan<T> right) { throw null; }
        public static implicit operator System.ReadOnlySpan<T> (System.ArraySegment<T> segment) { throw null; }
        public static implicit operator System.ReadOnlySpan<T> (T[] array) { throw null; }
        public static bool operator !=(System.ReadOnlySpan<T> left, System.ReadOnlySpan<T> right) { throw null; }
        public System.ReadOnlySpan<T> Slice(int start) { throw null; }
        public System.ReadOnlySpan<T> Slice(int start, int length) { throw null; }
        public T[] ToArray() { throw null; }
        public override string ToString() { throw null; }
        public bool TryCopyTo(System.Span<T> destination) { throw null; }
        public ref partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public ref readonly T Current { get { throw null; } }
            public bool MoveNext() { throw null; }
        }
    }
    public readonly partial struct SequencePosition : System.IEquatable<System.SequencePosition>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public SequencePosition(object @object, int integer) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.SequencePosition other) { throw null; }
        public override int GetHashCode() { throw null; }
        public int GetInteger() { throw null; }
        public object GetObject() { throw null; }
    }
    public readonly ref partial struct Span<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        [System.CLSCompliantAttribute(false)]
        public unsafe Span(void* pointer, int length) { throw null; }
        public Span(T[] array) { throw null; }
        public Span(T[] array, int start, int length) { throw null; }
        public static System.Span<T> Empty { get { throw null; } }
        public bool IsEmpty { get { throw null; } }
        public ref T this[int index] { get { throw null; } }
        public int Length { get { throw null; } }
        public void Clear() { }
        public void CopyTo(System.Span<T> destination) { }
        [System.ObsoleteAttribute("Equals() on Span will always throw an exception. Use == instead.")]
        public override bool Equals(object obj) { throw null; }
        public void Fill(T value) { }
        public System.Span<T>.Enumerator GetEnumerator() { throw null; }
        [System.ObsoleteAttribute("GetHashCode() on Span will always throw an exception.")]
        public override int GetHashCode() { throw null; }
        public ref T GetPinnableReference() { throw null; }
        public static bool operator ==(System.Span<T> left, System.Span<T> right) { throw null; }
        public static implicit operator System.Span<T> (System.ArraySegment<T> segment) { throw null; }
        public static implicit operator System.ReadOnlySpan<T> (System.Span<T> span) { throw null; }
        public static implicit operator System.Span<T> (T[] array) { throw null; }
        public static bool operator !=(System.Span<T> left, System.Span<T> right) { throw null; }
        public System.Span<T> Slice(int start) { throw null; }
        public System.Span<T> Slice(int start, int length) { throw null; }
        public T[] ToArray() { throw null; }
        public override string ToString() { throw null; }
        public bool TryCopyTo(System.Span<T> destination) { throw null; }
        public ref partial struct Enumerator
        {
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
        public static void CopyTo<T>(this in System.Buffers.ReadOnlySequence<T> source, System.Span<T> destination) { }
        public static System.SequencePosition? PositionOf<T>(this in System.Buffers.ReadOnlySequence<T> source, T value) where T : System.IEquatable<T> { throw null; }
        public static T[] ToArray<T>(this in System.Buffers.ReadOnlySequence<T> sequence) { throw null; }
        public static void Write<T>(this System.Buffers.IBufferWriter<T> writer, System.ReadOnlySpan<T> value) { }
    }
    public partial interface IBufferWriter<T>
    {
        void Advance(int count);
        System.Memory<T> GetMemory(int sizeHint = 0);
        System.Span<T> GetSpan(int sizeHint = 0);
    }
    public partial interface IMemoryOwner<T> : System.IDisposable
    {
        System.Memory<T> Memory { get; }
    }
    public partial interface IPinnable
    {
        System.Buffers.MemoryHandle Pin(int elementIndex);
        void Unpin();
    }
    public partial struct MemoryHandle : System.IDisposable
    {
        private object _dummy;
        private int _dummyPrimitive;
        [System.CLSCompliantAttribute(false)]
        public unsafe MemoryHandle(void* pointer, System.Runtime.InteropServices.GCHandle handle = default(System.Runtime.InteropServices.GCHandle), System.Buffers.IPinnable pinnable = null) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe void* Pointer { get { throw null; } }
        public void Dispose() { }
    }
    public abstract partial class MemoryManager<T> : System.Buffers.IMemoryOwner<T>, System.Buffers.IPinnable, System.IDisposable
    {
        protected MemoryManager() { }
        public virtual System.Memory<T> Memory { get { throw null; } }
        protected System.Memory<T> CreateMemory(int length) { throw null; }
        protected System.Memory<T> CreateMemory(int start, int length) { throw null; }
        protected abstract void Dispose(bool disposing);
        public abstract System.Span<T> GetSpan();
        public abstract System.Buffers.MemoryHandle Pin(int elementIndex = 0);
        void System.IDisposable.Dispose() { }
        protected internal virtual bool TryGetArray(out System.ArraySegment<T> segment) { throw null; }
        public abstract void Unpin();
    }
    public abstract partial class MemoryPool<T> : System.IDisposable
    {
        protected MemoryPool() { }
        public abstract int MaxBufferSize { get; }
        public static System.Buffers.MemoryPool<T> Shared { get { throw null; } }
        public void Dispose() { }
        protected abstract void Dispose(bool disposing);
        public abstract System.Buffers.IMemoryOwner<T> Rent(int minBufferSize = -1);
    }
    public enum OperationStatus
    {
        Done = 0,
        DestinationTooSmall = 1,
        NeedMoreData = 2,
        InvalidData = 3,
    }
    public abstract partial class ReadOnlySequenceSegment<T>
    {
        protected ReadOnlySequenceSegment() { }
        public System.ReadOnlyMemory<T> Memory { get { throw null; } protected set { } }
        public System.Buffers.ReadOnlySequenceSegment<T> Next { get { throw null; } protected set { } }
        public long RunningIndex { get { throw null; } protected set { } }
    }
    public readonly partial struct ReadOnlySequence<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public static readonly System.Buffers.ReadOnlySequence<T> Empty;
        public ReadOnlySequence(System.Buffers.ReadOnlySequenceSegment<T> startSegment, int startIndex, System.Buffers.ReadOnlySequenceSegment<T> endSegment, int endIndex) { throw null; }
        public ReadOnlySequence(System.ReadOnlyMemory<T> memory) { throw null; }
        public ReadOnlySequence(T[] array) { throw null; }
        public ReadOnlySequence(T[] array, int start, int length) { throw null; }
        public System.SequencePosition End { get { throw null; } }
        public System.ReadOnlyMemory<T> First { get { throw null; } }
        public bool IsEmpty { get { throw null; } }
        public bool IsSingleSegment { get { throw null; } }
        public long Length { get { throw null; } }
        public System.SequencePosition Start { get { throw null; } }
        public System.Buffers.ReadOnlySequence<T>.Enumerator GetEnumerator() { throw null; }
        public System.SequencePosition GetPosition(long offset) { throw null; }
        public System.SequencePosition GetPosition(long offset, System.SequencePosition origin) { throw null; }
        public System.Buffers.ReadOnlySequence<T> Slice(int start, int length) { throw null; }
        public System.Buffers.ReadOnlySequence<T> Slice(int start, System.SequencePosition end) { throw null; }
        public System.Buffers.ReadOnlySequence<T> Slice(long start) { throw null; }
        public System.Buffers.ReadOnlySequence<T> Slice(long start, long length) { throw null; }
        public System.Buffers.ReadOnlySequence<T> Slice(long start, System.SequencePosition end) { throw null; }
        public System.Buffers.ReadOnlySequence<T> Slice(System.SequencePosition start) { throw null; }
        public System.Buffers.ReadOnlySequence<T> Slice(System.SequencePosition start, int length) { throw null; }
        public System.Buffers.ReadOnlySequence<T> Slice(System.SequencePosition start, long length) { throw null; }
        public System.Buffers.ReadOnlySequence<T> Slice(System.SequencePosition start, System.SequencePosition end) { throw null; }
        public override string ToString() { throw null; }
        public bool TryGet(ref System.SequencePosition position, out System.ReadOnlyMemory<T> memory, bool advance = true) { throw null; }
        public partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public Enumerator(in System.Buffers.ReadOnlySequence<T> sequence) { throw null; }
            public System.ReadOnlyMemory<T> Current { get { throw null; } }
            public bool MoveNext() { throw null; }
        }
    }
    public readonly partial struct StandardFormat : System.IEquatable<System.Buffers.StandardFormat>
    {
        private readonly int _dummyPrimitive;
        public const byte MaxPrecision = (byte)99;
        public const byte NoPrecision = (byte)255;
        public StandardFormat(char symbol, byte precision = (byte)255) { throw null; }
        public bool HasPrecision { get { throw null; } }
        public bool IsDefault { get { throw null; } }
        public byte Precision { get { throw null; } }
        public char Symbol { get { throw null; } }
        public bool Equals(System.Buffers.StandardFormat other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Buffers.StandardFormat left, System.Buffers.StandardFormat right) { throw null; }
        public static implicit operator System.Buffers.StandardFormat (char symbol) { throw null; }
        public static bool operator !=(System.Buffers.StandardFormat left, System.Buffers.StandardFormat right) { throw null; }
        public static System.Buffers.StandardFormat Parse(System.ReadOnlySpan<char> format) { throw null; }
        public static System.Buffers.StandardFormat Parse(string format) { throw null; }
        public override string ToString() { throw null; }
    }
}
namespace System.Buffers.Binary
{
    public static partial class BinaryPrimitives
    {
        public static short ReadInt16BigEndian(System.ReadOnlySpan<byte> source) { throw null; }
        public static short ReadInt16LittleEndian(System.ReadOnlySpan<byte> source) { throw null; }
        public static int ReadInt32BigEndian(System.ReadOnlySpan<byte> source) { throw null; }
        public static int ReadInt32LittleEndian(System.ReadOnlySpan<byte> source) { throw null; }
        public static long ReadInt64BigEndian(System.ReadOnlySpan<byte> source) { throw null; }
        public static long ReadInt64LittleEndian(System.ReadOnlySpan<byte> source) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ushort ReadUInt16BigEndian(System.ReadOnlySpan<byte> source) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ushort ReadUInt16LittleEndian(System.ReadOnlySpan<byte> source) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static uint ReadUInt32BigEndian(System.ReadOnlySpan<byte> source) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static uint ReadUInt32LittleEndian(System.ReadOnlySpan<byte> source) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ulong ReadUInt64BigEndian(System.ReadOnlySpan<byte> source) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ulong ReadUInt64LittleEndian(System.ReadOnlySpan<byte> source) { throw null; }
        public static byte ReverseEndianness(byte value) { throw null; }
        public static short ReverseEndianness(short value) { throw null; }
        public static int ReverseEndianness(int value) { throw null; }
        public static long ReverseEndianness(long value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static sbyte ReverseEndianness(sbyte value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ushort ReverseEndianness(ushort value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static uint ReverseEndianness(uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ulong ReverseEndianness(ulong value) { throw null; }
        public static bool TryReadInt16BigEndian(System.ReadOnlySpan<byte> source, out short value) { throw null; }
        public static bool TryReadInt16LittleEndian(System.ReadOnlySpan<byte> source, out short value) { throw null; }
        public static bool TryReadInt32BigEndian(System.ReadOnlySpan<byte> source, out int value) { throw null; }
        public static bool TryReadInt32LittleEndian(System.ReadOnlySpan<byte> source, out int value) { throw null; }
        public static bool TryReadInt64BigEndian(System.ReadOnlySpan<byte> source, out long value) { throw null; }
        public static bool TryReadInt64LittleEndian(System.ReadOnlySpan<byte> source, out long value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryReadUInt16BigEndian(System.ReadOnlySpan<byte> source, out ushort value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryReadUInt16LittleEndian(System.ReadOnlySpan<byte> source, out ushort value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryReadUInt32BigEndian(System.ReadOnlySpan<byte> source, out uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryReadUInt32LittleEndian(System.ReadOnlySpan<byte> source, out uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryReadUInt64BigEndian(System.ReadOnlySpan<byte> source, out ulong value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryReadUInt64LittleEndian(System.ReadOnlySpan<byte> source, out ulong value) { throw null; }
        public static bool TryWriteInt16BigEndian(System.Span<byte> destination, short value) { throw null; }
        public static bool TryWriteInt16LittleEndian(System.Span<byte> destination, short value) { throw null; }
        public static bool TryWriteInt32BigEndian(System.Span<byte> destination, int value) { throw null; }
        public static bool TryWriteInt32LittleEndian(System.Span<byte> destination, int value) { throw null; }
        public static bool TryWriteInt64BigEndian(System.Span<byte> destination, long value) { throw null; }
        public static bool TryWriteInt64LittleEndian(System.Span<byte> destination, long value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryWriteUInt16BigEndian(System.Span<byte> destination, ushort value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryWriteUInt16LittleEndian(System.Span<byte> destination, ushort value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryWriteUInt32BigEndian(System.Span<byte> destination, uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryWriteUInt32LittleEndian(System.Span<byte> destination, uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryWriteUInt64BigEndian(System.Span<byte> destination, ulong value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryWriteUInt64LittleEndian(System.Span<byte> destination, ulong value) { throw null; }
        public static void WriteInt16BigEndian(System.Span<byte> destination, short value) { }
        public static void WriteInt16LittleEndian(System.Span<byte> destination, short value) { }
        public static void WriteInt32BigEndian(System.Span<byte> destination, int value) { }
        public static void WriteInt32LittleEndian(System.Span<byte> destination, int value) { }
        public static void WriteInt64BigEndian(System.Span<byte> destination, long value) { }
        public static void WriteInt64LittleEndian(System.Span<byte> destination, long value) { }
        [System.CLSCompliantAttribute(false)]
        public static void WriteUInt16BigEndian(System.Span<byte> destination, ushort value) { }
        [System.CLSCompliantAttribute(false)]
        public static void WriteUInt16LittleEndian(System.Span<byte> destination, ushort value) { }
        [System.CLSCompliantAttribute(false)]
        public static void WriteUInt32BigEndian(System.Span<byte> destination, uint value) { }
        [System.CLSCompliantAttribute(false)]
        public static void WriteUInt32LittleEndian(System.Span<byte> destination, uint value) { }
        [System.CLSCompliantAttribute(false)]
        public static void WriteUInt64BigEndian(System.Span<byte> destination, ulong value) { }
        [System.CLSCompliantAttribute(false)]
        public static void WriteUInt64LittleEndian(System.Span<byte> destination, ulong value) { }
    }
}
namespace System.Buffers.Text
{
    public static partial class Base64
    {
        public static System.Buffers.OperationStatus DecodeFromUtf8(System.ReadOnlySpan<byte> utf8, System.Span<byte> bytes, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true) { throw null; }
        public static System.Buffers.OperationStatus DecodeFromUtf8InPlace(System.Span<byte> buffer, out int bytesWritten) { throw null; }
        public static System.Buffers.OperationStatus EncodeToUtf8(System.ReadOnlySpan<byte> bytes, System.Span<byte> utf8, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true) { throw null; }
        public static System.Buffers.OperationStatus EncodeToUtf8InPlace(System.Span<byte> buffer, int dataLength, out int bytesWritten) { throw null; }
        public static int GetMaxDecodedFromUtf8Length(int length) { throw null; }
        public static int GetMaxEncodedToUtf8Length(int length) { throw null; }
    }
    public static partial class Utf8Formatter
    {
        public static bool TryFormat(bool value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(byte value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(System.DateTime value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(System.DateTimeOffset value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(System.Decimal value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(double value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(System.Guid value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(short value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(int value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(long value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryFormat(sbyte value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(float value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        public static bool TryFormat(System.TimeSpan value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryFormat(ushort value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryFormat(uint value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryFormat(ulong value, System.Span<byte> destination, out int bytesWritten, System.Buffers.StandardFormat format = default(System.Buffers.StandardFormat)) { throw null; }
    }
    public static partial class Utf8Parser
    {
        public static bool TryParse(System.ReadOnlySpan<byte> source, out bool value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out byte value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out System.DateTime value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out System.DateTimeOffset value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out System.Decimal value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out double value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out System.Guid value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out short value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out int value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out long value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(System.ReadOnlySpan<byte> source, out sbyte value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out float value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        public static bool TryParse(System.ReadOnlySpan<byte> source, out System.TimeSpan value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(System.ReadOnlySpan<byte> source, out ushort value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(System.ReadOnlySpan<byte> source, out uint value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(System.ReadOnlySpan<byte> source, out ulong value, out int bytesConsumed, char standardFormat = '\0') { throw null; }
    }
}
namespace System.Runtime.InteropServices
{
    public static partial class MemoryMarshal
    {
        public static System.ReadOnlySpan<byte> AsBytes<T>(System.ReadOnlySpan<T> span) where T : struct { throw null; }
        public static System.Span<byte> AsBytes<T>(System.Span<T> span) where T : struct { throw null; }
        public static System.Memory<T> AsMemory<T>(System.ReadOnlyMemory<T> memory) { throw null; }
        public static System.ReadOnlySpan<TTo> Cast<TFrom, TTo>(System.ReadOnlySpan<TFrom> span) where TFrom : struct where TTo : struct { throw null; }
        public static System.Span<TTo> Cast<TFrom, TTo>(System.Span<TFrom> span) where TFrom : struct where TTo : struct { throw null; }
        public static System.Memory<T> CreateFromPinnedArray<T>(T[] array, int start, int length) { throw null; }
        public static ref T GetReference<T>(System.ReadOnlySpan<T> span) { throw null; }
        public static ref T GetReference<T>(System.Span<T> span) { throw null; }
        public static T Read<T>(System.ReadOnlySpan<byte> source) where T : struct { throw null; }
        public static System.Collections.Generic.IEnumerable<T> ToEnumerable<T>(System.ReadOnlyMemory<T> memory) { throw null; }
        public static bool TryGetArray<T>(System.ReadOnlyMemory<T> memory, out System.ArraySegment<T> segment) { throw null; }
        public static bool TryGetMemoryManager<T, TManager>(System.ReadOnlyMemory<T> memory, out TManager manager) where TManager : System.Buffers.MemoryManager<T> { throw null; }
        public static bool TryGetMemoryManager<T, TManager>(System.ReadOnlyMemory<T> memory, out TManager manager, out int start, out int length) where TManager : System.Buffers.MemoryManager<T> { throw null; }
        public static bool TryGetString(System.ReadOnlyMemory<char> memory, out string text, out int start, out int length) { throw null; }
        public static bool TryRead<T>(System.ReadOnlySpan<byte> source, out T value) where T : struct { throw null; }
        public static bool TryWrite<T>(System.Span<byte> destination, ref T value) where T : struct { throw null; }
        public static void Write<T>(System.Span<byte> destination, ref T value) where T : struct { }
    }
    public static partial class SequenceMarshal
    {
        public static bool TryGetArray<T>(System.Buffers.ReadOnlySequence<T> sequence, out System.ArraySegment<T> segment) { throw null; }
        public static bool TryGetReadOnlyMemory<T>(System.Buffers.ReadOnlySequence<T> sequence, out System.ReadOnlyMemory<T> memory) { throw null; }
        public static bool TryGetReadOnlySequenceSegment<T>(System.Buffers.ReadOnlySequence<T> sequence, out System.Buffers.ReadOnlySequenceSegment<T> startSegment, out int startIndex, out System.Buffers.ReadOnlySequenceSegment<T> endSegment, out int endIndex) { throw null; }
    }
}
