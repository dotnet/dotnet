// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Represents a Windows Desktop runtime release.
    /// </summary>
    public class WindowsDesktopReleaseComponent : ReleaseComponent
    {
        /// <summary>
        /// The friendly display name for the component.
        /// </summary>
        public override string Name => ReleasesResources.WindowsDesktopReleaseName;

        /// <summary>
        /// Creates a new <see cref="WindowsDesktopReleaseComponent"/> instance.
        /// </summary>
        /// <param name="element">The JSON element of the component.</param>
        /// <param name="release">The release to which the component belongs.</param>
        internal WindowsDesktopReleaseComponent(JsonElement element, ProductRelease release) : base(element, release)
        {
        }
    }
}
