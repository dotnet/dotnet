// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens
{
    internal partial class Interop
    {
        internal static class BCrypt
        {
            internal static Exception CreateCryptographicException(NTSTATUS ntStatus)
            {
                int hr = unchecked((int)ntStatus) | 0x01000000;
                return hr.ToCryptographicException();
            }

            internal static unsafe SafeKeyHandle BCryptImportKey(SafeAlgorithmHandle hAlg, byte[] key)
            {
                const string BCRYPT_KEY_DATA_BLOB = "KeyDataBlob";
                int keySize = key.Length;
                int blobSize = sizeof(BCRYPT_KEY_DATA_BLOB_HEADER) + keySize;
                byte[] blob = new byte[blobSize];
                fixed (byte* pbBlob = blob)
                {
                    BCRYPT_KEY_DATA_BLOB_HEADER* pBlob = (BCRYPT_KEY_DATA_BLOB_HEADER*)pbBlob;
                    pBlob->dwMagic = BCRYPT_KEY_DATA_BLOB_HEADER.BCRYPT_KEY_DATA_BLOB_MAGIC;
                    pBlob->dwVersion = BCRYPT_KEY_DATA_BLOB_HEADER.BCRYPT_KEY_DATA_BLOB_VERSION1;
                    pBlob->cbKeyData = (uint)keySize;
                }

                key.CopyTo(blob, sizeof(BCRYPT_KEY_DATA_BLOB_HEADER));
                SafeKeyHandle hKey;
                NTSTATUS ntStatus = BCryptImportKey(hAlg, IntPtr.Zero, BCRYPT_KEY_DATA_BLOB, out hKey, IntPtr.Zero, 0, blob, blobSize, 0);

                if (ntStatus != NTSTATUS.STATUS_SUCCESS)
                    throw LogHelper.LogExceptionMessage(CreateCryptographicException(ntStatus));

                return hKey;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal unsafe struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO
            {
                private int cbSize;
                private uint dwInfoVersion;
                internal byte* pbNonce;
                internal int cbNonce;
                internal byte* pbAuthData;
                internal int cbAuthData;
                internal byte* pbTag;
                internal int cbTag;
                internal byte* pbMacContext;
                internal int cbMacContext;
                internal int cbAAD;
                internal ulong cbData;
                internal uint dwFlags;

                public static BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO Create()
                {
                    BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO ret = default;

                    ret.cbSize = sizeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO);

                    const uint BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO_VERSION = 1;
                    ret.dwInfoVersion = BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO_VERSION;

                    return ret;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct BCRYPT_KEY_DATA_BLOB_HEADER
            {
                public uint dwMagic;
                public uint dwVersion;
                public uint cbKeyData;

                public const uint BCRYPT_KEY_DATA_BLOB_MAGIC = 0x4d42444b;
                public const uint BCRYPT_KEY_DATA_BLOB_VERSION1 = 0x1;
            }

            internal enum NTSTATUS : uint
            {
                STATUS_SUCCESS = 0x0,
                STATUS_NOT_FOUND = 0xc0000225,
                STATUS_INVALID_PARAMETER = 0xc000000d,
                STATUS_NO_MEMORY = 0xc0000017,
                STATUS_AUTH_TAG_MISMATCH = 0xc000a002,
            }

            internal static class BCryptPropertyStrings
            {
                internal const string BCRYPT_CHAINING_MODE = "ChainingMode";
                internal const string BCRYPT_ECC_PARAMETERS = "ECCParameters";
                internal const string BCRYPT_EFFECTIVE_KEY_LENGTH = "EffectiveKeyLength";
                internal const string BCRYPT_HASH_LENGTH = "HashDigestLength";
                internal const string BCRYPT_MESSAGE_BLOCK_LENGTH = "MessageBlockLength";
            }

#region FOR TESTING ONLY
            [DllImport(Libraries.BCrypt, CharSet = CharSet.Unicode)]
            public static extern unsafe NTSTATUS BCryptEncrypt(SafeKeyHandle hKey, byte* pbInput, int cbInput, IntPtr paddingInfo, [In, Out] byte[] pbIV, int cbIV, byte* pbOutput, int cbOutput, out int cbResult, int dwFlags);
#endregion

            [DllImport(Libraries.BCrypt, CharSet = CharSet.Unicode)]
            public static extern unsafe NTSTATUS BCryptDecrypt(SafeKeyHandle hKey, byte* pbInput, int cbInput, IntPtr paddingInfo, [In, Out] byte[] pbIV, int cbIV, byte* pbOutput, int cbOutput, out int cbResult, int dwFlags);

            [DllImport(Libraries.BCrypt, CharSet = CharSet.Unicode)]
            private static extern NTSTATUS BCryptImportKey(SafeAlgorithmHandle hAlgorithm, IntPtr hImportKey, string pszBlobType, out SafeKeyHandle hKey, IntPtr pbKeyObject, int cbKeyObject, byte[] pbInput, int cbInput, int dwFlags);

            [DllImport(Libraries.BCrypt, CharSet = CharSet.Unicode)]
            public static extern NTSTATUS BCryptOpenAlgorithmProvider(out SafeAlgorithmHandle phAlgorithm, string pszAlgId, string pszImplementation, int dwFlags);

            [DllImport(Libraries.BCrypt, CharSet = CharSet.Unicode)]
            public static extern NTSTATUS BCryptSetProperty(SafeAlgorithmHandle hObject, string pszProperty, string pbInput, int cbInput, int dwFlags);

            [DllImport(Libraries.BCrypt, CharSet = CharSet.Unicode, EntryPoint = "BCryptSetProperty")]
            private static extern NTSTATUS BCryptSetIntPropertyPrivate(SafeBCryptHandle hObject, string pszProperty, ref int pdwInput, int cbInput, int dwFlags);

            public static unsafe NTSTATUS BCryptSetIntProperty(SafeBCryptHandle hObject, string pszProperty, ref int pdwInput, int dwFlags)
            {
                return BCryptSetIntPropertyPrivate(hObject, pszProperty, ref pdwInput, sizeof(int), dwFlags);
            }
        }

        internal static class Kernel32
        {
            private const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
            private const int FORMAT_MESSAGE_FROM_HMODULE = 0x00000800;
            private const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
            private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;
            private const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
            private const int ERROR_INSUFFICIENT_BUFFER = 0x7A;

            [DllImport(Libraries.Kernel32, CharSet = CharSet.Unicode, EntryPoint = "FormatMessageW", SetLastError = true, BestFitMapping = true, ExactSpelling = true)]
            private static extern unsafe int FormatMessage(
                int dwFlags,
                IntPtr lpSource,
                uint dwMessageId,
                int dwLanguageId,
                void* lpBuffer,
                int nSize,
                IntPtr arguments);

            /// <summary>
            ///     Returns a string message for the specified Win32 error code.
            /// </summary>
            internal static string GetMessage(int errorCode) =>
                GetMessage(errorCode, IntPtr.Zero);

            internal static unsafe string GetMessage(int errorCode, IntPtr moduleHandle)
            {
                int flags = FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_ARGUMENT_ARRAY;
                if (moduleHandle != IntPtr.Zero)
                    flags |= FORMAT_MESSAGE_FROM_HMODULE;

                // First try to format the message into the stack based buffer.  Most error messages will fit.
                char[] stackBuffer = new char[256]; // arbitrary stack limit
                fixed (char* bufferPtr = stackBuffer)
                {
                    int length = FormatMessage(flags, moduleHandle, unchecked((uint)errorCode), 0, bufferPtr, stackBuffer.Length, IntPtr.Zero);
                    if (length > 0)
                        return GetAndTrimString(stackBuffer, length);
                }

                // We got back an error.  If the error indicated that there wasn't enough room to store
                // the error message, then call FormatMessage again, but this time rather than passing in
                // a buffer, have the method allocate one, which we then need to free.
                if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
                {
                    IntPtr nativeMsgPtr = default;
                    try
                    {
                        int length = FormatMessage(flags | FORMAT_MESSAGE_ALLOCATE_BUFFER, moduleHandle, unchecked((uint)errorCode), 0, &nativeMsgPtr, 0, IntPtr.Zero);
                        if (length > 0)
                            return GetAndTrimString(Marshal.PtrToStringAnsi(nativeMsgPtr).ToCharArray(), length);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(nativeMsgPtr);
                    }
                }

                // Couldn't get a message, so manufacture one.
                return string.Format("Unknown error (0x{0:x})", errorCode);
            }

            private static string GetAndTrimString(char[] buffer, int length)
            {
                while (length > 0 && buffer[length - 1] <= 32)
                    length--; // trim off spaces and non-printable ASCII chars at the end of the resource

                return new string(buffer, 0, length);
            }
        }

        internal static class Libraries
        {
            internal const string BCrypt = "BCrypt.dll";
            internal const string Kernel32 = "kernel32.dll";
        }
    }
}
