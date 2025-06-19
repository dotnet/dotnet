// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;
using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

internal sealed partial class SlnXmlSerializer
{
    private sealed partial class Reader
    {
        private readonly string? fullPath;
        private readonly XmlDocument xmlDocument;

        internal Reader(string? fullPath, Stream readerStream)
        {
            this.fullPath = fullPath;

            // We ideally want to preserver whitespace, but if this is on
            // we need to manually handle preserving all indenting and new lines
            // when elements are added or removed.
            this.xmlDocument = new LineInfoXmlDocument() { PreserveWhitespace = true };
            this.xmlDocument.Load(readerStream);
        }

        internal SolutionModel Parse()
        {
            SlnxFile slnxFile = new SlnxFile(this.xmlDocument, new SlnxSerializerSettings(), null, this.fullPath);
            return slnxFile.ToModel();
        }
    }
}
