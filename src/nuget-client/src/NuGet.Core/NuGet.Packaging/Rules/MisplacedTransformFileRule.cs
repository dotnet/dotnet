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
    internal class MisplacedTransformFileRule : IPackageRule
    {
        private const string CodeTransformExtension = ".pp";
        private const string ConfigTransformExtension = ".transform";
        private const string ContentDirectory = "content";
        private const string ContentFilesDirectory = "contentFiles";

        public string MessageFormat { get; }

        public MisplacedTransformFileRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            foreach (var file in builder.GetFiles().Select(t => PathUtility.GetPathWithDirectorySeparator(t)))
            {
                // if not a .transform file, ignore
                if (!file.EndsWith(CodeTransformExtension, StringComparison.OrdinalIgnoreCase) &&
                    !file.EndsWith(ConfigTransformExtension, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // if not inside 'content' folder, warn
                if (!file.StartsWith(ContentDirectory + Path.DirectorySeparatorChar,
                    StringComparison.OrdinalIgnoreCase)
                    && !file.StartsWith(ContentFilesDirectory + Path.DirectorySeparatorChar,
                    StringComparison.OrdinalIgnoreCase))
                {
                    yield return CreatePackageIssueForMisplacedContent(file);
                }
            }
        }

        private PackagingLogMessage CreatePackageIssueForMisplacedContent(string path)
        {
            return PackagingLogMessage.CreateWarning(
                String.Format(CultureInfo.CurrentCulture, MessageFormat, path),
                NuGetLogCode.NU5108);
        }
    }
}
