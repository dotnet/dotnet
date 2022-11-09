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
[assembly: AssemblyTitle("System.Globalization")]
[assembly: AssemblyDescription("System.Globalization")]
[assembly: AssemblyDefaultAlias("System.Globalization")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("1.0.24212.01")]
[assembly: AssemblyInformationalVersion("1.0.24212.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.0.10.0")]




namespace System.Globalization
{
    public abstract partial class Calendar
    {
        public const int CurrentEra = 0;
        protected Calendar() { }
        public abstract int[] Eras { get; }
        public bool IsReadOnly { get { throw null; } }
        public virtual System.DateTime MaxSupportedDateTime { get { throw null; } }
        public virtual System.DateTime MinSupportedDateTime { get { throw null; } }
        public virtual int TwoDigitYearMax { get { throw null; } set { } }
        public virtual System.DateTime AddDays(System.DateTime time, int days) { throw null; }
        public virtual System.DateTime AddHours(System.DateTime time, int hours) { throw null; }
        public virtual System.DateTime AddMilliseconds(System.DateTime time, double milliseconds) { throw null; }
        public virtual System.DateTime AddMinutes(System.DateTime time, int minutes) { throw null; }
        public abstract System.DateTime AddMonths(System.DateTime time, int months);
        public virtual System.DateTime AddSeconds(System.DateTime time, int seconds) { throw null; }
        public virtual System.DateTime AddWeeks(System.DateTime time, int weeks) { throw null; }
        public abstract System.DateTime AddYears(System.DateTime time, int years);
        public abstract int GetDayOfMonth(System.DateTime time);
        public abstract System.DayOfWeek GetDayOfWeek(System.DateTime time);
        public abstract int GetDayOfYear(System.DateTime time);
        public virtual int GetDaysInMonth(int year, int month) { throw null; }
        public abstract int GetDaysInMonth(int year, int month, int era);
        public virtual int GetDaysInYear(int year) { throw null; }
        public abstract int GetDaysInYear(int year, int era);
        public abstract int GetEra(System.DateTime time);
        public virtual int GetHour(System.DateTime time) { throw null; }
        public virtual int GetLeapMonth(int year, int era) { throw null; }
        public virtual double GetMilliseconds(System.DateTime time) { throw null; }
        public virtual int GetMinute(System.DateTime time) { throw null; }
        public abstract int GetMonth(System.DateTime time);
        public virtual int GetMonthsInYear(int year) { throw null; }
        public abstract int GetMonthsInYear(int year, int era);
        public virtual int GetSecond(System.DateTime time) { throw null; }
        public virtual int GetWeekOfYear(System.DateTime time, System.Globalization.CalendarWeekRule rule, System.DayOfWeek firstDayOfWeek) { throw null; }
        public abstract int GetYear(System.DateTime time);
        public virtual bool IsLeapDay(int year, int month, int day) { throw null; }
        public abstract bool IsLeapDay(int year, int month, int day, int era);
        public virtual bool IsLeapMonth(int year, int month) { throw null; }
        public abstract bool IsLeapMonth(int year, int month, int era);
        public virtual bool IsLeapYear(int year) { throw null; }
        public abstract bool IsLeapYear(int year, int era);
        public virtual System.DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond) { throw null; }
        public abstract System.DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era);
        public virtual int ToFourDigitYear(int year) { throw null; }
    }
    public enum CalendarWeekRule
    {
        FirstDay = 0,
        FirstFourDayWeek = 2,
        FirstFullWeek = 1,
    }
    public static partial class CharUnicodeInfo
    {
        public static double GetNumericValue(char ch) { throw null; }
        public static double GetNumericValue(string s, int index) { throw null; }
        public static System.Globalization.UnicodeCategory GetUnicodeCategory(char ch) { throw null; }
        public static System.Globalization.UnicodeCategory GetUnicodeCategory(string s, int index) { throw null; }
    }
    public partial class CompareInfo
    {
        internal CompareInfo() { }
        public virtual string Name { get { throw null; } }
        public virtual int Compare(string string1, int offset1, int length1, string string2, int offset2, int length2) { throw null; }
        public virtual int Compare(string string1, int offset1, int length1, string string2, int offset2, int length2, System.Globalization.CompareOptions options) { throw null; }
        public virtual int Compare(string string1, int offset1, string string2, int offset2) { throw null; }
        public virtual int Compare(string string1, int offset1, string string2, int offset2, System.Globalization.CompareOptions options) { throw null; }
        public virtual int Compare(string string1, string string2) { throw null; }
        public virtual int Compare(string string1, string string2, System.Globalization.CompareOptions options) { throw null; }
        public override bool Equals(object value) { throw null; }
        public static System.Globalization.CompareInfo GetCompareInfo(string name) { throw null; }
        public override int GetHashCode() { throw null; }
        public virtual int GetHashCode(string source, System.Globalization.CompareOptions options) { throw null; }
        public virtual int IndexOf(string source, char value) { throw null; }
        public virtual int IndexOf(string source, char value, System.Globalization.CompareOptions options) { throw null; }
        public virtual int IndexOf(string source, char value, int startIndex, System.Globalization.CompareOptions options) { throw null; }
        public virtual int IndexOf(string source, char value, int startIndex, int count) { throw null; }
        public virtual int IndexOf(string source, char value, int startIndex, int count, System.Globalization.CompareOptions options) { throw null; }
        public virtual int IndexOf(string source, string value) { throw null; }
        public virtual int IndexOf(string source, string value, System.Globalization.CompareOptions options) { throw null; }
        public virtual int IndexOf(string source, string value, int startIndex, System.Globalization.CompareOptions options) { throw null; }
        public virtual int IndexOf(string source, string value, int startIndex, int count) { throw null; }
        public virtual int IndexOf(string source, string value, int startIndex, int count, System.Globalization.CompareOptions options) { throw null; }
        public virtual bool IsPrefix(string source, string prefix) { throw null; }
        public virtual bool IsPrefix(string source, string prefix, System.Globalization.CompareOptions options) { throw null; }
        public virtual bool IsSuffix(string source, string suffix) { throw null; }
        public virtual bool IsSuffix(string source, string suffix, System.Globalization.CompareOptions options) { throw null; }
        public virtual int LastIndexOf(string source, char value) { throw null; }
        public virtual int LastIndexOf(string source, char value, System.Globalization.CompareOptions options) { throw null; }
        public virtual int LastIndexOf(string source, char value, int startIndex, System.Globalization.CompareOptions options) { throw null; }
        public virtual int LastIndexOf(string source, char value, int startIndex, int count) { throw null; }
        public virtual int LastIndexOf(string source, char value, int startIndex, int count, System.Globalization.CompareOptions options) { throw null; }
        public virtual int LastIndexOf(string source, string value) { throw null; }
        public virtual int LastIndexOf(string source, string value, System.Globalization.CompareOptions options) { throw null; }
        public virtual int LastIndexOf(string source, string value, int startIndex, System.Globalization.CompareOptions options) { throw null; }
        public virtual int LastIndexOf(string source, string value, int startIndex, int count) { throw null; }
        public virtual int LastIndexOf(string source, string value, int startIndex, int count, System.Globalization.CompareOptions options) { throw null; }
        public override string ToString() { throw null; }
    }
    [System.FlagsAttribute]
    public enum CompareOptions
    {
        IgnoreCase = 1,
        IgnoreKanaType = 8,
        IgnoreNonSpace = 2,
        IgnoreSymbols = 4,
        IgnoreWidth = 16,
        None = 0,
        Ordinal = 1073741824,
        OrdinalIgnoreCase = 268435456,
        StringSort = 536870912,
    }
    public partial class CultureInfo : System.IFormatProvider
    {
        public CultureInfo(string name) { }
        public virtual System.Globalization.Calendar Calendar { get { throw null; } }
        public virtual System.Globalization.CompareInfo CompareInfo { get { throw null; } }
        public static System.Globalization.CultureInfo CurrentCulture { get { throw null; } set { } }
        public static System.Globalization.CultureInfo CurrentUICulture { get { throw null; } set { } }
        public virtual System.Globalization.DateTimeFormatInfo DateTimeFormat { get { throw null; } set { } }
        public static System.Globalization.CultureInfo DefaultThreadCurrentCulture { get { throw null; } set { } }
        public static System.Globalization.CultureInfo DefaultThreadCurrentUICulture { get { throw null; } set { } }
        public virtual string DisplayName { get { throw null; } }
        public virtual string EnglishName { get { throw null; } }
        public static System.Globalization.CultureInfo InvariantCulture { get { throw null; } }
        public virtual bool IsNeutralCulture { get { throw null; } }
        public bool IsReadOnly { get { throw null; } }
        public virtual string Name { get { throw null; } }
        public virtual string NativeName { get { throw null; } }
        public virtual System.Globalization.NumberFormatInfo NumberFormat { get { throw null; } set { } }
        public virtual System.Globalization.Calendar[] OptionalCalendars { get { throw null; } }
        public virtual System.Globalization.CultureInfo Parent { get { throw null; } }
        public virtual System.Globalization.TextInfo TextInfo { get { throw null; } }
        public virtual string TwoLetterISOLanguageName { get { throw null; } }
        public virtual object Clone() { throw null; }
        public override bool Equals(object value) { throw null; }
        public virtual object GetFormat(System.Type formatType) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Globalization.CultureInfo ReadOnly(System.Globalization.CultureInfo ci) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class CultureNotFoundException : System.ArgumentException
    {
        public CultureNotFoundException() { }
        public CultureNotFoundException(string message) { }
        public CultureNotFoundException(string message, System.Exception innerException) { }
        public CultureNotFoundException(string paramName, string message) { }
        public CultureNotFoundException(string message, string invalidCultureName, System.Exception innerException) { }
        public CultureNotFoundException(string paramName, string invalidCultureName, string message) { }
        public virtual string InvalidCultureName { get { throw null; } }
        public override string Message { get { throw null; } }
    }
    public sealed partial class DateTimeFormatInfo : System.IFormatProvider
    {
        public DateTimeFormatInfo() { }
        public string[] AbbreviatedDayNames { get { throw null; } set { } }
        public string[] AbbreviatedMonthGenitiveNames { get { throw null; } set { } }
        public string[] AbbreviatedMonthNames { get { throw null; } set { } }
        public string AMDesignator { get { throw null; } set { } }
        public System.Globalization.Calendar Calendar { get { throw null; } set { } }
        public System.Globalization.CalendarWeekRule CalendarWeekRule { get { throw null; } set { } }
        public static System.Globalization.DateTimeFormatInfo CurrentInfo { get { throw null; } }
        public string[] DayNames { get { throw null; } set { } }
        public System.DayOfWeek FirstDayOfWeek { get { throw null; } set { } }
        public string FullDateTimePattern { get { throw null; } set { } }
        public static System.Globalization.DateTimeFormatInfo InvariantInfo { get { throw null; } }
        public bool IsReadOnly { get { throw null; } }
        public string LongDatePattern { get { throw null; } set { } }
        public string LongTimePattern { get { throw null; } set { } }
        public string MonthDayPattern { get { throw null; } set { } }
        public string[] MonthGenitiveNames { get { throw null; } set { } }
        public string[] MonthNames { get { throw null; } set { } }
        public string PMDesignator { get { throw null; } set { } }
        public string RFC1123Pattern { get { throw null; } }
        public string ShortDatePattern { get { throw null; } set { } }
        public string[] ShortestDayNames { get { throw null; } set { } }
        public string ShortTimePattern { get { throw null; } set { } }
        public string SortableDateTimePattern { get { throw null; } }
        public string UniversalSortableDateTimePattern { get { throw null; } }
        public string YearMonthPattern { get { throw null; } set { } }
        public object Clone() { throw null; }
        public string GetAbbreviatedDayName(System.DayOfWeek dayofweek) { throw null; }
        public string GetAbbreviatedEraName(int era) { throw null; }
        public string GetAbbreviatedMonthName(int month) { throw null; }
        public string GetDayName(System.DayOfWeek dayofweek) { throw null; }
        public int GetEra(string eraName) { throw null; }
        public string GetEraName(int era) { throw null; }
        public object GetFormat(System.Type formatType) { throw null; }
        public static System.Globalization.DateTimeFormatInfo GetInstance(System.IFormatProvider provider) { throw null; }
        public string GetMonthName(int month) { throw null; }
        public static System.Globalization.DateTimeFormatInfo ReadOnly(System.Globalization.DateTimeFormatInfo dtfi) { throw null; }
    }
    public sealed partial class NumberFormatInfo : System.IFormatProvider
    {
        public NumberFormatInfo() { }
        public int CurrencyDecimalDigits { get { throw null; } set { } }
        public string CurrencyDecimalSeparator { get { throw null; } set { } }
        public string CurrencyGroupSeparator { get { throw null; } set { } }
        public int[] CurrencyGroupSizes { get { throw null; } set { } }
        public int CurrencyNegativePattern { get { throw null; } set { } }
        public int CurrencyPositivePattern { get { throw null; } set { } }
        public string CurrencySymbol { get { throw null; } set { } }
        public static System.Globalization.NumberFormatInfo CurrentInfo { get { throw null; } }
        public static System.Globalization.NumberFormatInfo InvariantInfo { get { throw null; } }
        public bool IsReadOnly { get { throw null; } }
        public string NaNSymbol { get { throw null; } set { } }
        public string NegativeInfinitySymbol { get { throw null; } set { } }
        public string NegativeSign { get { throw null; } set { } }
        public int NumberDecimalDigits { get { throw null; } set { } }
        public string NumberDecimalSeparator { get { throw null; } set { } }
        public string NumberGroupSeparator { get { throw null; } set { } }
        public int[] NumberGroupSizes { get { throw null; } set { } }
        public int NumberNegativePattern { get { throw null; } set { } }
        public int PercentDecimalDigits { get { throw null; } set { } }
        public string PercentDecimalSeparator { get { throw null; } set { } }
        public string PercentGroupSeparator { get { throw null; } set { } }
        public int[] PercentGroupSizes { get { throw null; } set { } }
        public int PercentNegativePattern { get { throw null; } set { } }
        public int PercentPositivePattern { get { throw null; } set { } }
        public string PercentSymbol { get { throw null; } set { } }
        public string PerMilleSymbol { get { throw null; } set { } }
        public string PositiveInfinitySymbol { get { throw null; } set { } }
        public string PositiveSign { get { throw null; } set { } }
        public object Clone() { throw null; }
        public object GetFormat(System.Type formatType) { throw null; }
        public static System.Globalization.NumberFormatInfo GetInstance(System.IFormatProvider formatProvider) { throw null; }
        public static System.Globalization.NumberFormatInfo ReadOnly(System.Globalization.NumberFormatInfo nfi) { throw null; }
    }
    public partial class RegionInfo
    {
        public RegionInfo(string name) { }
        public virtual string CurrencySymbol { get { throw null; } }
        public static System.Globalization.RegionInfo CurrentRegion { get { throw null; } }
        public virtual string DisplayName { get { throw null; } }
        public virtual string EnglishName { get { throw null; } }
        public virtual bool IsMetric { get { throw null; } }
        public virtual string ISOCurrencySymbol { get { throw null; } }
        public virtual string Name { get { throw null; } }
        public virtual string NativeName { get { throw null; } }
        public virtual string TwoLetterISORegionName { get { throw null; } }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class StringInfo
    {
        public StringInfo() { }
        public StringInfo(string value) { }
        public int LengthInTextElements { get { throw null; } }
        public string String { get { throw null; } set { } }
        public override bool Equals(object value) { throw null; }
        public override int GetHashCode() { throw null; }
        public static string GetNextTextElement(string str) { throw null; }
        public static string GetNextTextElement(string str, int index) { throw null; }
        public static System.Globalization.TextElementEnumerator GetTextElementEnumerator(string str) { throw null; }
        public static System.Globalization.TextElementEnumerator GetTextElementEnumerator(string str, int index) { throw null; }
        public static int[] ParseCombiningCharacters(string str) { throw null; }
    }
    public partial class TextElementEnumerator : System.Collections.IEnumerator
    {
        internal TextElementEnumerator() { }
        public object Current { get { throw null; } }
        public int ElementIndex { get { throw null; } }
        public string GetTextElement() { throw null; }
        public bool MoveNext() { throw null; }
        public void Reset() { }
    }
    public partial class TextInfo
    {
        internal TextInfo() { }
        public string CultureName { get { throw null; } }
        public bool IsReadOnly { get { throw null; } }
        public bool IsRightToLeft { get { throw null; } }
        public virtual string ListSeparator { get { throw null; } set { } }
        public override bool Equals(object obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public virtual char ToLower(char c) { throw null; }
        public virtual string ToLower(string str) { throw null; }
        public override string ToString() { throw null; }
        public virtual char ToUpper(char c) { throw null; }
        public virtual string ToUpper(string str) { throw null; }
    }
    public enum UnicodeCategory
    {
        ClosePunctuation = 21,
        ConnectorPunctuation = 18,
        Control = 14,
        CurrencySymbol = 26,
        DashPunctuation = 19,
        DecimalDigitNumber = 8,
        EnclosingMark = 7,
        FinalQuotePunctuation = 23,
        Format = 15,
        InitialQuotePunctuation = 22,
        LetterNumber = 9,
        LineSeparator = 12,
        LowercaseLetter = 1,
        MathSymbol = 25,
        ModifierLetter = 3,
        ModifierSymbol = 27,
        NonSpacingMark = 5,
        OpenPunctuation = 20,
        OtherLetter = 4,
        OtherNotAssigned = 29,
        OtherNumber = 10,
        OtherPunctuation = 24,
        OtherSymbol = 28,
        ParagraphSeparator = 13,
        PrivateUse = 17,
        SpaceSeparator = 11,
        SpacingCombiningMark = 6,
        Surrogate = 16,
        TitlecaseLetter = 2,
        UppercaseLetter = 0,
    }
}
