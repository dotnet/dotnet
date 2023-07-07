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
[assembly: System.Reflection.AssemblyFileVersion("4.6.23123.00")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.23123.00 built by: PROJECTKREL")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("", "")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.10.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Reflection
{
    public sealed partial class AmbiguousMatchException : Exception
    {
        public AmbiguousMatchException() { }

        public AmbiguousMatchException(string message, Exception inner) { }

        public AmbiguousMatchException(string message) { }
    }

    public abstract partial class Assembly
    {
        internal Assembly() { }

        public virtual Collections.Generic.IEnumerable<CustomAttributeData> CustomAttributes { get { throw null; } }

        public abstract Collections.Generic.IEnumerable<TypeInfo> DefinedTypes { get; }

        public virtual Collections.Generic.IEnumerable<Type> ExportedTypes { get { throw null; } }

        public virtual string FullName { get { throw null; } }

        public virtual bool IsDynamic { get { throw null; } }

        public virtual Module ManifestModule { get { throw null; } }

        public abstract Collections.Generic.IEnumerable<Module> Modules { get; }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public virtual ManifestResourceInfo GetManifestResourceInfo(string resourceName) { throw null; }

        public virtual string[] GetManifestResourceNames() { throw null; }

        public virtual IO.Stream GetManifestResourceStream(string name) { throw null; }

        public virtual AssemblyName GetName() { throw null; }

        public virtual Type GetType(string name, bool throwOnError, bool ignoreCase) { throw null; }

        public virtual Type GetType(string name) { throw null; }

        public static Assembly Load(AssemblyName assemblyRef) { throw null; }

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

    public abstract partial class ConstructorInfo : MethodBase
    {
        internal ConstructorInfo() { }

        public static readonly string ConstructorName;
        public static readonly string TypeConstructorName;
        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

        public virtual object Invoke(object[] parameters) { throw null; }
    }

    public partial class CustomAttributeData
    {
        internal CustomAttributeData() { }

        public virtual Type AttributeType { get { throw null; } }

        public virtual Collections.Generic.IList<CustomAttributeTypedArgument> ConstructorArguments { get { throw null; } }

        public virtual Collections.Generic.IList<CustomAttributeNamedArgument> NamedArguments { get { throw null; } }
    }

    public partial struct CustomAttributeNamedArgument
    {
        public bool IsField { get { throw null; } }

        public string MemberName { get { throw null; } }

        public CustomAttributeTypedArgument TypedValue { get { throw null; } }
    }

    public partial struct CustomAttributeTypedArgument
    {
        public Type ArgumentType { get { throw null; } }

        public object Value { get { throw null; } }
    }

    public abstract partial class EventInfo : MemberInfo
    {
        internal EventInfo() { }

        public virtual MethodInfo AddMethod { get { throw null; } }

        public abstract EventAttributes Attributes { get; }

        public virtual Type EventHandlerType { get { throw null; } }

        public bool IsSpecialName { get { throw null; } }

        public virtual MethodInfo RaiseMethod { get { throw null; } }

        public virtual MethodInfo RemoveMethod { get { throw null; } }

        public virtual void AddEventHandler(object target, Delegate handler) { }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }

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

        public override bool Equals(object obj) { throw null; }

        public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle, RuntimeTypeHandle declaringType) { throw null; }

        public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle) { throw null; }

        public override int GetHashCode() { throw null; }

        public abstract object GetValue(object obj);
        public virtual void SetValue(object obj, object value) { }
    }

    public static partial class IntrospectionExtensions
    {
        public static TypeInfo GetTypeInfo(this Type type) { throw null; }
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

    public abstract partial class MemberInfo
    {
        internal MemberInfo() { }

        public virtual Collections.Generic.IEnumerable<CustomAttributeData> CustomAttributes { get { throw null; } }

        public abstract Type DeclaringType { get; }

        public virtual Module Module { get { throw null; } }

        public abstract string Name { get; }

        public override bool Equals(object obj) { throw null; }

        public override int GetHashCode() { throw null; }
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

        public abstract ParameterInfo[] GetParameters();
        public virtual object Invoke(object obj, object[] parameters) { throw null; }
    }

    public abstract partial class MethodInfo : MethodBase
    {
        internal MethodInfo() { }

        public virtual ParameterInfo ReturnParameter { get { throw null; } }

        public virtual Type ReturnType { get { throw null; } }

        public virtual Delegate CreateDelegate(Type delegateType, object target) { throw null; }

        public virtual Delegate CreateDelegate(Type delegateType) { throw null; }

        public override bool Equals(object obj) { throw null; }

        public override Type[] GetGenericArguments() { throw null; }

        public virtual MethodInfo GetGenericMethodDefinition() { throw null; }

        public override int GetHashCode() { throw null; }

        public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments) { throw null; }
    }

    public abstract partial class Module
    {
        internal Module() { }

        public virtual Assembly Assembly { get { throw null; } }

        public virtual Collections.Generic.IEnumerable<CustomAttributeData> CustomAttributes { get { throw null; } }

        public virtual string FullyQualifiedName { get { throw null; } }

        public virtual string Name { get { throw null; } }

        public override bool Equals(object o) { throw null; }

        public override int GetHashCode() { throw null; }

        public virtual Type GetType(string className, bool throwOnError, bool ignoreCase) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class ParameterInfo
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
    }

    public abstract partial class PropertyInfo : MemberInfo
    {
        internal PropertyInfo() { }

        public abstract PropertyAttributes Attributes { get; }
        public abstract bool CanRead { get; }
        public abstract bool CanWrite { get; }

        public virtual MethodInfo GetMethod { get { throw null; } }

        public bool IsSpecialName { get { throw null; } }

        public abstract Type PropertyType { get; }

        public virtual MethodInfo SetMethod { get { throw null; } }

        public override bool Equals(object obj) { throw null; }

        public virtual object GetConstantValue() { throw null; }

        public override int GetHashCode() { throw null; }

        public abstract ParameterInfo[] GetIndexParameters();
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

        public abstract string Namespace { get; }

        public virtual Type AsType() { throw null; }

        public abstract int GetArrayRank();
        public virtual EventInfo GetDeclaredEvent(string name) { throw null; }

        public virtual FieldInfo GetDeclaredField(string name) { throw null; }

        public virtual MethodInfo GetDeclaredMethod(string name) { throw null; }

        public virtual Collections.Generic.IEnumerable<MethodInfo> GetDeclaredMethods(string name) { throw null; }

        public virtual TypeInfo GetDeclaredNestedType(string name) { throw null; }

        public virtual PropertyInfo GetDeclaredProperty(string name) { throw null; }

        public abstract Type GetElementType();
        public abstract Type[] GetGenericParameterConstraints();
        public abstract Type GetGenericTypeDefinition();
        public virtual bool IsAssignableFrom(TypeInfo typeInfo) { throw null; }

        public virtual bool IsSubclassOf(Type c) { throw null; }

        public abstract Type MakeArrayType();
        public abstract Type MakeArrayType(int rank);
        public abstract Type MakeByRefType();
        public abstract Type MakeGenericType(params Type[] typeArguments);
        public abstract Type MakePointerType();
        TypeInfo IReflectableType.GetTypeInfo() { throw null; }
    }
}