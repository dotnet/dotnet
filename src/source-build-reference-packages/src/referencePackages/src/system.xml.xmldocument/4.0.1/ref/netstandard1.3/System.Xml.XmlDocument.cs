// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Xml.XmlDocument")]
[assembly: AssemblyDescription("System.Xml.XmlDocument")]
[assembly: AssemblyDefaultAlias("System.Xml.XmlDocument")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.1.0")]




namespace System.Xml
{
    public partial class XmlAttribute : System.Xml.XmlNode
    {
        protected internal XmlAttribute(string prefix, string localName, string namespaceURI, System.Xml.XmlDocument doc) { }
        public override string BaseURI { get { throw null; } }
        public override string InnerText { set { } }
        public override string InnerXml { set { } }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public override string NamespaceURI { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public override System.Xml.XmlDocument OwnerDocument { get { throw null; } }
        public virtual System.Xml.XmlElement OwnerElement { get { throw null; } }
        public override System.Xml.XmlNode ParentNode { get { throw null; } }
        public override string Prefix { get { throw null; } set { } }
        public virtual bool Specified { get { throw null; } }
        public override string Value { get { throw null; } set { } }
        public override System.Xml.XmlNode AppendChild(System.Xml.XmlNode newChild) { throw null; }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public override System.Xml.XmlNode InsertAfter(System.Xml.XmlNode newChild, System.Xml.XmlNode refChild) { throw null; }
        public override System.Xml.XmlNode InsertBefore(System.Xml.XmlNode newChild, System.Xml.XmlNode refChild) { throw null; }
        public override System.Xml.XmlNode PrependChild(System.Xml.XmlNode newChild) { throw null; }
        public override System.Xml.XmlNode RemoveChild(System.Xml.XmlNode oldChild) { throw null; }
        public override System.Xml.XmlNode ReplaceChild(System.Xml.XmlNode newChild, System.Xml.XmlNode oldChild) { throw null; }
        public override void WriteContentTo(System.Xml.XmlWriter w) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
    public sealed partial class XmlAttributeCollection : System.Xml.XmlNamedNodeMap, System.Collections.ICollection, System.Collections.IEnumerable
    {
        internal XmlAttributeCollection() { }
        [System.Runtime.CompilerServices.IndexerName("ItemOf")]
        public System.Xml.XmlAttribute this[int i] { get { throw null; } }
        [System.Runtime.CompilerServices.IndexerName("ItemOf")]
        public System.Xml.XmlAttribute this[string name] { get { throw null; } }
        [System.Runtime.CompilerServices.IndexerName("ItemOf")]
        public System.Xml.XmlAttribute this[string localName, string namespaceURI] { get { throw null; } }
        int System.Collections.ICollection.Count { get { throw null; } }
        bool System.Collections.ICollection.IsSynchronized { get { throw null; } }
        object System.Collections.ICollection.SyncRoot { get { throw null; } }
        public System.Xml.XmlAttribute Append(System.Xml.XmlAttribute node) { throw null; }
        public void CopyTo(System.Xml.XmlAttribute[] array, int index) { }
        public System.Xml.XmlAttribute InsertAfter(System.Xml.XmlAttribute newNode, System.Xml.XmlAttribute refNode) { throw null; }
        public System.Xml.XmlAttribute InsertBefore(System.Xml.XmlAttribute newNode, System.Xml.XmlAttribute refNode) { throw null; }
        public System.Xml.XmlAttribute Prepend(System.Xml.XmlAttribute node) { throw null; }
        public System.Xml.XmlAttribute Remove(System.Xml.XmlAttribute node) { throw null; }
        public void RemoveAll() { }
        public System.Xml.XmlAttribute RemoveAt(int i) { throw null; }
        public override System.Xml.XmlNode SetNamedItem(System.Xml.XmlNode node) { throw null; }
        void System.Collections.ICollection.CopyTo(System.Array array, int index) { }
    }
    public partial class XmlCDataSection : System.Xml.XmlCharacterData
    {
        protected internal XmlCDataSection(string data, System.Xml.XmlDocument doc) : base (default(string), default(System.Xml.XmlDocument)) { }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public override System.Xml.XmlNode ParentNode { get { throw null; } }
        public override System.Xml.XmlNode PreviousText { get { throw null; } }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public override void WriteContentTo(System.Xml.XmlWriter w) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
    public abstract partial class XmlCharacterData : System.Xml.XmlLinkedNode
    {
        protected internal XmlCharacterData(string data, System.Xml.XmlDocument doc) { }
        public virtual string Data { get { throw null; } set { } }
        public virtual int Length { get { throw null; } }
        public override string Value { get { throw null; } set { } }
        public virtual void AppendData(string strData) { }
        public virtual void DeleteData(int offset, int count) { }
        public virtual void InsertData(int offset, string strData) { }
        public virtual void ReplaceData(int offset, int count, string strData) { }
        public virtual string Substring(int offset, int count) { throw null; }
    }
    public partial class XmlComment : System.Xml.XmlCharacterData
    {
        protected internal XmlComment(string comment, System.Xml.XmlDocument doc) : base (default(string), default(System.Xml.XmlDocument)) { }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public override void WriteContentTo(System.Xml.XmlWriter w) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
    public partial class XmlDeclaration : System.Xml.XmlLinkedNode
    {
        protected internal XmlDeclaration(string version, string encoding, string standalone, System.Xml.XmlDocument doc) { }
        public string Encoding { get { throw null; } set { } }
        public override string InnerText { get { throw null; } set { } }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public string Standalone { get { throw null; } set { } }
        public override string Value { get { throw null; } set { } }
        public string Version { get { throw null; } }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public override void WriteContentTo(System.Xml.XmlWriter w) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
    public partial class XmlDocument : System.Xml.XmlNode
    {
        public XmlDocument() { }
        protected internal XmlDocument(System.Xml.XmlImplementation imp) { }
        public XmlDocument(System.Xml.XmlNameTable nt) { }
        public override string BaseURI { get { throw null; } }
        public System.Xml.XmlElement DocumentElement { get { throw null; } }
        public System.Xml.XmlImplementation Implementation { get { throw null; } }
        public override string InnerText { set { } }
        public override string InnerXml { get { throw null; } set { } }
        public override bool IsReadOnly { get { throw null; } }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public System.Xml.XmlNameTable NameTable { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public override System.Xml.XmlDocument OwnerDocument { get { throw null; } }
        public override System.Xml.XmlNode ParentNode { get { throw null; } }
        public bool PreserveWhitespace { get { throw null; } set { } }
        public event System.Xml.XmlNodeChangedEventHandler NodeChanged { add { } remove { } }
        public event System.Xml.XmlNodeChangedEventHandler NodeChanging { add { } remove { } }
        public event System.Xml.XmlNodeChangedEventHandler NodeInserted { add { } remove { } }
        public event System.Xml.XmlNodeChangedEventHandler NodeInserting { add { } remove { } }
        public event System.Xml.XmlNodeChangedEventHandler NodeRemoved { add { } remove { } }
        public event System.Xml.XmlNodeChangedEventHandler NodeRemoving { add { } remove { } }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public System.Xml.XmlAttribute CreateAttribute(string name) { throw null; }
        public System.Xml.XmlAttribute CreateAttribute(string qualifiedName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlCDataSection CreateCDataSection(string data) { throw null; }
        public virtual System.Xml.XmlComment CreateComment(string data) { throw null; }
        public virtual System.Xml.XmlDocumentFragment CreateDocumentFragment() { throw null; }
        public System.Xml.XmlElement CreateElement(string name) { throw null; }
        public System.Xml.XmlElement CreateElement(string qualifiedName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlElement CreateElement(string prefix, string localName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlNode CreateNode(string nodeTypeString, string name, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlNode CreateNode(System.Xml.XmlNodeType type, string name, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlNode CreateNode(System.Xml.XmlNodeType type, string prefix, string name, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlProcessingInstruction CreateProcessingInstruction(string target, string data) { throw null; }
        public virtual System.Xml.XmlSignificantWhitespace CreateSignificantWhitespace(string text) { throw null; }
        public virtual System.Xml.XmlText CreateTextNode(string text) { throw null; }
        public virtual System.Xml.XmlWhitespace CreateWhitespace(string text) { throw null; }
        public virtual System.Xml.XmlDeclaration CreateXmlDeclaration(string version, string encoding, string standalone) { throw null; }
        public virtual System.Xml.XmlNodeList GetElementsByTagName(string name) { throw null; }
        public virtual System.Xml.XmlNodeList GetElementsByTagName(string localName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlNode ImportNode(System.Xml.XmlNode node, bool deep) { throw null; }
        public virtual void Load(System.IO.Stream inStream) { }
        public virtual void Load(System.IO.TextReader txtReader) { }
        public virtual void Load(System.Xml.XmlReader reader) { }
        public virtual void LoadXml(string xml) { }
        public virtual System.Xml.XmlNode ReadNode(System.Xml.XmlReader reader) { throw null; }
        public virtual void Save(System.IO.Stream outStream) { }
        public virtual void Save(System.IO.TextWriter writer) { }
        public virtual void Save(System.Xml.XmlWriter w) { }
        public override void WriteContentTo(System.Xml.XmlWriter xw) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
    public partial class XmlDocumentFragment : System.Xml.XmlNode
    {
        protected internal XmlDocumentFragment(System.Xml.XmlDocument ownerDocument) { }
        public override string InnerXml { get { throw null; } set { } }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public override System.Xml.XmlDocument OwnerDocument { get { throw null; } }
        public override System.Xml.XmlNode ParentNode { get { throw null; } }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public override void WriteContentTo(System.Xml.XmlWriter w) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
    public partial class XmlElement : System.Xml.XmlLinkedNode
    {
        protected internal XmlElement(string prefix, string localName, string namespaceURI, System.Xml.XmlDocument doc) { }
        public override System.Xml.XmlAttributeCollection Attributes { get { throw null; } }
        public virtual bool HasAttributes { get { throw null; } }
        public override string InnerText { get { throw null; } set { } }
        public override string InnerXml { get { throw null; } set { } }
        public bool IsEmpty { get { throw null; } set { } }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public override string NamespaceURI { get { throw null; } }
        public override System.Xml.XmlNode NextSibling { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public override System.Xml.XmlDocument OwnerDocument { get { throw null; } }
        public override System.Xml.XmlNode ParentNode { get { throw null; } }
        public override string Prefix { get { throw null; } set { } }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public virtual string GetAttribute(string name) { throw null; }
        public virtual string GetAttribute(string localName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlAttribute GetAttributeNode(string name) { throw null; }
        public virtual System.Xml.XmlAttribute GetAttributeNode(string localName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlNodeList GetElementsByTagName(string name) { throw null; }
        public virtual System.Xml.XmlNodeList GetElementsByTagName(string localName, string namespaceURI) { throw null; }
        public virtual bool HasAttribute(string name) { throw null; }
        public virtual bool HasAttribute(string localName, string namespaceURI) { throw null; }
        public override void RemoveAll() { }
        public virtual void RemoveAllAttributes() { }
        public virtual void RemoveAttribute(string name) { }
        public virtual void RemoveAttribute(string localName, string namespaceURI) { }
        public virtual System.Xml.XmlNode RemoveAttributeAt(int i) { throw null; }
        public virtual System.Xml.XmlAttribute RemoveAttributeNode(string localName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlAttribute RemoveAttributeNode(System.Xml.XmlAttribute oldAttr) { throw null; }
        public virtual void SetAttribute(string name, string value) { }
        public virtual string SetAttribute(string localName, string namespaceURI, string value) { throw null; }
        public virtual System.Xml.XmlAttribute SetAttributeNode(string localName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlAttribute SetAttributeNode(System.Xml.XmlAttribute newAttr) { throw null; }
        public override void WriteContentTo(System.Xml.XmlWriter w) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
    public partial class XmlImplementation
    {
        public XmlImplementation() { }
        public XmlImplementation(System.Xml.XmlNameTable nt) { }
        public virtual System.Xml.XmlDocument CreateDocument() { throw null; }
        public bool HasFeature(string strFeature, string strVersion) { throw null; }
    }
    public abstract partial class XmlLinkedNode : System.Xml.XmlNode
    {
        internal XmlLinkedNode() { }
        public override System.Xml.XmlNode NextSibling { get { throw null; } }
        public override System.Xml.XmlNode PreviousSibling { get { throw null; } }
    }
    public partial class XmlNamedNodeMap : System.Collections.IEnumerable
    {
        internal XmlNamedNodeMap() { }
        public virtual int Count { get { throw null; } }
        public virtual System.Collections.IEnumerator GetEnumerator() { throw null; }
        public virtual System.Xml.XmlNode GetNamedItem(string name) { throw null; }
        public virtual System.Xml.XmlNode GetNamedItem(string localName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlNode Item(int index) { throw null; }
        public virtual System.Xml.XmlNode RemoveNamedItem(string name) { throw null; }
        public virtual System.Xml.XmlNode RemoveNamedItem(string localName, string namespaceURI) { throw null; }
        public virtual System.Xml.XmlNode SetNamedItem(System.Xml.XmlNode node) { throw null; }
    }
    public abstract partial class XmlNode : System.Collections.IEnumerable
    {
        internal XmlNode() { }
        public virtual System.Xml.XmlAttributeCollection Attributes { get { throw null; } }
        public virtual string BaseURI { get { throw null; } }
        public virtual System.Xml.XmlNodeList ChildNodes { get { throw null; } }
        public virtual System.Xml.XmlNode FirstChild { get { throw null; } }
        public virtual bool HasChildNodes { get { throw null; } }
        public virtual string InnerText { get { throw null; } set { } }
        public virtual string InnerXml { get { throw null; } set { } }
        public virtual bool IsReadOnly { get { throw null; } }
        public virtual System.Xml.XmlElement this[string name] { get { throw null; } }
        public virtual System.Xml.XmlElement this[string localname, string ns] { get { throw null; } }
        public virtual System.Xml.XmlNode LastChild { get { throw null; } }
        public abstract string LocalName { get; }
        public abstract string Name { get; }
        public virtual string NamespaceURI { get { throw null; } }
        public virtual System.Xml.XmlNode NextSibling { get { throw null; } }
        public abstract System.Xml.XmlNodeType NodeType { get; }
        public virtual string OuterXml { get { throw null; } }
        public virtual System.Xml.XmlDocument OwnerDocument { get { throw null; } }
        public virtual System.Xml.XmlNode ParentNode { get { throw null; } }
        public virtual string Prefix { get { throw null; } set { } }
        public virtual System.Xml.XmlNode PreviousSibling { get { throw null; } }
        public virtual System.Xml.XmlNode PreviousText { get { throw null; } }
        public virtual string Value { get { throw null; } set { } }
        public virtual System.Xml.XmlNode AppendChild(System.Xml.XmlNode newChild) { throw null; }
        public abstract System.Xml.XmlNode CloneNode(bool deep);
        public System.Collections.IEnumerator GetEnumerator() { throw null; }
        public virtual string GetNamespaceOfPrefix(string prefix) { throw null; }
        public virtual string GetPrefixOfNamespace(string namespaceURI) { throw null; }
        public virtual System.Xml.XmlNode InsertAfter(System.Xml.XmlNode newChild, System.Xml.XmlNode refChild) { throw null; }
        public virtual System.Xml.XmlNode InsertBefore(System.Xml.XmlNode newChild, System.Xml.XmlNode refChild) { throw null; }
        public virtual void Normalize() { }
        public virtual System.Xml.XmlNode PrependChild(System.Xml.XmlNode newChild) { throw null; }
        public virtual void RemoveAll() { }
        public virtual System.Xml.XmlNode RemoveChild(System.Xml.XmlNode oldChild) { throw null; }
        public virtual System.Xml.XmlNode ReplaceChild(System.Xml.XmlNode newChild, System.Xml.XmlNode oldChild) { throw null; }
        public virtual bool Supports(string feature, string version) { throw null; }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { throw null; }
        public abstract void WriteContentTo(System.Xml.XmlWriter w);
        public abstract void WriteTo(System.Xml.XmlWriter w);
    }
    public enum XmlNodeChangedAction
    {
        Change = 2,
        Insert = 0,
        Remove = 1,
    }
    public partial class XmlNodeChangedEventArgs : System.EventArgs
    {
        public XmlNodeChangedEventArgs(System.Xml.XmlNode node, System.Xml.XmlNode oldParent, System.Xml.XmlNode newParent, string oldValue, string newValue, System.Xml.XmlNodeChangedAction action) { }
        public System.Xml.XmlNodeChangedAction Action { get { throw null; } }
        public System.Xml.XmlNode NewParent { get { throw null; } }
        public string NewValue { get { throw null; } }
        public System.Xml.XmlNode Node { get { throw null; } }
        public System.Xml.XmlNode OldParent { get { throw null; } }
        public string OldValue { get { throw null; } }
    }
    public delegate void XmlNodeChangedEventHandler(object sender, System.Xml.XmlNodeChangedEventArgs e);
    public abstract partial class XmlNodeList : System.Collections.IEnumerable, System.IDisposable
    {
        protected XmlNodeList() { }
        public abstract int Count { get; }
        [System.Runtime.CompilerServices.IndexerName("ItemOf")]
        public virtual System.Xml.XmlNode this[int i] { get { throw null; } }
        public abstract System.Collections.IEnumerator GetEnumerator();
        public abstract System.Xml.XmlNode Item(int index);
        protected virtual void PrivateDisposeNodeList() { }
        void System.IDisposable.Dispose() { }
    }
    public partial class XmlProcessingInstruction : System.Xml.XmlLinkedNode
    {
        protected internal XmlProcessingInstruction(string target, string data, System.Xml.XmlDocument doc) { }
        public string Data { get { throw null; } set { } }
        public override string InnerText { get { throw null; } set { } }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public string Target { get { throw null; } }
        public override string Value { get { throw null; } set { } }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public override void WriteContentTo(System.Xml.XmlWriter w) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
    public partial class XmlSignificantWhitespace : System.Xml.XmlCharacterData
    {
        protected internal XmlSignificantWhitespace(string strData, System.Xml.XmlDocument doc) : base (default(string), default(System.Xml.XmlDocument)) { }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public override System.Xml.XmlNode ParentNode { get { throw null; } }
        public override System.Xml.XmlNode PreviousText { get { throw null; } }
        public override string Value { get { throw null; } set { } }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public override void WriteContentTo(System.Xml.XmlWriter w) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
    public partial class XmlText : System.Xml.XmlCharacterData
    {
        protected internal XmlText(string strData, System.Xml.XmlDocument doc) : base (default(string), default(System.Xml.XmlDocument)) { }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public override System.Xml.XmlNode ParentNode { get { throw null; } }
        public override System.Xml.XmlNode PreviousText { get { throw null; } }
        public override string Value { get { throw null; } set { } }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public virtual System.Xml.XmlText SplitText(int offset) { throw null; }
        public override void WriteContentTo(System.Xml.XmlWriter w) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
    public partial class XmlWhitespace : System.Xml.XmlCharacterData
    {
        protected internal XmlWhitespace(string strData, System.Xml.XmlDocument doc) : base (default(string), default(System.Xml.XmlDocument)) { }
        public override string LocalName { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Xml.XmlNodeType NodeType { get { throw null; } }
        public override System.Xml.XmlNode ParentNode { get { throw null; } }
        public override System.Xml.XmlNode PreviousText { get { throw null; } }
        public override string Value { get { throw null; } set { } }
        public override System.Xml.XmlNode CloneNode(bool deep) { throw null; }
        public override void WriteContentTo(System.Xml.XmlWriter w) { }
        public override void WriteTo(System.Xml.XmlWriter w) { }
    }
}
