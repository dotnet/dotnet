// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Packaging.Signing;

namespace NuGet.CommandLine.XPlat
{
    internal static class SignCommand
    {
        private const string CommandName = "sign";

        internal static void Register(CommandLineApplication app,
            Func<ILogger> getLogger,
            Action<LogLevel> setLogLevel,
            Func<ISignCommandRunner> getCommandRunner)
        {
            app.Command(CommandName, signCmd =>
            {
                CommandArgument packagePaths = signCmd.Argument(
                    "<package-paths>",
                    Strings.SignCommandPackagePathDescription,
                    multipleValues: true);

                CommandOption outputDirectory = signCmd.Option(
                    "-o|--output",
                    Strings.SignCommandOutputDirectoryDescription,
                    CommandOptionType.SingleValue);

                CommandOption path = signCmd.Option(
                    "--certificate-path",
                    Strings.SignCommandCertificatePathDescription,
                    CommandOptionType.SingleValue);

                CommandOption store = signCmd.Option(
                    "--certificate-store-name",
                    Strings.SignCommandCertificateStoreNameDescription,
                    CommandOptionType.SingleValue);

                CommandOption location = signCmd.Option(
                    "--certificate-store-location",
                    Strings.SignCommandCertificateStoreLocationDescription,
                    CommandOptionType.SingleValue);

                CommandOption subject = signCmd.Option(
                    "--certificate-subject-name",
                    Strings.SignCommandCertificateSubjectNameDescription,
                    CommandOptionType.SingleValue);

                CommandOption fingerprint = signCmd.Option(
                    "--certificate-fingerprint",
                    Strings.SignCommandCertificateFingerprintDescription,
                    CommandOptionType.SingleValue);

                CommandOption password = signCmd.Option(
                    "--certificate-password",
                    Strings.SignCommandCertificatePasswordDescription,
                    CommandOptionType.SingleValue);

                CommandOption algorithm = signCmd.Option(
                    "--hash-algorithm",
                    Strings.SignCommandHashAlgorithmDescription,
                    CommandOptionType.SingleValue);

                CommandOption timestamper = signCmd.Option(
                    "--timestamper",
                    Strings.SignCommandTimestamperDescription,
                    CommandOptionType.SingleValue);

                CommandOption timestamperAlgorithm = signCmd.Option(
                    "--timestamp-hash-algorithm",
                    Strings.SignCommandTimestampHashAlgorithmDescription,
                    CommandOptionType.SingleValue);

                CommandOption overwrite = signCmd.Option(
                    "--overwrite",
                    Strings.SignCommandOverwriteDescription,
                    CommandOptionType.NoValue);

                CommandOption verbosity = signCmd.Option(
                    "-v|--verbosity",
                    Strings.Verbosity_Description,
                    CommandOptionType.SingleValue);

                signCmd.HelpOption(XPlatUtility.HelpOption);

                signCmd.Description = Strings.SignCommandDescription;

                signCmd.OnExecute(async () =>
                {
                    ILogger logger = getLogger();

                    ValidatePackagePaths(packagePaths);
                    WarnIfNoTimestamper(logger, timestamper);
                    ValidateCertificateInputs(path, fingerprint, subject, store, location, logger);
                    ValidateAndCreateOutputDirectory(outputDirectory);

                    SigningSpecificationsV1 signingSpec = SigningSpecifications.V1;
                    StoreLocation storeLocation = ValidateAndParseStoreLocation(location);
                    StoreName storeName = ValidateAndParseStoreName(store);
                    HashAlgorithmName hashAlgorithm = CommandLineUtility.ParseAndValidateHashAlgorithm(algorithm.Value(), algorithm.LongName, signingSpec);
                    HashAlgorithmName timestampHashAlgorithm = CommandLineUtility.ParseAndValidateHashAlgorithm(timestamperAlgorithm.Value(), timestamperAlgorithm.LongName, signingSpec);

                    var args = new SignArgs()
                    {
                        PackagePaths = packagePaths.Values,
                        OutputDirectory = outputDirectory.Value(),
                        CertificatePath = path.Value(),
                        CertificateStoreName = storeName,
                        CertificateStoreLocation = storeLocation,
                        CertificateSubjectName = subject.Value(),
                        CertificateFingerprint = fingerprint.Value(),
                        CertificatePassword = password.Value(),
                        SignatureHashAlgorithm = hashAlgorithm,
                        Logger = logger,
                        Overwrite = overwrite.HasValue(),
                        //The interactive option is not enabled at first, so the NonInteractive is always set to true. This is tracked by https://github.com/NuGet/Home/issues/10620
                        NonInteractive = true,
                        Timestamper = timestamper.Value(),
                        TimestampHashAlgorithm = timestampHashAlgorithm
                    };

                    setLogLevel(XPlatUtility.MSBuildVerbosityToNuGetLogLevel(verbosity.Value()));

                    X509TrustStore.InitializeForDotNetSdk(args.Logger);

                    ISignCommandRunner runner = getCommandRunner();
                    int result = await runner.ExecuteCommandAsync(args);
                    return result;
                });
            });
        }

        private static void ValidatePackagePaths(CommandArgument argument)
        {
            if (argument.Values.Count == 0 ||
                argument.Values.Any<string>(packagePath => string.IsNullOrEmpty(packagePath)))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PkgMissingArgument,
                    CommandName,
                    argument.Name));
            }
        }

        private static void WarnIfNoTimestamper(ILogger logger, CommandOption timeStamper)
        {
            if (!timeStamper.HasValue())
            {
                logger.Log(LogMessage.CreateWarning(NuGetLogCode.NU3002, Strings.SignCommandNoTimestamperWarning));
            }
        }

        private static void ValidateAndCreateOutputDirectory(CommandOption output)
        {
            if (output.HasValue())
            {
                string outputDir = output.Value();

                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
        }

        private static StoreLocation ValidateAndParseStoreLocation(CommandOption location)
        {
            StoreLocation storeLocation = StoreLocation.CurrentUser;

            if (location.HasValue())
            {
                if (!string.IsNullOrEmpty(location.Value()) &&
                    !Enum.TryParse(location.Value(), ignoreCase: true, result: out storeLocation))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        Strings.Err_InvalidValue,
                        location.LongName,
                        string.Join(",", Enum.GetValues(typeof(StoreLocation)).Cast<StoreLocation>().ToList())));
                }
            }

            return storeLocation;
        }

        private static StoreName ValidateAndParseStoreName(CommandOption store)
        {
            StoreName storeName = StoreName.My;

            if (store.HasValue())
            {
                if (!string.IsNullOrEmpty(store.Value()) &&
                    !Enum.TryParse(store.Value(), ignoreCase: true, result: out storeName))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        Strings.Err_InvalidValue,
                        store.LongName,
                        string.Join(",", Enum.GetValues(typeof(StoreName)).Cast<StoreName>().ToList())));
                }
            }

            return storeName;
        }

        private static void ValidateCertificateInputs(CommandOption path, CommandOption fingerprint,
                                                      CommandOption subject, CommandOption store, CommandOption location, ILogger logger)
        {
            if (string.IsNullOrEmpty(path.Value()) &&
                string.IsNullOrEmpty(fingerprint.Value()) &&
                string.IsNullOrEmpty(subject.Value()))
            {
                // Throw if user gave no certificate input
                throw new ArgumentException(Strings.SignCommandNoCertificateException);
            }
            else if (!string.IsNullOrEmpty(path.Value()) &&
                (!string.IsNullOrEmpty(fingerprint.Value()) ||
                 !string.IsNullOrEmpty(subject.Value()) ||
                 !string.IsNullOrEmpty(location.Value()) ||
                 !string.IsNullOrEmpty(store.Value())))
            {
                // Throw if the user provided a path and any one of the other options
                throw new ArgumentException(Strings.SignCommandMultipleCertificateException);
            }
            else if (!string.IsNullOrEmpty(fingerprint.Value()) && !string.IsNullOrEmpty(subject.Value()))
            {
                // Throw if the user provided a fingerprint and a subject
                throw new ArgumentException(Strings.SignCommandMultipleCertificateException);
            }
            else if (fingerprint.Value() != null)
            {
                bool isValidFingerprint = CertificateUtility.TryDeduceHashAlgorithm(fingerprint.Value(), out HashAlgorithmName hashAlgorithmName);
                bool isSHA1 = hashAlgorithmName == HashAlgorithmName.SHA1;
                int assemblyVersion = typeof(int).Assembly.GetName().Version.Major;
                string message = string.Format(CultureInfo.CurrentCulture, Strings.SignCommandInvalidCertificateFingerprint, NuGetLogCode.NU3043);

                if (!isValidFingerprint || (assemblyVersion >= 10 && isSHA1))
                {
                    throw new ArgumentException(message);
                }
                else if (isSHA1)
                {
                    logger.Log(LogMessage.Create(LogLevel.Warning, message));
                }
            }
        }
    }
}
