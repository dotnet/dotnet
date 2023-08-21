// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Reflection.AssemblyProduct("xUnit.net Testing Framework")]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Runtime.Versioning.TargetFramework(".NETPortable,Version=v4.5,Profile=Profile259", FrameworkDisplayName = ".NET Portable Subset")]
[assembly: System.Reflection.AssemblyTitle("xUnit.net Abstractions (PCL)")]
[assembly: System.Reflection.AssemblyCompany("Outercurve Foundation")]
[assembly: System.Reflection.AssemblyCopyright("Copyright (C) Outercurve Foundation")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Reflection.AssemblyVersionAttribute("2.0.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Xunit.Abstractions
{
    public partial interface IAfterTestFinished : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
        string AttributeName { get; }
    }

    public partial interface IAfterTestStarting : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
        string AttributeName { get; }
    }

    public partial interface IAssemblyInfo
    {
        string AssemblyPath { get; }

        string Name { get; }

        System.Collections.Generic.IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName);
        ITypeInfo GetType(string typeName);
        System.Collections.Generic.IEnumerable<ITypeInfo> GetTypes(bool includePrivateTypes);
    }

    public partial interface IAttributeInfo
    {
        System.Collections.Generic.IEnumerable<object> GetConstructorArguments();
        System.Collections.Generic.IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName);
        TValue GetNamedArgument<TValue>(string argumentName);
    }

    public partial interface IBeforeTestFinished : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
        string AttributeName { get; }
    }

    public partial interface IBeforeTestStarting : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
        string AttributeName { get; }
    }

    public partial interface IDiagnosticMessage : IMessageSinkMessage
    {
        string Message { get; }
    }

    public partial interface IDiscoveryCompleteMessage : IMessageSinkMessage
    {
    }

    public partial interface IErrorMessage : IFailureInformation, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface IExecutionMessage : IMessageSinkMessage
    {
        System.Collections.Generic.IEnumerable<ITestCase> TestCases { get; }
    }

    public partial interface IFailureInformation
    {
        int[] ExceptionParentIndices { get; }

        string[] ExceptionTypes { get; }

        string[] Messages { get; }

        string[] StackTraces { get; }
    }

    public partial interface IFinishedMessage : IMessageSinkMessage
    {
        decimal ExecutionTime { get; }

        int TestsFailed { get; }

        int TestsRun { get; }

        int TestsSkipped { get; }
    }

    public partial interface IMessageSink
    {
        bool OnMessage(IMessageSinkMessage message);
    }

    public partial interface IMessageSinkMessage
    {
    }

    public partial interface IMethodInfo
    {
        bool IsAbstract { get; }

        bool IsGenericMethodDefinition { get; }

        bool IsPublic { get; }

        bool IsStatic { get; }

        string Name { get; }

        ITypeInfo ReturnType { get; }

        ITypeInfo Type { get; }

        System.Collections.Generic.IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName);
        System.Collections.Generic.IEnumerable<ITypeInfo> GetGenericArguments();
        System.Collections.Generic.IEnumerable<IParameterInfo> GetParameters();
        IMethodInfo MakeGenericMethod(params ITypeInfo[] typeArguments);
    }

    public partial interface IParameterInfo
    {
        string Name { get; }

        ITypeInfo ParameterType { get; }
    }

    public partial interface IReflectionAssemblyInfo : IAssemblyInfo
    {
        System.Reflection.Assembly Assembly { get; }
    }

    public partial interface IReflectionAttributeInfo : IAttributeInfo
    {
        System.Attribute Attribute { get; }
    }

    public partial interface IReflectionMethodInfo : IMethodInfo
    {
        System.Reflection.MethodInfo MethodInfo { get; }
    }

    public partial interface IReflectionParameterInfo : IParameterInfo
    {
        System.Reflection.ParameterInfo ParameterInfo { get; }
    }

    public partial interface IReflectionTypeInfo : ITypeInfo
    {
        System.Type Type { get; }
    }

    public partial interface ISourceInformation : IXunitSerializable
    {
        string FileName { get; set; }

        int? LineNumber { get; set; }
    }

    public partial interface ISourceInformationProvider : System.IDisposable
    {
        ISourceInformation GetSourceInformation(ITestCase testCase);
    }

    public partial interface ITest
    {
        string DisplayName { get; }

        ITestCase TestCase { get; }
    }

    public partial interface ITestAssembly : IXunitSerializable
    {
        IAssemblyInfo Assembly { get; }

        string ConfigFileName { get; }
    }

    public partial interface ITestAssemblyCleanupFailure : ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage, IFailureInformation
    {
    }

    public partial interface ITestAssemblyFinished : ITestAssemblyMessage, IExecutionMessage, IFinishedMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestAssemblyMessage : IMessageSinkMessage
    {
        ITestAssembly TestAssembly { get; }
    }

    public partial interface ITestAssemblyStarting : ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
        System.DateTime StartTime { get; }

        string TestEnvironment { get; }

        string TestFrameworkDisplayName { get; }
    }

    public partial interface ITestCase : IXunitSerializable
    {
        string DisplayName { get; }

        string SkipReason { get; }

        ISourceInformation SourceInformation { get; set; }

        ITestMethod TestMethod { get; }

        object[] TestMethodArguments { get; }

        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> Traits { get; }

        string UniqueID { get; }
    }

    public partial interface ITestCaseCleanupFailure : ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage, IFailureInformation
    {
    }

    public partial interface ITestCaseDiscoveryMessage : ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestCaseFinished : ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IFinishedMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestCaseMessage : ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IMessageSinkMessage
    {
        ITestCase TestCase { get; }
    }

    public partial interface ITestCaseStarting : ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestClass : IXunitSerializable
    {
        ITypeInfo Class { get; }

        ITestCollection TestCollection { get; }
    }

    public partial interface ITestClassCleanupFailure : ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage, IFailureInformation
    {
    }

    public partial interface ITestClassConstructionFinished : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestClassConstructionStarting : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestClassDisposeFinished : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestClassDisposeStarting : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestClassFinished : ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IFinishedMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestClassMessage : ITestCollectionMessage, ITestAssemblyMessage, IMessageSinkMessage
    {
        ITestClass TestClass { get; }
    }

    public partial interface ITestClassStarting : ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestCleanupFailure : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage, IFailureInformation
    {
    }

    public partial interface ITestCollection : IXunitSerializable
    {
        ITypeInfo CollectionDefinition { get; }

        string DisplayName { get; }

        ITestAssembly TestAssembly { get; }

        System.Guid UniqueID { get; }
    }

    public partial interface ITestCollectionCleanupFailure : ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage, IFailureInformation
    {
    }

    public partial interface ITestCollectionFinished : ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IFinishedMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestCollectionMessage : ITestAssemblyMessage, IMessageSinkMessage
    {
        ITestCollection TestCollection { get; }
    }

    public partial interface ITestCollectionStarting : ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestFailed : ITestResultMessage, ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage, IFailureInformation
    {
    }

    public partial interface ITestFinished : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
        decimal ExecutionTime { get; }

        string Output { get; }
    }

    public partial interface ITestFramework : System.IDisposable
    {
        ISourceInformationProvider SourceInformationProvider { set; }

        ITestFrameworkDiscoverer GetDiscoverer(IAssemblyInfo assembly);
        ITestFrameworkExecutor GetExecutor(System.Reflection.AssemblyName assemblyName);
    }

    public partial interface ITestFrameworkDiscoverer : System.IDisposable
    {
        string TargetFramework { get; }

        string TestFrameworkDisplayName { get; }

        void Find(bool includeSourceInformation, IMessageSink discoveryMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions);
        void Find(string typeName, bool includeSourceInformation, IMessageSink discoveryMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions);
        string Serialize(ITestCase testCase);
    }

    public partial interface ITestFrameworkDiscoveryOptions : ITestFrameworkOptions
    {
    }

    public partial interface ITestFrameworkExecutionOptions : ITestFrameworkOptions
    {
    }

    public partial interface ITestFrameworkExecutor : System.IDisposable
    {
        ITestCase Deserialize(string value);
        void RunAll(IMessageSink executionMessageSink, ITestFrameworkDiscoveryOptions discoveryOptions, ITestFrameworkExecutionOptions executionOptions);
        void RunTests(System.Collections.Generic.IEnumerable<ITestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions);
    }

    public partial interface ITestFrameworkOptions
    {
        TValue GetValue<TValue>(string name);
        void SetValue<TValue>(string name, TValue value);
    }

    public partial interface ITestMessage : ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IMessageSinkMessage
    {
        ITest Test { get; }
    }

    public partial interface ITestMethod : IXunitSerializable
    {
        IMethodInfo Method { get; }

        ITestClass TestClass { get; }
    }

    public partial interface ITestMethodCleanupFailure : ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage, IFailureInformation
    {
    }

    public partial interface ITestMethodFinished : ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IFinishedMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestMethodMessage : ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IMessageSinkMessage
    {
        ITestMethod TestMethod { get; }
    }

    public partial interface ITestMethodStarting : ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestOutput : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
        string Output { get; }
    }

    public partial interface ITestOutputHelper
    {
        void WriteLine(string format, params object[] args);
        void WriteLine(string message);
    }

    public partial interface ITestPassed : ITestResultMessage, ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITestResultMessage : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IMessageSinkMessage
    {
        decimal ExecutionTime { get; }

        string Output { get; }
    }

    public partial interface ITestSkipped : ITestResultMessage, ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
        string Reason { get; }
    }

    public partial interface ITestStarting : ITestMessage, ITestCaseMessage, ITestMethodMessage, ITestClassMessage, ITestCollectionMessage, ITestAssemblyMessage, IExecutionMessage, IMessageSinkMessage
    {
    }

    public partial interface ITypeInfo
    {
        IAssemblyInfo Assembly { get; }

        ITypeInfo BaseType { get; }

        System.Collections.Generic.IEnumerable<ITypeInfo> Interfaces { get; }

        bool IsAbstract { get; }

        bool IsGenericParameter { get; }

        bool IsGenericType { get; }

        bool IsSealed { get; }

        bool IsValueType { get; }

        string Name { get; }

        System.Collections.Generic.IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName);
        System.Collections.Generic.IEnumerable<ITypeInfo> GetGenericArguments();
        IMethodInfo GetMethod(string methodName, bool includePrivateMethod);
        System.Collections.Generic.IEnumerable<IMethodInfo> GetMethods(bool includePrivateMethods);
    }

    public partial interface IXunitSerializable
    {
        void Deserialize(IXunitSerializationInfo info);
        void Serialize(IXunitSerializationInfo info);
    }

    public partial interface IXunitSerializationInfo
    {
        void AddValue(string key, object value, System.Type type = null);
        object GetValue(string key, System.Type type);
        T GetValue<T>(string key);
    }
}