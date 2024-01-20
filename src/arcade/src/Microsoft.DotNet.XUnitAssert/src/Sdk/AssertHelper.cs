#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8600
#pragma warning disable CS8601
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8621
#endif

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit.Sdk;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

#if NETCOREAPP3_0_OR_GREATER
using System.Threading.Tasks;
#endif

namespace Xunit.Internal
{
	internal static class AssertHelper
	{
		static readonly Dictionary<char, string> encodings = new Dictionary<char, string>
		{
			{ '\0', @"\0" },  // Null
			{ '\a', @"\a" },  // Alert
			{ '\b', @"\b" },  // Backspace
			{ '\f', @"\f" },  // Form feed
			{ '\n', @"\n" },  // New line
			{ '\r', @"\r" },  // Carriage return
			{ '\t', @"\t" },  // Horizontal tab
			{ '\v', @"\v" },  // Vertical tab
			{ '\\', @"\\" },  // Backslash
		};

#if XUNIT_NULLABLE
		static ConcurrentDictionary<Type, Dictionary<string, Func<object?, object?>>> gettersByType = new ConcurrentDictionary<Type, Dictionary<string, Func<object?, object?>>>();
#else
		static ConcurrentDictionary<Type, Dictionary<string, Func<object, object>>> gettersByType = new ConcurrentDictionary<Type, Dictionary<string, Func<object, object>>>();
#endif

		const string fileSystemInfoFqn = "System.IO.FileSystemInfo, System.Runtime";
#if XUNIT_NULLABLE
		static readonly Lazy<TypeInfo?> fileSystemInfoTypeInfo = new Lazy<TypeInfo?>(() => Type.GetType(fileSystemInfoFqn)?.GetTypeInfo());
		static readonly Lazy<PropertyInfo?> fileSystemInfoFullNameProperty = new Lazy<PropertyInfo?>(() => Type.GetType(fileSystemInfoFqn)?.GetTypeInfo().GetDeclaredProperty("FullName"));
#else
		static readonly Lazy<TypeInfo> fileSystemInfoTypeInfo = new Lazy<TypeInfo>(() => GetTypeInfo(fileSystemInfoFqn)?.GetTypeInfo());
		static readonly Lazy<PropertyInfo> fileSystemInfoFullNameProperty = new Lazy<PropertyInfo>(() => fileSystemInfoTypeInfo.Value?.GetDeclaredProperty("FullName"));
#endif

		static readonly Lazy<Assembly[]> getAssemblies = new Lazy<Assembly[]>(() =>
		{
#if NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6
			var appDomainType = Type.GetType("System.AppDomain");
			if (appDomainType != null)
			{
				var currentDomainProperty = appDomainType.GetRuntimeProperty("CurrentDomain");
				if (currentDomainProperty != null)
				{
					var getAssembliesMethod = appDomainType.GetRuntimeMethods().FirstOrDefault(m => m.Name == "GetAssemblies");
					if (getAssembliesMethod != null)
					{
						var currentDomain = currentDomainProperty.GetValue(null);
						if (currentDomain != null)
						{
							var getAssembliesArgs = getAssembliesMethod.GetParameters().Length == 1 ? new object[] { false } : new object[0];
							var assemblies = getAssembliesMethod.Invoke(currentDomain, getAssembliesArgs) as Assembly[];
							if (assemblies != null)
								return assemblies;
						}
					}
				}
			}

			return new Assembly[0];
#else
			return AppDomain.CurrentDomain.GetAssemblies();
#endif
		});

		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2111: Method 'lambda expression' with parameters or return value with `DynamicallyAccessedMembersAttribute` is accessed via reflection. Trimmer can't guarantee availability of the requirements of the method.", Justification = "The lambda will only be called by the value in the type parameter, which has the same requirements.")]
#if XUNIT_NULLABLE
		static Dictionary<string, Func<object?, object?>> GetGettersForType([DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties)] Type type) =>
#else
		static Dictionary<string, Func<object, object>> GetGettersForType([DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties)] Type type) =>
#endif
			gettersByType.GetOrAdd(type,
				([DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties)] Type _type) =>
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
						.Where(p => p.CanRead && p.GetMethod != null && p.GetMethod.IsPublic && !p.GetMethod.IsStatic && p.GetIndexParameters().Length == 0)
#if XUNIT_NULLABLE
						.Select(p => new { name = p.Name, getter = (Func<object?, object?>)p.GetValue });
#else
						.Select(p => new { name = p.Name, getter = (Func<object, object>)p.GetValue });
#endif

				return
					fieldGetters
						.Concat(propertyGetters)
						.ToDictionary(g => g.name, g => g.getter);
			});

#if !XUNIT_AOT
#if XUNIT_NULLABLE
		static TypeInfo? GetTypeInfo(string typeName)
#else
		static TypeInfo GetTypeInfo(string typeName)
#endif
		{
			try
			{
				foreach (var assembly in getAssemblies.Value)
				{
					var type = assembly.GetType(typeName);
					if (type != null)
						return type.GetTypeInfo();
				}

				return null;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Fatal error: Exception occurred while trying to retrieve type '{0}'", typeName), ex);
			}
		}
#endif

		internal static bool IsCompilerGenerated(Type type) =>
			type.GetTypeInfo().CustomAttributes.Any(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.CompilerGeneratedAttribute");

		internal static string ShortenAndEncodeString(
#if XUNIT_NULLABLE
			string? value,
#else
			string value,
#endif
			int index,
			out int pointerIndent)
		{
			if (value == null)
			{
				pointerIndent = -1;
				return "null";
			}

			var start = Math.Max(index - 20, 0);
			var end = Math.Min(start + 41, value.Length);
			start = Math.Max(end - 41, 0);
			var printedValue = new StringBuilder(100);
			pointerIndent = 0;

			if (start > 0)
			{
				printedValue.Append(ArgumentFormatter.Ellipsis);
				pointerIndent += 3;
			}

			printedValue.Append('\"');
			pointerIndent++;

			for (var idx = start; idx < end; ++idx)
			{
				var c = value[idx];
				var paddingLength = 1;

#if XUNIT_NULLABLE
				string? encoding;
#else
				string encoding;
#endif

				if (encodings.TryGetValue(c, out encoding))
				{
					printedValue.Append(encoding);
					paddingLength = encoding.Length;
				}
				else
					printedValue.Append(c);

				if (idx < index)
					pointerIndent += paddingLength;
			}

			printedValue.Append('\"');

			if (end < value.Length)
				printedValue.Append(ArgumentFormatter.Ellipsis);

			return printedValue.ToString();
		}

#if XUNIT_NULLABLE
		internal static string ShortenAndEncodeString(string? value)
#else
		internal static string ShortenAndEncodeString(string value)
#endif
		{
			int pointerIndent;

			return ShortenAndEncodeString(value, 0, out pointerIndent);
		}

#if XUNIT_NULLABLE
		internal static string ShortenAndEncodeStringEnd(string? value)
#else
		internal static string ShortenAndEncodeStringEnd(string value)
#endif
		{
			int pointerIndent;

			return ShortenAndEncodeString(value, (value?.Length - 1) ?? 0, out pointerIndent);
		}

#if NETCOREAPP3_0_OR_GREATER

#if XUNIT_NULLABLE
		[return: NotNullIfNotNull(nameof(data))]
		internal static IEnumerable<T>? ToEnumerable<T>(IAsyncEnumerable<T>? data) =>
#else
		internal static IEnumerable<T> ToEnumerable<T>(IAsyncEnumerable<T> data) =>
#endif
			data == null ? null : ToEnumerableImpl(data);

		static IEnumerable<T> ToEnumerableImpl<T>(IAsyncEnumerable<T> data)
		{
			var enumerator = data.GetAsyncEnumerator();

			try
			{
				while (WaitForValueTask(enumerator.MoveNextAsync()))
					yield return enumerator.Current;
			}
			finally
			{
				WaitForValueTask(enumerator.DisposeAsync());
			}
		}

#endif

		static bool TryConvert(
			object value,
			Type targetType,
#if XUNIT_NULLABLE
			[NotNullWhen(true)] out object? converted)
#else
			out object converted)
#endif
		{
			converted = Convert.ChangeType(value, targetType, CultureInfo.CurrentCulture);
			return converted != null;
		}

#if XUNIT_NULLABLE
		public static EquivalentException? VerifyEquivalence(
			object? expected,
			object? actual,
#else
		public static EquivalentException VerifyEquivalence(
			object expected,
			object actual,
#endif
			bool strict)
		{
			return VerifyEquivalence(expected, actual, strict, string.Empty, new HashSet<object>(), new HashSet<object>(), 1);
		}

#if XUNIT_NULLABLE
		static EquivalentException? VerifyEquivalence<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties)] T,
			[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties)] U>(
			T? expected,
			U? actual,
#else
		static EquivalentException VerifyEquivalence<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties)] T,
			[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties)] U>(
			T expected,
			U actual,
#endif
			bool strict,
			string prefix,
			HashSet<object> expectedRefs,
			HashSet<object> actualRefs,
			int depth)
		{
			// Check for exceeded depth
			if (depth == 50)
				return EquivalentException.ForExceededDepth(50, prefix);

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

			expectedRefs.Add(expected);
			actualRefs.Add(actual);

			try
			{
				var expectedType = expected.GetType();
				var expectedTypeInfo = expectedType.GetTypeInfo();
				var actualType = actual.GetType();
				var actualTypeInfo = actualType.GetTypeInfo();

				// Primitive types, enums and strings should just fall back to their Equals implementation
				if (expectedTypeInfo.IsPrimitive || expectedTypeInfo.IsEnum || expectedType == typeof(string))
					return VerifyEquivalenceIntrinsics(expected, actual, prefix);

				// DateTime and DateTimeOffset need to be compared via IComparable (because of a circular
				// reference via the Date property).
				if (expectedType == typeof(DateTime) || expectedType == typeof(DateTimeOffset))
					return VerifyEquivalenceDateTime(expected, actual, prefix);

				// FileSystemInfo has a recursion problem when getting the root directory
				if (fileSystemInfoTypeInfo.Value != null)
					if (fileSystemInfoTypeInfo.Value.IsAssignableFrom(expectedTypeInfo) && fileSystemInfoTypeInfo.Value.IsAssignableFrom(actualTypeInfo))
						return VerifyEquivalenceFileSystemInfo(expected, actual, strict, prefix, expectedRefs, actualRefs, depth);

				// Enumerables? Check equivalence of individual members
				var enumerableExpected = expected as IEnumerable;
				var enumerableActual = actual as IEnumerable;
				if (enumerableExpected != null && enumerableActual != null)
					return VerifyEquivalenceEnumerable(enumerableExpected, enumerableActual, strict, prefix, expectedRefs, actualRefs, depth);

				return VerifyEquivalenceReference(expected, actual, strict, prefix, expectedRefs, actualRefs, depth);
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
				var expectedComparable = expected as IComparable;
				if (expectedComparable != null)
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
				var actualComparable = actual as IComparable;
				if (actualComparable != null)
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
			int depth)
		{
#if XUNIT_NULLABLE
			var expectedValues = expected.Cast<object?>().ToList();
			var actualValues = actual.Cast<object?>().ToList();
#else
			var expectedValues = expected.Cast<object>().ToList();
			var actualValues = actual.Cast<object>().ToList();
#endif
			var actualOriginalValues = actualValues.ToList();

			// Walk the list of expected values, and look for actual values that are equivalent
			foreach (var expectedValue in expectedValues)
			{
				var actualIdx = 0;
				for (; actualIdx < actualValues.Count; ++actualIdx)
					if (VerifyEquivalence(expectedValue, actualValues[actualIdx], strict, "", expectedRefs, actualRefs, depth) == null)
						break;

				if (actualIdx == actualValues.Count)
					return EquivalentException.ForMissingCollectionValue(expectedValue, actualOriginalValues, prefix);

				actualValues.RemoveAt(actualIdx);
			}

			if (strict && actualValues.Count != 0)
				return EquivalentException.ForExtraCollectionValue(expectedValues, actualOriginalValues, actualValues, prefix);

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
			int depth)
		{
			if (fileSystemInfoFullNameProperty.Value == null)
				throw new InvalidOperationException("Could not find 'FullName' property on type 'System.IO.FileSystemInfo'");

			var expectedType = expected.GetType();
			var actualType = actual.GetType();

			if (expectedType != actualType)
				return EquivalentException.ForMismatchedTypes(expectedType, actualType, prefix);

			var fullName = fileSystemInfoFullNameProperty.Value.GetValue(expected);
			var expectedAnonymous = new { FullName = fullName };

			return VerifyEquivalenceReference(expectedAnonymous, actual, strict, prefix, expectedRefs, actualRefs, depth);
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

			var converted = default(object);
			if (!result && TryConvert(expected, actual.GetType(), out converted))
				result = converted.Equals(actual);
			if (!result && TryConvert(actual, expected.GetType(), out converted))
				result = converted.Equals(expected);

			return result ? null : EquivalentException.ForMemberValueMismatch(expected, actual, prefix);
		}

		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072", Justification = "We need to use the runtime type for getting the getters as we can't recursively preserve them. Any members that are trimmed were not touched by the test and likely are not important for equivalence.")]
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
			int depth)
		{
			Assert.GuardArgumentNotNull(nameof(prefix), prefix);

			var prefixDot = prefix.Length == 0 ? string.Empty : prefix + ".";

			// Enumerate over public instance fields and properties and validate equivalence
			var expectedGetters = GetGettersForType(expected.GetType());
			var actualGetters = GetGettersForType(actual.GetType());

			if (strict && expectedGetters.Count != actualGetters.Count)
				return EquivalentException.ForMemberListMismatch(expectedGetters.Keys, actualGetters.Keys, prefixDot);

			foreach (var kvp in expectedGetters)
			{
#if XUNIT_NULLABLE
				Func<object?, object?>? actualGetter;
#else
				Func<object, object> actualGetter;
#endif

				if (!actualGetters.TryGetValue(kvp.Key, out actualGetter))
					return EquivalentException.ForMemberListMismatch(expectedGetters.Keys, actualGetters.Keys, prefixDot);

				var expectedMemberValue = kvp.Value(expected);
				var actualMemberValue = actualGetter(actual);

				var ex = VerifyEquivalence(expectedMemberValue, actualMemberValue, strict, prefixDot + kvp.Key, expectedRefs, actualRefs, depth + 1);
				if (ex != null)
					return ex;
			}

			return null;
		}

#if NETCOREAPP3_0_OR_GREATER

		static void WaitForValueTask(ValueTask valueTask)
		{
			var valueTaskAwaiter = valueTask.GetAwaiter();
			if (valueTaskAwaiter.IsCompleted)
				return;

			// Let the task complete on a thread pool thread while we block the main thread
			Task.Run(valueTask.AsTask).GetAwaiter().GetResult();
		}

		static T WaitForValueTask<T>(ValueTask<T> valueTask)
		{
			var valueTaskAwaiter = valueTask.GetAwaiter();
			if (valueTaskAwaiter.IsCompleted)
				return valueTaskAwaiter.GetResult();

			// Let the task complete on a thread pool thread while we block the main thread
			return Task.Run(valueTask.AsTask).GetAwaiter().GetResult();
		}

#endif
	}
}
