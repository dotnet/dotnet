// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using Microsoft.SignCheck.Logging;
#if NET
using System.Reflection.PortableExecutable;
#endif

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
    }

    public class AuthentiCodeSecurityInfoProvider : ISecurityInfoProvider
    {
        public SignedCms ReadSecurityInfo(string path)
        {
#if NET
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
#else
            throw new NotSupportedException("Not supported on .NET Framework");
#endif
        }
    }
}
