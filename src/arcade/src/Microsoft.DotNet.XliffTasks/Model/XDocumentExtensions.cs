// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace XliffTasks.Model
{
    internal static class XDocumentExtensions
    {
        /// <summary>
        /// Save the given document to the given writer, with shared defaults
        /// for all XML writing by this library.
        /// </summary>
        public static void SaveCustom(this XDocument document, TextWriter writer)
        {
            XmlWriterSettings settings = new()
            {
                Indent = true,
                OmitXmlDeclaration = writer is StringWriter,
            };

            using XmlWriter xmlWriter = XmlWriter.Create(writer, settings);
            document.Save(xmlWriter);
        }

        public static void SelfCloseIfPossible(this XElement element)
        {
            if (element.Value.Length == 0)
            {
                element.RemoveNodes();
            }
        }
    }
}