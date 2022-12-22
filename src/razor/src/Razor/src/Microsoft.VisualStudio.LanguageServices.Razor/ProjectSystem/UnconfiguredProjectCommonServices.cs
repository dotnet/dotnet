// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.References;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

[Export(typeof(IUnconfiguredProjectCommonServices))]
internal class UnconfiguredProjectCommonServices : IUnconfiguredProjectCommonServices
{
    private readonly ActiveConfiguredProject<ConfiguredProject> _activeConfiguredProject;
    private readonly ActiveConfiguredProject<IAssemblyReferencesService> _activeConfiguredProjectAssemblyReferences;
    private readonly ActiveConfiguredProject<IPackageReferencesService> _activeConfiguredProjectPackageReferences;
    private readonly ActiveConfiguredProject<Rules.RazorProjectProperties> _activeConfiguredProjectProperties;

    [ImportingConstructor]
    public UnconfiguredProjectCommonServices(
        [Import(ExportContractNames.Scopes.UnconfiguredProject)] IProjectAsynchronousTasksService tasksService,
        IProjectThreadingService threadingService,
        UnconfiguredProject unconfiguredProject,
        IActiveConfiguredProjectSubscriptionService activeConfiguredProjectSubscription,
        ActiveConfiguredProject<ConfiguredProject> activeConfiguredProject,
        ActiveConfiguredProject<IAssemblyReferencesService> activeConfiguredProjectAssemblyReferences,
        ActiveConfiguredProject<IPackageReferencesService> activeConfiguredProjectPackageReferences,
        ActiveConfiguredProject<Rules.RazorProjectProperties> activeConfiguredProjectRazorProperties)
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

        if (activeConfiguredProjectSubscription is null)
        {
            throw new ArgumentNullException(nameof(activeConfiguredProjectSubscription));
        }

        if (activeConfiguredProject is null)
        {
            throw new ArgumentNullException(nameof(activeConfiguredProject));
        }

        if (activeConfiguredProjectAssemblyReferences is null)
        {
            throw new ArgumentNullException(nameof(activeConfiguredProjectAssemblyReferences));
        }

        if (activeConfiguredProjectPackageReferences is null)
        {
            throw new ArgumentNullException(nameof(activeConfiguredProjectPackageReferences));
        }

        if (activeConfiguredProjectRazorProperties is null)
        {
            throw new ArgumentNullException(nameof(activeConfiguredProjectRazorProperties));
        }

        TasksService = tasksService;
        ThreadingService = threadingService;
        UnconfiguredProject = unconfiguredProject;
        ActiveConfiguredProjectSubscription = activeConfiguredProjectSubscription;
        _activeConfiguredProject = activeConfiguredProject;
        _activeConfiguredProjectAssemblyReferences = activeConfiguredProjectAssemblyReferences;
        _activeConfiguredProjectPackageReferences = activeConfiguredProjectPackageReferences;
        _activeConfiguredProjectProperties = activeConfiguredProjectRazorProperties;
    }

    public ConfiguredProject ActiveConfiguredProject => _activeConfiguredProject.Value;

    public IAssemblyReferencesService ActiveConfiguredProjectAssemblyReferences => _activeConfiguredProjectAssemblyReferences.Value;

    public IPackageReferencesService ActiveConfiguredProjectPackageReferences => _activeConfiguredProjectPackageReferences.Value;

    public Rules.RazorProjectProperties ActiveConfiguredProjectRazorProperties => _activeConfiguredProjectProperties.Value;

    public IActiveConfiguredProjectSubscriptionService ActiveConfiguredProjectSubscription { get; }

    public IProjectAsynchronousTasksService TasksService { get; }

    public IProjectThreadingService ThreadingService { get; }

    public UnconfiguredProject UnconfiguredProject { get; }
}
