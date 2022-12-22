// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NuGet.Packaging.Core;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public class NupkgInfo
    {
        public NupkgInfo(PackageIdentity identity)
        {
            Id = identity.Id;
            Version = identity.Version.ToString();
            Prerelease = identity.Version.Release;
        }

        public string Id { get; }
        public string Version { get; }
        public string Prerelease { get; }

        public static bool IsSymbolPackagePath(string path) => path.EndsWith(".symbols.nupkg");
    }
}
