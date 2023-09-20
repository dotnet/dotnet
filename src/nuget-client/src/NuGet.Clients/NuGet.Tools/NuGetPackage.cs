// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.PackageManagement;
using NuGet.PackageManagement.UI;
using NuGet.PackageManagement.UI.Options;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Common.Telemetry;
using NuGet.VisualStudio.Internal.Contracts;
using NuGet.VisualStudio.Telemetry;
using NuGetConsole;
using NuGetConsole.Implementation;
using ContractsNuGetServices = NuGet.VisualStudio.Contracts.NuGetServices;
using ISettings = NuGet.Configuration.ISettings;
using ProvideBrokeredServiceAttribute = Microsoft.VisualStudio.Shell.ServiceBroker.ProvideBrokeredServiceAttribute;
using Resx = NuGet.PackageManagement.UI.Resources;
using ServiceAudience = Microsoft.VisualStudio.Shell.ServiceBroker.ServiceAudience;
using Task = System.Threading.Tasks.Task;
using UI = NuGet.PackageManagement.UI;

namespace NuGetVSExtension
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", ProductVersion)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(PowerConsoleToolWindow),
        Style = VsDockStyle.Tabbed,
        Window = "{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}", // this is the guid of the Output tool window, which is present in both VS and VWD
        Orientation = ToolWindowOrientation.Right)]
    [ProvideOptionPage(typeof(GeneralOptionPage), "NuGet Package Manager", "General", 113, 115, true)]
    [ProvideOptionPage(typeof(ConfigurationFilesOptionsPage), "NuGet Package Manager", "Configuration Files", 113, 117, true, Sort = 0)]
    [ProvideOptionPage(typeof(PackageSourceOptionsPage), "NuGet Package Manager", "Package Sources", 113, 114, true, Sort = 1)]
    [ProvideOptionPage(typeof(PackageSourceMappingOptionsPage), "NuGet Package Manager", "Package Source Mapping", 113, 116, true, Sort = 2)]
    [ProvideSearchProvider(typeof(NuGetSearchProvider), "NuGet Search")]
    // UI Context rule for a project that could be upgraded to PackageReference from packages.config based project.
    // Only exception is this UI context doesn't get enabled for right-click on Reference since there is no extension point on references
    // to know if there is packages.config file in this project hierarchy. So first-time right click on reference in a new VS instance
    // will not show Migrator option.
    [ProvideUIContextRule(GuidList.guidUpgradeableProjectLoadedString,
        "UpgradeableProjectLoaded",
        "SolutionExistsAndFullyLoaded & PackagesConfigBasedProjectLoaded",
        new[] { "SolutionExistsAndFullyLoaded", "PackagesConfigBasedProjectLoaded" },
        new[] { VSConstants.UICONTEXT.SolutionExistsAndFullyLoaded_string,
            "HierSingleSelectionName:packages.config"})]
    [ProvideAutoLoad(GuidList.guidUpgradeableProjectLoadedString, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(GuidList.guidAutoLoadNuGetString, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.ProjectRetargeting_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionOrProjectUpgrading_string, PackageAutoLoadFlags.BackgroundLoad)]
    [FontAndColorsRegistration("Package Manager Console", NuGetConsole.GuidList.GuidPackageManagerConsoleFontAndColorCategoryString, "{" + GuidList.guidNuGetPkgString + "}")]
    [ProvideBrokeredService(ContractsNuGetServices.NuGetProjectServiceName, ContractsNuGetServices.Version1, Audience = ServiceAudience.Local | ServiceAudience.RemoteExclusiveClient)]
    [ProvideBrokeredService(BrokeredServicesUtilities.SourceProviderServiceName, BrokeredServicesUtilities.SourceProviderServiceVersion, Audience = ServiceAudience.Local | ServiceAudience.RemoteExclusiveClient)]
    [ProvideBrokeredService(BrokeredServicesUtilities.SourceProviderServiceName, BrokeredServicesUtilities.SourceProviderServiceVersion_1_0_1, Audience = ServiceAudience.Local | ServiceAudience.RemoteExclusiveClient)]
    [ProvideBrokeredService(BrokeredServicesUtilities.SolutionManagerServiceName, BrokeredServicesUtilities.SolutionManagerServiceVersion, Audience = ServiceAudience.Local | ServiceAudience.RemoteExclusiveClient)]
    [ProvideBrokeredService(BrokeredServicesUtilities.ProjectManagerServiceName, BrokeredServicesUtilities.ProjectManagerServiceVersion, Audience = ServiceAudience.Local | ServiceAudience.RemoteExclusiveClient)]
    [ProvideBrokeredService(BrokeredServicesUtilities.ProjectUpgraderServiceName, BrokeredServicesUtilities.ProjectUpgraderServiceVersion, Audience = ServiceAudience.Local | ServiceAudience.RemoteExclusiveClient)]
    [ProvideBrokeredService(BrokeredServicesUtilities.PackageFileServiceName, BrokeredServicesUtilities.PackageFileServiceVersion, Audience = ServiceAudience.Local | ServiceAudience.RemoteExclusiveClient)]
    [ProvideBrokeredService(BrokeredServicesUtilities.SearchServiceName, BrokeredServicesUtilities.SearchServiceVersion, Audience = ServiceAudience.Local | ServiceAudience.RemoteExclusiveClient)]
    [Guid(GuidList.guidNuGetPkgString)]
    public sealed partial class NuGetPackage : AsyncPackage, IVsPackageExtensionProvider, IVsPersistSolutionOpts
    {
        private const string F1KeywordValuePmUI = "VS.NuGet.PackageManager.UI";

        private AsyncLazy<IVsMonitorSelection> _vsMonitorSelection;
        private IVsMonitorSelection VsMonitorSelection => ThreadHelper.JoinableTaskFactory.Run(_vsMonitorSelection.GetValueAsync);
        private readonly ReentrantSemaphore _semaphore = ReentrantSemaphore.Create(1, NuGetUIThreadHelper.JoinableTaskFactory.Context, ReentrantSemaphore.ReentrancyMode.Freeform);

        private DTE _dte;
        private OleMenuCommand _managePackageDialogCommand;
        private OleMenuCommand _managePackageForSolutionDialogCommand;
        private OleMenuCommandService _mcs;

        private uint _solutionExistsAndFullyLoadedContextCookie;
        private uint _solutionNotBuildingAndNotDebuggingContextCookie;
        private uint _solutionExistsCookie;
        private bool _powerConsoleCommandExecuting;
        private bool _initialized;
        private NuGetPowerShellUsageCollector _nuGetPowerShellUsageCollector;

        public NuGetPackage()
        {
            ServiceLocator.InitializePackageServiceProvider(this);
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

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            NuGetVSTelemetryService.Initialize();

            _nuGetPowerShellUsageCollector = new NuGetPowerShellUsageCollector();

            await base.InitializeAsync(cancellationToken, progress);

            // Add our command handlers for menu (commands must exist in the .vsct file)
            await AddMenuCommandHandlersAsync();

            // This instantiates a decoupled ICommand instance responsible to locate and display output pane by a UI control
            UI.Commands.ShowErrorsCommand = new ShowErrorsCommand(this);

            _vsMonitorSelection = new AsyncLazy<IVsMonitorSelection>(
                async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    // get the UI context cookie for the debugging mode
                    var vsMonitorSelection = await this.GetServiceAsync<IVsMonitorSelection, IVsMonitorSelection>();
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

            await NuGetBrokeredServiceFactory.ProfferServicesAsync(this);

            VsShellUtilities.ShutdownToken.Register(InstanceCloseTelemetryEmitter.OnShutdown);

            var componentModel = await this.GetFreeThreadedServiceAsync<SComponentModel, IComponentModel>();
            Assumes.Present(componentModel);
            componentModel.DefaultCompositionService.SatisfyImportsOnce(this);
        }

        /// <summary>
        /// Initialize solution experiences for this package and also add required event handlers.
        /// </summary>
        private async Task InitializeSolutionExperiencesAsync()
        {
            await _semaphore.ExecuteAsync(async () =>
            {
                if (_initialized)
                {
                    return;
                }

                var componentModel = await this.GetFreeThreadedServiceAsync<SComponentModel, IComponentModel>();
                Assumes.Present(componentModel);

                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                SolutionManager.Value.AfterNuGetProjectRenamed += SolutionManager_NuGetProjectRenamed;

                Brushes.LoadVsBrushes(NuGetExperimentationService.Value);

                _dte = await this.GetDTEAsync();
                Assumes.Present(_dte);

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

                IVsTrackProjectRetargeting vsTrackProjectRetargeting = await this.GetServiceAsync<SVsTrackProjectRetargeting, IVsTrackProjectRetargeting>();
                IVsMonitorSelection vsMonitorSelection = await this.GetServiceAsync<IVsMonitorSelection, IVsMonitorSelection>(throwOnFailure: false);
                ProjectRetargetingHandler = new ProjectRetargetingHandler(
                    _dte,
                    SolutionManager.Value,
                    this,
                    componentModel,
                    vsTrackProjectRetargeting,
                    vsMonitorSelection);

                IVsSolution2 vsSolution2 = await this.GetServiceAsync<SVsSolution, IVsSolution2>();
                ProjectUpgradeHandler = new ProjectUpgradeHandler(
                    SolutionManager.Value,
                    vsSolution2);

                SolutionUserOptions.Value.LoadSettings();

                _initialized = true;
            });
        }

        private void SolutionManager_NuGetProjectRenamed(object sender, NuGetProjectEventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var project = await SolutionManager.Value.GetVsProjectAdapterAsync(
                    await SolutionManager.Value.GetNuGetProjectSafeNameAsync(e.NuGetProject));
                var windowFrame = await FindExistingWindowFrameAsync(project.Project);
                if (windowFrame != null)
                {
                    windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_OwnerCaption, string.Format(
                        CultureInfo.CurrentCulture,
                        Resx.Label_NuGetWindowCaption,
                        project.ProjectName));
                }
            });
        }

        private async Task AddMenuCommandHandlersAsync()
        {
            _mcs = await this.GetServiceAsync<IMenuCommandService, OleMenuCommandService>();
            if (null != _mcs)
            {
                // Switch to Main Thread before calling AddCommand which calls GetService() which should
                // always be called on UI thread.
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                // menu command for upgrading packages.config files to PackageReference - References context menu
                var upgradeNuGetProjectCommandID = new CommandID(GuidList.guidNuGetDialogCmdSet, PkgCmdIDList.cmdidUpgradeNuGetProject);
                var upgradeNuGetProjectCommand = new OleMenuCommand(ExecuteUpgradeNuGetProjectCommand, null,
                    BeforeQueryStatusForUpgradeNuGetProject, upgradeNuGetProjectCommandID);
                _mcs.AddCommand(upgradeNuGetProjectCommand);

                // menu command for upgrading packages.config files to PackageReference - packages.config context menu
                var upgradePackagesConfigCommandID = new CommandID(GuidList.guidNuGetDialogCmdSet, PkgCmdIDList.cmdidUpgradePackagesConfig);
                var upgradePackagesConfigCommand = new OleMenuCommand(ExecuteUpgradeNuGetProjectCommand, null,
                    BeforeQueryStatusForUpgradePackagesConfig, upgradePackagesConfigCommandID);
                _mcs.AddCommand(upgradePackagesConfigCommand);

                // menu command for opening Package Manager Console
                var toolwndCommandID = new CommandID(GuidList.guidNuGetConsoleCmdSet, PkgCmdIDList.cmdidPowerConsole);
                var powerConsoleExecuteCommand = new OleMenuCommand(ExecutePowerConsoleCommand, null, BeforeQueryStatusForPowerConsole, toolwndCommandID);
                // '$' - This indicates that the input line other than the argument forms a single argument string with no autocompletion
                //       Autocompletion for filename(s) is supported for option 'p' or 'd' which is not applicable for this command
                powerConsoleExecuteCommand.ParametersDescription = "$";
                _mcs.AddCommand(powerConsoleExecuteCommand);

                // menu command for opening NuGet Debug Console
                // Remove debug console from Tools menu for 3.0.0-beta.
                //CommandID debugWndCommandID = new CommandID(GuidList.guidNuGetDebugConsoleCmdSet, PkgCmdIDList.cmdidDebugConsole);
                //OleMenuCommand debugConsoleExecuteCommand = new OleMenuCommand(ShowDebugConsole, null, null, debugWndCommandID);
                // '$' - This indicates that the input line other than the argument forms a single argument string with no autocompletion
                //       Autocompletion for filename(s) is supported for option 'p' or 'd' which is not applicable for this command
                //debugConsoleExecuteCommand.ParametersDescription = "$";
                //_mcs.AddCommand(debugConsoleExecuteCommand);

                // menu command for opening Manage NuGet packages dialog
                var managePackageDialogCommandID = new CommandID(GuidList.guidNuGetDialogCmdSet, PkgCmdIDList.cmdidAddPackageDialog);
                _managePackageDialogCommand = new OleMenuCommand(ShowManageLibraryPackageDialog, null, BeforeQueryStatusForAddPackageDialog, managePackageDialogCommandID);
                // '$' - This indicates that the input line other than the argument forms a single argument string with no autocompletion
                //       Autocompletion for filename(s) is supported for option 'p' or 'd' which is not applicable for this command
                _managePackageDialogCommand.ParametersDescription = "$";
                _mcs.AddCommand(_managePackageDialogCommand);

                // menu command for opening "Manage NuGet packages for solution" dialog
                var managePackageForSolutionDialogCommandID = new CommandID(GuidList.guidNuGetDialogCmdSet, PkgCmdIDList.cmdidAddPackageDialogForSolution);
                _managePackageForSolutionDialogCommand = new OleMenuCommand(ShowManageLibraryPackageForSolutionDialog, null, BeforeQueryStatusForAddPackageForSolutionDialog, managePackageForSolutionDialogCommandID);
                // '$' - This indicates that the input line other than the argument forms a single argument string with no autocompletion
                //       Autocompletion for filename(s) is supported for option 'p' or 'd' which is not applicable for this command
                _managePackageForSolutionDialogCommand.ParametersDescription = "$";
                _mcs.AddCommand(_managePackageForSolutionDialogCommand);

                // menu command for opening Package Source settings options page
                var settingsCommandID = new CommandID(GuidList.guidNuGetConsoleCmdSet, PkgCmdIDList.cmdidSourceSettings);
                var settingsMenuCommand = new OleMenuCommand(ShowPackageSourcesOptionPage, settingsCommandID);
                _mcs.AddCommand(settingsMenuCommand);

                // menu command for opening General options page
                var generalSettingsCommandID = new CommandID(GuidList.guidNuGetToolsGroupCmdSet, PkgCmdIDList.cmdIdGeneralSettings);
                var generalSettingsCommand = new OleMenuCommand(ShowGeneralSettingsOptionPage, generalSettingsCommandID);
                _mcs.AddCommand(generalSettingsCommand);

                // menu command for the Update option on each package or a selection of packages
                var updatePackageDialogCommandID = new CommandID(GuidList.guidNuGetDialogCmdSet, PkgCmdIDList.cmdidUpdatePackage);
                var updatePackageDialogCommand = new OleMenuCommand(ShowUpdatePackageDialog, changeHandler: null, BeforeQueryStatusForAddPackageDialog, updatePackageDialogCommandID);
                updatePackageDialogCommand.ParametersDescription = "$";
                _mcs.AddCommand(updatePackageDialogCommand);

                // menu command for the Update All option on the Dependencies -> Packages node
                var updatePackagesDialogCommandID = new CommandID(GuidList.guidNuGetDialogCmdSet, PkgCmdIDList.cmdidUpdatePackages);
                var updatePackagesDialogCommand = new OleMenuCommand(ShowUpdatePackagesDialog, changeHandler: null, BeforeQueryStatusForAddPackageDialog, updatePackagesDialogCommandID);
                updatePackageDialogCommand.ParametersDescription = "$";
                _mcs.AddCommand(updatePackagesDialogCommand);
            }
        }

        private void ExecutePowerConsoleCommand(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ShouldInitializeSolutionExperiences())
            {
                NuGetUIThreadHelper.JoinableTaskFactory.Run(InitializeSolutionExperiencesAsync);
            }

            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = FindToolWindow(typeof(PowerConsoleToolWindow), id: 0, create: true);
            if ((null == window)
                || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.CanNotCreateWindow);
            }
            var windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());

            // Parse the arguments to determine the command and arguments to be passed to IHost
            // passed which is of type OleMenuCmdEventArgs
            string command = null;
            var eventArgs = e as OleMenuCmdEventArgs;
            if (eventArgs != null
                && eventArgs.InValue != null)
            {
                command = eventArgs.InValue as string;
            }

            // If the command string is null or empty, simply launch the console and return
            if (!string.IsNullOrEmpty(command))
            {
                var powerConsoleService = (IPowerConsoleService)window;

                if (powerConsoleService.Execute(command, null))
                {
                    _powerConsoleCommandExecuting = true;
                    powerConsoleService.ExecuteEnd += PowerConsoleService_ExecuteEnd;
                }
            }
        }

        private void PowerConsoleService_ExecuteEnd(object sender, EventArgs e)
        {
            _powerConsoleCommandExecuting = false;
        }

        private async Task<IVsWindowFrame> FindExistingWindowFrameAsync(Project project)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            foreach (var windowFrame in VsUtility.GetDocumentWindows(uiShell))
            {
                object docView;
                var hr = windowFrame.GetProperty(
                    (int)__VSFPROPID.VSFPROPID_DocView,
                    out docView);
                if (hr == VSConstants.S_OK
                    && docView is PackageManagerWindowPane)
                {
                    var packageManagerWindowPane = (PackageManagerWindowPane)docView;
                    if (packageManagerWindowPane.Model.IsSolution)
                    {
                        // the window is the solution package manager
                        continue;
                    }

                    var projects = packageManagerWindowPane.Model.Context.Projects;
                    if (projects.Count() != 1)
                    {
                        continue;
                    }

                    IProjectContextInfo existingProject = projects.First();
                    IServiceBroker serviceBroker = await ServiceBrokerProvider.Value.GetAsync();
                    IProjectMetadataContextInfo projectMetadata = await existingProject.GetMetadataAsync(
                        serviceBroker,
                        CancellationToken.None);

                    if (string.Equals(projectMetadata.Name, project.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        return windowFrame;
                    }
                }
            }

            return null;
        }

        private async Task<IVsWindowFrame> CreateNewWindowFrameAsync(Project project)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var vsProject = await project.ToVsHierarchyAsync();
            var documentName = project.FullName;

            // Find existing hierarchy and item id of the document window if it's
            // already registered.
            var rdt = await this.GetServiceAsync<IVsRunningDocumentTable, IVsRunningDocumentTable>();
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
                    documentName,
                    out hier,
                    out itemId,
                    out docData,
                    out cookie);
                if (hr != VSConstants.S_OK)
                {
                    // the docuemnt window is not registered yet. So use the project as the
                    // hierarchy.
                    hier = vsProject;
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

            // Create the doc window using the hiearchy & item id.
            return await CreateDocWindowAsync(project, documentName, hier, itemId);
        }

        private async Task<IVsWindowFrame> CreateDocWindowAsync(
            Project project,
            string documentName,
            IVsHierarchy hier,
            uint itemId)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var windowFlags =
                (uint)_VSRDTFLAGS.RDT_DontAddToMRU |
                (uint)_VSRDTFLAGS.RDT_DontSaveAs;

            if (!await SolutionManager.Value.IsSolutionAvailableAsync())
            {
                throw new InvalidOperationException(Resources.SolutionIsNotSaved);
            }

            var uniqueName = project.GetUniqueName();
            var nugetProject = await SolutionManager.Value.GetNuGetProjectAsync(uniqueName);

            // If we failed to generate a cache entry in the solution manager something went wrong.
            if (nugetProject == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.ProjectHasAnInvalidNuGetConfiguration, project.Name));
            }

            // load packages.config. This makes sure that an exception will get thrown if there
            // are problems with packages.config, such as duplicate packages. When an exception
            // is thrown, an error dialog will pop up and this doc window will not be created.
            _ = await nugetProject.GetInstalledPackagesAsync(CancellationToken.None);

            IServiceBroker serviceBroker = await ServiceBrokerProvider.Value.GetAsync();
            IProjectContextInfo contextInfo = await ProjectContextInfo.CreateAsync(nugetProject, CancellationToken.None);
            INuGetUI uiController = await UIFactory.Value.CreateAsync(serviceBroker, contextInfo);

            // This model takes ownership of --- and Dispose() responsibility for --- the INuGetUI instance.
            var model = new PackageManagerModel(
                uiController,
                isSolution: false,
                editorFactoryGuid: GuidList.guidNuGetEditorType);

            PackageManagerControl control = await PackageManagerControl.CreateAsync(model, OutputConsoleLogger.Value);
            var windowPane = new PackageManagerWindowPane(control);
            var guidEditorType = GuidList.guidNuGetEditorType;
            var guidCommandUI = Guid.Empty;
            var caption = string.Format(
                CultureInfo.CurrentCulture,
                Resx.Label_NuGetWindowCaption,
                project.Name);

            IVsWindowFrame windowFrame;
            var uiShell = await this.GetServiceAsync<SVsUIShell, IVsUIShell>();
            Assumes.Present(uiShell);
            var ppunkDocView = IntPtr.Zero;
            var ppunkDocData = IntPtr.Zero;
            var hr = 0;

            try
            {
                ppunkDocView = Marshal.GetIUnknownForObject(windowPane);
                ppunkDocData = Marshal.GetIUnknownForObject(model);
                hr = uiShell.CreateDocumentWindow(
                    windowFlags,
                    documentName,
                    (IVsUIHierarchy)hier,
                    itemId,
                    ppunkDocView,
                    ppunkDocData,
                    ref guidEditorType,
                    null,
                    ref guidCommandUI,
                    null,
                    caption,
                    string.Empty,
                    null,
                    out windowFrame);
                if (windowFrame != null)
                {
                    WindowFrameHelper.AddF1HelpKeyword(windowFrame, keywordValue: F1KeywordValuePmUI);
                    WindowFrameHelper.DisableWindowAutoReopen(windowFrame);
                }
            }
            finally
            {
                if (ppunkDocView != IntPtr.Zero)
                {
                    Marshal.Release(ppunkDocData);
                }

                if (ppunkDocData != IntPtr.Zero)
                {
                    Marshal.Release(ppunkDocView);
                }
            }

            ErrorHandler.ThrowOnFailure(hr);
            return windowFrame;
        }

        private void ExecuteUpgradeNuGetProjectCommand(object sender, EventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async () =>
            {
                await ExecuteUpgradeNuGetProjectCommandAsync(sender, e);
            })
           .PostOnFailure(nameof(NuGetPackage), nameof(ExecuteUpgradeNuGetProjectCommand));
        }

        private async Task ExecuteUpgradeNuGetProjectCommandAsync(object sender, EventArgs e)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (ShouldInitializeSolutionExperiences())
            {
                await InitializeSolutionExperiencesAsync();
            }

            Project project = VsMonitorSelection.GetActiveProject();

            if (!await NuGetProjectUpgradeUtility.IsNuGetProjectUpgradeableAsync(nuGetProject: null, project))
            {
                MessageHelper.ShowWarningMessage(Resources.ProjectMigrateErrorMessage, Resources.ErrorDialogBoxTitle);
                return;
            }

            string uniqueName = await project.GetCustomUniqueNameAsync();
            // Close NuGet Package Manager if it is open for this project
            IVsWindowFrame windowFrame = await FindExistingWindowFrameAsync(project);
            windowFrame?.CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_SaveIfDirty);

            IServiceBroker serviceBroker = await ServiceBrokerProvider.Value.GetAsync();
            NuGetProject nuGetProject = await SolutionManager.Value.GetNuGetProjectAsync(uniqueName);
            IProjectContextInfo projectContextInfo = await ProjectContextInfo.CreateAsync(nuGetProject, CancellationToken.None);

            using (INuGetUI uiController = await UIFactory.Value.CreateAsync(serviceBroker, projectContextInfo))
            {
                await uiController.UIContext.UIActionEngine.UpgradeNuGetProjectAsync(uiController, projectContextInfo);

                uiController.UIContext.UserSettingsManager.PersistSettings();
            }
        }

        private void ShowManageLibraryPackageDialog(object sender, EventArgs e)
        {
            string parameterString = (e as OleMenuCmdEventArgs)?.InValue as string;
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                await ShowManageLibraryPackageDialogAsync(GetSearchText(parameterString));
            }).PostOnFailure(nameof(NuGetPackage), nameof(ShowManageLibraryPackageDialog));
        }

        private async Task ShowManageLibraryPackageDialogAsync(string searchText, ShowUpdatePackageOptions updatePackageOptions = null)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            if (ShouldInitializeSolutionExperiences())
            {
                await InitializeSolutionExperiencesAsync();
            }

            // *** temp code
            Project project = VsMonitorSelection.GetActiveProject();

            if (project != null
                &&
                !project.IsUnloaded()
                &&
                await EnvDTEProjectUtility.IsSupportedAsync(project))
            {
                IVsWindowFrame windowFrame = await FindExistingWindowFrameAsync(project);
                if (windowFrame == null)
                {
                    windowFrame = await CreateNewWindowFrameAsync(project);
                }

                if (windowFrame != null)
                {
                    ShowUpdatePackages(windowFrame, updatePackageOptions);
                    Search(windowFrame, searchText);
                    windowFrame.Show();
                }
            }
            else
            {
                // show error message when no supported project is selected.
                var projectName = project != null ? project.Name : string.Empty;

                var errorMessage = string.IsNullOrEmpty(projectName)
                    ? Resources.NoProjectSelected
                    : string.Format(CultureInfo.CurrentCulture, Resources.DTE_ProjectUnsupported, projectName);

                MessageHelper.ShowWarningMessage(errorMessage, Resources.ErrorDialogBoxTitle);
            }
        }

        private void ShowUpdatePackageDialog(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var updatePackages = ShowUpdatePackageOptions.UpdatePackages(GetSelectedPackages());

            string parameterString = (e as OleMenuCmdEventArgs)?.InValue as string;
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                await ShowManageLibraryPackageDialogAsync(GetSearchText(parameterString), updatePackages);
            }).PostOnFailure(nameof(NuGetPackage), nameof(ShowUpdatePackageDialog));
        }

        private void ShowUpdatePackagesDialog(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string parameterString = (e as OleMenuCmdEventArgs)?.InValue as string;
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                await ShowManageLibraryPackageDialogAsync(GetSearchText(parameterString), ShowUpdatePackageOptions.UpdateAllPackages());
            }).PostOnFailure(nameof(NuGetPackage), nameof(ShowUpdatePackagesDialog));
        }

        private async Task<IVsWindowFrame> FindExistingSolutionWindowFrameAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var uiShell = await this.GetServiceAsync<SVsUIShell, IVsUIShell>();
            foreach (var windowFrame in VsUtility.GetDocumentWindows(uiShell))
            {
                object property;
                var hr = windowFrame.GetProperty(
                    (int)__VSFPROPID.VSFPROPID_DocData,
                    out property);
                var packageManagerControl = VsUtility.GetPackageManagerControl(windowFrame);
                if (hr == VSConstants.S_OK
                    &&
                    property is IVsSolution
                    &&
                    packageManagerControl != null)
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
        /// Gets the names of the package or packages that are selected in the VsHierarchy.
        /// </summary>
        /// <returns>
        /// A string array containing the list of selected packages in the same hierarchy.
        /// If only one package is selected, the array will contain one package name, otherwise, it will be a list of package names.
        /// If the selected packages are across multiple hierarchies or if no packages are selected, an empty array will be returned.
        /// This method will return <c>null</c> if it fails to retrieve the selected hierarchy.
        /// </returns>
        private string[] GetSelectedPackages()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IntPtr hierarchyPtr = IntPtr.Zero;
            IntPtr selectionContainerPtr = IntPtr.Zero;
            try
            {
                if (!ErrorHandler.Succeeded(VsMonitorSelection.GetCurrentSelection(out hierarchyPtr, out uint itemId, out IVsMultiItemSelect multiItemSelect, out selectionContainerPtr))
                    || (itemId == VSConstants.VSITEMID_NIL && hierarchyPtr != IntPtr.Zero))
                {
                    return null;
                }

                var packages = new List<string>();
                if (itemId != VSConstants.VSITEMID_SELECTION)
                {
                    // This is a single item selection.

                    if (Marshal.GetTypedObjectForIUnknown(hierarchyPtr, typeof(IVsHierarchy)) is IVsHierarchy hierarchy
                        && TryGetPackageNameFromHierarchy(hierarchy, itemId, out string packageName))
                    {
                        packages.Add(packageName);
                    }
                }
                else if (multiItemSelect != null)
                {
                    // This is a multiple item selection.

                    VSITEMSELECTION[] vsItemSelections = multiItemSelect.GetSelectedItemsInSingleHierachy();
                    if (vsItemSelections != null)
                    {
                        foreach (VSITEMSELECTION sel in vsItemSelections)
                        {
                            if (sel.pHier != null && TryGetPackageNameFromHierarchy(sel.pHier, sel.itemid, out string packageName))
                            {
                                packages.Add(packageName);
                            }
                        }
                    }
                }

                return packages.ToArray();
            }
            finally
            {
                if (hierarchyPtr != IntPtr.Zero)
                {
                    Marshal.Release(hierarchyPtr);
                }
                if (selectionContainerPtr != IntPtr.Zero)
                {
                    Marshal.Release(selectionContainerPtr);
                }
            }
        }

        private static bool TryGetPackageNameFromHierarchy(IVsHierarchy hierarchy, uint itemId, out string packageName)
        {
            Assumes.NotNull(hierarchy);
            ThreadHelper.ThrowIfNotOnUIThread();

            int hr = hierarchy.GetProperty(itemId, (int)__VSHPROPID7.VSHPROPID_ProjectTreeCapabilities, out object treeCapabilities);
            packageName = ParsePackageNameFromTreeCapabilities(treeCapabilities as string);
            return hr == VSConstants.S_OK && packageName != null;
        }

        private static string ParsePackageNameFromTreeCapabilities(string treeCapabilities)
        {
            const string packageNamePrefix = "$ID:";

            if (treeCapabilities == null)
            {
                return null;
            }

            string[] capabilities = treeCapabilities.Split(' ');
            string packageName = capabilities.SingleOrDefault(p => p.StartsWith(packageNamePrefix, StringComparison.Ordinal));
            return packageName?.Substring(packageNamePrefix.Length);
        }

        private async Task<IVsWindowFrame> CreateDocWindowForSolutionAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            IVsWindowFrame windowFrame = null;
            var solution = await this.GetServiceAsync<SVsSolution, IVsSolution>();
            var uiShell = await this.GetServiceAsync<SVsUIShell, IVsUIShell>();
            var windowFlags =
                (uint)_VSRDTFLAGS.RDT_DontAddToMRU |
                (uint)_VSRDTFLAGS.RDT_DontSaveAs;

            // when VSSolutionManager is already initialized, then use the existing APIs to check pre-conditions.
            if (!await SolutionManager.Value.IsSolutionAvailableAsync())
            {
                throw new InvalidOperationException(Resources.SolutionIsNotSaved);
            }

            IServiceBroker serviceBroker = await ServiceBrokerProvider.Value.GetAsync();
            IReadOnlyCollection<IProjectContextInfo> projectContexts;

            using (INuGetProjectManagerService projectManagerService = await serviceBroker.GetProxyAsync<INuGetProjectManagerService>(
                NuGetServices.ProjectManagerService))
            {
                Assumes.NotNull(projectManagerService);
                projectContexts = await projectManagerService.GetProjectsAsync(CancellationToken.None);

                if (projectContexts.Count == 0)
                {
                    MessageHelper.ShowWarningMessage(Resources.NoSupportedProjectsInSolution, Resources.ErrorDialogBoxTitle);
                    return null;
                }
            }

            INuGetUI uiController = await UIFactory.Value.CreateAsync(serviceBroker, projectContexts.ToArray());
            var solutionName = (string)_dte.Solution.Properties.Item("Name").Value;

            // This model takes ownership of --- and Dispose() responsibility for --- the INuGetUI instance.
            var model = new PackageManagerModel(
                uiController,
                isSolution: true,
                editorFactoryGuid: GuidList.guidNuGetEditorType)
            {
                SolutionName = solutionName
            };

            PackageManagerControl control = await PackageManagerControl.CreateAsync(model, OutputConsoleLogger.Value);
            var windowPane = new PackageManagerWindowPane(control);
            var guidEditorType = GuidList.guidNuGetEditorType;
            var guidCommandUI = Guid.Empty;
            var caption = Resx.Label_SolutionNuGetWindowCaption;
            var documentName = await SolutionManager.Value.GetSolutionFilePathAsync();

            var ppunkDocView = IntPtr.Zero;
            var ppunkDocData = IntPtr.Zero;
            var hr = 0;

            try
            {
                ppunkDocView = Marshal.GetIUnknownForObject(windowPane);
                ppunkDocData = Marshal.GetIUnknownForObject(model);
                hr = uiShell.CreateDocumentWindow(
                    windowFlags,
                    documentName,
                    (IVsUIHierarchy)solution,
                    (uint)VSConstants.VSITEMID.Root,
                    ppunkDocView,
                    ppunkDocData,
                    ref guidEditorType,
                    null,
                    ref guidCommandUI,
                    null,
                    caption,
                    string.Empty,
                    null,
                    out windowFrame);

                if (windowFrame != null)
                {
                    WindowFrameHelper.AddF1HelpKeyword(windowFrame, keywordValue: F1KeywordValuePmUI);
                    WindowFrameHelper.DisableWindowAutoReopen(windowFrame);
                }
            }
            finally
            {
                if (ppunkDocView != IntPtr.Zero)
                {
                    Marshal.Release(ppunkDocData);
                }

                if (ppunkDocData != IntPtr.Zero)
                {
                    Marshal.Release(ppunkDocView);
                }
            }

            ErrorHandler.ThrowOnFailure(hr);
            return windowFrame;
        }

        private void ShowManageLibraryPackageForSolutionDialog(object sender, EventArgs e)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.RunAsync(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (ShouldInitializeSolutionExperiences())
                {
                    await InitializeSolutionExperiencesAsync();
                }

                var windowFrame = await FindExistingSolutionWindowFrameAsync();
                if (windowFrame == null)
                {
                    // Create the window frame
                    windowFrame = await CreateDocWindowForSolutionAsync();
                }

                if (windowFrame != null)
                {
                    // process search string
                    string parameterString = null;
                    var args = e as OleMenuCmdEventArgs;
                    if (args != null)
                    {
                        parameterString = args.InValue as string;
                    }
                    var searchText = GetSearchText(parameterString);
                    Search(windowFrame, searchText);

                    windowFrame.Show();
                }
            }).PostOnFailure(nameof(NuGetPackage), nameof(ShowManageLibraryPackageForSolutionDialog));
        }

        /// <summary>
        /// Search for packages using the searchText.
        /// </summary>
        /// <param name="windowFrame">A window frame that hosts the PackageManagerControl.</param>
        /// <param name="searchText">Search text.</param>
        private void Search(IVsWindowFrame windowFrame, string searchText)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return;
            }

            var packageManagerControl = VsUtility.GetPackageManagerControl(windowFrame);
            if (packageManagerControl != null)
            {
                if (packageManagerControl.ActiveFilter != UI.ItemFilter.All)
                {
                    packageManagerControl.ActiveFilter = UI.ItemFilter.All;
                }
                packageManagerControl.Search(searchText);
            }
        }

        private void ShowUpdatePackages(IVsWindowFrame windowFrame, ShowUpdatePackageOptions updatePackageOptions)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (updatePackageOptions == null)
            {
                return;
            }

            var packageManagerControl = VsUtility.GetPackageManagerControl(windowFrame);
            packageManagerControl?.ShowUpdatePackages(updatePackageOptions);
        }

        // For PowerShell, it's okay to query from the worker thread.
        private void BeforeQueryStatusForPowerConsole(object sender, EventArgs args)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (ShouldInitializeSolutionExperiences())
            {
                NuGetUIThreadHelper.JoinableTaskFactory.Run(InitializeSolutionExperiencesAsync);
            }

            var isConsoleBusy = false;
            if (ConsoleStatus != null)
            {
                isConsoleBusy = ConsoleStatus.Value.IsBusy;
            }

            var command = (OleMenuCommand)sender;
            command.Enabled = !isConsoleBusy && !_powerConsoleCommandExecuting;
        }

        private void BeforeQueryStatusForUpgradeNuGetProject(object sender, EventArgs args)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                if (ShouldInitializeSolutionExperiences())
                {
                    await InitializeSolutionExperiencesAsync();
                }

                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var command = (OleMenuCommand)sender;

                var isConsoleBusy = false;
                if (ConsoleStatus != null)
                {
                    isConsoleBusy = ConsoleStatus.Value.IsBusy;
                }

                command.Visible = GetIsSolutionOpen() && await IsPackagesConfigBasedProjectAsync();
                command.Enabled = !isConsoleBusy && IsSolutionExistsAndNotDebuggingAndNotBuilding() && await HasActiveLoadedSupportedProjectAsync();
            });
        }

        private void BeforeQueryStatusForUpgradePackagesConfig(object sender, EventArgs args)
        {
            // Check whether to show context menu item on packages.config
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                if (ShouldInitializeSolutionExperiences())
                {
                    await InitializeSolutionExperiencesAsync();
                }

                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var command = (OleMenuCommand)sender;

                var isConsoleBusy = false;
                if (ConsoleStatus != null)
                {
                    isConsoleBusy = ConsoleStatus.Value.IsBusy;
                }

                command.Visible = GetIsSolutionOpen() && IsPackagesConfigSelected();
                command.Enabled = !isConsoleBusy && IsSolutionExistsAndNotDebuggingAndNotBuilding() && await HasActiveLoadedSupportedProjectAsync();
            });
        }

        private async Task<bool> IsPackagesConfigBasedProjectAsync()
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dteProject = VsMonitorSelection.GetActiveProject();

            var uniqueName = dteProject.GetUniqueName();
            var nuGetProject = await SolutionManager.Value.GetNuGetProjectAsync(uniqueName);

            if (nuGetProject == null)
            {
                return false;
            }

            var msBuildNuGetProject = nuGetProject as MSBuildNuGetProject;

            if (msBuildNuGetProject == null || !msBuildNuGetProject.PackagesConfigNuGetProject.PackagesConfigExists())
            {
                return false;
            }

            return true;
        }

        private bool GetIsSolutionOpen()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return _dte?.Solution != null && _dte.Solution.IsOpen;
        }

        private bool IsPackagesConfigSelected()
        {
            return NuGetProjectUpgradeUtility.IsPackagesConfigSelected(VsMonitorSelection);
        }

        private void BeforeQueryStatusForAddPackageDialog(object sender, EventArgs args)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                if (ShouldInitializeSolutionExperiences())
                {
                    await InitializeSolutionExperiencesAsync();
                }

                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var command = (OleMenuCommand)sender;

                // Keep the 'Manage NuGet Packages' visible, only if a solution is open. Following is why.
                // When all menu commands in the 'Project' menu are invisible, when a solution is closed, Project menu goes away.
                // This is actually true. All the menu commands under the 'Project Menu' do go away when no solution is open.
                // If 'Manage NuGet Packages' is disabled but visible, 'Project' menu shows up just because 1 menu command is visible, even though, it is disabled
                // So, make it invisible when no solution is open
                command.Visible = GetIsSolutionOpen();

                var isConsoleBusy = false;
                if (ConsoleStatus != null)
                {
                    isConsoleBusy = ConsoleStatus.Value.IsBusy;
                }
                // Enable the 'Manage NuGet Packages' dialog menu
                // - if the solution exists and not debugging and not building AND
                // - if the console is NOT busy executing a command, AND
                // - if the active project is loaded and supported
                command.Enabled =
                    IsSolutionExistsAndNotDebuggingAndNotBuilding() &&
                    !isConsoleBusy &&
                    await HasActiveLoadedSupportedProjectAsync();
            });
        }

        private void BeforeQueryStatusForAddPackageForSolutionDialog(object sender, EventArgs args)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                if (ShouldInitializeSolutionExperiences())
                {
                    await InitializeSolutionExperiencesAsync();
                }

                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var command = (OleMenuCommand)sender;

                var isConsoleBusy = false;
                if (ConsoleStatus != null)
                {
                    isConsoleBusy = ConsoleStatus.Value.IsBusy;
                }
                // Enable the 'Manage NuGet Packages For Solution' dialog menu
                // - if the console is NOT busy executing a command, AND
                // - if the solution exists and not debugging and not building AND
                // - if there are NuGetProjects. This means there are loaded, supported projects.
                command.Enabled =
                    IsSolutionExistsAndNotDebuggingAndNotBuilding() &&
                    !isConsoleBusy &&
                    await SolutionManager.Value.DoesNuGetSupportsAnyProjectAsync();
            });
        }

        private bool IsSolutionExistsAndNotDebuggingAndNotBuilding()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var hr = VsMonitorSelection.IsCmdUIContextActive(
                _solutionExistsAndFullyLoadedContextCookie, out var pfActive);

            if (ErrorHandler.Succeeded(hr) && pfActive > 0)
            {
                hr = VsMonitorSelection.IsCmdUIContextActive(
                    _solutionNotBuildingAndNotDebuggingContextCookie, out pfActive);

                return ErrorHandler.Succeeded(hr) && pfActive > 0;
            }

            return false;
        }

        private void ShowPackageSourcesOptionPage(object sender, EventArgs args)
        {
            ShowOptionPageSafe(typeof(PackageSourceOptionsPage));
        }

        private void ShowGeneralSettingsOptionPage(object sender, EventArgs args)
        {
            ShowOptionPageSafe(typeof(GeneralOptionPage));
        }

        private void ShowOptionPageSafe(Type optionPageType)
        {
            try
            {
                ShowOptionPage(optionPageType);
            }
            catch (Exception exception)
            {
                MessageHelper.ShowErrorMessage(exception, Resources.ErrorDialogBoxTitle);
                ExceptionHelper.WriteErrorToActivityLog(exception);
            }
        }

        /// <summary>
        /// Gets whether the current IDE has an active, supported and non-unloaded project, which is a precondition for
        /// showing the Add Library Package Reference dialog
        /// </summary>
        private async Task<bool> HasActiveLoadedSupportedProjectAsync()
        {
            var project = VsMonitorSelection.GetActiveProject();
            return project != null && !project.IsUnloaded()
                   && await EnvDTEProjectUtility.IsSupportedAsync(project);
        }

        private bool ShouldInitializeSolutionExperiences()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!_initialized)
            {
                var hr = VsMonitorSelection.IsCmdUIContextActive(
                _solutionExistsCookie, out var pfActive);

                return ErrorHandler.Succeeded(hr) && pfActive > 0;
            }

            return false;
        }

        #region IVsPackageExtensionProvider implementation

        public dynamic CreateExtensionInstance(ref Guid extensionPoint, ref Guid instance)
        {
            if (instance == typeof(NuGetSearchProvider).GUID)
            {
                return new NuGetSearchProvider(_mcs, _managePackageDialogCommand, _managePackageForSolutionDialogCommand);
            }
            return null;
        }

        #endregion IVsPackageExtensionProvider implementation

        #region IVsPersistSolutionOpts

        // Called by the shell when a solution is opened and the SUO file is read.
        public int LoadUserOptions(IVsSolutionPersistence pPersistence, uint grfLoadOpts)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (ShouldInitializeSolutionExperiences())
            {
                NuGetUIThreadHelper.JoinableTaskFactory.Run(InitializeSolutionExperiencesAsync);
            }

            return SolutionUserOptions.Value.LoadUserOptions(pPersistence, grfLoadOpts);
        }

        public int ReadUserOptions(IStream _, string __)
        {
            // no package specific streams to read
            return VSConstants.S_OK;
        }

        // Called by the shell when the SUO file is saved. The provider calls the shell back to let it
        // know which options keys it will use in the suo file.
        public int SaveUserOptions(IVsSolutionPersistence pPersistence)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (SolutionUserOptions != null && SolutionUserOptions.IsValueCreated)
            {
                return SolutionUserOptions.Value.SaveUserOptions(pPersistence);
            }

            return VSConstants.S_OK;
        }

        public int WriteUserOptions(IStream _, string __)
        {
            // no package specific streams to write
            return VSConstants.S_OK;
        }

        #endregion IVsPersistSolutionOpts

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _mcs?.Dispose();
                    _nuGetPowerShellUsageCollector?.Dispose();
                    _semaphore.Dispose();
                    ProjectRetargetingHandler?.Dispose();
                    ProjectUpgradeHandler?.Dispose();

                    GC.SuppressFinalize(this);
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
