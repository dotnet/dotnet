// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// Flags corresponding to Heat commandline suppressions.
    /// </summary>
    [Flags]
    public enum HeatSuppressions
    {
        /// <summary>
        /// Suppress COM elements (-scom).
        /// </summary>
        SuppressComElements = 0x0001,

        /// <summary>
        /// Suppress fragments (-sfrag).
        /// </summary>
        SuppressFragments = 0x0002,

        /// <summary>
        /// Suppress harvesting the root directory as an element (-srd).
        /// </summary>
        SuppressRootDirectory = 0x0004,

        /// <summary>
        /// Suppress registry harvesting (-sreg).
        /// </summary>
        SuppressRegistryHarvesting = 0x0008,

        /// <summary>
        /// Suppress unique identifiers for files, components, and directories (-suid).
        /// </summary>
        SuppressUuid = 0x0010,

        /// <summary>
        /// Suppress VB6 COM elements (-svb6).
        /// </summary>
        SuppressVb6Com = 0x0020,
    }
}
