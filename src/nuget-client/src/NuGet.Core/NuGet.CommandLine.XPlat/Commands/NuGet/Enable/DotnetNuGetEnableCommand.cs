// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;
using System.Threading.Tasks;
using NuGet.Commands;

namespace NuGet.CommandLine.XPlat.Commands.NuGet.Enable
{
    internal static class DotnetNuGetEnableCommand
    {
        internal static void Register(Command parent, Func<ILoggerWithColor> getLogger)
        {
            var enableCmd = new Command("enable", Strings.Enable_Description);

            var sourceCmd = new Command("source", Strings.EnableSourceCommandDescription);

            var nameArg = new Argument<string>("name") { Description = Strings.SourcesCommandNameDescription };
            var configfile = new Option<string>("--configfile") { Description = Strings.Option_ConfigFile };

            sourceCmd.Arguments.Add(nameArg);
            sourceCmd.Options.Add(configfile);

            sourceCmd.SetAction((parseResult, cancellationToken) =>
            {
                var args = new EnableSourceArgs()
                {
                    Name = parseResult.GetValue(nameArg),
                    Configfile = parseResult.GetValue(configfile),
                };

                EnableSourceRunner.Run(args, () => getLogger());
                return Task.FromResult(0);
            });

            enableCmd.Subcommands.Add(sourceCmd);
            parent.Subcommands.Add(enableCmd);
        }
    }
}
