// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.TestPlatform.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AcceptanceTests;

[TestClass]
public class CodeCoveragePackageTargetsTests : AcceptanceTestBase
{
    [TestMethod]
    public void CopyTraceDataCollectorArtifactsDoesNotOverwriteApplicationPublishFilesWhenCollectorIsNewer()
    {
        string packageDirectory = Path.Combine(TempDirectory.Path, "package");
        string publishDirectory = Path.Combine(TempDirectory.Path, "publish");
        string applicationDirectory = Path.Combine(TempDirectory.Path, "application");

        Directory.CreateDirectory(packageDirectory);
        Directory.CreateDirectory(publishDirectory);
        Directory.CreateDirectory(applicationDirectory);

        string targetsPath = Path.Combine(packageDirectory, "Microsoft.CodeCoverage.targets");
        File.Copy(
            Path.Combine(
                IntegrationTestEnvironment.RepoRootDirectory,
                "src",
                "package",
                "Microsoft.CodeCoverage",
                "Microsoft.CodeCoverage.targets"),
            targetsPath);

        string collisionRelativePath = "System.Memory.dll";
        string collectorCollisionPath = Path.Combine(packageDirectory, collisionRelativePath);
        string applicationCollisionPath = Path.Combine(applicationDirectory, collisionRelativePath);
        string publishedCollisionPath = Path.Combine(publishDirectory, collisionRelativePath);
        string uniqueRelativePath = Path.Combine("collectors", "Unique.CodeCoverage.dll");
        string uniqueCollectorPath = Path.Combine(packageDirectory, uniqueRelativePath);
        string publishedUniquePath = Path.Combine(publishDirectory, uniqueRelativePath);

        File.WriteAllText(collectorCollisionPath, "collector");
        File.WriteAllText(applicationCollisionPath, "application");
        File.WriteAllText(publishedCollisionPath, "application");
        Directory.CreateDirectory(Path.GetDirectoryName(uniqueCollectorPath)!);
        File.WriteAllText(uniqueCollectorPath, "unique collector");

        DateTime now = DateTime.UtcNow;
        File.SetLastWriteTimeUtc(applicationCollisionPath, now.AddHours(-1));
        File.SetLastWriteTimeUtc(publishedCollisionPath, now.AddHours(-1));
        File.SetLastWriteTimeUtc(collectorCollisionPath, now);

        string projectPath = Path.Combine(TempDirectory.Path, "test.proj");
        File.WriteAllText(projectPath, $"""
            <Project>
              <PropertyGroup>
                <PublishDir>{Escape(publishDirectory + Path.DirectorySeparatorChar)}</PublishDir>
              </PropertyGroup>
              <ItemGroup>
                <ResolvedFileToPublish Include="{Escape(applicationCollisionPath)}"
                                       RelativePath="{collisionRelativePath}" />
              </ItemGroup>
              <Target Name="ComputeFilesToPublish" />
              <Import Project="{Escape(targetsPath)}" />
            </Project>
            """);

        string dotnetPath = Path.GetFullPath(
            Path.Combine(
                IntegrationTestEnvironment.RepoRootDirectory,
                "artifacts",
                "tmp",
                ".dotnet",
                OSUtils.IsWindows ? "dotnet.exe" : "dotnet"));

        ExecuteApplication(
            dotnetPath,
            $"msbuild \"{projectPath}\" /t:ComputeFilesToPublish /nologo /nodeReuse:false /v:minimal",
            out string buildOutput,
            out string buildError,
            out int buildExitCode,
            workingDirectory: TempDirectory.Path);

        Assert.AreEqual(
            0,
            buildExitCode,
            $"MSBuild failed (exit {buildExitCode}).\nSTDOUT:\n{buildOutput}\nSTDERR:\n{buildError}");
        Assert.AreEqual("application", File.ReadAllText(publishedCollisionPath));
        Assert.IsTrue(File.Exists(publishedUniquePath));
        Assert.AreEqual("unique collector", File.ReadAllText(publishedUniquePath));
    }

    private static string Escape(string value)
    {
        return value
            .Replace("&", "&amp;")
            .Replace("\"", "&quot;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");
    }
}
