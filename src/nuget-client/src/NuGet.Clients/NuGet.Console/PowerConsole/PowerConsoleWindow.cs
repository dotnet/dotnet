// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;

namespace NuGetConsole.Implementation.PowerConsole
{
    [Export(typeof(IPowerConsoleWindow))]
    [Export(typeof(IHostInitializer))]
    internal class PowerConsoleWindow : IPowerConsoleWindow, IHostInitializer, IDisposable
    {
        public const string ContentType = "PackageConsole";

        private Dictionary<string, HostInfo> _hostInfos;
        private HostInfo _activeHostInfo;

        [Import(typeof(SVsServiceProvider))]
        internal IServiceProvider ServiceProvider { get; set; }

        [Import]
        internal IWpfConsoleService WpfConsoleService { get; set; }

        [ImportMany]
        internal IEnumerable<Lazy<IHostProvider, IHostMetadata>> HostProviders { get; set; }

        private Dictionary<string, HostInfo> HostInfos
        {
            get
            {
                if (_hostInfos == null)
                {
                    _hostInfos = new Dictionary<string, HostInfo>();
                    foreach (var hostProvider in HostProviders)
                    {
                        var info = new HostInfo(this, hostProvider);
                        _hostInfos[info.HostName] = info;
                    }
                }
                return _hostInfos;
            }
        }

        internal HostInfo ActiveHostInfo
        {
            get
            {
                if (_activeHostInfo == null)
                {
                    // we only have exactly one host, the PowerShellHost. So always choose the first and only one.
                    _activeHostInfo = HostInfos.Values.FirstOrDefault();
                }
                return _activeHostInfo;
            }
        }

        // represent the default feed
        public string ActivePackageSource
        {
            get
            {
                var hi = ActiveHostInfo;
                return (hi != null && hi.WpfConsole != null && hi.WpfConsole.Host != null) ?
                    ActiveHostInfo.WpfConsole.Host.ActivePackageSource :
                    null;
            }
            set
            {
                var hi = ActiveHostInfo;
                if (hi != null
                    && hi.WpfConsole != null
                    && hi.WpfConsole.Host != null)
                {
                    hi.WpfConsole.Host.ActivePackageSource = value;
                }
            }
        }

        public string[] PackageSources
        {
            get { return ActiveHostInfo.WpfConsole.Host.GetPackageSources(); }
        }

        public string[] AvailableProjects
        {
            get { return ActiveHostInfo.WpfConsole.Host.GetAvailableProjects(); }
        }

        public string DefaultProject
        {
            get
            {
                var hi = ActiveHostInfo;
                return (hi != null && hi.WpfConsole != null && hi.WpfConsole.Host != null) ?
                    ActiveHostInfo.WpfConsole.Host.DefaultProject :
                    string.Empty;
            }
        }

        public void SetDefaultProjectIndex(int selectedIndex)
        {
            var hi = ActiveHostInfo;
            if (hi != null
                && hi.WpfConsole != null
                && hi.WpfConsole.Host != null)
            {
                hi.WpfConsole.Host.SetDefaultProjectIndex(selectedIndex);
            }
        }

        public void Show()
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var vsUIShell = await AsyncServiceProvider.GlobalProvider.GetServiceAsync<IVsUIShell, IVsUIShell>(throwOnFailure: false);
                if (vsUIShell != null)
                {
                    var guid = typeof(PowerConsoleToolWindow).GUID;

                    ErrorHandler.ThrowOnFailure(
                        vsUIShell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref guid, out var frame));

                    if (frame != null)
                    {
                        ErrorHandler.ThrowOnFailure(frame.Show());
                    }
                }
            });
        }

        public Task StartAsync()
        {
            return ActiveHostInfo.WpfConsole.Dispatcher.StartAsync();
        }

        public void SetDefaultRunspace()
        {
            ActiveHostInfo.WpfConsole.Host.SetDefaultRunspace();
        }

        void IDisposable.Dispose()
        {
            if (_hostInfos != null)
            {
                foreach (IDisposable hostInfo in _hostInfos.Values)
                {
                    if (hostInfo != null)
                    {
                        hostInfo.Dispose();
                    }
                }
            }
        }
    }
}
