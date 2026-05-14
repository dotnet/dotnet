// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
