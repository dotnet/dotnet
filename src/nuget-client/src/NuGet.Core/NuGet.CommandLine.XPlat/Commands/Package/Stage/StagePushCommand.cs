// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Credentials;
using NuGet.Protocol;

namespace NuGet.CommandLine.XPlat.Commands.Package.Stage
{
    internal static class StagePushCommand
    {
        internal static void Register(
            Command stageCommand,
            Option<bool> interactiveOption,
            Func<ILoggerWithColor> getLogger)
        {
            Register(
                stageCommand,
                interactiveOption,
                getLogger,
                SetupSettingsAndRunAsync);
        }

        internal static Task<int> SetupSettingsAndRunAsync(StagePushCommandArgs args)
        {
            DefaultCredentialServiceUtility.SetupDefaultCredentialService(
                args.Logger,
                !args.Interactive);

            ISettings settings = XPlatUtility.ProcessConfigFile(args.ConfigFile);
            var packageSourceProvider = new PackageSourceProvider(settings);
            var sourceRepositoryProvider = new CachingSourceProvider(packageSourceProvider);

            return StagePushCommandRunner.RunAsync(
                args,
                settings,
                packageSourceProvider,
                sourceRepositoryProvider);
        }

        internal static void Register(
            Command stageCommand,
            Option<bool> interactiveOption,
            Func<ILoggerWithColor> getLogger,
            Func<StagePushCommandArgs, Task<int>> action)
        {
            var pushCommand = new DocumentedCommand(
                "push",
                Strings.StagePushCommand_Description,
                "https://aka.ms/dotnet/package/stage");

            var packagePathArgument = new Argument<string>("PACKAGE_PATH")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.StagePushCommand_PackagePathArgument_Description,
            };

            var sourceOption = new Option<string>("--source", "-s")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.Source_Description,
            };

            var apiKeyOption = new Option<string>("--api-key", "-k")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.ApiKey_Description,
            };

            var groupOption = new Option<string>("--group")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.StagePushCommand_GroupOption_Description,
            };

            var noSymbolsOption = new Option<bool>("--no-symbols")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.StagePushCommand_NoSymbolsOption_Description,
            };

            var configFileOption = new Option<string>("--configfile")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Strings.Option_ConfigFile,
            };

            pushCommand.Arguments.Add(packagePathArgument);
            pushCommand.Options.Add(sourceOption);
            pushCommand.Options.Add(apiKeyOption);
            pushCommand.Options.Add(groupOption);
            pushCommand.Options.Add(noSymbolsOption);
            pushCommand.Options.Add(configFileOption);
            pushCommand.Options.Add(interactiveOption);

            pushCommand.SetAction(async (parseResult, cancellationToken) =>
            {
                ILoggerWithColor logger = getLogger();

                try
                {
                    var args = new StagePushCommandArgs
                    {
                        PackagePath = parseResult.GetValue(packagePathArgument)!,
                        Source = parseResult.GetValue(sourceOption),
                        ApiKey = parseResult.GetValue(apiKeyOption),
                        GroupId = parseResult.GetValue(groupOption),
                        NoSymbols = parseResult.GetValue(noSymbolsOption),
                        ConfigFile = parseResult.GetValue(configFileOption),
                        Interactive = parseResult.GetValue(interactiveOption),
                        Logger = logger,
                        CancellationToken = cancellationToken,
                    };

                    return await action(args);
                }
                catch (ArgumentException ex)
                {
                    logger.LogError(ex.Message);
                    return ExitCodes.InvalidArguments;
                }
            });

            stageCommand.Subcommands.Add(pushCommand);
        }
    }
}
