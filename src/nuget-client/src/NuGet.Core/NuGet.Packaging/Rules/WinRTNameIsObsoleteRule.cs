// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    internal class WinRTNameIsObsoleteRule : IPackageRule
    {
        private static string[] Prefixes = new string[]
            { "content\\winrt45\\", "lib\\winrt45\\", "tools\\winrt45\\", "content\\winrt\\", "lib\\winrt\\", "tools\\winrt\\" };

        public string MessageFormat { get; }

        public WinRTNameIsObsoleteRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }
        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            foreach (var file in builder.GetFiles().Select(t => PathUtility.GetPathWithDirectorySeparator(t)))
            {
                foreach (var prefix in Prefixes)
                {
                    if (file.StartsWith(PathUtility.GetPathWithDirectorySeparator(prefix), StringComparison.OrdinalIgnoreCase))
                    {
                        yield return CreateIssue(file);
                    }
                }
            }
        }

        private PackagingLogMessage CreateIssue(string file)
        {
            return PackagingLogMessage.CreateWarning(
                string.Format(CultureInfo.CurrentCulture, MessageFormat, file),
                NuGetLogCode.NU5106);
        }
    }
}
