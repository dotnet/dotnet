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
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Telemetry;
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
        public async void GetFiles_NoSolutionMock_ReturnsZip()
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
        public async Task WriteToZipAsync_MefImportsNotAvailable_AddsMefErrorsFie()
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
    }
}
