// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Reflection.AssemblyDefaultAlias("Microsoft.CSharp")]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyMetadata("PreferInbox", "True")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation. All rights reserved.")]
[assembly: System.Reflection.AssemblyDescription("Microsoft.CSharp")]
[assembly: System.Reflection.AssemblyFileVersion("4.700.19.56404")]
[assembly: System.Reflection.AssemblyInformationalVersion("3.1.0+0f7f38c4fd323b26da10cce95f857f77f0f09b48")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Core")]
[assembly: System.Reflection.AssemblyTitle("Microsoft.CSharp")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Microsoft.CSharp.RuntimeBinder
{
    public static partial class Binder
    {
        public static System.Runtime.CompilerServices.CallSiteBinder BinaryOperation(CSharpBinderFlags flags, System.Linq.Expressions.ExpressionType operation, System.Type context, System.Collections.Generic.IEnumerable<CSharpArgumentInfo> argumentInfo) { throw null; }

        public static System.Runtime.CompilerServices.CallSiteBinder Convert(CSharpBinderFlags flags, System.Type type, System.Type context) { throw null; }

        public static System.Runtime.CompilerServices.CallSiteBinder GetIndex(CSharpBinderFlags flags, System.Type context, System.Collections.Generic.IEnumerable<CSharpArgumentInfo> argumentInfo) { throw null; }

        public static System.Runtime.CompilerServices.CallSiteBinder GetMember(CSharpBinderFlags flags, string name, System.Type context, System.Collections.Generic.IEnumerable<CSharpArgumentInfo> argumentInfo) { throw null; }

        public static System.Runtime.CompilerServices.CallSiteBinder Invoke(CSharpBinderFlags flags, System.Type context, System.Collections.Generic.IEnumerable<CSharpArgumentInfo> argumentInfo) { throw null; }

        public static System.Runtime.CompilerServices.CallSiteBinder InvokeConstructor(CSharpBinderFlags flags, System.Type context, System.Collections.Generic.IEnumerable<CSharpArgumentInfo> argumentInfo) { throw null; }

        public static System.Runtime.CompilerServices.CallSiteBinder InvokeMember(CSharpBinderFlags flags, string name, System.Collections.Generic.IEnumerable<System.Type> typeArguments, System.Type context, System.Collections.Generic.IEnumerable<CSharpArgumentInfo> argumentInfo) { throw null; }

        public static System.Runtime.CompilerServices.CallSiteBinder IsEvent(CSharpBinderFlags flags, string name, System.Type context) { throw null; }

        public static System.Runtime.CompilerServices.CallSiteBinder SetIndex(CSharpBinderFlags flags, System.Type context, System.Collections.Generic.IEnumerable<CSharpArgumentInfo> argumentInfo) { throw null; }

        public static System.Runtime.CompilerServices.CallSiteBinder SetMember(CSharpBinderFlags flags, string name, System.Type context, System.Collections.Generic.IEnumerable<CSharpArgumentInfo> argumentInfo) { throw null; }

        public static System.Runtime.CompilerServices.CallSiteBinder UnaryOperation(CSharpBinderFlags flags, System.Linq.Expressions.ExpressionType operation, System.Type context, System.Collections.Generic.IEnumerable<CSharpArgumentInfo> argumentInfo) { throw null; }
    }

    public sealed partial class CSharpArgumentInfo
    {
        internal CSharpArgumentInfo() { }

        public static CSharpArgumentInfo Create(CSharpArgumentInfoFlags flags, string name) { throw null; }
    }

    [System.Flags]
    public enum CSharpArgumentInfoFlags
    {
        None = 0,
        UseCompileTimeType = 1,
        Constant = 2,
        NamedArgument = 4,
        IsRef = 8,
        IsOut = 16,
        IsStaticType = 32
    }

    [System.Flags]
    public enum CSharpBinderFlags
    {
        None = 0,
        CheckedContext = 1,
        InvokeSimpleName = 2,
        InvokeSpecialName = 4,
        BinaryOperationLogical = 8,
        ConvertExplicit = 16,
        ConvertArrayIndex = 32,
        ResultIndexed = 64,
        ValueFromCompoundAssignment = 128,
        ResultDiscarded = 256
    }

    public partial class RuntimeBinderException : System.Exception
    {
        public RuntimeBinderException() { }

        protected RuntimeBinderException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }

        public RuntimeBinderException(string message, System.Exception innerException) { }

        public RuntimeBinderException(string message) { }
    }

    public partial class RuntimeBinderInternalCompilerException : System.Exception
    {
        public RuntimeBinderInternalCompilerException() { }

        protected RuntimeBinderInternalCompilerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }

        public RuntimeBinderInternalCompilerException(string message, System.Exception innerException) { }

        public RuntimeBinderInternalCompilerException(string message) { }
    }
}