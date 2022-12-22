// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Describes an ASP.NET Core release.
    /// </summary>
    public class AspNetCoreReleaseComponent : ReleaseComponent
    {
        /// <summary>
        /// The versions of the ASP.NET Core Module in this release.
        /// </summary>
        public ReadOnlyCollection<Version> AspNetCoreModuleVersions
        {
            get;
        }

        /// <summary>
        /// The friendly display name for the component.
        /// </summary>
        public override string Name => ReleasesResources.AspNetCoreReleaseName;

        /// <summary>
        /// The versions of Visual Studio that includes this runtime. Multiple versions may be listed, e.g.
        /// &quot;15.9.25, 16.0.16, 16.4.11, 16.6.4&quot;
        /// </summary>
        public string VisualStudioVersion
        {
            get;
        }

        internal AspNetCoreReleaseComponent(JToken token, ProductRelease release) : base(token, release)
        {
            AspNetCoreModuleVersions = new ReadOnlyCollection<Version>(token["version-aspnetcoremodule"]?.ToObject<Version[]>(Utils.DefaultSerializer) ?? new Version[] { });
            VisualStudioVersion = (string)token["vs-version"];
        }
    }
}
