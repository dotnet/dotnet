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
    private readonly IProjectCapabilityResolver _projectCapabilityResolver;
    private readonly JoinableTaskFactory _jtf;
    private readonly RazorActivityLog _activityLog;
    private readonly CancellationTokenSource _disposeTokenSource;
    private readonly AsyncLazy<bool> _lazyLegacyEditorEnabled;

    [ImportingConstructor]
    public LspEditorFeatureDetector(
        IVsService<SVsSettingsPersistenceManager, ISettingsManager> vsSettingsManagerService,
        IUIContextService uiContextService,
        IProjectCapabilityResolver projectCapabilityResolver,
        JoinableTaskContext joinableTaskContext,
        RazorActivityLog activityLog)
    {
        _uiContextService = uiContextService;
        _projectCapabilityResolver = projectCapabilityResolver;
        _jtf = joinableTaskContext.Factory;
        _activityLog = activityLog;

        _disposeTokenSource = new();

        _lazyLegacyEditorEnabled = new(() =>
             ComputeUseLegacyEditorAsync(vsSettingsManagerService, activityLog, _disposeTokenSource.Token),
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
        IVsService<SVsSettingsPersistenceManager, ISettingsManager> vsSettingsManagerService,
        RazorActivityLog activityLog,
        CancellationToken cancellationToken)
    {
        var settingsManager = await vsSettingsManagerService.GetValueAsync(cancellationToken).ConfigureAwaitRunInline();
        var useLegacyEditorSetting = settingsManager.GetValueOrDefault<bool>(WellKnownSettingNames.UseLegacyASPNETCoreEditor);

        if (useLegacyEditorSetting)
        {
            activityLog.LogInfo($"Using legacy editor because the '{WellKnownSettingNames.UseLegacyASPNETCoreEditor}' setting is set to true.");
            return true;
        }

        activityLog.LogInfo($"Using LSP editor.");
        return false;
    }

    /// <inheritdoc/>
    public bool IsLspEditorEnabled()
    {
        // This method is first called by our IFilePathToContentTypeProvider.TryGetContentTypeForFilePath(...) implementations.
        // We call AsyncLazy<T>.GetValue() below to get the value. If the work hasn't yet completed, we guard against a hidden
        // JTF.Run(...) on a background thread by asserting the UI thread.

        if (!_lazyLegacyEditorEnabled.IsValueFactoryCompleted)
        {
#pragma warning disable VSTHRD108 // Assert thread affinity unconditionally
            _jtf.AssertUIThread();
#pragma warning restore VSTHRD108 // Assert thread affinity unconditionally
        }

        var useLegacyEditorEnabled = _lazyLegacyEditorEnabled.GetValue(_disposeTokenSource.Token);

        if (useLegacyEditorEnabled)
        {
            _activityLog.LogInfo("Using legacy editor because the option was set to true");
            return false;
        }

        _activityLog.LogInfo("LSP editor is enabled.");
        return true;
    }

    /// <inheritdoc/>
    public bool IsLspEditorSupported(string documentFilePath)
    {
        // Regardless of whether the LSP is enabled via tools/options, the document's project
        // might not support it. For example, .NET Framework projects don't support the LSP Razor editor.

        var useLegacyEditor = _projectCapabilityResolver.ResolveCapability(WellKnownProjectCapabilities.LegacyRazorEditor, documentFilePath);

        if (useLegacyEditor)
        {
            _activityLog.LogInfo($"'{documentFilePath}' does not support the LSP editor because it is associated with the '{WellKnownProjectCapabilities.LegacyRazorEditor}' capability.");
            return false;
        }

        var supportsRazor = _projectCapabilityResolver.ResolveCapability(WellKnownProjectCapabilities.DotNetCoreCSharp, documentFilePath);

        if (!supportsRazor)
        {
            _activityLog.LogInfo($"'{documentFilePath}' does not support the LSP editor because it is not associated with the '{WellKnownProjectCapabilities.DotNetCoreCSharp}' capability.");
            return false;
        }

        _activityLog.LogInfo($"LSP editor is supported for '{documentFilePath}'.");
        return supportsRazor;
    }

    public bool IsRemoteClient()
        => _uiContextService.IsActive(Guids.LiveShareGuestUIContextGuid) ||
           _uiContextService.IsActive(VSConstants.UICONTEXT.CloudEnvironmentConnected_guid);

    public bool IsLiveShareHost()
        => _uiContextService.IsActive(Guids.LiveShareHostUIContextGuid);
}
