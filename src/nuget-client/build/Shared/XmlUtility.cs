// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace NuGet.Shared
{
    internal static class XmlUtility
    {
        /// <summary>
        /// Creates a new <see cref="System.Xml.Linq.XDocument"/> from a file.
        /// </summary>
        /// <param name="path">The complete file path to be read into a new <see cref="System.Xml.Linq.XDocument"/>.</param>
        /// <returns>An <see cref="System.Xml.Linq.XDocument"/> that contains the contents of the specified file.</returns>
        internal static XDocument Load(string path)
        {
            return Load(path, LoadOptions.None);
        }

        /// <summary>
        /// Creates a new <see cref="System.Xml.Linq.XDocument"/> from a file. Optionally, whitespace can be preserved.
        /// </summary>
        /// <param name="path">The complete file path to be read into a new <see cref="System.Xml.Linq.XDocument"/>.</param>
        /// <param name="options">A set of <see cref="LoadOptions"/>.</param>
        /// <returns>An <see cref="System.Xml.Linq.XDocument"/> that contains the contents of the specified file.</returns>
        internal static XDocument Load(string path, LoadOptions options)
        {
            using FileStream fileStream = File.OpenRead(path);
            using var xmlReader = XmlReader.Create(fileStream, GetXmlReaderSettings(options));

            return XDocument.Load(xmlReader, options);
        }

        /// <summary>
        /// Creates a new <see cref="System.Xml.Linq.XDocument"/> from a stream.
        /// </summary>
        /// <param name="input">The stream that contains the XML data.</param>
        /// <returns>An <see cref="System.Xml.Linq.XDocument"/> that contains the contents of the specified stream.</returns>
        internal static XDocument Load(Stream input)
        {
            return Load(input, LoadOptions.None);
        }

        /// <summary>
        /// Creates a new System.Xml.Linq.XDocument from a stream. Optionally, whitespace can be preserved.
        /// </summary>
        /// <param name="input">The stream that contains the XML data.</param>
        /// <param name="options">A set of <see cref="LoadOptions"/>.</param>
        /// <returns>An <see cref="System.Xml.Linq.XDocument"/> that contains the contents of the specified stream.</returns>
        internal static XDocument Load(Stream input, LoadOptions options)
        {
            using (var reader = XmlReader.Create(input, GetXmlReaderSettings(options)))
            {
                return XDocument.Load(reader, options);
            }
        }

        /// <summary>
        /// Converts the name to a valid XML local name, if it is invalid.
        /// </summary>
        /// <param name="name">The name to be encoded.</param>
        /// <returns>The encoded name.</returns>
        internal static string GetEncodedXMLName(string name)
        {
            try
            {
                return XmlConvert.VerifyName(name);
            }
            catch (XmlException)
            {
                return XmlConvert.EncodeLocalName(name);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="System.Xml.XmlReaderSettings"/> with safe settings
        /// <param name="options">A set of <see cref="LoadOptions"/>.</param>
        /// </summary>
        internal static XmlReaderSettings GetXmlReaderSettings(LoadOptions options)
        {
            XmlReaderSettings rs = new XmlReaderSettings();
            if ((options & LoadOptions.PreserveWhitespace) == 0)
                rs.IgnoreWhitespace = true;
            rs.IgnoreProcessingInstructions = true;
            rs.DtdProcessing = DtdProcessing.Prohibit;
            return rs;
        }
    }
}
