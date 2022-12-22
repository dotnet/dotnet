// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Common;

namespace NuGet.CommandLine.XPlat
{
    internal class RemovePackageReferenceCommand
    {
        public static void Register(CommandLineApplication app, Func<ILogger> getLogger,
            Func<IPackageReferenceCommandRunner> getCommandRunner)
        {
            app.Command("remove", removePkg =>
            {
                removePkg.Description = Strings.RemovePkg_Description;
                removePkg.HelpOption(XPlatUtility.HelpOption);

                removePkg.Option(
                    CommandConstants.ForceEnglishOutputOption,
                    Strings.ForceEnglishOutput_Description,
                    CommandOptionType.NoValue);

                var id = removePkg.Option(
                    "--package",
                    Strings.RemovePkg_PackageIdDescription,
                    CommandOptionType.SingleValue);

                var projectPath = removePkg.Option(
                    "-p|--project",
                    Strings.RemovePkg_ProjectPathDescription,
                    CommandOptionType.SingleValue);

                var interactive = removePkg.Option(
                    "--interactive",
                    Strings.AddPkg_InteractiveDescription,
                    CommandOptionType.NoValue);

                removePkg.OnExecute(() =>
                {
                    ValidateArgument(id, removePkg.Name);
                    ValidateArgument(projectPath, removePkg.Name);
                    ValidateProjectPath(projectPath, removePkg.Name);
                    var logger = getLogger();
                    var packageRefArgs = new PackageReferenceArgs(projectPath.Value(), logger)
                    {
                        Interactive = interactive.HasValue(),
                        PackageId = id.Value()
                    };
                    var msBuild = new MSBuildAPIUtility(logger);
                    var removePackageRefCommandRunner = getCommandRunner();
                    return removePackageRefCommandRunner.ExecuteCommand(packageRefArgs, msBuild);
                });
            });
        }

        private static void ValidateArgument(CommandOption arg, string commandName)
        {
            if (arg.Values.Count < 1)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PkgMissingArgument,
                    commandName,
                    arg.Template));
            }
        }

        private static void ValidateProjectPath(CommandOption projectPath, string commandName)
        {
            if (!File.Exists(projectPath.Value()) || !projectPath.Value().EndsWith("proj", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Strings.Error_PkgMissingOrInvalidProjectFile,
                    commandName,
                    projectPath.Value()));
            }
        }
    }
}
