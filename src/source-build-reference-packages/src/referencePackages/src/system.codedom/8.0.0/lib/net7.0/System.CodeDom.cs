// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.CodeDom")]
[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
[assembly: System.Runtime.Versioning.UnsupportedOSPlatform("ios")]
[assembly: System.Runtime.Versioning.UnsupportedOSPlatform("tvos")]
[assembly: System.Runtime.Versioning.UnsupportedOSPlatform("maccatalyst")]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.System32)]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Provides types that can be used to model the structure of a source code document and to output source code for that model in a supported language.\r\n\r\nCommonly Used Types:\r\nSystem.CodeDom.CodeObject\r\nSystem.CodeDom.Compiler.CodeDomProvider\r\nMicrosoft.CSharp.CSharpCodeProvider\r\nMicrosoft.VisualBasic.VBCodeProvider")]
[assembly: System.Reflection.AssemblyFileVersion("8.0.23.53103")]
[assembly: System.Reflection.AssemblyInformationalVersion("8.0.0+5535e31a712343a63f5d7d796cd874e563e5ac14")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET")]
[assembly: System.Reflection.AssemblyTitle("System.CodeDom")]
[assembly: System.Reflection.AssemblyMetadata("RepositoryUrl", "https://github.com/dotnet/runtime")]
[assembly: System.Reflection.AssemblyVersionAttribute("8.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.CSharp
{
    public partial class CSharpCodeProvider : System.CodeDom.Compiler.CodeDomProvider
    {
        public CSharpCodeProvider() { }

        public CSharpCodeProvider(System.Collections.Generic.IDictionary<string, string> providerOptions) { }

        public override string FileExtension { get { throw null; } }

        [System.Obsolete("ICodeCompiler has been deprecated. Use the methods directly on the CodeDomProvider class instead.")]
        public override System.CodeDom.Compiler.ICodeCompiler CreateCompiler() { throw null; }

        [System.Obsolete("ICodeGenerator has been deprecated. Use the methods directly on the CodeDomProvider class instead.")]
        public override System.CodeDom.Compiler.ICodeGenerator CreateGenerator() { throw null; }

        public override void GenerateCodeFromMember(System.CodeDom.CodeTypeMember member, System.IO.TextWriter writer, System.CodeDom.Compiler.CodeGeneratorOptions options) { }

        public override System.ComponentModel.TypeConverter GetConverter(System.Type type) { throw null; }
    }
}

namespace Microsoft.VisualBasic
{
    public partial class VBCodeProvider : System.CodeDom.Compiler.CodeDomProvider
    {
        public VBCodeProvider() { }

        public VBCodeProvider(System.Collections.Generic.IDictionary<string, string> providerOptions) { }

        public override string FileExtension { get { throw null; } }

        public override System.CodeDom.Compiler.LanguageOptions LanguageOptions { get { throw null; } }

        [System.Obsolete("ICodeCompiler has been deprecated. Use the methods directly on the CodeDomProvider class instead.")]
        public override System.CodeDom.Compiler.ICodeCompiler CreateCompiler() { throw null; }

        [System.Obsolete("ICodeGenerator has been deprecated. Use the methods directly on the CodeDomProvider class instead.")]
        public override System.CodeDom.Compiler.ICodeGenerator CreateGenerator() { throw null; }

        public override void GenerateCodeFromMember(System.CodeDom.CodeTypeMember member, System.IO.TextWriter writer, System.CodeDom.Compiler.CodeGeneratorOptions options) { }

        public override System.ComponentModel.TypeConverter GetConverter(System.Type type) { throw null; }
    }
}

namespace System.CodeDom
{
    public partial class CodeArgumentReferenceExpression : CodeExpression
    {
        public CodeArgumentReferenceExpression() { }

        public CodeArgumentReferenceExpression(string parameterName) { }

        public string ParameterName { get { throw null; } set { } }
    }

    public partial class CodeArrayCreateExpression : CodeExpression
    {
        public CodeArrayCreateExpression() { }

        public CodeArrayCreateExpression(CodeTypeReference createType, CodeExpression size) { }

        public CodeArrayCreateExpression(CodeTypeReference createType, params CodeExpression[] initializers) { }

        public CodeArrayCreateExpression(CodeTypeReference createType, int size) { }

        public CodeArrayCreateExpression(string createType, CodeExpression size) { }

        public CodeArrayCreateExpression(string createType, params CodeExpression[] initializers) { }

        public CodeArrayCreateExpression(string createType, int size) { }

        public CodeArrayCreateExpression(Type createType, CodeExpression size) { }

        public CodeArrayCreateExpression(Type createType, params CodeExpression[] initializers) { }

        public CodeArrayCreateExpression(Type createType, int size) { }

        public CodeTypeReference CreateType { get { throw null; } set { } }

        public CodeExpressionCollection Initializers { get { throw null; } }

        public int Size { get { throw null; } set { } }

        public CodeExpression SizeExpression { get { throw null; } set { } }
    }

    public partial class CodeArrayIndexerExpression : CodeExpression
    {
        public CodeArrayIndexerExpression() { }

        public CodeArrayIndexerExpression(CodeExpression targetObject, params CodeExpression[] indices) { }

        public CodeExpressionCollection Indices { get { throw null; } }

        public CodeExpression TargetObject { get { throw null; } set { } }
    }

    public partial class CodeAssignStatement : CodeStatement
    {
        public CodeAssignStatement() { }

        public CodeAssignStatement(CodeExpression left, CodeExpression right) { }

        public CodeExpression Left { get { throw null; } set { } }

        public CodeExpression Right { get { throw null; } set { } }
    }

    public partial class CodeAttachEventStatement : CodeStatement
    {
        public CodeAttachEventStatement() { }

        public CodeAttachEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener) { }

        public CodeAttachEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener) { }

        public CodeEventReferenceExpression Event { get { throw null; } set { } }

        public CodeExpression Listener { get { throw null; } set { } }
    }

    public partial class CodeAttributeArgument
    {
        public CodeAttributeArgument() { }

        public CodeAttributeArgument(CodeExpression value) { }

        public CodeAttributeArgument(string name, CodeExpression value) { }

        public string Name { get { throw null; } set { } }

        public CodeExpression Value { get { throw null; } set { } }
    }

    public partial class CodeAttributeArgumentCollection : Collections.CollectionBase
    {
        public CodeAttributeArgumentCollection() { }

        public CodeAttributeArgumentCollection(CodeAttributeArgument[] value) { }

        public CodeAttributeArgumentCollection(CodeAttributeArgumentCollection value) { }

        public CodeAttributeArgument this[int index] { get { throw null; } set { } }

        public int Add(CodeAttributeArgument value) { throw null; }

        public void AddRange(CodeAttributeArgument[] value) { }

        public void AddRange(CodeAttributeArgumentCollection value) { }

        public bool Contains(CodeAttributeArgument value) { throw null; }

        public void CopyTo(CodeAttributeArgument[] array, int index) { }

        public int IndexOf(CodeAttributeArgument value) { throw null; }

        public void Insert(int index, CodeAttributeArgument value) { }

        public void Remove(CodeAttributeArgument value) { }
    }

    public partial class CodeAttributeDeclaration
    {
        public CodeAttributeDeclaration() { }

        public CodeAttributeDeclaration(CodeTypeReference attributeType, params CodeAttributeArgument[] arguments) { }

        public CodeAttributeDeclaration(CodeTypeReference attributeType) { }

        public CodeAttributeDeclaration(string name, params CodeAttributeArgument[] arguments) { }

        public CodeAttributeDeclaration(string name) { }

        public CodeAttributeArgumentCollection Arguments { get { throw null; } }

        public CodeTypeReference AttributeType { get { throw null; } }

        public string Name { get { throw null; } set { } }
    }

    public partial class CodeAttributeDeclarationCollection : Collections.CollectionBase
    {
        public CodeAttributeDeclarationCollection() { }

        public CodeAttributeDeclarationCollection(CodeAttributeDeclaration[] value) { }

        public CodeAttributeDeclarationCollection(CodeAttributeDeclarationCollection value) { }

        public CodeAttributeDeclaration this[int index] { get { throw null; } set { } }

        public int Add(CodeAttributeDeclaration value) { throw null; }

        public void AddRange(CodeAttributeDeclaration[] value) { }

        public void AddRange(CodeAttributeDeclarationCollection value) { }

        public bool Contains(CodeAttributeDeclaration value) { throw null; }

        public void CopyTo(CodeAttributeDeclaration[] array, int index) { }

        public int IndexOf(CodeAttributeDeclaration value) { throw null; }

        public void Insert(int index, CodeAttributeDeclaration value) { }

        public void Remove(CodeAttributeDeclaration value) { }
    }

    public partial class CodeBaseReferenceExpression : CodeExpression
    {
    }

    public partial class CodeBinaryOperatorExpression : CodeExpression
    {
        public CodeBinaryOperatorExpression() { }

        public CodeBinaryOperatorExpression(CodeExpression left, CodeBinaryOperatorType op, CodeExpression right) { }

        public CodeExpression Left { get { throw null; } set { } }

        public CodeBinaryOperatorType Operator { get { throw null; } set { } }

        public CodeExpression Right { get { throw null; } set { } }
    }

    public enum CodeBinaryOperatorType
    {
        Add = 0,
        Subtract = 1,
        Multiply = 2,
        Divide = 3,
        Modulus = 4,
        Assign = 5,
        IdentityInequality = 6,
        IdentityEquality = 7,
        ValueEquality = 8,
        BitwiseOr = 9,
        BitwiseAnd = 10,
        BooleanOr = 11,
        BooleanAnd = 12,
        LessThan = 13,
        LessThanOrEqual = 14,
        GreaterThan = 15,
        GreaterThanOrEqual = 16
    }

    public partial class CodeCastExpression : CodeExpression
    {
        public CodeCastExpression() { }

        public CodeCastExpression(CodeTypeReference targetType, CodeExpression expression) { }

        public CodeCastExpression(string targetType, CodeExpression expression) { }

        public CodeCastExpression(Type targetType, CodeExpression expression) { }

        public CodeExpression Expression { get { throw null; } set { } }

        public CodeTypeReference TargetType { get { throw null; } set { } }
    }

    public partial class CodeCatchClause
    {
        public CodeCatchClause() { }

        public CodeCatchClause(string localName, CodeTypeReference catchExceptionType, params CodeStatement[] statements) { }

        public CodeCatchClause(string localName, CodeTypeReference catchExceptionType) { }

        public CodeCatchClause(string localName) { }

        public CodeTypeReference CatchExceptionType { get { throw null; } set { } }

        public string LocalName { get { throw null; } set { } }

        public CodeStatementCollection Statements { get { throw null; } }
    }

    public partial class CodeCatchClauseCollection : Collections.CollectionBase
    {
        public CodeCatchClauseCollection() { }

        public CodeCatchClauseCollection(CodeCatchClause[] value) { }

        public CodeCatchClauseCollection(CodeCatchClauseCollection value) { }

        public CodeCatchClause this[int index] { get { throw null; } set { } }

        public int Add(CodeCatchClause value) { throw null; }

        public void AddRange(CodeCatchClause[] value) { }

        public void AddRange(CodeCatchClauseCollection value) { }

        public bool Contains(CodeCatchClause value) { throw null; }

        public void CopyTo(CodeCatchClause[] array, int index) { }

        public int IndexOf(CodeCatchClause value) { throw null; }

        public void Insert(int index, CodeCatchClause value) { }

        public void Remove(CodeCatchClause value) { }
    }

    public partial class CodeChecksumPragma : CodeDirective
    {
        public CodeChecksumPragma() { }

        public CodeChecksumPragma(string fileName, Guid checksumAlgorithmId, byte[] checksumData) { }

        public Guid ChecksumAlgorithmId { get { throw null; } set { } }

        public byte[] ChecksumData { get { throw null; } set { } }

        public string FileName { get { throw null; } set { } }
    }

    public partial class CodeComment : CodeObject
    {
        public CodeComment() { }

        public CodeComment(string text, bool docComment) { }

        public CodeComment(string text) { }

        public bool DocComment { get { throw null; } set { } }

        public string Text { get { throw null; } set { } }
    }

    public partial class CodeCommentStatement : CodeStatement
    {
        public CodeCommentStatement() { }

        public CodeCommentStatement(CodeComment comment) { }

        public CodeCommentStatement(string text, bool docComment) { }

        public CodeCommentStatement(string text) { }

        public CodeComment Comment { get { throw null; } set { } }
    }

    public partial class CodeCommentStatementCollection : Collections.CollectionBase
    {
        public CodeCommentStatementCollection() { }

        public CodeCommentStatementCollection(CodeCommentStatement[] value) { }

        public CodeCommentStatementCollection(CodeCommentStatementCollection value) { }

        public CodeCommentStatement this[int index] { get { throw null; } set { } }

        public int Add(CodeCommentStatement value) { throw null; }

        public void AddRange(CodeCommentStatement[] value) { }

        public void AddRange(CodeCommentStatementCollection value) { }

        public bool Contains(CodeCommentStatement value) { throw null; }

        public void CopyTo(CodeCommentStatement[] array, int index) { }

        public int IndexOf(CodeCommentStatement value) { throw null; }

        public void Insert(int index, CodeCommentStatement value) { }

        public void Remove(CodeCommentStatement value) { }
    }

    public partial class CodeCompileUnit : CodeObject
    {
        public CodeAttributeDeclarationCollection AssemblyCustomAttributes { get { throw null; } }

        public CodeDirectiveCollection EndDirectives { get { throw null; } }

        public CodeNamespaceCollection Namespaces { get { throw null; } }

        public Collections.Specialized.StringCollection ReferencedAssemblies { get { throw null; } }

        public CodeDirectiveCollection StartDirectives { get { throw null; } }
    }

    public partial class CodeConditionStatement : CodeStatement
    {
        public CodeConditionStatement() { }

        public CodeConditionStatement(CodeExpression condition, CodeStatement[] trueStatements, CodeStatement[] falseStatements) { }

        public CodeConditionStatement(CodeExpression condition, params CodeStatement[] trueStatements) { }

        public CodeExpression Condition { get { throw null; } set { } }

        public CodeStatementCollection FalseStatements { get { throw null; } }

        public CodeStatementCollection TrueStatements { get { throw null; } }
    }

    public partial class CodeConstructor : CodeMemberMethod
    {
        public CodeExpressionCollection BaseConstructorArgs { get { throw null; } }

        public CodeExpressionCollection ChainedConstructorArgs { get { throw null; } }
    }

    public partial class CodeDefaultValueExpression : CodeExpression
    {
        public CodeDefaultValueExpression() { }

        public CodeDefaultValueExpression(CodeTypeReference type) { }

        public CodeTypeReference Type { get { throw null; } set { } }
    }

    public partial class CodeDelegateCreateExpression : CodeExpression
    {
        public CodeDelegateCreateExpression() { }

        public CodeDelegateCreateExpression(CodeTypeReference delegateType, CodeExpression targetObject, string methodName) { }

        public CodeTypeReference DelegateType { get { throw null; } set { } }

        public string MethodName { get { throw null; } set { } }

        public CodeExpression TargetObject { get { throw null; } set { } }
    }

    public partial class CodeDelegateInvokeExpression : CodeExpression
    {
        public CodeDelegateInvokeExpression() { }

        public CodeDelegateInvokeExpression(CodeExpression targetObject, params CodeExpression[] parameters) { }

        public CodeDelegateInvokeExpression(CodeExpression targetObject) { }

        public CodeExpressionCollection Parameters { get { throw null; } }

        public CodeExpression TargetObject { get { throw null; } set { } }
    }

    public partial class CodeDirectionExpression : CodeExpression
    {
        public CodeDirectionExpression() { }

        public CodeDirectionExpression(FieldDirection direction, CodeExpression expression) { }

        public FieldDirection Direction { get { throw null; } set { } }

        public CodeExpression Expression { get { throw null; } set { } }
    }

    public partial class CodeDirective : CodeObject
    {
    }

    public partial class CodeDirectiveCollection : Collections.CollectionBase
    {
        public CodeDirectiveCollection() { }

        public CodeDirectiveCollection(CodeDirective[] value) { }

        public CodeDirectiveCollection(CodeDirectiveCollection value) { }

        public CodeDirective this[int index] { get { throw null; } set { } }

        public int Add(CodeDirective value) { throw null; }

        public void AddRange(CodeDirective[] value) { }

        public void AddRange(CodeDirectiveCollection value) { }

        public bool Contains(CodeDirective value) { throw null; }

        public void CopyTo(CodeDirective[] array, int index) { }

        public int IndexOf(CodeDirective value) { throw null; }

        public void Insert(int index, CodeDirective value) { }

        public void Remove(CodeDirective value) { }
    }

    public partial class CodeEntryPointMethod : CodeMemberMethod
    {
    }

    public partial class CodeEventReferenceExpression : CodeExpression
    {
        public CodeEventReferenceExpression() { }

        public CodeEventReferenceExpression(CodeExpression targetObject, string eventName) { }

        public string EventName { get { throw null; } set { } }

        public CodeExpression TargetObject { get { throw null; } set { } }
    }

    public partial class CodeExpression : CodeObject
    {
    }

    public partial class CodeExpressionCollection : Collections.CollectionBase
    {
        public CodeExpressionCollection() { }

        public CodeExpressionCollection(CodeExpression[] value) { }

        public CodeExpressionCollection(CodeExpressionCollection value) { }

        public CodeExpression this[int index] { get { throw null; } set { } }

        public int Add(CodeExpression value) { throw null; }

        public void AddRange(CodeExpression[] value) { }

        public void AddRange(CodeExpressionCollection value) { }

        public bool Contains(CodeExpression value) { throw null; }

        public void CopyTo(CodeExpression[] array, int index) { }

        public int IndexOf(CodeExpression value) { throw null; }

        public void Insert(int index, CodeExpression value) { }

        public void Remove(CodeExpression value) { }
    }

    public partial class CodeExpressionStatement : CodeStatement
    {
        public CodeExpressionStatement() { }

        public CodeExpressionStatement(CodeExpression expression) { }

        public CodeExpression Expression { get { throw null; } set { } }
    }

    public partial class CodeFieldReferenceExpression : CodeExpression
    {
        public CodeFieldReferenceExpression() { }

        public CodeFieldReferenceExpression(CodeExpression targetObject, string fieldName) { }

        public string FieldName { get { throw null; } set { } }

        public CodeExpression TargetObject { get { throw null; } set { } }
    }

    public partial class CodeGotoStatement : CodeStatement
    {
        public CodeGotoStatement() { }

        public CodeGotoStatement(string label) { }

        public string Label { get { throw null; } set { } }
    }

    public partial class CodeIndexerExpression : CodeExpression
    {
        public CodeIndexerExpression() { }

        public CodeIndexerExpression(CodeExpression targetObject, params CodeExpression[] indices) { }

        public CodeExpressionCollection Indices { get { throw null; } }

        public CodeExpression TargetObject { get { throw null; } set { } }
    }

    public partial class CodeIterationStatement : CodeStatement
    {
        public CodeIterationStatement() { }

        public CodeIterationStatement(CodeStatement initStatement, CodeExpression testExpression, CodeStatement incrementStatement, params CodeStatement[] statements) { }

        public CodeStatement IncrementStatement { get { throw null; } set { } }

        public CodeStatement InitStatement { get { throw null; } set { } }

        public CodeStatementCollection Statements { get { throw null; } }

        public CodeExpression TestExpression { get { throw null; } set { } }
    }

    public partial class CodeLabeledStatement : CodeStatement
    {
        public CodeLabeledStatement() { }

        public CodeLabeledStatement(string label, CodeStatement statement) { }

        public CodeLabeledStatement(string label) { }

        public string Label { get { throw null; } set { } }

        public CodeStatement Statement { get { throw null; } set { } }
    }

    public partial class CodeLinePragma
    {
        public CodeLinePragma() { }

        public CodeLinePragma(string fileName, int lineNumber) { }

        public string FileName { get { throw null; } set { } }

        public int LineNumber { get { throw null; } set { } }
    }

    public partial class CodeMemberEvent : CodeTypeMember
    {
        public CodeTypeReferenceCollection ImplementationTypes { get { throw null; } }

        public CodeTypeReference PrivateImplementationType { get { throw null; } set { } }

        public CodeTypeReference Type { get { throw null; } set { } }
    }

    public partial class CodeMemberField : CodeTypeMember
    {
        public CodeMemberField() { }

        public CodeMemberField(CodeTypeReference type, string name) { }

        public CodeMemberField(string type, string name) { }

        public CodeMemberField(Type type, string name) { }

        public CodeExpression InitExpression { get { throw null; } set { } }

        public CodeTypeReference Type { get { throw null; } set { } }
    }

    public partial class CodeMemberMethod : CodeTypeMember
    {
        public CodeTypeReferenceCollection ImplementationTypes { get { throw null; } }

        public CodeParameterDeclarationExpressionCollection Parameters { get { throw null; } }

        public CodeTypeReference PrivateImplementationType { get { throw null; } set { } }

        public CodeTypeReference ReturnType { get { throw null; } set { } }

        public CodeAttributeDeclarationCollection ReturnTypeCustomAttributes { get { throw null; } }

        public CodeStatementCollection Statements { get { throw null; } }

        public CodeTypeParameterCollection TypeParameters { get { throw null; } }

        public event EventHandler PopulateImplementationTypes { add { } remove { } }

        public event EventHandler PopulateParameters { add { } remove { } }

        public event EventHandler PopulateStatements { add { } remove { } }
    }

    public partial class CodeMemberProperty : CodeTypeMember
    {
        public CodeStatementCollection GetStatements { get { throw null; } }

        public bool HasGet { get { throw null; } set { } }

        public bool HasSet { get { throw null; } set { } }

        public CodeTypeReferenceCollection ImplementationTypes { get { throw null; } }

        public CodeParameterDeclarationExpressionCollection Parameters { get { throw null; } }

        public CodeTypeReference PrivateImplementationType { get { throw null; } set { } }

        public CodeStatementCollection SetStatements { get { throw null; } }

        public CodeTypeReference Type { get { throw null; } set { } }
    }

    public partial class CodeMethodInvokeExpression : CodeExpression
    {
        public CodeMethodInvokeExpression() { }

        public CodeMethodInvokeExpression(CodeExpression targetObject, string methodName, params CodeExpression[] parameters) { }

        public CodeMethodInvokeExpression(CodeMethodReferenceExpression method, params CodeExpression[] parameters) { }

        public CodeMethodReferenceExpression Method { get { throw null; } set { } }

        public CodeExpressionCollection Parameters { get { throw null; } }
    }

    public partial class CodeMethodReferenceExpression : CodeExpression
    {
        public CodeMethodReferenceExpression() { }

        public CodeMethodReferenceExpression(CodeExpression targetObject, string methodName, params CodeTypeReference[] typeParameters) { }

        public CodeMethodReferenceExpression(CodeExpression targetObject, string methodName) { }

        public string MethodName { get { throw null; } set { } }

        public CodeExpression TargetObject { get { throw null; } set { } }

        public CodeTypeReferenceCollection TypeArguments { get { throw null; } }
    }

    public partial class CodeMethodReturnStatement : CodeStatement
    {
        public CodeMethodReturnStatement() { }

        public CodeMethodReturnStatement(CodeExpression expression) { }

        public CodeExpression Expression { get { throw null; } set { } }
    }

    public partial class CodeNamespace : CodeObject
    {
        public CodeNamespace() { }

        public CodeNamespace(string name) { }

        public CodeCommentStatementCollection Comments { get { throw null; } }

        public CodeNamespaceImportCollection Imports { get { throw null; } }

        public string Name { get { throw null; } set { } }

        public CodeTypeDeclarationCollection Types { get { throw null; } }

        public event EventHandler PopulateComments { add { } remove { } }

        public event EventHandler PopulateImports { add { } remove { } }

        public event EventHandler PopulateTypes { add { } remove { } }
    }

    public partial class CodeNamespaceCollection : Collections.CollectionBase
    {
        public CodeNamespaceCollection() { }

        public CodeNamespaceCollection(CodeNamespace[] value) { }

        public CodeNamespaceCollection(CodeNamespaceCollection value) { }

        public CodeNamespace this[int index] { get { throw null; } set { } }

        public int Add(CodeNamespace value) { throw null; }

        public void AddRange(CodeNamespace[] value) { }

        public void AddRange(CodeNamespaceCollection value) { }

        public bool Contains(CodeNamespace value) { throw null; }

        public void CopyTo(CodeNamespace[] array, int index) { }

        public int IndexOf(CodeNamespace value) { throw null; }

        public void Insert(int index, CodeNamespace value) { }

        public void Remove(CodeNamespace value) { }
    }

    public partial class CodeNamespaceImport : CodeObject
    {
        public CodeNamespaceImport() { }

        public CodeNamespaceImport(string nameSpace) { }

        public CodeLinePragma LinePragma { get { throw null; } set { } }

        public string Namespace { get { throw null; } set { } }
    }

    public partial class CodeNamespaceImportCollection : Collections.IList, Collections.ICollection, Collections.IEnumerable
    {
        public int Count { get { throw null; } }

        public CodeNamespaceImport this[int index] { get { throw null; } set { } }

        int Collections.ICollection.Count { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        bool Collections.IList.IsFixedSize { get { throw null; } }

        bool Collections.IList.IsReadOnly { get { throw null; } }

        object Collections.IList.this[int index] { get { throw null; } set { } }

        public void Add(CodeNamespaceImport value) { }

        public void AddRange(CodeNamespaceImport[] value) { }

        public void Clear() { }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int index) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        int Collections.IList.Add(object value) { throw null; }

        void Collections.IList.Clear() { }

        bool Collections.IList.Contains(object value) { throw null; }

        int Collections.IList.IndexOf(object value) { throw null; }

        void Collections.IList.Insert(int index, object value) { }

        void Collections.IList.Remove(object value) { }

        void Collections.IList.RemoveAt(int index) { }
    }

    public partial class CodeObject
    {
        public Collections.IDictionary UserData { get { throw null; } }
    }

    public partial class CodeObjectCreateExpression : CodeExpression
    {
        public CodeObjectCreateExpression() { }

        public CodeObjectCreateExpression(CodeTypeReference createType, params CodeExpression[] parameters) { }

        public CodeObjectCreateExpression(string createType, params CodeExpression[] parameters) { }

        public CodeObjectCreateExpression(Type createType, params CodeExpression[] parameters) { }

        public CodeTypeReference CreateType { get { throw null; } set { } }

        public CodeExpressionCollection Parameters { get { throw null; } }
    }

    public partial class CodeParameterDeclarationExpression : CodeExpression
    {
        public CodeParameterDeclarationExpression() { }

        public CodeParameterDeclarationExpression(CodeTypeReference type, string name) { }

        public CodeParameterDeclarationExpression(string type, string name) { }

        public CodeParameterDeclarationExpression(Type type, string name) { }

        public CodeAttributeDeclarationCollection CustomAttributes { get { throw null; } set { } }

        public FieldDirection Direction { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public CodeTypeReference Type { get { throw null; } set { } }
    }

    public partial class CodeParameterDeclarationExpressionCollection : Collections.CollectionBase
    {
        public CodeParameterDeclarationExpressionCollection() { }

        public CodeParameterDeclarationExpressionCollection(CodeParameterDeclarationExpression[] value) { }

        public CodeParameterDeclarationExpressionCollection(CodeParameterDeclarationExpressionCollection value) { }

        public CodeParameterDeclarationExpression this[int index] { get { throw null; } set { } }

        public int Add(CodeParameterDeclarationExpression value) { throw null; }

        public void AddRange(CodeParameterDeclarationExpression[] value) { }

        public void AddRange(CodeParameterDeclarationExpressionCollection value) { }

        public bool Contains(CodeParameterDeclarationExpression value) { throw null; }

        public void CopyTo(CodeParameterDeclarationExpression[] array, int index) { }

        public int IndexOf(CodeParameterDeclarationExpression value) { throw null; }

        public void Insert(int index, CodeParameterDeclarationExpression value) { }

        public void Remove(CodeParameterDeclarationExpression value) { }
    }

    public partial class CodePrimitiveExpression : CodeExpression
    {
        public CodePrimitiveExpression() { }

        public CodePrimitiveExpression(object value) { }

        public object Value { get { throw null; } set { } }
    }

    public partial class CodePropertyReferenceExpression : CodeExpression
    {
        public CodePropertyReferenceExpression() { }

        public CodePropertyReferenceExpression(CodeExpression targetObject, string propertyName) { }

        public string PropertyName { get { throw null; } set { } }

        public CodeExpression TargetObject { get { throw null; } set { } }
    }

    public partial class CodePropertySetValueReferenceExpression : CodeExpression
    {
    }

    public partial class CodeRegionDirective : CodeDirective
    {
        public CodeRegionDirective() { }

        public CodeRegionDirective(CodeRegionMode regionMode, string regionText) { }

        public CodeRegionMode RegionMode { get { throw null; } set { } }

        public string RegionText { get { throw null; } set { } }
    }

    public enum CodeRegionMode
    {
        None = 0,
        Start = 1,
        End = 2
    }

    public partial class CodeRemoveEventStatement : CodeStatement
    {
        public CodeRemoveEventStatement() { }

        public CodeRemoveEventStatement(CodeEventReferenceExpression eventRef, CodeExpression listener) { }

        public CodeRemoveEventStatement(CodeExpression targetObject, string eventName, CodeExpression listener) { }

        public CodeEventReferenceExpression Event { get { throw null; } set { } }

        public CodeExpression Listener { get { throw null; } set { } }
    }

    public partial class CodeSnippetCompileUnit : CodeCompileUnit
    {
        public CodeSnippetCompileUnit() { }

        public CodeSnippetCompileUnit(string value) { }

        public CodeLinePragma LinePragma { get { throw null; } set { } }

        public string Value { get { throw null; } set { } }
    }

    public partial class CodeSnippetExpression : CodeExpression
    {
        public CodeSnippetExpression() { }

        public CodeSnippetExpression(string value) { }

        public string Value { get { throw null; } set { } }
    }

    public partial class CodeSnippetStatement : CodeStatement
    {
        public CodeSnippetStatement() { }

        public CodeSnippetStatement(string value) { }

        public string Value { get { throw null; } set { } }
    }

    public partial class CodeSnippetTypeMember : CodeTypeMember
    {
        public CodeSnippetTypeMember() { }

        public CodeSnippetTypeMember(string text) { }

        public string Text { get { throw null; } set { } }
    }

    public partial class CodeStatement : CodeObject
    {
        public CodeDirectiveCollection EndDirectives { get { throw null; } }

        public CodeLinePragma LinePragma { get { throw null; } set { } }

        public CodeDirectiveCollection StartDirectives { get { throw null; } }
    }

    public partial class CodeStatementCollection : Collections.CollectionBase
    {
        public CodeStatementCollection() { }

        public CodeStatementCollection(CodeStatement[] value) { }

        public CodeStatementCollection(CodeStatementCollection value) { }

        public CodeStatement this[int index] { get { throw null; } set { } }

        public int Add(CodeExpression value) { throw null; }

        public int Add(CodeStatement value) { throw null; }

        public void AddRange(CodeStatement[] value) { }

        public void AddRange(CodeStatementCollection value) { }

        public bool Contains(CodeStatement value) { throw null; }

        public void CopyTo(CodeStatement[] array, int index) { }

        public int IndexOf(CodeStatement value) { throw null; }

        public void Insert(int index, CodeStatement value) { }

        public void Remove(CodeStatement value) { }
    }

    public partial class CodeThisReferenceExpression : CodeExpression
    {
    }

    public partial class CodeThrowExceptionStatement : CodeStatement
    {
        public CodeThrowExceptionStatement() { }

        public CodeThrowExceptionStatement(CodeExpression toThrow) { }

        public CodeExpression ToThrow { get { throw null; } set { } }
    }

    public partial class CodeTryCatchFinallyStatement : CodeStatement
    {
        public CodeTryCatchFinallyStatement() { }

        public CodeTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses, CodeStatement[] finallyStatements) { }

        public CodeTryCatchFinallyStatement(CodeStatement[] tryStatements, CodeCatchClause[] catchClauses) { }

        public CodeCatchClauseCollection CatchClauses { get { throw null; } }

        public CodeStatementCollection FinallyStatements { get { throw null; } }

        public CodeStatementCollection TryStatements { get { throw null; } }
    }

    public partial class CodeTypeConstructor : CodeMemberMethod
    {
    }

    public partial class CodeTypeDeclaration : CodeTypeMember
    {
        public CodeTypeDeclaration() { }

        public CodeTypeDeclaration(string name) { }

        public CodeTypeReferenceCollection BaseTypes { get { throw null; } }

        public bool IsClass { get { throw null; } set { } }

        public bool IsEnum { get { throw null; } set { } }

        public bool IsInterface { get { throw null; } set { } }

        public bool IsPartial { get { throw null; } set { } }

        public bool IsStruct { get { throw null; } set { } }

        public CodeTypeMemberCollection Members { get { throw null; } }

        public Reflection.TypeAttributes TypeAttributes { get { throw null; } set { } }

        public CodeTypeParameterCollection TypeParameters { get { throw null; } }

        public event EventHandler PopulateBaseTypes { add { } remove { } }

        public event EventHandler PopulateMembers { add { } remove { } }
    }

    public partial class CodeTypeDeclarationCollection : Collections.CollectionBase
    {
        public CodeTypeDeclarationCollection() { }

        public CodeTypeDeclarationCollection(CodeTypeDeclaration[] value) { }

        public CodeTypeDeclarationCollection(CodeTypeDeclarationCollection value) { }

        public CodeTypeDeclaration this[int index] { get { throw null; } set { } }

        public int Add(CodeTypeDeclaration value) { throw null; }

        public void AddRange(CodeTypeDeclaration[] value) { }

        public void AddRange(CodeTypeDeclarationCollection value) { }

        public bool Contains(CodeTypeDeclaration value) { throw null; }

        public void CopyTo(CodeTypeDeclaration[] array, int index) { }

        public int IndexOf(CodeTypeDeclaration value) { throw null; }

        public void Insert(int index, CodeTypeDeclaration value) { }

        public void Remove(CodeTypeDeclaration value) { }
    }

    public partial class CodeTypeDelegate : CodeTypeDeclaration
    {
        public CodeTypeDelegate() { }

        public CodeTypeDelegate(string name) { }

        public CodeParameterDeclarationExpressionCollection Parameters { get { throw null; } }

        public CodeTypeReference ReturnType { get { throw null; } set { } }
    }

    public partial class CodeTypeMember : CodeObject
    {
        public MemberAttributes Attributes { get { throw null; } set { } }

        public CodeCommentStatementCollection Comments { get { throw null; } }

        public CodeAttributeDeclarationCollection CustomAttributes { get { throw null; } set { } }

        public CodeDirectiveCollection EndDirectives { get { throw null; } }

        public CodeLinePragma LinePragma { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public CodeDirectiveCollection StartDirectives { get { throw null; } }
    }

    public partial class CodeTypeMemberCollection : Collections.CollectionBase
    {
        public CodeTypeMemberCollection() { }

        public CodeTypeMemberCollection(CodeTypeMember[] value) { }

        public CodeTypeMemberCollection(CodeTypeMemberCollection value) { }

        public CodeTypeMember this[int index] { get { throw null; } set { } }

        public int Add(CodeTypeMember value) { throw null; }

        public void AddRange(CodeTypeMember[] value) { }

        public void AddRange(CodeTypeMemberCollection value) { }

        public bool Contains(CodeTypeMember value) { throw null; }

        public void CopyTo(CodeTypeMember[] array, int index) { }

        public int IndexOf(CodeTypeMember value) { throw null; }

        public void Insert(int index, CodeTypeMember value) { }

        public void Remove(CodeTypeMember value) { }
    }

    public partial class CodeTypeOfExpression : CodeExpression
    {
        public CodeTypeOfExpression() { }

        public CodeTypeOfExpression(CodeTypeReference type) { }

        public CodeTypeOfExpression(string type) { }

        public CodeTypeOfExpression(Type type) { }

        public CodeTypeReference Type { get { throw null; } set { } }
    }

    public partial class CodeTypeParameter : CodeObject
    {
        public CodeTypeParameter() { }

        public CodeTypeParameter(string name) { }

        public CodeTypeReferenceCollection Constraints { get { throw null; } }

        public CodeAttributeDeclarationCollection CustomAttributes { get { throw null; } }

        public bool HasConstructorConstraint { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }
    }

    public partial class CodeTypeParameterCollection : Collections.CollectionBase
    {
        public CodeTypeParameterCollection() { }

        public CodeTypeParameterCollection(CodeTypeParameter[] value) { }

        public CodeTypeParameterCollection(CodeTypeParameterCollection value) { }

        public CodeTypeParameter this[int index] { get { throw null; } set { } }

        public int Add(CodeTypeParameter value) { throw null; }

        public void Add(string value) { }

        public void AddRange(CodeTypeParameter[] value) { }

        public void AddRange(CodeTypeParameterCollection value) { }

        public bool Contains(CodeTypeParameter value) { throw null; }

        public void CopyTo(CodeTypeParameter[] array, int index) { }

        public int IndexOf(CodeTypeParameter value) { throw null; }

        public void Insert(int index, CodeTypeParameter value) { }

        public void Remove(CodeTypeParameter value) { }
    }

    public partial class CodeTypeReference : CodeObject
    {
        public CodeTypeReference() { }

        public CodeTypeReference(CodeTypeParameter typeParameter) { }

        public CodeTypeReference(CodeTypeReference arrayType, int rank) { }

        public CodeTypeReference(string typeName, params CodeTypeReference[] typeArguments) { }

        public CodeTypeReference(string? typeName, CodeTypeReferenceOptions codeTypeReferenceOption) { }

        public CodeTypeReference(string baseType, int rank) { }

        public CodeTypeReference(string? typeName) { }

        public CodeTypeReference(Type type, CodeTypeReferenceOptions codeTypeReferenceOption) { }

        public CodeTypeReference(Type type) { }

        public CodeTypeReference? ArrayElementType { get { throw null; } set { } }

        public int ArrayRank { get { throw null; } set { } }

        public string BaseType { get { throw null; } set { } }

        public CodeTypeReferenceOptions Options { get { throw null; } set { } }

        public CodeTypeReferenceCollection TypeArguments { get { throw null; } }
    }

    public partial class CodeTypeReferenceCollection : Collections.CollectionBase
    {
        public CodeTypeReferenceCollection() { }

        public CodeTypeReferenceCollection(CodeTypeReference[] value) { }

        public CodeTypeReferenceCollection(CodeTypeReferenceCollection value) { }

        public CodeTypeReference this[int index] { get { throw null; } set { } }

        public int Add(CodeTypeReference value) { throw null; }

        public void Add(string value) { }

        public void Add(Type value) { }

        public void AddRange(CodeTypeReference[] value) { }

        public void AddRange(CodeTypeReferenceCollection value) { }

        public bool Contains(CodeTypeReference value) { throw null; }

        public void CopyTo(CodeTypeReference[] array, int index) { }

        public int IndexOf(CodeTypeReference value) { throw null; }

        public void Insert(int index, CodeTypeReference value) { }

        public void Remove(CodeTypeReference value) { }
    }

    public partial class CodeTypeReferenceExpression : CodeExpression
    {
        public CodeTypeReferenceExpression() { }

        public CodeTypeReferenceExpression(CodeTypeReference type) { }

        public CodeTypeReferenceExpression(string type) { }

        public CodeTypeReferenceExpression(Type type) { }

        public CodeTypeReference Type { get { throw null; } set { } }
    }

    [Flags]
    public enum CodeTypeReferenceOptions
    {
        GlobalReference = 1,
        GenericTypeParameter = 2
    }

    public partial class CodeVariableDeclarationStatement : CodeStatement
    {
        public CodeVariableDeclarationStatement() { }

        public CodeVariableDeclarationStatement(CodeTypeReference type, string name, CodeExpression initExpression) { }

        public CodeVariableDeclarationStatement(CodeTypeReference type, string name) { }

        public CodeVariableDeclarationStatement(string type, string name, CodeExpression initExpression) { }

        public CodeVariableDeclarationStatement(string type, string name) { }

        public CodeVariableDeclarationStatement(Type type, string name, CodeExpression initExpression) { }

        public CodeVariableDeclarationStatement(Type type, string name) { }

        public CodeExpression InitExpression { get { throw null; } set { } }

        public string Name { get { throw null; } set { } }

        public CodeTypeReference Type { get { throw null; } set { } }
    }

    public partial class CodeVariableReferenceExpression : CodeExpression
    {
        public CodeVariableReferenceExpression() { }

        public CodeVariableReferenceExpression(string variableName) { }

        public string VariableName { get { throw null; } set { } }
    }

    public enum FieldDirection
    {
        In = 0,
        Out = 1,
        Ref = 2
    }

    public enum MemberAttributes
    {
        Abstract = 1,
        Final = 2,
        Static = 3,
        Override = 4,
        Const = 5,
        ScopeMask = 15,
        New = 16,
        VTableMask = 240,
        Overloaded = 256,
        Assembly = 4096,
        FamilyAndAssembly = 8192,
        Family = 12288,
        FamilyOrAssembly = 16384,
        Private = 20480,
        Public = 24576,
        AccessMask = 61440
    }
}

namespace System.CodeDom.Compiler
{
    public abstract partial class CodeCompiler : CodeGenerator, ICodeCompiler
    {
        protected abstract string CompilerName { get; }
        protected abstract string FileExtension { get; }

        protected abstract string CmdArgsFromParameters(CompilerParameters options);
        protected virtual CompilerResults FromDom(CompilerParameters options, CodeCompileUnit e) { throw null; }

        protected virtual CompilerResults FromDomBatch(CompilerParameters options, CodeCompileUnit[] ea) { throw null; }

        protected virtual CompilerResults FromFile(CompilerParameters options, string fileName) { throw null; }

        protected virtual CompilerResults FromFileBatch(CompilerParameters options, string[] fileNames) { throw null; }

        protected virtual CompilerResults FromSource(CompilerParameters options, string source) { throw null; }

        protected virtual CompilerResults FromSourceBatch(CompilerParameters options, string[] sources) { throw null; }

        protected virtual string GetResponseFileCmdArgs(CompilerParameters options, string cmdArgs) { throw null; }

        protected static string JoinStringArray(string[] sa, string separator) { throw null; }

        protected abstract void ProcessCompilerOutputLine(CompilerResults results, string line);
        CompilerResults ICodeCompiler.CompileAssemblyFromDom(CompilerParameters options, CodeCompileUnit e) { throw null; }

        CompilerResults ICodeCompiler.CompileAssemblyFromDomBatch(CompilerParameters options, CodeCompileUnit[] ea) { throw null; }

        CompilerResults ICodeCompiler.CompileAssemblyFromFile(CompilerParameters options, string fileName) { throw null; }

        CompilerResults ICodeCompiler.CompileAssemblyFromFileBatch(CompilerParameters options, string[] fileNames) { throw null; }

        CompilerResults ICodeCompiler.CompileAssemblyFromSource(CompilerParameters options, string source) { throw null; }

        CompilerResults ICodeCompiler.CompileAssemblyFromSourceBatch(CompilerParameters options, string[] sources) { throw null; }
    }

    [ComponentModel.ToolboxItem(false)]
    public abstract partial class CodeDomProvider : ComponentModel.Component
    {
        public virtual string FileExtension { get { throw null; } }

        public virtual LanguageOptions LanguageOptions { get { throw null; } }

        public virtual CompilerResults CompileAssemblyFromDom(CompilerParameters options, params CodeCompileUnit[] compilationUnits) { throw null; }

        public virtual CompilerResults CompileAssemblyFromFile(CompilerParameters options, params string[] fileNames) { throw null; }

        public virtual CompilerResults CompileAssemblyFromSource(CompilerParameters options, params string[] sources) { throw null; }

        [Obsolete("ICodeCompiler has been deprecated. Use the methods directly on the CodeDomProvider class instead. Classes inheriting from CodeDomProvider must still implement this interface, and should suppress this warning or also mark this method as obsolete.")]
        public abstract ICodeCompiler CreateCompiler();
        public virtual string CreateEscapedIdentifier(string value) { throw null; }

        [Obsolete("ICodeGenerator has been deprecated. Use the methods directly on the CodeDomProvider class instead. Classes inheriting from CodeDomProvider must still implement this interface, and should suppress this warning or also mark this method as obsolete.")]
        public abstract ICodeGenerator CreateGenerator();
        public virtual ICodeGenerator CreateGenerator(IO.TextWriter output) { throw null; }

        public virtual ICodeGenerator CreateGenerator(string fileName) { throw null; }

        [Obsolete("ICodeParser has been deprecated. Use the methods directly on the CodeDomProvider class instead. Classes inheriting from CodeDomProvider must still implement this interface, and should suppress this warning or also mark this method as obsolete.")]
        public virtual ICodeParser CreateParser() { throw null; }

        public static CodeDomProvider CreateProvider(string language, Collections.Generic.IDictionary<string, string> providerOptions) { throw null; }

        public static CodeDomProvider CreateProvider(string language) { throw null; }

        public virtual string CreateValidIdentifier(string value) { throw null; }

        public virtual void GenerateCodeFromCompileUnit(CodeCompileUnit compileUnit, IO.TextWriter writer, CodeGeneratorOptions options) { }

        public virtual void GenerateCodeFromExpression(CodeExpression expression, IO.TextWriter writer, CodeGeneratorOptions options) { }

        public virtual void GenerateCodeFromMember(CodeTypeMember member, IO.TextWriter writer, CodeGeneratorOptions options) { }

        public virtual void GenerateCodeFromNamespace(CodeNamespace codeNamespace, IO.TextWriter writer, CodeGeneratorOptions options) { }

        public virtual void GenerateCodeFromStatement(CodeStatement statement, IO.TextWriter writer, CodeGeneratorOptions options) { }

        public virtual void GenerateCodeFromType(CodeTypeDeclaration codeType, IO.TextWriter writer, CodeGeneratorOptions options) { }

        public static CompilerInfo[] GetAllCompilerInfo() { throw null; }

        public static CompilerInfo GetCompilerInfo(string language) { throw null; }

        public virtual ComponentModel.TypeConverter GetConverter(Type type) { throw null; }

        public static string GetLanguageFromExtension(string extension) { throw null; }

        public virtual string GetTypeOutput(CodeTypeReference type) { throw null; }

        public static bool IsDefinedExtension(string extension) { throw null; }

        public static bool IsDefinedLanguage(string language) { throw null; }

        public virtual bool IsValidIdentifier(string value) { throw null; }

        public virtual CodeCompileUnit Parse(IO.TextReader codeStream) { throw null; }

        public virtual bool Supports(GeneratorSupport generatorSupport) { throw null; }
    }

    public abstract partial class CodeGenerator : ICodeGenerator
    {
        protected CodeTypeDeclaration CurrentClass { get { throw null; } }

        protected CodeTypeMember CurrentMember { get { throw null; } }

        protected string CurrentMemberName { get { throw null; } }

        protected string CurrentTypeName { get { throw null; } }

        protected int Indent { get { throw null; } set { } }

        protected bool IsCurrentClass { get { throw null; } }

        protected bool IsCurrentDelegate { get { throw null; } }

        protected bool IsCurrentEnum { get { throw null; } }

        protected bool IsCurrentInterface { get { throw null; } }

        protected bool IsCurrentStruct { get { throw null; } }

        protected abstract string NullToken { get; }

        protected CodeGeneratorOptions Options { get { throw null; } }

        protected IO.TextWriter Output { get { throw null; } }

        protected virtual void ContinueOnNewLine(string st) { }

        protected abstract string CreateEscapedIdentifier(string value);
        protected abstract string CreateValidIdentifier(string value);
        protected abstract void GenerateArgumentReferenceExpression(CodeArgumentReferenceExpression e);
        protected abstract void GenerateArrayCreateExpression(CodeArrayCreateExpression e);
        protected abstract void GenerateArrayIndexerExpression(CodeArrayIndexerExpression e);
        protected abstract void GenerateAssignStatement(CodeAssignStatement e);
        protected abstract void GenerateAttachEventStatement(CodeAttachEventStatement e);
        protected abstract void GenerateAttributeDeclarationsEnd(CodeAttributeDeclarationCollection attributes);
        protected abstract void GenerateAttributeDeclarationsStart(CodeAttributeDeclarationCollection attributes);
        protected abstract void GenerateBaseReferenceExpression(CodeBaseReferenceExpression e);
        protected virtual void GenerateBinaryOperatorExpression(CodeBinaryOperatorExpression e) { }

        protected abstract void GenerateCastExpression(CodeCastExpression e);
        public virtual void GenerateCodeFromMember(CodeTypeMember member, IO.TextWriter writer, CodeGeneratorOptions options) { }

        protected abstract void GenerateComment(CodeComment e);
        protected virtual void GenerateCommentStatement(CodeCommentStatement e) { }

        protected virtual void GenerateCommentStatements(CodeCommentStatementCollection e) { }

        protected virtual void GenerateCompileUnit(CodeCompileUnit e) { }

        protected virtual void GenerateCompileUnitEnd(CodeCompileUnit e) { }

        protected virtual void GenerateCompileUnitStart(CodeCompileUnit e) { }

        protected abstract void GenerateConditionStatement(CodeConditionStatement e);
        protected abstract void GenerateConstructor(CodeConstructor e, CodeTypeDeclaration c);
        protected virtual void GenerateDecimalValue(decimal d) { }

        protected virtual void GenerateDefaultValueExpression(CodeDefaultValueExpression e) { }

        protected abstract void GenerateDelegateCreateExpression(CodeDelegateCreateExpression e);
        protected abstract void GenerateDelegateInvokeExpression(CodeDelegateInvokeExpression e);
        protected virtual void GenerateDirectionExpression(CodeDirectionExpression e) { }

        protected virtual void GenerateDirectives(CodeDirectiveCollection directives) { }

        protected virtual void GenerateDoubleValue(double d) { }

        protected abstract void GenerateEntryPointMethod(CodeEntryPointMethod e, CodeTypeDeclaration c);
        protected abstract void GenerateEvent(CodeMemberEvent e, CodeTypeDeclaration c);
        protected abstract void GenerateEventReferenceExpression(CodeEventReferenceExpression e);
        protected void GenerateExpression(CodeExpression e) { }

        protected abstract void GenerateExpressionStatement(CodeExpressionStatement e);
        protected abstract void GenerateField(CodeMemberField e);
        protected abstract void GenerateFieldReferenceExpression(CodeFieldReferenceExpression e);
        protected abstract void GenerateGotoStatement(CodeGotoStatement e);
        protected abstract void GenerateIndexerExpression(CodeIndexerExpression e);
        protected abstract void GenerateIterationStatement(CodeIterationStatement e);
        protected abstract void GenerateLabeledStatement(CodeLabeledStatement e);
        protected abstract void GenerateLinePragmaEnd(CodeLinePragma e);
        protected abstract void GenerateLinePragmaStart(CodeLinePragma e);
        protected abstract void GenerateMethod(CodeMemberMethod e, CodeTypeDeclaration c);
        protected abstract void GenerateMethodInvokeExpression(CodeMethodInvokeExpression e);
        protected abstract void GenerateMethodReferenceExpression(CodeMethodReferenceExpression e);
        protected abstract void GenerateMethodReturnStatement(CodeMethodReturnStatement e);
        protected virtual void GenerateNamespace(CodeNamespace e) { }

        protected abstract void GenerateNamespaceEnd(CodeNamespace e);
        protected abstract void GenerateNamespaceImport(CodeNamespaceImport e);
        protected void GenerateNamespaceImports(CodeNamespace e) { }

        protected void GenerateNamespaces(CodeCompileUnit e) { }

        protected abstract void GenerateNamespaceStart(CodeNamespace e);
        protected abstract void GenerateObjectCreateExpression(CodeObjectCreateExpression e);
        protected virtual void GenerateParameterDeclarationExpression(CodeParameterDeclarationExpression e) { }

        protected virtual void GeneratePrimitiveExpression(CodePrimitiveExpression e) { }

        protected abstract void GenerateProperty(CodeMemberProperty e, CodeTypeDeclaration c);
        protected abstract void GeneratePropertyReferenceExpression(CodePropertyReferenceExpression e);
        protected abstract void GeneratePropertySetValueReferenceExpression(CodePropertySetValueReferenceExpression e);
        protected abstract void GenerateRemoveEventStatement(CodeRemoveEventStatement e);
        protected virtual void GenerateSingleFloatValue(float s) { }

        protected virtual void GenerateSnippetCompileUnit(CodeSnippetCompileUnit e) { }

        protected abstract void GenerateSnippetExpression(CodeSnippetExpression e);
        protected abstract void GenerateSnippetMember(CodeSnippetTypeMember e);
        protected virtual void GenerateSnippetStatement(CodeSnippetStatement e) { }

        protected void GenerateStatement(CodeStatement e) { }

        protected void GenerateStatements(CodeStatementCollection stmts) { }

        protected abstract void GenerateThisReferenceExpression(CodeThisReferenceExpression e);
        protected abstract void GenerateThrowExceptionStatement(CodeThrowExceptionStatement e);
        protected abstract void GenerateTryCatchFinallyStatement(CodeTryCatchFinallyStatement e);
        protected abstract void GenerateTypeConstructor(CodeTypeConstructor e);
        protected abstract void GenerateTypeEnd(CodeTypeDeclaration e);
        protected virtual void GenerateTypeOfExpression(CodeTypeOfExpression e) { }

        protected virtual void GenerateTypeReferenceExpression(CodeTypeReferenceExpression e) { }

        protected void GenerateTypes(CodeNamespace e) { }

        protected abstract void GenerateTypeStart(CodeTypeDeclaration e);
        protected abstract void GenerateVariableDeclarationStatement(CodeVariableDeclarationStatement e);
        protected abstract void GenerateVariableReferenceExpression(CodeVariableReferenceExpression e);
        protected abstract string GetTypeOutput(CodeTypeReference value);
        protected abstract bool IsValidIdentifier(string value);
        public static bool IsValidLanguageIndependentIdentifier(string value) { throw null; }

        protected virtual void OutputAttributeArgument(CodeAttributeArgument arg) { }

        protected virtual void OutputAttributeDeclarations(CodeAttributeDeclarationCollection attributes) { }

        protected virtual void OutputDirection(FieldDirection dir) { }

        protected virtual void OutputExpressionList(CodeExpressionCollection expressions, bool newlineBetweenItems) { }

        protected virtual void OutputExpressionList(CodeExpressionCollection expressions) { }

        protected virtual void OutputFieldScopeModifier(MemberAttributes attributes) { }

        protected virtual void OutputIdentifier(string ident) { }

        protected virtual void OutputMemberAccessModifier(MemberAttributes attributes) { }

        protected virtual void OutputMemberScopeModifier(MemberAttributes attributes) { }

        protected virtual void OutputOperator(CodeBinaryOperatorType op) { }

        protected virtual void OutputParameters(CodeParameterDeclarationExpressionCollection parameters) { }

        protected abstract void OutputType(CodeTypeReference typeRef);
        protected virtual void OutputTypeAttributes(Reflection.TypeAttributes attributes, bool isStruct, bool isEnum) { }

        protected virtual void OutputTypeNamePair(CodeTypeReference typeRef, string name) { }

        protected abstract string QuoteSnippetString(string value);
        protected abstract bool Supports(GeneratorSupport support);
        string ICodeGenerator.CreateEscapedIdentifier(string value) { throw null; }

        string ICodeGenerator.CreateValidIdentifier(string value) { throw null; }

        void ICodeGenerator.GenerateCodeFromCompileUnit(CodeCompileUnit e, IO.TextWriter w, CodeGeneratorOptions o) { }

        void ICodeGenerator.GenerateCodeFromExpression(CodeExpression e, IO.TextWriter w, CodeGeneratorOptions o) { }

        void ICodeGenerator.GenerateCodeFromNamespace(CodeNamespace e, IO.TextWriter w, CodeGeneratorOptions o) { }

        void ICodeGenerator.GenerateCodeFromStatement(CodeStatement e, IO.TextWriter w, CodeGeneratorOptions o) { }

        void ICodeGenerator.GenerateCodeFromType(CodeTypeDeclaration e, IO.TextWriter w, CodeGeneratorOptions o) { }

        string ICodeGenerator.GetTypeOutput(CodeTypeReference type) { throw null; }

        bool ICodeGenerator.IsValidIdentifier(string value) { throw null; }

        bool ICodeGenerator.Supports(GeneratorSupport support) { throw null; }

        void ICodeGenerator.ValidateIdentifier(string value) { }

        protected virtual void ValidateIdentifier(string value) { }

        public static void ValidateIdentifiers(CodeObject e) { }
    }

    public partial class CodeGeneratorOptions
    {
        public bool BlankLinesBetweenMembers { get { throw null; } set { } }

        public string BracingStyle { get { throw null; } set { } }

        public bool ElseOnClosing { get { throw null; } set { } }

        public string IndentString { get { throw null; } set { } }

        public object this[string index] { get { throw null; } set { } }

        public bool VerbatimOrder { get { throw null; } set { } }
    }

    public abstract partial class CodeParser : ICodeParser
    {
        public abstract CodeCompileUnit Parse(IO.TextReader codeStream);
    }

    public partial class CompilerError
    {
        public CompilerError() { }

        public CompilerError(string fileName, int line, int column, string errorNumber, string errorText) { }

        public int Column { get { throw null; } set { } }

        public string ErrorNumber { get { throw null; } set { } }

        public string ErrorText { get { throw null; } set { } }

        public string FileName { get { throw null; } set { } }

        public bool IsWarning { get { throw null; } set { } }

        public int Line { get { throw null; } set { } }

        public override string ToString() { throw null; }
    }

    public partial class CompilerErrorCollection : Collections.CollectionBase
    {
        public CompilerErrorCollection() { }

        public CompilerErrorCollection(CompilerError[] value) { }

        public CompilerErrorCollection(CompilerErrorCollection value) { }

        public bool HasErrors { get { throw null; } }

        public bool HasWarnings { get { throw null; } }

        public CompilerError this[int index] { get { throw null; } set { } }

        public int Add(CompilerError value) { throw null; }

        public void AddRange(CompilerError[] value) { }

        public void AddRange(CompilerErrorCollection value) { }

        public bool Contains(CompilerError value) { throw null; }

        public void CopyTo(CompilerError[] array, int index) { }

        public int IndexOf(CompilerError value) { throw null; }

        public void Insert(int index, CompilerError value) { }

        public void Remove(CompilerError value) { }
    }

    public sealed partial class CompilerInfo
    {
        internal CompilerInfo() { }

        public Type CodeDomProviderType { get { throw null; } }

        public bool IsCodeDomProviderTypeValid { get { throw null; } }

        public CompilerParameters CreateDefaultCompilerParameters() { throw null; }

        public CodeDomProvider CreateProvider() { throw null; }

        public CodeDomProvider CreateProvider(Collections.Generic.IDictionary<string, string> providerOptions) { throw null; }

        public override bool Equals(object o) { throw null; }

        public string[] GetExtensions() { throw null; }

        public override int GetHashCode() { throw null; }

        public string[] GetLanguages() { throw null; }
    }

    public partial class CompilerParameters
    {
        public CompilerParameters() { }

        public CompilerParameters(string[] assemblyNames, string outputName, bool includeDebugInformation) { }

        public CompilerParameters(string[] assemblyNames, string outputName) { }

        public CompilerParameters(string[] assemblyNames) { }

        public string CompilerOptions { get { throw null; } set { } }

        public string CoreAssemblyFileName { get { throw null; } set { } }

        public Collections.Specialized.StringCollection EmbeddedResources { get { throw null; } }

        public bool GenerateExecutable { get { throw null; } set { } }

        public bool GenerateInMemory { get { throw null; } set { } }

        public bool IncludeDebugInformation { get { throw null; } set { } }

        public Collections.Specialized.StringCollection LinkedResources { get { throw null; } }

        public string MainClass { get { throw null; } set { } }

        public string OutputAssembly { get { throw null; } set { } }

        public Collections.Specialized.StringCollection ReferencedAssemblies { get { throw null; } }

        public TempFileCollection TempFiles { get { throw null; } set { } }

        public bool TreatWarningsAsErrors { get { throw null; } set { } }

        public nint UserToken { get { throw null; } set { } }

        public int WarningLevel { get { throw null; } set { } }

        public string Win32Resource { get { throw null; } set { } }
    }

    public partial class CompilerResults
    {
        public CompilerResults(TempFileCollection tempFiles) { }

        public Reflection.Assembly CompiledAssembly { get { throw null; } set { } }

        public CompilerErrorCollection Errors { get { throw null; } }

        public int NativeCompilerReturnValue { get { throw null; } set { } }

        public Collections.Specialized.StringCollection Output { get { throw null; } }

        public string PathToAssembly { get { throw null; } set { } }

        public TempFileCollection TempFiles { get { throw null; } set { } }
    }

    public static partial class Executor
    {
        public static void ExecWait(string cmd, TempFileCollection tempFiles) { }

        public static int ExecWaitWithCapture(nint userToken, string cmd, TempFileCollection tempFiles, ref string outputName, ref string errorName) { throw null; }

        public static int ExecWaitWithCapture(nint userToken, string cmd, string currentDir, TempFileCollection tempFiles, ref string outputName, ref string errorName) { throw null; }

        public static int ExecWaitWithCapture(string cmd, TempFileCollection tempFiles, ref string outputName, ref string errorName) { throw null; }

        public static int ExecWaitWithCapture(string cmd, string currentDir, TempFileCollection tempFiles, ref string outputName, ref string errorName) { throw null; }
    }

    [Flags]
    public enum GeneratorSupport
    {
        ArraysOfArrays = 1,
        EntryPointMethod = 2,
        GotoStatements = 4,
        MultidimensionalArrays = 8,
        StaticConstructors = 16,
        TryCatchStatements = 32,
        ReturnTypeAttributes = 64,
        DeclareValueTypes = 128,
        DeclareEnums = 256,
        DeclareDelegates = 512,
        DeclareInterfaces = 1024,
        DeclareEvents = 2048,
        AssemblyAttributes = 4096,
        ParameterAttributes = 8192,
        ReferenceParameters = 16384,
        ChainedConstructorArguments = 32768,
        NestedTypes = 65536,
        MultipleInterfaceMembers = 131072,
        PublicStaticMembers = 262144,
        ComplexExpressions = 524288,
        Win32Resources = 1048576,
        Resources = 2097152,
        PartialTypes = 4194304,
        GenericTypeReference = 8388608,
        GenericTypeDeclaration = 16777216,
        DeclareIndexerProperties = 33554432
    }

    public partial interface ICodeCompiler
    {
        CompilerResults CompileAssemblyFromDom(CompilerParameters options, CodeCompileUnit compilationUnit);
        CompilerResults CompileAssemblyFromDomBatch(CompilerParameters options, CodeCompileUnit[] compilationUnits);
        CompilerResults CompileAssemblyFromFile(CompilerParameters options, string fileName);
        CompilerResults CompileAssemblyFromFileBatch(CompilerParameters options, string[] fileNames);
        CompilerResults CompileAssemblyFromSource(CompilerParameters options, string source);
        CompilerResults CompileAssemblyFromSourceBatch(CompilerParameters options, string[] sources);
    }

    public partial interface ICodeGenerator
    {
        string CreateEscapedIdentifier(string value);
        string CreateValidIdentifier(string value);
        void GenerateCodeFromCompileUnit(CodeCompileUnit e, IO.TextWriter w, CodeGeneratorOptions o);
        void GenerateCodeFromExpression(CodeExpression e, IO.TextWriter w, CodeGeneratorOptions o);
        void GenerateCodeFromNamespace(CodeNamespace e, IO.TextWriter w, CodeGeneratorOptions o);
        void GenerateCodeFromStatement(CodeStatement e, IO.TextWriter w, CodeGeneratorOptions o);
        void GenerateCodeFromType(CodeTypeDeclaration e, IO.TextWriter w, CodeGeneratorOptions o);
        string GetTypeOutput(CodeTypeReference type);
        bool IsValidIdentifier(string value);
        bool Supports(GeneratorSupport supports);
        void ValidateIdentifier(string value);
    }

    public partial interface ICodeParser
    {
        CodeCompileUnit Parse(IO.TextReader codeStream);
    }

    [Flags]
    public enum LanguageOptions
    {
        None = 0,
        CaseInsensitive = 1
    }

    public partial class TempFileCollection : Collections.ICollection, Collections.IEnumerable, IDisposable
    {
        public TempFileCollection() { }

        public TempFileCollection(string tempDir, bool keepFiles) { }

        public TempFileCollection(string tempDir) { }

        public string BasePath { get { throw null; } }

        public int Count { get { throw null; } }

        public bool KeepFiles { get { throw null; } set { } }

        int Collections.ICollection.Count { get { throw null; } }

        bool Collections.ICollection.IsSynchronized { get { throw null; } }

        object Collections.ICollection.SyncRoot { get { throw null; } }

        public string TempDir { get { throw null; } }

        public string AddExtension(string fileExtension, bool keepFile) { throw null; }

        public string AddExtension(string fileExtension) { throw null; }

        public void AddFile(string fileName, bool keepFile) { }

        public void CopyTo(string[] fileNames, int start) { }

        public void Delete() { }

        protected virtual void Dispose(bool disposing) { }

        ~TempFileCollection() {
        }

        public Collections.IEnumerator GetEnumerator() { throw null; }

        void Collections.ICollection.CopyTo(Array array, int start) { }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator() { throw null; }

        void IDisposable.Dispose() { }
    }
}