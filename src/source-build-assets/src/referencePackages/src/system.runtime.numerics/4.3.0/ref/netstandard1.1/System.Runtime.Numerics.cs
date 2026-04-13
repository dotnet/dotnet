// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Runtime.Numerics")]
[assembly: System.Reflection.AssemblyDescription("System.Runtime.Numerics")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Runtime.Numerics")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Numerics
{
    public partial struct BigInteger : IComparable, IComparable<BigInteger>, IEquatable<BigInteger>, IFormattable
    {
        [CLSCompliant(false)]
        public BigInteger(byte[] value) { }

        public BigInteger(decimal value) { }

        public BigInteger(double value) { }

        public BigInteger(int value) { }

        public BigInteger(long value) { }

        public BigInteger(float value) { }

        [CLSCompliant(false)]
        public BigInteger(uint value) { }

        [CLSCompliant(false)]
        public BigInteger(ulong value) { }

        public bool IsEven { get { throw null; } }

        public bool IsOne { get { throw null; } }

        public bool IsPowerOfTwo { get { throw null; } }

        public bool IsZero { get { throw null; } }

        public static BigInteger MinusOne { get { throw null; } }

        public static BigInteger One { get { throw null; } }

        public int Sign { get { throw null; } }

        public static BigInteger Zero { get { throw null; } }

        public static BigInteger Abs(BigInteger value) { throw null; }

        public static BigInteger Add(BigInteger left, BigInteger right) { throw null; }

        public static int Compare(BigInteger left, BigInteger right) { throw null; }

        public int CompareTo(long other) { throw null; }

        public int CompareTo(BigInteger other) { throw null; }

        [CLSCompliant(false)]
        public int CompareTo(ulong other) { throw null; }

        public static BigInteger Divide(BigInteger dividend, BigInteger divisor) { throw null; }

        public static BigInteger DivRem(BigInteger dividend, BigInteger divisor, out BigInteger remainder) { throw null; }

        public bool Equals(long other) { throw null; }

        public bool Equals(BigInteger other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        [CLSCompliant(false)]
        public bool Equals(ulong other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static BigInteger GreatestCommonDivisor(BigInteger left, BigInteger right) { throw null; }

        public static double Log(BigInteger value, double baseValue) { throw null; }

        public static double Log(BigInteger value) { throw null; }

        public static double Log10(BigInteger value) { throw null; }

        public static BigInteger Max(BigInteger left, BigInteger right) { throw null; }

        public static BigInteger Min(BigInteger left, BigInteger right) { throw null; }

        public static BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus) { throw null; }

        public static BigInteger Multiply(BigInteger left, BigInteger right) { throw null; }

        public static BigInteger Negate(BigInteger value) { throw null; }

        public static BigInteger operator +(BigInteger left, BigInteger right) { throw null; }

        public static BigInteger operator &(BigInteger left, BigInteger right) { throw null; }

        public static BigInteger operator |(BigInteger left, BigInteger right) { throw null; }

        public static BigInteger operator --(BigInteger value) { throw null; }

        public static BigInteger operator /(BigInteger dividend, BigInteger divisor) { throw null; }

        public static bool operator ==(long left, BigInteger right) { throw null; }

        public static bool operator ==(BigInteger left, long right) { throw null; }

        public static bool operator ==(BigInteger left, BigInteger right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator ==(BigInteger left, ulong right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator ==(ulong left, BigInteger right) { throw null; }

        public static BigInteger operator ^(BigInteger left, BigInteger right) { throw null; }

        public static explicit operator BigInteger(decimal value) { throw null; }

        public static explicit operator BigInteger(double value) { throw null; }

        public static explicit operator byte(BigInteger value) { throw null; }

        public static explicit operator decimal(BigInteger value) { throw null; }

        public static explicit operator double(BigInteger value) { throw null; }

        public static explicit operator short(BigInteger value) { throw null; }

        public static explicit operator int(BigInteger value) { throw null; }

        public static explicit operator long(BigInteger value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator sbyte(BigInteger value) { throw null; }

        public static explicit operator float(BigInteger value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator ushort(BigInteger value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator uint(BigInteger value) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator ulong(BigInteger value) { throw null; }

        public static explicit operator BigInteger(float value) { throw null; }

        public static bool operator >(long left, BigInteger right) { throw null; }

        public static bool operator >(BigInteger left, long right) { throw null; }

        public static bool operator >(BigInteger left, BigInteger right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator >(BigInteger left, ulong right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator >(ulong left, BigInteger right) { throw null; }

        public static bool operator >=(long left, BigInteger right) { throw null; }

        public static bool operator >=(BigInteger left, long right) { throw null; }

        public static bool operator >=(BigInteger left, BigInteger right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator >=(BigInteger left, ulong right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator >=(ulong left, BigInteger right) { throw null; }

        public static implicit operator BigInteger(byte value) { throw null; }

        public static implicit operator BigInteger(short value) { throw null; }

        public static implicit operator BigInteger(int value) { throw null; }

        public static implicit operator BigInteger(long value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator BigInteger(sbyte value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator BigInteger(ushort value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator BigInteger(uint value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator BigInteger(ulong value) { throw null; }

        public static BigInteger operator ++(BigInteger value) { throw null; }

        public static bool operator !=(long left, BigInteger right) { throw null; }

        public static bool operator !=(BigInteger left, long right) { throw null; }

        public static bool operator !=(BigInteger left, BigInteger right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator !=(BigInteger left, ulong right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator !=(ulong left, BigInteger right) { throw null; }

        public static BigInteger operator <<(BigInteger value, int shift) { throw null; }

        public static bool operator <(long left, BigInteger right) { throw null; }

        public static bool operator <(BigInteger left, long right) { throw null; }

        public static bool operator <(BigInteger left, BigInteger right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator <(BigInteger left, ulong right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator <(ulong left, BigInteger right) { throw null; }

        public static bool operator <=(long left, BigInteger right) { throw null; }

        public static bool operator <=(BigInteger left, long right) { throw null; }

        public static bool operator <=(BigInteger left, BigInteger right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator <=(BigInteger left, ulong right) { throw null; }

        [CLSCompliant(false)]
        public static bool operator <=(ulong left, BigInteger right) { throw null; }

        public static BigInteger operator %(BigInteger dividend, BigInteger divisor) { throw null; }

        public static BigInteger operator *(BigInteger left, BigInteger right) { throw null; }

        public static BigInteger operator ~(BigInteger value) { throw null; }

        public static BigInteger operator >>(BigInteger value, int shift) { throw null; }

        public static BigInteger operator -(BigInteger left, BigInteger right) { throw null; }

        public static BigInteger operator -(BigInteger value) { throw null; }

        public static BigInteger operator +(BigInteger value) { throw null; }

        public static BigInteger Parse(string value, Globalization.NumberStyles style, IFormatProvider provider) { throw null; }

        public static BigInteger Parse(string value, Globalization.NumberStyles style) { throw null; }

        public static BigInteger Parse(string value, IFormatProvider provider) { throw null; }

        public static BigInteger Parse(string value) { throw null; }

        public static BigInteger Pow(BigInteger value, int exponent) { throw null; }

        public static BigInteger Remainder(BigInteger dividend, BigInteger divisor) { throw null; }

        public static BigInteger Subtract(BigInteger left, BigInteger right) { throw null; }

        int IComparable.CompareTo(object obj) { throw null; }

        public byte[] ToByteArray() { throw null; }

        public override string ToString() { throw null; }

        public string ToString(IFormatProvider provider) { throw null; }

        public string ToString(string format, IFormatProvider provider) { throw null; }

        public string ToString(string format) { throw null; }

        public static bool TryParse(string value, Globalization.NumberStyles style, IFormatProvider provider, out BigInteger result) { throw null; }

        public static bool TryParse(string value, out BigInteger result) { throw null; }
    }

    public partial struct Complex : IEquatable<Complex>, IFormattable
    {
        public static readonly Complex ImaginaryOne;
        public static readonly Complex One;
        public static readonly Complex Zero;
        public Complex(double real, double imaginary) { }

        public double Imaginary { get { throw null; } }

        public double Magnitude { get { throw null; } }

        public double Phase { get { throw null; } }

        public double Real { get { throw null; } }

        public static double Abs(Complex value) { throw null; }

        public static Complex Acos(Complex value) { throw null; }

        public static Complex Add(Complex left, Complex right) { throw null; }

        public static Complex Asin(Complex value) { throw null; }

        public static Complex Atan(Complex value) { throw null; }

        public static Complex Conjugate(Complex value) { throw null; }

        public static Complex Cos(Complex value) { throw null; }

        public static Complex Cosh(Complex value) { throw null; }

        public static Complex Divide(Complex dividend, Complex divisor) { throw null; }

        public bool Equals(Complex value) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public static Complex Exp(Complex value) { throw null; }

        public static Complex FromPolarCoordinates(double magnitude, double phase) { throw null; }

        public override int GetHashCode() { throw null; }

        public static Complex Log(Complex value, double baseValue) { throw null; }

        public static Complex Log(Complex value) { throw null; }

        public static Complex Log10(Complex value) { throw null; }

        public static Complex Multiply(Complex left, Complex right) { throw null; }

        public static Complex Negate(Complex value) { throw null; }

        public static Complex operator +(Complex left, Complex right) { throw null; }

        public static Complex operator /(Complex left, Complex right) { throw null; }

        public static bool operator ==(Complex left, Complex right) { throw null; }

        public static explicit operator Complex(decimal value) { throw null; }

        public static explicit operator Complex(BigInteger value) { throw null; }

        public static implicit operator Complex(byte value) { throw null; }

        public static implicit operator Complex(double value) { throw null; }

        public static implicit operator Complex(short value) { throw null; }

        public static implicit operator Complex(int value) { throw null; }

        public static implicit operator Complex(long value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator Complex(sbyte value) { throw null; }

        public static implicit operator Complex(float value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator Complex(ushort value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator Complex(uint value) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator Complex(ulong value) { throw null; }

        public static bool operator !=(Complex left, Complex right) { throw null; }

        public static Complex operator *(Complex left, Complex right) { throw null; }

        public static Complex operator -(Complex left, Complex right) { throw null; }

        public static Complex operator -(Complex value) { throw null; }

        public static Complex Pow(Complex value, double power) { throw null; }

        public static Complex Pow(Complex value, Complex power) { throw null; }

        public static Complex Reciprocal(Complex value) { throw null; }

        public static Complex Sin(Complex value) { throw null; }

        public static Complex Sinh(Complex value) { throw null; }

        public static Complex Sqrt(Complex value) { throw null; }

        public static Complex Subtract(Complex left, Complex right) { throw null; }

        public static Complex Tan(Complex value) { throw null; }

        public static Complex Tanh(Complex value) { throw null; }

        public override string ToString() { throw null; }

        public string ToString(IFormatProvider provider) { throw null; }

        public string ToString(string format, IFormatProvider provider) { throw null; }

        public string ToString(string format) { throw null; }
    }
}