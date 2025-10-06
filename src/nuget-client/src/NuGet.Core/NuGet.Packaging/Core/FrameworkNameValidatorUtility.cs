using System;
using System.IO;
using NuGet.Frameworks;

namespace NuGet.Packaging.Rules
{
    internal static class FrameworkNameValidatorUtility
    {
        internal static bool IsValidFrameworkName(NuGetFramework framework)
        {
            return IsValidFrameworkName(PackagingConstants.Folders.Build + Path.DirectorySeparatorChar
                + framework.GetShortFolderName() + Path.DirectorySeparatorChar);
        }

        internal static bool IsValidFrameworkName(string path)
        {
            NuGetFramework fx;
            try
            {
                string effectivePath;
                fx = FrameworkNameUtility.ParseNuGetFrameworkFromFilePath(path, out effectivePath);
            }
            catch (ArgumentException)
            {
                fx = null;
            }

            // return false if the framework is Null or Unsupported
            return fx != null && fx.Framework != NuGetFramework.UnsupportedFramework.Framework;
        }

        internal static bool IsValidCultureName(PackageArchiveReader builder, string name)
        {
            // starting from NuGet 1.8, we support localized packages, which
            // can have a culture folder under lib, e.g. lib\fr-FR\strings.resources.dll
            var nuspecReader = builder.NuspecReader;
            if (string.IsNullOrEmpty(nuspecReader.GetLanguage()))
            {
                return false;
            }

            // the folder name is considered valid if it matches the package's Language property.
            return name.Equals(nuspecReader.GetLanguage(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
