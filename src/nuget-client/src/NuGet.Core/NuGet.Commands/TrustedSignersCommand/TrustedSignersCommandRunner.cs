// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Signing;
using static NuGet.Commands.TrustedSignersArgs;

#if IS_SIGNING_SUPPORTED
using NuGet.Packaging;
using NuGet.Protocol;
#endif

namespace NuGet.Commands
{
    /// <summary>
    /// Command Runner used to run the business logic for nuget trusted-signers command
    /// </summary>
    public class TrustedSignersCommandRunner : ITrustedSignersCommandRunner
    {
        private const int SuccessCode = 0;

        private readonly ITrustedSignersProvider _trustedSignersProvider;
        private readonly IPackageSourceProvider _packageSourceProvider;

        public TrustedSignersCommandRunner(ITrustedSignersProvider trustedSignersProvider, IPackageSourceProvider packageSourceProvider)
        {
            _trustedSignersProvider = trustedSignersProvider ?? throw new ArgumentNullException(nameof(trustedSignersProvider));
            _packageSourceProvider = packageSourceProvider;
        }

        public async Task<int> ExecuteCommandAsync(TrustedSignersArgs trustedSignersArgs)
        {
            var logger = trustedSignersArgs.Logger ?? NullLogger.Instance;
            var actionsProvider = new TrustedSignerActionsProvider(_trustedSignersProvider, logger);

            switch (trustedSignersArgs.Action)
            {
                case TrustedSignersAction.List:
                    ValidateListArguments(trustedSignersArgs);

                    await ListAllTrustedSignersAsync(logger);

                    break;

                case TrustedSignersAction.Add:
                    ValidateNameExists(trustedSignersArgs.Name);

                    var isPackagePathProvided = !string.IsNullOrEmpty(trustedSignersArgs.PackagePath);
                    var isServiceIndexProvided = !string.IsNullOrEmpty(trustedSignersArgs.ServiceIndex);
                    var isFingerprintProvided = !string.IsNullOrEmpty(trustedSignersArgs.CertificateFingerprint);
                    var isAlgorithmProvided = !string.IsNullOrEmpty(trustedSignersArgs.FingerprintAlgorithm);

                    if (isPackagePathProvided)
                    {
#if IS_SIGNING_SUPPORTED
                        if (isServiceIndexProvided || isFingerprintProvided || isAlgorithmProvided)
                        {
                            throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture, Strings.Error_CouldNotAdd, Strings.Error_InvalidCombinationOfArguments));
                        }

                        if (!(trustedSignersArgs.Repository ^ trustedSignersArgs.Author))
                        {
                            throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture, Strings.Error_NoSignatureTrustedForPackage, trustedSignersArgs.PackagePath));
                        }

                        var trustTarget = VerificationTarget.None;
                        if (trustedSignersArgs.Author)
                        {
                            if (!trustedSignersArgs.Repository && trustedSignersArgs.Owners != null && trustedSignersArgs.Owners.Any())
                            {
                                throw new CommandLineArgumentCombinationException(Strings.Error_CannotTrustOwnersForAuthor);
                            }

                            trustTarget |= VerificationTarget.Author;
                        }

                        if (trustedSignersArgs.Repository)
                        {
                            trustTarget |= VerificationTarget.Repository;
                        }

                        if (trustTarget == VerificationTarget.None)
                        {
                            trustTarget = VerificationTarget.Unknown;
                        }

                        var packagesToTrust = LocalFolderUtility.ResolvePackageFromPath(trustedSignersArgs.PackagePath);
                        LocalFolderUtility.EnsurePackageFileExists(trustedSignersArgs.PackagePath, packagesToTrust);

                        if (packagesToTrust.Count() > 1)
                        {
                            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                                Strings.Multiple_Nupkgs_Detected,
                                trustedSignersArgs.PackagePath));
                        }

                        foreach (var package in packagesToTrust)
                        {
                            using (var packageReader = new PackageArchiveReader(package))
                            {
                                await actionsProvider.AddTrustedSignerAsync(
                                    trustedSignersArgs.Name,
                                    packageReader,
                                    trustTarget,
                                    trustedSignersArgs.AllowUntrustedRoot,
                                    trustedSignersArgs.Owners,
                                    CancellationToken.None);
                            }
                        }

                        break;

#else
                        throw new NotSupportedException();
#endif
                    }

                    if (isServiceIndexProvided)
                    {
                        if (isFingerprintProvided || isAlgorithmProvided || trustedSignersArgs.Author || trustedSignersArgs.Repository)
                        {
                            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_CouldNotAdd, Strings.Error_InvalidCombinationOfArguments));
                        }

                        var serviceIndex = ValidateAndParseV3ServiceIndexUrl(trustedSignersArgs.ServiceIndex);

                        await actionsProvider.AddTrustedRepositoryAsync(
                            trustedSignersArgs.Name,
                            serviceIndex,
                            trustedSignersArgs.Owners,
                            CancellationToken.None);

                        break;
                    }

                    if (isFingerprintProvided)
                    {
                        if (trustedSignersArgs.Owners != null && trustedSignersArgs.Owners.Any())
                        {
                            throw new ArgumentException(Strings.Error_CannotTrustOwnersForAuthor);
                        }

                        var hashAlgorithm = ValidateAndParseFingerprintAlgorithm(trustedSignersArgs.FingerprintAlgorithm);

                        actionsProvider.AddOrUpdateTrustedSigner(
                            trustedSignersArgs.Name,
                            trustedSignersArgs.CertificateFingerprint,
                            hashAlgorithm,
                            trustedSignersArgs.AllowUntrustedRoot);

                        break;
                    }

                    if (isAlgorithmProvided || trustedSignersArgs.Author || trustedSignersArgs.Repository)
                    {
                        throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture, Strings.Error_CouldNotAdd, Strings.Error_InvalidCombinationOfArguments));
                    }

                    if (_packageSourceProvider == null)
                    {
                        throw new ArgumentException(Strings.Error_NoSourcesInformation);
                    }

                    var packageSource = _packageSourceProvider.GetPackageSourceByName(trustedSignersArgs.Name);
                    if (packageSource == null || string.IsNullOrEmpty(packageSource.Source))
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_UnavailableSource, trustedSignersArgs.Name));
                    }

                    var sourceServiceIndex = ValidateAndParseV3ServiceIndexUrl(packageSource.Source);

                    await actionsProvider.AddTrustedRepositoryAsync(
                        trustedSignersArgs.Name,
                        sourceServiceIndex,
                        trustedSignersArgs.Owners,
                        CancellationToken.None);


                    break;

                case TrustedSignersAction.Remove:
                    ValidateRemoveArguments(trustedSignersArgs);

                    await RemoveTrustedSignerAsync(trustedSignersArgs.Name, logger);

                    break;

                case TrustedSignersAction.Sync:
                    ValidateSyncArguments(trustedSignersArgs);

                    await actionsProvider.SyncTrustedRepositoryAsync(trustedSignersArgs.Name, CancellationToken.None);

                    break;
            }

            return SuccessCode;
        }


        private async Task ListAllTrustedSignersAsync(ILogger logger)
        {
            var trustedSigners = _trustedSignersProvider.GetTrustedSigners();
            if (!trustedSigners.Any())
            {
                await logger.LogAsync(LogLevel.Minimal, Strings.NoTrustedSigners);
                return;
            }

            var trustedSignersLogs = new List<LogMessage>();

            await logger.LogAsync(LogLevel.Minimal, Strings.RegsiteredTrustedSigners);
            await logger.LogAsync(LogLevel.Minimal, Environment.NewLine);

            for (var i = 0; i < trustedSigners.Count; i++)
            {
                var item = trustedSigners[i];

                var trustedSignerBuilder = new StringBuilder();

                var index = $" {i + 1}.".PadRight(6);
                var defaultIndentation = string.Empty.PadRight(6);

                trustedSignerBuilder.AppendLine(index + string.Format(CultureInfo.CurrentCulture, Strings.TrustedSignerLogTitle, item.Name, item.ElementName));

                if (item is RepositoryItem repoItem)
                {
                    trustedSignerBuilder.AppendLine(defaultIndentation + string.Format(CultureInfo.CurrentCulture, Strings.TrustedSignerLogServiceIndex, repoItem.ServiceIndex));

                    if (repoItem.Owners != null && repoItem.Owners.Any())
                    {
                        trustedSignerBuilder.AppendLine(defaultIndentation + string.Format(CultureInfo.CurrentCulture, Strings.TrustedSignerLogOwners, string.Join("; ", repoItem.Owners)));
                    }
                }

                trustedSignerBuilder.AppendLine(defaultIndentation + Strings.TrustedSignerLogCertificates);

                foreach (var cert in item.Certificates)
                {
                    var extraIndentation = string.Empty.PadRight(2);

                    var summaryAllowUntrustedRoot = (cert.AllowUntrustedRoot) ? Strings.TrustedSignerLogCertificateSummaryAllowUntrustedRoot : Strings.TrustedSignerLogCertificateSummaryUnallowUntrustedRoot;
                    trustedSignerBuilder.AppendLine(defaultIndentation + extraIndentation + string.Format(CultureInfo.CurrentCulture, summaryAllowUntrustedRoot, cert.HashAlgorithm.ToString(), cert.Fingerprint));
                }

                trustedSignersLogs.Add(new LogMessage(LogLevel.Minimal, trustedSignerBuilder.ToString()));
            }

            await logger.LogMessagesAsync(trustedSignersLogs);
        }

        private async Task RemoveTrustedSignerAsync(string name, ILogger logger)
        {
            var trustedSigners = _trustedSignersProvider.GetTrustedSigners().Where(item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
            if (!trustedSigners.Any())
            {
                await logger.LogAsync(LogLevel.Minimal, string.Format(CultureInfo.CurrentCulture, Strings.NoTrustedSignersMatching, name));
                return;
            }

            _trustedSignersProvider.Remove(trustedSigners.ToList());

            await logger.LogAsync(LogLevel.Minimal, string.Format(CultureInfo.CurrentCulture, Strings.SuccessfullyRemovedTrustedSigner, name));
        }

        private void ValidateListArguments(TrustedSignersArgs args)
        {
            var isNameProvided = !string.IsNullOrEmpty(args.Name);
            var isPackagePathProvided = !string.IsNullOrEmpty(args.PackagePath);
            var isServiceIndexProvided = !string.IsNullOrEmpty(args.ServiceIndex);
            var isFingerprintProvided = !string.IsNullOrEmpty(args.CertificateFingerprint);
            var isAlgorithmProvided = !string.IsNullOrEmpty(args.FingerprintAlgorithm);
            var areOwnersProvided = args.Owners != null && args.Owners.Any();
            var isUntrustedRootProvided = args.AllowUntrustedRoot;
            var isAuthorProvided = args.Author;
            var isRepositoryProvided = args.Repository;

            if (isNameProvided || isPackagePathProvided || isServiceIndexProvided ||
                isFingerprintProvided || isAlgorithmProvided || areOwnersProvided ||
                isUntrustedRootProvided || isAuthorProvided || isRepositoryProvided)
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture, Strings.Error_CouldNotList, Strings.Error_InvalidCombinationOfArguments));
            }
        }

        private void ValidateRemoveArguments(TrustedSignersArgs args)
        {
            ValidateNameExists(args.Name);

            var isPackagePathProvided = !string.IsNullOrEmpty(args.PackagePath);
            var isServiceIndexProvided = !string.IsNullOrEmpty(args.ServiceIndex);
            var isFingerprintProvided = !string.IsNullOrEmpty(args.CertificateFingerprint);
            var isAlgorithmProvided = !string.IsNullOrEmpty(args.FingerprintAlgorithm);
            var areOwnersProvided = args.Owners != null && args.Owners.Any();
            var isUntrustedRootProvided = args.AllowUntrustedRoot;
            var isAuthorProvided = args.Author;
            var isRepositoryProvided = args.Repository;

            if (isPackagePathProvided || isServiceIndexProvided ||
                isFingerprintProvided || isAlgorithmProvided || areOwnersProvided ||
                isUntrustedRootProvided || isAuthorProvided || isRepositoryProvided)
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture, Strings.Error_CouldNotRemove, Strings.Error_InvalidCombinationOfArguments));
            }
        }

        private void ValidateSyncArguments(TrustedSignersArgs args)
        {
            ValidateNameExists(args.Name);

            var isPackagePathProvided = !string.IsNullOrEmpty(args.PackagePath);
            var isServiceIndexProvided = !string.IsNullOrEmpty(args.ServiceIndex);
            var isFingerprintProvided = !string.IsNullOrEmpty(args.CertificateFingerprint);
            var isAlgorithmProvided = !string.IsNullOrEmpty(args.FingerprintAlgorithm);
            var areOwnersProvided = args.Owners != null && args.Owners.Any();
            var isUntrustedRootProvided = args.AllowUntrustedRoot;
            var isAuthorProvided = args.Author;
            var isRepositoryProvided = args.Repository;

            if (isPackagePathProvided || isServiceIndexProvided ||
                isFingerprintProvided || isAlgorithmProvided || areOwnersProvided ||
                isUntrustedRootProvided || isAuthorProvided || isRepositoryProvided)
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture, Strings.Error_CouldNotSync, Strings.Error_InvalidCombinationOfArguments));
            }
        }

        private void ValidateNameExists(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PropertyCannotBeNullOrEmpty, nameof(name)));
            }
        }

        private HashAlgorithmName ValidateAndParseFingerprintAlgorithm(string algorithm)
        {
            if (string.IsNullOrEmpty(algorithm))
            {
                return HashAlgorithmName.SHA256;
            }

            var hashAlgorithm = CryptoHashUtility.GetHashAlgorithmName(algorithm);

            if (hashAlgorithm == HashAlgorithmName.Unknown || !SigningSpecifications.V1.AllowedHashAlgorithms.Contains(hashAlgorithm))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_NotSupportedHashAlgorithm, algorithm));
            }

            return hashAlgorithm;
        }

        private Uri ValidateAndParseV3ServiceIndexUrl(string serviceIndex)
        {
            var validUri = Uri.TryCreate(serviceIndex, UriKind.Absolute, out var serviceIndexUri);
            if (!validUri || !string.Equals(serviceIndexUri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_ServiceIndexShouldBeHttps, serviceIndex));
            }

            return serviceIndexUri;
        }
    }
}
