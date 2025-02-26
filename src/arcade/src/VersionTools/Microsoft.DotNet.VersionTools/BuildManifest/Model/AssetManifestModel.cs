// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.BuildManifest.Model
{
    public class AssetManifestModel : ArtifactModel
    {
        /// <summary>
        /// The asset manifest is not not represented as XML
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override XElement ToXml() => throw new NotImplementedException();
    }
}
