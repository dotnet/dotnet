// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Security;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;

namespace NuGetConsole
{
    [Export(typeof(IConsoleInitializer))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ConsoleInitializer : IConsoleInitializer
    {
        private readonly AsyncLazy<Action> _initializeTask = new AsyncLazy<Action>(GetInitializeTaskAsync, NuGetUIThreadHelper.JoinableTaskFactory);

        public Task<Action> Initialize()
        {
            return _initializeTask.GetValueAsync();
        }

        private static async Task<Action> GetInitializeTaskAsync()
        {
            var comSvc = await AsyncServiceProvider.GlobalProvider.GetComponentModelAsync();
            if (comSvc == null)
            {
                throw new InvalidOperationException();
            }

            try
            {
                // HACK: Short cut to set the Powershell execution policy for this process to RemoteSigned.
                // This is so that we can initialize the PowerShell host and load our modules successfully.
                Environment.SetEnvironmentVariable(
                    "PSExecutionPolicyPreference", "RemoteSigned", EnvironmentVariableTarget.Process);
            }
            catch (SecurityException)
            {
                // ignore if user doesn't have permission to add process-level environment variable,
                // which is very rare.
            }

            var initializer = comSvc.GetService<IHostInitializer>();

            if (initializer != null)
            {
                await TaskScheduler.Default;
                await initializer.StartAsync();
                return initializer.SetDefaultRunspace;
            }

            return delegate { };
        }
    }
}
