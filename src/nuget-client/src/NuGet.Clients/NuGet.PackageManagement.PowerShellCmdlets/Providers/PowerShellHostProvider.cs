// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
using NuGet.Common;
using NuGet.VisualStudio;
using NuGetConsole.Host.PowerShell.Implementation;

namespace NuGetConsole.Host
{
    [Export(typeof(IHostProvider))]
    [HostName(HostName)]
    [DisplayName("NuGet Provider")]
    internal class PowerShellHostProvider : IHostProvider
    {
        /// <summary>
        /// PowerConsole host name of PowerShell host.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public const string HostName = "NuGetConsole.Host.PowerShell";

        /// <summary>
        /// This PowerShell host name. Used for PowerShell "$host".
        /// </summary>
        public const string PowerConsoleHostName = "Package Manager Host";

        private readonly IRestoreEvents _restoreEvents;
        private readonly IEnvironmentVariableReader _environmentVariableReader;

        [ImportingConstructor]
        public PowerShellHostProvider(IRestoreEvents restoreEvents)
            : this(restoreEvents, EnvironmentVariableWrapper.Instance)
        {
        }

        internal PowerShellHostProvider(IRestoreEvents restoreEvents, IEnvironmentVariableReader environmentVariableReader)
        {
            _restoreEvents = restoreEvents ?? throw new ArgumentNullException(nameof(restoreEvents));

            _environmentVariableReader = environmentVariableReader ?? throw new ArgumentNullException(nameof(environmentVariableReader));
        }

        public IHost CreateHost(bool @async)
        {
            bool isPowerShell2Installed = RegistryHelper.CheckIfPowerShell2OrAboveInstalled();
            if (isPowerShell2Installed)
            {
                return CreatePowerShellHost(@async);
            }
            return new UnsupportedHost();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IHost CreatePowerShellHost(bool @async)
        {
            // backdoor: allow turning off async mode by setting enviroment variable NuGetSyncMode=1
            string syncModeFlag = _environmentVariableReader.GetEnvironmentVariable("NuGetSyncMode");
            if (syncModeFlag == "1")
            {
                @async = false;
            }

            return PowerShellHostService.CreateHost(PowerConsoleHostName, _restoreEvents, _environmentVariableReader, @async);
        }
    }
}
