// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Packaging.Core;

namespace NuGet.Packaging.Rules
{
    internal class InvalidPlaceholderFileRule : IPackageRule
    {
        public string MessageFormat { get; }

        public InvalidPlaceholderFileRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }
        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            foreach (var file in builder.GetFiles())
            {
                if (Path.GetFileName(file).Equals(PackagingCoreConstants.EmptyFolder, StringComparison.Ordinal))
                {
                    var directory = PathUtility.EnsureTrailingSlash(Path.GetDirectoryName(file));
                    if (builder.GetFiles().Count(f => PathUtility.EnsureTrailingSlash(Path.GetDirectoryName(f)).StartsWith(directory, StringComparison.OrdinalIgnoreCase)) > 1)
                    {
                        yield return CreatePackageIssueForPlaceholderFile(file);
                    }
                }
            }
        }

        private PackagingLogMessage CreatePackageIssueForPlaceholderFile(string target)
        {
            return PackagingLogMessage.CreateWarning(
                String.Format(CultureInfo.CurrentCulture, MessageFormat, target),
                NuGetLogCode.NU5109);
        }
    }
}
