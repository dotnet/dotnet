// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Commands;
using static NuGet.Commands.VerifyArgs;

namespace NuGet.CommandLine
{
    [Command(typeof(NuGetCommand), "verify", "VerifyCommandDescription",
        MinArgs = 1,
        MaxArgs = 1,
        UsageSummaryResourceName = "VerifyCommandUsageSummary",
        UsageExampleResourceName = "VerifyCommandUsageExamples")]
    public class VerifyCommand : Command
    {
        protected VerifyCommand() : base()
        {
            CertificateFingerprint = new List<string>();
        }

        [Option(typeof(NuGetCommand), "VerifyCommandCertificateFingerprintDescription")]
        public ICollection<string> CertificateFingerprint { get; set; }

        [Option(typeof(NuGetCommand), "VerifyCommandSignaturesDescription")]
        public bool Signatures { get; set; }

        [Option(typeof(NuGetCommand), "VerifyCommandAllDescription")]
        public bool All { get; set; }

        public override Task ExecuteCommandAsync()
        {
            var PackagePath = Arguments[0];

            if (string.IsNullOrEmpty(PackagePath))
            {
                throw new ArgumentException(nameof(PackagePath));
            }

            var verifyArgs = new VerifyArgs()
            {
                Verifications = GetVerificationTypes(),
                PackagePaths = new[] { PackagePath },
                CertificateFingerprint = CertificateFingerprint,
                Logger = Console,
                Settings = Settings
            };

            switch (Verbosity)
            {
                case Verbosity.Detailed:
                    verifyArgs.LogLevel = Common.LogLevel.Verbose;
                    break;
                case Verbosity.Normal:
                    verifyArgs.LogLevel = Common.LogLevel.Information;
                    break;
                case Verbosity.Quiet:
                    verifyArgs.LogLevel = Common.LogLevel.Minimal;
                    break;
            }

            var verifyCommandRunner = new VerifyCommandRunner();
            var result = verifyCommandRunner.ExecuteCommandAsync(verifyArgs).Result;
            if (result > 0)
            {
                throw new ExitCodeException(1);
            }
            return Task.CompletedTask;
        }

        private IList<Verification> GetVerificationTypes()
        {
            if (All)
            {
                return new[] { Verification.All };
            }

            var verifications = new List<Verification>();

            if (Signatures)
            {
                verifications.Add(Verification.Signatures);
            }

            return verifications;
        }
    }
}
