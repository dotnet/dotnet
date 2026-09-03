// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.CommandLine;
using System.Globalization;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.Credentials;

namespace NuGet.CommandLine.XPlat
{
    internal static class DeleteCommand
    {
        internal static void Register(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var deleteCmd = new Command("delete", Strings.Delete_Description);

            var sourceOption = new Option<string>("--source", "-s")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.Source_Description,
            };

            var nonInteractiveOption = new Option<bool>("--non-interactive")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.NonInteractive_Description,
            };

            var apiKeyOption = new Option<string>("--api-key", "-k")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.ApiKey_Description,
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

            var packageIdArgument = new Argument<string>("PackageId")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.Delete_PackageIdAndVersion_Description,
            };

            var packageVersionArgument = new Argument<string>("PackageVersion")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.Delete_PackageIdAndVersion_Description,
            };

            deleteCmd.Options.Add(sourceOption);
            deleteCmd.Options.Add(nonInteractiveOption);
            deleteCmd.Options.Add(apiKeyOption);
            deleteCmd.Options.Add(noServiceEndpointOption);
            deleteCmd.Options.Add(interactiveOption);
            deleteCmd.Arguments.Add(packageIdArgument);
            deleteCmd.Arguments.Add(packageVersionArgument);

            deleteCmd.SetAction(async (parseResult, cancellationToken) =>
            {
                string packageId = parseResult.GetValue(packageIdArgument);
                string packageVersion = parseResult.GetValue(packageVersionArgument);
                string sourcePath = parseResult.GetValue(sourceOption);
                string apiKeyValue = parseResult.GetValue(apiKeyOption);
                bool nonInteractiveValue = parseResult.GetValue(nonInteractiveOption);
                bool noServiceEndpoint = parseResult.GetValue(noServiceEndpointOption);
                bool interactiveValue = parseResult.GetValue(interactiveOption);

                DefaultCredentialServiceUtility.SetupDefaultCredentialService(getLogger(), !interactiveValue);

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

            parent.Subcommands.Add(deleteCmd);
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
                return result != null && result.StartsWith(Strings.ConsoleConfirmMessageAccept, StringComparison.OrdinalIgnoreCase);
            }
            finally
            {
                Console.ForegroundColor = currentColor;
            }
        }
    }
}
