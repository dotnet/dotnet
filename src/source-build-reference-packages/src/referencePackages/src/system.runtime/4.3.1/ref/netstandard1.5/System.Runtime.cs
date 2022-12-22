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
[assembly: AssemblyTitle("System.Runtime")]
[assembly: AssemblyDescription("System.Runtime")]
[assembly: AssemblyDefaultAlias("System.Runtime")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.0.0")]




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
        public static object CreateInstance(System.Type type) { throw null; }
        public static object CreateInstance(System.Type type, bool nonPublic) { throw null; }
        public static object CreateInstance(System.Type type, params object[] args) { throw null; }
        public static T CreateInstance<T>() { throw null; }
    }
    public partial class ArgumentException : System.Exception
    {
        public ArgumentException() { }
        public ArgumentException(string message) { }
        public ArgumentException(string message, System.Exception innerException) { }
        public ArgumentException(string message, string paramName) { }
        public ArgumentException(string message, string paramName, System.Exception innerException) { }
        public override string Message { get { throw null; } }
        public virtual string ParamName { get { throw null; } }
    }
    public partial class ArgumentNullException : System.ArgumentException
    {
        public ArgumentNullException() { }
        public ArgumentNullException(string paramName) { }
        public ArgumentNullException(string message, System.Exception innerException) { }
        public ArgumentNullException(string paramName, string message) { }
    }
    public partial class ArgumentOutOfRangeException : System.ArgumentException
    {
        public ArgumentOutOfRangeException() { }
        public ArgumentOutOfRangeException(string paramName) { }
        public ArgumentOutOfRangeException(string message, System.Exception innerException) { }
        public ArgumentOutOfRangeException(string paramName, object actualValue, string message) { }
        public ArgumentOutOfRangeException(string paramName, string message) { }
        public virtual object ActualValue { get { throw null; } }
        public override string Message { get { throw null; } }
    }
    public partial class ArithmeticException : System.Exception
    {
        public ArithmeticException() { }
        public ArithmeticException(string message) { }
        public ArithmeticException(string message, System.Exception innerException) { }
    }
    public abstract partial class Array : System.Collections.ICollection, System.Collections.IEnumerable, System.Collections.IList, System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable
    {
        internal Array() { }
        public int Length { get { throw null; } }
        public int Rank { get { throw null; } }
        int System.Collections.ICollection.Count { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        bool System.Collections.IList.IsFixedSize { get { throw null; } }
        bool System.Collections.IList.IsReadOnly { get { throw null; } }
        object System.Collections.IList.this[int index] { get { throw null; } set { } }
        public static int BinarySearch(System.Array array, int index, int length, object value) { throw null; }
        public static int BinarySearch(System.Array array, int index, int length, object value, System.Collections.IComparer comparer) { throw null; }
        public static int BinarySearch(System.Array array, object value) { throw null; }
        public static int BinarySearch(System.Array array, object value, System.Collections.IComparer comparer) { throw null; }
        public static int BinarySearch<T>(T[] array, int index, int length, T value) { throw null; }
        public static int BinarySearch<T>(T[] array, int index, int length, T value, System.Collections.Generic.IComparer<T> comparer) { throw null; }
        public static int BinarySearch<T>(T[] array, T value) { throw null; }
        public static int BinarySearch<T>(T[] array, T value, System.Collections.Generic.IComparer<T> comparer) { throw null; }
        public static void Clear(System.Array array, int index, int length) { }
        public object Clone() { throw null; }
        public static void ConstrainedCopy(System.Array sourceArray, int sourceIndex, System.Array destinationArray, int destinationIndex, int length) { }
        public static void Copy(System.Array sourceArray, System.Array destinationArray, int length) { }
        public static void Copy(System.Array sourceArray, int sourceIndex, System.Array destinationArray, int destinationIndex, int length) { }
        public void CopyTo(System.Array array, int index) { }
        public static System.Array CreateInstance(System.Type elementType, int length) { throw null; }
        public static System.Array CreateInstance(System.Type elementType, params int[] lengths) { throw null; }
        public static System.Array CreateInstance(System.Type elementType, int[] lengths, int[] lowerBounds) { throw null; }
        public static T[] Empty<T>() { throw null; }
        public static bool Exists<T>(T[] array, System.Predicate<T> match) { throw null; }
        public static T[] FindAll<T>(T[] array, System.Predicate<T> match) { throw null; }
        public static int FindIndex<T>(T[] array, int startIndex, int count, System.Predicate<T> match) { throw null; }
        public static int FindIndex<T>(T[] array, int startIndex, System.Predicate<T> match) { throw null; }
        public static int FindIndex<T>(T[] array, System.Predicate<T> match) { throw null; }
        public static int FindLastIndex<T>(T[] array, int startIndex, int count, System.Predicate<T> match) { throw null; }
        public static int FindLastIndex<T>(T[] array, int startIndex, System.Predicate<T> match) { throw null; }
        public static int FindLastIndex<T>(T[] array, System.Predicate<T> match) { throw null; }
        public static T FindLast<T>(T[] array, System.Predicate<T> match) { throw null; }
        public static T Find<T>(T[] array, System.Predicate<T> match) { throw null; }
        public System.Collections.IEnumerator GetEnumerator() { throw null; }
        public int GetLength(int dimension) { throw null; }
        public int GetLowerBound(int dimension) { throw null; }
        public int GetUpperBound(int dimension) { throw null; }
        public object GetValue(int index) { throw null; }
        public object GetValue(params int[] indices) { throw null; }
        public static int IndexOf(System.Array array, object value) { throw null; }
        public static int IndexOf(System.Array array, object value, int startIndex) { throw null; }
        public static int IndexOf(System.Array array, object value, int startIndex, int count) { throw null; }
        public static int IndexOf<T>(T[] array, T value) { throw null; }
        public static int IndexOf<T>(T[] array, T value, int startIndex) { throw null; }
        public static int IndexOf<T>(T[] array, T value, int startIndex, int count) { throw null; }
        public void Initialize() { }
        public static int LastIndexOf(System.Array array, object value) { throw null; }
        public static int LastIndexOf(System.Array array, object value, int startIndex) { throw null; }
        public static int LastIndexOf(System.Array array, object value, int startIndex, int count) { throw null; }
        public static int LastIndexOf<T>(T[] array, T value) { throw null; }
        public static int LastIndexOf<T>(T[] array, T value, int startIndex) { throw null; }
        public static int LastIndexOf<T>(T[] array, T value, int startIndex, int count) { throw null; }
        public static void Resize<T>(ref T[] array, int newSize) { }
        public static void Reverse(System.Array array) { }
        public static void Reverse(System.Array array, int index, int length) { }
        public void SetValue(object value, int index) { }
        public void SetValue(object value, params int[] indices) { }
        public static void Sort(System.Array array) { }
        public static void Sort(System.Array keys, System.Array items) { }
        public static void Sort(System.Array keys, System.Array items, System.Collections.IComparer comparer) { }
        public static void Sort(System.Array keys, System.Array items, int index, int length) { }
        public static void Sort(System.Array keys, System.Array items, int index, int length, System.Collections.IComparer comparer) { }
        public static void Sort(System.Array array, System.Collections.IComparer comparer) { }
        public static void Sort(System.Array array, int index, int length) { }
        public static void Sort(System.Array array, int index, int length, System.Collections.IComparer comparer) { }
        public static void Sort<T>(T[] array) { }
        public static void Sort<T>(T[] array, System.Collections.Generic.IComparer<T> comparer) { }
        public static void Sort<T>(T[] array, System.Comparison<T> comparison) { }
        public static void Sort<T>(T[] array, int index, int length) { }
        public static void Sort<T>(T[] array, int index, int length, System.Collections.Generic.IComparer<T> comparer) { }
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items) { }
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, System.Collections.Generic.IComparer<TKey> comparer) { }
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length) { }
        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length, System.Collections.Generic.IComparer<TKey> comparer) { }
        int System.Collections.IList.Add(object value) { throw null; }
        void System.Collections.IList.Clear() { }
        bool System.Collections.IList.Contains(object value) { throw null; }
        int System.Collections.IList.IndexOf(object value) { throw null; }
        void System.Collections.IList.Insert(int index, object value) { }
        void System.Collections.IList.Remove(object value) { }
        void System.Collections.IList.RemoveAt(int index) { }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        public static bool TrueForAll<T>(T[] array, System.Predicate<T> match) { throw null; }
    }
    public partial struct ArraySegment<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, System.Collections.Generic.IList<T>, System.Collections.Generic.IReadOnlyCollection<T>, System.Collections.Generic.IReadOnlyList<T>, System.Collections.IEnumerable
    {
        public ArraySegment(T[] array) { throw null; }
        public ArraySegment(T[] array, int offset, int count) { throw null; }
        public T[] Array { get { throw null; } }
        public int Count { get { throw null; } }
        public int Offset { get { throw null; } }
        bool System.Collections.Generic.ICollection<T>.IsReadOnly { get { throw null; } }
        T System.Collections.Generic.IList<T>.this[int index] { get { throw null; } set { } }
        T System.Collections.Generic.IReadOnlyList<T>.this[int index] { get { throw null; } }
        public bool Equals(System.ArraySegment<T> obj) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.ArraySegment<T> a, System.ArraySegment<T> b) { throw null; }
        public static bool operator !=(System.ArraySegment<T> a, System.ArraySegment<T> b) { throw null; }
        void System.Collections.Generic.ICollection<T>.Add(T item) { }
        void System.Collections.Generic.ICollection<T>.Clear() { }
        bool System.Collections.Generic.ICollection<T>.Contains(T item) { throw null; }
        void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) { }
        bool System.Collections.Generic.ICollection<T>.Remove(T item) { throw null; }
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() { throw null; }
        int System.Collections.Generic.IList<T>.IndexOf(T item) { throw null; }
        void System.Collections.Generic.IList<T>.Insert(int index, T item) { }
        void System.Collections.Generic.IList<T>.RemoveAt(int index) { }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
    }
    public partial class ArrayTypeMismatchException : System.Exception
    {
        public ArrayTypeMismatchException() { }
        public ArrayTypeMismatchException(string message) { }
        public ArrayTypeMismatchException(string message, System.Exception innerException) { }
    }
    public delegate void AsyncCallback(System.IAsyncResult ar);
    [System.AttributeUsageAttribute(System.AttributeTargets.All, Inherited=true, AllowMultiple=false)]
    public abstract partial class Attribute
    {
        protected Attribute() { }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    [System.FlagsAttribute]
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
        All = 32767,
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class, Inherited=true)]
    public sealed partial class AttributeUsageAttribute : System.Attribute
    {
        public AttributeUsageAttribute(System.AttributeTargets validOn) { }
        public bool AllowMultiple { get { throw null; } set { } }
        public bool Inherited { get { throw null; } set { } }
        public System.AttributeTargets ValidOn { get { throw null; } }
    }
    public partial class BadImageFormatException : System.Exception
    {
        public BadImageFormatException() { }
        public BadImageFormatException(string message) { }
        public BadImageFormatException(string message, System.Exception inner) { }
        public BadImageFormatException(string message, string fileName) { }
        public BadImageFormatException(string message, string fileName, System.Exception inner) { }
        public string FileName { get { throw null; } }
        public override string Message { get { throw null; } }
        public override string ToString() { throw null; }
    }
    public partial struct Boolean : System.IComparable, System.IComparable<bool>, System.IConvertible, System.IEquatable<bool>
    {
        public static readonly string FalseString;
        public static readonly string TrueString;
        public int CompareTo(System.Boolean value) { throw null; }
        public System.Boolean Equals(System.Boolean obj) { throw null; }
        public override System.Boolean Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Boolean Parse(string value) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        System.Boolean System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        string System.IConvertible.ToString(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public static System.Boolean TryParse(string value, out System.Boolean result) { throw null; }
    }
    public static partial class Buffer
    {
        public static void BlockCopy(System.Array src, int srcOffset, System.Array dst, int dstOffset, int count) { }
        public static int ByteLength(System.Array array) { throw null; }
        public static byte GetByte(System.Array array, int index) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe static void MemoryCopy(void* source, void* destination, long destinationSizeInBytes, long sourceBytesToCopy) { }
        [System.CLSCompliantAttribute(false)]
        public unsafe static void MemoryCopy(void* source, void* destination, ulong destinationSizeInBytes, ulong sourceBytesToCopy) { }
        public static void SetByte(System.Array array, int index, byte value) { }
    }
    public partial struct Byte : System.IComparable, System.IComparable<byte>, System.IConvertible, System.IEquatable<byte>, System.IFormattable
    {
        public const byte MaxValue = (byte)255;
        public const byte MinValue = (byte)0;
        public int CompareTo(System.Byte value) { throw null; }
        public bool Equals(System.Byte obj) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Byte Parse(string s) { throw null; }
        public static System.Byte Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        public static System.Byte Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        public static System.Byte Parse(string s, System.IFormatProvider provider) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        System.Byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        public static bool TryParse(string s, out System.Byte result) { throw null; }
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.Byte result) { throw null; }
    }
    public partial struct Char : System.IComparable, System.IComparable<char>, System.IConvertible, System.IEquatable<char>
    {
        public const char MaxValue = '\uFFFF';
        public const char MinValue = '\0';
        public int CompareTo(System.Char value) { throw null; }
        public static string ConvertFromUtf32(int utf32) { throw null; }
        public static int ConvertToUtf32(System.Char highSurrogate, System.Char lowSurrogate) { throw null; }
        public static int ConvertToUtf32(string s, int index) { throw null; }
        public bool Equals(System.Char obj) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static double GetNumericValue(System.Char c) { throw null; }
        public static double GetNumericValue(string s, int index) { throw null; }
        public static bool IsControl(System.Char c) { throw null; }
        public static bool IsControl(string s, int index) { throw null; }
        public static bool IsDigit(System.Char c) { throw null; }
        public static bool IsDigit(string s, int index) { throw null; }
        public static bool IsHighSurrogate(System.Char c) { throw null; }
        public static bool IsHighSurrogate(string s, int index) { throw null; }
        public static bool IsLetter(System.Char c) { throw null; }
        public static bool IsLetter(string s, int index) { throw null; }
        public static bool IsLetterOrDigit(System.Char c) { throw null; }
        public static bool IsLetterOrDigit(string s, int index) { throw null; }
        public static bool IsLower(System.Char c) { throw null; }
        public static bool IsLower(string s, int index) { throw null; }
        public static bool IsLowSurrogate(System.Char c) { throw null; }
        public static bool IsLowSurrogate(string s, int index) { throw null; }
        public static bool IsNumber(System.Char c) { throw null; }
        public static bool IsNumber(string s, int index) { throw null; }
        public static bool IsPunctuation(System.Char c) { throw null; }
        public static bool IsPunctuation(string s, int index) { throw null; }
        public static bool IsSeparator(System.Char c) { throw null; }
        public static bool IsSeparator(string s, int index) { throw null; }
        public static bool IsSurrogate(System.Char c) { throw null; }
        public static bool IsSurrogate(string s, int index) { throw null; }
        public static bool IsSurrogatePair(System.Char highSurrogate, System.Char lowSurrogate) { throw null; }
        public static bool IsSurrogatePair(string s, int index) { throw null; }
        public static bool IsSymbol(System.Char c) { throw null; }
        public static bool IsSymbol(string s, int index) { throw null; }
        public static bool IsUpper(System.Char c) { throw null; }
        public static bool IsUpper(string s, int index) { throw null; }
        public static bool IsWhiteSpace(System.Char c) { throw null; }
        public static bool IsWhiteSpace(string s, int index) { throw null; }
        public static System.Char Parse(string s) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        System.Char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        string System.IConvertible.ToString(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public static System.Char ToLower(System.Char c) { throw null; }
        public static System.Char ToLowerInvariant(System.Char c) { throw null; }
        public override string ToString() { throw null; }
        public static string ToString(System.Char c) { throw null; }
        public static System.Char ToUpper(System.Char c) { throw null; }
        public static System.Char ToUpperInvariant(System.Char c) { throw null; }
        public static bool TryParse(string s, out System.Char result) { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.All, Inherited=true, AllowMultiple=false)]
    public sealed partial class CLSCompliantAttribute : System.Attribute
    {
        public CLSCompliantAttribute(bool isCompliant) { }
        public bool IsCompliant { get { throw null; } }
    }
    public delegate int Comparison<in T>(T x, T y);
    public partial struct DateTime : System.IComparable, System.IComparable<System.DateTime>, System.IConvertible, System.IEquatable<System.DateTime>, System.IFormattable
    {
        public static readonly System.DateTime MaxValue;
        public static readonly System.DateTime MinValue;
        public DateTime(int year, int month, int day) { throw null; }
        public DateTime(int year, int month, int day, int hour, int minute, int second) { throw null; }
        public DateTime(int year, int month, int day, int hour, int minute, int second, System.DateTimeKind kind) { throw null; }
        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond) { throw null; }
        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, System.DateTimeKind kind) { throw null; }
        public DateTime(long ticks) { throw null; }
        public DateTime(long ticks, System.DateTimeKind kind) { throw null; }
        public System.DateTime Date { get { throw null; } }
        public int Day { get { throw null; } }
        public System.DayOfWeek DayOfWeek { get { throw null; } }
        public int DayOfYear { get { throw null; } }
        public int Hour { get { throw null; } }
        public System.DateTimeKind Kind { get { throw null; } }
        public int Millisecond { get { throw null; } }
        public int Minute { get { throw null; } }
        public int Month { get { throw null; } }
        public static System.DateTime Now { get { throw null; } }
        public int Second { get { throw null; } }
        public long Ticks { get { throw null; } }
        public System.TimeSpan TimeOfDay { get { throw null; } }
        public static System.DateTime Today { get { throw null; } }
        public static System.DateTime UtcNow { get { throw null; } }
        public int Year { get { throw null; } }
        public System.DateTime Add(System.TimeSpan value) { throw null; }
        public System.DateTime AddDays(double value) { throw null; }
        public System.DateTime AddHours(double value) { throw null; }
        public System.DateTime AddMilliseconds(double value) { throw null; }
        public System.DateTime AddMinutes(double value) { throw null; }
        public System.DateTime AddMonths(int months) { throw null; }
        public System.DateTime AddSeconds(double value) { throw null; }
        public System.DateTime AddTicks(long value) { throw null; }
        public System.DateTime AddYears(int value) { throw null; }
        public static int Compare(System.DateTime t1, System.DateTime t2) { throw null; }
        public int CompareTo(System.DateTime value) { throw null; }
        public static int DaysInMonth(int year, int month) { throw null; }
        public bool Equals(System.DateTime value) { throw null; }
        public static bool Equals(System.DateTime t1, System.DateTime t2) { throw null; }
        public override bool Equals(object value) { throw null; }
        public static System.DateTime FromBinary(long dateData) { throw null; }
        public static System.DateTime FromFileTime(long fileTime) { throw null; }
        public static System.DateTime FromFileTimeUtc(long fileTime) { throw null; }
        public string[] GetDateTimeFormats() { throw null; }
        public string[] GetDateTimeFormats(char format) { throw null; }
        public string[] GetDateTimeFormats(char format, System.IFormatProvider provider) { throw null; }
        public string[] GetDateTimeFormats(System.IFormatProvider provider) { throw null; }
        public override int GetHashCode() { throw null; }
        public bool IsDaylightSavingTime() { throw null; }
        public static bool IsLeapYear(int year) { throw null; }
        public static System.DateTime operator +(System.DateTime d, System.TimeSpan t) { throw null; }
        public static bool operator ==(System.DateTime d1, System.DateTime d2) { throw null; }
        public static bool operator >(System.DateTime t1, System.DateTime t2) { throw null; }
        public static bool operator >=(System.DateTime t1, System.DateTime t2) { throw null; }
        public static bool operator !=(System.DateTime d1, System.DateTime d2) { throw null; }
        public static bool operator <(System.DateTime t1, System.DateTime t2) { throw null; }
        public static bool operator <=(System.DateTime t1, System.DateTime t2) { throw null; }
        public static System.TimeSpan operator -(System.DateTime d1, System.DateTime d2) { throw null; }
        public static System.DateTime operator -(System.DateTime d, System.TimeSpan t) { throw null; }
        public static System.DateTime Parse(string s) { throw null; }
        public static System.DateTime Parse(string s, System.IFormatProvider provider) { throw null; }
        public static System.DateTime Parse(string s, System.IFormatProvider provider, System.Globalization.DateTimeStyles styles) { throw null; }
        public static System.DateTime ParseExact(string s, string format, System.IFormatProvider provider) { throw null; }
        public static System.DateTime ParseExact(string s, string format, System.IFormatProvider provider, System.Globalization.DateTimeStyles style) { throw null; }
        public static System.DateTime ParseExact(string s, string[] formats, System.IFormatProvider provider, System.Globalization.DateTimeStyles style) { throw null; }
        public static System.DateTime SpecifyKind(System.DateTime value, System.DateTimeKind kind) { throw null; }
        public System.TimeSpan Subtract(System.DateTime value) { throw null; }
        public System.DateTime Subtract(System.TimeSpan value) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public long ToBinary() { throw null; }
        public long ToFileTime() { throw null; }
        public long ToFileTimeUtc() { throw null; }
        public System.DateTime ToLocalTime() { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        public System.DateTime ToUniversalTime() { throw null; }
        public static bool TryParse(string s, out System.DateTime result) { throw null; }
        public static bool TryParse(string s, System.IFormatProvider provider, System.Globalization.DateTimeStyles styles, out System.DateTime result) { throw null; }
        public static bool TryParseExact(string s, string format, System.IFormatProvider provider, System.Globalization.DateTimeStyles style, out System.DateTime result) { throw null; }
        public static bool TryParseExact(string s, string[] formats, System.IFormatProvider provider, System.Globalization.DateTimeStyles style, out System.DateTime result) { throw null; }
    }
    public enum DateTimeKind
    {
        Unspecified = 0,
        Utc = 1,
        Local = 2,
    }
    public partial struct DateTimeOffset : System.IComparable, System.IComparable<System.DateTimeOffset>, System.IEquatable<System.DateTimeOffset>, System.IFormattable
    {
        public static readonly System.DateTimeOffset MaxValue;
        public static readonly System.DateTimeOffset MinValue;
        public DateTimeOffset(System.DateTime dateTime) { throw null; }
        public DateTimeOffset(System.DateTime dateTime, System.TimeSpan offset) { throw null; }
        public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, System.TimeSpan offset) { throw null; }
        public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, System.TimeSpan offset) { throw null; }
        public DateTimeOffset(long ticks, System.TimeSpan offset) { throw null; }
        public System.DateTime Date { get { throw null; } }
        public System.DateTime DateTime { get { throw null; } }
        public int Day { get { throw null; } }
        public System.DayOfWeek DayOfWeek { get { throw null; } }
        public int DayOfYear { get { throw null; } }
        public int Hour { get { throw null; } }
        public System.DateTime LocalDateTime { get { throw null; } }
        public int Millisecond { get { throw null; } }
        public int Minute { get { throw null; } }
        public int Month { get { throw null; } }
        public static System.DateTimeOffset Now { get { throw null; } }
        public System.TimeSpan Offset { get { throw null; } }
        public int Second { get { throw null; } }
        public long Ticks { get { throw null; } }
        public System.TimeSpan TimeOfDay { get { throw null; } }
        public System.DateTime UtcDateTime { get { throw null; } }
        public static System.DateTimeOffset UtcNow { get { throw null; } }
        public long UtcTicks { get { throw null; } }
        public int Year { get { throw null; } }
        public System.DateTimeOffset Add(System.TimeSpan timeSpan) { throw null; }
        public System.DateTimeOffset AddDays(double days) { throw null; }
        public System.DateTimeOffset AddHours(double hours) { throw null; }
        public System.DateTimeOffset AddMilliseconds(double milliseconds) { throw null; }
        public System.DateTimeOffset AddMinutes(double minutes) { throw null; }
        public System.DateTimeOffset AddMonths(int months) { throw null; }
        public System.DateTimeOffset AddSeconds(double seconds) { throw null; }
        public System.DateTimeOffset AddTicks(long ticks) { throw null; }
        public System.DateTimeOffset AddYears(int years) { throw null; }
        public static int Compare(System.DateTimeOffset first, System.DateTimeOffset second) { throw null; }
        public int CompareTo(System.DateTimeOffset other) { throw null; }
        public bool Equals(System.DateTimeOffset other) { throw null; }
        public static bool Equals(System.DateTimeOffset first, System.DateTimeOffset second) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool EqualsExact(System.DateTimeOffset other) { throw null; }
        public static System.DateTimeOffset FromFileTime(long fileTime) { throw null; }
        public static System.DateTimeOffset FromUnixTimeMilliseconds(long milliseconds) { throw null; }
        public static System.DateTimeOffset FromUnixTimeSeconds(long seconds) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.DateTimeOffset operator +(System.DateTimeOffset dateTimeOffset, System.TimeSpan timeSpan) { throw null; }
        public static bool operator ==(System.DateTimeOffset left, System.DateTimeOffset right) { throw null; }
        public static bool operator >(System.DateTimeOffset left, System.DateTimeOffset right) { throw null; }
        public static bool operator >=(System.DateTimeOffset left, System.DateTimeOffset right) { throw null; }
        public static implicit operator System.DateTimeOffset (System.DateTime dateTime) { throw null; }
        public static bool operator !=(System.DateTimeOffset left, System.DateTimeOffset right) { throw null; }
        public static bool operator <(System.DateTimeOffset left, System.DateTimeOffset right) { throw null; }
        public static bool operator <=(System.DateTimeOffset left, System.DateTimeOffset right) { throw null; }
        public static System.TimeSpan operator -(System.DateTimeOffset left, System.DateTimeOffset right) { throw null; }
        public static System.DateTimeOffset operator -(System.DateTimeOffset dateTimeOffset, System.TimeSpan timeSpan) { throw null; }
        public static System.DateTimeOffset Parse(string input) { throw null; }
        public static System.DateTimeOffset Parse(string input, System.IFormatProvider formatProvider) { throw null; }
        public static System.DateTimeOffset Parse(string input, System.IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles) { throw null; }
        public static System.DateTimeOffset ParseExact(string input, string format, System.IFormatProvider formatProvider) { throw null; }
        public static System.DateTimeOffset ParseExact(string input, string format, System.IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles) { throw null; }
        public static System.DateTimeOffset ParseExact(string input, string[] formats, System.IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles) { throw null; }
        public System.TimeSpan Subtract(System.DateTimeOffset value) { throw null; }
        public System.DateTimeOffset Subtract(System.TimeSpan value) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        public long ToFileTime() { throw null; }
        public System.DateTimeOffset ToLocalTime() { throw null; }
        public System.DateTimeOffset ToOffset(System.TimeSpan offset) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider formatProvider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider formatProvider) { throw null; }
        public System.DateTimeOffset ToUniversalTime() { throw null; }
        public long ToUnixTimeMilliseconds() { throw null; }
        public long ToUnixTimeSeconds() { throw null; }
        public static bool TryParse(string input, out System.DateTimeOffset result) { throw null; }
        public static bool TryParse(string input, System.IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles, out System.DateTimeOffset result) { throw null; }
        public static bool TryParseExact(string input, string format, System.IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles, out System.DateTimeOffset result) { throw null; }
        public static bool TryParseExact(string input, string[] formats, System.IFormatProvider formatProvider, System.Globalization.DateTimeStyles styles, out System.DateTimeOffset result) { throw null; }
    }
    public enum DayOfWeek
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
    }
    public partial struct Decimal : System.IComparable, System.IComparable<decimal>, System.IConvertible, System.IEquatable<decimal>, System.IFormattable
    {
        [System.Runtime.CompilerServices.DecimalConstantAttribute((byte)0, (byte)0, (uint)4294967295, (uint)4294967295, (uint)4294967295)]
        public static readonly decimal MaxValue;
        [System.Runtime.CompilerServices.DecimalConstantAttribute((byte)0, (byte)128, (uint)0, (uint)0, (uint)1)]
        public static readonly decimal MinusOne;
        [System.Runtime.CompilerServices.DecimalConstantAttribute((byte)0, (byte)128, (uint)4294967295, (uint)4294967295, (uint)4294967295)]
        public static readonly decimal MinValue;
        [System.Runtime.CompilerServices.DecimalConstantAttribute((byte)0, (byte)0, (uint)0, (uint)0, (uint)1)]
        public static readonly decimal One;
        [System.Runtime.CompilerServices.DecimalConstantAttribute((byte)0, (byte)0, (uint)0, (uint)0, (uint)0)]
        public static readonly decimal Zero;
        public Decimal(double value) { throw null; }
        public Decimal(int value) { throw null; }
        public Decimal(int lo, int mid, int hi, bool isNegative, byte scale) { throw null; }
        public Decimal(int[] bits) { throw null; }
        public Decimal(long value) { throw null; }
        public Decimal(float value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public Decimal(uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public Decimal(ulong value) { throw null; }
        public static System.Decimal Add(System.Decimal d1, System.Decimal d2) { throw null; }
        public static System.Decimal Ceiling(System.Decimal d) { throw null; }
        public static int Compare(System.Decimal d1, System.Decimal d2) { throw null; }
        public int CompareTo(System.Decimal value) { throw null; }
        public static System.Decimal Divide(System.Decimal d1, System.Decimal d2) { throw null; }
        public bool Equals(System.Decimal value) { throw null; }
        public static bool Equals(System.Decimal d1, System.Decimal d2) { throw null; }
        public override bool Equals(object value) { throw null; }
        public static System.Decimal Floor(System.Decimal d) { throw null; }
        public static int[] GetBits(System.Decimal d) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Decimal Multiply(System.Decimal d1, System.Decimal d2) { throw null; }
        public static System.Decimal Negate(System.Decimal d) { throw null; }
        public static System.Decimal operator +(System.Decimal d1, System.Decimal d2) { throw null; }
        public static System.Decimal operator --(System.Decimal d) { throw null; }
        public static System.Decimal operator /(System.Decimal d1, System.Decimal d2) { throw null; }
        public static bool operator ==(System.Decimal d1, System.Decimal d2) { throw null; }
        public static explicit operator byte (System.Decimal value) { throw null; }
        public static explicit operator char (System.Decimal value) { throw null; }
        public static explicit operator double (System.Decimal value) { throw null; }
        public static explicit operator short (System.Decimal value) { throw null; }
        public static explicit operator int (System.Decimal value) { throw null; }
        public static explicit operator long (System.Decimal value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator sbyte (System.Decimal value) { throw null; }
        public static explicit operator float (System.Decimal value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator ushort (System.Decimal value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator uint (System.Decimal value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator ulong (System.Decimal value) { throw null; }
        public static explicit operator System.Decimal (double value) { throw null; }
        public static explicit operator System.Decimal (float value) { throw null; }
        public static bool operator >(System.Decimal d1, System.Decimal d2) { throw null; }
        public static bool operator >=(System.Decimal d1, System.Decimal d2) { throw null; }
        public static implicit operator System.Decimal (byte value) { throw null; }
        public static implicit operator System.Decimal (char value) { throw null; }
        public static implicit operator System.Decimal (short value) { throw null; }
        public static implicit operator System.Decimal (int value) { throw null; }
        public static implicit operator System.Decimal (long value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static implicit operator System.Decimal (sbyte value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static implicit operator System.Decimal (ushort value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static implicit operator System.Decimal (uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static implicit operator System.Decimal (ulong value) { throw null; }
        public static System.Decimal operator ++(System.Decimal d) { throw null; }
        public static bool operator !=(System.Decimal d1, System.Decimal d2) { throw null; }
        public static bool operator <(System.Decimal d1, System.Decimal d2) { throw null; }
        public static bool operator <=(System.Decimal d1, System.Decimal d2) { throw null; }
        public static System.Decimal operator %(System.Decimal d1, System.Decimal d2) { throw null; }
        public static System.Decimal operator *(System.Decimal d1, System.Decimal d2) { throw null; }
        public static System.Decimal operator -(System.Decimal d1, System.Decimal d2) { throw null; }
        public static System.Decimal operator -(System.Decimal d) { throw null; }
        public static System.Decimal operator +(System.Decimal d) { throw null; }
        public static System.Decimal Parse(string s) { throw null; }
        public static System.Decimal Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        public static System.Decimal Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        public static System.Decimal Parse(string s, System.IFormatProvider provider) { throw null; }
        public static System.Decimal Remainder(System.Decimal d1, System.Decimal d2) { throw null; }
        public static System.Decimal Subtract(System.Decimal d1, System.Decimal d2) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        System.Decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public static byte ToByte(System.Decimal value) { throw null; }
        public static double ToDouble(System.Decimal d) { throw null; }
        public static short ToInt16(System.Decimal value) { throw null; }
        public static int ToInt32(System.Decimal d) { throw null; }
        public static long ToInt64(System.Decimal d) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static sbyte ToSByte(System.Decimal value) { throw null; }
        public static float ToSingle(System.Decimal d) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ushort ToUInt16(System.Decimal value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static uint ToUInt32(System.Decimal d) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ulong ToUInt64(System.Decimal d) { throw null; }
        public static System.Decimal Truncate(System.Decimal d) { throw null; }
        public static bool TryParse(string s, out System.Decimal result) { throw null; }
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.Decimal result) { throw null; }
    }
    public abstract partial class Delegate
    {
        internal Delegate() { }
        public object Target { get { throw null; } }
        public static System.Delegate Combine(System.Delegate a, System.Delegate b) { throw null; }
        public static System.Delegate Combine(params System.Delegate[] delegates) { throw null; }
        public object DynamicInvoke(params object[] args) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public virtual System.Delegate[] GetInvocationList() { throw null; }
        public static bool operator ==(System.Delegate d1, System.Delegate d2) { throw null; }
        public static bool operator !=(System.Delegate d1, System.Delegate d2) { throw null; }
        public static System.Delegate Remove(System.Delegate source, System.Delegate value) { throw null; }
        public static System.Delegate RemoveAll(System.Delegate source, System.Delegate value) { throw null; }
    }
    public partial class DivideByZeroException : System.ArithmeticException
    {
        public DivideByZeroException() { }
        public DivideByZeroException(string message) { }
        public DivideByZeroException(string message, System.Exception innerException) { }
    }
    public partial struct Double : System.IComparable, System.IComparable<double>, System.IConvertible, System.IEquatable<double>, System.IFormattable
    {
        public const double Epsilon = 5E-324;
        public const double MaxValue = 1.7976931348623157E+308;
        public const double MinValue = -1.7976931348623157E+308;
        public const double NaN = 0.0 / 0.0;
        public const double NegativeInfinity = -1.0 / 0.0;
        public const double PositiveInfinity = 1.0 / 0.0;
        public int CompareTo(System.Double value) { throw null; }
        public bool Equals(System.Double obj) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool IsInfinity(System.Double d) { throw null; }
        public static bool IsNaN(System.Double d) { throw null; }
        public static bool IsNegativeInfinity(System.Double d) { throw null; }
        public static bool IsPositiveInfinity(System.Double d) { throw null; }
        public static bool operator ==(System.Double left, System.Double right) { throw null; }
        public static bool operator >(System.Double left, System.Double right) { throw null; }
        public static bool operator >=(System.Double left, System.Double right) { throw null; }
        public static bool operator !=(System.Double left, System.Double right) { throw null; }
        public static bool operator <(System.Double left, System.Double right) { throw null; }
        public static bool operator <=(System.Double left, System.Double right) { throw null; }
        public static System.Double Parse(string s) { throw null; }
        public static System.Double Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        public static System.Double Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        public static System.Double Parse(string s, System.IFormatProvider provider) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        System.Double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        public static bool TryParse(string s, out System.Double result) { throw null; }
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.Double result) { throw null; }
    }
    public abstract partial class Enum : System.ValueType, System.IComparable, System.IConvertible, System.IFormattable
    {
        protected Enum() { }
        public int CompareTo(object target) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public static string Format(System.Type enumType, object value, string format) { throw null; }
        public override int GetHashCode() { throw null; }
        public static string GetName(System.Type enumType, object value) { throw null; }
        public static string[] GetNames(System.Type enumType) { throw null; }
        public static System.Type GetUnderlyingType(System.Type enumType) { throw null; }
        public static System.Array GetValues(System.Type enumType) { throw null; }
        public bool HasFlag(System.Enum flag) { throw null; }
        public static bool IsDefined(System.Type enumType, object value) { throw null; }
        public static object Parse(System.Type enumType, string value) { throw null; }
        public static object Parse(System.Type enumType, string value, bool ignoreCase) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        [System.ObsoleteAttribute("The provider argument is not used. Please use ToString().")]
        string System.IConvertible.ToString(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        [System.ObsoleteAttribute("The provider argument is not used. Please use ToString(String).")]
        string System.IFormattable.ToString(string format, System.IFormatProvider provider) { throw null; }
        public static object ToObject(System.Type enumType, object value) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(string format) { throw null; }
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct { throw null; }
        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct { throw null; }
    }
    public partial class EventArgs
    {
        public static readonly System.EventArgs Empty;
        public EventArgs() { }
    }
    public delegate void EventHandler(object sender, System.EventArgs e);
    public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e);
    public partial class Exception
    {
        public Exception() { }
        public Exception(string message) { }
        public Exception(string message, System.Exception innerException) { }
        public virtual System.Collections.IDictionary Data { get { throw null; } }
        public virtual string HelpLink { get { throw null; } set { } }
        public int HResult { get { throw null; } protected set { } }
        public System.Exception InnerException { get { throw null; } }
        public virtual string Message { get { throw null; } }
        public virtual string Source { get { throw null; } set { } }
        public virtual string StackTrace { get { throw null; } }
        public virtual System.Exception GetBaseException() { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class FieldAccessException : System.MemberAccessException
    {
        public FieldAccessException() { }
        public FieldAccessException(string message) { }
        public FieldAccessException(string message, System.Exception inner) { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Enum, Inherited=false)]
    public partial class FlagsAttribute : System.Attribute
    {
        public FlagsAttribute() { }
    }
    public partial class FormatException : System.Exception
    {
        public FormatException() { }
        public FormatException(string message) { }
        public FormatException(string message, System.Exception innerException) { }
    }
    public abstract partial class FormattableString : System.IFormattable
    {
        protected FormattableString() { }
        public abstract int ArgumentCount { get; }
        public abstract string Format { get; }
        public abstract object GetArgument(int index);
        public abstract object[] GetArguments();
        public static string Invariant(System.FormattableString formattable) { throw null; }
        string System.IFormattable.ToString(string ignored, System.IFormatProvider formatProvider) { throw null; }
        public override string ToString() { throw null; }
        public abstract string ToString(System.IFormatProvider formatProvider);
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
        public static int MaxGeneration { get { throw null; } }
        public static void AddMemoryPressure(long bytesAllocated) { }
        public static void Collect() { }
        public static void Collect(int generation) { }
        public static void Collect(int generation, System.GCCollectionMode mode) { }
        public static void Collect(int generation, System.GCCollectionMode mode, bool blocking) { }
        public static int CollectionCount(int generation) { throw null; }
        public static int GetGeneration(object obj) { throw null; }
        public static long GetTotalMemory(bool forceFullCollection) { throw null; }
        public static void KeepAlive(object obj) { }
        public static void RemoveMemoryPressure(long bytesAllocated) { }
        public static void ReRegisterForFinalize(object obj) { }
        public static void SuppressFinalize(object obj) { }
        public static void WaitForPendingFinalizers() { }
    }
    public enum GCCollectionMode
    {
        Default = 0,
        Forced = 1,
        Optimized = 2,
    }
    public partial struct Guid : System.IComparable, System.IComparable<System.Guid>, System.IEquatable<System.Guid>, System.IFormattable
    {
        public static readonly System.Guid Empty;
        public Guid(byte[] b) { throw null; }
        public Guid(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) { throw null; }
        public Guid(int a, short b, short c, byte[] d) { throw null; }
        public Guid(string g) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public Guid(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k) { throw null; }
        public int CompareTo(System.Guid value) { throw null; }
        public bool Equals(System.Guid g) { throw null; }
        public override bool Equals(object o) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Guid NewGuid() { throw null; }
        public static bool operator ==(System.Guid a, System.Guid b) { throw null; }
        public static bool operator !=(System.Guid a, System.Guid b) { throw null; }
        public static System.Guid Parse(string input) { throw null; }
        public static System.Guid ParseExact(string input, string format) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        string System.IFormattable.ToString(string format, System.IFormatProvider provider) { throw null; }
        public byte[] ToByteArray() { throw null; }
        public override string ToString() { throw null; }
        public string ToString(string format) { throw null; }
        public static bool TryParse(string input, out System.Guid result) { throw null; }
        public static bool TryParseExact(string input, string format, out System.Guid result) { throw null; }
    }
    public partial interface IAsyncResult
    {
        object AsyncState { get; }
        System.Threading.WaitHandle AsyncWaitHandle { get; }
        bool CompletedSynchronously { get; }
        bool IsCompleted { get; }
    }
    public partial interface IComparable
    {
        int CompareTo(object obj);
    }
    public partial interface IComparable<in T>
    {
        int CompareTo(T other);
    }
    [System.CLSCompliantAttribute(false)]
    public partial interface IConvertible
    {
        System.TypeCode GetTypeCode();
        bool ToBoolean(System.IFormatProvider provider);
        byte ToByte(System.IFormatProvider provider);
        char ToChar(System.IFormatProvider provider);
        System.DateTime ToDateTime(System.IFormatProvider provider);
        decimal ToDecimal(System.IFormatProvider provider);
        double ToDouble(System.IFormatProvider provider);
        short ToInt16(System.IFormatProvider provider);
        int ToInt32(System.IFormatProvider provider);
        long ToInt64(System.IFormatProvider provider);
        sbyte ToSByte(System.IFormatProvider provider);
        float ToSingle(System.IFormatProvider provider);
        string ToString(System.IFormatProvider provider);
        object ToType(System.Type conversionType, System.IFormatProvider provider);
        ushort ToUInt16(System.IFormatProvider provider);
        uint ToUInt32(System.IFormatProvider provider);
        ulong ToUInt64(System.IFormatProvider provider);
    }
    public partial interface ICustomFormatter
    {
        string Format(string format, object arg, System.IFormatProvider formatProvider);
    }
    public partial interface IDisposable
    {
        void Dispose();
    }
    public partial interface IEquatable<T>
    {
        bool Equals(T other);
    }
    public partial interface IFormatProvider
    {
        object GetFormat(System.Type formatType);
    }
    public partial interface IFormattable
    {
        string ToString(string format, System.IFormatProvider formatProvider);
    }
    public sealed partial class IndexOutOfRangeException : System.Exception
    {
        public IndexOutOfRangeException() { }
        public IndexOutOfRangeException(string message) { }
        public IndexOutOfRangeException(string message, System.Exception innerException) { }
    }
    public sealed partial class InsufficientExecutionStackException : System.Exception
    {
        public InsufficientExecutionStackException() { }
        public InsufficientExecutionStackException(string message) { }
        public InsufficientExecutionStackException(string message, System.Exception innerException) { }
    }
    public partial struct Int16 : System.IComparable, System.IComparable<short>, System.IConvertible, System.IEquatable<short>, System.IFormattable
    {
        public const short MaxValue = (short)32767;
        public const short MinValue = (short)-32768;
        public int CompareTo(System.Int16 value) { throw null; }
        public bool Equals(System.Int16 obj) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Int16 Parse(string s) { throw null; }
        public static System.Int16 Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        public static System.Int16 Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        public static System.Int16 Parse(string s, System.IFormatProvider provider) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        System.Int16 System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.Int16 result) { throw null; }
        public static bool TryParse(string s, out System.Int16 result) { throw null; }
    }
    public partial struct Int32 : System.IComparable, System.IComparable<int>, System.IConvertible, System.IEquatable<int>, System.IFormattable
    {
        public const int MaxValue = 2147483647;
        public const int MinValue = -2147483648;
        public System.Int32 CompareTo(System.Int32 value) { throw null; }
        public bool Equals(System.Int32 obj) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override System.Int32 GetHashCode() { throw null; }
        public static System.Int32 Parse(string s) { throw null; }
        public static System.Int32 Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        public static System.Int32 Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        public static System.Int32 Parse(string s, System.IFormatProvider provider) { throw null; }
        System.Int32 System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        System.Int32 System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.Int32 result) { throw null; }
        public static bool TryParse(string s, out System.Int32 result) { throw null; }
    }
    public partial struct Int64 : System.IComparable, System.IComparable<long>, System.IConvertible, System.IEquatable<long>, System.IFormattable
    {
        public const long MaxValue = (long)9223372036854775807;
        public const long MinValue = (long)-9223372036854775808;
        public int CompareTo(System.Int64 value) { throw null; }
        public bool Equals(System.Int64 obj) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Int64 Parse(string s) { throw null; }
        public static System.Int64 Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        public static System.Int64 Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        public static System.Int64 Parse(string s, System.IFormatProvider provider) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        System.Int64 System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.Int64 result) { throw null; }
        public static bool TryParse(string s, out System.Int64 result) { throw null; }
    }
    public partial struct IntPtr
    {
        public static readonly System.IntPtr Zero;
        public IntPtr(int value) { throw null; }
        public IntPtr(long value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe IntPtr(void* value) { throw null; }
        public static int Size { get { throw null; } }
        public static System.IntPtr Add(System.IntPtr pointer, int offset) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.IntPtr operator +(System.IntPtr pointer, int offset) { throw null; }
        public static bool operator ==(System.IntPtr value1, System.IntPtr value2) { throw null; }
        public static explicit operator System.IntPtr (int value) { throw null; }
        public static explicit operator System.IntPtr (long value) { throw null; }
        public static explicit operator int (System.IntPtr value) { throw null; }
        public static explicit operator long (System.IntPtr value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe static explicit operator void* (System.IntPtr value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe static explicit operator System.IntPtr (void* value) { throw null; }
        public static bool operator !=(System.IntPtr value1, System.IntPtr value2) { throw null; }
        public static System.IntPtr operator -(System.IntPtr pointer, int offset) { throw null; }
        public static System.IntPtr Subtract(System.IntPtr pointer, int offset) { throw null; }
        public int ToInt32() { throw null; }
        public long ToInt64() { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe void* ToPointer() { throw null; }
        public override string ToString() { throw null; }
        public string ToString(string format) { throw null; }
    }
    public partial class InvalidCastException : System.Exception
    {
        public InvalidCastException() { }
        public InvalidCastException(string message) { }
        public InvalidCastException(string message, System.Exception innerException) { }
        public InvalidCastException(string message, int errorCode) { }
    }
    public partial class InvalidOperationException : System.Exception
    {
        public InvalidOperationException() { }
        public InvalidOperationException(string message) { }
        public InvalidOperationException(string message, System.Exception innerException) { }
    }
    public sealed partial class InvalidProgramException : System.Exception
    {
        public InvalidProgramException() { }
        public InvalidProgramException(string message) { }
        public InvalidProgramException(string message, System.Exception inner) { }
    }
    public partial class InvalidTimeZoneException : System.Exception
    {
        public InvalidTimeZoneException() { }
        public InvalidTimeZoneException(string message) { }
        public InvalidTimeZoneException(string message, System.Exception innerException) { }
    }
    public partial interface IObservable<out T>
    {
        System.IDisposable Subscribe(System.IObserver<T> observer);
    }
    public partial interface IObserver<in T>
    {
        void OnCompleted();
        void OnError(System.Exception error);
        void OnNext(T value);
    }
    public partial interface IProgress<in T>
    {
        void Report(T value);
    }
    public partial class Lazy<T>
    {
        public Lazy() { }
        public Lazy(bool isThreadSafe) { }
        public Lazy(System.Func<T> valueFactory) { }
        public Lazy(System.Func<T> valueFactory, bool isThreadSafe) { }
        public Lazy(System.Func<T> valueFactory, System.Threading.LazyThreadSafetyMode mode) { }
        public Lazy(System.Threading.LazyThreadSafetyMode mode) { }
        public bool IsValueCreated { get { throw null; } }
        public T Value { get { throw null; } }
        public override string ToString() { throw null; }
    }
    public partial class Lazy<T, TMetadata> : System.Lazy<T>
    {
        public Lazy(System.Func<T> valueFactory, TMetadata metadata) { }
        public Lazy(System.Func<T> valueFactory, TMetadata metadata, bool isThreadSafe) { }
        public Lazy(System.Func<T> valueFactory, TMetadata metadata, System.Threading.LazyThreadSafetyMode mode) { }
        public Lazy(TMetadata metadata) { }
        public Lazy(TMetadata metadata, bool isThreadSafe) { }
        public Lazy(TMetadata metadata, System.Threading.LazyThreadSafetyMode mode) { }
        public TMetadata Metadata { get { throw null; } }
    }
    public partial class MemberAccessException : System.Exception
    {
        public MemberAccessException() { }
        public MemberAccessException(string message) { }
        public MemberAccessException(string message, System.Exception inner) { }
    }
    public partial class MethodAccessException : System.MemberAccessException
    {
        public MethodAccessException() { }
        public MethodAccessException(string message) { }
        public MethodAccessException(string message, System.Exception inner) { }
    }
    public partial class MissingFieldException : System.MissingMemberException
    {
        public MissingFieldException() { }
        public MissingFieldException(string message) { }
        public MissingFieldException(string message, System.Exception inner) { }
        public override string Message { get { throw null; } }
    }
    public partial class MissingMemberException : System.MemberAccessException
    {
        public MissingMemberException() { }
        public MissingMemberException(string message) { }
        public MissingMemberException(string message, System.Exception inner) { }
        public override string Message { get { throw null; } }
    }
    public partial class MissingMethodException : System.MissingMemberException
    {
        public MissingMethodException() { }
        public MissingMethodException(string message) { }
        public MissingMethodException(string message, System.Exception inner) { }
        public override string Message { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method)]
    public sealed partial class MTAThreadAttribute : System.Attribute
    {
        public MTAThreadAttribute() { }
    }
    public abstract partial class MulticastDelegate : System.Delegate
    {
        internal MulticastDelegate() { }
        public sealed override bool Equals(object obj) { throw null; }
        public sealed override int GetHashCode() { throw null; }
        public sealed override System.Delegate[] GetInvocationList() { throw null; }
        public static bool operator ==(System.MulticastDelegate d1, System.MulticastDelegate d2) { throw null; }
        public static bool operator !=(System.MulticastDelegate d1, System.MulticastDelegate d2) { throw null; }
    }
    public partial class NotImplementedException : System.Exception
    {
        public NotImplementedException() { }
        public NotImplementedException(string message) { }
        public NotImplementedException(string message, System.Exception inner) { }
    }
    public partial class NotSupportedException : System.Exception
    {
        public NotSupportedException() { }
        public NotSupportedException(string message) { }
        public NotSupportedException(string message, System.Exception innerException) { }
    }
    public static partial class Nullable
    {
        public static int Compare<T>(T? n1, T? n2) where T : struct { throw null; }
        public static bool Equals<T>(T? n1, T? n2) where T : struct { throw null; }
        public static System.Type GetUnderlyingType(System.Type nullableType) { throw null; }
    }
    public partial struct Nullable<T> where T : struct
    {
        public Nullable(T value) { throw null; }
        public bool HasValue { get { throw null; } }
        public T Value { get { throw null; } }
        public override bool Equals(object other) { throw null; }
        public override int GetHashCode() { throw null; }
        public T GetValueOrDefault() { throw null; }
        public T GetValueOrDefault(T defaultValue) { throw null; }
        public static explicit operator T (T? value) { throw null; }
        public static implicit operator T? (T value) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class NullReferenceException : System.Exception
    {
        public NullReferenceException() { }
        public NullReferenceException(string message) { }
        public NullReferenceException(string message, System.Exception innerException) { }
    }
    public partial class Object
    {
        public Object() { }
        public virtual bool Equals(System.Object obj) { throw null; }
        public static bool Equals(System.Object objA, System.Object objB) { throw null; }
        ~Object() { }
        public virtual int GetHashCode() { throw null; }
        public System.Type GetType() { throw null; }
        protected System.Object MemberwiseClone() { throw null; }
        public static bool ReferenceEquals(System.Object objA, System.Object objB) { throw null; }
        public virtual string ToString() { throw null; }
    }
    public partial class ObjectDisposedException : System.InvalidOperationException
    {
        public ObjectDisposedException(string objectName) { }
        public ObjectDisposedException(string message, System.Exception innerException) { }
        public ObjectDisposedException(string objectName, string message) { }
        public override string Message { get { throw null; } }
        public string ObjectName { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Constructor | System.AttributeTargets.Delegate | System.AttributeTargets.Enum | System.AttributeTargets.Event | System.AttributeTargets.Field | System.AttributeTargets.Interface | System.AttributeTargets.Method | System.AttributeTargets.Property | System.AttributeTargets.Struct, Inherited=false)]
    public sealed partial class ObsoleteAttribute : System.Attribute
    {
        public ObsoleteAttribute() { }
        public ObsoleteAttribute(string message) { }
        public ObsoleteAttribute(string message, bool error) { }
        public bool IsError { get { throw null; } }
        public string Message { get { throw null; } }
    }
    public partial class OutOfMemoryException : System.Exception
    {
        public OutOfMemoryException() { }
        public OutOfMemoryException(string message) { }
        public OutOfMemoryException(string message, System.Exception innerException) { }
    }
    public partial class OverflowException : System.ArithmeticException
    {
        public OverflowException() { }
        public OverflowException(string message) { }
        public OverflowException(string message, System.Exception innerException) { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter, Inherited=true, AllowMultiple=false)]
    public sealed partial class ParamArrayAttribute : System.Attribute
    {
        public ParamArrayAttribute() { }
    }
    public partial class PlatformNotSupportedException : System.NotSupportedException
    {
        public PlatformNotSupportedException() { }
        public PlatformNotSupportedException(string message) { }
        public PlatformNotSupportedException(string message, System.Exception inner) { }
    }
    public delegate bool Predicate<in T>(T obj);
    public partial class RankException : System.Exception
    {
        public RankException() { }
        public RankException(string message) { }
        public RankException(string message, System.Exception innerException) { }
    }
    public partial struct RuntimeFieldHandle
    {
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.RuntimeFieldHandle handle) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.RuntimeFieldHandle left, System.RuntimeFieldHandle right) { throw null; }
        public static bool operator !=(System.RuntimeFieldHandle left, System.RuntimeFieldHandle right) { throw null; }
    }
    public partial struct RuntimeMethodHandle
    {
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.RuntimeMethodHandle handle) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.RuntimeMethodHandle left, System.RuntimeMethodHandle right) { throw null; }
        public static bool operator !=(System.RuntimeMethodHandle left, System.RuntimeMethodHandle right) { throw null; }
    }
    public partial struct RuntimeTypeHandle
    {
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.RuntimeTypeHandle handle) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(object left, System.RuntimeTypeHandle right) { throw null; }
        public static bool operator ==(System.RuntimeTypeHandle left, object right) { throw null; }
        public static bool operator !=(object left, System.RuntimeTypeHandle right) { throw null; }
        public static bool operator !=(System.RuntimeTypeHandle left, object right) { throw null; }
    }
    [System.CLSCompliantAttribute(false)]
    public partial struct SByte : System.IComparable, System.IComparable<sbyte>, System.IConvertible, System.IEquatable<sbyte>, System.IFormattable
    {
        public const sbyte MaxValue = (sbyte)127;
        public const sbyte MinValue = (sbyte)-128;
        public int CompareTo(System.SByte value) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.SByte obj) { throw null; }
        public override int GetHashCode() { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.SByte Parse(string s) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.SByte Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.SByte Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.SByte Parse(string s, System.IFormatProvider provider) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        System.SByte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.SByte result) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(string s, out System.SByte result) { throw null; }
    }
    public partial struct Single : System.IComparable, System.IComparable<float>, System.IConvertible, System.IEquatable<float>, System.IFormattable
    {
        public const float Epsilon = 1E-45f;
        public const float MaxValue = 3.4028235E+38f;
        public const float MinValue = -3.4028235E+38f;
        public const float NaN = 0.0f / 0.0f;
        public const float NegativeInfinity = -1.0f / 0.0f;
        public const float PositiveInfinity = 1.0f / 0.0f;
        public int CompareTo(System.Single value) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Single obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool IsInfinity(System.Single f) { throw null; }
        public static bool IsNaN(System.Single f) { throw null; }
        public static bool IsNegativeInfinity(System.Single f) { throw null; }
        public static bool IsPositiveInfinity(System.Single f) { throw null; }
        public static bool operator ==(System.Single left, System.Single right) { throw null; }
        public static bool operator >(System.Single left, System.Single right) { throw null; }
        public static bool operator >=(System.Single left, System.Single right) { throw null; }
        public static bool operator !=(System.Single left, System.Single right) { throw null; }
        public static bool operator <(System.Single left, System.Single right) { throw null; }
        public static bool operator <=(System.Single left, System.Single right) { throw null; }
        public static System.Single Parse(string s) { throw null; }
        public static System.Single Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        public static System.Single Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        public static System.Single Parse(string s, System.IFormatProvider provider) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        System.Single System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.Single result) { throw null; }
        public static bool TryParse(string s, out System.Single result) { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method)]
    public sealed partial class STAThreadAttribute : System.Attribute
    {
        public STAThreadAttribute() { }
    }
    public sealed partial class String : System.Collections.Generic.IEnumerable<char>, System.Collections.IEnumerable, System.IComparable, System.IComparable<string>, System.IConvertible, System.IEquatable<string>
    {
        public static readonly string Empty;
        [System.CLSCompliantAttribute(false)]
        public unsafe String(char* value) { }
        [System.CLSCompliantAttribute(false)]
        public unsafe String(char* value, int startIndex, int length) { }
        public String(char c, int count) { }
        public String(char[] value) { }
        public String(char[] value, int startIndex, int length) { }
        [System.Runtime.CompilerServices.IndexerName("Chars")]
        public char this[int index] { get { throw null; } }
        public int Length { get { throw null; } }
        public static int Compare(System.String strA, int indexA, System.String strB, int indexB, int length) { throw null; }
        public static int Compare(System.String strA, int indexA, System.String strB, int indexB, int length, System.StringComparison comparisonType) { throw null; }
        public static int Compare(System.String strA, System.String strB) { throw null; }
        public static int Compare(System.String strA, System.String strB, bool ignoreCase) { throw null; }
        public static int Compare(System.String strA, System.String strB, System.StringComparison comparisonType) { throw null; }
        public static int CompareOrdinal(System.String strA, int indexA, System.String strB, int indexB, int length) { throw null; }
        public static int CompareOrdinal(System.String strA, System.String strB) { throw null; }
        public int CompareTo(System.String strB) { throw null; }
        public static System.String Concat(System.Collections.Generic.IEnumerable<string> values) { throw null; }
        public static System.String Concat(object arg0) { throw null; }
        public static System.String Concat(object arg0, object arg1) { throw null; }
        public static System.String Concat(object arg0, object arg1, object arg2) { throw null; }
        public static System.String Concat(params object[] args) { throw null; }
        public static System.String Concat(System.String str0, System.String str1) { throw null; }
        public static System.String Concat(System.String str0, System.String str1, System.String str2) { throw null; }
        public static System.String Concat(System.String str0, System.String str1, System.String str2, System.String str3) { throw null; }
        public static System.String Concat(params string[] values) { throw null; }
        public static System.String Concat<T>(System.Collections.Generic.IEnumerable<T> values) { throw null; }
        public bool Contains(System.String value) { throw null; }
        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) { }
        public bool EndsWith(System.String value) { throw null; }
        public bool EndsWith(System.String value, System.StringComparison comparisonType) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.String value) { throw null; }
        public static bool Equals(System.String a, System.String b) { throw null; }
        public static bool Equals(System.String a, System.String b, System.StringComparison comparisonType) { throw null; }
        public bool Equals(System.String value, System.StringComparison comparisonType) { throw null; }
        public static System.String Format(System.IFormatProvider provider, System.String format, object arg0) { throw null; }
        public static System.String Format(System.IFormatProvider provider, System.String format, object arg0, object arg1) { throw null; }
        public static System.String Format(System.IFormatProvider provider, System.String format, object arg0, object arg1, object arg2) { throw null; }
        public static System.String Format(System.IFormatProvider provider, System.String format, params object[] args) { throw null; }
        public static System.String Format(System.String format, object arg0) { throw null; }
        public static System.String Format(System.String format, object arg0, object arg1) { throw null; }
        public static System.String Format(System.String format, object arg0, object arg1, object arg2) { throw null; }
        public static System.String Format(System.String format, params object[] args) { throw null; }
        public override int GetHashCode() { throw null; }
        public int IndexOf(char value) { throw null; }
        public int IndexOf(char value, int startIndex) { throw null; }
        public int IndexOf(char value, int startIndex, int count) { throw null; }
        public int IndexOf(System.String value) { throw null; }
        public int IndexOf(System.String value, int startIndex) { throw null; }
        public int IndexOf(System.String value, int startIndex, int count) { throw null; }
        public int IndexOf(System.String value, int startIndex, int count, System.StringComparison comparisonType) { throw null; }
        public int IndexOf(System.String value, int startIndex, System.StringComparison comparisonType) { throw null; }
        public int IndexOf(System.String value, System.StringComparison comparisonType) { throw null; }
        public int IndexOfAny(char[] anyOf) { throw null; }
        public int IndexOfAny(char[] anyOf, int startIndex) { throw null; }
        public int IndexOfAny(char[] anyOf, int startIndex, int count) { throw null; }
        public System.String Insert(int startIndex, System.String value) { throw null; }
        public static bool IsNullOrEmpty(System.String value) { throw null; }
        public static bool IsNullOrWhiteSpace(System.String value) { throw null; }
        public static System.String Join(System.String separator, System.Collections.Generic.IEnumerable<string> values) { throw null; }
        public static System.String Join(System.String separator, params object[] values) { throw null; }
        public static System.String Join(System.String separator, params string[] value) { throw null; }
        public static System.String Join(System.String separator, string[] value, int startIndex, int count) { throw null; }
        public static System.String Join<T>(System.String separator, System.Collections.Generic.IEnumerable<T> values) { throw null; }
        public int LastIndexOf(char value) { throw null; }
        public int LastIndexOf(char value, int startIndex) { throw null; }
        public int LastIndexOf(char value, int startIndex, int count) { throw null; }
        public int LastIndexOf(System.String value) { throw null; }
        public int LastIndexOf(System.String value, int startIndex) { throw null; }
        public int LastIndexOf(System.String value, int startIndex, int count) { throw null; }
        public int LastIndexOf(System.String value, int startIndex, int count, System.StringComparison comparisonType) { throw null; }
        public int LastIndexOf(System.String value, int startIndex, System.StringComparison comparisonType) { throw null; }
        public int LastIndexOf(System.String value, System.StringComparison comparisonType) { throw null; }
        public int LastIndexOfAny(char[] anyOf) { throw null; }
        public int LastIndexOfAny(char[] anyOf, int startIndex) { throw null; }
        public int LastIndexOfAny(char[] anyOf, int startIndex, int count) { throw null; }
        public static bool operator ==(System.String a, System.String b) { throw null; }
        public static bool operator !=(System.String a, System.String b) { throw null; }
        public System.String PadLeft(int totalWidth) { throw null; }
        public System.String PadLeft(int totalWidth, char paddingChar) { throw null; }
        public System.String PadRight(int totalWidth) { throw null; }
        public System.String PadRight(int totalWidth, char paddingChar) { throw null; }
        public System.String Remove(int startIndex) { throw null; }
        public System.String Remove(int startIndex, int count) { throw null; }
        public System.String Replace(char oldChar, char newChar) { throw null; }
        public System.String Replace(System.String oldValue, System.String newValue) { throw null; }
        public string[] Split(params char[] separator) { throw null; }
        public string[] Split(char[] separator, int count) { throw null; }
        public string[] Split(char[] separator, int count, System.StringSplitOptions options) { throw null; }
        public string[] Split(char[] separator, System.StringSplitOptions options) { throw null; }
        public string[] Split(string[] separator, int count, System.StringSplitOptions options) { throw null; }
        public string[] Split(string[] separator, System.StringSplitOptions options) { throw null; }
        public bool StartsWith(System.String value) { throw null; }
        public bool StartsWith(System.String value, System.StringComparison comparisonType) { throw null; }
        public System.String Substring(int startIndex) { throw null; }
        public System.String Substring(int startIndex, int length) { throw null; }
        System.Collections.Generic.IEnumerator<char> System.Collections.Generic.IEnumerable<System.Char>.GetEnumerator() { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        System.String System.IConvertible.ToString(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public char[] ToCharArray() { throw null; }
        public char[] ToCharArray(int startIndex, int length) { throw null; }
        public System.String ToLower() { throw null; }
        public System.String ToLowerInvariant() { throw null; }
        public override System.String ToString() { throw null; }
        public System.String ToUpper() { throw null; }
        public System.String ToUpperInvariant() { throw null; }
        public System.String Trim() { throw null; }
        public System.String Trim(params char[] trimChars) { throw null; }
        public System.String TrimEnd(params char[] trimChars) { throw null; }
        public System.String TrimStart(params char[] trimChars) { throw null; }
    }
    public enum StringComparison
    {
        CurrentCulture = 0,
        CurrentCultureIgnoreCase = 1,
        Ordinal = 4,
        OrdinalIgnoreCase = 5,
    }
    [System.FlagsAttribute]
    public enum StringSplitOptions
    {
        None = 0,
        RemoveEmptyEntries = 1,
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field, Inherited=false)]
    public partial class ThreadStaticAttribute : System.Attribute
    {
        public ThreadStaticAttribute() { }
    }
    public partial class TimeoutException : System.Exception
    {
        public TimeoutException() { }
        public TimeoutException(string message) { }
        public TimeoutException(string message, System.Exception innerException) { }
    }
    public partial struct TimeSpan : System.IComparable, System.IComparable<System.TimeSpan>, System.IEquatable<System.TimeSpan>, System.IFormattable
    {
        public static readonly System.TimeSpan MaxValue;
        public static readonly System.TimeSpan MinValue;
        public const long TicksPerDay = (long)864000000000;
        public const long TicksPerHour = (long)36000000000;
        public const long TicksPerMillisecond = (long)10000;
        public const long TicksPerMinute = (long)600000000;
        public const long TicksPerSecond = (long)10000000;
        public static readonly System.TimeSpan Zero;
        public TimeSpan(int hours, int minutes, int seconds) { throw null; }
        public TimeSpan(int days, int hours, int minutes, int seconds) { throw null; }
        public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds) { throw null; }
        public TimeSpan(long ticks) { throw null; }
        public int Days { get { throw null; } }
        public int Hours { get { throw null; } }
        public int Milliseconds { get { throw null; } }
        public int Minutes { get { throw null; } }
        public int Seconds { get { throw null; } }
        public long Ticks { get { throw null; } }
        public double TotalDays { get { throw null; } }
        public double TotalHours { get { throw null; } }
        public double TotalMilliseconds { get { throw null; } }
        public double TotalMinutes { get { throw null; } }
        public double TotalSeconds { get { throw null; } }
        public System.TimeSpan Add(System.TimeSpan ts) { throw null; }
        public static int Compare(System.TimeSpan t1, System.TimeSpan t2) { throw null; }
        public int CompareTo(System.TimeSpan value) { throw null; }
        public System.TimeSpan Duration() { throw null; }
        public override bool Equals(object value) { throw null; }
        public bool Equals(System.TimeSpan obj) { throw null; }
        public static bool Equals(System.TimeSpan t1, System.TimeSpan t2) { throw null; }
        public static System.TimeSpan FromDays(double value) { throw null; }
        public static System.TimeSpan FromHours(double value) { throw null; }
        public static System.TimeSpan FromMilliseconds(double value) { throw null; }
        public static System.TimeSpan FromMinutes(double value) { throw null; }
        public static System.TimeSpan FromSeconds(double value) { throw null; }
        public static System.TimeSpan FromTicks(long value) { throw null; }
        public override int GetHashCode() { throw null; }
        public System.TimeSpan Negate() { throw null; }
        public static System.TimeSpan operator +(System.TimeSpan t1, System.TimeSpan t2) { throw null; }
        public static bool operator ==(System.TimeSpan t1, System.TimeSpan t2) { throw null; }
        public static bool operator >(System.TimeSpan t1, System.TimeSpan t2) { throw null; }
        public static bool operator >=(System.TimeSpan t1, System.TimeSpan t2) { throw null; }
        public static bool operator !=(System.TimeSpan t1, System.TimeSpan t2) { throw null; }
        public static bool operator <(System.TimeSpan t1, System.TimeSpan t2) { throw null; }
        public static bool operator <=(System.TimeSpan t1, System.TimeSpan t2) { throw null; }
        public static System.TimeSpan operator -(System.TimeSpan t1, System.TimeSpan t2) { throw null; }
        public static System.TimeSpan operator -(System.TimeSpan t) { throw null; }
        public static System.TimeSpan operator +(System.TimeSpan t) { throw null; }
        public static System.TimeSpan Parse(string s) { throw null; }
        public static System.TimeSpan Parse(string input, System.IFormatProvider formatProvider) { throw null; }
        public static System.TimeSpan ParseExact(string input, string format, System.IFormatProvider formatProvider) { throw null; }
        public static System.TimeSpan ParseExact(string input, string format, System.IFormatProvider formatProvider, System.Globalization.TimeSpanStyles styles) { throw null; }
        public static System.TimeSpan ParseExact(string input, string[] formats, System.IFormatProvider formatProvider) { throw null; }
        public static System.TimeSpan ParseExact(string input, string[] formats, System.IFormatProvider formatProvider, System.Globalization.TimeSpanStyles styles) { throw null; }
        public System.TimeSpan Subtract(System.TimeSpan ts) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider formatProvider) { throw null; }
        public static bool TryParse(string input, System.IFormatProvider formatProvider, out System.TimeSpan result) { throw null; }
        public static bool TryParse(string s, out System.TimeSpan result) { throw null; }
        public static bool TryParseExact(string input, string format, System.IFormatProvider formatProvider, System.Globalization.TimeSpanStyles styles, out System.TimeSpan result) { throw null; }
        public static bool TryParseExact(string input, string format, System.IFormatProvider formatProvider, out System.TimeSpan result) { throw null; }
        public static bool TryParseExact(string input, string[] formats, System.IFormatProvider formatProvider, System.Globalization.TimeSpanStyles styles, out System.TimeSpan result) { throw null; }
        public static bool TryParseExact(string input, string[] formats, System.IFormatProvider formatProvider, out System.TimeSpan result) { throw null; }
    }
    public sealed partial class TimeZoneInfo : System.IEquatable<System.TimeZoneInfo>
    {
        internal TimeZoneInfo() { }
        public System.TimeSpan BaseUtcOffset { get { throw null; } }
        public string DaylightName { get { throw null; } }
        public string DisplayName { get { throw null; } }
        public string Id { get { throw null; } }
        public static System.TimeZoneInfo Local { get { throw null; } }
        public string StandardName { get { throw null; } }
        public bool SupportsDaylightSavingTime { get { throw null; } }
        public static System.TimeZoneInfo Utc { get { throw null; } }
        public static System.DateTime ConvertTime(System.DateTime dateTime, System.TimeZoneInfo destinationTimeZone) { throw null; }
        public static System.DateTime ConvertTime(System.DateTime dateTime, System.TimeZoneInfo sourceTimeZone, System.TimeZoneInfo destinationTimeZone) { throw null; }
        public static System.DateTimeOffset ConvertTime(System.DateTimeOffset dateTimeOffset, System.TimeZoneInfo destinationTimeZone) { throw null; }
        public bool Equals(System.TimeZoneInfo other) { throw null; }
        public static System.TimeZoneInfo FindSystemTimeZoneById(string id) { throw null; }
        public System.TimeSpan[] GetAmbiguousTimeOffsets(System.DateTime dateTime) { throw null; }
        public System.TimeSpan[] GetAmbiguousTimeOffsets(System.DateTimeOffset dateTimeOffset) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Collections.ObjectModel.ReadOnlyCollection<System.TimeZoneInfo> GetSystemTimeZones() { throw null; }
        public System.TimeSpan GetUtcOffset(System.DateTime dateTime) { throw null; }
        public System.TimeSpan GetUtcOffset(System.DateTimeOffset dateTimeOffset) { throw null; }
        public bool IsAmbiguousTime(System.DateTime dateTime) { throw null; }
        public bool IsAmbiguousTime(System.DateTimeOffset dateTimeOffset) { throw null; }
        public bool IsDaylightSavingTime(System.DateTime dateTime) { throw null; }
        public bool IsDaylightSavingTime(System.DateTimeOffset dateTimeOffset) { throw null; }
        public bool IsInvalidTime(System.DateTime dateTime) { throw null; }
        public override string ToString() { throw null; }
    }
    public static partial class Tuple
    {
        public static System.Tuple<T1> Create<T1>(T1 item1) { throw null; }
        public static System.Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) { throw null; }
        public static System.Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) { throw null; }
        public static System.Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) { throw null; }
        public static System.Tuple<T1, T2, T3, T4, T5, T6, T7, System.Tuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) { throw null; }
    }
    public partial class Tuple<T1> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable
    {
        public Tuple(T1 item1) { }
        public T1 Item1 { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class Tuple<T1, T2> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable
    {
        public Tuple(T1 item1, T2 item2) { }
        public T1 Item1 { get { throw null; } }
        public T2 Item2 { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class Tuple<T1, T2, T3> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3) { }
        public T1 Item1 { get { throw null; } }
        public T2 Item2 { get { throw null; } }
        public T3 Item3 { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class Tuple<T1, T2, T3, T4> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4) { }
        public T1 Item1 { get { throw null; } }
        public T2 Item2 { get { throw null; } }
        public T3 Item3 { get { throw null; } }
        public T4 Item4 { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class Tuple<T1, T2, T3, T4, T5> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) { }
        public T1 Item1 { get { throw null; } }
        public T2 Item2 { get { throw null; } }
        public T3 Item3 { get { throw null; } }
        public T4 Item4 { get { throw null; } }
        public T5 Item5 { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class Tuple<T1, T2, T3, T4, T5, T6> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) { }
        public T1 Item1 { get { throw null; } }
        public T2 Item2 { get { throw null; } }
        public T3 Item3 { get { throw null; } }
        public T4 Item4 { get { throw null; } }
        public T5 Item5 { get { throw null; } }
        public T6 Item6 { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class Tuple<T1, T2, T3, T4, T5, T6, T7> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable
    {
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) { }
        public T1 Item1 { get { throw null; } }
        public T2 Item2 { get { throw null; } }
        public T3 Item3 { get { throw null; } }
        public T4 Item4 { get { throw null; } }
        public T5 Item5 { get { throw null; } }
        public T6 Item6 { get { throw null; } }
        public T7 Item7 { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> : System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.IComparable
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
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        int System.Collections.IStructuralComparable.CompareTo(object other, System.Collections.IComparer comparer) { throw null; }
        bool System.Collections.IStructuralEquatable.Equals(object other, System.Collections.IEqualityComparer comparer) { throw null; }
        int System.Collections.IStructuralEquatable.GetHashCode(System.Collections.IEqualityComparer comparer) { throw null; }
        int System.IComparable.CompareTo(object obj) { throw null; }
        public override string ToString() { throw null; }
    }
    public abstract partial class Type
    {
        internal Type() { }
        public static readonly char Delimiter;
        public static readonly System.Type[] EmptyTypes;
        public static readonly object Missing;
        public abstract string AssemblyQualifiedName { get; }
        public abstract System.Type DeclaringType { get; }
        public abstract string FullName { get; }
        public abstract int GenericParameterPosition { get; }
        public abstract System.Type[] GenericTypeArguments { get; }
        public bool HasElementType { get { throw null; } }
        public virtual bool IsArray { get { throw null; } }
        public virtual bool IsByRef { get { throw null; } }
        public abstract bool IsConstructedGenericType { get; }
        public abstract bool IsGenericParameter { get; }
        public bool IsNested { get { throw null; } }
        public virtual bool IsPointer { get { throw null; } }
        public abstract string Name { get; }
        public abstract string Namespace { get; }
        public virtual System.RuntimeTypeHandle TypeHandle { get { throw null; } }
        public override bool Equals(object o) { throw null; }
        public bool Equals(System.Type o) { throw null; }
        public abstract int GetArrayRank();
        public abstract System.Type GetElementType();
        public abstract System.Type GetGenericTypeDefinition();
        public override int GetHashCode() { throw null; }
        public static System.Type GetType(string typeName) { throw null; }
        public static System.Type GetType(string typeName, bool throwOnError) { throw null; }
        public static System.Type GetType(string typeName, bool throwOnError, bool ignoreCase) { throw null; }
        public static System.TypeCode GetTypeCode(System.Type type) { throw null; }
        public static System.Type GetTypeFromHandle(System.RuntimeTypeHandle handle) { throw null; }
        public abstract System.Type MakeArrayType();
        public abstract System.Type MakeArrayType(int rank);
        public abstract System.Type MakeByRefType();
        public abstract System.Type MakeGenericType(params System.Type[] typeArguments);
        public abstract System.Type MakePointerType();
        public override string ToString() { throw null; }
    }
    public partial class TypeAccessException : System.TypeLoadException
    {
        public TypeAccessException() { }
        public TypeAccessException(string message) { }
        public TypeAccessException(string message, System.Exception inner) { }
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
        String = 18,
    }
    public sealed partial class TypeInitializationException : System.Exception
    {
        public TypeInitializationException(string fullTypeName, System.Exception innerException) { }
        public string TypeName { get { throw null; } }
    }
    public partial class TypeLoadException : System.Exception
    {
        public TypeLoadException() { }
        public TypeLoadException(string message) { }
        public TypeLoadException(string message, System.Exception inner) { }
        public override string Message { get { throw null; } }
        public string TypeName { get { throw null; } }
    }
    [System.CLSCompliantAttribute(false)]
    public partial struct UInt16 : System.IComparable, System.IComparable<ushort>, System.IConvertible, System.IEquatable<ushort>, System.IFormattable
    {
        public const ushort MaxValue = (ushort)65535;
        public const ushort MinValue = (ushort)0;
        public int CompareTo(System.UInt16 value) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.UInt16 obj) { throw null; }
        public override int GetHashCode() { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt16 Parse(string s) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt16 Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt16 Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt16 Parse(string s, System.IFormatProvider provider) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        System.UInt16 System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.UInt16 result) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(string s, out System.UInt16 result) { throw null; }
    }
    [System.CLSCompliantAttribute(false)]
    public partial struct UInt32 : System.IComparable, System.IComparable<uint>, System.IConvertible, System.IEquatable<uint>, System.IFormattable
    {
        public const uint MaxValue = (uint)4294967295;
        public const uint MinValue = (uint)0;
        public int CompareTo(System.UInt32 value) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.UInt32 obj) { throw null; }
        public override int GetHashCode() { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt32 Parse(string s) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt32 Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt32 Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt32 Parse(string s, System.IFormatProvider provider) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        System.UInt32 System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        ulong System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.UInt32 result) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(string s, out System.UInt32 result) { throw null; }
    }
    [System.CLSCompliantAttribute(false)]
    public partial struct UInt64 : System.IComparable, System.IComparable<ulong>, System.IConvertible, System.IEquatable<ulong>, System.IFormattable
    {
        public const ulong MaxValue = (ulong)18446744073709551615;
        public const ulong MinValue = (ulong)0;
        public int CompareTo(System.UInt64 value) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.UInt64 obj) { throw null; }
        public override int GetHashCode() { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt64 Parse(string s) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt64 Parse(string s, System.Globalization.NumberStyles style) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt64 Parse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.UInt64 Parse(string s, System.IFormatProvider provider) { throw null; }
        int System.IComparable.CompareTo(object value) { throw null; }
        System.TypeCode System.IConvertible.GetTypeCode() { throw null; }
        bool System.IConvertible.ToBoolean(System.IFormatProvider provider) { throw null; }
        byte System.IConvertible.ToByte(System.IFormatProvider provider) { throw null; }
        char System.IConvertible.ToChar(System.IFormatProvider provider) { throw null; }
        System.DateTime System.IConvertible.ToDateTime(System.IFormatProvider provider) { throw null; }
        decimal System.IConvertible.ToDecimal(System.IFormatProvider provider) { throw null; }
        double System.IConvertible.ToDouble(System.IFormatProvider provider) { throw null; }
        short System.IConvertible.ToInt16(System.IFormatProvider provider) { throw null; }
        int System.IConvertible.ToInt32(System.IFormatProvider provider) { throw null; }
        long System.IConvertible.ToInt64(System.IFormatProvider provider) { throw null; }
        sbyte System.IConvertible.ToSByte(System.IFormatProvider provider) { throw null; }
        float System.IConvertible.ToSingle(System.IFormatProvider provider) { throw null; }
        object System.IConvertible.ToType(System.Type type, System.IFormatProvider provider) { throw null; }
        ushort System.IConvertible.ToUInt16(System.IFormatProvider provider) { throw null; }
        uint System.IConvertible.ToUInt32(System.IFormatProvider provider) { throw null; }
        System.UInt64 System.IConvertible.ToUInt64(System.IFormatProvider provider) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(System.IFormatProvider provider) { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider provider) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(string s, System.Globalization.NumberStyles style, System.IFormatProvider provider, out System.UInt64 result) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool TryParse(string s, out System.UInt64 result) { throw null; }
    }
    [System.CLSCompliantAttribute(false)]
    public partial struct UIntPtr
    {
        public static readonly System.UIntPtr Zero;
        public UIntPtr(uint value) { throw null; }
        public UIntPtr(ulong value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe UIntPtr(void* value) { throw null; }
        public static int Size { get { throw null; } }
        public static System.UIntPtr Add(System.UIntPtr pointer, int offset) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.UIntPtr operator +(System.UIntPtr pointer, int offset) { throw null; }
        public static bool operator ==(System.UIntPtr value1, System.UIntPtr value2) { throw null; }
        public static explicit operator System.UIntPtr (uint value) { throw null; }
        public static explicit operator System.UIntPtr (ulong value) { throw null; }
        public static explicit operator uint (System.UIntPtr value) { throw null; }
        public static explicit operator ulong (System.UIntPtr value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe static explicit operator void* (System.UIntPtr value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe static explicit operator System.UIntPtr (void* value) { throw null; }
        public static bool operator !=(System.UIntPtr value1, System.UIntPtr value2) { throw null; }
        public static System.UIntPtr operator -(System.UIntPtr pointer, int offset) { throw null; }
        public static System.UIntPtr Subtract(System.UIntPtr pointer, int offset) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe void* ToPointer() { throw null; }
        public override string ToString() { throw null; }
        public uint ToUInt32() { throw null; }
        public ulong ToUInt64() { throw null; }
    }
    public partial class UnauthorizedAccessException : System.Exception
    {
        public UnauthorizedAccessException() { }
        public UnauthorizedAccessException(string message) { }
        public UnauthorizedAccessException(string message, System.Exception inner) { }
    }
    public partial class Uri
    {
        public Uri(string uriString) { }
        public Uri(string uriString, System.UriKind uriKind) { }
        public Uri(System.Uri baseUri, string relativeUri) { }
        public Uri(System.Uri baseUri, System.Uri relativeUri) { }
        public string AbsolutePath { get { throw null; } }
        public string AbsoluteUri { get { throw null; } }
        public string Authority { get { throw null; } }
        public string DnsSafeHost { get { throw null; } }
        public string Fragment { get { throw null; } }
        public string Host { get { throw null; } }
        public System.UriHostNameType HostNameType { get { throw null; } }
        public string IdnHost { get { throw null; } }
        public bool IsAbsoluteUri { get { throw null; } }
        public bool IsDefaultPort { get { throw null; } }
        public bool IsFile { get { throw null; } }
        public bool IsLoopback { get { throw null; } }
        public bool IsUnc { get { throw null; } }
        public string LocalPath { get { throw null; } }
        public string OriginalString { get { throw null; } }
        public string PathAndQuery { get { throw null; } }
        public int Port { get { throw null; } }
        public string Query { get { throw null; } }
        public string Scheme { get { throw null; } }
        public string[] Segments { get { throw null; } }
        public bool UserEscaped { get { throw null; } }
        public string UserInfo { get { throw null; } }
        public static System.UriHostNameType CheckHostName(string name) { throw null; }
        public static bool CheckSchemeName(string schemeName) { throw null; }
        public static int Compare(System.Uri uri1, System.Uri uri2, System.UriComponents partsToCompare, System.UriFormat compareFormat, System.StringComparison comparisonType) { throw null; }
        public override bool Equals(object comparand) { throw null; }
        public static string EscapeDataString(string stringToEscape) { throw null; }
        public static string EscapeUriString(string stringToEscape) { throw null; }
        public string GetComponents(System.UriComponents components, System.UriFormat format) { throw null; }
        public override int GetHashCode() { throw null; }
        public bool IsBaseOf(System.Uri uri) { throw null; }
        public bool IsWellFormedOriginalString() { throw null; }
        public static bool IsWellFormedUriString(string uriString, System.UriKind uriKind) { throw null; }
        public System.Uri MakeRelativeUri(System.Uri uri) { throw null; }
        public static bool operator ==(System.Uri uri1, System.Uri uri2) { throw null; }
        public static bool operator !=(System.Uri uri1, System.Uri uri2) { throw null; }
        public override string ToString() { throw null; }
        public static bool TryCreate(string uriString, System.UriKind uriKind, out System.Uri result) { throw null; }
        public static bool TryCreate(System.Uri baseUri, string relativeUri, out System.Uri result) { throw null; }
        public static bool TryCreate(System.Uri baseUri, System.Uri relativeUri, out System.Uri result) { throw null; }
        public static string UnescapeDataString(string stringToUnescape) { throw null; }
    }
    [System.FlagsAttribute]
    public enum UriComponents
    {
        SerializationInfoString = -2147483648,
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
        KeepDelimiter = 1073741824,
    }
    public enum UriFormat
    {
        UriEscaped = 1,
        Unescaped = 2,
        SafeUnescaped = 3,
    }
    public partial class UriFormatException : System.FormatException
    {
        public UriFormatException() { }
        public UriFormatException(string textString) { }
        public UriFormatException(string textString, System.Exception e) { }
    }
    public enum UriHostNameType
    {
        Unknown = 0,
        Basic = 1,
        Dns = 2,
        IPv4 = 3,
        IPv6 = 4,
    }
    public enum UriKind
    {
        RelativeOrAbsolute = 0,
        Absolute = 1,
        Relative = 2,
    }
    public abstract partial class ValueType
    {
        protected ValueType() { }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public override string ToString() { throw null; }
    }
    public sealed partial class Version : System.IComparable, System.IComparable<System.Version>, System.IEquatable<System.Version>
    {
        public Version(int major, int minor) { }
        public Version(int major, int minor, int build) { }
        public Version(int major, int minor, int build, int revision) { }
        public Version(string version) { }
        public int Build { get { throw null; } }
        public int Major { get { throw null; } }
        public short MajorRevision { get { throw null; } }
        public int Minor { get { throw null; } }
        public short MinorRevision { get { throw null; } }
        public int Revision { get { throw null; } }
        public int CompareTo(System.Version value) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Version obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Version v1, System.Version v2) { throw null; }
        public static bool operator >(System.Version v1, System.Version v2) { throw null; }
        public static bool operator >=(System.Version v1, System.Version v2) { throw null; }
        public static bool operator !=(System.Version v1, System.Version v2) { throw null; }
        public static bool operator <(System.Version v1, System.Version v2) { throw null; }
        public static bool operator <=(System.Version v1, System.Version v2) { throw null; }
        public static System.Version Parse(string input) { throw null; }
        int System.IComparable.CompareTo(object version) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(int fieldCount) { throw null; }
        public static bool TryParse(string input, out System.Version result) { throw null; }
    }
    public partial struct Void
    {
    }
    public partial class WeakReference
    {
        public WeakReference(object target) { }
        public WeakReference(object target, bool trackResurrection) { }
        public virtual bool IsAlive { get { throw null; } }
        public virtual object Target { get { throw null; } set { } }
        public virtual bool TrackResurrection { get { throw null; } }
        ~WeakReference() { }
    }
    public sealed partial class WeakReference<T> where T : class
    {
        public WeakReference(T target) { }
        public WeakReference(T target, bool trackResurrection) { }
        ~WeakReference() { }
        public void SetTarget(T target) { }
        public bool TryGetTarget(out T target) { throw null; }
    }
}
namespace System.Collections
{
    public partial struct DictionaryEntry
    {
        public DictionaryEntry(object key, object value) { throw null; }
        public object Key { get { throw null; } set { } }
        public object Value { get { throw null; } set { } }
    }
    public partial interface ICollection : System.Collections.IEnumerable
    {
        int Count { get; }
        bool IsSynchronized { get; }
        object SyncRoot { get; }
        void CopyTo(System.Array array, int index);
    }
    public partial interface IComparer
    {
        int Compare(object x, object y);
    }
    public partial interface IDictionary : System.Collections.ICollection, System.Collections.IEnumerable
    {
        bool IsFixedSize { get; }
        bool IsReadOnly { get; }
        object this[object key] { get; set; }
        System.Collections.ICollection Keys { get; }
        System.Collections.ICollection Values { get; }
        void Add(object key, object value);
        void Clear();
        bool Contains(object key);
        new System.Collections.IDictionaryEnumerator GetEnumerator();
        void Remove(object key);
    }
    public partial interface IDictionaryEnumerator : System.Collections.IEnumerator
    {
        System.Collections.DictionaryEntry Entry { get; }
        object Key { get; }
        object Value { get; }
    }
    public partial interface IEnumerable
    {
        System.Collections.IEnumerator GetEnumerator();
    }
    public partial interface IEnumerator
    {
        object Current { get; }
        bool MoveNext();
        void Reset();
    }
    public partial interface IEqualityComparer
    {
        bool Equals(object x, object y);
        int GetHashCode(object obj);
    }
    public partial interface IList : System.Collections.ICollection, System.Collections.IEnumerable
    {
        bool IsFixedSize { get; }
        bool IsReadOnly { get; }
        object this[int index] { get; set; }
        int Add(object value);
        void Clear();
        bool Contains(object value);
        int IndexOf(object value);
        void Insert(int index, object value);
        void Remove(object value);
        void RemoveAt(int index);
    }
    public partial interface IStructuralComparable
    {
        int CompareTo(object other, System.Collections.IComparer comparer);
    }
    public partial interface IStructuralEquatable
    {
        bool Equals(object other, System.Collections.IEqualityComparer comparer);
        int GetHashCode(System.Collections.IEqualityComparer comparer);
    }
}
namespace System.Collections.Generic
{
    public partial interface ICollection<T> : System.Collections.Generic.IEnumerable<T>, System.Collections.IEnumerable
    {
        int Count { get; }
        bool IsReadOnly { get; }
        void Add(T item);
        void Clear();
        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
        bool Remove(T item);
    }
    public partial interface IComparer<in T>
    {
        int Compare(T x, T y);
    }
    public partial interface IDictionary<TKey, TValue> : System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey, TValue>>, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey, TValue>>, System.Collections.IEnumerable
    {
        TValue this[TKey key] { get; set; }
        System.Collections.Generic.ICollection<TKey> Keys { get; }
        System.Collections.Generic.ICollection<TValue> Values { get; }
        void Add(TKey key, TValue value);
        bool ContainsKey(TKey key);
        bool Remove(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }
    public partial interface IEnumerable<out T> : System.Collections.IEnumerable
    {
        new System.Collections.Generic.IEnumerator<T> GetEnumerator();
    }
    public partial interface IEnumerator<out T> : System.Collections.IEnumerator, System.IDisposable
    {
        new T Current { get; }
    }
    public partial interface IEqualityComparer<in T>
    {
        bool Equals(T x, T y);
        int GetHashCode(T obj);
    }
    public partial interface IList<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, System.Collections.IEnumerable
    {
        T this[int index] { get; set; }
        int IndexOf(T item);
        void Insert(int index, T item);
        void RemoveAt(int index);
    }
    public partial interface IReadOnlyCollection<out T> : System.Collections.Generic.IEnumerable<T>, System.Collections.IEnumerable
    {
        int Count { get; }
    }
    public partial interface IReadOnlyDictionary<TKey, TValue> : System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey, TValue>>, System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<TKey, TValue>>, System.Collections.IEnumerable
    {
        TValue this[TKey key] { get; }
        System.Collections.Generic.IEnumerable<TKey> Keys { get; }
        System.Collections.Generic.IEnumerable<TValue> Values { get; }
        bool ContainsKey(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }
    public partial interface IReadOnlyList<out T> : System.Collections.Generic.IEnumerable<T>, System.Collections.Generic.IReadOnlyCollection<T>, System.Collections.IEnumerable
    {
        T this[int index] { get; }
    }
    public partial interface ISet<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, System.Collections.IEnumerable
    {
        new bool Add(T item);
        void ExceptWith(System.Collections.Generic.IEnumerable<T> other);
        void IntersectWith(System.Collections.Generic.IEnumerable<T> other);
        bool IsProperSubsetOf(System.Collections.Generic.IEnumerable<T> other);
        bool IsProperSupersetOf(System.Collections.Generic.IEnumerable<T> other);
        bool IsSubsetOf(System.Collections.Generic.IEnumerable<T> other);
        bool IsSupersetOf(System.Collections.Generic.IEnumerable<T> other);
        bool Overlaps(System.Collections.Generic.IEnumerable<T> other);
        bool SetEquals(System.Collections.Generic.IEnumerable<T> other);
        void SymmetricExceptWith(System.Collections.Generic.IEnumerable<T> other);
        void UnionWith(System.Collections.Generic.IEnumerable<T> other);
    }
    public partial class KeyNotFoundException : System.Exception
    {
        public KeyNotFoundException() { }
        public KeyNotFoundException(string message) { }
        public KeyNotFoundException(string message, System.Exception innerException) { }
    }
    public partial struct KeyValuePair<TKey, TValue>
    {
        public KeyValuePair(TKey key, TValue value) { throw null; }
        public TKey Key { get { throw null; } }
        public TValue Value { get { throw null; } }
        public override string ToString() { throw null; }
    }
}
namespace System.Collections.ObjectModel
{
    public partial class Collection<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, System.Collections.Generic.IList<T>, System.Collections.Generic.IReadOnlyCollection<T>, System.Collections.Generic.IReadOnlyList<T>, System.Collections.ICollection, System.Collections.IEnumerable, System.Collections.IList
    {
        public Collection() { }
        public Collection(System.Collections.Generic.IList<T> list) { }
        public int Count { get { throw null; } }
        public T this[int index] { get { throw null; } set { } }
        protected System.Collections.Generic.IList<T> Items { get { throw null; } }
        bool System.Collections.Generic.ICollection<T>.IsReadOnly { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        bool System.Collections.IList.IsFixedSize { get { throw null; } }
        bool System.Collections.IList.IsReadOnly { get { throw null; } }
        object System.Collections.IList.this[int index] { get { throw null; } set { } }
        public void Add(T item) { }
        public void Clear() { }
        protected virtual void ClearItems() { }
        public bool Contains(T item) { throw null; }
        public void CopyTo(T[] array, int index) { }
        public System.Collections.Generic.IEnumerator<T> GetEnumerator() { throw null; }
        public int IndexOf(T item) { throw null; }
        public void Insert(int index, T item) { }
        protected virtual void InsertItem(int index, T item) { }
        public bool Remove(T item) { throw null; }
        public void RemoveAt(int index) { }
        protected virtual void RemoveItem(int index) { }
        protected virtual void SetItem(int index, T item) { }
        void System.Collections.ICollection.CopyTo(System.Array array, int index) { }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        int System.Collections.IList.Add(object value) { throw null; }
        bool System.Collections.IList.Contains(object value) { throw null; }
        int System.Collections.IList.IndexOf(object value) { throw null; }
        void System.Collections.IList.Insert(int index, object value) { }
        void System.Collections.IList.Remove(object value) { }
    }
    public partial class ReadOnlyCollection<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, System.Collections.Generic.IList<T>, System.Collections.Generic.IReadOnlyCollection<T>, System.Collections.Generic.IReadOnlyList<T>, System.Collections.ICollection, System.Collections.IEnumerable, System.Collections.IList
    {
        public ReadOnlyCollection(System.Collections.Generic.IList<T> list) { }
        public int Count { get { throw null; } }
        public T this[int index] { get { throw null; } }
        protected System.Collections.Generic.IList<T> Items { get { throw null; } }
        bool System.Collections.Generic.ICollection<T>.IsReadOnly { get { throw null; } }
        T System.Collections.Generic.IList<T>.this[int index] { get { throw null; } set { } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        bool System.Collections.IList.IsFixedSize { get { throw null; } }
        bool System.Collections.IList.IsReadOnly { get { throw null; } }
        object System.Collections.IList.this[int index] { get { throw null; } set { } }
        public bool Contains(T value) { throw null; }
        public void CopyTo(T[] array, int index) { }
        public System.Collections.Generic.IEnumerator<T> GetEnumerator() { throw null; }
        public int IndexOf(T value) { throw null; }
        void System.Collections.Generic.ICollection<T>.Add(T value) { }
        void System.Collections.Generic.ICollection<T>.Clear() { }
        bool System.Collections.Generic.ICollection<T>.Remove(T value) { throw null; }
        void System.Collections.Generic.IList<T>.Insert(int index, T value) { }
        void System.Collections.Generic.IList<T>.RemoveAt(int index) { }
        void System.Collections.ICollection.CopyTo(System.Array array, int index) { }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        int System.Collections.IList.Add(object value) { throw null; }
        void System.Collections.IList.Clear() { }
        bool System.Collections.IList.Contains(object value) { throw null; }
        int System.Collections.IList.IndexOf(object value) { throw null; }
        void System.Collections.IList.Insert(int index, object value) { }
        void System.Collections.IList.Remove(object value) { }
        void System.Collections.IList.RemoveAt(int index) { }
    }
}
namespace System.ComponentModel
{
    [System.AttributeUsageAttribute(System.AttributeTargets.All)]
    public partial class DefaultValueAttribute : System.Attribute
    {
        public DefaultValueAttribute(bool value) { }
        public DefaultValueAttribute(byte value) { }
        public DefaultValueAttribute(char value) { }
        public DefaultValueAttribute(double value) { }
        public DefaultValueAttribute(short value) { }
        public DefaultValueAttribute(int value) { }
        public DefaultValueAttribute(long value) { }
        public DefaultValueAttribute(object value) { }
        public DefaultValueAttribute(float value) { }
        public DefaultValueAttribute(string value) { }
        public DefaultValueAttribute(System.Type type, string value) { }
        public virtual object Value { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Constructor | System.AttributeTargets.Delegate | System.AttributeTargets.Enum | System.AttributeTargets.Event | System.AttributeTargets.Field | System.AttributeTargets.Interface | System.AttributeTargets.Method | System.AttributeTargets.Property | System.AttributeTargets.Struct)]
    public sealed partial class EditorBrowsableAttribute : System.Attribute
    {
        public EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState state) { }
        public System.ComponentModel.EditorBrowsableState State { get { throw null; } }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
    }
    public enum EditorBrowsableState
    {
        Always = 0,
        Never = 1,
        Advanced = 2,
    }
}
namespace System.Diagnostics
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Method, AllowMultiple=true)]
    public sealed partial class ConditionalAttribute : System.Attribute
    {
        public ConditionalAttribute(string conditionString) { }
        public string ConditionString { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.Module, AllowMultiple=false)]
    public sealed partial class DebuggableAttribute : System.Attribute
    {
        public DebuggableAttribute(System.Diagnostics.DebuggableAttribute.DebuggingModes modes) { }
        [System.FlagsAttribute]
        public enum DebuggingModes
        {
            None = 0,
            Default = 1,
            IgnoreSymbolStoreSequencePoints = 2,
            EnableEditAndContinue = 4,
            DisableOptimizations = 256,
        }
    }
}
namespace System.Globalization
{
    [System.FlagsAttribute]
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
        RoundtripKind = 128,
    }
    [System.FlagsAttribute]
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
        HexNumber = 515,
    }
    [System.FlagsAttribute]
    public enum TimeSpanStyles
    {
        None = 0,
        AssumeNegative = 1,
    }
}
namespace System.IO
{
    public partial class DirectoryNotFoundException : System.IO.IOException
    {
        public DirectoryNotFoundException() { }
        public DirectoryNotFoundException(string message) { }
        public DirectoryNotFoundException(string message, System.Exception innerException) { }
    }
    public partial class FileLoadException : System.IO.IOException
    {
        public FileLoadException() { }
        public FileLoadException(string message) { }
        public FileLoadException(string message, System.Exception inner) { }
        public FileLoadException(string message, string fileName) { }
        public FileLoadException(string message, string fileName, System.Exception inner) { }
        public string FileName { get { throw null; } }
        public override string Message { get { throw null; } }
        public override string ToString() { throw null; }
    }
    public partial class FileNotFoundException : System.IO.IOException
    {
        public FileNotFoundException() { }
        public FileNotFoundException(string message) { }
        public FileNotFoundException(string message, System.Exception innerException) { }
        public FileNotFoundException(string message, string fileName) { }
        public FileNotFoundException(string message, string fileName, System.Exception innerException) { }
        public string FileName { get { throw null; } }
        public override string Message { get { throw null; } }
        public override string ToString() { throw null; }
    }
    public partial class IOException : System.Exception
    {
        public IOException() { }
        public IOException(string message) { }
        public IOException(string message, System.Exception innerException) { }
        public IOException(string message, int hresult) { }
    }
    public partial class PathTooLongException : System.IO.IOException
    {
        public PathTooLongException() { }
        public PathTooLongException(string message) { }
        public PathTooLongException(string message, System.Exception innerException) { }
    }
}
namespace System.Reflection
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyCompanyAttribute : System.Attribute
    {
        public AssemblyCompanyAttribute(string company) { }
        public string Company { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyConfigurationAttribute : System.Attribute
    {
        public AssemblyConfigurationAttribute(string configuration) { }
        public string Configuration { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyCopyrightAttribute : System.Attribute
    {
        public AssemblyCopyrightAttribute(string copyright) { }
        public string Copyright { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyCultureAttribute : System.Attribute
    {
        public AssemblyCultureAttribute(string culture) { }
        public string Culture { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyDefaultAliasAttribute : System.Attribute
    {
        public AssemblyDefaultAliasAttribute(string defaultAlias) { }
        public string DefaultAlias { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyDelaySignAttribute : System.Attribute
    {
        public AssemblyDelaySignAttribute(bool delaySign) { }
        public bool DelaySign { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyDescriptionAttribute : System.Attribute
    {
        public AssemblyDescriptionAttribute(string description) { }
        public string Description { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyFileVersionAttribute : System.Attribute
    {
        public AssemblyFileVersionAttribute(string version) { }
        public string Version { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyFlagsAttribute : System.Attribute
    {
        public AssemblyFlagsAttribute(System.Reflection.AssemblyNameFlags assemblyFlags) { }
        public int AssemblyFlags { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyInformationalVersionAttribute : System.Attribute
    {
        public AssemblyInformationalVersionAttribute(string informationalVersion) { }
        public string InformationalVersion { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyKeyFileAttribute : System.Attribute
    {
        public AssemblyKeyFileAttribute(string keyFile) { }
        public string KeyFile { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyKeyNameAttribute : System.Attribute
    {
        public AssemblyKeyNameAttribute(string keyName) { }
        public string KeyName { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, AllowMultiple=true, Inherited=false)]
    public sealed partial class AssemblyMetadataAttribute : System.Attribute
    {
        public AssemblyMetadataAttribute(string key, string value) { }
        public string Key { get { throw null; } }
        public string Value { get { throw null; } }
    }
    [System.FlagsAttribute]
    public enum AssemblyNameFlags
    {
        None = 0,
        PublicKey = 1,
        Retargetable = 256,
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyProductAttribute : System.Attribute
    {
        public AssemblyProductAttribute(string product) { }
        public string Product { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false, AllowMultiple=false)]
    public sealed partial class AssemblySignatureKeyAttribute : System.Attribute
    {
        public AssemblySignatureKeyAttribute(string publicKey, string countersignature) { }
        public string Countersignature { get { throw null; } }
        public string PublicKey { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyTitleAttribute : System.Attribute
    {
        public AssemblyTitleAttribute(string title) { }
        public string Title { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyTrademarkAttribute : System.Attribute
    {
        public AssemblyTrademarkAttribute(string trademark) { }
        public string Trademark { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false)]
    public sealed partial class AssemblyVersionAttribute : System.Attribute
    {
        public AssemblyVersionAttribute(string version) { }
        public string Version { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Interface | System.AttributeTargets.Struct)]
    public sealed partial class DefaultMemberAttribute : System.Attribute
    {
        public DefaultMemberAttribute(string memberName) { }
        public string MemberName { get { throw null; } }
    }
    public enum ProcessorArchitecture
    {
        None = 0,
        MSIL = 1,
        X86 = 2,
        IA64 = 3,
        Amd64 = 4,
        Arm = 5,
    }
}
namespace System.Runtime
{
    public enum GCLargeObjectHeapCompactionMode
    {
        Default = 1,
        CompactOnce = 2,
    }
    public enum GCLatencyMode
    {
        Batch = 0,
        Interactive = 1,
        LowLatency = 2,
        SustainedLowLatency = 3,
    }
    public static partial class GCSettings
    {
        public static bool IsServerGC { get { throw null; } }
        public static System.Runtime.GCLargeObjectHeapCompactionMode LargeObjectHeapCompactionMode { get { throw null; } set { } }
        public static System.Runtime.GCLatencyMode LatencyMode { get { throw null; } set { } }
    }
}
namespace System.Runtime.CompilerServices
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Field)]
    public sealed partial class AccessedThroughPropertyAttribute : System.Attribute
    {
        public AccessedThroughPropertyAttribute(string propertyName) { }
        public string PropertyName { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method, Inherited=false, AllowMultiple=false)]
    public sealed partial class AsyncStateMachineAttribute : System.Runtime.CompilerServices.StateMachineAttribute
    {
        public AsyncStateMachineAttribute(System.Type stateMachineType) : base (default(System.Type)) { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter, Inherited=false)]
    public sealed partial class CallerFilePathAttribute : System.Attribute
    {
        public CallerFilePathAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter, Inherited=false)]
    public sealed partial class CallerLineNumberAttribute : System.Attribute
    {
        public CallerLineNumberAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter, Inherited=false)]
    public sealed partial class CallerMemberNameAttribute : System.Attribute
    {
        public CallerMemberNameAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.Class | System.AttributeTargets.Method | System.AttributeTargets.Module)]
    public partial class CompilationRelaxationsAttribute : System.Attribute
    {
        public CompilationRelaxationsAttribute(int relaxations) { }
        public int CompilationRelaxations { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.All, Inherited=true)]
    public sealed partial class CompilerGeneratedAttribute : System.Attribute
    {
        public CompilerGeneratedAttribute() { }
    }
    public sealed partial class ConditionalWeakTable<TKey, TValue> where TKey : class where TValue : class
    {
        public ConditionalWeakTable() { }
        public void Add(TKey key, TValue value) { }
        ~ConditionalWeakTable() { }
        public TValue GetOrCreateValue(TKey key) { throw null; }
        public TValue GetValue(TKey key, System.Runtime.CompilerServices.ConditionalWeakTable<TKey, TValue>.CreateValueCallback createValueCallback) { throw null; }
        public bool Remove(TKey key) { throw null; }
        public bool TryGetValue(TKey key, out TValue value) { throw null; }
        public delegate TValue CreateValueCallback(TKey key);
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Parameter, Inherited=false)]
    public abstract partial class CustomConstantAttribute : System.Attribute
    {
        protected CustomConstantAttribute() { }
        public abstract object Value { get; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Parameter, Inherited=false)]
    public sealed partial class DateTimeConstantAttribute : System.Runtime.CompilerServices.CustomConstantAttribute
    {
        public DateTimeConstantAttribute(long ticks) { }
        public override object Value { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field | System.AttributeTargets.Parameter, Inherited=false)]
    public sealed partial class DecimalConstantAttribute : System.Attribute
    {
        public DecimalConstantAttribute(byte scale, byte sign, int hi, int mid, int low) { }
        [System.CLSCompliantAttribute(false)]
        public DecimalConstantAttribute(byte scale, byte sign, uint hi, uint mid, uint low) { }
        public decimal Value { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public sealed partial class DisablePrivateReflectionAttribute : System.Attribute
    {
        public DisablePrivateReflectionAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.Class | System.AttributeTargets.Method)]
    public sealed partial class ExtensionAttribute : System.Attribute
    {
        public ExtensionAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field, Inherited=false)]
    public sealed partial class FixedBufferAttribute : System.Attribute
    {
        public FixedBufferAttribute(System.Type elementType, int length) { }
        public System.Type ElementType { get { throw null; } }
        public int Length { get { throw null; } }
    }
    public static partial class FormattableStringFactory
    {
        public static System.FormattableString Create(string format, params object[] arguments) { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Property, Inherited=true)]
    public sealed partial class IndexerNameAttribute : System.Attribute
    {
        public IndexerNameAttribute(string indexerName) { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, AllowMultiple=true, Inherited=false)]
    public sealed partial class InternalsVisibleToAttribute : System.Attribute
    {
        public InternalsVisibleToAttribute(string assemblyName) { }
        public string AssemblyName { get { throw null; } }
    }
    public static partial class IsConst
    {
    }
    public partial interface IStrongBox
    {
        object Value { get; set; }
    }
    public static partial class IsVolatile
    {
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method, Inherited=false, AllowMultiple=false)]
    public sealed partial class IteratorStateMachineAttribute : System.Runtime.CompilerServices.StateMachineAttribute
    {
        public IteratorStateMachineAttribute(System.Type stateMachineType) : base (default(System.Type)) { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Constructor | System.AttributeTargets.Method, Inherited=false)]
    public sealed partial class MethodImplAttribute : System.Attribute
    {
        public MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions methodImplOptions) { }
        public System.Runtime.CompilerServices.MethodImplOptions Value { get { throw null; } }
    }
    [System.FlagsAttribute]
    public enum MethodImplOptions
    {
        NoInlining = 8,
        NoOptimization = 64,
        PreserveSig = 128,
        AggressiveInlining = 256,
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed partial class ReferenceAssemblyAttribute : System.Attribute
    {
        public ReferenceAssemblyAttribute() { }
        public ReferenceAssemblyAttribute(string description) { }
        public string Description { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, Inherited=false, AllowMultiple=false)]
    public sealed partial class RuntimeCompatibilityAttribute : System.Attribute
    {
        public RuntimeCompatibilityAttribute() { }
        public bool WrapNonExceptionThrows { get { throw null; } set { } }
    }
    public static partial class RuntimeHelpers
    {
        public static int OffsetToStringData { get { throw null; } }
        public static void EnsureSufficientExecutionStack() { }
        public static int GetHashCode(object o) { throw null; }
        public static object GetObjectValue(object obj) { throw null; }
        public static void InitializeArray(System.Array array, System.RuntimeFieldHandle fldHandle) { }
        public static void RunClassConstructor(System.RuntimeTypeHandle type) { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method, Inherited=false, AllowMultiple=false)]
    public partial class StateMachineAttribute : System.Attribute
    {
        public StateMachineAttribute(System.Type stateMachineType) { }
        public System.Type StateMachineType { get { throw null; } }
    }
    public partial class StrongBox<T> : System.Runtime.CompilerServices.IStrongBox
    {
        public T Value;
        public StrongBox() { }
        public StrongBox(T value) { }
        object System.Runtime.CompilerServices.IStrongBox.Value { get { throw null; } set { } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Delegate | System.AttributeTargets.Enum | System.AttributeTargets.Interface | System.AttributeTargets.Struct, Inherited=false, AllowMultiple=false)]
    public sealed partial class TypeForwardedFromAttribute : System.Attribute
    {
        public TypeForwardedFromAttribute(string assemblyFullName) { }
        public string AssemblyFullName { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, AllowMultiple=true, Inherited=false)]
    public sealed partial class TypeForwardedToAttribute : System.Attribute
    {
        public TypeForwardedToAttribute(System.Type destination) { }
        public System.Type Destination { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Struct)]
    public sealed partial class UnsafeValueTypeAttribute : System.Attribute
    {
        public UnsafeValueTypeAttribute() { }
    }
}
namespace System.Runtime.ExceptionServices
{
    public sealed partial class ExceptionDispatchInfo
    {
        internal ExceptionDispatchInfo() { }
        public System.Exception SourceException { get { throw null; } }
        public static System.Runtime.ExceptionServices.ExceptionDispatchInfo Capture(System.Exception source) { throw null; }
        public void Throw() { }
    }
}
namespace System.Runtime.InteropServices
{
    public enum CharSet
    {
        Ansi = 2,
        Unicode = 3,
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.Class | System.AttributeTargets.Delegate | System.AttributeTargets.Enum | System.AttributeTargets.Field | System.AttributeTargets.Interface | System.AttributeTargets.Method | System.AttributeTargets.Property | System.AttributeTargets.Struct, Inherited=false)]
    public sealed partial class ComVisibleAttribute : System.Attribute
    {
        public ComVisibleAttribute(bool visibility) { }
        public bool Value { get { throw null; } }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Field, Inherited=false)]
    public sealed partial class FieldOffsetAttribute : System.Attribute
    {
        public FieldOffsetAttribute(int offset) { }
        public int Value { get { throw null; } }
    }
    public enum LayoutKind
    {
        Sequential = 0,
        Explicit = 2,
        Auto = 3,
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter, Inherited=false)]
    public sealed partial class OutAttribute : System.Attribute
    {
        public OutAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Struct, Inherited=false)]
    public sealed partial class StructLayoutAttribute : System.Attribute
    {
        public System.Runtime.InteropServices.CharSet CharSet;
        public int Pack;
        public int Size;
        public StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind layoutKind) { }
        public System.Runtime.InteropServices.LayoutKind Value { get { throw null; } }
    }
}
namespace System.Runtime.Versioning
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public sealed partial class TargetFrameworkAttribute : System.Attribute
    {
        public TargetFrameworkAttribute(string frameworkName) { }
        public string FrameworkDisplayName { get { throw null; } set { } }
        public string FrameworkName { get { throw null; } }
    }
}
namespace System.Security
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public sealed partial class AllowPartiallyTrustedCallersAttribute : System.Attribute
    {
        public AllowPartiallyTrustedCallersAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.Class | System.AttributeTargets.Constructor | System.AttributeTargets.Delegate | System.AttributeTargets.Enum | System.AttributeTargets.Field | System.AttributeTargets.Interface | System.AttributeTargets.Method | System.AttributeTargets.Struct, AllowMultiple=false, Inherited=false)]
    public sealed partial class SecurityCriticalAttribute : System.Attribute
    {
        public SecurityCriticalAttribute() { }
    }
    public partial class SecurityException : System.Exception
    {
        public SecurityException() { }
        public SecurityException(string message) { }
        public SecurityException(string message, System.Exception inner) { }
        public override string ToString() { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Constructor | System.AttributeTargets.Delegate | System.AttributeTargets.Enum | System.AttributeTargets.Field | System.AttributeTargets.Interface | System.AttributeTargets.Method | System.AttributeTargets.Struct, AllowMultiple=false, Inherited=false)]
    public sealed partial class SecuritySafeCriticalAttribute : System.Attribute
    {
        public SecuritySafeCriticalAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public sealed partial class SecurityTransparentAttribute : System.Attribute
    {
        public SecurityTransparentAttribute() { }
    }
    public partial class VerificationException : System.Exception
    {
        public VerificationException() { }
        public VerificationException(string message) { }
        public VerificationException(string message, System.Exception innerException) { }
    }
}
namespace System.Text
{
    public sealed partial class StringBuilder
    {
        public StringBuilder() { }
        public StringBuilder(int capacity) { }
        public StringBuilder(int capacity, int maxCapacity) { }
        public StringBuilder(string value) { }
        public StringBuilder(string value, int capacity) { }
        public StringBuilder(string value, int startIndex, int length, int capacity) { }
        public int Capacity { get { throw null; } set { } }
        [System.Runtime.CompilerServices.IndexerName("Chars")]
        public char this[int index] { get { throw null; } set { } }
        public int Length { get { throw null; } set { } }
        public int MaxCapacity { get { throw null; } }
        public System.Text.StringBuilder Append(bool value) { throw null; }
        public System.Text.StringBuilder Append(byte value) { throw null; }
        public System.Text.StringBuilder Append(char value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public unsafe System.Text.StringBuilder Append(char* value, int valueCount) { throw null; }
        public System.Text.StringBuilder Append(char value, int repeatCount) { throw null; }
        public System.Text.StringBuilder Append(char[] value) { throw null; }
        public System.Text.StringBuilder Append(char[] value, int startIndex, int charCount) { throw null; }
        public System.Text.StringBuilder Append(decimal value) { throw null; }
        public System.Text.StringBuilder Append(double value) { throw null; }
        public System.Text.StringBuilder Append(short value) { throw null; }
        public System.Text.StringBuilder Append(int value) { throw null; }
        public System.Text.StringBuilder Append(long value) { throw null; }
        public System.Text.StringBuilder Append(object value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public System.Text.StringBuilder Append(sbyte value) { throw null; }
        public System.Text.StringBuilder Append(float value) { throw null; }
        public System.Text.StringBuilder Append(string value) { throw null; }
        public System.Text.StringBuilder Append(string value, int startIndex, int count) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public System.Text.StringBuilder Append(ushort value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public System.Text.StringBuilder Append(uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public System.Text.StringBuilder Append(ulong value) { throw null; }
        public System.Text.StringBuilder AppendFormat(System.IFormatProvider provider, string format, object arg0) { throw null; }
        public System.Text.StringBuilder AppendFormat(System.IFormatProvider provider, string format, object arg0, object arg1) { throw null; }
        public System.Text.StringBuilder AppendFormat(System.IFormatProvider provider, string format, object arg0, object arg1, object arg2) { throw null; }
        public System.Text.StringBuilder AppendFormat(System.IFormatProvider provider, string format, params object[] args) { throw null; }
        public System.Text.StringBuilder AppendFormat(string format, object arg0) { throw null; }
        public System.Text.StringBuilder AppendFormat(string format, object arg0, object arg1) { throw null; }
        public System.Text.StringBuilder AppendFormat(string format, object arg0, object arg1, object arg2) { throw null; }
        public System.Text.StringBuilder AppendFormat(string format, params object[] args) { throw null; }
        public System.Text.StringBuilder AppendLine() { throw null; }
        public System.Text.StringBuilder AppendLine(string value) { throw null; }
        public System.Text.StringBuilder Clear() { throw null; }
        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) { }
        public int EnsureCapacity(int capacity) { throw null; }
        public bool Equals(System.Text.StringBuilder sb) { throw null; }
        public System.Text.StringBuilder Insert(int index, bool value) { throw null; }
        public System.Text.StringBuilder Insert(int index, byte value) { throw null; }
        public System.Text.StringBuilder Insert(int index, char value) { throw null; }
        public System.Text.StringBuilder Insert(int index, char[] value) { throw null; }
        public System.Text.StringBuilder Insert(int index, char[] value, int startIndex, int charCount) { throw null; }
        public System.Text.StringBuilder Insert(int index, decimal value) { throw null; }
        public System.Text.StringBuilder Insert(int index, double value) { throw null; }
        public System.Text.StringBuilder Insert(int index, short value) { throw null; }
        public System.Text.StringBuilder Insert(int index, int value) { throw null; }
        public System.Text.StringBuilder Insert(int index, long value) { throw null; }
        public System.Text.StringBuilder Insert(int index, object value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public System.Text.StringBuilder Insert(int index, sbyte value) { throw null; }
        public System.Text.StringBuilder Insert(int index, float value) { throw null; }
        public System.Text.StringBuilder Insert(int index, string value) { throw null; }
        public System.Text.StringBuilder Insert(int index, string value, int count) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public System.Text.StringBuilder Insert(int index, ushort value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public System.Text.StringBuilder Insert(int index, uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public System.Text.StringBuilder Insert(int index, ulong value) { throw null; }
        public System.Text.StringBuilder Remove(int startIndex, int length) { throw null; }
        public System.Text.StringBuilder Replace(char oldChar, char newChar) { throw null; }
        public System.Text.StringBuilder Replace(char oldChar, char newChar, int startIndex, int count) { throw null; }
        public System.Text.StringBuilder Replace(string oldValue, string newValue) { throw null; }
        public System.Text.StringBuilder Replace(string oldValue, string newValue, int startIndex, int count) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(int startIndex, int length) { throw null; }
    }
}
namespace System.Threading
{
    public enum LazyThreadSafetyMode
    {
        None = 0,
        PublicationOnly = 1,
        ExecutionAndPublication = 2,
    }
    public static partial class Timeout
    {
        public const int Infinite = -1;
        public static readonly System.TimeSpan InfiniteTimeSpan;
    }
    public abstract partial class WaitHandle : System.IDisposable
    {
        protected static readonly System.IntPtr InvalidHandle;
        public const int WaitTimeout = 258;
        protected WaitHandle() { }
        public void Dispose() { }
        protected virtual void Dispose(bool explicitDisposing) { }
        public static bool WaitAll(System.Threading.WaitHandle[] waitHandles) { throw null; }
        public static bool WaitAll(System.Threading.WaitHandle[] waitHandles, int millisecondsTimeout) { throw null; }
        public static bool WaitAll(System.Threading.WaitHandle[] waitHandles, System.TimeSpan timeout) { throw null; }
        public static int WaitAny(System.Threading.WaitHandle[] waitHandles) { throw null; }
        public static int WaitAny(System.Threading.WaitHandle[] waitHandles, int millisecondsTimeout) { throw null; }
        public static int WaitAny(System.Threading.WaitHandle[] waitHandles, System.TimeSpan timeout) { throw null; }
        public virtual bool WaitOne() { throw null; }
        public virtual bool WaitOne(int millisecondsTimeout) { throw null; }
        public virtual bool WaitOne(System.TimeSpan timeout) { throw null; }
    }
}
