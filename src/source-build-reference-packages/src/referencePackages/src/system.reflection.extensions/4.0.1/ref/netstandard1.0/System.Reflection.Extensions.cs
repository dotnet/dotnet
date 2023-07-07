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
[assembly: System.Reflection.AssemblyTitle("System.Reflection.Extensions")]
[assembly: System.Reflection.AssemblyDescription("System.Reflection.Extensions")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection.Extensions")]
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
namespace System.Reflection
{
    public static partial class CustomAttributeExtensions
    {
        public static Attribute GetCustomAttribute(this Assembly element, Type attributeType) { throw null; }

        public static Attribute GetCustomAttribute(this MemberInfo element, Type attributeType, bool inherit) { throw null; }

        public static Attribute GetCustomAttribute(this MemberInfo element, Type attributeType) { throw null; }

        public static Attribute GetCustomAttribute(this Module element, Type attributeType) { throw null; }

        public static Attribute GetCustomAttribute(this ParameterInfo element, Type attributeType, bool inherit) { throw null; }

        public static Attribute GetCustomAttribute(this ParameterInfo element, Type attributeType) { throw null; }

        public static T GetCustomAttribute<T>(this Assembly element)
            where T : Attribute { throw null; }

        public static T GetCustomAttribute<T>(this MemberInfo element, bool inherit)
            where T : Attribute { throw null; }

        public static T GetCustomAttribute<T>(this MemberInfo element)
            where T : Attribute { throw null; }

        public static T GetCustomAttribute<T>(this Module element)
            where T : Attribute { throw null; }

        public static T GetCustomAttribute<T>(this ParameterInfo element, bool inherit)
            where T : Attribute { throw null; }

        public static T GetCustomAttribute<T>(this ParameterInfo element)
            where T : Attribute { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this Assembly element, Type attributeType) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this Assembly element) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, bool inherit) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, Type attributeType, bool inherit) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, Type attributeType) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this Module element, Type attributeType) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this Module element) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element, bool inherit) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element, Type attributeType, bool inherit) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element, Type attributeType) { throw null; }

        public static Collections.Generic.IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element) { throw null; }

        public static Collections.Generic.IEnumerable<T> GetCustomAttributes<T>(this Assembly element)
            where T : Attribute { throw null; }

        public static Collections.Generic.IEnumerable<T> GetCustomAttributes<T>(this MemberInfo element, bool inherit)
            where T : Attribute { throw null; }

        public static Collections.Generic.IEnumerable<T> GetCustomAttributes<T>(this MemberInfo element)
            where T : Attribute { throw null; }

        public static Collections.Generic.IEnumerable<T> GetCustomAttributes<T>(this Module element)
            where T : Attribute { throw null; }

        public static Collections.Generic.IEnumerable<T> GetCustomAttributes<T>(this ParameterInfo element, bool inherit)
            where T : Attribute { throw null; }

        public static Collections.Generic.IEnumerable<T> GetCustomAttributes<T>(this ParameterInfo element)
            where T : Attribute { throw null; }

        public static bool IsDefined(this Assembly element, Type attributeType) { throw null; }

        public static bool IsDefined(this MemberInfo element, Type attributeType, bool inherit) { throw null; }

        public static bool IsDefined(this MemberInfo element, Type attributeType) { throw null; }

        public static bool IsDefined(this Module element, Type attributeType) { throw null; }

        public static bool IsDefined(this ParameterInfo element, Type attributeType, bool inherit) { throw null; }

        public static bool IsDefined(this ParameterInfo element, Type attributeType) { throw null; }
    }

    public partial struct InterfaceMapping
    {
        public MethodInfo[] InterfaceMethods;
        public Type InterfaceType;
        public MethodInfo[] TargetMethods;
        public Type TargetType;
    }

    public static partial class RuntimeReflectionExtensions
    {
        public static MethodInfo GetMethodInfo(this Delegate del) { throw null; }

        public static MethodInfo GetRuntimeBaseDefinition(this MethodInfo method) { throw null; }

        public static EventInfo GetRuntimeEvent(this Type type, string name) { throw null; }

        public static Collections.Generic.IEnumerable<EventInfo> GetRuntimeEvents(this Type type) { throw null; }

        public static FieldInfo GetRuntimeField(this Type type, string name) { throw null; }

        public static Collections.Generic.IEnumerable<FieldInfo> GetRuntimeFields(this Type type) { throw null; }

        public static InterfaceMapping GetRuntimeInterfaceMap(this TypeInfo typeInfo, Type interfaceType) { throw null; }

        public static MethodInfo GetRuntimeMethod(this Type type, string name, Type[] parameters) { throw null; }

        public static Collections.Generic.IEnumerable<MethodInfo> GetRuntimeMethods(this Type type) { throw null; }

        public static Collections.Generic.IEnumerable<PropertyInfo> GetRuntimeProperties(this Type type) { throw null; }

        public static PropertyInfo GetRuntimeProperty(this Type type, string name) { throw null; }
    }
}