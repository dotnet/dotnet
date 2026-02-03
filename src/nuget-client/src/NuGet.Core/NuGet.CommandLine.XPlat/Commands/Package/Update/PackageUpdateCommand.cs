// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update;

internal static class PackageUpdateCommand
{
    internal static void Register(Command packageCommand, Option<bool> interactiveOption)
    {
        Register(packageCommand, interactiveOption, PackageUpdateCommandRunner.Run);
    }

    internal static void Register(Command packageCommand, Option<bool> interactiveOption, Func<PackageUpdateArgs, CancellationToken, Task<int>> action)
    {
        var command = new DocumentedCommand("update", Strings.PackageUpdateCommand_Description, "https://aka.ms/dotnet/package/update");

        var packagesArguments = new Argument<IReadOnlyList<PackageWithVersionRange>>("packages")
        {
            Description = Strings.PackageUpdate_PackageArgumentDescription,
            Arity = ArgumentArity.ZeroOrMore,
            CustomParser = PackageWithVersionRange.Parse
        };
        command.Arguments.Add(packagesArguments);

        var projectOption = new Option<FileSystemInfo>("--project").AcceptExistingOnly();
        projectOption.Description = Strings.PackageUpdateCommand_ProjectOptionDescription;
        command.Options.Add(projectOption);

        var vulnerableOption = new Option<bool>("--vulnerable");
        vulnerableOption.Description = Strings.PackageUpdateCommand_VulnerableOptionDescription;
        command.Options.Add(vulnerableOption);

        command.Options.Add(interactiveOption);

        var verbosityOption = CommonOptions.GetVerbosityOption();
        command.Options.Add(verbosityOption);

        packageCommand.Subcommands.Add(command);
        command.SetAction(async (args, cancellationToken) =>
        {
            FileSystemInfo? project = args.GetValue(projectOption);
            IReadOnlyList<PackageWithVersionRange> packages = args.GetValue(packagesArguments) ?? [];
            bool interactive = args.GetValue(interactiveOption);
            VerbosityEnum verbosity = args.GetValue(verbosityOption) ?? VerbosityEnum.normal;
            LogLevel logLevel = verbosity.ToLogLevel();
            bool vulnerable = args.GetValue(vulnerableOption);

            var commandArgs = new PackageUpdateArgs
            {
                Project = project?.FullName ?? Environment.CurrentDirectory,
                Packages = packages,
                Interactive = interactive,
                LogLevel = logLevel,
                Vulnerable = vulnerable,
            };

            return await action(commandArgs, cancellationToken);
        });
    }
}
