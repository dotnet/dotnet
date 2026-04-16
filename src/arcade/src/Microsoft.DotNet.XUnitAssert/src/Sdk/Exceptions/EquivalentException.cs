#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable CA1200 // Avoid using cref tags with a prefix
#pragma warning disable IDE0090 // Use 'new(...)'

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8625
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Equivalent fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class EquivalentException : XunitException
	{
		EquivalentException(
			string message,
#if XUNIT_NULLABLE
			Exception? innerException = null) :
#else
			Exception innerException = null) :
#endif
				base(message, innerException)
		{ }

		static string FormatMemberNameList(
			IEnumerable<string> memberNames,
			string prefix) =>
				string.Format(
					CultureInfo.CurrentCulture,
					"[{0}]",
					string.Join(", ", memberNames.Select(k => string.Format(CultureInfo.CurrentCulture, "\"{0}{1}\"", prefix, k)))
				);

		/// <summary>
		/// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
		/// a circular reference was discovered.
		/// </summary>
		/// <param name="memberName">The name of the member that caused the circular reference</param>
		public static EquivalentException ForCircularReference(string memberName) =>
			new EquivalentException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.Equivalent() Failure: Circular reference found in '{0}'",
					Assert.GuardArgumentNotNull(nameof(memberName), memberName)
				)
			);

		/// <summary>
		/// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
		/// that the maximum comparison depth was exceeded.
		/// </summary>
		/// <param name="depth">The depth reached</param>
		/// <param name="memberName">The member access which caused the failure</param>
		public static EquivalentException ForExceededDepth(
			int depth,
			string memberName) =>
				new EquivalentException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Equivalent() Failure: Exceeded the maximum depth {0} with '{1}'; check for infinite recursion or circular references",
						depth,
						Assert.GuardArgumentNotNull(nameof(memberName), memberName)
					)
				);

		/// <summary>
		/// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
		/// that the fault comes from an individual value mismatch one of the members.
		/// </summary>
		/// <param name="expected">The expected member value</param>
		/// <param name="actual">The actual member value</param>
		/// <param name="keyName">The name of the key with mismatched values</param>
		public static EquivalentException ForGroupingWithMismatchedValues(
#if XUNIT_NULLABLE
			object? expected,
			object? actual,
#else
			object expected,
			object actual,
#endif
			string keyName) =>
				new EquivalentException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Equivalent() Failure: Grouping key [{0}] has mismatched values{1}Expected: {2}{3}Actual:   {4}",
						keyName,
						Environment.NewLine,
						ArgumentFormatter.Format(expected),
						Environment.NewLine,
						ArgumentFormatter.Format(actual)
					)
				);

		/// <summary>
		/// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
		/// that the list of available members does not match.
		/// </summary>
		/// <param name="expectedMemberNames">The expected member names</param>
		/// <param name="actualMemberNames">The actual member names</param>
		/// <param name="prefix">The prefix to be applied to the member names (may be an empty string for a
		/// top-level object, or a name in "member." format used as a prefix to show the member name list)</param>
		public static EquivalentException ForMemberListMismatch(
			IEnumerable<string> expectedMemberNames,
			IEnumerable<string> actualMemberNames,
			string prefix) =>
				new EquivalentException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Equivalent() Failure: Mismatched member list{0}Expected: {1}{2}Actual:   {3}",
						Environment.NewLine,
						FormatMemberNameList(Assert.GuardArgumentNotNull(nameof(expectedMemberNames), expectedMemberNames), prefix),
						Environment.NewLine,
						FormatMemberNameList(Assert.GuardArgumentNotNull(nameof(actualMemberNames), actualMemberNames), prefix)
					)
				);

		/// <summary>
		/// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
		/// that the fault comes from an individual value mismatch one of the members.
		/// </summary>
		/// <param name="expected">The expected member value</param>
		/// <param name="actual">The actual member value</param>
		/// <param name="memberName">The name of the mismatched member (may be an empty string for a
		/// top-level object)</param>
		/// <param name="innerException">The inner exception that was thrown during value comparison,
		/// typically during a call to <see cref="IComparable.CompareTo(object)"/></param>
		public static EquivalentException ForMemberValueMismatch(
#if XUNIT_NULLABLE
			object? expected,
			object? actual,
#else
			object expected,
			object actual,
#endif
			string memberName,
#if XUNIT_NULLABLE
			Exception? innerException = null) =>
#else
			Exception innerException = null) =>
#endif
				new EquivalentException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Equivalent() Failure{0}{1}Expected: {2}{3}Actual:   {4}",
						Assert.GuardArgumentNotNull(nameof(memberName), memberName).Length == 0 ? string.Empty : string.Format(CultureInfo.CurrentCulture, ": Mismatched value on member '{0}'", memberName),
						Environment.NewLine,
						ArgumentFormatter.Format(expected),
						Environment.NewLine,
						ArgumentFormatter.Format(actual)
					),
					innerException
				);

		/// <summary>
		/// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
		/// a value was missing from the <paramref name="actual"/> collection.
		/// </summary>
		/// <param name="expected">The object that was expected to be found in <paramref name="actual"/> collection.</param>
		/// <param name="actual">The actual collection which was missing the object.</param>
		/// <param name="memberName">The name of the member that was being inspected (may be an empty
		/// string for a top-level collection)</param>
		public static EquivalentException ForMissingCollectionValue(
#if XUNIT_NULLABLE
			object? expected,
			IEnumerable<object?> actual,
#else
			object expected,
			IEnumerable<object> actual,
#endif
			string memberName) =>
				new EquivalentException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Equivalent() Failure: Collection value not found{0}{1}Expected: {2}{3}In:       {4}",
						Assert.GuardArgumentNotNull(nameof(memberName), memberName).Length == 0 ? string.Empty : string.Format(CultureInfo.CurrentCulture, " in member '{0}'", memberName),
						Environment.NewLine,
						ArgumentFormatter.Format(expected),
						Environment.NewLine,
						ArgumentFormatter.Format(actual)
					)
				);

		/// <summary>
		/// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
		/// that <paramref name="actual"/> contained one or more values that were not specified
		/// in <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">The values expected to be found in the <paramref name="actual"/>
		/// collection.</param>
		/// <param name="actual">The actual collection values.</param>
		/// <param name="actualLeftovers">The values from <paramref name="actual"/> that did not have
		/// matching <paramref name="expected"/> values</param>
		/// <param name="memberName">The name of the member that was being inspected (may be an empty
		/// string for a top-level collection)</param>
		public static EquivalentException ForExtraCollectionValue(
#if XUNIT_NULLABLE
			IEnumerable<object?> expected,
			IEnumerable<object?> actual,
			IEnumerable<object?> actualLeftovers,
#else
			IEnumerable<object> expected,
			IEnumerable<object> actual,
			IEnumerable<object> actualLeftovers,
#endif
			string memberName) =>
				new EquivalentException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Equivalent() Failure: Extra values found{0}{1}Expected: {2}{3}Actual:   {4} left over from {5}",
						Assert.GuardArgumentNotNull(nameof(memberName), memberName).Length == 0 ? string.Empty : string.Format(CultureInfo.CurrentCulture, " in member '{0}'", memberName),
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(expected), expected)),
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(actualLeftovers), actualLeftovers)),
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(actual), actual))
					)
				);

		/// <summary>
		/// Creates a new instance of <see cref="EquivalentException"/> which shows a message that indicates
		/// that <paramref name="expectedType"/> does not match <paramref name="actualType"/>. This is typically
		/// only used in special case comparison where it would be known that general comparison would fail
		/// for other reasons, like two objects derived from <see cref="T:System.IO.FileSystemInfo"/> with
		/// different concrete types.
		/// </summary>
		/// <param name="expectedType">The expected type</param>
		/// <param name="actualType">The actual type</param>
		/// <param name="memberName">The name of the member that was being inspected (may be an empty
		/// string for a top-level comparison)</param>
		public static EquivalentException ForMismatchedTypes(
			Type expectedType,
			Type actualType,
			string memberName) =>
				new EquivalentException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Equivalent() Failure: Types did not match{0}{1}Expected type: {2}{3}Actual type:   {4}",
						Assert.GuardArgumentNotNull(nameof(memberName), memberName).Length == 0 ? string.Empty : string.Format(CultureInfo.CurrentCulture, " in member '{0}'", memberName),
						Environment.NewLine,
						ArgumentFormatter.FormatTypeName(Assert.GuardArgumentNotNull(nameof(expectedType), expectedType), fullTypeName: true),
						Environment.NewLine,
						ArgumentFormatter.FormatTypeName(Assert.GuardArgumentNotNull(nameof(actualType), actualType), fullTypeName: true)
					)
				);
	}
}
