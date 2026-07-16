// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.DotNet.ScenarioTests.Common;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Xml;
using Xunit.Abstractions;

namespace Microsoft.DotNet.ScenarioTests.SdkTemplateTests;

internal partial class DotNetSdkHelper
{
    private readonly string? _binlogDir;
    private readonly ITestOutputHelper _outputHelper;
    private const string PackageSourceCredentialsElementName = "packageSourceCredentials";

    [GeneratedRegex(
        $@"<{PackageSourceCredentialsElementName}\b[^>]*>.*?(</{PackageSourceCredentialsElementName}>|$)",
        RegexOptions.Singleline | RegexOptions.IgnoreCase)]
    private static partial Regex PackageSourceCredentialsRegex { get; }

    private static string EmbedFileInBinlogTargetsPath { get; } = Path.Combine(AppContext.BaseDirectory, "assets", "EmbedFileInBinlog.targets");

    // Sanitized copy of the RestoreConfigFile env var with any packageSourceCredentials element
    // removed. This is what we embed in binlogs so that CI feed credentials are never leaked.
    private static Lazy<string?> SanitizedRestoreConfigPathLazy { get; } = new(() =>
    {
        string? restoreConfigFile = Environment.GetEnvironmentVariable("RestoreConfigFile");
        if (string.IsNullOrEmpty(restoreConfigFile) || !File.Exists(restoreConfigFile))
        {
            return null;
        }

        string? binlogDir = Environment.GetEnvironmentVariable(ScenarioTestFixture.BinlogDirEnvironmentVariable);
        string outDir = string.IsNullOrEmpty(binlogDir) ? Path.GetTempPath() : binlogDir;
        Directory.CreateDirectory(outDir);
        string destPath = Path.Combine(outDir, "NuGet.Config.sanitized");
        SanitizeNuGetConfig(restoreConfigFile, destPath);
        return destPath;
    });

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

    private void ExecuteCmd(string args, string workingDirectory, Action<Process>? additionalProcessConfigCallback = null, int expectedExitCode = 0, int millisecondTimeout = -1, string? culture = null)
    {
        if (!string.IsNullOrEmpty(SdkVersion) && !File.Exists(Path.Combine(workingDirectory, "global.json")))
        {
            ExecuteCmdImpl($"new globaljson --sdk-version {SdkVersion}", workingDirectory);
        }

        ExecuteCmdImpl(args, workingDirectory, additionalProcessConfigCallback, expectedExitCode, millisecondTimeout, culture);
    }

    private void ExecuteCmdImpl(string args, string workingDirectory, Action<Process>? additionalProcessConfigCallback = null, int expectedExitCode = 0, int millisecondTimeout = -1, string? culture = null)
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
            ConfigureProcess(process, workingDirectory, DotNetRoot, nugetPackagesDirectory: null, setPath: false, culture: culture);

            additionalProcessConfigCallback?.Invoke(process);
        }
    }

    private static void ConfigureProcess(Process process, string workingDirectory, string dotnetRoot, string? nugetPackagesDirectory = null, bool setPath = false, bool clearEnv = false, string? culture = null)
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

        if (!string.IsNullOrEmpty(culture))
        {
            process.StartInfo.EnvironmentVariables["DOTNET_CLI_UI_LANGUAGE"] = culture;
        }

        if (!string.IsNullOrEmpty(nugetPackagesDirectory))
        {
            process.StartInfo.EnvironmentVariables["NUGET_PACKAGES"] = nugetPackagesDirectory;
        }

        if (setPath)
        {
            process.StartInfo.EnvironmentVariables["PATH"] = $"{dotnetRoot}:{Environment.GetEnvironmentVariable("PATH")}";
        }
    }

    public void ExecuteBuild(string projectDirectory, string? culture = null) =>
        ExecuteCmd($"build {GetBinLogOption(projectDirectory, "build")}", projectDirectory, culture: culture);

    /// <summary>
    /// Create a new .NET project and return the path to the created project folder.
    /// </summary>
    public string ExecuteNew(string projectType, string projectName, string projectDirectory, string? language = null, string? customArgs = null, string? culture = null)
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

        ExecuteCmd($"new {projectType} {options}", projectDirectory, culture: culture);

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

    public void ExecuteRun(string projectDirectory, string[]? frameworks = null, string? culture = null)
    {
        if (frameworks != null)
        {
            foreach (var item in frameworks)
            {
                ExecuteCmd($"run {GetBinLogOption(projectDirectory, "run")} --framework " + item, projectDirectory, culture: culture);
            }
        }
        else
        {
            ExecuteCmd($"run {GetBinLogOption(projectDirectory, "run")}", projectDirectory, culture: culture);
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

    /// <summary>
    /// Runs <c>dotnet test</c> for a generated project while collecting code coverage, then verifies a
    /// coverage artifact was produced. Supports both the VSTest runner and Microsoft.Testing.Platform (MTP).
    /// </summary>
    public void ExecuteTestWithCoverage(string projectDirectory, bool useMicrosoftTestingPlatform)
    {
        string resultsDirectory = Path.Combine(projectDirectory, "TestResults");

        string coverageArgs;
        if (useMicrosoftTestingPlatform)
        {
            // On the .NET 10+ SDK the VSTest bridge was removed from Microsoft.Testing.Platform, so
            // `dotnet test` has to run in its dedicated MTP mode. That mode is opted into via a global.json
            // that EnableTestingPlatformRunner writes before `dotnet new` (see its remarks for why the
            // ordering matters).
            //
            // MTP mode uses --coverage (Microsoft.Testing.Extensions.CodeCoverage) rather than the VSTest
            // --collect data collector. Unknown switches are forwarded to the test app, so only MTP options
            // are passed here (notably no --nologo, which the app rejects with "Zero tests ran").
            coverageArgs = "--coverage --coverage-output-format cobertura --coverage-output coverage.cobertura.xml";
        }
        else
        {
            coverageArgs = "--collect \"Code Coverage\"";
        }

        ExecuteCmd(
            $"test {coverageArgs} --results-directory \"{resultsDirectory}\" {GetBinLogOption(projectDirectory, "test")}",
            workingDirectory: projectDirectory);

        IReadOnlyList<string> coverageFiles = FindCoverageFiles(resultsDirectory);
        if (coverageFiles.Count == 0)
        {
            throw new InvalidOperationException(
                $"Expected a code coverage artifact under '{resultsDirectory}' but none was produced.");
        }

        foreach (string coverageFile in coverageFiles)
        {
            if (new FileInfo(coverageFile).Length == 0)
            {
                throw new InvalidOperationException($"Code coverage artifact was empty: '{coverageFile}'.");
            }

            _outputHelper.WriteLine($"Produced code coverage artifact: {coverageFile}");
        }
    }

    private static IReadOnlyList<string> FindCoverageFiles(string resultsDirectory)
    {
        if (!Directory.Exists(resultsDirectory))
        {
            return Array.Empty<string>();
        }

        return Directory.EnumerateFiles(resultsDirectory, "*", SearchOption.AllDirectories)
            .Where(file =>
                file.EndsWith(".coverage", StringComparison.OrdinalIgnoreCase) ||
                file.EndsWith(".cobertura.xml", StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    /// <summary>
    /// Opts a generated project into the Microsoft.Testing.Platform mode of <c>dotnet test</c> by merging
    /// <c>"test": { "runner": "Microsoft.Testing.Platform" }</c> into a global.json in the project directory,
    /// preserving any keys already present (for example an SDK version pin).
    /// </summary>
    /// <remarks>
    /// This MUST be called before <c>dotnet new ... --test-runner Microsoft.Testing.Platform</c>. That
    /// template switch opts in by <em>modifying the nearest global.json</em> it finds walking up from the
    /// output directory. With no local global.json it rewrites the repo-root one that every generated test
    /// project shares, which forces the sibling VSTest template tests into MTP mode and breaks them. Writing
    /// the local file first keeps the template's modification scoped to this project.
    /// </remarks>
    public static void EnableTestingPlatformRunner(string projectDirectory)
    {
        string globalJsonPath = Path.Combine(projectDirectory, "global.json");
        JsonObject root = File.Exists(globalJsonPath)
            ? JsonNode.Parse(File.ReadAllText(globalJsonPath)) as JsonObject ?? new JsonObject()
            : new JsonObject();

        root["test"] = new JsonObject { ["runner"] = "Microsoft.Testing.Platform" };
        File.WriteAllText(globalJsonPath, root.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }

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

        string binlogArgs = $"/bl:{Path.Combine(binlogDir, $"{fileName}.binlog")}";

        // Embed a sanitized copy of the NuGet.Config used by NuGet restore in the binlog so that
        // failures originating from the config (e.g. malformed XML breaking restore) can be
        // debugged from the binlog alone. Credentials (packageSourceCredentials) are stripped
        // before embedding. See https://github.com/dotnet/source-build/issues/5350.
        string? sanitizedConfigPath = SanitizedRestoreConfigPathLazy.Value;
        if (!string.IsNullOrEmpty(sanitizedConfigPath))
        {
            binlogArgs += $" /p:\"CustomAfterMicrosoftCommonTargets={EmbedFileInBinlogTargetsPath}\""
                + $" /p:\"CustomAfterMicrosoftCommonCrossTargetingTargets={EmbedFileInBinlogTargetsPath}\""
                + $" /p:\"EmbedFileInBinlogPath={sanitizedConfigPath}\"";
        }

        return binlogArgs;
    }

    /// <summary>
    /// Writes a copy of <paramref name="sourcePath"/> to <paramref name="destPath"/> with any
    /// <c>packageSourceCredentials</c> element removed and replaced by a comment placeholder.
    /// </summary>
    private static void SanitizeNuGetConfig(string sourcePath, string destPath)
    {
        const string PlaceholderComment = $"{PackageSourceCredentialsElementName} removed for binlog embedding";

        string content = File.ReadAllText(sourcePath);
        string sanitized = PackageSourceCredentialsRegex.Replace(
            content,
            "<!-- " + PlaceholderComment + " -->");
        File.WriteAllText(destPath, sanitized);
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
