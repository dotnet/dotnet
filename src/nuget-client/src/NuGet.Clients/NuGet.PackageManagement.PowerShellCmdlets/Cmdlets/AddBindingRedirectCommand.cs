// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    [Cmdlet(VerbsCommon.Add, "BindingRedirect")]
    [OutputType(typeof(AssemblyBinding))]
    public class AddBindingRedirectCommand : NuGetPowerShellBaseCommand
    {
        private readonly AsyncLazy<IVsFrameworkMultiTargeting> _frameworkMultiTargeting;

        public AddBindingRedirectCommand()
        {
            _frameworkMultiTargeting = new AsyncLazy<IVsFrameworkMultiTargeting>(
                () => ServiceLocator.GetGlobalServiceAsync<SVsFrameworkMultiTargeting, IVsFrameworkMultiTargeting>(),
                NuGetUIThreadHelper.JoinableTaskFactory);
        }

        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "PowerShell API requirement")]
        public string[] ProjectName { get; set; }

        /// <summary>
        /// logging time disabled for tab command
        /// </summary>
        protected override bool IsLoggingTimeDisabled
        {
            get
            {
                return true;
            }
        }

        protected override void ProcessRecordCore()
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    CheckSolutionState();

                    var projects = new List<IVsProjectAdapter>();

                    // if no project specified, use default
                    if (ProjectName == null)
                    {
                        var defaultProject = await GetDefaultProjectAsync();

                        // if no default project (empty solution), throw terminating
                        if (defaultProject == null)
                        {
                            ErrorHandler.ThrowNoCompatibleProjectsTerminatingError();
                        }

                        projects.Add(defaultProject);
                    }
                    else
                    {
                        // get matching projects, expanding wildcards
                        projects.AddRange(await GetProjectsByNameAsync(ProjectName));
                    }

                    // Create a new app domain so we don't load the assemblies into the host app domain
                    var domain = AppDomain.CreateDomain("domain");

                    try
                    {
                        var frameworkMultiTargeting = await _frameworkMultiTargeting.GetValueAsync();
                        foreach (var project in projects)
                        {
                            var projectAssembliesCache = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
                            var redirects = await RuntimeHelpers.AddBindingRedirectsAsync(VsSolutionManager, project, domain, projectAssembliesCache, frameworkMultiTargeting, this);

                            // Print out what we did
                            WriteObject(redirects, enumerateCollection: true);
                        }
                    }
                    finally
                    {
                        AppDomain.Unload(domain);
                    }
                });
        }
    }
}
