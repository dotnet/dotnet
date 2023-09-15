// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Threading;
using NuGet.Common;
using NuGet.PackageManagement.Telemetry;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    [Cmdlet(VerbsLifecycle.Uninstall, "Package")]
    public class UninstallPackageCommand : NuGetPowerShellBaseCommand
    {
        private readonly IDeleteOnRestartManager _deleteOnRestartManager;
        private readonly INuGetLockService _lockService;

        private UninstallationContext _context;

        public UninstallPackageCommand()
        {
            _deleteOnRestartManager = ServiceLocator.GetComponentModelService<IDeleteOnRestartManager>();
            _lockService = ServiceLocator.GetComponentModelService<INuGetLockService>();
        }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        public virtual string Id { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, Position = 1)]
        [ValidateNotNullOrEmpty]
        public virtual string ProjectName { get; set; }

        [Parameter(Position = 2)]
        [ValidateNotNullOrEmpty]
        public virtual string Version { get; set; }

        [Parameter]
        public SwitchParameter WhatIf { get; set; }

        [Parameter]
        public SwitchParameter Force { get; set; }

        [Parameter]
        public SwitchParameter RemoveDependencies { get; set; }

        private void Preprocess()
        {
            CheckSolutionState();

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await GetNuGetProjectAsync(ProjectName);
                await CheckMissingPackagesAsync();
            });

            ActionType = NuGetActionType.Uninstall;
        }

        protected override void ProcessRecordCore()
        {
            var startTime = DateTimeOffset.Now;
            _packageCount = 1;

            var stopWatch = Stopwatch.StartNew();

            // Run Preprocess outside of JTF
            Preprocess();

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await _lockService.ExecuteNuGetOperationAsync(() =>
                {
                    SubscribeToProgressEvents();
                    Task.Run(UninstallPackageAsync);
                    WaitAndLogPackageActions();
                    UnsubscribeFromProgressEvents();

                    return TaskResult.True;
                }, Token);
            });

            stopWatch.Stop();

            var isPackageSourceMappingEnabled = PackageSourceMappingUtility.IsMappingEnabled(ConfigSettings);
            var actionTelemetryEvent = VSTelemetryServiceUtility.GetActionTelemetryEvent(
                OperationId.ToString(),
                new[] { Project },
                NuGetProjectActionType.Uninstall,
                OperationSource.PMC,
                startTime,
                _status,
                _packageCount,
                stopWatch.Elapsed.TotalSeconds,
                isPackageSourceMappingEnabled: isPackageSourceMappingEnabled);

            // emit telemetry event along with granular level events
            TelemetryActivity.EmitTelemetryEvent(actionTelemetryEvent);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            var packageDirectoriesMarkedForDeletion = _deleteOnRestartManager.GetPackageDirectoriesMarkedForDeletion();
            if (packageDirectoriesMarkedForDeletion != null && packageDirectoriesMarkedForDeletion.Count != 0)
            {
                _deleteOnRestartManager.CheckAndRaisePackageDirectoriesMarkedForDeletion();
                var message = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    Resources.Cmdlet_RequestRestartToCompleteUninstall,
                    string.Join(", ", packageDirectoriesMarkedForDeletion));
                WriteWarning(message);
            }
        }

        /// <summary>
        /// Async call for uninstall a package from the current project
        /// </summary>
        private async Task UninstallPackageAsync()
        {
            try
            {
                await UninstallPackageByIdAsync(Project, Id, UninstallContext, this, WhatIf.IsPresent);
            }
            catch (Exception ex)
            {
                _status = NuGetOperationStatus.Failed;
                Log(MessageLevel.Error, ExceptionUtilities.DisplayMessage(ex));
            }
            finally
            {
                BlockingCollection.Add(new ExecutionCompleteMessage());
            }
        }

        /// <summary>
        /// Uninstall package by Id
        /// </summary>
        /// <param name="project"></param>
        /// <param name="packageId"></param>
        /// <param name="uninstallContext"></param>
        /// <param name="projectContext"></param>
        /// <param name="isPreview"></param>
        /// <returns></returns>
        protected async Task UninstallPackageByIdAsync(NuGetProject project, string packageId, UninstallationContext uninstallContext, INuGetProjectContext projectContext, bool isPreview)
        {
            var actions = await PackageManager.PreviewUninstallPackageAsync(project, packageId, uninstallContext, projectContext, CancellationToken.None);

            if (isPreview)
            {
                PreviewNuGetPackageActions(actions);
            }
            else
            {
                await PackageManager.ExecuteNuGetProjectActionsAsync(project, actions, projectContext, NullSourceCacheContext.Instance, CancellationToken.None);

                // Refresh Manager UI if needed
                RefreshUI(actions);
            }
        }

        /// <summary>
        /// Uninstallation Context for Uninstall-Package command
        /// </summary>
        public UninstallationContext UninstallContext
        {
            get
            {
                _context = new UninstallationContext(RemoveDependencies.IsPresent, Force.IsPresent);
                return _context;
            }
        }
    }
}
