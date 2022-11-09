//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Wraps a <see cref="XmlDictionaryWriter"/> and delegates to InnerWriter.
    /// </summary>
    public class DelegatingXmlDictionaryWriter : XmlDictionaryWriter
    {
        private XmlDictionaryWriter _innerWriter;
        private XmlDictionaryWriter _internalWriter;
        private XmlDictionaryWriter _tracingWriter;

        /// <summary>
        /// Initializes a new instance of <see cref="DelegatingXmlDictionaryWriter"/>
        /// </summary>
        protected DelegatingXmlDictionaryWriter()
        {
        }

        /// <summary>
        /// Gets or sets a <see cref="XmlDictionaryWriter"/> for tracing.
        /// </summary>
        /// <exception cref="ArgumentNullException"> if 'value' is null.</exception>
        protected XmlDictionaryWriter TracingWriter
        {
            get => _tracingWriter;
            set => _tracingWriter = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the InnerWriter.
        /// </summary>
        /// <exception cref="ArgumentNullException"> if 'value' is null.</exception>
        protected XmlDictionaryWriter InnerWriter
        {
            get => _innerWriter;
            set => _innerWriter = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the InternalWriter.
        /// </summary>
        /// <exception cref="ArgumentNullException"> if 'value' is null.</exception>
        internal XmlDictionaryWriter InternalWriter
        {
            get => _internalWriter;
            set => _internalWriter = value ?? throw LogArgumentNullException(nameof(value));
        }

#if NET45
        /// <summary>
        /// Closes the underlying stream.
        /// </summary>
        public override void Close()
        {
            UseInnerWriter.Close();
            TracingWriter?.Close();
            InternalWriter?.Close();
        }
#endif

        /// <summary>
        /// Flushes the underlying stream.
        /// </summary>
        public override void Flush()
        {
            UseInnerWriter.Flush();
            TracingWriter?.Flush();
            InternalWriter?.Flush();
        }

        /// <summary>
        /// Encodes the specified binary bytes as Base64 and writes out the resulting text.
        /// </summary>
        /// <param name="buffer">Byte array to encode.</param>
        /// <param name="index">The position in the buffer indicating the start of the bytes to write.</param>
        /// <param name="count">The number of bytes to write.</param>
        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            UseInnerWriter.WriteBase64(buffer, index, count);
            TracingWriter?.WriteBase64(buffer, index, count);
            InternalWriter?.WriteBase64(buffer, index, count);
        }

        /// <summary>
        /// Writes out a CDATA block containing the specified text.
        /// </summary>
        /// <param name="text">The text to place inside the CDATA block.</param>
        public override void WriteCData(string text)
        {
            UseInnerWriter.WriteCData(text);
            TracingWriter?.WriteCData(text);
            InternalWriter?.WriteCData(text);
        }

        /// <summary>
        /// Forces the generation of a character entity for the specified Unicode character value.
        /// </summary>
        /// <param name="ch">The Unicode character for which to generate a character entity.</param>
        public override void WriteCharEntity(char ch)
        {
            UseInnerWriter.WriteCharEntity(ch);
            TracingWriter?.WriteCharEntity(ch);
            InternalWriter?.WriteCharEntity(ch);
        }

        /// <summary>
        /// When overridden in a derived class, writes text one buffer at a time.
        /// </summary>
        /// <param name="buffer">Character array containing the text to write.</param>
        /// <param name="index">The position in the buffer indicating the start of the text to write.</param>
        /// <param name="count">The number of characters to write.</param>
        public override void WriteChars(char[] buffer, int index, int count)
        {
            UseInnerWriter.WriteChars(buffer, index, count);
            TracingWriter?.WriteChars(buffer, index, count);
            InternalWriter?.WriteChars(buffer, index, count);
        }

        /// <summary>
        /// Writes out a comment containing the specified text.
        /// </summary>
        /// <param name="text">Text to place inside the comment.</param>
        public override void WriteComment(string text)
        {
            UseInnerWriter.WriteComment(text);
            TracingWriter?.WriteComment(text);
            InternalWriter?.WriteComment(text);
        }

        /// <summary>
        /// Writes the DOCTYPE declaration with the specified name and optional attributes.
        /// </summary>
        /// <param name="name">The name of the DOCTYPE. This must be non-empty.</param>
        /// <param name="pubid">If non-null it also writes PUBLIC "pubid" "sysid" where pubid and sysid are
        /// replaced with the value of the given arguments.</param>
        /// <param name="sysid">If pubid is null and sysid is non-null it writes SYSTEM "sysid" where sysid
        /// is replaced with the value of this argument.</param>
        /// <param name="subset">If non-null it writes [subset] where subset is replaced with the value of
        /// this argument.</param>
        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            UseInnerWriter.WriteDocType(name, pubid, sysid, subset);
            TracingWriter?.WriteDocType(name, pubid, sysid, subset);
            InternalWriter?.WriteDocType(name, pubid, sysid, subset);
        }

        /// <summary>
        /// Closes the previous System.Xml.XmlWriter.WriteStartAttribute(System.String,System.String) call.
        /// </summary>
        public override void WriteEndAttribute()
        {
            UseInnerWriter.WriteEndAttribute();
            TracingWriter?.WriteEndAttribute();
            InternalWriter?.WriteEndAttribute();
        }

        /// <summary>
        /// Closes any open elements or attributes and puts the writer back in the Start state.
        /// </summary>
        public override void WriteEndDocument()
        {
            UseInnerWriter.WriteEndDocument();
            TracingWriter?.WriteEndDocument();
            InternalWriter?.WriteEndDocument();
        }

        /// <summary>
        /// Closes one element and pops the corresponding namespace scope.
        /// </summary>
        public override void WriteEndElement()
        {
            UseInnerWriter.WriteEndElement();
            TracingWriter?.WriteEndElement();
            InternalWriter?.WriteEndElement();
        }

        /// <summary>
        /// Writes out an entity reference as name.
        /// </summary>
        /// <param name="name">The name of the entity reference.</param>
        public override void WriteEntityRef(string name)
        {
            UseInnerWriter.WriteEntityRef(name);
            TracingWriter?.WriteEntityRef(name);
            InternalWriter?.WriteEntityRef(name);
        }

        /// <summary>
        /// Closes one element and pops the corresponding namespace scope.
        /// </summary>
        public override void WriteFullEndElement()
        {
            UseInnerWriter.WriteFullEndElement();
            TracingWriter?.WriteFullEndElement();
            InternalWriter?.WriteFullEndElement();
        }

        /// <summary>
        /// Writes out a processing instruction with a space between the name and text as follows: &lt;?name text?>.
        /// </summary>
        /// <param name="name">The name of the processing instruction.</param>
        /// <param name="text">The text to include in the processing instruction.</param>
        public override void WriteProcessingInstruction(string name, string text)
        {
            UseInnerWriter.WriteProcessingInstruction(name, text);
            TracingWriter?.WriteProcessingInstruction(name, text);
            InternalWriter?.WriteProcessingInstruction(name, text);
        }

        /// <summary>
        /// When overridden in a derived class, writes raw markup manually from a character buffer.
        /// </summary>
        /// <param name="buffer">Character array containing the text to write.</param>
        /// <param name="index">The position within the buffer indicating the start of the text to write.</param>
        /// <param name="count">The number of characters to write.</param>
        public override void WriteRaw(char[] buffer, int index, int count)
        {
            UseInnerWriter.WriteRaw(buffer, index, count);
            TracingWriter?.WriteRaw(buffer, index, count);
            InternalWriter?.WriteRaw(buffer, index, count);
        }

        /// <summary>
        /// Writes raw markup manually from a string.
        /// </summary>
        /// <param name="data">String containing the text to write.</param>
        public override void WriteRaw(string data)
        {
            UseInnerWriter.WriteRaw(data);
            TracingWriter?.WriteRaw(data);
            InternalWriter?.WriteRaw(data);
        }

        /// <summary>
        /// Writes the start of an attribute with the specified local name and namespace URI.
        /// </summary>
        /// <param name="prefix">The namespace prefix of the attribute.</param>
        /// <param name="localName">The local name of the attribute.</param>
        /// <param name="namespace">The namespace URI for the attribute.</param>
        public override void WriteStartAttribute(string prefix, string localName, string @namespace)
        {
            UseInnerWriter.WriteStartAttribute(prefix, localName, @namespace);
            TracingWriter?.WriteStartAttribute(prefix, localName, @namespace);
            InternalWriter?.WriteStartAttribute(prefix, localName, @namespace);
        }

        /// <summary>
        /// When overridden in a derived class, writes the XML declaration with the version "1.0".
        /// </summary>
        public override void WriteStartDocument()
        {
            UseInnerWriter.WriteStartDocument();
            TracingWriter?.WriteStartDocument();
            InternalWriter?.WriteStartDocument();
        }

        /// <summary>
        /// When overridden in a derived class, writes the XML declaration with the version
        /// "1.0" and the standalone attribute.
        /// </summary>
        /// <param name="standalone">If true, it writes "standalone=yes"; if false, it writes "standalone=no".</param>
        public override void WriteStartDocument(bool standalone)
        {
            UseInnerWriter.WriteStartDocument(standalone);
            TracingWriter?.WriteStartDocument(standalone);
            InternalWriter?.WriteStartDocument(standalone);
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified start tag and associates
        /// it with the given namespace and prefix.
        /// </summary>
        /// <param name="prefix">The namespace prefix of the element.</param>
        /// <param name="localName">The local name of the element.</param>
        /// <param name="namespace">The namespace URI to associate with the element.</param>
        public override void WriteStartElement(string prefix, string localName, string @namespace)
        {
            UseInnerWriter.WriteStartElement(prefix, localName, @namespace);
            TracingWriter?.WriteStartElement(prefix, localName, @namespace);
            InternalWriter?.WriteStartElement(prefix, localName, @namespace);
        }

        /// <summary>
        /// When overridden in a derived class, gets the state of the writer.
        /// </summary>
        public override WriteState WriteState
        {
            get { return UseInnerWriter.WriteState; }
        }

        /// <summary>
        /// Writes the given text content.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public override void WriteString(string text)
        {
            UseInnerWriter.WriteString(text);
            TracingWriter?.WriteString(text);
            InternalWriter?.WriteString(text);
        }

        /// <summary>
        /// Generates and writes the surrogate character entity for the surrogate character pair.
        /// </summary>
        /// <param name="lowChar">The low surrogate. This must be a value between 0xDC00 and 0xDFFF.</param>
        /// <param name="highChar">The high surrogate. This must be a value between 0xD800 and 0xDBFF.</param>
        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            UseInnerWriter.WriteSurrogateCharEntity(lowChar, highChar);
            TracingWriter?.WriteSurrogateCharEntity(lowChar, highChar);
            InternalWriter?.WriteSurrogateCharEntity(lowChar, highChar);
        }

        /// <summary>
        /// Writes out the given white space.
        /// </summary>
        /// <param name="ws">The string of white space characters.</param>
        public override void WriteWhitespace(string ws)
        {
            UseInnerWriter.WriteWhitespace(ws);
            TracingWriter?.WriteWhitespace(ws);
            InternalWriter?.WriteWhitespace(ws);
        }

        /// <summary>
        /// Writes an attribute as a xml attribute with the prefix 'xml:'.
        /// </summary>
        /// <param name="localName">Localname of the attribute.</param>
        /// <param name="value">Attribute value.</param>
        public override void WriteXmlAttribute(string localName, string value)
        {
            UseInnerWriter.WriteXmlAttribute(localName, value);
            TracingWriter?.WriteAttributeString(localName, value);
            InternalWriter?.WriteAttributeString(localName, value);
        }

        /// <summary>
        /// Writes an xmlns namespace declaration. 
        /// </summary>
        /// <param name="prefix">The prefix of the namespace declaration.</param>
        /// <param name="namespace">The namespace Uri itself.</param>
        public override void WriteXmlnsAttribute(string prefix, string @namespace)
        {
            UseInnerWriter.WriteXmlnsAttribute(prefix, @namespace);
            TracingWriter?.WriteAttributeString(prefix, String.Empty, @namespace, String.Empty);
            InternalWriter?.WriteAttributeString(prefix, String.Empty, @namespace, String.Empty);
        }

        /// <summary>
        /// Returns the closest prefix defined in the current namespace scope for the namespace URI.
        /// </summary>
        /// <param name="namespace">The namespace URI whose prefix to find.</param>
        /// <returns>The matching prefix or null if no matching namespace URI is found in the
        /// current scope.</returns>
        public override string LookupPrefix(string @namespace)
        {
            return UseInnerWriter.LookupPrefix(@namespace);
        }

        /// <summary>
        /// Gets the <see cref="UseInnerWriter"/>
        /// </summary>
        /// <exception cref="InvalidOperationException"> if <see cref="InnerWriter"/> is null.</exception>
        protected XmlDictionaryWriter UseInnerWriter
        {
            get => InnerWriter ?? throw LogExceptionMessage(new InvalidOperationException(LogMessages.IDX30028));
        }
    }
}
