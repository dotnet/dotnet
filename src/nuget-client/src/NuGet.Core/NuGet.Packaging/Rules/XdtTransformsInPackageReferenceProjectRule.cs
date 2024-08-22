// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    internal class XdtTransformInPackageReferenceProjectRule : IPackageRule
    {
        private const string ConfigTransformExtension = ".transform";
        private const string InstallXdtExtension = ".install.xdt";
        private const string UninstallXdtExtension = ".uninstall.xdt";
        private const string ContentDirectory = "content/";
        private const string ContentFilesDirectory = "contentFiles/";

        public string MessageFormat { get; }

        public XdtTransformInPackageReferenceProjectRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            foreach (var file in builder.GetFiles()
                .Where(f => f.StartsWith(ContentDirectory, StringComparison.OrdinalIgnoreCase) || f.StartsWith(ContentFilesDirectory, StringComparison.OrdinalIgnoreCase))
                .Select(t => PathUtility.GetPathWithDirectorySeparator(t)))
            {
                if (file.EndsWith(ConfigTransformExtension, StringComparison.OrdinalIgnoreCase)
                    || file.EndsWith(InstallXdtExtension, StringComparison.OrdinalIgnoreCase)
                    || file.EndsWith(UninstallXdtExtension, StringComparison.OrdinalIgnoreCase))
                {
                    yield return CreatePackageIssueForTransformFiles(file);
                }
            }
        }

        private PackagingLogMessage CreatePackageIssueForTransformFiles(string path)
        {
            return PackagingLogMessage.CreateWarning(
                string.Format(CultureInfo.CurrentCulture, MessageFormat, path),
                NuGetLogCode.NU5122);
        }
    }
}
