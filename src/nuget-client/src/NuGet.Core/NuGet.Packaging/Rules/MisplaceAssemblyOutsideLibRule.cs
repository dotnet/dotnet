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
    internal class MisplacedAssemblyOutsideLibRule : IPackageRule
    {
        public string MessageFormat { get; }

        public MisplacedAssemblyOutsideLibRule(string messageFormat)
        {
            MessageFormat = messageFormat;
        }
        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            foreach (var packageFile in builder.GetFiles())
            {
                var file = PathUtility.GetPathWithDirectorySeparator(packageFile);
                var directory = Path.GetDirectoryName(file);

                if (!ValidFolders.Any(folder => file.StartsWith(folder, StringComparison.OrdinalIgnoreCase)))
                {
                    // when checking for assemblies outside 'lib' folder, only check .dll files.
                    // .exe files are often legitimate outside 'lib'.
                    if (file.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ||
                        file.EndsWith(".winmd", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return CreatePackageIssueForAssembliesOutsideLib(file);
                    }
                }
            }
        }

        private PackagingLogMessage CreatePackageIssueForAssembliesOutsideLib(string target)
        {
            return PackagingLogMessage.CreateWarning(
                string.Format(CultureInfo.CurrentCulture, MessageFormat, target),
                NuGetLogCode.NU5100);
        }

        /// <summary>
        /// Folders that are expected to have .dll and .winmd files
        /// </summary>
        private static IEnumerable<string> ValidFolders
        {
            get
            {
                yield return PackagingConstants.Folders.Lib + Path.DirectorySeparatorChar;
                yield return PackagingConstants.Folders.Analyzers + Path.DirectorySeparatorChar;
                yield return PackagingConstants.Folders.Ref + Path.DirectorySeparatorChar;
                yield return PackagingConstants.Folders.Runtimes + Path.DirectorySeparatorChar;
                yield return PackagingConstants.Folders.Native + Path.DirectorySeparatorChar;
                yield return PackagingConstants.Folders.Build + Path.DirectorySeparatorChar;
                yield return PackagingConstants.Folders.BuildCrossTargeting + Path.DirectorySeparatorChar;
                yield return PackagingConstants.Folders.Tools + Path.DirectorySeparatorChar;
                yield break;
            }
        }
    }
}
