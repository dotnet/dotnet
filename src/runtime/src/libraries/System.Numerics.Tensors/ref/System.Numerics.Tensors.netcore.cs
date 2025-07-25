// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// ------------------------------------------------------------------------------
// Changes to this file must follow the https://aka.ms/api-review process.
// ------------------------------------------------------------------------------

namespace System.Buffers
{
    public readonly partial struct NIndex : System.IEquatable<System.Buffers.NIndex>
    {
        private readonly int _dummyPrimitive;
        public NIndex(System.Index index) { throw null; }
        public NIndex(nint value, bool fromEnd = false) { throw null; }
        public static System.Buffers.NIndex End { get { throw null; } }
        public bool IsFromEnd { get { throw null; } }
        public static System.Buffers.NIndex Start { get { throw null; } }
        public nint Value { get { throw null; } }
        public bool Equals(System.Buffers.NIndex other) { throw null; }
        public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] object? value) { throw null; }
        public static System.Buffers.NIndex FromEnd(nint value) { throw null; }
        public static System.Buffers.NIndex FromStart(nint value) { throw null; }
        public override int GetHashCode() { throw null; }
        public nint GetOffset(nint length) { throw null; }
        public static explicit operator checked System.Index (System.Buffers.NIndex value) { throw null; }
        public static explicit operator System.Index (System.Buffers.NIndex value) { throw null; }
        public static implicit operator System.Buffers.NIndex (System.Index value) { throw null; }
        public static implicit operator System.Buffers.NIndex (nint value) { throw null; }
        public System.Index ToIndex() { throw null; }
        public System.Index ToIndexUnchecked() { throw null; }
        public override string ToString() { throw null; }
    }
    public readonly partial struct NRange : System.IEquatable<System.Buffers.NRange>
    {
        private readonly int _dummyPrimitive;
        public NRange(System.Buffers.NIndex start, System.Buffers.NIndex end) { throw null; }
        public NRange(System.Range range) { throw null; }
        public static System.Buffers.NRange All { get { throw null; } }
        public System.Buffers.NIndex End { get { throw null; } }
        public System.Buffers.NIndex Start { get { throw null; } }
        public static System.Buffers.NRange EndAt(System.Buffers.NIndex end) { throw null; }
        public bool Equals(System.Buffers.NRange other) { throw null; }
        public override bool Equals([System.Diagnostics.CodeAnalysis.NotNullWhenAttribute(true)] object? value) { throw null; }
        public override int GetHashCode() { throw null; }
        public (nint Offset, nint Length) GetOffsetAndLength(nint length) { throw null; }
        public static explicit operator checked System.Range (System.Buffers.NRange value) { throw null; }
        public static explicit operator System.Range (System.Buffers.NRange value) { throw null; }
        public static implicit operator System.Buffers.NRange (System.Range range) { throw null; }
        public static System.Buffers.NRange StartAt(System.Buffers.NIndex start) { throw null; }
        public System.Range ToRange() { throw null; }
        public System.Range ToRangeUnchecked() { throw null; }
        public override string ToString() { throw null; }
    }
}
namespace System.Numerics.Tensors
{
    public partial interface IReadOnlyTensor
    {
        nint FlattenedLength { get; }
        bool HasAnyDenseDimensions { get; }
        bool IsDense { get; }
        bool IsEmpty { get; }
        bool IsPinned { get; }
        object? this[params scoped System.ReadOnlySpan<System.Buffers.NIndex> indexes] { get; }
        object? this[params scoped System.ReadOnlySpan<nint> indexes] { get; }
        [System.Diagnostics.CodeAnalysis.UnscopedRefAttribute]
        System.ReadOnlySpan<nint> Lengths { get; }
        int Rank { get; }
        [System.Diagnostics.CodeAnalysis.UnscopedRefAttribute]
        System.ReadOnlySpan<nint> Strides { get; }
    }
    public partial interface IReadOnlyTensor<TSelf, T> : System.Numerics.Tensors.IReadOnlyTensor
        where TSelf : System.Numerics.Tensors.IReadOnlyTensor<TSelf, T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        static abstract TSelf Empty { get; }
        new ref readonly T this[params scoped System.ReadOnlySpan<System.Buffers.NIndex> indexes] { get; }
        TSelf this[params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges] { get; }
        new ref readonly T this[params scoped System.ReadOnlySpan<nint> indexes] { get; }
        System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan();
        System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan(params scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes);
        System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan(params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges);
        System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan(params scoped System.ReadOnlySpan<nint> startIndexes);
        void CopyTo(scoped in System.Numerics.Tensors.TensorSpan<T> destination);
        void FlattenTo(scoped System.Span<T> destination);
        System.Numerics.Tensors.ReadOnlyTensorDimensionSpan<T> GetDimensionSpan(int dimension);
        ref readonly T GetPinnableReference();
        System.ReadOnlySpan<T> GetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length);
        System.ReadOnlySpan<T> GetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length);
        TSelf Slice(params scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes);
        TSelf Slice(params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges);
        TSelf Slice(params scoped System.ReadOnlySpan<nint> startIndexes);
        TSelf ToDenseTensor();
        bool TryCopyTo(scoped in System.Numerics.Tensors.TensorSpan<T> destination);
        bool TryFlattenTo(scoped System.Span<T> destination);
        bool TryGetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length, out System.ReadOnlySpan<T> span);
        bool TryGetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length, out System.ReadOnlySpan<T> span);
    }
    public partial interface ITensor : System.Numerics.Tensors.IReadOnlyTensor
    {
        bool IsReadOnly { get; }
        new object? this[params scoped System.ReadOnlySpan<System.Buffers.NIndex> indexes] { get; set; }
        new object? this[params scoped System.ReadOnlySpan<nint> indexes] { get; set; }
        void Clear();
        void Fill(object value);
    }
    public partial interface ITensor<TSelf, T> : System.Numerics.Tensors.IReadOnlyTensor, System.Numerics.Tensors.IReadOnlyTensor<TSelf, T>, System.Numerics.Tensors.ITensor
        where TSelf : System.Numerics.Tensors.ITensor<TSelf, T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        new ref T this[params scoped System.ReadOnlySpan<System.Buffers.NIndex> indexes] { get; }
        new TSelf this[params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges] { get; set; }
        new ref T this[params scoped System.ReadOnlySpan<nint> indexes] { get; }
        System.Numerics.Tensors.TensorSpan<T> AsTensorSpan();
        System.Numerics.Tensors.TensorSpan<T> AsTensorSpan(params scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes);
        System.Numerics.Tensors.TensorSpan<T> AsTensorSpan(params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges);
        System.Numerics.Tensors.TensorSpan<T> AsTensorSpan(params scoped System.ReadOnlySpan<nint> startIndexes);
        static abstract TSelf CreateFromShape(scoped System.ReadOnlySpan<nint> lengths, bool pinned = false);
        static abstract TSelf CreateFromShape(scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides, bool pinned = false);
        static abstract TSelf CreateFromShapeUninitialized(scoped System.ReadOnlySpan<nint> lengths, bool pinned = false);
        static abstract TSelf CreateFromShapeUninitialized(scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides, bool pinned = false);
        void Fill(T value);
        new System.Numerics.Tensors.TensorDimensionSpan<T> GetDimensionSpan(int dimension);
        new ref T GetPinnableReference();
        new System.Span<T> GetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length);
        new System.Span<T> GetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length);
        bool TryGetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length, out System.Span<T> span);
        bool TryGetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length, out System.Span<T> span);
    }
    public readonly ref partial struct ReadOnlyTensorDimensionSpan<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> this[nint index] { get { throw null; } }
        public bool IsDense { get { throw null; } }
        public nint Length { get { throw null; } }
        public System.Numerics.Tensors.ReadOnlyTensorDimensionSpan<T>.Enumerator GetEnumerator() { throw null; }
        public ref partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public readonly System.Numerics.Tensors.ReadOnlyTensorSpan<T> Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            public void Reset() { }
        }
    }
    public readonly ref partial struct ReadOnlyTensorSpan<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public ReadOnlyTensorSpan(System.Array? array) { throw null; }
        public ReadOnlyTensorSpan(System.Array? array, scoped System.ReadOnlySpan<int> start, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public ReadOnlyTensorSpan(System.ReadOnlySpan<T> span) { throw null; }
        public ReadOnlyTensorSpan(System.ReadOnlySpan<T> span, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        public ReadOnlyTensorSpan(System.ReadOnlySpan<T> span, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe ReadOnlyTensorSpan(T* data, nint dataLength) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe ReadOnlyTensorSpan(T* data, nint dataLength, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe ReadOnlyTensorSpan(T* data, nint dataLength, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public ReadOnlyTensorSpan(T[]? array) { throw null; }
        public ReadOnlyTensorSpan(T[]? array, int start, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public ReadOnlyTensorSpan(T[]? array, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        public ReadOnlyTensorSpan(T[]? array, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> Empty { get { throw null; } }
        public nint FlattenedLength { get { throw null; } }
        public bool HasAnyDenseDimensions { get { throw null; } }
        public bool IsDense { get { throw null; } }
        public bool IsEmpty { get { throw null; } }
        public bool IsPinned { get { throw null; } }
        public ref readonly T this[params scoped System.ReadOnlySpan<System.Buffers.NIndex> indexes] { get { throw null; } }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> this[params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges] { get { throw null; } }
        public ref readonly T this[params scoped System.ReadOnlySpan<nint> indexes] { get { throw null; } }
        [System.Diagnostics.CodeAnalysis.UnscopedRefAttribute]
        public System.ReadOnlySpan<nint> Lengths { get { throw null; } }
        public int Rank { get { throw null; } }
        [System.Diagnostics.CodeAnalysis.UnscopedRefAttribute]
        public System.ReadOnlySpan<nint> Strides { get { throw null; } }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> CastUp<TDerived>(in System.Numerics.Tensors.ReadOnlyTensorSpan<TDerived> items) where TDerived : class?, T? { throw null; }
        public void CopyTo(scoped in System.Numerics.Tensors.TensorSpan<T> destination) { }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.ObsoleteAttribute("Equals() on ReadOnlyTensorSpan will always throw an exception. Use the equality operator instead.")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override bool Equals(object? obj) { throw null; }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        public void FlattenTo(scoped System.Span<T> destination) { }
        public System.Numerics.Tensors.ReadOnlyTensorDimensionSpan<T> GetDimensionSpan(int dimension) { throw null; }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T>.Enumerator GetEnumerator() { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.ObsoleteAttribute("GetHashCode() on ReadOnlyTensorSpan will always throw an exception.")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override int GetHashCode() { throw null; }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public ref readonly T GetPinnableReference() { throw null; }
        public System.ReadOnlySpan<T> GetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length) { throw null; }
        public System.ReadOnlySpan<T> GetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length) { throw null; }
        public static bool operator ==(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> left, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> right) { throw null; }
        public static implicit operator System.Numerics.Tensors.ReadOnlyTensorSpan<T> (T[]? array) { throw null; }
        public static bool operator !=(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> left, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> right) { throw null; }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> Slice(params scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes) { throw null; }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> Slice(params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges) { throw null; }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> Slice(params scoped System.ReadOnlySpan<nint> startIndexes) { throw null; }
        public override string ToString() { throw null; }
        public bool TryCopyTo(scoped in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        public bool TryFlattenTo(scoped System.Span<T> destination) { throw null; }
        public bool TryGetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length, out System.ReadOnlySpan<T> span) { throw null; }
        public bool TryGetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length, out System.ReadOnlySpan<T> span) { throw null; }
        public ref partial struct Enumerator : System.Collections.Generic.IEnumerator<T>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public readonly ref readonly T Current { get { throw null; } }
            readonly T System.Collections.Generic.IEnumerator<T>.Current { get { throw null; } }
            readonly object? System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            public void Reset() { }
            readonly void System.IDisposable.Dispose() { }
        }
    }
    public static partial class Tensor
    {
        public static System.Numerics.Tensors.Tensor<T> Abs<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Abs<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumberBase<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Acosh<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Acosh<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> AcosPi<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> AcosPi<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Acos<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Acos<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Add<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Add<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Add<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Add<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Asinh<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Asinh<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> AsinPi<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> AsinPi<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Asin<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Asin<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan<T>(this T[]? array) { throw null; }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan<T>(this T[]? array, int start, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan<T>(this T[]? array, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan<T>(this T[]? array, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public static System.Numerics.Tensors.TensorSpan<T> AsTensorSpan<T>(this T[]? array) { throw null; }
        public static System.Numerics.Tensors.TensorSpan<T> AsTensorSpan<T>(this T[]? array, int start, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public static System.Numerics.Tensors.TensorSpan<T> AsTensorSpan<T>(this T[]? array, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        public static System.Numerics.Tensors.TensorSpan<T> AsTensorSpan<T>(this T[]? array, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Atan2Pi<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Atan2Pi<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Atan2Pi<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Atan2Pi<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Atan2Pi<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Atan2Pi<T>(T x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Atan2<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Atan2<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Atan2<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Atan2<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Atan2<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Atan2<T>(T x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Atanh<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Atanh<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> AtanPi<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> AtanPi<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Atan<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Atan<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static T Average<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> BitwiseAnd<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> BitwiseAnd<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> BitwiseAnd<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> BitwiseAnd<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> BitwiseOr<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> BitwiseOr<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> BitwiseOr<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> BitwiseOr<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static void BroadcastTo<T>(this in System.Numerics.Tensors.ReadOnlyTensorSpan<T> source, in System.Numerics.Tensors.TensorSpan<T> destination) { }
        public static void BroadcastTo<T>(this in System.Numerics.Tensors.TensorSpan<T> source, in System.Numerics.Tensors.TensorSpan<T> destination) { }
        public static void BroadcastTo<T>(this System.Numerics.Tensors.Tensor<T> source, in System.Numerics.Tensors.TensorSpan<T> destination) { }
        public static System.Numerics.Tensors.Tensor<T> Broadcast<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> source, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> lengthsSource) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Broadcast<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> source, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Cbrt<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Cbrt<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Ceiling<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Ceiling<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> ConcatenateOnDimension<T>(int dimension, params scoped System.ReadOnlySpan<System.Numerics.Tensors.Tensor<T>> tensors) { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> ConcatenateOnDimension<T>(int dimension, scoped System.ReadOnlySpan<System.Numerics.Tensors.Tensor<T>> tensors, in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Concatenate<T>(params scoped System.ReadOnlySpan<System.Numerics.Tensors.Tensor<T>> tensors) { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Concatenate<T>(scoped System.ReadOnlySpan<System.Numerics.Tensors.Tensor<T>> tensors, in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        public static System.Numerics.Tensors.Tensor<TTo> ConvertChecked<TFrom, TTo>(in System.Numerics.Tensors.ReadOnlyTensorSpan<TFrom> source) where TFrom : System.IEquatable<TFrom>, System.Numerics.IEqualityOperators<TFrom, TFrom, bool>, System.Numerics.INumberBase<TFrom> where TTo : System.Numerics.INumberBase<TTo> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<TTo> ConvertChecked<TFrom, TTo>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<TFrom> source, in System.Numerics.Tensors.TensorSpan<TTo> destination) where TFrom : System.IEquatable<TFrom>, System.Numerics.IEqualityOperators<TFrom, TFrom, bool>, System.Numerics.INumberBase<TFrom> where TTo : System.Numerics.INumberBase<TTo> { throw null; }
        public static System.Numerics.Tensors.Tensor<TTo> ConvertSaturating<TFrom, TTo>(in System.Numerics.Tensors.ReadOnlyTensorSpan<TFrom> source) where TFrom : System.IEquatable<TFrom>, System.Numerics.IEqualityOperators<TFrom, TFrom, bool>, System.Numerics.INumberBase<TFrom> where TTo : System.Numerics.INumberBase<TTo> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<TTo> ConvertSaturating<TFrom, TTo>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<TFrom> source, in System.Numerics.Tensors.TensorSpan<TTo> destination) where TFrom : System.IEquatable<TFrom>, System.Numerics.IEqualityOperators<TFrom, TFrom, bool>, System.Numerics.INumberBase<TFrom> where TTo : System.Numerics.INumberBase<TTo> { throw null; }
        public static System.Numerics.Tensors.Tensor<TTo> ConvertTruncating<TFrom, TTo>(in System.Numerics.Tensors.ReadOnlyTensorSpan<TFrom> source) where TFrom : System.IEquatable<TFrom>, System.Numerics.IEqualityOperators<TFrom, TFrom, bool>, System.Numerics.INumberBase<TFrom> where TTo : System.Numerics.INumberBase<TTo> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<TTo> ConvertTruncating<TFrom, TTo>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<TFrom> source, in System.Numerics.Tensors.TensorSpan<TTo> destination) where TFrom : System.IEquatable<TFrom>, System.Numerics.IEqualityOperators<TFrom, TFrom, bool>, System.Numerics.INumberBase<TFrom> where TTo : System.Numerics.INumberBase<TTo> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> CopySign<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> sign) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> CopySign<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> sign, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> CopySign<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T sign) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> CopySign<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T sign, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Cosh<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Cosh<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static T CosineSimilarity<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> CosPi<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> CosPi<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Cos<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Cos<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Create<T>(T[] array) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Create<T>(T[] array, int start, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Create<T>(T[] array, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Create<T>(T[] array, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> CreateFromShape<T>(scoped System.ReadOnlySpan<nint> lengths, bool pinned = false) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> CreateFromShape<T>(scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides, bool pinned = false) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> CreateFromShapeUninitialized<T>(scoped System.ReadOnlySpan<nint> lengths, bool pinned = false) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> CreateFromShapeUninitialized<T>(scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides, bool pinned = false) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> DegreesToRadians<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> DegreesToRadians<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static T Distance<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Divide<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IDivisionOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Divide<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IDivisionOperators<T, T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Divide<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IDivisionOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Divide<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IDivisionOperators<T, T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Divide<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IDivisionOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Divide<T>(T x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IDivisionOperators<T, T, T> { throw null; }
        public static T Dot<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T>, System.Numerics.IMultiplicativeIdentity<T, T>, System.Numerics.IMultiplyOperators<T, T, T> { throw null; }
        public static bool EqualsAll<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IEqualityOperators<T, T, bool> { throw null; }
        public static bool EqualsAll<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IEqualityOperators<T, T, bool> { throw null; }
        public static bool EqualsAny<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IEqualityOperators<T, T, bool> { throw null; }
        public static bool EqualsAny<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IEqualityOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> Equals<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IEqualityOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> Equals<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IEqualityOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> Equals<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IEqualityOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> Equals<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IEqualityOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Exp10M1<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Exp10M1<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Exp10<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Exp10<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Exp2M1<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Exp2M1<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Exp2<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Exp2<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> ExpM1<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> ExpM1<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Exp<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Exp<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> FillGaussianNormalDistribution<T>(in System.Numerics.Tensors.TensorSpan<T> destination, System.Random? random = null) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> FillUniformDistribution<T>(in System.Numerics.Tensors.TensorSpan<T> destination, System.Random? random = null) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> FilteredUpdate<T>(this in System.Numerics.Tensors.TensorSpan<T> tensor, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<bool> filter, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> values) { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> FilteredUpdate<T>(this in System.Numerics.Tensors.TensorSpan<T> tensor, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<bool> filter, T value) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Floor<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Floor<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static bool GreaterThanAll<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanAll<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanAll<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanAny<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanAny<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanAny<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanOrEqualAll<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanOrEqualAll<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanOrEqualAll<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanOrEqualAny<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanOrEqualAny<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool GreaterThanOrEqualAny<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> GreaterThanOrEqual<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> GreaterThanOrEqual<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> GreaterThanOrEqual<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> GreaterThanOrEqual<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> GreaterThanOrEqual<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> GreaterThanOrEqual<T>(T x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> GreaterThan<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> GreaterThan<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> GreaterThan<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> GreaterThan<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> GreaterThan<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> GreaterThan<T>(T x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Hypot<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Hypot<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Ieee754Remainder<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Ieee754Remainder<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Ieee754Remainder<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Ieee754Remainder<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Ieee754Remainder<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Ieee754Remainder<T>(T x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<int> ILogB<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<int> ILogB<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<int> destination) where T : System.Numerics.IFloatingPointIeee754<T> { throw null; }
        public static nint IndexOfMaxMagnitude<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static nint IndexOfMax<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static nint IndexOfMinMagnitude<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static nint IndexOfMin<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> LeadingZeroCount<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> LeadingZeroCount<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static bool LessThanAll<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanAll<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanAll<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanAny<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanAny<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanAny<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanOrEqualAll<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanOrEqualAll<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanOrEqualAll<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanOrEqualAny<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanOrEqualAny<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static bool LessThanOrEqualAny<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> LessThanOrEqual<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> LessThanOrEqual<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> LessThanOrEqual<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> LessThanOrEqual<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> LessThanOrEqual<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> LessThanOrEqual<T>(T x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> LessThan<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> LessThan<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> LessThan<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> LessThan<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<bool> LessThan<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<bool> LessThan<T>(T x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<bool> destination) where T : System.Numerics.IComparisonOperators<T, T, bool> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Log10P1<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Log10P1<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Log10<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Log10<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Log2P1<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Log2P1<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Log2<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Log2<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> LogP1<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> LogP1<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Log<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Log<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Log<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Log<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Log<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Log<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { throw null; }
        public static T MaxMagnitudeNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MaxMagnitudeNumber<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MaxMagnitudeNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MaxMagnitudeNumber<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MaxMagnitudeNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static T MaxMagnitude<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MaxMagnitude<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MaxMagnitude<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MaxMagnitude<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MaxMagnitude<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static T MaxNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MaxNumber<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MaxNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MaxNumber<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MaxNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static T Max<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Max<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Max<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Max<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Max<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static T MinMagnitudeNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MinMagnitudeNumber<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MinMagnitudeNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MinMagnitudeNumber<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MinMagnitudeNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static T MinMagnitude<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MinMagnitude<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MinMagnitude<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MinMagnitude<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MinMagnitude<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static T MinNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MinNumber<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MinNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> MinNumber<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> MinNumber<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static T Min<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Min<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Min<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Min<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.INumber<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Min<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.INumber<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Multiply<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IMultiplicativeIdentity<T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Multiply<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IMultiplicativeIdentity<T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Multiply<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IMultiplicativeIdentity<T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Multiply<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IMultiplicativeIdentity<T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Negate<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IUnaryNegationOperators<T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Negate<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IUnaryNegationOperators<T, T> { throw null; }
        public static T Norm<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> OnesComplement<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> OnesComplement<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> PermuteDimensions<T>(this System.Numerics.Tensors.Tensor<T> tensor, System.ReadOnlySpan<int> dimensions) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> PopCount<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> PopCount<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Pow<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IPowerFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Pow<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IPowerFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Pow<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IPowerFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Pow<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IPowerFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Pow<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IPowerFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Pow<T>(T x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IPowerFunctions<T> { throw null; }
        public static T Product<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IMultiplicativeIdentity<T, T>, System.Numerics.IMultiplyOperators<T, T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> RadiansToDegrees<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> RadiansToDegrees<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Reciprocal<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Reciprocal<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> Reshape<T>(this scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        public static System.Numerics.Tensors.TensorSpan<T> Reshape<T>(this scoped in System.Numerics.Tensors.TensorSpan<T> tensor, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Reshape<T>(this System.Numerics.Tensors.Tensor<T> tensor, System.ReadOnlySpan<nint> lengths) { throw null; }
        public static void ResizeTo<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, in System.Numerics.Tensors.TensorSpan<T> destination) { }
        public static void ResizeTo<T>(scoped in System.Numerics.Tensors.TensorSpan<T> tensor, in System.Numerics.Tensors.TensorSpan<T> destination) { }
        public static void ResizeTo<T>(scoped in System.Numerics.Tensors.Tensor<T> tensor, in System.Numerics.Tensors.TensorSpan<T> destination) { }
        public static System.Numerics.Tensors.Tensor<T> Resize<T>(System.Numerics.Tensors.Tensor<T> tensor, System.ReadOnlySpan<nint> lengths) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> ReverseDimension<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, int dimension) { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> ReverseDimension<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, in System.Numerics.Tensors.TensorSpan<T> destination, int dimension) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Reverse<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor) { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Reverse<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> RootN<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, int n) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> RootN<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, int n, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> RotateLeft<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, int rotateAmount) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> RotateLeft<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, int rotateAmount, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> RotateRight<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, int rotateAmount) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> RotateRight<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, int rotateAmount, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Round<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Round<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, int digits) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Round<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, int digits, System.MidpointRounding mode) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Round<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, int digits, System.MidpointRounding mode, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Round<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, int digits, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Round<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, System.MidpointRounding mode) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Round<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, System.MidpointRounding mode, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Round<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static bool SequenceEqual<T>(this scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> other) where T : System.IEquatable<T>? { throw null; }
        public static bool SequenceEqual<T>(this scoped in System.Numerics.Tensors.TensorSpan<T> tensor, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> other) where T : System.IEquatable<T>? { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> SetSlice<T>(this in System.Numerics.Tensors.TensorSpan<T> tensor, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> values, params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> SetSlice<T>(this System.Numerics.Tensors.Tensor<T> tensor, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> values, params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Sigmoid<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Sigmoid<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Sinh<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Sinh<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> SinPi<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> SinPi<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Sin<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Sin<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> SoftMax<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> SoftMax<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IExponentialFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T>[] Split<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, int splitCount, nint dimension) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Sqrt<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Sqrt<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> SqueezeDimension<T>(this scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, int dimension) { throw null; }
        public static System.Numerics.Tensors.TensorSpan<T> SqueezeDimension<T>(this scoped in System.Numerics.Tensors.TensorSpan<T> tensor, int dimension) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> SqueezeDimension<T>(this System.Numerics.Tensors.Tensor<T> tensor, int dimension) { throw null; }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> Squeeze<T>(this scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor) { throw null; }
        public static System.Numerics.Tensors.TensorSpan<T> Squeeze<T>(this scoped in System.Numerics.Tensors.TensorSpan<T> tensor) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Squeeze<T>(this System.Numerics.Tensors.Tensor<T> tensor) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> StackAlongDimension<T>(int dimension, params scoped System.ReadOnlySpan<System.Numerics.Tensors.Tensor<T>> tensors) { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> StackAlongDimension<T>(scoped System.ReadOnlySpan<System.Numerics.Tensors.Tensor<T>> tensors, in System.Numerics.Tensors.TensorSpan<T> destination, int dimension) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Stack<T>(params scoped System.ReadOnlySpan<System.Numerics.Tensors.Tensor<T>> tensors) { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Stack<T>(scoped in System.ReadOnlySpan<System.Numerics.Tensors.Tensor<T>> tensors, in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        public static T StdDev<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Subtract<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.ISubtractionOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Subtract<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ISubtractionOperators<T, T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Subtract<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.ISubtractionOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Subtract<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ISubtractionOperators<T, T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Subtract<T>(T x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.ISubtractionOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Subtract<T>(T x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ISubtractionOperators<T, T, T> { throw null; }
        public static T Sum<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Tanh<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Tanh<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> TanPi<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> TanPi<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Tan<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Tan<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { throw null; }
        public static string ToString<T>(this in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, System.ReadOnlySpan<nint> maximumLengths) { throw null; }
        public static string ToString<T>(this in System.Numerics.Tensors.TensorSpan<T> tensor, System.ReadOnlySpan<nint> maximumLengths) { throw null; }
        public static string ToString<T>(this System.Numerics.Tensors.Tensor<T> tensor, System.ReadOnlySpan<nint> maximumLengths) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> TrailingZeroCount<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> TrailingZeroCount<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Transpose<T>(System.Numerics.Tensors.Tensor<T> tensor) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Truncate<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Truncate<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IFloatingPoint<T> { throw null; }
        public static bool TryBroadcastTo<T>(this in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        public static bool TryBroadcastTo<T>(this in System.Numerics.Tensors.TensorSpan<T> tensor, in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        public static bool TryBroadcastTo<T>(this System.Numerics.Tensors.Tensor<T> tensor, in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> Unsqueeze<T>(this scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensor, int dimension) { throw null; }
        public static System.Numerics.Tensors.TensorSpan<T> Unsqueeze<T>(this scoped in System.Numerics.Tensors.TensorSpan<T> tensor, int dimension) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Unsqueeze<T>(this System.Numerics.Tensors.Tensor<T> tensor, int dimension) { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Xor<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Xor<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static System.Numerics.Tensors.Tensor<T> Xor<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
        public static ref readonly System.Numerics.Tensors.TensorSpan<T> Xor<T>(scoped in System.Numerics.Tensors.ReadOnlyTensorSpan<T> x, T y, in System.Numerics.Tensors.TensorSpan<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { throw null; }
    }
    public readonly ref partial struct TensorDimensionSpan<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public System.Numerics.Tensors.TensorSpan<T> this[nint index] { get { throw null; } }
        public bool IsDense { get { throw null; } }
        public nint Length { get { throw null; } }
        public static implicit operator System.Numerics.Tensors.ReadOnlyTensorDimensionSpan<T>(scoped in System.Numerics.Tensors.TensorDimensionSpan<T> tensorDimension) { throw null; }
        public System.Numerics.Tensors.TensorDimensionSpan<T>.Enumerator GetEnumerator() { throw null; }
        public ref partial struct Enumerator
        {
            private object _dummy;
            private int _dummyPrimitive;
            public readonly System.Numerics.Tensors.TensorSpan<T> Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            public void Reset() { }
        }
    }
    public static partial class TensorPrimitives
    {
        public static void Abs<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static void Acosh<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { }
        public static void AcosPi<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void Acos<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void AddMultiply<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.ReadOnlySpan<T> multiplier, System.Span<T> destination) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IMultiplyOperators<T, T, T> { }
        public static void AddMultiply<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, T multiplier, System.Span<T> destination) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IMultiplyOperators<T, T, T> { }
        public static void AddMultiply<T>(System.ReadOnlySpan<T> x, T y, System.ReadOnlySpan<T> multiplier, System.Span<T> destination) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IMultiplyOperators<T, T, T> { }
        public static void Add<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T> { }
        public static void Add<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T> { }
        public static void Asinh<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { }
        public static void AsinPi<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void Asin<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void Atan2Pi<T>(System.ReadOnlySpan<T> y, System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Atan2Pi<T>(System.ReadOnlySpan<T> y, T x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Atan2Pi<T>(T y, System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Atan2<T>(System.ReadOnlySpan<T> y, System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Atan2<T>(System.ReadOnlySpan<T> y, T x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Atan2<T>(T y, System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Atanh<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { }
        public static void AtanPi<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void Atan<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static T Average<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void BitDecrement<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void BitIncrement<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void BitwiseAnd<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { }
        public static void BitwiseAnd<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { }
        public static void BitwiseOr<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { }
        public static void BitwiseOr<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { }
        public static void Cbrt<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IRootFunctions<T> { }
        public static void Ceiling<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPoint<T> { }
        public static void Clamp<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> min, System.ReadOnlySpan<T> max, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void Clamp<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> min, T max, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void Clamp<T>(System.ReadOnlySpan<T> x, T min, System.ReadOnlySpan<T> max, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void Clamp<T>(System.ReadOnlySpan<T> x, T min, T max, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void Clamp<T>(T x, System.ReadOnlySpan<T> min, System.ReadOnlySpan<T> max, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void Clamp<T>(T x, System.ReadOnlySpan<T> min, T max, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void Clamp<T>(T x, T min, System.ReadOnlySpan<T> max, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void ConvertChecked<TFrom, TTo>(System.ReadOnlySpan<TFrom> source, System.Span<TTo> destination) where TFrom : System.Numerics.INumberBase<TFrom> where TTo : System.Numerics.INumberBase<TTo> { }
        public static void ConvertSaturating<TFrom, TTo>(System.ReadOnlySpan<TFrom> source, System.Span<TTo> destination) where TFrom : System.Numerics.INumberBase<TFrom> where TTo : System.Numerics.INumberBase<TTo> { }
        public static void ConvertToHalf(System.ReadOnlySpan<float> source, System.Span<System.Half> destination) { }
        public static void ConvertToSingle(System.ReadOnlySpan<System.Half> source, System.Span<float> destination) { }
        public static void ConvertTruncating<TFrom, TTo>(System.ReadOnlySpan<TFrom> source, System.Span<TTo> destination) where TFrom : System.Numerics.INumberBase<TFrom> where TTo : System.Numerics.INumberBase<TTo> { }
        public static void CopySign<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> sign, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void CopySign<T>(System.ReadOnlySpan<T> x, T sign, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void Cosh<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { }
        public static T CosineSimilarity<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static void CosPi<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void Cos<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void Decrement<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IDecrementOperators<T> { }
        public static void DegreesToRadians<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static T Distance<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static void Divide<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IDivisionOperators<T, T, T> { }
        public static void Divide<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.IDivisionOperators<T, T, T> { }
        public static void Divide<T>(T x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IDivisionOperators<T, T, T> { }
        public static void DivRem<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> quotientDestination, System.Span<T> remainderDestination) where T : System.Numerics.IBinaryInteger<T> { }
        public static void DivRem<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> quotientDestination, System.Span<T> remainderDestination) where T : System.Numerics.IBinaryInteger<T> { }
        public static void DivRem<T>(T x, System.ReadOnlySpan<T> y, System.Span<T> quotientDestination, System.Span<T> remainderDestination) where T : System.Numerics.IBinaryInteger<T> { }
        public static T Dot<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T>, System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IMultiplicativeIdentity<T, T> { throw null; }
        public static void Exp10M1<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IExponentialFunctions<T> { }
        public static void Exp10<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IExponentialFunctions<T> { }
        public static void Exp2M1<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IExponentialFunctions<T> { }
        public static void Exp2<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IExponentialFunctions<T> { }
        public static void ExpM1<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IExponentialFunctions<T> { }
        public static void Exp<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IExponentialFunctions<T> { }
        public static void Floor<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPoint<T> { }
        public static void FusedMultiplyAdd<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.ReadOnlySpan<T> addend, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void FusedMultiplyAdd<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, T addend, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void FusedMultiplyAdd<T>(System.ReadOnlySpan<T> x, T y, System.ReadOnlySpan<T> addend, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static long HammingBitDistance<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static int HammingDistance<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y) { throw null; }
        public static void Hypot<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IRootFunctions<T> { }
        public static void Ieee754Remainder<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Ieee754Remainder<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Ieee754Remainder<T>(T x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void ILogB<T>(System.ReadOnlySpan<T> x, System.Span<int> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Increment<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IIncrementOperators<T> { }
        public static int IndexOfMaxMagnitude<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static int IndexOfMax<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static int IndexOfMinMagnitude<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static int IndexOfMin<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static bool IsCanonicalAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsCanonicalAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsCanonical<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsComplexNumberAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsComplexNumberAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsComplexNumber<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsEvenIntegerAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsEvenIntegerAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsEvenInteger<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsFiniteAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsFiniteAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsFinite<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsImaginaryNumberAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsImaginaryNumberAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsImaginaryNumber<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsInfinityAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsInfinityAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsInfinity<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsIntegerAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsIntegerAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsInteger<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsNaNAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsNaNAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsNaN<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsNegativeAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsNegativeAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsNegativeInfinityAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsNegativeInfinityAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsNegativeInfinity<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static void IsNegative<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsNormalAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsNormalAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsNormal<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsOddIntegerAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsOddIntegerAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsOddInteger<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsPositiveAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsPositiveAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsPositiveInfinityAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsPositiveInfinityAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsPositiveInfinity<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static void IsPositive<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsPow2All<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.IBinaryNumber<T> { throw null; }
        public static bool IsPow2Any<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.IBinaryNumber<T> { throw null; }
        public static void IsPow2<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.IBinaryNumber<T> { }
        public static bool IsRealNumberAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsRealNumberAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsRealNumber<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsSubnormalAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsSubnormalAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsSubnormal<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static bool IsZeroAll<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static bool IsZeroAny<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void IsZero<T>(System.ReadOnlySpan<T> x, System.Span<bool> destination) where T : System.Numerics.INumberBase<T> { }
        public static void LeadingZeroCount<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IBinaryInteger<T> { }
        public static void Lerp<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.ReadOnlySpan<T> amount, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Lerp<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, T amount, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Lerp<T>(System.ReadOnlySpan<T> x, T y, System.ReadOnlySpan<T> amount, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Log10P1<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { }
        public static void Log10<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { }
        public static void Log2P1<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { }
        public static void Log2<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { }
        public static void LogP1<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { }
        public static void Log<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { }
        public static void Log<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { }
        public static void Log<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.ILogarithmicFunctions<T> { }
        public static T MaxMagnitudeNumber<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void MaxMagnitudeNumber<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static void MaxMagnitudeNumber<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static T MaxMagnitude<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void MaxMagnitude<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static void MaxMagnitude<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static T MaxNumber<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static void MaxNumber<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void MaxNumber<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static T Max<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static void Max<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void Max<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static T MinMagnitudeNumber<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void MinMagnitudeNumber<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static void MinMagnitudeNumber<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static T MinMagnitude<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static void MinMagnitude<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static void MinMagnitude<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static T MinNumber<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static void MinNumber<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void MinNumber<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static T Min<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumber<T> { throw null; }
        public static void Min<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void Min<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.INumber<T> { }
        public static void MultiplyAddEstimate<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.ReadOnlySpan<T> addend, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static void MultiplyAddEstimate<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, T addend, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static void MultiplyAddEstimate<T>(System.ReadOnlySpan<T> x, T y, System.ReadOnlySpan<T> addend, System.Span<T> destination) where T : System.Numerics.INumberBase<T> { }
        public static void MultiplyAdd<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.ReadOnlySpan<T> addend, System.Span<T> destination) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IMultiplyOperators<T, T, T> { }
        public static void MultiplyAdd<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, T addend, System.Span<T> destination) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IMultiplyOperators<T, T, T> { }
        public static void MultiplyAdd<T>(System.ReadOnlySpan<T> x, T y, System.ReadOnlySpan<T> addend, System.Span<T> destination) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IMultiplyOperators<T, T, T> { }
        public static void Multiply<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IMultiplicativeIdentity<T, T> { }
        public static void Multiply<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IMultiplicativeIdentity<T, T> { }
        public static void Negate<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IUnaryNegationOperators<T, T> { }
        public static T Norm<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static void OnesComplement<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { }
        public static long PopCount<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.IBinaryInteger<T> { throw null; }
        public static void PopCount<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IBinaryInteger<T> { }
        public static void Pow<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IPowerFunctions<T> { }
        public static void Pow<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.IPowerFunctions<T> { }
        public static void Pow<T>(T x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IPowerFunctions<T> { }
        public static T ProductOfDifferences<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y) where T : System.Numerics.ISubtractionOperators<T, T, T>, System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IMultiplicativeIdentity<T, T> { throw null; }
        public static T ProductOfSums<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T>, System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IMultiplicativeIdentity<T, T> { throw null; }
        public static T Product<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IMultiplicativeIdentity<T, T> { throw null; }
        public static void RadiansToDegrees<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void ReciprocalEstimate<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void ReciprocalSqrtEstimate<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void ReciprocalSqrt<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void Reciprocal<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPoint<T> { }
        public static void Remainder<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IModulusOperators<T, T, T> { }
        public static void Remainder<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.IModulusOperators<T, T, T> { }
        public static void Remainder<T>(T x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IModulusOperators<T, T, T> { }
        public static void RootN<T>(System.ReadOnlySpan<T> x, int n, System.Span<T> destination) where T : System.Numerics.IRootFunctions<T> { }
        public static void RotateLeft<T>(System.ReadOnlySpan<T> x, int rotateAmount, System.Span<T> destination) where T : System.Numerics.IBinaryInteger<T> { }
        public static void RotateRight<T>(System.ReadOnlySpan<T> x, int rotateAmount, System.Span<T> destination) where T : System.Numerics.IBinaryInteger<T> { }
        public static void Round<T>(System.ReadOnlySpan<T> x, int digits, System.MidpointRounding mode, System.Span<T> destination) where T : System.Numerics.IFloatingPoint<T> { }
        public static void Round<T>(System.ReadOnlySpan<T> x, int digits, System.Span<T> destination) where T : System.Numerics.IFloatingPoint<T> { }
        public static void Round<T>(System.ReadOnlySpan<T> x, System.MidpointRounding mode, System.Span<T> destination) where T : System.Numerics.IFloatingPoint<T> { }
        public static void Round<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPoint<T> { }
        public static void ScaleB<T>(System.ReadOnlySpan<T> x, int n, System.Span<T> destination) where T : System.Numerics.IFloatingPointIeee754<T> { }
        public static void ShiftLeft<T>(System.ReadOnlySpan<T> x, int shiftAmount, System.Span<T> destination) where T : System.Numerics.IShiftOperators<T, int, T> { }
        public static void ShiftRightArithmetic<T>(System.ReadOnlySpan<T> x, int shiftAmount, System.Span<T> destination) where T : System.Numerics.IShiftOperators<T, int, T> { }
        public static void ShiftRightLogical<T>(System.ReadOnlySpan<T> x, int shiftAmount, System.Span<T> destination) where T : System.Numerics.IShiftOperators<T, int, T> { }
        public static void Sigmoid<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IExponentialFunctions<T> { }
        public static void Sign<T>(System.ReadOnlySpan<T> x, System.Span<int> destination) where T : System.Numerics.INumber<T> { }
        public static void SinCosPi<T>(System.ReadOnlySpan<T> x, System.Span<T> sinPiDestination, System.Span<T> cosPiDestination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void SinCos<T>(System.ReadOnlySpan<T> x, System.Span<T> sinDestination, System.Span<T> cosDestination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void Sinh<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { }
        public static void SinPi<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void Sin<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void SoftMax<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IExponentialFunctions<T> { }
        public static void Sqrt<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IRootFunctions<T> { }
        public static T StdDev<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.IRootFunctions<T> { throw null; }
        public static void Subtract<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.ISubtractionOperators<T, T, T> { }
        public static void Subtract<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.ISubtractionOperators<T, T, T> { }
        public static void Subtract<T>(T x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.ISubtractionOperators<T, T, T> { }
        public static T SumOfMagnitudes<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.INumberBase<T> { throw null; }
        public static T SumOfSquares<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T>, System.Numerics.IMultiplyOperators<T, T, T> { throw null; }
        public static T Sum<T>(System.ReadOnlySpan<T> x) where T : System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.IAdditiveIdentity<T, T> { throw null; }
        public static void Tanh<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IHyperbolicFunctions<T> { }
        public static void TanPi<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void Tan<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.ITrigonometricFunctions<T> { }
        public static void TrailingZeroCount<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IBinaryInteger<T> { }
        public static void Truncate<T>(System.ReadOnlySpan<T> x, System.Span<T> destination) where T : System.Numerics.IFloatingPoint<T> { }
        public static void Xor<T>(System.ReadOnlySpan<T> x, System.ReadOnlySpan<T> y, System.Span<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { }
        public static void Xor<T>(System.ReadOnlySpan<T> x, T y, System.Span<T> destination) where T : System.Numerics.IBitwiseOperators<T, T, T> { }
    }
    public readonly ref partial struct TensorSpan<T>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public TensorSpan(System.Array? array) { throw null; }
        public TensorSpan(System.Array? array, scoped System.ReadOnlySpan<int> start, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public TensorSpan(System.Span<T> span) { throw null; }
        public TensorSpan(System.Span<T> span, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        public TensorSpan(System.Span<T> span, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe TensorSpan(T* data, nint dataLength) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe TensorSpan(T* data, nint dataLength, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe TensorSpan(T* data, nint dataLength, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public TensorSpan(T[]? array) { throw null; }
        public TensorSpan(T[]? array, int start, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public TensorSpan(T[]? array, scoped System.ReadOnlySpan<nint> lengths) { throw null; }
        public TensorSpan(T[]? array, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides) { throw null; }
        public static System.Numerics.Tensors.TensorSpan<T> Empty { get { throw null; } }
        public nint FlattenedLength { get { throw null; } }
        public bool HasAnyDenseDimensions { get { throw null; } }
        public bool IsDense { get { throw null; } }
        public bool IsEmpty { get { throw null; } }
        public bool IsPinned { get { throw null; } }
        public ref T this[params scoped System.ReadOnlySpan<System.Buffers.NIndex> indexes] { get { throw null; } }
        public System.Numerics.Tensors.TensorSpan<T> this[params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges] { get { throw null; } set { } }
        public ref T this[params scoped System.ReadOnlySpan<nint> indexes] { get { throw null; } }
        [System.Diagnostics.CodeAnalysis.UnscopedRefAttribute]
        public System.ReadOnlySpan<nint> Lengths { get { throw null; } }
        public int Rank { get { throw null; } }
        [System.Diagnostics.CodeAnalysis.UnscopedRefAttribute]
        public System.ReadOnlySpan<nint> Strides { get { throw null; } }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan() { throw null; }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan(params scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes) { throw null; }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan(params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges) { throw null; }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan(params scoped System.ReadOnlySpan<nint> startIndexes) { throw null; }
        public void Clear() { }
        public void CopyTo(scoped in System.Numerics.Tensors.TensorSpan<T> destination) { }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.ObsoleteAttribute("Equals() on TensorSpan will always throw an exception. Use the equality operator instead.")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override bool Equals(object? obj) { throw null; }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        public void Fill(T value) { }
        public void FlattenTo(scoped System.Span<T> destination) { }
        public System.Numerics.Tensors.TensorDimensionSpan<T> GetDimensionSpan(int dimension) { throw null; }
        public System.Numerics.Tensors.TensorSpan<T>.Enumerator GetEnumerator() { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.ObsoleteAttribute("GetHashCode() on TensorSpan will always throw an exception.")]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        public override int GetHashCode() { throw null; }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public ref T GetPinnableReference() { throw null; }
        public System.Span<T> GetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length) { throw null; }
        public System.Span<T> GetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length) { throw null; }
        public static bool operator ==(in System.Numerics.Tensors.TensorSpan<T> left, in System.Numerics.Tensors.TensorSpan<T> right) { throw null; }
        public static implicit operator System.Numerics.Tensors.ReadOnlyTensorSpan<T> (scoped in System.Numerics.Tensors.TensorSpan<T> tensor) { throw null; }
        public static implicit operator System.Numerics.Tensors.TensorSpan<T> (T[]? array) { throw null; }
        public static bool operator !=(in System.Numerics.Tensors.TensorSpan<T> left, in System.Numerics.Tensors.TensorSpan<T> right) { throw null; }
        public System.Numerics.Tensors.TensorSpan<T> Slice(params scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes) { throw null; }
        public System.Numerics.Tensors.TensorSpan<T> Slice(params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges) { throw null; }
        public System.Numerics.Tensors.TensorSpan<T> Slice(params scoped System.ReadOnlySpan<nint> startIndexes) { throw null; }
        public override string ToString() { throw null; }
        public bool TryCopyTo(scoped in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        public bool TryFlattenTo(scoped System.Span<T> destination) { throw null; }
        public bool TryGetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length, out System.Span<T> span) { throw null; }
        public bool TryGetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length, out System.Span<T> span) { throw null; }
        public bool TryGetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length, out System.ReadOnlySpan<T> span) { throw null; }
        public bool TryGetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length, out System.ReadOnlySpan<T> span) { throw null; }
        public ref partial struct Enumerator : System.Collections.Generic.IEnumerator<T>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public readonly ref T Current { get { throw null; } }
            readonly T System.Collections.Generic.IEnumerator<T>.Current { get { throw null; } }
            readonly object? System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            public void Reset() { }
            readonly void System.IDisposable.Dispose() { }
        }
    }
    public sealed partial class Tensor<T> : System.Collections.IEnumerable, System.Collections.Generic.IEnumerable<T>, System.Numerics.Tensors.IReadOnlyTensor, System.Numerics.Tensors.IReadOnlyTensor<System.Numerics.Tensors.Tensor<T>, T>, System.Numerics.Tensors.ITensor, System.Numerics.Tensors.ITensor<System.Numerics.Tensors.Tensor<T>, T>
    {
        internal Tensor() { }
        public static System.Numerics.Tensors.Tensor<T> Empty { get { throw null; } }
        public nint FlattenedLength { get { throw null; } }
        public bool HasAnyDenseDimensions { get { throw null; } }
        public bool IsDense { get { throw null; } }
        public bool IsEmpty { get { throw null; } }
        public bool IsPinned { get { throw null; } }
        public ref T this[params scoped System.ReadOnlySpan<System.Buffers.NIndex> indexes] { get { throw null; } }
        public System.Numerics.Tensors.Tensor<T> this[params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges] { get { throw null; } set { } }
        public ref T this[params scoped System.ReadOnlySpan<nint> indexes] { get { throw null; } }
        public System.ReadOnlySpan<nint> Lengths { get { throw null; } }
        public int Rank { get { throw null; } }
        public System.ReadOnlySpan<nint> Strides { get { throw null; } }
        object? System.Numerics.Tensors.IReadOnlyTensor.this[params scoped System.ReadOnlySpan<System.Buffers.NIndex> indexes] { get { throw null; } }
        object? System.Numerics.Tensors.IReadOnlyTensor.this[params scoped System.ReadOnlySpan<nint> indexes] { get { throw null; } }
        ref readonly T System.Numerics.Tensors.IReadOnlyTensor<System.Numerics.Tensors.Tensor<T>, T>.this[params scoped System.ReadOnlySpan<System.Buffers.NIndex> indexes] { get { throw null; } }
        ref readonly T System.Numerics.Tensors.IReadOnlyTensor<System.Numerics.Tensors.Tensor<T>, T>.this[params scoped System.ReadOnlySpan<nint> indexes] { get { throw null; } }
        bool System.Numerics.Tensors.ITensor.IsReadOnly { get { throw null; } }
        object? System.Numerics.Tensors.ITensor.this[params scoped System.ReadOnlySpan<System.Buffers.NIndex> indexes] { get { throw null; } set { } }
        object? System.Numerics.Tensors.ITensor.this[params scoped System.ReadOnlySpan<nint> indexes] { get { throw null; } set { } }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan() { throw null; }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan(params scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes) { throw null; }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan(params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges) { throw null; }
        public System.Numerics.Tensors.ReadOnlyTensorSpan<T> AsReadOnlyTensorSpan(params scoped System.ReadOnlySpan<nint> startIndexes) { throw null; }
        public System.Numerics.Tensors.TensorSpan<T> AsTensorSpan() { throw null; }
        public System.Numerics.Tensors.TensorSpan<T> AsTensorSpan(params scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes) { throw null; }
        public System.Numerics.Tensors.TensorSpan<T> AsTensorSpan(params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges) { throw null; }
        public System.Numerics.Tensors.TensorSpan<T> AsTensorSpan(params scoped System.ReadOnlySpan<nint> startIndexes) { throw null; }
        public void Clear() { }
        public void CopyTo(scoped in System.Numerics.Tensors.TensorSpan<T> destination) { }
        public void Fill(T value) { }
        public void FlattenTo(scoped System.Span<T> destination) { }
        public System.Numerics.Tensors.TensorDimensionSpan<T> GetDimensionSpan(int dimension) { throw null; }
        public System.Numerics.Tensors.Tensor<T>.Enumerator GetEnumerator() { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public ref T GetPinnableReference() { throw null; }
        public System.Buffers.MemoryHandle GetPinnedHandle() { throw null; }
        public System.Span<T> GetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length) { throw null; }
        public System.Span<T> GetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length) { throw null; }
        public static implicit operator System.Numerics.Tensors.ReadOnlyTensorSpan<T> (System.Numerics.Tensors.Tensor<T> tensor) { throw null; }
        public static implicit operator System.Numerics.Tensors.TensorSpan<T> (System.Numerics.Tensors.Tensor<T> tensor) { throw null; }
        public static implicit operator System.Numerics.Tensors.Tensor<T> (T[] array) { throw null; }
        public System.Numerics.Tensors.Tensor<T> Slice(params scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes) { throw null; }
        public System.Numerics.Tensors.Tensor<T> Slice(params scoped System.ReadOnlySpan<System.Buffers.NRange> ranges) { throw null; }
        public System.Numerics.Tensors.Tensor<T> Slice(params scoped System.ReadOnlySpan<nint> startIndexes) { throw null; }
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        void System.Numerics.Tensors.IReadOnlyTensor<System.Numerics.Tensors.Tensor<T>, T>.CopyTo(scoped in System.Numerics.Tensors.TensorSpan<T> destination) { }
        System.Numerics.Tensors.ReadOnlyTensorDimensionSpan<T> System.Numerics.Tensors.IReadOnlyTensor<System.Numerics.Tensors.Tensor<T>, T>.GetDimensionSpan(int dimension) { throw null; }
        ref readonly T System.Numerics.Tensors.IReadOnlyTensor<System.Numerics.Tensors.Tensor<T>, T>.GetPinnableReference() { throw null; }
        System.ReadOnlySpan<T> System.Numerics.Tensors.IReadOnlyTensor<System.Numerics.Tensors.Tensor<T>, T>.GetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length) { throw null; }
        System.ReadOnlySpan<T> System.Numerics.Tensors.IReadOnlyTensor<System.Numerics.Tensors.Tensor<T>, T>.GetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length) { throw null; }
        bool System.Numerics.Tensors.IReadOnlyTensor<System.Numerics.Tensors.Tensor<T>, T>.TryCopyTo(scoped in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        void System.Numerics.Tensors.ITensor.Fill(object value) { }
        static System.Numerics.Tensors.Tensor<T> System.Numerics.Tensors.ITensor<System.Numerics.Tensors.Tensor<T>, T>.CreateFromShape(scoped System.ReadOnlySpan<nint> lengths, bool pinned) { throw null; }
        static System.Numerics.Tensors.Tensor<T> System.Numerics.Tensors.ITensor<System.Numerics.Tensors.Tensor<T>, T>.CreateFromShape(scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides, bool pinned) { throw null; }
        static System.Numerics.Tensors.Tensor<T> System.Numerics.Tensors.ITensor<System.Numerics.Tensors.Tensor<T>, T>.CreateFromShapeUninitialized(scoped System.ReadOnlySpan<nint> lengths, bool pinned) { throw null; }
        static System.Numerics.Tensors.Tensor<T> System.Numerics.Tensors.ITensor<System.Numerics.Tensors.Tensor<T>, T>.CreateFromShapeUninitialized(scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides, bool pinned) { throw null; }
        public System.Numerics.Tensors.Tensor<T> ToDenseTensor() { throw null; }
        public string ToString(params scoped System.ReadOnlySpan<nint> maximumLengths) { throw null; }
        public bool TryCopyTo(scoped in System.Numerics.Tensors.TensorSpan<T> destination) { throw null; }
        public bool TryFlattenTo(scoped System.Span<T> destination) { throw null; }
        public bool TryGetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length, out System.Span<T> span) { throw null; }
        public bool TryGetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length, out System.Span<T> span) { throw null; }
        public bool TryGetSpan(scoped System.ReadOnlySpan<nint> startIndexes, int length, out System.ReadOnlySpan<T> span) { throw null; }
        public bool TryGetSpan(scoped System.ReadOnlySpan<System.Buffers.NIndex> startIndexes, int length, out System.ReadOnlySpan<T> span) { throw null; }
        public partial struct Enumerator : System.Collections.Generic.IEnumerator<T>, System.Collections.IEnumerator, System.IDisposable
        {
            private object _dummy;
            private int _dummyPrimitive;
            public readonly ref T Current { get { throw null; } }
            readonly T System.Collections.Generic.IEnumerator<T>.Current { get { throw null; } }
            readonly object? System.Collections.IEnumerator.Current { get { throw null; } }
            public bool MoveNext() { throw null; }
            public void Reset() { }
            void System.IDisposable.Dispose() { }
        }
    }
}
namespace System.Runtime.InteropServices
{
    public static class TensorMarshal
    {
        public static System.Numerics.Tensors.TensorSpan<T> CreateTensorSpan<T>(ref T data, nint dataLength, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides, bool pinned) { throw null; }
        public static System.Numerics.Tensors.ReadOnlyTensorSpan<T> CreateReadOnlyTensorSpan<T>(ref readonly T data, nint dataLength, scoped System.ReadOnlySpan<nint> lengths, scoped System.ReadOnlySpan<nint> strides, bool pinned) { throw null; }
        public static ref T GetReference<T>(in System.Numerics.Tensors.TensorSpan<T> tensorSpan) { throw null; }
        public static ref readonly T GetReference<T>(in System.Numerics.Tensors.ReadOnlyTensorSpan<T> tensorSpan) { throw null; }
    }
}
