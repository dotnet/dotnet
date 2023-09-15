// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// Windows Authenticode signature verifier.
    /// </summary>
    public sealed class WindowsEmbeddedSignatureVerifier : EmbeddedSignatureVerifier
    {
        [DllImport("wintrust.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int WinVerifyTrust([In] IntPtr hwnd,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid pgActionID,
            [In] WINTRUST_DATA pWVTData);

        /// <summary>
        /// Checks if a file has a valid Authenticode signature.
        /// </summary>
        /// <param name="filePath">The path of a file to be checked.</param>
        /// <returns><see langword="true" /> if the file has a valid signature; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="filePath" />
        /// is either <see langword="null" /> or an empty string.</exception>
        public override bool IsValid(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(Strings.ArgumentCannotBeNullOrEmpty, nameof(filePath));
            }

            var WINTRUST_ACTION_GENERIC_VERIFY_V2 = new Guid("{00AAC56B-CD44-11D0-8CC2-00C04FC295EE}");

            using (var pFilePath = new SafeCoTaskMem(filePath))
            {
                var fileInfo = new WINTRUST_FILE_INFO() { pcwszFilePath = pFilePath.DangerousGetHandle() };

                using (var pFile = new SafeCoTaskMem((int)fileInfo.cbStruct))
                {
                    Marshal.StructureToPtr(fileInfo, pFile.DangerousGetHandle(), fDeleteOld: false);

                    var data = new WINTRUST_DATA() { pFile = pFile.DangerousGetHandle() };

                    return WinVerifyTrust(IntPtr.Zero, WINTRUST_ACTION_GENERIC_VERIFY_V2, data) == 0;
                }
            }
        }

        private sealed class SafeCoTaskMem : SafeHandle
        {
            private SafeCoTaskMem() : base(IntPtr.Zero, ownsHandle: true)
            {
            }

            internal SafeCoTaskMem(int cbSize) : this()
            {
                handle = Marshal.AllocCoTaskMem(cbSize);
            }

            internal SafeCoTaskMem(string value) : this()
            {
                handle = Marshal.StringToCoTaskMemUni(value);
            }

            public override bool IsInvalid
            {
                get { return handle == IntPtr.Zero || handle == IntPtr.Zero; }
            }

            protected override bool ReleaseHandle()
            {
                if (!IsInvalid)
                {
                    Marshal.FreeCoTaskMem(handle);
                }

                return true;
            }
        }

        private enum UIChoice : uint
        {
            WTD_UI_ALL = 1,
            WTD_UI_NONE = 2,
            WTD_UI_NOBAD = 3,
            WTD_UI_NOGOOD = 4
        }

        private enum RevocationChecks : uint
        {
            WTD_REVOKE_NONE = 0,
            WTD_REVOKE_WHOLECHAIN = 1
        }

        private enum UnionChoice : uint
        {
            WTD_CHOICE_FILE = 1,
            WTD_CHOICE_CATALOG = 2,
            WTD_CHOICE_BLOB = 3,
            WTD_CHOICE_SIGNER = 4,
            WTD_CHOICE_CERT = 5
        }

        private enum StateAction : uint
        {
            WTD_STATEACTION_IGNORE = 0,
            WTD_STATEACTION_VERIFY = 1,
            WTD_STATEACTION_CLOSE = 2,
            WTD_STATEACTION_AUTO_CACHE = 3,
            WTD_STATEACTION_AUTO_CACHE_FLUSH = 4
        }

        [Flags]
        private enum ProviderFlags : uint
        {
            WTD_USE_IE4_TRUST_FLAG = 1,
            WTD_NO_IE4_CHAIN_FLAG = 2,
            WTD_NO_POLICY_USAGE_FLAG = 4,
            WTD_REVOCATION_CHECK_NONE = 16,
            WTD_REVOCATION_CHECK_END_CERT = 32,
            WTD_REVOCATION_CHECK_CHAIN = 64,
            WTD_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT = 128,
            WTD_SAFER_FLAG = 256,
            WTD_HASH_ONLY_FLAG = 512,
            WTD_USE_DEFAULT_OSVER_CHECK = 1024,
            WTD_LIFETIME_SIGNING_FLAG = 2048,
            WTD_CACHE_ONLY_URL_RETRIEVAL = 4096,
            WTD_DISABLE_MD2_MD4 = 8192,
            WTD_MOTW = 16384
        }

        [Flags]
        private enum UIContext : uint
        {
            WTD_UICONTEXT_EXECUTE = 0,
            WTD_UICONTEXT_INSTALL = 1
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private sealed class WINTRUST_FILE_INFO
        {
#if IS_DESKTOP
            internal uint cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_FILE_INFO));
#else
            internal uint cbStruct = (uint)Marshal.SizeOf<WINTRUST_FILE_INFO>();
#endif
            internal IntPtr pcwszFilePath;
            internal IntPtr hFile;
            internal IntPtr pgKnownSubject;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private sealed class WINTRUST_DATA
        {
#if IS_DESKTOP
            internal uint cbStruct = (uint)Marshal.SizeOf(typeof(WINTRUST_DATA));
#else
            internal uint cbStruct = (uint)Marshal.SizeOf<WINTRUST_DATA>();
#endif
            internal IntPtr pPolicyCallbackData = IntPtr.Zero;
            internal IntPtr pSIPClientData = IntPtr.Zero;
            internal UIChoice dwUIChoice = UIChoice.WTD_UI_NONE;
            internal RevocationChecks fdwRevocationChecks = RevocationChecks.WTD_REVOKE_NONE;
            internal UnionChoice dwUnionChoice = UnionChoice.WTD_CHOICE_FILE;
            internal IntPtr pFile;
            internal StateAction dwStateAction = StateAction.WTD_STATEACTION_IGNORE;
            internal IntPtr hWVTStateData = IntPtr.Zero;
            internal string pwszURLReference = null;
            internal ProviderFlags dwProvFlags = ProviderFlags.WTD_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT
                | ProviderFlags.WTD_DISABLE_MD2_MD4;
            internal UIContext dwUIContext = UIContext.WTD_UICONTEXT_EXECUTE;
        }
    }
}
