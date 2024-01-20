#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8600
#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8605
#pragma warning disable CS8618
#pragma warning disable CS8625
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
	static class ArgumentFormatter
	{
		internal static readonly string EllipsisInBrackets = "[" + new string((char)0x00B7, 3) + "]";

		/// <summary>
		/// Gets the maximum printing depth, in terms of objects before truncation.
		/// </summary>
		public const int MAX_DEPTH = 3;

		/// <summary>
		/// Gets the maximum number of values printed for collections before truncation.
		/// </summary>
		public const int MAX_ENUMERABLE_LENGTH = 5;

		/// <summary>
		/// Gets the maximum number of items (properties or fields) printed in an object before truncation.
		/// </summary>
		public const int MAX_OBJECT_ITEM_COUNT = 5;

		/// <summary>
		/// Gets the maximum strength length before truncation.
		/// </summary>
		public const int MAX_STRING_LENGTH = 50;

#pragma warning disable CA1825  // Can't use Array.Empty here because it's not available in .NET Standard 1.1
		static readonly object[] EmptyObjects = new object[0];
		static readonly Type[] EmptyTypes = new Type[0];
#pragma warning restore CA1825

		// List of intrinsic types => C# type names
		static readonly Dictionary<TypeInfo, string> TypeMappings = new Dictionary<TypeInfo, string>
		{
			{ typeof(bool).GetTypeInfo(), "bool" },
			{ typeof(byte).GetTypeInfo(), "byte" },
			{ typeof(sbyte).GetTypeInfo(), "sbyte" },
			{ typeof(char).GetTypeInfo(), "char" },
			{ typeof(decimal).GetTypeInfo(), "decimal" },
			{ typeof(double).GetTypeInfo(), "double" },
			{ typeof(float).GetTypeInfo(), "float" },
			{ typeof(int).GetTypeInfo(), "int" },
			{ typeof(uint).GetTypeInfo(), "uint" },
			{ typeof(long).GetTypeInfo(), "long" },
			{ typeof(ulong).GetTypeInfo(), "ulong" },
			{ typeof(object).GetTypeInfo(), "object" },
			{ typeof(short).GetTypeInfo(), "short" },
			{ typeof(ushort).GetTypeInfo(), "ushort" },
			{ typeof(string).GetTypeInfo(), "string" },
		};

		/// <summary>
		/// Gets the ellipsis value (three middle dots, aka U+00B7).
		/// </summary>
		public static string Ellipsis { get; } = new string((char)0x00B7, 3);

		/// <summary>
		/// Escapes a string for printing, attempting to most closely model the value on how you would
		/// enter the value in a C# string literal. That means control codes that are normally backslash
		/// escaped (like "\n" for newline) are represented like that; all other control codes for ASCII
		/// values under 32 are printed as "\xnn".
		/// </summary>
		/// <param name="s">The string value to be escaped</param>
		public static string EscapeString(string s)
		{
#if NET6_0_OR_GREATER
			ArgumentNullException.ThrowIfNull(s);
#else
			if (s == null)
				throw new ArgumentNullException(nameof(s));
#endif

			var builder = new StringBuilder(s.Length);
			for (var i = 0; i < s.Length; i++)
			{
				var ch = s[i];
#if XUNIT_NULLABLE
				string? escapeSequence;
#else
				string escapeSequence;
#endif
				if (TryGetEscapeSequence(ch, out escapeSequence))
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

#if XUNIT_NULLABLE
		public static string Format(Type? value)
#else
		public static string Format(Type value)
#endif
		{
			if (value is null)
				return "null";

			return string.Format(CultureInfo.CurrentCulture, "typeof({0})", FormatTypeName(value, fullTypeName: true));
		}

		/// <summary>
		/// Formats a value for display.
		/// </summary>
		/// <param name="value">The value to be formatted</param>
		/// <param name="depth">The optional printing depth (1 indicates a top-level value)</param>
		[DynamicDependency("ToString", typeof(object))]
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072", Justification = "We can't easily annotate callers of this type to require them to preserve the ToString method as we need to use the runtime type. We also can't preserve all of the properties and fields for the complex type printing, but any members that are trimmed aren't used and thus don't contribute to the asserts.")]
		public static string Format<
			[DynamicallyAccessedMembers(
				DynamicallyAccessedMemberTypes.PublicFields |
				DynamicallyAccessedMemberTypes.NonPublicFields |
				DynamicallyAccessedMemberTypes.PublicProperties |
				DynamicallyAccessedMemberTypes.NonPublicProperties |
				DynamicallyAccessedMemberTypes.PublicMethods)] T>(T value, int depth = 1)
		{
			if (value == null)
				return "null";

			var valueAsType = value as Type;
			if (valueAsType != null)
				return string.Format(CultureInfo.CurrentCulture, "typeof({0})", FormatTypeName(valueAsType, fullTypeName: true));

			try
			{
				if (value.GetType().GetTypeInfo().IsEnum)
					return FormatEnumValue(value);

				if (value is char c)
					return FormatCharValue(c);

				if (value is float)
					return FormatFloatValue(value);

				if (value is double)
					return FormatDoubleValue(value);

				if (value is DateTime || value is DateTimeOffset)
					return FormatDateTimeValue(value);

				var stringParameter = value as string;
				if (stringParameter != null)
					return FormatStringValue(stringParameter);

#if !XUNIT_ARGUMENTFORMATTER_PRIVATE
				var tracker = value as CollectionTracker;
				if (tracker != null)
					return tracker.FormatStart(depth);
#endif

				var enumerable = value as IEnumerable;
				if (enumerable != null)
					return FormatEnumerableValue(enumerable, depth);

				var type = value.GetType();
				var typeInfo = type.GetTypeInfo();

				if (value is ITuple tuple)
					return FormatTupleValue(tuple, depth);

				if (typeInfo.IsValueType)
					return FormatValueTypeValue(value, typeInfo);

				var task = value as Task;
				if (task != null)
				{
					var typeParameters = typeInfo.GenericTypeArguments;
					var typeName =
						typeParameters.Length == 0
							? "Task"
							: string.Format(CultureInfo.CurrentCulture, "Task<{0}>", string.Join(",", typeParameters.Select(t => FormatTypeName(t))));

					return string.Format(CultureInfo.CurrentCulture, "{0} {{ Status = {1} }}", typeName, task.Status);
				}

				// TODO: ValueTask?

				var isAnonymousType = typeInfo.IsAnonymousType();
				if (!isAnonymousType)
				{
					var toString = type.GetRuntimeMethod("ToString", EmptyTypes);

					if (toString != null && toString.DeclaringType != typeof(object))
#if XUNIT_NULLABLE
						return ((string?)toString.Invoke(value, EmptyObjects)) ?? "null";
#else
						return ((string)toString.Invoke(value, EmptyObjects)) ?? "null";
#endif
				}

				return FormatComplexValue(value, depth, type, isAnonymousType);
			}
			catch (Exception ex)
			{
				// Sometimes an exception is thrown when formatting an argument, such as in ToString.
				// In these cases, we don't want xunit to crash, as tests may have passed despite this.
				return string.Format(CultureInfo.CurrentCulture, "{0} was thrown formatting an object of type \"{1}\"", ex.GetType().Name, value.GetType());
			}
		}

		static string FormatCharValue(char value)
		{
			if (value == '\'')
				return @"'\''";

			// Take care of all of the escape sequences
#if XUNIT_NULLABLE
			string? escapeSequence;
#else
			string escapeSequence;
#endif
			if (TryGetEscapeSequence(value, out escapeSequence))
				return string.Format(CultureInfo.CurrentCulture, "'{0}'", escapeSequence);

			if (char.IsLetterOrDigit(value) || char.IsPunctuation(value) || char.IsSymbol(value) || value == ' ')
				return string.Format(CultureInfo.CurrentCulture, "'{0}'", value);

			// Fallback to hex
			return string.Format(CultureInfo.CurrentCulture, "0x{0:x4}", (int)value);
		}

		static string FormatComplexValue(
			object value,
			int depth,
			[DynamicallyAccessedMembers(
				DynamicallyAccessedMemberTypes.PublicFields |
				DynamicallyAccessedMemberTypes.NonPublicFields |
				DynamicallyAccessedMemberTypes.PublicProperties |
				DynamicallyAccessedMemberTypes.NonPublicProperties)] Type type,
			bool isAnonymousType)
		{
			var typeName = isAnonymousType ? "" : type.Name + " ";

			if (depth == MAX_DEPTH)
				return string.Format(CultureInfo.CurrentCulture, "{0}{{ {1} }}", typeName, Ellipsis);

			var fields =
				type
					.GetRuntimeFields()
					.Where(f => f.IsPublic && !f.IsStatic)
					.Select(f => new { name = f.Name, value = WrapAndGetFormattedValue(() => f.GetValue(value), depth) });

			var properties =
				type
					.GetRuntimeProperties()
					.Where(p => p.GetMethod != null && p.GetMethod.IsPublic && !p.GetMethod.IsStatic)
					.Select(p => new { name = p.Name, value = WrapAndGetFormattedValue(() => p.GetValue(value), depth) });

			var parameters =
				fields
					.Concat(properties)
					.OrderBy(p => p.name)
					.Take(MAX_OBJECT_ITEM_COUNT + 1)
					.ToList();

			if (parameters.Count == 0)
				return string.Format(CultureInfo.CurrentCulture, "{0}{{ }}", typeName);

			var formattedParameters = string.Join(", ", parameters.Take(MAX_OBJECT_ITEM_COUNT).Select(p => string.Format(CultureInfo.CurrentCulture, "{0} = {1}", p.name, p.value)));

			if (parameters.Count > MAX_OBJECT_ITEM_COUNT)
				formattedParameters += ", " + Ellipsis;

			return string.Format(CultureInfo.CurrentCulture, "{0}{{ {1} }}", typeName, formattedParameters);
		}

		static string FormatDateTimeValue(object value) =>
			string.Format(CultureInfo.CurrentCulture, "{0:o}", value);

		static string FormatDoubleValue(object value) =>
			string.Format(CultureInfo.CurrentCulture, "{0:G17}", value);

		static string FormatEnumValue(object value) =>
#if NETCOREAPP2_0_OR_GREATER
			value.ToString()?.Replace(", ", " | ", StringComparison.Ordinal) ?? "null";
#else
			value.ToString()?.Replace(", ", " | ") ?? "null";
#endif

		static string FormatEnumerableValue(
			IEnumerable enumerable,
			int depth)
		{
			if (depth == MAX_DEPTH || !SafeToMultiEnumerate(enumerable))
				return EllipsisInBrackets;

			// This should only be used on values that are known to be re-enumerable
			// safely, like collections that implement IDictionary or IList.
			var idx = 0;
			var result = new StringBuilder("[");
			var enumerator = enumerable.GetEnumerator();

			while (enumerator.MoveNext())
			{
				if (idx != 0)
					result.Append(", ");

				if (idx == MAX_ENUMERABLE_LENGTH)
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
#if NETCOREAPP2_0_OR_GREATER
			value = EscapeString(value).Replace(@"""", @"\""", StringComparison.Ordinal); // escape double quotes
#else
			value = EscapeString(value).Replace(@"""", @"\"""); // escape double quotes
#endif

			if (value.Length > MAX_STRING_LENGTH)
			{
				var displayed = value.Substring(0, MAX_STRING_LENGTH);
				return string.Format(CultureInfo.CurrentCulture, "\"{0}\"{1}", displayed, Ellipsis);
			}

			return string.Format(CultureInfo.CurrentCulture, "\"{0}\"", value);
		}

		static string FormatTupleValue(
			ITuple tupleParameter,
			int depth)
		{
			var result = new StringBuilder("Tuple (");
			var length = tupleParameter.Length;

			for (var idx = 0; idx < length; ++idx)
			{
				if (idx != 0)
					result.Append(", ");

				var value = tupleParameter[idx];
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
		/// <param name="fullTypeName">Set to <c>true</c> to include the namespace; set to <c>false</c> for just the simple type name</param>
		public static string FormatTypeName(
			Type type,
			bool fullTypeName = false)
		{
			var typeInfo = type.GetTypeInfo();
			var arraySuffix = "";

			// Deconstruct and re-construct array
			while (typeInfo.IsArray)
			{
				if (typeInfo.IsSZArrayType())
					arraySuffix += "[]";
				else
				{
					var rank = typeInfo.GetArrayRank();
					if (rank == 1)
						arraySuffix += "[*]";
					else
						arraySuffix += string.Format(CultureInfo.CurrentCulture, "[{0}]", new string(',', rank - 1));
				}

#if XUNIT_NULLABLE
				typeInfo = typeInfo.GetElementType()!.GetTypeInfo();
#else
				typeInfo = typeInfo.GetElementType().GetTypeInfo();
#endif
			}

			// Map C# built-in type names
#if XUNIT_NULLABLE
			string? result;
#else
			string result;
#endif
			var shortTypeInfo = typeInfo.IsGenericType ? typeInfo.GetGenericTypeDefinition().GetTypeInfo() : typeInfo;
			if (!TypeMappings.TryGetValue(shortTypeInfo, out result))
				result = fullTypeName ? typeInfo.FullName : typeInfo.Name;

			if (result == null)
				return typeInfo.Name;

#if NETCOREAPP2_1_OR_GREATER
			var tickIdx = result.IndexOf('`', StringComparison.Ordinal);
#else
			var tickIdx = result.IndexOf('`');
#endif
			if (tickIdx > 0)
				result = result.Substring(0, tickIdx);

			if (typeInfo.IsGenericTypeDefinition)
				result = string.Format(CultureInfo.CurrentCulture, "{0}<{1}>", result, new string(',', typeInfo.GenericTypeParameters.Length - 1));
			else if (typeInfo.IsGenericType)
			{
				if (typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
					result = FormatTypeName(typeInfo.GenericTypeArguments[0]) + "?";
				else
					result = string.Format(CultureInfo.CurrentCulture, "{0}<{1}>", result, string.Join(", ", typeInfo.GenericTypeArguments.Select(t => FormatTypeName(t))));
			}

			return result + arraySuffix;
		}

		[DynamicDependency(DynamicallyAccessedMemberTypes.PublicProperties, typeof(KeyValuePair<,>))]
		[DynamicDependency("ToString", typeof(object))]
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2070", Justification = "We can't easily annotate callers of this type to require them to preserve properties for the one type we need or the ToString method as we need to use the runtime type")]
		static string FormatValueTypeValue(
			object value,
			TypeInfo typeInfo)
		{
			if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
			{
				var k = typeInfo.GetProperty("Key")?.GetValue(value, null);
				var v = typeInfo.GetProperty("Value")?.GetValue(value, null);

				return string.Format(CultureInfo.CurrentCulture, "[{0}] = {1}", Format(k), Format(v));
			}

			return Convert.ToString(value, CultureInfo.CurrentCulture) ?? "null";
		}

		[DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ISet<>))]
		[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2075", Justification = "We can't easily annotate callers of this type to require them to preserve interfaces, so just preserve the one interface that's checked for.")]
#if XUNIT_NULLABLE
		internal static Type? GetSetElementType(object? obj)
#else
		internal static Type GetSetElementType(object obj)
#endif
		{
			if (obj == null)
				return null;

			return
				(from @interface in obj.GetType().GetTypeInfo().ImplementedInterfaces
				 where @interface.GetTypeInfo().IsGenericType
				 let genericTypeDefinition = @interface.GetGenericTypeDefinition()
				 where genericTypeDefinition == typeof(ISet<>)
				 select @interface.GetTypeInfo()).FirstOrDefault()?.GenericTypeArguments[0];
		}

		static bool IsAnonymousType(this TypeInfo typeInfo)
		{
			// There isn't a sanctioned way to do this, so we look for compiler-generated types that
			// include "AnonymousType" in their names.
			if (typeInfo.GetCustomAttribute(typeof(CompilerGeneratedAttribute)) == null)
				return false;

#if NETCOREAPP2_1_OR_GREATER
			return typeInfo.Name.Contains("AnonymousType", StringComparison.Ordinal);
#else
			return typeInfo.Name.Contains("AnonymousType");
#endif
		}

		static bool IsSZArrayType(this TypeInfo typeInfo)
		{
#if NETCOREAPP2_0_OR_GREATER
			return typeInfo.IsSZArray;
#elif XUNIT_NULLABLE
			return typeInfo == typeInfo.GetElementType()!.MakeArrayType().GetTypeInfo();
#else
			return typeInfo == typeInfo.GetElementType().MakeArrayType().GetTypeInfo();
#endif
		}

#if XUNIT_NULLABLE
		static bool SafeToMultiEnumerate(IEnumerable? collection) =>
#else
		static bool SafeToMultiEnumerate(IEnumerable collection) =>
#endif
			collection is Array ||
			collection is BitArray ||
			collection is IList ||
			collection is IDictionary ||
			GetSetElementType(collection) != null;

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
