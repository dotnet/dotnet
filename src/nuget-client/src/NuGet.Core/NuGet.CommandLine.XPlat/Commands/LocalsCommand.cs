// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Commands;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat
{
    internal static class LocalsCommand
    {
        public static void Register(CommandLineApplication app, Func<ILogger> getLogger)
        {
            app.Command("locals", locals =>
            {
                locals.Description = Strings.LocalsCommand_Description;
                locals.HelpOption(XPlatUtility.HelpOption);

                locals.Option(
                    CommandConstants.ForceEnglishOutputOption,
                    Strings.ForceEnglishOutput_Description,
                    CommandOptionType.NoValue);

                var clear = locals.Option(
                    "-c|--clear",
                    Strings.LocalsCommand_ClearDescription,
                    CommandOptionType.NoValue);

                var list = locals.Option(
                    "-l|--list",
                    Strings.LocalsCommand_ListDescription,
                    CommandOptionType.NoValue);

                var arguments = locals.Argument(
                    "Cache Location(s)",
                    Strings.LocalsCommand_ArgumentDescription,
                    multipleValues: false);

                locals.OnExecute(() =>
                {
                    var logger = getLogger();
                    var setting = XPlatUtility.GetSettingsForCurrentWorkingDirectory();

                    // Using both -clear and -list command options, or neither one of them, is not supported.
                    // We use MinArgs = 0 even though the first argument is required,
                    // to avoid throwing a command argument validation exception and
                    // immediately show usage help for this command instead.
                    if ((arguments.Values.Count < 1) || string.IsNullOrWhiteSpace(arguments.Values[0]))
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_NoArguments));
                    }
                    else if (clear.HasValue() && list.HasValue())
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_MultipleOperations));
                    }
                    else if (!clear.HasValue() && !list.HasValue())
                    {
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.LocalsCommand_NoOperation));
                    }
                    else
                    {
                        var localsArgs = new LocalsArgs(arguments.Values,
                            setting,
                            logger.LogInformation,
                            logger.LogError,
                            clear.HasValue(),
                            list.HasValue());

                        var localsCommandRunner = new LocalsCommandRunner();
                        localsCommandRunner.ExecuteCommand(localsArgs);
                    }

                    return 0;
                });
            });
        }
    }
}
