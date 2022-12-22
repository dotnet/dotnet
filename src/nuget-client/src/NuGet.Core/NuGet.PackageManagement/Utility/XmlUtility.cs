// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using XmlUtility = NuGet.Shared.XmlUtility;

namespace NuGet.ProjectManagement
{
    public static class XmlUtility
    {
        [Obsolete("This method is obsolete and will be removed in a future release.")]
        public static XDocument LoadSafe(string filePath)
        {
            return Shared.XmlUtility.Load(filePath);
        }

        [Obsolete("This method is obsolete and will be removed in a future release.")]
        public static XDocument LoadSafe(Stream input)
        {
            return Shared.XmlUtility.Load(input);
        }

        [Obsolete("This method is obsolete and will be removed in a future release.")]
        public static XDocument LoadSafe(Stream input, bool ignoreWhiteSpace)
        {
            if (ignoreWhiteSpace)
                return Shared.XmlUtility.Load(input);

            return Shared.XmlUtility.Load(input, LoadOptions.PreserveWhitespace);
        }

        [Obsolete("This method is obsolete and will be removed in a future release.")]
        public static XDocument LoadSafe(Stream input, LoadOptions options)
        {
            return Shared.XmlUtility.Load(input, options);
        }

        [Obsolete("This method is obsolete and will be removed in a future release.")]
        public static XDocument GetOrCreateDocument(XName rootName, string path, IMSBuildProjectSystem msBuildNuGetProjectSystem)
        {
            return MSBuildNuGetProjectSystemUtility.GetOrCreateDocument(rootName, path, msBuildNuGetProjectSystem);
        }

        public static XDocument GetOrCreateDocument(XName rootName, string root, string path, INuGetProjectContext nuGetProjectContext)
        {
            if (File.Exists(Path.Combine(root, path)))
            {
                try
                {
                    return Shared.XmlUtility.Load(Path.Combine(root, path), LoadOptions.PreserveWhitespace);
                }
                catch (FileNotFoundException) { }
            }

            return CreateDocument(rootName, root, path, nuGetProjectContext);
        }

        [Obsolete("This method is obsolete and will be removed in a future release.")]
        public static XDocument CreateDocument(XName rootName, string path, IMSBuildProjectSystem msBuildNuGetProjectSystem)
        {
            var document = new XDocument(new XElement(rootName));
            // Add it to the project system
            MSBuildNuGetProjectSystemUtility.AddFile(msBuildNuGetProjectSystem, path, document.Save);
            return document;
        }

        public static XDocument CreateDocument(XName rootName, string root, string path, INuGetProjectContext nuGetProjectContext)
        {
            var document = new XDocument(new XElement(rootName));
            // Add it to the file system
            FileSystemUtility.AddFile(root, path, document.Save, nuGetProjectContext);
            return document;
        }

        [Obsolete("This method is obsolete and will be removed in a future release.")]
        public static XDocument GetDocument(string root, string path)
        {
            var fullPath = Path.Combine(root, path);
            using (Stream configStream = File.OpenRead(fullPath))
            {
                return LoadSafe(configStream, LoadOptions.PreserveWhitespace);
            }
        }

        [Obsolete("This method is obsolete and will be removed in a future release.")]
        public static bool TryParseDocument(string content, out XDocument document)
        {
            document = null;
            try
            {
                document = XDocument.Parse(content);
                return true;
            }
            catch (XmlException)
            {
                return false;
            }
        }
    }
}
