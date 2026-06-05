// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.CommandLine;
using System.Threading.Tasks;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Credentials;

namespace NuGet.CommandLine.XPlat
{
    internal static class PushCommand
    {
        internal static void Register(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var pushCmd = new Command("push", Strings.Push_Description);

            var sourceOption = new Option<string>("--source", "-s")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.Source_Description,
            };

            var allowInsecureConnectionsOption = new Option<bool>("--allow-insecure-connections")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.AllowInsecureConnections_Description,
            };

            var symbolSourceOption = new Option<string>("--symbol-source", "-ss")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.SymbolSource_Description,
            };

            var timeoutOption = new Option<string>("--timeout", "-t")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.Push_Timeout_Description,
            };

            var apiKeyOption = new Option<string>("--api-key", "-k")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.ApiKey_Description,
            };

            var symbolApiKeyOption = new Option<string>("--symbol-api-key", "-sk")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.SymbolApiKey_Description,
            };

            var disableBufferingOption = new Option<bool>("--disable-buffering", "-d")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.DisableBuffering_Description,
            };

            var noSymbolsOption = new Option<bool>("--no-symbols", "-n")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.NoSymbols_Description,
            };

            var noServiceEndpointOption = new Option<bool>("--no-service-endpoint")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.NoServiceEndpoint_Description,
            };

            var interactiveOption = new Option<bool>("--interactive")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.NuGetXplatCommand_Interactive,
            };

            var skipDuplicateOption = new Option<bool>("--skip-duplicate")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.PushCommandSkipDuplicateDescription,
            };

            var configFileOption = new Option<string>("--configfile")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.Option_ConfigFile,
            };

            var packagePathsArgument = new Argument<string[]>("package-paths")
            {
                Arity = ArgumentArity.OneOrMore,
                Description = Strings.Push_Package_ApiKey_Description,
            };

            pushCmd.Options.Add(sourceOption);
            pushCmd.Options.Add(allowInsecureConnectionsOption);
            pushCmd.Options.Add(symbolSourceOption);
            pushCmd.Options.Add(timeoutOption);
            pushCmd.Options.Add(apiKeyOption);
            pushCmd.Options.Add(symbolApiKeyOption);
            pushCmd.Options.Add(disableBufferingOption);
            pushCmd.Options.Add(noSymbolsOption);
            pushCmd.Options.Add(noServiceEndpointOption);
            pushCmd.Options.Add(interactiveOption);
            pushCmd.Options.Add(skipDuplicateOption);
            pushCmd.Options.Add(configFileOption);
            pushCmd.Arguments.Add(packagePathsArgument);

            pushCmd.SetAction(async (parseResult, cancellationToken) =>
            {
                string[]? packagePaths = parseResult.GetValue(packagePathsArgument);
                if (packagePaths == null || packagePaths.Length < 1)
                {
                    throw new ArgumentException(Strings.Push_MissingArguments);
                }

                string? sourcePath = parseResult.GetValue(sourceOption);
                string? apiKeyValue = parseResult.GetValue(apiKeyOption);
                string? symbolSourcePath = parseResult.GetValue(symbolSourceOption);
                string? symbolApiKeyValue = parseResult.GetValue(symbolApiKeyOption);
                bool disableBufferingValue = parseResult.GetValue(disableBufferingOption);
                bool noSymbolsValue = parseResult.GetValue(noSymbolsOption);
                bool noServiceEndpoint = parseResult.GetValue(noServiceEndpointOption);
                bool skipDuplicateValue = parseResult.GetValue(skipDuplicateOption);
                bool allowInsecureConnectionsValue = parseResult.GetValue(allowInsecureConnectionsOption);
                bool interactiveValue = parseResult.GetValue(interactiveOption);
                string? timeoutValue = parseResult.GetValue(timeoutOption);
                string? configFile = parseResult.GetValue(configFileOption);
                int timeoutSeconds = 0;

                if (!string.IsNullOrEmpty(timeoutValue) && !int.TryParse(timeoutValue, out timeoutSeconds))
                {
                    throw new ArgumentException(Strings.Push_InvalidTimeout);
                }

#pragma warning disable CS0618 // Type or member is obsolete
                var sourceProvider = new PackageSourceProvider(XPlatUtility.ProcessConfigFile(configFile), enablePackageSourcesChangedEvent: false);
#pragma warning restore CS0618 // Type or member is obsolete

                try
                {
                    DefaultCredentialServiceUtility.SetupDefaultCredentialService(getLogger(), !interactiveValue);

                    await PushRunner.Run(
                        sourceProvider.Settings,
                        sourceProvider,
                        packagePaths,
                        sourcePath,
                        apiKeyValue,
                        symbolSourcePath,
                        symbolApiKeyValue,
                        timeoutSeconds,
                        disableBufferingValue,
                        noSymbolsValue,
                        noServiceEndpoint,
                        skipDuplicateValue,
                        allowInsecureConnectionsValue,
                        getLogger());
                }
                catch (TaskCanceledException ex)
                {
                    throw new AggregateException(ex, new Exception(Strings.Push_Timeout_Error));
                }

                return 0;
            });

            parent.Subcommands.Add(pushCmd);
        }
    }
}
