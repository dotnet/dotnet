
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NuGet.Common;

namespace NuGet.Packaging
{
    public class PackageFileExtractor
    {
        private readonly HashSet<string> _intellisenseXmlFiles;
        private readonly XmlDocFileSaveMode _xmlDocFileSaveMode;

        public PackageFileExtractor(IEnumerable<string> packageFiles, XmlDocFileSaveMode xmlDocFileSaveMode)
        {
            _xmlDocFileSaveMode = xmlDocFileSaveMode;
            if (xmlDocFileSaveMode == XmlDocFileSaveMode.Skip ||
                xmlDocFileSaveMode == XmlDocFileSaveMode.Compress)
            {
                _intellisenseXmlFiles = GatherIntellisenseXmlFiles(packageFiles);
            }
        }

        private static HashSet<string> GatherIntellisenseXmlFiles(IEnumerable<string> packageFiles)
        {
            var intellisenseXmlFiles = new HashSet<string>(StringComparer.Ordinal);

            var refAndLibFiles = packageFiles.Where(
                f => f.StartsWith("lib/", StringComparison.OrdinalIgnoreCase) ||
                f.StartsWith("ref/", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foreach (var file in refAndLibFiles)
            {
                if (!file.EndsWith(".xml", PathUtility.GetStringComparisonBasedOnOS()))
                {
                    continue;
                }

                // Xml files located next to the neutral language dll.
                var dllFile = Path.ChangeExtension(file, ".dll");
                if (refAndLibFiles.Contains(dllFile, StringComparer.OrdinalIgnoreCase))
                {
                    intellisenseXmlFiles.Add(file);
                    continue;
                }

                // For satellite assemblies, look for a corresponding .resources.dll
                var resourceDll = Path.ChangeExtension(file, ".resources.dll");
                if (refAndLibFiles.Contains(resourceDll, StringComparer.OrdinalIgnoreCase))
                {
                    intellisenseXmlFiles.Add(file);
                    continue;
                }

                // Xml files located in a language specific directory.
                var languageDllFile = GetBinaryForLanguageSpecificXml(file);
                if (languageDllFile != null && refAndLibFiles.Contains(languageDllFile, StringComparer.Ordinal))
                {
                    intellisenseXmlFiles.Add(file);
                }
            }

            return intellisenseXmlFiles;
        }

        private static string GetBinaryForLanguageSpecificXml(string file)
        {
            // For xml files located in language specific directories, look for a corresponding binary
            // in the parent directory.
            // e.g. ref/net45/fr/MyBinary.xml -> ref/net45/MyBinary.dll

            var fileSeparatorIndex = file.LastIndexOf('/');
            if (fileSeparatorIndex > 0)
            {
                var directorySeparatorIndex = file.LastIndexOf('/', fileSeparatorIndex - 1);
                if (directorySeparatorIndex >= 0)
                {
                    var fileAtParentDirectory =
                        file.Substring(0, directorySeparatorIndex) +
                        file.Substring(fileSeparatorIndex);
                    return Path.ChangeExtension(fileAtParentDirectory, ".dll");
                }
            }

            return null;
        }

        public string ExtractPackageFile(string source, string target, Stream stream)
        {
            if ((_xmlDocFileSaveMode == XmlDocFileSaveMode.Skip) && _intellisenseXmlFiles.Contains(source))
            {
                return null;
            }

            var extractDirectory = Path.GetDirectoryName(target);
            Directory.CreateDirectory(extractDirectory);

            if ((_xmlDocFileSaveMode == XmlDocFileSaveMode.Compress) && _intellisenseXmlFiles.Contains(source))
            {
                // If the package contains a file named {BinaryName}.xml.zip already exists, the result would be
                // ambigious.
                var targetZip = Path.ChangeExtension(target, ".xml.zip");
                using (var outputStream = NuGetExtractionFileIO.CreateFile(targetZip))
                using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create))
                {
                    var entry = zipArchive.CreateEntry(Path.GetFileName(source));
                    using (var entryStream = entry.Open())
                    {
                        stream.CopyTo(entryStream);
                    }
                }

                return targetZip;
            }

            if (Path.IsPathRooted(source))
            {
                // Copying files stream-to-stream is less efficient. Attempt to copy using File.Copy if we the source
                // is a file on disk.
                File.Copy(source, target, overwrite: true);
            }
            else
            {
                stream.CopyToFile(target);
            }

            return target;
        }
    }
}
