// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading;
using Microsoft.PowerShell;
using Microsoft.VisualStudio.Shell;
using NuGet;
using PathUtility = NuGet.Common.PathUtility;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    /// <summary>
    /// Wraps a runspace and protects the invoke method from being called on multiple threads through blocking.
    /// </summary>
    /// <remarks>
    /// Calls to Invoke on this object will block if the runspace is already busy. Calls to InvokeAsync will also
    /// block until
    /// the runspace is free. However, it will not block while the pipeline is actually running.
    /// </remarks>
    internal class RunspaceDispatcher : IDisposable
    {
        [ThreadStatic]
        private static bool IHaveTheLock;

        private readonly Runspace _runspace;
        private readonly SemaphoreSlim _dispatcherLock = new SemaphoreSlim(1, 1);

        public RunspaceAvailability RunspaceAvailability
        {
            get { return _runspace.RunspaceAvailability; }
        }

        public Version PSVersion
        {
            get { return _runspace.Version; }
        }

        public RunspaceDispatcher(Runspace runspace)
        {
            _runspace = runspace;
        }

        public void MakeDefault()
        {
            if (Runspace.DefaultRunspace == null)
            {
                WithLock(() =>
                    {
                        if (Runspace.DefaultRunspace == null)
                        {
                            // Set this runspace as DefaultRunspace so I can script DTE events.
                            //
                            // WARNING: MSDN says this is unsafe. The runspace must not be shared across
                            // threads. I need this to be able to use ScriptBlock for DTE events. The
                            // ScriptBlock event handlers execute on DefaultRunspace.

                            Runspace.DefaultRunspace = _runspace;
                        }
                    });
            }
        }

        public void InvokeCommands(PSCommand[] profileCommands)
        {
            WithLock(() =>
                {
                    using (var powerShell = System.Management.Automation.PowerShell.Create())
                    {
                        powerShell.Runspace = _runspace;

                        foreach (PSCommand command in profileCommands)
                        {
                            powerShell.Commands = command;
                            powerShell.AddCommand("out-default");
                            powerShell.Invoke();
                        }
                    }
                });
        }

        public Collection<PSObject> Invoke(string command, object[] inputs, bool outputResults)
        {
            // Invoking a command on the pipeline thread blocks the caller.
            // It will make VS stop responding when a caller is on the UI thread.
            ThreadHelper.ThrowIfOnUIThread();

            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            using (var pipeline = CreatePipeline(command, outputResults))
            {
                return InvokeCore(pipeline, inputs);
            }
        }

        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "It is disposed in the StateChanged event handler.")]
        public Pipeline InvokeAsync(
            string command,
            object[] inputs,
            bool outputResults,
            EventHandler<PipelineStateEventArgs> pipelineStateChanged)
        {
            if (String.IsNullOrEmpty(command))
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, CommonResources.Argument_Cannot_Be_Null_Or_Empty, command), nameof(command));
            }

            Pipeline pipeline = CreatePipeline(command, outputResults);

            InvokeCoreAsync(pipeline, inputs, pipelineStateChanged);
            return pipeline;
        }

        public string ExtractErrorFromErrorRecord(ErrorRecord record)
        {
            Pipeline pipeline = _runspace.CreatePipeline(command: "$input", addToHistory: false);
            pipeline.Commands.Add("out-string");

            Collection<PSObject> result;
            using (var inputCollection = new PSDataCollection<object>())
            {
                inputCollection.Add(record);
                inputCollection.Complete();
                result = InvokeCore(pipeline, inputCollection);
            }

            if (result.Count > 0)
            {
                string str = result[0].BaseObject as string;
                if (!string.IsNullOrEmpty(str))
                {
                    // Remove \r\n, which is added by the Out-String cmdlet.
                    return str.TrimEnd('\r', '\n');
                }
            }

            return String.Empty;
        }

        public ExecutionPolicy GetEffectiveExecutionPolicy()
        {
            return GetExecutionPolicy("Get-ExecutionPolicy");
        }

        public ExecutionPolicy GetExecutionPolicy(ExecutionPolicyScope scope)
        {
            return GetExecutionPolicy("Get-ExecutionPolicy -Scope " + scope);
        }

        private Pipeline CreatePipeline(string command, bool outputResults)
        {
            Pipeline pipeline = _runspace.CreatePipeline(command, addToHistory: true);
            if (outputResults)
            {
                pipeline.Commands.Add("out-default");
                pipeline.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
            }
            return pipeline;
        }

        private ExecutionPolicy GetExecutionPolicy(string command)
        {
            Collection<PSObject> results = Invoke(command, inputs: null, outputResults: false);
            if (results.Count > 0)
            {
                return (ExecutionPolicy)results[0].BaseObject;
            }
            return ExecutionPolicy.Undefined;
        }

        public void SetExecutionPolicy(ExecutionPolicy policy, ExecutionPolicyScope scope)
        {
            string command = string.Format(CultureInfo.InvariantCulture, "Set-ExecutionPolicy {0} -Scope {1} -Force", policy, scope);

            Invoke(command, inputs: null, outputResults: false);
        }

        public void ImportModule(string modulePath)
        {
            Invoke("Import-Module " + PathUtility.EscapePSPath(modulePath), null, false);
        }

        public void ChangePSDirectory(string directory)
        {
            if (!String.IsNullOrWhiteSpace(directory))
            {
                Invoke("Set-Location " + PathUtility.EscapePSPath(directory), null, false);
            }
        }

        public void Dispose()
        {
            _runspace.Dispose();
            _dispatcherLock.Dispose();
        }

        // Dispatcher synchronization methods
        private void WithLock(Action act)
        {
            if (IHaveTheLock)
            {
                act();
            }
            else
            {
                _dispatcherLock.Wait();
                try
                {
                    IHaveTheLock = true;
                    act();
                }
                finally
                {
                    IHaveTheLock = false;
                    _dispatcherLock.Release();
                }
            }
        }

        private Collection<PSObject> InvokeCore(Pipeline pipeline, IEnumerable<object> inputs)
        {
            Collection<PSObject> output = null;
            WithLock(() => { output = inputs == null ? pipeline.Invoke() : pipeline.Invoke(inputs); });
            return output;
        }

        private void InvokeCoreAsync(Pipeline pipeline, IEnumerable<object> inputs, EventHandler<PipelineStateEventArgs> pipelineStateChanged)
        {
            bool hadTheLockAlready = IHaveTheLock;

            pipeline.StateChanged += (sender, e) =>
                {
                    // Release the lock ASAP
                    bool finished = e.PipelineStateInfo.State == PipelineState.Completed ||
                                    e.PipelineStateInfo.State == PipelineState.Failed ||
                                    e.PipelineStateInfo.State == PipelineState.Stopped;
                    if (finished && !hadTheLockAlready)
                    {
                        // Release the dispatcher lock
                        _dispatcherLock.Release();
                    }

                    pipelineStateChanged.Raise(sender, e);

                    // Dispose Pipeline object upon completion
                    if (finished)
                    {
                        ((Pipeline)sender).Dispose();
                    }
                };

            if (inputs != null)
            {
                foreach (var input in inputs)
                {
                    pipeline.Input.Write(input);
                }
            }

            // Take the dispatcher lock and invoke the pipeline
            // REVIEW: This could probably be done in a Task so that we can return to the caller before even taking the dispatcher lock
            if (IHaveTheLock)
            {
                pipeline.InvokeAsync();
            }
            else
            {
                _dispatcherLock.Wait();
                try
                {
                    IHaveTheLock = true;
                    pipeline.InvokeAsync();
                }
                catch
                {
                    // Don't care about the exception, rethrow it, but first release the lock
                    IHaveTheLock = false;
                    _dispatcherLock.Release();
                    throw;
                }
            }
        }
    }
}
