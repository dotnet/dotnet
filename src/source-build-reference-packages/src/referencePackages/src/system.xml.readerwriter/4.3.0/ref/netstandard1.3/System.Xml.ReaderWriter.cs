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
[assembly: System.Reflection.AssemblyTitle("System.Xml.ReaderWriter")]
[assembly: System.Reflection.AssemblyDescription("System.Xml.ReaderWriter")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Xml.ReaderWriter")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.24705.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.24705.01. Commit Hash: 4d1af962ca0fede10beb01d197367c2f90e92c97")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Xml
{
    public enum ConformanceLevel
    {
        Auto = 0,
        Fragment = 1,
        Document = 2
    }

    public enum DtdProcessing
    {
        Prohibit = 0,
        Ignore = 1
    }

    public partial interface IXmlLineInfo
    {
        int LineNumber { get; }

        int LinePosition { get; }

        bool HasLineInfo();
    }

    public partial interface IXmlNamespaceResolver
    {
        Collections.Generic.IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope);
        string LookupNamespace(string prefix);
        string LookupPrefix(string namespaceName);
    }

    [Flags]
    public enum NamespaceHandling
    {
        Default = 0,
        OmitDuplicates = 1
    }

    public partial class NameTable : XmlNameTable
    {
        public override string Add(char[] key, int start, int len) { throw null; }

        public override string Add(string key) { throw null; }

        public override string Get(char[] key, int start, int len) { throw null; }

        public override string Get(string value) { throw null; }
    }

    public enum NewLineHandling
    {
        Replace = 0,
        Entitize = 1,
        None = 2
    }

    public enum ReadState
    {
        Initial = 0,
        Interactive = 1,
        Error = 2,
        EndOfFile = 3,
        Closed = 4
    }

    public enum WriteState
    {
        Start = 0,
        Prolog = 1,
        Element = 2,
        Attribute = 3,
        Content = 4,
        Closed = 5,
        Error = 6
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

        public static DateTime ToDateTime(string s, XmlDateTimeSerializationMode dateTimeOption) { throw null; }

        public static DateTimeOffset ToDateTimeOffset(string s, string format) { throw null; }

        public static DateTimeOffset ToDateTimeOffset(string s, string[] formats) { throw null; }

        public static DateTimeOffset ToDateTimeOffset(string s) { throw null; }

        public static decimal ToDecimal(string s) { throw null; }

        public static double ToDouble(string s) { throw null; }

        public static Guid ToGuid(string s) { throw null; }

        public static short ToInt16(string s) { throw null; }

        public static int ToInt32(string s) { throw null; }

        public static long ToInt64(string s) { throw null; }

        [CLSCompliant(false)]
        public static sbyte ToSByte(string s) { throw null; }

        public static float ToSingle(string s) { throw null; }

        public static string ToString(bool value) { throw null; }

        public static string ToString(byte value) { throw null; }

        public static string ToString(char value) { throw null; }

        public static string ToString(DateTime value, XmlDateTimeSerializationMode dateTimeOption) { throw null; }

        public static string ToString(DateTimeOffset value, string format) { throw null; }

        public static string ToString(DateTimeOffset value) { throw null; }

        public static string ToString(decimal value) { throw null; }

        public static string ToString(double value) { throw null; }

        public static string ToString(Guid value) { throw null; }

        public static string ToString(short value) { throw null; }

        public static string ToString(int value) { throw null; }

        public static string ToString(long value) { throw null; }

        [CLSCompliant(false)]
        public static string ToString(sbyte value) { throw null; }

        public static string ToString(float value) { throw null; }

        public static string ToString(TimeSpan value) { throw null; }

        [CLSCompliant(false)]
        public static string ToString(ushort value) { throw null; }

        [CLSCompliant(false)]
        public static string ToString(uint value) { throw null; }

        [CLSCompliant(false)]
        public static string ToString(ulong value) { throw null; }

        public static TimeSpan ToTimeSpan(string s) { throw null; }

        [CLSCompliant(false)]
        public static ushort ToUInt16(string s) { throw null; }

        [CLSCompliant(false)]
        public static uint ToUInt32(string s) { throw null; }

        [CLSCompliant(false)]
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
        Utc = 1,
        Unspecified = 2,
        RoundtripKind = 3
    }

    public partial class XmlException : Exception
    {
        public XmlException() { }

        public XmlException(string message, Exception innerException, int lineNumber, int linePosition) { }

        public XmlException(string message, Exception innerException) { }

        public XmlException(string message) { }

        public int LineNumber { get { throw null; } }

        public int LinePosition { get { throw null; } }

        public override string Message { get { throw null; } }
    }

    public partial class XmlNamespaceManager : Collections.IEnumerable, IXmlNamespaceResolver
    {
        public XmlNamespaceManager(XmlNameTable nameTable) { }

        public virtual string DefaultNamespace { get { throw null; } }

        public virtual XmlNameTable NameTable { get { throw null; } }

        public virtual void AddNamespace(string prefix, string uri) { }

        public virtual Collections.IEnumerator GetEnumerator() { throw null; }

        public virtual Collections.Generic.IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope) { throw null; }

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
        Local = 2
    }

    public abstract partial class XmlNameTable
    {
        public abstract string Add(char[] array, int offset, int length);
        public abstract string Add(string array);
        public abstract string Get(char[] array, int offset, int length);
        public abstract string Get(string array);
    }

    public enum XmlNodeType
    {
        None = 0,
        Element = 1,
        Attribute = 2,
        Text = 3,
        CDATA = 4,
        EntityReference = 5,
        Entity = 6,
        ProcessingInstruction = 7,
        Comment = 8,
        Document = 9,
        DocumentType = 10,
        DocumentFragment = 11,
        Notation = 12,
        Whitespace = 13,
        SignificantWhitespace = 14,
        EndElement = 15,
        EndEntity = 16,
        XmlDeclaration = 17
    }

    public partial class XmlParserContext
    {
        public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string docTypeName, string pubId, string sysId, string internalSubset, string baseURI, string xmlLang, XmlSpace xmlSpace, Text.Encoding enc) { }

        public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string docTypeName, string pubId, string sysId, string internalSubset, string baseURI, string xmlLang, XmlSpace xmlSpace) { }

        public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string xmlLang, XmlSpace xmlSpace, Text.Encoding enc) { }

        public XmlParserContext(XmlNameTable nt, XmlNamespaceManager nsMgr, string xmlLang, XmlSpace xmlSpace) { }

        public string BaseURI { get { throw null; } set { } }

        public string DocTypeName { get { throw null; } set { } }

        public Text.Encoding Encoding { get { throw null; } set { } }

        public string InternalSubset { get { throw null; } set { } }

        public XmlNamespaceManager NamespaceManager { get { throw null; } set { } }

        public XmlNameTable NameTable { get { throw null; } set { } }

        public string PublicId { get { throw null; } set { } }

        public string SystemId { get { throw null; } set { } }

        public string XmlLang { get { throw null; } set { } }

        public XmlSpace XmlSpace { get { throw null; } set { } }
    }

    public partial class XmlQualifiedName
    {
        public static readonly XmlQualifiedName Empty;
        public XmlQualifiedName() { }

        public XmlQualifiedName(string name, string ns) { }

        public XmlQualifiedName(string name) { }

        public bool IsEmpty { get { throw null; } }

        public string Name { get { throw null; } }

        public string Namespace { get { throw null; } }

        public override bool Equals(object other) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(XmlQualifiedName a, XmlQualifiedName b) { throw null; }

        public static bool operator !=(XmlQualifiedName a, XmlQualifiedName b) { throw null; }

        public override string ToString() { throw null; }

        public static string ToString(string name, string ns) { throw null; }
    }

    public abstract partial class XmlReader : IDisposable
    {
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

        public virtual string this[string name, string namespaceURI] { get { throw null; } }

        public virtual string this[string name] { get { throw null; } }

        public abstract string LocalName { get; }

        public virtual string Name { get { throw null; } }

        public abstract string NamespaceURI { get; }
        public abstract XmlNameTable NameTable { get; }
        public abstract XmlNodeType NodeType { get; }
        public abstract string Prefix { get; }
        public abstract ReadState ReadState { get; }

        public virtual XmlReaderSettings Settings { get { throw null; } }

        public abstract string Value { get; }

        public virtual Type ValueType { get { throw null; } }

        public virtual string XmlLang { get { throw null; } }

        public virtual XmlSpace XmlSpace { get { throw null; } }

        public static XmlReader Create(IO.Stream input, XmlReaderSettings settings, XmlParserContext inputContext) { throw null; }

        public static XmlReader Create(IO.Stream input, XmlReaderSettings settings) { throw null; }

        public static XmlReader Create(IO.Stream input) { throw null; }

        public static XmlReader Create(IO.TextReader input, XmlReaderSettings settings, XmlParserContext inputContext) { throw null; }

        public static XmlReader Create(IO.TextReader input, XmlReaderSettings settings) { throw null; }

        public static XmlReader Create(IO.TextReader input) { throw null; }

        public static XmlReader Create(string inputUri, XmlReaderSettings settings) { throw null; }

        public static XmlReader Create(string inputUri) { throw null; }

        public static XmlReader Create(XmlReader reader, XmlReaderSettings settings) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public abstract string GetAttribute(int i);
        public abstract string GetAttribute(string name, string namespaceURI);
        public abstract string GetAttribute(string name);
        public virtual Threading.Tasks.Task<string> GetValueAsync() { throw null; }

        public static bool IsName(string str) { throw null; }

        public static bool IsNameToken(string str) { throw null; }

        public virtual bool IsStartElement() { throw null; }

        public virtual bool IsStartElement(string localname, string ns) { throw null; }

        public virtual bool IsStartElement(string name) { throw null; }

        public abstract string LookupNamespace(string prefix);
        public virtual void MoveToAttribute(int i) { }

        public abstract bool MoveToAttribute(string name, string ns);
        public abstract bool MoveToAttribute(string name);
        public virtual XmlNodeType MoveToContent() { throw null; }

        public virtual Threading.Tasks.Task<XmlNodeType> MoveToContentAsync() { throw null; }

        public abstract bool MoveToElement();
        public abstract bool MoveToFirstAttribute();
        public abstract bool MoveToNextAttribute();
        public abstract bool Read();
        public virtual Threading.Tasks.Task<bool> ReadAsync() { throw null; }

        public abstract bool ReadAttributeValue();
        public virtual object ReadContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver) { throw null; }

        public virtual Threading.Tasks.Task<object> ReadContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver) { throw null; }

        public virtual int ReadContentAsBase64(byte[] buffer, int index, int count) { throw null; }

        public virtual Threading.Tasks.Task<int> ReadContentAsBase64Async(byte[] buffer, int index, int count) { throw null; }

        public virtual int ReadContentAsBinHex(byte[] buffer, int index, int count) { throw null; }

        public virtual Threading.Tasks.Task<int> ReadContentAsBinHexAsync(byte[] buffer, int index, int count) { throw null; }

        public virtual bool ReadContentAsBoolean() { throw null; }

        public virtual DateTimeOffset ReadContentAsDateTimeOffset() { throw null; }

        public virtual decimal ReadContentAsDecimal() { throw null; }

        public virtual double ReadContentAsDouble() { throw null; }

        public virtual float ReadContentAsFloat() { throw null; }

        public virtual int ReadContentAsInt() { throw null; }

        public virtual long ReadContentAsLong() { throw null; }

        public virtual object ReadContentAsObject() { throw null; }

        public virtual Threading.Tasks.Task<object> ReadContentAsObjectAsync() { throw null; }

        public virtual string ReadContentAsString() { throw null; }

        public virtual Threading.Tasks.Task<string> ReadContentAsStringAsync() { throw null; }

        public virtual object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver, string localName, string namespaceURI) { throw null; }

        public virtual object ReadElementContentAs(Type returnType, IXmlNamespaceResolver namespaceResolver) { throw null; }

        public virtual Threading.Tasks.Task<object> ReadElementContentAsAsync(Type returnType, IXmlNamespaceResolver namespaceResolver) { throw null; }

        public virtual int ReadElementContentAsBase64(byte[] buffer, int index, int count) { throw null; }

        public virtual Threading.Tasks.Task<int> ReadElementContentAsBase64Async(byte[] buffer, int index, int count) { throw null; }

        public virtual int ReadElementContentAsBinHex(byte[] buffer, int index, int count) { throw null; }

        public virtual Threading.Tasks.Task<int> ReadElementContentAsBinHexAsync(byte[] buffer, int index, int count) { throw null; }

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

        public virtual Threading.Tasks.Task<object> ReadElementContentAsObjectAsync() { throw null; }

        public virtual string ReadElementContentAsString() { throw null; }

        public virtual string ReadElementContentAsString(string localName, string namespaceURI) { throw null; }

        public virtual Threading.Tasks.Task<string> ReadElementContentAsStringAsync() { throw null; }

        public virtual void ReadEndElement() { }

        public virtual string ReadInnerXml() { throw null; }

        public virtual Threading.Tasks.Task<string> ReadInnerXmlAsync() { throw null; }

        public virtual string ReadOuterXml() { throw null; }

        public virtual Threading.Tasks.Task<string> ReadOuterXmlAsync() { throw null; }

        public virtual void ReadStartElement() { }

        public virtual void ReadStartElement(string localname, string ns) { }

        public virtual void ReadStartElement(string name) { }

        public virtual XmlReader ReadSubtree() { throw null; }

        public virtual bool ReadToDescendant(string localName, string namespaceURI) { throw null; }

        public virtual bool ReadToDescendant(string name) { throw null; }

        public virtual bool ReadToFollowing(string localName, string namespaceURI) { throw null; }

        public virtual bool ReadToFollowing(string name) { throw null; }

        public virtual bool ReadToNextSibling(string localName, string namespaceURI) { throw null; }

        public virtual bool ReadToNextSibling(string name) { throw null; }

        public virtual int ReadValueChunk(char[] buffer, int index, int count) { throw null; }

        public virtual Threading.Tasks.Task<int> ReadValueChunkAsync(char[] buffer, int index, int count) { throw null; }

        public abstract void ResolveEntity();
        public virtual void Skip() { }

        public virtual Threading.Tasks.Task SkipAsync() { throw null; }
    }

    public sealed partial class XmlReaderSettings
    {
        public bool Async { get { throw null; } set { } }

        public bool CheckCharacters { get { throw null; } set { } }

        public bool CloseInput { get { throw null; } set { } }

        public ConformanceLevel ConformanceLevel { get { throw null; } set { } }

        public DtdProcessing DtdProcessing { get { throw null; } set { } }

        public bool IgnoreComments { get { throw null; } set { } }

        public bool IgnoreProcessingInstructions { get { throw null; } set { } }

        public bool IgnoreWhitespace { get { throw null; } set { } }

        public int LineNumberOffset { get { throw null; } set { } }

        public int LinePositionOffset { get { throw null; } set { } }

        public long MaxCharactersFromEntities { get { throw null; } set { } }

        public long MaxCharactersInDocument { get { throw null; } set { } }

        public XmlNameTable NameTable { get { throw null; } set { } }

        public XmlReaderSettings Clone() { throw null; }

        public void Reset() { }
    }

    public enum XmlSpace
    {
        None = 0,
        Default = 1,
        Preserve = 2
    }

    public abstract partial class XmlWriter : IDisposable
    {
        public virtual XmlWriterSettings Settings { get { throw null; } }

        public abstract WriteState WriteState { get; }

        public virtual string XmlLang { get { throw null; } }

        public virtual XmlSpace XmlSpace { get { throw null; } }

        public static XmlWriter Create(IO.Stream output, XmlWriterSettings settings) { throw null; }

        public static XmlWriter Create(IO.Stream output) { throw null; }

        public static XmlWriter Create(IO.TextWriter output, XmlWriterSettings settings) { throw null; }

        public static XmlWriter Create(IO.TextWriter output) { throw null; }

        public static XmlWriter Create(Text.StringBuilder output, XmlWriterSettings settings) { throw null; }

        public static XmlWriter Create(Text.StringBuilder output) { throw null; }

        public static XmlWriter Create(XmlWriter output, XmlWriterSettings settings) { throw null; }

        public static XmlWriter Create(XmlWriter output) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        public abstract void Flush();
        public virtual Threading.Tasks.Task FlushAsync() { throw null; }

        public abstract string LookupPrefix(string ns);
        public virtual void WriteAttributes(XmlReader reader, bool defattr) { }

        public virtual Threading.Tasks.Task WriteAttributesAsync(XmlReader reader, bool defattr) { throw null; }

        public void WriteAttributeString(string prefix, string localName, string ns, string value) { }

        public void WriteAttributeString(string localName, string ns, string value) { }

        public void WriteAttributeString(string localName, string value) { }

        public Threading.Tasks.Task WriteAttributeStringAsync(string prefix, string localName, string ns, string value) { throw null; }

        public abstract void WriteBase64(byte[] buffer, int index, int count);
        public virtual Threading.Tasks.Task WriteBase64Async(byte[] buffer, int index, int count) { throw null; }

        public virtual void WriteBinHex(byte[] buffer, int index, int count) { }

        public virtual Threading.Tasks.Task WriteBinHexAsync(byte[] buffer, int index, int count) { throw null; }

        public abstract void WriteCData(string text);
        public virtual Threading.Tasks.Task WriteCDataAsync(string text) { throw null; }

        public abstract void WriteCharEntity(char ch);
        public virtual Threading.Tasks.Task WriteCharEntityAsync(char ch) { throw null; }

        public abstract void WriteChars(char[] buffer, int index, int count);
        public virtual Threading.Tasks.Task WriteCharsAsync(char[] buffer, int index, int count) { throw null; }

        public abstract void WriteComment(string text);
        public virtual Threading.Tasks.Task WriteCommentAsync(string text) { throw null; }

        public abstract void WriteDocType(string name, string pubid, string sysid, string subset);
        public virtual Threading.Tasks.Task WriteDocTypeAsync(string name, string pubid, string sysid, string subset) { throw null; }

        public void WriteElementString(string prefix, string localName, string ns, string value) { }

        public void WriteElementString(string localName, string ns, string value) { }

        public void WriteElementString(string localName, string value) { }

        public Threading.Tasks.Task WriteElementStringAsync(string prefix, string localName, string ns, string value) { throw null; }

        public abstract void WriteEndAttribute();
        protected internal virtual Threading.Tasks.Task WriteEndAttributeAsync() { throw null; }

        public abstract void WriteEndDocument();
        public virtual Threading.Tasks.Task WriteEndDocumentAsync() { throw null; }

        public abstract void WriteEndElement();
        public virtual Threading.Tasks.Task WriteEndElementAsync() { throw null; }

        public abstract void WriteEntityRef(string name);
        public virtual Threading.Tasks.Task WriteEntityRefAsync(string name) { throw null; }

        public abstract void WriteFullEndElement();
        public virtual Threading.Tasks.Task WriteFullEndElementAsync() { throw null; }

        public virtual void WriteName(string name) { }

        public virtual Threading.Tasks.Task WriteNameAsync(string name) { throw null; }

        public virtual void WriteNmToken(string name) { }

        public virtual Threading.Tasks.Task WriteNmTokenAsync(string name) { throw null; }

        public virtual void WriteNode(XmlReader reader, bool defattr) { }

        public virtual Threading.Tasks.Task WriteNodeAsync(XmlReader reader, bool defattr) { throw null; }

        public abstract void WriteProcessingInstruction(string name, string text);
        public virtual Threading.Tasks.Task WriteProcessingInstructionAsync(string name, string text) { throw null; }

        public virtual void WriteQualifiedName(string localName, string ns) { }

        public virtual Threading.Tasks.Task WriteQualifiedNameAsync(string localName, string ns) { throw null; }

        public abstract void WriteRaw(char[] buffer, int index, int count);
        public abstract void WriteRaw(string data);
        public virtual Threading.Tasks.Task WriteRawAsync(char[] buffer, int index, int count) { throw null; }

        public virtual Threading.Tasks.Task WriteRawAsync(string data) { throw null; }

        public abstract void WriteStartAttribute(string prefix, string localName, string ns);
        public void WriteStartAttribute(string localName, string ns) { }

        public void WriteStartAttribute(string localName) { }

        protected internal virtual Threading.Tasks.Task WriteStartAttributeAsync(string prefix, string localName, string ns) { throw null; }

        public abstract void WriteStartDocument();
        public abstract void WriteStartDocument(bool standalone);
        public virtual Threading.Tasks.Task WriteStartDocumentAsync() { throw null; }

        public virtual Threading.Tasks.Task WriteStartDocumentAsync(bool standalone) { throw null; }

        public abstract void WriteStartElement(string prefix, string localName, string ns);
        public void WriteStartElement(string localName, string ns) { }

        public void WriteStartElement(string localName) { }

        public virtual Threading.Tasks.Task WriteStartElementAsync(string prefix, string localName, string ns) { throw null; }

        public abstract void WriteString(string text);
        public virtual Threading.Tasks.Task WriteStringAsync(string text) { throw null; }

        public abstract void WriteSurrogateCharEntity(char lowChar, char highChar);
        public virtual Threading.Tasks.Task WriteSurrogateCharEntityAsync(char lowChar, char highChar) { throw null; }

        public virtual void WriteValue(bool value) { }

        public virtual void WriteValue(DateTime value) { }

        public virtual void WriteValue(DateTimeOffset value) { }

        public virtual void WriteValue(decimal value) { }

        public virtual void WriteValue(double value) { }

        public virtual void WriteValue(int value) { }

        public virtual void WriteValue(long value) { }

        public virtual void WriteValue(object value) { }

        public virtual void WriteValue(float value) { }

        public virtual void WriteValue(string value) { }

        public abstract void WriteWhitespace(string ws);
        public virtual Threading.Tasks.Task WriteWhitespaceAsync(string ws) { throw null; }
    }

    public sealed partial class XmlWriterSettings
    {
        public bool Async { get { throw null; } set { } }

        public bool CheckCharacters { get { throw null; } set { } }

        public bool CloseOutput { get { throw null; } set { } }

        public ConformanceLevel ConformanceLevel { get { throw null; } set { } }

        public Text.Encoding Encoding { get { throw null; } set { } }

        public bool Indent { get { throw null; } set { } }

        public string IndentChars { get { throw null; } set { } }

        public NamespaceHandling NamespaceHandling { get { throw null; } set { } }

        public string NewLineChars { get { throw null; } set { } }

        public NewLineHandling NewLineHandling { get { throw null; } set { } }

        public bool NewLineOnAttributes { get { throw null; } set { } }

        public bool OmitXmlDeclaration { get { throw null; } set { } }

        public bool WriteEndDocumentOnClose { get { throw null; } set { } }

        public XmlWriterSettings Clone() { throw null; }

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
        Unqualified = 2
    }
}

namespace System.Xml.Serialization
{
    public partial interface IXmlSerializable
    {
        Schema.XmlSchema GetSchema();
        void ReadXml(XmlReader reader);
        void WriteXml(XmlWriter writer);
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public sealed partial class XmlSchemaProviderAttribute : Attribute
    {
        public XmlSchemaProviderAttribute(string methodName) { }

        public bool IsAny { get { throw null; } set { } }

        public string MethodName { get { throw null; } }
    }
}