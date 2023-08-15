// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Describes a single runtime release.
    /// </summary>
    public class RuntimeReleaseComponent : ReleaseComponent
    {
        /// <summary>
        /// The friendly display name for the component.
        /// </summary>
        public override string Name => ReleasesResources.RuntimeReleaseName;

        /// <summary>
        /// The versions of Visual Studio for Mac that includes this runtime.
        /// </summary>
        public string VisualStudioMacVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// The versions of Visual Studio that includes this runtime. Multiple versions may be listed, e.g.
        /// &quot;15.9.25, 16.0.16, 16.4.11, 16.6.4&quot;
        /// </summary>
        public string VisualStudioVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new <see cref="RuntimeReleaseComponent"/> instance.
        /// </summary>
        /// <param name="element">The JSON element of the component.</param>
        /// <param name="release">The release to which the component belongs.</param>
        internal RuntimeReleaseComponent(JsonElement element, ProductRelease release) : base(element, release)
        {
            VisualStudioMacVersion = element.GetStringOrDefault("vs-mac-version");
            VisualStudioVersion = element.GetStringOrDefault("vs-version");
        }
    }
}
