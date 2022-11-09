// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Signing;
using static NuGet.Commands.VerifyArgs;

namespace NuGet.CommandLine.XPlat
{
    internal static class VerifyCommand
    {
        internal static void Register(CommandLineApplication app,
                              Func<ILogger> getLogger,
                              Action<LogLevel> setLogLevel,
                              Func<IVerifyCommandRunner> getCommandRunner)
        {
            app.Command("verify", verifyCmd =>
            {
                CommandArgument packagePaths = verifyCmd.Argument(
                    "<package-paths>",
                    Strings.VerifyCommandPackagePathDescription,
                    multipleValues: true);

                CommandOption all = verifyCmd.Option(
                    "--all",
                    Strings.VerifyCommandAllDescription,
                    CommandOptionType.NoValue);

                CommandOption fingerPrint = verifyCmd.Option(
                    "--certificate-fingerprint",
                    Strings.VerifyCommandCertificateFingerprintDescription,
                    CommandOptionType.MultipleValue);

                CommandOption configFile = verifyCmd.Option(
                    "--configfile",
                    Strings.Option_ConfigFile,
                    CommandOptionType.SingleValue);

                CommandOption verbosity = verifyCmd.Option(
                    "-v|--verbosity",
                    Strings.Verbosity_Description,
                    CommandOptionType.SingleValue);

                verifyCmd.HelpOption(XPlatUtility.HelpOption);
                verifyCmd.Description = Strings.VerifyCommandDescription;

                verifyCmd.OnExecute(async () =>
                {
                    ValidatePackagePaths(packagePaths);

                    VerifyArgs args = new VerifyArgs();
                    args.PackagePaths = packagePaths.Values;
                    args.Verifications = all.HasValue() ?
                        new List<Verification>() { Verification.All } :
                        new List<Verification>() { Verification.Signatures };
                    args.CertificateFingerprint = fingerPrint.Values;
                    args.Logger = getLogger();
                    args.Settings = XPlatUtility.ProcessConfigFile(configFile.Value());
                    setLogLevel(XPlatUtility.MSBuildVerbosityToNuGetLogLevel(verbosity.Value()));

                    X509TrustStore.InitializeForDotNetSdk(args.Logger);

                    var runner = getCommandRunner();
                    var verifyTask = runner.ExecuteCommandAsync(args);
                    await verifyTask;

                    return verifyTask.Result;
                });
            });
        }

        private static void ValidatePackagePaths(CommandArgument argument)
        {
            if (argument.Values.Count == 0 ||
                argument.Values.Any<string>(packagePath => string.IsNullOrEmpty(packagePath)))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PkgMissingArgument,
                    "verify",
                    argument.Name));
            }
        }
    }
}
