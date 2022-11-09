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
[assembly: AssemblyTitle("Microsoft.VisualBasic")]
[assembly: AssemblyDescription("Microsoft.VisualBasic")]
[assembly: AssemblyDefaultAlias("Microsoft.VisualBasic")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("10.0.2.0")]




namespace Microsoft.VisualBasic
{
    public enum CallType
    {
        Method = 1,
        Get = 2,
        Let = 4,
        Set = 8,
    }
    [Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute]
    public sealed partial class Constants
    {
        internal Constants() { }
        public const string vbBack = "\b";
        public const string vbCr = "\r";
        public const string vbCrLf = "\r\n";
        public const string vbFormFeed = "\f";
        public const string vbLf = "\n";
        [System.ObsoleteAttribute("For a carriage return and line feed, use vbCrLf.  For the current platform's newline, use System.Environment.NewLine.")]
        public const string vbNewLine = "\r\n";
        public const string vbNullChar = "\0";
        public const string vbNullString = null;
        public const string vbTab = "\t";
        public const string vbVerticalTab = "\v";
    }
    public sealed partial class ControlChars
    {
        public const char Back = '\b';
        public const char Cr = '\r';
        public const string CrLf = "\r\n";
        public const char FormFeed = '\f';
        public const char Lf = '\n';
        public const string NewLine = "\r\n";
        public const char NullChar = '\0';
        public const char Quote = '"';
        public const char Tab = '\t';
        public const char VerticalTab = '\v';
        public ControlChars() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class, Inherited=false)]
    public partial class HideModuleNameAttribute : System.Attribute
    {
        public HideModuleNameAttribute() { }
    }
    [Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute]
    public sealed partial class Interaction
    {
        internal Interaction() { }
    }
    [Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute]
    public sealed partial class Strings
    {
        internal Strings() { }
        public static int AscW(char @string) { throw null; }
        public static int AscW(string @string) { throw null; }
        public static char ChrW(int charCode) { throw null; }
        public static string Left(string str, int length) { throw null; }
    }
}
namespace Microsoft.VisualBasic.CompilerServices
{
    public partial class Conversions
    {
        internal Conversions() { }
        public static object ChangeType(object expression, System.Type targetType) { throw null; }
        [System.ObsoleteAttribute("do not use this method", true)]
        public static object FallbackUserDefinedConversion(object expression, System.Type targetType) { throw null; }
        public static System.Globalization.CultureInfo GetCultureInfo() { throw null; }
        public static bool IsHexOrOctValue(string value, ref long i64Value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static bool IsHexOrOctValue(string value, ref ulong ui64Value) { throw null; }
        public static bool ToBoolean(object value) { throw null; }
        public static bool ToBoolean(string value) { throw null; }
        public static byte ToByte(object value) { throw null; }
        public static byte ToByte(string value) { throw null; }
        public static char ToChar(object value) { throw null; }
        public static char ToChar(string value) { throw null; }
        public static char[] ToCharArrayRankOne(object value) { throw null; }
        public static char[] ToCharArrayRankOne(string value) { throw null; }
        public static System.DateTime ToDate(object value) { throw null; }
        public static System.DateTime ToDate(string value) { throw null; }
        public static decimal ToDecimal(bool value) { throw null; }
        public static decimal ToDecimal(object value) { throw null; }
        public static decimal ToDecimal(string value) { throw null; }
        public static double ToDouble(object value) { throw null; }
        public static double ToDouble(string value) { throw null; }
        public static T ToGenericParameter<T>(object value) { throw null; }
        public static string ToHalfwidthNumbers(string s, System.Globalization.CultureInfo culture) { throw null; }
        public static int ToInteger(object value) { throw null; }
        public static int ToInteger(string value) { throw null; }
        public static long ToLong(object value) { throw null; }
        public static long ToLong(string value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static sbyte ToSByte(object value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static sbyte ToSByte(string value) { throw null; }
        public static short ToShort(object value) { throw null; }
        public static short ToShort(string value) { throw null; }
        public static float ToSingle(object value) { throw null; }
        public static float ToSingle(string value) { throw null; }
        public static string ToString(bool value) { throw null; }
        public static string ToString(byte value) { throw null; }
        public static string ToString(char value) { throw null; }
        public static string ToString(System.DateTime value) { throw null; }
        public static string ToString(decimal value) { throw null; }
        public static string ToString(double value) { throw null; }
        public static string ToString(short value) { throw null; }
        public static string ToString(int value) { throw null; }
        public static string ToString(long value) { throw null; }
        public static string ToString(object value) { throw null; }
        public static string ToString(float value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static string ToString(uint value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static string ToString(ulong value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static uint ToUInteger(object value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static uint ToUInteger(string value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ulong ToULong(object value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ulong ToULong(string value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ushort ToUShort(object value) { throw null; }
        [System.CLSCompliantAttribute(false)]
        public static ushort ToUShort(string value) { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class, Inherited=false)]
    public partial class DesignerGeneratedAttribute : System.Attribute
    {
        public DesignerGeneratedAttribute() { }
    }
    public partial class IncompleteInitialization : System.Exception
    {
        public IncompleteInitialization() { }
    }
    public sealed partial class InternalErrorException : System.Exception
    {
        public InternalErrorException() { }
        public InternalErrorException(string message) { }
        public InternalErrorException(string message, System.Exception innerException) { }
    }
    public sealed partial class NewLateBinding
    {
        internal NewLateBinding() { }
        [System.ObsoleteAttribute("do not use this method", true)]
        public static object FallbackCall(object instance, string memberName, object[] arguments, string[] argumentNames, bool ignoreReturn) { throw null; }
        [System.ObsoleteAttribute("do not use this method", true)]
        public static object FallbackGet(object instance, string memberName, object[] arguments, string[] argumentNames) { throw null; }
        [System.ObsoleteAttribute("do not use this method", true)]
        public static void FallbackIndexSet(object instance, object[] arguments, string[] argumentNames) { }
        [System.ObsoleteAttribute("do not use this method", true)]
        public static void FallbackIndexSetComplex(object instance, object[] arguments, string[] argumentNames, bool optimisticSet, bool rValueBase) { }
        [System.ObsoleteAttribute("do not use this method", true)]
        public static object FallbackInvokeDefault1(object instance, object[] arguments, string[] argumentNames, bool reportErrors) { throw null; }
        [System.ObsoleteAttribute("do not use this method", true)]
        public static object FallbackInvokeDefault2(object instance, object[] arguments, string[] argumentNames, bool reportErrors) { throw null; }
        [System.ObsoleteAttribute("do not use this method", true)]
        public static void FallbackSet(object instance, string memberName, object[] arguments) { }
        [System.ObsoleteAttribute("do not use this method", true)]
        public static void FallbackSetComplex(object instance, string memberName, object[] arguments, bool optimisticSet, bool rValueBase) { }
        public static object LateCall(object instance, System.Type type, string memberName, object[] arguments, string[] argumentNames, System.Type[] typeArguments, bool[] copyBack, bool ignoreReturn) { throw null; }
        public static object LateCallInvokeDefault(object instance, object[] arguments, string[] argumentNames, bool reportErrors) { throw null; }
        public static bool LateCanEvaluate(object instance, System.Type type, string memberName, object[] arguments, bool allowFunctionEvaluation, bool allowPropertyEvaluation) { throw null; }
        public static object LateGet(object instance, System.Type type, string memberName, object[] arguments, string[] argumentNames, System.Type[] typeArguments, bool[] copyBack) { throw null; }
        public static object LateGetInvokeDefault(object instance, object[] arguments, string[] argumentNames, bool reportErrors) { throw null; }
        public static object LateIndexGet(object instance, object[] arguments, string[] argumentNames) { throw null; }
        public static void LateIndexSet(object instance, object[] arguments, string[] argumentNames) { }
        public static void LateIndexSetComplex(object instance, object[] arguments, string[] argumentNames, bool optimisticSet, bool rValueBase) { }
        public static void LateSet(object instance, System.Type type, string memberName, object[] arguments, string[] argumentNames, System.Type[] typeArguments) { }
        public static void LateSet(object instance, System.Type type, string memberName, object[] arguments, string[] argumentNames, System.Type[] typeArguments, bool optimisticSet, bool rValueBase, Microsoft.VisualBasic.CallType callType) { }
        public static void LateSetComplex(object instance, System.Type type, string memberName, object[] arguments, string[] argumentNames, System.Type[] typeArguments, bool optimisticSet, bool rValueBase) { }
    }
    public sealed partial class ObjectFlowControl
    {
        internal ObjectFlowControl() { }
        public static void CheckForSyncLockOnValueType(object expression) { }
        public sealed partial class ForLoopControl
        {
            internal ForLoopControl() { }
            public static bool ForLoopInitObj(object counter, object start, object limit, object stepValue, ref object loopForResult, ref object counterResult) { throw null; }
            public static bool ForNextCheckDec(decimal count, decimal limit, decimal stepValue) { throw null; }
            public static bool ForNextCheckObj(object counter, object loopObj, ref object counterResult) { throw null; }
            public static bool ForNextCheckR4(float count, float limit, float stepValue) { throw null; }
            public static bool ForNextCheckR8(double count, double limit, double stepValue) { throw null; }
        }
    }
    public sealed partial class Operators
    {
        internal Operators() { }
        public static object AddObject(object left, object right) { throw null; }
        public static object AndObject(object left, object right) { throw null; }
        public static int CompareObject(object left, object right, bool textCompare) { throw null; }
        public static object CompareObjectEqual(object left, object right, bool textCompare) { throw null; }
        public static object CompareObjectGreater(object left, object right, bool textCompare) { throw null; }
        public static object CompareObjectGreaterEqual(object left, object right, bool textCompare) { throw null; }
        public static object CompareObjectLess(object left, object right, bool textCompare) { throw null; }
        public static object CompareObjectLessEqual(object left, object right, bool textCompare) { throw null; }
        public static object CompareObjectNotEqual(object left, object right, bool textCompare) { throw null; }
        public static int CompareString(string left, string right, bool textCompare) { throw null; }
        public static object ConcatenateObject(object left, object right) { throw null; }
        public static bool ConditionalCompareObjectEqual(object left, object right, bool textCompare) { throw null; }
        public static bool ConditionalCompareObjectGreater(object left, object right, bool textCompare) { throw null; }
        public static bool ConditionalCompareObjectGreaterEqual(object left, object right, bool textCompare) { throw null; }
        public static bool ConditionalCompareObjectLess(object left, object right, bool textCompare) { throw null; }
        public static bool ConditionalCompareObjectLessEqual(object left, object right, bool textCompare) { throw null; }
        public static bool ConditionalCompareObjectNotEqual(object left, object right, bool textCompare) { throw null; }
        public static object DivideObject(object left, object right) { throw null; }
        public static object ExponentObject(object left, object right) { throw null; }
        [System.ObsoleteAttribute("do not use this method", true)]
        public static object FallbackInvokeUserDefinedOperator(object vbOp, object[] arguments) { throw null; }
        public static object IntDivideObject(object left, object right) { throw null; }
        public static object LeftShiftObject(object operand, object amount) { throw null; }
        public static object ModObject(object left, object right) { throw null; }
        public static object MultiplyObject(object left, object right) { throw null; }
        public static object NegateObject(object operand) { throw null; }
        public static object NotObject(object operand) { throw null; }
        public static object OrObject(object left, object right) { throw null; }
        public static object PlusObject(object operand) { throw null; }
        public static object RightShiftObject(object operand, object amount) { throw null; }
        public static object SubtractObject(object left, object right) { throw null; }
        public static object XorObject(object left, object right) { throw null; }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Parameter, Inherited=false)]
    public partial class OptionCompareAttribute : System.Attribute
    {
        public OptionCompareAttribute() { }
    }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class, Inherited=false)]
    public partial class OptionTextAttribute : System.Attribute
    {
        public OptionTextAttribute() { }
    }
    public partial class ProjectData
    {
        internal ProjectData() { }
        public static void ClearProjectError() { }
        public static void SetProjectError(System.Exception ex) { }
        public static void SetProjectError(System.Exception ex, int lErl) { }
    }
    public delegate object SiteDelegate0(System.Runtime.CompilerServices.CallSite site, object instance);
    public delegate object SiteDelegate1(System.Runtime.CompilerServices.CallSite site, object instance, ref object arg0);
    public delegate object SiteDelegate2(System.Runtime.CompilerServices.CallSite site, object instance, ref object arg0, ref object arg1);
    public delegate object SiteDelegate3(System.Runtime.CompilerServices.CallSite site, object instance, ref object arg0, ref object arg1, ref object arg2);
    public delegate object SiteDelegate4(System.Runtime.CompilerServices.CallSite site, object instance, ref object arg0, ref object arg1, ref object arg2, ref object arg3);
    public delegate object SiteDelegate5(System.Runtime.CompilerServices.CallSite site, object instance, ref object arg0, ref object arg1, ref object arg2, ref object arg3, ref object arg4);
    public delegate object SiteDelegate6(System.Runtime.CompilerServices.CallSite site, object instance, ref object arg0, ref object arg1, ref object arg2, ref object arg3, ref object arg4, ref object arg5);
    public delegate object SiteDelegate7(System.Runtime.CompilerServices.CallSite site, object instance, ref object arg0, ref object arg1, ref object arg2, ref object arg3, ref object arg4, ref object arg5, ref object arg6);
    [System.AttributeUsageAttribute(System.AttributeTargets.Class, Inherited=false)]
    public partial class StandardModuleAttribute : System.Attribute
    {
        public StandardModuleAttribute() { }
    }
    public partial class StaticLocalInitFlag
    {
        public short State;
        public StaticLocalInitFlag() { }
    }
    public sealed partial class Utils
    {
        internal Utils() { }
        public static System.Array CopyArray(System.Array arySrc, System.Array aryDest) { throw null; }
        public static string GetResourceString(string resourceKey, params string[] args) { throw null; }
        public static string MethodToString(System.Reflection.MethodBase method) { throw null; }
    }
}
