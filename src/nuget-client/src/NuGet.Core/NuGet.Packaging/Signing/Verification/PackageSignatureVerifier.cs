// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    public class PackageSignatureVerifier : IPackageSignatureVerifier
    {
        private readonly List<ISignatureVerificationProvider> _verificationProviders;

        public PackageSignatureVerifier(IEnumerable<ISignatureVerificationProvider> verificationProviders)
        {
            _verificationProviders = verificationProviders?.ToList() ?? throw new ArgumentNullException(nameof(verificationProviders));
        }

        public async Task<VerifySignaturesResult> VerifySignaturesAsync(ISignedPackageReader package, SignedPackageVerifierSettings settings, CancellationToken token, Guid parentId = default(Guid))
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var valid = false;
            var trustResults = new List<PackageVerificationResult>();

            var packageSigningTelemetryEvent = new PackageSigningTelemetryEvent();
            using (var telemetry = TelemetryActivity.Create(parentId, packageSigningTelemetryEvent))
            {
                var isSigned = await package.IsSignedAsync(token);
                if (isSigned)
                {
                    try
                    {
                        var signature = await package.GetPrimarySignatureAsync(token);

                        if (signature != null)
                        {
                            // Verify that the signature is trusted
                            var sigTrustResults = await Task.WhenAll(_verificationProviders.Select(e => e.GetTrustResultAsync(package, signature, settings, token)));
                            valid = IsValid(sigTrustResults);
                            trustResults.AddRange(sigTrustResults);
                        }
                        else
                        {
                            valid = false;
                        }
                    }
                    catch (SignatureException e)
                    {
                        // SignatureException generated while parsing signatures
                        var issues = new[] {
                            SignatureLog.Issue(!settings.AllowIllegal, e.Code, e.Message),
                            SignatureLog.DebugLog(e.ToString())
                        };
                        trustResults.Add(new InvalidSignaturePackageVerificationResult(
                            settings.AllowIllegal ? SignatureVerificationStatus.Valid : SignatureVerificationStatus.Disallowed,
                            issues));
                        valid = settings.AllowIllegal;
                    }
                    catch (CryptographicException e)
                    {
                        // CryptographicException generated while parsing the SignedCms object
                        var issues = new[] {
                            SignatureLog.Issue(!settings.AllowIllegal, NuGetLogCode.NU3003, Strings.ErrorPackageSignatureInvalid),
                            SignatureLog.DebugLog(e.ToString())
                        };
                        trustResults.Add(new InvalidSignaturePackageVerificationResult(
                            settings.AllowIllegal ? SignatureVerificationStatus.Valid : SignatureVerificationStatus.Disallowed,
                            issues));
                        valid = settings.AllowIllegal;
                    }
                }
                else if (settings.AllowUnsigned)
                {
                    // An unsigned package is valid only if unsigned packages are allowed.
                    valid = true;
                }
                else
                {
                    var issues = new[] { SignatureLog.Issue(fatal: true, code: NuGetLogCode.NU3004, message: Strings.ErrorPackageNotSigned) };
                    trustResults.Add(new UnsignedPackageVerificationResult(
                        settings.AllowIllegal ? SignatureVerificationStatus.Valid : SignatureVerificationStatus.Disallowed,
                        issues));
                    valid = false;
                }

                var status = valid ? NuGetOperationStatus.Succeeded : NuGetOperationStatus.Failed;
                packageSigningTelemetryEvent.SetResult(isSigned ? PackageSignType.Signed : PackageSignType.Unsigned, status);

                return new VerifySignaturesResult(valid, isSigned, trustResults);
            }
        }

        /// <summary>
        /// True if a provider trusts the package signature.
        /// </summary>
        private static bool IsValid(IEnumerable<PackageVerificationResult> verificationResults)
        {
            return verificationResults.Any() &&
                verificationResults.All(e => e.Trust == SignatureVerificationStatus.Valid);
        }
    }
}
