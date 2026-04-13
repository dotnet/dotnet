// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: InternalsVisibleTo("System.Private.Reflection.Extensibility, PublicKey=002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293")]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: AssemblyTitle("System.Runtime")]
[assembly: AssemblyDescription("System.Runtime")]
[assembly: AssemblyDefaultAlias("System.Runtime")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System
{
    public delegate void Action();
    public delegate void Action<in T>(T obj);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);
    public delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);
    public delegate void Action<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);
    public delegate void Action<in T1, in T2, in T3, in T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
    public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
    public static partial class Activator
    {
        public static Object CreateInstance(Type type, Boolean nonPublic) { throw null; }

        public static Object CreateInstance(Type type, params Object[] args) { throw null; }

        public static Object CreateInstance(Type type) { throw null; }

        public static T CreateInstance<T>() { throw null; }
    }

    public partial class ArgumentException : Exception
    {
        public ArgumentException() { }

        public ArgumentException(String message, Exception innerException) { }

        public ArgumentException(String message, String paramName, Exception innerException) { }

        public ArgumentException(String message, String paramName) { }

        public ArgumentException(String message) { }

        public override String Message { get { throw null; } }

        public virtual String ParamName { get { throw null; } }
    }

    public partial class ArgumentNullException : ArgumentException
    {
        public ArgumentNullException() { }

        public ArgumentNullException(String message, Exception innerException) { }

        public ArgumentNullException(String paramName, String message) { }

        public ArgumentNullException(String paramName) { }
    }

    public partial class ArgumentOutOfRangeException : ArgumentException
    {
        public ArgumentOutOfRangeException() { }

        public ArgumentOutOfRangeException(String message, Exception innerException) { }

        public ArgumentOutOfRangeException(String paramName, Object actualValue, String message) { }

        public ArgumentOutOfRangeException(String paramName, String message) { }

        public ArgumentOutOfRangeException(String paramName) { }

        public virtual Object ActualValue { get { throw null; } }

        public override String Message { get { throw null; } }
    }

    public partial class ArithmeticException : Exception
    {
        public ArithmeticException() { }

        public ArithmeticException(String message, Exception innerException) { }

        public ArithmeticException(String message) { }
    }

    public abstract partial class Array : Collections.ICollection, Collections.IEnumerable, Collections.IList, Collections.IStructuralComparable, Collections.IStructuralEquatable
    {
        internal Array() { }

        public Int32 Length { get { throw null; } }

        public Int32 Rank { get { throw null; } }

        Int32 Collections.ICollection.Count { get { throw null; } }

        Boolean Collections.ICollection.IsSynchronized { get { throw null; } }

        Object Collections.ICollection.SyncRoot { get { throw null; } }

        Boolean Collections.IList.IsFixedSize { get { throw null; } }

        Boolean Collections.IList.IsReadOnly { get { throw null; } }

        Object Collections.IList.this[Int32 index] { get { throw null; } set { } }

        public static Int32 BinarySearch(Array array, Int32 index, Int32 length, Object value, Collections.IComparer comparer) { throw null; }

        public static Int32 BinarySearch(Array array, Int32 index, Int32 length, Object value) { throw null; }

        public static Int32 BinarySearch(Array array, Object value, Collections.IComparer comparer) { throw null; }

        public static Int32 BinarySearch(Array array, Object value) { throw null; }

        public static Int32 BinarySearch<T>(T[] array, T value, Collections.Generic.IComparer<T> comparer) { throw null; }

        public static Int32 BinarySearch<T>(T[] array, T value) { throw null; }

        public static Int32 BinarySearch<T>(T[] array, Int32 index, Int32 length, T value, Collections.Generic.IComparer<T> comparer) { throw null; }

        public static Int32 BinarySearch<T>(T[] array, Int32 index, Int32 length, T value) { throw null; }

        public static void Clear(Array array, Int32 index, Int32 length) { }

        public Object Clone() { throw null; }

        public static void ConstrainedCopy(Array sourceArray, Int32 sourceIndex, Array destinationArray, Int32 destinationIndex, Int32 length) { }

        public static void Copy(Array sourceArray, Array destinationArray, Int32 length) { }

        public static void Copy(Array sourceArray, Int32 sourceIndex, Array destinationArray, Int32 destinationIndex, Int32 length) { }

        public void CopyTo(Array array, Int32 index) { }

        public static Array CreateInstance(Type elementType, Int32 length) { throw null; }

        public static Array CreateInstance(Type elementType, Int32[] lengths, Int32[] lowerBounds) { throw null; }

        public static Array CreateInstance(Type elementType, params Int32[] lengths) { throw null; }

        public static T[] Empty<T>() { throw null; }

        public static Boolean Exists<T>(T[] array, Predicate<T> match) { throw null; }

        public static T Find<T>(T[] array, Predicate<T> match) { throw null; }

        public static T[] FindAll<T>(T[] array, Predicate<T> match) { throw null; }

        public static Int32 FindIndex<T>(T[] array, Int32 startIndex, Int32 count, Predicate<T> match) { throw null; }

        public static Int32 FindIndex<T>(T[] array, Int32 startIndex, Predicate<T> match) { throw null; }

        public static Int32 FindIndex<T>(T[] array, Predicate<T> match) { throw null; }

        public static T FindLast<T>(T[] array, Predicate<T> match) { throw null; }

        public static Int32 FindLastIndex<T>(T[] array, Int32 startIndex, Int32 count, Predicate<T> match) { throw null; }

        public static Int32 FindLastIndex<T>(T[] array, Int32 startIndex, Predicate<T> match) { throw null; }

        public static Int32 FindLastIndex<T>(T[] array, Predicate<T> match) { throw null; }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        public Int32 GetLength(Int32 dimension) { throw null; }

        public Int32 GetLowerBound(Int32 dimension) { throw null; }

        public Int32 GetUpperBound(Int32 dimension) { throw null; }

        public Object GetValue(Int32 index) { throw null; }

        public Object GetValue(params Int32[] indices) { throw null; }

        public static Int32 IndexOf(Array array, Object value, Int32 startIndex, Int32 count) { throw null; }

        public static Int32 IndexOf(Array array, Object value, Int32 startIndex) { throw null; }

        public static Int32 IndexOf(Array array, Object value) { throw null; }

        public static Int32 IndexOf<T>(T[] array, T value, Int32 startIndex, Int32 count) { throw null; }

        public static Int32 IndexOf<T>(T[] array, T value, Int32 startIndex) { throw null; }

        public static Int32 IndexOf<T>(T[] array, T value) { throw null; }

        public void Initialize() { }

        public static Int32 LastIndexOf(Array array, Object value, Int32 startIndex, Int32 count) { throw null; }

        public static Int32 LastIndexOf(Array array, Object value, Int32 startIndex) { throw null; }

        public static Int32 LastIndexOf(Array array, Object value) { throw null; }

        public static Int32 LastIndexOf<T>(T[] array, T value, Int32 startIndex, Int32 count) { throw null; }

        public static Int32 LastIndexOf<T>(T[] array, T value, Int32 startIndex) { throw null; }

        public static Int32 LastIndexOf<T>(T[] array, T value) { throw null; }

        public static void Resize<T>(ref T[] array, Int32 newSize) { }

        public static void Reverse(Array array, Int32 index, Int32 length) { }

        public static void Reverse(Array array) { }

        public void SetValue(Object value, Int32 index) { }

        public void SetValue(Object value, params Int32[] indices) { }

        public static void Sort(Array keys, Array items, Collections.IComparer comparer) { }

        public static void Sort(Array keys, Array items, Int32 index, Int32 length, Collections.IComparer comparer) { }

        public static void Sort(Array keys, Array items, Int32 index, Int32 length) { }

        public static void Sort(Array keys, Array items) { }

        public static void Sort(Array array, Collections.IComparer comparer) { }

        public static void Sort(Array array, Int32 index, Int32 length, Collections.IComparer comparer) { }

        public static void Sort(Array array, Int32 index, Int32 length) { }

        public static void Sort(Array array) { }

        public static void Sort<T>(T[] array, Collections.Generic.IComparer<T> comparer) { }

        public static void Sort<T>(T[] array, Comparison<T> comparison) { }

        public static void Sort<T>(T[] array, Int32 index, Int32 length, Collections.Generic.IComparer<T> comparer) { }

        public static void Sort<T>(T[] array, Int32 index, Int32 length) { }

        public static void Sort<T>(T[] array) { }

        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, Collections.Generic.IComparer<TKey> comparer) { }

        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, Int32 index, Int32 length, Collections.Generic.IComparer<TKey> comparer) { }

        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, Int32 index, Int32 length) { }

        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items) { }

        Int32 Collections.IList.Add(Object value) { throw null; }

        void Collections.IList.Clear() { }

        Boolean Collections.IList.Contains(Object value) { throw null; }

        Int32 Collections.IList.IndexOf(Object value) { throw null; }

        void Collections.IList.Insert(Int32 index, Object value) { }

        void Collections.IList.Remove(Object value) { }

        void Collections.IList.RemoveAt(Int32 index) { }

        Int32 Collections.IStructuralComparable.CompareTo(Object other, Collections.IComparer comparer) { throw null; }

        Boolean Collections.IStructuralEquatable.Equals(Object other, Collections.IEqualityComparer comparer) { throw null; }

        Int32 Collections.IStructuralEquatable.GetHashCode(Collections.IEqualityComparer comparer) { throw null; }

        public static Boolean TrueForAll<T>(T[] array, Predicate<T> match) { throw null; }
    }

    public partial struct ArraySegment<T> : Collections.Generic.ICollection<T>, Collections.Generic.IEnumerable<T>, Collections.IEnumerable, Collections.Generic.IList<T>, Collections.Generic.IReadOnlyCollection<T>, Collections.Generic.IReadOnlyList<T>
    {
        public ArraySegment(T[] array, Int32 offset, Int32 count) { }

        public ArraySegment(T[] array) { }

        public T[] Array { get { throw null; } }

        public Int32 Count { get { throw null; } }

        public Int32 Offset { get { throw null; } }

        Boolean Collections.Generic.ICollection<T>.IsReadOnly { get { throw null; } }

        T Collections.Generic.IList<T>.this[Int32 index] { get { throw null; } set { } }

        T Collections.Generic.IReadOnlyList<T>.this[Int32 index] { get { throw null; } }

        public Boolean Equals(ArraySegment<T> obj) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Boolean operator ==(ArraySegment<T> a, ArraySegment<T> b) { throw null; }

        public static Boolean operator !=(ArraySegment<T> a, ArraySegment<T> b) { throw null; }

        void Collections.Generic.ICollection<T>.Add(T item) { }

        void Collections.Generic.ICollection<T>.Clear() { }

        Boolean Collections.Generic.ICollection<T>.Contains(T item) { throw null; }

        void Collections.Generic.ICollection<T>.CopyTo(T[] array, Int32 arrayIndex) { }

        Boolean Collections.Generic.ICollection<T>.Remove(T item) { throw null; }

        Collections.Generic.IEnumerator<T> Collections.Generic.IEnumerable<T>.GetEnumerator() { throw null; }

        Int32 Collections.Generic.IList<T>.IndexOf(T item) { throw null; }

        void Collections.Generic.IList<T>.Insert(Int32 index, T item) { }

        void Collections.Generic.IList<T>.RemoveAt(Int32 index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }
    }

    public partial class ArrayTypeMismatchException : Exception
    {
        public ArrayTypeMismatchException() { }

        public ArrayTypeMismatchException(String message, Exception innerException) { }

        public ArrayTypeMismatchException(String message) { }
    }

    public delegate void AsyncCallback(IAsyncResult ar);
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public abstract partial class Attribute
    {
        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }
    }

    [Flags]
    public enum AttributeTargets
    {
        Assembly = 1,
        Module = 2,
        Class = 4,
        Struct = 8,
        Enum = 16,
        Constructor = 32,
        Method = 64,
        Property = 128,
        Field = 256,
        Event = 512,
        Interface = 1024,
        Parameter = 2048,
        Delegate = 4096,
        ReturnValue = 8192,
        GenericParameter = 16384,
        All = 32767
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed partial class AttributeUsageAttribute : Attribute
    {
        public AttributeUsageAttribute(AttributeTargets validOn) { }

        public Boolean AllowMultiple { get { throw null; } set { } }

        public Boolean Inherited { get { throw null; } set { } }

        public AttributeTargets ValidOn { get { throw null; } }
    }

    public partial class BadImageFormatException : Exception
    {
        public BadImageFormatException() { }

        public BadImageFormatException(String message, Exception inner) { }

        public BadImageFormatException(String message, String fileName, Exception inner) { }

        public BadImageFormatException(String message, String fileName) { }

        public BadImageFormatException(String message) { }

        public String FileName { get { throw null; } }

        public override String Message { get { throw null; } }

        public override String ToString() { throw null; }
    }

    public partial struct Boolean : IComparable, IComparable<Boolean>, IConvertible, IEquatable<Boolean>
    {
        public static readonly String FalseString;
        public static readonly String TrueString;
        public Int32 CompareTo(Boolean value) { throw null; }

        public Boolean Equals(Boolean obj) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Boolean Parse(String value) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        String IConvertible.ToString(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public static Boolean TryParse(String value, out Boolean result) { throw null; }
    }

    public static partial class Buffer
    {
        public static void BlockCopy(Array src, Int32 srcOffset, Array dst, Int32 dstOffset, Int32 count) { }

        public static Int32 ByteLength(Array array) { throw null; }

        public static Byte GetByte(Array array, Int32 index) { throw null; }

        [CLSCompliant(false)]
        public static unsafe void MemoryCopy(void* source, void* destination, Int64 destinationSizeInBytes, Int64 sourceBytesToCopy) { }

        [CLSCompliant(false)]
        public static unsafe void MemoryCopy(void* source, void* destination, UInt64 destinationSizeInBytes, UInt64 sourceBytesToCopy) { }

        public static void SetByte(Array array, Int32 index, Byte value) { }
    }

    public partial struct Byte : IComparable, IComparable<Byte>, IConvertible, IEquatable<Byte>, IFormattable
    {
        public const Byte MaxValue = 255;
        public const Byte MinValue = 0;
        public Int32 CompareTo(Byte value) { throw null; }

        public Boolean Equals(Byte obj) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Byte Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        public static Byte Parse(String s, Globalization.NumberStyles style) { throw null; }

        public static Byte Parse(String s, IFormatProvider provider) { throw null; }

        public static Byte Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        public static Boolean TryParse(String s, out Byte result) { throw null; }

        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out Byte result) { throw null; }
    }

    public partial struct Char : IComparable, IComparable<Char>, IConvertible, IEquatable<Char>
    {
        public const Char MaxValue = '\uffff';
        public const Char MinValue = '\0';
        public Int32 CompareTo(Char value) { throw null; }

        public static String ConvertFromUtf32(Int32 utf32) { throw null; }

        public static Int32 ConvertToUtf32(Char highSurrogate, Char lowSurrogate) { throw null; }

        public static Int32 ConvertToUtf32(String s, Int32 index) { throw null; }

        public Boolean Equals(Char obj) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Double GetNumericValue(Char c) { throw null; }

        public static Double GetNumericValue(String s, Int32 index) { throw null; }

        public static Boolean IsControl(Char c) { throw null; }

        public static Boolean IsControl(String s, Int32 index) { throw null; }

        public static Boolean IsDigit(Char c) { throw null; }

        public static Boolean IsDigit(String s, Int32 index) { throw null; }

        public static Boolean IsHighSurrogate(Char c) { throw null; }

        public static Boolean IsHighSurrogate(String s, Int32 index) { throw null; }

        public static Boolean IsLetter(Char c) { throw null; }

        public static Boolean IsLetter(String s, Int32 index) { throw null; }

        public static Boolean IsLetterOrDigit(Char c) { throw null; }

        public static Boolean IsLetterOrDigit(String s, Int32 index) { throw null; }

        public static Boolean IsLower(Char c) { throw null; }

        public static Boolean IsLower(String s, Int32 index) { throw null; }

        public static Boolean IsLowSurrogate(Char c) { throw null; }

        public static Boolean IsLowSurrogate(String s, Int32 index) { throw null; }

        public static Boolean IsNumber(Char c) { throw null; }

        public static Boolean IsNumber(String s, Int32 index) { throw null; }

        public static Boolean IsPunctuation(Char c) { throw null; }

        public static Boolean IsPunctuation(String s, Int32 index) { throw null; }

        public static Boolean IsSeparator(Char c) { throw null; }

        public static Boolean IsSeparator(String s, Int32 index) { throw null; }

        public static Boolean IsSurrogate(Char c) { throw null; }

        public static Boolean IsSurrogate(String s, Int32 index) { throw null; }

        public static Boolean IsSurrogatePair(Char highSurrogate, Char lowSurrogate) { throw null; }

        public static Boolean IsSurrogatePair(String s, Int32 index) { throw null; }

        public static Boolean IsSymbol(Char c) { throw null; }

        public static Boolean IsSymbol(String s, Int32 index) { throw null; }

        public static Boolean IsUpper(Char c) { throw null; }

        public static Boolean IsUpper(String s, Int32 index) { throw null; }

        public static Boolean IsWhiteSpace(Char c) { throw null; }

        public static Boolean IsWhiteSpace(String s, Int32 index) { throw null; }

        public static Char Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        String IConvertible.ToString(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public static Char ToLower(Char c) { throw null; }

        public static Char ToLowerInvariant(Char c) { throw null; }

        public override String ToString() { throw null; }

        public static String ToString(Char c) { throw null; }

        public static Char ToUpper(Char c) { throw null; }

        public static Char ToUpperInvariant(Char c) { throw null; }

        public static Boolean TryParse(String s, out Char result) { throw null; }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public sealed partial class CLSCompliantAttribute : Attribute
    {
        public CLSCompliantAttribute(Boolean isCompliant) { }

        public Boolean IsCompliant { get { throw null; } }
    }

    public delegate Int32 Comparison<in T>(T x, T y);
    public partial struct DateTime : IComparable, IComparable<DateTime>, IConvertible, IEquatable<DateTime>, IFormattable
    {
        public static readonly DateTime MaxValue;
        public static readonly DateTime MinValue;
        public DateTime(Int32 year, Int32 month, Int32 day, Int32 hour, Int32 minute, Int32 second, DateTimeKind kind) { }

        public DateTime(Int32 year, Int32 month, Int32 day, Int32 hour, Int32 minute, Int32 second, Int32 millisecond, DateTimeKind kind) { }

        public DateTime(Int32 year, Int32 month, Int32 day, Int32 hour, Int32 minute, Int32 second, Int32 millisecond) { }

        public DateTime(Int32 year, Int32 month, Int32 day, Int32 hour, Int32 minute, Int32 second) { }

        public DateTime(Int32 year, Int32 month, Int32 day) { }

        public DateTime(Int64 ticks, DateTimeKind kind) { }

        public DateTime(Int64 ticks) { }

        public DateTime Date { get { throw null; } }

        public Int32 Day { get { throw null; } }

        public DayOfWeek DayOfWeek { get { throw null; } }

        public Int32 DayOfYear { get { throw null; } }

        public Int32 Hour { get { throw null; } }

        public DateTimeKind Kind { get { throw null; } }

        public Int32 Millisecond { get { throw null; } }

        public Int32 Minute { get { throw null; } }

        public Int32 Month { get { throw null; } }

        public static DateTime Now { get { throw null; } }

        public Int32 Second { get { throw null; } }

        public Int64 Ticks { get { throw null; } }

        public TimeSpan TimeOfDay { get { throw null; } }

        public static DateTime Today { get { throw null; } }

        public static DateTime UtcNow { get { throw null; } }

        public Int32 Year { get { throw null; } }

        public DateTime Add(TimeSpan value) { throw null; }

        public DateTime AddDays(Double value) { throw null; }

        public DateTime AddHours(Double value) { throw null; }

        public DateTime AddMilliseconds(Double value) { throw null; }

        public DateTime AddMinutes(Double value) { throw null; }

        public DateTime AddMonths(Int32 months) { throw null; }

        public DateTime AddSeconds(Double value) { throw null; }

        public DateTime AddTicks(Int64 value) { throw null; }

        public DateTime AddYears(Int32 value) { throw null; }

        public static Int32 Compare(DateTime t1, DateTime t2) { throw null; }

        public Int32 CompareTo(DateTime value) { throw null; }

        public static Int32 DaysInMonth(Int32 year, Int32 month) { throw null; }

        public static Boolean Equals(DateTime t1, DateTime t2) { throw null; }

        public Boolean Equals(DateTime value) { throw null; }

        public override Boolean Equals(Object value) { throw null; }

        public static DateTime FromBinary(Int64 dateData) { throw null; }

        public static DateTime FromFileTime(Int64 fileTime) { throw null; }

        public static DateTime FromFileTimeUtc(Int64 fileTime) { throw null; }

        public String[] GetDateTimeFormats() { throw null; }

        public String[] GetDateTimeFormats(Char format, IFormatProvider provider) { throw null; }

        public String[] GetDateTimeFormats(Char format) { throw null; }

        public String[] GetDateTimeFormats(IFormatProvider provider) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public Boolean IsDaylightSavingTime() { throw null; }

        public static Boolean IsLeapYear(Int32 year) { throw null; }

        public static DateTime operator +(DateTime d, TimeSpan t) { throw null; }

        public static Boolean operator ==(DateTime d1, DateTime d2) { throw null; }

        public static Boolean operator >(DateTime t1, DateTime t2) { throw null; }

        public static Boolean operator >=(DateTime t1, DateTime t2) { throw null; }

        public static Boolean operator !=(DateTime d1, DateTime d2) { throw null; }

        public static Boolean operator <(DateTime t1, DateTime t2) { throw null; }

        public static Boolean operator <=(DateTime t1, DateTime t2) { throw null; }

        public static TimeSpan operator -(DateTime d1, DateTime d2) { throw null; }

        public static DateTime operator -(DateTime d, TimeSpan t) { throw null; }

        public static DateTime Parse(String s, IFormatProvider provider, Globalization.DateTimeStyles styles) { throw null; }

        public static DateTime Parse(String s, IFormatProvider provider) { throw null; }

        public static DateTime Parse(String s) { throw null; }

        public static DateTime ParseExact(String s, String format, IFormatProvider provider, Globalization.DateTimeStyles style) { throw null; }

        public static DateTime ParseExact(String s, String format, IFormatProvider provider) { throw null; }

        public static DateTime ParseExact(String s, String[] formats, IFormatProvider provider, Globalization.DateTimeStyles style) { throw null; }

        public static DateTime SpecifyKind(DateTime value, DateTimeKind kind) { throw null; }

        public TimeSpan Subtract(DateTime value) { throw null; }

        public DateTime Subtract(TimeSpan value) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public Int64 ToBinary() { throw null; }

        public Int64 ToFileTime() { throw null; }

        public Int64 ToFileTimeUtc() { throw null; }

        public DateTime ToLocalTime() { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        public DateTime ToUniversalTime() { throw null; }

        public static Boolean TryParse(String s, out DateTime result) { throw null; }

        public static Boolean TryParse(String s, IFormatProvider provider, Globalization.DateTimeStyles styles, out DateTime result) { throw null; }

        public static Boolean TryParseExact(String s, String format, IFormatProvider provider, Globalization.DateTimeStyles style, out DateTime result) { throw null; }

        public static Boolean TryParseExact(String s, String[] formats, IFormatProvider provider, Globalization.DateTimeStyles style, out DateTime result) { throw null; }
    }

    public enum DateTimeKind
    {
        Unspecified = 0,
        Utc = 1,
        Local = 2
    }

    public partial struct DateTimeOffset : IComparable, IComparable<DateTimeOffset>, IEquatable<DateTimeOffset>, IFormattable
    {
        public static readonly DateTimeOffset MaxValue;
        public static readonly DateTimeOffset MinValue;
        public DateTimeOffset(DateTime dateTime, TimeSpan offset) { }

        public DateTimeOffset(DateTime dateTime) { }

        public DateTimeOffset(Int32 year, Int32 month, Int32 day, Int32 hour, Int32 minute, Int32 second, Int32 millisecond, TimeSpan offset) { }

        public DateTimeOffset(Int32 year, Int32 month, Int32 day, Int32 hour, Int32 minute, Int32 second, TimeSpan offset) { }

        public DateTimeOffset(Int64 ticks, TimeSpan offset) { }

        public DateTime Date { get { throw null; } }

        public DateTime DateTime { get { throw null; } }

        public Int32 Day { get { throw null; } }

        public DayOfWeek DayOfWeek { get { throw null; } }

        public Int32 DayOfYear { get { throw null; } }

        public Int32 Hour { get { throw null; } }

        public DateTime LocalDateTime { get { throw null; } }

        public Int32 Millisecond { get { throw null; } }

        public Int32 Minute { get { throw null; } }

        public Int32 Month { get { throw null; } }

        public static DateTimeOffset Now { get { throw null; } }

        public TimeSpan Offset { get { throw null; } }

        public Int32 Second { get { throw null; } }

        public Int64 Ticks { get { throw null; } }

        public TimeSpan TimeOfDay { get { throw null; } }

        public DateTime UtcDateTime { get { throw null; } }

        public static DateTimeOffset UtcNow { get { throw null; } }

        public Int64 UtcTicks { get { throw null; } }

        public Int32 Year { get { throw null; } }

        public DateTimeOffset Add(TimeSpan timeSpan) { throw null; }

        public DateTimeOffset AddDays(Double days) { throw null; }

        public DateTimeOffset AddHours(Double hours) { throw null; }

        public DateTimeOffset AddMilliseconds(Double milliseconds) { throw null; }

        public DateTimeOffset AddMinutes(Double minutes) { throw null; }

        public DateTimeOffset AddMonths(Int32 months) { throw null; }

        public DateTimeOffset AddSeconds(Double seconds) { throw null; }

        public DateTimeOffset AddTicks(Int64 ticks) { throw null; }

        public DateTimeOffset AddYears(Int32 years) { throw null; }

        public static Int32 Compare(DateTimeOffset first, DateTimeOffset second) { throw null; }

        public Int32 CompareTo(DateTimeOffset other) { throw null; }

        public static Boolean Equals(DateTimeOffset first, DateTimeOffset second) { throw null; }

        public Boolean Equals(DateTimeOffset other) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public Boolean EqualsExact(DateTimeOffset other) { throw null; }

        public static DateTimeOffset FromFileTime(Int64 fileTime) { throw null; }

        public static DateTimeOffset FromUnixTimeMilliseconds(Int64 milliseconds) { throw null; }

        public static DateTimeOffset FromUnixTimeSeconds(Int64 seconds) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static DateTimeOffset operator +(DateTimeOffset dateTimeOffset, TimeSpan timeSpan) { throw null; }

        public static Boolean operator ==(DateTimeOffset left, DateTimeOffset right) { throw null; }

        public static Boolean operator >(DateTimeOffset left, DateTimeOffset right) { throw null; }

        public static Boolean operator >=(DateTimeOffset left, DateTimeOffset right) { throw null; }

        public static implicit operator DateTimeOffset(DateTime dateTime) { throw null; }

        public static Boolean operator !=(DateTimeOffset left, DateTimeOffset right) { throw null; }

        public static Boolean operator <(DateTimeOffset left, DateTimeOffset right) { throw null; }

        public static Boolean operator <=(DateTimeOffset left, DateTimeOffset right) { throw null; }

        public static TimeSpan operator -(DateTimeOffset left, DateTimeOffset right) { throw null; }

        public static DateTimeOffset operator -(DateTimeOffset dateTimeOffset, TimeSpan timeSpan) { throw null; }

        public static DateTimeOffset Parse(String input, IFormatProvider formatProvider, Globalization.DateTimeStyles styles) { throw null; }

        public static DateTimeOffset Parse(String input, IFormatProvider formatProvider) { throw null; }

        public static DateTimeOffset Parse(String input) { throw null; }

        public static DateTimeOffset ParseExact(String input, String format, IFormatProvider formatProvider, Globalization.DateTimeStyles styles) { throw null; }

        public static DateTimeOffset ParseExact(String input, String format, IFormatProvider formatProvider) { throw null; }

        public static DateTimeOffset ParseExact(String input, String[] formats, IFormatProvider formatProvider, Globalization.DateTimeStyles styles) { throw null; }

        public TimeSpan Subtract(DateTimeOffset value) { throw null; }

        public DateTimeOffset Subtract(TimeSpan value) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        public Int64 ToFileTime() { throw null; }

        public DateTimeOffset ToLocalTime() { throw null; }

        public DateTimeOffset ToOffset(TimeSpan offset) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider formatProvider) { throw null; }

        public String ToString(String format, IFormatProvider formatProvider) { throw null; }

        public String ToString(String format) { throw null; }

        public DateTimeOffset ToUniversalTime() { throw null; }

        public Int64 ToUnixTimeMilliseconds() { throw null; }

        public Int64 ToUnixTimeSeconds() { throw null; }

        public static Boolean TryParse(String input, out DateTimeOffset result) { throw null; }

        public static Boolean TryParse(String input, IFormatProvider formatProvider, Globalization.DateTimeStyles styles, out DateTimeOffset result) { throw null; }

        public static Boolean TryParseExact(String input, String format, IFormatProvider formatProvider, Globalization.DateTimeStyles styles, out DateTimeOffset result) { throw null; }

        public static Boolean TryParseExact(String input, String[] formats, IFormatProvider formatProvider, Globalization.DateTimeStyles styles, out DateTimeOffset result) { throw null; }
    }

    public enum DayOfWeek
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

    public partial struct Decimal : IComparable, IComparable<Decimal>, IConvertible, IEquatable<Decimal>, IFormattable
    {
        public const Decimal MaxValue = 79228162514264337593543950335M;
        public const Decimal MinusOne = -1M;
        public const Decimal MinValue = -79228162514264337593543950335M;
        public const Decimal One = 1M;
        public const Decimal Zero = 0M;
        public Decimal(Double value) { }

        public Decimal(Int32 lo, Int32 mid, Int32 hi, Boolean isNegative, Byte scale) { }

        public Decimal(Int32 value) { }

        public Decimal(Int32[] bits) { }

        public Decimal(Int64 value) { }

        public Decimal(Single value) { }

        [CLSCompliant(false)]
        public Decimal(UInt32 value) { }

        [CLSCompliant(false)]
        public Decimal(UInt64 value) { }

        public static Decimal Add(Decimal d1, Decimal d2) { throw null; }

        public static Decimal Ceiling(Decimal d) { throw null; }

        public static Int32 Compare(Decimal d1, Decimal d2) { throw null; }

        public Int32 CompareTo(Decimal value) { throw null; }

        public static Decimal Divide(Decimal d1, Decimal d2) { throw null; }

        public static Boolean Equals(Decimal d1, Decimal d2) { throw null; }

        public Boolean Equals(Decimal value) { throw null; }

        public override Boolean Equals(Object value) { throw null; }

        public static Decimal Floor(Decimal d) { throw null; }

        public static Int32[] GetBits(Decimal d) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Decimal Multiply(Decimal d1, Decimal d2) { throw null; }

        public static Decimal Negate(Decimal d) { throw null; }

        public static Decimal operator +(Decimal d1, Decimal d2) { throw null; }

        public static Decimal operator --(Decimal d) { throw null; }

        public static Decimal operator /(Decimal d1, Decimal d2) { throw null; }

        public static Boolean operator ==(Decimal d1, Decimal d2) { throw null; }

        public static explicit operator Byte(Decimal value) { throw null; }

        public static explicit operator Char(Decimal value) { throw null; }

        public static explicit operator Double(Decimal value) { throw null; }

        public static explicit operator Int16(Decimal value) { throw null; }

        public static explicit operator Int32(Decimal value) { throw null; }

        public static explicit operator Int64(Decimal value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator SByte(Decimal value) { throw null; }

        public static explicit operator Single(Decimal value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator UInt16(Decimal value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator UInt32(Decimal value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator UInt64(Decimal value) { throw null; }

        public static explicit operator Decimal(Double value) { throw null; }

        public static explicit operator Decimal(Single value) { throw null; }

        public static Boolean operator >(Decimal d1, Decimal d2) { throw null; }

        public static Boolean operator >=(Decimal d1, Decimal d2) { throw null; }

        public static implicit operator Decimal(Byte value) { throw null; }

        public static implicit operator Decimal(Char value) { throw null; }

        public static implicit operator Decimal(Int16 value) { throw null; }

        public static implicit operator Decimal(Int32 value) { throw null; }

        public static implicit operator Decimal(Int64 value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator Decimal(SByte value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator Decimal(UInt16 value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator Decimal(UInt32 value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator Decimal(UInt64 value) { throw null; }

        public static Decimal operator ++(Decimal d) { throw null; }

        public static Boolean operator !=(Decimal d1, Decimal d2) { throw null; }

        public static Boolean operator <(Decimal d1, Decimal d2) { throw null; }

        public static Boolean operator <=(Decimal d1, Decimal d2) { throw null; }

        public static Decimal operator %(Decimal d1, Decimal d2) { throw null; }

        public static Decimal operator *(Decimal d1, Decimal d2) { throw null; }

        public static Decimal operator -(Decimal d1, Decimal d2) { throw null; }

        public static Decimal operator -(Decimal d) { throw null; }

        public static Decimal operator +(Decimal d) { throw null; }

        public static Decimal Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        public static Decimal Parse(String s, Globalization.NumberStyles style) { throw null; }

        public static Decimal Parse(String s, IFormatProvider provider) { throw null; }

        public static Decimal Parse(String s) { throw null; }

        public static Decimal Remainder(Decimal d1, Decimal d2) { throw null; }

        public static Decimal Subtract(Decimal d1, Decimal d2) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public static Byte ToByte(Decimal value) { throw null; }

        public static Double ToDouble(Decimal d) { throw null; }

        public static Int16 ToInt16(Decimal value) { throw null; }

        public static Int32 ToInt32(Decimal d) { throw null; }

        public static Int64 ToInt64(Decimal d) { throw null; }

        [CLSCompliant(false)]
        public static SByte ToSByte(Decimal value) { throw null; }

        public static Single ToSingle(Decimal d) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        [CLSCompliant(false)]
        public static UInt16 ToUInt16(Decimal value) { throw null; }

        [CLSCompliant(false)]
        public static UInt32 ToUInt32(Decimal d) { throw null; }

        [CLSCompliant(false)]
        public static UInt64 ToUInt64(Decimal d) { throw null; }

        public static Decimal Truncate(Decimal d) { throw null; }

        public static Boolean TryParse(String s, out Decimal result) { throw null; }

        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out Decimal result) { throw null; }
    }

    public abstract partial class Delegate
    {
        internal Delegate() { }

        public Object Target { get { throw null; } }

        public static Delegate Combine(Delegate a, Delegate b) { throw null; }

        public static Delegate Combine(params Delegate[] delegates) { throw null; }

        public Object DynamicInvoke(params Object[] args) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public virtual Delegate[] GetInvocationList() { throw null; }

        public static Boolean operator ==(Delegate d1, Delegate d2) { throw null; }

        public static Boolean operator !=(Delegate d1, Delegate d2) { throw null; }

        public static Delegate Remove(Delegate source, Delegate value) { throw null; }

        public static Delegate RemoveAll(Delegate source, Delegate value) { throw null; }
    }

    public partial class DivideByZeroException : ArithmeticException
    {
        public DivideByZeroException() { }

        public DivideByZeroException(String message, Exception innerException) { }

        public DivideByZeroException(String message) { }
    }

    public partial struct Double : IComparable, IComparable<Double>, IConvertible, IEquatable<Double>, IFormattable
    {
        public const Double Epsilon = 5E-324D;
        public const Double MaxValue = 1.7976931348623157E+308D;
        public const Double MinValue = -1.7976931348623157E+308D;
        public const Double NaN = 0D / 0D;
        public const Double NegativeInfinity = -1D / 0D;
        public const Double PositiveInfinity = 1D / 0D;
        public Int32 CompareTo(Double value) { throw null; }

        public Boolean Equals(Double obj) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Boolean IsInfinity(Double d) { throw null; }

        public static Boolean IsNaN(Double d) { throw null; }

        public static Boolean IsNegativeInfinity(Double d) { throw null; }

        public static Boolean IsPositiveInfinity(Double d) { throw null; }

        public static Boolean operator ==(Double left, Double right) { throw null; }

        public static Boolean operator >(Double left, Double right) { throw null; }

        public static Boolean operator >=(Double left, Double right) { throw null; }

        public static Boolean operator !=(Double left, Double right) { throw null; }

        public static Boolean operator <(Double left, Double right) { throw null; }

        public static Boolean operator <=(Double left, Double right) { throw null; }

        public static Double Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        public static Double Parse(String s, Globalization.NumberStyles style) { throw null; }

        public static Double Parse(String s, IFormatProvider provider) { throw null; }

        public static Double Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        public static Boolean TryParse(String s, out Double result) { throw null; }

        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out Double result) { throw null; }
    }

    public abstract partial class Enum : ValueType, IComparable, IConvertible, IFormattable
    {
        public Int32 CompareTo(Object target) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public static String Format(Type enumType, Object value, String format) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static String GetName(Type enumType, Object value) { throw null; }

        public static String[] GetNames(Type enumType) { throw null; }

        public static Type GetUnderlyingType(Type enumType) { throw null; }

        public static Array GetValues(Type enumType) { throw null; }

        public Boolean HasFlag(Enum flag) { throw null; }

        public static Boolean IsDefined(Type enumType, Object value) { throw null; }

        public static Object Parse(Type enumType, String value, Boolean ignoreCase) { throw null; }

        public static Object Parse(Type enumType, String value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        [Obsolete("The provider argument is not used. Please use ToString().")]
        String IConvertible.ToString(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        [Obsolete("The provider argument is not used. Please use ToString(String).")]
        String IFormattable.ToString(String format, IFormatProvider provider) { throw null; }

        public static Object ToObject(Type enumType, Object value) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(String format) { throw null; }

        public static Boolean TryParse<TEnum>(String value, out TEnum result)
            where TEnum : struct { throw null; }

        public static Boolean TryParse<TEnum>(String value, Boolean ignoreCase, out TEnum result)
            where TEnum : struct { throw null; }
    }

    public partial class EventArgs
    {
        public static readonly EventArgs Empty;
    }

    public delegate void EventHandler(Object sender, EventArgs e);
    public delegate void EventHandler<TEventArgs>(Object sender, TEventArgs e);
    public partial class Exception
    {
        public Exception() { }

        public Exception(String message, Exception innerException) { }

        public Exception(String message) { }

        public virtual Collections.IDictionary Data { get { throw null; } }

        public virtual String HelpLink { get { throw null; } set { } }

        public Int32 HResult { get { throw null; } protected set { } }

        public Exception InnerException { get { throw null; } }

        public virtual String Message { get { throw null; } }

        public virtual String Source { get { throw null; } set { } }

        public virtual String StackTrace { get { throw null; } }

        public virtual Exception GetBaseException() { throw null; }

        public override String ToString() { throw null; }
    }

    public partial class FieldAccessException : MemberAccessException
    {
        public FieldAccessException() { }

        public FieldAccessException(String message, Exception inner) { }

        public FieldAccessException(String message) { }
    }

    [AttributeUsage(AttributeTargets.Enum, Inherited = false)]
    public partial class FlagsAttribute : Attribute
    {
    }

    public partial class FormatException : Exception
    {
        public FormatException() { }

        public FormatException(String message, Exception innerException) { }

        public FormatException(String message) { }
    }

    public abstract partial class FormattableString : IFormattable
    {
        public abstract Int32 ArgumentCount { get; }
        public abstract String Format { get; }

        public abstract Object GetArgument(Int32 index);
        public abstract Object[] GetArguments();
        public static String Invariant(FormattableString formattable) { throw null; }

        String IFormattable.ToString(String ignored, IFormatProvider formatProvider) { throw null; }

        public override String ToString() { throw null; }

        public abstract String ToString(IFormatProvider formatProvider);
    }

    public delegate TResult Func<out TResult>();
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);
    public delegate TResult Func<in T, out TResult>(T arg);
    public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);
    public delegate TResult Func<in T1, in T2, in T3, out TResult>(T1 arg1, T2 arg2, T3 arg3);
    public delegate TResult Func<in T1, in T2, in T3, in T4, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    public delegate TResult Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
    public static partial class GC
    {
        public static Int32 MaxGeneration { get { throw null; } }

        public static void AddMemoryPressure(Int64 bytesAllocated) { }

        public static void Collect() { }

        public static void Collect(Int32 generation, GCCollectionMode mode, Boolean blocking) { }

        public static void Collect(Int32 generation, GCCollectionMode mode) { }

        public static void Collect(Int32 generation) { }

        public static Int32 CollectionCount(Int32 generation) { throw null; }

        public static Int32 GetGeneration(Object obj) { throw null; }

        public static Int64 GetTotalMemory(Boolean forceFullCollection) { throw null; }

        public static void KeepAlive(Object obj) { }

        public static void RemoveMemoryPressure(Int64 bytesAllocated) { }

        public static void ReRegisterForFinalize(Object obj) { }

        public static void SuppressFinalize(Object obj) { }

        public static void WaitForPendingFinalizers() { }
    }

    public enum GCCollectionMode
    {
        Default = 0,
        Forced = 1,
        Optimized = 2
    }

    public partial struct Guid : IComparable, IComparable<Guid>, IEquatable<Guid>, IFormattable
    {
        public static readonly Guid Empty;
        public Guid(Byte[] b) { }

        public Guid(Int32 a, Int16 b, Int16 c, Byte d, Byte e, Byte f, Byte g, Byte h, Byte i, Byte j, Byte k) { }

        public Guid(Int32 a, Int16 b, Int16 c, Byte[] d) { }

        public Guid(String g) { }

        [CLSCompliant(false)]
        public Guid(UInt32 a, UInt16 b, UInt16 c, Byte d, Byte e, Byte f, Byte g, Byte h, Byte i, Byte j, Byte k) { }

        public Int32 CompareTo(Guid value) { throw null; }

        public Boolean Equals(Guid g) { throw null; }

        public override Boolean Equals(Object o) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Guid NewGuid() { throw null; }

        public static Boolean operator ==(Guid a, Guid b) { throw null; }

        public static Boolean operator !=(Guid a, Guid b) { throw null; }

        public static Guid Parse(String input) { throw null; }

        public static Guid ParseExact(String input, String format) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        String IFormattable.ToString(String format, IFormatProvider provider) { throw null; }

        public Byte[] ToByteArray() { throw null; }

        public override String ToString() { throw null; }

        public String ToString(String format) { throw null; }

        public static Boolean TryParse(String input, out Guid result) { throw null; }

        public static Boolean TryParseExact(String input, String format, out Guid result) { throw null; }
    }

    public partial interface IAsyncResult
    {
        Object AsyncState { get; }

        Threading.WaitHandle AsyncWaitHandle { get; }

        Boolean CompletedSynchronously { get; }

        Boolean IsCompleted { get; }
    }

    public partial interface IComparable
    {
        Int32 CompareTo(Object obj);
    }

    public partial interface IComparable<in T>
    {
        Int32 CompareTo(T other);
    }

    [CLSCompliant(false)]
    public partial interface IConvertible
    {
        TypeCode GetTypeCode();
        Boolean ToBoolean(IFormatProvider provider);
        Byte ToByte(IFormatProvider provider);
        Char ToChar(IFormatProvider provider);
        DateTime ToDateTime(IFormatProvider provider);
        Decimal ToDecimal(IFormatProvider provider);
        Double ToDouble(IFormatProvider provider);
        Int16 ToInt16(IFormatProvider provider);
        Int32 ToInt32(IFormatProvider provider);
        Int64 ToInt64(IFormatProvider provider);
        SByte ToSByte(IFormatProvider provider);
        Single ToSingle(IFormatProvider provider);
        String ToString(IFormatProvider provider);
        Object ToType(Type conversionType, IFormatProvider provider);
        UInt16 ToUInt16(IFormatProvider provider);
        UInt32 ToUInt32(IFormatProvider provider);
        UInt64 ToUInt64(IFormatProvider provider);
    }

    public partial interface ICustomFormatter
    {
        String Format(String format, Object arg, IFormatProvider formatProvider);
    }

    public partial interface IDisposable
    {
        void Dispose();
    }

    public partial interface IEquatable<T>
    {
        Boolean Equals(T other);
    }

    public partial interface IFormatProvider
    {
        Object GetFormat(Type formatType);
    }

    public partial interface IFormattable
    {
        String ToString(String format, IFormatProvider formatProvider);
    }

    public sealed partial class IndexOutOfRangeException : Exception
    {
        public IndexOutOfRangeException() { }

        public IndexOutOfRangeException(String message, Exception innerException) { }

        public IndexOutOfRangeException(String message) { }
    }

    public sealed partial class InsufficientExecutionStackException : Exception
    {
        public InsufficientExecutionStackException() { }

        public InsufficientExecutionStackException(String message, Exception innerException) { }

        public InsufficientExecutionStackException(String message) { }
    }

    public partial struct Int16 : IComparable, IComparable<Int16>, IConvertible, IEquatable<Int16>, IFormattable
    {
        public const Int16 MaxValue = 32767;
        public const Int16 MinValue = -32768;
        public Int32 CompareTo(Int16 value) { throw null; }

        public Boolean Equals(Int16 obj) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Int16 Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        public static Int16 Parse(String s, Globalization.NumberStyles style) { throw null; }

        public static Int16 Parse(String s, IFormatProvider provider) { throw null; }

        public static Int16 Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out Int16 result) { throw null; }

        public static Boolean TryParse(String s, out Int16 result) { throw null; }
    }

    public partial struct Int32 : IComparable, IComparable<Int32>, IConvertible, IEquatable<Int32>, IFormattable
    {
        public const Int32 MaxValue = 2147483647;
        public const Int32 MinValue = -2147483648;
        public Int32 CompareTo(Int32 value) { throw null; }

        public Boolean Equals(Int32 obj) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Int32 Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        public static Int32 Parse(String s, Globalization.NumberStyles style) { throw null; }

        public static Int32 Parse(String s, IFormatProvider provider) { throw null; }

        public static Int32 Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out Int32 result) { throw null; }

        public static Boolean TryParse(String s, out Int32 result) { throw null; }
    }

    public partial struct Int64 : IComparable, IComparable<Int64>, IConvertible, IEquatable<Int64>, IFormattable
    {
        public const Int64 MaxValue = 9223372036854775807L;
        public const Int64 MinValue = -9223372036854775808;
        public Int32 CompareTo(Int64 value) { throw null; }

        public Boolean Equals(Int64 obj) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Int64 Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        public static Int64 Parse(String s, Globalization.NumberStyles style) { throw null; }

        public static Int64 Parse(String s, IFormatProvider provider) { throw null; }

        public static Int64 Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out Int64 result) { throw null; }

        public static Boolean TryParse(String s, out Int64 result) { throw null; }
    }

    public partial struct IntPtr
    {
        public static readonly IntPtr Zero;
        public IntPtr(Int32 value) { }

        public IntPtr(Int64 value) { }

        [CLSCompliant(false)]
        public unsafe IntPtr(void* value) { }

        public static Int32 Size { get { throw null; } }

        public static IntPtr Add(IntPtr pointer, Int32 offset) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static IntPtr operator +(IntPtr pointer, Int32 offset) { throw null; }

        public static Boolean operator ==(IntPtr value1, IntPtr value2) { throw null; }

        public static explicit operator IntPtr(Int32 value) { throw null; }

        public static explicit operator IntPtr(Int64 value) { throw null; }

        public static explicit operator Int32(IntPtr value) { throw null; }

        public static explicit operator Int64(IntPtr value) { throw null; }

        [CLSCompliant(false)]
        public static unsafe explicit operator void*(IntPtr value) { throw null; }

        [CLSCompliant(false)]
        public static unsafe explicit operator IntPtr(void* value) { throw null; }

        public static Boolean operator !=(IntPtr value1, IntPtr value2) { throw null; }

        public static IntPtr operator -(IntPtr pointer, Int32 offset) { throw null; }

        public static IntPtr Subtract(IntPtr pointer, Int32 offset) { throw null; }

        public Int32 ToInt32() { throw null; }

        public Int64 ToInt64() { throw null; }

        [CLSCompliant(false)]
        public unsafe void* ToPointer() { throw null; }

        public override String ToString() { throw null; }

        public String ToString(String format) { throw null; }
    }

    public partial class InvalidCastException : Exception
    {
        public InvalidCastException() { }

        public InvalidCastException(String message, Exception innerException) { }

        public InvalidCastException(String message, Int32 errorCode) { }

        public InvalidCastException(String message) { }
    }

    public partial class InvalidOperationException : Exception
    {
        public InvalidOperationException() { }

        public InvalidOperationException(String message, Exception innerException) { }

        public InvalidOperationException(String message) { }
    }

    public sealed partial class InvalidProgramException : Exception
    {
        public InvalidProgramException() { }

        public InvalidProgramException(String message, Exception inner) { }

        public InvalidProgramException(String message) { }
    }

    public partial class InvalidTimeZoneException : Exception
    {
        public InvalidTimeZoneException() { }

        public InvalidTimeZoneException(String message, Exception innerException) { }

        public InvalidTimeZoneException(String message) { }
    }

    public partial interface IObservable<out T>
    {
        IDisposable Subscribe(IObserver<T> observer);
    }

    public partial interface IObserver<in T>
    {
        void OnCompleted();
        void OnError(Exception error);
        void OnNext(T value);
    }

    public partial interface IProgress<in T>
    {
        void Report(T value);
    }

    public partial class Lazy<T>
    {
        public Lazy() { }

        public Lazy(Boolean isThreadSafe) { }

        public Lazy(Func<T> valueFactory, Boolean isThreadSafe) { }

        public Lazy(Func<T> valueFactory, Threading.LazyThreadSafetyMode mode) { }

        public Lazy(Func<T> valueFactory) { }

        public Lazy(Threading.LazyThreadSafetyMode mode) { }

        public Boolean IsValueCreated { get { throw null; } }

        public T Value { get { throw null; } }

        public override String ToString() { throw null; }
    }

    public partial class Lazy<T, TMetadata> : Lazy<T>
    {
        public Lazy(TMetadata metadata, Boolean isThreadSafe) { }

        public Lazy(TMetadata metadata, Threading.LazyThreadSafetyMode mode) { }

        public Lazy(TMetadata metadata) { }

        public Lazy(Func<T> valueFactory, TMetadata metadata, Boolean isThreadSafe) { }

        public Lazy(Func<T> valueFactory, TMetadata metadata, Threading.LazyThreadSafetyMode mode) { }

        public Lazy(Func<T> valueFactory, TMetadata metadata) { }

        public TMetadata Metadata { get { throw null; } }
    }

    public partial class MemberAccessException : Exception
    {
        public MemberAccessException() { }

        public MemberAccessException(String message, Exception inner) { }

        public MemberAccessException(String message) { }
    }

    public partial class MethodAccessException : MemberAccessException
    {
        public MethodAccessException() { }

        public MethodAccessException(String message, Exception inner) { }

        public MethodAccessException(String message) { }
    }

    public partial class MissingFieldException : MissingMemberException
    {
        public MissingFieldException() { }

        public MissingFieldException(String message, Exception inner) { }

        public MissingFieldException(String message) { }

        public override String Message { get { throw null; } }
    }

    public partial class MissingMemberException : MemberAccessException
    {
        public MissingMemberException() { }

        public MissingMemberException(String message, Exception inner) { }

        public MissingMemberException(String message) { }

        public override String Message { get { throw null; } }
    }

    public partial class MissingMethodException : MissingMemberException
    {
        public MissingMethodException() { }

        public MissingMethodException(String message, Exception inner) { }

        public MissingMethodException(String message) { }

        public override String Message { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed partial class MTAThreadAttribute : Attribute
    {
    }

    public abstract partial class MulticastDelegate : Delegate
    {
        internal MulticastDelegate() { }

        public sealed override Boolean Equals(Object obj) { throw null; }

        public sealed override Int32 GetHashCode() { throw null; }

        public sealed override Delegate[] GetInvocationList() { throw null; }

        public static Boolean operator ==(MulticastDelegate d1, MulticastDelegate d2) { throw null; }

        public static Boolean operator !=(MulticastDelegate d1, MulticastDelegate d2) { throw null; }
    }

    public partial class NotImplementedException : Exception
    {
        public NotImplementedException() { }

        public NotImplementedException(String message, Exception inner) { }

        public NotImplementedException(String message) { }
    }

    public partial class NotSupportedException : Exception
    {
        public NotSupportedException() { }

        public NotSupportedException(String message, Exception innerException) { }

        public NotSupportedException(String message) { }
    }

    public static partial class Nullable
    {
        public static Int32 Compare<T>(T? n1, T? n2)
            where T : struct { throw null; }

        public static Boolean Equals<T>(T? n1, T? n2)
            where T : struct { throw null; }

        public static Type GetUnderlyingType(Type nullableType) { throw null; }
    }

    public partial struct Nullable<T>
        where T : struct
    {
        public Nullable(T value) { }

        public Boolean HasValue { get { throw null; } }

        public T Value { get { throw null; } }

        public override Boolean Equals(Object other) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public T GetValueOrDefault() { throw null; }

        public T GetValueOrDefault(T defaultValue) { throw null; }

        public static explicit operator T(T? value) { throw null; }

        public static implicit operator T?(T value) { throw null; }

        public override String ToString() { throw null; }
    }

    public partial class NullReferenceException : Exception
    {
        public NullReferenceException() { }

        public NullReferenceException(String message, Exception innerException) { }

        public NullReferenceException(String message) { }
    }

    public partial class Object
    {
        public static Boolean Equals(Object objA, Object objB) { throw null; }

        public virtual Boolean Equals(Object obj) { throw null; }

        ~Object() {
        }

        public virtual Int32 GetHashCode() { throw null; }

        public Type GetType() { throw null; }

        protected Object MemberwiseClone() { throw null; }

        public static Boolean ReferenceEquals(Object objA, Object objB) { throw null; }

        public virtual String ToString() { throw null; }
    }

    public partial class ObjectDisposedException : InvalidOperationException
    {
        public ObjectDisposedException(String message, Exception innerException) { }

        public ObjectDisposedException(String objectName, String message) { }

        public ObjectDisposedException(String objectName) { }

        public override String Message { get { throw null; } }

        public String ObjectName { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
    public sealed partial class ObsoleteAttribute : Attribute
    {
        public ObsoleteAttribute() { }

        public ObsoleteAttribute(String message, Boolean error) { }

        public ObsoleteAttribute(String message) { }

        public Boolean IsError { get { throw null; } }

        public String Message { get { throw null; } }
    }

    public partial class OutOfMemoryException : Exception
    {
        public OutOfMemoryException() { }

        public OutOfMemoryException(String message, Exception innerException) { }

        public OutOfMemoryException(String message) { }
    }

    public partial class OverflowException : ArithmeticException
    {
        public OverflowException() { }

        public OverflowException(String message, Exception innerException) { }

        public OverflowException(String message) { }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed partial class ParamArrayAttribute : Attribute
    {
    }

    public partial class PlatformNotSupportedException : NotSupportedException
    {
        public PlatformNotSupportedException() { }

        public PlatformNotSupportedException(String message, Exception inner) { }

        public PlatformNotSupportedException(String message) { }
    }

    public delegate Boolean Predicate<in T>(T obj);
    public partial class RankException : Exception
    {
        public RankException() { }

        public RankException(String message, Exception innerException) { }

        public RankException(String message) { }
    }

    public partial struct RuntimeFieldHandle
    {
        public override Boolean Equals(Object obj) { throw null; }

        public Boolean Equals(RuntimeFieldHandle handle) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Boolean operator ==(RuntimeFieldHandle left, RuntimeFieldHandle right) { throw null; }

        public static Boolean operator !=(RuntimeFieldHandle left, RuntimeFieldHandle right) { throw null; }
    }

    public partial struct RuntimeMethodHandle
    {
        public override Boolean Equals(Object obj) { throw null; }

        public Boolean Equals(RuntimeMethodHandle handle) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Boolean operator ==(RuntimeMethodHandle left, RuntimeMethodHandle right) { throw null; }

        public static Boolean operator !=(RuntimeMethodHandle left, RuntimeMethodHandle right) { throw null; }
    }

    public partial struct RuntimeTypeHandle
    {
        public override Boolean Equals(Object obj) { throw null; }

        public Boolean Equals(RuntimeTypeHandle handle) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Boolean operator ==(Object left, RuntimeTypeHandle right) { throw null; }

        public static Boolean operator ==(RuntimeTypeHandle left, Object right) { throw null; }

        public static Boolean operator !=(Object left, RuntimeTypeHandle right) { throw null; }

        public static Boolean operator !=(RuntimeTypeHandle left, Object right) { throw null; }
    }

    [CLSCompliant(false)]
    public partial struct SByte : IComparable, IComparable<SByte>, IConvertible, IEquatable<SByte>, IFormattable
    {
        public const SByte MaxValue = 127;
        public const SByte MinValue = -128;
        public Int32 CompareTo(SByte value) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public Boolean Equals(SByte obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        [CLSCompliant(false)]
        public static SByte Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        [CLSCompliant(false)]
        public static SByte Parse(String s, Globalization.NumberStyles style) { throw null; }

        [CLSCompliant(false)]
        public static SByte Parse(String s, IFormatProvider provider) { throw null; }

        [CLSCompliant(false)]
        public static SByte Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        [CLSCompliant(false)]
        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out SByte result) { throw null; }

        [CLSCompliant(false)]
        public static Boolean TryParse(String s, out SByte result) { throw null; }
    }

    public partial struct Single : IComparable, IComparable<Single>, IConvertible, IEquatable<Single>, IFormattable
    {
        public const Single Epsilon = 1E-45F;
        public const Single MaxValue = 3.4028235E+38F;
        public const Single MinValue = -3.4028235E+38F;
        public const Single NaN = 0F / 0F;
        public const Single NegativeInfinity = -1F / 0F;
        public const Single PositiveInfinity = 1F / 0F;
        public Int32 CompareTo(Single value) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public Boolean Equals(Single obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Boolean IsInfinity(Single f) { throw null; }

        public static Boolean IsNaN(Single f) { throw null; }

        public static Boolean IsNegativeInfinity(Single f) { throw null; }

        public static Boolean IsPositiveInfinity(Single f) { throw null; }

        public static Boolean operator ==(Single left, Single right) { throw null; }

        public static Boolean operator >(Single left, Single right) { throw null; }

        public static Boolean operator >=(Single left, Single right) { throw null; }

        public static Boolean operator !=(Single left, Single right) { throw null; }

        public static Boolean operator <(Single left, Single right) { throw null; }

        public static Boolean operator <=(Single left, Single right) { throw null; }

        public static Single Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        public static Single Parse(String s, Globalization.NumberStyles style) { throw null; }

        public static Single Parse(String s, IFormatProvider provider) { throw null; }

        public static Single Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out Single result) { throw null; }

        public static Boolean TryParse(String s, out Single result) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed partial class STAThreadAttribute : Attribute
    {
    }

    public sealed partial class String : Collections.Generic.IEnumerable<Char>, Collections.IEnumerable, IComparable, IComparable<String>, IConvertible, IEquatable<String>
    {
        public static readonly String Empty;
        public String(Char c, Int32 count) { }

        public String(Char[] value, Int32 startIndex, Int32 length) { }

        public String(Char[] value) { }

        [CLSCompliant(false)]
        public unsafe String(Char* value, Int32 startIndex, Int32 length) { }

        [CLSCompliant(false)]
        public unsafe String(Char* value) { }

        public Char this[Int32 index] { get { throw null; } }

        public Int32 Length { get { throw null; } }

        public static Int32 Compare(String strA, Int32 indexA, String strB, Int32 indexB, Int32 length, StringComparison comparisonType) { throw null; }

        public static Int32 Compare(String strA, Int32 indexA, String strB, Int32 indexB, Int32 length) { throw null; }

        public static Int32 Compare(String strA, String strB, Boolean ignoreCase) { throw null; }

        public static Int32 Compare(String strA, String strB, StringComparison comparisonType) { throw null; }

        public static Int32 Compare(String strA, String strB) { throw null; }

        public static Int32 CompareOrdinal(String strA, Int32 indexA, String strB, Int32 indexB, Int32 length) { throw null; }

        public static Int32 CompareOrdinal(String strA, String strB) { throw null; }

        public Int32 CompareTo(String strB) { throw null; }

        public static String Concat(Collections.Generic.IEnumerable<String> values) { throw null; }

        public static String Concat(Object arg0, Object arg1, Object arg2) { throw null; }

        public static String Concat(Object arg0, Object arg1) { throw null; }

        public static String Concat(Object arg0) { throw null; }

        public static String Concat(params Object[] args) { throw null; }

        public static String Concat(String str0, String str1, String str2, String str3) { throw null; }

        public static String Concat(String str0, String str1, String str2) { throw null; }

        public static String Concat(String str0, String str1) { throw null; }

        public static String Concat(params String[] values) { throw null; }

        public static String Concat<T>(Collections.Generic.IEnumerable<T> values) { throw null; }

        public Boolean Contains(String value) { throw null; }

        public void CopyTo(Int32 sourceIndex, Char[] destination, Int32 destinationIndex, Int32 count) { }

        public Boolean EndsWith(String value, StringComparison comparisonType) { throw null; }

        public Boolean EndsWith(String value) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public static Boolean Equals(String a, String b, StringComparison comparisonType) { throw null; }

        public static Boolean Equals(String a, String b) { throw null; }

        public Boolean Equals(String value, StringComparison comparisonType) { throw null; }

        public Boolean Equals(String value) { throw null; }

        public static String Format(IFormatProvider provider, String format, Object arg0, Object arg1, Object arg2) { throw null; }

        public static String Format(IFormatProvider provider, String format, Object arg0, Object arg1) { throw null; }

        public static String Format(IFormatProvider provider, String format, Object arg0) { throw null; }

        public static String Format(IFormatProvider provider, String format, params Object[] args) { throw null; }

        public static String Format(String format, Object arg0, Object arg1, Object arg2) { throw null; }

        public static String Format(String format, Object arg0, Object arg1) { throw null; }

        public static String Format(String format, Object arg0) { throw null; }

        public static String Format(String format, params Object[] args) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public Int32 IndexOf(Char value, Int32 startIndex, Int32 count) { throw null; }

        public Int32 IndexOf(Char value, Int32 startIndex) { throw null; }

        public Int32 IndexOf(Char value) { throw null; }

        public Int32 IndexOf(String value, Int32 startIndex, Int32 count, StringComparison comparisonType) { throw null; }

        public Int32 IndexOf(String value, Int32 startIndex, Int32 count) { throw null; }

        public Int32 IndexOf(String value, Int32 startIndex, StringComparison comparisonType) { throw null; }

        public Int32 IndexOf(String value, Int32 startIndex) { throw null; }

        public Int32 IndexOf(String value, StringComparison comparisonType) { throw null; }

        public Int32 IndexOf(String value) { throw null; }

        public Int32 IndexOfAny(Char[] anyOf, Int32 startIndex, Int32 count) { throw null; }

        public Int32 IndexOfAny(Char[] anyOf, Int32 startIndex) { throw null; }

        public Int32 IndexOfAny(Char[] anyOf) { throw null; }

        public String Insert(Int32 startIndex, String value) { throw null; }

        public static Boolean IsNullOrEmpty(String value) { throw null; }

        public static Boolean IsNullOrWhiteSpace(String value) { throw null; }

        public static String Join(String separator, Collections.Generic.IEnumerable<String> values) { throw null; }

        public static String Join(String separator, params Object[] values) { throw null; }

        public static String Join(String separator, String[] value, Int32 startIndex, Int32 count) { throw null; }

        public static String Join(String separator, params String[] value) { throw null; }

        public static String Join<T>(String separator, Collections.Generic.IEnumerable<T> values) { throw null; }

        public Int32 LastIndexOf(Char value, Int32 startIndex, Int32 count) { throw null; }

        public Int32 LastIndexOf(Char value, Int32 startIndex) { throw null; }

        public Int32 LastIndexOf(Char value) { throw null; }

        public Int32 LastIndexOf(String value, Int32 startIndex, Int32 count, StringComparison comparisonType) { throw null; }

        public Int32 LastIndexOf(String value, Int32 startIndex, Int32 count) { throw null; }

        public Int32 LastIndexOf(String value, Int32 startIndex, StringComparison comparisonType) { throw null; }

        public Int32 LastIndexOf(String value, Int32 startIndex) { throw null; }

        public Int32 LastIndexOf(String value, StringComparison comparisonType) { throw null; }

        public Int32 LastIndexOf(String value) { throw null; }

        public Int32 LastIndexOfAny(Char[] anyOf, Int32 startIndex, Int32 count) { throw null; }

        public Int32 LastIndexOfAny(Char[] anyOf, Int32 startIndex) { throw null; }

        public Int32 LastIndexOfAny(Char[] anyOf) { throw null; }

        public static Boolean operator ==(String a, String b) { throw null; }

        public static Boolean operator !=(String a, String b) { throw null; }

        public String PadLeft(Int32 totalWidth, Char paddingChar) { throw null; }

        public String PadLeft(Int32 totalWidth) { throw null; }

        public String PadRight(Int32 totalWidth, Char paddingChar) { throw null; }

        public String PadRight(Int32 totalWidth) { throw null; }

        public String Remove(Int32 startIndex, Int32 count) { throw null; }

        public String Remove(Int32 startIndex) { throw null; }

        public String Replace(Char oldChar, Char newChar) { throw null; }

        public String Replace(String oldValue, String newValue) { throw null; }

        public String[] Split(Char[] separator, Int32 count, StringSplitOptions options) { throw null; }

        public String[] Split(Char[] separator, Int32 count) { throw null; }

        public String[] Split(Char[] separator, StringSplitOptions options) { throw null; }

        public String[] Split(params Char[] separator) { throw null; }

        public String[] Split(String[] separator, Int32 count, StringSplitOptions options) { throw null; }

        public String[] Split(String[] separator, StringSplitOptions options) { throw null; }

        public Boolean StartsWith(String value, StringComparison comparisonType) { throw null; }

        public Boolean StartsWith(String value) { throw null; }

        public String Substring(Int32 startIndex, Int32 length) { throw null; }

        public String Substring(Int32 startIndex) { throw null; }

        Collections.Generic.IEnumerator<Char> Collections.Generic.IEnumerable<Char>.GetEnumerator() { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        String IConvertible.ToString(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public Char[] ToCharArray() { throw null; }

        public Char[] ToCharArray(Int32 startIndex, Int32 length) { throw null; }

        public String ToLower() { throw null; }

        public String ToLowerInvariant() { throw null; }

        public override String ToString() { throw null; }

        public String ToUpper() { throw null; }

        public String ToUpperInvariant() { throw null; }

        public String Trim() { throw null; }

        public String Trim(params Char[] trimChars) { throw null; }

        public String TrimEnd(params Char[] trimChars) { throw null; }

        public String TrimStart(params Char[] trimChars) { throw null; }
    }

    public enum StringComparison
    {
        CurrentCulture = 0,
        CurrentCultureIgnoreCase = 1,
        Ordinal = 4,
        OrdinalIgnoreCase = 5
    }

    [Flags]
    public enum StringSplitOptions
    {
        None = 0,
        RemoveEmptyEntries = 1
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public partial class ThreadStaticAttribute : Attribute
    {
    }

    public partial class TimeoutException : Exception
    {
        public TimeoutException() { }

        public TimeoutException(String message, Exception innerException) { }

        public TimeoutException(String message) { }
    }

    public partial struct TimeSpan : IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>, IFormattable
    {
        public static readonly TimeSpan MaxValue;
        public static readonly TimeSpan MinValue;
        public const Int64 TicksPerDay = 864000000000L;
        public const Int64 TicksPerHour = 36000000000L;
        public const Int64 TicksPerMillisecond = 10000L;
        public const Int64 TicksPerMinute = 600000000L;
        public const Int64 TicksPerSecond = 10000000L;
        public static readonly TimeSpan Zero;
        public TimeSpan(Int32 days, Int32 hours, Int32 minutes, Int32 seconds, Int32 milliseconds) { }

        public TimeSpan(Int32 days, Int32 hours, Int32 minutes, Int32 seconds) { }

        public TimeSpan(Int32 hours, Int32 minutes, Int32 seconds) { }

        public TimeSpan(Int64 ticks) { }

        public Int32 Days { get { throw null; } }

        public Int32 Hours { get { throw null; } }

        public Int32 Milliseconds { get { throw null; } }

        public Int32 Minutes { get { throw null; } }

        public Int32 Seconds { get { throw null; } }

        public Int64 Ticks { get { throw null; } }

        public Double TotalDays { get { throw null; } }

        public Double TotalHours { get { throw null; } }

        public Double TotalMilliseconds { get { throw null; } }

        public Double TotalMinutes { get { throw null; } }

        public Double TotalSeconds { get { throw null; } }

        public TimeSpan Add(TimeSpan ts) { throw null; }

        public static Int32 Compare(TimeSpan t1, TimeSpan t2) { throw null; }

        public Int32 CompareTo(TimeSpan value) { throw null; }

        public TimeSpan Duration() { throw null; }

        public override Boolean Equals(Object value) { throw null; }

        public static Boolean Equals(TimeSpan t1, TimeSpan t2) { throw null; }

        public Boolean Equals(TimeSpan obj) { throw null; }

        public static TimeSpan FromDays(Double value) { throw null; }

        public static TimeSpan FromHours(Double value) { throw null; }

        public static TimeSpan FromMilliseconds(Double value) { throw null; }

        public static TimeSpan FromMinutes(Double value) { throw null; }

        public static TimeSpan FromSeconds(Double value) { throw null; }

        public static TimeSpan FromTicks(Int64 value) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public TimeSpan Negate() { throw null; }

        public static TimeSpan operator +(TimeSpan t1, TimeSpan t2) { throw null; }

        public static Boolean operator ==(TimeSpan t1, TimeSpan t2) { throw null; }

        public static Boolean operator >(TimeSpan t1, TimeSpan t2) { throw null; }

        public static Boolean operator >=(TimeSpan t1, TimeSpan t2) { throw null; }

        public static Boolean operator !=(TimeSpan t1, TimeSpan t2) { throw null; }

        public static Boolean operator <(TimeSpan t1, TimeSpan t2) { throw null; }

        public static Boolean operator <=(TimeSpan t1, TimeSpan t2) { throw null; }

        public static TimeSpan operator -(TimeSpan t1, TimeSpan t2) { throw null; }

        public static TimeSpan operator -(TimeSpan t) { throw null; }

        public static TimeSpan operator +(TimeSpan t) { throw null; }

        public static TimeSpan Parse(String input, IFormatProvider formatProvider) { throw null; }

        public static TimeSpan Parse(String s) { throw null; }

        public static TimeSpan ParseExact(String input, String format, IFormatProvider formatProvider, Globalization.TimeSpanStyles styles) { throw null; }

        public static TimeSpan ParseExact(String input, String format, IFormatProvider formatProvider) { throw null; }

        public static TimeSpan ParseExact(String input, String[] formats, IFormatProvider formatProvider, Globalization.TimeSpanStyles styles) { throw null; }

        public static TimeSpan ParseExact(String input, String[] formats, IFormatProvider formatProvider) { throw null; }

        public TimeSpan Subtract(TimeSpan ts) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(String format, IFormatProvider formatProvider) { throw null; }

        public String ToString(String format) { throw null; }

        public static Boolean TryParse(String input, IFormatProvider formatProvider, out TimeSpan result) { throw null; }

        public static Boolean TryParse(String s, out TimeSpan result) { throw null; }

        public static Boolean TryParseExact(String input, String format, IFormatProvider formatProvider, Globalization.TimeSpanStyles styles, out TimeSpan result) { throw null; }

        public static Boolean TryParseExact(String input, String format, IFormatProvider formatProvider, out TimeSpan result) { throw null; }

        public static Boolean TryParseExact(String input, String[] formats, IFormatProvider formatProvider, Globalization.TimeSpanStyles styles, out TimeSpan result) { throw null; }

        public static Boolean TryParseExact(String input, String[] formats, IFormatProvider formatProvider, out TimeSpan result) { throw null; }
    }

    public sealed partial class TimeZoneInfo : IEquatable<TimeZoneInfo>
    {
        internal TimeZoneInfo() { }

        public TimeSpan BaseUtcOffset { get { throw null; } }

        public String DaylightName { get { throw null; } }

        public String DisplayName { get { throw null; } }

        public String Id { get { throw null; } }

        public static TimeZoneInfo Local { get { throw null; } }

        public String StandardName { get { throw null; } }

        public Boolean SupportsDaylightSavingTime { get { throw null; } }

        public static TimeZoneInfo Utc { get { throw null; } }

        public static DateTime ConvertTime(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone) { throw null; }

        public static DateTime ConvertTime(DateTime dateTime, TimeZoneInfo destinationTimeZone) { throw null; }

        public static DateTimeOffset ConvertTime(DateTimeOffset dateTimeOffset, TimeZoneInfo destinationTimeZone) { throw null; }

        public Boolean Equals(TimeZoneInfo other) { throw null; }

        public static TimeZoneInfo FindSystemTimeZoneById(String id) { throw null; }

        public TimeSpan[] GetAmbiguousTimeOffsets(DateTime dateTime) { throw null; }

        public TimeSpan[] GetAmbiguousTimeOffsets(DateTimeOffset dateTimeOffset) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones() { throw null; }

        public TimeSpan GetUtcOffset(DateTime dateTime) { throw null; }

        public TimeSpan GetUtcOffset(DateTimeOffset dateTimeOffset) { throw null; }

        public Boolean IsAmbiguousTime(DateTime dateTime) { throw null; }

        public Boolean IsAmbiguousTime(DateTimeOffset dateTimeOffset) { throw null; }

        public Boolean IsDaylightSavingTime(DateTime dateTime) { throw null; }

        public Boolean IsDaylightSavingTime(DateTimeOffset dateTimeOffset) { throw null; }

        public Boolean IsInvalidTime(DateTime dateTime) { throw null; }

        public override String ToString() { throw null; }
    }

    public static partial class Tuple
    {
        public static Tuple<T1> Create<T1>(T1 item1) { throw null; }

        public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) { throw null; }

        public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) { throw null; }

        public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) { throw null; }

        public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) { throw null; }

        public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) { throw null; }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) { throw null; }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) { throw null; }
    }

    public partial class Tuple<T1> : Collections.IStructuralComparable, Collections.IStructuralEquatable, IComparable
    {
        public Tuple(T1 item1) { }

        public T1 Item1 { get { throw null; } }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        Int32 Collections.IStructuralComparable.CompareTo(Object other, Collections.IComparer comparer) { throw null; }

        Boolean Collections.IStructuralEquatable.Equals(Object other, Collections.IEqualityComparer comparer) { throw null; }

        Int32 Collections.IStructuralEquatable.GetHashCode(Collections.IEqualityComparer comparer) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        public override String ToString() { throw null; }
    }

    public partial class Tuple<T1, T2> : Collections.IStructuralComparable, Collections.IStructuralEquatable, IComparable
    {
        public Tuple(T1 item1, T2 item2) { }

        public T1 Item1 { get { throw null; } }

        public T2 Item2 { get { throw null; } }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        Int32 Collections.IStructuralComparable.CompareTo(Object other, Collections.IComparer comparer) { throw null; }

        Boolean Collections.IStructuralEquatable.Equals(Object other, Collections.IEqualityComparer comparer) { throw null; }

        Int32 Collections.IStructuralEquatable.GetHashCode(Collections.IEqualityComparer comparer) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        public override String ToString() { throw null; }
    }

    public partial class Tuple<T1, T2, T3> : Collections.IStructuralComparable, Collections.IStructuralEquatable, IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3) { }

        public T1 Item1 { get { throw null; } }

        public T2 Item2 { get { throw null; } }

        public T3 Item3 { get { throw null; } }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        Int32 Collections.IStructuralComparable.CompareTo(Object other, Collections.IComparer comparer) { throw null; }

        Boolean Collections.IStructuralEquatable.Equals(Object other, Collections.IEqualityComparer comparer) { throw null; }

        Int32 Collections.IStructuralEquatable.GetHashCode(Collections.IEqualityComparer comparer) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        public override String ToString() { throw null; }
    }

    public partial class Tuple<T1, T2, T3, T4> : Collections.IStructuralComparable, Collections.IStructuralEquatable, IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4) { }

        public T1 Item1 { get { throw null; } }

        public T2 Item2 { get { throw null; } }

        public T3 Item3 { get { throw null; } }

        public T4 Item4 { get { throw null; } }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        Int32 Collections.IStructuralComparable.CompareTo(Object other, Collections.IComparer comparer) { throw null; }

        Boolean Collections.IStructuralEquatable.Equals(Object other, Collections.IEqualityComparer comparer) { throw null; }

        Int32 Collections.IStructuralEquatable.GetHashCode(Collections.IEqualityComparer comparer) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        public override String ToString() { throw null; }
    }

    public partial class Tuple<T1, T2, T3, T4, T5> : Collections.IStructuralComparable, Collections.IStructuralEquatable, IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) { }

        public T1 Item1 { get { throw null; } }

        public T2 Item2 { get { throw null; } }

        public T3 Item3 { get { throw null; } }

        public T4 Item4 { get { throw null; } }

        public T5 Item5 { get { throw null; } }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        Int32 Collections.IStructuralComparable.CompareTo(Object other, Collections.IComparer comparer) { throw null; }

        Boolean Collections.IStructuralEquatable.Equals(Object other, Collections.IEqualityComparer comparer) { throw null; }

        Int32 Collections.IStructuralEquatable.GetHashCode(Collections.IEqualityComparer comparer) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        public override String ToString() { throw null; }
    }

    public partial class Tuple<T1, T2, T3, T4, T5, T6> : Collections.IStructuralComparable, Collections.IStructuralEquatable, IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) { }

        public T1 Item1 { get { throw null; } }

        public T2 Item2 { get { throw null; } }

        public T3 Item3 { get { throw null; } }

        public T4 Item4 { get { throw null; } }

        public T5 Item5 { get { throw null; } }

        public T6 Item6 { get { throw null; } }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        Int32 Collections.IStructuralComparable.CompareTo(Object other, Collections.IComparer comparer) { throw null; }

        Boolean Collections.IStructuralEquatable.Equals(Object other, Collections.IEqualityComparer comparer) { throw null; }

        Int32 Collections.IStructuralEquatable.GetHashCode(Collections.IEqualityComparer comparer) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        public override String ToString() { throw null; }
    }

    public partial class Tuple<T1, T2, T3, T4, T5, T6, T7> : Collections.IStructuralComparable, Collections.IStructuralEquatable, IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) { }

        public T1 Item1 { get { throw null; } }

        public T2 Item2 { get { throw null; } }

        public T3 Item3 { get { throw null; } }

        public T4 Item4 { get { throw null; } }

        public T5 Item5 { get { throw null; } }

        public T6 Item6 { get { throw null; } }

        public T7 Item7 { get { throw null; } }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        Int32 Collections.IStructuralComparable.CompareTo(Object other, Collections.IComparer comparer) { throw null; }

        Boolean Collections.IStructuralEquatable.Equals(Object other, Collections.IEqualityComparer comparer) { throw null; }

        Int32 Collections.IStructuralEquatable.GetHashCode(Collections.IEqualityComparer comparer) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        public override String ToString() { throw null; }
    }

    public partial class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> : Collections.IStructuralComparable, Collections.IStructuralEquatable, IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest) { }

        public T1 Item1 { get { throw null; } }

        public T2 Item2 { get { throw null; } }

        public T3 Item3 { get { throw null; } }

        public T4 Item4 { get { throw null; } }

        public T5 Item5 { get { throw null; } }

        public T6 Item6 { get { throw null; } }

        public T7 Item7 { get { throw null; } }

        public TRest Rest { get { throw null; } }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        Int32 Collections.IStructuralComparable.CompareTo(Object other, Collections.IComparer comparer) { throw null; }

        Boolean Collections.IStructuralEquatable.Equals(Object other, Collections.IEqualityComparer comparer) { throw null; }

        Int32 Collections.IStructuralEquatable.GetHashCode(Collections.IEqualityComparer comparer) { throw null; }

        Int32 IComparable.CompareTo(Object obj) { throw null; }

        public override String ToString() { throw null; }
    }

    public abstract partial class Type
    {
        internal Type() { }

        public static readonly Char Delimiter;
        public static readonly Type[] EmptyTypes;
        public static readonly Object Missing;
        public abstract String AssemblyQualifiedName { get; }
        public abstract Type DeclaringType { get; }
        public abstract String FullName { get; }
        public abstract Int32 GenericParameterPosition { get; }
        public abstract Type[] GenericTypeArguments { get; }

        public Boolean HasElementType { get { throw null; } }

        public virtual Boolean IsArray { get { throw null; } }

        public virtual Boolean IsByRef { get { throw null; } }

        public abstract Boolean IsConstructedGenericType { get; }
        public abstract Boolean IsGenericParameter { get; }

        public Boolean IsNested { get { throw null; } }

        public virtual Boolean IsPointer { get { throw null; } }

        public abstract String Name { get; }
        public abstract String Namespace { get; }

        public virtual RuntimeTypeHandle TypeHandle { get { throw null; } }

        public override Boolean Equals(Object o) { throw null; }

        public Boolean Equals(Type o) { throw null; }

        public abstract Int32 GetArrayRank();
        public abstract Type GetElementType();
        public abstract Type GetGenericTypeDefinition();
        public override Int32 GetHashCode() { throw null; }

        public static Type GetType(String typeName, Boolean throwOnError, Boolean ignoreCase) { throw null; }

        public static Type GetType(String typeName, Boolean throwOnError) { throw null; }

        public static Type GetType(String typeName) { throw null; }

        public static TypeCode GetTypeCode(Type type) { throw null; }

        public static Type GetTypeFromHandle(RuntimeTypeHandle handle) { throw null; }

        public abstract Type MakeArrayType();
        public abstract Type MakeArrayType(Int32 rank);
        public abstract Type MakeByRefType();
        public abstract Type MakeGenericType(params Type[] typeArguments);
        public abstract Type MakePointerType();
        public override String ToString() { throw null; }
    }

    public partial class TypeAccessException : TypeLoadException
    {
        public TypeAccessException() { }

        public TypeAccessException(String message, Exception inner) { }

        public TypeAccessException(String message) { }
    }

    public enum TypeCode
    {
        Empty = 0,
        Object = 1,
        Boolean = 3,
        Char = 4,
        SByte = 5,
        Byte = 6,
        Int16 = 7,
        UInt16 = 8,
        Int32 = 9,
        UInt32 = 10,
        Int64 = 11,
        UInt64 = 12,
        Single = 13,
        Double = 14,
        Decimal = 15,
        DateTime = 16,
        String = 18
    }

    public sealed partial class TypeInitializationException : Exception
    {
        public TypeInitializationException(String fullTypeName, Exception innerException) { }

        public String TypeName { get { throw null; } }
    }

    public partial class TypeLoadException : Exception
    {
        public TypeLoadException() { }

        public TypeLoadException(String message, Exception inner) { }

        public TypeLoadException(String message) { }

        public override String Message { get { throw null; } }

        public String TypeName { get { throw null; } }
    }

    [CLSCompliant(false)]
    public partial struct UInt16 : IComparable, IComparable<UInt16>, IConvertible, IEquatable<UInt16>, IFormattable
    {
        public const UInt16 MaxValue = 65535;
        public const UInt16 MinValue = 0;
        public Int32 CompareTo(UInt16 value) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public Boolean Equals(UInt16 obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        [CLSCompliant(false)]
        public static UInt16 Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        [CLSCompliant(false)]
        public static UInt16 Parse(String s, Globalization.NumberStyles style) { throw null; }

        [CLSCompliant(false)]
        public static UInt16 Parse(String s, IFormatProvider provider) { throw null; }

        [CLSCompliant(false)]
        public static UInt16 Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        [CLSCompliant(false)]
        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out UInt16 result) { throw null; }

        [CLSCompliant(false)]
        public static Boolean TryParse(String s, out UInt16 result) { throw null; }
    }

    [CLSCompliant(false)]
    public partial struct UInt32 : IComparable, IComparable<UInt32>, IConvertible, IEquatable<UInt32>, IFormattable
    {
        public const UInt32 MaxValue = 4294967295U;
        public const UInt32 MinValue = 0U;
        public Int32 CompareTo(UInt32 value) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public Boolean Equals(UInt32 obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        [CLSCompliant(false)]
        public static UInt32 Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        [CLSCompliant(false)]
        public static UInt32 Parse(String s, Globalization.NumberStyles style) { throw null; }

        [CLSCompliant(false)]
        public static UInt32 Parse(String s, IFormatProvider provider) { throw null; }

        [CLSCompliant(false)]
        public static UInt32 Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        [CLSCompliant(false)]
        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out UInt32 result) { throw null; }

        [CLSCompliant(false)]
        public static Boolean TryParse(String s, out UInt32 result) { throw null; }
    }

    [CLSCompliant(false)]
    public partial struct UInt64 : IComparable, IComparable<UInt64>, IConvertible, IEquatable<UInt64>, IFormattable
    {
        public const UInt64 MaxValue = 18446744073709551615UL;
        public const UInt64 MinValue = 0UL;
        public Int32 CompareTo(UInt64 value) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public Boolean Equals(UInt64 obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        [CLSCompliant(false)]
        public static UInt64 Parse(String s, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        [CLSCompliant(false)]
        public static UInt64 Parse(String s, Globalization.NumberStyles style) { throw null; }

        [CLSCompliant(false)]
        public static UInt64 Parse(String s, IFormatProvider provider) { throw null; }

        [CLSCompliant(false)]
        public static UInt64 Parse(String s) { throw null; }

        Int32 IComparable.CompareTo(Object value) { throw null; }

        TypeCode IConvertible.GetTypeCode() { throw null; }

        Boolean IConvertible.ToBoolean(IFormatProvider provider) { throw null; }

        Byte IConvertible.ToByte(IFormatProvider provider) { throw null; }

        Char IConvertible.ToChar(IFormatProvider provider) { throw null; }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) { throw null; }

        Decimal IConvertible.ToDecimal(IFormatProvider provider) { throw null; }

        Double IConvertible.ToDouble(IFormatProvider provider) { throw null; }

        Int16 IConvertible.ToInt16(IFormatProvider provider) { throw null; }

        Int32 IConvertible.ToInt32(IFormatProvider provider) { throw null; }

        Int64 IConvertible.ToInt64(IFormatProvider provider) { throw null; }

        SByte IConvertible.ToSByte(IFormatProvider provider) { throw null; }

        Single IConvertible.ToSingle(IFormatProvider provider) { throw null; }

        Object IConvertible.ToType(Type type, IFormatProvider provider) { throw null; }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider) { throw null; }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider) { throw null; }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(IFormatProvider provider) { throw null; }

        public String ToString(String format, IFormatProvider provider) { throw null; }

        public String ToString(String format) { throw null; }

        [CLSCompliant(false)]
        public static Boolean TryParse(String s, Globalization.NumberStyles style, IFormatProvider provider, out UInt64 result) { throw null; }

        [CLSCompliant(false)]
        public static Boolean TryParse(String s, out UInt64 result) { throw null; }
    }

    [CLSCompliant(false)]
    public partial struct UIntPtr
    {
        public static readonly UIntPtr Zero;
        public UIntPtr(UInt32 value) { }

        public UIntPtr(UInt64 value) { }

        [CLSCompliant(false)]
        public unsafe UIntPtr(void* value) { }

        public static Int32 Size { get { throw null; } }

        public static UIntPtr Add(UIntPtr pointer, Int32 offset) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static UIntPtr operator +(UIntPtr pointer, Int32 offset) { throw null; }

        public static Boolean operator ==(UIntPtr value1, UIntPtr value2) { throw null; }

        public static explicit operator UIntPtr(UInt32 value) { throw null; }

        public static explicit operator UIntPtr(UInt64 value) { throw null; }

        public static explicit operator UInt32(UIntPtr value) { throw null; }

        public static explicit operator UInt64(UIntPtr value) { throw null; }

        [CLSCompliant(false)]
        public static unsafe explicit operator void*(UIntPtr value) { throw null; }

        [CLSCompliant(false)]
        public static unsafe explicit operator UIntPtr(void* value) { throw null; }

        public static Boolean operator !=(UIntPtr value1, UIntPtr value2) { throw null; }

        public static UIntPtr operator -(UIntPtr pointer, Int32 offset) { throw null; }

        public static UIntPtr Subtract(UIntPtr pointer, Int32 offset) { throw null; }

        [CLSCompliant(false)]
        public unsafe void* ToPointer() { throw null; }

        public override String ToString() { throw null; }

        public UInt32 ToUInt32() { throw null; }

        public UInt64 ToUInt64() { throw null; }
    }

    public partial class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException() { }

        public UnauthorizedAccessException(String message, Exception inner) { }

        public UnauthorizedAccessException(String message) { }
    }

    public partial class Uri
    {
        public Uri(String uriString, UriKind uriKind) { }

        public Uri(String uriString) { }

        public Uri(Uri baseUri, String relativeUri) { }

        public Uri(Uri baseUri, Uri relativeUri) { }

        public String AbsolutePath { get { throw null; } }

        public String AbsoluteUri { get { throw null; } }

        public String Authority { get { throw null; } }

        public String DnsSafeHost { get { throw null; } }

        public String Fragment { get { throw null; } }

        public String Host { get { throw null; } }

        public UriHostNameType HostNameType { get { throw null; } }

        public String IdnHost { get { throw null; } }

        public Boolean IsAbsoluteUri { get { throw null; } }

        public Boolean IsDefaultPort { get { throw null; } }

        public Boolean IsFile { get { throw null; } }

        public Boolean IsLoopback { get { throw null; } }

        public Boolean IsUnc { get { throw null; } }

        public String LocalPath { get { throw null; } }

        public String OriginalString { get { throw null; } }

        public String PathAndQuery { get { throw null; } }

        public Int32 Port { get { throw null; } }

        public String Query { get { throw null; } }

        public String Scheme { get { throw null; } }

        public String[] Segments { get { throw null; } }

        public Boolean UserEscaped { get { throw null; } }

        public String UserInfo { get { throw null; } }

        public static UriHostNameType CheckHostName(String name) { throw null; }

        public static Boolean CheckSchemeName(String schemeName) { throw null; }

        public static Int32 Compare(Uri uri1, Uri uri2, UriComponents partsToCompare, UriFormat compareFormat, StringComparison comparisonType) { throw null; }

        public override Boolean Equals(Object comparand) { throw null; }

        public static String EscapeDataString(String stringToEscape) { throw null; }

        public static String EscapeUriString(String stringToEscape) { throw null; }

        public String GetComponents(UriComponents components, UriFormat format) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public Boolean IsBaseOf(Uri uri) { throw null; }

        public Boolean IsWellFormedOriginalString() { throw null; }

        public static Boolean IsWellFormedUriString(String uriString, UriKind uriKind) { throw null; }

        public Uri MakeRelativeUri(Uri uri) { throw null; }

        public static Boolean operator ==(Uri uri1, Uri uri2) { throw null; }

        public static Boolean operator !=(Uri uri1, Uri uri2) { throw null; }

        public override String ToString() { throw null; }

        public static Boolean TryCreate(String uriString, UriKind uriKind, out Uri result) { throw null; }

        public static Boolean TryCreate(Uri baseUri, String relativeUri, out Uri result) { throw null; }

        public static Boolean TryCreate(Uri baseUri, Uri relativeUri, out Uri result) { throw null; }

        public static String UnescapeDataString(String stringToUnescape) { throw null; }
    }

    [Flags]
    public enum UriComponents
    {
        SerializationInfoString = Int32.MinValue,
        Scheme = 1,
        UserInfo = 2,
        Host = 4,
        Port = 8,
        SchemeAndServer = 13,
        Path = 16,
        Query = 32,
        PathAndQuery = 48,
        HttpRequestUrl = 61,
        Fragment = 64,
        AbsoluteUri = 127,
        StrongPort = 128,
        HostAndPort = 132,
        StrongAuthority = 134,
        NormalizedHost = 256,
        KeepDelimiter = 1073741824
    }

    public enum UriFormat
    {
        UriEscaped = 1,
        Unescaped = 2,
        SafeUnescaped = 3
    }

    public partial class UriFormatException : FormatException
    {
        public UriFormatException() { }

        public UriFormatException(String textString, Exception e) { }

        public UriFormatException(String textString) { }
    }

    public enum UriHostNameType
    {
        Unknown = 0,
        Basic = 1,
        Dns = 2,
        IPv4 = 3,
        IPv6 = 4
    }

    public enum UriKind
    {
        RelativeOrAbsolute = 0,
        Absolute = 1,
        Relative = 2
    }

    public abstract partial class ValueType
    {
        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public override String ToString() { throw null; }
    }

    public sealed partial class Version : IComparable, IComparable<Version>, IEquatable<Version>
    {
        public Version(Int32 major, Int32 minor, Int32 build, Int32 revision) { }

        public Version(Int32 major, Int32 minor, Int32 build) { }

        public Version(Int32 major, Int32 minor) { }

        public Version(String version) { }

        public Int32 Build { get { throw null; } }

        public Int32 Major { get { throw null; } }

        public Int16 MajorRevision { get { throw null; } }

        public Int32 Minor { get { throw null; } }

        public Int16 MinorRevision { get { throw null; } }

        public Int32 Revision { get { throw null; } }

        public Int32 CompareTo(Version value) { throw null; }

        public override Boolean Equals(Object obj) { throw null; }

        public Boolean Equals(Version obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }

        public static Boolean operator ==(Version v1, Version v2) { throw null; }

        public static Boolean operator >(Version v1, Version v2) { throw null; }

        public static Boolean operator >=(Version v1, Version v2) { throw null; }

        public static Boolean operator !=(Version v1, Version v2) { throw null; }

        public static Boolean operator <(Version v1, Version v2) { throw null; }

        public static Boolean operator <=(Version v1, Version v2) { throw null; }

        public static Version Parse(String input) { throw null; }

        Int32 IComparable.CompareTo(Object version) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(Int32 fieldCount) { throw null; }

        public static Boolean TryParse(String input, out Version result) { throw null; }
    }

    public partial struct Void
    {
    }

    public partial class WeakReference
    {
        public WeakReference(Object target, Boolean trackResurrection) { }

        public WeakReference(Object target) { }

        public virtual Boolean IsAlive { get { throw null; } }

        public virtual Object Target { get { throw null; } set { } }

        public virtual Boolean TrackResurrection { get { throw null; } }

        ~WeakReference() {
        }
    }

    public sealed partial class WeakReference<T>
        where T : class
    {
        public WeakReference(T target, Boolean trackResurrection) { }

        public WeakReference(T target) { }

        ~WeakReference() {
        }

        public void SetTarget(T target) { }

        public Boolean TryGetTarget(out T target) { throw null; }
    }
}

namespace System.Collections
{
    public partial struct DictionaryEntry
    {
        public DictionaryEntry(Object key, Object value) { }

        public Object Key { get { throw null; } set { } }

        public Object Value { get { throw null; } set { } }
    }

    public partial interface ICollection : IEnumerable
    {
        Int32 Count { get; }

        Boolean IsSynchronized { get; }

        Object SyncRoot { get; }

        void CopyTo(Array array, Int32 index);
    }

    public partial interface IComparer
    {
        Int32 Compare(Object x, Object y);
    }

    public partial interface IDictionary : ICollection, IEnumerable
    {
        Boolean IsFixedSize { get; }

        Boolean IsReadOnly { get; }

        Object this[Object key] { get; set; }

        ICollection Keys { get; }

        ICollection Values { get; }

        void Add(Object key, Object value);
        void Clear();
        Boolean Contains(Object key);
        IDictionaryEnumerator GetEnumerator();
        void Remove(Object key);
    }

    public partial interface IDictionaryEnumerator : IEnumerator
    {
        DictionaryEntry Entry { get; }

        Object Key { get; }

        Object Value { get; }
    }

    public partial interface IEnumerable
    {
        IEnumerator GetEnumerator();
    }

    public partial interface IEnumerator
    {
        Object Current { get; }

        Boolean MoveNext();
        void Reset();
    }

    public partial interface IEqualityComparer
    {
        Boolean Equals(Object x, Object y);
        Int32 GetHashCode(Object obj);
    }

    public partial interface IList : ICollection, IEnumerable
    {
        Boolean IsFixedSize { get; }

        Boolean IsReadOnly { get; }

        Object this[Int32 index] { get; set; }

        Int32 Add(Object value);
        void Clear();
        Boolean Contains(Object value);
        Int32 IndexOf(Object value);
        void Insert(Int32 index, Object value);
        void Remove(Object value);
        void RemoveAt(Int32 index);
    }

    public partial interface IStructuralComparable
    {
        Int32 CompareTo(Object other, IComparer comparer);
    }

    public partial interface IStructuralEquatable
    {
        Boolean Equals(Object other, IEqualityComparer comparer);
        Int32 GetHashCode(IEqualityComparer comparer);
    }
}

namespace System.Collections.Generic
{
    public partial interface ICollection<T> : IEnumerable<T>, IEnumerable
    {
        Int32 Count { get; }

        Boolean IsReadOnly { get; }

        void Add(T item);
        void Clear();
        Boolean Contains(T item);
        void CopyTo(T[] array, Int32 arrayIndex);
        Boolean Remove(T item);
    }

    public partial interface IComparer<in T>
    {
        Int32 Compare(T x, T y);
    }

    public partial interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        TValue this[TKey key] { get; set; }

        ICollection<TKey> Keys { get; }

        ICollection<TValue> Values { get; }

        void Add(TKey key, TValue value);
        Boolean ContainsKey(TKey key);
        Boolean Remove(TKey key);
        Boolean TryGetValue(TKey key, out TValue value);
    }

    public partial interface IEnumerable<out T> : IEnumerable
    {
        IEnumerator<T> GetEnumerator();
    }

    public partial interface IEnumerator<out T> : IEnumerator, IDisposable
    {
        T Current { get; }
    }

    public partial interface IEqualityComparer<in T>
    {
        Boolean Equals(T x, T y);
        Int32 GetHashCode(T obj);
    }

    public partial interface IList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        T this[Int32 index] { get; set; }

        Int32 IndexOf(T item);
        void Insert(Int32 index, T item);
        void RemoveAt(Int32 index);
    }

    public partial interface IReadOnlyCollection<out T> : IEnumerable<T>, IEnumerable
    {
        Int32 Count { get; }
    }

    public partial interface IReadOnlyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
    {
        TValue this[TKey key] { get; }

        IEnumerable<TKey> Keys { get; }

        IEnumerable<TValue> Values { get; }

        Boolean ContainsKey(TKey key);
        Boolean TryGetValue(TKey key, out TValue value);
    }

    public partial interface IReadOnlyList<out T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
    {
        T this[Int32 index] { get; }
    }

    public partial interface ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        Boolean Add(T item);
        void ExceptWith(IEnumerable<T> other);
        void IntersectWith(IEnumerable<T> other);
        Boolean IsProperSubsetOf(IEnumerable<T> other);
        Boolean IsProperSupersetOf(IEnumerable<T> other);
        Boolean IsSubsetOf(IEnumerable<T> other);
        Boolean IsSupersetOf(IEnumerable<T> other);
        Boolean Overlaps(IEnumerable<T> other);
        Boolean SetEquals(IEnumerable<T> other);
        void SymmetricExceptWith(IEnumerable<T> other);
        void UnionWith(IEnumerable<T> other);
    }

    public partial class KeyNotFoundException : Exception
    {
        public KeyNotFoundException() { }

        public KeyNotFoundException(String message, Exception innerException) { }

        public KeyNotFoundException(String message) { }
    }

    public partial struct KeyValuePair<TKey, TValue>
    {
        public KeyValuePair(TKey key, TValue value) { }

        public TKey Key { get { throw null; } }

        public TValue Value { get { throw null; } }

        public override String ToString() { throw null; }
    }
}

namespace System.Collections.ObjectModel
{
    public partial class Collection<T> : Generic.ICollection<T>, Generic.IEnumerable<T>, IEnumerable, Generic.IList<T>, Generic.IReadOnlyCollection<T>, Generic.IReadOnlyList<T>, ICollection, IList
    {
        public Collection() { }

        public Collection(Generic.IList<T> list) { }

        public Int32 Count { get { throw null; } }

        public T this[Int32 index] { get { throw null; } set { } }

        protected Generic.IList<T> Items { get { throw null; } }

        Boolean Generic.ICollection<T>.IsReadOnly { get { throw null; } }

        Boolean ICollection.IsSynchronized { get { throw null; } }

        Object ICollection.SyncRoot { get { throw null; } }

        Boolean IList.IsFixedSize { get { throw null; } }

        Boolean IList.IsReadOnly { get { throw null; } }

        Object IList.this[Int32 index] { get { throw null; } set { } }

        public void Add(T item) { }

        public void Clear() { }

        protected virtual void ClearItems() { }

        public Boolean Contains(T item) { throw null; }

        public void CopyTo(T[] array, Int32 index) { }

        public Generic.IEnumerator<T> GetEnumerator() { throw null; }

        public Int32 IndexOf(T item) { throw null; }

        public void Insert(Int32 index, T item) { }

        protected virtual void InsertItem(Int32 index, T item) { }

        public Boolean Remove(T item) { throw null; }

        public void RemoveAt(Int32 index) { }

        protected virtual void RemoveItem(Int32 index) { }

        protected virtual void SetItem(Int32 index, T item) { }

        void ICollection.CopyTo(Array array, Int32 index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        Int32 IList.Add(Object value) { throw null; }

        Boolean IList.Contains(Object value) { throw null; }

        Int32 IList.IndexOf(Object value) { throw null; }

        void IList.Insert(Int32 index, Object value) { }

        void IList.Remove(Object value) { }
    }

    public partial class ReadOnlyCollection<T> : Generic.ICollection<T>, Generic.IEnumerable<T>, IEnumerable, Generic.IList<T>, Generic.IReadOnlyCollection<T>, Generic.IReadOnlyList<T>, ICollection, IList
    {
        public ReadOnlyCollection(Generic.IList<T> list) { }

        public Int32 Count { get { throw null; } }

        public T this[Int32 index] { get { throw null; } }

        protected Generic.IList<T> Items { get { throw null; } }

        Boolean Generic.ICollection<T>.IsReadOnly { get { throw null; } }

        T Generic.IList<T>.this[Int32 index] { get { throw null; } set { } }

        Boolean ICollection.IsSynchronized { get { throw null; } }

        Object ICollection.SyncRoot { get { throw null; } }

        Boolean IList.IsFixedSize { get { throw null; } }

        Boolean IList.IsReadOnly { get { throw null; } }

        Object IList.this[Int32 index] { get { throw null; } set { } }

        public Boolean Contains(T value) { throw null; }

        public void CopyTo(T[] array, Int32 index) { }

        public Generic.IEnumerator<T> GetEnumerator() { throw null; }

        public Int32 IndexOf(T value) { throw null; }

        void Generic.ICollection<T>.Add(T value) { }

        void Generic.ICollection<T>.Clear() { }

        Boolean Generic.ICollection<T>.Remove(T value) { throw null; }

        void Generic.IList<T>.Insert(Int32 index, T value) { }

        void Generic.IList<T>.RemoveAt(Int32 index) { }

        void ICollection.CopyTo(Array array, Int32 index) { }

        IEnumerator IEnumerable.GetEnumerator() { throw null; }

        Int32 IList.Add(Object value) { throw null; }

        void IList.Clear() { }

        Boolean IList.Contains(Object value) { throw null; }

        Int32 IList.IndexOf(Object value) { throw null; }

        void IList.Insert(Int32 index, Object value) { }

        void IList.Remove(Object value) { }

        void IList.RemoveAt(Int32 index) { }
    }
}

namespace System.ComponentModel
{
    [AttributeUsage(AttributeTargets.All)]
    public partial class DefaultValueAttribute : Attribute
    {
        public DefaultValueAttribute(Boolean value) { }

        public DefaultValueAttribute(Byte value) { }

        public DefaultValueAttribute(Char value) { }

        public DefaultValueAttribute(Double value) { }

        public DefaultValueAttribute(Int16 value) { }

        public DefaultValueAttribute(Int32 value) { }

        public DefaultValueAttribute(Int64 value) { }

        public DefaultValueAttribute(Object value) { }

        public DefaultValueAttribute(Single value) { }

        public DefaultValueAttribute(String value) { }

        public DefaultValueAttribute(Type type, String value) { }

        public virtual Object Value { get { throw null; } }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate)]
    public sealed partial class EditorBrowsableAttribute : Attribute
    {
        public EditorBrowsableAttribute(EditorBrowsableState state) { }

        public EditorBrowsableState State { get { throw null; } }

        public override Boolean Equals(Object obj) { throw null; }

        public override Int32 GetHashCode() { throw null; }
    }

    public enum EditorBrowsableState
    {
        Always = 0,
        Never = 1,
        Advanced = 2
    }
}

namespace System.Diagnostics
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed partial class ConditionalAttribute : Attribute
    {
        public ConditionalAttribute(String conditionString) { }

        public String ConditionString { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, AllowMultiple = false)]
    public sealed partial class DebuggableAttribute : Attribute
    {
        public DebuggableAttribute(DebuggingModes modes) { }

        [Flags]
        public enum DebuggingModes
        {
            None = 0,
            Default = 1,
            IgnoreSymbolStoreSequencePoints = 2,
            EnableEditAndContinue = 4,
            DisableOptimizations = 256
        }
    }
}

namespace System.Globalization
{
    [Flags]
    public enum DateTimeStyles
    {
        None = 0,
        AllowLeadingWhite = 1,
        AllowTrailingWhite = 2,
        AllowInnerWhite = 4,
        AllowWhiteSpaces = 7,
        NoCurrentDateDefault = 8,
        AdjustToUniversal = 16,
        AssumeLocal = 32,
        AssumeUniversal = 64,
        RoundtripKind = 128
    }

    [Flags]
    public enum NumberStyles
    {
        None = 0,
        AllowLeadingWhite = 1,
        AllowTrailingWhite = 2,
        AllowLeadingSign = 4,
        Integer = 7,
        AllowTrailingSign = 8,
        AllowParentheses = 16,
        AllowDecimalPoint = 32,
        AllowThousands = 64,
        Number = 111,
        AllowExponent = 128,
        Float = 167,
        AllowCurrencySymbol = 256,
        Currency = 383,
        Any = 511,
        AllowHexSpecifier = 512,
        HexNumber = 515
    }

    [Flags]
    public enum TimeSpanStyles
    {
        None = 0,
        AssumeNegative = 1
    }
}

namespace System.IO
{
    public partial class DirectoryNotFoundException : IOException
    {
        public DirectoryNotFoundException() { }

        public DirectoryNotFoundException(String message, Exception innerException) { }

        public DirectoryNotFoundException(String message) { }
    }

    public partial class FileLoadException : IOException
    {
        public FileLoadException() { }

        public FileLoadException(String message, Exception inner) { }

        public FileLoadException(String message, String fileName, Exception inner) { }

        public FileLoadException(String message, String fileName) { }

        public FileLoadException(String message) { }

        public String FileName { get { throw null; } }

        public override String Message { get { throw null; } }

        public override String ToString() { throw null; }
    }

    public partial class FileNotFoundException : IOException
    {
        public FileNotFoundException() { }

        public FileNotFoundException(String message, Exception innerException) { }

        public FileNotFoundException(String message, String fileName, Exception innerException) { }

        public FileNotFoundException(String message, String fileName) { }

        public FileNotFoundException(String message) { }

        public String FileName { get { throw null; } }

        public override String Message { get { throw null; } }

        public override String ToString() { throw null; }
    }

    public partial class IOException : Exception
    {
        public IOException() { }

        public IOException(String message, Exception innerException) { }

        public IOException(String message, Int32 hresult) { }

        public IOException(String message) { }
    }

    public partial class PathTooLongException : IOException
    {
        public PathTooLongException() { }

        public PathTooLongException(String message, Exception innerException) { }

        public PathTooLongException(String message) { }
    }
}

namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyCompanyAttribute : Attribute
    {
        public AssemblyCompanyAttribute(String company) { }

        public String Company { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyConfigurationAttribute : Attribute
    {
        public AssemblyConfigurationAttribute(String configuration) { }

        public String Configuration { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyCopyrightAttribute : Attribute
    {
        public AssemblyCopyrightAttribute(String copyright) { }

        public String Copyright { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyCultureAttribute : Attribute
    {
        public AssemblyCultureAttribute(String culture) { }

        public String Culture { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyDefaultAliasAttribute : Attribute
    {
        public AssemblyDefaultAliasAttribute(String defaultAlias) { }

        public String DefaultAlias { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyDelaySignAttribute : Attribute
    {
        public AssemblyDelaySignAttribute(Boolean delaySign) { }

        public Boolean DelaySign { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyDescriptionAttribute : Attribute
    {
        public AssemblyDescriptionAttribute(String description) { }

        public String Description { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyFileVersionAttribute : Attribute
    {
        public AssemblyFileVersionAttribute(String version) { }

        public String Version { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyFlagsAttribute : Attribute
    {
        public AssemblyFlagsAttribute(AssemblyNameFlags assemblyFlags) { }

        public Int32 AssemblyFlags { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyInformationalVersionAttribute : Attribute
    {
        public AssemblyInformationalVersionAttribute(String informationalVersion) { }

        public String InformationalVersion { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyKeyFileAttribute : Attribute
    {
        public AssemblyKeyFileAttribute(String keyFile) { }

        public String KeyFile { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyKeyNameAttribute : Attribute
    {
        public AssemblyKeyNameAttribute(String keyName) { }

        public String KeyName { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed partial class AssemblyMetadataAttribute : Attribute
    {
        public AssemblyMetadataAttribute(String key, String value) { }

        public String Key { get { throw null; } }

        public String Value { get { throw null; } }
    }

    [Flags]
    public enum AssemblyNameFlags
    {
        None = 0,
        PublicKey = 1,
        Retargetable = 256
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyProductAttribute : Attribute
    {
        public AssemblyProductAttribute(String product) { }

        public String Product { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed partial class AssemblySignatureKeyAttribute : Attribute
    {
        public AssemblySignatureKeyAttribute(String publicKey, String countersignature) { }

        public String Countersignature { get { throw null; } }

        public String PublicKey { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyTitleAttribute : Attribute
    {
        public AssemblyTitleAttribute(String title) { }

        public String Title { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyTrademarkAttribute : Attribute
    {
        public AssemblyTrademarkAttribute(String trademark) { }

        public String Trademark { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed partial class AssemblyVersionAttribute : Attribute
    {
        public AssemblyVersionAttribute(String version) { }

        public String Version { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public sealed partial class DefaultMemberAttribute : Attribute
    {
        public DefaultMemberAttribute(String memberName) { }

        public String MemberName { get { throw null; } }
    }

    public enum ProcessorArchitecture
    {
        None = 0,
        MSIL = 1,
        X86 = 2,
        IA64 = 3,
        Amd64 = 4,
        Arm = 5
    }
}

namespace System.Runtime
{
    public enum GCLargeObjectHeapCompactionMode
    {
        Default = 1,
        CompactOnce = 2
    }

    public enum GCLatencyMode
    {
        Batch = 0,
        Interactive = 1,
        LowLatency = 2,
        SustainedLowLatency = 3
    }

    public static partial class GCSettings
    {
        public static Boolean IsServerGC { get { throw null; } }

        public static GCLargeObjectHeapCompactionMode LargeObjectHeapCompactionMode { get { throw null; } set { } }

        public static GCLatencyMode LatencyMode { get { throw null; } set { } }
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed partial class AccessedThroughPropertyAttribute : Attribute
    {
        public AccessedThroughPropertyAttribute(String propertyName) { }

        public String PropertyName { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed partial class AsyncStateMachineAttribute : StateMachineAttribute
    {
        public AsyncStateMachineAttribute(Type stateMachineType) : base(default!) { }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class CallerFilePathAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class CallerLineNumberAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class CallerMemberNameAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Method)]
    public partial class CompilationRelaxationsAttribute : Attribute
    {
        public CompilationRelaxationsAttribute(Int32 relaxations) { }

        public Int32 CompilationRelaxations { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public sealed partial class CompilerGeneratedAttribute : Attribute
    {
    }

    public sealed partial class ConditionalWeakTable<TKey, TValue>
        where TKey : class where TValue : class
    {
        public void Add(TKey key, TValue value) { }

        ~ConditionalWeakTable() {
        }

        public TValue GetOrCreateValue(TKey key) { throw null; }

        public TValue GetValue(TKey key, ConditionalWeakTable<TKey, TValue>.CreateValueCallback createValueCallback) { throw null; }

        public Boolean Remove(TKey key) { throw null; }

        public Boolean TryGetValue(TKey key, out TValue value) { throw null; }

        public delegate TValue CreateValueCallback(TKey key);
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    public abstract partial class CustomConstantAttribute : Attribute
    {
        public abstract Object Value { get; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class DateTimeConstantAttribute : CustomConstantAttribute
    {
        public DateTimeConstantAttribute(Int64 ticks) { }

        public override Object Value { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class DecimalConstantAttribute : Attribute
    {
        public DecimalConstantAttribute(Byte scale, Byte sign, Int32 hi, Int32 mid, Int32 low) { }

        [CLSCompliant(false)]
        public DecimalConstantAttribute(Byte scale, Byte sign, UInt32 hi, UInt32 mid, UInt32 low) { }

        public Decimal Value { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed partial class DisablePrivateReflectionAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed partial class ExtensionAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public sealed partial class FixedBufferAttribute : Attribute
    {
        public FixedBufferAttribute(Type elementType, Int32 length) { }

        public Type ElementType { get { throw null; } }

        public Int32 Length { get { throw null; } }
    }

    public static partial class FormattableStringFactory
    {
        public static FormattableString Create(String format, params Object[] arguments) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed partial class IndexerNameAttribute : Attribute
    {
        public IndexerNameAttribute(String indexerName) { }
    }

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed partial class InternalsVisibleToAttribute : Attribute
    {
        public InternalsVisibleToAttribute(String assemblyName) { }

        public String AssemblyName { get { throw null; } }
    }

    public static partial class IsConst
    {
    }

    public partial interface IStrongBox
    {
        Object Value { get; set; }
    }

    public static partial class IsVolatile
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed partial class IteratorStateMachineAttribute : StateMachineAttribute
    {
        public IteratorStateMachineAttribute(Type stateMachineType) : base(default!) { }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
    public sealed partial class MethodImplAttribute : Attribute
    {
        public MethodImplAttribute(MethodImplOptions methodImplOptions) { }

        public MethodImplOptions Value { get { throw null; } }
    }

    [Flags]
    public enum MethodImplOptions
    {
        NoInlining = 8,
        NoOptimization = 64,
        PreserveSig = 128,
        AggressiveInlining = 256
    }

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed partial class ReferenceAssemblyAttribute : Attribute
    {
        public ReferenceAssemblyAttribute() { }

        public ReferenceAssemblyAttribute(String description) { }

        public String Description { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed partial class RuntimeCompatibilityAttribute : Attribute
    {
        public Boolean WrapNonExceptionThrows { get { throw null; } set { } }
    }

    public static partial class RuntimeHelpers
    {
        public static Int32 OffsetToStringData { get { throw null; } }

        public static void EnsureSufficientExecutionStack() { }

        public static Int32 GetHashCode(Object o) { throw null; }

        public static Object GetObjectValue(Object obj) { throw null; }

        public static void InitializeArray(Array array, RuntimeFieldHandle fldHandle) { }

        public static void RunClassConstructor(RuntimeTypeHandle type) { }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public partial class StateMachineAttribute : Attribute
    {
        public StateMachineAttribute(Type stateMachineType) { }

        public Type StateMachineType { get { throw null; } }
    }

    public partial class StrongBox<T> : IStrongBox
    {
        public T Value;
        public StrongBox() { }

        public StrongBox(T value) { }

        Object IStrongBox.Value { get { throw null; } set { } }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
    public sealed partial class TypeForwardedFromAttribute : Attribute
    {
        public TypeForwardedFromAttribute(String assemblyFullName) { }

        public String AssemblyFullName { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed partial class TypeForwardedToAttribute : Attribute
    {
        public TypeForwardedToAttribute(Type destination) { }

        public Type Destination { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Struct)]
    public sealed partial class UnsafeValueTypeAttribute : Attribute
    {
    }
}

namespace System.Runtime.ExceptionServices
{
    public sealed partial class ExceptionDispatchInfo
    {
        internal ExceptionDispatchInfo() { }

        public Exception SourceException { get { throw null; } }

        public static ExceptionDispatchInfo Capture(Exception source) { throw null; }

        public void Throw() { }
    }
}

namespace System.Runtime.InteropServices
{
    public enum CharSet
    {
        Ansi = 2,
        Unicode = 3
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
    public sealed partial class ComVisibleAttribute : Attribute
    {
        public ComVisibleAttribute(Boolean visibility) { }

        public Boolean Value { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public sealed partial class FieldOffsetAttribute : Attribute
    {
        public FieldOffsetAttribute(Int32 offset) { }

        public Int32 Value { get { throw null; } }
    }

    public enum LayoutKind
    {
        Sequential = 0,
        Explicit = 2,
        Auto = 3
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class OutAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed partial class StructLayoutAttribute : Attribute
    {
        public CharSet CharSet;
        public Int32 Pack;
        public Int32 Size;
        public StructLayoutAttribute(LayoutKind layoutKind) { }

        public LayoutKind Value { get { throw null; } }
    }
}

namespace System.Runtime.Versioning
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed partial class TargetFrameworkAttribute : Attribute
    {
        public TargetFrameworkAttribute(String frameworkName) { }

        public String FrameworkDisplayName { get { throw null; } set { } }

        public String FrameworkName { get { throw null; } }
    }
}

namespace System.Security
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed partial class AllowPartiallyTrustedCallersAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    public sealed partial class SecurityCriticalAttribute : Attribute
    {
    }

    public partial class SecurityException : Exception
    {
        public SecurityException() { }

        public SecurityException(String message, Exception inner) { }

        public SecurityException(String message) { }

        public override String ToString() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    public sealed partial class SecuritySafeCriticalAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed partial class SecurityTransparentAttribute : Attribute
    {
    }

    public partial class VerificationException : Exception
    {
        public VerificationException() { }

        public VerificationException(String message, Exception innerException) { }

        public VerificationException(String message) { }
    }
}

namespace System.Text
{
    public sealed partial class StringBuilder
    {
        public StringBuilder() { }

        public StringBuilder(Int32 capacity, Int32 maxCapacity) { }

        public StringBuilder(Int32 capacity) { }

        public StringBuilder(String value, Int32 startIndex, Int32 length, Int32 capacity) { }

        public StringBuilder(String value, Int32 capacity) { }

        public StringBuilder(String value) { }

        public Int32 Capacity { get { throw null; } set { } }

        public Char this[Int32 index] { get { throw null; } set { } }

        public Int32 Length { get { throw null; } set { } }

        public Int32 MaxCapacity { get { throw null; } }

        public StringBuilder Append(Boolean value) { throw null; }

        public StringBuilder Append(Byte value) { throw null; }

        public StringBuilder Append(Char value, Int32 repeatCount) { throw null; }

        public StringBuilder Append(Char value) { throw null; }

        public StringBuilder Append(Char[] value, Int32 startIndex, Int32 charCount) { throw null; }

        public StringBuilder Append(Char[] value) { throw null; }

        [CLSCompliant(false)]
        public unsafe StringBuilder Append(Char* value, Int32 valueCount) { throw null; }

        public StringBuilder Append(Decimal value) { throw null; }

        public StringBuilder Append(Double value) { throw null; }

        public StringBuilder Append(Int16 value) { throw null; }

        public StringBuilder Append(Int32 value) { throw null; }

        public StringBuilder Append(Int64 value) { throw null; }

        public StringBuilder Append(Object value) { throw null; }

        [CLSCompliant(false)]
        public StringBuilder Append(SByte value) { throw null; }

        public StringBuilder Append(Single value) { throw null; }

        public StringBuilder Append(String value, Int32 startIndex, Int32 count) { throw null; }

        public StringBuilder Append(String value) { throw null; }

        [CLSCompliant(false)]
        public StringBuilder Append(UInt16 value) { throw null; }

        [CLSCompliant(false)]
        public StringBuilder Append(UInt32 value) { throw null; }

        [CLSCompliant(false)]
        public StringBuilder Append(UInt64 value) { throw null; }

        public StringBuilder AppendFormat(IFormatProvider provider, String format, Object arg0, Object arg1, Object arg2) { throw null; }

        public StringBuilder AppendFormat(IFormatProvider provider, String format, Object arg0, Object arg1) { throw null; }

        public StringBuilder AppendFormat(IFormatProvider provider, String format, Object arg0) { throw null; }

        public StringBuilder AppendFormat(IFormatProvider provider, String format, params Object[] args) { throw null; }

        public StringBuilder AppendFormat(String format, Object arg0, Object arg1, Object arg2) { throw null; }

        public StringBuilder AppendFormat(String format, Object arg0, Object arg1) { throw null; }

        public StringBuilder AppendFormat(String format, Object arg0) { throw null; }

        public StringBuilder AppendFormat(String format, params Object[] args) { throw null; }

        public StringBuilder AppendLine() { throw null; }

        public StringBuilder AppendLine(String value) { throw null; }

        public StringBuilder Clear() { throw null; }

        public void CopyTo(Int32 sourceIndex, Char[] destination, Int32 destinationIndex, Int32 count) { }

        public Int32 EnsureCapacity(Int32 capacity) { throw null; }

        public Boolean Equals(StringBuilder sb) { throw null; }

        public StringBuilder Insert(Int32 index, Boolean value) { throw null; }

        public StringBuilder Insert(Int32 index, Byte value) { throw null; }

        public StringBuilder Insert(Int32 index, Char value) { throw null; }

        public StringBuilder Insert(Int32 index, Char[] value, Int32 startIndex, Int32 charCount) { throw null; }

        public StringBuilder Insert(Int32 index, Char[] value) { throw null; }

        public StringBuilder Insert(Int32 index, Decimal value) { throw null; }

        public StringBuilder Insert(Int32 index, Double value) { throw null; }

        public StringBuilder Insert(Int32 index, Int16 value) { throw null; }

        public StringBuilder Insert(Int32 index, Int32 value) { throw null; }

        public StringBuilder Insert(Int32 index, Int64 value) { throw null; }

        public StringBuilder Insert(Int32 index, Object value) { throw null; }

        [CLSCompliant(false)]
        public StringBuilder Insert(Int32 index, SByte value) { throw null; }

        public StringBuilder Insert(Int32 index, Single value) { throw null; }

        public StringBuilder Insert(Int32 index, String value, Int32 count) { throw null; }

        public StringBuilder Insert(Int32 index, String value) { throw null; }

        [CLSCompliant(false)]
        public StringBuilder Insert(Int32 index, UInt16 value) { throw null; }

        [CLSCompliant(false)]
        public StringBuilder Insert(Int32 index, UInt32 value) { throw null; }

        [CLSCompliant(false)]
        public StringBuilder Insert(Int32 index, UInt64 value) { throw null; }

        public StringBuilder Remove(Int32 startIndex, Int32 length) { throw null; }

        public StringBuilder Replace(Char oldChar, Char newChar, Int32 startIndex, Int32 count) { throw null; }

        public StringBuilder Replace(Char oldChar, Char newChar) { throw null; }

        public StringBuilder Replace(String oldValue, String newValue, Int32 startIndex, Int32 count) { throw null; }

        public StringBuilder Replace(String oldValue, String newValue) { throw null; }

        public override String ToString() { throw null; }

        public String ToString(Int32 startIndex, Int32 length) { throw null; }
    }
}

namespace System.Threading
{
    public enum LazyThreadSafetyMode
    {
        None = 0,
        PublicationOnly = 1,
        ExecutionAndPublication = 2
    }

    public static partial class Timeout
    {
        public const Int32 Infinite = -1;
        public static readonly TimeSpan InfiniteTimeSpan;
    }

    public abstract partial class WaitHandle : IDisposable
    {
        protected static readonly IntPtr InvalidHandle;
        public const Int32 WaitTimeout = 258;
        public void Dispose() { }

        protected virtual void Dispose(Boolean explicitDisposing) { }

        public static Boolean WaitAll(WaitHandle[] waitHandles, Int32 millisecondsTimeout) { throw null; }

        public static Boolean WaitAll(WaitHandle[] waitHandles, TimeSpan timeout) { throw null; }

        public static Boolean WaitAll(WaitHandle[] waitHandles) { throw null; }

        public static Int32 WaitAny(WaitHandle[] waitHandles, Int32 millisecondsTimeout) { throw null; }

        public static Int32 WaitAny(WaitHandle[] waitHandles, TimeSpan timeout) { throw null; }

        public static Int32 WaitAny(WaitHandle[] waitHandles) { throw null; }

        public virtual Boolean WaitOne() { throw null; }

        public virtual Boolean WaitOne(Int32 millisecondsTimeout) { throw null; }

        public virtual Boolean WaitOne(TimeSpan timeout) { throw null; }
    }
}
