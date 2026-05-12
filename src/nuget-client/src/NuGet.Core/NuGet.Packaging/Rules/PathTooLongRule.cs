using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    public class PathTooLongRule : IPackageRule
    {
        private const int _pathLenghtWarningThreshold = 200;
        public string MessageFormat { get; }

        public PathTooLongRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            var packageIdentity = builder.GetIdentity();
            var versionFolderPathResolver = new VersionFolderPathResolver(string.Empty);
            var installedPath = versionFolderPathResolver.GetInstallPath(packageIdentity.Id, packageIdentity.Version);

            foreach (var file in builder.GetFiles())
            {
                if (Path.Combine(installedPath, file).Length > _pathLenghtWarningThreshold)
                {
                    yield return CreatePackageIssueForPathTooLong(file);
                }
            }
        }

        private PackagingLogMessage CreatePackageIssueForPathTooLong(string target)
        {
            return PackagingLogMessage.CreateWarning(
               string.Format(CultureInfo.CurrentCulture, MessageFormat, target),
               NuGetLogCode.NU5123);
        }
    }
}
