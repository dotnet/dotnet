// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;

namespace NuGet.CommandLine.XPlat
{
    internal static class DeleteCommand
    {
        public static void Register(CommandLineApplication app, Func<ILogger> getLogger)
        {
            app.Command("delete", delete =>
            {
                delete.Description = Strings.Delete_Description;
                delete.HelpOption(XPlatUtility.HelpOption);

                delete.Option(
                    CommandConstants.ForceEnglishOutputOption,
                    Strings.ForceEnglishOutput_Description,
                    CommandOptionType.NoValue);

                var source = delete.Option(
                    "-s|--source <source>",
                    Strings.Source_Description,
                    CommandOptionType.SingleValue);

                var nonInteractive = delete.Option(
                    "--non-interactive",
                    Strings.NonInteractive_Description,
                    CommandOptionType.NoValue);

                var apikey = delete.Option(
                    "-k|--api-key <apiKey>",
                    Strings.ApiKey_Description,
                    CommandOptionType.SingleValue);

                var arguments = delete.Argument(
                    "[root]",
                    Strings.Delete_PackageIdAndVersion_Description,
                    multipleValues: true);

                var noServiceEndpointDescription = delete.Option(
                    "--no-service-endpoint",
                    Strings.NoServiceEndpoint_Description,
                    CommandOptionType.NoValue);

                var interactive = delete.Option(
                    "--interactive",
                    Strings.NuGetXplatCommand_Interactive,
                    CommandOptionType.NoValue);

                delete.OnExecute(async () =>
                {
                    if (arguments.Values.Count < 2)
                    {
                        throw new ArgumentException(Strings.Delete_MissingArguments);
                    }

                    string packageId = arguments.Values[0];
                    string packageVersion = arguments.Values[1];
                    string sourcePath = source.Value();
                    string apiKeyValue = apikey.Value();
                    bool nonInteractiveValue = nonInteractive.HasValue();
                    bool noServiceEndpoint = noServiceEndpointDescription.HasValue();

                    DefaultCredentialServiceUtility.SetupDefaultCredentialService(getLogger(), !interactive.HasValue());

#pragma warning disable CS0618 // Type or member is obsolete
                    PackageSourceProvider sourceProvider = new PackageSourceProvider(XPlatUtility.GetSettingsForCurrentWorkingDirectory(), enablePackageSourcesChangedEvent: false);
#pragma warning restore CS0618 // Type or member is obsolete

                    await DeleteRunner.Run(
                        sourceProvider.Settings,
                        sourceProvider,
                        packageId,
                        packageVersion,
                        sourcePath,
                        apiKeyValue,
                        nonInteractiveValue,
                        noServiceEndpoint,
                        Confirm,
                        getLogger());

                    return 0;
                });
            });
        }

        private static bool Confirm(string description)
        {
            ConsoleColor currentColor = ConsoleColor.Gray;
            try
            {
                currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, Strings.ConsoleConfirmMessage, description));
                var result = Console.ReadLine();
                return result.StartsWith(Strings.ConsoleConfirmMessageAccept, StringComparison.OrdinalIgnoreCase);
            }
            finally
            {
                Console.ForegroundColor = currentColor;
            }
        }
    }
}
