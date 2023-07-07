// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Reflection.AssemblyDescription("System.Diagnostics.Debug.dll")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Diagnostics.Debug.dll")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyTitle("System.Diagnostics.Debug.dll")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.30319.17929")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Diagnostics
{
    public static partial class Debug
    {
        public static void Assert(bool condition, string message) { }

        public static void Assert(bool condition) { }

        public static void WriteLine(object value) { }

        public static void WriteLine(string format, params object[] args) { }

        public static void WriteLine(string message) { }

        public static void WriteLineIf(bool condition, string message) { }
    }

    public static partial class Debugger
    {
        public static bool IsAttached { get { throw null; } }

        public static void Break() { }

        public static bool Launch() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed partial class DebuggerBrowsableAttribute : Attribute
    {
        public DebuggerBrowsableAttribute(DebuggerBrowsableState state) { }

        public DebuggerBrowsableState State { get { throw null; } }
    }

    public enum DebuggerBrowsableState
    {
        Never = 0,
        Collapsed = 2,
        RootHidden = 3
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Delegate, AllowMultiple = true)]
    public sealed partial class DebuggerDisplayAttribute : Attribute
    {
        public DebuggerDisplayAttribute(string value) { }

        public string Name { get { throw null; } set { } }

        public Type Target { get { throw null; } set { } }

        public string TargetTypeName { get { throw null; } set { } }

        public string Type { get { throw null; } set { } }

        public string Value { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
    public sealed partial class DebuggerHiddenAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
    public sealed partial class DebuggerNonUserCodeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
    public sealed partial class DebuggerStepThroughAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public sealed partial class DebuggerTypeProxyAttribute : Attribute
    {
        public DebuggerTypeProxyAttribute(string typeName) { }

        public DebuggerTypeProxyAttribute(Type type) { }

        public string ProxyTypeName { get { throw null; } }

        public Type Target { get { throw null; } set { } }

        public string TargetTypeName { get { throw null; } set { } }
    }
}