// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json.Linq;

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

        internal WindowsDesktopReleaseComponent(JToken token, ProductRelease release) : base(token, release)
        {
        }
    }
}
