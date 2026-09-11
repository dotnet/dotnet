// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Arcade.Common;
using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.DotNet.Helix.Sdk;

/// <summary>
/// Task that installs a .NET tool in a given folder.
/// Handles parallel builds that install the same tool.
/// </summary>
// TODO: https://github.com/dotnet/arcade/issues/17378 - not yet annotated with
// [MSBuildMultiThreadableTask]. Paths are resolved through TaskEnvironment below, but the child
// `dotnet tool install` process is spawned through Microsoft.Arcade.Common's ICommandFactory,
// which builds its ProcessStartInfo from the ambient process environment rather than from an
// injected TaskEnvironment. Per-project environment variables would therefore leak between
// projects sharing a node, so MSBuild keeps routing this task through the out-of-proc TaskHost.
//
// Implementing IMultiThreadableTask without the attribute is deliberate. Routing is decided by
// the attribute alone (TaskRouter.NeedsTaskHostInMultiThreadedMode); it cannot key off the
// interface, because ToolTask implements it and that would opt in every ToolTask-derived task in
// the ecosystem. The interface only causes TaskEnvironment to be injected. Do not remove it to
// "make this safe" - that would revert the path resolution below to the process current
// directory while leaving the task exactly as unsafe as it is now.
#pragma warning disable MSBuildTask0013 // Interface without the attribute is deliberate; see the comment above.
public class InstallDotNetTool : MSBuildTaskBase, IMultiThreadableTask
{
#pragma warning restore MSBuildTask0013
    /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
    public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

    /// <summary>
    /// The name of the tool to install (same as the NuGet package name, e.g. Microsoft.DotNet.XHarness.CLI)
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Directory where the tool will be installed
    /// </summary>
    [Required]
    public string DestinationPath { get; set; }

    /// <summary>
    /// Version of the tool to install
    /// 
    /// Must not contain * so that we can make sure the right version is installed.
    /// </summary>
    [Required]
    public string Version { get; set; }

    /// <summary>
    /// Optional path to dotnet.exe to use
    /// </summary>
    public string DotnetPath { get; set; }

    /// <summary>
    /// Source to install the tool from
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// The target framework to install the tool for
    /// </summary>
    public string TargetFramework { get; set; }

    /// <summary>
    /// The target architecture to install the tool for
    /// </summary>
    public string TargetArchitecture { get; set; }

    /// <summary>
    /// Working directory when executing the command
    /// 
    /// There is an issue in the SDK where if the working directory contains .proj files, `dotnet tool install` will fail.
    /// https://github.com/dotnet/sdk/issues/12120
    /// You can use this property to work around this.
    /// </summary>
    public string WorkingDirectory { get; set; }

    /// <summary>
    /// Do not use a cached .nupkg when installing
    /// </summary>
    public bool NoCache { get; set; } = false;

    /// <summary>
    /// Location of where the tool was installed (including the version)
    /// </summary>
    [Output]
    public string ToolPath { get; set; }

    public override void ConfigureServices(IServiceCollection collection)
    {
        collection.TryAddTransient<ICommandFactory, CommandFactory>();
        collection.TryAddTransient<IFileSystem, FileSystem>();
        collection.TryAddTransient<IHelpers, Arcade.Common.Helpers>();
        collection.TryAddSingleton(Log);
    }

    public bool ExecuteTask(ICommandFactory commandFactory, IFileSystem fileSystem, IHelpers helpers)
    {
        if (Version.Contains("*"))
        {
            Log.LogError("InstallDotNetTool task doesn't accept * in the version");
            return false;
        }

        // We install the tool in [dest]/[name]/[version] because if we tried to install 2 versions in the same dir,
        // `dotnet tool install` would fail.
        var version = Version.ToLowerInvariant();
        string absoluteDestinationPath = TaskEnvironment.GetAbsolutePath(DestinationPath);
        ToolPath = Path.Combine(absoluteDestinationPath, Name, Version);

        if (!fileSystem.DirectoryExists(ToolPath))
        {
            fileSystem.CreateDirectory(absoluteDestinationPath);
        }

        string versionInstallPath = Path.Combine(ToolPath, ".store", Name.ToLowerInvariant(), version);

        return helpers.DirectoryMutexExec(() =>
        {
            if (fileSystem.DirectoryExists(versionInstallPath))
            {
                Log.LogMessage($"{Name} v{Version} is already installed");
                return true;
            }

            return InstallTool(commandFactory);
        }, ToolPath);
    }

    private bool InstallTool(ICommandFactory commandFactory)
    {
        Log.LogMessage($"Installing {Name} v{Version}...");

        var args = new List<string>()
        {
            "tool",
            "install",
            "--version",
            Version,
            "--tool-path",
            ToolPath,
        };

        if (!string.IsNullOrEmpty(TargetFramework))
        {
            args.Add("--framework");
            args.Add(TargetFramework);
        }

        if (!string.IsNullOrEmpty(TargetArchitecture))
        {
            args.Add("--arch");
            args.Add(TargetArchitecture);
        }

        if (!string.IsNullOrEmpty(Source))
        {
            args.Add("--source");
            args.Add(Source);
        }

        if (NoCache)
        {
            args.Add("--no-cache");
        }

        args.Add(Name);

        // A bare executable name such as "dotnet" or "dotnet.exe" must stay unresolved so the
        // process launcher can find it on PATH. RepoLayout.props deliberately produces that form
        // when the repo-local .NET install is absent, and XHarnessRunner.targets forwards it here
        // as DotnetPath. Only a value that actually carries a directory component is absolutized.
        string executable;
        if (string.IsNullOrEmpty(DotnetPath))
        {
            executable = "dotnet";
        }
        else if (string.IsNullOrEmpty(Path.GetDirectoryName(DotnetPath)))
        {
            executable = DotnetPath;
        }
        else
        {
            executable = TaskEnvironment.GetAbsolutePath(DotnetPath);
        }

        // Log the executable actually invoked. DotnetPath is empty in the common case, which made
        // this line report a blank command.
        Log.LogMessage($"Executing {executable} {string.Join(" ", args)}");

        ICommand command = commandFactory.Create(executable, args);

        if (!string.IsNullOrEmpty(WorkingDirectory))
        {
            command.WorkingDirectory(TaskEnvironment.GetAbsolutePath(WorkingDirectory));
        }

        CommandResult result = command.Execute();

        if (result.ExitCode != 0)
        {
            Log.LogError(
                $"Failed to install the dotnet tool. Installation exited with {result.ExitCode}. " +
                Environment.NewLine + (string.IsNullOrEmpty(result.StdErr) ? result.StdOut : result.StdErr));
            return false;
        }

        Log.LogMessage($"{Name} v{Version} installed");
        return true;
    }
}
