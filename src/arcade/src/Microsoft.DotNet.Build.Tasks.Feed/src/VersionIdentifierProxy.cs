// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.DotNet.VersionTools.BuildManifest;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    internal abstract class IVersionIdentifierProxy
    {
        internal abstract string GetVersion(string assetName);
        internal abstract string RemoveVersions(string assetName);
    }

    internal class VersionIdentifierProxy : IVersionIdentifierProxy
    {
        internal override string GetVersion(string assetName)
        {
            return VersionIdentifier.GetVersion(assetName);
        }

        internal override string RemoveVersions(string assetName)
        {
            return VersionIdentifier.RemoveVersions(assetName);
        }
    }
}
