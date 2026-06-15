// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.Threading.Tasks;
using NuGet.Commands;

namespace NuGet.CommandLine.XPlat.Commands.NuGet.Remove
{
    internal static class DotnetNuGetRemoveCommand
    {
        internal static void Register(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var removeCmd = new Command("remove", Strings.Remove_Description);

            RegisterRemoveSource(removeCmd, getLogger);
            RegisterRemoveClientCert(removeCmd, getLogger);

            parent.Subcommands.Add(removeCmd);
        }

        private static void RegisterRemoveSource(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var sourceCmd = new Command("source", Strings.RemoveSourceCommandDescription);

            var nameArg = new Argument<string>("name") { Description = Strings.SourcesCommandNameDescription };
            var configfile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile };

            sourceCmd.Arguments.Add(nameArg);
            sourceCmd.Options.Add(configfile);

            sourceCmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new RemoveSourceArgs()
                {
                    Name = parseResult.GetValue(nameArg),
                    Configfile = parseResult.GetValue(configfile),
                };

                RemoveSourceRunner.Run(args, () => getLogger());
                return Task.FromResult(0);
            });

            parent.Subcommands.Add(sourceCmd);
        }

        private static void RegisterRemoveClientCert(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var clientCertCmd = new Command("client-cert", Strings.RemoveClientCertCommandDescription);

            var packagesource = new Option<string>("--package-source", "-s") { Description = Strings.Option_PackageSource };
            var configfile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile };

            clientCertCmd.Options.Add(packagesource);
            clientCertCmd.Options.Add(configfile);

            clientCertCmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new RemoveClientCertArgs()
                {
                    PackageSource = parseResult.GetValue(packagesource),
                    Configfile = parseResult.GetValue(configfile),
                };

                RemoveClientCertRunner.Run(args, () => getLogger());
                return Task.FromResult(0);
            });

            parent.Subcommands.Add(clientCertCmd);
        }
    }
}
