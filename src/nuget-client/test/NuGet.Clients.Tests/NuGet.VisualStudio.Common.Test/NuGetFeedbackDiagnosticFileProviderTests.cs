// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.PackageManagement.Test;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectManagement.Projects;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio.Telemetry;
using Test.Utility;
using Xunit;

namespace NuGet.VisualStudio.Common.Test
{
    public class NuGetFeedbackDiagnosticFileProviderTests
    {
        private NuGetFeedbackDiagnosticFileProvider _target;
        private Mock<ISolutionManager> _solutionManager;
        private Mock<INuGetTelemetryProvider> _telemetryProvider;
        private Mock<ISettings> _settings;

        public NuGetFeedbackDiagnosticFileProviderTests()
        {
            _solutionManager = new Mock<ISolutionManager>();
            _solutionManager.Setup(sm => sm.GetNuGetProjectsAsync())
                .Returns(Task.FromResult<IEnumerable<NuGetProject>>(Array.Empty<NuGetProject>())); // empty or no solution

            _telemetryProvider = new Mock<INuGetTelemetryProvider>();

            _settings = new Mock<ISettings>();

            _target = new NuGetFeedbackDiagnosticFileProvider();
            _target.SolutionManager = _solutionManager.Object;
            _target.TelemetryProvider = _telemetryProvider.Object;
            _target.Settings = _settings.Object;
        }

        [Fact]
        public async Task GetFiles_NoSolutionMock_ReturnsZip()
        {
            // Arrange - also see constructor
            List<Task> backgroundTasks = new();
            _target.BackgroundTaskStarted += (_, task) => backgroundTasks.Add(task);

            // Act
            IReadOnlyCollection<string> files = _target.GetFiles();
            await Task.WhenAll(backgroundTasks);

            try
            {
                // Assert
                // As per feedback team's docs, multiple files should be saved in a zip, so our class should only
                // return that 1 filename.
                string fullPath = Assert.Single(files);
                Assert.EndsWith(".zip", fullPath);
                Assert.True(Path.IsPathRooted(fullPath));
                Assert.True(File.Exists(fullPath));

                // ensure file is readable (file handle isn't still open with FileShare.None)
                File.OpenRead(fullPath).Dispose();
            }
            finally
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
        }

        [Fact]
        public async Task WriteToZipAsync_NoSolutionMock_ContainsOnlyExpectedFiles()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act
            await _target.WriteToZipAsync(stream);

            // Assert
            using (var zip = new ZipArchive(stream))
            {
                IEnumerable<string> zipFiles = zip.Entries.Select(e => e.FullName);
                var expectedFiles = new[] { "dgspec.json" };

                Assert.Equal(zipFiles.OrderBy(f => f), expectedFiles);
            }
        }

        [Fact]
        public async Task WriteToZipAsync_NoSolutionMock_EmitsTelemetryWithDuration()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act
            await _target.WriteToZipAsync(stream);

            // Assert
            _telemetryProvider.Verify(tp => tp.EmitEvent(It.IsAny<TelemetryEvent>()), Times.Once());

            var telemetry = (TelemetryEvent)_telemetryProvider.Invocations.Single(i => i.Method.Name == "EmitEvent").Arguments[0];
            Assert.Equal("feedback", telemetry.Name);
            Assert.NotNull(telemetry["duration_ms"]);
        }


        // Many times, over the years, VS's MEF cache has become broken and NuGet fails because of MEF composition exceptions.
        // When customers want to report problems, we need to make sure we don't cause problems for the VS feedback tool
        // by causing MEF composition errors when trying to create our provider (although there's a good chance that
        // our provider won't even be discovered when the MEF cache is missing NuGet types).
        [Fact]
        public async Task GetFiles_MefImportsNotAvailable_ReturnsFiles()
        {
            // Arrange
            var container = new CompositionContainer();
            List<Task> backgroundTasks = new();
            _target.BackgroundTaskStarted += (_, task) => backgroundTasks.Add(task);

            _target.SolutionManager = null;
            _target.TelemetryProvider = null;
            _target.Settings = null;

            // Act
            container.ComposeParts(_target);
            IReadOnlyCollection<string> files = _target.GetFiles();
            await Task.WhenAll(backgroundTasks);

            try
            {
                // Assert
                // As per feedback team's docs, multiple files should be saved in a zip, so our class should only
                // return that 1 filename.
                string fullPath = Assert.Single(files);
                Assert.EndsWith(".zip", fullPath);
                Assert.True(Path.IsPathRooted(fullPath));
                Assert.True(File.Exists(fullPath));

                // ensure file is readable (file handle isn't still open with FileShare.None)
                File.OpenRead(fullPath).Dispose();
            }
            finally
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
        }

        [Fact]
        public async Task WriteToZipAsync_MefImportsNotAvailable_AddsMefErrorsFile()
        {
            // Arrange
            _target.SolutionManager = null;
            _target.Settings = null;
            using var stream = new MemoryStream();

            // Act
            await _target.WriteToZipAsync(stream);

            // Assert
            using (var zip = new ZipArchive(stream))
            {
                IEnumerable<string> zipFiles = zip.Entries.Select(e => e.FullName);
                Assert.Contains("mef-errors.txt", zipFiles);
            }
        }

        [Fact]
        public async Task WriteToZipAsync_WithMSSource_SourceRemainsStill()
        {
            // Arrange
            var projectName = "testproj";
            var dgSpecFileName = "dgspec.json";

            using (var solutionManager = new TestSolutionManager())
            {
                var projectFolder = new DirectoryInfo(Path.Combine(solutionManager.SolutionDirectory, projectName));
                projectFolder.Create();
                var projectConfig = new FileInfo(Path.Combine(projectFolder.FullName, "project.json"));
                var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));
                string extractPath = Path.Combine(solutionManager.SolutionDirectory);

                BuildIntegrationTestUtility.CreateConfigJson(projectConfig.FullName);
                var json = JObject.Parse(File.ReadAllText(projectConfig.FullName));

                JsonConfigUtility.AddDependency(json, new PackageDependency("nuget.versioning", VersionRange.Parse("1.0.7")));

                using (var writer = new StreamWriter(projectConfig.FullName))
                {
                    writer.Write(json.ToString());
                }

                var sources = new List<SourceRepository> { };
                var testLogger = new TestLogger();
                var settings = Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config");
                var project = new ProjectJsonNuGetProject(projectConfig.FullName, msbuildProjectPath.FullName);

                solutionManager.NuGetProjects.Add(project);

                var restoreContext = new DependencyGraphCacheContext(testLogger, settings);
                var providersCache = new RestoreCommandProvidersCache();
                DependencyGraphSpec dgSpec = await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext);

                using var stream = new MemoryStream();
                _target.Settings = settings;
                _target.SolutionManager = solutionManager;

                // Act
                await _target.WriteToZipAsync(stream);

                // Assert
                using (var zip = new ZipArchive(stream))
                {
                    IEnumerable<string> zipFiles = zip.Entries.Select(e => e.FullName);
                    var expectedFiles = new[] { dgSpecFileName };

                    Assert.Equal(zipFiles.OrderBy(f => f), expectedFiles);

                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));
                        if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                            entry.ExtractToFile(destinationPath);
                    }
                }

                DependencyGraphSpec vsFeedbackDgSpec = DependencyGraphSpec.Load(Path.Combine(extractPath, dgSpecFileName));
                Assert.Equal(dgSpec.Projects.Count, vsFeedbackDgSpec.Projects.Count);
                Assert.Equal(dgSpec.Projects[0].RestoreMetadata.Sources.Count, vsFeedbackDgSpec.Projects[0].RestoreMetadata.Sources.Count);
                Assert.Equal(dgSpec.Projects[0].RestoreMetadata.Sources[0].Source, vsFeedbackDgSpec.Projects[0].RestoreMetadata.Sources[0].Source);
                // dgSpec.Save replaces source name with source.
                Assert.Equal(dgSpec.Projects[0].RestoreMetadata.Sources[0].Source, vsFeedbackDgSpec.Projects[0].RestoreMetadata.Sources[0].Name);
            }
        }

        [Fact]
        public async Task WriteToZipAsync_WithNonMSSource_SourceHashed()
        {
            // Arrange
            var projectName = "testproj";
            var dgSpecFileName = "dgspec.json";

            using (var solutionManager = new TestSolutionManager())
            {
                var privateRepositoryPath = Path.Combine(solutionManager.TestDirectory, "SharedRepository");
                Directory.CreateDirectory(privateRepositoryPath);

                var configPath = Path.Combine(solutionManager.TestDirectory, "nuget.config");
                File.WriteAllText(configPath, $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <packageSources>
    <!--To inherit the global NuGet package sources remove the <clear/> line below -->
    <clear />
    <add key=""PrivateRepository"" value=""{privateRepositoryPath}"" />
    </packageSources>
</configuration>");

                var projectFolder = new DirectoryInfo(Path.Combine(solutionManager.SolutionDirectory, projectName));
                projectFolder.Create();
                var projectConfig = new FileInfo(Path.Combine(projectFolder.FullName, "project.json"));
                var msbuildProjectPath = new FileInfo(Path.Combine(projectFolder.FullName, $"{projectName}.csproj"));
                string extractPath = Path.Combine(solutionManager.SolutionDirectory);

                BuildIntegrationTestUtility.CreateConfigJson(projectConfig.FullName);

                var sources = new List<SourceRepository> { };
                var testLogger = new TestLogger();
                var settings = Settings.LoadSpecificSettings(solutionManager.SolutionDirectory, "NuGet.Config");
                var project = new ProjectJsonNuGetProject(projectConfig.FullName, msbuildProjectPath.FullName);

                solutionManager.NuGetProjects.Add(project);

                var restoreContext = new DependencyGraphCacheContext(testLogger, settings);
                var providersCache = new RestoreCommandProvidersCache();
                DependencyGraphSpec dgSpec = await DependencyGraphRestoreUtility.GetSolutionRestoreSpec(solutionManager, restoreContext);

                using var stream = new MemoryStream();
                _target.Settings = settings;
                _target.SolutionManager = solutionManager;

                // Act
                await _target.WriteToZipAsync(stream);

                // Assert
                using (var zip = new ZipArchive(stream))
                {
                    IEnumerable<string> zipFiles = zip.Entries.Select(e => e.FullName);
                    var expectedFiles = new[] { dgSpecFileName };

                    Assert.Equal(zipFiles.OrderBy(f => f), expectedFiles);

                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));
                        if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal))
                            entry.ExtractToFile(destinationPath);
                    }
                }

                DependencyGraphSpec vsFeedbackDgSpec = DependencyGraphSpec.Load(Path.Combine(extractPath, dgSpecFileName));
                Assert.Equal(dgSpec.Projects.Count, vsFeedbackDgSpec.Projects.Count);
                Assert.Equal(dgSpec.Projects[0].RestoreMetadata.Sources.Count, vsFeedbackDgSpec.Projects[0].RestoreMetadata.Sources.Count);
                var hmac = NuGetFeedbackDiagnosticFileProvider.CreateHMACSHA256();
                string hashedSource = NuGetFeedbackDiagnosticFileProvider.ComputeHash(hmac, privateRepositoryPath);
                Assert.Equal(hashedSource, vsFeedbackDgSpec.Projects[0].RestoreMetadata.Sources[0].Source);
                // dgSpec.Save replaces source name with source.
                Assert.Equal(hashedSource, vsFeedbackDgSpec.Projects[0].RestoreMetadata.Sources[0].Name);
            }
        }

        [Fact]
        public void ComputeHash_WithNuGetOrgUrl_ReturnsKnownHash()
        {
            // Arrange
            var hmac = NuGetFeedbackDiagnosticFileProvider.CreateHMACSHA256();
            var source = "https://api.nuget.org/v3/index.json";

            // Act
            var hash = NuGetFeedbackDiagnosticFileProvider.ComputeHash(hmac, source);

            // Assert
            hash.Should().Be("6BA80C99934CF77597030870B3693F1DCD1D38F13DB8D4988B44BF4DF9E2B976");
        }
    }
}
