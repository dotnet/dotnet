// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyFileVersion("3.0.0.34420")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.0.0+29.d4d088b6a9c793525b1a27a119cb66ba4587bb39")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v2.0", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyCompany("Microsoft")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Microsoft Xml Document Transformation (XDT) enables transformig XML files. This is the same technology used to transform web.config files for Visual Studio web projects.")]
[assembly: System.Reflection.AssemblyProduct("Microsoft.Web.XmlTransform")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.Web.XmlTransform")]
[assembly: System.Reflection.AssemblyVersionAttribute("3.0.0.34420")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.Web.XmlTransform
{
    public abstract partial class AttributeTransform : Transform
    {
        protected System.Xml.XmlNodeList TargetAttributes { get { throw null; } }

        protected System.Xml.XmlNodeList TransformAttributes { get { throw null; } }
    }

    public sealed partial class Condition : Locator
    {
        protected override string ConstructPredicate() { throw null; }
    }

    public partial interface IXmlOriginalDocumentService
    {
        System.Xml.XmlNodeList SelectNodes(string path, System.Xml.XmlNamespaceManager nsmgr);
    }

    public partial interface IXmlTransformationLogger
    {
        void EndSection(MessageType type, string message, params object[] messageArgs);
        void EndSection(string message, params object[] messageArgs);
        void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs);
        void LogError(string message, params object[] messageArgs);
        void LogError(string file, string message, params object[] messageArgs);
        void LogErrorFromException(System.Exception ex, string file, int lineNumber, int linePosition);
        void LogErrorFromException(System.Exception ex, string file);
        void LogErrorFromException(System.Exception ex);
        void LogMessage(MessageType type, string message, params object[] messageArgs);
        void LogMessage(string message, params object[] messageArgs);
        void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs);
        void LogWarning(string message, params object[] messageArgs);
        void LogWarning(string file, string message, params object[] messageArgs);
        void StartSection(MessageType type, string message, params object[] messageArgs);
        void StartSection(string message, params object[] messageArgs);
    }

    public abstract partial class Locator
    {
        protected System.Collections.Generic.IList<string> Arguments { get { throw null; } }

        protected string ArgumentString { get { throw null; } }

        protected System.Xml.XmlNode CurrentElement { get { throw null; } }

        protected XmlTransformationLogger Log { get { throw null; } }

        protected virtual XPathAxis NextStepAxis { get { throw null; } }

        protected virtual string NextStepNodeTest { get { throw null; } }

        protected virtual string ParentPath { get { throw null; } }

        protected string AppendStep(string basePath, XPathAxis stepAxis, string stepNodeTest, string predicate) { throw null; }

        protected string AppendStep(string basePath, XPathAxis stepAxis, string stepNodeTest) { throw null; }

        protected string AppendStep(string basePath, string stepNodeTest, string predicate) { throw null; }

        protected string AppendStep(string basePath, string stepNodeTest) { throw null; }

        protected virtual string ConstructPath() { throw null; }

        protected virtual string ConstructPredicate() { throw null; }

        protected void EnsureArguments() { }

        protected void EnsureArguments(int min, int max) { }

        protected void EnsureArguments(int min) { }
    }

    public sealed partial class Match : Locator
    {
        protected override string ConstructPredicate() { throw null; }
    }

    public enum MessageType
    {
        Normal = 0,
        Verbose = 1
    }

    public enum MissingTargetMessage
    {
        None = 0,
        Information = 1,
        Warning = 2,
        Error = 3
    }

    public partial class RemoveAttributes : AttributeTransform
    {
        protected override void Apply() { }
    }

    public partial class SetAttributes : AttributeTransform
    {
        protected override void Apply() { }
    }

    public partial class SetTokenizedAttributes : AttributeTransform
    {
        public static readonly string ParameterAttribute;
        public static readonly string Token;
        public static readonly string TokenNumber;
        public static readonly string XpathLocator;
        public static readonly string XPathWithIndex;
        public static readonly string XPathWithLocator;
        protected SetTokenizedAttributeStorage TransformStorage { get { throw null; } }

        protected override void Apply() { }

        protected string EscapeDirRegexSpecialCharacter(string value, bool escape) { throw null; }

        protected string GetAttributeValue(string attributeName) { throw null; }

        protected static string SubstituteKownValue(string transformValue, System.Text.RegularExpressions.Regex patternRegex, string patternPrefix, GetValueCallback getValueDelegate) { throw null; }

        protected delegate string GetValueCallback(string key);
    }

    public partial class SetTokenizedAttributeStorage
    {
        public SetTokenizedAttributeStorage() { }

        public SetTokenizedAttributeStorage(int capacity) { }

        public System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>> DictionaryList { get { throw null; } set { } }

        public bool EnableTokenizeParameters { get { throw null; } set { } }

        public string TokenFormat { get { throw null; } set { } }

        public bool UseXpathToFormParameter { get { throw null; } set { } }
    }

    public abstract partial class Transform
    {
        protected Transform() { }

        protected Transform(TransformFlags flags, MissingTargetMessage message) { }

        protected Transform(TransformFlags flags) { }

        protected bool ApplyTransformToAllTargetNodes { get { throw null; } set { } }

        protected System.Collections.Generic.IList<string> Arguments { get { throw null; } }

        protected string ArgumentString { get { throw null; } }

        protected XmlTransformationLogger Log { get { throw null; } }

        protected MissingTargetMessage MissingTargetMessage { get { throw null; } set { } }

        protected System.Xml.XmlNodeList TargetChildNodes { get { throw null; } }

        protected System.Xml.XmlNode TargetNode { get { throw null; } }

        protected System.Xml.XmlNodeList TargetNodes { get { throw null; } }

        protected System.Xml.XmlNode TransformNode { get { throw null; } }

        protected bool UseParentAsTargetNode { get { throw null; } set { } }

        protected abstract void Apply();
        protected T GetService<T>()
            where T : class { throw null; }
    }

    [System.Flags]
    public enum TransformFlags
    {
        None = 0,
        ApplyTransformToAllTargetNodes = 1,
        UseParentAsTargetNode = 2
    }

    public partial class XmlFileInfoDocument : System.Xml.XmlDocument, System.IDisposable
    {
        public override System.Xml.XmlAttribute CreateAttribute(string prefix, string localName, string namespaceURI) { throw null; }

        public override System.Xml.XmlElement CreateElement(string prefix, string localName, string namespaceURI) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~XmlFileInfoDocument() {
        }

        public override void Load(string filename) { }

        public override void Load(System.Xml.XmlReader reader) { }

        public override void Save(System.IO.Stream w) { }

        public override void Save(string filename) { }
    }

    public sealed partial class XmlNodeException : XmlTransformationException
    {
        public XmlNodeException(System.Exception innerException, System.Xml.XmlNode node) : base(default!) { }

        public XmlNodeException(string message, System.Xml.XmlNode node) : base(default!) { }

        public string FileName { get { throw null; } }

        public bool HasErrorInfo { get { throw null; } }

        public int LineNumber { get { throw null; } }

        public int LinePosition { get { throw null; } }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }

        public static System.Exception Wrap(System.Exception ex, System.Xml.XmlNode node) { throw null; }
    }

    public partial class XmlTransformableDocument : XmlFileInfoDocument, IXmlOriginalDocumentService
    {
        public bool IsChanged { get { throw null; } }

        System.Xml.XmlNodeList IXmlOriginalDocumentService.SelectNodes(string xpath, System.Xml.XmlNamespaceManager nsmgr) { throw null; }
    }

    public partial class XmlTransformation : System.IServiceProvider, System.IDisposable
    {
        public XmlTransformation(System.IO.Stream transformStream, IXmlTransformationLogger logger) { }

        public XmlTransformation(string transform, IXmlTransformationLogger logger) { }

        public XmlTransformation(string transform, bool isTransformAFile, IXmlTransformationLogger logger) { }

        public XmlTransformation(string transformFile) { }

        public bool HasTransformNamespace { get { throw null; } }

        public void AddTransformationService(System.Type serviceType, object serviceInstance) { }

        public bool Apply(System.Xml.XmlDocument xmlTarget) { throw null; }

        public void Dispose() { }

        protected virtual void Dispose(bool disposing) { }

        ~XmlTransformation() {
        }

        public object GetService(System.Type serviceType) { throw null; }

        public void RemoveTransformationService(System.Type serviceType) { }
    }

    public partial class XmlTransformationException : System.Exception
    {
        public XmlTransformationException(string message, System.Exception innerException) { }

        public XmlTransformationException(string message) { }
    }

    public partial class XmlTransformationLogger
    {
        internal XmlTransformationLogger() { }

        public bool SupressWarnings { get { throw null; } set { } }

        public void EndSection(MessageType type, string message, params object[] messageArgs) { }

        public void EndSection(string message, params object[] messageArgs) { }

        public void LogError(string message, params object[] messageArgs) { }

        public void LogError(System.Xml.XmlNode referenceNode, string message, params object[] messageArgs) { }

        public void LogMessage(MessageType type, string message, params object[] messageArgs) { }

        public void LogMessage(string message, params object[] messageArgs) { }

        public void LogWarning(string message, params object[] messageArgs) { }

        public void LogWarning(System.Xml.XmlNode referenceNode, string message, params object[] messageArgs) { }

        public void StartSection(MessageType type, string message, params object[] messageArgs) { }

        public void StartSection(string message, params object[] messageArgs) { }
    }

    public sealed partial class XPath : Locator
    {
        protected override string ParentPath { get { throw null; } }

        protected override string ConstructPath() { throw null; }
    }

    public enum XPathAxis
    {
        Child = 0,
        Descendant = 1,
        Parent = 2,
        Ancestor = 3,
        FollowingSibling = 4,
        PrecedingSibling = 5,
        Following = 6,
        Preceding = 7,
        Self = 8,
        DescendantOrSelf = 9,
        AncestorOrSelf = 10
    }
}