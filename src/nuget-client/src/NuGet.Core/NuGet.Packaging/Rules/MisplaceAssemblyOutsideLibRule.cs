// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        internal MisplacedAssemblyOutsideLibRule()
            : this(AnalysisResources.AssemblyOutsideLibWarning)
        {
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            return Validate(builder.GetFiles);
        }

        internal IEnumerable<PackagingLogMessage> Validate(Func<IEnumerable<string>> getFiles)
        {
            foreach (var packageFile in getFiles())
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
        private static ImmutableArray<string> ValidFolders =
        [
            PackagingConstants.Folders.Lib + Path.DirectorySeparatorChar,
            PackagingConstants.Folders.Analyzers + Path.DirectorySeparatorChar,
            PackagingConstants.Folders.Ref + Path.DirectorySeparatorChar,
            PackagingConstants.Folders.Runtimes + Path.DirectorySeparatorChar,
            PackagingConstants.Folders.Native + Path.DirectorySeparatorChar,
            PackagingConstants.Folders.Build + Path.DirectorySeparatorChar,
            PackagingConstants.Folders.BuildCrossTargeting + Path.DirectorySeparatorChar,
            PackagingConstants.Folders.BuildTransitive + Path.DirectorySeparatorChar,
            PackagingConstants.Folders.Tools + Path.DirectorySeparatorChar,
         ];
    }
}
