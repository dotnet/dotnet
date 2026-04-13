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
[assembly: System.Reflection.AssemblyTitle("System.Reflection.TypeExtensions")]
[assembly: System.Reflection.AssemblyDescription("System.Reflection.TypeExtensions")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection.TypeExtensions")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("4.6.23123.00")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.6.23123.00 built by: PROJECTKREL")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata("", "")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Reflection
{
    public static partial class AssemblyExtensions
    {
        public static Type[] GetExportedTypes(this Assembly assembly) { throw null; }

        public static Module[] GetModules(this Assembly assembly) { throw null; }

        public static Type[] GetTypes(this Assembly assembly) { throw null; }
    }

    [Flags]
    public enum BindingFlags
    {
        IgnoreCase = 1,
        DeclaredOnly = 2,
        Instance = 4,
        Static = 8,
        Public = 16,
        NonPublic = 32,
        FlattenHierarchy = 64
    }

    public static partial class EventInfoExtensions
    {
        public static MethodInfo GetAddMethod(this EventInfo eventInfo, bool nonPublic) { throw null; }

        public static MethodInfo GetAddMethod(this EventInfo eventInfo) { throw null; }

        public static MethodInfo GetRaiseMethod(this EventInfo eventInfo, bool nonPublic) { throw null; }

        public static MethodInfo GetRaiseMethod(this EventInfo eventInfo) { throw null; }

        public static MethodInfo GetRemoveMethod(this EventInfo eventInfo, bool nonPublic) { throw null; }

        public static MethodInfo GetRemoveMethod(this EventInfo eventInfo) { throw null; }
    }

    public static partial class MethodInfoExtensions
    {
        public static MethodInfo GetBaseDefinition(this MethodInfo method) { throw null; }
    }

    public static partial class PropertyInfoExtensions
    {
        public static MethodInfo[] GetAccessors(this PropertyInfo property, bool nonPublic) { throw null; }

        public static MethodInfo[] GetAccessors(this PropertyInfo property) { throw null; }

        public static MethodInfo GetGetMethod(this PropertyInfo property, bool nonPublic) { throw null; }

        public static MethodInfo GetGetMethod(this PropertyInfo property) { throw null; }

        public static MethodInfo GetSetMethod(this PropertyInfo property, bool nonPublic) { throw null; }

        public static MethodInfo GetSetMethod(this PropertyInfo property) { throw null; }
    }

    public static partial class TypeExtensions
    {
        public static ConstructorInfo GetConstructor(this Type type, Type[] types) { throw null; }

        public static ConstructorInfo[] GetConstructors(this Type type, BindingFlags bindingAttr) { throw null; }

        public static ConstructorInfo[] GetConstructors(this Type type) { throw null; }

        public static MemberInfo[] GetDefaultMembers(this Type type) { throw null; }

        public static EventInfo GetEvent(this Type type, string name, BindingFlags bindingAttr) { throw null; }

        public static EventInfo GetEvent(this Type type, string name) { throw null; }

        public static EventInfo[] GetEvents(this Type type, BindingFlags bindingAttr) { throw null; }

        public static EventInfo[] GetEvents(this Type type) { throw null; }

        public static FieldInfo GetField(this Type type, string name, BindingFlags bindingAttr) { throw null; }

        public static FieldInfo GetField(this Type type, string name) { throw null; }

        public static FieldInfo[] GetFields(this Type type, BindingFlags bindingAttr) { throw null; }

        public static FieldInfo[] GetFields(this Type type) { throw null; }

        public static Type[] GetGenericArguments(this Type type) { throw null; }

        public static Type[] GetInterfaces(this Type type) { throw null; }

        public static MemberInfo[] GetMember(this Type type, string name, BindingFlags bindingAttr) { throw null; }

        public static MemberInfo[] GetMember(this Type type, string name) { throw null; }

        public static MemberInfo[] GetMembers(this Type type, BindingFlags bindingAttr) { throw null; }

        public static MemberInfo[] GetMembers(this Type type) { throw null; }

        public static MethodInfo GetMethod(this Type type, string name, BindingFlags bindingAttr) { throw null; }

        public static MethodInfo GetMethod(this Type type, string name, Type[] types) { throw null; }

        public static MethodInfo GetMethod(this Type type, string name) { throw null; }

        public static MethodInfo[] GetMethods(this Type type, BindingFlags bindingAttr) { throw null; }

        public static MethodInfo[] GetMethods(this Type type) { throw null; }

        public static Type GetNestedType(this Type type, string name, BindingFlags bindingAttr) { throw null; }

        public static Type[] GetNestedTypes(this Type type, BindingFlags bindingAttr) { throw null; }

        public static PropertyInfo[] GetProperties(this Type type, BindingFlags bindingAttr) { throw null; }

        public static PropertyInfo[] GetProperties(this Type type) { throw null; }

        public static PropertyInfo GetProperty(this Type type, string name, BindingFlags bindingAttr) { throw null; }

        public static PropertyInfo GetProperty(this Type type, string name, Type returnType, Type[] types) { throw null; }

        public static PropertyInfo GetProperty(this Type type, string name, Type returnType) { throw null; }

        public static PropertyInfo GetProperty(this Type type, string name) { throw null; }

        public static bool IsAssignableFrom(this Type type, Type c) { throw null; }

        public static bool IsInstanceOfType(this Type type, object o) { throw null; }
    }
}