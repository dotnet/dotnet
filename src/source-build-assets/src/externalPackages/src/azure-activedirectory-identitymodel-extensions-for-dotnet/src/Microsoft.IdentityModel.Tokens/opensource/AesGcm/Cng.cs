// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Logging;
using static Microsoft.IdentityModel.Tokens.Interop.BCrypt;

namespace Microsoft.IdentityModel.Tokens
{
    internal static class AesAead
    {
        public static void CheckArgumentsForNull(
            byte[] nonce,
            byte[] plaintext,
            byte[] ciphertext,
            byte[] tag)
        {
            if (nonce == null)
                throw LogHelper.LogArgumentNullException(nameof(nonce));

            if (plaintext == null)
                throw LogHelper.LogArgumentNullException(nameof(plaintext));

            if (ciphertext == null)
                throw LogHelper.LogArgumentNullException(nameof(ciphertext));

            if (tag == null)
                throw LogHelper.LogArgumentNullException(nameof(tag));
        }

        public static unsafe void Decrypt(
            SafeKeyHandle keyHandle,
            byte[] nonce,
            byte[] associatedData,
            byte[] ciphertext,
            byte[] tag,
            byte[] plaintext,
            bool clearPlaintextOnFailure)
        {
            fixed (byte* plaintextBytes = plaintext)
            fixed (byte* nonceBytes = nonce)
            fixed (byte* ciphertextBytes = ciphertext)
            fixed (byte* tagBytes = tag)
            fixed (byte* associatedDataBytes = associatedData)
            {
                BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO authInfo = BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO.Create();
                authInfo.pbNonce = nonceBytes;
                authInfo.cbNonce = nonce.Length;
                authInfo.pbTag = tagBytes;
                authInfo.cbTag = tag.Length;
                authInfo.pbAuthData = associatedDataBytes;
                if (associatedData == null)
                    authInfo.cbAuthData = 0;
                else
                    authInfo.cbAuthData = associatedData.Length;

                NTSTATUS ntStatus = Interop.BCrypt.BCryptDecrypt(
                    keyHandle,
                    ciphertextBytes,
                    ciphertext.Length,
                    new IntPtr(&authInfo),
                    null,
                    0,
                    plaintextBytes,
                    plaintext.Length,
                    out int plaintextBytesWritten,
                    0);

                Debug.Assert(ciphertext.Length == plaintextBytesWritten);

                switch (ntStatus)
                {
                    case NTSTATUS.STATUS_SUCCESS:
                        return;
                    case NTSTATUS.STATUS_AUTH_TAG_MISMATCH:
                        if (clearPlaintextOnFailure)
                            CryptographicOperations.ZeroMemory(plaintext);

                        throw LogHelper.LogExceptionMessage(new CryptographicException(LogHelper.FormatInvariant(LogMessages.IDX10714)));
                    default:
                        throw LogHelper.LogExceptionMessage(Interop.BCrypt.CreateCryptographicException(ntStatus));
                }
            }
        }

        #region FOR TESTING ONLY
        internal static unsafe void Encrypt(
            SafeKeyHandle keyHandle,
            byte[] nonce,
            byte[] associatedData,
            byte[] plaintext,
            byte[] ciphertext,
            byte[] tag)
        {
            fixed (byte* plaintextBytes = plaintext)
            fixed (byte* nonceBytes = nonce)
            fixed (byte* ciphertextBytes = ciphertext)
            fixed (byte* tagBytes = tag)
            fixed (byte* associatedDataBytes = associatedData)
            {
                BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO authInfo = BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO.Create();
                authInfo.pbNonce = nonceBytes;
                authInfo.cbNonce = nonce.Length;
                authInfo.pbTag = tagBytes;
                authInfo.cbTag = tag.Length;
                authInfo.pbAuthData = associatedDataBytes;
                if (associatedData == null)
                    authInfo.cbAuthData = 0;
                else
                    authInfo.cbAuthData = associatedData.Length;

                NTSTATUS ntStatus = Interop.BCrypt.BCryptEncrypt(
                    keyHandle,
                    plaintextBytes,
                    plaintext.Length,
                    new IntPtr(&authInfo),
                    null,
                    0,
                    ciphertextBytes,
                    ciphertext.Length,
                    out int ciphertextBytesWritten,
                    0);

                Debug.Assert(plaintext.Length == ciphertextBytesWritten);

                if (ntStatus != NTSTATUS.STATUS_SUCCESS)
                {
                    throw Interop.BCrypt.CreateCryptographicException(ntStatus);
                }
            }
        }
        #endregion
    }

    internal static class AesBCryptModes
    {
        internal static Lazy<SafeAlgorithmHandle> OpenAesAlgorithm(string cipherMode)
        {
            return new Lazy<SafeAlgorithmHandle>(() =>
            {
                SafeAlgorithmHandle hAlg = Cng.BCryptOpenAlgorithmProvider(Cng.BCRYPT_AES_ALGORITHM, null, Cng.OpenAlgorithmProviderFlags.NONE);
                hAlg.SetCipherMode(cipherMode);

                return hAlg;
            });
        }
    }

    //
    // Interop layer around Windows CNG api.
    //
    internal static class Cng
    {
        [Flags]
        public enum OpenAlgorithmProviderFlags
        {
            NONE = 0x00000000,
            BCRYPT_ALG_HANDLE_HMAC_FLAG = 0x00000008,
        }

        public const string BCRYPT_AES_ALGORITHM = "AES";

        public const string BCRYPT_CHAIN_MODE_GCM = "ChainingModeGCM";

        public static SafeAlgorithmHandle BCryptOpenAlgorithmProvider(string pszAlgId, string pszImplementation, OpenAlgorithmProviderFlags dwFlags)
        {
            SafeAlgorithmHandle hAlgorithm;
            NTSTATUS ntStatus = Interop.BCrypt.BCryptOpenAlgorithmProvider(out hAlgorithm, pszAlgId, pszImplementation, (int)dwFlags);

            if (ntStatus != NTSTATUS.STATUS_SUCCESS)
                throw LogHelper.LogExceptionMessage(CreateCryptographicException(ntStatus));

            return hAlgorithm;
        }

        public static void SetCipherMode(this SafeAlgorithmHandle hAlg, string cipherMode)
        {
            NTSTATUS ntStatus = Interop.BCrypt.BCryptSetProperty(hAlg, BCryptPropertyStrings.BCRYPT_CHAINING_MODE, cipherMode, (cipherMode.Length + 1) * 2, 0);

            if (ntStatus != NTSTATUS.STATUS_SUCCESS)
                throw LogHelper.LogExceptionMessage(CreateCryptographicException(ntStatus));
        }

        private static Exception CreateCryptographicException(NTSTATUS ntStatus)
        {
            int hr = ((int)ntStatus) | 0x01000000;
            return hr.ToCryptographicException();
        }
    }

    internal static class CryptographicOperations
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void ZeroMemory(byte[] buffer)
        {
            // NoOptimize to prevent the optimizer from deciding this call is unnecessary
            // NoInlining to prevent the inliner from forgetting that the method was no-optimize
            Array.Clear(buffer, 0, buffer.Length);
        }
    }

    internal static class CryptoThrowHelper
    {
        public static CryptographicException ToCryptographicException(this int hr)
        {
            string message = Interop.Kernel32.GetMessage(hr);

            if ((hr & 0x80000000) != 0x80000000)
                hr = (hr & 0x0000FFFF) | unchecked((int)0x80070000);

            return new WindowsCryptographicException(hr, message);
        }

        private sealed class WindowsCryptographicException : CryptographicException
        {
            public WindowsCryptographicException(int hr, string message)
                : base(message)
            {
                HResult = hr;
            }

            public WindowsCryptographicException(string message) : base(message)
            {
            }

            public WindowsCryptographicException(string message, Exception innerException) : base(message, innerException)
            {
            }

            public WindowsCryptographicException()
            {
            }
        }
    }
}
