// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.DotNet.ScenarioTests.Common;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml;
using Xunit.Abstractions;

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

internal class DotNetSdkHelper
{
    private readonly string? _binlogDir;
    private readonly ITestOutputHelper _outputHelper;

    public string DotNetRoot { get; }

    public string? SdkVersion { get; }

    public string DotNetExecutablePath  =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Path.Combine(DotNetRoot, "dotnet.exe") : Path.Combine(DotNetRoot, "dotnet");

    public DotNetSdkHelper(ITestOutputHelper outputHelper, string dotnetRoot, string? sdkVersion, string? binlogDir)
    {
        _outputHelper = outputHelper;
        DotNetRoot = dotnetRoot;
        SdkVersion = sdkVersion;
        _binlogDir = binlogDir;
    }

    private void ExecuteCmd(string args, string workingDirectory, Action<Process>? additionalProcessConfigCallback = null, int expectedExitCode = 0, int millisecondTimeout = -1)
    {
        if (!string.IsNullOrEmpty(SdkVersion) && !File.Exists(Path.Combine(workingDirectory, "global.json")))
        {
            ExecuteCmdImpl($"new globaljson --sdk-version {SdkVersion}", workingDirectory);
        }

        ExecuteCmdImpl(args, workingDirectory, additionalProcessConfigCallback, expectedExitCode, millisecondTimeout);
    }

    private void ExecuteCmdImpl(string args, string workingDirectory, Action<Process>? additionalProcessConfigCallback = null, int expectedExitCode = 0, int millisecondTimeout = -1)
    {
        (Process Process, string StdOut, string StdErr) executeResult = ExecuteHelper.ExecuteProcess(
            DotNetExecutablePath,
            args,
            _outputHelper,
            configure: (process) => configureProcess(process, workingDirectory),
            millisecondTimeout: millisecondTimeout);

        ExecuteHelper.ValidateExitCode(executeResult, expectedExitCode);

        void configureProcess(Process process, string workingDirectory)
        {
            ConfigureProcess(process, workingDirectory, DotNetRoot, nugetPackagesDirectory: null, setPath: false);

            additionalProcessConfigCallback?.Invoke(process);
        }
    }

    private static void ConfigureProcess(Process process, string workingDirectory, string dotnetRoot, string? nugetPackagesDirectory = null, bool setPath = false, bool clearEnv = false)
    {
        process.StartInfo.WorkingDirectory = workingDirectory;

        // The `dotnet test` execution context sets a number of dotnet related ENVs that cause issues when executing
        // dotnet commands. Same for MSBuild which adds env vars when invoking the runner via the Exec task.
        // Clear these to avoid side effects.
        foreach (string key in process.StartInfo.Environment.Keys.Where(key => key.StartsWith("DOTNET_", StringComparison.OrdinalIgnoreCase) || key.StartsWith("MSBUILD", StringComparison.OrdinalIgnoreCase)).ToArray())
        {
            process.StartInfo.Environment.Remove(key);
        }

        process.StartInfo.EnvironmentVariables["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";
        process.StartInfo.EnvironmentVariables["DOTNET_SKIP_FIRST_TIME_EXPERIENCE"] = "1";
        process.StartInfo.EnvironmentVariables["DOTNET_ROOT"] = dotnetRoot;
        process.StartInfo.EnvironmentVariables["DOTNET_ROLL_FORWARD"] = "Major";
        // Don't use the repo infrastructure
        process.StartInfo.EnvironmentVariables["ImportDirectoryBuildProps"] = "false";
        process.StartInfo.EnvironmentVariables["ImportDirectoryBuildTargets"] = "false";
        process.StartInfo.EnvironmentVariables["ImportDirectoryPackagesProps"] = "false";

        if (!string.IsNullOrEmpty(nugetPackagesDirectory))
        {
            process.StartInfo.EnvironmentVariables["NUGET_PACKAGES"] = nugetPackagesDirectory;
        }

        if (setPath)
        {
            process.StartInfo.EnvironmentVariables["PATH"] = $"{dotnetRoot}:{Environment.GetEnvironmentVariable("PATH")}";
        }
    }

    public void ExecuteBuild(string projectDirectory) =>
        ExecuteCmd($"build {GetBinLogOption(projectDirectory, "build")}", projectDirectory);

    /// <summary>
    /// Create a new .NET project and return the path to the created project folder.
    /// </summary>
    public string ExecuteNew(string projectType, string projectName, string projectDirectory, string? language = null, string? customArgs = null)
    {
        string options = $"--name {projectName} --output {projectDirectory}";
        if (language != null)
        {
            options += $" --language \"{language}\"";
        }
        if (!string.IsNullOrEmpty(customArgs))
        {
            options += $" {customArgs}";
        }

        ExecuteCmd($"new {projectType} {options}", projectDirectory);

        return projectDirectory;
    }

    public void ExecutePublish(string projectDirectory, string? rid = null, bool? selfContained = null, bool trimmed = false, bool readyToRun = false, bool? aot = false, string[]? frameworks = null)
    {
        string options = string.Empty;
        string binlogDifferentiator = string.Empty;

        if (!string.IsNullOrEmpty(rid))
        {
            options += $" -r {rid}";
            binlogDifferentiator += $"-{rid}";
        }
        
        if (selfContained.HasValue)
        {
            options += $" --self-contained {selfContained.Value.ToString().ToLowerInvariant()}";
            if (selfContained.Value)
            {
                binlogDifferentiator += "self-contained";
                if (trimmed)
                {
                    options += " /p:PublishTrimmed=true";
                    binlogDifferentiator += "-trimmed";
                }
                if (readyToRun)
                {
                    options += " /p:PublishReadyToRun=true";
                    binlogDifferentiator += "-R2R";
                }
            }
        }

        if (aot.HasValue)
        {
            options += $" /p:PublishAot={aot.Value.ToString().ToLowerInvariant()}";
            if (aot.Value)
            {
                binlogDifferentiator += "-aot";
            }
        }

        if (frameworks != null)
        {
            foreach (var item in frameworks)
            {
                ExecuteCmd(
                args: $"publish {options} {GetBinLogOption(projectDirectory, "publish", binlogDifferentiator)} --framework " + item,
                workingDirectory: projectDirectory);
            }
        }
        else
        {
            ExecuteCmd(
                args: $"publish {options} {GetBinLogOption(projectDirectory, "publish", binlogDifferentiator)}",
                workingDirectory: projectDirectory);
        }
    }

    public void ExecuteRun(string projectDirectory, string[]? frameworks = null)
    {
        if (frameworks != null)
        {
            foreach (var item in frameworks)
            {
                ExecuteCmd($"run {GetBinLogOption(projectDirectory, "run")} --framework " + item, projectDirectory);
            }
        }
        else
        {
            ExecuteCmd($"run {GetBinLogOption(projectDirectory, "run")}", projectDirectory);
        }
    }

    public void ExecuteRunWeb(string projectDirectory)
    {
        //checks what Process.Kill() will return based. Windows this is -1, Mac and Linux this appears 
        //to be process based. Currently 137
        int exitCode = OperatingSystemFinder.IsWindowsPlatform() ? -1 : 137;

        ExecuteCmd(
            $"run {GetBinLogOption(projectDirectory, "run")}",
            projectDirectory,
            additionalProcessConfigCallback: processConfigCallback,
            expectedExitCode: exitCode,
            millisecondTimeout: 60000);

        void processConfigCallback(Process process)
        {
            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (e.Data?.Contains("Application started. Press Ctrl+C to shut down.") ?? false)
                {
                    process.Kill(true);
                    process.WaitForExit();
                }
            });
        }
    }

    public void ExecuteRunUIApp(string projectDirectory, string[]? frameworks = null)
    {
        if (frameworks != null)
        {
            foreach (var item in frameworks)
            {
                ExecuteCmd(
                $"run {GetBinLogOption(projectDirectory, "run")} --framework " + item,
                projectDirectory,
                additionalProcessConfigCallback: processConfigCallback,
                millisecondTimeout: 30000);
            }
        }
        else
        {
            ExecuteCmd(
                $"run {GetBinLogOption(projectDirectory, "run")}",
                projectDirectory,
                additionalProcessConfigCallback: processConfigCallback,
                millisecondTimeout: 30000);
        }

        [DllImport("Kernel32.dll")]
        static extern bool TerminateProcess(IntPtr process, uint uExit);
        async void processConfigCallback(Process process)
        {
            await Task.Delay(5000);
            while (!checkProcess(projectDirectory, process))
            {
                await Task.Delay(5000);
            }
            TerminateProcess(process.Handle, 0);
            process.WaitForExit();
        }

        bool checkProcess(string projectDirectory, Process process)
        {
            try
            {
                Process.GetProcessById(process.Id);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }

    public void ExecuteTest(string projectDirectory) =>
        ExecuteCmd($"test {GetBinLogOption(projectDirectory, "test")}", workingDirectory: projectDirectory);

    private string GetBinLogOption(string projectDirectory, string command, string? differentiator = null)
    {
        string binlogDir = _binlogDir is null ?
            projectDirectory :
            Path.Combine(_binlogDir, Path.GetFileName(projectDirectory)!);

        string fileName = $"{command}";
        if (!string.IsNullOrEmpty(differentiator))
        {
            fileName += $"-{differentiator}";
        }

        return $"/bl:{Path.Combine(binlogDir, $"{fileName}.binlog")}";
    }

    public void ExecuteAddClassReference(string projectDirectory)
    {
        //Very hacky fix to grab class library path assuming Console referencing Classlib
        string classDirectory = projectDirectory.Replace("Console", "ClassLib");
        ExecuteCmd($"add reference {classDirectory}", projectDirectory);
    }

    public void ExecuteWorkloadInstall(string projectDirectory, string workloadIds)
    {
        ExecuteCmd($"workload install {workloadIds} --skip-manifest-update", projectDirectory);
    }

    public string ExecuteWorkloadList(string projectDirectory, string workloadIds, bool shouldBeInstalled, 
        string originalSource = "", bool firstRun = false)
    {
        ExecuteCmd($"workload list", projectDirectory, additionalProcessConfigCallback: processConfigCallback);

        void processConfigCallback(Process process)
        {
            string output = "";
            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (e.Data is null)
                {
                    return;
                }

                output += e.Data;
                if (!output.Contains("find additional workloads to install."))
                {
                    return;
                }

                if (output.Contains(workloadIds))
                {
                    if (!shouldBeInstalled)
                    {
                        if (firstRun)
                        {
                            originalSource = output;
                        }
                        else if(output != originalSource)
                        {
                            _outputHelper.WriteLine("output is " + output);
                            _outputHelper.WriteLine("originalSource is " + originalSource);
                            throw new Exception($"{workloadIds} shouldn't be installed but was found.");
                        }
                    }
                    _outputHelper.WriteLine($"{workloadIds} is installed");
                }
                else
                {
                    if (shouldBeInstalled)
                    {
                        throw new Exception($"{workloadIds} should be installed but wasn't found.");
                    }
                    _outputHelper.WriteLine($"{workloadIds} is not installed");
                }
            });
        }

        return originalSource;
    }

    public void ExecuteWorkloadUninstall(string projectDirectory, string workloadIds)
    {
        ExecuteCmd($"workload uninstall {workloadIds}", projectDirectory);
    }

    public void ExecuteAddMultiTFM(string projectName, string projectDirectory, DotNetLanguage language, string[] frameworks)
    {
        string extension;
        switch (language)
        {
            case DotNetLanguage.CSharp:
                extension = ".csproj";
                break;
            case DotNetLanguage.FSharp:
                extension = ".fsproj";
                break;
            case DotNetLanguage.VB:
                extension = ".vbproj";
                break;
            default:
                extension = ".csproj";
                break;
        }
        string framework = "";
        foreach (var item in frameworks)
        {
            framework += item + ";";
        }

        XmlDocument document = new XmlDocument();
        projectDirectory = Path.Combine(projectDirectory, projectName + extension);
        document.Load(projectDirectory);
        if (document.HasChildNodes)
        {
            try
            {
                if (document.FirstChild != null) {
                    XmlNode root = document.FirstChild;
                    XmlNode? oldNode = root.SelectSingleNode(".//TargetFramework");
                    XmlNode newNode = document.CreateElement("TargetFrameworks");
                    newNode.InnerXml = framework;
                    if (oldNode != null && oldNode.ParentNode != null)
                    {
                        oldNode.ParentNode.InsertBefore(newNode, oldNode);
                        oldNode.ParentNode.RemoveChild(oldNode);
                    }
                    document.Save(projectDirectory);
                }
            }
            catch (XPathException e)
            {
                _outputHelper.WriteLine("Unable to find node");
                _outputHelper.WriteLine(e.Message);
                throw;
            }
        }
    }

    internal void CopyHelper(string projectDirectory, string existing, bool recursive)
    {
        var sourceDirectory = new DirectoryInfo(existing);
        if (!sourceDirectory.Exists)
        {
            throw new DirectoryNotFoundException($"Existing Directory not found: {existing}");
        }
        DirectoryInfo[] directoryInfo = sourceDirectory.GetDirectories();
        Directory.CreateDirectory(projectDirectory);
        foreach (var file in sourceDirectory.GetFiles())
        {
            string targetPath = Path.Combine(projectDirectory, file.Name);
            file.CopyTo(targetPath);
            _outputHelper.WriteLine($"Copying {file.Name} to {targetPath}");
        }
        if (recursive)
        {
            foreach (var directory in directoryInfo)
            {
                string newProjectDirectory = Path.Combine(projectDirectory, directory.Name);
                CopyHelper(newProjectDirectory, directory.FullName, recursive);
            }
        }
    }
}
