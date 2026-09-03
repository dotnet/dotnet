// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Signing;
using static NuGet.Commands.VerifyArgs;

namespace NuGet.CommandLine.XPlat
{
    internal static class VerifyCommand
    {
        internal static void Register(Command parent,
                              Func<ILoggerWithColor> getLogger,
                              Action<LogLevel> setLogLevel,
                              Func<IVerifyCommandRunner> getCommandRunner)
        {
            var verifyCmd = new Command("verify", Strings.VerifyCommandDescription);

            var packagePaths = new Argument<string[]>("package-paths")
            {
                Description = Strings.VerifyCommandPackagePathDescription,
                Arity = ArgumentArity.OneOrMore
            };

            var all = new Option<bool>("--all")
            {
                Description = Strings.VerifyCommandAllDescription,
                Arity = ArgumentArity.Zero
            };

            var fingerPrint = new Option<string[]>("--certificate-fingerprint")
            {
                Description = Strings.VerifyCommandCertificateFingerprintDescription,
                Arity = ArgumentArity.OneOrMore
            };

            var configFile = new Option<string>("--configfile")
            {
                Description = Strings.Option_ConfigFile,
                Arity = ArgumentArity.ZeroOrOne
            };

            var verbosity = new Option<string>("--verbosity", "-v")
            {
                Description = Strings.Verbosity_Description,
                Arity = ArgumentArity.ZeroOrOne
            };

            verifyCmd.Arguments.Add(packagePaths);
            verifyCmd.Options.Add(all);
            verifyCmd.Options.Add(fingerPrint);
            verifyCmd.Options.Add(configFile);
            verifyCmd.Options.Add(verbosity);

            verifyCmd.SetAction(async (parseResult, cancellationToken) =>
            {
                string[]? packagePathValues = parseResult.GetValue(packagePaths);
                ValidatePackagePaths(packagePathValues, "package-paths");

                VerifyArgs args = new VerifyArgs();
                args.PackagePaths = new List<string>(packagePathValues);
                args.Verifications = parseResult.GetValue(all) ?
                    new List<Verification>() { Verification.All } :
                    new List<Verification>() { Verification.Signatures };
                args.CertificateFingerprint = new List<string>(parseResult.GetValue(fingerPrint) ?? Array.Empty<string>());
                args.Logger = getLogger();
                args.Settings = XPlatUtility.ProcessConfigFile(parseResult.GetValue(configFile));
                setLogLevel(XPlatUtility.MSBuildVerbosityToNuGetLogLevel(parseResult.GetValue(verbosity)));

                X509TrustStore.InitializeForDotNetSdk(args.Logger);

                var runner = getCommandRunner();
                int result = await runner.ExecuteCommandAsync(args);

                return result;
            });

            parent.Subcommands.Add(verifyCmd);
        }

        private static void ValidatePackagePaths([NotNull] string[]? packagePaths, string argumentName)
        {
            if (packagePaths == null ||
                packagePaths.Length == 0 ||
                packagePaths.Any(packagePath => string.IsNullOrEmpty(packagePath)))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PkgMissingArgument,
                    "verify",
                    argumentName));
            }
        }
    }
}
