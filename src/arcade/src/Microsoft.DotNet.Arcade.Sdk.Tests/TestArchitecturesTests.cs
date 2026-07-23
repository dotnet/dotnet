// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Xunit;

namespace Microsoft.DotNet.Arcade.Sdk.Tests
{
    public class TestArchitecturesTests
    {
        private static readonly string s_toolsDirectory = Path.Combine(
            AppContext.BaseDirectory,
            "testassets",
            "ArcadeSdkTools");

        [Fact]
        public void VSTestPassesRequestedArchitectureToDotNetTest()
        {
            XDocument targets = LoadTargets("VSTest.targets");

            string command = targets
                .Descendants("_TestRunnerCommand")
                .Select(element => element.Value)
                .Single(value => value.Contains("$(DotNetTool)", StringComparison.Ordinal));

            Assert.Contains("--arch %(TestToRun.Architecture)", command);
        }

        [Fact]
        public void XUnitCoreRequiresAnExactArchitectureHost()
        {
            XDocument targets = LoadTargets("XUnit", "XUnit.Runner.targets");
            XElement runTests = GetTarget(targets, "RunTests");

            Assert.Contains(
                runTests.Descendants("_TestRunner"),
                element => element.Attribute("Condition")?.Value.Contains(
                    "'$(_TestArchitecture)' == '$(_DotNetToolArchitecture)'",
                    StringComparison.Ordinal) == true);
            Assert.Contains(
                runTests.Descendants("_TestRunner"),
                element => element.Attribute("Condition")?.Value.Contains(
                    "Exists('$(_ArchitectureSpecificDotNetTool)')",
                    StringComparison.Ordinal) == true);
            Assert.Contains(
                runTests.Descendants("Error"),
                element => element.Attribute("Text")?.Value.Contains(
                    "requires a matching dotnet host",
                    StringComparison.Ordinal) == true);
            Assert.Contains(
                runTests.Descendants("Error"),
                element => element.Attribute("Text")?.Value.Contains(
                    "path cannot be determined",
                    StringComparison.Ordinal) == true);
        }

        [Theory]
        [InlineData("XUnitV3", "XUnitV3.Runner.targets")]
        [InlineData("Microsoft.Testing.Platform", "Microsoft.Testing.Platform.targets")]
        public void FixedHostRunnersOptIntoArchitectureValidation(string directory, string fileName)
        {
            XDocument targets = LoadTargets(directory, fileName);
            XElement runTests = GetTarget(targets, "RunTests");

            Assert.Contains(
                targets.Descendants("_TestRunnerRequiresMatchingArchitecture"),
                element => element.Value == "true");
            Assert.Contains(
                runTests.Descendants("_TestRunner"),
                element => element.Attribute("Condition")?.Value.Contains(
                    "'$(_UseDefaultDotNetRunCommand)' == 'true'",
                    StringComparison.Ordinal) == true);
            Assert.Contains(
                runTests.Descendants("Error"),
                element => element.Attribute("Text")?.Value.Contains(
                    "requires a matching dotnet host",
                    StringComparison.Ordinal) == true);
            Assert.Contains(
                runTests.Descendants("Error"),
                element => element.Attribute("Text")?.Value.Contains(
                    "path cannot be determined",
                    StringComparison.Ordinal) == true);
        }

        [Fact]
        public void TestsValidateRequestedArchitectureAgainstFixedHost()
        {
            XDocument targets = LoadTargets("Tests.targets");
            XElement initialize = GetTarget(targets, "_InitializeTestArchitectures");
            XElement validate = GetTarget(targets, "_ValidateTestArchitectures");

            Assert.Contains(initialize.Descendants("_CurrentProcessArchitecture"), element =>
                element.Value.Contains("RuntimeInformation]::ProcessArchitecture", StringComparison.Ordinal));
            Assert.Contains(initialize.Descendants("_BuiltTestArchitecture"), element =>
                element.Value.Contains("AppHostRuntimeIdentifier", StringComparison.Ordinal) &&
                !element.Value.Contains("DefaultAppHostRuntimeIdentifier", StringComparison.Ordinal));
            Assert.Contains(initialize.Descendants("_BuiltTestArchitecture"), element =>
                element.Value.Contains("DefaultAppHostRuntimeIdentifier", StringComparison.Ordinal) &&
                element.Attribute("Condition")?.Value.Contains("'$(UseAppHost)' == 'true'", StringComparison.Ordinal) == true);
            Assert.Contains(initialize.Descendants("_BuiltTestArchitecture"), element =>
                element.Value == "$(_DotNetToolArchitecture)" &&
                element.Attribute("Condition")?.Value.Contains("'$(UseAppHost)' != 'true'", StringComparison.Ordinal) == true);
            Assert.Contains(initialize.Descendants("_BuiltTestArchitecture"), element =>
                element.Value == "x86" &&
                element.Attribute("Condition")?.Value.Contains("'$(Prefer32Bit)' == 'true'", StringComparison.Ordinal) == true);
            Assert.Contains(initialize.Descendants("_BuiltTestArchitecture"), element =>
                element.Value == "$(_CurrentOSArchitecture)");
            Assert.Contains(initialize.Descendants("TestArchitectures"), element =>
                element.Value == "x64" &&
                element.Attribute("Condition")?.Value.Contains("'$(TestRunnerName)' == 'XUnit'", StringComparison.Ordinal) == true &&
                element.Attribute("Condition")?.Value.Contains("'$(OS)' == 'Windows_NT'", StringComparison.Ordinal) == true);
            Assert.Contains(validate.Descendants("Error"), element =>
                element.Attribute("Text")?.Value.Contains(
                    "PlatformTarget, Prefer32Bit, or RuntimeIdentifier",
                    StringComparison.Ordinal) == true);
            Assert.Contains(validate.Descendants("_MismatchedTestArchitecture"), element =>
                element.Attribute("Condition")?.Value.Contains(
                    "'%(TestToRun.UseAppHost)' == 'true'",
                    StringComparison.Ordinal) == true);
            Assert.Contains(validate.Descendants("_MismatchedTestArchitecture"), element =>
                element.Attribute("Condition")?.Value.Contains(
                    "'%(TestToRun.TestRuntime)' == 'Full'",
                    StringComparison.Ordinal) == true);

            XElement testToRun = initialize
                .Document
                .Descendants("TestToRun")
                .Single();
            Assert.Contains(testToRun.Elements("UseAppHost"), element => element.Value == "$(UseAppHost)");
            Assert.Contains(testToRun.Elements("UseDefaultDotNetRunCommand"), element =>
                element.Attribute("Condition")?.Value.Contains("'$(RunCommand)' == 'dotnet'", StringComparison.Ordinal) == true);
            Assert.Contains(testToRun.Elements("DotNetHostRoot"), element => element.Value == "$(_DotNetHostRoot)");

            Assert.Contains(initialize.Descendants("_DotNetHostRoot"), element =>
                element.Value.Contains("GetDirectoryName('$(DotNetTool)')", StringComparison.Ordinal));

            XElement initializationDependencies = targets
                .Descendants("_InitializeTestArchitecturesDependsOn")
                .Single();
            Assert.DoesNotContain("TestArchitectures", initializationDependencies.Attribute("Condition")?.Value);

            XElement testTarget = GetTarget(targets, "Test");
            Assert.Contains("_ValidateTestArchitectures", testTarget.Attribute("DependsOnTargets")?.Value);
        }

        [Fact]
        public void AppHostArchitectureDrivesDefaultTestArchitecture() =>
            Assert.Equal("x86|x86", CaptureArchitectures(
                useAppHost: true,
                appHostRuntimeIdentifier: "win-x86"));

        [Fact]
        public void FrameworkDependentTestsDefaultToDotNetToolArchitecture() =>
            Assert.Equal("arm64|arm64", CaptureArchitectures(
                useAppHost: false,
                runtimeIdentifier: "win-x86",
                platformTarget: "x86",
                dotNetToolArchitecture: "arm64"));

        private static string CaptureArchitectures(
            bool useAppHost,
            string appHostRuntimeIdentifier = "",
            string runtimeIdentifier = "",
            string platformTarget = "",
            string dotNetToolArchitecture = "")
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), "arcade", Path.GetRandomFileName());
            string projectPath = Path.Combine(tempDirectory, "TestArchitectures.proj");
            string outputPath = Path.Combine(tempDirectory, "architectures.txt");
            Directory.CreateDirectory(Path.Combine(tempDirectory, "eng"));

            try
            {
                var projectDocument = new XDocument(
                    new XElement("Project",
                        new XElement("PropertyGroup",
                            new XElement("IsTestProject", "true"),
                            new XElement("UsingToolXUnit", "false"),
                            new XElement("TestRuntime", "Core"),
                            new XElement("TargetFramework", "net10.0"),
                            new XElement("TargetFrameworkIdentifier", ".NETCoreApp"),
                            new XElement("TargetFrameworkVersion", "v10.0"),
                            new XElement("TargetPath", Path.Combine(tempDirectory, "Sample.Tests.dll")),
                            new XElement("ArtifactsTestResultsDir", tempDirectory + Path.DirectorySeparatorChar),
                            new XElement("ArtifactsLogDir", tempDirectory + Path.DirectorySeparatorChar),
                            new XElement("RepositoryEngineeringDir", Path.Combine(tempDirectory, "eng") + Path.DirectorySeparatorChar),
                            new XElement("UseAppHost", useAppHost),
                            new XElement("AppHostRuntimeIdentifier", appHostRuntimeIdentifier),
                            new XElement("RuntimeIdentifier", runtimeIdentifier),
                            new XElement("PlatformTarget", platformTarget),
                            new XElement("DotNetToolArchitecture", dotNetToolArchitecture)),
                        new XElement("UsingTask",
                            new XAttribute("TaskName", "Microsoft.Build.Tasks.WriteLinesToFile"),
                            new XAttribute("AssemblyFile", typeof(Microsoft.Build.Tasks.WriteLinesToFile).Assembly.Location)),
                        new XElement("Import",
                            new XAttribute("Project", Path.Combine(s_toolsDirectory, "Tests.targets"))),
                        new XElement("Target",
                            new XAttribute("Name", "ComputeRunArguments")),
                        new XElement("Target",
                            new XAttribute("Name", "CaptureArchitectures"),
                            new XAttribute("DependsOnTargets", "_InnerGetTestsToRun"),
                            new XElement("WriteLinesToFile",
                                new XAttribute("File", outputPath),
                                new XAttribute("Overwrite", "true"),
                                new XAttribute("Lines", "@(TestToRun->'%(Architecture)|%(BuiltTestArchitecture)')")))));
                projectDocument.Save(projectPath);

                using var projectCollection = new ProjectCollection();
                try
                {
                    Project project = projectCollection.LoadProject(projectPath);
                    var logger = new ErrorLogger();
                    Assert.True(project.Build("CaptureArchitectures", new[] { logger }), logger.Errors);
                }
                finally
                {
                    projectCollection.UnloadAllProjects();
                }

                return File.ReadAllText(outputPath).Trim();
            }
            finally
            {
                Directory.Delete(tempDirectory, recursive: true);
            }
        }

        private static XElement GetTarget(XDocument document, string name) =>
            document
                .Descendants("Target")
                .Single(element => element.Attribute("Name")?.Value == name);

        private static XDocument LoadTargets(params string[] path) =>
            XDocument.Load(Path.Combine(new[] { s_toolsDirectory }.Concat(path).ToArray()));

        private sealed class ErrorLogger : ILogger
        {
            private readonly StringBuilder _errors = new StringBuilder();

            public string Parameters { get; set; }
            public LoggerVerbosity Verbosity { get; set; }
            public string Errors => _errors.ToString();

            public void Initialize(IEventSource eventSource) =>
                eventSource.ErrorRaised += (_, args) => _errors.AppendLine(args.Message);

            public void Shutdown()
            {
            }
        }
    }
}
