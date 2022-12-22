// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Common;
using NuGet.Packaging.Signing;

namespace NuGet.CommandLine.XPlat
{
    internal static class AddPackageReferenceCommand
    {
        public static void Register(CommandLineApplication app, Func<ILogger> getLogger,
            Func<IPackageReferenceCommandRunner> getCommandRunner)
        {
            app.Command("add", addpkg =>
            {
                addpkg.Description = Strings.AddPkg_Description;
                addpkg.HelpOption(XPlatUtility.HelpOption);

                addpkg.Option(
                    CommandConstants.ForceEnglishOutputOption,
                    Strings.ForceEnglishOutput_Description,
                    CommandOptionType.NoValue);

                var id = addpkg.Option(
                    "--package",
                    Strings.AddPkg_PackageIdDescription,
                    CommandOptionType.SingleValue);

                var version = addpkg.Option(
                    "--version",
                    Strings.AddPkg_PackageVersionDescription,
                    CommandOptionType.SingleValue);

                var dgFilePath = addpkg.Option(
                    "-d|--dg-file",
                    Strings.AddPkg_DgFileDescription,
                    CommandOptionType.SingleValue);

                var projectPath = addpkg.Option(
                    "-p|--project",
                    Strings.AddPkg_ProjectPathDescription,
                    CommandOptionType.SingleValue);

                var frameworks = addpkg.Option(
                    "-f|--framework",
                    Strings.AddPkg_FrameworksDescription,
                    CommandOptionType.MultipleValue);

                var noRestore = addpkg.Option(
                    "-n|--no-restore",
                    Strings.AddPkg_NoRestoreDescription,
                    CommandOptionType.NoValue);

                var sources = addpkg.Option(
                    "-s|--source",
                    Strings.AddPkg_SourcesDescription,
                    CommandOptionType.MultipleValue);

                var packageDirectory = addpkg.Option(
                    "--package-directory",
                    Strings.AddPkg_PackageDirectoryDescription,
                    CommandOptionType.SingleValue);

                var interactive = addpkg.Option(
                    "--interactive",
                    Strings.AddPkg_InteractiveDescription,
                    CommandOptionType.NoValue);

                var prerelease = addpkg.Option(
                    "--prerelease",
                    Strings.AddPkg_PackagePrerelease,
                    CommandOptionType.NoValue);

                addpkg.OnExecute(() =>
                {
                    ValidateArgument(id, addpkg.Name);
                    ValidateArgument(projectPath, addpkg.Name);
                    ValidateProjectPath(projectPath, addpkg.Name);
                    if (!noRestore.HasValue())
                    {
                        ValidateArgument(dgFilePath, addpkg.Name);
                    }
                    var logger = getLogger();
                    var noVersion = !version.HasValue();
                    var packageVersion = version.HasValue() ? version.Value() : null;
                    ValidatePrerelease(prerelease.HasValue(), noVersion, addpkg.Name);
                    var packageRefArgs = new PackageReferenceArgs(projectPath.Value(), logger)
                    {
                        Frameworks = CommandLineUtility.SplitAndJoinAcrossMultipleValues(frameworks.Values),
                        Sources = CommandLineUtility.SplitAndJoinAcrossMultipleValues(sources.Values),
                        PackageDirectory = packageDirectory.Value(),
                        NoRestore = noRestore.HasValue(),
                        NoVersion = noVersion,
                        DgFilePath = dgFilePath.Value(),
                        Interactive = interactive.HasValue(),
                        Prerelease = prerelease.HasValue(),
                        PackageVersion = packageVersion,
                        PackageId = id.Values[0]
                    };
                    var msBuild = new MSBuildAPIUtility(logger);

                    X509TrustStore.InitializeForDotNetSdk(logger);

                    var addPackageRefCommandRunner = getCommandRunner();
                    return addPackageRefCommandRunner.ExecuteCommand(packageRefArgs, msBuild);
                });
            });
        }

        private static void ValidatePrerelease(bool prerelease, bool noVersion, string commandName)
        {
            if (prerelease && !noVersion)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PrereleaseWhenVersionSpecified,
                    commandName));
            }
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
