// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.SignCheck.Interop.PortableExecutable;
using Microsoft.SignCheck.Logging;

namespace Microsoft.SignCheck.Verification
{
    /// <summary>
    /// A generic verifier for portable executables (EXEs and DLLs).
    /// </summary>
    public class PortableExecutableVerifier : AuthentiCodeVerifier
    {
        protected PortableExecutableHeader PEHeader
        {
            get;
            private set;
        }

        public PortableExecutableVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options, string fileExtension) :
            base(log, exclusions, options, fileExtension)
        {
            FinalizeResult = false;            
        }

        /// <summary>
        /// Verify whether the portable executable contains an AuthentiCode signature and optionally check the
        /// StrongName signature if it is enabled and the file represents a managed code executable.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath)
        {
            // Defer to the base implementation to check the AuthentiCode signature.
            SignatureVerificationResult svr = base.VerifySignature(path, parent, virtualPath);
            PEHeader = new PortableExecutableHeader(svr.FullPath);

            if (VerifyStrongNameSignature)
            {
                VerifyStrongName(svr, PEHeader);
            }

            svr.IsSigned = svr.IsAuthentiCodeSigned & ((svr.IsStrongNameSigned) || (!VerifyStrongNameSignature) || svr.IsNativeImage);
            svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, svr.IsSigned);

            return svr;
        }

        public void VerifyStrongName(SignatureVerificationResult svr, PortableExecutableHeader portableExecutableHeader)
        {
            if (portableExecutableHeader.IsManagedCode)
            {
                svr.IsNativeImage = !portableExecutableHeader.IsILImage;
                // NGEN/CrossGen don't preserve StrongName signatures.
                if (!svr.IsNativeImage)
                {
                    bool wasVerified = false;
                    int hresult = StrongName.ClrStrongName.StrongNameSignatureVerificationEx(svr.FullPath, fForceVerification: true, pfWasVerified: out wasVerified);
                    svr.IsStrongNameSigned = hresult == StrongName.S_OK;
                    svr.AddDetail(DetailKeys.StrongName, SignCheckResources.DetailSignedStrongName, svr.IsStrongNameSigned);

                    if (hresult != StrongName.S_OK)
                    {
                        svr.AddDetail(DetailKeys.StrongName, SignCheckResources.DetailHResult, hresult);
                    }
                    else
                    {
                        string publicToken;
                        hresult = StrongName.GetStrongNameTokenFromAssembly(svr.FullPath, out publicToken);
                        if (hresult == StrongName.S_OK)
                        {
                            svr.AddDetail(DetailKeys.StrongName, SignCheckResources.DetailPublicKeyToken, publicToken);
                        }
                    }
                }
                else
                {
                    svr.AddDetail(DetailKeys.StrongName, SignCheckResources.DetailNativeImage);
                }
            }
            else
            {
                svr.IsNativeImage = true;
            }
        }
    }
}
