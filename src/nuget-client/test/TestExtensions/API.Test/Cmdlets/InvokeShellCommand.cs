// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Management.Automation;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace API.Test.Cmdlets
{
    [Cmdlet(VerbsLifecycle.Invoke, "ShellCommand")]
    public sealed class InvokeShellCommand : TestExtensionCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string CommandName { get; set; }

        protected override async Task ProcessRecordAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = ServiceLocator.GetDTE();
            dte.ExecuteCommand(CommandName);
        }
    }
}
