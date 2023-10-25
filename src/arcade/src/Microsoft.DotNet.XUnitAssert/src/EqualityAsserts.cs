#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8600
#pragma warning disable CS8604
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit
{
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class Assert
	{
		static Type typeofDictionary = typeof(Dictionary<,>);
		static Type typeofHashSet = typeof(HashSet<>);
		static Type typeofSet = typeof(ISet<>);

#if XUNIT_SPAN
		/// <summary>
		/// Verifies that two arrays of un-managed type T are equal, using Span&lt;T&gt;.SequenceEqual.
		/// This can be significantly faster than generic enumerables, when the collections are actually
		/// equal, because the system can optimize packed-memory comparisons for value type arrays.
		/// </summary>
		/// <typeparam name="T">The type of items whose arrays are to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <remarks>
		/// If Span&lt;T&gt;.SequenceEqual fails, a call to Assert.Equal(object, object) is made,
		/// to provide a more meaningful error message.
		/// </remarks>
		public static void Equal<T>(
#if XUNIT_NULLABLE
			[AllowNull] T[] expected,
			[AllowNull] T[] actual)
				where T : unmanaged, IEquatable<T>
#else
			T[] expected,
			T[] actual)
				where T : IEquatable<T>
#endif
		{
			if (expected == null && actual == null)
				return;

			if (expected == null || actual == null || !expected.AsSpan().SequenceEqual(actual))
				// Call into Equal<object> (even though we'll re-enumerate) so we get proper formatting
				// of the sequence, including the "first mismatch" pointer
				Equal<object>(expected, actual);
		}
#endif

		/// <summary>
		/// Verifies that two objects are equal, using a default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <exception cref="EqualException">Thrown when the objects are not equal</exception>
		public static void Equal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(
#if XUNIT_NULLABLE
			[AllowNull] T expected,
			[AllowNull] T actual) =>
#else
			T expected,
			T actual) =>
#endif
				Equal(expected, actual, GetEqualityComparer<T>());

		/// <summary>
		/// Verifies that two objects are equal, using a custom comparer function.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The comparer used to compare the two objects</param>
		public static void Equal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(
#if XUNIT_NULLABLE
			[AllowNull] T expected,
			[AllowNull] T actual,
#else
			T expected,
			T actual,
#endif
			Func<T, T, bool> comparer) =>
				Equal(expected, actual, AssertEqualityComparer<T>.FromComparer(comparer));

		/// <summary>
		/// Verifies that two objects are equal, using a custom equatable comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The comparer used to compare the two objects</param>
		/// <exception cref="EqualException">Thrown when the objects are not equal</exception>
		public static void Equal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(
#if XUNIT_NULLABLE
			[AllowNull] T expected,
			[AllowNull] T actual,
#else
			T expected,
			T actual,
#endif
			IEqualityComparer<T> comparer)
		{
			GuardArgumentNotNull(nameof(comparer), comparer);

			if (expected == null && actual == null)
				return;

			var expectedTracker = expected.AsNonStringTracker();
			var actualTracker = actual.AsNonStringTracker();

			try
			{
				var haveCollections =
					(expectedTracker != null && actualTracker != null) ||
					(expectedTracker != null && actual == null) ||
					(expected == null && actualTracker != null);

				if (!haveCollections)
				{
					if (!comparer.Equals(expected, actual))
						throw EqualException.ForMismatchedValues(expected, actual);
				}
				else
				{
					int? mismatchedIndex = null;

					// If we have "known" comparers, we can ignore them and instead do our own thing, since we know
					// we want to be able to consume the tracker, and that's not type compatible.
					var itemComparer = default(IEqualityComparer);

					var aec = comparer as AssertEqualityComparer<T>;
					if (aec != null)
						itemComparer = aec.InnerComparer;
					else if (comparer == EqualityComparer<T>.Default)
						itemComparer = EqualityComparer<object>.Default;

					string formattedExpected;
					string formattedActual;
					int? expectedPointer = null;
					int? actualPointer = null;
#if XUNIT_NULLABLE
					string? expectedItemType = null;
					string? actualItemType = null;
#else
					string expectedItemType = null;
					string actualItemType = null;
#endif

					if (itemComparer != null)
					{
						if (CollectionTracker.AreCollectionsEqual(expectedTracker, actualTracker, itemComparer, itemComparer == AssertEqualityComparer<T>.DefaultInnerComparer, out mismatchedIndex))
							return;

						var expectedStartIdx = -1;
						var expectedEndIdx = -1;
						expectedTracker?.GetMismatchExtents(mismatchedIndex, out expectedStartIdx, out expectedEndIdx);

						var actualStartIdx = -1;
						var actualEndIdx = -1;
						actualTracker?.GetMismatchExtents(mismatchedIndex, out actualStartIdx, out actualEndIdx);

						// If either located index is past the end of the collection, then we want to try to shift
						// the too-short collection start point forward so we can align the equal values for
						// a more readable and obvious output. See CollectionAssertTests+Equals+Arrays.Truncation
						// for overrun examples.
						if (mismatchedIndex.HasValue)
						{
							if (expectedStartIdx > -1 && expectedEndIdx < mismatchedIndex.Value)
								expectedStartIdx = actualStartIdx;
							else if (actualStartIdx > -1 && actualEndIdx < mismatchedIndex.Value)
								actualStartIdx = expectedStartIdx;
						}

						expectedPointer = null;
						formattedExpected = expectedTracker?.FormatIndexedMismatch(expectedStartIdx, expectedEndIdx, mismatchedIndex, out expectedPointer) ?? ArgumentFormatter.Format(expected);
						expectedItemType = expectedTracker?.TypeAt(mismatchedIndex);

						actualPointer = null;
						formattedActual = actualTracker?.FormatIndexedMismatch(actualStartIdx, actualEndIdx, mismatchedIndex, out actualPointer) ?? ArgumentFormatter.Format(actual);
						actualItemType = actualTracker?.TypeAt(mismatchedIndex);
					}
					else
					{
						if (comparer.Equals(expected, actual))
							return;

						formattedExpected = ArgumentFormatter.Format(expected);
						formattedActual = ArgumentFormatter.Format(actual);
					}

#if XUNIT_NULLABLE
					string? collectionDisplay = null;
#else
					string collectionDisplay = null;
#endif

					var expectedType = expected?.GetType();
					var expectedTypeDefinition = SafeGetGenericTypeDefinition(expectedType);
					var expectedInterfaceTypeDefinitions = expectedType?.GetTypeInfo().ImplementedInterfaces.Where(i => i.GetTypeInfo().IsGenericType).Select(i => i.GetGenericTypeDefinition());

					var actualType = actual?.GetType();
					var actualTypeDefinition = SafeGetGenericTypeDefinition(actualType);
					var actualInterfaceTypeDefinitions = actualType?.GetTypeInfo().ImplementedInterfaces.Where(i => i.GetTypeInfo().IsGenericType).Select(i => i.GetGenericTypeDefinition());

					if (expectedTypeDefinition == typeofDictionary && actualTypeDefinition == typeofDictionary)
						collectionDisplay = "Dictionaries";
					else if (expectedTypeDefinition == typeofHashSet && actualTypeDefinition == typeofHashSet)
						collectionDisplay = "HashSets";
					else if (expectedInterfaceTypeDefinitions != null && actualInterfaceTypeDefinitions != null && expectedInterfaceTypeDefinitions.Contains(typeofSet) && actualInterfaceTypeDefinitions.Contains(typeofSet))
						collectionDisplay = "Sets";

					if (expectedType != actualType)
					{
						var expectedTypeName = expectedType == null ? "" : ArgumentFormatter.FormatTypeName(expectedType) + " ";
						var actualTypeName = actualType == null ? "" : ArgumentFormatter.FormatTypeName(actualType) + " ";

						var typeNameIndent = Math.Max(expectedTypeName.Length, actualTypeName.Length);

						formattedExpected = expectedTypeName.PadRight(typeNameIndent) + formattedExpected;
						formattedActual = actualTypeName.PadRight(typeNameIndent) + formattedActual;

						if (expectedPointer != null)
							expectedPointer += typeNameIndent;
						if (actualPointer != null)
							actualPointer += typeNameIndent;
					}

					throw EqualException.ForMismatchedCollections(mismatchedIndex, formattedExpected, expectedPointer, expectedItemType, formattedActual, actualPointer, actualItemType, collectionDisplay);
				}
			}
			finally
			{
				expectedTracker?.Dispose();
				actualTracker?.Dispose();
			}
		}

		/// <summary>
		/// Verifies that two <see cref="double"/> values are equal, within the number of decimal
		/// places given by <paramref name="precision"/>. The values are rounded before comparison.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The number of decimal places (valid values: 0-15)</param>
		public static void Equal(
			double expected,
			double actual,
			int precision)
		{
			var expectedRounded = Math.Round(expected, precision);
			var actualRounded = Math.Round(actual, precision);

			if (!object.Equals(expectedRounded, actualRounded))
				throw EqualException.ForMismatchedValues(
					$"{expectedRounded:G17} (rounded from {expected:G17})",
					$"{actualRounded:G17} (rounded from {actual:G17})",
					$"Values are not within {precision} decimal place{(precision == 1 ? "" : "s")}"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="double"/> values are equal, within the number of decimal
		/// places given by <paramref name="precision"/>. The values are rounded before comparison.
		/// The rounding method to use is given by <paramref name="rounding" />
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The number of decimal places (valid values: 0-15)</param>
		/// <param name="rounding">Rounding method to use to process a number that is midway between two numbers</param>
		public static void Equal(
			double expected,
			double actual,
			int precision,
			MidpointRounding rounding)
		{
			var expectedRounded = Math.Round(expected, precision, rounding);
			var actualRounded = Math.Round(actual, precision, rounding);

			if (!object.Equals(expectedRounded, actualRounded))
				throw EqualException.ForMismatchedValues(
					$"{expectedRounded:G17} (rounded from {expected:G17})",
					$"{actualRounded:G17} (rounded from {actual:G17})",
					$"Values are not within {precision} decimal place{(precision == 1 ? "" : "s")}"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="double"/> values are equal, within the tolerance given by
		/// <paramref name="tolerance"/> (positive or negative).
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="tolerance">The allowed difference between values</param>
		public static void Equal(
			double expected,
			double actual,
			double tolerance)
		{
			if (double.IsNaN(tolerance) || double.IsNegativeInfinity(tolerance) || tolerance < 0.0)
				throw new ArgumentException("Tolerance must be greater than or equal to zero", nameof(tolerance));

			if (!(object.Equals(expected, actual) || Math.Abs(expected - actual) <= tolerance))
				throw EqualException.ForMismatchedValues(
					expected.ToString("G17", CultureInfo.CurrentCulture),
					actual.ToString("G17", CultureInfo.CurrentCulture),
					$"Values are not within tolerance {tolerance:G17}"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="float"/> values are equal, within the number of decimal
		/// places given by <paramref name="precision"/>. The values are rounded before comparison.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The number of decimal places (valid values: 0-15)</param>
		public static void Equal(
			float expected,
			float actual,
			int precision)
		{
			var expectedRounded = Math.Round(expected, precision);
			var actualRounded = Math.Round(actual, precision);

			if (!object.Equals(expectedRounded, actualRounded))
				throw EqualException.ForMismatchedValues(
					$"{expectedRounded:G9} (rounded from {expected:G9})",
					$"{actualRounded:G9} (rounded from {actual:G9})",
					$"Values are not within {precision} decimal place{(precision == 1 ? "" : "s")}"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="float"/> values are equal, within the number of decimal
		/// places given by <paramref name="precision"/>. The values are rounded before comparison.
		/// The rounding method to use is given by <paramref name="rounding" />
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The number of decimal places (valid values: 0-15)</param>
		/// <param name="rounding">Rounding method to use to process a number that is midway between two numbers</param>
		public static void Equal(
			float expected,
			float actual,
			int precision,
			MidpointRounding rounding)
		{
			var expectedRounded = Math.Round(expected, precision, rounding);
			var actualRounded = Math.Round(actual, precision, rounding);

			if (!object.Equals(expectedRounded, actualRounded))
				throw EqualException.ForMismatchedValues(
					$"{expectedRounded:G9} (rounded from {expected:G9})",
					$"{actualRounded:G9} (rounded from {actual:G9})",
					$"Values are not within {precision} decimal place{(precision == 1 ? "" : "s")}"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="float"/> values are equal, within the tolerance given by
		/// <paramref name="tolerance"/> (positive or negative).
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="tolerance">The allowed difference between values</param>
		public static void Equal(
			float expected,
			float actual,
			float tolerance)
		{
			if (float.IsNaN(tolerance) || float.IsNegativeInfinity(tolerance) || tolerance < 0.0)
				throw new ArgumentException("Tolerance must be greater than or equal to zero", nameof(tolerance));

			if (!(object.Equals(expected, actual) || Math.Abs(expected - actual) <= tolerance))
				throw EqualException.ForMismatchedValues(
					expected.ToString("G9", CultureInfo.CurrentCulture),
					actual.ToString("G9", CultureInfo.CurrentCulture),
					$"Values are not within tolerance {tolerance:G9}"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="decimal"/> values are equal, within the number of decimal
		/// places given by <paramref name="precision"/>. The values are rounded before comparison.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The number of decimal places (valid values: 0-28)</param>
		public static void Equal(
			decimal expected,
			decimal actual,
			int precision)
		{
			var expectedRounded = Math.Round(expected, precision);
			var actualRounded = Math.Round(actual, precision);

			if (expectedRounded != actualRounded)
				throw EqualException.ForMismatchedValues($"{expectedRounded} (rounded from {expected})", $"{actualRounded} (rounded from {actual})");
		}

		/// <summary>
		/// Verifies that two <see cref="DateTime"/> values are equal.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		public static void Equal(
			DateTime expected,
			DateTime actual) =>
				Equal(expected, actual, TimeSpan.Zero);

		/// <summary>
		/// Verifies that two <see cref="DateTime"/> values are equal, within the precision
		/// given by <paramref name="precision"/>.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The allowed difference in time where the two dates are considered equal</param>
		public static void Equal(
			DateTime expected,
			DateTime actual,
			TimeSpan precision)
		{
			var difference = (expected - actual).Duration();

			if (difference > precision)
			{
				var actualValue =
					ArgumentFormatter.Format(actual) +
					(precision == TimeSpan.Zero ? "" : $" (difference {difference} is larger than {precision})");

				throw EqualException.ForMismatchedValues(expected, actualValue);
			}
		}

		/// <summary>
		/// Verifies that two <see cref="DateTimeOffset"/> values are equal.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		public static void Equal(
			DateTimeOffset expected,
			DateTimeOffset actual) =>
				Equal(expected, actual, TimeSpan.Zero);

		/// <summary>
		/// Verifies that two <see cref="DateTimeOffset"/> values are equal, within the precision
		/// given by <paramref name="precision"/>.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The allowed difference in time where the two dates are considered equal</param>
		public static void Equal(
			DateTimeOffset expected,
			DateTimeOffset actual,
			TimeSpan precision)
		{
			var difference = (expected - actual).Duration();

			if (difference > precision)
			{
				var actualValue =
					ArgumentFormatter.Format(actual) +
					(precision == TimeSpan.Zero ? "" : $" (difference {difference} is larger than {precision})");

				throw EqualException.ForMismatchedValues(expected, actualValue);
			}
		}

#if XUNIT_SPAN
		/// <summary>
		/// Verifies that two arrays of un-managed type T are not equal, using Span&lt;T&gt;.SequenceEqual.
		/// </summary>
		/// <typeparam name="T">The type of items whose arrays are to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		public static void NotEqual<T>(
#if XUNIT_NULLABLE
			[AllowNull] T[] expected,
			[AllowNull] T[] actual)
				where T : unmanaged, IEquatable<T>
#else
			T[] expected,
			T[] actual)
				where T : IEquatable<T>
#endif
		{
			// Call into NotEqual<object> so we get proper formatting of the sequence
			if (expected == null && actual == null)
				NotEqual<object>(expected, actual);
			if (expected == null || actual == null)
				return;
			if (expected.AsSpan().SequenceEqual(actual))
				NotEqual<object>(expected, actual);
		}
#endif

		/// <summary>
		/// Verifies that two objects are not equal, using a default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		/// <exception cref="NotEqualException">Thrown when the objects are equal</exception>
		public static void NotEqual<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(
#if XUNIT_NULLABLE
			[AllowNull] T expected,
			[AllowNull] T actual) =>
#else
			T expected,
			T actual) =>
#endif
				NotEqual(expected, actual, GetEqualityComparer<T>());

		/// <summary>
		/// Verifies that two objects are not equal, using a custom equality comparer function.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		/// <param name="comparer">The comparer used to examine the objects</param>
		public static void NotEqual<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(
#if XUNIT_NULLABLE
			[AllowNull] T expected,
			[AllowNull] T actual,
#else
			T expected,
			T actual,
#endif
			Func<T, T, bool> comparer) =>
				NotEqual(expected, actual, AssertEqualityComparer<T>.FromComparer(comparer));

		/// <summary>
		/// Verifies that two objects are not equal, using a custom equality comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		/// <param name="comparer">The comparer used to examine the objects</param>
		public static void NotEqual<
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(
#if XUNIT_NULLABLE
			[AllowNull] T expected,
			[AllowNull] T actual,
#else
			T expected,
			T actual,
#endif
			IEqualityComparer<T> comparer)
		{
			GuardArgumentNotNull(nameof(comparer), comparer);

			var expectedTracker = expected.AsNonStringTracker();
			var actualTracker = actual.AsNonStringTracker();

			try
			{
				var haveCollections =
					(expectedTracker != null && actualTracker != null) ||
					(expectedTracker != null && actual == null) ||
					(expected == null && actualTracker != null);

				if (!haveCollections)
				{
					if (comparer.Equals(expected, actual))
					{
						var formattedExpected = ArgumentFormatter.Format(expected);
						var formattedActual = ArgumentFormatter.Format(actual);

						var expectedIsString = expected is string;
						var actualIsString = actual is string;
						var isStrings =
							(expectedIsString && actual == null) ||
							(actualIsString && expected == null) ||
							(expectedIsString && actualIsString);

						if (isStrings)
							throw NotEqualException.ForEqualCollections(formattedExpected, formattedActual, "Strings");
						else
							throw NotEqualException.ForEqualValues(formattedExpected, formattedActual);
					}
				}
				else
				{
					// If we have "known" comparers, we can ignore them and instead do our own thing, since we know
					// we want to be able to consume the tracker, and that's not type compatible.
					var itemComparer = default(IEqualityComparer);

					var aec = comparer as AssertEqualityComparer<T>;
					if (aec != null)
						itemComparer = aec.InnerComparer;
					else if (comparer == EqualityComparer<T>.Default)
						itemComparer = EqualityComparer<object>.Default;

					string formattedExpected;
					string formattedActual;

					if (itemComparer != null)
					{
						int? mismatchedIndex;
						if (!CollectionTracker.AreCollectionsEqual(expectedTracker, actualTracker, itemComparer, itemComparer == AssertEqualityComparer<T>.DefaultInnerComparer, out mismatchedIndex))
							return;

						formattedExpected = expectedTracker?.FormatStart() ?? "null";
						formattedActual = actualTracker?.FormatStart() ?? "null";
					}
					else
					{
						if (!comparer.Equals(expected, actual))
							return;

						formattedExpected = ArgumentFormatter.Format(expected);
						formattedActual = ArgumentFormatter.Format(actual);
					}

#if XUNIT_NULLABLE
					string? collectionDisplay = null;
#else
					string collectionDisplay = null;
#endif

					var expectedType = expected?.GetType();
					var expectedTypeDefinition = SafeGetGenericTypeDefinition(expectedType);
					var expectedInterfaceTypeDefinitions = expectedType?.GetTypeInfo().ImplementedInterfaces.Where(i => i.GetTypeInfo().IsGenericType).Select(i => i.GetGenericTypeDefinition());

					var actualType = actual?.GetType();
					var actualTypeDefinition = SafeGetGenericTypeDefinition(actualType);
					var actualInterfaceTypeDefinitions = actualType?.GetTypeInfo().ImplementedInterfaces.Where(i => i.GetTypeInfo().IsGenericType).Select(i => i.GetGenericTypeDefinition());

					if (expectedTypeDefinition == typeofDictionary && actualTypeDefinition == typeofDictionary)
						collectionDisplay = "Dictionaries";
					else if (expectedTypeDefinition == typeofHashSet && actualTypeDefinition == typeofHashSet)
						collectionDisplay = "HashSets";
					else if (expectedInterfaceTypeDefinitions != null && actualInterfaceTypeDefinitions != null && expectedInterfaceTypeDefinitions.Contains(typeofSet) && actualInterfaceTypeDefinitions.Contains(typeofSet))
						collectionDisplay = "Sets";

					if (expectedType != actualType)
					{
						var expectedTypeName = expectedType == null ? "" : ArgumentFormatter.FormatTypeName(expectedType) + " ";
						var actualTypeName = actualType == null ? "" : ArgumentFormatter.FormatTypeName(actualType) + " ";

						var typeNameIndent = Math.Max(expectedTypeName.Length, actualTypeName.Length);

						formattedExpected = expectedTypeName.PadRight(typeNameIndent) + formattedExpected;
						formattedActual = actualTypeName.PadRight(typeNameIndent) + formattedActual;
					}

					throw NotEqualException.ForEqualCollections(formattedExpected, formattedActual, collectionDisplay);
				}
			}
			finally
			{
				expectedTracker?.Dispose();
				actualTracker?.Dispose();
			}
		}

		/// <summary>
		/// Verifies that two <see cref="double"/> values are not equal, within the number of decimal
		/// places given by <paramref name="precision"/>.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The number of decimal places (valid values: 0-15)</param>
		public static void NotEqual(
			double expected,
			double actual,
			int precision)
		{
			var expectedRounded = Math.Round(expected, precision);
			var actualRounded = Math.Round(actual, precision);

			if (object.Equals(expectedRounded, actualRounded))
				throw NotEqualException.ForEqualValues(
					$"{expectedRounded:G17} (rounded from {expected:G17})",
					$"{actualRounded:G17} (rounded from {actual:G17})",
					$"Values are within {precision} decimal places"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="double"/> values are not equal, within the number of decimal
		/// places given by <paramref name="precision"/>. The values are rounded before comparison.
		/// The rounding method to use is given by <paramref name="rounding" />
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The number of decimal places (valid values: 0-15)</param>
		/// <param name="rounding">Rounding method to use to process a number that is midway between two numbers</param>
		public static void NotEqual(
			double expected,
			double actual,
			int precision,
			MidpointRounding rounding)
		{
			var expectedRounded = Math.Round(expected, precision, rounding);
			var actualRounded = Math.Round(actual, precision, rounding);

			if (object.Equals(expectedRounded, actualRounded))
				throw NotEqualException.ForEqualValues(
					$"{expectedRounded:G17} (rounded from {expected:G17})",
					$"{actualRounded:G17} (rounded from {actual:G17})",
					$"Values are within {precision} decimal places"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="double"/> values are not equal, within the tolerance given by
		/// <paramref name="tolerance"/> (positive or negative).
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="tolerance">The allowed difference between values</param>
		public static void NotEqual(
			double expected,
			double actual,
			double tolerance)
		{
			if (double.IsNaN(tolerance) || double.IsNegativeInfinity(tolerance) || tolerance < 0.0)
				throw new ArgumentException("Tolerance must be greater than or equal to zero", nameof(tolerance));

			if (object.Equals(expected, actual) || Math.Abs(expected - actual) <= tolerance)
				throw NotEqualException.ForEqualValues(
					expected.ToString("G17", CultureInfo.CurrentCulture),
					actual.ToString("G17", CultureInfo.CurrentCulture),
					$"Values are within tolerance {tolerance:G17}"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="float"/> values are not equal, within the number of decimal
		/// places given by <paramref name="precision"/>.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The number of decimal places (valid values: 0-15)</param>
		public static void NotEqual(
			float expected,
			float actual,
			int precision)
		{
			var expectedRounded = Math.Round(expected, precision);
			var actualRounded = Math.Round(actual, precision);

			if (object.Equals(expectedRounded, actualRounded))
				throw NotEqualException.ForEqualValues(
					$"{expectedRounded:G9} (rounded from {expected:G9})",
					$"{actualRounded:G9} (rounded from {actual:G9})",
					$"Values are within {precision} decimal places"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="float"/> values are not equal, within the number of decimal
		/// places given by <paramref name="precision"/>. The values are rounded before comparison.
		/// The rounding method to use is given by <paramref name="rounding" />
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The number of decimal places (valid values: 0-15)</param>
		/// <param name="rounding">Rounding method to use to process a number that is midway between two numbers</param>
		public static void NotEqual(
			float expected,
			float actual,
			int precision,
			MidpointRounding rounding)
		{
			var expectedRounded = Math.Round(expected, precision, rounding);
			var actualRounded = Math.Round(actual, precision, rounding);

			if (object.Equals(expectedRounded, actualRounded))
				throw NotEqualException.ForEqualValues(
					$"{expectedRounded:G9} (rounded from {expected:G9})",
					$"{actualRounded:G9} (rounded from {actual:G9})",
					$"Values are within {precision} decimal places"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="float"/> values are not equal, within the tolerance given by
		/// <paramref name="tolerance"/> (positive or negative).
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="tolerance">The allowed difference between values</param>
		public static void NotEqual(
			float expected,
			float actual,
			float tolerance)
		{
			if (float.IsNaN(tolerance) || float.IsNegativeInfinity(tolerance) || tolerance < 0.0)
				throw new ArgumentException("Tolerance must be greater than or equal to zero", nameof(tolerance));

			if (object.Equals(expected, actual) || Math.Abs(expected - actual) <= tolerance)
				throw NotEqualException.ForEqualValues(
					expected.ToString("G9", CultureInfo.CurrentCulture),
					actual.ToString("G9", CultureInfo.CurrentCulture),
					$"Values are within tolerance {tolerance:G9}"
				);
		}

		/// <summary>
		/// Verifies that two <see cref="decimal"/> values are not equal, within the number of decimal
		/// places given by <paramref name="precision"/>.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="precision">The number of decimal places (valid values: 0-28)</param>
		public static void NotEqual(
			decimal expected,
			decimal actual,
			int precision)
		{
			var expectedRounded = Math.Round(expected, precision);
			var actualRounded = Math.Round(actual, precision);

			if (expectedRounded == actualRounded)
				throw NotEqualException.ForEqualValues(
					$"{expectedRounded} (rounded from {expected})",
					$"{actualRounded} (rounded from {actual})"
				);
		}

		/// <summary>
		/// Verifies that two objects are strictly not equal, using the type's default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		public static void NotStrictEqual<T>(
#if XUNIT_NULLABLE
			[AllowNull] T expected,
			[AllowNull] T actual)
#else
			T expected,
			T actual)
#endif
		{
			if (!EqualityComparer<T>.Default.Equals(expected, actual))
				return;

			throw NotStrictEqualException.ForEqualValues(
				ArgumentFormatter.Format(expected),
				ArgumentFormatter.Format(actual)
			);
		}

		/// <summary>
		/// Verifies that two objects are strictly equal, using the type's default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		public static void StrictEqual<T>(
#if XUNIT_NULLABLE
			[AllowNull] T expected,
			[AllowNull] T actual)
#else
			T expected,
			T actual)
#endif
		{
			if (EqualityComparer<T>.Default.Equals(expected, actual))
				return;

			throw StrictEqualException.ForEqualValues(
				ArgumentFormatter.Format(expected),
				ArgumentFormatter.Format(actual)
			);
		}
	}
}
