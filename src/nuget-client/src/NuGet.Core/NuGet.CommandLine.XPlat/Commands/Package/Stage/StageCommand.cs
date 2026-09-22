// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.CommandLine;

namespace NuGet.CommandLine.XPlat.Commands.Package.Stage
{
    internal static class StageCommand
    {
        internal static void Register(
            Command rootCommand,
            Option<bool> interactiveOption,
            Func<ILoggerWithColor> getLogger)
        {
            var stageCommand = new DocumentedCommand(
                "stage",
                Strings.StageCommand_Description,
                "https://aka.ms/dotnet/package/stage");

            StagePushCommand.Register(stageCommand, interactiveOption, getLogger);
            rootCommand.Subcommands.Add(stageCommand);
        }
    }
}
