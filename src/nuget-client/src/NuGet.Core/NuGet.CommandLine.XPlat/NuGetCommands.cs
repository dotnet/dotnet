// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.CommandLine;
using System.Linq;
using NuGet.CommandLine.XPlat.Commands.Package.Update;

namespace NuGet.CommandLine.XPlat;

/// <summary>
/// Class used by the .NET SDK to register NuGet's commands into the dotnet CLI.
/// </summary>
public static class NuGetCommands
{
    /// <summary>
    /// <para>Adds NuGet's dotnet CLI commands to the dotnet CLI RootCommand object</para>
    /// </summary>
    /// <param name="rootCommand">The CLI's RootCommand instance</param>
    /// <remarks>Many of NuGet's commands are defined in the dotnet/sdk repo, and those run NuGet.CommandLine.XPlat.dll as a child process.
    /// Those commands are not added by this method.</remarks>
    public static void Add(RootCommand rootCommand)
    {
        var packageCommand = rootCommand.Subcommands.FirstOrDefault(c => c.Name == "package");
        if (packageCommand is null)
        {
            packageCommand = new Command("package");
            rootCommand.Subcommands.Add(packageCommand);
        }

        PackageUpdateCommand.Register(packageCommand);
    }
}
