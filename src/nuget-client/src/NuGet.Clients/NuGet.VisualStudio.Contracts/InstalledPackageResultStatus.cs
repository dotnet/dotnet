// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Contracts
{
    /// <summary>The status of a <see cref="InstalledPackagesResult"/> result</summary>
    public enum InstalledPackageResultStatus
    {
        /// <summary>Unknown status</summary>
        /// <remarks>Probably represents a bug in the method that created the result.</remarks>
        Unknown = 0,

        /// <summary>Successful</summary>
        Successful,

        /// <summary>The project is not yet ready</summary>
        /// <remarks>There are several scenarios where this might happen:
        /// If the project was recently loaded, NuGet might not have been notified by the project system yet.
        /// Restore might not have completed yet.
        /// The requested project is not in the solution or is not loaded.
        /// </remarks>
        ProjectNotReady,

        /// <summary>Package information could not be retrieved because the project is in an invalid state</summary>
        /// <remarks>If a project has an invalid target framework value, or a package reference has a version value, NuGet may be unable to generate basic project information, such as requested packages.</remarks>
        ProjectInvalid
    }
}
