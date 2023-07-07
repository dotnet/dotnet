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
[assembly: System.Reflection.AssemblyTitle("System.Security.Cryptography.Algorithms")]
[assembly: System.Reflection.AssemblyDescription("System.Security.Cryptography.Algorithms")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Security.Cryptography.Algorithms")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.2.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Security.Cryptography
{
    public abstract partial class Aes : SymmetricAlgorithm
    {
        public override KeySizes[] LegalBlockSizes { get { throw null; } }

        public override KeySizes[] LegalKeySizes { get { throw null; } }

        public static Aes Create() { throw null; }
    }

    public abstract partial class DeriveBytes : IDisposable
    {
        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public abstract byte[] GetBytes(int cb);
        public abstract void Reset();
    }

    public partial struct ECCurve
    {
        public byte[] A;
        public byte[] B;
        public byte[] Cofactor;
        public ECCurveType CurveType;
        public ECPoint G;
        public HashAlgorithmName? Hash;
        public byte[] Order;
        public byte[] Polynomial;
        public byte[] Prime;
        public byte[] Seed;
        public bool IsCharacteristic2 { get { throw null; } }

        public bool IsExplicit { get { throw null; } }

        public bool IsNamed { get { throw null; } }

        public bool IsPrime { get { throw null; } }

        public Oid Oid { get { throw null; } }

        public static ECCurve CreateFromFriendlyName(string oidFriendlyName) { throw null; }

        public static ECCurve CreateFromOid(Oid curveOid) { throw null; }

        public static ECCurve CreateFromValue(string oidValue) { throw null; }

        public void Validate() { }

        public enum ECCurveType
        {
            Implicit = 0,
            PrimeShortWeierstrass = 1,
            PrimeTwistedEdwards = 2,
            PrimeMontgomery = 3,
            Characteristic2 = 4,
            Named = 5
        }

        public static partial class NamedCurves
        {
            public static ECCurve brainpoolP160r1 { get { throw null; } }

            public static ECCurve brainpoolP160t1 { get { throw null; } }

            public static ECCurve brainpoolP192r1 { get { throw null; } }

            public static ECCurve brainpoolP192t1 { get { throw null; } }

            public static ECCurve brainpoolP224r1 { get { throw null; } }

            public static ECCurve brainpoolP224t1 { get { throw null; } }

            public static ECCurve brainpoolP256r1 { get { throw null; } }

            public static ECCurve brainpoolP256t1 { get { throw null; } }

            public static ECCurve brainpoolP320r1 { get { throw null; } }

            public static ECCurve brainpoolP320t1 { get { throw null; } }

            public static ECCurve brainpoolP384r1 { get { throw null; } }

            public static ECCurve brainpoolP384t1 { get { throw null; } }

            public static ECCurve brainpoolP512r1 { get { throw null; } }

            public static ECCurve brainpoolP512t1 { get { throw null; } }

            public static ECCurve nistP256 { get { throw null; } }

            public static ECCurve nistP384 { get { throw null; } }

            public static ECCurve nistP521 { get { throw null; } }
        }
    }

    public abstract partial class ECDsa : AsymmetricAlgorithm
    {
        public static ECDsa Create() { throw null; }

        public static ECDsa Create(ECCurve curve) { throw null; }

        public static ECDsa Create(ECParameters parameters) { throw null; }

        public virtual ECParameters ExportExplicitParameters(bool includePrivateParameters) { throw null; }

        public virtual ECParameters ExportParameters(bool includePrivateParameters) { throw null; }

        public virtual void GenerateKey(ECCurve curve) { }

        protected abstract byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm);
        protected abstract byte[] HashData(IO.Stream data, HashAlgorithmName hashAlgorithm);
        public virtual void ImportParameters(ECParameters parameters) { }

        public virtual byte[] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm) { throw null; }

        public virtual byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm) { throw null; }

        public virtual byte[] SignData(IO.Stream data, HashAlgorithmName hashAlgorithm) { throw null; }

        public abstract byte[] SignHash(byte[] hash);
        public bool VerifyData(byte[] data, byte[] signature, HashAlgorithmName hashAlgorithm) { throw null; }

        public virtual bool VerifyData(byte[] data, int offset, int count, byte[] signature, HashAlgorithmName hashAlgorithm) { throw null; }

        public bool VerifyData(IO.Stream data, byte[] signature, HashAlgorithmName hashAlgorithm) { throw null; }

        public abstract bool VerifyHash(byte[] hash, byte[] signature);
    }

    public partial struct ECParameters
    {
        public ECCurve Curve;
        public byte[] D;
        public ECPoint Q;
        public void Validate() { }
    }

    public partial struct ECPoint
    {
        public byte[] X;
        public byte[] Y;
    }

    public partial class HMACMD5 : HMAC
    {
        public HMACMD5() { }

        public HMACMD5(byte[] key) { }

        public override int HashSize { get { throw null; } }

        public override byte[] Key { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        protected override void HashCore(byte[] rgb, int ib, int cb) { }

        protected override byte[] HashFinal() { throw null; }

        public override void Initialize() { }
    }

    public partial class HMACSHA1 : HMAC
    {
        public HMACSHA1() { }

        public HMACSHA1(byte[] key) { }

        public override int HashSize { get { throw null; } }

        public override byte[] Key { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        protected override void HashCore(byte[] rgb, int ib, int cb) { }

        protected override byte[] HashFinal() { throw null; }

        public override void Initialize() { }
    }

    public partial class HMACSHA256 : HMAC
    {
        public HMACSHA256() { }

        public HMACSHA256(byte[] key) { }

        public override int HashSize { get { throw null; } }

        public override byte[] Key { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        protected override void HashCore(byte[] rgb, int ib, int cb) { }

        protected override byte[] HashFinal() { throw null; }

        public override void Initialize() { }
    }

    public partial class HMACSHA384 : HMAC
    {
        public HMACSHA384() { }

        public HMACSHA384(byte[] key) { }

        public override int HashSize { get { throw null; } }

        public override byte[] Key { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        protected override void HashCore(byte[] rgb, int ib, int cb) { }

        protected override byte[] HashFinal() { throw null; }

        public override void Initialize() { }
    }

    public partial class HMACSHA512 : HMAC
    {
        public HMACSHA512() { }

        public HMACSHA512(byte[] key) { }

        public override int HashSize { get { throw null; } }

        public override byte[] Key { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        protected override void HashCore(byte[] rgb, int ib, int cb) { }

        protected override byte[] HashFinal() { throw null; }

        public override void Initialize() { }
    }

    public sealed partial class IncrementalHash : IDisposable
    {
        internal IncrementalHash() { }

        public HashAlgorithmName AlgorithmName { get { throw null; } }

        public void AppendData(byte[] data, int offset, int count) { }

        public void AppendData(byte[] data) { }

        public static IncrementalHash CreateHash(HashAlgorithmName hashAlgorithm) { throw null; }

        public static IncrementalHash CreateHMAC(HashAlgorithmName hashAlgorithm, byte[] key) { throw null; }

        public void Dispose() { }

        public byte[] GetHashAndReset() { throw null; }
    }

    public abstract partial class MD5 : HashAlgorithm
    {
        public static MD5 Create() { throw null; }
    }

    public abstract partial class RandomNumberGenerator : IDisposable
    {
        public static RandomNumberGenerator Create() { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public abstract void GetBytes(byte[] data);
    }

    public partial class Rfc2898DeriveBytes : DeriveBytes
    {
        public Rfc2898DeriveBytes(byte[] password, byte[] salt, int iterations) { }

        public Rfc2898DeriveBytes(string password, byte[] salt, int iterations) { }

        public Rfc2898DeriveBytes(string password, byte[] salt) { }

        public Rfc2898DeriveBytes(string password, int saltSize, int iterations) { }

        public Rfc2898DeriveBytes(string password, int saltSize) { }

        public int IterationCount { get { throw null; } set { } }

        public byte[] Salt { get { throw null; } set { } }

        protected override void Dispose(bool disposing) { }

        public override byte[] GetBytes(int cb) { throw null; }

        public override void Reset() { }
    }

    public abstract partial class RSA : AsymmetricAlgorithm
    {
        public static RSA Create() { throw null; }

        public abstract byte[] Decrypt(byte[] data, RSAEncryptionPadding padding);
        public abstract byte[] Encrypt(byte[] data, RSAEncryptionPadding padding);
        public abstract RSAParameters ExportParameters(bool includePrivateParameters);
        protected abstract byte[] HashData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm);
        protected abstract byte[] HashData(IO.Stream data, HashAlgorithmName hashAlgorithm);
        public abstract void ImportParameters(RSAParameters parameters);
        public virtual byte[] SignData(byte[] data, int offset, int count, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }

        public byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }

        public virtual byte[] SignData(IO.Stream data, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }

        public abstract byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding);
        public bool VerifyData(byte[] data, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }

        public virtual bool VerifyData(byte[] data, int offset, int count, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }

        public bool VerifyData(IO.Stream data, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding) { throw null; }

        public abstract bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding);
    }

    public sealed partial class RSAEncryptionPadding : IEquatable<RSAEncryptionPadding>
    {
        internal RSAEncryptionPadding() { }

        public RSAEncryptionPaddingMode Mode { get { throw null; } }

        public HashAlgorithmName OaepHashAlgorithm { get { throw null; } }

        public static RSAEncryptionPadding OaepSHA1 { get { throw null; } }

        public static RSAEncryptionPadding OaepSHA256 { get { throw null; } }

        public static RSAEncryptionPadding OaepSHA384 { get { throw null; } }

        public static RSAEncryptionPadding OaepSHA512 { get { throw null; } }

        public static RSAEncryptionPadding Pkcs1 { get { throw null; } }

        public static RSAEncryptionPadding CreateOaep(HashAlgorithmName hashAlgorithm) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(RSAEncryptionPadding other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(RSAEncryptionPadding left, RSAEncryptionPadding right) { throw null; }

        public static bool operator !=(RSAEncryptionPadding left, RSAEncryptionPadding right) { throw null; }

        public override string ToString() { throw null; }
    }

    public enum RSAEncryptionPaddingMode
    {
        Pkcs1 = 0,
        Oaep = 1
    }

    public partial struct RSAParameters
    {
        public byte[] D;
        public byte[] DP;
        public byte[] DQ;
        public byte[] Exponent;
        public byte[] InverseQ;
        public byte[] Modulus;
        public byte[] P;
        public byte[] Q;
    }

    public sealed partial class RSASignaturePadding : IEquatable<RSASignaturePadding>
    {
        internal RSASignaturePadding() { }

        public RSASignaturePaddingMode Mode { get { throw null; } }

        public static RSASignaturePadding Pkcs1 { get { throw null; } }

        public static RSASignaturePadding Pss { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(RSASignaturePadding other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(RSASignaturePadding left, RSASignaturePadding right) { throw null; }

        public static bool operator !=(RSASignaturePadding left, RSASignaturePadding right) { throw null; }

        public override string ToString() { throw null; }
    }

    public enum RSASignaturePaddingMode
    {
        Pkcs1 = 0,
        Pss = 1
    }

    public abstract partial class SHA1 : HashAlgorithm
    {
        public static SHA1 Create() { throw null; }
    }

    public abstract partial class SHA256 : HashAlgorithm
    {
        public static SHA256 Create() { throw null; }
    }

    public abstract partial class SHA384 : HashAlgorithm
    {
        public static SHA384 Create() { throw null; }
    }

    public abstract partial class SHA512 : HashAlgorithm
    {
        public static SHA512 Create() { throw null; }
    }

    public abstract partial class TripleDES : SymmetricAlgorithm
    {
        public override byte[] Key { get { throw null; } set { } }

        public override KeySizes[] LegalBlockSizes { get { throw null; } }

        public override KeySizes[] LegalKeySizes { get { throw null; } }

        public static TripleDES Create() { throw null; }

        public static bool IsWeakKey(byte[] rgbKey) { throw null; }
    }
}