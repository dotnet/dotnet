#pragma warning disable CA1031 // Do not catch general exception types

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
using Xunit.Internal;
using Xunit.Sdk;

namespace Xunit
{
	partial class Assert
	{
		/// <summary>
		/// Verifies that two objects are equal, using a default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		public static void Equal<T>(
#if XUNIT_NULLABLE
			T? expected,
			T? actual) =>
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
		public static void Equal<T>(
#if XUNIT_NULLABLE
			T? expected,
			T? actual,
#else
			T expected,
			T actual,
#endif
			Func<T, T, bool> comparer) =>
				Equal(expected, actual, AssertEqualityComparer<T>.FromComparer(comparer));

		/// <summary>
		/// Verifies that two objects are equal, using a custom equality comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The comparer used to compare the two objects</param>
		public static void Equal<T>(
#if XUNIT_NULLABLE
			T? expected,
			T? actual,
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
			var exception = default(Exception);
			var mismatchedIndex = default(int?);

			try
			{
				var haveCollections =
					(expectedTracker != null && actualTracker != null) ||
					(expectedTracker != null && actual == null) ||
					(expected == null && actualTracker != null);

				if (!haveCollections)
				{
					try
					{
						if (comparer.Equals(expected, actual))
							return;
					}
					catch (Exception ex)
					{
						exception = ex;
					}

					throw EqualException.ForMismatchedValuesWithError(
						expected as string ?? ArgumentFormatter.Format(expected),
						actual as string ?? ArgumentFormatter.Format(actual),
						exception
					);
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
						AssertEqualityResult result;

						// Call AssertEqualityComparer.Equals because it checks for IEquatable<> before using CollectionTracker
						if (aec != null)
							result = aec.Equals(expected, expectedTracker, actual, actualTracker);
						else
							result = CollectionTracker.AreCollectionsEqual(expectedTracker, actualTracker, itemComparer, itemComparer == AssertEqualityComparer<T>.DefaultInnerComparer);

						if (result.Equal)
							return;

						if (result.InnerResult is AssertEqualityResult innerResult)
						{
							var innerExpectedString = innerResult.X as string;
							var innerExpectedMismatch = innerResult.MismatchIndexX;
							var innerActualString = innerResult.Y as string;
							var innerActualMismatch = innerResult.MismatchIndexY;

							if ((innerExpectedString != null || innerActualString != null) && innerExpectedMismatch.HasValue && innerActualMismatch.HasValue)
								throw EqualException.ForMismatchedStringsWithHeader(
									innerExpectedString,
									innerActualString,
									innerExpectedMismatch.Value,
									innerActualMismatch.Value,
									"Collections differ at index " + result.MismatchIndexX
								);
						}

						exception = result.Exception;
						mismatchedIndex = result.MismatchIndexX;

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
						try
						{
							if (comparer.Equals(expected, actual))
								return;
						}
						catch (Exception ex)
						{
							exception = ex;
						}

						formattedExpected = ArgumentFormatter.Format(expected);
						formattedActual = ArgumentFormatter.Format(actual);
					}

					var expectedType = expected?.GetType();
					var expectedTypeDefinition = SafeGetGenericTypeDefinition(expectedType);

					var actualType = actual?.GetType();
					var actualTypeDefinition = SafeGetGenericTypeDefinition(actualType);

					var collectionDisplay = GetCollectionDisplay(expectedType, expectedTypeDefinition, actualType, actualTypeDefinition);

					if (expectedType != actualType)
					{
						var expectedTypeName = expectedType == null ? "" : (AssertHelper.IsCompilerGenerated(expectedType) ? "<generated> " : ArgumentFormatter.FormatTypeName(expectedType) + " ");
						var actualTypeName = actualType == null ? "" : (AssertHelper.IsCompilerGenerated(actualType) ? "<generated> " : ArgumentFormatter.FormatTypeName(actualType) + " ");

						var typeNameIndent = Math.Max(expectedTypeName.Length, actualTypeName.Length);

						formattedExpected = expectedTypeName.PadRight(typeNameIndent) + formattedExpected;
						formattedActual = actualTypeName.PadRight(typeNameIndent) + formattedActual;

						if (expectedPointer != null)
							expectedPointer += typeNameIndent;
						if (actualPointer != null)
							actualPointer += typeNameIndent;
					}

					throw EqualException.ForMismatchedCollectionsWithError(mismatchedIndex, formattedExpected, expectedPointer, expectedItemType, formattedActual, actualPointer, actualItemType, exception, collectionDisplay);
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
					string.Format(CultureInfo.CurrentCulture, "{0:G17} (rounded from {1:G17})", expectedRounded, expected),
					string.Format(CultureInfo.CurrentCulture, "{0:G17} (rounded from {1:G17})", actualRounded, actual),
					string.Format(CultureInfo.CurrentCulture, "Values are not within {0} decimal place{1}", precision, precision == 1 ? "" : "s")
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
					string.Format(CultureInfo.CurrentCulture, "{0:G17} (rounded from {1:G17})", expectedRounded, expected),
					string.Format(CultureInfo.CurrentCulture, "{0:G17} (rounded from {1:G17})", actualRounded, actual),
					string.Format(CultureInfo.CurrentCulture, "Values are not within {0} decimal place{1}", precision, precision == 1 ? "" : "s")
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
					string.Format(CultureInfo.CurrentCulture, "Values are not within tolerance {0:G17}", tolerance)
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
					string.Format(CultureInfo.CurrentCulture, "{0:G9} (rounded from {1:G9})", expectedRounded, expected),
					string.Format(CultureInfo.CurrentCulture, "{0:G9} (rounded from {1:G9})", actualRounded, actual),
					string.Format(CultureInfo.CurrentCulture, "Values are not within {0} decimal place{1}", precision, precision == 1 ? "" : "s")
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
					string.Format(CultureInfo.CurrentCulture, "{0:G9} (rounded from {1:G9})", expectedRounded, expected),
					string.Format(CultureInfo.CurrentCulture, "{0:G9} (rounded from {1:G9})", actualRounded, actual),
					string.Format(CultureInfo.CurrentCulture, "Values are not within {0} decimal place{1}", precision, precision == 1 ? "" : "s")
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
					string.Format(CultureInfo.CurrentCulture, "Values are not within tolerance {0:G9}", tolerance)
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
				throw EqualException.ForMismatchedValues(
					string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", expectedRounded, expected),
					string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", actualRounded, actual)
				);
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
					(precision == TimeSpan.Zero ? "" : string.Format(CultureInfo.CurrentCulture, " (difference {0} is larger than {1})", difference, precision));

				throw EqualException.ForMismatchedValues(ArgumentFormatter.Format(expected), actualValue);
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
					(precision == TimeSpan.Zero ? "" : string.Format(CultureInfo.CurrentCulture, " (difference {0} is larger than {1})", difference, precision));

				throw EqualException.ForMismatchedValues(ArgumentFormatter.Format(expected), actualValue);
			}
		}

		/// <summary>
		/// Verifies that two objects are not equal, using a default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		public static void NotEqual<T>(
#if XUNIT_NULLABLE
			T? expected,
			T? actual) =>
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
		public static void NotEqual<T>(
#if XUNIT_NULLABLE
			T? expected,
			T? actual,
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
		public static void NotEqual<T>(
#if XUNIT_NULLABLE
			T? expected,
			T? actual,
#else
			T expected,
			T actual,
#endif
			IEqualityComparer<T> comparer)
		{
			GuardArgumentNotNull(nameof(comparer), comparer);

			var expectedTracker = expected.AsNonStringTracker();
			var actualTracker = actual.AsNonStringTracker();
			var exception = default(Exception);
			var mismatchedIndex = default(int?);

			try
			{
				var haveCollections =
					(expectedTracker != null && actualTracker != null) ||
					(expectedTracker != null && actual == null) ||
					(expected == null && actualTracker != null);

				if (!haveCollections)
				{
					try
					{
						if (!comparer.Equals(expected, actual))
							return;
					}
					catch (Exception ex)
					{
						exception = ex;
					}

					var formattedExpected = ArgumentFormatter.Format(expected);
					var formattedActual = ArgumentFormatter.Format(actual);

					var expectedIsString = expected is string;
					var actualIsString = actual is string;
					var isStrings =
						(expectedIsString && actual == null) ||
						(actualIsString && expected == null) ||
						(expectedIsString && actualIsString);

					if (isStrings)
						throw NotEqualException.ForEqualCollectionsWithError(null, formattedExpected, null, formattedActual, null, exception, "Strings");
					else
						throw NotEqualException.ForEqualValuesWithError(formattedExpected, formattedActual, exception);
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
					int? expectedPointer = null;
					int? actualPointer = null;

					if (itemComparer != null)
					{
						AssertEqualityResult result;

						// Call AssertEqualityComparer.Equals because it checks for IEquatable<> before using CollectionTracker
						if (aec != null)
							result = aec.Equals(expected, expectedTracker, actual, actualTracker);
						else
							result = CollectionTracker.AreCollectionsEqual(expectedTracker, actualTracker, itemComparer, itemComparer == AssertEqualityComparer<T>.DefaultInnerComparer);

						if (!result.Equal && result.Exception is null)
							return;

						mismatchedIndex = result.MismatchIndexX;

						if (result.Exception is null)
						{
							// For NotEqual that doesn't throw, pointers are irrelevant, because
							// the values are considered to be equal
							formattedExpected = expectedTracker?.FormatStart() ?? "null";
							formattedActual = actualTracker?.FormatStart() ?? "null";
						}
						else
						{
							exception = result.Exception;

							// When an exception was thrown, we want to provide a pointer so the user knows
							// which item was being inspected when the exception was thrown
							var expectedStartIdx = -1;
							var expectedEndIdx = -1;
							expectedTracker?.GetMismatchExtents(mismatchedIndex, out expectedStartIdx, out expectedEndIdx);

							var actualStartIdx = -1;
							var actualEndIdx = -1;
							actualTracker?.GetMismatchExtents(mismatchedIndex, out actualStartIdx, out actualEndIdx);

							expectedPointer = null;
							formattedExpected = expectedTracker?.FormatIndexedMismatch(expectedStartIdx, expectedEndIdx, mismatchedIndex, out expectedPointer) ?? ArgumentFormatter.Format(expected);

							actualPointer = null;
							formattedActual = actualTracker?.FormatIndexedMismatch(actualStartIdx, actualEndIdx, mismatchedIndex, out actualPointer) ?? ArgumentFormatter.Format(actual);
						}
					}
					else
					{
						try
						{
							if (!comparer.Equals(expected, actual))
								return;
						}
						catch (Exception ex)
						{
							exception = ex;
						}

						formattedExpected = ArgumentFormatter.Format(expected);
						formattedActual = ArgumentFormatter.Format(actual);
					}

					var expectedType = expected?.GetType();
					var expectedTypeDefinition = SafeGetGenericTypeDefinition(expectedType);

					var actualType = actual?.GetType();
					var actualTypeDefinition = SafeGetGenericTypeDefinition(actualType);

					var collectionDisplay = GetCollectionDisplay(expectedType, expectedTypeDefinition, actualType, actualTypeDefinition);

					if (expectedType != actualType)
					{
						var expectedTypeName = expectedType == null ? "" : (AssertHelper.IsCompilerGenerated(expectedType) ? "<generated> " : ArgumentFormatter.FormatTypeName(expectedType) + " ");
						var actualTypeName = actualType == null ? "" : (AssertHelper.IsCompilerGenerated(actualType) ? "<generated> " : ArgumentFormatter.FormatTypeName(actualType) + " ");

						var typeNameIndent = Math.Max(expectedTypeName.Length, actualTypeName.Length);

						formattedExpected = expectedTypeName.PadRight(typeNameIndent) + formattedExpected;
						formattedActual = actualTypeName.PadRight(typeNameIndent) + formattedActual;

						if (expectedPointer != null)
							expectedPointer += typeNameIndent;
						if (actualPointer != null)
							actualPointer += typeNameIndent;
					}

					throw NotEqualException.ForEqualCollectionsWithError(mismatchedIndex, formattedExpected, expectedPointer, formattedActual, actualPointer, exception, collectionDisplay);
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
					string.Format(CultureInfo.CurrentCulture, "{0:G17} (rounded from {1:G17})", expectedRounded, expected),
					string.Format(CultureInfo.CurrentCulture, "{0:G17} (rounded from {1:G17})", actualRounded, actual),
					string.Format(CultureInfo.CurrentCulture, "Values are within {0} decimal places", precision)
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
					string.Format(CultureInfo.CurrentCulture, "{0:G17} (rounded from {1:G17})", expectedRounded, expected),
					string.Format(CultureInfo.CurrentCulture, "{0:G17} (rounded from {1:G17})", actualRounded, actual),
					string.Format(CultureInfo.CurrentCulture, "Values are within {0} decimal places", precision)
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
					string.Format(CultureInfo.CurrentCulture, "Values are within tolerance {0:G17}", tolerance)
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
					string.Format(CultureInfo.CurrentCulture, "{0:G9} (rounded from {1:G9})", expectedRounded, expected),
					string.Format(CultureInfo.CurrentCulture, "{0:G9} (rounded from {1:G9})", actualRounded, actual),
					string.Format(CultureInfo.CurrentCulture, "Values are within {0} decimal places", precision)
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
					string.Format(CultureInfo.CurrentCulture, "{0:G9} (rounded from {1:G9})", expectedRounded, expected),
					string.Format(CultureInfo.CurrentCulture, "{0:G9} (rounded from {1:G9})", actualRounded, actual),
					string.Format(CultureInfo.CurrentCulture, "Values are within {0} decimal places", precision)
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
					string.Format(CultureInfo.CurrentCulture, "Values are within tolerance {0:G9}", tolerance)
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
					string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", expectedRounded, expected),
					string.Format(CultureInfo.CurrentCulture, "{0} (rounded from {1})", actualRounded, actual)
				);
		}
	}
}
