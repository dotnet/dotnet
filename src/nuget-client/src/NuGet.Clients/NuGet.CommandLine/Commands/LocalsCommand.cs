// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using NuGet.Commands;

namespace NuGet.CommandLine.Commands
{
    [Command(typeof(NuGetCommand), "locals", "LocalsCommandDescription", MinArgs = 0, MaxArgs = 1,
        UsageSummaryResourceName = "LocalsCommandSummary",
        UsageExampleResourceName = "LocalsCommandExamples")]
    public class LocalsCommand
        : Command
    {
        // Default constructor used only for testing, since the Command Default Constructor is protected
        public LocalsCommand() : base()
        {
        }

        [Option(typeof(NuGetCommand), "LocalsCommandClearDescription")]
        public bool Clear { get; set; }

        [Option(typeof(NuGetCommand), "LocalsCommandListDescription")]
        public bool List { get; set; }

        public ILocalsCommandRunner LocalsCommandRunner { get; set; }

        public override Task ExecuteCommandAsync()
        {
            if ((!Arguments.Any() || string.IsNullOrWhiteSpace(Arguments[0]))
                || (!Clear && !List)
                || (Clear && List))
            {
                // Using both -clear and -list command options, or neither one of them, is not supported.
                // We use MinArgs = 0 even though the first argument is required,
                // to avoid throwing a command argument validation exception and
                // immediately show usage help for this command instead.
                HelpCommand.ViewHelpForCommand(CommandAttribute.CommandName);

                return Task.CompletedTask;
            }

            if (LocalsCommandRunner == null)
            {
                LocalsCommandRunner = new LocalsCommandRunner();
            }
            var localsArgs = new LocalsArgs(Arguments, Settings, Console.LogInformation, Console.LogError, Clear, List);
            LocalsCommandRunner.ExecuteCommand(localsArgs);
            return Task.CompletedTask;
        }
    }
}
