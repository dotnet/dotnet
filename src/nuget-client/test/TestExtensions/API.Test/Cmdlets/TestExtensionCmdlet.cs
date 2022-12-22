// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Management.Automation;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace API.Test.Cmdlets
{
    public abstract class TestExtensionCmdlet : Cmdlet
    {
        public string CmdletName { get; }

        protected TestExtensionCmdlet()
        {
            var attribute = Attribute.GetCustomAttribute(GetType(), typeof(CmdletAttribute), inherit: true) as CmdletAttribute;
            Assumes.NotNull(attribute);
            CmdletName = $"{attribute.VerbName}-{attribute.NounName}";
        }

        protected override void ProcessRecord()
        {
            ThreadHelper.JoinableTaskFactory.Run(() => ProcessRecordAsync());
        }

        protected override void BeginProcessing()
        {
            WriteVerbose($"{CmdletName}: Begin");

            base.BeginProcessing();
        }

        protected override void EndProcessing()
        {
            WriteVerbose($"{CmdletName}: End");

            base.EndProcessing();
        }
        protected override void StopProcessing()
        {
            WriteVerbose($"{CmdletName}: Stop");

            base.StopProcessing();
        }

        protected abstract Task ProcessRecordAsync();
    }
}
