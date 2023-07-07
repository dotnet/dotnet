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
[assembly: System.Reflection.AssemblyTitle("System.Reflection.Emit.Lightweight")]
[assembly: System.Reflection.AssemblyDescription("System.Reflection.Emit.Lightweight")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Reflection.Emit.Lightweight")]
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
    public sealed partial class DynamicMethod : MethodInfo
    {
        public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility) { }

        public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility) { }

        public DynamicMethod(string name, Type returnType, Type[] parameterTypes, bool restrictedSkipVisibility) { }

        public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility) { }

        public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m) { }

        public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility) { }

        public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner) { }

        public DynamicMethod(string name, Type returnType, Type[] parameterTypes) { }

        public override MethodAttributes Attributes { get { throw null; } }

        public override CallingConventions CallingConvention { get { throw null; } }

        public override Type DeclaringType { get { throw null; } }

        public bool InitLocals { get { throw null; } set { } }

        public override MethodImplAttributes MethodImplementationFlags { get { throw null; } }

        public override string Name { get { throw null; } }

        public override ParameterInfo ReturnParameter { get { throw null; } }

        public override Type ReturnType { get { throw null; } }

        public sealed override Delegate CreateDelegate(Type delegateType, object target) { throw null; }

        public sealed override Delegate CreateDelegate(Type delegateType) { throw null; }

        public ILGenerator GetILGenerator() { throw null; }

        public ILGenerator GetILGenerator(int streamSize) { throw null; }

        public override ParameterInfo[] GetParameters() { throw null; }

        public override string ToString() { throw null; }
    }
}