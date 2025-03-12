// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.SignCheck.Interop.PortableExecutable;
using Microsoft.SignCheck.Logging;
using Microsoft.DotNet.StrongName;

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
                VerifyStrongName(svr);
            }

            svr.IsSigned = svr.IsAuthentiCodeSigned & ((svr.IsStrongNameSigned) || (!VerifyStrongNameSignature) || svr.IsNativeImage);
            svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, svr.IsSigned);

            return svr;
        }

        private void VerifyStrongName(SignatureVerificationResult svr)
        {
            if (PEHeader.IsManagedCode && PEHeader.IsILImage)
            {
                VerifyManagedStrongName(svr);
            }
            else
            {
                // NGEN/CrossGen don't preserve StrongName signatures.
                svr.IsNativeImage = true;
                svr.AddDetail(DetailKeys.StrongName, SignCheckResources.DetailNativeImage);
            }
        }

        private void VerifyManagedStrongName(SignatureVerificationResult svr)
        {
            svr.IsStrongNameSigned = StrongNameHelper.IsSigned(svr.FullPath);
            svr.AddDetail(DetailKeys.StrongName, SignCheckResources.DetailSignedStrongName, svr.IsStrongNameSigned);

            if (svr.IsStrongNameSigned)
            {
                if (StrongNameHelper.GetStrongNameTokenFromAssembly(svr.FullPath, out string tokenStr) == 0)
                {
                    svr.AddDetail(DetailKeys.StrongName, SignCheckResources.DetailPublicKeyToken, tokenStr);
                }
                else
                {
                    svr.AddDetail(DetailKeys.Error, SignCheckResources.ErrorInvalidOrMissingStrongNamePublicKeyToken);
                }
            }
        }
    }
}
