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
[assembly: System.Reflection.AssemblyTitle("System.Xml.XPath")]
[assembly: System.Reflection.AssemblyDescription("System.Xml.XPath")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Xml.XPath")]
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
    public enum XmlNodeOrder
    {
        Before = 0,
        After = 1,
        Same = 2,
        Unknown = 3
    }
}

namespace System.Xml.XPath
{
    public partial interface IXPathNavigable
    {
        XPathNavigator CreateNavigator();
    }

    public enum XmlCaseOrder
    {
        None = 0,
        UpperFirst = 1,
        LowerFirst = 2
    }

    public enum XmlDataType
    {
        Text = 1,
        Number = 2
    }

    public enum XmlSortOrder
    {
        Ascending = 1,
        Descending = 2
    }

    public partial class XPathDocument : IXPathNavigable
    {
        public XPathDocument(IO.Stream stream) { }

        public XPathDocument(IO.TextReader textReader) { }

        public XPathDocument(string uri, XmlSpace space) { }

        public XPathDocument(string uri) { }

        public XPathDocument(XmlReader reader, XmlSpace space) { }

        public XPathDocument(XmlReader reader) { }

        public XPathNavigator CreateNavigator() { throw null; }
    }

    public partial class XPathException : Exception
    {
        public XPathException() { }

        public XPathException(string message, Exception innerException) { }

        public XPathException(string message) { }
    }

    public abstract partial class XPathExpression
    {
        public abstract string Expression { get; }
        public abstract XPathResultType ReturnType { get; }

        public abstract void AddSort(object expr, Collections.IComparer comparer);
        public abstract void AddSort(object expr, XmlSortOrder order, XmlCaseOrder caseOrder, string lang, XmlDataType dataType);
        public abstract XPathExpression Clone();
        public static XPathExpression Compile(string xpath, IXmlNamespaceResolver nsResolver) { throw null; }

        public static XPathExpression Compile(string xpath) { throw null; }

        public abstract void SetContext(IXmlNamespaceResolver nsResolver);
        public abstract void SetContext(XmlNamespaceManager nsManager);
    }

    public abstract partial class XPathItem
    {
        public abstract bool IsNode { get; }
        public abstract object TypedValue { get; }
        public abstract string Value { get; }
        public abstract bool ValueAsBoolean { get; }
        public abstract DateTime ValueAsDateTime { get; }
        public abstract double ValueAsDouble { get; }
        public abstract int ValueAsInt { get; }
        public abstract long ValueAsLong { get; }
        public abstract Type ValueType { get; }

        public abstract object ValueAs(Type returnType, IXmlNamespaceResolver nsResolver);
        public virtual object ValueAs(Type returnType) { throw null; }
    }

    public enum XPathNamespaceScope
    {
        All = 0,
        ExcludeXml = 1,
        Local = 2
    }

    public abstract partial class XPathNavigator : XPathItem, IXmlNamespaceResolver, IXPathNavigable
    {
        protected XPathNavigator() { }

        public abstract string BaseURI { get; }

        public virtual bool CanEdit { get { throw null; } }

        public virtual bool HasAttributes { get { throw null; } }

        public virtual bool HasChildren { get { throw null; } }

        public virtual string InnerXml { get { throw null; } set { } }

        public abstract bool IsEmptyElement { get; }

        public sealed override bool IsNode { get { throw null; } }

        public abstract string LocalName { get; }
        public abstract string Name { get; }
        public abstract string NamespaceURI { get; }
        public abstract XmlNameTable NameTable { get; }

        public static Collections.IEqualityComparer NavigatorComparer { get { throw null; } }

        public abstract XPathNodeType NodeType { get; }

        public virtual string OuterXml { get { throw null; } set { } }

        public abstract string Prefix { get; }

        public override object TypedValue { get { throw null; } }

        public virtual object UnderlyingObject { get { throw null; } }

        public override bool ValueAsBoolean { get { throw null; } }

        public override DateTime ValueAsDateTime { get { throw null; } }

        public override double ValueAsDouble { get { throw null; } }

        public override int ValueAsInt { get { throw null; } }

        public override long ValueAsLong { get { throw null; } }

        public override Type ValueType { get { throw null; } }

        public virtual string XmlLang { get { throw null; } }

        public virtual XmlWriter AppendChild() { throw null; }

        public virtual void AppendChild(string newChild) { }

        public virtual void AppendChild(XmlReader newChild) { }

        public virtual void AppendChild(XPathNavigator newChild) { }

        public virtual void AppendChildElement(string prefix, string localName, string namespaceURI, string value) { }

        public abstract XPathNavigator Clone();
        public virtual XmlNodeOrder ComparePosition(XPathNavigator nav) { throw null; }

        public virtual XPathExpression Compile(string xpath) { throw null; }

        public virtual void CreateAttribute(string prefix, string localName, string namespaceURI, string value) { }

        public virtual XmlWriter CreateAttributes() { throw null; }

        public virtual XPathNavigator CreateNavigator() { throw null; }

        public virtual void DeleteRange(XPathNavigator lastSiblingToDelete) { }

        public virtual void DeleteSelf() { }

        public virtual object Evaluate(string xpath, IXmlNamespaceResolver resolver) { throw null; }

        public virtual object Evaluate(string xpath) { throw null; }

        public virtual object Evaluate(XPathExpression expr, XPathNodeIterator context) { throw null; }

        public virtual object Evaluate(XPathExpression expr) { throw null; }

        public virtual string GetAttribute(string localName, string namespaceURI) { throw null; }

        public virtual string GetNamespace(string name) { throw null; }

        public virtual Collections.Generic.IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope) { throw null; }

        public virtual XmlWriter InsertAfter() { throw null; }

        public virtual void InsertAfter(string newSibling) { }

        public virtual void InsertAfter(XmlReader newSibling) { }

        public virtual void InsertAfter(XPathNavigator newSibling) { }

        public virtual XmlWriter InsertBefore() { throw null; }

        public virtual void InsertBefore(string newSibling) { }

        public virtual void InsertBefore(XmlReader newSibling) { }

        public virtual void InsertBefore(XPathNavigator newSibling) { }

        public virtual void InsertElementAfter(string prefix, string localName, string namespaceURI, string value) { }

        public virtual void InsertElementBefore(string prefix, string localName, string namespaceURI, string value) { }

        public virtual bool IsDescendant(XPathNavigator nav) { throw null; }

        public abstract bool IsSamePosition(XPathNavigator other);
        public virtual string LookupNamespace(string prefix) { throw null; }

        public virtual string LookupPrefix(string namespaceURI) { throw null; }

        public virtual bool Matches(string xpath) { throw null; }

        public virtual bool Matches(XPathExpression expr) { throw null; }

        public abstract bool MoveTo(XPathNavigator other);
        public virtual bool MoveToAttribute(string localName, string namespaceURI) { throw null; }

        public virtual bool MoveToChild(string localName, string namespaceURI) { throw null; }

        public virtual bool MoveToChild(XPathNodeType type) { throw null; }

        public virtual bool MoveToFirst() { throw null; }

        public abstract bool MoveToFirstAttribute();
        public abstract bool MoveToFirstChild();
        public bool MoveToFirstNamespace() { throw null; }

        public abstract bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope);
        public virtual bool MoveToFollowing(string localName, string namespaceURI, XPathNavigator end) { throw null; }

        public virtual bool MoveToFollowing(string localName, string namespaceURI) { throw null; }

        public virtual bool MoveToFollowing(XPathNodeType type, XPathNavigator end) { throw null; }

        public virtual bool MoveToFollowing(XPathNodeType type) { throw null; }

        public abstract bool MoveToId(string id);
        public virtual bool MoveToNamespace(string name) { throw null; }

        public abstract bool MoveToNext();
        public virtual bool MoveToNext(string localName, string namespaceURI) { throw null; }

        public virtual bool MoveToNext(XPathNodeType type) { throw null; }

        public abstract bool MoveToNextAttribute();
        public bool MoveToNextNamespace() { throw null; }

        public abstract bool MoveToNextNamespace(XPathNamespaceScope namespaceScope);
        public abstract bool MoveToParent();
        public abstract bool MoveToPrevious();
        public virtual void MoveToRoot() { }

        public virtual XmlWriter PrependChild() { throw null; }

        public virtual void PrependChild(string newChild) { }

        public virtual void PrependChild(XmlReader newChild) { }

        public virtual void PrependChild(XPathNavigator newChild) { }

        public virtual void PrependChildElement(string prefix, string localName, string namespaceURI, string value) { }

        public virtual XmlReader ReadSubtree() { throw null; }

        public virtual XmlWriter ReplaceRange(XPathNavigator lastSiblingToReplace) { throw null; }

        public virtual void ReplaceSelf(string newNode) { }

        public virtual void ReplaceSelf(XmlReader newNode) { }

        public virtual void ReplaceSelf(XPathNavigator newNode) { }

        public virtual XPathNodeIterator Select(string xpath, IXmlNamespaceResolver resolver) { throw null; }

        public virtual XPathNodeIterator Select(string xpath) { throw null; }

        public virtual XPathNodeIterator Select(XPathExpression expr) { throw null; }

        public virtual XPathNodeIterator SelectAncestors(string name, string namespaceURI, bool matchSelf) { throw null; }

        public virtual XPathNodeIterator SelectAncestors(XPathNodeType type, bool matchSelf) { throw null; }

        public virtual XPathNodeIterator SelectChildren(string name, string namespaceURI) { throw null; }

        public virtual XPathNodeIterator SelectChildren(XPathNodeType type) { throw null; }

        public virtual XPathNodeIterator SelectDescendants(string name, string namespaceURI, bool matchSelf) { throw null; }

        public virtual XPathNodeIterator SelectDescendants(XPathNodeType type, bool matchSelf) { throw null; }

        public virtual XPathNavigator SelectSingleNode(string xpath, IXmlNamespaceResolver resolver) { throw null; }

        public virtual XPathNavigator SelectSingleNode(string xpath) { throw null; }

        public virtual XPathNavigator SelectSingleNode(XPathExpression expression) { throw null; }

        public virtual void SetTypedValue(object typedValue) { }

        public virtual void SetValue(string value) { }

        public override string ToString() { throw null; }

        public override object ValueAs(Type returnType, IXmlNamespaceResolver nsResolver) { throw null; }

        public virtual void WriteSubtree(XmlWriter writer) { }
    }

    public abstract partial class XPathNodeIterator : Collections.IEnumerable
    {
        protected XPathNodeIterator() { }

        public virtual int Count { get { throw null; } }

        public abstract XPathNavigator Current { get; }
        public abstract int CurrentPosition { get; }

        public abstract XPathNodeIterator Clone();
        public virtual Collections.IEnumerator GetEnumerator() { throw null; }

        public abstract bool MoveNext();
    }

    public enum XPathNodeType
    {
        Root = 0,
        Element = 1,
        Attribute = 2,
        Namespace = 3,
        Text = 4,
        SignificantWhitespace = 5,
        Whitespace = 6,
        ProcessingInstruction = 7,
        Comment = 8,
        All = 9
    }

    public enum XPathResultType
    {
        Number = 0,
        Navigator = 1,
        String = 1,
        Boolean = 2,
        NodeSet = 3,
        Any = 5,
        Error = 6
    }
}