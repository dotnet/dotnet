// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Microsoft.VisualStudio.ComponentModelHost;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.PackageManagement.VisualStudio
{
    public sealed class NuGetSolutionManagerService : INuGetSolutionManagerService
    {
        private readonly ServiceActivationOptions _options;
        private readonly IServiceBroker _serviceBroker;
        private readonly AuthorizationServiceClient _authorizationServiceClient;

        [Import]
        private IVsSolutionManager? SolutionManager { get; set; }

        public event EventHandler<string>? AfterNuGetCacheUpdated;
        public event EventHandler<IProjectContextInfo>? AfterProjectRenamed;
        public event EventHandler<IProjectContextInfo>? ProjectAdded;
        public event EventHandler<IProjectContextInfo>? ProjectRemoved;
        public event EventHandler<IProjectContextInfo>? ProjectRenamed;
        public event EventHandler<IProjectContextInfo>? ProjectUpdated;

        private NuGetSolutionManagerService(
            ServiceActivationOptions options,
            IServiceBroker sb,
            AuthorizationServiceClient ac,
            IComponentModel componentModel)
        {
            Assumes.NotNull(sb);
            Assumes.NotNull(ac);

            _options = options;
            _serviceBroker = sb;
            _authorizationServiceClient = ac;

            componentModel.DefaultCompositionService.SatisfyImportsOnce(this);

            Assumes.NotNull(SolutionManager);

            RegisterEventHandlers();
        }

        public static async ValueTask<NuGetSolutionManagerService> CreateAsync(
            ServiceActivationOptions options,
            IServiceBroker sb,
            AuthorizationServiceClient ac,
            CancellationToken cancellationToken)
        {
            Assumes.NotNull(sb);
            Assumes.NotNull(ac);

            cancellationToken.ThrowIfCancellationRequested();

            IComponentModel componentModel = await ServiceLocator.GetComponentModelAsync();

            Assumes.NotNull(componentModel);

            return new NuGetSolutionManagerService(options, sb, ac, componentModel);
        }

        public void Dispose()
        {
            UnregisterEventHandlers();

            _authorizationServiceClient.Dispose();

            GC.SuppressFinalize(this);
        }

        public async ValueTask<string> GetSolutionDirectoryAsync(CancellationToken cancellationToken)
        {
            Assumes.NotNull(SolutionManager);

            cancellationToken.ThrowIfCancellationRequested();

            return await SolutionManager.GetSolutionDirectoryAsync();
        }

        private static string CreateProjectActionId()
        {
            return Guid.NewGuid().ToString("N", provider: null);
        }

        private void RegisterEventHandlers()
        {
            Assumes.NotNull(SolutionManager);

            SolutionManager!.AfterNuGetCacheUpdated += OnAfterNuGetCacheUpdated;
            SolutionManager!.AfterNuGetProjectRenamed += OnAfterProjectRenamed;
            SolutionManager!.NuGetProjectAdded += OnProjectAdded;
            SolutionManager!.NuGetProjectRemoved += OnProjectRemoved;
            SolutionManager!.NuGetProjectRenamed += OnProjectRenamed;
            SolutionManager!.NuGetProjectUpdated += OnProjectUpdated;
        }

        private void OnAfterNuGetCacheUpdated(object sender, NuGetEventArgs<string> e)
        {
            AfterNuGetCacheUpdated?.Invoke(sender, e.Arg);
        }

        private void OnAfterProjectRenamed(object sender, NuGetProjectEventArgs e)
        {
            OnProjectEvent(AfterProjectRenamed, nameof(OnAfterProjectRenamed), sender, e);
        }

        private void OnProjectAdded(object sender, NuGetProjectEventArgs e)
        {
            OnProjectEvent(ProjectAdded, nameof(OnProjectAdded), sender, e);
        }

        private void OnProjectRemoved(object sender, NuGetProjectEventArgs e)
        {
            OnProjectEvent(ProjectRemoved, nameof(OnProjectRemoved), sender, e);
        }

        private void OnProjectRenamed(object sender, NuGetProjectEventArgs e)
        {
            OnProjectEvent(ProjectRenamed, nameof(OnProjectRenamed), sender, e);
        }

        private void OnProjectUpdated(object sender, NuGetProjectEventArgs e)
        {
            OnProjectEvent(ProjectUpdated, nameof(OnProjectUpdated), sender, e);
        }

        private void OnProjectEvent(
            EventHandler<IProjectContextInfo>? eventHandler,
            string memberName,
            object sender,
            NuGetProjectEventArgs e)
        {
            if (eventHandler != null)
            {
                NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                    {
                        IProjectContextInfo project = await ProjectContextInfo.CreateAsync(
                            e.NuGetProject,
                            CancellationToken.None);

                        eventHandler(sender, project);
                    })
                    .PostOnFailure(nameof(NuGetSolutionManagerService), memberName);
            }
        }

        private void UnregisterEventHandlers()
        {
            Assumes.NotNull(SolutionManager);

            SolutionManager!.AfterNuGetCacheUpdated -= OnAfterNuGetCacheUpdated;
            SolutionManager!.AfterNuGetProjectRenamed -= OnAfterProjectRenamed;
            SolutionManager!.NuGetProjectAdded -= OnProjectAdded;
            SolutionManager!.NuGetProjectRemoved -= OnProjectRemoved;
            SolutionManager!.NuGetProjectRenamed -= OnProjectRenamed;
            SolutionManager!.NuGetProjectUpdated -= OnProjectUpdated;
        }
    }
}
