// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using NuGet.Packaging.Signing;

namespace NuGet.CommandLine.XPlat
{
    internal static class AddPackageReferenceCommand
    {
        internal static void Register(Command parent, Func<ILoggerWithColor> getLogger,
            Func<IPackageReferenceCommandRunner> getCommandRunner,
            Func<IVirtualProjectBuilder?>? getVirtualProjectBuilder = null)
        {
            var addCommand = new Command("add", Strings.AddPkg_Description);

            var id = new Option<string>("--package")
            {
                Description = Strings.AddPkg_PackageIdDescription,
                Arity = ArgumentArity.ExactlyOne
            };

            var version = new Option<string>("--version")
            {
                Description = Strings.AddPkg_PackageVersionDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var dgFilePath = new Option<string>("--dg-file", "-d")
            {
                Description = Strings.AddPkg_DgFileDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var projectPath = new Option<string>("--project", "-p")
            {
                Description = Strings.AddPkg_ProjectPathDescription,
                Arity = ArgumentArity.ExactlyOne
            };

            var frameworks = new Option<string[]>("--framework", "-f")
            {
                Description = Strings.AddPkg_FrameworksDescription,
                Arity = ArgumentArity.OneOrMore
            };

            var noRestore = new Option<bool>("--no-restore", "-n")
            {
                Description = Strings.AddPkg_NoRestoreDescription,
                Arity = ArgumentArity.Zero
            };

            var sources = new Option<string[]>("--source", "-s")
            {
                Description = Strings.AddPkg_SourcesDescription,
                Arity = ArgumentArity.OneOrMore
            };

            var packageDirectory = new Option<string>("--package-directory")
            {
                Description = Strings.AddPkg_PackageDirectoryDescription,
                Arity = ArgumentArity.ZeroOrOne
            };

            var interactive = new Option<bool>("--interactive")
            {
                Description = Strings.AddPkg_InteractiveDescription,
                Arity = ArgumentArity.Zero
            };

            var prerelease = new Option<bool>("--prerelease")
            {
                Description = Strings.Prerelease_Description,
                Arity = ArgumentArity.Zero
            };

            addCommand.Options.Add(id);
            addCommand.Options.Add(version);
            addCommand.Options.Add(dgFilePath);
            addCommand.Options.Add(projectPath);
            addCommand.Options.Add(frameworks);
            addCommand.Options.Add(noRestore);
            addCommand.Options.Add(sources);
            addCommand.Options.Add(packageDirectory);
            addCommand.Options.Add(interactive);
            addCommand.Options.Add(prerelease);

            addCommand.SetAction(async (parseResult, cancellationToken) =>
            {
                var virtualProjectBuilder = getVirtualProjectBuilder?.Invoke();

                var idValue = parseResult.GetValue(id);
                var projectPathValue = parseResult.GetValue(projectPath);
                var dgFilePathValue = parseResult.GetValue(dgFilePath);
                var noRestoreValue = parseResult.GetValue(noRestore);
                var prereleaseValue = parseResult.GetValue(prerelease);
                var versionValue = parseResult.GetValue(version);

                ValidateArgument(idValue, "--package", "add");
                ValidateArgument(projectPathValue, "--project", "add");
                ValidateProjectPath(projectPathValue, "add", virtualProjectBuilder);
                if (!noRestoreValue)
                {
                    ValidateArgument(dgFilePathValue, "--dg-file", "add");
                }
                var logger = getLogger();
                var noVersion = string.IsNullOrEmpty(versionValue);
                var packageVersion = !string.IsNullOrEmpty(versionValue) ? versionValue : null;
                ValidatePrerelease(prereleaseValue, noVersion, "add");

                var frameworkValues = parseResult.GetValue(frameworks) ?? Array.Empty<string>();
                var sourceValues = parseResult.GetValue(sources) ?? Array.Empty<string>();

                var packageRefArgs = new PackageReferenceArgs(projectPathValue, logger)
                {
                    Frameworks = CommandLineUtility.SplitAndJoinAcrossMultipleValues(frameworkValues),
                    Sources = CommandLineUtility.SplitAndJoinAcrossMultipleValues(sourceValues),
                    PackageDirectory = parseResult.GetValue(packageDirectory),
                    NoRestore = noRestoreValue,
                    NoVersion = noVersion,
                    DgFilePath = dgFilePathValue,
                    Interactive = parseResult.GetValue(interactive),
                    Prerelease = prereleaseValue,
                    PackageVersion = packageVersion!,
                    PackageId = idValue
                };
                var msBuild = new MSBuildAPIUtility(logger, virtualProjectBuilder!);

                X509TrustStore.InitializeForDotNetSdk(logger);

                var addPackageRefCommandRunner = getCommandRunner();
                return await addPackageRefCommandRunner.ExecuteCommand(packageRefArgs, msBuild);
            });

            parent.Subcommands.Add(addCommand);
        }

        private static void ValidatePrerelease(bool prerelease, bool noVersion, string commandName)
        {
            if (prerelease && !noVersion)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.Error_PrereleaseWhenVersionSpecified,
                    commandName));
            }
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
