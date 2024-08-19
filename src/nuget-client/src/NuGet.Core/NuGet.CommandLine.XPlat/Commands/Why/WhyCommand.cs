// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;

namespace NuGet.CommandLine.XPlat.Commands.Why
{
    internal static class WhyCommand
    {
        internal static void Register(CommandLineApplication app)
        {
            app.Command("why", whyCmd =>
            {
                whyCmd.Description = Strings.WhyCommand_Description;
            });
        }

        internal static void Register(CliCommand rootCommand, Func<ILoggerWithColor> getLogger)
        {
            Register(rootCommand, getLogger, WhyCommandRunner.ExecuteCommand);
        }

        internal static void Register(CliCommand rootCommand, Func<ILoggerWithColor> getLogger, Func<WhyCommandArgs, int> action)
        {
            var whyCommand = new CliCommand("why", Strings.WhyCommand_Description);

            CliArgument<string> path = new CliArgument<string>("PROJECT|SOLUTION")
            {
                Description = Strings.WhyCommand_PathArgument_Description,
                // We really want this to be zero or one, however, because this is the first argument, it doesn't work.
                // Instead, we need to use a CustomParser to choose if the argument is the path, or the package.
                // In order for the parser to tell us there's more than 1 argument available, we need to tell CliArgument
                // that it supports more than one, but then in the custom parser we'll make sure we only take at most 1.
                Arity = ArgumentArity.ZeroOrMore,
                CustomParser = ar =>
                {
                    if (ar.Tokens.Count > 1)
                    {
                        var value = ar.Tokens[0];
                        ar.OnlyTake(1);
                        return value.Value;
                    }

                    ar.OnlyTake(0);
                    var currentDirectory = Directory.GetCurrentDirectory();
                    return currentDirectory;
                }
            };

            CliArgument<string> package = new CliArgument<string>("PACKAGE")
            {
                Description = Strings.WhyCommand_PackageArgument_Description,
                Arity = ArgumentArity.ExactlyOne
            };

            CliOption<List<string>> frameworks = new CliOption<List<string>>("--framework", "-f")
            {
                Description = Strings.WhyCommand_FrameworksOption_Description,
                Arity = ArgumentArity.OneOrMore
            };

            HelpOption help = new HelpOption()
            {
                Arity = ArgumentArity.Zero
            };

            whyCommand.Arguments.Add(path);
            whyCommand.Arguments.Add(package);
            whyCommand.Options.Add(frameworks);
            whyCommand.Options.Add(help);

            whyCommand.SetAction((parseResult) =>
            {
                ILoggerWithColor logger = getLogger();

                try
                {
                    var whyCommandArgs = new WhyCommandArgs(
                        parseResult.GetValue(path),
                        parseResult.GetValue(package),
                        parseResult.GetValue(frameworks),
                        logger);

                    int exitCode = action(whyCommandArgs);
                    return exitCode;
                }
                catch (ArgumentException ex)
                {
                    logger.LogError(ex.Message);
                    return ExitCodes.InvalidArguments;
                }
            });

            rootCommand.Subcommands.Add(whyCommand);
        }
    }
}
