// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.Build.Tasks.Workloads.Wix
{
    /// <summary>
    /// An enumeration that defines the GUID generation options used by the Heat command.
    /// </summary>
    public enum GuidOptions
    {
        /// <summary>
        /// Generate GUIDs now, during harvesting (-gg).
        /// </summary>
        GenerateNow,

        /// <summary>
        /// Autogenerate GUIDs at compile time (-ag).
        /// </summary>
        GenerateAtCompileTime
    }
}
