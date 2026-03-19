#pragma warning disable CA1052 // Static holder types should be static
#pragma warning disable CA1720 // Identifier contains type name

#if XUNIT_NULLABLE
#nullable enable
#endif

#if XUNIT_POINTERS
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
#endif

using Xunit.Sdk;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit
{
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

#if XUNIT_POINTERS

		/// <summary>
		/// Verifies that an unmanaged pointer is not null.
		/// </summary>
		/// <typeparam name="T">The type of the pointer</typeparam>
		/// <param name="value">The pointer value</param>
#if XUNIT_NULLABLE
		public static unsafe void NotNull<T>([NotNull] T* value)
#else
		public static unsafe void NotNull<T>(T* value)
#endif
		{
			if (value == null)
				throw NotNullException.ForNullPointer(typeof(T));
		}

#endif  // XUNIT_POINTERS

		/// <summary>
		/// Verifies that a nullable struct value is not null.
		/// </summary>
		/// <typeparam name="T">The type of the struct</typeparam>
		/// <param name="value">The value to e validated</param>
		/// <returns>The non-<see langword="null"/> value</returns>
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
		public static void Null<T>(T? value)
			where T : struct
		{
			if (value.HasValue)
				throw NullException.ForNonNullStruct(typeof(T), value);
		}

#if XUNIT_POINTERS

		/// <summary>
		/// Verifies that an unmanaged pointer is null.
		/// </summary>
		/// <typeparam name="T">The type of the pointer</typeparam>
		/// <param name="value">The pointer value</param>
#if XUNIT_NULLABLE
		public static unsafe void Null<T>([NotNull] T* value)
#else
		public static unsafe void Null<T>(T* value)
#endif
		{
			if (value != null)
				throw NullException.ForNonNullPointer(typeof(T));
		}

#endif  // XUNIT_POINTERS
	}
}
