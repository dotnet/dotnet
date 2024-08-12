// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.CommandLine;
using NuGet.Commands;
using NuGet.Packaging.Signing;

namespace NuGet.MSSigning.Extensions
{
    [Command(typeof(NuGetMSSignCommand), "reposign", "RepoSignCommandDescription",
       MinArgs = 1,
       MaxArgs = 1,
       UsageSummaryResourceName = "RepoSignCommandUsageSummary",
       UsageExampleResourceName = "RepoSignCommandUsageExamples",
       UsageDescriptionResourceName = "RepoSignCommandUsageDescription")]
    public class RepoSignCommand : MSSignAbstract
    {
        [Option(typeof(NuGetMSSignCommand), "RepoSignCommandPackageOwnersDescription")]
        public IList<string> PackageOwners { get; set; }

        [Option(typeof(NuGetMSSignCommand), "RepoSignCommandV3ServiceIndexUrlDescription")]
        public string V3ServiceIndexUrl { get; set; }

        public RepoSignCommand() : base()
        {
            PackageOwners = new List<string>();
        }

        public override async Task ExecuteCommandAsync()
        {
            using (var signRequest = GetRepositorySignRequest())
            {
                var packages = GetPackages();
                var signCommandRunner = new SignCommandRunner();
                var result = await signCommandRunner.ExecuteCommandAsync(
                    packages, signRequest, Timestamper, Console, OutputDirectory, overwrite: false, token: CancellationToken.None);

                if (result != 0)
                {
                    throw new ExitCodeException(exitCode: result);
                }
            }
        }

        public RepositorySignPackageRequest GetRepositorySignRequest()
        {
            ValidatePackagePath();
            WarnIfNoTimestamper(Console);
            ValidateCertificateInputs(Console);
            EnsureOutputDirectory();
            ValidatePackageOwners();

            var signingSpec = SigningSpecifications.V1;
            var signatureHashAlgorithm = ValidateAndParseHashAlgorithm(HashAlgorithm, nameof(HashAlgorithm), signingSpec);
            var timestampHashAlgorithm = ValidateAndParseHashAlgorithm(TimestampHashAlgorithm, nameof(TimestampHashAlgorithm), signingSpec);
            var v3ServiceIndexUri = ValidateAndParseV3ServiceIndexUrl();

            var certCollection = GetCertificateCollection();
            var certificate = GetCertificate(certCollection);
            var privateKey = GetPrivateKey(certificate);

            var request = new RepositorySignPackageRequest(
                certificate,
                signatureHashAlgorithm,
                timestampHashAlgorithm,
                v3ServiceIndexUri,
                new ReadOnlyCollection<string>(PackageOwners))
            {
                PrivateKey = privateKey
            };

            request.AdditionalCertificates.AddRange(certCollection);

            return request;
        }

        private Uri ValidateAndParseV3ServiceIndexUrl()
        {
            // Assert mandatory argument
            if (Uri.TryCreate(V3ServiceIndexUrl, UriKind.Absolute, out var v3ServiceIndexUrl))
            {
                if (v3ServiceIndexUrl.Scheme == Uri.UriSchemeHttps)
                {
                    return v3ServiceIndexUrl;
                }
            }

            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    NuGetMSSignCommand.MSSignCommandInvalidArgumentException,
                    nameof(V3ServiceIndexUrl)));
        }

        private void ValidatePackageOwners()
        {
            if (PackageOwners.Any(packageOwner => string.IsNullOrWhiteSpace(packageOwner)))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    NuGetMSSignCommand.MSSignCommandInvalidArgumentException,
                    nameof(PackageOwners)));
            }
        }
    }
}
