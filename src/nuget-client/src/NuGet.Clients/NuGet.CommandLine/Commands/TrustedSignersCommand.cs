// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Signing;
using static NuGet.Commands.TrustedSignersArgs;

namespace NuGet.CommandLine.Commands
{
    [Command(typeof(NuGetCommand), "trusted-signers", "TrustedSignersCommandDescription",
        MinArgs = 0,
        MaxArgs = 2,
        UsageSummaryResourceName = "TrustedSignersCommandUsageSummary",
        UsageExampleResourceName = "TrustedSignersCommandUsageExamples")]
    public class TrustedSignersCommand : Command
    {
        [Option(typeof(NuGetCommand), "TrustedSignersCommandNameDescription")]
        public string Name { get; set; }

        [Option(typeof(NuGetCommand), "TrustedSignersCommandServiceIndexDescription")]
        public string ServiceIndex { get; set; }

        [Option(typeof(NuGetCommand), "TrustedSignersCommandCertificateFingerprintDescription")]
        public string CertificateFingerprint { get; set; }

        [Option(typeof(NuGetCommand), "TrustedSignersCommandFingerprintAlgorithmDescription")]
        public string FingerprintAlgorithm { get; set; }

        [Option(typeof(NuGetCommand), "TrustedSignersCommandAllowUntrustedRootDescription")]
        public bool AllowUntrustedRoot { get; set; }

        [Option(typeof(NuGetCommand), "TrustedSignersCommandAuthorDescription")]
        public bool Author { get; set; }

        [Option(typeof(NuGetCommand), "TrustedSignersCommandRepositoryDescription")]
        public bool Repository { get; set; }

        [Option(typeof(NuGetCommand), "TrustedSignersCommandOwnersDescription")]
        public ICollection<string> Owners { get; set; }

        internal ITrustedSignersCommandRunner TrustedSignersCommandRunner { get; set; }

        internal TrustedSignersCommand() : base()
        {
            Owners = new List<string>();
        }

        public override async Task ExecuteCommandAsync()
        {
            var actionString = Arguments.FirstOrDefault();

            TrustedSignersAction action;
            if (string.IsNullOrEmpty(actionString))
            {
                action = TrustedSignersAction.List;
            }
            else if (!Enum.TryParse(actionString, ignoreCase: true, result: out action))
            {
                throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture, NuGetResources.Error_UnknownAction, actionString));
            }

            string packagePath = null;
            if (Arguments.Count() > 1)
            {
                packagePath = Arguments[1];
            }

            var trustedSignersProvider = new TrustedSignersProvider(Settings);

            var trustedSignersArgs = new TrustedSignersArgs()
            {
                Action = action,
                PackagePath = packagePath,
                Name = Name,
                ServiceIndex = ServiceIndex,
                CertificateFingerprint = CertificateFingerprint,
                FingerprintAlgorithm = FingerprintAlgorithm,
                AllowUntrustedRoot = AllowUntrustedRoot,
                Author = Author,
                Repository = Repository,
                Owners = Owners,
                Logger = Console
            };

            if (TrustedSignersCommandRunner == null)
            {
                TrustedSignersCommandRunner = new TrustedSignersCommandRunner(trustedSignersProvider, SourceProvider);
            }

            var result = await TrustedSignersCommandRunner.ExecuteCommandAsync(trustedSignersArgs);

            if (result > 0)
            {
                throw new ExitCodeException(1);
            }
        }
    }
}
