// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;

namespace NuGet.CommandLine.XPlat
{
    internal static class PushCommand
    {
        public static void Register(CommandLineApplication app, Func<ILogger> getLogger)
        {
            app.Command("push", push =>
            {
                push.Description = Strings.Push_Description;
                push.HelpOption(XPlatUtility.HelpOption);

                push.Option(
                    CommandConstants.ForceEnglishOutputOption,
                    Strings.ForceEnglishOutput_Description,
                    CommandOptionType.NoValue);

                var source = push.Option(
                    "-s|--source <source>",
                    Strings.Source_Description,
                    CommandOptionType.SingleValue);

                var symbolSource = push.Option(
                    "-ss|--symbol-source <source>",
                    Strings.SymbolSource_Description,
                    CommandOptionType.SingleValue);

                var timeout = push.Option(
                    "-t|--timeout <timeout>",
                    Strings.Push_Timeout_Description,
                    CommandOptionType.SingleValue);

                var apikey = push.Option(
                    "-k|--api-key <apiKey>",
                    Strings.ApiKey_Description,
                    CommandOptionType.SingleValue);

                var symbolApiKey = push.Option(
                    "-sk|--symbol-api-key <apiKey>",
                    Strings.SymbolApiKey_Description,
                    CommandOptionType.SingleValue);

                var disableBuffering = push.Option(
                    "-d|--disable-buffering",
                    Strings.DisableBuffering_Description,
                    CommandOptionType.NoValue);

                var noSymbols = push.Option(
                    "-n|--no-symbols",
                    Strings.NoSymbols_Description,
                    CommandOptionType.NoValue);

                var arguments = push.Argument(
                    "[root]",
                    Strings.Push_Package_ApiKey_Description,
                    multipleValues: true);

                var noServiceEndpointDescription = push.Option(
                    "--no-service-endpoint",
                    Strings.NoServiceEndpoint_Description,
                    CommandOptionType.NoValue);

                var interactive = push.Option(
                    "--interactive",
                    Strings.NuGetXplatCommand_Interactive,
                    CommandOptionType.NoValue);

                var skipDuplicate = push.Option(
                    "--skip-duplicate",
                    Strings.PushCommandSkipDuplicateDescription,
                    CommandOptionType.NoValue);

                var configurationFile = push.Option(
                    "--configfile",
                    Strings.Option_ConfigFile,
                    CommandOptionType.SingleValue);

                push.OnExecute(async () =>
                {
                    if (arguments.Values.Count < 1)
                    {
                        throw new ArgumentException(Strings.Push_MissingArguments);
                    }

                    IList<string> packagePaths = arguments.Values;
                    string sourcePath = source.Value();
                    string apiKeyValue = apikey.Value();
                    string symbolSourcePath = symbolSource.Value();
                    string symbolApiKeyValue = symbolApiKey.Value();
                    bool disableBufferingValue = disableBuffering.HasValue();
                    bool noSymbolsValue = noSymbols.HasValue();
                    bool noServiceEndpoint = noServiceEndpointDescription.HasValue();
                    bool skipDuplicateValue = skipDuplicate.HasValue();
                    int timeoutSeconds = 0;

                    if (timeout.HasValue() && !int.TryParse(timeout.Value(), out timeoutSeconds))
                    {
                        throw new ArgumentException(Strings.Push_InvalidTimeout);
                    }

#pragma warning disable CS0618 // Type or member is obsolete
                    var sourceProvider = new PackageSourceProvider(XPlatUtility.ProcessConfigFile(configurationFile.Value()), enablePackageSourcesChangedEvent: false);
#pragma warning restore CS0618 // Type or member is obsolete

                    try
                    {
                        DefaultCredentialServiceUtility.SetupDefaultCredentialService(getLogger(), !interactive.HasValue());
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
                            getLogger());
                    }
                    catch (TaskCanceledException ex)
                    {
                        throw new AggregateException(ex, new Exception(Strings.Push_Timeout_Error));
                    }

                    return 0;
                });
            });
        }
    }
}
