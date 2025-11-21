// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

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
using NuGet.VisualStudio.Services;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    [Export(typeof(IVsProjectJsonToPackageReferenceMigrator))]
    [Export(typeof(IProjectJsonToPackageReferenceMigratorExt))]
    internal class VsProjectJsonToPackageReferenceMigrator : IProjectJsonToPackageReferenceMigratorExt
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

        public async Task<object> MigrateProjectJsonToPackageReferenceAsync(string projectFullPath)
        {
            const string eventName = nameof(IVsProjectJsonToPackageReferenceMigrator) + "." + nameof(MigrateProjectJsonToPackageReferenceAsync);
            using var _ = NuGetETW.ExtensibilityEventSource.StartStopEvent(eventName);

            try
            {
                if (string.IsNullOrEmpty(projectFullPath))
                {
                    throw new ArgumentNullException(nameof(projectFullPath));
                }

                if (!File.Exists(projectFullPath))
                {
                    throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, VsResources.Error_FileNotExists, projectFullPath));
                }

                (NuGetProject nuGetProject, IVsProjectAdapter projectAdapter) = await GetNuGetProjectAndVSAdapter(projectFullPath);

                return await MigrateProjectToPackageRefAsync(nuGetProject, projectAdapter);
            }
            catch (Exception ex)
            {
                await _telemetryProvider.PostFaultAsync(ex, nameof(VsProjectJsonToPackageReferenceMigrator));
                throw;
            }
        }

        public async Task<object> MigrateProjectJsonToPackageReferenceAsync(NuGetProject nuGetProject, IVsProjectAdapter projectAdapter)
        {
            try
            {
                return await MigrateProjectToPackageRefAsync(nuGetProject, projectAdapter);
            }
            catch (Exception ex)
            {
                await _telemetryProvider.PostFaultAsync(ex, nameof(VsProjectJsonToPackageReferenceMigrator));
                throw;
            }
        }

        private async Task<object> MigrateProjectToPackageRefAsync(NuGetProject nuGetProject, IVsProjectAdapter projectAdapter)
        {
            var startTime = DateTimeOffset.Now;
            var stopwatch = Stopwatch.StartNew();
            if (nuGetProject is LegacyPackageReferenceProject)
            {
                EmitTelemetryEvent(startTime, stopwatch, nuGetProject, NuGetOperationStatus.NoOp);
                return new VsProjectJsonToPackageReferenceMigrateResult(success: true, errorMessage: null);
            }

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                await nuGetProject.SaveAsync(CancellationToken.None);

                var legacyPackageRefBasedProject = await _projectFactory.Value
                    .CreateNuGetProjectAsync<LegacyPackageReferenceProject>(
                        projectAdapter, optionalContext: null);
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
                EmitTelemetryEvent(startTime, stopwatch, nuGetProject, NuGetOperationStatus.Failed);
                return new VsProjectJsonToPackageReferenceMigrateResult(success: false, errorMessage: ex.Message);
            }
        }

        private async Task<(NuGetProject nuGetProject, IVsProjectAdapter projectAdapter)> GetNuGetProjectAndVSAdapter(string projectUniqueName)
        {
            var projectAdapter = await _solutionManager.Value.GetVsProjectAdapterAsync(projectUniqueName);

            if (projectAdapter == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, VsResources.Error_ProjectNotInCache, projectUniqueName));
            }

            var projectSafeName = projectAdapter.CustomUniqueName;

            var nuGetProject = await _solutionManager.Value.GetNuGetProjectAsync(projectSafeName);
            return (nuGetProject, projectAdapter);
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
    }
}

