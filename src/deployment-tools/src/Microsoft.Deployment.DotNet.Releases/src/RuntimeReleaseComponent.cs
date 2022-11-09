// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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
        [JsonProperty(PropertyName = "vs-mac-version")]
        public string VisualStudioMacVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// The versions of Visual Studio that includes this runtime. Multiple versions may be listed, e.g.
        /// &quot;15.9.25, 16.0.16, 16.4.11, 16.6.4&quot;
        /// </summary>
        [JsonProperty(PropertyName = "vs-version")]
        public string VisualStudioVersion
        {
            get;
            private set;
        }

        internal RuntimeReleaseComponent(JToken token, ProductRelease release) : base(token, release)
        {
        }
    }
}
