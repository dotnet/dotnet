// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests;

[Collection(nameof(XUnitRunnerTestCollection))]
public class VisualStudioOptimizationDataTests
{
    private static readonly string s_targetsPath = Path.Combine(
        AppContext.BaseDirectory, "testassets", "ArcadeSdkTools", "VisualStudio.AcquireOptimizationData.targets");

    [Fact]
    public void ExplicitDropTakesPrecedenceOverSourceBranch()
    {
        string tempDirectory = Path.Combine(
            Path.GetTempPath(), "arcade", "visual studio optimization data " + Path.GetRandomFileName());
        Directory.CreateDirectory(tempDirectory);

        try
        {
            bool windows = OperatingSystem.IsWindows();
            string invocationPath = Path.Combine(tempDirectory, "drop arguments.txt");
            string dropToolPath = Path.Combine(tempDirectory, windows ? "fake drop.cmd" : "fake drop.sh");
            string optimizationDataDirectory = Path.Combine(tempDirectory, "optimization data");
            string logDirectory = Path.Combine(tempDirectory, "logs");

            File.WriteAllText(dropToolPath, (windows
                ? $"""
                    @echo off
                    setlocal EnableExtensions EnableDelayedExpansion
                    if not "%~1"=="get" exit /b 1
                    :record
                    if "%~1"=="" exit /b 0
                    set "argument=%~1"
                    >>"{invocationPath}" echo(!argument!
                    shift
                    goto record
                    """
                : $"""
                    #!/bin/sh
                    [ "$1" = "get" ] || exit 1
                    printf '%s\n' "$@" >> "{invocationPath}"
                    """).ReplaceLineEndings(windows ? "\r\n" : "\n"));

            if (!windows)
            {
                File.SetUnixFileMode(
                    dropToolPath,
                    UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
            }

            string projectPath = Path.Combine(tempDirectory, "AcquireOptimizationData.proj");
            new XDocument(
                new XElement("Project",
                    new[] { "Error", "Message", "MakeDir", "Exec" }.Select(task =>
                        new XElement("UsingTask",
                            new XAttribute("TaskName", "Microsoft.Build.Tasks." + task),
                            new XAttribute("AssemblyFile", typeof(Microsoft.Build.Tasks.Exec).Assembly.Location))),
                    new XElement("PropertyGroup",
                        new XElement("_DropToolExists", "true"),
                        new XElement("_DropToolPath", dropToolPath),
                        new XElement("VisualStudioIbcDrop", "OptimizationData/explicit/drop"),
                        new XElement("VisualStudioIbcSourceBranchName", "default-branch"),
                        new XElement("VisualStudioIbcRepositoryName", "default-repo"),
                        new XElement("OfficialBuild", "false"),
                        new XElement("IbcOptimizationDataDir", optimizationDataDirectory),
                        new XElement("ArtifactsLogDir", logDirectory + Path.DirectorySeparatorChar)),
                    new XElement("Import", new XAttribute("Project", s_targetsPath))))
                .Save(projectPath);

            using var collection = new ProjectCollection();
            try
            {
                var project = collection.LoadProject(projectPath).CreateProjectInstance();
                Assert.True(project.Build(
                    "_DownloadVisualStudioOptimizationDataOpt",
                    Array.Empty<ILogger>()));

                string[] invocationArguments = File.ReadAllLines(invocationPath);
                Assert.Equal(
                    new[]
                    {
                        "get",
                        "--dropservice",
                        "https://devdiv.artifacts.visualstudio.com",
                        "--name",
                        "OptimizationData/explicit/drop",
                        "--dest",
                        optimizationDataDirectory,
                        "--traceto",
                        Path.Combine(logDirectory, "OptimizationDataAcquisition.log"),
                    },
                    invocationArguments);
                Assert.DoesNotContain("list", invocationArguments);
                Assert.Equal("OptimizationData/explicit/drop", project.GetPropertyValue("_DropName"));
            }
            finally
            {
                collection.UnloadAllProjects();
            }
        }
        finally
        {
            Directory.Delete(tempDirectory, recursive: true);
        }
    }
}
