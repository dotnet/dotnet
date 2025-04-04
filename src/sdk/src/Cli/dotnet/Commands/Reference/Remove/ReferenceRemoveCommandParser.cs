// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.CommandLine;
using LocalizableStrings = Microsoft.DotNet.Tools.Reference.Remove.LocalizableStrings;

namespace Microsoft.DotNet.Cli.Commands.Reference.Remove;

internal static class ReferenceRemoveCommandParser
{
    public static readonly CliArgument<IEnumerable<string>> ProjectPathArgument = new DynamicArgument<IEnumerable<string>>(LocalizableStrings.ProjectPathArgumentName)
    {
        Description = LocalizableStrings.ProjectPathArgumentDescription,
        Arity = ArgumentArity.OneOrMore,
    }.AddCompletions(Complete.Complete.ProjectReferencesFromProjectFile);

    public static readonly CliOption<string> FrameworkOption = new("--framework", "-f")
    {
        Description = LocalizableStrings.CmdFrameworkDescription,
        HelpName = CommonLocalizableStrings.CmdFramework
    };

    private static readonly CliCommand Command = ConstructCommand();

    public static CliCommand GetCommand()
    {
        return Command;
    }

    private static CliCommand ConstructCommand()
    {
        var command = new CliCommand("remove", LocalizableStrings.AppFullName);

        command.Arguments.Add(ProjectPathArgument);
        command.Options.Add(FrameworkOption);

        command.SetAction((parseResult) => new RemoveProjectToProjectReferenceCommand(parseResult).Execute());

        return command;
    }
}
