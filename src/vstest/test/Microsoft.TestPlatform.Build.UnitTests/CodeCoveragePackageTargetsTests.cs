// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.Build.UnitTests;

[TestClass]
public class CodeCoveragePackageTargetsTests
{
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
            string copyTaskAssemblyPath = typeof(Microsoft.Build.Tasks.Copy).Assembly.Location;
            File.WriteAllText(projectPath, $"""
                <Project>
                  <PropertyGroup>
                    <PublishDir>{Escape(publishDirectory + Path.DirectorySeparatorChar)}</PublishDir>
                  </PropertyGroup>
                  <UsingTask TaskName="Copy"
                             AssemblyFile="{Escape(copyTaskAssemblyPath)}" />
                  <ItemGroup>
                    <ResolvedFileToPublish Include="{Escape(applicationCollisionPath)}"
                                           RelativePath="{collisionRelativePath}" />
                  </ItemGroup>
                  <Target Name="ComputeFilesToPublish" />
                  <Import Project="{Escape(targetsPath)}" />
                </Project>
                """);

            using var projectCollection = new ProjectCollection();
            Project project = projectCollection.LoadProject(projectPath);

            Assert.IsTrue(project.Build("ComputeFilesToPublish", [new ConsoleLogger(LoggerVerbosity.Diagnostic)]));
            Assert.AreEqual("application", File.ReadAllText(publishedCollisionPath));
            Assert.IsTrue(File.Exists(publishedUniquePath));
            Assert.AreEqual("unique collector", File.ReadAllText(publishedUniquePath));
        }
        finally
        {
            Directory.Delete(testDirectory, recursive: true);
        }
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
