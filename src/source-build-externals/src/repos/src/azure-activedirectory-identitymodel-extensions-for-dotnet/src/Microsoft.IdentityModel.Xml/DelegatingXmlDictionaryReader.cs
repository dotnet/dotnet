// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml;
using static Microsoft.IdentityModel.Logging.LogHelper;

namespace Microsoft.IdentityModel.Xml
{
    /// <summary>
    /// Wraps a <see cref="XmlDictionaryReader"/> delegates to InnerReader.
    /// </summary>
    public class DelegatingXmlDictionaryReader : XmlDictionaryReader, IXmlLineInfo
    {
        private XmlDictionaryReader _innerReader;

        /// <summary>
        /// Creates a new <see cref="DelegatingXmlDictionaryReader"/>.
        /// </summary>
        protected DelegatingXmlDictionaryReader()
        {
        }

        /// <summary>
        /// Gets or sets the Inner <see cref="XmlDictionaryReader"/>.
        /// </summary>
        protected XmlDictionaryReader InnerReader
        {
            get => _innerReader;
            set => _innerReader = value ?? throw LogArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets the value of the InnerReaders's attribute with the specified index.
        /// </summary>
        /// <param name="i">index of the attribute.</param>
        /// <returns>Attribute value at the specified index.</returns>
        public override string this[int i]
        {
            get => UseInnerReader[i];
        }

        /// <summary>
        /// Gets the value of the InnerReaders's attribute with the specified Name.
        /// </summary>
        /// <param name="name">The qualified name of the attribute.</param>
        /// <returns>The value of the specified attribute. If the attribute is not found, 
        /// null is returned.</returns>
        public override string this[string name]
        {
            get => UseInnerReader[name];
        }

        /// <summary>
        /// Gets the value of the InnerReaders's attribute with the specified LocalName and NamespaceURI.
        /// </summary>
        /// <param name="name">The local name of the attribute.</param>
        /// <param name="namespace">The namespace URI of the attribute.</param>
        /// <returns>The value of the specified attribute. If the attribute is not found, 
        /// null is returned.</returns>
        public override string this[string name, string @namespace]
        {
            get => UseInnerReader[name, @namespace];
        }

        /// <summary>
        /// Gets the number of InnerReaders's attributes at the current reader position.
        /// </summary>
        public override int AttributeCount
        {
            get => UseInnerReader.AttributeCount;
        }

        /// <summary>
        /// Gets the InnerReaders's base Uri of the current node.
        /// </summary>
        public override string BaseURI
        {
            get => UseInnerReader.BaseURI;
        }

        /// <summary>
        /// Gets a value indicating if the InnerReader can read binary content
        /// </summary>
        public override bool CanReadBinaryContent
        {
            get => UseInnerReader.CanReadBinaryContent;
        }

        /// <summary>
        /// Gets a value indicating if the InnerReader can read value chunk.
        /// </summary>
        public override bool CanReadValueChunk
        {
            get => UseInnerReader.CanReadValueChunk;
        }

        /// <summary>
        /// Gets the  InnerReaders's current depth.
        /// </summary>
        public override int Depth
        {
            get => UseInnerReader.Depth;
        }

        /// <summary>
        /// Gets a value indicating if the InnerReader is positioned at the end of the stream.
        /// </summary>
        public override bool EOF
        {
            get => UseInnerReader.EOF;
        }

        /// <summary>
        /// Gets a value indicating if the InnerReader current node has a Value.
        /// </summary>
        public override bool HasValue
        {
            get => UseInnerReader.HasValue;
        }

        /// <summary>
        /// Gets a value indicating if the InnerReader's current node is an attribute that
        /// was generated from the default value defined in the DTD or Schema.
        /// </summary>
        public override bool IsDefault
        {
            get => UseInnerReader.IsDefault;
        }

        /// <summary>
        /// Gets a value indicating if the InnerReader's current node is empty.
        /// </summary>
        public override bool IsEmptyElement
        {
            get => UseInnerReader.IsEmptyElement;
        }

        /// <summary>
        /// Gets the InnerReader's LineNumber
        /// </summary>
        /// <remarks>If the InnerReader does not support <see cref="IXmlLineInfo"/> 1 is returned.</remarks>
        public int LineNumber
        {
            get
            {
                var lineInfo = UseInnerReader as IXmlLineInfo;
                if (lineInfo == null)
                    return 1;

                return lineInfo.LineNumber;
            }
        }

        /// <summary>
        /// Gets the InnerReader's LinePosition.
        /// </summary>
        /// <remarks>If the InnerReader does not support <see cref="IXmlLineInfo"/> 1 is returned.</remarks>
        public int LinePosition
        {
            get
            {
                var lineInfo = UseInnerReader as IXmlLineInfo;
                if (lineInfo == null)
                    return 1;

                return lineInfo.LinePosition;
            }
        }

        /// <summary>
        /// Gets the InnerReader's LocalName of the current node.
        /// </summary>
        public override string LocalName
        {
            get { return UseInnerReader.LocalName; }
        }

        /// <summary>
        /// Gets the InnerReader's Name of the current node.
        /// </summary>
        public override string Name
        {
            get => UseInnerReader.Name;
        }

        /// <summary>
        /// Gets the InnerReader's NamespaceURI of the current node.
        /// </summary>
        public override string NamespaceURI
        {
            get => UseInnerReader.NamespaceURI;
        }

        /// <summary>
        /// Gets the InnerReader's XmlNameTable at the current node.
        /// </summary>
        public override XmlNameTable NameTable
        {
            get => UseInnerReader.NameTable;
        }

        /// <summary>
        /// Gets the type of the InnerReader's current node type.
        /// </summary>
        public override XmlNodeType NodeType
        {
            get => UseInnerReader.NodeType;
        }

        /// <summary>
        /// Gets the prefix of the InnerReader's current node.
        /// </summary>
        public override string Prefix
        {
            get => UseInnerReader.Prefix;
        }

        /// <summary>
        /// Gets the InnerReader's ReadState. 
        /// </summary>
        public override ReadState ReadState
        {
            get => UseInnerReader.ReadState;
        }

        /// <summary>
        /// Gets the Value of the InnerReader's current node.
        /// </summary>
        public override string Value
        {
            get => UseInnerReader.Value;
        }

        /// <summary>
        /// Gets the ValueType of InnerReader's current node.
        /// </summary>
        public override Type ValueType
        {
            get => UseInnerReader.ValueType;
        }

        /// <summary>
        /// Gets the InnerReader's XmlLang.
        /// </summary>
        public override string XmlLang
        {
            get => UseInnerReader.XmlLang;
        }

        /// <summary>
        /// Gets the InnerReader's XmlSpace.
        /// </summary>
        public override XmlSpace XmlSpace
        {
            get => UseInnerReader.XmlSpace;
        }

        /// <summary>
        /// Closes the reader and changes the System.Xml.XmlReader.ReadState
        /// to Closed.
        /// </summary>
        public override void Close()
        {
            UseInnerReader.Close();
        }

        /// <summary>
        /// Gets the value of the InnerReader's attribute at the given index.
        /// </summary>
        /// <param name="i">The index of the attribute. The index is 0 based index.</param>
        /// <returns>The value of the attribute at the specified index.</returns>
        /// <remarks>The method does not move the reader position.</remarks>
        public override string GetAttribute(int i)
        {
            return UseInnerReader.GetAttribute(i);
        }

        /// <summary>
        /// Gets the value of the InnerReader's attribute with the given name.
        /// </summary>
        /// <param name="name">The qualified name of the attribute.</param>
        /// <returns>The value of the attribute. If the attribute is not found null
        /// is returned.</returns>
        /// <remarks>The method does not move the reader position.</remarks>
        public override string GetAttribute(string name)
        {
            return UseInnerReader.GetAttribute(name);
        }

        /// <summary>
        /// Gets the value of the InnerReader's attribute with the given name and namespace Uri.
        /// </summary>
        /// <param name="name">The local name of the attribute.</param>
        /// <param name="namespace">The namespace of the attribute.</param>
        /// <returns>The value of the attribute. If the attribute is not found
        /// null is returned.</returns>
        /// <remarks>The method does not move the reader.</remarks>
        public override string GetAttribute(string name, string @namespace)
        {
            return UseInnerReader.GetAttribute(name, @namespace);
        }

        /// <summary>
        /// Gets a value indicating if the InnerReader HasLineInfo
        /// </summary>
        public bool HasLineInfo()
        {
            var lineInfo = UseInnerReader as IXmlLineInfo;

            if (lineInfo == null)
                return false;

            return lineInfo.HasLineInfo();
        }

        /// <summary>
        /// Resolves the InnerReader's namespace prefix in the current element scope.
        /// </summary>
        /// <param name="prefix">Prefix whose namespace Uri to be resolved.</param>
        /// <returns>The namespace Uri to which the prefix matches or null if no matching
        /// prefix is found.</returns>
        public override string LookupNamespace(string prefix)
        {
            return UseInnerReader.LookupNamespace(prefix);
        }

        /// <summary>
        /// Moves to the InnerReader's attribute with the specified index.
        /// </summary>
        /// <param name="index">The index of the attribute.</param>
        public override void MoveToAttribute(int index)
        {
            UseInnerReader.MoveToAttribute(index);
        }

        /// <summary>
        /// Moves to the InnerReader's attribute with the given local name.
        /// </summary>
        /// <param name="name">The qualified name of the attribute.</param>
        /// <returns>true if the attribute is found; otherwise, false.</returns>
        public override bool MoveToAttribute(string name)
        {
            return UseInnerReader.MoveToAttribute(name);
        }

        /// <summary>
        /// Moves to the InnerReader's attribute with the specified LocalName and NamespaceURI.
        /// </summary>
        /// <param name="name">The local name of the attribute.</param>
        /// <param name="namespace">The namespace URI of the attribute.</param>
        /// <returns>true if the attribute is found; otherwise, false.</returns>
        public override bool MoveToAttribute(string name, string @namespace)
        {
            return UseInnerReader.MoveToAttribute(name, @namespace);
        }

        /// <summary>
        /// Moves the InnerReader to a node of type Element.
        /// </summary>
        /// <returns>true if the reader is positioned on an element else false</returns>
        public override bool MoveToElement()
        {
            return UseInnerReader.MoveToElement();
        }

        /// <summary>
        /// Moves the InnerReader to the first attribute.
        /// </summary>
        /// <returns>Returns true if the reader is positioned at a attribute else false.</returns>
        /// <remarks>When returning false the reader position will not be changed.</remarks>
        public override bool MoveToFirstAttribute()
        {
            return UseInnerReader.MoveToFirstAttribute();
        }

        /// <summary>
        /// Moves the InnerReader to the next attribute.
        /// </summary>
        /// <returns>Returns true if the reader is positioned at an attribute else false.</returns>
        /// <remarks>When returning false the reader position will not be changed.</remarks>
        public override bool MoveToNextAttribute()
        {
            return UseInnerReader.MoveToNextAttribute();
        }

        /// <summary>
        /// Reads the InnerReader's next node from the stream.
        /// </summary>
        /// <returns>true if the next node was read successfully.</returns>
        public override bool Read()
        {
            return UseInnerReader.Read();
        }

        /// <summary>
        /// Parses the InnerReader's attribute value into one or more Text, EntityReference, or EndEntity nodes.
        /// </summary>
        /// <returns>true if there are nodes to return.false if the reader is not positioned on
        /// an attribute node when the initial call is made or if all the attribute values
        /// have been read.</returns>
        public override bool ReadAttributeValue()
        {
            return UseInnerReader.ReadAttributeValue();
        }

        /// <summary>
        /// Reads the InnerReader's content and returns the Base64 decoded binary bytes.
        /// </summary>
        /// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
        /// <param name="index">The offset into the buffer where to start copying the result.</param>
        /// <param name="count">The maximum number of bytes to copy into the buffer.</param>
        /// <returns>The number of bytes written to the buffer.</returns>
        public override int ReadContentAsBase64(byte[] buffer, int index, int count)
        {
            return UseInnerReader.ReadContentAsBase64(buffer, index, count);
        }

        /// <summary>
        /// Reads the InnerReader's content and returns the BinHex decoded binary bytes.
        /// </summary>
        /// <param name="buffer">The buffer into which to copy the resulting text. This value cannot be null.</param>
        /// <param name="index">The offset into the buffer where to start copying the result.</param>
        /// <param name="count">The maximum number of bytes to copy into the buffer.</param>
        /// <returns>The number of bytes written to the buffer.</returns>
        public override int ReadContentAsBinHex(byte[] buffer, int index, int count)
        {
            return UseInnerReader.ReadContentAsBinHex(buffer, index, count);
        }

        /// <summary>
        /// Reads the content and returns the contained string.
        /// </summary>
        public override UniqueId ReadContentAsUniqueId()
        {
            return UseInnerReader.ReadContentAsUniqueId();
        }

        /// <summary>
        /// Resolves the InnerReader's EntityReference nodes.
        /// </summary>
        public override void ResolveEntity()
        {
            UseInnerReader.ResolveEntity();
        }

        /// <summary>
        /// Reads large streams of text embedded in an XML document from the InnerReader.
        /// </summary>
        /// <param name="buffer">The array of characters that serves as the buffer to which the text contents
        /// are written. This value cannot be null.</param>
        /// <param name="index">The offset within the buffer where the System.Xml.XmlReader can start to
        /// copy the results.</param>
        /// <param name="count">The maximum number of characters to copy into the buffer. The actual number
        /// of characters copied is returned from this method.</param>
        /// <returns>The number of characters read into the buffer. The value zero is returned
        /// when there is no more text content.</returns>
        public override int ReadValueChunk(char[] buffer, int index, int count)
        {
            return UseInnerReader.ReadValueChunk(buffer, index, count);
        }

        /// <summary>
        /// Gets the <see cref="UseInnerReader"/>
        /// </summary>
        /// <exception cref="InvalidOperationException"> if <see cref="InnerReader"/> is null.</exception>
        protected XmlDictionaryReader UseInnerReader
        {
            get => InnerReader ?? throw LogExceptionMessage(new InvalidOperationException(LogMessages.IDX30027));
        }
    }
}
