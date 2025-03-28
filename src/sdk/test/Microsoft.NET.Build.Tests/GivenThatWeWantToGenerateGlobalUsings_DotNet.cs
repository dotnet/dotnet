// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

namespace Microsoft.NET.Build.Tests
{
    public class GivenThatWeWantToGenerateGlobalUsings_DotNet : SdkTest
    {
        public GivenThatWeWantToGenerateGlobalUsings_DotNet(ITestOutputHelper log) : base(log) { }

        [RequiresMSBuildVersionFact("17.0.0.32901")]
        public void It_can_generate_global_usings_and_builds_successfully()
        {
            var tfm = ToolsetInfo.CurrentTargetFramework;
            var testProject = CreateTestProject(tfm);
            testProject.AdditionalProperties["ImplicitUsings"] = "enable";
            var testAsset = _testAssetsManager.CreateTestProject(testProject);
            var globalUsingsFileName = $"{testAsset.TestProject.Name}.GlobalUsings.g.cs";

            var buildCommand = new BuildCommand(testAsset);
            buildCommand
                .Execute()
                .Should()
                .Pass();

            var outputDirectory = buildCommand.GetIntermediateDirectory(tfm);

            outputDirectory.Should().HaveFile(globalUsingsFileName);

            File.ReadAllText(Path.Combine(outputDirectory.FullName, globalUsingsFileName)).Should().Be(
@"// <auto-generated/>
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
");
        }

        [Fact]
        public void Implicit_Usings_Are_Not_Enabled_By_Default()
        {
            var tfm = ToolsetInfo.CurrentTargetFramework;
            var testProject = CreateTestProject(tfm);
            var testAsset = _testAssetsManager.CreateTestProject(testProject);
            var globalUsingsFileName = $"{testAsset.TestProject.Name}.GlobalUsings.g.cs";

            var buildCommand = new BuildCommand(testAsset);
            buildCommand
                .Execute()
                .Should()
                .Fail();

            var outputDirectory = buildCommand.GetIntermediateDirectory(tfm);

            outputDirectory.Should().NotHaveFile(globalUsingsFileName);
        }

        [RequiresMSBuildVersionFact("17.0.0.32901")]
        public void It_can_remove_specific_usings_in_project_file()
        {
            var tfm = ToolsetInfo.CurrentTargetFramework;
            var testProject = CreateTestProject(tfm);
            testProject.AdditionalProperties["ImplicitUsings"] = "enable";
            testProject.AddItem("Using", new Dictionary<string, string> { ["Remove"] = "System.IO" });
            var testAsset = _testAssetsManager.CreateTestProject(testProject);
            var globalUsingsFileName = $"{testAsset.TestProject.Name}.GlobalUsings.g.cs";


            var buildCommand = new BuildCommand(testAsset);
            buildCommand
                .Execute()
                .Should()
                .Pass();

            var outputDirectory = buildCommand.GetIntermediateDirectory(tfm);

            outputDirectory.Should().HaveFile(globalUsingsFileName);

            File.ReadAllText(Path.Combine(outputDirectory.FullName, globalUsingsFileName)).Should().Be(
@"// <auto-generated/>
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
");
        }

        [Fact]
        public void It_can_generate_custom_usings()
        {
            var tfm = ToolsetInfo.CurrentTargetFramework;
            var testProject = CreateTestProject(tfm);
            testProject.ProjectChanges.Add(projectXml =>
            {
                var ns = projectXml.Root.Name.Namespace;
                var itemGroup = new XElement(ns + "ItemGroup");
                projectXml.Root.Add(XElement.Parse(
@"<ItemGroup>
    <Using Include=""CustomNamespace"" />
    <Using Include=""TestStaticNamespace"" Static=""True"" />
    <Using Include=""System.Biology"" Alias=""AppliedChemistry"" />
    <Using Include=""(System.String X, int Y)"" Alias=""MyTuple"" />
    <Using Include=""dynamic"" Alias=""Any"" />
    <Using Include=""int"" Alias=""Number"" />
</ItemGroup>"));
            });

            var testAsset = _testAssetsManager.CreateTestProject(testProject);
            var globalUsingsFileName = $"{testAsset.TestProject.Name}.GlobalUsings.g.cs";

            var buildCommand = new BuildCommand(testAsset);
            buildCommand
                .Execute()
                .Should()
                .Fail();

            var outputDirectory = buildCommand.GetIntermediateDirectory(tfm);

            outputDirectory.Should().HaveFile(globalUsingsFileName);

            File.ReadAllText(Path.Combine(outputDirectory.FullName, globalUsingsFileName)).Should().Be(
@"// <auto-generated/>
global using CustomNamespace;
global using Any = dynamic;
global using AppliedChemistry = System.Biology;
global using MyTuple = (System.String X, int Y);
global using Number = int;
global using static TestStaticNamespace;
");
        }

        [Fact]
        public void It_considers_switches_when_deduping()
        {
            var tfm = ToolsetInfo.CurrentTargetFramework;
            var testProject = CreateTestProject(tfm);
            testProject.ProjectChanges.Add(projectXml =>
            {
                var ns = projectXml.Root.Name.Namespace;
                var itemGroup = new XElement(ns + "ItemGroup");
                projectXml.Root.Add(XElement.Parse(
@"<ItemGroup>
    <Using Include=""CustomNamespace"" />
    <Using Include=""System.IO.File"" Alias=""FileIO"" />
    <Using Include=""TestStaticNamespace"" Static=""True"" />
    <Using Include=""TestStaticNamespace"" />
    <Using Include=""TestStaticNamespace"" Static=""True"" />
    <Using Include=""CustomNamespace"" />
    <Using Include=""System.IO.File"" Alias=""Disk"" />
    <Using Include=""System.IO.File"" Alias=""FileIO"" />
</ItemGroup>"));
            });

            var testAsset = _testAssetsManager.CreateTestProject(testProject);
            var globalUsingsFileName = $"{testAsset.TestProject.Name}.GlobalUsings.g.cs";

            var buildCommand = new BuildCommand(testAsset);
            buildCommand
                .Execute()
                .Should()
                .Fail();

            var outputDirectory = buildCommand.GetIntermediateDirectory(tfm);

            outputDirectory.Should().HaveFile(globalUsingsFileName);

            File.ReadAllText(Path.Combine(outputDirectory.FullName, globalUsingsFileName)).Should().Be(
@"// <auto-generated/>
global using CustomNamespace;
global using TestStaticNamespace;
global using Disk = System.IO.File;
global using FileIO = System.IO.File;
global using static TestStaticNamespace;
");
        }

        [RequiresMSBuildVersionFact("17.0.0.32901")]
        public void It_can_persist_generatedfile_between_cleans()
        {
            // Regression test for https://devdiv.visualstudio.com/DevDiv/_workitems/edit/1405579
            var tfm = ToolsetInfo.CurrentTargetFramework;
            var testProject = CreateTestProject(tfm);
            testProject.AdditionalProperties["ImplicitUsings"] = "enable";
            var testAsset = _testAssetsManager.CreateTestProject(testProject);
            var globalUsingsFileName = $"{testAsset.TestProject.Name}.GlobalUsings.g.cs";

            var buildCommand = new BuildCommand(testAsset);
            buildCommand
                .Execute()
                .Should()
                .Pass();

            var outputDirectory = buildCommand.GetIntermediateDirectory(tfm);

            outputDirectory.Should().HaveFile(globalUsingsFileName);

            var cleanCommand = new CleanCommand(testAsset);
            cleanCommand
                .Execute()
                .Should()
                .Pass();

            // Verify the GlobalUsings.g.cs does not get removed on clean.
            outputDirectory.Should().HaveFile(globalUsingsFileName);
            File.ReadAllText(Path.Combine(outputDirectory.FullName, globalUsingsFileName)).Should().Be(
@"// <auto-generated/>
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
");
        }

        [RequiresMSBuildVersionFact("17.0.0.32901")]
        public void It_not_generate_global_usings_for_system_net_http_when_multitarget()
        {
            var tfm = "net472;netstandard2.0;net6.0";
            var testProject = CreateTestProject(tfm);
            testProject.AdditionalProperties["ImplicitUsings"] = "enable";
            testProject.AdditionalProperties["LangVersion"] = "10.0";
            var testAsset = _testAssetsManager.CreateTestProject(testProject);
            var globalUsingsFileName = $"{testAsset.TestProject.Name}.GlobalUsings.g.cs";

            var buildCommand = new BuildCommand(testAsset);
            buildCommand
                .Execute()
                .Should()
                .Pass();

            var outputDirectory = buildCommand.GetIntermediateDirectory("net472");

            outputDirectory.Should().HaveFile(globalUsingsFileName);

            File.ReadAllText(Path.Combine(outputDirectory.FullName, globalUsingsFileName)).Should().Be(
@"// <auto-generated/>
global using System;
global using System.Collections.Generic;
global using System.IO;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
");
        }

        private TestProject CreateTestProject(string tfm)
        {
            var testProject = new TestProject
            {
                IsExe = true,
                TargetFrameworks = tfm,
                ProjectSdk = "Microsoft.NET.Sdk"
            };
            testProject.SourceFiles["Program.cs"] = @"
namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}
";
            return testProject;
        }
    }
}
