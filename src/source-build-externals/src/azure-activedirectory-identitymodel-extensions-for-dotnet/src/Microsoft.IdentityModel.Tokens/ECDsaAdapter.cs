// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    delegate ECDsa CreateECDsaDelegate(JsonWebKey jsonWebKey, bool usePrivateKey);

    /// <summary>
    /// This adapter abstracts the <see cref="ECDsa"/> differences between versions of .Net targets.
    /// </summary>
    internal class ECDsaAdapter
    {
        internal readonly CreateECDsaDelegate CreateECDsaFunction = ECDsaNotSupported;
        internal static ECDsaAdapter Instance = new ECDsaAdapter();

        /// <summary>
        /// Initializes a new instance of the <see cref="ECDsaAdapter"/> class.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">
        /// <see cref="ECDsa"/> creation is not supported by some platforms.
        /// For more details, see https://aka.ms/IdentityModel/create-ecdsa.
        /// </exception>
        internal ECDsaAdapter()
        {
#if NET472 || NET6_0_OR_GREATER
            CreateECDsaFunction = CreateECDsaUsingECParams;
#elif NETSTANDARD2_0
            // Although NETSTANDARD2_0 specifies that ECParameters are supported, we still need to call SupportsECParameters()
            // as NET462 is listed as supporting NETSTANDARD2_0, but DOES NOT support ECParameters.
            // See: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecparameters?view=netstandard-2.0
            if (SupportsECParameters()) CreateECDsaFunction = CreateECDsaUsingECParams;
            else CreateECDsaFunction = CreateECDsaUsingCNGKey;
#else
        CreateECDsaFunction = CreateECDsaUsingCNGKey;
#endif
        }

        /// <summary>
        /// Creates an ECDsa object using the <paramref name="jsonWebKey"/> and <paramref name="usePrivateKey"/>.
        /// </summary>
        internal ECDsa CreateECDsa(JsonWebKey jsonWebKey, bool usePrivateKey)
        {
            return CreateECDsaFunction(jsonWebKey, usePrivateKey);
        }

#if NET462 || NETSTANDARD2_0
        /// <summary>
        /// Creates an ECDsa object using the <paramref name="jsonWebKey"/> and <paramref name="usePrivateKey"/>.
        /// 'ECParameters' structure is available in .NET Framework 4.7+, .NET Standard 1.6+, and .NET Core 1.0+.
        /// This method is supported only on Windows as other platforms don't support operations with <see cref="CngKey"/>.
        /// </summary>
        private ECDsa CreateECDsaUsingCNGKey(JsonWebKey jsonWebKey, bool usePrivateKey)
        {
            if (jsonWebKey == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey));

            if (jsonWebKey.Crv == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.Crv));

            if (jsonWebKey.X == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.X));

            if (jsonWebKey.Y == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.Y));

            GCHandle keyBlobHandle = new GCHandle();
            try
            {
                uint dwMagic = GetMagicValue(jsonWebKey.Crv, usePrivateKey);
                uint cbKey = GetKeyByteCount(jsonWebKey.Crv);
                byte[] keyBlob;

                if (usePrivateKey)
                    keyBlob = new byte[3 * cbKey + 2 * Marshal.SizeOf<uint>()];
                else
                    keyBlob = new byte[2 * cbKey + 2 * Marshal.SizeOf<uint>()];

                keyBlobHandle = GCHandle.Alloc(keyBlob, GCHandleType.Pinned);
                IntPtr keyBlobPtr = keyBlobHandle.AddrOfPinnedObject();

                byte[] x = Base64UrlEncoder.DecodeBytes(jsonWebKey.X);
                if (x.Length > cbKey)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(jsonWebKey), LogHelper.FormatInvariant(LogMessages.IDX10675, LogHelper.MarkAsNonPII(nameof(jsonWebKey)), LogHelper.MarkAsNonPII(nameof(jsonWebKey.X)), cbKey, LogHelper.MarkAsNonPII(x.Length))));

                byte[] y = Base64UrlEncoder.DecodeBytes(jsonWebKey.Y);
                if (y.Length > cbKey)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(jsonWebKey), LogHelper.FormatInvariant(LogMessages.IDX10675, LogHelper.MarkAsNonPII(nameof(jsonWebKey)), LogHelper.MarkAsNonPII(nameof(jsonWebKey.Y)), cbKey, LogHelper.MarkAsNonPII(y.Length))));

                Marshal.WriteInt64(keyBlobPtr, 0, dwMagic);
                Marshal.WriteInt64(keyBlobPtr, 4, cbKey);

                int index = 8;
                foreach (byte b in x)
                    Marshal.WriteByte(keyBlobPtr, index++, b);

                foreach (byte b in y)
                    Marshal.WriteByte(keyBlobPtr, index++, b);

                if (usePrivateKey)
                {
                    if (jsonWebKey.D == null)
                        throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.D));

                    byte[] d = Base64UrlEncoder.DecodeBytes(jsonWebKey.D);
                    if (d.Length > cbKey)
                        throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(jsonWebKey), LogHelper.FormatInvariant(LogMessages.IDX10675, LogHelper.MarkAsNonPII(nameof(jsonWebKey)), LogHelper.MarkAsNonPII(nameof(jsonWebKey.D)), cbKey, LogHelper.MarkAsNonPII(d.Length))));

                    foreach (byte b in d)
                        Marshal.WriteByte(keyBlobPtr, index++, b);

                    Marshal.Copy(keyBlobPtr, keyBlob, 0, keyBlob.Length);
                    using (CngKey cngKey = CngKey.Import(keyBlob, CngKeyBlobFormat.EccPrivateBlob))
                    {
                        return new ECDsaCng(cngKey);
                    }
                }
                else
                {
                    Marshal.Copy(keyBlobPtr, keyBlob, 0, keyBlob.Length);
                    using (CngKey cngKey = CngKey.Import(keyBlob, CngKeyBlobFormat.EccPublicBlob))
                    {
                        return new ECDsaCng(cngKey);
                    }
                }
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new CryptographicException(LogMessages.IDX10689, ex));
            }
            finally
            {
                if (keyBlobHandle != null && keyBlobHandle.IsAllocated)
                    keyBlobHandle.Free();
            }
        }
#endif

        internal static ECDsa ECDsaNotSupported(JsonWebKey jsonWebKey, bool usePrivateKey)
        {
            // we will get here on platforms that are not supported.
            throw LogHelper.LogExceptionMessage(new PlatformNotSupportedException(LogMessages.IDX10690));
        }

        /// <summary>
        /// Returns the size of key in bytes
        /// </summary>
        /// <param name="curveId">Represents ecdsa curve -P256, P384, P521</param>
        /// <returns>Size of the key in bytes</returns>
        private static uint GetKeyByteCount(string curveId)
        {
            if (string.IsNullOrEmpty(curveId))
                throw LogHelper.LogArgumentNullException(nameof(curveId));

            uint keyByteCount;
            switch (curveId)
            {
                case JsonWebKeyECTypes.P256:
                    keyByteCount = 32;
                    break;
                case JsonWebKeyECTypes.P384:
                    keyByteCount = 48;
                    break;
                case JsonWebKeyECTypes.P512: // treat 512 as 521. 512 doesn't exist, but we released with "512" instead of "521", so don't break now.
                case JsonWebKeyECTypes.P521:
                    keyByteCount = 66;
                    break;
                default:
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10645, LogHelper.MarkAsNonPII(curveId))));
            }
            return keyByteCount;
        }

        /// <summary>
        /// Magic numbers identifying ECDSA blob types
        /// </summary>
        private enum KeyBlobMagicNumber : uint
        {
            BCRYPT_ECDSA_PUBLIC_P256_MAGIC = 0x31534345,
            BCRYPT_ECDSA_PUBLIC_P384_MAGIC = 0x33534345,
            BCRYPT_ECDSA_PUBLIC_P521_MAGIC = 0x35534345,
            BCRYPT_ECDSA_PRIVATE_P256_MAGIC = 0x32534345,
            BCRYPT_ECDSA_PRIVATE_P384_MAGIC = 0x34534345,
            BCRYPT_ECDSA_PRIVATE_P521_MAGIC = 0x36534345,
        }

        /// <summary>
        /// Returns the magic value representing the curve corresponding to the curve id.
        /// </summary>
        /// <param name="curveId">Represents ecdsa curve -P256, P384, P512</param>
        /// <param name="willCreateSignatures">Whether the provider will create signatures or not</param>
        /// <returns>Uint representing the magic number</returns>
        private static uint GetMagicValue(string curveId, bool willCreateSignatures)
        {
            if (string.IsNullOrEmpty(curveId))
                throw LogHelper.LogArgumentNullException(nameof(curveId));

            KeyBlobMagicNumber magicNumber;
            switch (curveId)
            {
                case JsonWebKeyECTypes.P256:
                    if (willCreateSignatures)
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PRIVATE_P256_MAGIC;
                    else
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PUBLIC_P256_MAGIC;
                    break;
                case JsonWebKeyECTypes.P384:
                    if (willCreateSignatures)
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PRIVATE_P384_MAGIC;
                    else
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PUBLIC_P384_MAGIC;
                    break;
                case JsonWebKeyECTypes.P512: // treat 512 as 521. 512 doesn't exist, but we released with "512" instead of "521", so don't break now.
                case JsonWebKeyECTypes.P521:
                    if (willCreateSignatures)
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PRIVATE_P521_MAGIC;
                    else
                        magicNumber = KeyBlobMagicNumber.BCRYPT_ECDSA_PUBLIC_P521_MAGIC;
                    break;
                default:
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10645, LogHelper.MarkAsNonPII(curveId))));
            }
            return (uint)magicNumber;
        }

#if NET472 || NETSTANDARD2_0 || NET6_0_OR_GREATER
        /// <summary>
        /// Creates an ECDsa object using the <paramref name="jsonWebKey"/> and <paramref name="usePrivateKey"/>.
        /// 'ECParameters' structure is available in .NET Framework 4.7+, .NET Standard 1.6+, and .NET Core 1.0+.
        /// </summary>
        private static ECDsa CreateECDsaUsingECParams(JsonWebKey jsonWebKey, bool usePrivateKey)
        {
            if (jsonWebKey == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey));

            if (jsonWebKey.Crv == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.Crv));

            if (jsonWebKey.X == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.X));

            if (jsonWebKey.Y == null)
                throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.Y));

            try
            {
                var ecParams = new ECParameters
                {
                    Curve = GetNamedECCurve(jsonWebKey.Crv),
                    Q = { X = Base64UrlEncoder.DecodeBytes(jsonWebKey.X), Y = Base64UrlEncoder.DecodeBytes(jsonWebKey.Y) }
                };

                if (usePrivateKey)
                {
                    if (jsonWebKey.D == null)
                        throw LogHelper.LogArgumentNullException(nameof(jsonWebKey.D));

                    ecParams.D = Base64UrlEncoder.DecodeBytes(jsonWebKey.D);
                }

                return ECDsa.Create(ecParams);
            }
            catch (Exception ex)
            {
                throw LogHelper.LogExceptionMessage(new CryptographicException(LogMessages.IDX10689, ex));
            }
        }

        /// <summary>
        /// Returns the elliptic curve corresponding to the curve id.
        /// </summary>
        /// <param name="curveId">Represents ecdsa curve -P256, P384, P512</param>
        private static ECCurve GetNamedECCurve(string curveId)
        {
            if (string.IsNullOrEmpty(curveId))
                throw LogHelper.LogArgumentNullException(nameof(curveId));

            switch (curveId)
            {
                case JsonWebKeyECTypes.P256:
                    return ECCurve.NamedCurves.nistP256;
                case JsonWebKeyECTypes.P384:
                    return ECCurve.NamedCurves.nistP384;
                case JsonWebKeyECTypes.P512: // treat 512 as 521. 512 doesn't exist, but we released with "512" instead of "521", so don't break now.
                case JsonWebKeyECTypes.P521:
                    return ECCurve.NamedCurves.nistP521;
                default:
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10645, LogHelper.MarkAsNonPII(curveId))));
            }
        }

        internal static string GetCrvParameterValue(ECCurve curve)
        {
            if (curve.Oid == null)
                throw LogHelper.LogArgumentNullException(nameof(curve.Oid));

            if (string.Equals(curve.Oid.Value, ECCurve.NamedCurves.nistP256.Oid.Value) || string.Equals(curve.Oid.FriendlyName, ECCurve.NamedCurves.nistP256.Oid.FriendlyName))
                return JsonWebKeyECTypes.P256;
            else if (string.Equals(curve.Oid.Value, ECCurve.NamedCurves.nistP384.Oid.Value) || string.Equals(curve.Oid.FriendlyName, ECCurve.NamedCurves.nistP384.Oid.FriendlyName))
                return JsonWebKeyECTypes.P384;
            else if (string.Equals(curve.Oid.Value, ECCurve.NamedCurves.nistP521.Oid.Value) || string.Equals(curve.Oid.FriendlyName, ECCurve.NamedCurves.nistP521.Oid.FriendlyName))
                return JsonWebKeyECTypes.P521;
            else
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10645, (curve.Oid.Value ?? curve.Oid.FriendlyName) ?? "null")));
        }
            

        /// <summary>
        /// Tests if user application's runtime supports <see cref="ECParameters"/> structure.
        /// </summary>
        /// <returns>True if <see cref="ECParameters"/> structure is supported, false otherwise.</returns>
        internal static bool SupportsECParameters()
        {
#if NET472 || NET6_0_OR_GREATER
            return true;
#else
            try
            {
                LoadECParametersType();
                return true;
            }
            catch
            {
                return false;
            }
#endif
        }

        /// <summary>
        /// Throws <see cref="TypeLoadException"/> during runtime if user application's runtime doesn't support <see cref="ECParameters"/> structure.
        /// </summary>
#pragma warning disable CS0168 //the variable is declared but never used
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static void LoadECParametersType()
        {
            ECParameters _;
        }
#pragma warning restore CS0168 //the variable is declared but never used
#endif
    }
}
