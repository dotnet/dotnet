// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("System.Runtime.InteropServices, PublicKey=002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("System.Reflection.Emit, PublicKey=002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("System.Reflection.Emit.Lightweight, PublicKey=002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293")]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Reflection.Emit")]
[assembly: System.Reflection.AssemblyDescription("System.Reflection.Emit")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection.Emit")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Reflection.Emit
{
    public sealed partial class AssemblyBuilder : Assembly
    {
        internal AssemblyBuilder() { }

        public override Collections.Generic.IEnumerable<TypeInfo> DefinedTypes { get { throw null; } }

        public override string FullName { get { throw null; } }

        public override bool IsDynamic { get { throw null; } }

        public override Module ManifestModule { get { throw null; } }

        public override Collections.Generic.IEnumerable<Module> Modules { get { throw null; } }

        public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Collections.Generic.IEnumerable<CustomAttributeBuilder> assemblyAttributes) { throw null; }

        public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access) { throw null; }

        public ModuleBuilder DefineDynamicModule(string name) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public ModuleBuilder GetDynamicModule(string name) { throw null; }

        public override int GetHashCode() { throw null; }

        public override ManifestResourceInfo GetManifestResourceInfo(string resourceName) { throw null; }

        public override string[] GetManifestResourceNames() { throw null; }

        public override IO.Stream GetManifestResourceStream(string name) { throw null; }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
    }

    [Flags]
    public enum AssemblyBuilderAccess
    {
        Run = 1,
        RunAndCollect = 9
    }

    public sealed partial class ConstructorBuilder : ConstructorInfo
    {
        internal ConstructorBuilder() { }

        public override MethodAttributes Attributes { get { throw null; } }

        public override CallingConventions CallingConvention { get { throw null; } }

        public override Type DeclaringType { get { throw null; } }

        public bool InitLocals { get { throw null; } set { } }

        public override MethodImplAttributes MethodImplementationFlags { get { throw null; } }

        public override string Name { get { throw null; } }

        public ParameterBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string strParamName) { throw null; }

        public ILGenerator GetILGenerator() { throw null; }

        public ILGenerator GetILGenerator(int streamSize) { throw null; }

        public override ParameterInfo[] GetParameters() { throw null; }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }

        public void SetImplementationFlags(MethodImplAttributes attributes) { }

        public override string ToString() { throw null; }
    }

    public sealed partial class EnumBuilder : TypeInfo
    {
        internal EnumBuilder() { }

        public override Assembly Assembly { get { throw null; } }

        public override string AssemblyQualifiedName { get { throw null; } }

        public override TypeAttributes Attributes { get { throw null; } }

        public override Type BaseType { get { throw null; } }

        public override bool ContainsGenericParameters { get { throw null; } }

        public override MethodBase DeclaringMethod { get { throw null; } }

        public override Type DeclaringType { get { throw null; } }

        public override string FullName { get { throw null; } }

        public override GenericParameterAttributes GenericParameterAttributes { get { throw null; } }

        public override int GenericParameterPosition { get { throw null; } }

        public override Type[] GenericTypeArguments { get { throw null; } }

        public override Guid GUID { get { throw null; } }

        public override bool IsEnum { get { throw null; } }

        public override bool IsGenericParameter { get { throw null; } }

        public override bool IsGenericType { get { throw null; } }

        public override bool IsGenericTypeDefinition { get { throw null; } }

        public override bool IsSerializable { get { throw null; } }

        public override Module Module { get { throw null; } }

        public override string Name { get { throw null; } }

        public override string Namespace { get { throw null; } }

        public FieldBuilder UnderlyingField { get { throw null; } }

        public TypeInfo CreateTypeInfo() { throw null; }

        public FieldBuilder DefineLiteral(string literalName, object literalValue) { throw null; }

        public override int GetArrayRank() { throw null; }

        public override Type GetElementType() { throw null; }

        public override Type[] GetGenericParameterConstraints() { throw null; }

        public override Type GetGenericTypeDefinition() { throw null; }

        public override bool IsAssignableFrom(TypeInfo typeInfo) { throw null; }

        public override Type MakeArrayType() { throw null; }

        public override Type MakeArrayType(int rank) { throw null; }

        public override Type MakeByRefType() { throw null; }

        public override Type MakeGenericType(params Type[] typeArguments) { throw null; }

        public override Type MakePointerType() { throw null; }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
    }

    public sealed partial class EventBuilder
    {
        internal EventBuilder() { }

        public void AddOtherMethod(MethodBuilder mdBuilder) { }

        public void SetAddOnMethod(MethodBuilder mdBuilder) { }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }

        public void SetRaiseMethod(MethodBuilder mdBuilder) { }

        public void SetRemoveOnMethod(MethodBuilder mdBuilder) { }
    }

    public sealed partial class FieldBuilder : FieldInfo
    {
        internal FieldBuilder() { }

        public override FieldAttributes Attributes { get { throw null; } }

        public override Type DeclaringType { get { throw null; } }

        public override Type FieldType { get { throw null; } }

        public override string Name { get { throw null; } }

        public override object GetValue(object obj) { throw null; }

        public void SetConstant(object defaultValue) { }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }

        public void SetOffset(int iOffset) { }
    }

    public sealed partial class GenericTypeParameterBuilder : TypeInfo
    {
        internal GenericTypeParameterBuilder() { }

        public override Assembly Assembly { get { throw null; } }

        public override string AssemblyQualifiedName { get { throw null; } }

        public override TypeAttributes Attributes { get { throw null; } }

        public override Type BaseType { get { throw null; } }

        public override bool ContainsGenericParameters { get { throw null; } }

        public override MethodBase DeclaringMethod { get { throw null; } }

        public override Type DeclaringType { get { throw null; } }

        public override string FullName { get { throw null; } }

        public override GenericParameterAttributes GenericParameterAttributes { get { throw null; } }

        public override int GenericParameterPosition { get { throw null; } }

        public override Type[] GenericTypeArguments { get { throw null; } }

        public override Guid GUID { get { throw null; } }

        public override bool IsEnum { get { throw null; } }

        public override bool IsGenericParameter { get { throw null; } }

        public override bool IsGenericType { get { throw null; } }

        public override bool IsGenericTypeDefinition { get { throw null; } }

        public override bool IsSerializable { get { throw null; } }

        public override Module Module { get { throw null; } }

        public override string Name { get { throw null; } }

        public override string Namespace { get { throw null; } }

        public override bool Equals(object o) { throw null; }

        public override int GetArrayRank() { throw null; }

        public override Type GetElementType() { throw null; }

        public override Type[] GetGenericParameterConstraints() { throw null; }

        public override Type GetGenericTypeDefinition() { throw null; }

        public override int GetHashCode() { throw null; }

        public override bool IsAssignableFrom(TypeInfo typeInfo) { throw null; }

        public override bool IsSubclassOf(Type c) { throw null; }

        public override Type MakeArrayType() { throw null; }

        public override Type MakeArrayType(int rank) { throw null; }

        public override Type MakeByRefType() { throw null; }

        public override Type MakeGenericType(params Type[] typeArguments) { throw null; }

        public override Type MakePointerType() { throw null; }

        public void SetBaseTypeConstraint(Type baseTypeConstraint) { }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }

        public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes) { }

        public void SetInterfaceConstraints(params Type[] interfaceConstraints) { }

        public override string ToString() { throw null; }
    }

    public sealed partial class MethodBuilder : MethodInfo
    {
        internal MethodBuilder() { }

        public override MethodAttributes Attributes { get { throw null; } }

        public override CallingConventions CallingConvention { get { throw null; } }

        public override bool ContainsGenericParameters { get { throw null; } }

        public override Type DeclaringType { get { throw null; } }

        public bool InitLocals { get { throw null; } set { } }

        public override bool IsGenericMethod { get { throw null; } }

        public override bool IsGenericMethodDefinition { get { throw null; } }

        public override MethodImplAttributes MethodImplementationFlags { get { throw null; } }

        public override string Name { get { throw null; } }

        public override ParameterInfo ReturnParameter { get { throw null; } }

        public override Type ReturnType { get { throw null; } }

        public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names) { throw null; }

        public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string strParamName) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override Type[] GetGenericArguments() { throw null; }

        public override MethodInfo GetGenericMethodDefinition() { throw null; }

        public override int GetHashCode() { throw null; }

        public ILGenerator GetILGenerator() { throw null; }

        public ILGenerator GetILGenerator(int size) { throw null; }

        public override ParameterInfo[] GetParameters() { throw null; }

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) { throw null; }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }

        public void SetImplementationFlags(MethodImplAttributes attributes) { }

        public void SetParameters(params Type[] parameterTypes) { }

        public void SetReturnType(Type returnType) { }

        public void SetSignature(Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers) { }

        public override string ToString() { throw null; }
    }

    public partial class ModuleBuilder : Module
    {
        internal ModuleBuilder() { }

        public override Assembly Assembly { get { throw null; } }

        public override string FullyQualifiedName { get { throw null; } }

        public override string Name { get { throw null; } }

        public void CreateGlobalFunctions() { }

        public EnumBuilder DefineEnum(string name, TypeAttributes visibility, Type underlyingType) { throw null; }

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers) { throw null; }

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) { throw null; }

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes) { throw null; }

        public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes) { throw null; }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, int typesize) { throw null; }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize) { throw null; }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packsize) { throw null; }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, Type[] interfaces) { throw null; }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent) { throw null; }

        public TypeBuilder DefineType(string name, TypeAttributes attr) { throw null; }

        public TypeBuilder DefineType(string name) { throw null; }

        public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public MethodInfo GetArrayMethod(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) { throw null; }

        public override int GetHashCode() { throw null; }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
    }

    public sealed partial class PropertyBuilder : PropertyInfo
    {
        internal PropertyBuilder() { }

        public override PropertyAttributes Attributes { get { throw null; } }

        public override bool CanRead { get { throw null; } }

        public override bool CanWrite { get { throw null; } }

        public override Type DeclaringType { get { throw null; } }

        public override string Name { get { throw null; } }

        public override Type PropertyType { get { throw null; } }

        public void AddOtherMethod(MethodBuilder mdBuilder) { }

        public override ParameterInfo[] GetIndexParameters() { throw null; }

        public override object GetValue(object obj, object[] index) { throw null; }

        public void SetConstant(object defaultValue) { }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }

        public void SetGetMethod(MethodBuilder mdBuilder) { }

        public void SetSetMethod(MethodBuilder mdBuilder) { }

        public override void SetValue(object obj, object value, object[] index) { }
    }

    public sealed partial class TypeBuilder : TypeInfo
    {
        internal TypeBuilder() { }

        public const int UnspecifiedTypeSize = 0;
        public override Assembly Assembly { get { throw null; } }

        public override string AssemblyQualifiedName { get { throw null; } }

        public override TypeAttributes Attributes { get { throw null; } }

        public override Type BaseType { get { throw null; } }

        public override bool ContainsGenericParameters { get { throw null; } }

        public override MethodBase DeclaringMethod { get { throw null; } }

        public override Type DeclaringType { get { throw null; } }

        public override string FullName { get { throw null; } }

        public override GenericParameterAttributes GenericParameterAttributes { get { throw null; } }

        public override int GenericParameterPosition { get { throw null; } }

        public override Type[] GenericTypeArguments { get { throw null; } }

        public override Guid GUID { get { throw null; } }

        public override bool IsEnum { get { throw null; } }

        public override bool IsGenericParameter { get { throw null; } }

        public override bool IsGenericType { get { throw null; } }

        public override bool IsGenericTypeDefinition { get { throw null; } }

        public override bool IsSerializable { get { throw null; } }

        public override Module Module { get { throw null; } }

        public override string Name { get { throw null; } }

        public override string Namespace { get { throw null; } }

        public PackingSize PackingSize { get { throw null; } }

        public int Size { get { throw null; } }

        public void AddInterfaceImplementation(Type interfaceType) { }

        public TypeInfo CreateTypeInfo() { throw null; }

        public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers) { throw null; }

        public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes) { throw null; }

        public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes) { throw null; }

        public EventBuilder DefineEvent(string name, EventAttributes attributes, Type eventtype) { throw null; }

        public FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes) { throw null; }

        public FieldBuilder DefineField(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes) { throw null; }

        public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names) { throw null; }

        public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes) { throw null; }

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers) { throw null; }

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) { throw null; }

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention) { throw null; }

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes) { throw null; }

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes) { throw null; }

        public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration) { }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, int typeSize) { throw null; }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize, int typeSize) { throw null; }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize) { throw null; }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces) { throw null; }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent) { throw null; }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr) { throw null; }

        public TypeBuilder DefineNestedType(string name) { throw null; }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers) { throw null; }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) { throw null; }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers) { throw null; }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes) { throw null; }

        public ConstructorBuilder DefineTypeInitializer() { throw null; }

        public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes) { throw null; }

        public override int GetArrayRank() { throw null; }

        public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor) { throw null; }

        public override Type GetElementType() { throw null; }

        public static FieldInfo GetField(Type type, FieldInfo field) { throw null; }

        public override Type[] GetGenericParameterConstraints() { throw null; }

        public override Type GetGenericTypeDefinition() { throw null; }

        public static MethodInfo GetMethod(Type type, MethodInfo method) { throw null; }

        public override bool IsAssignableFrom(TypeInfo typeInfo) { throw null; }

        public bool IsCreated() { throw null; }

        public override Type MakeArrayType() { throw null; }

        public override Type MakeArrayType(int rank) { throw null; }

        public override Type MakeByRefType() { throw null; }

        public override Type MakeGenericType(params Type[] typeArguments) { throw null; }

        public override Type MakePointerType() { throw null; }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }

        public void SetParent(Type parent) { }

        public override string ToString() { throw null; }
    }
}