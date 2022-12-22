// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using NuGet.Commands;
using NuGet.PackageManagement;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Etw;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.SolutionRestoreManager
{
    [Export(typeof(IVsNuGetProjectUpdateEvents))]
    [Export(typeof(IVsNuGetProgressReporter))]
    [Export(typeof(IRestoreProgressReporter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class VsRestoreProgressEvents : IVsNuGetProjectUpdateEvents, IVsNuGetProgressReporter
    {

        [ImportingConstructor]
        public VsRestoreProgressEvents(IPackageProjectEventsProvider eventProvider, INuGetTelemetryProvider telemetryProvider)
        {
            _ = eventProvider ?? throw new ArgumentNullException(nameof(eventProvider));

            // MEF components do not participate in Visual Studio's Package extensibility,
            // hence importing INuGetTelemetryProvider ensures that the ETW collector is
            // set up correctly.
            _ = telemetryProvider ?? throw new ArgumentNullException(nameof(telemetryProvider));

            var eventSource = eventProvider.GetPackageProjectEvents();
            eventSource.BatchStart += NotifyBatchStart;
            eventSource.BatchEnd += NotifyBatchEnd;
        }

        private const string SolutionRestoreStartedEventName = nameof(IVsNuGetProjectUpdateEvents) + "." + nameof(SolutionRestoreStarted);
        public event SolutionRestoreEventHandler SolutionRestoreStarted
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(SolutionRestoreStartedEventName, NuGetETW.AddEventOptions);
                _solutionRestoreStarted += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(SolutionRestoreStartedEventName, NuGetETW.RemoveEventOptions);
                _solutionRestoreStarted -= value;
            }
        }

        private const string SolutionRestoreFinishedEventName = nameof(IVsNuGetProjectUpdateEvents) + "." + nameof(SolutionRestoreFinished);
        public event SolutionRestoreEventHandler SolutionRestoreFinished
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(SolutionRestoreFinishedEventName, NuGetETW.AddEventOptions);
                _solutionRestoreFinished += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(SolutionRestoreFinishedEventName, NuGetETW.RemoveEventOptions);
                _solutionRestoreFinished -= value;
            }
        }

        private const string ProjectUpdateStartedEventName = nameof(IVsNuGetProjectUpdateEvents) + "." + nameof(ProjectUpdateStarted);
        public event ProjectUpdateEventHandler ProjectUpdateStarted
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(ProjectUpdateStartedEventName, NuGetETW.AddEventOptions);
                _projectUpdateStarted += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(ProjectUpdateStartedEventName, NuGetETW.RemoveEventOptions);
                _projectUpdateStarted -= value;
            }
        }

        private const string ProjectUpdateFinishedEventName = nameof(IVsNuGetProjectUpdateEvents) + "." + nameof(ProjectUpdateFinished);
        public event ProjectUpdateEventHandler ProjectUpdateFinished
        {
            add
            {
                NuGetETW.ExtensibilityEventSource.Write(ProjectUpdateFinishedEventName, NuGetETW.AddEventOptions);
                _projectUpdateFinished += value;
            }
            remove
            {
                NuGetETW.ExtensibilityEventSource.Write(ProjectUpdateFinishedEventName, NuGetETW.RemoveEventOptions);
                _projectUpdateFinished -= value;
            }
        }

        private event SolutionRestoreEventHandler _solutionRestoreStarted;
        private event SolutionRestoreEventHandler _solutionRestoreFinished;
        private event ProjectUpdateEventHandler _projectUpdateStarted;
        private event ProjectUpdateEventHandler _projectUpdateFinished;

        public void EndProjectUpdate(string projectName, IReadOnlyList<string> updatedFiles)
        {
            if (projectName == null)
            {
                throw new ArgumentNullException(nameof(projectName));
            }

            if (updatedFiles == null)
            {
                throw new ArgumentNullException(nameof(updatedFiles));
            }

            if (_projectUpdateFinished != null)
            {
                foreach (var handler in _projectUpdateFinished.GetInvocationList())
                {
                    try
                    {
                        handler.DynamicInvoke(projectName, updatedFiles);
                    }
                    catch { }
                }
            }
        }

        public void StartProjectUpdate(string projectName, IReadOnlyList<string> updatedFiles)
        {
            if (projectName == null)
            {
                throw new ArgumentNullException(nameof(projectName));
            }

            if (updatedFiles == null)
            {
                throw new ArgumentNullException(nameof(updatedFiles));
            }

            if (_projectUpdateStarted != null)
            {
                foreach (var handler in _projectUpdateStarted.GetInvocationList())
                {
                    try
                    {
                        handler.DynamicInvoke(projectName, updatedFiles);
                    }
                    catch { }
                }
            }
        }

        public void StartSolutionRestore(IReadOnlyList<string> projects)
        {
            if (projects == null || projects.Count == 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(projects)));
            }

            if (_solutionRestoreStarted != null)
            {
                foreach (var handler in _solutionRestoreStarted.GetInvocationList())
                {
                    try
                    {
                        handler.DynamicInvoke(projects);
                    }
                    catch { }
                }
            }
        }

        public void EndSolutionRestore(IReadOnlyList<string> projects)
        {
            if (projects == null || projects.Count == 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Argument_Cannot_Be_Null_Or_Empty, nameof(projects)));
            }

            if (_solutionRestoreFinished != null)
            {
                foreach (var handler in _solutionRestoreFinished.GetInvocationList())
                {
                    try
                    {
                        handler.DynamicInvoke(projects);
                    }
                    catch { }
                }
            }
        }

        private void NotifyBatchEnd(object sender, PackageProjectEventArgs e)
        {
            if (e.ProjectPath != null)
            {
                EndProjectUpdate(e.ProjectPath, new List<string>() { e.ProjectPath });
            }
        }

        private void NotifyBatchStart(object sender, PackageProjectEventArgs e)
        {
            if (e.ProjectPath != null)
            {
                StartProjectUpdate(e.ProjectPath, new List<string>() { e.ProjectPath });
            }
        }
    }
}
