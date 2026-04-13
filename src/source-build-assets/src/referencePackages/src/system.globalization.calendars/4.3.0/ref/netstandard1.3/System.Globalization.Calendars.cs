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
[assembly: System.Reflection.AssemblyTitle("System.Globalization.Calendars")]
[assembly: System.Reflection.AssemblyDescription("System.Globalization.Calendars")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Globalization.Calendars")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.0.1.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
namespace System.Globalization
{
    public partial class ChineseLunisolarCalendar : EastAsianLunisolarCalendar
    {
        public ChineseLunisolarCalendar() { }

        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int GetEra(DateTime time) { throw null; }
    }

    public abstract partial class EastAsianLunisolarCalendar : Calendar
    {
        internal EastAsianLunisolarCalendar() { }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public int GetCelestialStem(int sexagenaryYear) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public virtual int GetSexagenaryYear(DateTime time) { throw null; }

        public int GetTerrestrialBranch(int sexagenaryYear) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }

    public partial class GregorianCalendar : Calendar
    {
        public GregorianCalendar() { }

        public GregorianCalendar(GregorianCalendarTypes type) { }

        public virtual GregorianCalendarTypes CalendarType { get { throw null; } set { } }

        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetEra(DateTime time) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }

    public enum GregorianCalendarTypes
    {
        Localized = 1,
        USEnglish = 2,
        MiddleEastFrench = 9,
        Arabic = 10,
        TransliteratedEnglish = 11,
        TransliteratedFrench = 12
    }

    public partial class HebrewCalendar : Calendar
    {
        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetEra(DateTime time) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }

    public partial class HijriCalendar : Calendar
    {
        public override int[] Eras { get { throw null; } }

        public int HijriAdjustment { get { throw null; } set { } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetEra(DateTime time) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }

    public partial class JapaneseCalendar : Calendar
    {
        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetEra(DateTime time) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }

    public partial class JapaneseLunisolarCalendar : EastAsianLunisolarCalendar
    {
        public JapaneseLunisolarCalendar() { }

        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int GetEra(DateTime time) { throw null; }
    }

    public partial class JulianCalendar : Calendar
    {
        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetEra(DateTime time) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }

    public partial class KoreanCalendar : Calendar
    {
        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetEra(DateTime time) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }

    public partial class KoreanLunisolarCalendar : EastAsianLunisolarCalendar
    {
        public KoreanLunisolarCalendar() { }

        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int GetEra(DateTime time) { throw null; }
    }

    public partial class PersianCalendar : Calendar
    {
        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetEra(DateTime time) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }

    public partial class TaiwanCalendar : Calendar
    {
        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetEra(DateTime time) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }

    public partial class TaiwanLunisolarCalendar : EastAsianLunisolarCalendar
    {
        public TaiwanLunisolarCalendar() { }

        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int GetEra(DateTime time) { throw null; }
    }

    public partial class ThaiBuddhistCalendar : Calendar
    {
        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetEra(DateTime time) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }

    public partial class UmAlQuraCalendar : Calendar
    {
        public override int[] Eras { get { throw null; } }

        public override DateTime MaxSupportedDateTime { get { throw null; } }

        public override DateTime MinSupportedDateTime { get { throw null; } }

        public override int TwoDigitYearMax { get { throw null; } set { } }

        public override DateTime AddMonths(DateTime time, int months) { throw null; }

        public override DateTime AddYears(DateTime time, int years) { throw null; }

        public override int GetDayOfMonth(DateTime time) { throw null; }

        public override DayOfWeek GetDayOfWeek(DateTime time) { throw null; }

        public override int GetDayOfYear(DateTime time) { throw null; }

        public override int GetDaysInMonth(int year, int month, int era) { throw null; }

        public override int GetDaysInYear(int year, int era) { throw null; }

        public override int GetEra(DateTime time) { throw null; }

        public override int GetLeapMonth(int year, int era) { throw null; }

        public override int GetMonth(DateTime time) { throw null; }

        public override int GetMonthsInYear(int year, int era) { throw null; }

        public override int GetYear(DateTime time) { throw null; }

        public override bool IsLeapDay(int year, int month, int day, int era) { throw null; }

        public override bool IsLeapMonth(int year, int month, int era) { throw null; }

        public override bool IsLeapYear(int year, int era) { throw null; }

        public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era) { throw null; }

        public override int ToFourDigitYear(int year) { throw null; }
    }
}