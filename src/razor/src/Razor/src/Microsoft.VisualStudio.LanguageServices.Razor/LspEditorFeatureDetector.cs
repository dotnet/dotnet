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
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Razor;

[Export(typeof(ILspEditorFeatureDetector))]
internal sealed class LspEditorFeatureDetector : ILspEditorFeatureDetector, IDisposable
{
    private readonly IUIContextService _uiContextService;
    private readonly JoinableTaskFactory _jtf;
    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncLazy<bool> _lazyUseLegacyEditorTask;

    [ImportingConstructor]
    public LspEditorFeatureDetector(
        IVsService<SVsFeatureFlags, IVsFeatureFlags> vsFeatureFlagsService,
        IVsService<SVsSettingsPersistenceManager, ISettingsManager> vsSettingsManagerService,
        IUIContextService uiContextService,
        JoinableTaskContext joinableTaskContext,
        RazorActivityLog activityLog)
    {
        _uiContextService = uiContextService;
        _jtf = joinableTaskContext.Factory;
        _disposeTokenSource = new();

        _lazyUseLegacyEditorTask = new(() =>
             ComputeUseLegacyEditorAsync(vsFeatureFlagsService, vsSettingsManagerService, activityLog, _disposeTokenSource.Token),
             _jtf);
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

    public bool IsLspEditorEnabled()
    {
        // This method is first called by out IFilePathToContentTypeProvider.TryGetContentTypeForFilePath(...) implementations.
        // We call AsyncLazy<T>.GetValue() below to get the value. If the work hasn't yet completed, we guard against a hidden+
        // JTF.Run(...) on a background thread by asserting the UI thread.

        if (!_lazyUseLegacyEditorTask.IsValueFactoryCompleted)
        {
            _jtf.AssertUIThread();
        }

        return !_lazyUseLegacyEditorTask.GetValue(_disposeTokenSource.Token);
    }

    public bool IsRemoteClient()
        => _uiContextService.IsActive(Guids.LiveShareGuestUIContextGuid) ||
           _uiContextService.IsActive(VSConstants.UICONTEXT.CloudEnvironmentConnected_guid);

    public bool IsLiveShareHost()
        => _uiContextService.IsActive(Guids.LiveShareHostUIContextGuid);
}
