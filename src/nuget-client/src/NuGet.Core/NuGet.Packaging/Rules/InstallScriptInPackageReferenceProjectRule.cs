using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    internal class InstallScriptInPackageReferenceProjectRule : IPackageRule
    {
        public string MessageFormat { get; }

        public InstallScriptInPackageReferenceProjectRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            foreach (var toolItem in builder.GetFiles()
                .Select(t => PathUtility.GetPathWithDirectorySeparator(t))
                .Where(t =>
                    t.StartsWith(PackagingConstants.Folders.Tools + Path.DirectorySeparatorChar,
                    StringComparison.OrdinalIgnoreCase)))
            {
                if (toolItem.EndsWith("install.ps1", StringComparison.OrdinalIgnoreCase))
                {
                    var issue = new List<PackagingLogMessage>();
                    issue.Add(PackagingLogMessage.CreateWarning(
                        string.Format(CultureInfo.CurrentCulture, MessageFormat), NuGetLogCode.NU5120));
                    return issue;
                }
            }

            return new List<PackagingLogMessage>();
        }
    }
}
