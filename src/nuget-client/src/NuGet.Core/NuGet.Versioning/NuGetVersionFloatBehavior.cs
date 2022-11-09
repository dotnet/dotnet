// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Versioning
{
    /// <summary>
    /// Specifies the floating behavior type.
    /// </summary>
    public enum NuGetVersionFloatBehavior
    {
        /// <summary>
        /// Lowest version, no float
        /// </summary>
        None,

        /// <summary>
        /// Highest matching pre-release label
        /// </summary>
        Prerelease,

        /// <summary>
        /// x.y.z.*
        /// </summary>
        Revision,

        /// <summary>
        /// x.y.*
        /// </summary>
        Patch,

        /// <summary>
        /// x.*
        /// </summary>
        Minor,

        /// <summary>
        /// *
        /// </summary>
        Major,

        /// <summary>
        /// Float major and pre-release, *-*
        /// </summary>
        AbsoluteLatest,

        /// <summary>
        /// Float revision and pre-release x.y.z.*-*
        /// </summary>
        PrereleaseRevision,

        /// <summary>
        /// Float patch and pre-release x.y.*-*
        /// </summary>
        PrereleasePatch,

        /// <summary>
        /// Float minor and pre-release x.*-*
        /// </summary>
        PrereleaseMinor,

        /// <summary>
        /// Float major and prerelease, but only with partial prerelease *-rc.*.
        /// *-* is captured by <see cref="AbsoluteLatest"/>
        /// </summary>
        PrereleaseMajor,
    }
}
