// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Reflection.TypeExtensions")]
[assembly: AssemblyDescription("System.Reflection.TypeExtensions")]
[assembly: AssemblyDefaultAlias("System.Reflection.TypeExtensions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.25519.03")]
[assembly: AssemblyInformationalVersion("4.6.25519.03 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.2.0")]

[assembly: TypeForwardedTo(typeof(System.Reflection.BindingFlags))]



namespace System.Reflection
{
    public static partial class AssemblyExtensions
    {
        public static System.Type[] GetExportedTypes(this System.Reflection.Assembly assembly) { throw null; }
        public static System.Reflection.Module[] GetModules(this System.Reflection.Assembly assembly) { throw null; }
        public static System.Type[] GetTypes(this System.Reflection.Assembly assembly) { throw null; }
    }
    public static partial class EventInfoExtensions
    {
        public static System.Reflection.MethodInfo GetAddMethod(this System.Reflection.EventInfo eventInfo) { throw null; }
        public static System.Reflection.MethodInfo GetAddMethod(this System.Reflection.EventInfo eventInfo, bool nonPublic) { throw null; }
        public static System.Reflection.MethodInfo GetRaiseMethod(this System.Reflection.EventInfo eventInfo) { throw null; }
        public static System.Reflection.MethodInfo GetRaiseMethod(this System.Reflection.EventInfo eventInfo, bool nonPublic) { throw null; }
        public static System.Reflection.MethodInfo GetRemoveMethod(this System.Reflection.EventInfo eventInfo) { throw null; }
        public static System.Reflection.MethodInfo GetRemoveMethod(this System.Reflection.EventInfo eventInfo, bool nonPublic) { throw null; }
    }
    public static partial class MemberInfoExtensions
    {
        public static int GetMetadataToken(this System.Reflection.MemberInfo member) { throw null; }
        public static bool HasMetadataToken(this System.Reflection.MemberInfo member) { throw null; }
    }
    public static partial class MethodInfoExtensions
    {
        public static System.Reflection.MethodInfo GetBaseDefinition(this System.Reflection.MethodInfo method) { throw null; }
    }
    public static partial class ModuleExtensions
    {
        public static System.Guid GetModuleVersionId(this System.Reflection.Module module) { throw null; }
        public static bool HasModuleVersionId(this System.Reflection.Module module) { throw null; }
    }
    public static partial class PropertyInfoExtensions
    {
        public static System.Reflection.MethodInfo[] GetAccessors(this System.Reflection.PropertyInfo property) { throw null; }
        public static System.Reflection.MethodInfo[] GetAccessors(this System.Reflection.PropertyInfo property, bool nonPublic) { throw null; }
        public static System.Reflection.MethodInfo GetGetMethod(this System.Reflection.PropertyInfo property) { throw null; }
        public static System.Reflection.MethodInfo GetGetMethod(this System.Reflection.PropertyInfo property, bool nonPublic) { throw null; }
        public static System.Reflection.MethodInfo GetSetMethod(this System.Reflection.PropertyInfo property) { throw null; }
        public static System.Reflection.MethodInfo GetSetMethod(this System.Reflection.PropertyInfo property, bool nonPublic) { throw null; }
    }
    public static partial class TypeExtensions
    {
        public static System.Reflection.ConstructorInfo GetConstructor(this System.Type type, System.Type[] types) { throw null; }
        public static System.Reflection.ConstructorInfo[] GetConstructors(this System.Type type) { throw null; }
        public static System.Reflection.ConstructorInfo[] GetConstructors(this System.Type type, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Reflection.MemberInfo[] GetDefaultMembers(this System.Type type) { throw null; }
        public static System.Reflection.EventInfo GetEvent(this System.Type type, string name) { throw null; }
        public static System.Reflection.EventInfo GetEvent(this System.Type type, string name, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Reflection.EventInfo[] GetEvents(this System.Type type) { throw null; }
        public static System.Reflection.EventInfo[] GetEvents(this System.Type type, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Reflection.FieldInfo GetField(this System.Type type, string name) { throw null; }
        public static System.Reflection.FieldInfo GetField(this System.Type type, string name, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Reflection.FieldInfo[] GetFields(this System.Type type) { throw null; }
        public static System.Reflection.FieldInfo[] GetFields(this System.Type type, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Type[] GetGenericArguments(this System.Type type) { throw null; }
        public static System.Type[] GetInterfaces(this System.Type type) { throw null; }
        public static System.Reflection.MemberInfo[] GetMember(this System.Type type, string name) { throw null; }
        public static System.Reflection.MemberInfo[] GetMember(this System.Type type, string name, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Reflection.MemberInfo[] GetMembers(this System.Type type) { throw null; }
        public static System.Reflection.MemberInfo[] GetMembers(this System.Type type, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Reflection.MethodInfo GetMethod(this System.Type type, string name) { throw null; }
        public static System.Reflection.MethodInfo GetMethod(this System.Type type, string name, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Reflection.MethodInfo GetMethod(this System.Type type, string name, System.Type[] types) { throw null; }
        public static System.Reflection.MethodInfo[] GetMethods(this System.Type type) { throw null; }
        public static System.Reflection.MethodInfo[] GetMethods(this System.Type type, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Type GetNestedType(this System.Type type, string name, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Type[] GetNestedTypes(this System.Type type, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Reflection.PropertyInfo[] GetProperties(this System.Type type) { throw null; }
        public static System.Reflection.PropertyInfo[] GetProperties(this System.Type type, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Reflection.PropertyInfo GetProperty(this System.Type type, string name) { throw null; }
        public static System.Reflection.PropertyInfo GetProperty(this System.Type type, string name, System.Reflection.BindingFlags bindingAttr) { throw null; }
        public static System.Reflection.PropertyInfo GetProperty(this System.Type type, string name, System.Type returnType) { throw null; }
        public static System.Reflection.PropertyInfo GetProperty(this System.Type type, string name, System.Type returnType, System.Type[] types) { throw null; }
        public static bool IsAssignableFrom(this System.Type type, System.Type c) { throw null; }
        public static bool IsInstanceOfType(this System.Type type, object o) { throw null; }
    }
}
