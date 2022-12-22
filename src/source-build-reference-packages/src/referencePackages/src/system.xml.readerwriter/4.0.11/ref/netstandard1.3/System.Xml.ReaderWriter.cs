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
[assembly: AssemblyTitle("System.Xml.ReaderWriter")]
[assembly: AssemblyDescription("System.Xml.ReaderWriter")]
[assembly: AssemblyDefaultAlias("System.Xml.ReaderWriter")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.10.0")]




namespace System.Xml
{
    public enum ConformanceLevel
    {
        Auto = 0,
        Document = 2,
        Fragment = 1,
    }
    public enum DtdProcessing
    {
        Ignore = 1,
        Prohibit = 0,
    }
    public partial interface IXmlLineInfo
    {
        int LineNumber { get; }
        int LinePosition { get; }
        bool HasLineInfo();
    }
    public partial interface IXmlNamespaceResolver
    {
        System.Collections.Generic.IDictionary<string, string> GetNamespacesInScope(System.Xml.XmlNamespaceScope scope);
        string LookupNamespace(string prefix);
        string LookupPrefix(string namespaceName);
    }
    [System.FlagsAttribute]
    public enum NamespaceHandling
    {
        Default = 0,
        OmitDuplicates = 1,
    }
    public partial class NameTable : System.Xml.XmlNameTable
    {
        public NameTable() { }
        public override string Add(char[] key, int start, int len) { throw null; }
        public override string Add(string key) { throw null; }
        public override string Get(char[] key, int start, int len) { throw null; }
        public override string Get(string value) { throw null; }
    }
    public enum NewLineHandling
    {
        Entitize = 1,
        None = 2,
        Replace = 0,
    }
    public enum ReadState
    {
        Closed = 4,
        EndOfFile = 3,
        Error = 2,
        Initial = 0,
        Interactive = 1,
    }
    public enum WriteState
    {
        Attribute = 3,
        Closed = 5,
        Content = 4,
        Element = 2,
        Error = 6,
        Prolog = 1,
        Start = 0,
    }
    public static partial class XmlConvert
    {
        public static string DecodeName(string name) { throw null; }
        public static string EncodeLocalName(string name) { throw null; }
        public static string EncodeName(string name) { throw null; }
        public static string EncodeNmToken(string name) { throw null; }
        public static bool ToBoolean(string s) { throw null; }
        public static byte ToByte(string s) { throw null; }
        public static char ToChar(string s) { throw null; }
        public static System.DateTime ToDateTime(string s, System.Xml.XmlDateTimeSerializationMode dateTimeOption) { throw null; }
        public static System.DateTimeOffset ToDateTimeOffset(string s) { throw null; }
        public static System.DateTimeOffset ToDateTimeOffset(string s, string format) { throw null; }
        public static System.DateTimeOffset ToDateTimeOffset(string s, string[] formats) { throw null; }
        public static decimal ToDecimal(string s) { throw null; }
        public static double ToDouble(string s) { throw null; }
        public static System.Guid ToGuid(string s) { throw null; }
        public static short ToInt16(string s) { throw null; }
        public static int ToInt32(string s) { throw null; }
        public static long ToInt64(string s) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static sbyte ToSByte(string s) { throw null; }
        public static float ToSingle(string s) { throw null; }
        public static string ToString(bool value) { throw null; }
        public static string ToString(byte value) { throw null; }
        public static string ToString(char value) { throw null; }
        public static string ToString(System.DateTime value, System.Xml.XmlDateTimeSerializationMode dateTimeOption) { throw null; }
        public static string ToString(System.DateTimeOffset value) { throw null; }
        public static string ToString(System.DateTimeOffset value, string format) { throw null; }
        public static string ToString(decimal value) { throw null; }
        public static string ToString(double value) { throw null; }
        public static string ToString(System.Guid value) { throw null; }
        public static string ToString(short value) { throw null; }
        public static string ToString(int value) { throw null; }
        public static string ToString(long value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static string ToString(sbyte value) { throw null; }
        public static string ToString(float value) { throw null; }
        public static string ToString(System.TimeSpan value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static string ToString(ushort value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static string ToString(uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static string ToString(ulong value) { throw null; }
        public static System.TimeSpan ToTimeSpan(string s) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ushort ToUInt16(string s) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static uint ToUInt32(string s) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ulong ToUInt64(string s) { throw null; }
        public static string VerifyName(string name) { throw null; }
        public static string VerifyNCName(string name) { throw null; }
        public static string VerifyNMTOKEN(string name) { throw null; }
        public static string VerifyPublicId(string publicId) { throw null; }
        public static string VerifyWhitespace(string content) { throw null; }
        public static string VerifyXmlChars(string content) { throw null; }
    }
    public enum XmlDateTimeSerializationMode
    {
        Local = 0,
        RoundtripKind = 3,
        Unspecified = 2,
        Utc = 1,
    }
    public partial class XmlException : System.Exception
    {
        public XmlException() { }
        public XmlException(string message) { }
        public XmlException(string message, System.Exception innerException) { }
        public XmlException(string message, System.Exception innerException, int lineNumber, int linePosition) { }
        public int LineNumber { get { throw null; } }
        public int LinePosition { get { throw null; } }
        public override string Message { get { throw null; } }
    }
    public partial class XmlNamespaceManager : System.Collections.IEnumerable, System.Xml.IXmlNamespaceResolver
    {
        public XmlNamespaceManager(System.Xml.XmlNameTable nameTable) { }
        public virtual string DefaultNamespace { get { throw null; } }
        public virtual System.Xml.XmlNameTable NameTable { get { throw null; } }
        public virtual void AddNamespace(string prefix, string uri) { }
        public virtual System.Collections.IEnumerator GetEnumerator() { throw null; }
        public virtual System.Collections.Generic.IDictionary<string, string> GetNamespacesInScope(System.Xml.XmlNamespaceScope scope) { throw null; }
        public virtual bool HasNamespace(string prefix) { throw null; }
        public virtual string LookupNamespace(string prefix) { throw null; }
        public virtual string LookupPrefix(string uri) { throw null; }
        public virtual bool PopScope() { throw null; }
        public virtual void PushScope() { }
        public virtual void RemoveNamespace(string prefix, string uri) { }
    }
    public enum XmlNamespaceScope
    {
        All = 0,
        ExcludeXml = 1,
        Local = 2,
    }
    public abstract partial class XmlNameTable
    {
        protected XmlNameTable() { }
        public abstract string Add(char[] array, int offset, int length);
        public abstract string Add(string array);
        public abstract string Get(char[] array, int offset, int length);
        public abstract string Get(string array);
    }
    public enum XmlNodeType
    {
        Attribute = 2,
        CDATA = 4,
        Comment = 8,
        Document = 9,
        DocumentFragment = 11,
        DocumentType = 10,
        Element = 1,
        EndElement = 15,
        EndEntity = 16,
        Entity = 6,
        EntityReference = 5,
        None = 0,
        Notation = 12,
        ProcessingInstruction = 7,
        SignificantWhitespace = 14,
        Text = 3,
        Whitespace = 13,
        XmlDeclaration = 17,
    }
    public partial class XmlParserContext
    {
        public XmlParserContext(System.Xml.XmlNameTable nt, System.Xml.XmlNamespaceManager nsMgr, string docTypeName, string pubId, string sysId, string internalSubset, string baseURI, string xmlLang, System.Xml.XmlSpace xmlSpace) { }
        public XmlParserContext(System.Xml.XmlNameTable nt, System.Xml.XmlNamespaceManager nsMgr, string docTypeName, string pubId, string sysId, string internalSubset, string baseURI, string xmlLang, System.Xml.XmlSpace xmlSpace, System.Text.Encoding enc) { }
        public XmlParserContext(System.Xml.XmlNameTable nt, System.Xml.XmlNamespaceManager nsMgr, string xmlLang, System.Xml.XmlSpace xmlSpace) { }
        public XmlParserContext(System.Xml.XmlNameTable nt, System.Xml.XmlNamespaceManager nsMgr, string xmlLang, System.Xml.XmlSpace xmlSpace, System.Text.Encoding enc) { }
        public string BaseURI { get { throw null; } set { } }
        public string DocTypeName { get { throw null; } set { } }
        public System.Text.Encoding Encoding { get { throw null; } set { } }
        public string InternalSubset { get { throw null; } set { } }
        public System.Xml.XmlNamespaceManager NamespaceManager { get { throw null; } set { } }
        public System.Xml.XmlNameTable NameTable { get { throw null; } set { } }
        public string PublicId { get { throw null; } set { } }
        public string SystemId { get { throw null; } set { } }
        public string XmlLang { get { throw null; } set { } }
        public System.Xml.XmlSpace XmlSpace { get { throw null; } set { } }
    }
    public partial class XmlQualifiedName
    {
        public static readonly System.Xml.XmlQualifiedName Empty;
        public XmlQualifiedName() { }
        public XmlQualifiedName(string name) { }
        public XmlQualifiedName(string name, string ns) { }
        public bool IsEmpty { get { throw null; } }
        public string Name { get { throw null; } }
        public string Namespace { get { throw null; } }
        public override bool Equals(object other) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Xml.XmlQualifiedName a, System.Xml.XmlQualifiedName b) { throw null; }
        public static bool operator !=(System.Xml.XmlQualifiedName a, System.Xml.XmlQualifiedName b) { throw null; }
        public override string ToString() { throw null; }
        public static string ToString(string name, string ns) { throw null; }
    }
    public abstract partial class XmlReader : System.IDisposable
    {
        protected XmlReader() { }
        public abstract int AttributeCount { get; }
        public abstract string BaseURI { get; }
        public virtual bool CanReadBinaryContent { get { throw null; } }
        public virtual bool CanReadValueChunk { get { throw null; } }
        public virtual bool CanResolveEntity { get { throw null; } }
        public abstract int Depth { get; }
        public abstract bool EOF { get; }
        public virtual bool HasAttributes { get { throw null; } }
        public virtual bool HasValue { get { throw null; } }
        public virtual bool IsDefault { get { throw null; } }
        public abstract bool IsEmptyElement { get; }
        public virtual string this[int i] { get { throw null; } }
        public virtual string this[string name] { get { throw null; } }
        public virtual string this[string name, string namespaceURI] { get { throw null; } }
        public abstract string LocalName { get; }
        public virtual string Name { get { throw null; } }
        public abstract string NamespaceURI { get; }
        public abstract System.Xml.XmlNameTable NameTable { get; }
        public abstract System.Xml.XmlNodeType NodeType { get; }
        public abstract string Prefix { get; }
        public abstract System.Xml.ReadState ReadState { get; }
        public virtual System.Xml.XmlReaderSettings Settings { get { throw null; } }
        public abstract string Value { get; }
        public virtual System.Type ValueType { get { throw null; } }
        public virtual string XmlLang { get { throw null; } }
        public virtual System.Xml.XmlSpace XmlSpace { get { throw null; } }
        public static System.Xml.XmlReader Create(System.IO.Stream input) { throw null; }
        public static System.Xml.XmlReader Create(System.IO.Stream input, System.Xml.XmlReaderSettings settings) { throw null; }
        public static System.Xml.XmlReader Create(System.IO.Stream input, System.Xml.XmlReaderSettings settings, System.Xml.XmlParserContext inputContext) { throw null; }
        public static System.Xml.XmlReader Create(System.IO.TextReader input) { throw null; }
        public static System.Xml.XmlReader Create(System.IO.TextReader input, System.Xml.XmlReaderSettings settings) { throw null; }
        public static System.Xml.XmlReader Create(System.IO.TextReader input, System.Xml.XmlReaderSettings settings, System.Xml.XmlParserContext inputContext) { throw null; }
        public static System.Xml.XmlReader Create(string inputUri) { throw null; }
        public static System.Xml.XmlReader Create(string inputUri, System.Xml.XmlReaderSettings settings) { throw null; }
        public static System.Xml.XmlReader Create(System.Xml.XmlReader reader, System.Xml.XmlReaderSettings settings) { throw null; }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public abstract string GetAttribute(int i);
        public abstract string GetAttribute(string name);
        public abstract string GetAttribute(string name, string namespaceURI);
        public virtual System.Threading.Tasks.Task<string> GetValueAsync() { throw null; }
        public static bool IsName(string str) { throw null; }
        public static bool IsNameToken(string str) { throw null; }
        public virtual bool IsStartElement() { throw null; }
        public virtual bool IsStartElement(string name) { throw null; }
        public virtual bool IsStartElement(string localname, string ns) { throw null; }
        public abstract string LookupNamespace(string prefix);
        public virtual void MoveToAttribute(int i) { }
        public abstract bool MoveToAttribute(string name);
        public abstract bool MoveToAttribute(string name, string ns);
        public virtual System.Xml.XmlNodeType MoveToContent() { throw null; }
        public virtual System.Threading.Tasks.Task<System.Xml.XmlNodeType> MoveToContentAsync() { throw null; }
        public abstract bool MoveToElement();
        public abstract bool MoveToFirstAttribute();
        public abstract bool MoveToNextAttribute();
        public abstract bool Read();
        public virtual System.Threading.Tasks.Task<bool> ReadAsync() { throw null; }
        public abstract bool ReadAttributeValue();
        public virtual object ReadContentAs(System.Type returnType, System.Xml.IXmlNamespaceResolver namespaceResolver) { throw null; }
        public virtual System.Threading.Tasks.Task<object> ReadContentAsAsync(System.Type returnType, System.Xml.IXmlNamespaceResolver namespaceResolver) { throw null; }
        public virtual int ReadContentAsBase64(byte[] buffer, int index, int count) { throw null; }
        public virtual System.Threading.Tasks.Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count) { throw null; }
        public virtual int ReadContentAsBinHex(byte[] buffer, int index, int count) { throw null; }
        public virtual System.Threading.Tasks.Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count) { throw null; }
        public virtual bool ReadContentAsBoolean() { throw null; }
        public virtual System.DateTimeOffset ReadContentAsDateTimeOffset() { throw null; }
        public virtual decimal ReadContentAsDecimal() { throw null; }
        public virtual double ReadContentAsDouble() { throw null; }
        public virtual float ReadContentAsFloat() { throw null; }
        public virtual int ReadContentAsInt() { throw null; }
        public virtual long ReadContentAsLong() { throw null; }
        public virtual object ReadContentAsObject() { throw null; }
        public virtual System.Threading.Tasks.Task<object> ReadContentAsObjectAsync() { throw null; }
        public virtual string ReadContentAsString() { throw null; }
        public virtual System.Threading.Tasks.Task<string> ReadContentAsStringAsync() { throw null; }
        public virtual object ReadElementContentAs(System.Type returnType, System.Xml.IXmlNamespaceResolver namespaceResolver) { throw null; }
        public virtual object ReadElementContentAs(System.Type returnType, System.Xml.IXmlNamespaceResolver namespaceResolver, string localName, string namespaceURI) { throw null; }
        public virtual System.Threading.Tasks.Task<object> ReadElementContentAsAsync(System.Type returnType, System.Xml.IXmlNamespaceResolver namespaceResolver) { throw null; }
        public virtual int ReadElementContentAsBase64(byte[] buffer, int index, int count) { throw null; }
        public virtual System.Threading.Tasks.Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count) { throw null; }
        public virtual int ReadElementContentAsBinHex(byte[] buffer, int index, int count) { throw null; }
        public virtual System.Threading.Tasks.Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count) { throw null; }
        public virtual bool ReadElementContentAsBoolean() { throw null; }
        public virtual bool ReadElementContentAsBoolean(string localName, string namespaceURI) { throw null; }
        public virtual decimal ReadElementContentAsDecimal() { throw null; }
        public virtual decimal ReadElementContentAsDecimal(string localName, string namespaceURI) { throw null; }
        public virtual double ReadElementContentAsDouble() { throw null; }
        public virtual double ReadElementContentAsDouble(string localName, string namespaceURI) { throw null; }
        public virtual float ReadElementContentAsFloat() { throw null; }
        public virtual float ReadElementContentAsFloat(string localName, string namespaceURI) { throw null; }
        public virtual int ReadElementContentAsInt() { throw null; }
        public virtual int ReadElementContentAsInt(string localName, string namespaceURI) { throw null; }
        public virtual long ReadElementContentAsLong() { throw null; }
        public virtual long ReadElementContentAsLong(string localName, string namespaceURI) { throw null; }
        public virtual object ReadElementContentAsObject() { throw null; }
        public virtual object ReadElementContentAsObject(string localName, string namespaceURI) { throw null; }
        public virtual System.Threading.Tasks.Task<object> ReadElementContentAsObjectAsync() { throw null; }
        public virtual string ReadElementContentAsString() { throw null; }
        public virtual string ReadElementContentAsString(string localName, string namespaceURI) { throw null; }
        public virtual System.Threading.Tasks.Task<string> ReadElementContentAsStringAsync() { throw null; }
        public virtual void ReadEndElement() { }
        public virtual string ReadInnerXml() { throw null; }
        public virtual System.Threading.Tasks.Task<string> ReadInnerXmlAsync() { throw null; }
        public virtual string ReadOuterXml() { throw null; }
        public virtual System.Threading.Tasks.Task<string> ReadOuterXmlAsync() { throw null; }
        public virtual void ReadStartElement() { }
        public virtual void ReadStartElement(string name) { }
        public virtual void ReadStartElement(string localname, string ns) { }
        public virtual System.Xml.XmlReader ReadSubtree() { throw null; }
        public virtual bool ReadToDescendant(string name) { throw null; }
        public virtual bool ReadToDescendant(string localName, string namespaceURI) { throw null; }
        public virtual bool ReadToFollowing(string name) { throw null; }
        public virtual bool ReadToFollowing(string localName, string namespaceURI) { throw null; }
        public virtual bool ReadToNextSibling(string name) { throw null; }
        public virtual bool ReadToNextSibling(string localName, string namespaceURI) { throw null; }
        public virtual int ReadValueChunk(char[] buffer, int index, int count) { throw null; }
        public virtual System.Threading.Tasks.Task<int> ReadValueChunkAsync(char[] buffer, int index, int count) { throw null; }
        public abstract void ResolveEntity();
        public virtual void Skip() { }
        public virtual System.Threading.Tasks.Task SkipAsync() { throw null; }
    }
    public sealed partial class XmlReaderSettings
    {
        public XmlReaderSettings() { }
        public bool Async { get { throw null; } set { } }
        public bool CheckCharacters { get { throw null; } set { } }
        public bool CloseInput { get { throw null; } set { } }
        public System.Xml.ConformanceLevel ConformanceLevel { get { throw null; } set { } }
        public System.Xml.DtdProcessing DtdProcessing { get { throw null; } set { } }
        public bool IgnoreComments { get { throw null; } set { } }
        public bool IgnoreProcessingInstructions { get { throw null; } set { } }
        public bool IgnoreWhitespace { get { throw null; } set { } }
        public int LineNumberOffset { get { throw null; } set { } }
        public int LinePositionOffset { get { throw null; } set { } }
        public long MaxCharactersFromEntities { get { throw null; } set { } }
        public long MaxCharactersInDocument { get { throw null; } set { } }
        public System.Xml.XmlNameTable NameTable { get { throw null; } set { } }
        public System.Xml.XmlReaderSettings Clone() { throw null; }
        public void Reset() { }
    }
    public enum XmlSpace
    {
        Default = 1,
        None = 0,
        Preserve = 2,
    }
    public abstract partial class XmlWriter : System.IDisposable
    {
        protected XmlWriter() { }
        public virtual System.Xml.XmlWriterSettings Settings { get { throw null; } }
        public abstract System.Xml.WriteState WriteState { get; }
        public virtual string XmlLang { get { throw null; } }
        public virtual System.Xml.XmlSpace XmlSpace { get { throw null; } }
        public static System.Xml.XmlWriter Create(System.IO.Stream output) { throw null; }
        public static System.Xml.XmlWriter Create(System.IO.Stream output, System.Xml.XmlWriterSettings settings) { throw null; }
        public static System.Xml.XmlWriter Create(System.IO.TextWriter output) { throw null; }
        public static System.Xml.XmlWriter Create(System.IO.TextWriter output, System.Xml.XmlWriterSettings settings) { throw null; }
        public static System.Xml.XmlWriter Create(System.Text.StringBuilder output) { throw null; }
        public static System.Xml.XmlWriter Create(System.Text.StringBuilder output, System.Xml.XmlWriterSettings settings) { throw null; }
        public static System.Xml.XmlWriter Create(System.Xml.XmlWriter output) { throw null; }
        public static System.Xml.XmlWriter Create(System.Xml.XmlWriter output, System.Xml.XmlWriterSettings settings) { throw null; }
        public void Dispose() { }
        protected virtual void Dispose(bool disposing) { }
        public abstract void Flush();
        public virtual System.Threading.Tasks.Task FlushAsync() { throw null; }
        public abstract string LookupPrefix(string ns);
        public virtual void WriteAttributes(System.Xml.XmlReader reader, bool defattr) { }
        public virtual System.Threading.Tasks.Task WriteAttributesAsync(System.Xml.XmlReader reader, bool defattr) { throw null; }
        public void WriteAttributeString(string localName, string value) { }
        public void WriteAttributeString(string localName, string ns, string value) { }
        public void WriteAttributeString(string prefix, string localName, string ns, string value) { }
        public System.Threading.Tasks.Task WriteAttributeStringAsync(string prefix, string localName, string ns, string value) { throw null; }
        public abstract void WriteBase64(byte[] buffer, int index, int count);
        public virtual System.Threading.Tasks.Task WriteBase64Async(byte[] buffer, int index, int count) { throw null; }
        public virtual void WriteBinHex(byte[] buffer, int index, int count) { }
        public virtual System.Threading.Tasks.Task WriteBinHexAsync(byte[] buffer, int index, int count) { throw null; }
        public abstract void WriteCData(string text);
        public virtual System.Threading.Tasks.Task WriteCDataAsync(string text) { throw null; }
        public abstract void WriteCharEntity(char ch);
        public virtual System.Threading.Tasks.Task WriteCharEntityAsync(char ch) { throw null; }
        public abstract void WriteChars(char[] buffer, int index, int count);
        public virtual System.Threading.Tasks.Task WriteCharsAsync(char[] buffer, int index, int count) { throw null; }
        public abstract void WriteComment(string text);
        public virtual System.Threading.Tasks.Task WriteCommentAsync(string text) { throw null; }
        public abstract void WriteDocType(string name, string pubid, string sysid, string subset);
        public virtual System.Threading.Tasks.Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset) { throw null; }
        public void WriteElementString(string localName, string value) { }
        public void WriteElementString(string localName, string ns, string value) { }
        public void WriteElementString(string prefix, string localName, string ns, string value) { }
        public System.Threading.Tasks.Task WriteElementStringAsync(string prefix, string localName, string ns, string value) { throw null; }
        public abstract void WriteEndAttribute();
        protected internal virtual System.Threading.Tasks.Task WriteEndAttributeAsync() { throw null; }
        public abstract void WriteEndDocument();
        public virtual System.Threading.Tasks.Task WriteEndDocumentAsync() { throw null; }
        public abstract void WriteEndElement();
        public virtual System.Threading.Tasks.Task WriteEndElementAsync() { throw null; }
        public abstract void WriteEntityRef(string name);
        public virtual System.Threading.Tasks.Task WriteEntityRefAsync(string name) { throw null; }
        public abstract void WriteFullEndElement();
        public virtual System.Threading.Tasks.Task WriteFullEndElementAsync() { throw null; }
        public virtual void WriteName(string name) { }
        public virtual System.Threading.Tasks.Task WriteNameAsync(string name) { throw null; }
        public virtual void WriteNmToken(string name) { }
        public virtual System.Threading.Tasks.Task WriteNmTokenAsync(string name) { throw null; }
        public virtual void WriteNode(System.Xml.XmlReader reader, bool defattr) { }
        public virtual System.Threading.Tasks.Task WriteNodeAsync(System.Xml.XmlReader reader, bool defattr) { throw null; }
        public abstract void WriteProcessingInstruction(string name, string text);
        public virtual System.Threading.Tasks.Task WriteProcessingInstructionAsync(string name, string text) { throw null; }
        public virtual void WriteQualifiedName(string localName, string ns) { }
        public virtual System.Threading.Tasks.Task WriteQualifiedNameAsync(string localName, string ns) { throw null; }
        public abstract void WriteRaw(char[] buffer, int index, int count);
        public abstract void WriteRaw(string data);
        public virtual System.Threading.Tasks.Task WriteRawAsync(char[] buffer, int index, int count) { throw null; }
        public virtual System.Threading.Tasks.Task WriteRawAsync(string data) { throw null; }
        public void WriteStartAttribute(string localName) { }
        public void WriteStartAttribute(string localName, string ns) { }
        public abstract void WriteStartAttribute(string prefix, string localName, string ns);
        protected internal virtual System.Threading.Tasks.Task WriteStartAttributeAsync(string prefix, string localName, string ns) { throw null; }
        public abstract void WriteStartDocument();
        public abstract void WriteStartDocument(bool standalone);
        public virtual System.Threading.Tasks.Task WriteStartDocumentAsync() { throw null; }
        public virtual System.Threading.Tasks.Task WriteStartDocumentAsync(bool standalone) { throw null; }
        public void WriteStartElement(string localName) { }
        public void WriteStartElement(string localName, string ns) { }
        public abstract void WriteStartElement(string prefix, string localName, string ns);
        public virtual System.Threading.Tasks.Task WriteStartElementAsync(string prefix, string localName, string ns) { throw null; }
        public abstract void WriteString(string text);
        public virtual System.Threading.Tasks.Task WriteStringAsync(string text) { throw null; }
        public abstract void WriteSurrogateCharEntity(char lowChar, char highChar);
        public virtual System.Threading.Tasks.Task WriteSurrogateCharEntityAsync(char lowChar, char highChar) { throw null; }
        public virtual void WriteValue(bool value) { }
        public virtual void WriteValue(System.DateTimeOffset value) { }
        public virtual void WriteValue(decimal value) { }
        public virtual void WriteValue(double value) { }
        public virtual void WriteValue(int value) { }
        public virtual void WriteValue(long value) { }
        public virtual void WriteValue(object value) { }
        public virtual void WriteValue(float value) { }
        public virtual void WriteValue(string value) { }
        public abstract void WriteWhitespace(string ws);
        public virtual System.Threading.Tasks.Task WriteWhitespaceAsync(string ws) { throw null; }
    }
    public sealed partial class XmlWriterSettings
    {
        public XmlWriterSettings() { }
        public bool Async { get { throw null; } set { } }
        public bool CheckCharacters { get { throw null; } set { } }
        public bool CloseOutput { get { throw null; } set { } }
        public System.Xml.ConformanceLevel ConformanceLevel { get { throw null; } set { } }
        public System.Text.Encoding Encoding { get { throw null; } set { } }
        public bool Indent { get { throw null; } set { } }
        public string IndentChars { get { throw null; } set { } }
        public System.Xml.NamespaceHandling NamespaceHandling { get { throw null; } set { } }
        public string NewLineChars { get { throw null; } set { } }
        public System.Xml.NewLineHandling NewLineHandling { get { throw null; } set { } }
        public bool NewLineOnAttributes { get { throw null; } set { } }
        public bool OmitXmlDeclaration { get { throw null; } set { } }
        public bool WriteEndDocumentOnClose { get { throw null; } set { } }
        public System.Xml.XmlWriterSettings Clone() { throw null; }
        public void Reset() { }
    }
}
namespace System.Xml.Schema
{
    public partial class XmlSchema
    {
        internal XmlSchema() { }
    }
    public enum XmlSchemaForm
    {
        None = 0,
        Qualified = 1,
        Unqualified = 2,
    }
}
namespace System.Xml.Serialization
{
    public partial interface IXmlSerializable
    {
        System.Xml.Schema.XmlSchema GetSchema();
        void ReadXml(System.Xml.XmlReader reader);
        void WriteXml(System.Xml.XmlWriter writer);
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Interface | System.AttributeTargets.Struct)]
    public sealed partial class XmlSchemaProviderAttribute : System.Attribute
    {
        public XmlSchemaProviderAttribute(string methodName) { }
        public bool IsAny { get { throw null; } set { } }
        public string MethodName { get { throw null; } }
    }
}
