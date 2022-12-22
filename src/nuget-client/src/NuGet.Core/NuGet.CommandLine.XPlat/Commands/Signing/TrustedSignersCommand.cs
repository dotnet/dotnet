// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Signing;
using static NuGet.Commands.TrustedSignersArgs;

namespace NuGet.CommandLine.XPlat
{
    internal static class TrustedSignersCommand
    {
        internal static void Register(CommandLineApplication app,
                      Func<ILogger> getLogger,
                      Action<LogLevel> setLogLevel)
        {
            app.Command("trust", trustedSignersCmd =>
            {
                // sub-commands
                trustedSignersCmd.Command("list", (listCommand) =>
                {
                    listCommand.Description = Strings.TrustListCommandDescription;
                    CommandOption configFile = listCommand.Option(
                        "--configfile",
                        Strings.Option_ConfigFile,
                        CommandOptionType.SingleValue);

                    listCommand.HelpOption(XPlatUtility.HelpOption);

                    CommandOption verbosity = listCommand.VerbosityOption();

                    listCommand.OnExecute(async () =>
                    {
                        return await ExecuteCommand(TrustCommand.List, algorithm: null, allowUntrustedRootOption: false, owners: null, verbosity, configFile, getLogger, setLogLevel);
                    });
                });

                trustedSignersCmd.Command("sync", (syncCommand) =>
                {
                    syncCommand.Description = Strings.TrustSyncCommandDescription;
                    CommandOption configFile = syncCommand.Option(
                        "--configfile",
                        Strings.Option_ConfigFile,
                        CommandOptionType.SingleValue);

                    syncCommand.HelpOption(XPlatUtility.HelpOption);

                    CommandOption verbosity = syncCommand.VerbosityOption(); ;

                    CommandArgument name = syncCommand.Argument("<NAME>",
                                               Strings.TrustedSignerNameExists);

                    syncCommand.OnExecute(async () =>
                    {
                        return await ExecuteCommand(TrustCommand.Sync, algorithm: null, allowUntrustedRootOption: false, owners: null, verbosity, configFile, getLogger, setLogLevel, name: name.Value);
                    });
                });

                trustedSignersCmd.Command("remove", (syncCommand) =>
                {
                    syncCommand.Description = Strings.TrustRemoveCommandDescription;
                    CommandOption configFile = syncCommand.Option(
                        "--configfile",
                        Strings.Option_ConfigFile,
                        CommandOptionType.SingleValue);

                    syncCommand.HelpOption(XPlatUtility.HelpOption);

                    CommandOption verbosity = syncCommand.VerbosityOption();

                    CommandArgument name = syncCommand.Argument("<NAME>",
                                               Strings.TrustedSignerNameToRemove);

                    syncCommand.OnExecute(async () =>
                    {
                        return await ExecuteCommand(TrustCommand.Remove, algorithm: null, allowUntrustedRootOption: false, owners: null, verbosity, configFile, getLogger, setLogLevel, name: name.Value);
                    });
                });

                trustedSignersCmd.Command("author", (authorCommand) =>
                {
                    authorCommand.Description = Strings.TrustAuthorCommandDescription;

                    CommandOption allowUntrustedRootOption = authorCommand.Option(
                                                "--allow-untrusted-root",
                                                Strings.TrustCommandAllowUntrustedRoot,
                                                CommandOptionType.NoValue);

                    CommandOption configFile = authorCommand.Option(
                        "--configfile",
                        Strings.Option_ConfigFile,
                        CommandOptionType.SingleValue);

                    authorCommand.HelpOption(XPlatUtility.HelpOption);

                    CommandOption verbosity = authorCommand.VerbosityOption();

                    CommandArgument name = authorCommand.Argument("<NAME>",
                                               Strings.TrustedSignerNameToAdd);
                    CommandArgument package = authorCommand.Argument("<PACKAGE>",
                                               Strings.TrustLocalSignedNupkgPath);

                    authorCommand.OnExecute(async () =>
                    {
                        return await ExecuteCommand(TrustCommand.Author, algorithm: null, allowUntrustedRootOption.HasValue(), owners: null, verbosity, configFile, getLogger, setLogLevel, name: name.Value, sourceUrl: null, packagePath: package.Value);
                    });
                });

                trustedSignersCmd.Command("repository", (repositoryCommand) =>
                {
                    repositoryCommand.Description = Strings.TrustRepositoryCommandDescription;

                    CommandOption allowUntrustedRootOption = repositoryCommand.Option(
                                                "--allow-untrusted-root",
                                                Strings.TrustCommandAllowUntrustedRoot,
                                                CommandOptionType.NoValue);

                    CommandOption configFile = repositoryCommand.Option(
                        "--configfile",
                        Strings.Option_ConfigFile,
                        CommandOptionType.SingleValue);

                    repositoryCommand.HelpOption(XPlatUtility.HelpOption);

                    CommandOption owners = repositoryCommand.Option(
                        "--owners",
                        Strings.TrustCommandOwners,
                        CommandOptionType.SingleValue);

                    CommandOption verbosity = repositoryCommand.VerbosityOption();

                    CommandArgument name = repositoryCommand.Argument("<NAME>",
                                               Strings.TrustedSignerNameToAdd);
                    CommandArgument package = repositoryCommand.Argument("<PACKAGE>",
                                               Strings.TrustLocalSignedNupkgPath);

                    repositoryCommand.OnExecute(async () =>
                    {
                        return await ExecuteCommand(TrustCommand.Repository, algorithm: null, allowUntrustedRootOption.HasValue(), owners: owners, verbosity, configFile, getLogger, setLogLevel, name: name.Value, sourceUrl: null, packagePath: package.Value);
                    });
                });

                trustedSignersCmd.Command("certificate", (certificateCommand) =>
                {
                    certificateCommand.Description = Strings.TrustRepositoryCommandDescription;

                    CommandOption algorithm = certificateCommand.Option(
                        "--algorithm",
                        Strings.TrustCommandAlgorithm,
                        CommandOptionType.SingleValue);

                    CommandOption allowUntrustedRootOption = certificateCommand.Option(
                                                "--allow-untrusted-root",
                                                Strings.TrustCommandAllowUntrustedRoot,
                                                CommandOptionType.NoValue);

                    CommandOption configFile = certificateCommand.Option(
                        "--configfile",
                        Strings.Option_ConfigFile,
                        CommandOptionType.SingleValue);

                    certificateCommand.HelpOption(XPlatUtility.HelpOption);

                    CommandOption verbosity = certificateCommand.VerbosityOption();

                    CommandArgument name = certificateCommand.Argument("<NAME>",
                                               Strings.TrustedCertificateSignerNameToAdd);
                    CommandArgument fingerprint = certificateCommand.Argument("<FINGERPRINT>",
                                               Strings.TrustCertificateFingerprint);

                    certificateCommand.OnExecute(async () =>
                    {
                        return await ExecuteCommand(TrustCommand.Certificate, algorithm, allowUntrustedRootOption.HasValue(), owners: null, verbosity, configFile, getLogger, setLogLevel, name: name.Value, sourceUrl: null, packagePath: null, fingerprint: fingerprint.Value);
                    });
                });

                trustedSignersCmd.Command("source", (sourceCommand) =>
                {
                    sourceCommand.Description = Strings.TrustSourceCommandDescription;

                    CommandOption configFile = sourceCommand.Option(
                        "--configfile",
                        Strings.Option_ConfigFile,
                        CommandOptionType.SingleValue);

                    sourceCommand.HelpOption(XPlatUtility.HelpOption);

                    CommandOption owners = sourceCommand.Option(
                        "--owners",
                        Strings.TrustCommandOwners,
                        CommandOptionType.SingleValue);

                    CommandOption sourceUrl = sourceCommand.Option(
                        "--source-url",
                        Strings.TrustSourceUrl,
                        CommandOptionType.SingleValue);

                    CommandOption verbosity = sourceCommand.VerbosityOption();

                    CommandArgument name = sourceCommand.Argument("<NAME>",
                        Strings.TrustSourceSignerName);

                    sourceCommand.OnExecute(async () =>
                    {
                        return await ExecuteCommand(TrustCommand.Source, algorithm: null, allowUntrustedRootOption: false, owners, verbosity, configFile, getLogger, setLogLevel, name: name.Value, sourceUrl: sourceUrl.Value());
                    });
                });

                // Main command
                trustedSignersCmd.Description = Strings.TrustCommandDescription;
                CommandOption mainConfigFile = trustedSignersCmd.Option(
                    "--configfile",
                    Strings.Option_ConfigFile,
                    CommandOptionType.SingleValue);

                trustedSignersCmd.HelpOption(XPlatUtility.HelpOption);

                CommandOption mainVerbosity = trustedSignersCmd.VerbosityOption();

                trustedSignersCmd.OnExecute(async () =>
                {
                    // If no command specified then default to List command.
                    return await ExecuteCommand(TrustCommand.List, algorithm: null, allowUntrustedRootOption: false, owners: null, mainVerbosity, mainConfigFile, getLogger, setLogLevel);
                });
            });
        }

        private static async Task<int> ExecuteCommand(TrustCommand action,
                      CommandOption algorithm,
                      bool allowUntrustedRootOption,
                      CommandOption owners,
                      CommandOption verbosity,
                      CommandOption configFile,
                      Func<ILogger> getLogger,
                      Action<LogLevel> setLogLevel,
                      string name = null,
                      string sourceUrl = null,
                      string packagePath = null,
                      string fingerprint = null)
        {
            ILogger logger = getLogger();

            try
            {
                ISettings settings = XPlatUtility.ProcessConfigFile(configFile.Value());

                var trustedSignersArgs = new TrustedSignersArgs()
                {
                    Action = MapTrustEnumAction(action),
                    PackagePath = packagePath,
                    Name = name,
                    ServiceIndex = sourceUrl,
                    CertificateFingerprint = fingerprint,
                    FingerprintAlgorithm = algorithm?.Value(),
                    AllowUntrustedRoot = allowUntrustedRootOption,
                    Author = action == TrustCommand.Author,
                    Repository = action == TrustCommand.Repository,
                    Owners = CommandLineUtility.SplitAndJoinAcrossMultipleValues(owners?.Values),
                    Logger = logger
                };

                setLogLevel(XPlatUtility.MSBuildVerbosityToNuGetLogLevel(verbosity.Value()));

                // Add is the only action which does certificate chain building.
                if (trustedSignersArgs.Action == TrustedSignersAction.Add)
                {
                    X509TrustStore.InitializeForDotNetSdk(logger);
                }

#pragma warning disable CS0618 // Type or member is obsolete
                var sourceProvider = new PackageSourceProvider(settings, enablePackageSourcesChangedEvent: false);
#pragma warning restore CS0618 // Type or member is obsolete
                var trustedSignersProvider = new TrustedSignersProvider(settings);

                var runner = new TrustedSignersCommandRunner(trustedSignersProvider, sourceProvider);
                Task<int> trustedSignTask = runner.ExecuteCommandAsync(trustedSignersArgs);
                return await trustedSignTask;
            }
            catch (InvalidOperationException e)
            {
                // nuget trust command handled exceptions.
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var error_TrustedSignerAlreadyExistsMessage = string.Format(CultureInfo.CurrentCulture, Strings.Error_TrustedSignerAlreadyExists, name);

                    if (e.Message == error_TrustedSignerAlreadyExistsMessage)
                    {
                        logger.LogError(error_TrustedSignerAlreadyExistsMessage);
                        return 1;
                    }
                }

                if (!string.IsNullOrWhiteSpace(sourceUrl))
                {
                    var error_TrustedRepoAlreadyExists = string.Format(CultureInfo.CurrentCulture, Strings.Error_TrustedRepoAlreadyExists, sourceUrl);

                    if (e.Message == error_TrustedRepoAlreadyExists)
                    {
                        logger.LogError(error_TrustedRepoAlreadyExists);
                        return 1;
                    }
                }

                throw;
            }
            catch (ArgumentException e)
            {
                if (e.Data is System.Collections.IDictionary)
                {
                    logger.LogError(string.Format(CultureInfo.CurrentCulture, Strings.Error_TrustFingerPrintAlreadyExist));
                    return 1;
                }

                throw;
            }
        }

        private static CommandOption VerbosityOption(this CommandLineApplication command)
        {
            return command.Option(
                "-v|--verbosity",
                Strings.Verbosity_Description,
                CommandOptionType.SingleValue);
        }

        private static TrustedSignersAction MapTrustEnumAction(TrustCommand trustCommand)
        {
            switch (trustCommand)
            {
                case TrustCommand.List:
                    return TrustedSignersAction.List;
                case TrustCommand.Remove:
                    return TrustedSignersAction.Remove;
                case TrustCommand.Sync:
                    return TrustedSignersAction.Sync;
                default:
                    return TrustedSignersAction.Add;
            }
        }
    }
}
