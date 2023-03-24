// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Workspace.VSIntegration.UI;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.PackageManagement.UI;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;
using Resx = NuGet.PackageManagement.UI.Resources;
using Task = System.Threading.Tasks.Task;
using vsShellInterop = Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.VisualStudio.OnlineEnvironment.Client
{
    internal class PackageManagerUICommandHandler
    {
        private const bool IsPackageManagerUISupportAvailable = true;
        private readonly JoinableTaskFactory _joinableTaskFactory;
        private readonly IAsyncServiceProvider _asyncServiceProvider;

        private AsyncLazy<vsShellInterop.IVsMonitorSelection> _vsMonitorSelection;
        private vsShellInterop.IVsMonitorSelection VsMonitorSelection => ThreadHelper.JoinableTaskFactory.Run(_vsMonitorSelection.GetValueAsync);

        private DTE _dte;
        private DTEEvents _dteEvents;
        private const string F1KeywordValuePmUI = "VS.NuGet.PackageManager.UI";
        private uint _solutionExistsAndFullyLoadedContextCookie;
        private uint _solutionNotBuildingAndNotDebuggingContextCookie;
        private uint _solutionExistsCookie;
        private uint _maxToolWindowId = 0;
        private Dictionary<string, uint> _projectGuidToToolWindowId;

        private bool _initialized;

        public PackageManagerUICommandHandler(JoinableTaskFactory joinableTaskFactory, IAsyncServiceProvider asyncServiceProvider)
        {
            _joinableTaskFactory = joinableTaskFactory ?? throw new ArgumentNullException(nameof(joinableTaskFactory));
            _asyncServiceProvider = asyncServiceProvider ?? throw new ArgumentNullException(nameof(asyncServiceProvider));
            Initialize();
        }

        [Import]
        private Lazy<IConsoleStatus> ConsoleStatus { get; set; }

        [Import]
        private Lazy<IDeleteOnRestartManager> DeleteOnRestartManager { get; set; }

        [Import]
        private Lazy<INuGetUILogger> OutputConsoleLogger { get; set; }

        [Import]
        private Lazy<INuGetProjectContext> ProjectContext { get; set; }

        [Import]
        private Lazy<ISettings> Settings { get; set; }

        [Import]
        private Lazy<IVsSolutionManager> SolutionManager { get; set; }

        [Import]
        private Lazy<SolutionUserOptions> SolutionUserOptions { get; set; }

        /// <summary>
        /// This initializes the IVSSourceControlTracker, even though SourceControlTracker is unused.
        /// </summary>
        [Import]
        private Lazy<IVsSourceControlTracker> SourceControlTracker { get; set; }

        [Import]
        private Lazy<INuGetUIFactory> UIFactory { get; set; }

        private IDisposable ProjectRetargetingHandler { get; set; }

        private IDisposable ProjectUpgradeHandler { get; set; }

        [Import]
        private Lazy<IServiceBrokerProvider> ServiceBrokerProvider { get; set; }

        [Import]
        private Lazy<INuGetExperimentationService> NuGetExperimentationService { get; set; }

        private void Initialize()
        {
            _vsMonitorSelection = new AsyncLazy<IVsMonitorSelection>(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    // get the UI context cookie for the debugging mode
                    var vsMonitorSelection = await _asyncServiceProvider.GetServiceAsync<IVsMonitorSelection, IVsMonitorSelection>();
                    Assumes.Present(vsMonitorSelection);

                    // get the solution not building and not debugging cookie
                    var guidCmdUI = VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_guid;
                    vsMonitorSelection.GetCmdUIContextCookie(
                        ref guidCmdUI, out _solutionExistsAndFullyLoadedContextCookie);

                    guidCmdUI = VSConstants.UICONTEXT.SolutionExistsAndNotBuildingAndNotDebugging_guid;
                    vsMonitorSelection.GetCmdUIContextCookie(
                        ref guidCmdUI, out _solutionNotBuildingAndNotDebuggingContextCookie);

                    guidCmdUI = VSConstants.UICONTEXT.SolutionExists_guid;
                    vsMonitorSelection.GetCmdUIContextCookie(
                        ref guidCmdUI, out _solutionExistsCookie);

                    return vsMonitorSelection;
                },
                ThreadHelper.JoinableTaskFactory);
            _projectGuidToToolWindowId = new Dictionary<string, uint>();
        }

        public bool IsPackageManagerUISupported(WorkspaceVisualNodeBase workspaceVisualNodeBase)
        {
            if (workspaceVisualNodeBase is null)
            {
                return false;
            }
            return IsPackageManagerUISupportAvailable;
        }

        public void OpenPackageManagerUI(WorkspaceVisualNodeBase workspaceVisualNodeBase)
        {
            _joinableTaskFactory.RunAsync(() => OpenPackageManagerUIAsync(workspaceVisualNodeBase)).PostOnFailure(nameof(PackageManagerUICommandHandler));
        }

        private async Task OpenPackageManagerUIAsync(WorkspaceVisualNodeBase workspaceVisualNodeBase)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            await ShowPackageManagerUIAsync(workspaceVisualNodeBase);
        }

        /// <summary>
        /// Initialize all MEF imports for this package and also add required event handlers.
        /// </summary>
        private async Task InitializeMEFAsync()
        {
            _initialized = true;

            var componentModel = await _asyncServiceProvider.GetComponentModelAsync();
            Assumes.Present(componentModel);
            componentModel.DefaultCompositionService.SatisfyImportsOnce(this);
            var experimentationService = NuGetExperimentationService.Value;
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            Brushes.LoadVsBrushes(experimentationService);

            _dte = await _asyncServiceProvider.GetDTEAsync();
            Assumes.Present(_dte);

            _dteEvents = _dte.Events.DTEEvents;
            //_dteEvents.OnBeginShutdown += OnBeginShutDown;

            if (SolutionManager.Value.NuGetProjectContext == null)
            {
                SolutionManager.Value.NuGetProjectContext = ProjectContext.Value;
            }

            // when NuGet loads, if the current solution has some package
            // folders marked for deletion (because a previous uninstalltion didn't succeed),
            // delete them now.
            if (await SolutionManager.Value.IsSolutionOpenAsync())
            {
                await DeleteOnRestartManager.Value.DeleteMarkedPackageDirectoriesAsync(ProjectContext.Value);
            }

            //ProjectRetargetingHandler = new ProjectRetargetingHandler(_dte, SolutionManager.Value, this, componentModel);
            //ProjectUpgradeHandler = new ProjectUpgradeHandler(this, SolutionManager.Value);

            SolutionUserOptions.Value.LoadSettings();
        }

        private async Task ShowPackageManagerUIAsync(WorkspaceVisualNodeBase workspaceVisualNodeBase)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (ShouldMEFBeInitialized())
            {
                await InitializeMEFAsync();
            }

            IVsWindowFrame window = await CreateNewWindowFrameAsync(workspaceVisualNodeBase);
            if (window != null)
            {
                Search(window, string.Empty);
                window.Show();
            }
        }

        private async Task<IVsWindowFrame> CreateNewWindowFrameAsync(WorkspaceVisualNodeBase workspaceVisualNodeBase)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Find existing hierarchy and item id of the document window if it's already registered.
            var rdt = await _asyncServiceProvider.GetServiceAsync<IVsRunningDocumentTable, IVsRunningDocumentTable>();
            Assumes.Present(rdt);
            IVsHierarchy hier;
            uint itemId;
            var docData = IntPtr.Zero;
            int hr;

            try
            {
                uint cookie;
                hr = rdt.FindAndLockDocument(
                    (uint)_VSRDTFLAGS.RDT_NoLock,
                    workspaceVisualNodeBase.NodeMoniker,
                    out hier,
                    out itemId,
                    out docData,
                    out cookie);

                if (hr != VSConstants.S_OK)
                {
                    // the docuemnt window is not registered yet. So use the hierarchy from the current selection.
                    itemId = (uint)VSConstants.VSITEMID.Root;
                }
            }
            finally
            {
                if (docData != IntPtr.Zero)
                {
                    Marshal.Release(docData);
                    docData = IntPtr.Zero;
                }
            }

            return await CreateToolWindowAsync(workspaceVisualNodeBase, hier, itemId);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Caller's responsibility to dispose.")]
        private async ValueTask<IVsWindowFrame> CreateToolWindowAsync(WorkspaceVisualNodeBase workspaceVisualNodeBase, IVsHierarchy hier, uint itemId)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (!Guid.TryParse(IProjectContextInfoUtility.GetProjectGuidStringFromVslsQueryString(workspaceVisualNodeBase.VSSelectionMoniker), out Guid projectGuid))
            {
                throw new InvalidOperationException();
            }

            IVsWindowFrame windowFrame = null;

            var uiShell = await _asyncServiceProvider.GetServiceAsync<SVsUIShell, IVsUIShell>();
            Assumes.Present(uiShell);

            uint toolWindowId;
            bool foundToolWindowId = _projectGuidToToolWindowId.TryGetValue(projectGuid.ToString(), out toolWindowId);
            const uint FTW_none = 0;

            if (foundToolWindowId)
            {
                ErrorHandler.ThrowOnFailure(
                    uiShell.FindToolWindowEx(
                        FTW_none, //grfFTW - badly-documented enum value.
                        typeof(PackageManagerToolWindowPane).GUID,    // rguidPersistenceSlot
                        toolWindowId,   // dwToolWindowId
                        out windowFrame));

                if (windowFrame != null)
                {
                    ((IVsWindowFrame2)windowFrame).ActivateOwnerDockedWindow();
                }
                else
                {
                    _projectGuidToToolWindowId.Remove(projectGuid.ToString());
                }
            }

            if (windowFrame == null)
            {
                IServiceBroker serviceBroker = await ServiceBrokerProvider.Value.GetAsync();
                IProjectContextInfo projectContextInfo = await IProjectContextInfoUtility.CreateAsync(serviceBroker, projectGuid.ToString(), CancellationToken.None);
                INuGetUI uiController = await UIFactory.Value.CreateAsync(serviceBroker, projectContextInfo);
                // This model takes ownership of --- and Dispose() responsibility for --- the INuGetUI instance.
                var model = new PackageManagerModel(uiController, isSolution: false, editorFactoryGuid: GuidList.NuGetEditorType);
                var control = await PackageManagerControl.CreateAsync(model, OutputConsoleLogger.Value);
                var caption = string.Format(CultureInfo.CurrentCulture, Resx.Label_NuGetWindowCaption, Path.GetFileNameWithoutExtension(workspaceVisualNodeBase.NodeMoniker));

                int[] pfDefaultPosition = null;

                var windowPane = new PackageManagerToolWindowPane(control, projectGuid.ToString());
                ErrorHandler.ThrowOnFailure(
                    uiShell.CreateToolWindow(
                        (uint)__VSCREATETOOLWIN.CTW_fInitNew,
                        ++_maxToolWindowId,    // dwToolWindowId
                        windowPane,     // ToolWindowPane
                        Guid.Empty,     // rclsidTool = GUID_NULL
                        typeof(PackageManagerToolWindowPane).GUID,    // rguidPersistenceSlot
                        Guid.Empty,     // reserved - do not use - GUID_NULL
                        null,           // IServiceProvider
                        caption,
                        pfDefaultPosition,
                        out windowFrame));
                _projectGuidToToolWindowId.Add(projectGuid.ToString(), _maxToolWindowId);
                windowPane.Closed += WindowPane_Closed;

                if (windowFrame != null)
                {
                    WindowFrameHelper.AddF1HelpKeyword(windowFrame, keywordValue: F1KeywordValuePmUI);
                    WindowFrameHelper.DisableWindowAutoReopen(windowFrame);
                    WindowFrameHelper.DockToolWindow(windowFrame);
                }
            }

            return windowFrame;
        }

        private void WindowPane_Closed(object sender, EventArgs e)
        {
            var windowPane = (PackageManagerToolWindowPane)sender;
            _projectGuidToToolWindowId.Remove(windowPane.ProjectGuid);
            windowPane.Closed -= WindowPane_Closed;
        }

        private async Task<IVsWindowFrame> FindExistingSolutionWindowFrameAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var uiShell = await _asyncServiceProvider.GetServiceAsync<SVsUIShell, IVsUIShell>();
            foreach (var windowFrame in VsUtility.GetDocumentWindows(uiShell))
            {
                object property;
                var hr = windowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocData, out property);
                var packageManagerControl = VsUtility.GetPackageManagerControl(windowFrame);
                if (hr == VSConstants.S_OK
                    && property is IVsSolution
                    && packageManagerControl != null)
                {
                    return windowFrame;
                }
            }

            return null;
        }

        private static string GetSearchText(string parameterString)
        {
            if (parameterString == null)
            {
                return null;
            }

            // The parameterString looks like:
            //     "jquery /searchin:online"
            // or just "jquery"

            parameterString = parameterString.Trim();
            var lastIndexOfSearchInSwitch = parameterString.LastIndexOf("/searchin:", StringComparison.OrdinalIgnoreCase);

            if (lastIndexOfSearchInSwitch == -1)
            {
                return parameterString;
            }
            return parameterString.Substring(0, lastIndexOfSearchInSwitch);
        }

        /// <summary>
        /// Search for packages using the searchText.
        /// </summary>
        /// <param name="windowFrame">A window frame that hosts the PackageManagerControl.</param>
        /// <param name="searchText">Search text.</param>
        // private void Search(System.Windows.Window windowFrame, string searchText)
        private void Search(vsShellInterop.IVsWindowFrame windowFrame, string searchText)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return;
            }

            var packageManagerControl = VsUtility.GetPackageManagerControl(windowFrame);
            if (packageManagerControl != null)
            {
                packageManagerControl.Search(searchText);
            }
        }

        private bool ShouldMEFBeInitialized()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return !_initialized;
        }
    }
}
