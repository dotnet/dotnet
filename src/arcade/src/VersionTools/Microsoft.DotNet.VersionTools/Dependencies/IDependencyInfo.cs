// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.DotNet.VersionTools.Dependencies
{
    /// <summary>
    /// Information about the current state of a dependency. This information is at some point in
    /// time, not necessarily the latest state available.
    /// </summary>
    public interface IDependencyInfo
    {
        /// <summary>
        /// A simple, short name for this dependency info. Used in the suggested commit message.
        /// </summary>
        string SimpleName { get; }

        /// <summary>
        /// A simple, short name for the overall version of this dependency info. Used in the
        /// suggested commit message.
        /// </summary>
        string SimpleVersion { get; }
    }
}
