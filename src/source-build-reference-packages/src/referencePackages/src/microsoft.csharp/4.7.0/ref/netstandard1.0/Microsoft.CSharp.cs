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
[assembly: AssemblyTitle("Microsoft.CSharp")]
[assembly: AssemblyDescription("Microsoft.CSharp")]
[assembly: AssemblyDefaultAlias("Microsoft.CSharp")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.0.0")]




namespace Microsoft.CSharp.RuntimeBinder
{
    public static partial class Binder
    {
        public static System.Runtime.CompilerServices.CallSiteBinder BinaryOperation(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, System.Linq.Expressions.ExpressionType operation, System.Type context, System.Collections.Generic.IEnumerable<Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo> argumentInfo) { throw null; }
        public static System.Runtime.CompilerServices.CallSiteBinder Convert(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, System.Type type, System.Type context) { throw null; }
        public static System.Runtime.CompilerServices.CallSiteBinder GetIndex(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, System.Type context, System.Collections.Generic.IEnumerable<Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo> argumentInfo) { throw null; }
        public static System.Runtime.CompilerServices.CallSiteBinder GetMember(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, string name, System.Type context, System.Collections.Generic.IEnumerable<Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo> argumentInfo) { throw null; }
        public static System.Runtime.CompilerServices.CallSiteBinder Invoke(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, System.Type context, System.Collections.Generic.IEnumerable<Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo> argumentInfo) { throw null; }
        public static System.Runtime.CompilerServices.CallSiteBinder InvokeConstructor(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, System.Type context, System.Collections.Generic.IEnumerable<Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo> argumentInfo) { throw null; }
        public static System.Runtime.CompilerServices.CallSiteBinder InvokeMember(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, string name, System.Collections.Generic.IEnumerable<System.Type> typeArguments, System.Type context, System.Collections.Generic.IEnumerable<Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo> argumentInfo) { throw null; }
        public static System.Runtime.CompilerServices.CallSiteBinder IsEvent(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, string name, System.Type context) { throw null; }
        public static System.Runtime.CompilerServices.CallSiteBinder SetIndex(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, System.Type context, System.Collections.Generic.IEnumerable<Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo> argumentInfo) { throw null; }
        public static System.Runtime.CompilerServices.CallSiteBinder SetMember(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, string name, System.Type context, System.Collections.Generic.IEnumerable<Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo> argumentInfo) { throw null; }
        public static System.Runtime.CompilerServices.CallSiteBinder UnaryOperation(Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags flags, System.Linq.Expressions.ExpressionType operation, System.Type context, System.Collections.Generic.IEnumerable<Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo> argumentInfo) { throw null; }
    }
    public sealed partial class CSharpArgumentInfo
    {
        internal CSharpArgumentInfo() { }
        public static Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo Create(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags flags, string name) { throw null; }
    }
    [System.FlagsAttribute]
    public enum CSharpArgumentInfoFlags
    {
        None = 0,
        UseCompileTimeType = 1,
        Constant = 2,
        NamedArgument = 4,
        IsRef = 8,
        IsOut = 16,
        IsStaticType = 32,
    }
    [System.FlagsAttribute]
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
        ResultDiscarded = 256,
    }
    public partial class RuntimeBinderException : System.Exception
    {
        public RuntimeBinderException() { }
        public RuntimeBinderException(string message) { }
        public RuntimeBinderException(string message, System.Exception innerException) { }
    }
    public partial class RuntimeBinderInternalCompilerException : System.Exception
    {
        public RuntimeBinderInternalCompilerException() { }
        public RuntimeBinderInternalCompilerException(string message) { }
        public RuntimeBinderInternalCompilerException(string message, System.Exception innerException) { }
    }
}
