// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests;

// ProjectInstance.Build uses the shared default BuildManager.
[CollectionDefinition(nameof(XUnitRunnerTestCollection), DisableParallelization = true)]
public class XUnitRunnerTestCollection
{
}

[Collection(nameof(XUnitRunnerTestCollection))]
public class XUnitRunnerTests
{
    private static readonly string s_targetsPath = Path.Combine(
        AppContext.BaseDirectory, "testassets", "ArcadeSdkTools", "XUnit", "XUnit.Runner.targets");

    [Fact]
    public void CommandIsWrittenBeforeStartingTheRunner()
    {
        XElement target = XDocument.Load(s_targetsPath).Descendants("Target").Single();
        XElement[] tasks = target.Elements().ToArray();
        XElement writer = Assert.Single(target.Elements("WriteLinesToFile"));

        Assert.True(Array.IndexOf(tasks, target.Element("Delete")) < Array.IndexOf(tasks, writer));
        Assert.True(Array.IndexOf(tasks, writer) < Array.IndexOf(tasks, target.Element("Exec")));
        Assert.Equal("true", writer.Attribute("Overwrite").Value);
        Assert.Equal(";=== COMMAND LINE ===;$(_TestRunnerCommand)", writer.Attribute("Lines").Value);
        Assert.Equal("'$(TestCaptureOutput)' != 'false'", writer.Attribute("Condition").Value);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("", 7)]
    [InlineData("true", 0)]
    [InlineData("false", 0)]
    [InlineData("false", 7)]
    public void RunnerPreservesOutputAndExitCode(string captureOutput, int exitCode)
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), "arcade", "xunit runner " + Path.GetRandomFileName());
        Directory.CreateDirectory(tempDirectory);

        try
        {
            bool windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            string runnerPath = Path.Combine(tempDirectory, windows ? "fake runner.cmd" : "fake runner.sh");
            string logPath = Path.Combine(tempDirectory, "results with spaces", "test.log");
            string xmlPath = Path.ChangeExtension(logPath, ".xml");
            string htmlPath = Path.ChangeExtension(logPath, ".html");
            string assemblyPath = Path.Combine(tempDirectory, "test assembly.dll");
            string runnerTool = windows ? Environment.GetEnvironmentVariable("ComSpec") ?? "cmd.exe" : "/bin/sh";
            string runnerArguments = windows ? $"/d /c call \"{runnerPath}\"" : $"\"{runnerPath}\"";
            const string quotedArgument = "quoted & argument";

            // Stand in for Mono, without requiring a runtime or an xUnit package.
            // Windows-built assemblies also run on Unix; raw strings retain source line endings.
            File.WriteAllText(runnerPath, windows
                ? $"""
                    @echo off
                    echo runner stdout
                    echo runner stderr 1>&2
                    echo assembly=%2
                    echo xml=%5
                    echo html=%7
                    echo argument=%8
                    exit /b {exitCode}
                    """
                : $"""
                    printf '%s\n' 'runner stdout'
                    printf '%s\n' 'runner stderr' >&2
                    printf 'assembly="%s"\n' "$2"
                    printf 'xml="%s"\n' "$5"
                    printf 'html="%s"\n' "$7"
                    printf 'argument="%s"\n' "$8"
                    exit {exitCode}
                    """.ReplaceLineEndings("\n"));

            string projectPath = Path.Combine(tempDirectory, "Runner.proj");
            new XDocument(
                new XElement("Project",
                    new XElement("PropertyGroup",
                        new XElement("TestCaptureOutput", captureOutput),
                        new XElement("MonoTool", runnerTool),
                        new XElement("TestRuntimeAdditionalArguments", runnerArguments),
                        new XElement("NuGetPackageRoot", tempDirectory + Path.DirectorySeparatorChar),
                        new XElement("XUnitVersion", "unused")),
                    new[] { "MakeDir", "Delete", "WriteLinesToFile", "Message", "Exec", "Error" }.Select(task =>
                        new XElement("UsingTask",
                            new XAttribute("TaskName", "Microsoft.Build.Tasks." + task),
                            new XAttribute("AssemblyFile", typeof(Microsoft.Build.Tasks.Exec).Assembly.Location))),
                    new XElement("ItemGroup",
                        new XElement("TestToRun",
                            new XAttribute("Include", assemblyPath),
                            new XElement("TestRuntime", "Mono"),
                            new XElement("Architecture", "x64"),
                            new XElement("CurrentProcessArchitecture", "x64"),
                            new XElement("TestTimeout", "30000"),
                            new XElement("TestRunnerAdditionalArguments", $"\"{quotedArgument}\""),
                            new XElement("ResultsStdOutPath", logPath),
                            new XElement("ResultsXmlPath", xmlPath),
                            new XElement("ResultsHtmlPath", htmlPath))),
                    new XElement("Import", new XAttribute("Project", s_targetsPath))))
                .Save(projectPath);

            // Repeat with stale output to verify that each invocation starts a fresh log.
            for (int iteration = 0; iteration < 2; iteration++)
            {
                if (iteration != 0)
                {
                    File.AppendAllText(logPath, "stale output");
                }

                using var collection = new ProjectCollection();
                try
                {
                    var project = collection.LoadProject(projectPath).CreateProjectInstance();
                    var logger = new RunnerLogger();
                    Assert.Equal(exitCode == 0, project.Build("RunTests", new[] { logger }));
                    Assert.Equal(exitCode.ToString(), project.GetPropertyValue("_TestErrorCode"));
                    Assert.Equal(exitCode == 0 ? "" : $"Tests failed: {logPath} []{Environment.NewLine}", logger.Errors);

                    string output;
                    if (captureOutput == "false")
                    {
                        Assert.False(File.Exists(logPath));
                        Assert.DoesNotContain("=== COMMAND LINE ===", logger.Messages);
                        output = logger.Messages;
                    }
                    else
                    {
                        output = File.ReadAllText(logPath);
                        string command = project.GetPropertyValue("_TestRunnerCommand");
                        Assert.StartsWith($"=== COMMAND LINE ==={Environment.NewLine}{command}{Environment.NewLine}", output);
                        Assert.Contains($" >> \"{logPath}\" 2>&1", command);
                        Assert.Equal(1, output.Split("=== COMMAND LINE ===").Length - 1);
                        Assert.DoesNotContain("stale output", output);
                    }

                    Assert.Contains("runner stdout", output);
                    Assert.Contains("runner stderr", output);
                    Assert.Contains($"assembly=\"{assemblyPath}\"", output);
                    Assert.Contains($"xml=\"{xmlPath}\"", output);
                    Assert.Contains($"html=\"{htmlPath}\"", output);
                    Assert.Contains($"argument=\"{quotedArgument}\"", output);
                    if (exitCode == 0)
                    {
                        Assert.Equal(
                            new[] { xmlPath, htmlPath, logPath },
                            project.GetItems("FileWrites").Select(item => item.EvaluatedInclude));
                        Assert.Contains("Tests succeeded:", logger.Messages);
                    }
                }
                finally
                {
                    collection.UnloadAllProjects();
                }
            }
        }
        finally
        {
            Directory.Delete(tempDirectory, recursive: true);
        }
    }

    private sealed class RunnerLogger : ILogger
    {
        private readonly StringBuilder _errors = new();
        private readonly StringBuilder _messages = new();

        public string Parameters { get; set; }
        public LoggerVerbosity Verbosity { get; set; }
        public string Errors => _errors.ToString();
        public string Messages => _messages.ToString();

        public void Initialize(IEventSource eventSource)
        {
            eventSource.ErrorRaised += (_, e) => _errors.AppendLine(e.Message);
            eventSource.MessageRaised += (_, e) => _messages.AppendLine(e.Message);
        }

        public void Shutdown() { }
    }
}
