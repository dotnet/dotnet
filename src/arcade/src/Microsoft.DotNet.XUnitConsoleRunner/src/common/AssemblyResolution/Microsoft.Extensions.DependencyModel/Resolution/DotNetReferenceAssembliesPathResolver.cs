// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Internal.Microsoft.DotNet.PlatformAbstractions;

namespace Internal.Microsoft.Extensions.DependencyModel.Resolution
{
    internal class DotNetReferenceAssembliesPathResolver
    {
        public static readonly string DotNetReferenceAssembliesPathEnv = "DOTNET_REFERENCE_ASSEMBLIES_PATH";

        internal static string Resolve(IEnvironment envirnment, IFileSystem fileSystem)
        {
            var path = envirnment.GetEnvironmentVariable(DotNetReferenceAssembliesPathEnv);
            if (!string.IsNullOrEmpty(path))
            {
                return path;
            }

            return GetDefaultDotNetReferenceAssembliesPath(fileSystem);
        }

        private static string GetDefaultDotNetReferenceAssembliesPath(IFileSystem fileSystem)
        {
            var os = RuntimeEnvironment.OperatingSystemPlatform;

            if (os == Platform.Windows)
            {
                return null;
            }

            if (os == Platform.Darwin &&
                fileSystem.Directory.Exists("/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/xbuild-frameworks"))
            {
                return "/Library/Frameworks/Mono.framework/Versions/Current/lib/mono/xbuild-frameworks";
            }

            if (fileSystem.Directory.Exists("/usr/local/lib/mono/xbuild-frameworks"))
            {
                return "/usr/local/lib/mono/xbuild-frameworks";
            }

            if (fileSystem.Directory.Exists("/usr/lib/mono/xbuild-frameworks"))
            {
                return "/usr/lib/mono/xbuild-frameworks";
            }

            return null;
        }
    }
}
