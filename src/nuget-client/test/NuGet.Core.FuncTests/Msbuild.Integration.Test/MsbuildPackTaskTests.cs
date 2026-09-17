// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NuGet.Frameworks;
using NuGet.Test.Utility;
using Xunit;
using Xunit.Abstractions;

namespace Msbuild.Integration.Test
{
    public class MsbuildPackTaskTests : IClassFixture<MsbuildIntegrationTestFixture>
    {
        private readonly MsbuildIntegrationTestFixture _msbuildFixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public MsbuildPackTaskTests(MsbuildIntegrationTestFixture fixture, ITestOutputHelper testOutputHelper)
        {
            _msbuildFixture = fixture;
            _testOutputHelper = testOutputHelper;
        }

        [PlatformFact(Platform.Windows)]
        public void MsbuildPack_ClassicPackageReferenceProjectWithNonStandardPackageId_EmitsNU5052()
        {
            using var pathContext = new SimpleTestPathContext();

            const string projectName = "ClassLibrary1";
            const string targetFramework = "net472";
            var project = SimpleTestProjectContext.CreateLegacyPackageReference(
                projectName,
                pathContext.SolutionRoot,
                NuGetFramework.Parse(targetFramework));
            project.Properties.Add("PackageId", "Contöso.Utilities");
            project.Properties.Add("Authors", "test");
            project.Properties.Add("IncludeBuildOutput", "false");
            project.Properties.Add("TargetFrameworkVersion", "v4.7.2");

            var solution = new SimpleTestSolutionContext(pathContext.SolutionRoot, project);
            solution.Create();

            File.WriteAllText(
                Path.Combine(Path.GetDirectoryName(project.ProjectPath), "content.txt"),
                "content");
            ProjectFileUtils.AddItem(
                project.ProjectPath,
                "None",
                "content.txt",
                string.Empty,
                new Dictionary<string, string> { { "Pack", "true" } });

            File.WriteAllText(
                Path.Combine(pathContext.SolutionRoot, "Directory.Build.targets"),
$@"<Project>
  <Import Project=""$(NuGetBuildTasksPackTargets)"" />
  <Target Name=""SetTargetFrameworkForPack""
          AfterTargets=""ResolveReferences""
          BeforeTargets=""_GetFrameworkAssemblyReferences"">
    <PropertyGroup>
      <TargetFramework>{targetFramework}</TargetFramework>
    </PropertyGroup>
  </Target>
</Project>");

            var restoreResult = _msbuildFixture.RunMsBuild(
                pathContext.WorkingDirectory,
                $@"/t:restore ""{project.ProjectPath}""",
                ignoreExitCode: true,
                testOutputHelper: _testOutputHelper);
            restoreResult.ExitCode.Should().Be(0, restoreResult.AllOutput);

            var packResult = _msbuildFixture.RunMsBuild(
                pathContext.WorkingDirectory,
                $@"/t:pack ""{project.ProjectPath}"" /p:NoBuild=true /p:NoPackageAnalysis=true /p:PackageOutputPath=""{pathContext.WorkingDirectory}""",
                ignoreExitCode: true,
                testOutputHelper: _testOutputHelper);

            packResult.ExitCode.Should().Be(0, packResult.AllOutput);
            packResult.AllOutput.Should().Contain("NU5052");
        }
    }
}
