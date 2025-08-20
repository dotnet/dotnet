// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TestUtilities;
using Xunit.Abstractions;

namespace Microsoft.DotNet.SourceBuild.Tests;

internal class DotNetHelper
{
    private static readonly object s_lockObj = new();

    public static string DotNetPath { get; } = Path.Combine(Config.DotNetDirectory, "dotnet");
    public static string PackagesDirectory { get; } = Path.Combine(Directory.GetCurrentDirectory(), "packages");
    public static string ProjectsDirectory { get; } = Path.Combine(Directory.GetCurrentDirectory(), $"projects-{DateTime.Now:yyyyMMddHHmmssffff}");

    private ITestOutputHelper OutputHelper { get; }
    public bool IsMonoRuntime { get; }

    public DotNetHelper(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;

        lock (s_lockObj)
        {
            IsMonoRuntime = DetermineIsMonoRuntime(Config.DotNetDirectory);

            if (!Directory.Exists(ProjectsDirectory))
            {
                Directory.CreateDirectory(ProjectsDirectory);
                InitNugetConfig();
            }

            if (!Directory.Exists(PackagesDirectory))
            {
                Directory.CreateDirectory(PackagesDirectory);
            }
        }
    }

    private static void InitNugetConfig()
    {
        bool useCustomPackages = !string.IsNullOrEmpty(Config.CustomPackagesPath);
        string nugetConfigPrefix = useCustomPackages ? "custom" : "default";
        string nugetConfigPath = Path.Combine(ProjectsDirectory, "NuGet.Config");
        File.Copy(
            Path.Combine(BaselineHelper.GetAssetsDirectory(), $"{nugetConfigPrefix}.NuGet.Config"),
            nugetConfigPath);

        if (useCustomPackages)
        {
            // This package feed is optional.  You can use an alternative feed of dependency packages which can be 
            // required in sandboxed scenarios where public feeds need to be avoided.
            if (!Directory.Exists(Config.CustomPackagesPath))
            {
                throw new ArgumentException($"Specified CustomPackagesPath '{Config.CustomPackagesPath}' does not exist.");
            }

            string nugetConfig = File.ReadAllText(nugetConfigPath)
                .Replace("CUSTOM_PACKAGE_FEED", Config.CustomPackagesPath);
            File.WriteAllText(nugetConfigPath, nugetConfig);
        }
    }

    public void ExecuteCmd(string args, string? workingDirectory = null, Action<Process>? processConfigCallback = null,
        int? expectedExitCode = 0, int millisecondTimeout = -1)
    {
        (Process Process, string StdOut, string StdErr) executeResult = ExecuteHelper.ExecuteProcess(
            DotNetPath,
            args,
            OutputHelper,
            configureCallback: (process) => configureProcess(process, workingDirectory),
            millisecondTimeout: millisecondTimeout);

        if (expectedExitCode != null)
        {
            ExecuteHelper.ValidateExitCode(executeResult, (int)expectedExitCode);
        }

        void configureProcess(Process process, string? workingDirectory)
        {
            ConfigureProcess(process, workingDirectory);

            processConfigCallback?.Invoke(process);
        }
    }

    public static void ConfigureProcess(Process process, string? workingDirectory)
    {
        if (workingDirectory != null)
        {
            process.StartInfo.WorkingDirectory = workingDirectory;
        }

        process.StartInfo.EnvironmentVariables["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";
        process.StartInfo.EnvironmentVariables["DOTNET_SKIP_FIRST_TIME_EXPERIENCE"] = "1";
        process.StartInfo.EnvironmentVariables["DOTNET_ROOT"] = Config.DotNetDirectory;
        process.StartInfo.EnvironmentVariables["NUGET_PACKAGES"] = PackagesDirectory;
        process.StartInfo.EnvironmentVariables["PATH"] = $"{Config.DotNetDirectory}:{Environment.GetEnvironmentVariable("PATH")}";
        // Don't use the repo infrastructure
        process.StartInfo.EnvironmentVariables["ImportDirectoryBuildProps"] = "false";
        process.StartInfo.EnvironmentVariables["ImportDirectoryBuildTargets"] = "false";
        process.StartInfo.EnvironmentVariables["ImportDirectoryPackagesProps"] = "false";
    }

    /// <summary>
    /// Create a new .NET project and return the path to the created project folder.
    /// </summary>
    public string ExecuteNew(string projectType, string name, string? language = null, string? customArgs = null)
    {
        string projectDirectory = GetProjectDirectory(name);
        string options = $"--name {name} --output {projectDirectory}";
        if (language != null)
        {
            options += $" --language \"{language}\"";
        }
        if (string.IsNullOrEmpty(customArgs))
        {
            options += $" {customArgs}";
        }

        ExecuteCmd($"new {projectType} {options}");

        return projectDirectory;
    }

    private static bool DetermineIsMonoRuntime(string dotnetRoot)
    {
        string sharedFrameworkRoot = Path.Combine(dotnetRoot, "shared", "Microsoft.NETCore.App");
        if (!Directory.Exists(sharedFrameworkRoot))
        {
            return false;
        }

        string? version = Directory.GetDirectories(sharedFrameworkRoot).FirstOrDefault();
        if (version is null)
        {
            return false;
        }

        string sharedFramework = Path.Combine(sharedFrameworkRoot, version);

        // Check the presence of one of the mono header files.
        return File.Exists(Path.Combine(sharedFramework, "mono-gc.h"));
    }

    private static string GetProjectDirectory(string projectName) => Path.Combine(ProjectsDirectory, projectName);
}
