// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Describes an SDK release.
    /// </summary>
    public class SdkReleaseComponent : ReleaseComponent
    {
        /// <summary>
        /// The C# language version supported by this SDK.
        /// </summary>
        public string CSharpVersion
        {
            get;
        }

        /// <summary>
        /// The F# language version supported by this SDK.
        /// </summary>
        public string FSharpVersion
        {
            get;
        }

        /// <summary>
        /// The friendly display name for the component.
        /// </summary>
        public override string Name => ReleasesResources.SdkReleaseName;

        /// <summary>
        /// The version of the runtime included with the SDK.
        /// </summary>
        public ReleaseVersion RuntimeVersion
        {
            get;
        }

        /// <summary>
        /// The versions of Visual Studio for Mac that supports this SDK or <see langword="null"/>. 
        /// </summary>
        public string VisualStudioMacSupport
        {
            get;
        }

        /// <summary>
        /// The versions of Visual Studio for Mac that includes this SDK or <see langword="null"/>. 
        /// </summary>
        public string VisualStudioMacVersion
        {
            get;
        }

        /// <summary>
        /// The versions of Visual Studio that support this SDK or <see langword="null"/>.
        /// </summary>
        public string VisualStudioSupport
        {
            get;
        }

        /// <summary>
        /// The versions of Visual Studio that includes this SDK. Multiple versions may be listed, e.g.
        /// &quot;15.9.25, 16.0.16, 16.4.11, 16.6.4&quot;
        /// </summary>
        public string VisualStudioVersion
        {
            get;
        }

        /// <summary>
        /// The Visual Basic language version supported by this SDK.
        /// </summary>
        public string VisualBasicVersion
        {
            get;
        }

        /// <summary>
        /// Creates a new <see cref="SdkReleaseComponent"/> instance.
        /// </summary>
        /// <param name="element">The JSON element of the component.</param>
        /// <param name="release">The release to which the component belongs.</param>
        internal SdkReleaseComponent(JsonElement element, ProductRelease release) : base(element, release)
        {
            CSharpVersion = element.GetStringOrDefault("csharp-version");
            FSharpVersion = element.GetStringOrDefault("fsharp-version");
            VisualBasicVersion = element.GetStringOrDefault("vb-version");
            RuntimeVersion = element.GetReleaseVersionOrDefault("runtime-version");
            VisualStudioMacSupport = element.GetStringOrDefault("vs-mac-support");
            VisualStudioMacVersion = element.GetStringOrDefault("vs-mac-version");
            VisualStudioSupport = element.GetStringOrDefault("vs-support");
            VisualStudioVersion = element.GetStringOrDefault("vs-version");
        }
    }
}
