// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.CommandLine;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using NuGet.Commands;

namespace NuGet.CommandLine.XPlat
{
    internal static class LocalsCommand
    {
        internal static void Register(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var localsCmd = new Command("locals", Strings.LocalsCommand_Description);

            var clearOption = new Option<bool>("--clear", "-c")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.LocalsCommand_ClearDescription,
            };

            var listOption = new Option<bool>("--list", "-l")
            {
                Arity = ArgumentArity.Zero,
                Description = Strings.LocalsCommand_ListDescription,
            };

            var cacheLocationArgument = new Argument<string>("Cache Location(s)")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Strings.LocalsCommand_ArgumentDescription,
            };

            localsCmd.Options.Add(clearOption);
            localsCmd.Options.Add(listOption);
            localsCmd.Arguments.Add(cacheLocationArgument);

            localsCmd.SetAction((parseResult, cancellationToken) =>
            {
                var logger = getLogger();
                var setting = XPlatUtility.GetSettingsForCurrentWorkingDirectory();

                string? cacheLocation = parseResult.GetValue(cacheLocationArgument);
                bool clear = parseResult.GetValue(clearOption);
                bool list = parseResult.GetValue(listOption);

                // Using both -clear and -list command options, or neither one of them, is not supported.
                // We use MinArgs = 0 even though the first argument is required,
                // to avoid throwing a command argument validation exception and
                // immediately show usage help for this command instead.
                if (string.IsNullOrWhiteSpace(cacheLocation))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_NoArguments));
                }
                else if (clear && list)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_MultipleOperations));
                }
                else if (!clear && !list)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_NoOperation));
                }
                else
                {
                    var localsArgs = new LocalsArgs(new List<string> { cacheLocation },
                        setting,
                        logger.LogInformation,
                        logger.LogError,
                        clear,
                        list);

                    var localsCommandRunner = new LocalsCommandRunner();
                    localsCommandRunner.ExecuteCommand(localsArgs);
                }

                return Task.FromResult(0);
            });

            parent.Subcommands.Add(localsCmd);
        }
    }
}
