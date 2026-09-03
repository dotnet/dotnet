// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.Threading.Tasks;
using NuGet.Commands;

namespace NuGet.CommandLine.XPlat.Commands.NuGet.List
{
    internal static class DotnetNuGetListCommand
    {
        internal static void Register(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var listCmd = new Command("list", Strings.List_Description);

            RegisterListSource(listCmd, getLogger);
            RegisterListClientCert(listCmd, getLogger);

            parent.Subcommands.Add(listCmd);
        }

        private static void RegisterListSource(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var sourceCmd = new Command("source", Strings.ListSourceCommandDescription);

            var format = new Option<string>("--format") { Description = Strings.SourcesCommandFormatDescription };
            var configfile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile };

            sourceCmd.Options.Add(format);
            sourceCmd.Options.Add(configfile);

            sourceCmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new ListSourceArgs()
                {
                    Format = parseResult.GetValue(format),
                    Configfile = parseResult.GetValue(configfile),
                };

                ListSourceRunner.Run(args, () => getLogger());
                return Task.FromResult(0);
            });

            parent.Subcommands.Add(sourceCmd);
        }

        private static void RegisterListClientCert(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var clientCertCmd = new Command("client-cert", Strings.ListClientCertCommandDescription);

            var configfile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile };

            clientCertCmd.Options.Add(configfile);

            clientCertCmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new ListClientCertArgs()
                {
                    Configfile = parseResult.GetValue(configfile),
                };

                ListClientCertRunner.Run(args, () => getLogger());
                return Task.FromResult(0);
            });

            parent.Subcommands.Add(clientCertCmd);
        }
    }
}
