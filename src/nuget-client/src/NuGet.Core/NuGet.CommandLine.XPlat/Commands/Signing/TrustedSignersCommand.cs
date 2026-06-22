// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.CommandLine;
using System.Globalization;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Signing;
using static NuGet.Commands.TrustedSignersArgs;

namespace NuGet.CommandLine.XPlat
{
    internal static class TrustedSignersCommand
    {
        internal static void Register(Command parent,
                      Func<ILoggerWithColor> getLogger,
                      Action<LogLevel> setLogLevel)
        {
            var trustedSignersCmd = new Command("trust", Strings.TrustCommandDescription);

            // --- list subcommand ---
            var listCommand = new Command("list", Strings.TrustListCommandDescription);
            {
                var configFile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile, Arity = ArgumentArity.ZeroOrOne };
                var verbosity = CreateVerbosityOption();

                listCommand.Options.Add(configFile);
                listCommand.Options.Add(verbosity);

                listCommand.SetAction(async (parseResult, cancellationToken) =>
                {
                    return await ExecuteCommand(TrustCommand.List, algorithm: null, allowUntrustedRootOption: false, owners: null, parseResult.GetValue(verbosity), parseResult.GetValue(configFile), getLogger, setLogLevel);
                });
            }
            trustedSignersCmd.Subcommands.Add(listCommand);

            // --- sync subcommand ---
            var syncCommand = new Command("sync", Strings.TrustSyncCommandDescription);
            {
                var name = new Argument<string>("NAME") { Description = Strings.TrustedSignerNameExists };
                var configFile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile, Arity = ArgumentArity.ZeroOrOne };
                var verbosity = CreateVerbosityOption();

                syncCommand.Arguments.Add(name);
                syncCommand.Options.Add(configFile);
                syncCommand.Options.Add(verbosity);

                syncCommand.SetAction(async (parseResult, cancellationToken) =>
                {
                    return await ExecuteCommand(TrustCommand.Sync, algorithm: null, allowUntrustedRootOption: false, owners: null, parseResult.GetValue(verbosity), parseResult.GetValue(configFile), getLogger, setLogLevel, name: parseResult.GetValue(name));
                });
            }
            trustedSignersCmd.Subcommands.Add(syncCommand);

            // --- remove subcommand ---
            var removeCommand = new Command("remove", Strings.TrustRemoveCommandDescription);
            {
                var name = new Argument<string>("NAME") { Description = Strings.TrustedSignerNameToRemove };
                var configFile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile, Arity = ArgumentArity.ZeroOrOne };
                var verbosity = CreateVerbosityOption();

                removeCommand.Arguments.Add(name);
                removeCommand.Options.Add(configFile);
                removeCommand.Options.Add(verbosity);

                removeCommand.SetAction(async (parseResult, cancellationToken) =>
                {
                    return await ExecuteCommand(TrustCommand.Remove, algorithm: null, allowUntrustedRootOption: false, owners: null, parseResult.GetValue(verbosity), parseResult.GetValue(configFile), getLogger, setLogLevel, name: parseResult.GetValue(name));
                });
            }
            trustedSignersCmd.Subcommands.Add(removeCommand);

            // --- author subcommand ---
            var authorCommand = new Command("author", Strings.TrustAuthorCommandDescription);
            {
                var name = new Argument<string>("NAME") { Description = Strings.TrustedSignerNameToAdd };
                var package = new Argument<string>("PACKAGE") { Description = Strings.TrustLocalSignedNupkgPath };
                var allowUntrustedRootOption = new Option<bool>("--allow-untrusted-root") { Description = Strings.TrustCommandAllowUntrustedRoot, Arity = ArgumentArity.Zero };
                var configFile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile, Arity = ArgumentArity.ZeroOrOne };
                var verbosity = CreateVerbosityOption();

                authorCommand.Arguments.Add(name);
                authorCommand.Arguments.Add(package);
                authorCommand.Options.Add(allowUntrustedRootOption);
                authorCommand.Options.Add(configFile);
                authorCommand.Options.Add(verbosity);

                authorCommand.SetAction(async (parseResult, cancellationToken) =>
                {
                    return await ExecuteCommand(TrustCommand.Author, algorithm: null, parseResult.GetValue(allowUntrustedRootOption), owners: null, parseResult.GetValue(verbosity), parseResult.GetValue(configFile), getLogger, setLogLevel, name: parseResult.GetValue(name), sourceUrl: null, packagePath: parseResult.GetValue(package));
                });
            }
            trustedSignersCmd.Subcommands.Add(authorCommand);

            // --- repository subcommand ---
            var repositoryCommand = new Command("repository", Strings.TrustRepositoryCommandDescription);
            {
                var name = new Argument<string>("NAME") { Description = Strings.TrustedSignerNameToAdd };
                var package = new Argument<string>("PACKAGE") { Description = Strings.TrustLocalSignedNupkgPath };
                var allowUntrustedRootOption = new Option<bool>("--allow-untrusted-root") { Description = Strings.TrustCommandAllowUntrustedRoot, Arity = ArgumentArity.Zero };
                var owners = new Option<string>("--owners") { Description = Strings.TrustCommandOwners, Arity = ArgumentArity.ZeroOrOne };
                var configFile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile, Arity = ArgumentArity.ZeroOrOne };
                var verbosity = CreateVerbosityOption();

                repositoryCommand.Arguments.Add(name);
                repositoryCommand.Arguments.Add(package);
                repositoryCommand.Options.Add(allowUntrustedRootOption);
                repositoryCommand.Options.Add(owners);
                repositoryCommand.Options.Add(configFile);
                repositoryCommand.Options.Add(verbosity);

                repositoryCommand.SetAction(async (parseResult, cancellationToken) =>
                {
                    return await ExecuteCommand(TrustCommand.Repository, algorithm: null, parseResult.GetValue(allowUntrustedRootOption), owners: parseResult.GetValue(owners), parseResult.GetValue(verbosity), parseResult.GetValue(configFile), getLogger, setLogLevel, name: parseResult.GetValue(name), sourceUrl: null, packagePath: parseResult.GetValue(package));
                });
            }
            trustedSignersCmd.Subcommands.Add(repositoryCommand);

            // --- certificate subcommand ---
            var certificateCommand = new Command("certificate", Strings.TrustRepositoryCommandDescription);
            {
                var name = new Argument<string>("NAME") { Description = Strings.TrustedCertificateSignerNameToAdd };
                var fingerprint = new Argument<string>("FINGERPRINT") { Description = Strings.TrustCertificateFingerprint };
                var algorithm = new Option<string>("--algorithm") { Description = Strings.TrustCommandAlgorithm, Arity = ArgumentArity.ZeroOrOne };
                var allowUntrustedRootOption = new Option<bool>("--allow-untrusted-root") { Description = Strings.TrustCommandAllowUntrustedRoot, Arity = ArgumentArity.Zero };
                var configFile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile, Arity = ArgumentArity.ZeroOrOne };
                var verbosity = CreateVerbosityOption();

                certificateCommand.Arguments.Add(name);
                certificateCommand.Arguments.Add(fingerprint);
                certificateCommand.Options.Add(algorithm);
                certificateCommand.Options.Add(allowUntrustedRootOption);
                certificateCommand.Options.Add(configFile);
                certificateCommand.Options.Add(verbosity);

                certificateCommand.SetAction(async (parseResult, cancellationToken) =>
                {
                    return await ExecuteCommand(TrustCommand.Certificate, algorithm: parseResult.GetValue(algorithm), parseResult.GetValue(allowUntrustedRootOption), owners: null, parseResult.GetValue(verbosity), parseResult.GetValue(configFile), getLogger, setLogLevel, name: parseResult.GetValue(name), sourceUrl: null, packagePath: null, fingerprint: parseResult.GetValue(fingerprint));
                });
            }
            trustedSignersCmd.Subcommands.Add(certificateCommand);

            // --- source subcommand ---
            var sourceCommand = new Command("source", Strings.TrustSourceCommandDescription);
            {
                var name = new Argument<string>("NAME") { Description = Strings.TrustSourceSignerName };
                var sourceUrl = new Option<string>("--source-url") { Description = Strings.TrustSourceUrl, Arity = ArgumentArity.ZeroOrOne };
                var owners = new Option<string>("--owners") { Description = Strings.TrustCommandOwners, Arity = ArgumentArity.ZeroOrOne };
                var configFile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile, Arity = ArgumentArity.ZeroOrOne };
                var verbosity = CreateVerbosityOption();

                sourceCommand.Arguments.Add(name);
                sourceCommand.Options.Add(sourceUrl);
                sourceCommand.Options.Add(owners);
                sourceCommand.Options.Add(configFile);
                sourceCommand.Options.Add(verbosity);

                sourceCommand.SetAction(async (parseResult, cancellationToken) =>
                {
                    return await ExecuteCommand(TrustCommand.Source, algorithm: null, allowUntrustedRootOption: false, owners: parseResult.GetValue(owners), parseResult.GetValue(verbosity), parseResult.GetValue(configFile), getLogger, setLogLevel, name: parseResult.GetValue(name), sourceUrl: parseResult.GetValue(sourceUrl));
                });
            }
            trustedSignersCmd.Subcommands.Add(sourceCommand);

            // --- Main command (defaults to list behavior) ---
            var mainConfigFile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile, Arity = ArgumentArity.ZeroOrOne };
            var mainVerbosity = CreateVerbosityOption();

            trustedSignersCmd.Options.Add(mainConfigFile);
            trustedSignersCmd.Options.Add(mainVerbosity);

            trustedSignersCmd.SetAction(async (parseResult, cancellationToken) =>
            {
                // If no command specified then default to List command.
                return await ExecuteCommand(TrustCommand.List, algorithm: null, allowUntrustedRootOption: false, owners: null, parseResult.GetValue(mainVerbosity), parseResult.GetValue(mainConfigFile), getLogger, setLogLevel);
            });

            parent.Subcommands.Add(trustedSignersCmd);
        }

        private static async Task<int> ExecuteCommand(TrustCommand action,
                      string algorithm,
                      bool allowUntrustedRootOption,
                      string owners,
                      string verbosity,
                      string configFile,
                      Func<ILoggerWithColor> getLogger,
                      Action<LogLevel> setLogLevel,
                      string name = null,
                      string sourceUrl = null,
                      string packagePath = null,
                      string fingerprint = null)
        {
            ILogger logger = getLogger();

            try
            {
                ISettings settings = XPlatUtility.ProcessConfigFile(configFile);

                var trustedSignersArgs = new TrustedSignersArgs()
                {
                    Action = MapTrustEnumAction(action),
                    PackagePath = packagePath,
                    Name = name,
                    ServiceIndex = sourceUrl,
                    CertificateFingerprint = fingerprint,
                    FingerprintAlgorithm = algorithm,
                    AllowUntrustedRoot = allowUntrustedRootOption,
                    Author = action == TrustCommand.Author,
                    Repository = action == TrustCommand.Repository,
                    Owners = CommandLineUtility.SplitAndJoinAcrossMultipleValues(owners != null ? new[] { owners } : null),
                    Logger = logger
                };

                setLogLevel(XPlatUtility.MSBuildVerbosityToNuGetLogLevel(verbosity));

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

        private static Option<string> CreateVerbosityOption()
        {
            return new Option<string>("--verbosity", "-v")
            {
                Description = Strings.Verbosity_Description,
                Arity = ArgumentArity.ZeroOrOne
            };
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
