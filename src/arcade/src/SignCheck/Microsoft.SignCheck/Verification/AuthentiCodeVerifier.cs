// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using Microsoft.SignCheck.Interop;
using Microsoft.SignCheck.Logging;
using System.Reflection.PortableExecutable;

namespace Microsoft.SignCheck.Verification
{
    /// <summary>
    /// A generic FileVerifier that can be used to validate AuthentiCode signatures
    /// </summary>
    public class AuthentiCodeVerifier : FileVerifier
    {
        ISecurityInfoProvider _securityInfoProvider = new AuthentiCodeSecurityInfoProvider();
        protected bool FinalizeResult
        {
            get;
            set;
        } = true;

        public AuthentiCodeVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension, ISecurityInfoProvider securityInfoProvider = null)
            : base(log, exclusions, options, fileExtension)
        {
            if (securityInfoProvider != null)
            {
                _securityInfoProvider = securityInfoProvider;
            }
        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
        {
            SignatureVerificationResult svr = VerifyAuthentiCode(path, parent, virtualPath);

            if (FinalizeResult)
            {
                // Derived class that need to evaluate additional properties and results must
                // set FinalizeResult = false, otherwise the Signed result can be logged multiple times.
                svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, svr.IsSigned);
            }

            return svr;
        }

        protected SignatureVerificationResult VerifyAuthentiCode(string path, string parent, string virtualPath)
        {
            var svr = new SignatureVerificationResult(path, parent, virtualPath);
            svr.IsAuthentiCodeSigned = AuthentiCode.IsSigned(path, svr, _securityInfoProvider);
            svr.IsSigned = svr.IsAuthentiCodeSigned;

            // WinVerifyTrust validates the file hash against the Authenticode signature,
            // catching post-signing modifications that CMS-only validation would miss.
            if (svr.IsAuthentiCodeSigned && OperatingSystem.IsWindows())
            {
                if (!VerifyFileIntegrity(path, svr))
                {
                    svr.IsAuthentiCodeSigned = false;
                    svr.IsSigned = false;
                }
            }

            // TODO: Should only check if there is a signature, even if it's invalid
            if (VerifyAuthenticodeTimestamps)
            {
                try
                {
                    svr.Timestamps = AuthentiCode.GetTimestamps(path, _securityInfoProvider).ToList();

                    foreach (Timestamp timestamp in svr.Timestamps)
                    {
                        svr.AddDetail(DetailKeys.AuthentiCode, SignCheckResources.DetailTimestamp, timestamp.SignedOn, timestamp.SignatureAlgorithm);
                        svr.IsAuthentiCodeSigned &= timestamp.IsValid;
                    }
                }
                catch
                {
                    svr.AddDetail(DetailKeys.AuthentiCode, SignCheckResources.DetailTimestampError);
                    svr.IsSigned = false;
                }
            }
            else
            {
                svr.AddDetail(DetailKeys.AuthentiCode, SignCheckResources.DetailTimestampSkipped);
            }

            svr.AddDetail(DetailKeys.AuthentiCode, SignCheckResources.DetailSignedAuthentiCode, svr.IsAuthentiCodeSigned);

            return svr;
        }

        /// <summary>
        /// Uses WinVerifyTrust to verify the file's Authenticode signature integrity,
        /// ensuring the file content has not been modified after signing.
        /// Uses WTD_HASH_ONLY_FLAG so only the file digest is verified, not trust policy
        /// (untrusted roots, expired certs, etc. are handled separately by CMS validation).
        /// </summary>
        [SupportedOSPlatform("windows")]
        private bool VerifyFileIntegrity(string path, SignatureVerificationResult svr)
        {
            var fileInfo = new WinTrustFileInfo
            {
                cbStruct = (uint)Marshal.SizeOf<WinTrustFileInfo>(),
                pcwszFilePath = path,
                hFile = IntPtr.Zero,
                pgKnownSubject = IntPtr.Zero
            };

            IntPtr pFileInfo = Marshal.AllocHGlobal(Marshal.SizeOf<WinTrustFileInfo>());
            IntPtr pGuid = Marshal.AllocHGlobal(Marshal.SizeOf<Guid>());
            try
            {
                Marshal.StructureToPtr(fileInfo, pFileInfo, false);

                var trustData = new WinTrustData
                {
                    cbStruct = (uint)Marshal.SizeOf<WinTrustData>(),
                    pPolicyCallbackData = IntPtr.Zero,
                    pSIPClientData = IntPtr.Zero,
                    dwUIChoice = (uint)UIChoice.WTD_UI_NONE,
                    fdwRevocationChecks = (uint)RevocationChecks.WTD_REVOKE_NONE,
                    dwUnionChoice = (uint)UnionChoice.WTD_CHOICE_FILE,
                    pFile = pFileInfo,
                    dwStateAction = (uint)StateAction.WTD_STATEACTION_VERIFY,
                    hWVTStateData = IntPtr.Zero,
                    pwszURLReference = IntPtr.Zero,
                    dwProvFlags = (uint)(Provider.WTD_HASH_ONLY_FLAG),
                    dwUIContext = 0
                };

                IntPtr pTrustData = Marshal.AllocHGlobal(Marshal.SizeOf<WinTrustData>());
                try
                {
                    Marshal.StructureToPtr(trustData, pTrustData, false);

                    Guid actionId = WinTrust.WINTRUST_ACTION_GENERIC_VERIFY_V2;
                    Marshal.StructureToPtr(actionId, pGuid, false);

                    uint result = WinTrust.WinVerifyTrust(IntPtr.Zero, pGuid, pTrustData);

                    if (result != 0)
                    {
                        svr.AddDetail(DetailKeys.Error, "WinVerifyTrust failed: 0x{0:X8}. The file may have been modified after signing.", result);
                        return false;
                    }

                    return true;
                }
                finally
                {
                    // Read back the state handle that WinVerifyTrust wrote into the unmanaged buffer
                    trustData = Marshal.PtrToStructure<WinTrustData>(pTrustData);
                    trustData.dwStateAction = (uint)StateAction.WTD_STATEACTION_CLOSE;
                    Marshal.StructureToPtr(trustData, pTrustData, false);
                    WinTrust.WinVerifyTrust(IntPtr.Zero, pGuid, pTrustData);
                    Marshal.FreeHGlobal(pTrustData);
                }
            }
            finally
            {
                Marshal.DestroyStructure<WinTrustFileInfo>(pFileInfo);
                Marshal.FreeHGlobal(pFileInfo);
                Marshal.FreeHGlobal(pGuid);
            }
        }
    }

    public class AuthentiCodeSecurityInfoProvider : ISecurityInfoProvider
    {
        public SignedCms ReadSecurityInfo(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (PEReader peReader = new PEReader(fs))
            {
                var securityDirectory = peReader.PEHeaders.PEHeader.CertificateTableDirectory;
                if (securityDirectory.RelativeVirtualAddress != 0 && securityDirectory.Size != 0)
                {
                    int securityHeaderSize = 8; // 4(length of cert) + 2(cert revision) + 2(cert type)
                    if (securityDirectory.Size <= securityHeaderSize)
                    {
                        // No security entry - just the header
                        return null;
                    }

                    // Skip the header
                    fs.Position = securityDirectory.RelativeVirtualAddress + securityHeaderSize;
                    byte[] securityEntry = new byte[securityDirectory.Size - securityHeaderSize];

                    // Ensure the stream has enough data to read
                    if (fs.Length < fs.Position + securityEntry.Length)
                    {
                        throw new CryptographicException($"File '{path}' is too small to contain a valid security entry.");
                    }

                    // Read the security entry
                    fs.ReadExactly(securityEntry);

                    // Decode the security entry
                    var signedCms = new SignedCms();
                    signedCms.Decode(securityEntry);
                    return signedCms;
                }
            }

            return null;
        }
    }
}
