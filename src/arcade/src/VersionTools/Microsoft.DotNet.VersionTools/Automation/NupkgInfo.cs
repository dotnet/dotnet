// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public class NupkgInfo
    {
        public NupkgInfo(PackageIdentity identity)
        {
            Id = identity.Id;
            Version = identity.Version;
        }

        public string Id { get; }
        public string Version { get; }
        public string Prerelease { get { throw new NotImplementedException();} }

        public static bool IsSymbolPackagePath(string path) => path.EndsWith(".symbols.nupkg");
    }

    public class PackageIdentity
    {
        public PackageIdentity(string id, string version)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
            Version = version;
        }

        public string Id { get; }

        public string Version { get; }
    }
}
