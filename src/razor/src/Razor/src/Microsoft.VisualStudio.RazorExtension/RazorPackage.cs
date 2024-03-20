// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Editor.Razor.Debugging;
using Microsoft.VisualStudio.Editor.Razor.Logging;
using Microsoft.VisualStudio.Editor.Razor.Snippets;
using Microsoft.VisualStudio.LanguageServerClient.Razor.Options;
using Microsoft.VisualStudio.LanguageServices.Razor;
using Microsoft.VisualStudio.RazorExtension.Options;
using Microsoft.VisualStudio.RazorExtension.Snippets;
using Microsoft.VisualStudio.RazorExtension.SyntaxVisualizer;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.ServiceBroker;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.VisualStudio.RazorExtension;

[PackageRegistration(UseManagedResourcesOnly = true)]
[AboutDialogInfo(PackageGuidString, "Razor (ASP.NET Core)", "#110", "#112", IconResourceID = "#400")]
[ProvideService(typeof(RazorLanguageService))]
[ProvideLanguageService(typeof(RazorLanguageService), RazorConstants.RazorLSPContentTypeName, 110)]
[ProvideBrokeredServiceHubService("Microsoft.VisualStudio.Razor.ClientInitialization", Audience = ServiceAudience.Local)]
[ProvideBrokeredServiceHubService("Microsoft.VisualStudio.Razor.ClientInitialization64", Audience = ServiceAudience.Local)]
[ProvideBrokeredServiceHubService("Microsoft.VisualStudio.Razor.ClientInitialization64S", Audience = ServiceAudience.Local)]
[ProvideBrokeredServiceHubService("Microsoft.VisualStudio.Razor.ClientInitializationCore64", ServiceLocation = ProvideBrokeredServiceHubServiceAttribute.DefaultServiceLocation + @"\ServiceHubCore", Audience = ServiceAudience.Local)]
[ProvideBrokeredServiceHubService("Microsoft.VisualStudio.Razor.ClientInitializationCore64S", ServiceLocation = ProvideBrokeredServiceHubServiceAttribute.DefaultServiceLocation + @"\ServiceHubCore", Audience = ServiceAudience.Local)]
[ProvideBrokeredServiceHubService("Microsoft.VisualStudio.Razor.TagHelperProvider", Audience = ServiceAudience.Local)]
[ProvideBrokeredServiceHubService("Microsoft.VisualStudio.Razor.TagHelperProvider64", Audience = ServiceAudience.Local)]
[ProvideBrokeredServiceHubService("Microsoft.VisualStudio.Razor.TagHelperProvider64S", Audience = ServiceAudience.Local)]
[ProvideBrokeredServiceHubService("Microsoft.VisualStudio.Razor.TagHelperProviderCore64", ServiceLocation = ProvideBrokeredServiceHubServiceAttribute.DefaultServiceLocation + @"\ServiceHubCore", Audience = ServiceAudience.Local)]
[ProvideBrokeredServiceHubService("Microsoft.VisualStudio.Razor.TagHelperProviderCore64S", ServiceLocation = ProvideBrokeredServiceHubServiceAttribute.DefaultServiceLocation + @"\ServiceHubCore", Audience = ServiceAudience.Local)]
[ProvideMenuResource("Menus.ctmenu", 1)]
[ProvideMenuResource("SyntaxVisualizerMenu.ctmenu", 1)]
[ProvideToolWindow(typeof(SyntaxVisualizerToolWindow))]
[ProvideLanguageEditorOptionPage(typeof(AdvancedOptionPage), RazorConstants.RazorLSPContentTypeName, category: null, "Advanced", pageNameResourceId: "#1050", keywordListResourceId: 1060)]
[Guid(PackageGuidString)]
internal sealed class RazorPackage : AsyncPackage
{
    public const string PackageGuidString = "13b72f58-279e-49e0-a56d-296be02f0805";

    internal const string GuidSyntaxVisualizerMenuCmdSetString = "a3a603a2-2b17-4ce2-bd21-cbb8ccc084ec";
    internal static readonly Guid GuidSyntaxVisualizerMenuCmdSet = new Guid(GuidSyntaxVisualizerMenuCmdSetString);
    internal const uint CmdIDRazorSyntaxVisualizer = 0x101;

    private OptionsStorage? _optionsStorage = null;

    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        var container = this as IServiceContainer;
        container.AddService(typeof(RazorLanguageService), (container, type) =>
        {
            var componentModel = (IComponentModel)GetGlobalService(typeof(SComponentModel));
            var breakpointResolver = componentModel.GetService<RazorBreakpointResolver>();
            var proximityExpressionResolver = componentModel.GetService<RazorProximityExpressionResolver>();
            var uiThreadOperationExecutor = componentModel.GetService<IUIThreadOperationExecutor>();
            var editorAdaptersFactory = componentModel.GetService<IVsEditorAdaptersFactoryService>();
            var lspServerActivationTracker = componentModel.GetService<ILspServerActivationTracker>();
            var joinableTaskContext = componentModel.GetService<JoinableTaskContext>();

            return new RazorLanguageService(breakpointResolver, proximityExpressionResolver, lspServerActivationTracker, uiThreadOperationExecutor, editorAdaptersFactory, joinableTaskContext.Factory);
        }, promote: true);

        // Add our command handlers for menu (commands must exist in the .vsct file).
        if (await GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService mcs)
        {
            // Create the command for the tool window.
            var toolwndCommandID = new CommandID(GuidSyntaxVisualizerMenuCmdSet, (int)CmdIDRazorSyntaxVisualizer);
            var menuToolWin = new MenuCommand(ShowToolWindow, toolwndCommandID);
            mcs.AddCommand(menuToolWin);
        }

        var componentModel = (IComponentModel)GetGlobalService(typeof(SComponentModel));
        _optionsStorage = componentModel.GetService<OptionsStorage>();
        CreateSnippetService(componentModel);

        // LogHub can be initialized off the UI thread
        await TaskScheduler.Default;

        var traceProvider = componentModel.GetService<RazorLogHubTraceProvider>();
        await traceProvider.InitializeTraceAsync("Razor", 1, cancellationToken);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _optionsStorage?.Dispose();
        _optionsStorage = null;
    }

    private SnippetService CreateSnippetService(IComponentModel componentModel)
    {
        var joinableTaskContext = componentModel.GetService<JoinableTaskContext>();
        var cache = componentModel.GetService<SnippetCache>();
        return new SnippetService(joinableTaskContext.Factory, this, cache, _optionsStorage.AssumeNotNull());
    }

    /// <summary>
    /// This function is called when the user clicks the menu item that shows the
    /// tool window. See the Initialize method to see how the menu item is associated to
    /// this function using the OleMenuCommandService service and the MenuCommand class.
    /// </summary>
    private void ShowToolWindow(object sender, EventArgs e)
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        // Get the instance number 0 of this tool window. This window is single instance so this instance
        // is actually the only one. The last flag is set to true so that if the tool window does not exist
        // it will be created.
        var window = (SyntaxVisualizerToolWindow)FindToolWindow(typeof(SyntaxVisualizerToolWindow), id: 0, create: true);
        if (window?.Frame is not IVsWindowFrame windowFrame)
        {
            throw new NotSupportedException("Can not create window");
        }

        // Initialize command handlers in the window
        if (!window.CommandHandlersInitialized)
        {
            var mcs = (IMenuCommandService)GetService(typeof(IMenuCommandService));
            window.InitializeCommands(mcs, GuidSyntaxVisualizerMenuCmdSet);
        }

        ErrorHandler.ThrowOnFailure(windowFrame.Show());
    }
}
