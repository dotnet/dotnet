// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.CommandLine.XPlat.Commands.Package.PackageDownload
{
    internal class PackageDownloadCommand
    {
        internal static void Register(Command packageCommand, Option<bool> interactiveOption)
        {
            Register(packageCommand, interactiveOption, PackageDownloadRunner.RunAsync);
        }

        public static void Register(Command packageCommand, Option<bool> interactiveOption, Func<PackageDownloadArgs, CancellationToken, Task<int>> action)
        {
            var downloadCommand = new DocumentedCommand(
                "download",
                Strings.PackageDownloadCommand_Description,
                "https://aka.ms/dotnet/package/download");

            // Arguments
            var packagesArguments = new Argument<IReadOnlyList<PackageWithNuGetVersion>>("packages")
            {
                Description = Strings.PackageUpdate_PackageArgumentDescription,
                Arity = ArgumentArity.OneOrMore,
                CustomParser = PackageWithNuGetVersion.Parse
            };

            // Options
            var allowInsecureConnections = new Option<bool>("--allow-insecure-connections")
            {
                Description = Strings.PackageDownloadCommand_AllowInsecureConnectionsDescription,
                Arity = ArgumentArity.Zero
            };

            var configFile = new Option<string>("--configfile")
            {
                Description = Strings.Option_ConfigFile,
                Arity = ArgumentArity.ExactlyOne
            };

            var outputDirectory = new Option<string>("--output", "-o")
            {
                Description = Strings.PackageDownloadCommand_OutputDirectoryDescription,
                Arity = ArgumentArity.ExactlyOne
            };

            var prerelease = new Option<bool>("--prerelease")
            {
                Description = Strings.Prerelease_Description,
                Arity = ArgumentArity.Zero
            };

            var sources = new Option<List<string>>("--source", "-s")
            {
                Description = Strings.PackageDownloadCommand_SourcesDescription,
                Arity = ArgumentArity.OneOrMore
            };

            var verbosity = CommonOptions.GetVerbosityOption();

            downloadCommand.Arguments.Add(packagesArguments);
            downloadCommand.Options.Add(allowInsecureConnections);
            downloadCommand.Options.Add(configFile);
            downloadCommand.Options.Add(interactiveOption);
            downloadCommand.Options.Add(outputDirectory);
            downloadCommand.Options.Add(prerelease);
            downloadCommand.Options.Add(sources);
            downloadCommand.Options.Add(verbosity);

            downloadCommand.SetAction(async (parserResult, cancellationToken) =>
            {
                IReadOnlyList<PackageWithNuGetVersion> packages = parserResult.GetValue(packagesArguments) ?? [];
                var args = new PackageDownloadArgs()
                {
                    Packages = packages,
                    Sources = parserResult.GetValue(sources),
                    OutputDirectory = parserResult.GetValue(outputDirectory),
                    IncludePrerelease = parserResult.GetValue(prerelease),
                    AllowInsecureConnections = parserResult.GetValue(allowInsecureConnections),
                    Interactive = parserResult.GetValue(interactiveOption),
                    ConfigFile = parserResult.GetValue(configFile),
                    LogLevel = (parserResult.GetValue(verbosity) ?? VerbosityEnum.normal).ToLogLevel()
                };

                return await action(args, cancellationToken);
            });

            packageCommand.Subcommands.Add(downloadCommand);
        }
    }
}
