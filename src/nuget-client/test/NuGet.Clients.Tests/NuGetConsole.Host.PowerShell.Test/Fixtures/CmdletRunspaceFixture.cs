// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Threading;
using NuGet.PackageManagement.PowerShellCmdlets;
using PSCommand = System.Management.Automation.Runspaces.Command;

namespace NuGetConsole.Host.PowerShell.Test
{
    /// <summary>
    /// Encapsulates runspace and host setup for invoking NuGet PowerShell cmdlets in tests.
    /// </summary>
    internal sealed class CmdletRunspaceFixture : IDisposable
    {
        private readonly Runspace _runspace;

        public CmdletRunspaceFixture(string activeSource = "https://contoso.com/v3/index.json")
        {
            var host = new TestPSHost(activeSource);
            var initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.Commands.Add(
                new SessionStateCmdletEntry("Find-Package", typeof(FindPackageCommand), null));
            initialSessionState.Commands.Add(
                new SessionStateCmdletEntry("Get-Package", typeof(GetPackageCommand), null));

            _runspace = RunspaceFactory.CreateRunspace(host, initialSessionState);
            _runspace.Open();
        }

        public IList<PSObject> Invoke(string cmdletName, Dictionary<string, object> parameters)
        {
            using var pipeline = _runspace.CreatePipeline();
            var cmd = new PSCommand(cmdletName);
            foreach (var kvp in parameters)
            {
                cmd.Parameters.Add(kvp.Key, kvp.Value);
            }
            pipeline.Commands.Add(cmd);
            return pipeline.Invoke().ToList();
        }

        public void Dispose()
        {
            _runspace.Close();
            _runspace.Dispose();
        }
    }

    /// <summary>
    /// Minimal PSHost that provides PrivateData with properties expected by NuGet cmdlets.
    /// </summary>
    internal sealed class TestPSHost : PSHost
    {
        private readonly Guid _instanceId = Guid.NewGuid();
        private readonly PSObject _privateData;

        public TestPSHost(string activeSource)
        {
            _privateData = new PSObject();
            _privateData.Properties.Add(new PSNoteProperty("activePackageSource", activeSource));
            _privateData.Properties.Add(new PSNoteProperty("CancellationTokenKey", CancellationToken.None));
        }

        public override CultureInfo CurrentCulture => CultureInfo.InvariantCulture;
        public override CultureInfo CurrentUICulture => CultureInfo.InvariantCulture;
        public override Guid InstanceId => _instanceId;
        public override string Name => "TestNuGetHost";
        public override PSObject PrivateData => _privateData;
        public override PSHostUserInterface? UI => null;
        public override Version Version => new Version(1, 0);

        public override void EnterNestedPrompt() { }
        public override void ExitNestedPrompt() { }
        public override void NotifyBeginApplication() { }
        public override void NotifyEndApplication() { }
        public override void SetShouldExit(int exitCode) { }
    }
}
