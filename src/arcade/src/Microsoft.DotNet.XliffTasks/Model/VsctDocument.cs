// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace XliffTasks.Model
{
    /// <summary>
    /// A <see cref="TranslatableDocument"/> for files in .vsct format.
    /// See https://msdn.microsoft.com/en-us/library/bb164699.aspx
    /// </summary>
    internal sealed class VsctDocument : TranslatableXmlDocument
    {
        protected override IEnumerable<TranslatableNode> GetTranslatableNodes()
        {
            HashSet<string> nonUniqueIds = FindNonUniqueIds();

            foreach (XElement strings in Document.Descendants(Document.Root.Name.Namespace + "Strings"))
            {
                string id = strings.Parent.Attribute("id").Value;
                if (nonUniqueIds.Contains(id))
                {
                    // The ID by itself is not unique in this document; we must include
                    // the GUID as well.
                    string guid = strings.Parent.Attribute("guid").Value;
                    id = guid + "|" + id;
                }

                foreach (XElement child in strings.Elements())
                {
                    XName name = child.Name;

                    if (name.LocalName == "CanonicalName")
                    {
                        // CanonicalName is never localized.
                        // LocCanonicalName can be used to specify a localized alternative.
                        // See https://msdn.microsoft.com/en-us/library/bb491712.aspx
                        continue;
                    }

                    yield return new TranslatableXmlElement(
                        id: $"{id}|{name.LocalName}",
                        source: child.Value,
                        note: null,
                        element: child);
                }
            }
        }

        /// <summary>
        /// Finds the set of IDs that do not uniquely identify a single set of strings.
        /// Within a .vsct file only the combination of GUID and ID needs to be unique,
        /// but for brevity we'd prefer to use just the ID when we can. By finding the
        /// set of non-unique IDs we can identify when this will not be sufficient.
        /// </summary>
        /// <returns></returns>
        private HashSet<string> FindNonUniqueIds()
        {
            HashSet<string> idsAlreadySeen = new();
            HashSet<string> conflictingIds = new();

            foreach (XElement strings in Document.Descendants(Document.Root.Name.Namespace + "Strings"))
            {
                string id = strings.Parent.Attribute("id").Value;

                if (!idsAlreadySeen.Add(id))
                {
                    conflictingIds.Add(id);
                }
            }

            return conflictingIds;
        }

        public override void RewriteRelativePathsToAbsolute(string sourceFullPath)
        {
            foreach (XElement imageTag in Document.Descendants(Document.Root.Name.Namespace + "Bitmap"))
            {
                XAttribute hrefAttribute = imageTag.Attribute("href");

                if (hrefAttribute != null)
                {
                    string resourceRelativePath = hrefAttribute.Value.Replace('\\', Path.DirectorySeparatorChar);

                    string absolutePath = Path.Combine(Path.GetDirectoryName(sourceFullPath), resourceRelativePath);

                    imageTag.Attribute("href").Value = absolutePath;
                }
            }
        }
    }
}
