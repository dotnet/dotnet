// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace XliffTasks.Model
{
    /// <summary>
    /// A <see cref="TranslatableDocument"/> backed by an XML-based format
    /// </summary>
    internal abstract class TranslatableXmlDocument : TranslatableDocument
    {
        protected XDocument Document { get; private set; }

        protected override void LoadCore(TextReader reader)
        {
            Document = XDocument.Load(reader);
        }

        protected override void SaveCore(TextWriter writer)
        {
            Document.SaveCustom(writer);
        }

        protected sealed class TranslatableXmlElement : TranslatableNode
        {
            private readonly XElement _element;

            public TranslatableXmlElement(string id, string source, string note, XElement element)
               : base(id, source, note)
            {
                _element = element;
            }

            public override void Translate(string translation)
            {
                _element.Value = translation;
            }
        }

        protected sealed class TranslatableXmlAttribute : TranslatableNode
        {
            private readonly XAttribute _attribute;

            public TranslatableXmlAttribute(string id, string source, string note, XAttribute attribute)
               : base(id, source, note)
            {
                _attribute = attribute;
            }

            public override void Translate(string translation)
            {
                _attribute.Value = translation;
            }
        }
    }
}
