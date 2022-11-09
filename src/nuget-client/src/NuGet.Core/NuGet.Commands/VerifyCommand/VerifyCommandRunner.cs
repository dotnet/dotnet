// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using static NuGet.Commands.VerifyArgs;

namespace NuGet.Commands
{
    /// <summary>
    /// Command Runner used to run the business logic for nuget verify command
    /// </summary>
    public class VerifyCommandRunner : IVerifyCommandRunner
    {
        private const int SuccessCode = 0;
        private const int FailureCode = 1;
        private const HashAlgorithmName _defaultFingerprintAlgorithm = HashAlgorithmName.SHA256;

        public async Task<int> ExecuteCommandAsync(VerifyArgs verifyArgs)
        {
            if (verifyArgs.Verifications.Count == 0)
            {
                verifyArgs.Logger.LogError(string.Format(CultureInfo.CurrentCulture, Strings.VerifyCommand_VerificationTypeNotSupported));
                return FailureCode;
            }

            var errorCount = 0;

            if (ShouldExecuteVerification(verifyArgs, Verification.Signatures))
            {
                if (!IsSignatureVerifyCommandSupported())
                {
                    verifyArgs.Logger.LogError(string.Format(CultureInfo.CurrentCulture, Strings.VerifyCommand_NotSupported));
                    return FailureCode;
                }

                var packagesToVerify = verifyArgs.PackagePaths.SelectMany(packagePath =>
                {
                    var packages = LocalFolderUtility.ResolvePackageFromPath(packagePath);
                    LocalFolderUtility.EnsurePackageFileExists(packagePath, packages);
                    return packages;
                });

                ClientPolicyContext clientPolicyContext = ClientPolicyContext.GetClientPolicy(verifyArgs.Settings, verifyArgs.Logger);

                // List of values passed through --certificate-fingerprint option read
                var allowListEntries = verifyArgs.CertificateFingerprint.Select(fingerprint =>
                    new CertificateHashAllowListEntry(
                        VerificationTarget.Author | VerificationTarget.Repository,
                        SignaturePlacement.PrimarySignature,
                        fingerprint,
                        _defaultFingerprintAlgorithm)).ToList();

                var verifierSettings = SignedPackageVerifierSettings.GetVerifyCommandDefaultPolicy();
                var verificationProviders = new List<ISignatureVerificationProvider>()
                {
                    new IntegrityVerificationProvider()
                };

                // trustedSigners section >> Owners are considered here.
                verificationProviders.Add(
                    new AllowListVerificationProvider(
                        clientPolicyContext.AllowList,
                        requireNonEmptyAllowList: clientPolicyContext.Policy == SignatureValidationMode.Require,
                        emptyListErrorMessage: Strings.Error_NoClientAllowList,
                        noMatchErrorMessage: Strings.Error_NoMatchingClientCertificate));

                IEnumerable<KeyValuePair<string, HashAlgorithmName>> trustedSignerAllowUntrustedRootList = clientPolicyContext.AllowList?
                    .Where(c => c.AllowUntrustedRoot)
                    .Select(c => new KeyValuePair<string, HashAlgorithmName>(c.Fingerprint, c.FingerprintAlgorithm));

                // trustedSigners section >> allowUntrustedRoot set true are considered here.
                verificationProviders.Add(new SignatureTrustAndValidityVerificationProvider(trustedSignerAllowUntrustedRootList));

                // List of values passed through --certificate-fingerprint option are considered here.
                verificationProviders.Add(
                    new AllowListVerificationProvider(
                        allowListEntries,
                        requireNonEmptyAllowList: false,
                        noMatchErrorMessage: Strings.Error_NoMatchingCertificate));

                var verifier = new PackageSignatureVerifier(verificationProviders);

                foreach (var package in packagesToVerify)
                {
                    try
                    {
                        errorCount += await VerifySignatureForPackageAsync(package, verifyArgs.Logger, verifier, verifierSettings);
                    }
                    catch (InvalidDataException e)
                    {
                        verifyArgs.Logger.LogError(string.Format(CultureInfo.CurrentCulture, Strings.VerifyCommand_PackageIsNotValid, package));
                        ExceptionUtilities.LogException(e, verifyArgs.Logger);
                    }
                }
            }

            return errorCount == 0 ? SuccessCode : FailureCode;
        }

        private async Task<int> VerifySignatureForPackageAsync(string packagePath, ILogger logger, PackageSignatureVerifier verifier, SignedPackageVerifierSettings verifierSettings)
        {
            var result = 0;
            using (var packageReader = new PackageArchiveReader(packagePath))
            {
                var verificationResult = await verifier.VerifySignaturesAsync(packageReader, verifierSettings, CancellationToken.None);

                var packageIdentity = packageReader.GetIdentity();

                logger.LogMinimal(Environment.NewLine + string.Format(CultureInfo.CurrentCulture,
                    Strings.VerifyCommand_VerifyingPackage,
                    packageIdentity.ToString()));
                logger.LogInformation($"{packagePath}{Environment.NewLine}");

                var logMessages = verificationResult.Results.SelectMany(p => p.Issues).ToList();

                //log issues first
                await logger.LogMessagesAsync(logMessages.Where(m => m.Level < LogLevel.Warning));

                if (logMessages.Any(m => m.Level >= LogLevel.Warning))
                {
                    logger.LogMinimal(Environment.NewLine);

                    IEnumerable<SignatureLog> warnsanderrors = logMessages.Where(m => m.Level >= LogLevel.Warning);

                    var errors = warnsanderrors.Count(m => m.Level == LogLevel.Error);
                    var warnings = warnsanderrors.Count() - errors;

                    logger.LogInformation(string.Format(CultureInfo.CurrentCulture, Strings.VerifyCommand_FinishedWithErrors, errors, warnings));

                    // log warnigs and errors at the end
                    await logger.LogMessagesAsync(warnsanderrors);

                    result = errors;
                }

                if (verificationResult.IsValid)
                {
                    logger.LogInformation(Environment.NewLine + string.Format(CultureInfo.CurrentCulture, Strings.VerifyCommand_Success, packageIdentity.ToString()));
                }
                else
                {
                    logger.LogMinimal(Environment.NewLine + Strings.VerifyCommand_Failed);
                }

                return result;
            }
        }

        private bool ShouldExecuteVerification(VerifyArgs args, Verification v)
        {
            return args.Verifications.Any(verification => verification == Verification.All || verification == v);
        }

        private bool IsSignatureVerifyCommandSupported()
        {
#if IS_DESKTOP
            if (RuntimeEnvironmentHelper.IsMono)
            {
                return false;
            }
#endif
            return true;
        }
    }
}
