// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    internal class InvalidFrameworkFolderRule : IPackageRule
    {
        private const string LibDirectory = "lib";

        public string MessageFormat { get; }

        public InvalidFrameworkFolderRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var file in builder.GetFiles().Select(t => PathUtility.GetPathWithDirectorySeparator(t)))
            {
                var parts = file.Split(Path.DirectorySeparatorChar);
                if (parts.Length >= 3 && parts[0].Equals(LibDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    set.Add(file);
                }
            }

            return set.Where(s => !FrameworkNameValidatorUtility.IsValidFrameworkName(s) && !FrameworkNameValidatorUtility.IsValidCultureName(builder, s))
                      .Select(CreatePackageIssue);
        }

        private PackagingLogMessage CreatePackageIssue(string target)
        {
            return PackagingLogMessage.CreateWarning(
                string.Format(CultureInfo.CurrentCulture, MessageFormat, target),
                NuGetLogCode.NU5103);
        }
    }
}
