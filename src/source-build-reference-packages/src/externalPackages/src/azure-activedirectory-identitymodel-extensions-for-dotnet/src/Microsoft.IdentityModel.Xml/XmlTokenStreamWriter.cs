// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    internal class XmlTokenStreamWriter
    {
        public XmlTokenStreamWriter(XmlTokenStream tokenStream)
        {
            Position = 0;
            TokenStream = tokenStream;
        }

        public int Count => TokenStream.XmlTokens.Count;

        public int Position { get; private set; }

        public IList<XmlToken> Tokens => TokenStream.XmlTokens;

        public XmlTokenStream TokenStream { get; private set; }

        public XmlNodeType NodeType
        {
            get { return Tokens[Position].NodeType; }
        }

        public bool IsEmptyElement
        {
            get { return Tokens[Position].IsEmptyElement; }
        }

        public string Prefix
        {
            get { return Tokens[Position].Prefix; }
        }

        public string LocalName
        {
            get { return Tokens[Position].LocalName; }
        }

        public string Namespace
        {
            get { return Tokens[Position].Namespace; }
        }

        public string Value
        {
            get { return Tokens[Position].Value; }
        }

        public bool MoveToFirst()
        {
            Position = 0;
            return Count > 0;
        }

        public bool MoveToFirstAttribute()
        {
            if (Position < Count - 1 && Tokens[Position + 1].NodeType == XmlNodeType.Attribute)
            {
                Position++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MoveToNext()
        {
            if (Position < Count - 1)
            {
                Position++;
                return true;
            }
            return false;
        }

        public bool MoveToNextAttribute()
        {
            if (Position < Count - 1 && Tokens[Position + 1].NodeType == XmlNodeType.Attribute)
            {
                Position++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void WriteTo(XmlWriter writer)
        {
            WriteTo(writer, null, null);
        }

        public void WriteTo(XmlWriter writer, string excludedElement, string excludedNamespace)
        {
            if (writer == null)
                throw LogExceptionMessage(new ArgumentNullException(nameof(writer)));

            if (!MoveToFirst())
                throw LogExceptionMessage(new ArgumentException("XmlTokenBufferIsEmpty"));

            bool excluedSignatureElement = (XmlSignatureConstants.Elements.Signature == excludedElement && XmlSignatureConstants.Namespace == excludedNamespace);

            int depth = 0;
            int recordedDepth = -1;
            bool include = true;
            do
            {
                switch (NodeType)
                {
                    case XmlNodeType.Element:
                        bool isEmpty = IsEmptyElement;
                        depth++;
                        if (include
                            && LocalName == excludedElement
                            && Namespace == excludedNamespace
                            )
                        {
                            if (excluedSignatureElement && Position == TokenStream.SignatureElement)
                            {
                                include = false;
                                recordedDepth = depth;
                            }
                        }
                        if (include)
                        {
                            writer.WriteStartElement(Prefix, LocalName, Namespace);
                        }
                        if (MoveToFirstAttribute())
                        {
                            do
                            {
                                if (include)
                                {
                                    writer.WriteAttributeString(Prefix, LocalName, Namespace, Value);
                                }
                            }
                            while (MoveToNextAttribute());
                        }
                        if (isEmpty)
                        {
                            goto case XmlNodeType.EndElement;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (include)
                        {
                            writer.WriteEndElement();
                        }
                        else if (recordedDepth == depth)
                        {
                            include = true;
                            recordedDepth = -1;
                        }
                        depth--;
                        break;
                    case XmlNodeType.CDATA:
                        if (include)
                        {
                            writer.WriteCData(Value);
                        }
                        break;
                    case XmlNodeType.Comment:
                        if (include)
                        {
                            writer.WriteComment(Value);
                        }
                        break;
                    case XmlNodeType.Text:
                        if (include)
                        {
                            writer.WriteString(Value);
                        }
                        break;
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        if (include)
                        {
                            writer.WriteWhitespace(Value);
                        }
                        break;
                    case XmlNodeType.DocumentType:
                    case XmlNodeType.XmlDeclaration:
                        break;
                }
            }
            while (MoveToNext());
        }

        internal void WriteAndReplaceSignature(XmlWriter writer, Signature signature, DSigSerializer dSigSerializer)
        {
            if (writer == null)
                throw LogExceptionMessage(new ArgumentNullException(nameof(writer)));

            if (signature == null)
                throw LogExceptionMessage(new ArgumentNullException(nameof(signature)));

            if (dSigSerializer == null)
                throw LogExceptionMessage(new ArgumentNullException(nameof(dSigSerializer)));

            if (!MoveToFirst())
                throw LogExceptionMessage(new ArgumentException("XmlTokenBufferIsEmpty"));

            bool include = true;
            do
            {
                switch (NodeType)
                {
                    case XmlNodeType.Element:
                        bool isEmpty = IsEmptyElement;
                        // if the current XmlToken represents a placeholder signature element, skip writing the placeholder token
                        // and write the signature using the provided DSigSerializer.
                        if (LocalName == EnvelopedSignatureWriter.SignaturePlaceholder)
                        {
                            dSigSerializer.WriteSignature(writer, signature);
                            include = false;
                        }
                        else
                        {
                            writer.WriteStartElement(Prefix, LocalName, Namespace);
                        }
                        if (MoveToFirstAttribute())
                        {
                            do
                            {
                                if (include)
                                {
                                    writer.WriteAttributeString(Prefix, LocalName, Namespace, Value);
                                }
                            }
                            while (MoveToNextAttribute());
                        }
                        if (isEmpty)
                        {
                            goto case XmlNodeType.EndElement;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (include)
                        {
                            writer.WriteEndElement();
                        }
                        else
                        {
                            // skip writing EndElement as it's already written by the provided DSigSerializer.
                            include = true;
                        }
                        break;
                    case XmlNodeType.CDATA:
                        writer.WriteCData(Value);
                        break;
                    case XmlNodeType.Comment:
                        writer.WriteComment(Value);
                        break;
                    case XmlNodeType.Text:
                        writer.WriteString(Value);
                        break;
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        writer.WriteWhitespace(Value);
                        break;
                    case XmlNodeType.DocumentType:
                    case XmlNodeType.XmlDeclaration:
                        break;
                }
            }
            while (MoveToNext());
        }
    }
}
