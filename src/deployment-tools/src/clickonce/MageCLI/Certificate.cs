// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
#if RUNTIME_TYPE_NETCORE
using System.Runtime.Versioning;
#endif

namespace Microsoft.Deployment.Utilities
{
    /// <summary>
    /// Common certificate operations
    /// </summary>
    internal class Certificate
    {
        private static string szOID_KEY_USAGE = "2.5.29.15";
        private static string szOID_ENHANCED_KEY_USAGE = "2.5.29.37";
        private static string szOID_CODE_SIGNING = "1.3.6.1.5.5.7.3.3";
        internal const int    ERROR_NO_MORE_ITEMS = 259;

        // CSP types, applicable to CAPI providers
        public const int PROV_RSA_FULL = 1;
        public const int PROV_RSA_AES = 24;
        public const int PROV_RSA_SIG = 2;
        public const int PROV_RSA_SCHANNEL = 12;
        public const int PROV_UNINITIALIZED = -1;

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptEnumProviders(
           uint          dwIndex,
           IntPtr        pdwReserved,
           uint          dwFlags,
           ref uint      pdwProvType,
           StringBuilder pszProvName,
           ref uint      pcbProvName);

        internal static int CryptEnumProviders(
           uint dwIndex,
           ref uint pdwProvType,
           out StringBuilder pszProvName)
        {
            uint cbName = 0;
            pszProvName = null;

            if (!CryptEnumProviders(dwIndex, IntPtr.Zero, 0, ref pdwProvType, null, ref cbName))
            {
                return Marshal.GetLastWin32Error();
            }

            pszProvName = new StringBuilder(checked((int)cbName));
            if (!CryptEnumProviders(dwIndex, IntPtr.Zero, 0, ref pdwProvType, pszProvName, ref cbName))
            {
                return Marshal.GetLastWin32Error();
            }

            return 0;
        }

        /// <summary>
        /// Enumerates all CAPI RSA crypto providers and retrieve their names and types.
        /// </summary>
        /// <returns>ProviderAndType enumerator</returns>
        internal static IEnumerable<ProviderAndType> CryptEnumRsaProviders()
        {
            uint dwType = 0;

            for (uint dwIndex = 0; ; dwIndex++)
            {
                int errorCode = CryptEnumProviders(dwIndex, ref dwType, out StringBuilder pszName);
                if (0 == errorCode)
                {
                    if (dwType == Certificate.PROV_RSA_FULL || dwType == Certificate.PROV_RSA_AES ||
                        dwType == Certificate.PROV_RSA_SIG || dwType == Certificate.PROV_RSA_SCHANNEL)
                    {
                        yield return new ProviderAndType(pszName, dwType);
                    }
                }
                else if (ERROR_NO_MORE_ITEMS == errorCode)
                {
                    break;
                }
                // for other errors, try to access the next provider
            }
        }

        /// <summary>
        /// Finds provider type. Zero corresponds to CNG key storage providers. 
        /// </summary>
        /// <param name="providerName">Provider name</param>
        /// <returns>Provider type</returns>
        internal static int GetCspType(string providerName)
        {
            foreach (ProviderAndType info in Certificate.CryptEnumRsaProviders())
            {
                if (string.Equals(info.Name, providerName, StringComparison.Ordinal))
                {
                    return info.Type;
                }
            }

            if (0 == NCryptMethods.NCryptOpenStorageProvider(out SafeNCryptProviderHandle hProvider, providerName))
            {
                hProvider.Dispose();
                return 0;
            }

            return PROV_UNINITIALIZED;
        }

        /// <summary>
        /// Determine whether a cert can be used for code signing.
        /// </summary>
        /// <param name="cert">Certificate</param>
        /// <returns>True or false</returns>
        public static bool CanSignWith(X509Certificate2 cert)
        {
            // Check key usages to make sure it is good for signing.
            bool bCodeSigningEnabled = true;

            if (cert.Extensions.Count > 0)
            {
                foreach (X509Extension extension in cert.Extensions)
                {
                    bool? bCodeSignEnhancedKeyPresent = null;

                    if (String.Compare(extension.Oid.Value, szOID_KEY_USAGE, true, CultureInfo.InvariantCulture) == 0)
                    {
                        X509KeyUsageExtension keyUsage = new X509KeyUsageExtension();
                        keyUsage.CopyFrom(extension);
                        if ((keyUsage.KeyUsages & X509KeyUsageFlags.DigitalSignature) == 0)
                        {
                            bCodeSigningEnabled = false;
                        }
                        else
                        {
                            // Check if enhanced code-signing key is present.
                            foreach (X509Extension codesignextension in cert.Extensions)
                            {
                                bCodeSignEnhancedKeyPresent = CodeSignEnhancedKeyPresent(codesignextension);

                                if (bCodeSignEnhancedKeyPresent.HasValue)
                                {
                                    bCodeSigningEnabled = bCodeSignEnhancedKeyPresent.Value;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    else
                    {
                        bCodeSignEnhancedKeyPresent = CodeSignEnhancedKeyPresent(extension);
                        if (bCodeSignEnhancedKeyPresent.HasValue && bCodeSignEnhancedKeyPresent.Value == false)
                        {
                            bCodeSigningEnabled = false;
                        }
                    }
                }
            }

            return bCodeSigningEnabled;
        }

        /// <summary>
        /// Looks for the szOID_ENHANCED_KEY_USAGE in an extension
        /// </summary>
        /// <param name="extension">X509Extension to be used</param>
        /// <returns>
        /// null, if no szOID_ENHANCED_KEY_USAGE is found
        /// false, if szOID_ENHANCED_KEY_USAGE is found but no szOID_CODE_SIGNING usage
        /// trye, if szOID_ENHANCED_KEY_USAGE is found and szOID_CODE_SIGNING usage
        /// </returns>
        private static bool? CodeSignEnhancedKeyPresent(X509Extension extension)
        {
            // null value means that no enhanced key was found
            bool? bResult = null;

            if (String.Compare(extension.Oid.Value, szOID_ENHANCED_KEY_USAGE, true, CultureInfo.InvariantCulture) == 0)
            {
                if (extension is X509EnhancedKeyUsageExtension enhancedextension)
                {
                    foreach (Oid enhancedOid in enhancedextension.EnhancedKeyUsages)
                    {
                        if (String.Compare(enhancedOid.Value, szOID_CODE_SIGNING, true, CultureInfo.InvariantCulture) == 0)
                        {
                            bResult = true;
                            break;
                        }
                    }
                }

                // Enhanced Key usage found but no CODE_SIGN usage.
                if (!bResult.HasValue)
                {
                    bResult = false;
                }
            }
            return bResult;
        }

        /// <summary>
        /// Verifies if a certificate has an RSA private key and if the user can read it.
        /// </summary>
        /// <param name="cert">X509Certificate2 to be used </param>
        /// <returns></returns>
        public static bool HasPrivateKey(X509Certificate2 cert)
        {
            using (RSA privateKey = cert.GetRSAPrivateKey())
            {
                return (privateKey != null);
            }
        }

        /// <summary>
        /// If the certificate has private key, this cert can be used for signing, if it does not, 
        /// try to access private key stored in the CSP, if CSP provider or key container name 
        /// is not provided, certificate can't be used for signing.
        ///
        /// In .NET 5+, CspParameters type is only available on Windows - this method cannot be used
        /// on other platforms.
        /// Signing is only supported on Windows, anyway, due to limitations in signing code in MSBuild.
        /// </summary>
        /// <param name="certificate">Certificate</param>
        /// <param name="cryptoProviderName">Crypto provider name</param>
        /// <param name="keyContainerName">Key container name</param>
        /// <param name="providerType">Provider type</param>
        /// <returns></returns>
#if RUNTIME_TYPE_NETCORE
        [SupportedOSPlatform("windows")]
#endif
        public static bool SetPrivateKeyIfNeeded(X509Certificate2 certificate, string cryptoProviderName, string keyContainerName, int providerType = -1)
        {
            if (Certificate.HasPrivateKey(certificate))
            {
                // We got a .pfx file.
                // Silently ignore CSP name or key container if either was provided with a .pfx fle.
                return true;
            }

            // If provider and key container names are specified, try to access private key. 
            if (cryptoProviderName != null && keyContainerName != null)
            {
                try
                {
                    bool result = true;

                    if (providerType == PROV_UNINITIALIZED)
                    {
                        providerType = Certificate.GetCspType(cryptoProviderName);
                    }

                    if (providerType == PROV_UNINITIALIZED)
                    {
                        return false;
                    }

                    // The following code will modify state of certificates from x509Store, 
                    // make sure to assign private keys only to certificates that were created from a .cer files
                    if (providerType != 0)
                    {
                        CspParameters parameters;
                        parameters = new CspParameters
                        {
                            ProviderName = cryptoProviderName,
                            ProviderType = providerType,
                            KeyContainerName = keyContainerName,
                            KeyNumber = (int)KeyNumber.Signature,
                            // Make sure key creation is not attempted.
                            Flags = CspProviderFlags.UseExistingKey
                        };

                        if (IsMachineCryptKey(cryptoProviderName, (uint)providerType, keyContainerName))
                        {
                            // Search only Machine key store for this private key, if not set we search only user store.
                            parameters.Flags |= CspProviderFlags.UseMachineKeyStore;
                        }

#pragma warning disable SYSLIB0028
                        certificate.PrivateKey = new RSACryptoServiceProvider(parameters);
#pragma warning restore SYSLIB0028
                    }
                    else
                    {
                        CAPI.CRYPT_KEY_PROV_INFO keyProvInfo = new CAPI.CRYPT_KEY_PROV_INFO
                        {
                            pwszProvName = cryptoProviderName,
                            pwszContainerName = keyContainerName,
                            dwProvType = (uint)providerType,
                            dwKeySpec = (int)KeyNumber.Signature,
                            dwFlags = IsMachineNCryptKey(cryptoProviderName, keyContainerName) ? NCryptMethods.NCRYPT_MACHINE_KEY_FLAG : (uint)0
                        };

                        using (SafeLocalAllocHandle pKeyProvInfo = CAPI.LocalAlloc(CAPI.LPTR, (uint)Marshal.SizeOf(typeof(CAPI.CRYPT_KEY_PROV_INFO))))
                        {
                            Marshal.StructureToPtr(keyProvInfo, pKeyProvInfo.DangerousGetHandle(), false);
                            result = CAPI.CertSetKeyProviderInfoProperty(certificate.Handle, pKeyProvInfo);
                            Marshal.DestroyStructure(pKeyProvInfo.DangerousGetHandle(), typeof(CAPI.CRYPT_KEY_PROV_INFO));
                        }
                    }

                    return result;
                }
                catch (Exception e)
                {
                    if (Misc.IsCriticalException(e))
                    {
                        throw;
                    }
                    // Caller displays an error message.         
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if key exists in Cng provider.
        /// </summary>
        /// <param name="providerName">Provider name</param>
        /// <param name="keyContainer">Key container</param>
        /// <returns>True or false</returns>
        internal static bool IsMachineNCryptKey(string providerName, string keyContainer)
        {
            if (0 == NCryptMethods.NCryptOpenStorageProvider(out SafeNCryptProviderHandle hProvider, providerName))
            {
                using (hProvider)
                {
                    List<string> containers = new List<string>();
                    GetContainersFromCngProvider(containers, hProvider, true);
                    return containers.Contains(keyContainer);
                }
            }
            // default to user store
            return false;
        }

        /// <summary>
        /// Checks if key exists in Capi provider.
        /// </summary>
        /// <param name="providerName">Provider name</param>
        /// <param name="type">Type</param>
        /// <param name="keyContainer">Key container</param>
        /// <returns>True or false</returns>
        internal static bool IsMachineCryptKey(string providerName, uint type, string keyContainer)
        {
            foreach (string name in GetContainersFromCapiProvider(providerName, type, true))
            {
                if (string.Equals(name, keyContainer, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            // default to user store
            return false;
        }

        /// <summary>
        /// Gets all key containers from Capi provider.
        /// </summary>
        /// <param name="providerName">Provider name</param>
        /// <param name="type">Type</param>
        /// <param name="getMachineKeys">Indicates whether to obtain machine keys</param>
        /// <returns>Enumerator of key strings</returns>
        internal static IEnumerable<string> GetContainersFromCapiProvider(string providerName, uint type, bool getMachineKeys)
        {
            SafeCryptProvHandle provider = SafeCryptProvHandle.InvalidHandle;
            uint flags = getMachineKeys ? CAPI.CRYPT_VERIFYCONTEXT | CAPI.CRYPT_MACHINE_KEYSET : CAPI.CRYPT_VERIFYCONTEXT;
            int errorCode = 0;
            if (CAPI.CryptAcquireContext(ref provider,
                                         null,
                                         providerName,
                                         type,
                                         flags,
                                         out errorCode))
            {
                using (provider)
                {
                    uint dwMaxSize = 0;
                    if (CAPI.CryptGetProvParam(provider, CAPI.PP_ENUMCONTAINERS, null, ref dwMaxSize, CAPI.CRYPT_FIRST))
                    {
                        StringBuilder container = new StringBuilder(checked((int)dwMaxSize));
                        if (CAPI.CryptGetProvParam(provider, CAPI.PP_ENUMCONTAINERS, container, ref dwMaxSize, CAPI.CRYPT_FIRST))
                        {
                            yield return container.ToString();
                            bool success = true;
                            do
                            {
                                success = CAPI.CryptGetProvParam(provider, CAPI.PP_ENUMCONTAINERS, container, ref dwMaxSize, CAPI.CRYPT_NEXT);
                                if (success)
                                {
                                    yield return container.ToString();
                                    dwMaxSize = (uint)container.Capacity;
                                }

                            } while (success);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets all key containers from Cng provider.
        /// </summary>
        /// <param name="containers">List of containers</param>
        /// <param name="hProvider">Provider handle</param>
        /// <param name="getMachineKeys">Indicates whether to obtain machine keys</param>
        internal static void GetContainersFromCngProvider(List<string> containers, SafeNCryptProviderHandle hProvider, bool getMachineKeys)
        {
            int flags = getMachineKeys ? NCryptMethods.NCRYPT_MACHINE_KEY_FLAG : 0;
            unsafe
            {
                void* enumState = null;
                int status;

                while (true)
                {
                    // Potentially display a UI.
                    status = NCryptMethods.NCryptEnumKeys(hProvider, null, out NCryptMethods.NCryptKeyName* keyName, ref enumState, flags);
                    if (status == NCryptMethods.NTE_NO_MORE_ITEMS
                        || status == NCryptMethods.NTE_BAD_FLAGS     // some providers don't support Machine keys
                        || status == NCryptMethods.SCARD_E_NO_READERS_AVAILABLE
                        || status == NCryptMethods.SCARD_W_CANCELLED_BY_USER)
                    {
                        // Done with this provider, or the user cancelled the UI.
                        break;
                    }
                    else if (status != 0)
                    {
                        // Failed to read this one, read the next key.
                        continue;
                    }
                    containers.Add(keyName[0].Name);
                    NCryptMethods.NCryptFreeBuffer(keyName);
                }
            }
        }
    }

    /// <summary>
    /// Helper class for CryptEnumProviders
    /// </summary>
    internal class ProviderAndType
    {
        public int Type;
        public string Name;

        public ProviderAndType(StringBuilder pszName, uint dwType)
        {
            this.Name = pszName.ToString();
            this.Type = (int)dwType;
        }
    }
}

