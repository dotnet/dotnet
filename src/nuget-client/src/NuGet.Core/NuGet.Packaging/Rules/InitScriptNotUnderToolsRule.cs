// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NuGet.Common;

namespace NuGet.Packaging.Rules
{
    internal class InitScriptNotUnderToolsRule : IPackageRule
    {
        public string MessageFormat { get; }

        public InitScriptNotUnderToolsRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }
        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            foreach (var file in builder.GetFiles())
            {
                var name = Path.GetFileName(file);
                var dirName = Path.GetFileName(Path.GetDirectoryName(file));
                if (name.Equals("init.ps1", StringComparison.OrdinalIgnoreCase) && !dirName.Equals(PackagingConstants.Folders.Tools, StringComparison.OrdinalIgnoreCase))
                {
                    yield return CreatePackageIssue(file);
                }
            }
        }

        private PackagingLogMessage CreatePackageIssue(string file)
        {
            return PackagingLogMessage.CreateWarning(
                string.Format(CultureInfo.CurrentCulture, MessageFormat, file),
                NuGetLogCode.NU5107);
        }
    }
}
