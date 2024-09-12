// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

#if IS_SIGNING_SUPPORTED
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;
#endif

namespace NuGet.Packaging.Signing
{
    public class IntegrityVerificationProvider : ISignatureVerificationProvider
    {
        public Task<PackageVerificationResult> GetTrustResultAsync(ISignedPackageReader package, PrimarySignature signature, SignedPackageVerifierSettings settings, CancellationToken token)
        {
            return VerifyPackageIntegrityAsync(package, signature, settings);
        }

#if IS_SIGNING_SUPPORTED
        private async Task<PackageVerificationResult> VerifyPackageIntegrityAsync(ISignedPackageReader package, PrimarySignature signature, SignedPackageVerifierSettings settings)
        {
            var status = SignatureVerificationStatus.Unknown;
            var issues = new List<SignatureLog>();

            var validHashOids = SigningSpecifications.V1.AllowedHashAlgorithmOids;
            var signatureHashOid = signature.SignatureContent.HashAlgorithm.ConvertToOidString();
            if (validHashOids.Contains(signatureHashOid, StringComparer.InvariantCultureIgnoreCase))
            {
                issues.Add(SignatureLog.InformationLog(string.Format(CultureInfo.CurrentCulture, Strings.SignatureHashAlgorithm, signature.SignatureContent.HashAlgorithm)));

                try
                {
                    await package.ValidateIntegrityAsync(signature.SignatureContent, CancellationToken.None);
                    status = SignatureVerificationStatus.Valid;
                }
                catch (Exception e)
                {
                    status = SignatureVerificationStatus.Suspect;
                    issues.Add(SignatureLog.Error(NuGetLogCode.NU3008, Strings.SignaturePackageIntegrityFailure));
                    issues.Add(SignatureLog.DebugLog(e.ToString()));
                }
            }
            else
            {
                issues.Add(SignatureLog.Issue(!settings.AllowIllegal, NuGetLogCode.NU3016, Strings.SignatureFailureInvalidHashAlgorithmOid));
                issues.Add(SignatureLog.DebugLog(string.Format(CultureInfo.CurrentCulture, Strings.SignatureDebug_HashOidFound, signatureHashOid)));
            }

            return new SignedPackageVerificationResult(status, signature, issues);
        }
#else
        private Task<PackageVerificationResult> VerifyPackageIntegrityAsync(ISignedPackageReader package, PrimarySignature signature, SignedPackageVerifierSettings settings)
        {
            throw new NotSupportedException();
        }
#endif
    }
}
