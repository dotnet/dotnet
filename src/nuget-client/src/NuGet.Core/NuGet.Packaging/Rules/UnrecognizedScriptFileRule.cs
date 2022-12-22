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
    internal class UnrecognizedScriptFileRule : IPackageRule
    {
        private const string ScriptExtension = ".ps1";

        public string MessageFormat { get; }

        public UnrecognizedScriptFileRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            foreach (var file in builder.GetFiles().Select(t => PathUtility.GetPathWithDirectorySeparator(t)))
            {
                if (!file.EndsWith(ScriptExtension, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                else
                {
                    var name = Path.GetFileNameWithoutExtension(file);
                    if (!name.Equals("install", StringComparison.OrdinalIgnoreCase) &&
                        !name.Equals("uninstall", StringComparison.OrdinalIgnoreCase) &&
                        !name.Equals("init", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return CreatePackageIssueForUnrecognizedScripts(file);
                    }
                }
            }
        }

        private PackagingLogMessage CreatePackageIssueForUnrecognizedScripts(string target)
        {
            return PackagingLogMessage.CreateWarning(
                String.Format(CultureInfo.CurrentCulture, MessageFormat, target),
                NuGetLogCode.NU5111);
        }
    }
}
