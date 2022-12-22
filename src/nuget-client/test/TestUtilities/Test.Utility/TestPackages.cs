// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using NuGet.Test.Utility;
using NuGet.Versioning;

namespace Test.Utility
{
    public static class TestPackagesGroupedByFolder
    {
        private static readonly string NuspecStringFormat = @"<?xml version=""1.0"" encoding=""utf-8""?>
                            <package xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
                              <metadata{3}>
                                <id>{0}</id>
                                <version>{1}</version>
                                <authors>Author1, author2</authors>
                                <description>Sample description</description>
                                <language>en-US</language>
                                <projectUrl>http://www.nuget.org/</projectUrl>
                                <licenseUrl>http://www.nuget.org/license</licenseUrl>
                                {2}
                              </metadata>
                            </package>";

        private static readonly string FrameworkAssembliesStringFormat = @"<frameworkAssemblies>
            <frameworkAssembly assemblyName='{0}' targetFramework='{1}' />
        </frameworkAssemblies>";

        private static readonly string DependenciesStringFormat = @"<dependencies>
            <dependency id='{0}' version='{1}' />
        </dependencies>";

        public static FileInfo GetLegacyTestPackage(string path,
            string packageId = "packageA",
            string packageVersion = "2.0.3")
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "lib/test.dll",
                    "lib/net40/test40.dll",
                    "lib/net40/test40b.dll",
                    "lib/net45/test45.dll"
                });
        }

        public static FileInfo GetLegacyTestPackageWithMultipleReferences(string path,
            string packageId = "packageA",
            string packageVersion = "2.0.3")
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "lib/net45/a.dll",
                    "lib/net45/b.dll"
                });
        }

        public static FileInfo GetNet45TestPackage(string path,
            string packageId = "packageA",
            string packageVersion = "2.0.3")
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "tools/tool.exe",
                    "lib/net45/test45.dll"
                });
        }

        public static FileInfo GetEmptyNet45TestPackage(string path,
            string packageId = "packageA",
            string packageVersion = "2.0.3")
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "lib/net45/"
                });
        }

        public static FileInfo GetNet45TestPackageWithDummyFile(string path,
            string packageId = "packageA",
            string packageVersion = "2.0.3")
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "lib/net45/_._"
                });
        }

        public static FileInfo GetTestPackageWithDummyFile(string path,
            string packageId = "packageA",
            string packageVersion = "2.0.3")
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "lib/_._"
                });
        }

        public static FileInfo GetMixedPackage(string path, string baseName, string packageId, string packageVersion)
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "tools/" + baseName + ".exe",
                    "lib/net45/" + baseName + ".dll",
                    "Content/Scripts/" + baseName + ".js",
                    "Content/"+ baseName + "/" + baseName + "." + baseName
                });
        }

        public static FileInfo GetLegacyContentPackage(string path, string packageId, string packageVersion)
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "Content/",
                    "Content/Scripts/",
                    "Content/Scripts/test1.js",
                    "Content/Scripts/test2.js",
                    "Content/Scripts/test3.js"
                });
        }

        public static FileInfo GetPackageWithPPFiles(string path, string packageId, string packageVersion)
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "Content/",
                    "Content/Bar.cs.pp",
                    "Content/Foo.cs.pp"
                });
        }

        public static FileInfo GetContentPackageWithTargetFramework(string path,
            string packageId,
            string packageVersion)
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "Content/",
                    "Content/net45/",
                    "Content/net45/Scripts/",
                    "Content/net45/Scripts/net45test1.js",
                    "Content/net45/Scripts/net45test2.js",
                    "Content/net45/Scripts/net45test3.js"
                });
        }

        public static FileInfo GetPackageWithWebConfigTransform(string path,
            string packageId,
            string packageVersion,
            string webConfigTransformContent)
        {
            return GeneratePackage(
                path,
                packageId,
                packageVersion,
                new[]
                {
                    "Content/",
                    "Content/web.config.transform",
                },
                new[]
                {
                    string.Empty,
                    webConfigTransformContent
                },
                frameworkAssemblies: false,
                minClientVersion: null,
                dependencies: false);
        }

        public static FileInfo GetPackageWithBuildFiles(string path, string packageId, string packageVersion)
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "build/net45/" + packageId + ".targets"
                });
        }

        public static FileInfo GetPackageWithEmptyFolders(string path, string packageId, string packageVersion)
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "build/net45/_._",
                    "lib/net45/_._",
                    "content/_._"
                });
        }

        public static FileInfo GetPackageWithFrameworkReference(string path,
            string packageId = "packageA",
            string packageVersion = "2.0.3")
        {
            return GeneratePackage(
                path,
                packageId,
                packageVersion,
                new string[] { },
                frameworkAssemblies: true,
                minClientVersion: null,
                dependencies: false);
        }

        public static FileInfo GetPackageWithPowershellScripts(string path, string packageId, string packageVersion)
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "tools/InIT.ps1",
                    "tools/net45/inSTAll.ps1",
                    "tools/net45/UNinSTAll.ps1"
                });
        }

        public static FileInfo GetLegacySolutionLevelPackage(string path, string packageId, string packageVersion)
        {
            return GeneratePackage(path, packageId, packageVersion,
                new[]
                {
                    "tools/tool.exe"
                });
        }

        public static FileInfo GetInvalidPackage(string path, string packageId, string packageVersion)
        {
            return GeneratePackage(path, packageId, packageVersion, Array.Empty<string>());
        }

        public static FileInfo GetEmptyPackageWithDependencies(string path, string packageId, string packageVersion)
        {
            return GeneratePackage(
                path,
                packageId,
                packageVersion,
                new string[] { },
                frameworkAssemblies: false,
                minClientVersion: null,
                dependencies: true);
        }

        public static FileInfo GetPackageWithMinClientVersion(string path,
            string packageId,
            string packageVersion,
            SemanticVersion minClientVersion)
        {
            return GeneratePackage(
                path,
                packageId,
                packageVersion,
                new string[] { },
                frameworkAssemblies: false,
                minClientVersion: minClientVersion,
                dependencies: false);
        }

        public static FileInfo GetLargePackage(string path,
            string packageId,
            string packageVersion)
        {
            string[] packageFiles = new[]
            {
                "tools/file1",
                "tools/file2"
            };


            StringBuilder sb = new();
            for (int i = 0; i < 10000; i++)
            {
                sb.AppendLine(i.ToString(CultureInfo.CurrentCulture));
            }
            string contents = sb.ToString();

            string[] fileContents = new string[packageFiles.Length];

            for (int i = 0; i < fileContents.Length; i++)
            {
                fileContents[i] = contents;
            }

            return GeneratePackage(path,
                packageId,
                packageVersion,
                packageFiles,
                fileContents,
                frameworkAssemblies: false,
                minClientVersion: null,
                dependencies: false);
        }

        private static FileInfo GeneratePackage(
            string path,
            string packageId,
            string packageVersion,
            string[] zipEntries)
        {
            return GeneratePackage(
                path,
                packageId,
                packageVersion,
                zipEntries,
                frameworkAssemblies: false,
                minClientVersion: null,
                dependencies: false);
        }

        private static FileInfo GeneratePackage(
            string path,
            string packageId,
            string packageVersion,
            string[] zipEntries,
            bool frameworkAssemblies,
            SemanticVersion minClientVersion,
            bool dependencies)
        {
            var zipContents = Enumerable.Repeat(string.Empty, zipEntries.Length).ToArray();
            return GeneratePackage(
                path,
                packageId,
                packageVersion,
                zipEntries,
                zipContents,
                frameworkAssemblies,
                minClientVersion,
                dependencies);
        }

        private static FileInfo GeneratePackage(
            string path,
            string packageId,
            string packageVersion,
            string[] zipEntries,
            string[] zipContents,
            bool frameworkAssemblies,
            SemanticVersion minClientVersion,
            bool dependencies)
        {
            if (zipEntries == null || zipContents == null || zipEntries.Length != zipContents.Length)
            {
                throw new InvalidOperationException("TEST Exception: zipEntries and zipContents should be non-null and" +
                    "zipEntries.Length should be equal to zipContents.Length");
            }

            var fileInfo = GetFileInfo(path, packageId, packageVersion);

            using (var zip = new ZipArchive(File.Create(fileInfo.FullName), ZipArchiveMode.Create))
            {
                for (int i = 0; i < zipEntries.Length; i++)
                {
                    zip.AddEntry(zipEntries[i], zipContents[i], Encoding.UTF8);
                }

                SetSimpleNuspec(zip, packageId, packageVersion, frameworkAssemblies, minClientVersion, dependencies);
            }

            return fileInfo;
        }

        private static FileInfo GetFileInfo(string path, string packageId, string packageVersion)
        {
            var file = Path.Combine(path, Guid.NewGuid() + ".nupkg");
            var fileInfo = new FileInfo(file);

            return fileInfo;
        }

        public static void SetSimpleNuspec(ZipArchive zip,
            string packageId,
            string packageVersion,
            bool frameworkAssemblies,
            SemanticVersion minClientVersion,
            bool dependencies)
        {
            zip.AddEntry(packageId + ".nuspec", GetSimpleNuspecString(packageId,
                packageVersion,
                frameworkAssemblies,
                minClientVersion,
                dependencies),
                Encoding.UTF8);
        }

        private static readonly string MinClientVersionStringFormat = "minClientVersion=\"{0}\"";

        private static string GetSimpleNuspecString(string packageId,
            string packageVersion,
            bool frameworkAssemblies,
            SemanticVersion minClientVersion,
            bool dependencies)
        {
            var frameworkAssemblyReferences = frameworkAssemblies ?
                string.Format(CultureInfo.CurrentCulture, FrameworkAssembliesStringFormat, "System.Xml", "net45") : string.Empty;

            var minClientVersionString = minClientVersion == null ? string.Empty :
                string.Format(CultureInfo.CurrentCulture, MinClientVersionStringFormat, minClientVersion.ToNormalizedString());

            var dependenciesString = dependencies ?
                string.Format(CultureInfo.CurrentCulture, DependenciesStringFormat, "Owin", "1.0") : string.Empty;
            return string.Format(CultureInfo.CurrentCulture, NuspecStringFormat, packageId, packageVersion,
                string.Join(Environment.NewLine, frameworkAssemblyReferences, dependenciesString),
                minClientVersionString);
        }
    }
}
