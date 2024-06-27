// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Razor.Extensions;
using Microsoft.VisualStudio.Razor.Logging;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor;

[Export(typeof(ILspEditorFeatureDetector))]
internal sealed class LspEditorFeatureDetector : ILspEditorFeatureDetector, IDisposable
{
    internal const string DotNetCoreCSharpCapability = "CSharp&CPS";
    internal const string LegacyRazorEditorCapability = "LegacyRazorEditor";

    private readonly IUIContextService _uiContextService;
    private readonly JoinableTaskFactory _jtf;
    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly IAggregateProjectCapabilityResolver _projectCapabilityResolver;
    private readonly Lazy<IVsUIShellOpenDocument> _vsUIShellOpenDocument;
    private readonly AsyncLazy<bool> _lazyUseLegacyEditorTask;
    private readonly RazorActivityLog _activityLog;

    [ImportingConstructor]
    public LspEditorFeatureDetector(
        IAggregateProjectCapabilityResolver projectCapabilityResolver,
        IVsService<SVsFeatureFlags, IVsFeatureFlags> vsFeatureFlagsService,
        IVsService<SVsSettingsPersistenceManager, ISettingsManager> vsSettingsManagerService,
        IUIContextService uiContextService,
        JoinableTaskContext joinableTaskContext,
        RazorActivityLog activityLog)
        : this(
              projectCapabilityResolver,
              vsFeatureFlagsService,
              vsSettingsManagerService,
              uiContextService,
              joinableTaskContext,
              activityLog,
              new Lazy<IVsUIShellOpenDocument>(() =>
              {
                // This method is first called by out IFilePathToContentTypeProvider.TryGetContentTypeForFilePath(...) implementations on UI thread.
                ThreadHelper.ThrowIfNotOnUIThread();
                var shellOpenDocument = (IVsUIShellOpenDocument)ServiceProvider.GlobalProvider.GetService(typeof(SVsUIShellOpenDocument));
                Assumes.Present(shellOpenDocument);

                return shellOpenDocument;
              })
        )
    { }

    // Primarily for unit tests
    public LspEditorFeatureDetector(
        IAggregateProjectCapabilityResolver projectCapabilityResolver,
        IVsService<SVsFeatureFlags, IVsFeatureFlags> vsFeatureFlagsService,
        IVsService<SVsSettingsPersistenceManager, ISettingsManager> vsSettingsManagerService,
        IUIContextService uiContextService,
        JoinableTaskContext joinableTaskContext,
        RazorActivityLog activityLog,
        Lazy<IVsUIShellOpenDocument> vsUIShellOpenDocument)
    {
        _uiContextService = uiContextService;
        _jtf = joinableTaskContext.Factory;
        _disposeTokenSource = new();

        _projectCapabilityResolver = projectCapabilityResolver;
        _vsUIShellOpenDocument = vsUIShellOpenDocument;

        _lazyUseLegacyEditorTask = new(() =>
             ComputeUseLegacyEditorAsync(vsFeatureFlagsService, vsSettingsManagerService, activityLog, _disposeTokenSource.Token),
             _jtf);
        _activityLog = activityLog;
    }

    public void Dispose()
    {
        if (_disposeTokenSource.IsCancellationRequested)
        {
            return;
        }

        _disposeTokenSource.Cancel();
        _disposeTokenSource.Dispose();
    }

    private static async Task<bool> ComputeUseLegacyEditorAsync(
        IVsService<SVsFeatureFlags, IVsFeatureFlags> vsFeatureFlagsService,
        IVsService<SVsSettingsPersistenceManager, ISettingsManager> vsSettingsManagerService,
        RazorActivityLog activityLog,
        CancellationToken cancellationToken)
    {
        var vsFeatureFlags = await vsFeatureFlagsService.GetValueAsync(cancellationToken).ConfigureAwaitRunInline();

        // IVsFeatureFlags is free-threaded but VSTHRD010 seems to be reported anyway.
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        if (vsFeatureFlags.IsFeatureEnabled(WellKnownFeatureFlagNames.UseLegacyRazorEditor, defaultValue: false))
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        {
            activityLog.LogInfo($"Using Legacy Razor editor because the '{WellKnownFeatureFlagNames.UseLegacyRazorEditor}' feature flag is enabled.");
            return true;
        }

        var settingsManager = await vsSettingsManagerService.GetValueAsync(cancellationToken).ConfigureAwaitRunInline();
        var useLegacyEditorSetting = settingsManager.GetValueOrDefault<bool>(WellKnownSettingNames.UseLegacyASPNETCoreEditor);

        if (useLegacyEditorSetting)
        {
            activityLog.LogInfo($"Using Legacy Razor editor because the '{WellKnownSettingNames.UseLegacyASPNETCoreEditor}' setting is set to true.");
            return true;
        }

        activityLog.LogInfo($"Using LSP Razor editor.");
        return false;
    }

    /// <summary>
    /// Checks that LSP editor is enabled via feature flag and tools/options setting, and available for document's project
    /// </summary>
    /// <param name="documentMoniker">Document to check for project compatibility with LSP</param>
    /// <returns>true if LSP editor is enabled and available for document's project</returns>
    public bool IsLspEditorEnabledAndAvailable(string documentMoniker)
    {
        // This method is first called by out IFilePathToContentTypeProvider.TryGetContentTypeForFilePath(...) implementations.
        // We call AsyncLazy<T>.GetValue() below to get the value. If the work hasn't yet completed, we guard against a hidden+
        // JTF.Run(...) on a background thread by asserting the UI thread.

        if (!_lazyUseLegacyEditorTask.IsValueFactoryCompleted)
        {
            _jtf.AssertUIThread();
        }
        
        var isLspEditorEnabled = !_lazyUseLegacyEditorTask.GetValue(_disposeTokenSource.Token);

        if (!isLspEditorEnabled)
        {
            _activityLog.LogInfo("Using Legacy editor because the option or feature flag was set to true");
            return false;
        }

        // Even if LSP editor is enabled via feature flag and tools/options, document's project might not support
        // LSP editor (e.g. .Net Framework projects don't support LSP Razor editor)
        if (!ProjectSupportsLspEditor(documentMoniker))
        {
            // Current project hierarchy doesn't support the LSP Razor editor
            _activityLog.LogInfo("Using Legacy editor because the current project does not support LSP Editor");
            return false;
        }

        _activityLog.LogInfo("LSP Editor is enabled and available");
        return true;
    }

    // NOTE: This code is needed for legacy Razor editor support in .Net Framework projects. Do not delete unless support for .Net Framework projects is discontinued.
    private bool ProjectSupportsLspEditor(string documentMoniker)
    {
        var hr = _vsUIShellOpenDocument.Value.IsDocumentInAProject(documentMoniker, out var uiHierarchy, out _, out _, out _);
        var hierarchy = uiHierarchy as IVsHierarchy;
        if (!ErrorHandler.Succeeded(hr))
        {
            _activityLog.LogWarning($"Project does not support LSP Editor because {nameof(_vsUIShellOpenDocument.Value.IsDocumentInAProject)} failed with exit code {hr}");
            return false;
        }
        
        if (hierarchy is null)
        {
            _activityLog.LogWarning($"Project does not support LSP Editor because {nameof(hierarchy)} is null");
            return false;
        }

        // We allow projects to specifically opt-out of the legacy Razor editor because there are legacy scenarios which would rely on behind-the-scenes
        // opt-out mechanics to enable the .NET Core editor in non-.NET Core scenarios. Therefore, we need a similar mechanic to continue supporting
        // those types of scenarios for the new .NET Core Razor editor.
        if (_projectCapabilityResolver.HasCapability(documentMoniker, hierarchy, LegacyRazorEditorCapability))
        {
            _activityLog.LogInfo($"Project does not support LSP Editor because '{documentMoniker}' has Capability {LegacyRazorEditorCapability}");
            // CPS project that requires the legacy editor
            return false;
        }

        if (_projectCapabilityResolver.HasCapability(documentMoniker, hierarchy, DotNetCoreCSharpCapability))
        {
            // .NET Core project that supports C#
            return true;
        }

        _activityLog.LogInfo($"Project {documentMoniker} does not support LSP Editor because it does not have the {DotNetCoreCSharpCapability} capability.");
        // Not a C# .NET Core project. This typically happens for legacy Razor scenarios
        return false;
    }

    public bool IsRemoteClient()
        => _uiContextService.IsActive(Guids.LiveShareGuestUIContextGuid) ||
           _uiContextService.IsActive(VSConstants.UICONTEXT.CloudEnvironmentConnected_guid);

    public bool IsLiveShareHost()
        => _uiContextService.IsActive(Guids.LiveShareHostUIContextGuid);
}
