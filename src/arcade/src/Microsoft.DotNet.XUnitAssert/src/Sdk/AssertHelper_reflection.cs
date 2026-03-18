#if !XUNIT_AOT

#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0300 // Collection initialization can be simplified
#pragma warning disable IDE0301 // Simplify collection initialization

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8621
#pragma warning disable CS8625
#pragma warning disable CS8767
#endif

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit.Internal
{
	partial class AssertHelper
	{
#if XUNIT_NULLABLE
		static readonly Lazy<Type?> fileSystemInfoType = new Lazy<Type?>(() => GetTypeByName("System.IO.FileSystemInfo"));
		static readonly Lazy<PropertyInfo?> fileSystemInfoFullNameProperty = new Lazy<PropertyInfo?>(() => fileSystemInfoType.Value?.GetProperty("FullName"));
		static readonly ConcurrentDictionary<Type, Dictionary<string, Func<object?, object?>>> gettersByType = new ConcurrentDictionary<Type, Dictionary<string, Func<object?, object?>>>();
#else
		static readonly Lazy<Type> fileSystemInfoType = new Lazy<Type>(() => GetTypeByName("System.IO.FileSystemInfo"));
		static readonly Lazy<PropertyInfo> fileSystemInfoFullNameProperty = new Lazy<PropertyInfo>(() => fileSystemInfoType.Value?.GetProperty("FullName"));
		static readonly ConcurrentDictionary<Type, Dictionary<string, Func<object, object>>> gettersByType = new ConcurrentDictionary<Type, Dictionary<string, Func<object, object>>>();
#endif

		static readonly IReadOnlyList<(string Prefix, string Member)> emptyExclusions = Array.Empty<(string Prefix, string Member)>();
		static readonly Lazy<Assembly[]> getAssemblies = new Lazy<Assembly[]>(AppDomain.CurrentDomain.GetAssemblies);
		static readonly Lazy<int> maxCompareDepth = new Lazy<int>(() =>
		{
			var stringValue = Environment.GetEnvironmentVariable(EnvironmentVariables.AssertEquivalentMaxDepth);
			if (stringValue is null || !int.TryParse(stringValue, out var intValue) || intValue <= 0)
				return EnvironmentVariables.Defaults.AssertEquivalentMaxDepth;
			return intValue;
		});
		static readonly Type objectType = typeof(object);
		static readonly IEqualityComparer<object> referenceEqualityComparer = new ReferenceEqualityComparer();

#if XUNIT_NULLABLE
		static Dictionary<string, Func<object?, object?>> GetGettersForType(Type type) =>
#else
		static Dictionary<string, Func<object, object>> GetGettersForType(Type type) =>
#endif
			gettersByType.GetOrAdd(type, _type =>
			{
				var fieldGetters =
					_type
						.GetRuntimeFields()
						.Where(f => f.IsPublic && !f.IsStatic)
#if XUNIT_NULLABLE
						.Select(f => new { name = f.Name, getter = (Func<object?, object?>)f.GetValue });
#else
						.Select(f => new { name = f.Name, getter = (Func<object, object>)f.GetValue });
#endif

				var propertyGetters =
					_type
						.GetRuntimeProperties()
						.Where(p =>
							p.CanRead
							&& p.GetMethod != null
							&& p.GetMethod.IsPublic
							&& !p.GetMethod.IsStatic
#if NET8_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
							&& !p.GetMethod.ReturnType.IsByRefLike
#endif
							&& p.GetIndexParameters().Length == 0
							&& !p.GetCustomAttributes<ObsoleteAttribute>().Any()
							&& !p.GetMethod.GetCustomAttributes<ObsoleteAttribute>().Any()
						)
						.GroupBy(p => p.Name)
						.Select(group =>
						{
							// When there is more than one property with the same name, we take the one from
							// the most derived class. Start assuming the first one is the correct one, and then
							// visit each in turn to see whether it's more derived or not.
							var targetProperty = group.First();

							foreach (var candidateProperty in group.Skip(1))
								for (var candidateType = candidateProperty.DeclaringType?.BaseType; candidateType != null; candidateType = candidateType.BaseType)
									if (targetProperty.DeclaringType == candidateType)
									{
										targetProperty = candidateProperty;
										break;
									}

#if XUNIT_NULLABLE
							return new { name = targetProperty.Name, getter = (Func<object?, object?>)targetProperty.GetValue };
#else
							return new { name = targetProperty.Name, getter = (Func<object, object>)targetProperty.GetValue };
#endif
						});

				return
					fieldGetters
						.Concat(propertyGetters)
						.ToDictionary(g => g.name, g => g.getter);
			});

#if XUNIT_NULLABLE
		static Type? GetTypeByName(string typeName)
#else
		static Type GetTypeByName(string typeName)
#endif
		{
			try
			{
				foreach (var assembly in getAssemblies.Value)
				{
					var type = assembly.GetType(typeName);
					if (type != null)
						return type;
				}

				return null;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Fatal error: Exception occurred while trying to retrieve type '{0}'", typeName), ex);
			}
		}

		static bool TryConvert(
			object value,
			Type targetType,
#if XUNIT_NULLABLE
			[NotNullWhen(true)] out object? converted)
#else
			out object converted)
#endif
		{
			try
			{
				converted = Convert.ChangeType(value, targetType, CultureInfo.CurrentCulture);
				return converted != null;
			}
			catch (InvalidCastException)
			{
				converted = null;
				return false;
			}
		}

#if XUNIT_NULLABLE
		static object? UnwrapLazy(
			object? value,
#else
		static object UnwrapLazy(
			object value,
#endif
			out Type valueType)
		{
			if (value == null)
			{
				valueType = objectType;

				return null;
			}

			valueType = value.GetType();

			if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Lazy<>))
			{
				var property = valueType.GetRuntimeProperty("Value");
				if (property != null)
				{
					valueType = valueType.GenericTypeArguments[0];
					return property.GetValue(value);
				}
			}

			return value;
		}

		/// <summary/>
#if XUNIT_NULLABLE
		public static EquivalentException? VerifyEquivalence(
			object? expected,
			object? actual,
#else
		public static EquivalentException VerifyEquivalence(
			object expected,
			object actual,
#endif
			bool strict,
#if XUNIT_NULLABLE
			IReadOnlyList<(string Prefix, string Member)>? exclusions = null) =>
#else
			IReadOnlyList<(string Prefix, string Member)> exclusions = null) =>
#endif
				VerifyEquivalence(
					expected,
					actual,
					strict,
					string.Empty,
					new HashSet<object>(referenceEqualityComparer),
					new HashSet<object>(referenceEqualityComparer),
					1,
					exclusions ?? emptyExclusions
				);

#if XUNIT_NULLABLE
		static EquivalentException? VerifyEquivalence(
			object? expected,
			object? actual,
#else
		static EquivalentException VerifyEquivalence(
			object expected,
			object actual,
#endif
			bool strict,
			string prefix,
			HashSet<object> expectedRefs,
			HashSet<object> actualRefs,
			int depth,
			IReadOnlyList<(string Prefix, string Member)> exclusions)
		{
			// Check for exceeded depth
			if (depth > maxCompareDepth.Value)
				return EquivalentException.ForExceededDepth(maxCompareDepth.Value, prefix);

			// Unwrap Lazy<T>
			expected = UnwrapLazy(expected, out var expectedType);
			actual = UnwrapLazy(actual, out var actualType);

			// Check for null equivalence
			if (expected == null)
				return
					actual == null
						? null
						: EquivalentException.ForMemberValueMismatch(expected, actual, prefix);

			if (actual == null)
				return EquivalentException.ForMemberValueMismatch(expected, actual, prefix);

			// Check for identical references
			if (ReferenceEquals(expected, actual))
				return null;

			// Prevent circular references
			if (expectedRefs.Contains(expected))
				return EquivalentException.ForCircularReference(string.Format(CultureInfo.CurrentCulture, "{0}.{1}", nameof(expected), prefix));

			if (actualRefs.Contains(actual))
				return EquivalentException.ForCircularReference(string.Format(CultureInfo.CurrentCulture, "{0}.{1}", nameof(actual), prefix));

			try
			{
				expectedRefs.Add(expected);
				actualRefs.Add(actual);

				// Primitive types, enums and strings should just fall back to their Equals implementation
				if (expectedType.IsPrimitive || expectedType.IsEnum || expectedType == typeof(string) || expectedType == typeof(decimal) || expectedType == typeof(Guid))
					return VerifyEquivalenceIntrinsics(expected, actual, prefix);

				// DateTime and DateTimeOffset need to be compared via IComparable (because of a circular
				// reference via the Date property).
				if (expectedType == typeof(DateTime) || expectedType == typeof(DateTimeOffset))
					return VerifyEquivalenceDateTime(expected, actual, prefix);

				// FileSystemInfo has a recursion problem when getting the root directory
				if (fileSystemInfoType.Value != null)
					if (fileSystemInfoType.Value.IsAssignableFrom(expectedType) && fileSystemInfoType.Value.IsAssignableFrom(actualType))
						return VerifyEquivalenceFileSystemInfo(expected, actual, strict, prefix, expectedRefs, actualRefs, depth, exclusions);

				// Uri can throw for relative URIs
				var expectedUri = expected as Uri;
				var actualUri = actual as Uri;
				if (expectedUri != null && actualUri != null)
					return VerifyEquivalenceUri(expectedUri, actualUri, prefix);

				// IGrouping<TKey,TValue> is special, since it implements IEnumerable<TValue>
				var expectedGroupingTypes = ArgumentFormatter.GetGroupingTypes(expected);
				if (expectedGroupingTypes != null)
				{
					var actualGroupingTypes = ArgumentFormatter.GetGroupingTypes(actual);
					if (actualGroupingTypes != null)
						return VerifyEquivalenceGroupings(expected, expectedGroupingTypes, actual, actualGroupingTypes, strict);
				}

				// Enumerables? Check equivalence of individual members
				if (expected is IEnumerable enumerableExpected && actual is IEnumerable enumerableActual)
					return VerifyEquivalenceEnumerable(enumerableExpected, enumerableActual, strict, prefix, expectedRefs, actualRefs, depth, exclusions);

				return VerifyEquivalenceReference(expected, actual, strict, prefix, expectedRefs, actualRefs, depth, exclusions);
			}
			finally
			{
				expectedRefs.Remove(expected);
				actualRefs.Remove(actual);
			}
		}

#if XUNIT_NULLABLE
		static EquivalentException? VerifyEquivalenceDateTime(
#else
		static EquivalentException VerifyEquivalenceDateTime(
#endif
			object expected,
			object actual,
			string prefix)
		{
			try
			{
				if (expected is IComparable expectedComparable)
					return
						expectedComparable.CompareTo(actual) == 0
							? null
							: EquivalentException.ForMemberValueMismatch(expected, actual, prefix);
			}
			catch (Exception ex)
			{
				return EquivalentException.ForMemberValueMismatch(expected, actual, prefix, ex);
			}

			try
			{
				if (actual is IComparable actualComparable)
					return
						actualComparable.CompareTo(expected) == 0
							? null
							: EquivalentException.ForMemberValueMismatch(expected, actual, prefix);
			}
			catch (Exception ex)
			{
				return EquivalentException.ForMemberValueMismatch(expected, actual, prefix, ex);
			}

			throw new InvalidOperationException(
				string.Format(
					CultureInfo.CurrentCulture,
					"VerifyEquivalenceDateTime was given non-DateTime(Offset) objects; typeof(expected) = {0}, typeof(actual) = {1}",
					ArgumentFormatter.FormatTypeName(expected.GetType()),
					ArgumentFormatter.FormatTypeName(actual.GetType())
				)
			);
		}

#if XUNIT_NULLABLE
		static EquivalentException? VerifyEquivalenceEnumerable(
#else
		static EquivalentException VerifyEquivalenceEnumerable(
#endif
			IEnumerable expected,
			IEnumerable actual,
			bool strict,
			string prefix,
			HashSet<object> expectedRefs,
			HashSet<object> actualRefs,
			int depth,
			IReadOnlyList<(string Prefix, string Member)> exclusions)
		{
#if XUNIT_NULLABLE
			var expectedValues = expected.Cast<object?>().ToList();
			var actualValues = actual.Cast<object?>().ToList();
#else
			var expectedValues = expected.Cast<object>().ToList();
			var actualValues = actual.Cast<object>().ToList();
#endif
			var actualOriginalValues = actualValues.ToList();
			var collectionPrefix = prefix.Length == 0 ? string.Empty : prefix + "[]";

			// Walk the list of expected values, and look for actual values that are equivalent
			foreach (var expectedValue in expectedValues)
			{
				var actualIdx = 0;

				for (; actualIdx < actualValues.Count; ++actualIdx)
					if (VerifyEquivalence(expectedValue, actualValues[actualIdx], strict, collectionPrefix, expectedRefs, actualRefs, depth, exclusions) == null)
						break;

				if (actualIdx == actualValues.Count)
					return EquivalentException.ForMissingCollectionValue(expectedValue, actualOriginalValues, collectionPrefix);

				actualValues.RemoveAt(actualIdx);
			}

			if (strict && actualValues.Count != 0)
				return EquivalentException.ForExtraCollectionValue(expectedValues, actualOriginalValues, actualValues, collectionPrefix);

			return null;
		}

#if XUNIT_NULLABLE
		static EquivalentException? VerifyEquivalenceFileSystemInfo(
#else
		static EquivalentException VerifyEquivalenceFileSystemInfo(
#endif
			object expected,
			object actual,
			bool strict,
			string prefix,
			HashSet<object> expectedRefs,
			HashSet<object> actualRefs,
			int depth,
			IReadOnlyList<(string Prefix, string Member)> exclusions)
		{
			if (fileSystemInfoFullNameProperty.Value == null)
				throw new InvalidOperationException("Could not find 'FullName' property on type 'System.IO.FileSystemInfo'");

			var expectedType = expected.GetType();
			var actualType = actual.GetType();

			if (expectedType != actualType)
				return EquivalentException.ForMismatchedTypes(expectedType, actualType, prefix);

			var fullName = fileSystemInfoFullNameProperty.Value.GetValue(expected);
			var expectedAnonymous = new { FullName = fullName };

			return VerifyEquivalenceReference(expectedAnonymous, actual, strict, prefix, expectedRefs, actualRefs, depth, exclusions);
		}

#if XUNIT_NULLABLE
		static EquivalentException? VerifyEquivalenceGroupings(
#else
		static EquivalentException VerifyEquivalenceGroupings(
#endif
			object expected,
			Type[] expectedGroupingTypes,
			object actual,
			Type[] actualGroupingTypes,
			bool strict)
		{
			var expectedKey = typeof(IGrouping<,>).MakeGenericType(expectedGroupingTypes).GetRuntimeProperty("Key")?.GetValue(expected);
			var actualKey = typeof(IGrouping<,>).MakeGenericType(actualGroupingTypes).GetRuntimeProperty("Key")?.GetValue(actual);

			var keyException = VerifyEquivalence(expectedKey, actualKey, strict: false);
			if (keyException != null)
				return keyException;

			var toArrayMethod =
				typeof(Enumerable)
					.GetRuntimeMethods()
					.FirstOrDefault(m => m.IsStatic && m.IsPublic && m.Name == nameof(Enumerable.ToArray) && m.GetParameters().Length == 1)
						?? throw new InvalidOperationException("Could not find method Enumerable.ToArray<>");

			// Convert everything to an array so it doesn't endlessly loop on the IGrouping<> test
			var expectedToArrayMethod = toArrayMethod.MakeGenericMethod(expectedGroupingTypes[1]);
			var expectedValues = expectedToArrayMethod.Invoke(null, new[] { expected });

			var actualToArrayMethod = toArrayMethod.MakeGenericMethod(actualGroupingTypes[1]);
			var actualValues = actualToArrayMethod.Invoke(null, new[] { actual });

			if (VerifyEquivalence(expectedValues, actualValues, strict) != null)
				throw EquivalentException.ForGroupingWithMismatchedValues(expectedValues, actualValues, ArgumentFormatter.Format(expectedKey));

			return null;
		}

#if XUNIT_NULLABLE
		static EquivalentException? VerifyEquivalenceIntrinsics(
#else
		static EquivalentException VerifyEquivalenceIntrinsics(
#endif
			object expected,
			object actual,
			string prefix)
		{
			var result = expected.Equals(actual);

			if (!result && TryConvert(expected, actual.GetType(), out var converted))
				result = converted.Equals(actual);
			if (!result && TryConvert(actual, expected.GetType(), out converted))
				result = converted.Equals(expected);

			return result ? null : EquivalentException.ForMemberValueMismatch(expected, actual, prefix);
		}

#if XUNIT_NULLABLE
		static EquivalentException? VerifyEquivalenceReference(
#else
		static EquivalentException VerifyEquivalenceReference(
#endif
			object expected,
			object actual,
			bool strict,
			string prefix,
			HashSet<object> expectedRefs,
			HashSet<object> actualRefs,
			int depth,
			IReadOnlyList<(string Prefix, string Member)> exclusions)
		{
			Assert.GuardArgumentNotNull(nameof(prefix), prefix);

			var prefixDot = prefix.Length == 0 ? string.Empty : prefix + ".";

			// Enumerate over public instance fields and properties and validate equivalence
			var expectedGetters = GetGettersForType(expected.GetType());
			var actualGetters = GetGettersForType(actual.GetType());

			if (strict && expectedGetters.Count != actualGetters.Count)
				return EquivalentException.ForMemberListMismatch(expectedGetters.Keys, actualGetters.Keys, prefixDot);

			var excludedAtThisLevel =
				new HashSet<string>(
					exclusions
						.Where(e => e.Prefix == prefix)
						.Select(e => e.Member)
				);

			foreach (var kvp in expectedGetters)
			{
				if (excludedAtThisLevel.Contains(kvp.Key))
					continue;

				if (!actualGetters.TryGetValue(kvp.Key, out var actualGetter))
					return EquivalentException.ForMemberListMismatch(expectedGetters.Keys, actualGetters.Keys, prefixDot);

				var expectedMemberValue = kvp.Value(expected);
				var actualMemberValue = actualGetter(actual);

				var ex = VerifyEquivalence(expectedMemberValue, actualMemberValue, strict, prefixDot + kvp.Key, expectedRefs, actualRefs, depth + 1, exclusions);
				if (ex != null)
					return ex;
			}

			return null;
		}

#if XUNIT_NULLABLE
		static EquivalentException? VerifyEquivalenceUri(
#else
		static EquivalentException VerifyEquivalenceUri(
#endif
			Uri expected,
			Uri actual,
			string prefix)
		{
			if (expected.OriginalString != actual.OriginalString)
				return EquivalentException.ForMemberValueMismatch(expected, actual, prefix);

			return null;
		}
	}

	sealed class ReferenceEqualityComparer : IEqualityComparer<object>
	{
		public new bool Equals(
#if XUNIT_NULLABLE
			object? x,
			object? y) =>
#else
			object x,
			object y) =>
#endif
				ReferenceEquals(x, y);

#if XUNIT_NULLABLE
		public int GetHashCode([DisallowNull] object obj) =>
#else
		public int GetHashCode(object obj) =>
#endif
			obj.GetHashCode();
	}
}

#endif  // !XUNIT_AOT
