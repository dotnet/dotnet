// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection.Emit")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("System.Reflection.Emit")]
[assembly: System.Reflection.AssemblyFileVersion("4.700.19.56404")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.1.0+0f7f38c4fd323b26da10cce95f857f77f0f09b48")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Core")]
[assembly: System.Reflection.AssemblyTitle("System.Reflection.Emit")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Reflection.Emit
{
    public sealed partial class AssemblyBuilder : Assembly
    {
        internal AssemblyBuilder() { }
        public override string? CodeBase { get { throw null; } }
        public override MethodInfo? EntryPoint { get { throw null; } }
        public override string? FullName { get { throw null; } }
        public override bool GlobalAssemblyCache { get { throw null; } }
        public override long HostContext { get { throw null; } }
        public override string ImageRuntimeVersion { get { throw null; } }
        public override bool IsDynamic { get { throw null; } }
        public override string Location { get { throw null; } }
        public override Module ManifestModule { get { throw null; } }
        public override bool ReflectionOnly { get { throw null; } }

        public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access, Collections.Generic.IEnumerable<CustomAttributeBuilder>? assemblyAttributes) { throw null; }
        public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access) { throw null; }
        public ModuleBuilder DefineDynamicModule(string name) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override object[] GetCustomAttributes(bool inherit) { throw null; }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) { throw null; }
        public override Collections.Generic.IList<CustomAttributeData> GetCustomAttributesData() { throw null; }
        public ModuleBuilder? GetDynamicModule(string name) { throw null; }
        public override Type[] GetExportedTypes() { throw null; }
        public override IO.FileStream GetFile(string name) { throw null; }
        public override IO.FileStream[] GetFiles(bool getResourceModules) { throw null; }
        public override int GetHashCode() { throw null; }
        public override Module[] GetLoadedModules(bool getResourceModules) { throw null; }
        public override ManifestResourceInfo? GetManifestResourceInfo(string resourceName) { throw null; }
        public override string[] GetManifestResourceNames() { throw null; }
        public override IO.Stream? GetManifestResourceStream(string name) { throw null; }
        public override IO.Stream? GetManifestResourceStream(Type type, string name) { throw null; }
        public override Module? GetModule(string name) { throw null; }
        public override Module[] GetModules(bool getResourceModules) { throw null; }
        public override AssemblyName GetName(bool copiedName) { throw null; }
        public override AssemblyName[] GetReferencedAssemblies() { throw null; }
        public override Assembly GetSatelliteAssembly(Globalization.CultureInfo culture, Version? version) { throw null; }
        public override Assembly GetSatelliteAssembly(Globalization.CultureInfo culture) { throw null; }
        public override Type? GetType(string name, bool throwOnError, bool ignoreCase) { throw null; }
        public override bool IsDefined(Type attributeType, bool inherit) { throw null; }
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
        public override Type? DeclaringType { get { throw null; } }
        public bool InitLocals { get { throw null; } set { } }
        public override RuntimeMethodHandle MethodHandle { get { throw null; } }
        public override Module Module { get { throw null; } }
        public override string Name { get { throw null; } }
        public override Type? ReflectedType { get { throw null; } }

        public ParameterBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string? strParamName) { throw null; }
        public override object[] GetCustomAttributes(bool inherit) { throw null; }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) { throw null; }
        public ILGenerator GetILGenerator() { throw null; }
        public ILGenerator GetILGenerator(int streamSize) { throw null; }
        public override MethodImplAttributes GetMethodImplementationFlags() { throw null; }
        public override ParameterInfo[] GetParameters() { throw null; }
        public override object Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, Globalization.CultureInfo? culture) { throw null; }
        public override object Invoke(BindingFlags invokeAttr, Binder? binder, object?[]? parameters, Globalization.CultureInfo? culture) { throw null; }
        public override bool IsDefined(Type attributeType, bool inherit) { throw null; }
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
        public void SetImplementationFlags(MethodImplAttributes attributes) { }
        public override string ToString() { throw null; }
    }

    public sealed partial class EnumBuilder : Type
    {
        internal EnumBuilder() { }
        public override Assembly Assembly { get { throw null; } }
        public override string? AssemblyQualifiedName { get { throw null; } }
        public override Type? BaseType { get { throw null; } }
        public override Type? DeclaringType { get { throw null; } }
        public override string? FullName { get { throw null; } }
        public override Guid GUID { get { throw null; } }
        public override bool IsConstructedGenericType { get { throw null; } }
        public override Module Module { get { throw null; } }
        public override string Name { get { throw null; } }
        public override string? Namespace { get { throw null; } }
        public override Type? ReflectedType { get { throw null; } }
        public override RuntimeTypeHandle TypeHandle { get { throw null; } }
        public FieldBuilder UnderlyingField { get { throw null; } }
        public override Type UnderlyingSystemType { get { throw null; } }

        public TypeInfo? CreateTypeInfo() { throw null; }
        public FieldBuilder DefineLiteral(string literalName, object? literalValue) { throw null; }
        protected override TypeAttributes GetAttributeFlagsImpl() { throw null; }
        protected override ConstructorInfo? GetConstructorImpl(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers) { throw null; }
        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) { throw null; }
        public override object[] GetCustomAttributes(bool inherit) { throw null; }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) { throw null; }
        public override Type? GetElementType() { throw null; }
        public override Type GetEnumUnderlyingType() { throw null; }
        public override EventInfo? GetEvent(string name, BindingFlags bindingAttr) { throw null; }
        public override EventInfo[] GetEvents() { throw null; }
        public override EventInfo[] GetEvents(BindingFlags bindingAttr) { throw null; }
        public override FieldInfo? GetField(string name, BindingFlags bindingAttr) { throw null; }
        public override FieldInfo[] GetFields(BindingFlags bindingAttr) { throw null; }
        public override Type? GetInterface(string name, bool ignoreCase) { throw null; }
        public override InterfaceMapping GetInterfaceMap(Type interfaceType) { throw null; }
        public override Type[] GetInterfaces() { throw null; }
        public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr) { throw null; }
        public override MemberInfo[] GetMembers(BindingFlags bindingAttr) { throw null; }
        protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers) { throw null; }
        public override MethodInfo[] GetMethods(BindingFlags bindingAttr) { throw null; }
        public override Type? GetNestedType(string name, BindingFlags bindingAttr) { throw null; }
        public override Type[] GetNestedTypes(BindingFlags bindingAttr) { throw null; }
        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) { throw null; }
        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers) { throw null; }
        protected override bool HasElementTypeImpl() { throw null; }
        public override object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, Globalization.CultureInfo? culture, string[]? namedParameters) { throw null; }
        protected override bool IsArrayImpl() { throw null; }
        protected override bool IsByRefImpl() { throw null; }
        protected override bool IsCOMObjectImpl() { throw null; }
        public override bool IsDefined(Type attributeType, bool inherit) { throw null; }
        protected override bool IsPointerImpl() { throw null; }
        protected override bool IsPrimitiveImpl() { throw null; }
        protected override bool IsValueTypeImpl() { throw null; }
        public override Type MakeArrayType() { throw null; }
        public override Type MakeArrayType(int rank) { throw null; }
        public override Type MakeByRefType() { throw null; }
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
        public override Type? DeclaringType { get { throw null; } }
        public override RuntimeFieldHandle FieldHandle { get { throw null; } }
        public override Type FieldType { get { throw null; } }
        public override Module Module { get { throw null; } }
        public override string Name { get { throw null; } }
        public override Type? ReflectedType { get { throw null; } }

        public override object[] GetCustomAttributes(bool inherit) { throw null; }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) { throw null; }
        public override object? GetValue(object? obj) { throw null; }
        public override bool IsDefined(Type attributeType, bool inherit) { throw null; }
        public void SetConstant(object? defaultValue) { }
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
        public void SetOffset(int iOffset) { }
        public override void SetValue(object? obj, object? val, BindingFlags invokeAttr, Binder? binder, Globalization.CultureInfo? culture) { }
    }

    public sealed partial class GenericTypeParameterBuilder : Type
    {
        internal GenericTypeParameterBuilder() { }
        public override Assembly Assembly { get { throw null; } }
        public override string? AssemblyQualifiedName { get { throw null; } }
        public override Type? BaseType { get { throw null; } }
        public override bool ContainsGenericParameters { get { throw null; } }
        public override MethodBase? DeclaringMethod { get { throw null; } }
        public override Type? DeclaringType { get { throw null; } }
        public override string? FullName { get { throw null; } }
        public override GenericParameterAttributes GenericParameterAttributes { get { throw null; } }
        public override int GenericParameterPosition { get { throw null; } }
        public override Guid GUID { get { throw null; } }
        public override bool IsConstructedGenericType { get { throw null; } }
        public override bool IsGenericParameter { get { throw null; } }
        public override bool IsGenericType { get { throw null; } }
        public override bool IsGenericTypeDefinition { get { throw null; } }
        public override Module Module { get { throw null; } }
        public override string Name { get { throw null; } }
        public override string? Namespace { get { throw null; } }
        public override Type? ReflectedType { get { throw null; } }
        public override RuntimeTypeHandle TypeHandle { get { throw null; } }
        public override Type UnderlyingSystemType { get { throw null; } }

        public override bool Equals(object? o) { throw null; }
        protected override TypeAttributes GetAttributeFlagsImpl() { throw null; }
        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers) { throw null; }
        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) { throw null; }
        public override object[] GetCustomAttributes(bool inherit) { throw null; }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) { throw null; }
        public override Type GetElementType() { throw null; }
        public override EventInfo GetEvent(string name, BindingFlags bindingAttr) { throw null; }
        public override EventInfo[] GetEvents() { throw null; }
        public override EventInfo[] GetEvents(BindingFlags bindingAttr) { throw null; }
        public override FieldInfo GetField(string name, BindingFlags bindingAttr) { throw null; }
        public override FieldInfo[] GetFields(BindingFlags bindingAttr) { throw null; }
        public override Type[] GetGenericArguments() { throw null; }
        public override Type GetGenericTypeDefinition() { throw null; }
        public override int GetHashCode() { throw null; }
        public override Type GetInterface(string name, bool ignoreCase) { throw null; }
        public override InterfaceMapping GetInterfaceMap(Type interfaceType) { throw null; }
        public override Type[] GetInterfaces() { throw null; }
        public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr) { throw null; }
        public override MemberInfo[] GetMembers(BindingFlags bindingAttr) { throw null; }
        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers) { throw null; }
        public override MethodInfo[] GetMethods(BindingFlags bindingAttr) { throw null; }
        public override Type GetNestedType(string name, BindingFlags bindingAttr) { throw null; }
        public override Type[] GetNestedTypes(BindingFlags bindingAttr) { throw null; }
        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) { throw null; }
        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers) { throw null; }
        protected override bool HasElementTypeImpl() { throw null; }
        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, Globalization.CultureInfo? culture, string[]? namedParameters) { throw null; }
        protected override bool IsArrayImpl() { throw null; }
        public override bool IsAssignableFrom(Type? c) { throw null; }
        protected override bool IsByRefImpl() { throw null; }
        protected override bool IsCOMObjectImpl() { throw null; }
        public override bool IsDefined(Type attributeType, bool inherit) { throw null; }
        protected override bool IsPointerImpl() { throw null; }
        protected override bool IsPrimitiveImpl() { throw null; }
        public override bool IsSubclassOf(Type c) { throw null; }
        protected override bool IsValueTypeImpl() { throw null; }
        public override Type MakeArrayType() { throw null; }
        public override Type MakeArrayType(int rank) { throw null; }
        public override Type MakeByRefType() { throw null; }
        public override Type MakeGenericType(params Type[] typeArguments) { throw null; }
        public override Type MakePointerType() { throw null; }
        public void SetBaseTypeConstraint(Type? baseTypeConstraint) { }
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
        public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes) { }
        public void SetInterfaceConstraints(params Type[]? interfaceConstraints) { }
        public override string ToString() { throw null; }
    }

    public sealed partial class MethodBuilder : MethodInfo
    {
        internal MethodBuilder() { }
        public override MethodAttributes Attributes { get { throw null; } }
        public override CallingConventions CallingConvention { get { throw null; } }
        public override bool ContainsGenericParameters { get { throw null; } }
        public override Type? DeclaringType { get { throw null; } }
        public bool InitLocals { get { throw null; } set { } }
        public override bool IsGenericMethod { get { throw null; } }
        public override bool IsGenericMethodDefinition { get { throw null; } }
        public override bool IsSecurityCritical { get { throw null; } }
        public override bool IsSecuritySafeCritical { get { throw null; } }
        public override bool IsSecurityTransparent { get { throw null; } }
        public override RuntimeMethodHandle MethodHandle { get { throw null; } }
        public override Module Module { get { throw null; } }
        public override string Name { get { throw null; } }
        public override Type? ReflectedType { get { throw null; } }
        public override ParameterInfo ReturnParameter { get { throw null; } }
        public override Type ReturnType { get { throw null; } }
        public override ICustomAttributeProvider ReturnTypeCustomAttributes { get { throw null; } }

        public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names) { throw null; }
        public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string? strParamName) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public override MethodInfo GetBaseDefinition() { throw null; }
        public override object[] GetCustomAttributes(bool inherit) { throw null; }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) { throw null; }
        public override Type[] GetGenericArguments() { throw null; }
        public override MethodInfo GetGenericMethodDefinition() { throw null; }
        public override int GetHashCode() { throw null; }
        public ILGenerator GetILGenerator() { throw null; }
        public ILGenerator GetILGenerator(int size) { throw null; }
        public override MethodImplAttributes GetMethodImplementationFlags() { throw null; }
        public override ParameterInfo[] GetParameters() { throw null; }
        public override object Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, Globalization.CultureInfo? culture) { throw null; }
        public override bool IsDefined(Type attributeType, bool inherit) { throw null; }
        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) { throw null; }
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
        public void SetImplementationFlags(MethodImplAttributes attributes) { }
        public void SetParameters(params Type[] parameterTypes) { }
        public void SetReturnType(Type? returnType) { }
        public void SetSignature(Type? returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers) { }
        public override string ToString() { throw null; }
    }

    public partial class ModuleBuilder : Module
    {
        internal ModuleBuilder() { }
        public override Assembly Assembly { get { throw null; } }
        public override string FullyQualifiedName { get { throw null; } }
        public override int MDStreamVersion { get { throw null; } }
        public override int MetadataToken { get { throw null; } }
        public override Guid ModuleVersionId { get { throw null; } }
        public override string Name { get { throw null; } }
        public override string ScopeName { get { throw null; } }

        public void CreateGlobalFunctions() { }
        public EnumBuilder DefineEnum(string name, TypeAttributes visibility, Type underlyingType) { throw null; }
        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? requiredReturnTypeCustomModifiers, Type[]? optionalReturnTypeCustomModifiers, Type[]? parameterTypes, Type[][]? requiredParameterTypeCustomModifiers, Type[][]? optionalParameterTypeCustomModifiers) { throw null; }
        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes) { throw null; }
        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type? returnType, Type[]? parameterTypes) { throw null; }
        public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes) { throw null; }
        public TypeBuilder DefineType(string name, TypeAttributes attr, Type? parent, int typesize) { throw null; }
        public TypeBuilder DefineType(string name, TypeAttributes attr, Type? parent, PackingSize packingSize, int typesize) { throw null; }
        public TypeBuilder DefineType(string name, TypeAttributes attr, Type? parent, PackingSize packsize) { throw null; }
        public TypeBuilder DefineType(string name, TypeAttributes attr, Type? parent, Type[]? interfaces) { throw null; }
        public TypeBuilder DefineType(string name, TypeAttributes attr, Type? parent) { throw null; }
        public TypeBuilder DefineType(string name, TypeAttributes attr) { throw null; }
        public TypeBuilder DefineType(string name) { throw null; }
        public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes) { throw null; }
        public override bool Equals(object? obj) { throw null; }
        public MethodInfo GetArrayMethod(Type arrayClass, string methodName, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes) { throw null; }
        public override object[] GetCustomAttributes(bool inherit) { throw null; }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) { throw null; }
        public override Collections.Generic.IList<CustomAttributeData> GetCustomAttributesData() { throw null; }
        public override FieldInfo? GetField(string name, BindingFlags bindingAttr) { throw null; }
        public override FieldInfo[] GetFields(BindingFlags bindingFlags) { throw null; }
        public override int GetHashCode() { throw null; }
        public override MethodInfo[] GetMethods(BindingFlags bindingFlags) { throw null; }
        public override void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine) { throw null; }
        public override Type? GetType(string className, bool throwOnError, bool ignoreCase) { throw null; }
        public override Type? GetType(string className, bool ignoreCase) { throw null; }
        public override Type? GetType(string className) { throw null; }
        public override Type[] GetTypes() { throw null; }
        public override bool IsDefined(Type attributeType, bool inherit) { throw null; }
        public override bool IsResource() { throw null; }
        public override FieldInfo? ResolveField(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments) { throw null; }
        public override MemberInfo? ResolveMember(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments) { throw null; }
        public override MethodBase? ResolveMethod(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments) { throw null; }
        public override byte[] ResolveSignature(int metadataToken) { throw null; }
        public override string ResolveString(int metadataToken) { throw null; }
        public override Type ResolveType(int metadataToken, Type[]? genericTypeArguments, Type[]? genericMethodArguments) { throw null; }
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
    }

    public sealed partial class PropertyBuilder : PropertyInfo
    {
        internal PropertyBuilder() { }
        public override PropertyAttributes Attributes { get { throw null; } }
        public override bool CanRead { get { throw null; } }
        public override bool CanWrite { get { throw null; } }
        public override Type? DeclaringType { get { throw null; } }
        public override Module Module { get { throw null; } }
        public override string Name { get { throw null; } }
        public override Type PropertyType { get { throw null; } }
        public override Type? ReflectedType { get { throw null; } }

        public void AddOtherMethod(MethodBuilder mdBuilder) { }
        public override MethodInfo[] GetAccessors(bool nonPublic) { throw null; }
        public override object[] GetCustomAttributes(bool inherit) { throw null; }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) { throw null; }
        public override MethodInfo? GetGetMethod(bool nonPublic) { throw null; }
        public override ParameterInfo[] GetIndexParameters() { throw null; }
        public override MethodInfo? GetSetMethod(bool nonPublic) { throw null; }
        public override object GetValue(object? obj, object?[]? index) { throw null; }
        public override object GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, Globalization.CultureInfo? culture) { throw null; }
        public override bool IsDefined(Type attributeType, bool inherit) { throw null; }
        public void SetConstant(object? defaultValue) { }
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
        public void SetGetMethod(MethodBuilder mdBuilder) { }
        public void SetSetMethod(MethodBuilder mdBuilder) { }
        public override void SetValue(object? obj, object? value, object?[]? index) { }
        public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, Globalization.CultureInfo? culture) { }
    }

    public sealed partial class TypeBuilder : Type
    {
        internal TypeBuilder() { }
        public const int UnspecifiedTypeSize = 0;
        public override Assembly Assembly { get { throw null; } }
        public override string? AssemblyQualifiedName { get { throw null; } }
        public override Type? BaseType { get { throw null; } }
        public override MethodBase? DeclaringMethod { get { throw null; } }
        public override Type? DeclaringType { get { throw null; } }
        public override string? FullName { get { throw null; } }
        public override GenericParameterAttributes GenericParameterAttributes { get { throw null; } }
        public override int GenericParameterPosition { get { throw null; } }
        public override Guid GUID { get { throw null; } }
        public override bool IsConstructedGenericType { get { throw null; } }
        public override bool IsGenericParameter { get { throw null; } }
        public override bool IsGenericType { get { throw null; } }
        public override bool IsGenericTypeDefinition { get { throw null; } }
        public override bool IsSecurityCritical { get { throw null; } }
        public override bool IsSecuritySafeCritical { get { throw null; } }
        public override bool IsSecurityTransparent { get { throw null; } }
        public override Module Module { get { throw null; } }
        public override string Name { get { throw null; } }
        public override string? Namespace { get { throw null; } }
        public PackingSize PackingSize { get { throw null; } }
        public override Type? ReflectedType { get { throw null; } }
        public int Size { get { throw null; } }
        public override RuntimeTypeHandle TypeHandle { get { throw null; } }
        public override Type UnderlyingSystemType { get { throw null; } }

        public void AddInterfaceImplementation(Type interfaceType) { }
        public TypeInfo? CreateTypeInfo() { throw null; }
        public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[]? parameterTypes, Type[][]? requiredCustomModifiers, Type[][]? optionalCustomModifiers) { throw null; }
        public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[]? parameterTypes) { throw null; }
        public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes) { throw null; }
        public EventBuilder DefineEvent(string name, EventAttributes attributes, Type eventtype) { throw null; }
        public FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes) { throw null; }
        public FieldBuilder DefineField(string fieldName, Type type, Type[]? requiredCustomModifiers, Type[]? optionalCustomModifiers, FieldAttributes attributes) { throw null; }
        public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names) { throw null; }
        public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes) { throw null; }
        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers) { throw null; }
        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type? returnType, Type[]? parameterTypes) { throw null; }
        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention) { throw null; }
        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, Type? returnType, Type[]? parameterTypes) { throw null; }
        public MethodBuilder DefineMethod(string name, MethodAttributes attributes) { throw null; }
        public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration) { }
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type? parent, int typeSize) { throw null; }
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type? parent, PackingSize packSize, int typeSize) { throw null; }
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type? parent, PackingSize packSize) { throw null; }
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type? parent, Type[]? interfaces) { throw null; }
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type? parent) { throw null; }
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr) { throw null; }
        public TypeBuilder DefineNestedType(string name) { throw null; }
        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers) { throw null; }
        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[]? parameterTypes) { throw null; }
        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[]? returnTypeRequiredCustomModifiers, Type[]? returnTypeOptionalCustomModifiers, Type[]? parameterTypes, Type[][]? parameterTypeRequiredCustomModifiers, Type[][]? parameterTypeOptionalCustomModifiers) { throw null; }
        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[]? parameterTypes) { throw null; }
        public ConstructorBuilder DefineTypeInitializer() { throw null; }
        public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes) { throw null; }
        protected override TypeAttributes GetAttributeFlagsImpl() { throw null; }
        public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor) { throw null; }
        protected override ConstructorInfo? GetConstructorImpl(BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[] types, ParameterModifier[]? modifiers) { throw null; }
        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) { throw null; }
        public override object[] GetCustomAttributes(bool inherit) { throw null; }
        public override object[] GetCustomAttributes(Type attributeType, bool inherit) { throw null; }
        public override Type GetElementType() { throw null; }
        public override EventInfo? GetEvent(string name, BindingFlags bindingAttr) { throw null; }
        public override EventInfo[] GetEvents() { throw null; }
        public override EventInfo[] GetEvents(BindingFlags bindingAttr) { throw null; }
        public override FieldInfo? GetField(string name, BindingFlags bindingAttr) { throw null; }
        public static FieldInfo GetField(Type type, FieldInfo field) { throw null; }
        public override FieldInfo[] GetFields(BindingFlags bindingAttr) { throw null; }
        public override Type[] GetGenericArguments() { throw null; }
        public override Type GetGenericTypeDefinition() { throw null; }
        public override Type? GetInterface(string name, bool ignoreCase) { throw null; }
        public override InterfaceMapping GetInterfaceMap(Type interfaceType) { throw null; }
        public override Type[] GetInterfaces() { throw null; }
        public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr) { throw null; }
        public override MemberInfo[] GetMembers(BindingFlags bindingAttr) { throw null; }
        public static MethodInfo GetMethod(Type type, MethodInfo method) { throw null; }
        protected override MethodInfo? GetMethodImpl(string name, BindingFlags bindingAttr, Binder? binder, CallingConventions callConvention, Type[]? types, ParameterModifier[]? modifiers) { throw null; }
        public override MethodInfo[] GetMethods(BindingFlags bindingAttr) { throw null; }
        public override Type? GetNestedType(string name, BindingFlags bindingAttr) { throw null; }
        public override Type[] GetNestedTypes(BindingFlags bindingAttr) { throw null; }
        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) { throw null; }
        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder? binder, Type? returnType, Type[]? types, ParameterModifier[]? modifiers) { throw null; }
        protected override bool HasElementTypeImpl() { throw null; }
        public override object? InvokeMember(string name, BindingFlags invokeAttr, Binder? binder, object? target, object?[]? args, ParameterModifier[]? modifiers, Globalization.CultureInfo? culture, string[]? namedParameters) { throw null; }
        protected override bool IsArrayImpl() { throw null; }
        public override bool IsAssignableFrom(Type? c) { throw null; }
        protected override bool IsByRefImpl() { throw null; }
        protected override bool IsCOMObjectImpl() { throw null; }
        public bool IsCreated() { throw null; }
        public override bool IsDefined(Type attributeType, bool inherit) { throw null; }
        protected override bool IsPointerImpl() { throw null; }
        protected override bool IsPrimitiveImpl() { throw null; }
        public override bool IsSubclassOf(Type c) { throw null; }
        public override Type MakeArrayType() { throw null; }
        public override Type MakeArrayType(int rank) { throw null; }
        public override Type MakeByRefType() { throw null; }
        public override Type MakeGenericType(params Type[] typeArguments) { throw null; }
        public override Type MakePointerType() { throw null; }
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute) { }
        public void SetCustomAttribute(CustomAttributeBuilder customBuilder) { }
        public void SetParent(Type? parent) { }
        public override string ToString() { throw null; }
    }
}