// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Deployment.DotNet.Releases
{
    /// <summary>
    /// Describes an SDK release.
    /// </summary>
    public class SdkReleaseComponent : ReleaseComponent
    {
        /// <summary>
        /// The version of C# supported by this SDK or <see langword="null"/>.
        /// </summary>
        public string CSharpVersion
        {
            get;
        }

        /// <summary>
        /// The F# version supported by this SDK or <see langword="null"/>.
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
        /// The Visual Basic version supported by this SDK or <see langword="null"/>.
        /// </summary>
        public string VisualBasicVersion
        {
            get;
        }

        internal SdkReleaseComponent(JToken token, ProductRelease release) : base(token, release)
        {
            CSharpVersion = (string)token["csharp-version"];
            FSharpVersion = (string)token["fsharp-version"]; 
            VisualBasicVersion = (string)token["vb-version"];
            RuntimeVersion = token["runtime-version"]?.ToObject<ReleaseVersion>(Utils.DefaultSerializer);
            VisualStudioMacSupport = (string)token["vs-mac-support"];
            VisualStudioMacVersion = (string)token["vs-mac-version"];
            VisualStudioSupport = (string)token["vs-support"];
            VisualStudioVersion = (string)token["vs-version"];
        }
    }
}
