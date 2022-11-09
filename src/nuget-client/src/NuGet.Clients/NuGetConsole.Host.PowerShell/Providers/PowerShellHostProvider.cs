// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;
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

        [ImportingConstructor]
        public PowerShellHostProvider(IRestoreEvents restoreEvents)
        {
            if (restoreEvents == null)
            {
                throw new ArgumentNullException(nameof(restoreEvents));
            }

            _restoreEvents = restoreEvents;
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
            string syncModeFlag = Environment.GetEnvironmentVariable("NuGetSyncMode", EnvironmentVariableTarget.User);
            if (syncModeFlag == "1")
            {
                @async = false;
            }

            return PowerShellHostService.CreateHost(PowerConsoleHostName, _restoreEvents, @async);
        }
    }
}
