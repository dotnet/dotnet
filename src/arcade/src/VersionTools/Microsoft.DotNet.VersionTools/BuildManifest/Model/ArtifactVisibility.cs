// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    /// <summary>
    /// Description of how an artifact should be published between different builds in various different scenarios.
    /// </summary>
    [Flags]
    public enum ArtifactVisibility
    {
        /// <summary>
        /// No!
        /// </summary>
        None = 0,
        /// <summary>
        /// The artifact should be published for external usage, whether as a shipping or non-shipping package.
        /// </summary>
        External = 0x1,
        /// <summary>
        /// The artifact is used by different jobs within the same overall build, vertical or not.
        /// The artifact should be uploaded to build artifacts, but should not be published to any NuGet/blob feeds.
        /// </summary>
        /// <remarks>
        /// This visibility should be used for artifacts that must be flowed to jobs in a later build pass but not published.
        /// </remarks>
        Internal = 0x2,
        /// <summary>
        /// The artifact is used by other repositories targeting the same target RID/platform.
        /// In vertical builds, it should be published to the "on-disk" locations, but not to any NuGet/blob feeds.
        /// In non-vertical builds, it should be treated as a non-shipping package with <see cref="External"/> visibility.
        /// </summary>
        Vertical = 0x4,

        /// <summary>
        /// All visibility options.
        /// </summary>
        All = External | Internal | Vertical
    }
}
