#if !XUNIT_AOT

#pragma warning disable IDE0300 // Simplify collection initialization
#pragma warning disable IDE0301 // Simplify collection initialization
#pragma warning disable IDE0305 // Simplify collection initialization
#pragma warning disable CA1810 // Initialize reference type static fields inline

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8600
#pragma warning disable CS8601
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8618
#pragma warning disable CS8625
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#if XUNIT_ARGUMENTFORMATTER_PRIVATE
namespace Xunit.Internal
#else
namespace Xunit.Sdk
#endif
{
	partial class ArgumentFormatter
	{
		static readonly object[] EmptyObjects = Array.Empty<object>();
		static readonly Type[] EmptyTypes = Array.Empty<Type>();

#if !NET8_0_OR_GREATER

#if XUNIT_NULLABLE
		static readonly PropertyInfo? tupleIndexer;
		static readonly Type? tupleInterfaceType;
		static readonly PropertyInfo? tupleLength;
#else
		static readonly PropertyInfo tupleIndexer;
		static readonly Type tupleInterfaceType;
		static readonly PropertyInfo tupleLength;
#endif

		static ArgumentFormatter()
		{
			tupleInterfaceType = Type.GetType("System.Runtime.CompilerServices.ITuple");

			if (tupleInterfaceType != null)
			{
				tupleIndexer = tupleInterfaceType.GetRuntimeProperty("Item");
				tupleLength = tupleInterfaceType.GetRuntimeProperty("Length");
			}

			if (tupleIndexer == null || tupleLength == null)
				tupleInterfaceType = null;
		}

#endif  // !NET8_0_OR_GREATER

		static string FormatComplexValue(
			object value,
			int depth,
			Type type,
			bool isAnonymousType)
		{
			// For objects which implement a custom ToString method, just call that
			if (!isAnonymousType)
			{
				var toString = type.GetRuntimeMethod("ToString", EmptyTypes);
				if (toString != null && toString.DeclaringType != typeof(object))
					return toString.Invoke(value, EmptyObjects) as string ?? "null";
			}

			var typeName = isAnonymousType ? "" : type.Name + " ";

			if (depth > MaxObjectDepth)
				return string.Format(CultureInfo.CurrentCulture, "{0}{{ {1} }}", typeName, Ellipsis);

			var fields =
				type
					.GetRuntimeFields()
					.Where(f => f.IsPublic && !f.IsStatic)
					.Select(f => new { name = f.Name, value = WrapAndGetFormattedValue(() => f.GetValue(value), depth + 1) });

			var properties =
				type
					.GetRuntimeProperties()
					.Where(p => p.GetMethod != null && p.GetMethod.IsPublic && !p.GetMethod.IsStatic)
					.Select(p => new { name = p.Name, value = WrapAndGetFormattedValue(() => p.GetValue(value), depth + 1) });

			var parameters =
				MaxObjectMemberCount == int.MaxValue
					? fields.Concat(properties).OrderBy(p => p.name).ToList()
					: fields.Concat(properties).OrderBy(p => p.name).Take(MaxObjectMemberCount + 1).ToList();

			if (parameters.Count == 0)
				return string.Format(CultureInfo.CurrentCulture, "{0}{{ }}", typeName);

			var formattedParameters = string.Join(", ", parameters.Take(MaxObjectMemberCount).Select(p => string.Format(CultureInfo.CurrentCulture, "{0} = {1}", p.name, p.value)));

			if (parameters.Count > MaxObjectMemberCount)
				formattedParameters += ", " + Ellipsis;

			return string.Format(CultureInfo.CurrentCulture, "{0}{{ {1} }}", typeName, formattedParameters);
		}

		static string FormatValueTypeValue(
			object value,
			Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
			{
				var k = type.GetProperty("Key")?.GetValue(value, null);
				var v = type.GetProperty("Value")?.GetValue(value, null);

				return string.Format(CultureInfo.CurrentCulture, "[{0}] = {1}", Format(k), Format(v));
			}

			return Convert.ToString(value, CultureInfo.CurrentCulture) ?? "null";
		}

#if XUNIT_NULLABLE
		static string? GetGroupingKeyPrefix(IEnumerable enumerable)
#else
		static string GetGroupingKeyPrefix(IEnumerable enumerable)
#endif
		{
			var groupingTypes = GetGroupingTypes(enumerable);
			if (groupingTypes == null)
				return null;

			var groupingInterface = typeof(IGrouping<,>).MakeGenericType(groupingTypes);
			var key = groupingInterface.GetRuntimeProperty("Key")?.GetValue(enumerable);
			return string.Format(CultureInfo.CurrentCulture, "[{0}] = ", key?.ToString() ?? "null");
		}

#if XUNIT_NULLABLE
		internal static Type[]? GetGroupingTypes(object? obj)
#else
		internal static Type[] GetGroupingTypes(object obj)
#endif
		{
			if (obj == null)
				return null;

			return
				(from @interface in obj.GetType().GetInterfaces()
				 where @interface.IsGenericType
				 let genericTypeDefinition = @interface.GetGenericTypeDefinition()
				 where genericTypeDefinition == typeof(IGrouping<,>)
				 select @interface).FirstOrDefault()?.GenericTypeArguments;
		}

#if XUNIT_NULLABLE
		internal static Type? GetSetElementType(object? obj)
#else
		internal static Type GetSetElementType(object obj)
#endif
		{
			if (obj == null)
				return null;

			return
				(from @interface in obj.GetType().GetInterfaces()
				 where @interface.IsGenericType
				 let genericTypeDefinition = @interface.GetGenericTypeDefinition()
				 where genericTypeDefinition == typeof(ISet<>)
				 select @interface).FirstOrDefault()?.GenericTypeArguments[0];
		}

		static bool IsEnumerableOfGrouping(IEnumerable collection)
		{
			var genericEnumerableType =
				(from @interface in collection.GetType().GetInterfaces()
				 where @interface.IsGenericType
				 let genericTypeDefinition = @interface.GetGenericTypeDefinition()
				 where genericTypeDefinition == typeof(IEnumerable<>)
				 select @interface).FirstOrDefault()?.GenericTypeArguments[0];

			if (genericEnumerableType == null)
				return false;

			return
				genericEnumerableType
					.GetInterfaces()
					.Concat(new[] { genericEnumerableType })
					.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGrouping<,>));
		}
	}
}

#endif  // !XUNIT_AOT
