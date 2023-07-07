// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Globalization.dll")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Reflection.AssemblyTitle("System.Globalization.dll")]
[assembly: System.Reflection.AssemblyDescription("System.Globalization.dll")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyFileVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyInformationalVersion("4.0.30319.17929")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Globalization
{
    public abstract partial class Calendar
    {
        public const int CurrentEra = 0;
        public abstract int[] Eras { get; }

        public bool IsReadOnly { get { throw null; } }

        public virtual DateTime MaxSupportedDateTime { get { throw null; } }

        public virtual DateTime MinSupportedDateTime { get { throw null; } }

        public virtual int TwoDigitYearMax { get { throw null; } set { } }

        public virtual DateTime AddDays(DateTime time, int days) { throw null; }

        public virtual DateTime AddHours(DateTime time, int hours) { throw null; }

        public virtual DateTime AddMilliseconds(DateTime time, double milliseconds) { throw null; }

        public virtual DateTime AddMinutes(DateTime time, int minutes) { throw null; }

        public abstract DateTime AddMonths(DateTime time, int months);
        public virtual DateTime AddSeconds(DateTime time, int seconds) { throw null; }

        public virtual DateTime AddWeeks(DateTime time, int weeks) { throw null; }

        public abstract DateTime AddYears(DateTime time, int years);
        public abstract int GetDayOfMonth(DateTime time);
        public abstract DayOfWeek GetDayOfWeek(DateTime time);
        public abstract int GetDayOfYear(DateTime time);
        public abstract int GetDaysInMonth(int year, int month, int era);
        public virtual int GetDaysInMonth(int year, int month) { throw null; }

        public abstract int GetDaysInYear(int year, int era);
        public virtual int GetDaysInYear(int year) { throw null; }

        public abstract int GetEra(DateTime time);
        public virtual int GetHour(DateTime time) { throw null; }

        public virtual int GetLeapMonth(int year, int era) { throw null; }

        public virtual double GetMilliseconds(DateTime time) { throw null; }

        public virtual int GetMinute(DateTime time) { throw null; }

        public abstract int GetMonth(DateTime time);
        public abstract int GetMonthsInYear(int year, int era);
        public virtual int GetMonthsInYear(int year) { throw null; }

        public virtual int GetSecond(DateTime time) { throw null; }

        public virtual int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek) { throw null; }

        public abstract int GetYear(DateTime time);
        public abstract bool IsLeapDay(int year, int month, int day, int era);
        public virtual bool IsLeapDay(int year, int month, int day) { throw null; }

        public abstract bool IsLeapMonth(int year, int month, int era);
        public virtual bool IsLeapMonth(int year, int month) { throw null; }

        public abstract bool IsLeapYear(int year, int era);
        public virtual bool IsLeapYear(int year) { throw null; }

        public abstract DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era);
        public virtual DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond) { throw null; }

        public virtual int ToFourDigitYear(int year) { throw null; }
    }

    public enum CalendarWeekRule
    {
        FirstDay = 0,
        FirstFullWeek = 1,
        FirstFourDayWeek = 2
    }

    public static partial class CharUnicodeInfo
    {
        public static double GetNumericValue(char ch) { throw null; }

        public static double GetNumericValue(string s, int index) { throw null; }

        public static UnicodeCategory GetUnicodeCategory(char ch) { throw null; }

        public static UnicodeCategory GetUnicodeCategory(string s, int index) { throw null; }
    }

    public partial class CompareInfo
    {
        internal CompareInfo() { }

        public virtual string Name { get { throw null; } }

        public virtual int Compare(string string1, int offset1, int length1, string string2, int offset2, int length2, CompareOptions options) { throw null; }

        public virtual int Compare(string string1, int offset1, int length1, string string2, int offset2, int length2) { throw null; }

        public virtual int Compare(string string1, int offset1, string string2, int offset2, CompareOptions options) { throw null; }

        public virtual int Compare(string string1, int offset1, string string2, int offset2) { throw null; }

        public virtual int Compare(string string1, string string2, CompareOptions options) { throw null; }

        public virtual int Compare(string string1, string string2) { throw null; }

        public override bool Equals(object value) { throw null; }

        public static CompareInfo GetCompareInfo(string name) { throw null; }

        public override int GetHashCode() { throw null; }

        public virtual int IndexOf(string source, char value, CompareOptions options) { throw null; }

        public virtual int IndexOf(string source, char value, int startIndex, CompareOptions options) { throw null; }

        public virtual int IndexOf(string source, char value, int startIndex, int count, CompareOptions options) { throw null; }

        public virtual int IndexOf(string source, char value, int startIndex, int count) { throw null; }

        public virtual int IndexOf(string source, char value) { throw null; }

        public virtual int IndexOf(string source, string value, CompareOptions options) { throw null; }

        public virtual int IndexOf(string source, string value, int startIndex, CompareOptions options) { throw null; }

        public virtual int IndexOf(string source, string value, int startIndex, int count, CompareOptions options) { throw null; }

        public virtual int IndexOf(string source, string value, int startIndex, int count) { throw null; }

        public virtual int IndexOf(string source, string value) { throw null; }

        public virtual bool IsPrefix(string source, string prefix, CompareOptions options) { throw null; }

        public virtual bool IsPrefix(string source, string prefix) { throw null; }

        public virtual bool IsSuffix(string source, string suffix, CompareOptions options) { throw null; }

        public virtual bool IsSuffix(string source, string suffix) { throw null; }

        public virtual int LastIndexOf(string source, char value, CompareOptions options) { throw null; }

        public virtual int LastIndexOf(string source, char value, int startIndex, CompareOptions options) { throw null; }

        public virtual int LastIndexOf(string source, char value, int startIndex, int count, CompareOptions options) { throw null; }

        public virtual int LastIndexOf(string source, char value, int startIndex, int count) { throw null; }

        public virtual int LastIndexOf(string source, char value) { throw null; }

        public virtual int LastIndexOf(string source, string value, CompareOptions options) { throw null; }

        public virtual int LastIndexOf(string source, string value, int startIndex, CompareOptions options) { throw null; }

        public virtual int LastIndexOf(string source, string value, int startIndex, int count, CompareOptions options) { throw null; }

        public virtual int LastIndexOf(string source, string value, int startIndex, int count) { throw null; }

        public virtual int LastIndexOf(string source, string value) { throw null; }

        public override string ToString() { throw null; }
    }

    [Flags]
    public enum CompareOptions
    {
        None = 0,
        IgnoreCase = 1,
        IgnoreNonSpace = 2,
        IgnoreSymbols = 4,
        IgnoreKanaType = 8,
        IgnoreWidth = 16,
        OrdinalIgnoreCase = 268435456,
        StringSort = 536870912,
        Ordinal = 1073741824
    }

    public partial class CultureInfo : IFormatProvider
    {
        public CultureInfo(string name) { }

        public virtual Calendar Calendar { get { throw null; } }

        public virtual CompareInfo CompareInfo { get { throw null; } }

        public static CultureInfo CurrentCulture { get { throw null; } }

        public static CultureInfo CurrentUICulture { get { throw null; } }

        public virtual DateTimeFormatInfo DateTimeFormat { get { throw null; } set { } }

        public static CultureInfo DefaultThreadCurrentCulture { get { throw null; } set { } }

        public static CultureInfo DefaultThreadCurrentUICulture { get { throw null; } set { } }

        public virtual string DisplayName { get { throw null; } }

        public virtual string EnglishName { get { throw null; } }

        public static CultureInfo InvariantCulture { get { throw null; } }

        public virtual bool IsNeutralCulture { get { throw null; } }

        public bool IsReadOnly { get { throw null; } }

        public virtual string Name { get { throw null; } }

        public virtual string NativeName { get { throw null; } }

        public virtual NumberFormatInfo NumberFormat { get { throw null; } set { } }

        public virtual Calendar[] OptionalCalendars { get { throw null; } }

        public virtual CultureInfo Parent { get { throw null; } }

        public virtual TextInfo TextInfo { get { throw null; } }

        public virtual string TwoLetterISOLanguageName { get { throw null; } }

        public virtual object Clone() { throw null; }

        public override bool Equals(object value) { throw null; }

        public virtual object GetFormat(Type formatType) { throw null; }

        public override int GetHashCode() { throw null; }

        public static CultureInfo ReadOnly(CultureInfo ci) { throw null; }

        public override string ToString() { throw null; }
    }

    public partial class CultureNotFoundException : ArgumentException
    {
        public CultureNotFoundException() { }

        public CultureNotFoundException(string message, Exception innerException) { }

        public CultureNotFoundException(string message, string invalidCultureName, Exception innerException) { }

        public CultureNotFoundException(string paramName, string invalidCultureName, string message) { }

        public CultureNotFoundException(string paramName, string message) { }

        public CultureNotFoundException(string message) { }

        public virtual string InvalidCultureName { get { throw null; } }

        public override string Message { get { throw null; } }
    }

    public sealed partial class DateTimeFormatInfo : IFormatProvider
    {
        public string[] AbbreviatedDayNames { get { throw null; } set { } }

        public string[] AbbreviatedMonthGenitiveNames { get { throw null; } set { } }

        public string[] AbbreviatedMonthNames { get { throw null; } set { } }

        public string AMDesignator { get { throw null; } set { } }

        public Calendar Calendar { get { throw null; } set { } }

        public CalendarWeekRule CalendarWeekRule { get { throw null; } set { } }

        public static DateTimeFormatInfo CurrentInfo { get { throw null; } }

        public string[] DayNames { get { throw null; } set { } }

        public DayOfWeek FirstDayOfWeek { get { throw null; } set { } }

        public string FullDateTimePattern { get { throw null; } set { } }

        public static DateTimeFormatInfo InvariantInfo { get { throw null; } }

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

        public string GetAbbreviatedDayName(DayOfWeek dayofweek) { throw null; }

        public string GetAbbreviatedEraName(int era) { throw null; }

        public string GetAbbreviatedMonthName(int month) { throw null; }

        public string GetDayName(DayOfWeek dayofweek) { throw null; }

        public int GetEra(string eraName) { throw null; }

        public string GetEraName(int era) { throw null; }

        public object GetFormat(Type formatType) { throw null; }

        public static DateTimeFormatInfo GetInstance(IFormatProvider provider) { throw null; }

        public string GetMonthName(int month) { throw null; }

        public static DateTimeFormatInfo ReadOnly(DateTimeFormatInfo dtfi) { throw null; }
    }

    public sealed partial class NumberFormatInfo : IFormatProvider
    {
        public int CurrencyDecimalDigits { get { throw null; } set { } }

        public string CurrencyDecimalSeparator { get { throw null; } set { } }

        public string CurrencyGroupSeparator { get { throw null; } set { } }

        public int[] CurrencyGroupSizes { get { throw null; } set { } }

        public int CurrencyNegativePattern { get { throw null; } set { } }

        public int CurrencyPositivePattern { get { throw null; } set { } }

        public string CurrencySymbol { get { throw null; } set { } }

        public static NumberFormatInfo CurrentInfo { get { throw null; } }

        public static NumberFormatInfo InvariantInfo { get { throw null; } }

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

        public object GetFormat(Type formatType) { throw null; }

        public static NumberFormatInfo GetInstance(IFormatProvider formatProvider) { throw null; }

        public static NumberFormatInfo ReadOnly(NumberFormatInfo nfi) { throw null; }
    }

    public partial class RegionInfo
    {
        public RegionInfo(string name) { }

        public virtual string CurrencySymbol { get { throw null; } }

        public static RegionInfo CurrentRegion { get { throw null; } }

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

        public static string GetNextTextElement(string str, int index) { throw null; }

        public static string GetNextTextElement(string str) { throw null; }

        public static TextElementEnumerator GetTextElementEnumerator(string str, int index) { throw null; }

        public static TextElementEnumerator GetTextElementEnumerator(string str) { throw null; }

        public static int[] ParseCombiningCharacters(string str) { throw null; }
    }

    public partial class TextElementEnumerator : Collections.IEnumerator
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
        UppercaseLetter = 0,
        LowercaseLetter = 1,
        TitlecaseLetter = 2,
        ModifierLetter = 3,
        OtherLetter = 4,
        NonSpacingMark = 5,
        SpacingCombiningMark = 6,
        EnclosingMark = 7,
        DecimalDigitNumber = 8,
        LetterNumber = 9,
        OtherNumber = 10,
        SpaceSeparator = 11,
        LineSeparator = 12,
        ParagraphSeparator = 13,
        Control = 14,
        Format = 15,
        Surrogate = 16,
        PrivateUse = 17,
        ConnectorPunctuation = 18,
        DashPunctuation = 19,
        OpenPunctuation = 20,
        ClosePunctuation = 21,
        InitialQuotePunctuation = 22,
        FinalQuotePunctuation = 23,
        OtherPunctuation = 24,
        MathSymbol = 25,
        CurrencySymbol = 26,
        ModifierSymbol = 27,
        OtherSymbol = 28,
        OtherNotAssigned = 29
    }
}