// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Signing;

namespace NuGet.CommandLine.XPlat
{
    internal static class SignCommand
    {
        private const string CommandName = "sign";

        internal static void Register(Command parent,
            Func<ILoggerWithColor> getLogger,
            Action<LogLevel> setLogLevel,
            Func<ISignCommandRunner> getCommandRunner)
        {
            var signCmd = new Command(CommandName, Strings.SignCommandDescription);

            var packagePaths = new Argument<string[]>("package-paths")
            {
                Description = Strings.SignCommandPackagePathDescription,
                Arity = ArgumentArity.OneOrMore
            };

            var outputDirectory = new Option<string>("--output", "-o")
            {
                Description = Strings.SignCommandOutputDirectoryDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var path = new Option<string>("--certificate-path")
            {
                Description = Strings.SignCommandCertificatePathDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var store = new Option<string>("--certificate-store-name")
            {
                Description = Strings.SignCommandCertificateStoreNameDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var location = new Option<string>("--certificate-store-location")
            {
                Description = Strings.SignCommandCertificateStoreLocationDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var subject = new Option<string>("--certificate-subject-name")
            {
                Description = Strings.SignCommandCertificateSubjectNameDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var fingerprint = new Option<string>("--certificate-fingerprint")
            {
                Description = Strings.SignCommandCertificateFingerprintDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var password = new Option<string>("--certificate-password")
            {
                Description = Strings.SignCommandCertificatePasswordDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var algorithm = new Option<string>("--hash-algorithm")
            {
                Description = Strings.SignCommandHashAlgorithmDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var timestamper = new Option<string>("--timestamper")
            {
                Description = Strings.SignCommandTimestamperDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var timestamperAlgorithm = new Option<string>("--timestamp-hash-algorithm")
            {
                Description = Strings.SignCommandTimestampHashAlgorithmDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var overwrite = new Option<bool>("--overwrite")
            {
                Description = Strings.SignCommandOverwriteDescription,
                Arity = ArgumentArity.Zero
            };

            var allowUntrustedRoot = new Option<bool>("--allow-untrusted-root")
            {
                Description = Strings.SignCommandAllowUntrustedRootDescription,
                Arity = ArgumentArity.Zero
            };

            var verbosity = new Option<string>("--verbosity", "-v")
            {
                Description = Strings.Verbosity_Description,
                Arity = ArgumentArity.ZeroOrOne
            };

            signCmd.Arguments.Add(packagePaths);
            signCmd.Options.Add(outputDirectory);
            signCmd.Options.Add(path);
            signCmd.Options.Add(store);
            signCmd.Options.Add(location);
            signCmd.Options.Add(subject);
            signCmd.Options.Add(fingerprint);
            signCmd.Options.Add(password);
            signCmd.Options.Add(algorithm);
            signCmd.Options.Add(timestamper);
            signCmd.Options.Add(timestamperAlgorithm);
            signCmd.Options.Add(overwrite);
            signCmd.Options.Add(allowUntrustedRoot);
            signCmd.Options.Add(verbosity);

            signCmd.SetAction(async (parseResult, cancellationToken) =>
            {
                ILogger logger = getLogger();

                string[]? packagePathValues = parseResult.GetValue(packagePaths);
                string? pathValue = parseResult.GetValue(path);
                string? fingerprintValue = parseResult.GetValue(fingerprint);
                string? subjectValue = parseResult.GetValue(subject);
                string? storeValue = parseResult.GetValue(store);
                string? locationValue = parseResult.GetValue(location);
                string? outputDirectoryValue = parseResult.GetValue(outputDirectory);
                string? timestamperValue = parseResult.GetValue(timestamper);
                string? algorithmValue = parseResult.GetValue(algorithm);
                string? timestamperAlgorithmValue = parseResult.GetValue(timestamperAlgorithm);

                ValidatePackagePaths(packagePathValues, "package-paths");
                WarnIfNoTimestamper(logger, timestamperValue);
                ValidateCertificateInputs(pathValue, fingerprintValue, subjectValue, storeValue, locationValue, logger);
                ValidateAndCreateOutputDirectory(outputDirectoryValue);

                SigningSpecificationsV1 signingSpec = SigningSpecifications.V1;
                StoreLocation storeLocation = ValidateAndParseStoreLocation(locationValue);
                StoreName storeName = ValidateAndParseStoreName(storeValue);
                HashAlgorithmName hashAlgorithm = CommandLineUtility.ParseAndValidateHashAlgorithm(algorithmValue, "--hash-algorithm", signingSpec);
                HashAlgorithmName timestampHashAlgorithm = CommandLineUtility.ParseAndValidateHashAlgorithm(timestamperAlgorithmValue, "--timestamp-hash-algorithm", signingSpec);

                var args = new SignArgs()
                {
                    PackagePaths = new List<string>(packagePathValues),
                    OutputDirectory = outputDirectoryValue,
                    CertificatePath = pathValue,
                    CertificateStoreName = storeName,
                    CertificateStoreLocation = storeLocation,
                    CertificateSubjectName = subjectValue,
                    CertificateFingerprint = fingerprintValue,
                    CertificatePassword = parseResult.GetValue(password),
                    SignatureHashAlgorithm = hashAlgorithm,
                    Logger = logger,
                    Overwrite = parseResult.GetValue(overwrite),
                    AllowUntrustedRoot = parseResult.GetValue(allowUntrustedRoot),
                    //The interactive option is not enabled at first, so the NonInteractive is always set to true. This is tracked by https://github.com/NuGet/Home/issues/10620
                    NonInteractive = true,
                    Timestamper = timestamperValue,
                    TimestampHashAlgorithm = timestampHashAlgorithm
                };

                setLogLevel(XPlatUtility.MSBuildVerbosityToNuGetLogLevel(parseResult.GetValue(verbosity)));

                X509TrustStore.InitializeForDotNetSdk(args.Logger);

                ISignCommandRunner runner = getCommandRunner();
                int result = await runner.ExecuteCommandAsync(args);
                return result;
            });

            parent.Subcommands.Add(signCmd);
        }

        private static void ValidatePackagePaths([NotNull] string[]? packagePaths, string argumentName)
        {
            if (packagePaths == null ||
                packagePaths.Length == 0 ||
                packagePaths.Any(packagePath => string.IsNullOrEmpty(packagePath)))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PkgMissingArgument,
                    CommandName,
                    argumentName));
            }
        }

        private static void WarnIfNoTimestamper(ILogger logger, string? timestamper)
        {
            if (string.IsNullOrEmpty(timestamper))
            {
                logger.Log(LogMessage.CreateWarning(NuGetLogCode.NU3002, Strings.SignCommandNoTimestamperWarning));
            }
        }

        private static void ValidateAndCreateOutputDirectory(string? outputDirectory)
        {
            if (!string.IsNullOrEmpty(outputDirectory))
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
        }

        private static StoreLocation ValidateAndParseStoreLocation(string? locationValue)
        {
            StoreLocation storeLocation = StoreLocation.CurrentUser;

            if (!string.IsNullOrEmpty(locationValue))
            {
                if (!Enum.TryParse(locationValue, ignoreCase: true, result: out storeLocation))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        Strings.Err_InvalidValue,
                        "--certificate-store-location",
                        string.Join(",", Enum.GetValues<StoreLocation>().ToList())));
                }
            }

            return storeLocation;
        }

        private static StoreName ValidateAndParseStoreName(string? storeValue)
        {
            StoreName storeName = StoreName.My;

            if (!string.IsNullOrEmpty(storeValue))
            {
                if (!Enum.TryParse(storeValue, ignoreCase: true, result: out storeName))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        Strings.Err_InvalidValue,
                        "--certificate-store-name",
                        string.Join(",", Enum.GetValues<StoreName>().ToList())));
                }
            }

            return storeName;
        }

        private static void ValidateCertificateInputs(string? path, string? fingerprint,
                                                      string? subject, string? store, string? location, ILogger logger)
        {
            if (string.IsNullOrEmpty(path) &&
                string.IsNullOrEmpty(fingerprint) &&
                string.IsNullOrEmpty(subject))
            {
                // Throw if user gave no certificate input
                throw new ArgumentException(Strings.SignCommandNoCertificateException);
            }
            else if (!string.IsNullOrEmpty(path) &&
                (!string.IsNullOrEmpty(fingerprint) ||
                 !string.IsNullOrEmpty(subject) ||
                 !string.IsNullOrEmpty(location) ||
                 !string.IsNullOrEmpty(store)))
            {
                // Throw if the user provided a path and any one of the other options
                throw new ArgumentException(Strings.SignCommandMultipleCertificateException);
            }
            else if (!string.IsNullOrEmpty(fingerprint) && !string.IsNullOrEmpty(subject))
            {
                // Throw if the user provided a fingerprint and a subject
                throw new ArgumentException(Strings.SignCommandMultipleCertificateException);
            }
            else if (fingerprint != null)
            {
                bool isValidFingerprint = CertificateUtility.TryDeduceHashAlgorithm(fingerprint, out HashAlgorithmName hashAlgorithmName);
                bool isSHA1 = hashAlgorithmName == HashAlgorithmName.SHA1;
                string message = string.Format(CultureInfo.CurrentCulture, Strings.SignCommandInvalidCertificateFingerprint, NuGetLogCode.NU3043);

                if (!isValidFingerprint || isSHA1)
                {
                    throw new ArgumentException(message);
                }
            }
        }
    }
}
