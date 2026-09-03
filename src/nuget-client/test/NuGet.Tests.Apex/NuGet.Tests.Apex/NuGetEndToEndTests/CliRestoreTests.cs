// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Internal.NuGet.Testing.SignedPackages.ChildProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using Test.Utility;

namespace NuGet.Tests.Apex
{
    /// <summary>
    /// These tests are intended to validate that restore works with MSBuild when NuGet is installed in VS normally.
    /// MSBuild.Integration.Tests should be used for more exhaustive testing of restore with MSBuild.
    /// </summary>
    /// <remarks>
    /// These tests locate MSBuild.exe using vswhere.exe, so when running these tests locally, they will not take into
    /// account any changes you have made to any product code. It will only test the NuGet that is installed in the VS
    /// installation directory. The VS "experimental instance" is a devenv.exe only concept, not MSBuild.
    /// </remarks>
    [TestClass]
    public class CliRestoreTests
    {
        private const int DefaultTimeout = 5 * 60 * 1000; // 5 minutes
        private const string LoadMSBuildAssembliesMarkerFileName = "LoadMSBuildAssemblies.ran";

        public TestContext TestContext { get; set; } = null!;

        /// <summary>
        /// Validates restore still runs when assemblies from MSBuild's bin directory are loaded first.
        /// </summary>
        /// <remarks>
        /// Uses a V3 HTTP feed to exercise Newtonsoft.Json and contentFiles wildcard matching to exercise
        /// Microsoft.Extensions.FileSystemGlobbing.
        /// </remarks>
        [TestMethod]
        [Timeout(DefaultTimeout)]
        public async Task RestoreRunsWhenMSBuildDependenciesAreLoaded()
        {
            // Arrange
            string? msbuildPath = VisualStudioMSBuildLocator.GetMSBuildPath();
            if (msbuildPath is null)
            {
                Assert.Inconclusive(
                    "Could not locate MSBuild from the Visual Studio installation under test using vswhere. " +
                    "Ensure a Visual Studio instance with MSBuild is installed.");
                return;
            }

            using var pathContext = new SimpleTestPathContext();

            const string PackageName = "TestPackage";
            const string PackageVersion = "1.0.0";
            const string ContentFilePackagePath = "contentFiles/any/any/sample.txt";

            await CreatePackageWithContentFileAsync(pathContext.PackageSource, PackageName, PackageVersion, ContentFilePackagePath);

            using var mockServer = CreateMockServer(pathContext);

            string projectPath = Path.Combine(pathContext.SolutionRoot, "test.csproj");
            File.WriteAllText(projectPath, GetProjectXml(PackageName, PackageVersion));

            // Act
            CommandRunnerResult result = RunMSBuildRestore(msbuildPath, pathContext.SolutionRoot, projectPath);
            TestContext.WriteLine($"msbuild -t:restore exit code: {result.ExitCode}");
            TestContext.WriteLine(result.AllOutput);

            // Assert
            Assert.AreEqual(
                0,
                result.ExitCode,
                result.AllOutput);

            AssertLoadMSBuildAssembliesRan(pathContext.SolutionRoot, result.AllOutput);
            AssertContentFileSelected(pathContext.SolutionRoot, PackageName, PackageVersion, ContentFilePackagePath, result.AllOutput);
        }

        private static async Task CreatePackageWithContentFileAsync(
            string packageSource,
            string packageName,
            string packageVersion,
            string contentFilePackagePath)
        {
            var package = new SimpleTestPackageContext(packageName, packageVersion);
            package.Files.Clear();
            package.AddFile("lib/net472/_._");
            package.AddFile(contentFilePackagePath, "// content file served by the test package");
            package.Nuspec = XDocument.Parse($@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns=""http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"">
  <metadata>
    <id>{packageName}</id>
    <version>{packageVersion}</version>
    <title>{packageName}</title>
    <authors>NuGet</authors>
    <description>{packageName}</description>
    <contentFiles>
      <files include=""any/any/*.txt"" buildAction=""Content"" copyToOutput=""true"" flatten=""false"" />
    </contentFiles>
  </metadata>
</package>");

            await SimpleTestPackageUtility.CreatePackagesAsync(packageSource, package);
        }

        private static FileSystemBackedV3MockServer CreateMockServer(SimpleTestPathContext pathContext)
        {
            var mockServer = new FileSystemBackedV3MockServer(pathContext.PackageSource);
            mockServer.Start();

            pathContext.Settings.RemoveSource(SimpleTestSettingsContext.DefaultPackageSourceName);
            pathContext.Settings.AddSource("mockSource", mockServer.ServiceIndexUri, allowInsecureConnectionsValue: "true");

            return mockServer;
        }

        private static CommandRunnerResult RunMSBuildRestore(string msbuildPath, string solutionRoot, string projectPath)
        {
            return CommandRunner.Run(
                filename: msbuildPath,
                workingDirectory: solutionRoot,
                arguments: $"-t:restore \"{projectPath}\"",
                timeOutInMilliseconds: DefaultTimeout);
        }

        private static void AssertLoadMSBuildAssembliesRan(string solutionRoot, string output)
        {
            string markerFilePath = Path.Combine(solutionRoot, "obj", LoadMSBuildAssembliesMarkerFileName);
            Assert.IsTrue(
                File.Exists(markerFilePath),
                $"Expected LoadMSBuildAssemblies to create '{markerFilePath}'.{Environment.NewLine}{output}");
        }

        private static void AssertContentFileSelected(
            string solutionRoot,
            string packageName,
            string packageVersion,
            string contentFilePackagePath,
            string output)
        {
            string assetsFilePath = Path.Combine(solutionRoot, "obj", "project.assets.json");
            Assert.IsTrue(
                File.Exists(assetsFilePath),
                $"Expected restore to generate '{assetsFilePath}'.{Environment.NewLine}{output}");

            TestLogger logger = new();
            LockFile? assetsFile = new LockFileFormat().Read(assetsFilePath, logger);
            Assert.AreEqual(0, logger.Messages.Count, string.Join("\n", logger.Messages));

            if (assetsFile is null)
            {
                Assert.Fail($"Expected restore to generate a valid assets file at '{assetsFilePath}'.{Environment.NewLine}{output}");
                return;
            }

            LockFileTarget target = assetsFile.Targets.Single();
            LockFileTargetLibrary library = target.Libraries.Single(l => l.Name == packageName);
            LockFileContentFile? selectedContentFile = library.ContentFiles
                .SingleOrDefault(contentFile => StringComparer.OrdinalIgnoreCase.Equals(contentFile.Path, contentFilePackagePath));

            if (selectedContentFile is null)
            {
                Assert.Fail(
                    $"Expected the assets file to select content file '{contentFilePackagePath}' for '{packageName}', " +
                    $"but found: {string.Join(", ", library.ContentFiles.Select(c => c.Path))}.{Environment.NewLine}{output}");
                return;
            }

            Assert.AreEqual("Content", selectedContentFile.BuildAction.Value, ignoreCase: true);
            Assert.AreEqual(true, selectedContentFile.CopyToOutput);
        }

        private static string GetProjectXml(string packageName, string packageVersion)
        {
            return $$"""
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <TargetFramework>net472</TargetFramework>
                    <LoadMSBuildAssembliesMarkerFile>$(MSBuildProjectDirectory)\obj\{{LoadMSBuildAssembliesMarkerFileName}}</LoadMSBuildAssembliesMarkerFile>
                  </PropertyGroup>
                  <ItemGroup>
                    <PackageReference Include="{{packageName}}" Version="{{packageVersion}}" />
                  </ItemGroup>

                  <UsingTask TaskName="LoadMSBuildAssemblies" TaskFactory="RoslynCodeTaskFactory" AssemblyFile="$(MSBuildToolsPath)\Microsoft.Build.Tasks.Core.dll">
                    <ParameterGroup>
                      <Directory ParameterType="System.String" Required="true" />
                      <MarkerFile ParameterType="System.String" Required="true" />
                    </ParameterGroup>
                    <Task>
                      <Code Type="Fragment" Language="cs"><![CDATA[
                        foreach (string file in System.IO.Directory.GetFiles(Directory, "*.dll"))
                        {
                            try
                            {
                                System.Reflection.Assembly.LoadFrom(file);
                            }
                            catch
                            {
                                // Skip assemblies that fail to load.
                            }
                        }

                        string markerDirectory = System.IO.Path.GetDirectoryName(MarkerFile);
                        if (!string.IsNullOrEmpty(markerDirectory))
                        {
                            System.IO.Directory.CreateDirectory(markerDirectory);
                        }

                        System.IO.File.WriteAllText(MarkerFile, string.Empty);
                      ]]></Code>
                    </Task>
                  </UsingTask>

                  <Target Name="LoadMSBuildAssembliesBeforeRestore" BeforeTargets="CollectPackageReferences">
                    <LoadMSBuildAssemblies Directory="$(MSBuildBinPath)" MarkerFile="$(LoadMSBuildAssembliesMarkerFile)" />
                  </Target>
                </Project>
                """;
        }
    }
}
