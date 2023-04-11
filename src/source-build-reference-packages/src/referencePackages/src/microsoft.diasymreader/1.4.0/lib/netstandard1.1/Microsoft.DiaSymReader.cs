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
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v1.1", FrameworkDisplayName = "")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.DiaSymReader.PortablePdb, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.DiaSymReader.UnitTests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100b5fc90e7027f67871e773a8fde8938c81dd402ba65b9201d60593e96c492651e889cc13f1415ebb53fac1131ae0bd333c5ee6021672d9718ea31a8aebd0da0072f25d87dba6fc90ffd598ed4da35e44c398c454307e8e33b8426143daec9f596836f97c8f74750e5975c64e2189f45def46b2a2b1247adc3652bf5c308055da9")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Microsoft DiaSymReader interop interfaces and utilities.")]
[assembly: System.Reflection.AssemblyFileVersion("1.400.22.11101")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.4.0+636a10b9d0aa317736c57d95f80e1c0849450715")]
[assembly: System.Reflection.AssemblyProduct("Microsoft.DiaSymReader")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.DiaSymReader")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/symreader")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.4.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.DiaSymReader
{
    public partial interface IMetadataImportProvider
    {
        object GetMetadataImport();
    }

    public partial interface ISymEncUnmanagedMethod
    {
        int GetDocumentsForMethod(int bufferLength, out int count, ISymUnmanagedDocument[] documents);
        int GetDocumentsForMethodCount(out int count);
        int GetFileNameFromOffset(int offset, int bufferLength, out int count, char[] name);
        int GetLineFromOffset(int offset, out int startLine, out int startColumn, out int endLine, out int endColumn, out int sequencePointOffset);
        int GetSourceExtentInDocument(ISymUnmanagedDocument document, out int startLine, out int endLine);
    }

    public partial interface ISymReaderMetadataProvider
    {
        unsafe bool TryGetStandaloneSignature(int standaloneSignatureToken, out byte* signature, out int length);
        bool TryGetTypeDefinitionInfo(int typeDefinitionToken, out string namespaceName, out string typeName, out System.Reflection.TypeAttributes attributes);
        bool TryGetTypeReferenceInfo(int typeReferenceToken, out string namespaceName, out string typeName);
    }

    public partial interface ISymUnmanagedAsyncMethod
    {
        int GetAsyncStepInfo(int bufferLength, out int count, int[] yieldOffsets, int[] breakpointOffset, int[] breakpointMethod);
        int GetAsyncStepInfoCount(out int count);
        int GetCatchHandlerILOffset(out int offset);
        int GetKickoffMethod(out int kickoffMethodToken);
        int HasCatchHandlerILOffset(out bool offset);
        int IsAsyncMethod(out bool value);
    }

    public partial interface ISymUnmanagedBinder
    {
        int GetReaderForFile(object metadataImporter, string fileName, string searchPath, out ISymUnmanagedReader reader);
        int GetReaderFromStream(object metadataImporter, object stream, out ISymUnmanagedReader reader);
    }

    public partial interface ISymUnmanagedBinder2 : ISymUnmanagedBinder
    {
        int GetReaderForFile(object metadataImporter, string fileName, string searchPath, out ISymUnmanagedReader reader);
        int GetReaderForFile2(object metadataImporter, string fileName, string searchPath, SymUnmanagedSearchPolicy searchPolicy, out ISymUnmanagedReader reader);
        int GetReaderFromStream(object metadataImporter, object stream, out ISymUnmanagedReader reader);
    }

    public partial interface ISymUnmanagedBinder3 : ISymUnmanagedBinder2, ISymUnmanagedBinder
    {
        int GetReaderForFile(object metadataImporter, string fileName, string searchPath, out ISymUnmanagedReader reader);
        int GetReaderForFile2(object metadataImporter, string fileName, string searchPath, SymUnmanagedSearchPolicy searchPolicy, out ISymUnmanagedReader reader);
        int GetReaderFromCallback(object metadataImporter, string fileName, string searchPath, SymUnmanagedSearchPolicy searchPolicy, object callback, out ISymUnmanagedReader reader);
        int GetReaderFromStream(object metadataImporter, object stream, out ISymUnmanagedReader reader);
    }

    public partial interface ISymUnmanagedBinder4 : ISymUnmanagedBinder3, ISymUnmanagedBinder2, ISymUnmanagedBinder
    {
        int GetReaderForFile(object metadataImporter, string fileName, string searchPath, out ISymUnmanagedReader reader);
        int GetReaderForFile2(object metadataImporter, string fileName, string searchPath, SymUnmanagedSearchPolicy searchPolicy, out ISymUnmanagedReader reader);
        int GetReaderFromCallback(object metadataImporter, string fileName, string searchPath, SymUnmanagedSearchPolicy searchPolicy, object callback, out ISymUnmanagedReader reader);
        int GetReaderFromPdbFile(IMetadataImportProvider metadataImportProvider, string pdbFilePath, out ISymUnmanagedReader reader);
        int GetReaderFromPdbStream(IMetadataImportProvider metadataImportProvider, object stream, out ISymUnmanagedReader reader);
        int GetReaderFromStream(object metadataImporter, object stream, out ISymUnmanagedReader reader);
    }

    public partial interface ISymUnmanagedCompilerInfoReader
    {
        int GetCompilerInfo(out ushort major, out ushort minor, out ushort build, out ushort revision, int bufferLength, out int count, char[] name);
    }

    public partial interface ISymUnmanagedCompilerInfoWriter
    {
        int AddCompilerInfo(ushort major, ushort minor, ushort build, ushort revision, string name);
    }

    public partial interface ISymUnmanagedConstant
    {
        int GetName(int bufferLength, out int count, char[] name);
        int GetSignature(int bufferLength, out int count, byte[] signature);
        int GetValue(out object value);
    }

    public partial interface ISymUnmanagedDispose
    {
        int Destroy();
    }

    public partial interface ISymUnmanagedDocument
    {
        int FindClosestLine(int line, out int closestLine);
        int GetChecksum(int bufferLength, out int count, byte[] checksum);
        int GetChecksumAlgorithmId(ref System.Guid algorithm);
        int GetDocumentType(ref System.Guid documentType);
        int GetLanguage(ref System.Guid language);
        int GetLanguageVendor(ref System.Guid vendor);
        int GetSourceLength(out int length);
        int GetSourceRange(int startLine, int startColumn, int endLine, int endColumn, int bufferLength, out int count, byte[] source);
        int GetUrl(int bufferLength, out int count, char[] url);
        int HasEmbeddedSource(out bool value);
    }

    public partial interface ISymUnmanagedEncUpdate
    {
        int GetLocalVariableCount(int methodToken, out int count);
        int GetLocalVariables(int methodToken, int bufferLength, ISymUnmanagedVariable[] variables, out int count);
        int InitializeForEnc();
        int UpdateMethodLines(int methodToken, int[] deltas, int count);
        int UpdateSymbolStore2(System.Runtime.InteropServices.ComTypes.IStream stream, SymUnmanagedLineDelta[] lineDeltas, int lineDeltaCount);
    }

    public partial interface ISymUnmanagedMethod
    {
        int GetNamespace(out ISymUnmanagedNamespace @namespace);
        int GetOffset(ISymUnmanagedDocument document, int line, int column, out int offset);
        int GetParameters(int bufferLength, out int count, ISymUnmanagedVariable[] parameters);
        int GetRanges(ISymUnmanagedDocument document, int line, int column, int bufferLength, out int count, int[] ranges);
        int GetRootScope(out ISymUnmanagedScope scope);
        int GetScopeFromOffset(int offset, out ISymUnmanagedScope scope);
        int GetSequencePointCount(out int count);
        int GetSequencePoints(int bufferLength, out int count, int[] offsets, ISymUnmanagedDocument[] documents, int[] startLines, int[] startColumns, int[] endLines, int[] endColumns);
        int GetSourceStartEnd(ISymUnmanagedDocument[] documents, int[] lines, int[] columns, out bool defined);
        int GetToken(out int methodToken);
    }

    public partial interface ISymUnmanagedMethod2 : ISymUnmanagedMethod
    {
        int GetLocalSignatureToken(out int localSignatureToken);
        int GetNamespace(out ISymUnmanagedNamespace @namespace);
        int GetOffset(ISymUnmanagedDocument document, int line, int column, out int offset);
        int GetParameters(int bufferLength, out int count, ISymUnmanagedVariable[] parameters);
        int GetRanges(ISymUnmanagedDocument document, int line, int column, int bufferLength, out int count, int[] ranges);
        int GetRootScope(out ISymUnmanagedScope scope);
        int GetScopeFromOffset(int offset, out ISymUnmanagedScope scope);
        int GetSequencePointCount(out int count);
        int GetSequencePoints(int bufferLength, out int count, int[] offsets, ISymUnmanagedDocument[] documents, int[] startLines, int[] startColumns, int[] endLines, int[] endColumns);
        int GetSourceStartEnd(ISymUnmanagedDocument[] documents, int[] lines, int[] columns, out bool defined);
        int GetToken(out int methodToken);
    }

    public partial interface ISymUnmanagedNamespace
    {
        int GetName(int bufferLength, out int count, char[] name);
        int GetNamespaces(int bufferLength, out int count, ISymUnmanagedNamespace[] namespaces);
        int GetVariables(int bufferLength, out int count, ISymUnmanagedVariable[] variables);
    }

    public partial interface ISymUnmanagedReader
    {
        int GetDocument(string url, System.Guid language, System.Guid languageVendor, System.Guid documentType, out ISymUnmanagedDocument document);
        int GetDocuments(int bufferLength, out int count, ISymUnmanagedDocument[] documents);
        int GetDocumentVersion(ISymUnmanagedDocument document, out int version, out bool isCurrent);
        int GetGlobalVariables(int bufferLength, out int count, ISymUnmanagedVariable[] variables);
        int GetMethod(int methodToken, out ISymUnmanagedMethod method);
        int GetMethodByVersion(int methodToken, int version, out ISymUnmanagedMethod method);
        int GetMethodFromDocumentPosition(ISymUnmanagedDocument document, int line, int column, out ISymUnmanagedMethod method);
        int GetMethodsFromDocumentPosition(ISymUnmanagedDocument document, int line, int column, int bufferLength, out int count, ISymUnmanagedMethod[] methods);
        int GetMethodVersion(ISymUnmanagedMethod method, out int version);
        int GetNamespaces(int bufferLength, out int count, ISymUnmanagedNamespace[] namespaces);
        int GetSymAttribute(int methodToken, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymbolStoreFileName(int bufferLength, out int count, char[] name);
        int GetUserEntryPoint(out int methodToken);
        int GetVariables(int methodToken, int bufferLength, out int count, ISymUnmanagedVariable[] variables);
        int Initialize(object metadataImporter, string fileName, string searchPath, System.Runtime.InteropServices.ComTypes.IStream stream);
        int ReplaceSymbolStore(string fileName, System.Runtime.InteropServices.ComTypes.IStream stream);
        int UpdateSymbolStore(string fileName, System.Runtime.InteropServices.ComTypes.IStream stream);
    }

    public partial interface ISymUnmanagedReader2 : ISymUnmanagedReader
    {
        int GetDocument(string url, System.Guid language, System.Guid languageVendor, System.Guid documentType, out ISymUnmanagedDocument document);
        int GetDocuments(int bufferLength, out int count, ISymUnmanagedDocument[] documents);
        int GetDocumentVersion(ISymUnmanagedDocument document, out int version, out bool isCurrent);
        int GetGlobalVariables(int bufferLength, out int count, ISymUnmanagedVariable[] variables);
        int GetMethod(int methodToken, out ISymUnmanagedMethod method);
        int GetMethodByVersion(int methodToken, int version, out ISymUnmanagedMethod method);
        int GetMethodByVersionPreRemap(int methodToken, int version, out ISymUnmanagedMethod method);
        int GetMethodFromDocumentPosition(ISymUnmanagedDocument document, int line, int column, out ISymUnmanagedMethod method);
        int GetMethodsFromDocumentPosition(ISymUnmanagedDocument document, int line, int column, int bufferLength, out int count, ISymUnmanagedMethod[] methods);
        int GetMethodsInDocument(ISymUnmanagedDocument document, int bufferLength, out int count, ISymUnmanagedMethod[] methods);
        int GetMethodVersion(ISymUnmanagedMethod method, out int version);
        int GetNamespaces(int bufferLength, out int count, ISymUnmanagedNamespace[] namespaces);
        int GetSymAttribute(int methodToken, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymAttributePreRemap(int methodToken, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymbolStoreFileName(int bufferLength, out int count, char[] name);
        int GetUserEntryPoint(out int methodToken);
        int GetVariables(int methodToken, int bufferLength, out int count, ISymUnmanagedVariable[] variables);
        int Initialize(object metadataImporter, string fileName, string searchPath, System.Runtime.InteropServices.ComTypes.IStream stream);
        int ReplaceSymbolStore(string fileName, System.Runtime.InteropServices.ComTypes.IStream stream);
        int UpdateSymbolStore(string fileName, System.Runtime.InteropServices.ComTypes.IStream stream);
    }

    public partial interface ISymUnmanagedReader3 : ISymUnmanagedReader2, ISymUnmanagedReader
    {
        int GetDocument(string url, System.Guid language, System.Guid languageVendor, System.Guid documentType, out ISymUnmanagedDocument document);
        int GetDocuments(int bufferLength, out int count, ISymUnmanagedDocument[] documents);
        int GetDocumentVersion(ISymUnmanagedDocument document, out int version, out bool isCurrent);
        int GetGlobalVariables(int bufferLength, out int count, ISymUnmanagedVariable[] variables);
        int GetMethod(int methodToken, out ISymUnmanagedMethod method);
        int GetMethodByVersion(int methodToken, int version, out ISymUnmanagedMethod method);
        int GetMethodByVersionPreRemap(int methodToken, int version, out ISymUnmanagedMethod method);
        int GetMethodFromDocumentPosition(ISymUnmanagedDocument document, int line, int column, out ISymUnmanagedMethod method);
        int GetMethodsFromDocumentPosition(ISymUnmanagedDocument document, int line, int column, int bufferLength, out int count, ISymUnmanagedMethod[] methods);
        int GetMethodsInDocument(ISymUnmanagedDocument document, int bufferLength, out int count, ISymUnmanagedMethod[] methods);
        int GetMethodVersion(ISymUnmanagedMethod method, out int version);
        int GetNamespaces(int bufferLength, out int count, ISymUnmanagedNamespace[] namespaces);
        int GetSymAttribute(int methodToken, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymAttributeByVersion(int methodToken, int version, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymAttributeByVersionPreRemap(int methodToken, int version, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymAttributePreRemap(int methodToken, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymbolStoreFileName(int bufferLength, out int count, char[] name);
        int GetUserEntryPoint(out int methodToken);
        int GetVariables(int methodToken, int bufferLength, out int count, ISymUnmanagedVariable[] variables);
        int Initialize(object metadataImporter, string fileName, string searchPath, System.Runtime.InteropServices.ComTypes.IStream stream);
        int ReplaceSymbolStore(string fileName, System.Runtime.InteropServices.ComTypes.IStream stream);
        int UpdateSymbolStore(string fileName, System.Runtime.InteropServices.ComTypes.IStream stream);
    }

    public partial interface ISymUnmanagedReader4 : ISymUnmanagedReader3, ISymUnmanagedReader2, ISymUnmanagedReader
    {
        int GetDocument(string url, System.Guid language, System.Guid languageVendor, System.Guid documentType, out ISymUnmanagedDocument document);
        int GetDocuments(int bufferLength, out int count, ISymUnmanagedDocument[] documents);
        int GetDocumentVersion(ISymUnmanagedDocument document, out int version, out bool isCurrent);
        int GetGlobalVariables(int bufferLength, out int count, ISymUnmanagedVariable[] variables);
        int GetMethod(int methodToken, out ISymUnmanagedMethod method);
        int GetMethodByVersion(int methodToken, int version, out ISymUnmanagedMethod method);
        int GetMethodByVersionPreRemap(int methodToken, int version, out ISymUnmanagedMethod method);
        int GetMethodFromDocumentPosition(ISymUnmanagedDocument document, int line, int column, out ISymUnmanagedMethod method);
        int GetMethodsFromDocumentPosition(ISymUnmanagedDocument document, int line, int column, int bufferLength, out int count, ISymUnmanagedMethod[] methods);
        int GetMethodsInDocument(ISymUnmanagedDocument document, int bufferLength, out int count, ISymUnmanagedMethod[] methods);
        int GetMethodVersion(ISymUnmanagedMethod method, out int version);
        int GetNamespaces(int bufferLength, out int count, ISymUnmanagedNamespace[] namespaces);
        unsafe int GetPortableDebugMetadata(out byte* metadata, out int size);
        unsafe int GetSourceServerData(out byte* data, out int size);
        int GetSymAttribute(int methodToken, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymAttributeByVersion(int methodToken, int version, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymAttributeByVersionPreRemap(int methodToken, int version, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymAttributePreRemap(int methodToken, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymbolStoreFileName(int bufferLength, out int count, char[] name);
        int GetUserEntryPoint(out int methodToken);
        int GetVariables(int methodToken, int bufferLength, out int count, ISymUnmanagedVariable[] variables);
        int Initialize(object metadataImporter, string fileName, string searchPath, System.Runtime.InteropServices.ComTypes.IStream stream);
        int MatchesModule(System.Guid guid, uint stamp, int age, out bool result);
        int ReplaceSymbolStore(string fileName, System.Runtime.InteropServices.ComTypes.IStream stream);
        int UpdateSymbolStore(string fileName, System.Runtime.InteropServices.ComTypes.IStream stream);
    }

    public partial interface ISymUnmanagedReader5 : ISymUnmanagedReader4, ISymUnmanagedReader3, ISymUnmanagedReader2, ISymUnmanagedReader
    {
        int GetDocument(string url, System.Guid language, System.Guid languageVendor, System.Guid documentType, out ISymUnmanagedDocument document);
        int GetDocuments(int bufferLength, out int count, ISymUnmanagedDocument[] documents);
        int GetDocumentVersion(ISymUnmanagedDocument document, out int version, out bool isCurrent);
        int GetGlobalVariables(int bufferLength, out int count, ISymUnmanagedVariable[] variables);
        int GetMethod(int methodToken, out ISymUnmanagedMethod method);
        int GetMethodByVersion(int methodToken, int version, out ISymUnmanagedMethod method);
        int GetMethodByVersionPreRemap(int methodToken, int version, out ISymUnmanagedMethod method);
        int GetMethodFromDocumentPosition(ISymUnmanagedDocument document, int line, int column, out ISymUnmanagedMethod method);
        int GetMethodsFromDocumentPosition(ISymUnmanagedDocument document, int line, int column, int bufferLength, out int count, ISymUnmanagedMethod[] methods);
        int GetMethodsInDocument(ISymUnmanagedDocument document, int bufferLength, out int count, ISymUnmanagedMethod[] methods);
        int GetMethodVersion(ISymUnmanagedMethod method, out int version);
        int GetNamespaces(int bufferLength, out int count, ISymUnmanagedNamespace[] namespaces);
        unsafe int GetPortableDebugMetadata(out byte* metadata, out int size);
        unsafe int GetPortableDebugMetadataByVersion(int version, out byte* metadata, out int size);
        unsafe int GetSourceServerData(out byte* data, out int size);
        int GetSymAttribute(int methodToken, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymAttributeByVersion(int methodToken, int version, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymAttributeByVersionPreRemap(int methodToken, int version, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymAttributePreRemap(int methodToken, string name, int bufferLength, out int count, byte[] customDebugInformation);
        int GetSymbolStoreFileName(int bufferLength, out int count, char[] name);
        int GetUserEntryPoint(out int methodToken);
        int GetVariables(int methodToken, int bufferLength, out int count, ISymUnmanagedVariable[] variables);
        int Initialize(object metadataImporter, string fileName, string searchPath, System.Runtime.InteropServices.ComTypes.IStream stream);
        int MatchesModule(System.Guid guid, uint stamp, int age, out bool result);
        int ReplaceSymbolStore(string fileName, System.Runtime.InteropServices.ComTypes.IStream stream);
        int UpdateSymbolStore(string fileName, System.Runtime.InteropServices.ComTypes.IStream stream);
    }

    public partial interface ISymUnmanagedScope
    {
        int GetChildren(int bufferLength, out int count, ISymUnmanagedScope[] children);
        int GetEndOffset(out int offset);
        int GetLocalCount(out int count);
        int GetLocals(int bufferLength, out int count, ISymUnmanagedVariable[] locals);
        int GetMethod(out ISymUnmanagedMethod method);
        int GetNamespaces(int bufferLength, out int count, ISymUnmanagedNamespace[] namespaces);
        int GetParent(out ISymUnmanagedScope scope);
        int GetStartOffset(out int offset);
    }

    public partial interface ISymUnmanagedScope2 : ISymUnmanagedScope
    {
        int GetChildren(int bufferLength, out int count, ISymUnmanagedScope[] children);
        int GetConstantCount(out int count);
        int GetConstants(int bufferLength, out int count, ISymUnmanagedConstant[] constants);
        int GetEndOffset(out int offset);
        int GetLocalCount(out int count);
        int GetLocals(int bufferLength, out int count, ISymUnmanagedVariable[] locals);
        int GetMethod(out ISymUnmanagedMethod method);
        int GetNamespaces(int bufferLength, out int count, ISymUnmanagedNamespace[] namespaces);
        int GetParent(out ISymUnmanagedScope scope);
        int GetStartOffset(out int offset);
    }

    public partial interface ISymUnmanagedSourceServerModule
    {
        unsafe int GetSourceServerData(out int length, out byte* data);
    }

    public partial interface ISymUnmanagedVariable
    {
        int GetAddressField1(out int value);
        int GetAddressField2(out int value);
        int GetAddressField3(out int value);
        int GetAddressKind(out int kind);
        int GetAttributes(out int attributes);
        int GetEndOffset(out int offset);
        int GetName(int bufferLength, out int count, char[] name);
        int GetSignature(int bufferLength, out int count, byte[] signature);
        int GetStartOffset(out int offset);
    }

    public partial interface ISymWriterMetadataProvider
    {
        bool TryGetEnclosingType(int nestedTypeToken, out int enclosingTypeToken);
        bool TryGetMethodInfo(int methodDefinitionToken, out string methodName, out int declaringTypeToken);
        bool TryGetTypeDefinitionInfo(int typeDefinitionToken, out string namespaceName, out string typeName, out System.Reflection.TypeAttributes attributes);
    }

    public partial struct SymUnmanagedAsyncStepInfo : System.IEquatable<SymUnmanagedAsyncStepInfo>
    {
        private int _dummyPrimitive;
        public SymUnmanagedAsyncStepInfo(int yieldOffset, int resumeOffset, int resumeMethod) { }

        public int ResumeMethod { get { throw null; } }

        public int ResumeOffset { get { throw null; } }

        public int YieldOffset { get { throw null; } }

        public bool Equals(SymUnmanagedAsyncStepInfo other) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
    }

    public static partial class SymUnmanagedExtensions
    {
        public static ISymUnmanagedAsyncMethod AsAsyncMethod(this ISymUnmanagedMethod method) { throw null; }

        public static System.Collections.Generic.IEnumerable<SymUnmanagedAsyncStepInfo> GetAsyncStepInfos(this ISymUnmanagedAsyncMethod method) { throw null; }

        public static int GetAttributes(this ISymUnmanagedVariable local) { throw null; }

        public static int GetCatchHandlerILOffset(this ISymUnmanagedAsyncMethod method) { throw null; }

        public static byte[] GetChecksum(this ISymUnmanagedDocument document) { throw null; }

        public static ISymUnmanagedScope[] GetChildren(this ISymUnmanagedScope scope) { throw null; }

        public static ISymUnmanagedConstant[] GetConstants(this ISymUnmanagedScope scope) { throw null; }

        public static ISymUnmanagedConstant[] GetConstants(this ISymUnmanagedScope2 scope) { throw null; }

        public static byte[] GetCustomDebugInfo(this ISymUnmanagedReader3 reader, int methodToken, int methodVersion) { throw null; }

        public static ISymUnmanagedDocument GetDocument(this ISymUnmanagedReader reader, string name) { throw null; }

        public static ISymUnmanagedDocument[] GetDocuments(this ISymUnmanagedReader reader) { throw null; }

        public static ISymUnmanagedDocument[] GetDocumentsForMethod(this ISymUnmanagedMethod method) { throw null; }

        public static System.Guid GetDocumentType(this ISymUnmanagedDocument document) { throw null; }

        public static System.ArraySegment<byte> GetEmbeddedSource(this ISymUnmanagedDocument document) { throw null; }

        public static int GetEndOffset(this ISymUnmanagedScope scope) { throw null; }

        public static System.Guid GetHashAlgorithm(this ISymUnmanagedDocument document) { throw null; }

        public static int GetKickoffMethod(this ISymUnmanagedAsyncMethod method) { throw null; }

        public static System.Guid GetLanguage(this ISymUnmanagedDocument document) { throw null; }

        public static System.Guid GetLanguageVendor(this ISymUnmanagedDocument document) { throw null; }

        public static ISymUnmanagedVariable[] GetLocals(this ISymUnmanagedScope scope) { throw null; }

        public static int GetLocalSignatureToken(this ISymUnmanagedMethod2 method) { throw null; }

        public static ISymUnmanagedMethod GetMethod(this ISymUnmanagedReader reader, int methodToken) { throw null; }

        public static ISymUnmanagedMethod GetMethodByVersion(this ISymUnmanagedReader reader, int methodToken, int methodVersion) { throw null; }

        public static ISymUnmanagedMethod[] GetMethodsInDocument(this ISymUnmanagedReader reader, ISymUnmanagedDocument symDocument) { throw null; }

        public static int GetMethodVersion(this ISymUnmanagedReader reader, ISymUnmanagedMethod method) { throw null; }

        public static string GetName(this ISymUnmanagedConstant constant) { throw null; }

        public static string GetName(this ISymUnmanagedDocument document) { throw null; }

        public static string GetName(this ISymUnmanagedNamespace @namespace) { throw null; }

        public static string GetName(this ISymUnmanagedVariable local) { throw null; }

        public static ISymUnmanagedNamespace[] GetNamespaces(this ISymUnmanagedScope scope) { throw null; }

        public static ISymUnmanagedReader GetReaderFromPdbStream(this ISymUnmanagedBinder4 binder, System.IO.Stream stream, IMetadataImportProvider metadataImportProvider) { throw null; }

        public static ISymUnmanagedReader GetReaderFromStream(this ISymUnmanagedBinder binder, System.IO.Stream stream, object metadataImporter) { throw null; }

        public static ISymUnmanagedScope GetRootScope(this ISymUnmanagedMethod method) { throw null; }

        public static System.Collections.Generic.IEnumerable<SymUnmanagedSequencePoint> GetSequencePoints(this ISymUnmanagedMethod method) { throw null; }

        public static byte[] GetSignature(this ISymUnmanagedConstant constant) { throw null; }

        public static int GetSlot(this ISymUnmanagedVariable local) { throw null; }

        public static void GetSourceExtentInDocument(this ISymEncUnmanagedMethod method, ISymUnmanagedDocument document, out int startLine, out int endLine) { throw null; }

        public static int GetStartOffset(this ISymUnmanagedScope scope) { throw null; }

        public static int GetToken(this ISymUnmanagedMethod method) { throw null; }

        public static int GetUserEntryPoint(this ISymUnmanagedReader reader) { throw null; }

        public static object GetValue(this ISymUnmanagedConstant constant) { throw null; }

        public static void Initialize(this ISymUnmanagedReader3 reader, System.IO.Stream stream, object metadataImporter, string fileName = null, string searchPath = null) { }

        public static bool TryGetCompilerInfo(this ISymUnmanagedCompilerInfoReader reader, out System.Version version, out string name) { throw null; }

        public static void UpdateSymbolStore(this ISymUnmanagedReader reader, System.IO.Stream stream, string fileName = null) { }
    }

    public partial struct SymUnmanagedLineDelta
    {
        public readonly int Delta;
        public readonly int MethodToken;
        public SymUnmanagedLineDelta(int methodToken, int delta) { }
    }

    [System.Flags]
    public enum SymUnmanagedReaderCreationOptions
    {
        Default = 0,
        UseAlternativeLoadPath = 2,
        UseComRegistry = 4
    }

    public static partial class SymUnmanagedReaderFactory
    {
        public static TSymUnmanagedReader CreateReader<TSymUnmanagedReader>(System.IO.Stream pdbStream, ISymReaderMetadataProvider metadataProvider, SymUnmanagedReaderCreationOptions options = SymUnmanagedReaderCreationOptions.Default)
            where TSymUnmanagedReader : class, ISymUnmanagedReader3 { throw null; }

        public static TSymUnmanagedReader CreateReaderWithMetadataImport<TSymUnmanagedReader>(System.IO.Stream pdbStream, object metadataImport, SymUnmanagedReaderCreationOptions options = SymUnmanagedReaderCreationOptions.Default)
            where TSymUnmanagedReader : class, ISymUnmanagedReader3 { throw null; }

        public static object CreateSymReaderMetadataImport(ISymReaderMetadataProvider metadataProvider) { throw null; }
    }

    public enum SymUnmanagedSearchPolicy
    {
        AllowRegistryAccess = 1,
        AllowSymbolServerAccess = 2,
        AllowOriginalPathAccess = 4,
        AllowReferencePathAccess = 8
    }

    public partial struct SymUnmanagedSequencePoint
    {
        public readonly ISymUnmanagedDocument Document;
        public readonly int EndColumn;
        public readonly int EndLine;
        public readonly int Offset;
        public readonly int StartColumn;
        public readonly int StartLine;
        public SymUnmanagedSequencePoint(int offset, ISymUnmanagedDocument document, int startLine, int startColumn, int endLine, int endColumn) { }

        public bool IsHidden { get { throw null; } }
    }

    public sealed partial class SymUnmanagedSequencePointsWriter
    {
        public SymUnmanagedSequencePointsWriter(SymUnmanagedWriter writer, int capacity = 64) { }

        public void Add(int documentIndex, int offset, int startLine, int startColumn, int endLine, int endColumn) { }

        public void Flush() { }
    }

    public static partial class SymUnmanagedStreamFactory
    {
        public static System.Runtime.InteropServices.ComTypes.IStream CreateStream(System.IO.Stream stream) { throw null; }
    }

    public abstract partial class SymUnmanagedWriter : System.IDisposable
    {
        protected SymUnmanagedWriter() { }

        public abstract int DocumentTableCapacity { get; set; }

        public virtual void AddCompilerInfo(ushort major, ushort minor, ushort build, ushort revision, string name) { }

        public abstract void CloseMethod();
        public abstract void CloseScope(int endOffset);
        public abstract void CloseTokensToSourceSpansMap();
        public abstract void DefineCustomMetadata(byte[] metadata);
        public abstract int DefineDocument(string name, System.Guid language, System.Guid vendor, System.Guid type, System.Guid algorithmId, byte[] checksum, byte[] source);
        public abstract bool DefineLocalConstant(string name, object value, int constantSignatureToken);
        public abstract void DefineLocalVariable(int index, string name, int attributes, int localSignatureToken);
        public abstract void DefineSequencePoints(int documentIndex, int count, int[] offsets, int[] startLines, int[] startColumns, int[] endLines, int[] endColumns);
        public abstract void Dispose();
        public abstract void GetSignature(out System.Guid guid, out uint stamp, out int age);
        public abstract System.Collections.Generic.IEnumerable<System.ArraySegment<byte>> GetUnderlyingData();
        public abstract void MapTokenToSourceSpan(int token, int documentIndex, int startLine, int startColumn, int endLine, int endColumn);
        public abstract void OpenMethod(int methodToken);
        public abstract void OpenScope(int startOffset);
        public abstract void OpenTokensToSourceSpansMap();
        public abstract void SetAsyncInfo(int moveNextMethodToken, int kickoffMethodToken, int catchHandlerOffset, int[] yieldOffsets, int[] resumeOffsets);
        public abstract void SetEntryPoint(int entryMethodToken);
        public abstract void SetSourceLinkData(byte[] data);
        public abstract void SetSourceServerData(byte[] data);
        public abstract void UpdateSignature(System.Guid guid, uint stamp, int age);
        public abstract void UsingNamespace(string importString);
        public abstract void WriteTo(System.IO.Stream stream);
    }

    [System.Flags]
    public enum SymUnmanagedWriterCreationOptions
    {
        Default = 0,
        UseAlternativeLoadPath = 2,
        UseComRegistry = 4,
        Deterministic = 8
    }

    public sealed partial class SymUnmanagedWriterException : System.Exception
    {
        public SymUnmanagedWriterException() { }

        public SymUnmanagedWriterException(string message, System.Exception innerException, string implementationModuleName) { }

        public SymUnmanagedWriterException(string message, System.Exception innerException) { }

        public SymUnmanagedWriterException(string message) { }

        public string ImplementationModuleName { get { throw null; } }
    }

    public static partial class SymUnmanagedWriterFactory
    {
        public static SymUnmanagedWriter CreateWriter(ISymWriterMetadataProvider metadataProvider, SymUnmanagedWriterCreationOptions options = SymUnmanagedWriterCreationOptions.Default) { throw null; }
    }
}