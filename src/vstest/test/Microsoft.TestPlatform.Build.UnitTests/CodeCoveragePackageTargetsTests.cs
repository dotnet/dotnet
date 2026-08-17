// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Build.UnitTests;

[TestClass]
public class CodeCoveragePackageTargetsTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public void CopyTraceDataCollectorArtifactsDoesNotOverwriteApplicationPublishFilesWhenCollectorIsNewer()
    {
        string testDirectory = Path.Combine(Path.GetTempPath(), nameof(CodeCoveragePackageTargetsTests), Guid.NewGuid().ToString("N"));
        string packageDirectory = Path.Combine(testDirectory, "package");
        string publishDirectory = Path.Combine(testDirectory, "publish");
        string applicationDirectory = Path.Combine(testDirectory, "application");

        Directory.CreateDirectory(packageDirectory);
        Directory.CreateDirectory(publishDirectory);
        Directory.CreateDirectory(applicationDirectory);

        try
        {
            string targetsPath = Path.Combine(packageDirectory, "Microsoft.CodeCoverage.targets");
            File.Copy(Path.Combine(AppContext.BaseDirectory, "Microsoft.CodeCoverage.targets"), targetsPath);

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

            string projectPath = Path.Combine(testDirectory, "test.proj");
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

            var startInfo = new ProcessStartInfo
            {
                FileName = GetDotnetHostPath(),
                Arguments = $"msbuild \"{projectPath}\" /t:ComputeFilesToPublish /nologo /nodeReuse:false /v:minimal",
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };

            using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start MSBuild.");
            Task<string> standardOutput = Task.Run(process.StandardOutput.ReadToEnd, TestContext.CancellationToken);
            Task<string> standardError = Task.Run(process.StandardError.ReadToEnd, TestContext.CancellationToken);
            process.WaitForExit();

            Assert.AreEqual(
                0,
                process.ExitCode,
                $"{standardOutput.GetAwaiter().GetResult()}{Environment.NewLine}{standardError.GetAwaiter().GetResult()}");
            Assert.AreEqual("application", File.ReadAllText(publishedCollisionPath));
            Assert.IsTrue(File.Exists(publishedUniquePath));
            Assert.AreEqual("unique collector", File.ReadAllText(publishedUniquePath));
        }
        finally
        {
            Directory.Delete(testDirectory, recursive: true);
        }
    }

    private static string GetDotnetHostPath()
    {
        string? dotnetHostPath = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH");
        if (!string.IsNullOrEmpty(dotnetHostPath))
        {
            return dotnetHostPath;
        }

        string? dotnetRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT_X64")
            ?? Environment.GetEnvironmentVariable("DOTNET_ROOT");
        if (string.IsNullOrEmpty(dotnetRoot))
        {
            throw new InvalidOperationException("Could not locate the dotnet host.");
        }

        return Path.Combine(dotnetRoot, RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "dotnet.exe" : "dotnet");
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
