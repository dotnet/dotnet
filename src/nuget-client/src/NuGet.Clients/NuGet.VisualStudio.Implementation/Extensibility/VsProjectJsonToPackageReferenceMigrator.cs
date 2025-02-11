// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Implementation.Resources;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    [Export(typeof(IVsProjectJsonToPackageReferenceMigrator))]
    internal class VsProjectJsonToPackageReferenceMigrator : IVsProjectJsonToPackageReferenceMigrator
    {
        private readonly Lazy<IVsSolutionManager> _solutionManager;
        private readonly Lazy<NuGetProjectFactory> _projectFactory;
        private readonly INuGetTelemetryProvider _telemetryProvider;

        [ImportingConstructor]
        public VsProjectJsonToPackageReferenceMigrator(
            Lazy<IVsSolutionManager> solutionManager,
            Lazy<NuGetProjectFactory> projectFactory,
            INuGetTelemetryProvider telemetryProvider)
        {
            Assumes.Present(solutionManager);
            Assumes.Present(projectFactory);
            Assumes.Present(telemetryProvider);

            _solutionManager = solutionManager;
            _projectFactory = projectFactory;
            _telemetryProvider = telemetryProvider;
        }

        public async Task<object> MigrateProjectJsonToPackageReferenceAsync(string projectUniqueName)
        {
            const string eventName = nameof(IVsProjectJsonToPackageReferenceMigrator) + "." + nameof(MigrateProjectJsonToPackageReferenceAsync);
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            try
            {
                if (string.IsNullOrEmpty(projectUniqueName))
                {
                    throw new ArgumentNullException(nameof(projectUniqueName));
                }

                if (!File.Exists(projectUniqueName))
                {
                    throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, VsResources.Error_FileNotExists, projectUniqueName));
                }

                return await MigrateProjectToPackageRefAsync(projectUniqueName);
            }
            catch (Exception ex)
            {
                await _telemetryProvider.PostFaultAsync(ex, nameof(VsProjectJsonToPackageReferenceMigrator));
                throw;
            }
        }

        private async Task<object> MigrateProjectToPackageRefAsync(string projectUniqueName)
        {
            var startTime = DateTimeOffset.Now;
            var stopwatch = Stopwatch.StartNew();
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var project = await _solutionManager.Value.GetVsProjectAdapterAsync(projectUniqueName);

            if (project == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, VsResources.Error_ProjectNotInCache, projectUniqueName));
            }

            var projectSafeName = project.CustomUniqueName;

            var nuGetProject = await _solutionManager.Value.GetNuGetProjectAsync(projectSafeName);

            // If the project already has PackageReference, do nothing.
            if (nuGetProject is LegacyPackageReferenceProject)
            {
                EmitTelemetryEvent(startTime, stopwatch, nuGetProject, NuGetOperationStatus.NoOp);
                return new VsProjectJsonToPackageReferenceMigrateResult(success: true, errorMessage: null);
            }

            try
            {
                await nuGetProject.SaveAsync(CancellationToken.None);

                var legacyPackageRefBasedProject = await _projectFactory.Value
                    .CreateNuGetProjectAsync<LegacyPackageReferenceProject>(
                        project, optionalContext: null);
                Assumes.Present(legacyPackageRefBasedProject);

                await ProjectJsonToPackageRefMigrator.MigrateAsync(legacyPackageRefBasedProject);
                var result = new VsProjectJsonToPackageReferenceMigrateResult(success: true, errorMessage: null);
                await nuGetProject.SaveAsync(CancellationToken.None);
                await _solutionManager.Value.UpgradeProjectToPackageReferenceAsync(nuGetProject);

                EmitTelemetryEvent(startTime, stopwatch, nuGetProject, NuGetOperationStatus.Succeeded);
                return result;

            }
            catch (Exception ex)
            {
                // reload the project in memory from the file on disk, discarding any changes that might have
                // been made as a result of an incomplete migration.
                await ReloadProjectAsync(project);
                EmitTelemetryEvent(startTime, stopwatch, nuGetProject, NuGetOperationStatus.Failed);
                return new VsProjectJsonToPackageReferenceMigrateResult(success: false, errorMessage: ex.Message);
            }
        }

        private void EmitTelemetryEvent(DateTimeOffset startTime, Stopwatch stopwatch, NuGetProject nuGetProject, NuGetOperationStatus operationStatus)
        {
            try
            {
                stopwatch.Stop();
                string projectId = nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.ProjectId);
                string fullPath = nuGetProject.GetMetadata<string>(NuGetProjectMetadataKeys.FullPath);
                _telemetryProvider.EmitEvent(new ProjectJsonMigrationEvent(projectId, fullPath, operationStatus, startTime, stopwatch.Elapsed.TotalSeconds));
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
            {
                // Ignore issues sending telemetry. We don't want to fail 
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private async Task ReloadProjectAsync(IVsProjectAdapter project)
        {
            project = await _solutionManager.Value.GetVsProjectAdapterAsync(project.FullName);
        }
    }
}

