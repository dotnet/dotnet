// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Xml.XmlDocument")]
[assembly: System.Reflection.AssemblyDescription("System.Xml.XmlDocument")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Xml.XmlDocument")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Xml
{
    public partial class XmlAttribute : XmlNode
    {
        protected internal XmlAttribute(string prefix, string localName, string namespaceURI, XmlDocument doc) { }

        public override string BaseURI { get { throw null; } }

        public override string InnerText { set { } }

        public override string InnerXml { set { } }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override string NamespaceURI { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public override XmlDocument OwnerDocument { get { throw null; } }

        public virtual XmlElement OwnerElement { get { throw null; } }

        public override XmlNode ParentNode { get { throw null; } }

        public override string Prefix { get { throw null; } set { } }

        public virtual bool Specified { get { throw null; } }

        public override string Value { get { throw null; } set { } }

        public override XmlNode AppendChild(XmlNode newChild) { throw null; }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public override XmlNode InsertAfter(XmlNode newChild, XmlNode refChild) { throw null; }

        public override XmlNode InsertBefore(XmlNode newChild, XmlNode refChild) { throw null; }

        public override XmlNode PrependChild(XmlNode newChild) { throw null; }

        public override XmlNode RemoveChild(XmlNode oldChild) { throw null; }

        public override XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild) { throw null; }

        public override void WriteContentTo(XmlWriter w) { }

        public override void WriteTo(XmlWriter w) { }
    }

    public sealed partial class XmlAttributeCollection : XmlNamedNodeMap, Collections.ICollection, Collections.IEnumerable
    {
        internal XmlAttributeCollection() { }

        public XmlAttribute this[int i] { get { throw null; } }

        public XmlAttribute this[string localName, string namespaceURI] { get { throw null; } }

        public XmlAttribute this[string name] { get { throw null; } }

        int Collections.ICollection.Count { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public XmlAttribute Append(XmlAttribute node) { throw null; }

        public void CopyTo(XmlAttribute[] array, int index) { }

        public XmlAttribute InsertAfter(XmlAttribute newNode, XmlAttribute refNode) { throw null; }

        public XmlAttribute InsertBefore(XmlAttribute newNode, XmlAttribute refNode) { throw null; }

        public XmlAttribute Prepend(XmlAttribute node) { throw null; }

        public XmlAttribute Remove(XmlAttribute node) { throw null; }

        public void RemoveAll() { }

        public XmlAttribute RemoveAt(int i) { throw null; }

        public override XmlNode SetNamedItem(XmlNode node) { throw null; }

        void Collections.ICollection.CopyTo(Array array, int index) { }
    }

    public partial class XmlCDataSection : XmlCharacterData
    {
        protected internal XmlCDataSection(string data, XmlDocument doc) : base(default!, default!) { }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public override XmlNode ParentNode { get { throw null; } }

        public override XmlNode PreviousText { get { throw null; } }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public override void WriteContentTo(XmlWriter w) { }

        public override void WriteTo(XmlWriter w) { }
    }

    public abstract partial class XmlCharacterData : XmlLinkedNode
    {
        protected internal XmlCharacterData(string data, XmlDocument doc) { }

        public virtual string Data { get { throw null; } set { } }

        public virtual int Length { get { throw null; } }

        public override string Value { get { throw null; } set { } }

        public virtual void AppendData(string strData) { }

        public virtual void DeleteData(int offset, int count) { }

        public virtual void InsertData(int offset, string strData) { }

        public virtual void ReplaceData(int offset, int count, string strData) { }

        public virtual string Substring(int offset, int count) { throw null; }
    }

    public partial class XmlComment : XmlCharacterData
    {
        protected internal XmlComment(string comment, XmlDocument doc) : base(default!, default!) { }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public override void WriteContentTo(XmlWriter w) { }

        public override void WriteTo(XmlWriter w) { }
    }

    public partial class XmlDeclaration : XmlLinkedNode
    {
        protected internal XmlDeclaration(string version, string encoding, string standalone, XmlDocument doc) { }

        public string Encoding { get { throw null; } set { } }

        public override string InnerText { get { throw null; } set { } }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public string Standalone { get { throw null; } set { } }

        public override string Value { get { throw null; } set { } }

        public string Version { get { throw null; } }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public override void WriteContentTo(XmlWriter w) { }

        public override void WriteTo(XmlWriter w) { }
    }

    public partial class XmlDocument : XmlNode
    {
        public XmlDocument() { }

        protected internal XmlDocument(XmlImplementation imp) { }

        public XmlDocument(XmlNameTable nt) { }

        public override string BaseURI { get { throw null; } }

        public XmlElement DocumentElement { get { throw null; } }

        public XmlImplementation Implementation { get { throw null; } }

        public override string InnerText { set { } }

        public override string InnerXml { get { throw null; } set { } }

        public override bool IsReadOnly { get { throw null; } }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public XmlNameTable NameTable { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public override XmlDocument OwnerDocument { get { throw null; } }

        public override XmlNode ParentNode { get { throw null; } }

        public bool PreserveWhitespace { get { throw null; } set { } }

        public event XmlNodeChangedEventHandler NodeChanged { add { } remove { } }

        public event XmlNodeChangedEventHandler NodeChanging { add { } remove { } }

        public event XmlNodeChangedEventHandler NodeInserted { add { } remove { } }

        public event XmlNodeChangedEventHandler NodeInserting { add { } remove { } }

        public event XmlNodeChangedEventHandler NodeRemoved { add { } remove { } }

        public event XmlNodeChangedEventHandler NodeRemoving { add { } remove { } }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public virtual XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI) { throw null; }

        public XmlAttribute CreateAttribute(string qualifiedName, string namespaceURI) { throw null; }

        public XmlAttribute CreateAttribute(string name) { throw null; }

        public virtual XmlCDataSection CreateCDataSection(string data) { throw null; }

        public virtual XmlComment CreateComment(string data) { throw null; }

        public virtual XmlDocumentFragment CreateDocumentFragment() { throw null; }

        public virtual XmlElement CreateElement(string prefix, string localName, string namespaceURI) { throw null; }

        public XmlElement CreateElement(string qualifiedName, string namespaceURI) { throw null; }

        public XmlElement CreateElement(string name) { throw null; }

        public virtual XmlNode CreateNode(string nodeTypeString, string name, string namespaceURI) { throw null; }

        public virtual XmlNode CreateNode(XmlNodeType type, string prefix, string name, string namespaceURI) { throw null; }

        public virtual XmlNode CreateNode(XmlNodeType type, string name, string namespaceURI) { throw null; }

        public virtual XmlProcessingInstruction CreateProcessingInstruction(string target, string data) { throw null; }

        public virtual XmlSignificantWhitespace CreateSignificantWhitespace(string text) { throw null; }

        public virtual XmlText CreateTextNode(string text) { throw null; }

        public virtual XmlWhitespace CreateWhitespace(string text) { throw null; }

        public virtual XmlDeclaration CreateXmlDeclaration(string version, string encoding, string standalone) { throw null; }

        public virtual XmlNodeList GetElementsByTagName(string localName, string namespaceURI) { throw null; }

        public virtual XmlNodeList GetElementsByTagName(string name) { throw null; }

        public virtual XmlNode ImportNode(XmlNode node, bool deep) { throw null; }

        public virtual void Load(IO.Stream inStream) { }

        public virtual void Load(IO.TextReader txtReader) { }

        public virtual void Load(XmlReader reader) { }

        public virtual void LoadXml(string xml) { }

        public virtual XmlNode ReadNode(XmlReader reader) { throw null; }

        public virtual void Save(IO.Stream outStream) { }

        public virtual void Save(IO.TextWriter writer) { }

        public virtual void Save(XmlWriter w) { }

        public override void WriteContentTo(XmlWriter xw) { }

        public override void WriteTo(XmlWriter w) { }
    }

    public partial class XmlDocumentFragment : XmlNode
    {
        protected internal XmlDocumentFragment(XmlDocument ownerDocument) { }

        public override string InnerXml { get { throw null; } set { } }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public override XmlDocument OwnerDocument { get { throw null; } }

        public override XmlNode ParentNode { get { throw null; } }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public override void WriteContentTo(XmlWriter w) { }

        public override void WriteTo(XmlWriter w) { }
    }

    public partial class XmlElement : XmlLinkedNode
    {
        protected internal XmlElement(string prefix, string localName, string namespaceURI, XmlDocument doc) { }

        public override XmlAttributeCollection Attributes { get { throw null; } }

        public virtual bool HasAttributes { get { throw null; } }

        public override string InnerText { get { throw null; } set { } }

        public override string InnerXml { get { throw null; } set { } }

        public bool IsEmpty { get { throw null; } set { } }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override string NamespaceURI { get { throw null; } }

        public override XmlNode NextSibling { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public override XmlDocument OwnerDocument { get { throw null; } }

        public override XmlNode ParentNode { get { throw null; } }

        public override string Prefix { get { throw null; } set { } }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public virtual string GetAttribute(string localName, string namespaceURI) { throw null; }

        public virtual string GetAttribute(string name) { throw null; }

        public virtual XmlAttribute GetAttributeNode(string localName, string namespaceURI) { throw null; }

        public virtual XmlAttribute GetAttributeNode(string name) { throw null; }

        public virtual XmlNodeList GetElementsByTagName(string localName, string namespaceURI) { throw null; }

        public virtual XmlNodeList GetElementsByTagName(string name) { throw null; }

        public virtual bool HasAttribute(string localName, string namespaceURI) { throw null; }

        public virtual bool HasAttribute(string name) { throw null; }

        public override void RemoveAll() { }

        public virtual void RemoveAllAttributes() { }

        public virtual void RemoveAttribute(string localName, string namespaceURI) { }

        public virtual void RemoveAttribute(string name) { }

        public virtual XmlNode RemoveAttributeAt(int i) { throw null; }

        public virtual XmlAttribute RemoveAttributeNode(string localName, string namespaceURI) { throw null; }

        public virtual XmlAttribute RemoveAttributeNode(XmlAttribute oldAttr) { throw null; }

        public virtual string SetAttribute(string localName, string namespaceURI, string value) { throw null; }

        public virtual void SetAttribute(string name, string value) { }

        public virtual XmlAttribute SetAttributeNode(string localName, string namespaceURI) { throw null; }

        public virtual XmlAttribute SetAttributeNode(XmlAttribute newAttr) { throw null; }

        public override void WriteContentTo(XmlWriter w) { }

        public override void WriteTo(XmlWriter w) { }
    }

    public partial class XmlImplementation
    {
        public XmlImplementation() { }

        public XmlImplementation(XmlNameTable nt) { }

        public virtual XmlDocument CreateDocument() { throw null; }

        public bool HasFeature(string strFeature, string strVersion) { throw null; }
    }

    public abstract partial class XmlLinkedNode : XmlNode
    {
        internal XmlLinkedNode() { }

        public override XmlNode NextSibling { get { throw null; } }

        public override XmlNode PreviousSibling { get { throw null; } }
    }

    public partial class XmlNamedNodeMap : Collections.IEnumerable
    {
        internal XmlNamedNodeMap() { }

        public virtual int Count { get { throw null; } }

        public virtual Collections.IEnumerator GetEnumerator() { throw null; }

        public virtual XmlNode GetNamedItem(string localName, string namespaceURI) { throw null; }

        public virtual XmlNode GetNamedItem(string name) { throw null; }

        public virtual XmlNode Item(int index) { throw null; }

        public virtual XmlNode RemoveNamedItem(string localName, string namespaceURI) { throw null; }

        public virtual XmlNode RemoveNamedItem(string name) { throw null; }

        public virtual XmlNode SetNamedItem(XmlNode node) { throw null; }
    }

    public abstract partial class XmlNode : Collections.IEnumerable
    {
        internal XmlNode() { }

        public virtual XmlAttributeCollection Attributes { get { throw null; } }

        public virtual string BaseURI { get { throw null; } }

        public virtual XmlNodeList ChildNodes { get { throw null; } }

        public virtual XmlNode FirstChild { get { throw null; } }

        public virtual bool HasChildNodes { get { throw null; } }

        public virtual string InnerText { get { throw null; } set { } }

        public virtual string InnerXml { get { throw null; } set { } }

        public virtual bool IsReadOnly { get { throw null; } }

        public virtual XmlElement this[string localname, string ns] { get { throw null; } }

        public virtual XmlElement this[string name] { get { throw null; } }

        public virtual XmlNode LastChild { get { throw null; } }

        public abstract string LocalName { get; }
        public abstract string Name { get; }

        public virtual string NamespaceURI { get { throw null; } }

        public virtual XmlNode NextSibling { get { throw null; } }

        public abstract XmlNodeType NodeType { get; }

        public virtual string OuterXml { get { throw null; } }

        public virtual XmlDocument OwnerDocument { get { throw null; } }

        public virtual XmlNode ParentNode { get { throw null; } }

        public virtual string Prefix { get { throw null; } set { } }

        public virtual XmlNode PreviousSibling { get { throw null; } }

        public virtual XmlNode PreviousText { get { throw null; } }

        public virtual string Value { get { throw null; } set { } }

        public virtual XmlNode AppendChild(XmlNode newChild) { throw null; }

        public abstract XmlNode CloneNode(bool deep);
        public Collections.IEnumerator GetEnumerator() { throw null; }

        public virtual string GetNamespaceOfPrefix(string prefix) { throw null; }

        public virtual string GetPrefixOfNamespace(string namespaceURI) { throw null; }

        public virtual XmlNode InsertAfter(XmlNode newChild, XmlNode refChild) { throw null; }

        public virtual XmlNode InsertBefore(XmlNode newChild, XmlNode refChild) { throw null; }

        public virtual void Normalize() { }

        public virtual XmlNode PrependChild(XmlNode newChild) { throw null; }

        public virtual void RemoveAll() { }

        public virtual XmlNode RemoveChild(XmlNode oldChild) { throw null; }

        public virtual XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild) { throw null; }

        public virtual bool Supports(string feature, string version) { throw null; }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        public abstract void WriteContentTo(XmlWriter w);
        public abstract void WriteTo(XmlWriter w);
    }

    public enum XmlNodeChangedAction
    {
        Insert = 0,
        Remove = 1,
        Change = 2
    }

    public partial class XmlNodeChangedEventArgs : EventArgs
    {
        public XmlNodeChangedEventArgs(XmlNode node, XmlNode oldParent, XmlNode newParent, string oldValue, string newValue, XmlNodeChangedAction action) { }

        public XmlNodeChangedAction Action { get { throw null; } }

        public XmlNode NewParent { get { throw null; } }

        public string NewValue { get { throw null; } }

        public XmlNode Node { get { throw null; } }

        public XmlNode OldParent { get { throw null; } }

        public string OldValue { get { throw null; } }
    }

    public delegate void XmlNodeChangedEventHandler(object sender, XmlNodeChangedEventArgs e);
    public abstract partial class XmlNodeList : Collections.IEnumerable, IDisposable
    {
        public abstract int Count { get; }
        [Runtime.CompilerServices.IndexerName("ItemOf")]
        public virtual XmlNode this[int i] { get { throw null; } }

        public abstract Collections.IEnumerator GetEnumerator();
        public abstract XmlNode Item(int index);
        protected virtual void PrivateDisposeNodeList() { }

        void IDisposable.Dispose() { }
    }

    public partial class XmlProcessingInstruction : XmlLinkedNode
    {
        protected internal XmlProcessingInstruction(string target, string data, XmlDocument doc) { }

        public string Data { get { throw null; } set { } }

        public override string InnerText { get { throw null; } set { } }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public string Target { get { throw null; } }

        public override string Value { get { throw null; } set { } }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public override void WriteContentTo(XmlWriter w) { }

        public override void WriteTo(XmlWriter w) { }
    }

    public partial class XmlSignificantWhitespace : XmlCharacterData
    {
        protected internal XmlSignificantWhitespace(string strData, XmlDocument doc) : base(default!, default!) { }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public override XmlNode ParentNode { get { throw null; } }

        public override XmlNode PreviousText { get { throw null; } }

        public override string Value { get { throw null; } set { } }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public override void WriteContentTo(XmlWriter w) { }

        public override void WriteTo(XmlWriter w) { }
    }

    public partial class XmlText : XmlCharacterData
    {
        protected internal XmlText(string strData, XmlDocument doc) : base(default!, default!) { }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public override XmlNode ParentNode { get { throw null; } }

        public override XmlNode PreviousText { get { throw null; } }

        public override string Value { get { throw null; } set { } }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public virtual XmlText SplitText(int offset) { throw null; }

        public override void WriteContentTo(XmlWriter w) { }

        public override void WriteTo(XmlWriter w) { }
    }

    public partial class XmlWhitespace : XmlCharacterData
    {
        protected internal XmlWhitespace(string strData, XmlDocument doc) : base(default!, default!) { }

        public override string LocalName { get { throw null; } }

        public override string Name { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public override XmlNode ParentNode { get { throw null; } }

        public override XmlNode PreviousText { get { throw null; } }

        public override string Value { get { throw null; } set { } }

        public override XmlNode CloneNode(bool deep) { throw null; }

        public override void WriteContentTo(XmlWriter w) { }

        public override void WriteTo(XmlWriter w) { }
    }
}