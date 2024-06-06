// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;

namespace NuGet.CommandLine.XPlat
{
    internal static class WhyCommand
    {
        private static CliArgument<string> Path = new CliArgument<string>("PROJECT|SOLUTION")
        {
            Description = Strings.WhyCommand_PathArgument_Description,
            Arity = ArgumentArity.ExactlyOne
        };

        private static CliArgument<string> Package = new CliArgument<string>("PACKAGE")
        {
            Description = Strings.WhyCommand_PackageArgument_Description,
            Arity = ArgumentArity.ExactlyOne
        };

        private static CliOption<List<string>> Frameworks = new CliOption<List<string>>("--framework", "-f")
        {
            Description = Strings.WhyCommand_FrameworksOption_Description,
            Arity = ArgumentArity.OneOrMore
        };

        private static HelpOption Help = new HelpOption()
        {
            Arity = ArgumentArity.Zero
        };

        internal static void Register(CliCommand rootCommand, Func<ILoggerWithColor> getLogger)
        {
            var whyCommand = new CliCommand("why", Strings.WhyCommand_Description);

            whyCommand.Arguments.Add(Path);
            whyCommand.Arguments.Add(Package);
            whyCommand.Options.Add(Frameworks);
            whyCommand.Options.Add(Help);

            whyCommand.SetAction((parseResult) =>
            {
                ILoggerWithColor logger = getLogger();

                try
                {
                    var whyCommandArgs = new WhyCommandArgs(
                        parseResult.GetValue(Path),
                        parseResult.GetValue(Package),
                        parseResult.GetValue(Frameworks),
                        logger);

                    int exitCode = WhyCommandRunner.ExecuteCommand(whyCommandArgs);
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
