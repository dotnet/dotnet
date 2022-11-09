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
[assembly: AssemblyTitle("System.Reflection.Emit.Lightweight")]
[assembly: AssemblyDescription("System.Reflection.Emit.Lightweight")]
[assembly: AssemblyDefaultAlias("System.Reflection.Emit.Lightweight")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace System.Reflection.Emit
{
    public sealed partial class DynamicMethod : System.Reflection.MethodInfo
    {
        public DynamicMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, System.Type returnType, System.Type[] parameterTypes, System.Reflection.Module m, bool skipVisibility) { }
        public DynamicMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, System.Type returnType, System.Type[] parameterTypes, System.Type owner, bool skipVisibility) { }
        public DynamicMethod(string name, System.Type returnType, System.Type[] parameterTypes) { }
        public DynamicMethod(string name, System.Type returnType, System.Type[] parameterTypes, bool restrictedSkipVisibility) { }
        public DynamicMethod(string name, System.Type returnType, System.Type[] parameterTypes, System.Reflection.Module m) { }
        public DynamicMethod(string name, System.Type returnType, System.Type[] parameterTypes, System.Reflection.Module m, bool skipVisibility) { }
        public DynamicMethod(string name, System.Type returnType, System.Type[] parameterTypes, System.Type owner) { }
        public DynamicMethod(string name, System.Type returnType, System.Type[] parameterTypes, System.Type owner, bool skipVisibility) { }
        public override System.Reflection.MethodAttributes Attributes { get { throw null; } }
        public override System.Reflection.CallingConventions CallingConvention { get { throw null; } }
        public override System.Type DeclaringType { get { throw null; } }
        public bool InitLocals { get { throw null; } set { } }
        public override System.Reflection.MethodImplAttributes MethodImplementationFlags { get { throw null; } }
        public override string Name { get { throw null; } }
        public override System.Reflection.ParameterInfo ReturnParameter { get { throw null; } }
        public override System.Type ReturnType { get { throw null; } }
        public sealed override System.Delegate CreateDelegate(System.Type delegateType) { throw null; }
        public sealed override System.Delegate CreateDelegate(System.Type delegateType, object target) { throw null; }
        public System.Reflection.Emit.ILGenerator GetILGenerator() { throw null; }
        public System.Reflection.Emit.ILGenerator GetILGenerator(int streamSize) { throw null; }
        public override System.Reflection.ParameterInfo[] GetParameters() { throw null; }
        public override string ToString() { throw null; }
    }
}
