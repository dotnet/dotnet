// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System.IO;
using System.Xml.Linq;

namespace NuGet.ProjectManagement
{
    public static class XmlUtility
    {
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

        public static XDocument CreateDocument(XName rootName, string root, string path, INuGetProjectContext nuGetProjectContext)
        {
            var document = new XDocument(new XElement(rootName));
            // Add it to the file system
            FileSystemUtility.AddFile(root, path, document.Save, nuGetProjectContext);
            return document;
        }
    }
}
