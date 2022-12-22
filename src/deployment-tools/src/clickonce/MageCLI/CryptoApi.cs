// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32.SafeHandles;
using _FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace Microsoft.Deployment.Utilities
{
    internal abstract class CAPIBase
    {
        //
        // PInvoke dll's.
        //

        internal const String ADVAPI32 = "advapi32.dll";
        internal const String CRYPT32  = "crypt32.dll";
        internal const String CRYPTUI  = "cryptui.dll";
        internal const String KERNEL32 = "kernel32.dll";

        //
        // Constants
        //

        internal const uint LMEM_FIXED    = 0x0000;
        internal const uint LMEM_ZEROINIT = 0x0040;
        internal const uint LPTR          = (LMEM_FIXED | LMEM_ZEROINIT);

        internal const uint CRYPT_MACHINE_DEFAULT = 0x00000001;

        internal const uint AT_SIGNATURE = 2;

        internal const int S_OK        = 0;
        internal const int S_FALSE     = 1;
        internal const string szOID_RSA_SHA256RSA   = "1.2.840.113549.1.1.11";

        // dwFlags definitions for CryptAcquireContext
        internal const uint CRYPT_VERIFYCONTEXT     = 0xF0000000;
        internal const uint CRYPT_NEWKEYSET         = 0x00000008;
        internal const uint CRYPT_DELETEKEYSET      = 0x00000010;
        internal const uint CRYPT_MACHINE_KEYSET    = 0x00000020;
        internal const uint CRYPT_SILENT            = 0x00000040;
        internal const uint CRYPT_USER_KEYSET       = 0x00001000;

        internal const byte CUR_BLOB_VERSION        = 2;
        internal const int NTE_BAD_KEYSET           = unchecked((int) 0x80090016); // Keyset does not exist    

        internal const int CERT_KEY_PROV_INFO_PROP_ID = 2;

        internal const string szOID_ENHANCED_KEY_USAGE        = "2.5.29.37";        
        internal const string szOID_PKIX_KP_CODE_SIGNING      = "1.3.6.1.5.5.7.3.3";

        //
        // Structures
        //

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        internal struct CRYPT_KEY_PROV_INFO
        {
            internal string pwszContainerName;
            internal string pwszProvName;
            internal uint   dwProvType;
            internal uint   dwFlags;
            internal uint   cProvParam;
            internal IntPtr rgProvParam;
            internal uint   dwKeySpec;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        internal struct CRYPTOAPI_BLOB
        {
            internal uint   cbData;
            internal IntPtr pbData;
        }
    }

    /// <summary>
    /// CAPINative - CAPI wrapper class containing only static
    /// methods to wrap native CAPI through P/Invoke.
    /// 
    /// All methods within this group require caller to have unmanaged 
    /// code permission. So, it is fine to put any method in here
    /// without worry of security breach.
    /// </summary>
    internal abstract class CAPINative : CAPIBase
    {
    }

    /// <summary>
    /// CAPISafe - CAPI wrapper class containing only static methods
    /// to wrap safe CAPI through P/Invoke.
    /// 
    /// All methods within this class will suppress unmanaged code 
    /// permission and demand NO other permission, which means it is 
    /// OK to be called by anyone.
    /// </summary>
    [SuppressUnmanagedCodeSecurityAttribute()]
    internal abstract class CAPISafe : CAPINative
    {
        [DllImport(KERNEL32, CharSet=CharSet.Auto, SetLastError=true)]
        internal static extern SafeLocalAllocHandle LocalAlloc(
            [In] uint uFlags, 
            [In] uint sizetdwBytes);

        [DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true, BestFitMapping = false)]
        internal static extern bool CryptGetProvParam(
              [In] SafeCryptProvHandle hProv,
              [In] uint dwParam,
              [MarshalAs(UnmanagedType.LPStr)] StringBuilder pbData,
              ref uint dwDataLen,
              [In] uint dwFlags);

        [DllImport(CRYPT32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CertSetCertificateContextProperty(
            IntPtr pCertContext,
            int dwPropId,
            int dwFlags,
            SafeLocalAllocHandle pvData);
    }

    /// <summary>
    /// CAPIUnsafe - CAPI wrapper class containing only static methods to wrap
    /// unsafe CAPI through P/Invoke.
    /// 
    /// All methods within this class will suppress unmanaged code permission 
    /// but should in turn demand for other appropriate permission(s).
    /// </summary>
    [SuppressUnmanagedCodeSecurityAttribute()]
    internal abstract class CAPIUnsafe : CAPISafe
    {
        [DllImport(ADVAPI32, CharSet=CharSet.Ansi, EntryPoint="CryptAcquireContextA", BestFitMapping = false, SetLastError = true)]
        internal protected extern static bool CryptAcquireContext(
            [In,Out] ref SafeCryptProvHandle                  hCryptProv,
            [In]     [MarshalAs(UnmanagedType.LPStr)] string  pszContainer,
            [In]     [MarshalAs(UnmanagedType.LPStr)] string  pszProvider,
            [In]     uint                                     dwProvType,
            [In]     uint                                     dwFlags);
    }

    /// <summary>
    /// CAPIMethods - dummy layer.
    /// </summary>
    internal abstract class CAPIMethods : CAPIUnsafe
    {
    }

    /// <summary>
    /// CAPI - This class provides overrides for above classes.
    /// </summary>
    internal sealed class CAPI : CAPIMethods
    {
#if DEBUG
        private const int ERROR_NO_MORE_ITEMS = 259;
#endif
        internal const uint PP_ENUMCONTAINERS = 2;
        internal const uint CRYPT_FIRST = 1;
        internal const uint CRYPT_NEXT = 2;

        private CAPI () {}

        new internal static SafeLocalAllocHandle LocalAlloc(uint uFlags, uint sizetdwBytes)
        {
            SafeLocalAllocHandle safeLocalAllocHandle = CAPIMethods.LocalAlloc(uFlags, sizetdwBytes);
            if (safeLocalAllocHandle == null || safeLocalAllocHandle.IsInvalid) 
                throw new OutOfMemoryException();

            return safeLocalAllocHandle;
        }

        internal static bool CryptAcquireContext(
            ref SafeCryptProvHandle hCryptProv,
            string pwszContainer,
            string pwszProvider,
            uint dwProvType,
            uint dwFlags,
            out int errorCode)
        {
            errorCode = 0;

#if !RUNTIME_TYPE_NETCORE
            CspParameters parameters = new CspParameters();
            parameters.ProviderName = pwszProvider;
            parameters.KeyContainerName = pwszContainer;
            parameters.ProviderType = (int)dwProvType;
            parameters.KeyNumber = -1;
            parameters.Flags = (CspProviderFlags)((dwFlags & CAPI.CRYPT_MACHINE_KEYSET) == CAPI.CRYPT_MACHINE_KEYSET ? CspProviderFlags.UseMachineKeyStore : 0);

            KeyContainerPermission kp = new KeyContainerPermission(KeyContainerPermissionFlags.NoFlags);
            KeyContainerPermissionAccessEntry entry = new KeyContainerPermissionAccessEntry(parameters, KeyContainerPermissionFlags.Open);
            kp.AccessEntries.Add(entry);
            kp.Demand();
#endif

            bool rc = CAPIMethods.CryptAcquireContext(ref hCryptProv,
                                                      pwszContainer,
                                                      pwszProvider,
                                                      dwProvType,
                                                      dwFlags);

            if (!rc)
            {
                errorCode = Marshal.GetLastWin32Error();
            }

            return rc;
        }

        new internal static bool CryptGetProvParam(
            SafeCryptProvHandle hProv,
            uint dwParam,
            StringBuilder pbData,
            ref uint dwDataLen,
            uint dwFlags)
        {
            bool result = CAPIMethods.CryptGetProvParam(hProv, dwParam, pbData, ref dwDataLen, dwFlags);
#if DEBUG
            if (!result)
            {
                int errorCode = Marshal.GetLastWin32Error();
                if (errorCode != 0 && errorCode != CAPI.ERROR_NO_MORE_ITEMS)
                {
                    System.Diagnostics.Debug.WriteLine("Error " + errorCode);
                }
            }
#endif
            return result;
        }

        internal static bool CertSetKeyProviderInfoProperty(IntPtr pCert, SafeLocalAllocHandle handle)
        {
            if (pCert == IntPtr.Zero)
                throw new ArgumentNullException(nameof(pCert));

            if (handle.IsInvalid)
                throw new ArgumentException(nameof(handle));

#if !RUNTIME_TYPE_NETCORE
            new PermissionSet(PermissionState.Unrestricted).Demand();
#endif

            return CertSetCertificateContextProperty(pCert, CAPI.CERT_KEY_PROV_INFO_PROP_ID, 0, handle);
        }
    }

    /// <summary>
    ///  This class exports functions that access CNG key storage providers.
    /// </summary>
    internal sealed class NCryptMethods
    {
        internal const string NCrypt = "ncrypt.dll";

        internal const int NTE_NO_MORE_ITEMS = unchecked((int)0x8009002A);
        internal const int NTE_BAD_FLAGS = unchecked((int)0x80090009);
        internal const int SCARD_E_NO_READERS_AVAILABLE = unchecked((int)0x8010002E);
        internal const int SCARD_W_CANCELLED_BY_USER = unchecked((int)0x8010006E);
        internal const int NCRYPT_MACHINE_KEY_FLAG = (int)0x00000020;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal unsafe struct NCryptProviderName
        {
            private char* pszName;
            private char* pszComment;
            internal string Name => new string(this.pszName);
            internal string Comment => new string(this.pszComment);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal unsafe struct NCryptKeyName
        {
            private char* pszName;
            private char* pszAlgid;
            internal int dwLegacyKeySpec;
            internal int dwFlags;
            internal string Name => new string(this.pszName);
            internal string Algid => new string(this.pszAlgid);
        };

        [DllImport(NCrypt, CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        internal static extern unsafe int NCryptEnumStorageProviders(
            [Out] out int pdwProviderCount,
            [Out] out NCryptProviderName* ppProviderList,
            [In] int dwFlags = 0); // Display the UI if needed.

        [DllImport(NCrypt, CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        internal static extern int NCryptOpenStorageProvider(
            [Out] out SafeNCryptProviderHandle phProvider,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszProviderName,
            [In] int dwFlags = 0); // no flags are defined for this function

        [DllImport(nameof(NCrypt), CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        public static extern unsafe int NCryptEnumKeys(
           [In] SafeNCryptProviderHandle hProvider,
           [In] string pszScope,
           [Out] out NCryptKeyName* ppKeyName,
           [In, Out] ref void* ppEnumState,
           [In] int dwFlags = 0);

        [DllImport(NCrypt, ExactSpelling = true)]
        internal static extern unsafe int NCryptFreeBuffer(
            [In] void* pvInput);

    }

    internal sealed class SafeLocalAllocHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeLocalAllocHandle () : base(true) {}

        // 0 is an Invalid Handle
        internal SafeLocalAllocHandle (IntPtr handle) : base (true)
        {
            SetHandle(handle);
        }

        internal static SafeLocalAllocHandle InvalidHandle
        {
            get { return new SafeLocalAllocHandle(IntPtr.Zero); }
        }

        [DllImport(CAPI.KERNEL32, SetLastError=true),
         SuppressUnmanagedCodeSecurity
#if !RUNTIME_TYPE_NETCORE
         , ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)
#endif
        ]
        private static extern int LocalFree(IntPtr handle);

        override protected bool ReleaseHandle()
        {
            return LocalFree(handle) == 0;
        }
    }

    internal sealed class SafeCryptProvHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeCryptProvHandle() : base (true) {}

        // 0 is an Invalid Handle
        internal SafeCryptProvHandle(IntPtr handle) : base (true) {
            SetHandle(handle);
        }

        internal static SafeCryptProvHandle InvalidHandle {
            get { return new SafeCryptProvHandle(IntPtr.Zero); }
        }

        [DllImport(CAPI.ADVAPI32, SetLastError=true),
         SuppressUnmanagedCodeSecurity
#if !RUNTIME_TYPE_NETCORE
         , ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)
#endif
        ]
        private static extern bool CryptReleaseContext(IntPtr hCryptProv, uint dwFlags); 

        override protected bool ReleaseHandle()
        {
            return CryptReleaseContext(handle, 0);
        }
    }
}
