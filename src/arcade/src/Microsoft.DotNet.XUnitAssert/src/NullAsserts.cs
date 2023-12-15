#if XUNIT_NULLABLE
#nullable enable
#endif

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
		/// <summary>
		/// Verifies that an object reference is not null.
		/// </summary>
		/// <param name="object">The object to be validated</param>
		/// <exception cref="NotNullException">Thrown when the object reference is null</exception>
#if XUNIT_NULLABLE
		public static void NotNull([NotNull] object? @object)
#else
		public static void NotNull(object @object)
#endif
		{
			if (@object == null)
				throw NotNullException.ForNullValue();
		}

		/// <summary>
		/// Verifies that a nullable struct value is not null.
		/// </summary>
		/// <typeparam name="T">The type of the struct</typeparam>
		/// <param name="value">The value to e validated</param>
		/// <returns>The non-<c>null</c> value</returns>
		/// <exception cref="NotNullException">Thrown when the value is null</exception>
#if XUNIT_NULLABLE
		public static T NotNull<T>([NotNull] T? value)
#else
		public static T NotNull<T>(T? value)
#endif
			where T : struct
		{
			if (!value.HasValue)
				throw NotNullException.ForNullStruct(typeof(T));

			return value.Value;
		}

		/// <summary>
		/// Verifies that an object reference is null.
		/// </summary>
		/// <param name="object">The object to be inspected</param>
		/// <exception cref="NullException">Thrown when the object reference is not null</exception>
#if XUNIT_NULLABLE
		public static void Null([MaybeNull] object? @object)
#else
		public static void Null(object @object)
#endif
		{
			if (@object != null)
				throw NullException.ForNonNullValue(@object);
		}

		/// <summary>
		/// Verifies that a nullable struct value is null.
		/// </summary>
		/// <param name="value">The value to be inspected</param>
		/// <exception cref="NullException">Thrown when the value is not null</exception>
		public static void Null<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(T? value)
			where T : struct
		{
			if (value.HasValue)
				throw NullException.ForNonNullStruct(typeof(T), value);
		}
	}
}
