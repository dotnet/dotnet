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

public class ConfigureFingerprintTests : IDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory("CMakeFingerprint-").FullName;

    [Theory]
    [InlineData("unchanged", true)]
    [InlineData("target", true)]
    [InlineData("define", false)]
    [InlineData("argument", false)]
    [InlineData("missing", false)]
    [InlineData("overwritten", false)]
    public async Task ValidatesConfiguredInputsBeforeBuilding(string change, bool succeeds)
    {
        string outputDirectory = Path.Combine(_directory, "build");
        string fingerprint = Path.Combine(outputDirectory, ".cmake", "configure.fingerprint");
        string marker = Path.Combine(_directory, "compiled");
        string projectPath = Path.Combine(_directory, "test.proj");
        var sdkTargets = XDocument.Load(Path.Combine(AppContext.BaseDirectory, "testassets", "CMakeSdkBuild", "Microsoft.DotNet.CMake.Sdk.targets"));
        var project = new XElement("Project",
            new XElement("UsingTask",
                new XAttribute("TaskName", "Microsoft.DotNet.CMake.Sdk.ExecWithMutex"),
                new XAttribute("AssemblyFile", typeof(ExecWithMutex).Assembly.Location)),
            new XElement("PropertyGroup",
                new XElement("CMakeOutputDir", outputDirectory),
                new XElement("CMakeConfigScript", "echo configured"),
                new XElement("CMakeBuildScript", $"echo compiled > \"{marker}\"")),
            new XElement("ItemGroup",
                new XElement("CMakeDefines", new XAttribute("Include", "FEATURE"), new XAttribute("Value", "Enabled")),
                new XElement("CMakeArguments", new XAttribute("Include", "--log-level=DEBUG"))),
            new XElement("Target", new XAttribute("Name", "GetConfigScript")),
            new XElement("Target", new XAttribute("Name", "GetBuildScript")),
            new XElement("Target", new XAttribute("Name", "CreateFileApiQuery"),
                new XElement("MakeDir", new XAttribute("Directories", Path.GetDirectoryName(fingerprint)))),
            sdkTargets.Root.Elements("Target").Where(t => t.Attribute("Name")?.Value is "Configure" or "CoreCompile"));

        XElement mutation = change switch
        {
            "define" => new XElement("ItemGroup",
                new XElement("CMakeDefines", new XAttribute("Update", "FEATURE"), new XAttribute("Value", "enabled"))),
            "argument" => new XElement("ItemGroup",
                new XElement("CMakeArguments", new XAttribute("Remove", "@(CMakeArguments)")),
                new XElement("CMakeArguments", new XAttribute("Include", "--log-level=debug"))),
            "target" => new XElement("PropertyGroup", new XElement("CMakeBuildTarget", "another-target")),
            "missing" => new XElement("Delete", new XAttribute("Files", fingerprint)),
            "overwritten" => new XElement("WriteLinesToFile",
                new XAttribute("File", fingerprint), new XAttribute("Lines", "another-configuration"), new XAttribute("Overwrite", "true")),
            _ => null
        };
        project.Add(new XElement("Target", new XAttribute("Name", "ChangeInputs"), new XAttribute("DependsOnTargets", "Configure"),
            mutation));
        project.Add(new XElement("Target", new XAttribute("Name", "Run"),
            new XAttribute("DependsOnTargets", "ChangeInputs;CoreCompile")));
        new XDocument(project).Save(projectPath);

        var startInfo = new ProcessStartInfo(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet")
        {
            WorkingDirectory = _directory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (string argument in new[] { "msbuild", projectPath, "-target:Run", "-nologo", "-verbosity:minimal", "-nodeReuse:false" })
        {
            startInfo.ArgumentList.Add(argument);
        }

        using Process process = Process.Start(startInfo);
        Task<string> stdout = process.StandardOutput.ReadToEndAsync();
        Task<string> stderr = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        string output = await stdout + await stderr;

        Assert.True((process.ExitCode == 0) == succeeds, output);
        Assert.Equal(succeeds, File.Exists(marker));
        if (succeeds)
        {
            Assert.False(string.IsNullOrWhiteSpace(File.ReadAllText(fingerprint)));
            Assert.DoesNotContain("Enabled", File.ReadAllText(fingerprint), StringComparison.Ordinal);
        }
        else
        {
            Assert.Contains(change == "missing" ? "fingerprint is missing" : "differ from the configured build tree", output, StringComparison.Ordinal);
        }
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
