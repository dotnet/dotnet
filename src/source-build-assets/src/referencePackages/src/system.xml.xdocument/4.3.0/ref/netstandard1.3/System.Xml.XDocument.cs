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
[assembly: System.Reflection.AssemblyTitle("System.Xml.XDocument")]
[assembly: System.Reflection.AssemblyDescription("System.Xml.XDocument")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Xml.XDocument")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.10.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Xml.Linq
{
    public static partial class Extensions
    {
        public static Collections.Generic.IEnumerable<XElement> Ancestors<T>(this Collections.Generic.IEnumerable<T> source, XName name)
            where T : XNode { throw null; }

        public static Collections.Generic.IEnumerable<XElement> Ancestors<T>(this Collections.Generic.IEnumerable<T> source)
            where T : XNode { throw null; }

        public static Collections.Generic.IEnumerable<XElement> AncestorsAndSelf(this Collections.Generic.IEnumerable<XElement> source, XName name) { throw null; }

        public static Collections.Generic.IEnumerable<XElement> AncestorsAndSelf(this Collections.Generic.IEnumerable<XElement> source) { throw null; }

        public static Collections.Generic.IEnumerable<XAttribute> Attributes(this Collections.Generic.IEnumerable<XElement> source, XName name) { throw null; }

        public static Collections.Generic.IEnumerable<XAttribute> Attributes(this Collections.Generic.IEnumerable<XElement> source) { throw null; }

        public static Collections.Generic.IEnumerable<XNode> DescendantNodes<T>(this Collections.Generic.IEnumerable<T> source)
            where T : XContainer { throw null; }

        public static Collections.Generic.IEnumerable<XNode> DescendantNodesAndSelf(this Collections.Generic.IEnumerable<XElement> source) { throw null; }

        public static Collections.Generic.IEnumerable<XElement> Descendants<T>(this Collections.Generic.IEnumerable<T> source, XName name)
            where T : XContainer { throw null; }

        public static Collections.Generic.IEnumerable<XElement> Descendants<T>(this Collections.Generic.IEnumerable<T> source)
            where T : XContainer { throw null; }

        public static Collections.Generic.IEnumerable<XElement> DescendantsAndSelf(this Collections.Generic.IEnumerable<XElement> source, XName name) { throw null; }

        public static Collections.Generic.IEnumerable<XElement> DescendantsAndSelf(this Collections.Generic.IEnumerable<XElement> source) { throw null; }

        public static Collections.Generic.IEnumerable<XElement> Elements<T>(this Collections.Generic.IEnumerable<T> source, XName name)
            where T : XContainer { throw null; }

        public static Collections.Generic.IEnumerable<XElement> Elements<T>(this Collections.Generic.IEnumerable<T> source)
            where T : XContainer { throw null; }

        public static Collections.Generic.IEnumerable<T> InDocumentOrder<T>(this Collections.Generic.IEnumerable<T> source)
            where T : XNode { throw null; }

        public static Collections.Generic.IEnumerable<XNode> Nodes<T>(this Collections.Generic.IEnumerable<T> source)
            where T : XContainer { throw null; }

        public static void Remove(this Collections.Generic.IEnumerable<XAttribute> source) { }

        public static void Remove<T>(this Collections.Generic.IEnumerable<T> source)
            where T : XNode { }
    }

    [Flags]
    public enum LoadOptions
    {
        None = 0,
        PreserveWhitespace = 1,
        SetBaseUri = 2,
        SetLineInfo = 4
    }

    [Flags]
    public enum ReaderOptions
    {
        None = 0,
        OmitDuplicateNamespaces = 1
    }

    [Flags]
    public enum SaveOptions
    {
        None = 0,
        DisableFormatting = 1,
        OmitDuplicateNamespaces = 2
    }

    public partial class XAttribute : XObject
    {
        public XAttribute(XAttribute other) { }

        public XAttribute(XName name, object value) { }

        public static Collections.Generic.IEnumerable<XAttribute> EmptySequence { get { throw null; } }

        public bool IsNamespaceDeclaration { get { throw null; } }

        public XName Name { get { throw null; } }

        public XAttribute NextAttribute { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public XAttribute PreviousAttribute { get { throw null; } }

        public string Value { get { throw null; } set { } }

        [CLSCompliant(false)]
        public static explicit operator bool(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator DateTime(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator DateTimeOffset(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator decimal(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator double(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator Guid(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator int(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator long(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator bool?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator DateTime?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator DateTimeOffset?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator decimal?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator double?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator Guid?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator int?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator long?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator float?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator TimeSpan?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator uint?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator ulong?(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator float(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator string(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator TimeSpan(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator uint(XAttribute attribute) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator ulong(XAttribute attribute) { throw null; }

        public void Remove() { }

        public void SetValue(object value) { }

        public override string ToString() { throw null; }
    }

    public partial class XCData : XText
    {
        public XCData(string value) : base(default(string)!) { }

        public XCData(XCData other) : base(default(string)!) { }

        public override XmlNodeType NodeType { get { throw null; } }

        public override void WriteTo(XmlWriter writer) { }
    }

    public partial class XComment : XNode
    {
        public XComment(string value) { }

        public XComment(XComment other) { }

        public override XmlNodeType NodeType { get { throw null; } }

        public string Value { get { throw null; } set { } }

        public override void WriteTo(XmlWriter writer) { }
    }

    public abstract partial class XContainer : XNode
    {
        internal XContainer() { }

        public XNode FirstNode { get { throw null; } }

        public XNode LastNode { get { throw null; } }

        public void Add(object content) { }

        public void Add(params object[] content) { }

        public void AddFirst(object content) { }

        public void AddFirst(params object[] content) { }

        public XmlWriter CreateWriter() { throw null; }

        public Collections.Generic.IEnumerable<XNode> DescendantNodes() { throw null; }

        public Collections.Generic.IEnumerable<XElement> Descendants() { throw null; }

        public Collections.Generic.IEnumerable<XElement> Descendants(XName name) { throw null; }

        public XElement Element(XName name) { throw null; }

        public Collections.Generic.IEnumerable<XElement> Elements() { throw null; }

        public Collections.Generic.IEnumerable<XElement> Elements(XName name) { throw null; }

        public Collections.Generic.IEnumerable<XNode> Nodes() { throw null; }

        public void RemoveNodes() { }

        public void ReplaceNodes(object content) { }

        public void ReplaceNodes(params object[] content) { }
    }

    public partial class XDeclaration
    {
        public XDeclaration(string version, string encoding, string standalone) { }

        public XDeclaration(XDeclaration other) { }

        public string Encoding { get { throw null; } set { } }

        public string Standalone { get { throw null; } set { } }

        public string Version { get { throw null; } set { } }

        public override string ToString() { throw null; }
    }

    public partial class XDocument : XContainer
    {
        public XDocument() { }

        public XDocument(params object[] content) { }

        public XDocument(XDeclaration declaration, params object[] content) { }

        public XDocument(XDocument other) { }

        public XDeclaration Declaration { get { throw null; } set { } }

        public XDocumentType DocumentType { get { throw null; } }

        public override XmlNodeType NodeType { get { throw null; } }

        public XElement Root { get { throw null; } }

        public static XDocument Load(IO.Stream stream, LoadOptions options) { throw null; }

        public static XDocument Load(IO.Stream stream) { throw null; }

        public static XDocument Load(IO.TextReader textReader, LoadOptions options) { throw null; }

        public static XDocument Load(IO.TextReader textReader) { throw null; }

        public static XDocument Load(string uri, LoadOptions options) { throw null; }

        public static XDocument Load(string uri) { throw null; }

        public static XDocument Load(XmlReader reader, LoadOptions options) { throw null; }

        public static XDocument Load(XmlReader reader) { throw null; }

        public static XDocument Parse(string text, LoadOptions options) { throw null; }

        public static XDocument Parse(string text) { throw null; }

        public void Save(IO.Stream stream, SaveOptions options) { }

        public void Save(IO.Stream stream) { }

        public void Save(IO.TextWriter textWriter, SaveOptions options) { }

        public void Save(IO.TextWriter textWriter) { }

        public void Save(XmlWriter writer) { }

        public override void WriteTo(XmlWriter writer) { }
    }

    public partial class XDocumentType : XNode
    {
        public XDocumentType(string name, string publicId, string systemId, string internalSubset) { }

        public XDocumentType(XDocumentType other) { }

        public string InternalSubset { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public override XmlNodeType NodeType { get { throw null; } }

        public string PublicId { get { throw null; } set { } }

        public string SystemId { get { throw null; } set { } }

        public override void WriteTo(XmlWriter writer) { }
    }

    public partial class XElement : XContainer, Serialization.IXmlSerializable
    {
        public XElement(XElement other) { }

        public XElement(XName name, object content) { }

        public XElement(XName name, params object[] content) { }

        public XElement(XName name) { }

        public XElement(XStreamingElement other) { }

        public static Collections.Generic.IEnumerable<XElement> EmptySequence { get { throw null; } }

        public XAttribute FirstAttribute { get { throw null; } }

        public bool HasAttributes { get { throw null; } }

        public bool HasElements { get { throw null; } }

        public bool IsEmpty { get { throw null; } }

        public XAttribute LastAttribute { get { throw null; } }

        public XName Name { get { throw null; } set { } }

        public override XmlNodeType NodeType { get { throw null; } }

        public string Value { get { throw null; } set { } }

        public Collections.Generic.IEnumerable<XElement> AncestorsAndSelf() { throw null; }

        public Collections.Generic.IEnumerable<XElement> AncestorsAndSelf(XName name) { throw null; }

        public XAttribute Attribute(XName name) { throw null; }

        public Collections.Generic.IEnumerable<XAttribute> Attributes() { throw null; }

        public Collections.Generic.IEnumerable<XAttribute> Attributes(XName name) { throw null; }

        public Collections.Generic.IEnumerable<XNode> DescendantNodesAndSelf() { throw null; }

        public Collections.Generic.IEnumerable<XElement> DescendantsAndSelf() { throw null; }

        public Collections.Generic.IEnumerable<XElement> DescendantsAndSelf(XName name) { throw null; }

        public XNamespace GetDefaultNamespace() { throw null; }

        public XNamespace GetNamespaceOfPrefix(string prefix) { throw null; }

        public string GetPrefixOfNamespace(XNamespace ns) { throw null; }

        public static XElement Load(IO.Stream stream, LoadOptions options) { throw null; }

        public static XElement Load(IO.Stream stream) { throw null; }

        public static XElement Load(IO.TextReader textReader, LoadOptions options) { throw null; }

        public static XElement Load(IO.TextReader textReader) { throw null; }

        public static XElement Load(string uri, LoadOptions options) { throw null; }

        public static XElement Load(string uri) { throw null; }

        public static XElement Load(XmlReader reader, LoadOptions options) { throw null; }

        public static XElement Load(XmlReader reader) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator bool(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator DateTime(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator DateTimeOffset(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator decimal(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator double(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator Guid(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator int(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator long(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator bool?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator DateTime?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator DateTimeOffset?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator decimal?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator double?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator Guid?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator int?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator long?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator float?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator TimeSpan?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator uint?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator ulong?(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator float(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator string(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator TimeSpan(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator uint(XElement element) { throw null; }

        [CLSCompliant(false)]
        public static explicit operator ulong(XElement element) { throw null; }

        public static XElement Parse(string text, LoadOptions options) { throw null; }

        public static XElement Parse(string text) { throw null; }

        public void RemoveAll() { }

        public void RemoveAttributes() { }

        public void ReplaceAll(object content) { }

        public void ReplaceAll(params object[] content) { }

        public void ReplaceAttributes(object content) { }

        public void ReplaceAttributes(params object[] content) { }

        public void Save(IO.Stream stream, SaveOptions options) { }

        public void Save(IO.Stream stream) { }

        public void Save(IO.TextWriter textWriter, SaveOptions options) { }

        public void Save(IO.TextWriter textWriter) { }

        public void Save(XmlWriter writer) { }

        public void SetAttributeValue(XName name, object value) { }

        public void SetElementValue(XName name, object value) { }

        public void SetValue(object value) { }

        Schema.XmlSchema Serialization.IXmlSerializable.GetSchema() { throw null; }

        void Serialization.IXmlSerializable.ReadXml(XmlReader reader) { }

        void Serialization.IXmlSerializable.WriteXml(XmlWriter writer) { }

        public override void WriteTo(XmlWriter writer) { }
    }

    public sealed partial class XName : IEquatable<XName>
    {
        internal XName() { }

        public string LocalName { get { throw null; } }

        public XNamespace Namespace { get { throw null; } }

        public string NamespaceName { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public static XName Get(string localName, string namespaceName) { throw null; }

        public static XName Get(string expandedName) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(XName left, XName right) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator XName(string expandedName) { throw null; }

        public static bool operator !=(XName left, XName right) { throw null; }

        bool IEquatable<XName>.Equals(XName other) { throw null; }

        public override string ToString() { throw null; }
    }

    public sealed partial class XNamespace
    {
        internal XNamespace() { }

        public string NamespaceName { get { throw null; } }

        public static XNamespace None { get { throw null; } }

        public static XNamespace Xml { get { throw null; } }

        public static XNamespace Xmlns { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public static XNamespace Get(string namespaceName) { throw null; }

        public override int GetHashCode() { throw null; }

        public XName GetName(string localName) { throw null; }

        public static XName operator +(XNamespace ns, string localName) { throw null; }

        public static bool operator ==(XNamespace left, XNamespace right) { throw null; }

        [CLSCompliant(false)]
        public static implicit operator XNamespace(string namespaceName) { throw null; }

        public static bool operator !=(XNamespace left, XNamespace right) { throw null; }

        public override string ToString() { throw null; }
    }

    public abstract partial class XNode : XObject
    {
        internal XNode() { }

        public static XNodeDocumentOrderComparer DocumentOrderComparer { get { throw null; } }

        public static XNodeEqualityComparer EqualityComparer { get { throw null; } }

        public XNode NextNode { get { throw null; } }

        public XNode PreviousNode { get { throw null; } }

        public void AddAfterSelf(object content) { }

        public void AddAfterSelf(params object[] content) { }

        public void AddBeforeSelf(object content) { }

        public void AddBeforeSelf(params object[] content) { }

        public Collections.Generic.IEnumerable<XElement> Ancestors() { throw null; }

        public Collections.Generic.IEnumerable<XElement> Ancestors(XName name) { throw null; }

        public static int CompareDocumentOrder(XNode n1, XNode n2) { throw null; }

        public XmlReader CreateReader() { throw null; }

        public XmlReader CreateReader(ReaderOptions readerOptions) { throw null; }

        public static bool DeepEquals(XNode n1, XNode n2) { throw null; }

        public Collections.Generic.IEnumerable<XElement> ElementsAfterSelf() { throw null; }

        public Collections.Generic.IEnumerable<XElement> ElementsAfterSelf(XName name) { throw null; }

        public Collections.Generic.IEnumerable<XElement> ElementsBeforeSelf() { throw null; }

        public Collections.Generic.IEnumerable<XElement> ElementsBeforeSelf(XName name) { throw null; }

        public bool IsAfter(XNode node) { throw null; }

        public bool IsBefore(XNode node) { throw null; }

        public Collections.Generic.IEnumerable<XNode> NodesAfterSelf() { throw null; }

        public Collections.Generic.IEnumerable<XNode> NodesBeforeSelf() { throw null; }

        public static XNode ReadFrom(XmlReader reader) { throw null; }

        public void Remove() { }

        public void ReplaceWith(object content) { }

        public void ReplaceWith(params object[] content) { }

        public override string ToString() { throw null; }

        public string ToString(SaveOptions options) { throw null; }

        public abstract void WriteTo(XmlWriter writer);
    }

    public sealed partial class XNodeDocumentOrderComparer : Collections.Generic.IComparer<XNode>, Collections.IComparer
    {
        public int Compare(XNode x, XNode y) { throw null; }

        int Collections.IComparer.Compare(object x, object y) { throw null; }
    }

    public sealed partial class XNodeEqualityComparer : Collections.Generic.IEqualityComparer<XNode>, Collections.IEqualityComparer
    {
        public bool Equals(XNode x, XNode y) { throw null; }

        public int GetHashCode(XNode obj) { throw null; }

        bool Collections.IEqualityComparer.Equals(object x, object y) { throw null; }

        int Collections.IEqualityComparer.GetHashCode(object obj) { throw null; }
    }

    public abstract partial class XObject : IXmlLineInfo
    {
        internal XObject() { }

        public string BaseUri { get { throw null; } }

        public XDocument Document { get { throw null; } }

        public abstract XmlNodeType NodeType { get; }

        public XElement Parent { get { throw null; } }

        int IXmlLineInfo.LineNumber { get { throw null; } }

        int IXmlLineInfo.LinePosition { get { throw null; } }

        public event EventHandler<XObjectChangeEventArgs> Changed { add { } remove { } }

        public event EventHandler<XObjectChangeEventArgs> Changing { add { } remove { } }

        public void AddAnnotation(object annotation) { }

        public object Annotation(Type type) { throw null; }

        public T Annotation<T>()
            where T : class { throw null; }

        public Collections.Generic.IEnumerable<object> Annotations(Type type) { throw null; }

        public Collections.Generic.IEnumerable<T> Annotations<T>()
            where T : class { throw null; }

        public void RemoveAnnotations(Type type) { }

        public void RemoveAnnotations<T>()
            where T : class { }

        bool IXmlLineInfo.HasLineInfo() { throw null; }
    }

    public enum XObjectChange
    {
        Add = 0,
        Remove = 1,
        Name = 2,
        Value = 3
    }

    public partial class XObjectChangeEventArgs : EventArgs
    {
        public static readonly XObjectChangeEventArgs Add;
        public static readonly XObjectChangeEventArgs Name;
        public static readonly XObjectChangeEventArgs Remove;
        public static readonly XObjectChangeEventArgs Value;
        public XObjectChangeEventArgs(XObjectChange objectChange) { }

        public XObjectChange ObjectChange { get { throw null; } }
    }

    public partial class XProcessingInstruction : XNode
    {
        public XProcessingInstruction(string target, string data) { }

        public XProcessingInstruction(XProcessingInstruction other) { }

        public string Data { get { throw null; } set { } }

        public override XmlNodeType NodeType { get { throw null; } }

        public string Target { get { throw null; } set { } }

        public override void WriteTo(XmlWriter writer) { }
    }

    public partial class XStreamingElement
    {
        public XStreamingElement(XName name, object content) { }

        public XStreamingElement(XName name, params object[] content) { }

        public XStreamingElement(XName name) { }

        public XName Name { get { throw null; } set { } }

        public void Add(object content) { }

        public void Add(params object[] content) { }

        public void Save(IO.Stream stream, SaveOptions options) { }

        public void Save(IO.Stream stream) { }

        public void Save(IO.TextWriter textWriter, SaveOptions options) { }

        public void Save(IO.TextWriter textWriter) { }

        public void Save(XmlWriter writer) { }

        public override string ToString() { throw null; }

        public string ToString(SaveOptions options) { throw null; }

        public void WriteTo(XmlWriter writer) { }
    }

    public partial class XText : XNode
    {
        public XText(string value) { }

        public XText(XText other) { }

        public override XmlNodeType NodeType { get { throw null; } }

        public string Value { get { throw null; } set { } }

        public override void WriteTo(XmlWriter writer) { }
    }
}