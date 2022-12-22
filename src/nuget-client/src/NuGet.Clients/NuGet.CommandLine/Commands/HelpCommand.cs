// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace NuGet.CommandLine
{
    [Export(typeof(HelpCommand))]
    [Command(typeof(NuGetCommand), "help", "HelpCommandDescription", AltName = "?", MaxArgs = 1,
        UsageSummaryResourceName = "HelpCommandUsageSummary", UsageDescriptionResourceName = "HelpCommandUsageDescription",
        UsageExampleResourceName = "HelpCommandUsageExamples")]
    public class HelpCommand : Command
    {
        private readonly string _commandExe;
        private readonly ICommandManager _commandManager;

        private string CommandName
        {
            get
            {
                if (Arguments != null && Arguments.Count > 0)
                {
                    return Arguments[0];
                }
                return null;
            }
        }

        [Option(typeof(NuGetCommand), "HelpCommandAll")]
        public bool All { get; set; }

        [Option(typeof(NuGetCommand), "HelpCommandMarkdown")]
        public bool Markdown { get; set; }

        [ImportingConstructor]
        public HelpCommand(ICommandManager commandManager)
        {
            _commandManager = commandManager;
            _commandExe = Assembly.GetExecutingAssembly().GetName().Name;
        }

        public override void ExecuteCommand()
        {
            if (!string.IsNullOrEmpty(CommandName))
            {
                ViewHelpForCommand(CommandName);
            }
            else if (All && Markdown)
            {
                ViewMarkdownHelp();
            }
            else if (All)
            {
                ViewHelpForAllCommands();
            }
            else
            {
                ViewHelp();
            }
        }

        public void ViewHelp()
        {
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, NuGetCommand.HelpCommand_Usage, _commandExe));
            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, NuGetCommand.HelpCommand_Suggestion, _commandExe));
            Console.WriteLine();
            Console.WriteLine(NuGetCommand.HelpCommand_AvailableCommands);
            Console.WriteLine();

            var commands = from c in _commandManager.GetCommands()
                           orderby c.CommandAttribute.CommandName
                           select c.CommandAttribute;

            // Padding for printing
            int maxWidth = commands.Max(c => c.CommandName.Length + GetAltText(c.AltName).Length);

            foreach (var command in commands)
            {
                PrintCommand(maxWidth, command);
                Console.WriteLine();
            }

            Console.WriteLine(string.Format(CultureInfo.InvariantCulture,
                LocalizedResourceManager.GetString("HelpCommandForMoreInfo"),
                CommandLineConstants.NuGetDocsCommandLineReference));
        }

        // Help command always outputs NuGet version
        protected override bool ShouldOutputNuGetVersion { get { return true; } }

        private void PrintCommand(int maxWidth, CommandAttribute commandAttribute)
        {
            // Write out the command name left justified with the max command's width's padding
            Console.Write(" {0, -" + maxWidth + "}   ", GetCommandText(commandAttribute));
            // Starting index of the description
            int descriptionPadding = maxWidth + 4;
            Console.PrintJustified(descriptionPadding, commandAttribute.Description);
        }

        private static string GetCommandText(CommandAttribute commandAttribute)
        {
            return commandAttribute.CommandName + GetAltText(commandAttribute.AltName);
        }

        public void ViewHelpForCommand(string commandName)
        {
            ICommand command = _commandManager.GetCommand(commandName);
            CommandAttribute attribute = command.CommandAttribute;

            Console.WriteLine(string.Format(CultureInfo.CurrentCulture, NuGetCommand.HelpCommand_UsageDetail, _commandExe, attribute.CommandName, attribute.UsageSummary));
            Console.WriteLine();

            if (!string.IsNullOrEmpty(attribute.AltName))
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, NuGetCommand.HelpCommand_Alias, attribute.AltName));
                Console.WriteLine();
            }

            Console.WriteLine(attribute.Description);
            Console.WriteLine();

            if (attribute.UsageDescription != null)
            {
                const int padding = 5;
                Console.PrintJustified(padding, attribute.UsageDescription);
                Console.WriteLine();
            }

            var options = _commandManager.GetCommandOptions(command);

            if (options.Count > 0)
            {
                Console.WriteLine(NuGetCommand.HelpCommand_Options);
                Console.WriteLine();

                // Get the max option width. +2 for showing + against multivalued properties
                int maxOptionWidth = options.Max(o => o.Value.Name.Length) + 2;
                // Get the max altname option width
                int maxAltOptionWidth = options.Max(o => (o.Key.AltName ?? string.Empty).Length);

                foreach (KeyValuePair<OptionAttribute, PropertyInfo> o in options)
                {
                    if (TypeHelper.IsMultiValuedProperty(o.Value))
                    {
                        Console.Write(string.Format(CultureInfo.CurrentCulture, $"-{{0, -{maxOptionWidth + 2}}} +", o.Value.Name));
                    }
                    else
                    {
                        Console.Write(string.Format(CultureInfo.CurrentCulture, $"-{{0, -{maxOptionWidth + 2}}}", o.Value.Name));
                    }
                    Console.Write(string.Format(CultureInfo.CurrentCulture, $" {{0, -{maxAltOptionWidth + 4}}}", GetAltText(o.Key.AltName)));
                    Console.PrintJustified((10 + maxAltOptionWidth + maxOptionWidth), o.Key.Description);
                }

                Console.WriteLine();
            }

            if (!string.IsNullOrEmpty(attribute.UsageExample))
            {
                Console.WriteLine(NuGetCommand.HelpCommand_Examples);
                Console.WriteLine();
                Console.WriteLine(attribute.UsageExample);
                Console.WriteLine();
            }

            Console.WriteLine(string.Format(CultureInfo.InvariantCulture,
                LocalizedResourceManager.GetString("HelpCommandForMoreInfo"),
                CommandLineConstants.NuGetDocsCommandLineReference));

            Console.WriteLine();
        }

        private void ViewHelpForAllCommands()
        {
            var commands = from c in _commandManager.GetCommands()
                           orderby c.CommandAttribute.CommandName
                           select c.CommandAttribute;
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;

            foreach (var command in commands)
            {
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, NuGetCommand.HelpCommand_Title, info.ToTitleCase(command.CommandName)));
                ViewHelpForCommand(command.CommandName);
            }
        }

        /// <summary>
        /// Prints help for all commands in markdown format.
        /// </summary>
        private void ViewMarkdownHelp()
        {
            var commands = from c in _commandManager.GetCommands()
                           orderby c.CommandAttribute.CommandName
                           select c;
            foreach (var command in commands)
            {
                var template = new HelpCommandMarkdownTemplate
                {
                    CommandAttribute = command.CommandAttribute,
                    Options = from item in _commandManager.GetCommandOptions(command)
                              select new { Name = item.Value.Name, Description = item.Key.Description }
                };
                Console.WriteLine(template.TransformText());
            }
        }

        private static string GetAltText(string altNameText)
        {
            if (string.IsNullOrEmpty(altNameText))
            {
                return string.Empty;
            }
            return string.Format(CultureInfo.CurrentCulture, NuGetCommand.HelpCommand_AltText, altNameText);
        }

    }
}
