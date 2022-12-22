// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Commands;
using NuGet.Common;

namespace NuGet.CommandLine.Commands
{
    [Command(typeof(NuGetCommand),
        "client-certs",
        "ClientCertificatesDescription",
        MinArgs = 0,
        UsageSummaryResourceName = "ClientCertificatesCommandUsageSummary",
        UsageExampleResourceName = "ClientCertificatesCommandUsageExamples")]
    public class ClientCertificatesCommand : Command
    {
        internal ClientCertificatesCommand()
        {
        }

        [Option(typeof(NuGetCommand), "ClientCertificatesCommandFindByDescription", AltName = nameof(IClientCertArgsWithStoreData.FindBy))]
        public string FindBy { get; set; }

        [Option(typeof(NuGetCommand), "ClientCertificatesCommandFindValueDescription", AltName = nameof(IClientCertArgsWithStoreData.FindValue))]
        public string FindValue { get; set; }

        [Option(typeof(NuGetCommand), "ClientCertificatesCommandForceDescription", AltName = nameof(IClientCertArgsWithForce.Force))]
        public bool Force { get; set; }

        [Option(typeof(NuGetCommand), "ClientCertificatesCommandPackageSourceDescription", AltName = nameof(IClientCertArgsWithPackageSource.PackageSource))]
        public string PackageSource { get; set; }

        [Option(typeof(NuGetCommand), "ClientCertificatesCommandPasswordDescription", AltName = nameof(IClientCertArgsWithFileData.Password))]
        public string Password { get; set; }

        [Option(typeof(NuGetCommand), "ClientCertificatesCommandFilePathDescription", AltName = nameof(IClientCertArgsWithFileData.Path))]
        public string Path { get; set; }

        [Option(typeof(NuGetCommand), "ClientCertificatesCommandStoreLocationDescription", AltName = nameof(IClientCertArgsWithStoreData.StoreLocation))]
        public string StoreLocation { get; set; }

        [Option(typeof(NuGetCommand), "ClientCertificatesCommandStoreNameDescription", AltName = nameof(IClientCertArgsWithStoreData.StoreName))]
        public string StoreName { get; set; }

        [Option(typeof(NuGetCommand), "ClientCertificatesCommandStorePasswordInClearTextDescription", AltName = nameof(IClientCertArgsWithFileData.StorePasswordInClearText))]
        public bool StorePasswordInClearText { get; set; }

        public override void ExecuteCommand()
        {
            var actionString = Arguments.FirstOrDefault() ?? "list";
            switch (actionString.ToUpperInvariant())
            {
                case "LIST":
                    ExecuteListCommandRunner();
                    break;
                case "ADD":
                    ExecuteAddCommandRunner();
                    break;
                case "REMOVE":
                    ExecuteRemoveCommandRunner();
                    break;
                case "UPDATE":
                    ExecuteUpdateCommandRunner();
                    break;
                default:
                    throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture, NuGetResources.Error_UnknownAction, actionString));
            }
        }

        private void ExecuteAddCommandRunner()
        {
            var args = new AddClientCertArgs
            {
                Configfile = ConfigFile,
                PackageSource = PackageSource,
                FindBy = FindBy,
                Path = Path,
                StoreName = StoreName,
                StoreLocation = StoreLocation,
                FindValue = FindValue,
                Password = Password,
                StorePasswordInClearText = StorePasswordInClearText,
                Force = Force
            };
            AddClientCertRunner.Run(args, () => Console);
        }

        private void ExecuteListCommandRunner()
        {
            ValidateNotExpectedOptions(new Dictionary<string, string>
            {
                { nameof(Path), Path },
                { nameof(FindBy), FindBy },
                { nameof(FindValue), FindValue },
                { nameof(PackageSource), PackageSource },
                { nameof(Password), Password },
                { nameof(StoreLocation), StoreLocation },
                { nameof(StoreName), StoreName }
            });

            var args = new ListClientCertArgs { Configfile = ConfigFile };
            ListClientCertRunner.Run(args, () => Console);
        }

        private void ExecuteRemoveCommandRunner()
        {
            ValidateNotExpectedOptions(new Dictionary<string, string>
            {
                { nameof(Path), Path },
                { nameof(FindBy), FindBy },
                { nameof(FindValue), FindValue },
                { nameof(Password), Password },
                { nameof(StoreLocation), StoreLocation },
                { nameof(StoreName), StoreName }
            });

            var args = new RemoveClientCertArgs
            {
                Configfile = ConfigFile,
                PackageSource = PackageSource
            };
            RemoveClientCertRunner.Run(args, () => Console);
        }

        private void ExecuteUpdateCommandRunner()
        {
            var args = new UpdateClientCertArgs
            {
                Configfile = ConfigFile,
                PackageSource = PackageSource,
                FindBy = FindBy,
                Path = Path,
                StoreName = StoreName,
                StoreLocation = StoreLocation,
                FindValue = FindValue,
                Password = Password,
                StorePasswordInClearText = StorePasswordInClearText,
                Force = Force
            };
            UpdateClientCertRunner.Run(args, () => Console);
        }

        private void ValidateNotExpectedOptions(Dictionary<string, string> options)
        {
            foreach (var option in options)
            {
                if (!string.IsNullOrWhiteSpace(option.Value))
                {
                    throw new CommandLineArgumentCombinationException(string.Format(CultureInfo.CurrentCulture, NuGetResources.Error_ArgumentNotExpected, option.Value));
                }
            }
        }
    }
}
