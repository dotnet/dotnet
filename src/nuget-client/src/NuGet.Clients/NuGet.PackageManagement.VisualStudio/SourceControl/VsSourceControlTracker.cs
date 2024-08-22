// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(IVsSourceControlTracker))]
    internal sealed class VsSourceControlTracker : IVsSourceControlTracker
    {
        private readonly TrackProjectDocumentEventListener _projectDocumentListener;
        private readonly AsyncLazy<IVsTrackProjectDocuments2> _projectTracker;
        private readonly ISolutionManager _solutionManager;
        private readonly ISourceControlManagerProvider _sourceControlManagerProvider;
        private readonly Configuration.ISettings _vsSettings;
        private uint? _trackingCookie;

        private IVsTrackProjectDocuments2 ProjectTracker => ThreadHelper.JoinableTaskFactory.Run(_projectTracker.GetValueAsync);

        [ImportingConstructor]
        public VsSourceControlTracker(
            [Import(typeof(SVsServiceProvider))]
            IServiceProvider serviceProvider,
            ISolutionManager solutionManager,
            ISourceControlManagerProvider sourceControlManagerProvider,
            Configuration.ISettings vsSettings)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (solutionManager == null)
            {
                throw new ArgumentNullException(nameof(solutionManager));
            }

            if (sourceControlManagerProvider == null)
            {
                throw new ArgumentNullException(nameof(sourceControlManagerProvider));
            }

            if (vsSettings == null)
            {
                throw new ArgumentNullException(nameof(vsSettings));
            }

            _solutionManager = solutionManager;
            _sourceControlManagerProvider = sourceControlManagerProvider;
            _vsSettings = vsSettings;

            _projectTracker = new AsyncLazy<IVsTrackProjectDocuments2>(async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    return serviceProvider.GetService<SVsTrackProjectDocuments, IVsTrackProjectDocuments2>();
                },
                ThreadHelper.JoinableTaskFactory);

            _projectDocumentListener = new TrackProjectDocumentEventListener(this);

            _solutionManager.SolutionOpened += OnSolutionOpened;
            _solutionManager.SolutionClosed += OnSolutionClosed;

            if (_solutionManager.IsSolutionOpen)
            {
                StartTracking();
            }
        }

        public event EventHandler SolutionBoundToSourceControl = delegate { };

        private void OnSolutionOpened(object sender, EventArgs e)
        {
            StartTracking();
        }

        private void OnSolutionClosed(object sender, EventArgs e)
        {
            StopTracking();
        }

        private bool IsTracking
        {
            get { return _trackingCookie != null; }
        }

        private void StartTracking()
        {
            // don't track again if already tracking
            if (IsTracking)
            {
                return;
            }

            // don't do anything if user explicitly disables source control integration
            if (_vsSettings != null
                && SourceControlUtility.IsSourceControlDisabled(_vsSettings))
            {
                return;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                uint cookie;
                if (ProjectTracker.AdviseTrackProjectDocumentsEvents(_projectDocumentListener, out cookie) == VSConstants.S_OK)
                {
                    _trackingCookie = cookie;
                }
            });
        }

        private void StopTracking()
        {
            if (!IsTracking)
            {
                return;
            }

            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var hr = ProjectTracker.UnadviseTrackProjectDocumentsEvents(_trackingCookie.Value);
                ErrorHandler.ThrowOnFailure(hr);
            });

            _trackingCookie = null;
        }

        private void OnSourceControlBound()
        {
            if (_vsSettings == null)
            {
                return;
            }

            try
            {
                var solutionRepositoryPath = PackagesFolderPathUtility.GetPackagesFolderPath(_solutionManager, _vsSettings);
                if (Directory.Exists(solutionRepositoryPath)
                    && _sourceControlManagerProvider != null)
                {
                    var sourceControlManager = _sourceControlManagerProvider.GetSourceControlManager();

                    // only proceed if the source-control is in use
                    if (sourceControlManager != null)
                    {
                        var allFiles = Directory.EnumerateFiles(solutionRepositoryPath, "*.*",
                            SearchOption.AllDirectories);
                        var file = allFiles.FirstOrDefault();
                        if (file != null)
                        {
                            // If there are any files under the packages directory and any given file (in this case the first one we come across) is not under
                            // source control, bind it.
                            sourceControlManager.PendAddFiles(allFiles, solutionRepositoryPath, new EmptyNuGetProjectContext());
                        }
                    }
                }
            }
            finally
            {
                StopTracking();

                // raise event. This event is likely never used by anything
                SolutionBoundToSourceControl(this, EventArgs.Empty);
            }
        }

        private class TrackProjectDocumentEventListener : IVsTrackProjectDocumentsEvents2
        {
            private readonly VsSourceControlTracker _parent;

            public TrackProjectDocumentEventListener(VsSourceControlTracker parent)
            {
                _parent = parent;
            }

            public int OnAfterSccStatusChanged(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, uint[] rgdwSccStatus)
            {
                // The value 0x1F is the AND of the following bits.
                // We want to check if any of these 5 bits is on.

                // SCC_STATUS_CONTROLLED = 1,
                // SCC_STATUS_CHECKEDOUT = 2,
                // SCC_STATUS_OUTOTHER = 4,
                // SCC_STATUS_OUTEXCLUSIVE = 8,
                // SCC_STATUS_OUTMULTIPLE = 16,

                if (cProjects > 0
                    &&
                    cFiles > 0
                    &&
                    rgdwSccStatus != null
                    &&
                    rgdwSccStatus.Any(f => (f & 0x1F) != 0)
                    &&
                    rgpszMkDocuments != null
                    &&
                    rgpszMkDocuments.Any(s => s.EndsWith(".sln", StringComparison.OrdinalIgnoreCase)))
                {
                    _parent.OnSourceControlBound();
                }

                return VSConstants.S_OK;
            }

            #region Irrelevant members

            public int OnAfterAddDirectoriesEx(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDDIRECTORYFLAGS[] rgFlags)
            {
                return VSConstants.S_OK;
            }

            public int OnAfterAddFilesEx(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDFILEFLAGS[] rgFlags)
            {
                return VSConstants.S_OK;
            }

            public int OnAfterRemoveDirectories(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEDIRECTORYFLAGS[] rgFlags)
            {
                return VSConstants.S_OK;
            }

            public int OnAfterRemoveFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEFILEFLAGS[] rgFlags)
            {
                return VSConstants.S_OK;
            }

            public int OnAfterRenameDirectories(int cProjects, int cDirs, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEDIRECTORYFLAGS[] rgFlags)
            {
                return VSConstants.S_OK;
            }

            public int OnAfterRenameFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEFILEFLAGS[] rgFlags)
            {
                return VSConstants.S_OK;
            }

            public int OnQueryAddDirectories(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments, VSQUERYADDDIRECTORYFLAGS[] rgFlags, VSQUERYADDDIRECTORYRESULTS[] pSummaryResult, VSQUERYADDDIRECTORYRESULTS[] rgResults)
            {
                return VSConstants.S_OK;
            }

            public int OnQueryAddFiles(IVsProject pProject, int cFiles, string[] rgpszMkDocuments, VSQUERYADDFILEFLAGS[] rgFlags, VSQUERYADDFILERESULTS[] pSummaryResult, VSQUERYADDFILERESULTS[] rgResults)
            {
                return VSConstants.S_OK;
            }

            public int OnQueryRemoveDirectories(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments, VSQUERYREMOVEDIRECTORYFLAGS[] rgFlags, VSQUERYREMOVEDIRECTORYRESULTS[] pSummaryResult, VSQUERYREMOVEDIRECTORYRESULTS[] rgResults)
            {
                return VSConstants.S_OK;
            }

            public int OnQueryRemoveFiles(IVsProject pProject, int cFiles, string[] rgpszMkDocuments, VSQUERYREMOVEFILEFLAGS[] rgFlags, VSQUERYREMOVEFILERESULTS[] pSummaryResult, VSQUERYREMOVEFILERESULTS[] rgResults)
            {
                return VSConstants.S_OK;
            }

            public int OnQueryRenameDirectories(IVsProject pProject, int cDirs, string[] rgszMkOldNames, string[] rgszMkNewNames, VSQUERYRENAMEDIRECTORYFLAGS[] rgFlags, VSQUERYRENAMEDIRECTORYRESULTS[] pSummaryResult, VSQUERYRENAMEDIRECTORYRESULTS[] rgResults)
            {
                return VSConstants.S_OK;
            }

            public int OnQueryRenameFiles(IVsProject pProject, int cFiles, string[] rgszMkOldNames, string[] rgszMkNewNames, VSQUERYRENAMEFILEFLAGS[] rgFlags, VSQUERYRENAMEFILERESULTS[] pSummaryResult, VSQUERYRENAMEFILERESULTS[] rgResults)
            {
                return VSConstants.S_OK;
            }

            #endregion
        }
    }
}
