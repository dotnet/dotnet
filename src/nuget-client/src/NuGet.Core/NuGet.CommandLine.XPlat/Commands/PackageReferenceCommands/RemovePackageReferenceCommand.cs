// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace NuGet.CommandLine.XPlat
{
    internal static class RemovePackageReferenceCommand
    {
        internal static void Register(Command parent, Func<ILoggerWithColor> getLogger,
            Func<IPackageReferenceCommandRunner> getCommandRunner,
            Func<IVirtualProjectBuilder?>? getVirtualProjectBuilder = null)
        {
            var removeCommand = new Command("remove", Strings.RemovePkg_Description);

            var id = new Option<string>("--package")
            {
                Description = Strings.RemovePkg_PackageIdDescription,
                Arity = ArgumentArity.ExactlyOne
            };

            var projectPath = new Option<string>("--project", "-p")
            {
                Description = Strings.RemovePkg_ProjectPathDescription,
                Arity = ArgumentArity.ExactlyOne
            };

            var interactive = new Option<bool>("--interactive")
            {
                Description = Strings.AddPkg_InteractiveDescription,
                Arity = ArgumentArity.Zero
            };

            removeCommand.Options.Add(id);
            removeCommand.Options.Add(projectPath);
            removeCommand.Options.Add(interactive);

            removeCommand.SetAction(async (parseResult, cancellationToken) =>
            {
                var virtualProjectBuilder = getVirtualProjectBuilder?.Invoke();

                var idValue = parseResult.GetValue(id);
                var projectPathValue = parseResult.GetValue(projectPath);

                ValidateArgument(idValue, "--package", "remove");
                ValidateArgument(projectPathValue, "--project", "remove");
                ValidateProjectPath(projectPathValue, "remove", virtualProjectBuilder);
                var logger = getLogger();
                var packageRefArgs = new PackageReferenceArgs(projectPathValue, logger)
                {
                    Interactive = parseResult.GetValue(interactive),
                    PackageId = idValue
                };
                var msBuild = new MSBuildAPIUtility(logger, virtualProjectBuilder!);
                var removePackageRefCommandRunner = getCommandRunner();
                return await removePackageRefCommandRunner.ExecuteCommand(packageRefArgs, msBuild);
            });

            parent.Subcommands.Add(removeCommand);
        }

        private static void ValidateArgument([NotNull] string? value, string optionName, string commandName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PkgMissingArgument,
                    commandName,
                    optionName));
            }
        }

        private static void ValidateProjectPath(string projectPath, string commandName, IVirtualProjectBuilder? virtualProjectBuilder)
        {
            if (!File.Exists(projectPath)
                || (!projectPath.EndsWith("proj", StringComparison.OrdinalIgnoreCase)
                    && virtualProjectBuilder?.IsValidEntryPointPath(projectPath) != true))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Strings.Error_PkgMissingOrInvalidProjectFile,
                    commandName,
                    projectPath));
            }
        }
    }
}
