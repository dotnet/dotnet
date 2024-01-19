// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Credentials;

namespace NuGet.CommandLine.XPlat
{
    internal class PackageSearchCommand
    {
        public static void Register(CliCommand rootCommand, Func<ILoggerWithColor> getLogger)
        {
            Register(rootCommand, getLogger, SetupSettingsAndRunSearchAsync);
        }

        public static void Register(CliCommand rootCommand, Func<ILoggerWithColor> getLogger, Func<PackageSearchArgs, string, CancellationToken, Task<int>> setupSettingsAndRunSearchAsync)
        {
            var searchCommand = new CliCommand("search", Strings.pkgSearch_Description);

            var searchTerm = new CliArgument<string>("Search Term")
            {
                Description = Strings.pkgSearch_termDescription,
                Arity = ArgumentArity.ZeroOrOne,
            };

            var sources = new CliOption<List<string>>("--source")
            {
                Description = Strings.pkgSearch_SourceDescription,
                Arity = ArgumentArity.OneOrMore
            };

            var exactMatch = new CliOption<bool>("--exact-match")
            {
                Description = Strings.pkgSearch_ExactMatchDescription,
                Arity = ArgumentArity.Zero
            };

            var prerelease = new CliOption<bool>("--prerelease")
            {
                Description = Strings.pkgSearch_PrereleaseDescription,
                Arity = ArgumentArity.Zero
            };

            var interactive = new CliOption<bool>("--interactive")
            {
                Description = Strings.pkgSearch_InteractiveDescription,
                Arity = ArgumentArity.Zero
            };

            var take = new CliOption<string>("--take")
            {
                Description = Strings.pkgSearch_TakeDescription,
                Arity = ArgumentArity.ExactlyOne
            };

            var skip = new CliOption<string>("--skip")
            {
                Description = Strings.pkgSearch_SkipDescription,
                Arity = ArgumentArity.ExactlyOne
            };

            var format = new CliOption<string>("--format")
            {
                Description = Strings.pkgSearch_FormatDescription,
                Arity = ArgumentArity.ExactlyOne
            };

            var verbosity = new CliOption<string>("--verbosity")
            {
                Description = Strings.pkgSearch_VerbosityDescription,
                Arity = ArgumentArity.ExactlyOne
            };

            var configFile = new CliOption<string>("--configfile")
            {
                Description = Strings.Option_ConfigFile,
                Arity = ArgumentArity.ExactlyOne
            };

            var help = new HelpOption()
            {
                Arity = ArgumentArity.Zero
            };

            searchCommand.Arguments.Add(searchTerm);
            searchCommand.Options.Add(sources);
            searchCommand.Options.Add(exactMatch);
            searchCommand.Options.Add(prerelease);
            searchCommand.Options.Add(interactive);
            searchCommand.Options.Add(take);
            searchCommand.Options.Add(skip);
            searchCommand.Options.Add(format);
            searchCommand.Options.Add(verbosity);
            searchCommand.Options.Add(configFile);
            searchCommand.Options.Add(help);

            searchCommand.SetAction(async (parserResult, cancelationToken) =>
            {
                ILoggerWithColor logger = getLogger();

                try
                {
                    var packageSearchArgs = new PackageSearchArgs(parserResult.GetValue(skip), parserResult.GetValue(take), parserResult.GetValue(format), parserResult.GetValue(verbosity))
                    {
                        Sources = parserResult.GetValue(sources),
                        SearchTerm = parserResult.GetValue(searchTerm),
                        ExactMatch = parserResult.GetValue(exactMatch),
                        Interactive = parserResult.GetValue(interactive),
                        Prerelease = parserResult.GetValue(prerelease),
                        Logger = logger,
                    };

                    return await setupSettingsAndRunSearchAsync(packageSearchArgs, parserResult.GetValue(configFile), cancelationToken);
                }
                catch (ArgumentException ex)
                {
                    logger.LogError(ex.Message);
                    return 1;
                }
            });

            rootCommand.Subcommands.Add(searchCommand);
        }

        public static async Task<int> SetupSettingsAndRunSearchAsync(PackageSearchArgs packageSearchArgs, string configFile, CancellationToken cancellationToken)
        {
            DefaultCredentialServiceUtility.SetupDefaultCredentialService(packageSearchArgs.Logger, !packageSearchArgs.Interactive);

            ISettings settings = Settings.LoadDefaultSettings(
                Directory.GetCurrentDirectory(),
                configFileName: configFile,
                machineWideSettings: new XPlatMachineWideSetting());
            PackageSourceProvider sourceProvider = new PackageSourceProvider(settings);

            return await PackageSearchRunner.RunAsync(
                sourceProvider,
                packageSearchArgs,
                cancellationToken);
        }
    }
}
