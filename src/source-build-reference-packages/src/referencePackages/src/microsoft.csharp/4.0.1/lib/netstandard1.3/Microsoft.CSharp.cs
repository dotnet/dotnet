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
[assembly: AssemblyVersion("4.0.1.0")]




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
        Constant = 2,
        IsOut = 16,
        IsRef = 8,
        IsStaticType = 32,
        NamedArgument = 4,
        None = 0,
        UseCompileTimeType = 1,
    }
    [System.FlagsAttribute]
    public enum CSharpBinderFlags
    {
        BinaryOperationLogical = 8,
        CheckedContext = 1,
        ConvertArrayIndex = 32,
        ConvertExplicit = 16,
        InvokeSimpleName = 2,
        InvokeSpecialName = 4,
        None = 0,
        ResultDiscarded = 256,
        ResultIndexed = 64,
        ValueFromCompoundAssignment = 128,
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
namespace Microsoft.CSharp.RuntimeBinder.Errors
{
    public enum ErrorCode
    {
        ERR_AbstractBaseCall = 205,
        ERR_AmbigBinaryOps = 34,
        ERR_AmbigCall = 121,
        ERR_AmbigMember = 229,
        ERR_AmbigUDConv = 457,
        ERR_AmbigUnaryOp = 35,
        ERR_AssgLvalueExpected = 131,
        ERR_AssgReadonly = 191,
        ERR_AssgReadonly2 = 1648,
        ERR_AssgReadonlyLocal = 1604,
        ERR_AssgReadonlyLocalCause = 1656,
        ERR_AssgReadonlyProp = 200,
        ERR_AssgReadonlyStatic = 198,
        ERR_AssgReadonlyStatic2 = 1650,
        ERR_BadAccess = 122,
        ERR_BadArgCount = 1501,
        ERR_BadArgExtraRef = 1615,
        ERR_BadArgRef = 1620,
        ERR_BadArgType = 1503,
        ERR_BadArgTypes = 1502,
        ERR_BadArgTypesForCollectionAdd = 1950,
        ERR_BadArity = 305,
        ERR_BadBinaryOps = 19,
        ERR_BadBoolOp = 217,
        ERR_BadCastInFixed = 254,
        ERR_BadCtorArgCount = 1729,
        ERR_BadDelArgCount = 1593,
        ERR_BadDelArgTypes = 1594,
        ERR_BadDelegateConstructor = 148,
        ERR_BadExtensionArgTypes = 1928,
        ERR_BadIndexCount = 22,
        ERR_BadIndexLHS = 21,
        ERR_BadInstanceArgType = 1929,
        ERR_BadNamedArgument = 5003,
        ERR_BadNamedArgumentForDelegateInvoke = 5004,
        ERR_BadProtectedAccess = 1540,
        ERR_BadRetType = 407,
        ERR_BadTypeArgument = 306,
        ERR_BadUnaryOp = 23,
        ERR_BaseConstraintConflict = 455,
        ERR_BindToBogus = 570,
        ERR_BindToBogusProp1 = 1546,
        ERR_BindToBogusProp2 = 1545,
        ERR_BogusType = 648,
        ERR_CallingBaseFinalizeDeprecated = 250,
        ERR_CallingFinalizeDepracated = 245,
        ERR_CantCallSpecialMethod = 571,
        ERR_CantInferMethTypeArgs = 411,
        ERR_CheckedOverflow = 220,
        ERR_CircularConstraint = 454,
        ERR_ConstOutOfRange = 31,
        ERR_ConstOutOfRangeChecked = 221,
        ERR_ConvertToStaticClass = 716,
        ERR_ConWithValCon = 456,
        ERR_DelegateOnNullable = 1728,
        ERR_DuplicateNamedArgument = 5005,
        ERR_FieldInitRefNonstatic = 236,
        ERR_FixedNotNeeded = 213,
        ERR_GenericArgIsStaticClass = 718,
        ERR_GenericConstraintNotSatisfiedNullableEnum = 312,
        ERR_GenericConstraintNotSatisfiedNullableInterface = 313,
        ERR_GenericConstraintNotSatisfiedRefType = 311,
        ERR_GenericConstraintNotSatisfiedTyVar = 314,
        ERR_GenericConstraintNotSatisfiedValType = 315,
        ERR_HasNoTypeVars = 308,
        ERR_InaccessibleGetter = 271,
        ERR_InaccessibleSetter = 272,
        ERR_IncrementLvalueExpected = 1059,
        ERR_InitializerAddHasParamModifiers = 1954,
        ERR_IntDivByZero = 20,
        ERR_LiteralDoubleCast = 664,
        ERR_ManagedAddr = 208,
        ERR_MethDelegateMismatch = 123,
        ERR_MethGrpToNonDel = 428,
        ERR_MissingPredefinedMember = 656,
        ERR_MustHaveOpTF = 218,
        ERR_NamedArgumentSpecificationBeforeFixedArgument = 5002,
        ERR_NamedArgumentUsedInPositional = 5006,
        ERR_NewConstraintNotSatisfied = 310,
        ERR_NoConstructors = 143,
        ERR_NoExplicitConv = 30,
        ERR_NoImplicitConv = 29,
        ERR_NoImplicitConvCast = 266,
        ERR_NonInvocableMemberCalled = 1955,
        ERR_NoSuchMember = 117,
        ERR_NoSuchMemberOrExtension = 1061,
        ERR_ObjectProhibited = 176,
        ERR_ObjectRequired = 120,
        ERR_PartialMethodToDelegate = 762,
        ERR_PredefinedTypeBadType = 520,
        ERR_PredefinedTypeNotFound = 518,
        ERR_PropertyLacksGet = 154,
        ERR_RefConstraintNotSatisfied = 452,
        ERR_RefLvalueExpected = 1510,
        ERR_RefProperty = 206,
        ERR_RefReadonly = 192,
        ERR_RefReadonly2 = 1649,
        ERR_RefReadonlyLocal = 1605,
        ERR_RefReadonlyLocalCause = 1657,
        ERR_RefReadonlyStatic = 199,
        ERR_RefReadonlyStatic2 = 1651,
        ERR_ReturnNotLValue = 1612,
        ERR_SizeofUnsafe = 233,
        ERR_ThisStructNotInAnonMeth = 1673,
        ERR_TypeArgsNotAllowed = 307,
        ERR_TypeVarCantBeNull = 403,
        ERR_UnifyingInterfaceInstantiations = 695,
        ERR_UnsafeNeeded = 214,
        ERR_ValConstraintNotSatisfied = 453,
        ERR_ValueCantBeNull = 37,
        ERR_ValueTypeExtDelegate = 1113,
        ERR_WrongNestedThis = 38,
    }
}
