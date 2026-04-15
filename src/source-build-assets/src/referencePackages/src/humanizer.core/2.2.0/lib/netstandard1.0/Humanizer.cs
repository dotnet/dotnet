// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Resources.NeutralResourcesLanguage("en")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETStandard,Version=v1.0", FrameworkDisplayName = "")]
[assembly: System.Reflection.AssemblyCompany("Mehdi Khalili, Oren Novotny")]
[assembly: System.Reflection.AssemblyConfiguration("Release")]
[assembly: System.Reflection.AssemblyCopyright("Copyright © 2012-2017 Mehdi Khalili")]
[assembly: System.Reflection.AssemblyDescription("A micro-framework that turns your normal strings, type names, enum fields, date fields ETC into a human friendly format")]
[assembly: System.Reflection.AssemblyFileVersion("2.2.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersion("2.2.0+Branch.master.Sha.b30550eed103a6970d8465fe7c5c16300b70be81")]
[assembly: System.Reflection.AssemblyProduct("Humanizer (netstandard1.0)")]
[assembly: System.Reflection.AssemblyTitle("Humanizer")]
[assembly: System.Reflection.AssemblyVersionAttribute("2.2.0.0")]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace Humanizer
{
    public static partial class ByteSizeExtensions
    {
        public static Bytes.ByteSize Bits(this byte input) { throw null; }

        public static Bytes.ByteSize Bits(this short input) { throw null; }

        public static Bytes.ByteSize Bits(this int input) { throw null; }

        public static Bytes.ByteSize Bits(this long input) { throw null; }

        public static Bytes.ByteSize Bits(this sbyte input) { throw null; }

        public static Bytes.ByteSize Bits(this ushort input) { throw null; }

        public static Bytes.ByteSize Bits(this uint input) { throw null; }

        public static Bytes.ByteSize Bytes(this byte input) { throw null; }

        public static Bytes.ByteSize Bytes(this double input) { throw null; }

        public static Bytes.ByteSize Bytes(this short input) { throw null; }

        public static Bytes.ByteSize Bytes(this int input) { throw null; }

        public static Bytes.ByteSize Bytes(this long input) { throw null; }

        public static Bytes.ByteSize Bytes(this sbyte input) { throw null; }

        public static Bytes.ByteSize Bytes(this ushort input) { throw null; }

        public static Bytes.ByteSize Bytes(this uint input) { throw null; }

        public static Bytes.ByteSize Gigabytes(this byte input) { throw null; }

        public static Bytes.ByteSize Gigabytes(this double input) { throw null; }

        public static Bytes.ByteSize Gigabytes(this short input) { throw null; }

        public static Bytes.ByteSize Gigabytes(this int input) { throw null; }

        public static Bytes.ByteSize Gigabytes(this long input) { throw null; }

        public static Bytes.ByteSize Gigabytes(this sbyte input) { throw null; }

        public static Bytes.ByteSize Gigabytes(this ushort input) { throw null; }

        public static Bytes.ByteSize Gigabytes(this uint input) { throw null; }

        public static string Humanize(this Bytes.ByteSize input, string format = null) { throw null; }

        public static Bytes.ByteSize Kilobytes(this byte input) { throw null; }

        public static Bytes.ByteSize Kilobytes(this double input) { throw null; }

        public static Bytes.ByteSize Kilobytes(this short input) { throw null; }

        public static Bytes.ByteSize Kilobytes(this int input) { throw null; }

        public static Bytes.ByteSize Kilobytes(this long input) { throw null; }

        public static Bytes.ByteSize Kilobytes(this sbyte input) { throw null; }

        public static Bytes.ByteSize Kilobytes(this ushort input) { throw null; }

        public static Bytes.ByteSize Kilobytes(this uint input) { throw null; }

        public static Bytes.ByteSize Megabytes(this byte input) { throw null; }

        public static Bytes.ByteSize Megabytes(this double input) { throw null; }

        public static Bytes.ByteSize Megabytes(this short input) { throw null; }

        public static Bytes.ByteSize Megabytes(this int input) { throw null; }

        public static Bytes.ByteSize Megabytes(this long input) { throw null; }

        public static Bytes.ByteSize Megabytes(this sbyte input) { throw null; }

        public static Bytes.ByteSize Megabytes(this ushort input) { throw null; }

        public static Bytes.ByteSize Megabytes(this uint input) { throw null; }

        public static Bytes.ByteRate Per(this Bytes.ByteSize size, System.TimeSpan interval) { throw null; }

        public static Bytes.ByteSize Terabytes(this byte input) { throw null; }

        public static Bytes.ByteSize Terabytes(this double input) { throw null; }

        public static Bytes.ByteSize Terabytes(this short input) { throw null; }

        public static Bytes.ByteSize Terabytes(this int input) { throw null; }

        public static Bytes.ByteSize Terabytes(this long input) { throw null; }

        public static Bytes.ByteSize Terabytes(this sbyte input) { throw null; }

        public static Bytes.ByteSize Terabytes(this ushort input) { throw null; }

        public static Bytes.ByteSize Terabytes(this uint input) { throw null; }
    }

    public static partial class CasingExtensions
    {
        public static string ApplyCase(this string input, LetterCasing casing) { throw null; }
    }

    public static partial class CollectionHumanizeExtensions
    {
        public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T, string> displayFormatter, string separator) { throw null; }

        public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T, string> displayFormatter) { throw null; }

        public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, string separator) { throw null; }

        public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection) { throw null; }
    }

    public static partial class DateHumanizeExtensions
    {
        public static string Humanize(this System.DateTime input, bool utcDate = true, System.DateTime? dateToCompareAgainst = null, System.Globalization.CultureInfo culture = null) { throw null; }

        public static string Humanize(this System.DateTimeOffset input, System.DateTimeOffset? dateToCompareAgainst = null, System.Globalization.CultureInfo culture = null) { throw null; }

        public static string Humanize(this System.DateTime? input, bool utcDate = true, System.DateTime? dateToCompareAgainst = null, System.Globalization.CultureInfo culture = null) { throw null; }

        public static string Humanize(this System.DateTimeOffset? input, System.DateTimeOffset? dateToCompareAgainst = null, System.Globalization.CultureInfo culture = null) { throw null; }
    }

    public static partial class DateToOrdinalWordsExtensions
    {
        public static string ToOrdinalWords(this System.DateTime input, GrammaticalCase grammaticalCase) { throw null; }

        public static string ToOrdinalWords(this System.DateTime input) { throw null; }
    }

    public static partial class EnumDehumanizeExtensions
    {
        public static System.Enum DehumanizeTo(this string input, System.Type targetEnum, OnNoMatch onNoMatch = OnNoMatch.ThrowsException) { throw null; }

        public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input)
            where TTargetEnum : struct, System.IComparable, System.IFormattable { throw null; }
    }

    public static partial class EnumHumanizeExtensions
    {
        public static string Humanize(this System.Enum input, LetterCasing casing) { throw null; }

        public static string Humanize(this System.Enum input) { throw null; }
    }

    public enum GrammaticalCase
    {
        Nominative = 0,
        Genitive = 1,
        Dative = 2,
        Accusative = 3,
        Instrumental = 4,
        Prepositional = 5
    }

    public enum GrammaticalGender
    {
        Masculine = 0,
        Feminine = 1,
        Neuter = 2
    }

    public partial class In
    {
        public static System.DateTime April { get { throw null; } }

        public static System.DateTime August { get { throw null; } }

        public static System.DateTime December { get { throw null; } }

        public static System.DateTime February { get { throw null; } }

        public static System.DateTime January { get { throw null; } }

        public static System.DateTime July { get { throw null; } }

        public static System.DateTime June { get { throw null; } }

        public static System.DateTime March { get { throw null; } }

        public static System.DateTime May { get { throw null; } }

        public static System.DateTime November { get { throw null; } }

        public static System.DateTime October { get { throw null; } }

        public static System.DateTime September { get { throw null; } }

        public static System.DateTime AprilOf(int year) { throw null; }

        public static System.DateTime AugustOf(int year) { throw null; }

        public static System.DateTime DecemberOf(int year) { throw null; }

        public static System.DateTime FebruaryOf(int year) { throw null; }

        public static System.DateTime JanuaryOf(int year) { throw null; }

        public static System.DateTime JulyOf(int year) { throw null; }

        public static System.DateTime JuneOf(int year) { throw null; }

        public static System.DateTime MarchOf(int year) { throw null; }

        public static System.DateTime MayOf(int year) { throw null; }

        public static System.DateTime NovemberOf(int year) { throw null; }

        public static System.DateTime OctoberOf(int year) { throw null; }

        public static System.DateTime SeptemberOf(int year) { throw null; }

        public static System.DateTime TheYear(int year) { throw null; }

        public static partial class Eight
        {
            public static System.DateTime Days { get { throw null; } }

            public static System.DateTime Hours { get { throw null; } }

            public static System.DateTime Minutes { get { throw null; } }

            public static System.DateTime Months { get { throw null; } }

            public static System.DateTime Seconds { get { throw null; } }

            public static System.DateTime Weeks { get { throw null; } }

            public static System.DateTime Years { get { throw null; } }

            public static System.DateTime DaysFrom(System.DateTime date) { throw null; }

            public static System.DateTime HoursFrom(System.DateTime date) { throw null; }

            public static System.DateTime MinutesFrom(System.DateTime date) { throw null; }

            public static System.DateTime MonthsFrom(System.DateTime date) { throw null; }

            public static System.DateTime SecondsFrom(System.DateTime date) { throw null; }

            public static System.DateTime WeeksFrom(System.DateTime date) { throw null; }

            public static System.DateTime YearsFrom(System.DateTime date) { throw null; }
        }

        public static partial class Five
        {
            public static System.DateTime Days { get { throw null; } }

            public static System.DateTime Hours { get { throw null; } }

            public static System.DateTime Minutes { get { throw null; } }

            public static System.DateTime Months { get { throw null; } }

            public static System.DateTime Seconds { get { throw null; } }

            public static System.DateTime Weeks { get { throw null; } }

            public static System.DateTime Years { get { throw null; } }

            public static System.DateTime DaysFrom(System.DateTime date) { throw null; }

            public static System.DateTime HoursFrom(System.DateTime date) { throw null; }

            public static System.DateTime MinutesFrom(System.DateTime date) { throw null; }

            public static System.DateTime MonthsFrom(System.DateTime date) { throw null; }

            public static System.DateTime SecondsFrom(System.DateTime date) { throw null; }

            public static System.DateTime WeeksFrom(System.DateTime date) { throw null; }

            public static System.DateTime YearsFrom(System.DateTime date) { throw null; }
        }

        public static partial class Four
        {
            public static System.DateTime Days { get { throw null; } }

            public static System.DateTime Hours { get { throw null; } }

            public static System.DateTime Minutes { get { throw null; } }

            public static System.DateTime Months { get { throw null; } }

            public static System.DateTime Seconds { get { throw null; } }

            public static System.DateTime Weeks { get { throw null; } }

            public static System.DateTime Years { get { throw null; } }

            public static System.DateTime DaysFrom(System.DateTime date) { throw null; }

            public static System.DateTime HoursFrom(System.DateTime date) { throw null; }

            public static System.DateTime MinutesFrom(System.DateTime date) { throw null; }

            public static System.DateTime MonthsFrom(System.DateTime date) { throw null; }

            public static System.DateTime SecondsFrom(System.DateTime date) { throw null; }

            public static System.DateTime WeeksFrom(System.DateTime date) { throw null; }

            public static System.DateTime YearsFrom(System.DateTime date) { throw null; }
        }

        public static partial class Nine
        {
            public static System.DateTime Days { get { throw null; } }

            public static System.DateTime Hours { get { throw null; } }

            public static System.DateTime Minutes { get { throw null; } }

            public static System.DateTime Months { get { throw null; } }

            public static System.DateTime Seconds { get { throw null; } }

            public static System.DateTime Weeks { get { throw null; } }

            public static System.DateTime Years { get { throw null; } }

            public static System.DateTime DaysFrom(System.DateTime date) { throw null; }

            public static System.DateTime HoursFrom(System.DateTime date) { throw null; }

            public static System.DateTime MinutesFrom(System.DateTime date) { throw null; }

            public static System.DateTime MonthsFrom(System.DateTime date) { throw null; }

            public static System.DateTime SecondsFrom(System.DateTime date) { throw null; }

            public static System.DateTime WeeksFrom(System.DateTime date) { throw null; }

            public static System.DateTime YearsFrom(System.DateTime date) { throw null; }
        }

        public static partial class One
        {
            public static System.DateTime Day { get { throw null; } }

            public static System.DateTime Hour { get { throw null; } }

            public static System.DateTime Minute { get { throw null; } }

            public static System.DateTime Month { get { throw null; } }

            public static System.DateTime Second { get { throw null; } }

            public static System.DateTime Week { get { throw null; } }

            public static System.DateTime Year { get { throw null; } }

            public static System.DateTime DayFrom(System.DateTime date) { throw null; }

            public static System.DateTime HourFrom(System.DateTime date) { throw null; }

            public static System.DateTime MinuteFrom(System.DateTime date) { throw null; }

            public static System.DateTime MonthFrom(System.DateTime date) { throw null; }

            public static System.DateTime SecondFrom(System.DateTime date) { throw null; }

            public static System.DateTime WeekFrom(System.DateTime date) { throw null; }

            public static System.DateTime YearFrom(System.DateTime date) { throw null; }
        }

        public static partial class Seven
        {
            public static System.DateTime Days { get { throw null; } }

            public static System.DateTime Hours { get { throw null; } }

            public static System.DateTime Minutes { get { throw null; } }

            public static System.DateTime Months { get { throw null; } }

            public static System.DateTime Seconds { get { throw null; } }

            public static System.DateTime Weeks { get { throw null; } }

            public static System.DateTime Years { get { throw null; } }

            public static System.DateTime DaysFrom(System.DateTime date) { throw null; }

            public static System.DateTime HoursFrom(System.DateTime date) { throw null; }

            public static System.DateTime MinutesFrom(System.DateTime date) { throw null; }

            public static System.DateTime MonthsFrom(System.DateTime date) { throw null; }

            public static System.DateTime SecondsFrom(System.DateTime date) { throw null; }

            public static System.DateTime WeeksFrom(System.DateTime date) { throw null; }

            public static System.DateTime YearsFrom(System.DateTime date) { throw null; }
        }

        public static partial class Six
        {
            public static System.DateTime Days { get { throw null; } }

            public static System.DateTime Hours { get { throw null; } }

            public static System.DateTime Minutes { get { throw null; } }

            public static System.DateTime Months { get { throw null; } }

            public static System.DateTime Seconds { get { throw null; } }

            public static System.DateTime Weeks { get { throw null; } }

            public static System.DateTime Years { get { throw null; } }

            public static System.DateTime DaysFrom(System.DateTime date) { throw null; }

            public static System.DateTime HoursFrom(System.DateTime date) { throw null; }

            public static System.DateTime MinutesFrom(System.DateTime date) { throw null; }

            public static System.DateTime MonthsFrom(System.DateTime date) { throw null; }

            public static System.DateTime SecondsFrom(System.DateTime date) { throw null; }

            public static System.DateTime WeeksFrom(System.DateTime date) { throw null; }

            public static System.DateTime YearsFrom(System.DateTime date) { throw null; }
        }

        public static partial class Ten
        {
            public static System.DateTime Days { get { throw null; } }

            public static System.DateTime Hours { get { throw null; } }

            public static System.DateTime Minutes { get { throw null; } }

            public static System.DateTime Months { get { throw null; } }

            public static System.DateTime Seconds { get { throw null; } }

            public static System.DateTime Weeks { get { throw null; } }

            public static System.DateTime Years { get { throw null; } }

            public static System.DateTime DaysFrom(System.DateTime date) { throw null; }

            public static System.DateTime HoursFrom(System.DateTime date) { throw null; }

            public static System.DateTime MinutesFrom(System.DateTime date) { throw null; }

            public static System.DateTime MonthsFrom(System.DateTime date) { throw null; }

            public static System.DateTime SecondsFrom(System.DateTime date) { throw null; }

            public static System.DateTime WeeksFrom(System.DateTime date) { throw null; }

            public static System.DateTime YearsFrom(System.DateTime date) { throw null; }
        }

        public static partial class Three
        {
            public static System.DateTime Days { get { throw null; } }

            public static System.DateTime Hours { get { throw null; } }

            public static System.DateTime Minutes { get { throw null; } }

            public static System.DateTime Months { get { throw null; } }

            public static System.DateTime Seconds { get { throw null; } }

            public static System.DateTime Weeks { get { throw null; } }

            public static System.DateTime Years { get { throw null; } }

            public static System.DateTime DaysFrom(System.DateTime date) { throw null; }

            public static System.DateTime HoursFrom(System.DateTime date) { throw null; }

            public static System.DateTime MinutesFrom(System.DateTime date) { throw null; }

            public static System.DateTime MonthsFrom(System.DateTime date) { throw null; }

            public static System.DateTime SecondsFrom(System.DateTime date) { throw null; }

            public static System.DateTime WeeksFrom(System.DateTime date) { throw null; }

            public static System.DateTime YearsFrom(System.DateTime date) { throw null; }
        }

        public static partial class Two
        {
            public static System.DateTime Days { get { throw null; } }

            public static System.DateTime Hours { get { throw null; } }

            public static System.DateTime Minutes { get { throw null; } }

            public static System.DateTime Months { get { throw null; } }

            public static System.DateTime Seconds { get { throw null; } }

            public static System.DateTime Weeks { get { throw null; } }

            public static System.DateTime Years { get { throw null; } }

            public static System.DateTime DaysFrom(System.DateTime date) { throw null; }

            public static System.DateTime HoursFrom(System.DateTime date) { throw null; }

            public static System.DateTime MinutesFrom(System.DateTime date) { throw null; }

            public static System.DateTime MonthsFrom(System.DateTime date) { throw null; }

            public static System.DateTime SecondsFrom(System.DateTime date) { throw null; }

            public static System.DateTime WeeksFrom(System.DateTime date) { throw null; }

            public static System.DateTime YearsFrom(System.DateTime date) { throw null; }
        }
    }

    public static partial class InflectorExtensions
    {
        public static string Camelize(this string input) { throw null; }

        public static string Dasherize(this string underscoredWord) { throw null; }

        public static string Hyphenate(this string underscoredWord) { throw null; }

        public static string Kebaberize(this string input) { throw null; }

        public static string Pascalize(this string input) { throw null; }

        public static string Pluralize(this string word, bool inputIsKnownToBeSingular = true) { throw null; }

        public static string Singularize(this string word, bool inputIsKnownToBePlural = true) { throw null; }

        public static string Titleize(this string input) { throw null; }

        public static string Underscore(this string input) { throw null; }
    }

    public partial interface IStringTransformer
    {
        string Transform(string input);
    }

    public partial interface ITruncator
    {
        string Truncate(string value, int length, string truncationString, TruncateFrom truncateFrom = TruncateFrom.Right);
    }

    public enum LetterCasing
    {
        Title = 0,
        AllCaps = 1,
        LowerCase = 2,
        Sentence = 3
    }

    public static partial class MetricNumeralExtensions
    {
        public static double FromMetric(this string input) { throw null; }

        public static string ToMetric(this double input, bool hasSpace = false, bool useSymbol = true, int? decimals = null) { throw null; }

        public static string ToMetric(this int input, bool hasSpace = false, bool useSymbol = true, int? decimals = null) { throw null; }
    }

    public partial class NoMatchFoundException : System.Exception
    {
        public NoMatchFoundException() { }

        public NoMatchFoundException(string message, System.Exception inner) { }

        public NoMatchFoundException(string message) { }
    }

    public static partial class NumberToNumberExtensions
    {
        public static double Billions(this double input) { throw null; }

        public static int Billions(this int input) { throw null; }

        public static long Billions(this long input) { throw null; }

        public static uint Billions(this uint input) { throw null; }

        public static ulong Billions(this ulong input) { throw null; }

        public static double Hundreds(this double input) { throw null; }

        public static int Hundreds(this int input) { throw null; }

        public static long Hundreds(this long input) { throw null; }

        public static uint Hundreds(this uint input) { throw null; }

        public static ulong Hundreds(this ulong input) { throw null; }

        public static double Millions(this double input) { throw null; }

        public static int Millions(this int input) { throw null; }

        public static long Millions(this long input) { throw null; }

        public static uint Millions(this uint input) { throw null; }

        public static ulong Millions(this ulong input) { throw null; }

        public static double Tens(this double input) { throw null; }

        public static int Tens(this int input) { throw null; }

        public static long Tens(this long input) { throw null; }

        public static uint Tens(this uint input) { throw null; }

        public static ulong Tens(this ulong input) { throw null; }

        public static double Thousands(this double input) { throw null; }

        public static int Thousands(this int input) { throw null; }

        public static long Thousands(this long input) { throw null; }

        public static uint Thousands(this uint input) { throw null; }

        public static ulong Thousands(this ulong input) { throw null; }
    }

    public static partial class NumberToTimeSpanExtensions
    {
        public static System.TimeSpan Days(this byte days) { throw null; }

        public static System.TimeSpan Days(this double days) { throw null; }

        public static System.TimeSpan Days(this short days) { throw null; }

        public static System.TimeSpan Days(this int days) { throw null; }

        public static System.TimeSpan Days(this long days) { throw null; }

        public static System.TimeSpan Days(this sbyte days) { throw null; }

        public static System.TimeSpan Days(this ushort days) { throw null; }

        public static System.TimeSpan Days(this uint days) { throw null; }

        public static System.TimeSpan Days(this ulong days) { throw null; }

        public static System.TimeSpan Hours(this byte hours) { throw null; }

        public static System.TimeSpan Hours(this double hours) { throw null; }

        public static System.TimeSpan Hours(this short hours) { throw null; }

        public static System.TimeSpan Hours(this int hours) { throw null; }

        public static System.TimeSpan Hours(this long hours) { throw null; }

        public static System.TimeSpan Hours(this sbyte hours) { throw null; }

        public static System.TimeSpan Hours(this ushort hours) { throw null; }

        public static System.TimeSpan Hours(this uint hours) { throw null; }

        public static System.TimeSpan Hours(this ulong hours) { throw null; }

        public static System.TimeSpan Milliseconds(this byte ms) { throw null; }

        public static System.TimeSpan Milliseconds(this double ms) { throw null; }

        public static System.TimeSpan Milliseconds(this short ms) { throw null; }

        public static System.TimeSpan Milliseconds(this int ms) { throw null; }

        public static System.TimeSpan Milliseconds(this long ms) { throw null; }

        public static System.TimeSpan Milliseconds(this sbyte ms) { throw null; }

        public static System.TimeSpan Milliseconds(this ushort ms) { throw null; }

        public static System.TimeSpan Milliseconds(this uint ms) { throw null; }

        public static System.TimeSpan Milliseconds(this ulong ms) { throw null; }

        public static System.TimeSpan Minutes(this byte minutes) { throw null; }

        public static System.TimeSpan Minutes(this double minutes) { throw null; }

        public static System.TimeSpan Minutes(this short minutes) { throw null; }

        public static System.TimeSpan Minutes(this int minutes) { throw null; }

        public static System.TimeSpan Minutes(this long minutes) { throw null; }

        public static System.TimeSpan Minutes(this sbyte minutes) { throw null; }

        public static System.TimeSpan Minutes(this ushort minutes) { throw null; }

        public static System.TimeSpan Minutes(this uint minutes) { throw null; }

        public static System.TimeSpan Minutes(this ulong minutes) { throw null; }

        public static System.TimeSpan Seconds(this byte seconds) { throw null; }

        public static System.TimeSpan Seconds(this double seconds) { throw null; }

        public static System.TimeSpan Seconds(this short seconds) { throw null; }

        public static System.TimeSpan Seconds(this int seconds) { throw null; }

        public static System.TimeSpan Seconds(this long seconds) { throw null; }

        public static System.TimeSpan Seconds(this sbyte seconds) { throw null; }

        public static System.TimeSpan Seconds(this ushort seconds) { throw null; }

        public static System.TimeSpan Seconds(this uint seconds) { throw null; }

        public static System.TimeSpan Seconds(this ulong seconds) { throw null; }

        public static System.TimeSpan Weeks(this byte input) { throw null; }

        public static System.TimeSpan Weeks(this double input) { throw null; }

        public static System.TimeSpan Weeks(this short input) { throw null; }

        public static System.TimeSpan Weeks(this int input) { throw null; }

        public static System.TimeSpan Weeks(this long input) { throw null; }

        public static System.TimeSpan Weeks(this sbyte input) { throw null; }

        public static System.TimeSpan Weeks(this ushort input) { throw null; }

        public static System.TimeSpan Weeks(this uint input) { throw null; }

        public static System.TimeSpan Weeks(this ulong input) { throw null; }
    }

    public static partial class NumberToWordsExtension
    {
        public static string ToOrdinalWords(this int number, GrammaticalGender gender, System.Globalization.CultureInfo culture = null) { throw null; }

        public static string ToOrdinalWords(this int number, System.Globalization.CultureInfo culture = null) { throw null; }

        public static string ToWords(this int number, GrammaticalGender gender, System.Globalization.CultureInfo culture = null) { throw null; }

        public static string ToWords(this int number, System.Globalization.CultureInfo culture = null) { throw null; }

        public static string ToWords(this long number, GrammaticalGender gender, System.Globalization.CultureInfo culture = null) { throw null; }

        public static string ToWords(this long number, System.Globalization.CultureInfo culture = null) { throw null; }
    }

    public partial class On
    {
        public partial class April
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class August
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The31st { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class December
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The31st { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class February
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class January
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The31st { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class July
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The31st { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class June
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class March
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The31st { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class May
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The31st { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class November
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class October
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The31st { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }

        public partial class September
        {
            public static System.DateTime The10th { get { throw null; } }

            public static System.DateTime The11th { get { throw null; } }

            public static System.DateTime The12th { get { throw null; } }

            public static System.DateTime The13th { get { throw null; } }

            public static System.DateTime The14th { get { throw null; } }

            public static System.DateTime The15th { get { throw null; } }

            public static System.DateTime The16th { get { throw null; } }

            public static System.DateTime The17th { get { throw null; } }

            public static System.DateTime The18th { get { throw null; } }

            public static System.DateTime The19th { get { throw null; } }

            public static System.DateTime The1st { get { throw null; } }

            public static System.DateTime The20th { get { throw null; } }

            public static System.DateTime The21st { get { throw null; } }

            public static System.DateTime The22nd { get { throw null; } }

            public static System.DateTime The23rd { get { throw null; } }

            public static System.DateTime The24th { get { throw null; } }

            public static System.DateTime The25th { get { throw null; } }

            public static System.DateTime The26th { get { throw null; } }

            public static System.DateTime The27th { get { throw null; } }

            public static System.DateTime The28th { get { throw null; } }

            public static System.DateTime The29th { get { throw null; } }

            public static System.DateTime The2nd { get { throw null; } }

            public static System.DateTime The30th { get { throw null; } }

            public static System.DateTime The3rd { get { throw null; } }

            public static System.DateTime The4th { get { throw null; } }

            public static System.DateTime The5th { get { throw null; } }

            public static System.DateTime The6th { get { throw null; } }

            public static System.DateTime The7th { get { throw null; } }

            public static System.DateTime The8th { get { throw null; } }

            public static System.DateTime The9th { get { throw null; } }

            public static System.DateTime The(int dayNumber) { throw null; }
        }
    }

    public enum OnNoMatch
    {
        ThrowsException = 0,
        ReturnsNull = 1
    }

    public static partial class OrdinalizeExtensions
    {
        public static string Ordinalize(this int number, GrammaticalGender gender) { throw null; }

        public static string Ordinalize(this int number) { throw null; }

        public static string Ordinalize(this string numberString, GrammaticalGender gender) { throw null; }

        public static string Ordinalize(this string numberString) { throw null; }
    }

    public enum Plurality
    {
        Singular = 0,
        Plural = 1,
        CouldBeEither = 2
    }

    public static partial class PrepositionsExtensions
    {
        public static System.DateTime At(this System.DateTime date, int hour, int min = 0, int second = 0, int millisecond = 0) { throw null; }

        public static System.DateTime AtMidnight(this System.DateTime date) { throw null; }

        public static System.DateTime AtNoon(this System.DateTime date) { throw null; }

        public static System.DateTime In(this System.DateTime date, int year) { throw null; }
    }

    public static partial class RomanNumeralExtensions
    {
        public static int FromRoman(this string input) { throw null; }

        public static string ToRoman(this int input) { throw null; }
    }

    public enum ShowQuantityAs
    {
        None = 0,
        Numeric = 1,
        Words = 2
    }

    public static partial class StringDehumanizeExtensions
    {
        public static string Dehumanize(this string input) { throw null; }
    }

    public static partial class StringExtensions
    {
        public static string FormatWith(this string format, System.IFormatProvider provider, params object[] args) { throw null; }

        public static string FormatWith(this string format, params object[] args) { throw null; }
    }

    public static partial class StringHumanizeExtensions
    {
        public static string Humanize(this string input, LetterCasing casing) { throw null; }

        public static string Humanize(this string input) { throw null; }
    }

    public static partial class TimeSpanHumanizeExtensions
    {
        public static string Humanize(this System.TimeSpan timeSpan, int precision, bool countEmptyUnits, System.Globalization.CultureInfo culture = null, Localisation.TimeUnit maxUnit = Localisation.TimeUnit.Week, Localisation.TimeUnit minUnit = Localisation.TimeUnit.Millisecond, string collectionSeparator = ", ") { throw null; }

        public static string Humanize(this System.TimeSpan timeSpan, int precision = 1, System.Globalization.CultureInfo culture = null, Localisation.TimeUnit maxUnit = Localisation.TimeUnit.Week, Localisation.TimeUnit minUnit = Localisation.TimeUnit.Millisecond, string collectionSeparator = ", ") { throw null; }
    }

    public static partial class To
    {
        public static IStringTransformer LowerCase { get { throw null; } }

        public static IStringTransformer SentenceCase { get { throw null; } }

        public static IStringTransformer TitleCase { get { throw null; } }

        public static IStringTransformer UpperCase { get { throw null; } }

        public static string Transform(this string input, params IStringTransformer[] transformers) { throw null; }
    }

    public static partial class ToQuantityExtensions
    {
        public static string ToQuantity(this string input, int quantity, ShowQuantityAs showQuantityAs = ShowQuantityAs.Numeric) { throw null; }

        public static string ToQuantity(this string input, int quantity, string format, System.IFormatProvider formatProvider = null) { throw null; }

        public static string ToQuantity(this string input, long quantity, ShowQuantityAs showQuantityAs = ShowQuantityAs.Numeric) { throw null; }

        public static string ToQuantity(this string input, long quantity, string format, System.IFormatProvider formatProvider = null) { throw null; }
    }

    public static partial class TruncateExtensions
    {
        public static string Truncate(this string input, int length, ITruncator truncator, TruncateFrom from = TruncateFrom.Right) { throw null; }

        public static string Truncate(this string input, int length, string truncationString, ITruncator truncator, TruncateFrom from = TruncateFrom.Right) { throw null; }

        public static string Truncate(this string input, int length, string truncationString, TruncateFrom from = TruncateFrom.Right) { throw null; }

        public static string Truncate(this string input, int length) { throw null; }
    }

    public enum TruncateFrom
    {
        Left = 0,
        Right = 1
    }

    public static partial class Truncator
    {
        public static ITruncator FixedLength { get { throw null; } }

        public static ITruncator FixedNumberOfCharacters { get { throw null; } }

        public static ITruncator FixedNumberOfWords { get { throw null; } }
    }
}

namespace Humanizer.Bytes
{
    public partial class ByteRate
    {
        public ByteRate(ByteSize size, System.TimeSpan interval) { }

        public System.TimeSpan Interval { get { throw null; } }

        public ByteSize Size { get { throw null; } }

        public string Humanize(Localisation.TimeUnit timeUnit = Localisation.TimeUnit.Second) { throw null; }

        public string Humanize(string format, Localisation.TimeUnit timeUnit = Localisation.TimeUnit.Second) { throw null; }
    }

    public partial struct ByteSize : System.IComparable<ByteSize>, System.IEquatable<ByteSize>, System.IComparable
    {
        private int _dummyPrimitive;
        public const long BitsInByte = 8L;
        public const string BitSymbol = "b";
        public const long BytesInGigabyte = 1073741824L;
        public const long BytesInKilobyte = 1024L;
        public const long BytesInMegabyte = 1048576L;
        public const long BytesInTerabyte = 1099511627776L;
        public const string ByteSymbol = "B";
        public const string GigabyteSymbol = "GB";
        public const string KilobyteSymbol = "KB";
        public static readonly ByteSize MaxValue;
        public const string MegabyteSymbol = "MB";
        public static readonly ByteSize MinValue;
        public const string TerabyteSymbol = "TB";
        public ByteSize(double byteSize) { }

        public long Bits { get { throw null; } }

        public double Bytes { get { throw null; } }

        public double Gigabytes { get { throw null; } }

        public double Kilobytes { get { throw null; } }

        public string LargestWholeNumberSymbol { get { throw null; } }

        public double LargestWholeNumberValue { get { throw null; } }

        public double Megabytes { get { throw null; } }

        public double Terabytes { get { throw null; } }

        public ByteSize Add(ByteSize bs) { throw null; }

        public ByteSize AddBits(long value) { throw null; }

        public ByteSize AddBytes(double value) { throw null; }

        public ByteSize AddGigabytes(double value) { throw null; }

        public ByteSize AddKilobytes(double value) { throw null; }

        public ByteSize AddMegabytes(double value) { throw null; }

        public ByteSize AddTerabytes(double value) { throw null; }

        public int CompareTo(ByteSize other) { throw null; }

        public int CompareTo(object obj) { throw null; }

        public bool Equals(ByteSize value) { throw null; }

        public override bool Equals(object value) { throw null; }

        public static ByteSize FromBits(long value) { throw null; }

        public static ByteSize FromBytes(double value) { throw null; }

        public static ByteSize FromGigabytes(double value) { throw null; }

        public static ByteSize FromKilobytes(double value) { throw null; }

        public static ByteSize FromMegabytes(double value) { throw null; }

        public static ByteSize FromTerabytes(double value) { throw null; }

        public override int GetHashCode() { throw null; }

        public static ByteSize operator +(ByteSize b1, ByteSize b2) { throw null; }

        public static ByteSize operator --(ByteSize b) { throw null; }

        public static bool operator ==(ByteSize b1, ByteSize b2) { throw null; }

        public static bool operator >(ByteSize b1, ByteSize b2) { throw null; }

        public static bool operator >=(ByteSize b1, ByteSize b2) { throw null; }

        public static ByteSize operator ++(ByteSize b) { throw null; }

        public static bool operator !=(ByteSize b1, ByteSize b2) { throw null; }

        public static bool operator <(ByteSize b1, ByteSize b2) { throw null; }

        public static bool operator <=(ByteSize b1, ByteSize b2) { throw null; }

        public static ByteSize operator -(ByteSize b) { throw null; }

        public static ByteSize Parse(string s) { throw null; }

        public ByteSize Subtract(ByteSize bs) { throw null; }

        public override string ToString() { throw null; }

        public string ToString(string format) { throw null; }

        public static bool TryParse(string s, out ByteSize result) { throw null; }
    }
}

namespace Humanizer.Configuration
{
    public static partial class Configurator
    {
        public static LocaliserRegistry<Localisation.CollectionFormatters.ICollectionFormatter> CollectionFormatters { get { throw null; } }

        public static DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy DateTimeHumanizeStrategy { get { throw null; } set { } }

        public static DateTimeHumanizeStrategy.IDateTimeOffsetHumanizeStrategy DateTimeOffsetHumanizeStrategy { get { throw null; } set { } }

        public static LocaliserRegistry<Localisation.DateToOrdinalWords.IDateToOrdinalWordConverter> DateToOrdinalWordsConverters { get { throw null; } }

        public static System.Func<System.Reflection.PropertyInfo, bool> EnumDescriptionPropertyLocator { get { throw null; } set { } }

        public static LocaliserRegistry<Localisation.Formatters.IFormatter> Formatters { get { throw null; } }

        public static LocaliserRegistry<Localisation.NumberToWords.INumberToWordsConverter> NumberToWordsConverters { get { throw null; } }

        public static LocaliserRegistry<Localisation.Ordinalizers.IOrdinalizer> Ordinalizers { get { throw null; } }
    }

    public partial class LocaliserRegistry<TLocaliser>
        where TLocaliser : class
    {
        public LocaliserRegistry(TLocaliser defaultLocaliser) { }

        public LocaliserRegistry(System.Func<System.Globalization.CultureInfo, TLocaliser> defaultLocaliser) { }

        public void Register(string localeCode, TLocaliser localiser) { }

        public void Register(string localeCode, System.Func<System.Globalization.CultureInfo, TLocaliser> localiser) { }

        public TLocaliser ResolveForCulture(System.Globalization.CultureInfo culture) { throw null; }

        public TLocaliser ResolveForUiCulture() { throw null; }
    }
}

namespace Humanizer.DateTimeHumanizeStrategy
{
    public partial class DefaultDateTimeHumanizeStrategy : IDateTimeHumanizeStrategy
    {
        public string Humanize(System.DateTime input, System.DateTime comparisonBase, System.Globalization.CultureInfo culture) { throw null; }
    }

    public partial class DefaultDateTimeOffsetHumanizeStrategy : IDateTimeOffsetHumanizeStrategy
    {
        public string Humanize(System.DateTimeOffset input, System.DateTimeOffset comparisonBase, System.Globalization.CultureInfo culture) { throw null; }
    }

    public partial interface IDateTimeHumanizeStrategy
    {
        string Humanize(System.DateTime input, System.DateTime comparisonBase, System.Globalization.CultureInfo culture);
    }

    public partial interface IDateTimeOffsetHumanizeStrategy
    {
        string Humanize(System.DateTimeOffset input, System.DateTimeOffset comparisonBase, System.Globalization.CultureInfo culture);
    }

    public partial class PrecisionDateTimeHumanizeStrategy : IDateTimeHumanizeStrategy
    {
        public PrecisionDateTimeHumanizeStrategy(double precision = 0.75) { }

        public string Humanize(System.DateTime input, System.DateTime comparisonBase, System.Globalization.CultureInfo culture) { throw null; }
    }

    public partial class PrecisionDateTimeOffsetHumanizeStrategy : IDateTimeOffsetHumanizeStrategy
    {
        public PrecisionDateTimeOffsetHumanizeStrategy(double precision = 0.75) { }

        public string Humanize(System.DateTimeOffset input, System.DateTimeOffset comparisonBase, System.Globalization.CultureInfo culture) { throw null; }
    }
}

namespace Humanizer.Inflections
{
    public static partial class Vocabularies
    {
        public static Vocabulary Default { get { throw null; } }
    }

    public partial class Vocabulary
    {
        internal Vocabulary() { }

        public void AddIrregular(string singular, string plural, bool matchEnding = true) { }

        public void AddPlural(string rule, string replacement) { }

        public void AddSingular(string rule, string replacement) { }

        public void AddUncountable(string word) { }

        public string Pluralize(string word, bool inputIsKnownToBeSingular = true) { throw null; }

        public string Singularize(string word, bool inputIsKnownToBePlural = true) { throw null; }
    }
}

namespace Humanizer.Localisation
{
    public partial class ResourceKeys
    {
        public static partial class DateHumanize
        {
            public const string Never = "DateHumanize_Never";
            public const string Now = "DateHumanize_Now";
            public static string GetResourceKey(TimeUnit timeUnit, Tense timeUnitTense, int count = 1) { throw null; }
        }

        public static partial class TimeSpanHumanize
        {
            public static string GetResourceKey(TimeUnit unit, int count = 1) { throw null; }
        }
    }

    public static partial class Resources
    {
        public static string GetResource(string resourceKey, System.Globalization.CultureInfo culture = null) { throw null; }
    }

    public enum Tense
    {
        Future = 0,
        Past = 1
    }

    public enum TimeUnit
    {
        Millisecond = 0,
        Second = 1,
        Minute = 2,
        Hour = 3,
        Day = 4,
        Week = 5,
        Month = 6,
        Year = 7
    }
}

namespace Humanizer.Localisation.CollectionFormatters
{
    public partial interface ICollectionFormatter
    {
        string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, System.Func<T, string> objectFormatter, string separator);
        string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, System.Func<T, string> objectFormatter);
        string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, string separator);
        string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection);
    }
}

namespace Humanizer.Localisation.DateToOrdinalWords
{
    public partial interface IDateToOrdinalWordConverter
    {
        string Convert(System.DateTime date, GrammaticalCase grammaticalCase);
        string Convert(System.DateTime date);
    }
}

namespace Humanizer.Localisation.Formatters
{
    public partial class DefaultFormatter : IFormatter
    {
        public DefaultFormatter(string localeCode) { }

        public virtual string DateHumanize_Never() { throw null; }

        public virtual string DateHumanize_Now() { throw null; }

        public virtual string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit) { throw null; }

        protected virtual string Format(string resourceKey, int number) { throw null; }

        protected virtual string Format(string resourceKey) { throw null; }

        protected virtual string GetResourceKey(string resourceKey, int number) { throw null; }

        protected virtual string GetResourceKey(string resourceKey) { throw null; }

        public virtual string TimeSpanHumanize_Zero() { throw null; }

        public virtual string TimeSpanHumanize(TimeUnit timeUnit, int unit) { throw null; }
    }

    public partial interface IFormatter
    {
        string DateHumanize_Never();
        string DateHumanize_Now();
        string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit);
        string TimeSpanHumanize_Zero();
        string TimeSpanHumanize(TimeUnit timeUnit, int unit);
    }
}

namespace Humanizer.Localisation.NumberToWords
{
    public partial interface INumberToWordsConverter
    {
        string Convert(long number, GrammaticalGender gender);
        string Convert(long number);
        string ConvertToOrdinal(int number, GrammaticalGender gender);
        string ConvertToOrdinal(int number);
    }
}

namespace Humanizer.Localisation.Ordinalizers
{
    public partial interface IOrdinalizer
    {
        string Convert(int number, string numberString, GrammaticalGender gender);
        string Convert(int number, string numberString);
    }
}