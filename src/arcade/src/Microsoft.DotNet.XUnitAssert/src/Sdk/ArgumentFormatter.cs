#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable IDE0019 // Use pattern matching
#pragma warning disable IDE0057 // Use range operator
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0300 // Simplify collection initialization

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8605
#pragma warning disable CS8625
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit.Internal;

#if XUNIT_ARGUMENTFORMATTER_PRIVATE
namespace Xunit.Internal
#else
namespace Xunit.Sdk
#endif
{
	/// <summary>
	/// Formats value for display in assertion messages and data-driven test display names.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL || XUNIT_ARGUMENTFORMATTER_PRIVATE
	internal
#else
	public
#endif
	static partial class ArgumentFormatter
	{
		static readonly Lazy<int> maxEnumerableLength = new Lazy<int>(
			() => GetEnvironmentValue(EnvironmentVariables.PrintMaxEnumerableLength, EnvironmentVariables.Defaults.PrintMaxEnumerableLength));
		static readonly Lazy<int> maxObjectDepth = new Lazy<int>(
			() => GetEnvironmentValue(EnvironmentVariables.PrintMaxObjectDepth, EnvironmentVariables.Defaults.PrintMaxObjectDepth));
		static readonly Lazy<int> maxObjectMemberCount = new Lazy<int>(
			() => GetEnvironmentValue(EnvironmentVariables.PrintMaxObjectMemberCount, EnvironmentVariables.Defaults.PrintMaxObjectMemberCount));
		static readonly Lazy<int> maxStringLength = new Lazy<int>(
			() => GetEnvironmentValue(EnvironmentVariables.PrintMaxStringLength, EnvironmentVariables.Defaults.PrintMaxStringLength));

		internal static readonly string EllipsisInBrackets = "[" + new string((char)0x00B7, 3) + "]";

		// List of intrinsic types => C# type names
		static readonly Dictionary<Type, string> TypeMappings = new Dictionary<Type, string>
		{
			{ typeof(bool), "bool" },
			{ typeof(byte), "byte" },
			{ typeof(sbyte), "sbyte" },
			{ typeof(char), "char" },
			{ typeof(decimal), "decimal" },
			{ typeof(double), "double" },
			{ typeof(float), "float" },
			{ typeof(int), "int" },
			{ typeof(uint), "uint" },
			{ typeof(long), "long" },
			{ typeof(ulong), "ulong" },
			{ typeof(object), "object" },
			{ typeof(short), "short" },
			{ typeof(ushort), "ushort" },
			{ typeof(string), "string" },
			{ typeof(IntPtr), "nint" },
			{ typeof(UIntPtr), "nuint" },
		};

		/// <summary>
		/// Gets the ellipsis value (three middle dots, aka U+00B7).
		/// </summary>
		public static string Ellipsis { get; } = new string((char)0x00B7, 3);

		/// <summary>
		/// Gets the maximum number of values printed for collections before truncation.
		/// </summary>
		public static int MaxEnumerableLength => maxEnumerableLength.Value;

		/// <summary>
		/// Gets the maximum printing depth, in terms of objects before truncation.
		/// </summary>
		public static int MaxObjectDepth => maxObjectDepth.Value;

		/// <summary>
		/// Gets the maximum number of items (properties or fields) printed in an object before truncation.
		/// </summary>
		public static int MaxObjectMemberCount => maxObjectMemberCount.Value;

		/// <summary>
		/// Gets the maximum strength length before truncation.
		/// </summary>
		public static int MaxStringLength => maxStringLength.Value;

		/// <summary>
		/// Escapes a string for printing, attempting to most closely model the value on how you would
		/// enter the value in a C# string literal. That means control codes that are normally backslash
		/// escaped (like "\n" for newline) are represented like that; all other control codes for ASCII
		/// values under 32 are printed as "\xnn".
		/// </summary>
		/// <param name="s">The string value to be escaped</param>
		public static string EscapeString(string s)
		{
#if NET8_0_OR_GREATER
			ArgumentNullException.ThrowIfNull(s);
#else
			if (s == null)
				throw new ArgumentNullException(nameof(s));
#endif

			var builder = new StringBuilder(s.Length);
			for (var i = 0; i < s.Length; i++)
			{
				var ch = s[i];

				if (TryGetEscapeSequence(ch, out var escapeSequence))
					builder.Append(escapeSequence);
				else if (ch < 32) // C0 control char
					builder.AppendFormat(CultureInfo.CurrentCulture, @"\x{0}", (+ch).ToString("x2", CultureInfo.CurrentCulture));
				else if (char.IsSurrogatePair(s, i)) // should handle the case of ch being the last one
				{
					// For valid surrogates, append like normal
					builder.Append(ch);
					builder.Append(s[++i]);
				}
				// Check for stray surrogates/other invalid chars
				else if (char.IsSurrogate(ch) || ch == '\uFFFE' || ch == '\uFFFF')
				{
					builder.AppendFormat(CultureInfo.CurrentCulture, @"\x{0}", (+ch).ToString("x4", CultureInfo.CurrentCulture));
				}
				else
					builder.Append(ch); // Append the char like normal
			}
			return builder.ToString();
		}

		/// <summary>
		/// Formats a value for display.
		/// </summary>
		/// <param name="value">The value to be formatted</param>
		/// <param name="depth">The optional printing depth (1 indicates a top-level value)</param>
		public static string Format(
#if XUNIT_NULLABLE
			object? value,
#else
			object value,
#endif
			int depth = 1)
		{
			if (value == null)
				return "null";

			var valueAsType = value as Type;
			if (valueAsType != null)
				return string.Format(CultureInfo.CurrentCulture, "typeof({0})", FormatTypeName(valueAsType, fullTypeName: true));

			try
			{
				if (value.GetType().IsEnum)
					return FormatEnumValue(value);

				if (value is char c)
					return FormatCharValue(c);

				if (value is float)
					return FormatFloatValue(value);

				if (value is double)
					return FormatDoubleValue(value);

				if (value is DateTime || value is DateTimeOffset)
					return FormatDateTimeValue(value);

				if (value is string stringParameter)
					return FormatStringValue(stringParameter);

#if !XUNIT_ARGUMENTFORMATTER_PRIVATE
				if (value is CollectionTracker tracker)
					return tracker.FormatStart(depth);
#endif

				if (value is IEnumerable enumerable)
					return FormatEnumerableValue(enumerable, depth);

				var type = value.GetType();

#if NET8_0_OR_GREATER
				if (value is ITuple tuple)
					return FormatTupleValue(tuple, depth);
#else
				if (tupleInterfaceType != null && type.GetInterfaces().Contains(tupleInterfaceType))
					return FormatTupleValue(value, depth);
#endif

				if (type.IsValueType)
					return FormatValueTypeValue(value, type);

				if (value is Task task)
				{
					var typeParameters = type.GenericTypeArguments;
					var typeName =
						typeParameters.Length == 0
							? "Task"
							: string.Format(CultureInfo.CurrentCulture, "Task<{0}>", string.Join(",", typeParameters.Select(t => FormatTypeName(t))));

					return string.Format(CultureInfo.CurrentCulture, "{0} {{ Status = {1} }}", typeName, task.Status);
				}

				// TODO: ValueTask?

				var isAnonymousType = type.IsAnonymousType();
				return FormatComplexValue(value, depth, type, isAnonymousType);
			}
			catch (Exception ex)
			{
				// Sometimes an exception is thrown when formatting an argument, such as in ToString.
				// In these cases, we don't want to crash, as tests may have passed despite this.
				return string.Format(CultureInfo.CurrentCulture, "{0} was thrown formatting an object of type \"{1}\"", ex.GetType().Name, value.GetType());
			}
		}

		static string FormatCharValue(char value)
		{
			if (value == '\'')
				return @"'\''";

			// Take care of all of the escape sequences
			if (TryGetEscapeSequence(value, out var escapeSequence))
				return string.Format(CultureInfo.CurrentCulture, "'{0}'", escapeSequence);

			if (char.IsLetterOrDigit(value) || char.IsPunctuation(value) || char.IsSymbol(value) || value == ' ')
				return string.Format(CultureInfo.CurrentCulture, "'{0}'", value);

			// Fallback to hex
			return string.Format(CultureInfo.CurrentCulture, "0x{0:x4}", (int)value);
		}

		static string FormatDateTimeValue(object value) =>
			string.Format(CultureInfo.CurrentCulture, "{0:o}", value);

		static string FormatDoubleValue(object value) =>
			string.Format(CultureInfo.CurrentCulture, "{0:G17}", value);

		static string FormatEnumValue(object value) =>
#if NET8_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
			value.ToString()?.Replace(", ", " | ", StringComparison.Ordinal) ?? "null";
#else
			value.ToString()?.Replace(", ", " | ") ?? "null";
#endif

		static string FormatEnumerableValue(
			IEnumerable enumerable,
			int depth)
		{
			if (depth > MaxObjectDepth)
				return EllipsisInBrackets;

			var result = new StringBuilder(GetGroupingKeyPrefix(enumerable));
			if (result.Length == 0 && !SafeToMultiEnumerate(enumerable))
				return EllipsisInBrackets;

			// This should only be used on values that are known to be re-enumerable
			// safely, like collections that implement IDictionary or IList.
			var idx = 0;
			var enumerator = enumerable.GetEnumerator();

			result.Append('[');

			while (enumerator.MoveNext())
			{
				if (idx != 0)
					result.Append(", ");

				if (idx == MaxEnumerableLength)
				{
					result.Append(Ellipsis);
					break;
				}

				var current = enumerator.Current;
				var nextDepth = current is IEnumerable ? depth + 1 : depth;

				result.Append(Format(current, nextDepth));

				++idx;
			}

			result.Append(']');
			return result.ToString();
		}

		static string FormatFloatValue(object value) =>
			string.Format(CultureInfo.CurrentCulture, "{0:G9}", value);

		static string FormatStringValue(string value)
		{
#if NET8_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
			value = EscapeString(value).Replace(@"""", @"\""", StringComparison.Ordinal); // escape double quotes
#else
			value = EscapeString(value).Replace(@"""", @"\"""); // escape double quotes
#endif

			if (value.Length > MaxStringLength)
			{
				var displayed = value.Substring(0, MaxStringLength);
				return string.Format(CultureInfo.CurrentCulture, "\"{0}\"{1}", displayed, Ellipsis);
			}

			return string.Format(CultureInfo.CurrentCulture, "\"{0}\"", value);
		}

		static string FormatTupleValue(
#if NET8_0_OR_GREATER
			ITuple tupleParameter,
#else
			object tupleParameter,
#endif
			int depth)
		{
			var result = new StringBuilder("Tuple (");
#if NET8_0_OR_GREATER
			var length = tupleParameter.Length;
#elif XUNIT_NULLABLE
			var length = (int)tupleLength!.GetValue(tupleParameter)!;
#else
			var length = (int)tupleLength.GetValue(tupleParameter);
#endif

			for (var idx = 0; idx < length; ++idx)
			{
				if (idx != 0)
					result.Append(", ");

#if NET8_0_OR_GREATER
				var value = tupleParameter[idx];
#elif XUNIT_NULLABLE
				var value = tupleIndexer!.GetValue(tupleParameter, new object[] { idx });
#else
				var value = tupleIndexer.GetValue(tupleParameter, new object[] { idx });
#endif
				result.Append(Format(value, depth + 1));
			}

			result.Append(')');

			return result.ToString();
		}

		/// <summary>
		/// Formats a type. This maps built-in C# types to their C# native name (e.g., printing "int" instead
		/// of "Int32" or "System.Int32").
		/// </summary>
		/// <param name="type">The type to get the formatted name of</param>
		/// <param name="fullTypeName">Set to <see langword="true"/> to include the namespace; set to <see langword="false"/> for just the simple type name</param>
		public static string FormatTypeName(
			Type type,
			bool fullTypeName = false)
		{
#if NET8_0_OR_GREATER
			ArgumentNullException.ThrowIfNull(type, nameof(type));
#else
			if (type is null)
				throw new ArgumentNullException(nameof(type));
#endif

			var arraySuffix = "";

			// Deconstruct and re-construct array
			while (type.IsArray)
			{
				if (type.IsSZArrayType())
					arraySuffix += "[]";
				else
				{
					var rank = type.GetArrayRank();
					if (rank == 1)
						arraySuffix += "[*]";
					else
						arraySuffix += string.Format(CultureInfo.CurrentCulture, "[{0}]", new string(',', rank - 1));
				}

#if XUNIT_NULLABLE
				type = type.GetElementType()!;
#else
				type = type.GetElementType();
#endif
			}

			// Map C# built-in type names
			var shortType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
			if (!TypeMappings.TryGetValue(shortType, out var result))
				result = fullTypeName ? type.FullName : type.Name;

			if (result is null)
				return type.Name;

#if NET8_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
			var tickIdx = result.IndexOf('`', StringComparison.Ordinal);
#else
			var tickIdx = result.IndexOf('`');
#endif
			if (tickIdx > 0)
				result = result.Substring(0, tickIdx);

			if (type.IsGenericTypeDefinition)
				result = string.Format(CultureInfo.CurrentCulture, "{0}<{1}>", result, new string(',', type.GetGenericArguments().Length - 1));
			else if (type.IsGenericType)
			{
				if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
					result = FormatTypeName(type.GenericTypeArguments[0]) + "?";
				else
					result = string.Format(CultureInfo.CurrentCulture, "{0}<{1}>", result, string.Join(", ", type.GenericTypeArguments.Select(t => FormatTypeName(t))));
			}

			return result + arraySuffix;
		}

		static int GetEnvironmentValue(
			string environmentVariableName,
			int defaultValue,
			bool allowMaxValue = true)
		{
			var stringValue = Environment.GetEnvironmentVariable(environmentVariableName);
			if (string.IsNullOrWhiteSpace(stringValue) || !int.TryParse(stringValue, out var intValue))
				return defaultValue;

			if (intValue <= 0)
				return allowMaxValue ? int.MaxValue : defaultValue;

			return intValue;
		}

		static bool IsAnonymousType(this Type type)
		{
			// There isn't a sanctioned way to do this, so we look for compiler-generated types that
			// include "AnonymousType" in their names.
			if (type.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
				return false;

#if NET8_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
			return type.Name.Contains("AnonymousType", StringComparison.Ordinal);
#else
			return type.Name.Contains("AnonymousType");
#endif
		}

		static bool IsSZArrayType(this Type type) =>
#if NET8_0_OR_GREATER
			type.IsSZArray;
#elif XUNIT_NULLABLE
			type == type.GetElementType()!.MakeArrayType();
#else
			type == type.GetElementType().MakeArrayType();
#endif

		static bool SafeToMultiEnumerate(IEnumerable collection) =>
			collection is Array ||
			collection is BitArray ||
			collection is IList ||
			collection is IDictionary ||
			GetSetElementType(collection) != null ||
			IsEnumerableOfGrouping(collection);

		static bool TryGetEscapeSequence(
			char ch,
#if XUNIT_NULLABLE
			out string? value)
#else
			out string value)
#endif
		{
			value = null;

			if (ch == '\t') // tab
				value = @"\t";
			if (ch == '\n') // newline
				value = @"\n";
			if (ch == '\v') // vertical tab
				value = @"\v";
			if (ch == '\a') // alert
				value = @"\a";
			if (ch == '\r') // carriage return
				value = @"\r";
			if (ch == '\f') // formfeed
				value = @"\f";
			if (ch == '\b') // backspace
				value = @"\b";
			if (ch == '\0') // null char
				value = @"\0";
			if (ch == '\\') // backslash
				value = @"\\";

			return value != null;
		}

#if XUNIT_NULLABLE
		internal static Exception? UnwrapException(Exception? ex)
#else
		internal static Exception UnwrapException(Exception ex)
#endif
		{
			if (ex == null)
				return null;

			while (true)
			{
				var tiex = ex as TargetInvocationException;
				if (tiex == null || tiex.InnerException == null)
					return ex;

				ex = tiex.InnerException;
			}
		}

		static string WrapAndGetFormattedValue(
#if XUNIT_NULLABLE
			Func<object?> getter,
#else
			Func<object> getter,
#endif
			int depth)
		{
			try
			{
				return Format(getter(), depth + 1);
			}
			catch (Exception ex)
			{
				return string.Format(CultureInfo.CurrentCulture, "(throws {0})", UnwrapException(ex)?.GetType().Name);
			}
		}
	}
}
