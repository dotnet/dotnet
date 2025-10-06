// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.VisualStudio;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    internal class AsyncPowerShellHost : PowerShellHost, IAsyncHost
    {
        public event EventHandler ExecuteEnd;

        public AsyncPowerShellHost(string name, IRestoreEvents restoreEvents, IRunspaceManager runspaceManager, IEnvironmentVariableReader environmentVariableReader)
            : base(name, restoreEvents, runspaceManager, environmentVariableReader)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override bool ExecuteHost(string fullCommand, string command, params object[] inputs)
        {
            SetPrivateDataOnHost(false);

            try
            {
                Pipeline pipeline = Runspace.InvokeAsync(fullCommand, inputs, true, (sender, e) =>
                    {
                        switch (e.PipelineStateInfo.State)
                        {
                            case PipelineState.Completed:
                            case PipelineState.Failed:
                            case PipelineState.Stopped:
                                if (e.PipelineStateInfo.Reason != null)
                                {
                                    ReportError(e.PipelineStateInfo.Reason);
                                }

                                OnExecuteCommandEnd();
                                ExecuteEnd.Raise(this, EventArgs.Empty);
                                break;
                        }
                    });

                ExecutingPipeline = pipeline;
                return true;
            }
            catch (RuntimeException e)
            {
                ReportError(e.ErrorRecord);
                ExceptionHelper.WriteErrorToActivityLog(e);
            }
            catch (Exception e)
            {
                ReportError(e);
                ExceptionHelper.WriteErrorToActivityLog(e);
            }

            return false; // Error occurred, command not executing
        }

        protected override Task<string[]> GetExpansionsAsyncCore(string line, string lastWord, CancellationToken token)
        {
            return GetExpansionsAsyncCore(line, lastWord, isSync: false, token: token);
        }

        protected override Task<SimpleExpansion> GetPathExpansionsAsyncCore(string line, CancellationToken token)
        {
            return GetPathExpansionsAsyncCore(line, isSync: false, token: token);
        }
    }
}
