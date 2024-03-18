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

            try
            {
                using Stream stream = File.OpenRead(path);
                ZipArchive zipArchive = new(stream, ZipArchiveMode.Read);
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    if (entry.Name.EndsWith(".nuspec"))
                    {
                        using Stream nuspecFileStream = entry.Open();
                        PackageIdentity identity = GetIdentity(nuspecFileStream);
                        return new NupkgInfo(identity);
                    }
                }

                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, "Did not extract nuspec file from package: {0}", path));
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(string.Format(CultureInfo.CurrentCulture, "Invalid package", path), ex);
            }
        }

        private static PackageIdentity GetIdentity(Stream nuspecFileStream)
        {
            XDocument doc = XDocument.Load(nuspecFileStream, LoadOptions.PreserveWhitespace);
            XElement metadataElement = GetSingleElement(doc.Root, "metadata");
            return new PackageIdentity(GetSingleElement(metadataElement, "id").Value, GetSingleElement(metadataElement, "version").Value);
        }   

        private static XElement GetSingleElement(XElement el, string name)
        {
            return el.Descendants().First(c => c.Name.LocalName.ToString() == name);
        }
    }
}
