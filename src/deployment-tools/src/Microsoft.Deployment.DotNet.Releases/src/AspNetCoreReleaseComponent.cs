// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

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

        /// <summary>
        /// Creates a new <see cref="AspNetCoreReleaseComponent"/> instance.
        /// </summary>
        /// <param name="element">The JSON element of the component.</param>
        /// <param name="release">The release to which the component belongs.</param>
        internal AspNetCoreReleaseComponent(JsonElement element, ProductRelease release) : base(element, release)
        {
            AspNetCoreModuleVersions = element.TryGetProperty("version-aspnetcoremodule", out JsonElement ancmVersionValue) && ancmVersionValue.ValueKind != JsonValueKind.Null ?
                new ReadOnlyCollection<Version>(JsonSerializer.Deserialize<Version[]>(ancmVersionValue, SerializerOptions.Default)) : new ReadOnlyCollection<Version>(new List<Version>());
            VisualStudioVersion = element.GetStringOrDefault("vs-version");
        }
    }
}
