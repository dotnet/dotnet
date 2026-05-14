// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public static class VersionUtility
    {
        public static readonly Version MaxVersion = new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);

        public static bool IsCompatibleApiVersion(Version referenceVersion, Version definitionVersion)
        {
            return (referenceVersion.Major == definitionVersion.Major &&
                referenceVersion.Minor == definitionVersion.Minor &&
                    (referenceVersion.Build < definitionVersion.Build || // If the Build number is greater, then we don't need to check revision
                    (referenceVersion.Build == definitionVersion.Build && referenceVersion.Revision <= definitionVersion.Revision)));
        }


        public static Version GetAssemblyVersion(string assemblyPath)
        {
            using (var fileStream = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read))
            {
                return GetAssemblyVersion(fileStream);
            }
        }

        public static Version GetAssemblyVersion(Stream assemblyStream)
        {
            Version result = null;
            try
            {
                using (PEReader peReader = new PEReader(assemblyStream, PEStreamOptions.LeaveOpen))
                {
                    if (peReader.HasMetadata)
                    {
                        MetadataReader reader = peReader.GetMetadataReader();
                        if (reader.IsAssembly)
                        {
                            result = reader.GetAssemblyDefinition().Version;
                        }
                    }
                }
            }
            catch (BadImageFormatException)
            {
                // not a PE
            }

            return result;
        }

        public static Version As3PartVersion(Version version)
        {
            if (version == null)
                return null;

            int build = version.Build;

            if (build == -1)
            {
                // we have a 2-part version
                build = 0;
            }
            else if (version.Revision == -1)
            {
                // we already have a 3-part version
                return version;
            }

            return new Version(version.Major, version.Minor, build);
        }

        public static Version As2PartVersion(Version version)
        {
            if (version == null)
                return null;

            if (version.Build == -1)
            {
                // we already have a 2-part version
                return version;
            }
            return new Version(version.Major, version.Minor);
        }

        public static Version As4PartVersion(Version version)
        {
            if (version == null)
                return null;

            int build = version.Build, revision = version.Revision;

            if (build == -1)
            {
                // we have a 2-part version
                build = 0;
                revision = 0;
            }
            else if (revision == -1)
            {
                // we have a 3-part version
                revision = 0;
            }
            else
            {
                // we already have a 4-part version
                return version;
            }

            return new Version(version.Major, version.Minor, build, revision);
        }
    }
}
