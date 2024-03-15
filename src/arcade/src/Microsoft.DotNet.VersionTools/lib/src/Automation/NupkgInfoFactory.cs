// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.DotNet.VersionTools.Automation
{
    public interface INupkgInfoFactory
    {
        NupkgInfo CreateNupkgInfo(string path);
    }

    public class NupkgInfoFactory : INupkgInfoFactory
    {
        private readonly IPackageArchiveReaderFactory _packageArchiveReaderFactory;

        public NupkgInfoFactory(IPackageArchiveReaderFactory packageArchiveReaderFactory)
        {
            _packageArchiveReaderFactory = packageArchiveReaderFactory;
        }

        public NupkgInfo CreateNupkgInfo(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            Stream stream = null;
            Stream ZipReadStream = null;
            ZipArchive _zipArchive;
            try
            {
                stream = File.OpenRead(path);
                ZipReadStream = stream;
                _zipArchive = new ZipArchive(stream, ZipArchiveMode.Read);
                string nuspecFile = Path.GetTempFileName();
                foreach (ZipArchiveEntry entry in _zipArchive.Entries)
                {
                    if (entry.Name.EndsWith(".nuspec"))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(nuspecFile));
                        entry.ExtractToFile(nuspecFile, true);
                    }
                }

                if (!File.Exists(nuspecFile))
                {
                    throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, "Did not extract nuspec file from package: {0}", path));
                }

                PackageIdentity identity = GetIdentity(nuspecFile);
                return new NupkgInfo(identity);
            }
            catch (Exception ex)
            {
                stream?.Dispose();
                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, "Invalid package", path), ex);
            }
        }

        private static PackageIdentity GetIdentity(string nuspecFile)
        {
            if (nuspecFile == null)
            {
                throw new ArgumentNullException(nameof(nuspecFile));
            }

            XDocument doc = XDocument.Load(nuspecFile, LoadOptions.PreserveWhitespace);
            XElement metadataElement = GetSingleElement(doc.Root, "metadata");
            return new PackageIdentity(GetSingleElement(metadataElement, "id").Value, GetSingleElement(metadataElement, "version").Value);
        }   

        private static XElement GetSingleElement(XElement el, string name)
        {
            return el.Descendants().First(c => c.Name.LocalName.ToString() == name);
        }
    }
}
