// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.ProjectSystem;

[Export(typeof(IUnconfiguredProjectCommonServices))]
internal class UnconfiguredProjectCommonServices : IUnconfiguredProjectCommonServices
{
    [ImportingConstructor]
    public UnconfiguredProjectCommonServices(
        [Import(ExportContractNames.Scopes.UnconfiguredProject)] IProjectAsynchronousTasksService tasksService,
        IProjectThreadingService threadingService,
        UnconfiguredProject unconfiguredProject,
        IProjectFaultHandlerService faultHandlerService,
        IActiveConfigurationGroupSubscriptionService activeConfigurationGroupSubscriptionService)
    {
        if (tasksService is null)
        {
            throw new ArgumentNullException(nameof(tasksService));
        }

        if (threadingService is null)
        {
            throw new ArgumentNullException(nameof(threadingService));
        }

        if (unconfiguredProject is null)
        {
            throw new ArgumentNullException(nameof(unconfiguredProject));
        }

        if (activeConfigurationGroupSubscriptionService is null)
        {
            throw new ArgumentNullException(nameof(activeConfigurationGroupSubscriptionService));
        }

        TasksService = tasksService;
        ThreadingService = threadingService;
        UnconfiguredProject = unconfiguredProject;
        FaultHandlerService = faultHandlerService;
        ActiveConfigurationGroupSubscriptionService = activeConfigurationGroupSubscriptionService;
    }

    public IProjectAsynchronousTasksService TasksService { get; }

    public IProjectThreadingService ThreadingService { get; }

    public UnconfiguredProject UnconfiguredProject { get; }

    public IProjectFaultHandlerService FaultHandlerService { get; }

    public IActiveConfigurationGroupSubscriptionService ActiveConfigurationGroupSubscriptionService { get; }
}
