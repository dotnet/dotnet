// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.CommandLine.XPlat.Commands.Package.Update
{
    internal static class PackageUpdateCommand
    {
        internal static void Register(Command packageCommand)
        {
            Func<ILoggerWithColor> getLogger = () =>
            {
                var logger = new CommandOutputLogger(Common.LogLevel.Minimal);
                logger.HidePrefixForInfoAndMinimal = true;
                return logger;
            };
            Register(packageCommand, getLogger);
        }

        internal static void Register(Command packageCommand, Func<ILoggerWithColor> getLogger)
        {
            Register(packageCommand, getLogger, PackageUpdateCommandRunner.Run);
        }

        internal static void Register(Command packageCommand, Func<ILoggerWithColor> getLogger, Func<PackageUpdateArgs, ILoggerWithColor, IDGSpecFactory, MSBuildAPIUtility, CancellationToken, Task<int>> action)
        {
            var command = new DocumentedCommand("update", Strings.PackageUpdateCommand_Description, "https://aka.ms/dotnet/package/update");

            var packagesArguments = new Argument<List<string>>("packages")
            {
                Arity = ArgumentArity.ZeroOrMore,
            };
            command.Arguments.Add(packagesArguments);

            var projectOption = new Option<FileSystemInfo>("--project").AcceptExistingOnly();
            projectOption.Description = Strings.PackageUpdateCommand_ProjectOptionDescription;
            command.Options.Add(projectOption);

            packageCommand.Subcommands.Add(command);
            command.SetAction(async (args, cancellationToken) =>
            {
                var logger = getLogger();
                var project = args.GetValue(projectOption);
                var packages = args.GetValue(packagesArguments);

                var commandArgs = new PackageUpdateArgs
                {
                    Project = project?.FullName ?? Environment.CurrentDirectory,
                    Packages = packages,
                };

                IDGSpecFactory dGSpecFactory = new DGSpecFactory();
                MSBuildAPIUtility mSBuild = new(logger);

                return await action(commandArgs, logger, dGSpecFactory, mSBuild, cancellationToken);
            });
        }
    }
}
