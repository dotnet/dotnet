// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Threading;
using NuGet.VisualStudio;
using LocalResources = NuGet.PackageManagement.PowerShellCmdlets.Resources;

namespace NuGetConsole.Host.PowerShell.Implementation
{
    internal class NuGetPSHost : PSHost, IHostSupportsInteractiveSession
    {
        private readonly CultureInfo _culture = Thread.CurrentThread.CurrentCulture;
        private readonly Guid _instanceId = Guid.NewGuid();
        private readonly string _name;
        private readonly PSObject _privateData;
        private readonly CultureInfo _uiCulture = Thread.CurrentThread.CurrentUICulture;
        private PSHostUserInterface _ui;

        public NuGetPSHost(string name, params Tuple<string, object>[] extraData)
        {
            _name = name;
            _privateData = new PSObject(new Commander(this));

            // add extra data as note properties
            foreach (Tuple<string, object> tuple in extraData)
            {
                _privateData.Properties.Add(new PSNoteProperty(tuple.Item1, tuple.Item2));
            }
        }

        public IConsole ActiveConsole { get; set; }

        public override CultureInfo CurrentCulture
        {
            get { return _culture; }
        }

        public override CultureInfo CurrentUICulture
        {
            get { return _uiCulture; }
        }

        public override Guid InstanceId
        {
            get { return _instanceId; }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override PSObject PrivateData
        {
            get { return _privateData; }
        }

        public override PSHostUserInterface UI
        {
            get
            {
                if (_ui == null)
                {
                    _ui = new NuGetHostUserInterface(this);
                }
                return _ui;
            }
        }

        public override Version Version
        {
            get { return GetType().Assembly.GetName().Version; }
        }

        #region IHostSupportsInteractiveSession Members

        public void PushRunspace(Runspace runspace)
        {
            throw new NotSupportedException();
        }

        public void PopRunspace()
        {
            throw new NotSupportedException();
        }

        public bool IsRunspacePushed
        {
            get { return false; }
        }

        public Runspace Runspace
        {
            get { return Runspace.DefaultRunspace; }
        }

        #endregion

        public override void EnterNestedPrompt()
        {
            UI.WriteErrorLine(LocalResources.ErrorNestedPromptNotSupported);
        }

        public override void ExitNestedPrompt()
        {
            throw new NotSupportedException();
        }

        public override void NotifyBeginApplication()
        {
        }

        public override void NotifyEndApplication()
        {
        }

        public override void SetShouldExit(int exitCode)
        {
        }

        #region Nested type: Commander

        private class Commander
        {
            private readonly NuGetPSHost _host;

            public Commander(NuGetPSHost host)
            {
                _host = host;
            }

            [SuppressMessage(
                "Microsoft.Performance",
                "CA1811:AvoidUncalledPrivateCode",
                Justification = "This method can be dynamically invoked from PS script.")]
            public void ClearHost()
            {
                if (_host.ActiveConsole != null)
                {
                    NuGetUIThreadHelper.JoinableTaskFactory.Run(() => _host.ActiveConsole.ClearAsync());
                }
            }
        }

        #endregion
    }
}
