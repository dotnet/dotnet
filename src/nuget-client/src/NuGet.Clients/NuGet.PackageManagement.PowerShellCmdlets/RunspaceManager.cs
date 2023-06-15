// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using EnvDTE;
using EnvDTE80;
using Microsoft.PowerShell;
using NuGet.PackageManagement;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    internal class RunspaceManager : IRunspaceManager
    {
        // Cache Runspace by name. There should be only one Runspace instance created though.
        private readonly ConcurrentDictionary<string, Tuple<RunspaceDispatcher, NuGetPSHost>> _runspaceCache = new ConcurrentDictionary<string, Tuple<RunspaceDispatcher, NuGetPSHost>>();

        public const string ProfilePrefix = "NuGet";

        public Tuple<RunspaceDispatcher, NuGetPSHost> GetRunspace(IConsole console, string hostName)
        {
            return _runspaceCache.GetOrAdd(hostName, name => CreateAndSetupRunspace(console, name));
        }

        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "We can't dispose it if we want to return it.")]
        private static Tuple<RunspaceDispatcher, NuGetPSHost> CreateAndSetupRunspace(IConsole console, string hostName)
        {
            Tuple<RunspaceDispatcher, NuGetPSHost> runspace = CreateRunspace(console, hostName);
            SetupExecutionPolicy(runspace.Item1);
            LoadModules(runspace.Item1);
            LoadProfilesIntoRunspace(runspace.Item1);

            return Tuple.Create(runspace.Item1, runspace.Item2);
        }

        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2000:Dispose objects before losing scope",
            Justification = "We can't dispose it if we want to return it.")]
        private static Tuple<RunspaceDispatcher, NuGetPSHost> CreateRunspace(IConsole console, string hostName)
        {
            DTE2 dte = NuGetUIThreadHelper.JoinableTaskFactory.Run(() => ServiceLocator.GetGlobalServiceAsync<DTE, DTE2>());

            InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.Variables.Add(
                new SessionStateVariableEntry(
                    "DTE",
                    dte,
                    "Visual Studio DTE automation object",
                    ScopedItemOptions.AllScope | ScopedItemOptions.Constant)
                );

            // this is used by the functional tests
            var sourceRepositoryProvider = ServiceLocator.GetComponentModelService<ISourceRepositoryProvider>();
            var solutionManager = ServiceLocator.GetComponentModelService<ISolutionManager>();
            var sourceRepoTuple = Tuple.Create<string, object>("SourceRepositoryProvider", sourceRepositoryProvider);
            var solutionManagerTuple = Tuple.Create<string, object>("VsSolutionManager", solutionManager);

            Tuple<string, object>[] privateData = { sourceRepoTuple, solutionManagerTuple };

            var host = new NuGetPSHost(hostName, privateData)
            {
                ActiveConsole = console
            };

            var runspace = RunspaceFactory.CreateRunspace(host, initialSessionState);
            runspace.ThreadOptions = PSThreadOptions.Default;
            runspace.Open();

            //
            // Set this runspace as DefaultRunspace so I can script DTE events.
            //
            // WARNING: MSDN says this is unsafe. The runspace must not be shared across
            // threads. I need this to be able to use ScriptBlock for DTE events. The
            // ScriptBlock event handlers execute on DefaultRunspace.
            //
            Runspace.DefaultRunspace = runspace;

            return Tuple.Create(new RunspaceDispatcher(runspace), host);
        }

        private static void SetupExecutionPolicy(RunspaceDispatcher runspace)
        {
            ExecutionPolicy policy = runspace.GetEffectiveExecutionPolicy();
            if (policy != ExecutionPolicy.Unrestricted &&
                policy != ExecutionPolicy.RemoteSigned &&
                policy != ExecutionPolicy.Bypass)
            {
                ExecutionPolicy machinePolicy = runspace.GetExecutionPolicy(ExecutionPolicyScope.MachinePolicy);
                ExecutionPolicy userPolicy = runspace.GetExecutionPolicy(ExecutionPolicyScope.UserPolicy);

                if (machinePolicy == ExecutionPolicy.Undefined && userPolicy == ExecutionPolicy.Undefined)
                {
                    runspace.SetExecutionPolicy(ExecutionPolicy.RemoteSigned, ExecutionPolicyScope.Process);
                }
            }
        }

        private static void LoadModules(RunspaceDispatcher runspace)
        {
            // We store our PS module file at <extension root>\Modules\NuGet\NuGet.psd1
            string extensionRoot = Path.GetDirectoryName(typeof(RunspaceManager).Assembly.Location);
            string modulePath = Path.Combine(extensionRoot, "Modules", "NuGet", "NuGet.psd1");
            runspace.ImportModule(modulePath);

            // provide backdoor to enable function test
            string functionalTestPath = Environment.GetEnvironmentVariable("NuGetFunctionalTestPath");
            if (functionalTestPath != null
                && File.Exists(functionalTestPath))
            {
                runspace.ImportModule(functionalTestPath);
            }
        }

        private static void LoadProfilesIntoRunspace(RunspaceDispatcher runspace)
        {
            PSCommand[] profileCommands = HostUtilities.GetProfileCommands(ProfilePrefix);
            runspace.InvokeCommands(profileCommands);
        }
    }
}
