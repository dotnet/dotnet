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
    internal class MisplacedScriptFileRule : IPackageRule
    {
        private const string ScriptExtension = ".ps1";
        private static readonly string ToolsDirectory = PackagingConstants.Folders.Tools;

        public string MessageFormat { get; }

        public MisplacedScriptFileRule(string messageFormat)
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

                if (!file.StartsWith(ToolsDirectory + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                {
                    yield return CreatePackageIssueForMisplacedScript(file);
                }
            }
        }

        private PackagingLogMessage CreatePackageIssueForMisplacedScript(string target)
        {
            return PackagingLogMessage.CreateWarning(
                string.Format(CultureInfo.CurrentCulture, MessageFormat, target),
                NuGetLogCode.NU5110);
        }
    }
}
