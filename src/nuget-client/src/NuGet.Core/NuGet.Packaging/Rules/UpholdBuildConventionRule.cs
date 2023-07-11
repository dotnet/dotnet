// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using NuGet.Client;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.RuntimeModel;

namespace NuGet.Packaging.Rules
{
    internal class UpholdBuildConventionRule : IPackageRule
    {
        private static ManagedCodeConventions ManagedCodeConventions = new ManagedCodeConventions(RuntimeGraph.Empty);
        private static readonly IReadOnlyList<string> BuildFolders = new string[]
        {
            PackagingConstants.Folders.Build,
            PackagingConstants.Folders.BuildTransitive,
            PackagingConstants.Folders.BuildCrossTargeting,
            "buildMultiTargeting"
        };

        public string MessageFormat => AnalysisResources.BuildConventionIsViolatedWarning;

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader builder)
        {
            var packageId = builder.NuspecReader.GetId();
            var filesInPackage = builder.GetFiles();

            var expectedFiles = FindAbsentExpectedFiles(filesInPackage, packageId);
            var warning = GenerateWarning(expectedFiles);
            return warning == null
                ? Array.Empty<PackagingLogMessage>()
                : new[] { warning };
        }

        internal PackagingLogMessage GenerateWarning(ICollection<ExpectedFile> expectedFiles)
        {
            if (expectedFiles.Count == 0)
            {
                return null;
            }

            var warningMessage = new StringBuilder();
            foreach (var expectedFile in expectedFiles)
            {
                warningMessage.AppendLine(string.Format(CultureInfo.CurrentCulture, MessageFormat, expectedFile.Extension, expectedFile.Path, expectedFile.ExpectedPath));
            }

            var message = PackagingLogMessage.CreateWarning(warningMessage.ToString(), NuGetLogCode.NU5129);
            return message;
        }

        internal List<ExpectedFile> FindAbsentExpectedFiles(IEnumerable<string> files, string packageId)
        {
            var expectedFiles = new List<ExpectedFile>();

            var normalizedFiles = files.Select(PathUtility.GetPathWithForwardSlashes);
            var msbuildFiles = normalizedFiles.Where(EndsWithMsbuildFileExtension);
            var msbuildFilesInBuildFolder = msbuildFiles.Where(InsideBuildFolder);
            var msbuildFilesGroupedByBuildFolder = msbuildFilesInBuildFolder.GroupBy(GetBuildFolder);

            foreach (var buildFolder in msbuildFilesGroupedByBuildFolder)
            {
                foreach (var tfm in buildFolder.GroupBy(GetTfm))
                {
                    foreach (var extension in tfm.GroupBy(Path.GetExtension))
                    {
                        string expectedFileName = tfm.Key == string.Empty
                            ? buildFolder.Key + packageId + extension.Key
                            : buildFolder.Key + tfm.Key + '/' + packageId + extension.Key;
                        if (!extension.Any(file => string.Equals(expectedFileName, file, StringComparison.OrdinalIgnoreCase)))
                        {
                            string packageFolder = tfm.Key == string.Empty ? buildFolder.Key : buildFolder.Key + tfm.Key + '/';
                            expectedFiles.Add(new ExpectedFile(packageFolder, extension.Key, expectedFileName));
                        }
                    }
                }
            }

            expectedFiles.Sort(ExpectedFileComparer.Instance);

            return expectedFiles;
        }

        private string GetTfm(string file)
        {
#if NETCOREAPP
            var index1 = file.IndexOf('/', StringComparison.Ordinal);
            var index2 = file.IndexOf('/', index1 + 1);
#else
            var index1 = file.IndexOf('/');
            var index2 = file.IndexOf('/', index1 + 1);
#endif
            if (index2 == -1)
            {
                return string.Empty;
            }

            var folderName = file.Substring(index1 + 1, index2 - index1 - 1);
            var framework = NuGetFramework.ParseFolder(folderName);

            return framework.IsUnsupported ? string.Empty : folderName;
        }

        private string GetBuildFolder(string file)
        {
#if NETCOREAPP
            var index = file.IndexOf('/', StringComparison.Ordinal);
#else
            var index = file.IndexOf('/');
#endif
            return file.Substring(0, index + 1);
        }

        private bool EndsWithMsbuildFileExtension(string file)
        {
            foreach (var extension in ManagedCodeConventions.Properties["msbuild"].FileExtensions)
            {
                if (file.EndsWith(extension, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private bool InsideBuildFolder(string file)
        {
            foreach (var buildFolder in BuildFolders)
            {
                if (file.StartsWith(buildFolder, StringComparison.Ordinal) && file[buildFolder.Length] == '/')
                {
                    return true;
                }
            }

            return false;
        }

        internal class ExpectedFile
        {
            public string Path { get; }

            public string Extension { get; }

            public string ExpectedPath { get; }

            public ExpectedFile(string filePath, string extension, string expectedFile)
            {
                if (filePath[filePath.Length - 1] != '/' && filePath[filePath.Length - 1] != '\\')
                {
                    throw new ArgumentException("Path must end with directory separator", nameof(filePath));
                }

                if (extension[0] != '.')
                {
                    throw new ArgumentException("Extension must include period character", nameof(extension));
                }

                Path = filePath;
                Extension = extension;
                ExpectedPath = expectedFile;
            }
        }

        internal class ExpectedFileComparer : IComparer<ExpectedFile>
        {
            internal static ExpectedFileComparer Instance { get; } = new ExpectedFileComparer();

            public int Compare(ExpectedFile x, ExpectedFile y)
            {
                var result = string.Compare(x.Path, y.Path, StringComparison.Ordinal);
                if (result != 0)
                {
                    return result;
                }

                result = string.Compare(x.Extension, y.Extension, StringComparison.Ordinal);
                if (result != 0)
                {
                    return result;
                }

                return string.Compare(x.ExpectedPath, y.ExpectedPath, StringComparison.Ordinal);
            }
        }
    }
}
