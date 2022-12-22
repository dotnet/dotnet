// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;

namespace NuGetConsole
{
    [Export(typeof(IOutputConsoleProvider))]
    public class OutputConsoleProvider : IOutputConsoleProvider
    {
        private readonly IEnumerable<Lazy<IHostProvider, IHostMetadata>> _hostProviders;
        private readonly AsyncLazy<IConsole> _cachedOutputConsole;
        private readonly AsyncLazy<bool> _isServerMode;
        private IAsyncServiceProvider _asyncServiceProvider;

        private readonly AsyncLazy<IVsOutputWindow> _vsOutputWindow;

        [ImportingConstructor]
        OutputConsoleProvider(
            [ImportMany]
            IEnumerable<Lazy<IHostProvider, IHostMetadata>> hostProviders)
            : this(AsyncServiceProvider.GlobalProvider, hostProviders)
        { }

        OutputConsoleProvider(
            IAsyncServiceProvider asyncServiceProvider,
            IEnumerable<Lazy<IHostProvider, IHostMetadata>> hostProviders)
        {
            _asyncServiceProvider = asyncServiceProvider ?? throw new ArgumentNullException(nameof(asyncServiceProvider));
            _hostProviders = hostProviders ?? throw new ArgumentNullException(nameof(hostProviders));

            _vsOutputWindow = new AsyncLazy<IVsOutputWindow>(
                async () =>
                {
                    return await asyncServiceProvider.GetServiceAsync<SVsOutputWindow, IVsOutputWindow>();
                },
                NuGetUIThreadHelper.JoinableTaskFactory);

            _cachedOutputConsole = new AsyncLazy<IConsole>(
                async () =>
                {
                    if (await _isServerMode.GetValueAsync())
                    {
                        // This is disposable, but it lives for the duration of the process.
                        return new ChannelOutputConsole(
                                _asyncServiceProvider,
                                GuidList.NuGetOutputWindowPaneGuid,
                                Resources.OutputConsolePaneName,
                                NuGetUIThreadHelper.JoinableTaskFactory);
                    }
                    else
                    {
                        var vsUIShell = await asyncServiceProvider.GetServiceAsync<SVsUIShell, IVsUIShell>();
                        var vsOutputWindow = await _vsOutputWindow.GetValueAsync();
                        return new OutputConsole(vsOutputWindow, vsUIShell);
                    }
                }, NuGetUIThreadHelper.JoinableTaskFactory);

            _isServerMode = new AsyncLazy<bool>(
                () =>
                {
                    return VisualStudioContextHelper.IsInServerModeAsync(CancellationToken.None);
                }, NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public async Task<IOutputConsole> CreateBuildOutputConsoleAsync()
        {
            if (await _isServerMode.GetValueAsync())
            {
                return await _cachedOutputConsole.GetValueAsync();
            }
            else
            {
                var vsOutputWindow = await _vsOutputWindow.GetValueAsync();
                return new BuildOutputConsole(vsOutputWindow);
            }
        }

        public async Task<IOutputConsole> CreatePackageManagerConsoleAsync()
        {
            return await _cachedOutputConsole.GetValueAsync();
        }

        public async Task<IConsole> CreatePowerShellConsoleAsync()
        {
            var console = await _cachedOutputConsole.GetValueAsync();

            if (console.Host == null)
            {
                var hostProvider = GetPowerShellHostProvider();
                console.Host = hostProvider.CreateHost(@async: false);
            }

            return console;
        }

        private IHostProvider GetPowerShellHostProvider()
        {
            // The PowerConsole design enables multiple hosts (PowerShell, Python, Ruby)
            // For the Output window console, we're only interested in the PowerShell host.
            // Here we filter out the PowerShell host provider based on its name.

            // The PowerShell host provider name is defined in PowerShellHostProvider.cs
            const string PowerShellHostProviderName = "NuGetConsole.Host.PowerShell";

            var psProvider = _hostProviders
                .Single(export => export.Metadata.HostName == PowerShellHostProviderName);

            return psProvider.Value;
        }
    }
}
