using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    internal class ContentFolderInPackageReferenceProjectRule : IPackageRule
    {
        public string MessageFormat
        {
            get;
        }

        public ContentFolderInPackageReferenceProjectRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            if (builder.GetFiles()
                .Select(t => PathUtility.GetPathWithDirectorySeparator(t))
                .Any(t => t.StartsWith
                    (PackagingConstants.Folders.Content + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)))
            {
                // if package has content folder but no contentFiles folder
                if (!builder.GetFiles()
                .Select(t => PathUtility.GetPathWithDirectorySeparator(t))
                .Any(t => t.StartsWith
                    (PackagingConstants.Folders.ContentFiles + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)))
                {
                    var issue = new List<PackagingLogMessage>();
                    issue.Add(PackagingLogMessage.CreateWarning(
                        string.Format(CultureInfo.CurrentCulture, MessageFormat), NuGetLogCode.NU5121));
                    return issue;
                }
            }

            return new List<PackagingLogMessage>();
        }
    }
}
