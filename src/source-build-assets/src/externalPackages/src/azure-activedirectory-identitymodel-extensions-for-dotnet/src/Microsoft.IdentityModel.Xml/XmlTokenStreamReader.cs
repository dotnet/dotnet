// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// An XmlReader that captures xml as a XmlTokenStream
    /// </summary>
    public class XmlTokenStreamReader : DelegatingXmlDictionaryReader
    {
        private int _depth;
        private bool _recordDone;
        private XmlTokenStreamReader _innerTokenStreamReader;

        /// <summary>
        /// Initializes a new instance of <see cref="XmlTokenStreamReader"/> for creating a <see cref="XmlTokenStream"/>.
        /// </summary>
        /// <param name="reader">an <see cref="XmlDictionaryReader"/> to capture the <see cref="XmlTokenStream"/>.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="reader"/> if null.</exception>
        /// <exception cref="ArgumentException">if <paramref name="reader"/>.IsStartElement() is false.</exception>
        public XmlTokenStreamReader(XmlDictionaryReader reader)
        {
            if (reader == null)
                throw LogArgumentNullException(nameof(reader));

            if (!reader.IsStartElement())
                throw LogExceptionMessage(new ArgumentException(FormatInvariant(LogMessages.IDX30026, MarkAsNonPII(reader.NodeType))));

            // When multiple signed elements are being processed, the EnvelopedSignatureReader always creates a new XmlTokenStreamReader.
            // This creates a chain of readers.
            // The outer reader need to record the position of its 'Signature' element.
            // When calculating the digest all inner 'Signatures' are considered as inner text.
            // This allows for embedded signed elements.
            _innerTokenStreamReader = reader as XmlTokenStreamReader;
            TokenStream = new XmlTokenStream();
            InnerReader = reader;
            Record();
        }

        /// <summary>
        /// Gets the <see cref="XmlTokenStream"/>
        /// </summary>
        public XmlTokenStream TokenStream
        {
            get;
        }

        internal XmlDictionaryReader XmlDictionaryReader => InnerReader;

        /// <summary>
        /// Delegates to InnerReader, then calls Record()
        /// </summary>
        /// <remarks>When multiple signed elements are being processed, the EnvelopedSignatureReader always creates a new XmlTokenStreamReader.
        /// The outer reader need to record the position of its 'Signature' element and ignore any inner 'Signatures'.
        /// This allows for embedded signed elements.</remarks>
        internal bool Read(bool recordSignaturePosition)
        {
            if (_innerTokenStreamReader != null)
            {
                if (!_innerTokenStreamReader.Read(recordSignaturePosition))
                    return false;
            }
            else if (!InnerReader.Read())
                return false;

            if (!_recordDone)
                Record(recordSignaturePosition);

            return true;
        }

        /// <summary>
        /// Delegates to InnerReader, then calls Record()
        /// </summary>
        public override bool Read()
        {
            if (_innerTokenStreamReader != null)
            {
                if (!_innerTokenStreamReader.Read(false))
                    return false;
            }
            else if (!InnerReader.Read())
                return false;

            if (!_recordDone)
                Record(true);

            return true;
        }

        private void Record()
        {
            Record(true);
        }

        private void Record(bool recordSignaturePosition)
        {
            switch (InnerReader.NodeType)
            {
                case XmlNodeType.Element:
                    {
                        bool isEmpty = InnerReader.IsEmptyElement;
                        if (recordSignaturePosition && InnerReader.IsLocalName(XmlSignatureConstants.Elements.Signature) && InnerReader.IsNamespaceUri(XmlSignatureConstants.Namespace))
                            TokenStream.SignatureElement = TokenStream.XmlTokens.Count;

                        TokenStream.AddElement(InnerReader.Prefix, InnerReader.LocalName, InnerReader.NamespaceURI, isEmpty);

                        if (InnerReader.MoveToFirstAttribute())
                        {
                            do
                            {
                                TokenStream.AddAttribute(InnerReader.Prefix, InnerReader.LocalName, InnerReader.NamespaceURI, InnerReader.Value);
                            }
                            while (InnerReader.MoveToNextAttribute());
                            InnerReader.MoveToElement();
                        }

                        if (!isEmpty)
                        {
                            _depth++;
                        }
                        else if (_depth == 0)
                        {
                            _recordDone = true;
                        }

                        break;
                    }
                case XmlNodeType.CDATA:
                case XmlNodeType.Comment:
                case XmlNodeType.Text:
                case XmlNodeType.EntityReference:
                case XmlNodeType.EndEntity:
                case XmlNodeType.SignificantWhitespace:
                case XmlNodeType.Whitespace:
                    {
                        TokenStream.Add(InnerReader.NodeType, InnerReader.Value);
                        break;
                    }
                case XmlNodeType.EndElement:
                    {
                        TokenStream.Add(InnerReader.NodeType, InnerReader.Value);
                        if (--_depth == 0)
                            _recordDone = true;

                        break;
                    }
                case XmlNodeType.DocumentType:
                case XmlNodeType.XmlDeclaration:
                    {
                        break;
                    }
                default:
                    {
                        throw LogExceptionMessage(new XmlException(FormatInvariant(LogMessages.IDX30406, MarkAsNonPII(InnerReader.NodeType))));
                    }
            }
        }
    }
}
