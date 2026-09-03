// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.CommandLine;
using System.Linq;
using NuGet.CommandLine.XPlat.Commands.Package.PackageDownload;
using NuGet.CommandLine.XPlat.Commands.Package.Update;

namespace NuGet.CommandLine.XPlat;

/// <summary>
/// Class used by the .NET SDK to register NuGet's commands into the dotnet CLI.
/// </summary>
public static class NuGetCommands
{
    /// <summary>
    /// <para>Adds NuGet's dotnet CLI commands to the dotnet CLI command object</para>
    /// </summary>
    /// <param name="command">The CLI's Command instance</param>
    /// <param name="interactiveOption">The .NET SDK has code to detect when output is redirected or </param>
    /// <param name="virtualProjectBuilder">For handling file-based apps.</param>
    /// <remarks>Many of NuGet's commands are defined in the dotnet/sdk repo, and those run NuGet.CommandLine.XPlat.dll as a child process.
    /// Those commands are not added by this method.</remarks>
    public static void Add(Command command, Option<bool> interactiveOption, IVirtualProjectBuilder? virtualProjectBuilder)
    {
        var packageCommand = command.Subcommands.FirstOrDefault(c => c.Name == "package");
        if (packageCommand is null)
        {
            packageCommand = new Command("package");
            command.Subcommands.Add(packageCommand);
        }

        PackageUpdateCommand.Register(packageCommand, interactiveOption, virtualProjectBuilder);
        PackageDownloadCommand.Register(packageCommand, interactiveOption);
    }

    // For binary backcompat. To delete once the SDK starts using the Command overload.
    public static void Add(RootCommand rootCommand, Option<bool> interactiveOption, IVirtualProjectBuilder? virtualProjectBuilder)
    {
        Add((Command)rootCommand, interactiveOption, virtualProjectBuilder);
    }

    // For binary backcompat. To delete once the SDK starts using the Command overload.
    public static void Add(RootCommand rootCommand, Option<bool> interactiveOption)
    {
        Add((Command)rootCommand, interactiveOption, virtualProjectBuilder: null);
    }
}
