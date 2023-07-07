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
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("System.Private.Reflection.Extensibility, PublicKey=002400000480000094000000060200000024000052534131000400000100010007D1FA57C4AED9F0A32E84AA0FAEFD0DE9E8FD6AEC8F87FB03766C834C99921EB23BE79AD9D5DCC1DD9AD236132102900B723CF980957FC4E177108FC607774F29E8320E92EA05ECE4E821C0A5EFE8F1645C4C0C93C1AB99285D622CAA652C1DFAD63D745D6F2DE5F17E5EAF0FC4963D261C8A12436518206DC093344D5AD293")]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Reflection")]
[assembly: System.Reflection.AssemblyDescription("System.Reflection")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Reflection
{
    public sealed partial class AmbiguousMatchException : Exception
    {
        public AmbiguousMatchException() { }

        public AmbiguousMatchException(string message, Exception inner) { }

        public AmbiguousMatchException(string message) { }
    }

    public abstract partial class Assembly : ICustomAttributeProvider
    {
        internal Assembly() { }

        public virtual string CodeBase { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<CustomAttributeData> CustomAttributes { get { throw null; } }

        public abstract Collections.Generic.IEnumerable<TypeInfo> DefinedTypes { get; }

        public virtual MethodInfo EntryPoint { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<Type> ExportedTypes { get { throw null; } }

        public virtual string FullName { get { throw null; } }

        public virtual string ImageRuntimeVersion { get { throw null; } }

        public virtual bool IsDynamic { get { throw null; } }

        public virtual string Location { get { throw null; } }

        public virtual Module ManifestModule { get { throw null; } }

        public abstract Collections.Generic.IEnumerable<Module> Modules { get; }

        public object CreateInstance(string typeName, bool ignoreCase) { throw null; }

        public object CreateInstance(string typeName) { throw null; }

        public static string CreateQualifiedName(string assemblyName, string typeName) { throw null; }

        public override bool Equals(object o) { throw null; }

        public static Assembly GetEntryAssembly() { throw null; }

        public virtual Type[] GetExportedTypes() { throw null; }

        public override int GetHashCode() { throw null; }

        public virtual ManifestResourceInfo GetManifestResourceInfo(string resourceName) { throw null; }

        public virtual string[] GetManifestResourceNames() { throw null; }

        public virtual IO.Stream GetManifestResourceStream(string name) { throw null; }

        public virtual AssemblyName GetName() { throw null; }

        public virtual AssemblyName[] GetReferencedAssemblies() { throw null; }

        public virtual Type GetType(string name, bool throwOnError, bool ignoreCase) { throw null; }

        public virtual Type GetType(string name, bool throwOnError) { throw null; }

        public virtual Type GetType(string name) { throw null; }

        public virtual Type[] GetTypes() { throw null; }

        public static Assembly Load(AssemblyName assemblyRef) { throw null; }

        object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit) { throw null; }

        object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit) { throw null; }

        bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit) { throw null; }

        public override string ToString() { throw null; }
    }

    public enum AssemblyContentType
    {
        Default = 0,
        WindowsRuntime = 1
    }

    public sealed partial class AssemblyName
    {
        public AssemblyName() { }

        public AssemblyName(string assemblyName) { }

        public AssemblyContentType ContentType { get { throw null; } set { } }

        public string CultureName { get { throw null; } set { } }

        public AssemblyNameFlags Flags { get { throw null; } set { } }

        public string FullName { get { throw null; } }

        public string Name { get { throw null; } set { } }

        public ProcessorArchitecture ProcessorArchitecture { get { throw null; } set { } }

        public Version Version { get { throw null; } set { } }

        public byte[] GetPublicKey() { throw null; }

        public byte[] GetPublicKeyToken() { throw null; }

        public void SetPublicKey(byte[] publicKey) { }

        public void SetPublicKeyToken(byte[] publicKeyToken) { }

        public override string ToString() { throw null; }
    }

    [Flags]
    public enum BindingFlags
    {
        Default = 0,
        IgnoreCase = 1,
        DeclaredOnly = 2,
        Instance = 4,
        Static = 8,
        Public = 16,
        NonPublic = 32,
        FlattenHierarchy = 64,
        InvokeMethod = 256,
        CreateInstance = 512,
        GetField = 1024,
        SetField = 2048,
        GetProperty = 4096,
        SetProperty = 8192
    }

    public abstract partial class ConstructorInfo : MethodBase
    {
        internal ConstructorInfo() { }

        public static readonly string ConstructorName;
        public static readonly string TypeConstructorName;
        public override MemberTypes MemberType { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public virtual object Invoke(object[] parameters) { throw null; }
    }

    public partial class CustomAttributeData
    {
        internal CustomAttributeData() { }

        public virtual Type AttributeType { get { throw null; } }

        public virtual ConstructorInfo Constructor { get { throw null; } }

        public virtual Collections.Generic.IList<CustomAttributeTypedArgument> ConstructorArguments { get { throw null; } }

        public virtual Collections.Generic.IList<CustomAttributeNamedArgument> NamedArguments { get { throw null; } }

        public static Collections.Generic.IList<CustomAttributeData> GetCustomAttributes(Assembly target) { throw null; }

        public static Collections.Generic.IList<CustomAttributeData> GetCustomAttributes(MemberInfo target) { throw null; }

        public static Collections.Generic.IList<CustomAttributeData> GetCustomAttributes(Module target) { throw null; }

        public static Collections.Generic.IList<CustomAttributeData> GetCustomAttributes(ParameterInfo target) { throw null; }
    }

    public partial struct CustomAttributeNamedArgument
    {
        public bool IsField { get { throw null; } }

        public string MemberName { get { throw null; } }

        public CustomAttributeTypedArgument TypedValue { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right) { throw null; }

        public static bool operator !=(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial struct CustomAttributeTypedArgument
    {
        public Type ArgumentType { get { throw null; } }

        public object Value { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right) { throw null; }

        public static bool operator !=(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right) { throw null; }

        public override string ToString() { throw null; }
    }

    public abstract partial class EventInfo : MemberInfo
    {
        internal EventInfo() { }

        public virtual MethodInfo AddMethod { get { throw null; } }

        public abstract EventAttributes Attributes { get; }

        public virtual Type EventHandlerType { get { throw null; } }

        public virtual bool IsMulticast { get { throw null; } }

        public bool IsSpecialName { get { throw null; } }

        public override MemberTypes MemberType { get { throw null; } }

        public virtual MethodInfo RaiseMethod { get { throw null; } }

        public virtual MethodInfo RemoveMethod { get { throw null; } }

        public virtual void AddEventHandler(object target, Delegate handler) { }

        public override bool Equals(object obj) { throw null; }

        public MethodInfo GetAddMethod() { throw null; }

        public abstract MethodInfo GetAddMethod(bool nonPublic);
        public override int GetHashCode() { throw null; }

        public MethodInfo GetRaiseMethod() { throw null; }

        public abstract MethodInfo GetRaiseMethod(bool nonPublic);
        public MethodInfo GetRemoveMethod() { throw null; }

        public abstract MethodInfo GetRemoveMethod(bool nonPublic);
        public virtual void RemoveEventHandler(object target, Delegate handler) { }
    }

    public abstract partial class FieldInfo : MemberInfo
    {
        internal FieldInfo() { }

        public abstract FieldAttributes Attributes { get; }
        public abstract Type FieldType { get; }

        public bool IsAssembly { get { throw null; } }

        public bool IsFamily { get { throw null; } }

        public bool IsFamilyAndAssembly { get { throw null; } }

        public bool IsFamilyOrAssembly { get { throw null; } }

        public bool IsInitOnly { get { throw null; } }

        public bool IsLiteral { get { throw null; } }

        public bool IsPrivate { get { throw null; } }

        public bool IsPublic { get { throw null; } }

        public bool IsSpecialName { get { throw null; } }

        public bool IsStatic { get { throw null; } }

        public override MemberTypes MemberType { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle, RuntimeTypeHandle declaringType) { throw null; }

        public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle) { throw null; }

        public override int GetHashCode() { throw null; }

        public virtual Type[] GetOptionalCustomModifiers() { throw null; }

        public virtual object GetRawConstantValue() { throw null; }

        public virtual Type[] GetRequiredCustomModifiers() { throw null; }

        public abstract object GetValue(object obj);
        public virtual void SetValue(object obj, object value) { }
    }

    public partial interface ICustomAttributeProvider
    {
        object[] GetCustomAttributes(bool inherit);
        object[] GetCustomAttributes(Type attributeType, bool inherit);
        bool IsDefined(Type attributeType, bool inherit);
    }

    public static partial class IntrospectionExtensions
    {
        public static TypeInfo GetTypeInfo(this Type type) { throw null; }
    }

    public partial class InvalidFilterCriteriaException : Exception
    {
        public InvalidFilterCriteriaException() { }

        public InvalidFilterCriteriaException(string message, Exception inner) { }

        public InvalidFilterCriteriaException(string message) { }
    }

    public partial interface IReflectableType
    {
        TypeInfo GetTypeInfo();
    }

    public partial class LocalVariableInfo
    {
        protected LocalVariableInfo() { }

        public virtual bool IsPinned { get { throw null; } }

        public virtual int LocalIndex { get { throw null; } }

        public virtual Type LocalType { get { throw null; } }

        public override string ToString() { throw null; }
    }

    public partial class ManifestResourceInfo
    {
        public ManifestResourceInfo(Assembly containingAssembly, string containingFileName, ResourceLocation resourceLocation) { }

        public virtual string FileName { get { throw null; } }

        public virtual Assembly ReferencedAssembly { get { throw null; } }

        public virtual ResourceLocation ResourceLocation { get { throw null; } }
    }

    public delegate bool MemberFilter(MemberInfo m, object filterCriteria);
    public abstract partial class MemberInfo : ICustomAttributeProvider
    {
        internal MemberInfo() { }

        public virtual Collections.Generic.IEnumerable<CustomAttributeData> CustomAttributes { get { throw null; } }

        public abstract Type DeclaringType { get; }
        public abstract MemberTypes MemberType { get; }

        public virtual int MetadataToken { get { throw null; } }

        public virtual Module Module { get { throw null; } }

        public abstract string Name { get; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit) { throw null; }

        object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit) { throw null; }

        bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit) { throw null; }
    }

    [Flags]
    public enum MemberTypes
    {
        Constructor = 1,
        Event = 2,
        Field = 4,
        Method = 8,
        Property = 16,
        TypeInfo = 32,
        Custom = 64,
        NestedType = 128,
        All = 191
    }

    public abstract partial class MethodBase : MemberInfo
    {
        internal MethodBase() { }

        public abstract MethodAttributes Attributes { get; }

        public virtual CallingConventions CallingConvention { get { throw null; } }

        public virtual bool ContainsGenericParameters { get { throw null; } }

        public bool IsAbstract { get { throw null; } }

        public bool IsAssembly { get { throw null; } }

        public bool IsConstructor { get { throw null; } }

        public bool IsFamily { get { throw null; } }

        public bool IsFamilyAndAssembly { get { throw null; } }

        public bool IsFamilyOrAssembly { get { throw null; } }

        public bool IsFinal { get { throw null; } }

        public virtual bool IsGenericMethod { get { throw null; } }

        public virtual bool IsGenericMethodDefinition { get { throw null; } }

        public bool IsHideBySig { get { throw null; } }

        public bool IsPrivate { get { throw null; } }

        public bool IsPublic { get { throw null; } }

        public bool IsSpecialName { get { throw null; } }

        public bool IsStatic { get { throw null; } }

        public bool IsVirtual { get { throw null; } }

        public abstract MethodImplAttributes MethodImplementationFlags { get; }

        public override bool Equals(object obj) { throw null; }

        public virtual Type[] GetGenericArguments() { throw null; }

        public override int GetHashCode() { throw null; }

        public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType) { throw null; }

        public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle) { throw null; }

        public abstract MethodImplAttributes GetMethodImplementationFlags();
        public abstract ParameterInfo[] GetParameters();
        public virtual object Invoke(object obj, object[] parameters) { throw null; }
    }

    public abstract partial class MethodInfo : MethodBase
    {
        internal MethodInfo() { }

        public override MemberTypes MemberType { get { throw null; } }

        public virtual ParameterInfo ReturnParameter { get { throw null; } }

        public virtual Type ReturnType { get { throw null; } }

        public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

        public virtual Delegate CreateDelegate(Type delegateType, object target) { throw null; }

        public virtual Delegate CreateDelegate(Type delegateType) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public abstract MethodInfo GetBaseDefinition();
        public override Type[] GetGenericArguments() { throw null; }

        public virtual MethodInfo GetGenericMethodDefinition() { throw null; }

        public override int GetHashCode() { throw null; }

        public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments) { throw null; }
    }

    public abstract partial class Module : ICustomAttributeProvider
    {
        internal Module() { }

        public static readonly TypeFilter FilterTypeName;
        public static readonly TypeFilter FilterTypeNameIgnoreCase;
        public virtual Assembly Assembly { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<CustomAttributeData> CustomAttributes { get { throw null; } }

        public virtual string FullyQualifiedName { get { throw null; } }

        public virtual Guid ModuleVersionId { get { throw null; } }

        public virtual string Name { get { throw null; } }

        public virtual string ScopeName { get { throw null; } }

        public override bool Equals(object o) { throw null; }

        public virtual Type[] FindTypes(TypeFilter filter, object filterCriteria) { throw null; }

        public virtual FieldInfo GetField(string name, BindingFlags bindingAttr) { throw null; }

        public FieldInfo GetField(string name) { throw null; }

        public FieldInfo[] GetFields() { throw null; }

        public virtual FieldInfo[] GetFields(BindingFlags bindingFlags) { throw null; }

        public override int GetHashCode() { throw null; }

        public MethodInfo GetMethod(string name, Type[] types) { throw null; }

        public MethodInfo GetMethod(string name) { throw null; }

        public MethodInfo[] GetMethods() { throw null; }

        public virtual MethodInfo[] GetMethods(BindingFlags bindingFlags) { throw null; }

        public virtual Type GetType(string className, bool throwOnError, bool ignoreCase) { throw null; }

        public virtual Type GetType(string className, bool ignoreCase) { throw null; }

        public virtual Type GetType(string className) { throw null; }

        public virtual Type[] GetTypes() { throw null; }

        object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit) { throw null; }

        object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit) { throw null; }

        bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class ParameterInfo : ICustomAttributeProvider
    {
        internal ParameterInfo() { }

        public virtual ParameterAttributes Attributes { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<CustomAttributeData> CustomAttributes { get { throw null; } }

        public virtual object DefaultValue { get { throw null; } }

        public virtual bool HasDefaultValue { get { throw null; } }

        public bool IsIn { get { throw null; } }

        public bool IsOptional { get { throw null; } }

        public bool IsOut { get { throw null; } }

        public bool IsRetval { get { throw null; } }

        public virtual MemberInfo Member { get { throw null; } }

        public virtual string Name { get { throw null; } }

        public virtual Type ParameterType { get { throw null; } }

        public virtual int Position { get { throw null; } }

        public virtual object RawDefaultValue { get { throw null; } }

        public virtual Type[] GetOptionalCustomModifiers() { throw null; }

        public virtual Type[] GetRequiredCustomModifiers() { throw null; }

        object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit) { throw null; }

        object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit) { throw null; }

        bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit) { throw null; }
    }

    public partial struct ParameterModifier
    {
        public ParameterModifier(int parameterCount) { }

        public bool this[int index] { get { throw null; } set { } }
    }

    public abstract partial class PropertyInfo : MemberInfo
    {
        internal PropertyInfo() { }

        public abstract PropertyAttributes Attributes { get; }
        public abstract bool CanRead { get; }
        public abstract bool CanWrite { get; }

        public virtual MethodInfo GetMethod { get { throw null; } }

        public bool IsSpecialName { get { throw null; } }

        public override MemberTypes MemberType { get { throw null; } }

        public abstract Type PropertyType { get; }

        public virtual MethodInfo SetMethod { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public MethodInfo[] GetAccessors() { throw null; }

        public abstract MethodInfo[] GetAccessors(bool nonPublic);
        public virtual object GetConstantValue() { throw null; }

        public MethodInfo GetGetMethod() { throw null; }

        public abstract MethodInfo GetGetMethod(bool nonPublic);
        public override int GetHashCode() { throw null; }

        public abstract ParameterInfo[] GetIndexParameters();
        public virtual Type[] GetOptionalCustomModifiers() { throw null; }

        public virtual object GetRawConstantValue() { throw null; }

        public virtual Type[] GetRequiredCustomModifiers() { throw null; }

        public MethodInfo GetSetMethod() { throw null; }

        public abstract MethodInfo GetSetMethod(bool nonPublic);
        public virtual object GetValue(object obj, object[] index) { throw null; }

        public object GetValue(object obj) { throw null; }

        public virtual void SetValue(object obj, object value, object[] index) { }

        public void SetValue(object obj, object value) { }
    }

    public abstract partial class ReflectionContext
    {
        public virtual TypeInfo GetTypeForObject(object value) { throw null; }

        public abstract Assembly MapAssembly(Assembly assembly);
        public abstract TypeInfo MapType(TypeInfo type);
    }

    public sealed partial class ReflectionTypeLoadException : Exception
    {
        public ReflectionTypeLoadException(Type[] classes, Exception[] exceptions, string message) { }

        public ReflectionTypeLoadException(Type[] classes, Exception[] exceptions) { }

        public Exception[] LoaderExceptions { get { throw null; } }

        public Type[] Types { get { throw null; } }
    }

    [Flags]
    public enum ResourceLocation
    {
        Embedded = 1,
        ContainedInAnotherAssembly = 2,
        ContainedInManifestFile = 4
    }

    public partial class TargetException : Exception
    {
        public TargetException() { }

        public TargetException(string message, Exception inner) { }

        public TargetException(string message) { }
    }

    public sealed partial class TargetInvocationException : Exception
    {
        public TargetInvocationException(Exception inner) { }

        public TargetInvocationException(string message, Exception inner) { }
    }

    public sealed partial class TargetParameterCountException : Exception
    {
        public TargetParameterCountException() { }

        public TargetParameterCountException(string message, Exception inner) { }

        public TargetParameterCountException(string message) { }
    }

    public delegate bool TypeFilter(Type m, object filterCriteria);
    public abstract partial class TypeInfo : MemberInfo, IReflectableType
    {
        internal TypeInfo() { }

        public abstract Assembly Assembly { get; }
        public abstract string AssemblyQualifiedName { get; }
        public abstract TypeAttributes Attributes { get; }
        public abstract Type BaseType { get; }
        public abstract bool ContainsGenericParameters { get; }

        public virtual Collections.Generic.IEnumerable<ConstructorInfo> DeclaredConstructors { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<EventInfo> DeclaredEvents { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<FieldInfo> DeclaredFields { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<MemberInfo> DeclaredMembers { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<MethodInfo> DeclaredMethods { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<TypeInfo> DeclaredNestedTypes { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<PropertyInfo> DeclaredProperties { get { throw null; } }

        public abstract MethodBase DeclaringMethod { get; }
        public abstract string FullName { get; }
        public abstract GenericParameterAttributes GenericParameterAttributes { get; }
        public abstract int GenericParameterPosition { get; }
        public abstract Type[] GenericTypeArguments { get; }

        public virtual Type[] GenericTypeParameters { get { throw null; } }

        public abstract Guid GUID { get; }

        public bool HasElementType { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<Type> ImplementedInterfaces { get { throw null; } }

        public bool IsAbstract { get { throw null; } }

        public bool IsAnsiClass { get { throw null; } }

        public bool IsArray { get { throw null; } }

        public bool IsAutoClass { get { throw null; } }

        public bool IsAutoLayout { get { throw null; } }

        public bool IsByRef { get { throw null; } }

        public bool IsClass { get { throw null; } }

        public virtual bool IsCOMObject { get { throw null; } }

        public abstract bool IsEnum { get; }

        public bool IsExplicitLayout { get { throw null; } }

        public abstract bool IsGenericParameter { get; }
        public abstract bool IsGenericType { get; }
        public abstract bool IsGenericTypeDefinition { get; }

        public bool IsImport { get { throw null; } }

        public bool IsInterface { get { throw null; } }

        public bool IsLayoutSequential { get { throw null; } }

        public bool IsMarshalByRef { get { throw null; } }

        public bool IsNested { get { throw null; } }

        public bool IsNestedAssembly { get { throw null; } }

        public bool IsNestedFamANDAssem { get { throw null; } }

        public bool IsNestedFamily { get { throw null; } }

        public bool IsNestedFamORAssem { get { throw null; } }

        public bool IsNestedPrivate { get { throw null; } }

        public bool IsNestedPublic { get { throw null; } }

        public bool IsNotPublic { get { throw null; } }

        public bool IsPointer { get { throw null; } }

        public virtual bool IsPrimitive { get { throw null; } }

        public bool IsPublic { get { throw null; } }

        public bool IsSealed { get { throw null; } }

        public abstract bool IsSerializable { get; }

        public bool IsSpecialName { get { throw null; } }

        public bool IsUnicodeClass { get { throw null; } }

        public virtual bool IsValueType { get { throw null; } }

        public bool IsVisible { get { throw null; } }

        public override MemberTypes MemberType { get { throw null; } }

        public abstract string Namespace { get; }

        public virtual Runtime.InteropServices.StructLayoutAttribute StructLayoutAttribute { get { throw null; } }

        public ConstructorInfo TypeInitializer { get { throw null; } }

        public virtual Type UnderlyingSystemType { get { throw null; } }

        public virtual Type AsType() { throw null; }

        public virtual Type[] FindInterfaces(TypeFilter filter, object filterCriteria) { throw null; }

        public virtual MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria) { throw null; }

        public abstract int GetArrayRank();
        public ConstructorInfo GetConstructor(Type[] types) { throw null; }

        public ConstructorInfo[] GetConstructors() { throw null; }

        public virtual ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) { throw null; }

        public virtual EventInfo GetDeclaredEvent(string name) { throw null; }

        public virtual FieldInfo GetDeclaredField(string name) { throw null; }

        public virtual MethodInfo GetDeclaredMethod(string name) { throw null; }

        public virtual Collections.Generic.IEnumerable<MethodInfo> GetDeclaredMethods(string name) { throw null; }

        public virtual TypeInfo GetDeclaredNestedType(string name) { throw null; }

        public virtual PropertyInfo GetDeclaredProperty(string name) { throw null; }

        public virtual MemberInfo[] GetDefaultMembers() { throw null; }

        public abstract Type GetElementType();
        public virtual string GetEnumName(object value) { throw null; }

        public virtual string[] GetEnumNames() { throw null; }

        public virtual Type GetEnumUnderlyingType() { throw null; }

        public virtual Array GetEnumValues() { throw null; }

        public virtual EventInfo GetEvent(string name, BindingFlags bindingAttr) { throw null; }

        public EventInfo GetEvent(string name) { throw null; }

        public virtual EventInfo[] GetEvents() { throw null; }

        public virtual EventInfo[] GetEvents(BindingFlags bindingAttr) { throw null; }

        public virtual FieldInfo GetField(string name, BindingFlags bindingAttr) { throw null; }

        public FieldInfo GetField(string name) { throw null; }

        public FieldInfo[] GetFields() { throw null; }

        public virtual FieldInfo[] GetFields(BindingFlags bindingAttr) { throw null; }

        public virtual Type[] GetGenericArguments() { throw null; }

        public abstract Type[] GetGenericParameterConstraints();
        public abstract Type GetGenericTypeDefinition();
        public virtual Type GetInterface(string name, bool ignoreCase) { throw null; }

        public Type GetInterface(string name) { throw null; }

        public virtual Type[] GetInterfaces() { throw null; }

        public virtual MemberInfo[] GetMember(string name, BindingFlags bindingAttr) { throw null; }

        public virtual MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr) { throw null; }

        public MemberInfo[] GetMember(string name) { throw null; }

        public MemberInfo[] GetMembers() { throw null; }

        public virtual MemberInfo[] GetMembers(BindingFlags bindingAttr) { throw null; }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr) { throw null; }

        public MethodInfo GetMethod(string name, Type[] types, ParameterModifier[] modifiers) { throw null; }

        public MethodInfo GetMethod(string name, Type[] types) { throw null; }

        public MethodInfo GetMethod(string name) { throw null; }

        public MethodInfo[] GetMethods() { throw null; }

        public virtual MethodInfo[] GetMethods(BindingFlags bindingAttr) { throw null; }

        public virtual Type GetNestedType(string name, BindingFlags bindingAttr) { throw null; }

        public Type GetNestedType(string name) { throw null; }

        public Type[] GetNestedTypes() { throw null; }

        public virtual Type[] GetNestedTypes(BindingFlags bindingAttr) { throw null; }

        public PropertyInfo[] GetProperties() { throw null; }

        public virtual PropertyInfo[] GetProperties(BindingFlags bindingAttr) { throw null; }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr) { throw null; }

        public PropertyInfo GetProperty(string name, Type returnType, Type[] types, ParameterModifier[] modifiers) { throw null; }

        public PropertyInfo GetProperty(string name, Type returnType, Type[] types) { throw null; }

        public PropertyInfo GetProperty(string name, Type returnType) { throw null; }

        public PropertyInfo GetProperty(string name, Type[] types) { throw null; }

        public PropertyInfo GetProperty(string name) { throw null; }

        public virtual bool IsAssignableFrom(TypeInfo typeInfo) { throw null; }

        public virtual bool IsAssignableFrom(Type c) { throw null; }

        public virtual bool IsEnumDefined(object value) { throw null; }

        public virtual bool IsEquivalentTo(Type other) { throw null; }

        public virtual bool IsInstanceOfType(object o) { throw null; }

        public virtual bool IsSubclassOf(Type c) { throw null; }

        public abstract Type MakeArrayType();
        public abstract Type MakeArrayType(int rank);
        public abstract Type MakeByRefType();
        public abstract Type MakeGenericType(params Type[] typeArguments);
        public abstract Type MakePointerType();
        TypeInfo IReflectableType.GetTypeInfo() { throw null; }
    }
}