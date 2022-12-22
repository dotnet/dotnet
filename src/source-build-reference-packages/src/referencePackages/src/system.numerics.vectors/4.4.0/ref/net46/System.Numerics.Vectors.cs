// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Numerics.Vectors")]
[assembly: AssemblyDescription("System.Numerics.Vectors")]
[assembly: AssemblyDefaultAlias("System.Numerics.Vectors")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.25519.03")]
[assembly: AssemblyInformationalVersion("4.6.25519.03 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.3.0")]

[assembly: TypeForwardedTo(typeof(System.Numerics.Matrix3x2))]
[assembly: TypeForwardedTo(typeof(System.Numerics.Matrix4x4))]
[assembly: TypeForwardedTo(typeof(System.Numerics.Plane))]
[assembly: TypeForwardedTo(typeof(System.Numerics.Quaternion))]
[assembly: TypeForwardedTo(typeof(System.Numerics.Vector2))]
[assembly: TypeForwardedTo(typeof(System.Numerics.Vector3))]
[assembly: TypeForwardedTo(typeof(System.Numerics.Vector4))]



namespace System.Numerics
{
    public static partial class Vector
    {
        public static bool IsHardwareAccelerated { get { throw null; } }
        public static System.Numerics.Vector<T> Abs<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        public static System.Numerics.Vector<T> Add<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> AndNot<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Byte> AsVectorByte<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Double> AsVectorDouble<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Int16> AsVectorInt16<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Int32> AsVectorInt32<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Int64> AsVectorInt64<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.SByte> AsVectorSByte<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Single> AsVectorSingle<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.UInt16> AsVectorUInt16<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.UInt32> AsVectorUInt32<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.UInt64> AsVectorUInt64<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        public static System.Numerics.Vector<T> BitwiseAnd<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> BitwiseOr<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Single> ConditionalSelect(System.Numerics.Vector<System.Int32> condition, System.Numerics.Vector<System.Single> left, System.Numerics.Vector<System.Single> right) { throw null; }
        public static System.Numerics.Vector<System.Double> ConditionalSelect(System.Numerics.Vector<System.Int64> condition, System.Numerics.Vector<System.Double> left, System.Numerics.Vector<System.Double> right) { throw null; }
        public static System.Numerics.Vector<T> ConditionalSelect<T>(System.Numerics.Vector<T> condition, System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Double> ConvertToDouble(System.Numerics.Vector<System.Int64> value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.Double> ConvertToDouble(System.Numerics.Vector<System.UInt64> value) { throw null; }
        public static System.Numerics.Vector<System.Int32> ConvertToInt32(System.Numerics.Vector<System.Single> value) { throw null; }
        public static System.Numerics.Vector<System.Int64> ConvertToInt64(System.Numerics.Vector<System.Double> value) { throw null; }
        public static System.Numerics.Vector<System.Single> ConvertToSingle(System.Numerics.Vector<System.Int32> value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.Single> ConvertToSingle(System.Numerics.Vector<System.UInt32> value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.UInt32> ConvertToUInt32(System.Numerics.Vector<System.Single> value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.UInt64> ConvertToUInt64(System.Numerics.Vector<System.Double> value) { throw null; }
        public static System.Numerics.Vector<T> Divide<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static T Dot<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Int64> Equals(System.Numerics.Vector<System.Double> left, System.Numerics.Vector<System.Double> right) { throw null; }
        public static System.Numerics.Vector<System.Int32> Equals(System.Numerics.Vector<System.Int32> left, System.Numerics.Vector<System.Int32> right) { throw null; }
        public static System.Numerics.Vector<System.Int64> Equals(System.Numerics.Vector<System.Int64> left, System.Numerics.Vector<System.Int64> right) { throw null; }
        public static System.Numerics.Vector<System.Int32> Equals(System.Numerics.Vector<System.Single> left, System.Numerics.Vector<System.Single> right) { throw null; }
        public static bool EqualsAll<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static bool EqualsAny<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> Equals<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Int64> GreaterThan(System.Numerics.Vector<System.Double> left, System.Numerics.Vector<System.Double> right) { throw null; }
        public static System.Numerics.Vector<System.Int32> GreaterThan(System.Numerics.Vector<System.Int32> left, System.Numerics.Vector<System.Int32> right) { throw null; }
        public static System.Numerics.Vector<System.Int64> GreaterThan(System.Numerics.Vector<System.Int64> left, System.Numerics.Vector<System.Int64> right) { throw null; }
        public static System.Numerics.Vector<System.Int32> GreaterThan(System.Numerics.Vector<System.Single> left, System.Numerics.Vector<System.Single> right) { throw null; }
        public static bool GreaterThanAll<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static bool GreaterThanAny<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Int64> GreaterThanOrEqual(System.Numerics.Vector<System.Double> left, System.Numerics.Vector<System.Double> right) { throw null; }
        public static System.Numerics.Vector<System.Int32> GreaterThanOrEqual(System.Numerics.Vector<System.Int32> left, System.Numerics.Vector<System.Int32> right) { throw null; }
        public static System.Numerics.Vector<System.Int64> GreaterThanOrEqual(System.Numerics.Vector<System.Int64> left, System.Numerics.Vector<System.Int64> right) { throw null; }
        public static System.Numerics.Vector<System.Int32> GreaterThanOrEqual(System.Numerics.Vector<System.Single> left, System.Numerics.Vector<System.Single> right) { throw null; }
        public static bool GreaterThanOrEqualAll<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static bool GreaterThanOrEqualAny<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> GreaterThanOrEqual<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> GreaterThan<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Int64> LessThan(System.Numerics.Vector<System.Double> left, System.Numerics.Vector<System.Double> right) { throw null; }
        public static System.Numerics.Vector<System.Int32> LessThan(System.Numerics.Vector<System.Int32> left, System.Numerics.Vector<System.Int32> right) { throw null; }
        public static System.Numerics.Vector<System.Int64> LessThan(System.Numerics.Vector<System.Int64> left, System.Numerics.Vector<System.Int64> right) { throw null; }
        public static System.Numerics.Vector<System.Int32> LessThan(System.Numerics.Vector<System.Single> left, System.Numerics.Vector<System.Single> right) { throw null; }
        public static bool LessThanAll<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static bool LessThanAny<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Int64> LessThanOrEqual(System.Numerics.Vector<System.Double> left, System.Numerics.Vector<System.Double> right) { throw null; }
        public static System.Numerics.Vector<System.Int32> LessThanOrEqual(System.Numerics.Vector<System.Int32> left, System.Numerics.Vector<System.Int32> right) { throw null; }
        public static System.Numerics.Vector<System.Int64> LessThanOrEqual(System.Numerics.Vector<System.Int64> left, System.Numerics.Vector<System.Int64> right) { throw null; }
        public static System.Numerics.Vector<System.Int32> LessThanOrEqual(System.Numerics.Vector<System.Single> left, System.Numerics.Vector<System.Single> right) { throw null; }
        public static bool LessThanOrEqualAll<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static bool LessThanOrEqualAny<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> LessThanOrEqual<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> LessThan<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> Max<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> Min<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> Multiply<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> Multiply<T>(System.Numerics.Vector<T> left, T right) where T : struct { throw null; }
        public static System.Numerics.Vector<T> Multiply<T>(T left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        public static System.Numerics.Vector<System.Single> Narrow(System.Numerics.Vector<System.Double> source1, System.Numerics.Vector<System.Double> source2) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.SByte> Narrow(System.Numerics.Vector<System.Int16> source1, System.Numerics.Vector<System.Int16> source2) { throw null; }
        public static System.Numerics.Vector<System.Int16> Narrow(System.Numerics.Vector<System.Int32> source1, System.Numerics.Vector<System.Int32> source2) { throw null; }
        public static System.Numerics.Vector<System.Int32> Narrow(System.Numerics.Vector<System.Int64> source1, System.Numerics.Vector<System.Int64> source2) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.Byte> Narrow(System.Numerics.Vector<System.UInt16> source1, System.Numerics.Vector<System.UInt16> source2) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.UInt16> Narrow(System.Numerics.Vector<System.UInt32> source1, System.Numerics.Vector<System.UInt32> source2) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static System.Numerics.Vector<System.UInt32> Narrow(System.Numerics.Vector<System.UInt64> source1, System.Numerics.Vector<System.UInt64> source2) { throw null; }
        public static System.Numerics.Vector<T> Negate<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        public static System.Numerics.Vector<T> OnesComplement<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        public static System.Numerics.Vector<T> SquareRoot<T>(System.Numerics.Vector<T> value) where T : struct { throw null; }
        public static System.Numerics.Vector<T> Subtract<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static void Widen(System.Numerics.Vector<System.Byte> source, out System.Numerics.Vector<System.UInt16> dest1, out System.Numerics.Vector<System.UInt16> dest2) { throw null; }
        public static void Widen(System.Numerics.Vector<System.Int16> source, out System.Numerics.Vector<System.Int32> dest1, out System.Numerics.Vector<System.Int32> dest2) { throw null; }
        public static void Widen(System.Numerics.Vector<System.Int32> source, out System.Numerics.Vector<System.Int64> dest1, out System.Numerics.Vector<System.Int64> dest2) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static void Widen(System.Numerics.Vector<System.SByte> source, out System.Numerics.Vector<System.Int16> dest1, out System.Numerics.Vector<System.Int16> dest2) { throw null; }
        public static void Widen(System.Numerics.Vector<System.Single> source, out System.Numerics.Vector<System.Double> dest1, out System.Numerics.Vector<System.Double> dest2) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static void Widen(System.Numerics.Vector<System.UInt16> source, out System.Numerics.Vector<System.UInt32> dest1, out System.Numerics.Vector<System.UInt32> dest2) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static void Widen(System.Numerics.Vector<System.UInt32> source, out System.Numerics.Vector<System.UInt64> dest1, out System.Numerics.Vector<System.UInt64> dest2) { throw null; }
        public static System.Numerics.Vector<T> Xor<T>(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) where T : struct { throw null; }
    }
    public partial struct Vector<T> : System.IEquatable<System.Numerics.Vector<T>>, System.IFormattable where T : struct
    {
        public Vector(T value) { throw null; }
        public Vector(T[] values) { throw null; }
        public Vector(T[] values, int index) { throw null; }
        public static int Count { get { throw null; } }
        public T this[int index] { get { throw null; } }
        public static System.Numerics.Vector<T> One { get { throw null; } }
        public static System.Numerics.Vector<T> Zero { get { throw null; } }
        public void CopyTo(T[] destination) { }
        public void CopyTo(T[] destination, int startIndex) { }
        public bool Equals(System.Numerics.Vector<T> other) { throw null; }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Numerics.Vector<T> operator +(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) { throw null; }
        public static System.Numerics.Vector<T> operator &(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) { throw null; }
        public static System.Numerics.Vector<T> operator |(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) { throw null; }
        public static System.Numerics.Vector<T> operator /(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) { throw null; }
        public static bool operator ==(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) { throw null; }
        public static System.Numerics.Vector<T> operator ^(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) { throw null; }
        public static explicit operator System.Numerics.Vector<System.Byte> (System.Numerics.Vector<T> value) { throw null; }
        public static explicit operator System.Numerics.Vector<System.Double> (System.Numerics.Vector<T> value) { throw null; }
        public static explicit operator System.Numerics.Vector<System.Int16> (System.Numerics.Vector<T> value) { throw null; }
        public static explicit operator System.Numerics.Vector<System.Int32> (System.Numerics.Vector<T> value) { throw null; }
        public static explicit operator System.Numerics.Vector<System.Int64> (System.Numerics.Vector<T> value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator System.Numerics.Vector<System.SByte> (System.Numerics.Vector<T> value) { throw null; }
        public static explicit operator System.Numerics.Vector<System.Single> (System.Numerics.Vector<T> value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator System.Numerics.Vector<System.UInt16> (System.Numerics.Vector<T> value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator System.Numerics.Vector<System.UInt32> (System.Numerics.Vector<T> value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static explicit operator System.Numerics.Vector<System.UInt64> (System.Numerics.Vector<T> value) { throw null; }
        public static bool operator !=(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) { throw null; }
        public static System.Numerics.Vector<T> operator *(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) { throw null; }
        public static System.Numerics.Vector<T> operator *(System.Numerics.Vector<T> value, T factor) { throw null; }
        public static System.Numerics.Vector<T> operator *(T factor, System.Numerics.Vector<T> value) { throw null; }
        public static System.Numerics.Vector<T> operator ~(System.Numerics.Vector<T> value) { throw null; }
        public static System.Numerics.Vector<T> operator -(System.Numerics.Vector<T> left, System.Numerics.Vector<T> right) { throw null; }
        public static System.Numerics.Vector<T> operator -(System.Numerics.Vector<T> value) { throw null; }
        public override string ToString() { throw null; }
        public string ToString(string format) { throw null; }
        public string ToString(string format, System.IFormatProvider formatProvider) { throw null; }
    }
}
