// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Manages VS InfoBars displayed at the top of a window frame that exposes an <see cref="IVsInfoBarHost"/>.
    /// This is a generic, keyed host/registry: each notification scenario owns a key and supplies the
    /// <see cref="InfoBarModel"/> plus per-key callbacks for action clicks and user close. All scenario
    /// policy (when to show/hide, message text, suppression) lives in the calling scenario, not here.
    /// </summary>
    internal sealed class PackageManagerInfoBarService : IVsInfoBarUIEvents, IDisposable
    {
        private readonly IVsInfoBarHost _infoBarHost;
        private readonly IVsInfoBarUIFactory _infoBarFactory;
        private readonly Dictionary<string, InfoBarEntry> _activeInfoBars = new();
        private bool _disposed;

        private PackageManagerInfoBarService(IVsInfoBarHost infoBarHost, IVsInfoBarUIFactory infoBarFactory)
        {
            _infoBarHost = infoBarHost;
            _infoBarFactory = infoBarFactory;
        }

        /// <summary>
        /// Creates a <see cref="PackageManagerInfoBarService"/> for the given window frame, or returns null if the frame doesn't support InfoBars.
        /// </summary>
        public static PackageManagerInfoBarService? TryCreate(IVsWindowFrame windowFrame, IVsInfoBarUIFactory infoBarFactory)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (windowFrame == null || infoBarFactory == null)
            {
                return null;
            }

            int hr = windowFrame.GetProperty((int)__VSFPROPID7.VSFPROPID_InfoBarHost, out object hostObj);
            if (ErrorHandler.Failed(hr) || hostObj is not IVsInfoBarHost infoBarHost)
            {
                return null;
            }

            return new PackageManagerInfoBarService(infoBarHost, infoBarFactory);
        }

        /// <summary>
        /// Shows an InfoBar with the given key. If an InfoBar with that key is already visible, this is a no-op
        /// (use <see cref="UpdateInfoBar"/> to change its content).
        /// </summary>
        /// <param name="key">Stable identifier for the scenario owning this InfoBar.</param>
        /// <param name="model">The InfoBar content to display.</param>
        /// <param name="onActionClicked">Invoked when the user clicks an action item (hyperlink/button) in this InfoBar.</param>
        /// <param name="onClosed">Invoked when the user dismisses this InfoBar with the close ('x') button. Not raised for programmatic hide/update.</param>
        public void ShowInfoBar(string key, InfoBarModel model, Action<IVsInfoBarActionItem>? onActionClicked = null, Action? onClosed = null)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_disposed || _activeInfoBars.ContainsKey(key))
            {
                return;
            }

            CreateAndAdd(key, model, onActionClicked, onClosed);
        }

        /// <summary>
        /// Replaces the content of an existing InfoBar in place, preserving its callbacks. If the key isn't
        /// currently visible, this is a no-op (callers should <see cref="ShowInfoBar"/> first).
        /// </summary>
        public void UpdateInfoBar(string key, InfoBarModel model)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_disposed || !_activeInfoBars.TryGetValue(key, out InfoBarEntry existing))
            {
                return;
            }

            // Programmatic close: unadvise first so VS doesn't raise OnClosed (which would fire the user-close callback).
            existing.UIElement.Unadvise(existing.Cookie);
            existing.UIElement.Close();
            _activeInfoBars.Remove(key);

            CreateAndAdd(key, model, existing.OnActionClicked, existing.OnClosed);
        }

        /// <summary>
        /// Hides and removes the InfoBar with the given key. Does not raise the scenario's onClosed callback,
        /// since this is a programmatic hide rather than a user dismissal.
        /// </summary>
        public void HideInfoBar(string key)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_activeInfoBars.TryGetValue(key, out InfoBarEntry entry))
            {
                entry.UIElement.Unadvise(entry.Cookie);
                entry.UIElement.Close();
                _activeInfoBars.Remove(key);
            }
        }

        /// <summary>
        /// Returns whether an InfoBar with the given key is currently displayed.
        /// </summary>
        public bool IsInfoBarVisible(string key) => _activeInfoBars.ContainsKey(key);

        private void CreateAndAdd(string key, InfoBarModel model, Action<IVsInfoBarActionItem>? onActionClicked, Action? onClosed)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IVsInfoBarUIElement uiElement = _infoBarFactory.CreateInfoBar(model);
            if (uiElement == null)
            {
                return;
            }

            uiElement.Advise(this, out uint cookie);
            _infoBarHost.AddInfoBar(uiElement);

            _activeInfoBars[key] = new InfoBarEntry(uiElement, cookie, onActionClicked, onClosed);
        }

        private bool TryFindKey(IVsInfoBarUIElement uiElement, out string key)
        {
            foreach (KeyValuePair<string, InfoBarEntry> kvp in _activeInfoBars)
            {
                if (kvp.Value.UIElement == uiElement)
                {
                    key = kvp.Key;
                    return true;
                }
            }

            key = string.Empty;
            return false;
        }

        void IVsInfoBarUIEvents.OnClosed(IVsInfoBarUIElement infoBarUIElement)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!TryFindKey(infoBarUIElement, out string key))
            {
                return;
            }

            InfoBarEntry entry = _activeInfoBars[key];
            entry.UIElement.Unadvise(entry.Cookie);
            _activeInfoBars.Remove(key);

            // User dismissed the InfoBar; let the scenario react (e.g. suppress for the session).
            entry.OnClosed?.Invoke();
        }

        void IVsInfoBarUIEvents.OnActionItemClicked(IVsInfoBarUIElement infoBarUIElement, IVsInfoBarActionItem actionItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (TryFindKey(infoBarUIElement, out string key))
            {
                _activeInfoBars[key].OnActionClicked?.Invoke(actionItem);
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            // InfoBar cleanup requires the UI thread. If called off-thread (rare), skip cleanup —
            // the window frame closing will release the InfoBars.
            if (!ThreadHelper.CheckAccess())
            {
                return;
            }

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread — guarded by CheckAccess() above
            foreach (InfoBarEntry entry in _activeInfoBars.Values)
            {
                entry.UIElement.Unadvise(entry.Cookie);
                entry.UIElement.Close();
            }
#pragma warning restore VSTHRD010

            _activeInfoBars.Clear();
        }

        private sealed class InfoBarEntry
        {
            public InfoBarEntry(IVsInfoBarUIElement uiElement, uint cookie, Action<IVsInfoBarActionItem>? onActionClicked, Action? onClosed)
            {
                UIElement = uiElement;
                Cookie = cookie;
                OnActionClicked = onActionClicked;
                OnClosed = onClosed;
            }

            public IVsInfoBarUIElement UIElement { get; }
            public uint Cookie { get; }
            public Action<IVsInfoBarActionItem>? OnActionClicked { get; }
            public Action? OnClosed { get; }
        }
    }
}
