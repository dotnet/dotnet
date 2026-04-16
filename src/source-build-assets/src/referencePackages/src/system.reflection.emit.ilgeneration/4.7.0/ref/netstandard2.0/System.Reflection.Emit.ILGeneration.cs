// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection.Emit.ILGeneration")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Reflection.Emit.ILGeneration")]
[assembly: System.Reflection.AssemblyFileVersion("4.700.19.56404")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.1.0+0f7f38c4fd323b26da10cce95f857f77f0f09b48")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Core")]
[assembly: System.Reflection.AssemblyTitle("System.Reflection.Emit.ILGeneration")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Reflection.Emit
{
    public partial class CustomAttributeBuilder
    {
        public CustomAttributeBuilder(ConstructorInfo con, object?[] constructorArgs, FieldInfo[] namedFields, object[] fieldValues) { }
        public CustomAttributeBuilder(ConstructorInfo con, object?[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues, FieldInfo[] namedFields, object[] fieldValues) { }
        public CustomAttributeBuilder(ConstructorInfo con, object?[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues) { }
        public CustomAttributeBuilder(ConstructorInfo con, object?[] constructorArgs) { }
    }
    public partial class ILGenerator
    {
        internal ILGenerator() { }
        public virtual int ILOffset { get { throw null; } }

        public virtual void BeginCatchBlock(Type exceptionType) { }
        public virtual void BeginExceptFilterBlock() { }
        public virtual Label BeginExceptionBlock() { throw null; }
        public virtual void BeginFaultBlock() { }
        public virtual void BeginFinallyBlock() { }
        public virtual void BeginScope() { }
        public virtual LocalBuilder DeclareLocal(Type localType, bool pinned) { throw null; }
        public virtual LocalBuilder DeclareLocal(Type localType) { throw null; }
        public virtual Label DefineLabel() { throw null; }
        public virtual void Emit(OpCode opcode, byte arg) { }
        public virtual void Emit(OpCode opcode, double arg) { }
        public virtual void Emit(OpCode opcode, short arg) { }
        public virtual void Emit(OpCode opcode, int arg) { }
        public virtual void Emit(OpCode opcode, long arg) { }
        public virtual void Emit(OpCode opcode, ConstructorInfo con) { }
        public virtual void Emit(OpCode opcode, Label label) { }
        public virtual void Emit(OpCode opcode, Label[] labels) { }
        public virtual void Emit(OpCode opcode, LocalBuilder local) { }
        public virtual void Emit(OpCode opcode, SignatureHelper signature) { }
        public virtual void Emit(OpCode opcode, FieldInfo field) { }
        public virtual void Emit(OpCode opcode, MethodInfo meth) { }
        [CLSCompliant(false)]
        public void Emit(OpCode opcode, sbyte arg) { }
        public virtual void Emit(OpCode opcode, float arg) { }
        public virtual void Emit(OpCode opcode, string str) { }
        public virtual void Emit(OpCode opcode, Type cls) { }
        public virtual void Emit(OpCode opcode) { }
        public virtual void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[]? optionalParameterTypes) { }
        public virtual void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes, Type[]? optionalParameterTypes) { }
        public virtual void EmitWriteLine(LocalBuilder localBuilder) { }
        public virtual void EmitWriteLine(FieldInfo fld) { }
        public virtual void EmitWriteLine(string value) { }
        public virtual void EndExceptionBlock() { }
        public virtual void EndScope() { }
        public virtual void MarkLabel(Label loc) { }
        public virtual void ThrowException(Type excType) { }
        public virtual void UsingNamespace(string usingNamespace) { }
    }
    public readonly partial struct Label
    {
        private readonly int _dummyPrimitive;
        public override readonly bool Equals(object? obj) { throw null; }
        public readonly bool Equals(Label obj) { throw null; }
        public override readonly int GetHashCode() { throw null; }
        public static bool operator ==(Label a, Label b) { throw null; }
        public static bool operator !=(Label a, Label b) { throw null; }
    }

    public sealed partial class LocalBuilder : LocalVariableInfo
    {
        internal LocalBuilder() { }
        public override bool IsPinned { get { throw null; } }
        public override int LocalIndex { get { throw null; } }
        public override Type LocalType { get { throw null; } }
    }

    public partial class ParameterBuilder
    {
        internal ParameterBuilder() { }
        public virtual int Attributes { get { throw null; } }
        public bool IsIn { get { throw null; } }
        public bool IsOptional { get { throw null; } }
        public bool IsOut { get { throw null; } }
        public virtual string? Name { get { throw null; } }
        public virtual int Position { get { throw null; } }

        public virtual void SetConstant(object? defaultValue) { }
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
    }
    public sealed partial class SignatureHelper
    {
        internal SignatureHelper() { }
        public void AddArgument(Type argument, bool pinned) { }
        public void AddArgument(Type argument, Type[]? requiredCustomModifiers, Type[]? optionalCustomModifiers) { }
        public void AddArgument(Type clsArgument) { }
        public void AddArguments(Type[]? arguments, Type[][]? requiredCustomModifiers, Type[][]? optionalCustomModifiers) { }
        public void AddSentinel() { }
        public override bool Equals(object? obj) { throw null; }
        public static SignatureHelper GetFieldSigHelper(Module? mod) { throw null; }
        public override int GetHashCode() { throw null; }
        public static SignatureHelper GetLocalVarSigHelper() { throw null; }
        public static SignatureHelper GetLocalVarSigHelper(Module? mod) { throw null; }
        public static SignatureHelper GetMethodSigHelper(CallingConventions callingConvention, Type? returnType) { throw null; }
        public static SignatureHelper GetMethodSigHelper(Module? mod, CallingConventions callingConvention, Type? returnType) { throw null; }
        public static SignatureHelper GetMethodSigHelper(Module? mod, Type? returnType, Type[]? parameterTypes) { throw null; }
        public static SignatureHelper GetPropertySigHelper(Module? mod, CallingConventions callingConvention, Type? returnType, Type[]? requiredReturnTypeCustomModifiers, Type[]? optionalReturnTypeCustomModifiers, Type[]? parameterTypes, Type[][]? requiredParameterTypeCustomModifiers, Type[][]? optionalParameterTypeCustomModifiers) { throw null; }
        public static SignatureHelper GetPropertySigHelper(Module? mod, Type? returnType, Type[]? requiredReturnTypeCustomModifiers, Type[]? optionalReturnTypeCustomModifiers, Type[]? parameterTypes, Type[][]? requiredParameterTypeCustomModifiers, Type[][]? optionalParameterTypeCustomModifiers) { throw null; }
        public static SignatureHelper GetPropertySigHelper(Module? mod, Type? returnType, Type[]? parameterTypes) { throw null; }
        public byte[] GetSignature() { throw null; }
        public override string ToString() { throw null; }
    }
}