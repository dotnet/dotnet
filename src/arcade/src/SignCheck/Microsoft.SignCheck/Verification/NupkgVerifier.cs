// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SignCheck.Logging;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Signing;

namespace Microsoft.SignCheck.Verification
{
    public class NupkgVerifier : ArchiveVerifier
    {
        public NupkgVerifier(Log log, Exclusions exclusions, SignatureVerificationOptions options) : base(log, exclusions, options, fileExtension: ".nupkg")
        {

        }

        public override SignatureVerificationResult VerifySignature(string path, string parent, string virtualPath) 
        {
            SignatureVerificationResult svr = new SignatureVerificationResult(path, parent, virtualPath);
            string fullPath = svr.FullPath;

            svr.IsSigned = IsSigned(fullPath);
            svr.AddDetail(DetailKeys.File, SignCheckResources.DetailSigned, svr.IsSigned);
            VerifyContent(svr);

            return svr;
        }

        private bool IsSigned(string path)
        {
            List<ISignatureVerificationProvider> providers = new()
            {
                new IntegrityVerificationProvider(),
                new SignatureTrustAndValidityVerificationProvider(),
            };
            var verifierSettings = SignedPackageVerifierSettings.GetVerifyCommandDefaultPolicy();
            var packageSignatureVerifier = new PackageSignatureVerifier(providers);

            using (var pr = new PackageArchiveReader(path))
            {
                Task<VerifySignaturesResult> verifySignatureResult = packageSignatureVerifier.VerifySignaturesAsync(pr, verifierSettings, CancellationToken.None);

                return verifySignatureResult.Result.IsValid;
            }
        }
    }
}
