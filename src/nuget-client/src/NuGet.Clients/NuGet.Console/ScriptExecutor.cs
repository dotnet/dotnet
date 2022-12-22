// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Configuration;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace NuGetConsole
{
    [Export(typeof(IScriptExecutor))]
    internal class ScriptExecutor : IScriptExecutor
    {
        private ConcurrentDictionary<PackageIdentity, PackageInitPS1State> _initScriptExecutions
            = new ConcurrentDictionary<PackageIdentity, PackageInitPS1State>(PackageIdentityComparer.Default);

        private AsyncLazy<IHost> Host { get; }

        [Import]
        private Lazy<ISettings> Settings { get; set; }

        [Import]
        private ISolutionManager SolutionManager { get; set; }

        [Import]
        public IPowerConsoleWindow PowerConsoleWindow { get; set; }

        [Import]
        public IOutputConsoleProvider OutputConsoleProvider { get; set; }

        public ScriptExecutor()
        {
            Host = new AsyncLazy<IHost>(GetHostAsync, ThreadHelper.JoinableTaskFactory);
            Reset();
        }

        public void Reset()
        {
            _initScriptExecutions.Clear();
        }

        public async Task<bool> ExecuteAsync(
            PackageIdentity identity,
            string installPath,
            string relativeScriptPath,
            EnvDTE.Project project,
            INuGetProjectContext nuGetProjectContext,
            bool throwOnFailure)
        {
            var scriptPath = Path.Combine(installPath, relativeScriptPath);

            if (File.Exists(scriptPath))
            {
                if (scriptPath.EndsWith(PowerShellScripts.Init, StringComparison.OrdinalIgnoreCase)
                    && !TryMarkVisited(identity, PackageInitPS1State.FoundAndExecuted))
                {
                    return true;
                }

                var request = new ScriptExecutionRequest(scriptPath, installPath, identity, project);

                var psNuGetProjectContext = nuGetProjectContext as IPSNuGetProjectContext;
                if (psNuGetProjectContext != null
                    && psNuGetProjectContext.IsExecuting
                    && psNuGetProjectContext.CurrentPSCmdlet != null)
                {
                    var psVariable = psNuGetProjectContext.CurrentPSCmdlet.SessionState.PSVariable;

                    // set temp variables to pass to the script
                    psVariable.Set("__rootPath", request.InstallPath);
                    psVariable.Set("__toolsPath", request.ToolsPath);
                    psVariable.Set("__package", request.ScriptPackage);
                    psVariable.Set("__project", request.Project);

                    psNuGetProjectContext.ExecutePSScript(request.ScriptPath, throwOnFailure);
                }
                else
                {
                    var logMessage = string.Format(CultureInfo.CurrentCulture, Resources.ExecutingScript, scriptPath);
                    // logging to both the Output window and progress window.
                    nuGetProjectContext.Log(MessageLevel.Info, logMessage);
                    try
                    {
                        await ExecuteScriptCoreAsync(request);
                    }
                    catch (Exception ex)
                    {
                        // throwFailure is set by Package Manager.
                        if (throwOnFailure)
                        {
                            throw;
                        }
                        nuGetProjectContext.Log(MessageLevel.Warning, ex.Message);
                    }
                }

                return true;
            }
            else
            {
                if (scriptPath.EndsWith(PowerShellScripts.Init, StringComparison.OrdinalIgnoreCase))
                {
                    TryMarkVisited(identity, PackageInitPS1State.NotFound);
                }
            }
            return false;
        }

        public bool TryMarkVisited(PackageIdentity packageIdentity, PackageInitPS1State initPS1State)
        {
            return _initScriptExecutions.TryAdd(packageIdentity, initPS1State);
        }

        public async Task<bool> ExecuteInitScriptAsync(PackageIdentity identity)
        {
            var result = false;
            // Reserve the key. We can remove if the package has not been restored.
            if (TryMarkVisited(identity, PackageInitPS1State.NotFound))
            {
                var nugetPaths = NuGetPathContext.Create(Settings.Value);
                var fallbackResolver = new FallbackPackagePathResolver(nugetPaths);
                var installPath = fallbackResolver.GetPackageDirectory(identity.Id, identity.Version);

                if (!string.IsNullOrEmpty(installPath))
                {
                    var scriptPath = Path.Combine(installPath, "tools", PowerShellScripts.Init);

                    if (File.Exists(scriptPath))
                    {
                        // Init.ps1 is present and will be executed.
                        _initScriptExecutions.TryUpdate(
                            identity,
                            PackageInitPS1State.FoundAndExecuted,
                            PackageInitPS1State.NotFound);

                        var request = new ScriptExecutionRequest(scriptPath, installPath, identity, project: null);

                        await ExecuteScriptCoreAsync(request);

                        result = true;
                    }
                }
                else
                {
                    // Package is not restored. Do not cache the results.
                    PackageInitPS1State dummy;
                    _initScriptExecutions.TryRemove(identity, out dummy);
                    result = false;
                }
            }
            else
            {
                // Key is already present. Simply access its value
                result = (_initScriptExecutions[identity] == PackageInitPS1State.FoundAndExecuted);
            }

            return result;
        }

        private async Task ExecuteScriptCoreAsync(ScriptExecutionRequest request)
        {
            var console = await OutputConsoleProvider.CreatePowerShellConsoleAsync();
            var host = await Host.GetValueAsync();

            // Host.Execute calls powershell's pipeline.Invoke and blocks the calling thread
            // to switch to powershell pipeline execution thread. In order not to block the UI thread,
            // go off the UI thread. This is important, since, switches to UI thread,
            // using SwitchToMainThreadAsync will deadlock otherwise
            await Task.Run(() => host.Execute(console, request.BuildCommand(), request.BuildInput()));
        }

        private async Task<IHost> GetHostAsync()
        {
            // Since we are creating the output console and the output window pane, switch to the main thread
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // create the console and instantiate the PS host on demand
            var console = await OutputConsoleProvider.CreatePowerShellConsoleAsync();
            var host = console.Host;

            // start the console 
            await console.Dispatcher.StartAsync();

            // gives the host a chance to do initialization works before dispatching commands to it
            // Host.Initialize calls powershell's pipeline.Invoke and blocks the calling thread
            // to switch to powershell pipeline execution thread. In order not to block the UI thread, go off the UI thread.
            // This is important, since, switches to UI thread, using SwitchToMainThreadAsync will deadlock otherwise
            await Task.Run(() => host.Initialize(console));

            // after the host initializes, it may set IsCommandEnabled = false
            if (host.IsCommandEnabled)
            {
                return host;
            }
            // the PowerShell host fails to initialize if group policy restricts to AllSigned
            throw new InvalidOperationException(Resources.Console_InitializeHostFails);
        }
    }
}
