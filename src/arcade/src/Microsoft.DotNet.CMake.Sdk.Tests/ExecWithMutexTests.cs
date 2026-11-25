// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Microsoft.DotNet.CMake.Sdk.Tests;

public class ExecWithMutexTests : IDisposable
{
    private readonly string _testDirectory = Directory.CreateTempSubdirectory("CMakeMutex-").FullName;

    public static bool IsWindows => OperatingSystem.IsWindows();
    public static bool HasLinuxSetsid => OperatingSystem.IsLinux() && (File.Exists("/usr/bin/setsid") || File.Exists("/bin/setsid"));

    [ConditionalFact(typeof(ExecWithMutexTests), nameof(IsWindows))]
    public async Task SerializesDifferentCasingOfTheSameWindowsDirectory()
    {
        string directory = Directory.CreateDirectory(Path.Combine(_testDirectory, "BuildDirectory")).FullName;
        await VerifyConcurrentExecutions(directory, directory.ToUpperInvariant(), shouldSerialize: true);
    }

    [Fact]
    public async Task SerializesAnAliasedParentDirectory()
    {
        string parent = Directory.CreateDirectory(Path.Combine(_testDirectory, "physical")).FullName;
        string directory = Directory.CreateDirectory(Path.Combine(parent, "build")).FullName;
        string link = Path.Combine(_testDirectory, "alias");
        CreateDirectoryLink(link, parent);

        await VerifyConcurrentExecutions(directory, Path.Combine(link, "build"), shouldSerialize: true);
    }

    [Fact]
    public async Task SerializesFileApiReadsWithNativeBuilds()
    {
        string directory = Directory.CreateDirectory(Path.Combine(_testDirectory, "build")).FullName;
        string replyDirectory = Path.Combine(directory, ".cmake", "api", "v1", "reply");
        Directory.CreateDirectory(replyDirectory);
        File.WriteAllText(Path.Combine(replyDirectory, "index-1.json"),
            """{"reply":{"client-Microsoft.DotNet.CMake.Sdk":{"codemodel-v2":{"jsonFile":"codemodel.json"}}}}""");
        File.WriteAllText(Path.Combine(replyDirectory, "codemodel.json"),
            """{"configurations":[{"name":"Debug","targets":[]}]}""");

        string firstReady = Path.Combine(_testDirectory, "first-ready");
        string firstGo = Path.Combine(_testDirectory, "first-go");
        string firstEntered = Path.Combine(_testDirectory, "first-entered");
        string secondReady = Path.Combine(_testDirectory, "second-ready");
        string secondGo = Path.Combine(_testDirectory, "second-go");
        string secondEntered = Path.Combine(_testDirectory, "second-entered");
        string release = Path.Combine(_testDirectory, "release");

        using Process first = StartWorker("first", directory, firstReady, firstGo, firstEntered, release);
        Process second = null;
        try
        {
            try
            {
                await WaitFor(() => File.Exists(firstReady), first);
                File.WriteAllText(firstGo, string.Empty);
                await WaitFor(() => File.Exists(firstEntered), first);

                second = StartWorker("second", directory, secondReady, secondGo, secondEntered, release, readFileApi: true);
                await WaitFor(() => File.Exists(secondReady), first, second);
                File.WriteAllText(secondGo, string.Empty);
                await Task.Delay(TimeSpan.FromSeconds(2));
                Assert.False(File.Exists(secondEntered), "The File API read completed during a native build.");
            }
            finally
            {
                File.WriteAllText(firstGo, string.Empty);
                File.WriteAllText(secondGo, string.Empty);
                File.WriteAllText(release, string.Empty);
                if (second == null)
                {
                    await first.WaitForExitAsync();
                }
                else
                {
                    await Task.WhenAll(first.WaitForExitAsync(), second.WaitForExitAsync());
                }
            }

            Assert.True(first.ExitCode == 0, await first.StandardOutput.ReadToEndAsync() + await first.StandardError.ReadToEndAsync());
            Assert.True(second.ExitCode == 0, await second.StandardOutput.ReadToEndAsync() + await second.StandardError.ReadToEndAsync());
            Assert.True(File.Exists(secondEntered));
        }
        finally
        {
            second?.Dispose();
        }
    }

    [ConditionalFact(typeof(ExecWithMutexTests), nameof(HasLinuxSetsid))]
    public async Task SerializesAcrossLinuxSessions()
    {
        string setsid = File.Exists("/usr/bin/setsid") ? "/usr/bin/setsid" : "/bin/setsid";
        string directory = Directory.CreateDirectory(Path.Combine(_testDirectory, "build")).FullName;
        await VerifyConcurrentExecutions(directory, directory, shouldSerialize: true, sessionStarter: setsid);
    }

    [Fact]
    public async Task DoesNotSerializeDifferentDirectories()
    {
        string first = Directory.CreateDirectory(Path.Combine(_testDirectory, "first")).FullName;
        string second = Directory.CreateDirectory(Path.Combine(_testDirectory, "second")).FullName;

        await VerifyConcurrentExecutions(first, second, shouldSerialize: false);
    }

    private async Task VerifyConcurrentExecutions(string firstPath, string secondPath, bool shouldSerialize, string sessionStarter = null)
    {
        string go = Path.Combine(_testDirectory, "go");
        string release = Path.Combine(_testDirectory, "release");
        string firstReady = Path.Combine(_testDirectory, "first-ready");
        string secondReady = Path.Combine(_testDirectory, "second-ready");
        string firstEntered = Path.Combine(_testDirectory, "first-entered");
        string secondEntered = Path.Combine(_testDirectory, "second-entered");

        using Process first = StartWorker("first", firstPath, firstReady, go, firstEntered, release);
        Process second = null;
        try
        {
            try
            {
                second = StartWorker("second", secondPath, secondReady, go, secondEntered, release, sessionStarter);
                await WaitFor(() => File.Exists(firstReady) && File.Exists(secondReady), first, second);
                File.WriteAllText(go, string.Empty);
                await WaitFor(() => File.Exists(firstEntered) || File.Exists(secondEntered), first, second);

                if (shouldSerialize)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    Assert.False(File.Exists(firstEntered) && File.Exists(secondEntered),
                        "Both commands entered the same build tree at the same time.");
                }
                else
                {
                    await WaitFor(() => File.Exists(firstEntered) && File.Exists(secondEntered), first, second);
                }
            }
            finally
            {
                File.WriteAllText(go, string.Empty);
                File.WriteAllText(release, string.Empty);
                if (second == null)
                {
                    await first.WaitForExitAsync();
                }
                else
                {
                    await Task.WhenAll(first.WaitForExitAsync(), second.WaitForExitAsync());
                }
            }

            Assert.True(first.ExitCode == 0, await first.StandardOutput.ReadToEndAsync() + await first.StandardError.ReadToEndAsync());
            Assert.True(second.ExitCode == 0, await second.StandardOutput.ReadToEndAsync() + await second.StandardError.ReadToEndAsync());
            Assert.True(File.Exists(firstEntered) && File.Exists(secondEntered));
        }
        finally
        {
            second?.Dispose();
        }
    }

    private Process StartWorker(string name, string mutexPath, string ready, string go, string entered, string release, string sessionStarter = null, bool readFileApi = false)
    {
        string projectPath = Path.Combine(_testDirectory, $"{name}.proj");
        string taskName = readFileApi ? "GetCMakeArtifactsFromFileApi" : "ExecWithMutex";
        var project = new XElement("Project", new XAttribute("DefaultTargets", "Run"),
            new XElement("UsingTask",
                new XAttribute("TaskName", $"Microsoft.DotNet.CMake.Sdk.{taskName}"),
                new XAttribute("AssemblyFile", typeof(ExecWithMutex).Assembly.Location)));

        XElement operation;
        if (readFileApi)
        {
            string referencedProject = Path.Combine(_testDirectory, $"{name}-native.proj");
            new XDocument(
                new XElement("Project",
                    new XElement("Target", new XAttribute("Name", "GetCMakeOutputDirProperty"), new XAttribute("Returns", mutexPath)),
                    new XElement("Target", new XAttribute("Name", "GetConfigurationProperty"), new XAttribute("Returns", "Debug"))))
                .Save(referencedProject);
            project.Add(
                new XElement("PropertyGroup",
                    new XElement("ReferencedCMakeProject", referencedProject),
                    new XElement("ReferencedCMakeTargets", "widget")),
                new XElement("Import", new XAttribute("Project",
                    Path.Combine(AppContext.BaseDirectory, "testassets", "CMakeSdkBuild", "ProjectReference.targets"))));
            operation = new XElement("CallTarget", new XAttribute("Targets", "CopyNativeProjectBinariesFromFileApi"));
        }
        else
        {
            operation = new XElement(taskName,
                new XAttribute("MutexName", mutexPath),
                new XAttribute("Command", SignalAndWait(entered, release)));
        }

        project.Add(new XElement("Target", new XAttribute("Name", "Run"),
            new XElement("Exec", new XAttribute("Command", SignalAndWait(ready, go))),
            operation,
            readFileApi ? new XElement("Exec", new XAttribute("Command", SignalAndWait(entered, release))) : null));
        new XDocument(project).Save(projectPath);

        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet";
        var startInfo = new ProcessStartInfo
        {
            FileName = sessionStarter ?? dotnet,
            WorkingDirectory = _testDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        if (sessionStarter != null)
        {
            startInfo.ArgumentList.Add("--wait");
            startInfo.ArgumentList.Add(dotnet);
        }
        startInfo.ArgumentList.Add("msbuild");
        startInfo.ArgumentList.Add(projectPath);
        startInfo.ArgumentList.Add("-nologo");
        startInfo.ArgumentList.Add("-verbosity:quiet");
        startInfo.ArgumentList.Add("-nodeReuse:false");
        return Process.Start(startInfo);
    }

    private static string SignalAndWait(string signal, string release)
    {
        if (OperatingSystem.IsWindows())
        {
            string escapedRelease = release.Replace("'", "''");
            return $$"""
                echo ready>"{{signal}}"
                powershell.exe -NoProfile -NonInteractive -Command "$deadline = [DateTime]::UtcNow.AddSeconds(45); while (-not [IO.File]::Exists('{{escapedRelease}}')) { if ([DateTime]::UtcNow -ge $deadline) { exit 1 }; Start-Sleep -Milliseconds 50 }"
                """;
        }

        string quotedSignal = QuoteForShell(signal);
        string quotedRelease = QuoteForShell(release);
        string attempts = string.Join(" ", Enumerable.Range(1, 45));
        return $"""
            echo ready > {quotedSignal}
            for i in {attempts}; do [ -f {quotedRelease} ] && exit 0; sleep 1; done
            exit 1
            """;
    }

    private static string QuoteForShell(string path) => "'" + path.Replace("'", "'\\''") + "'";

    private static async Task WaitFor(Func<bool> condition, params Process[] workers)
    {
        DateTime deadline = DateTime.UtcNow.AddSeconds(30);
        while (!condition() && DateTime.UtcNow < deadline)
        {
            foreach (Process worker in workers)
            {
                if (worker.HasExited)
                {
                    string output = await worker.StandardOutput.ReadToEndAsync() + await worker.StandardError.ReadToEndAsync();
                    Assert.Fail($"Worker exited before reaching the barrier: {output}");
                }
            }

            await Task.Delay(50);
        }

        Assert.True(condition(), "Timed out waiting for the worker processes.");
    }

    private static void CreateDirectoryLink(string link, string target)
    {
        if (!OperatingSystem.IsWindows())
        {
            Directory.CreateSymbolicLink(link, target);
            return;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = Environment.GetEnvironmentVariable("ComSpec") ?? "cmd.exe",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        startInfo.ArgumentList.Add("/d");
        startInfo.ArgumentList.Add("/c");
        startInfo.ArgumentList.Add("mklink");
        startInfo.ArgumentList.Add("/J");
        startInfo.ArgumentList.Add(link);
        startInfo.ArgumentList.Add(target);
        using Process process = Process.Start(startInfo);
        process.WaitForExit();
        Assert.True(process.ExitCode == 0, process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd());
    }

    public void Dispose() => Directory.Delete(_testDirectory, recursive: true);
}
