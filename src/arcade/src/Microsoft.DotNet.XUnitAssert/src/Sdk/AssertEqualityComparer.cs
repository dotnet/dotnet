#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0063 // Use simple 'using' statement
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0290 // Use primary constructor

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#pragma warning disable CS8625
#pragma warning disable CS8767
#endif

using System;
using System.Collections;
using System.Collections.Generic;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit.Sdk
{
	static partial class AssertEqualityComparer
	{
		/// <summary>
		/// This exception is thrown when an operation failure has occured during equality comparison operations.
		/// This generally indicates that a necessary pre-condition was not met for comparison operations to succeed.
		/// </summary>
		public sealed class OperationalFailureException : Exception
		{
			OperationalFailureException(string message) :
				base(message)
			{ }

			/// <summary>
			/// Gets an exception that indicates that GetHashCode was called on <see cref="AssertEqualityComparer{T}.FuncEqualityComparer"/>
			/// which usually indicates that an item comparison function was used to try to compare two hash sets.
			/// </summary>
			public static OperationalFailureException ForIllegalGetHashCode() =>
				new OperationalFailureException("During comparison of two collections, GetHashCode was called, but only a comparison function was provided. This typically indicates trying to compare two sets with an item comparison function, which is not supported. For more information, see https://xunit.net/docs/hash-sets-vs-linear-containers");
		}
	}

	/// <summary>
	/// Default implementation of <see cref="IAssertEqualityComparer{T}" /> used by the assertion library.
	/// </summary>
	/// <typeparam name="T">The type that is being compared.</typeparam>
	sealed partial class AssertEqualityComparer<T> : IAssertEqualityComparer<T>
	{
		internal static readonly IEqualityComparer DefaultInnerComparer = AssertEqualityComparer.GetDefaultInnerComparer(typeof(T));

		readonly Lazy<IEqualityComparer> innerComparer;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssertEqualityComparer{T}" /> class.
		/// </summary>
		/// <param name="innerComparer">The inner comparer to be used when the compared objects are enumerable.</param>
#if XUNIT_NULLABLE
		public AssertEqualityComparer(IEqualityComparer? innerComparer = null)
#else
		public AssertEqualityComparer(IEqualityComparer innerComparer = null)
#endif
		{
			// Use a thunk to delay evaluation of DefaultInnerComparer
			this.innerComparer = new Lazy<IEqualityComparer>(() => innerComparer ?? DefaultInnerComparer);
		}

		public IEqualityComparer InnerComparer =>
			innerComparer.Value;

		/// <inheritdoc/>
		public bool Equals(
#if XUNIT_NULLABLE
			T? x,
			T? y)
#else
			T x,
			T y)
#endif
		{
			using (var xTracker = x.AsNonStringTracker())
			using (var yTracker = y.AsNonStringTracker())
				return Equals(x, xTracker, y, yTracker).Equal;
		}

#if XUNIT_NULLABLE
		public static IEqualityComparer<T?> FromComparer(Func<T, T, bool> comparer) =>
#else
		public static IEqualityComparer<T> FromComparer(Func<T, T, bool> comparer) =>
#endif
			new FuncEqualityComparer(comparer);

		/// <inheritdoc/>
		public int GetHashCode(T obj) =>
			innerComparer.Value.GetHashCode(GuardArgumentNotNull(nameof(obj), obj));

		/// <summary/>
#if XUNIT_NULLABLE
		[return: NotNull]
#endif
		internal static TArg GuardArgumentNotNull<TArg>(
			string argName,
#if XUNIT_NULLABLE
			[NotNull] TArg? argValue)
#else
			TArg argValue)
#endif
		{
			if (argValue == null)
				throw new ArgumentNullException(argName.TrimStart('@'));

			return argValue;
		}

#if XUNIT_NULLABLE
		sealed class FuncEqualityComparer : IEqualityComparer<T?>
#else
		sealed class FuncEqualityComparer : IEqualityComparer<T>
#endif
		{
			readonly Func<T, T, bool> comparer;

			public FuncEqualityComparer(Func<T, T, bool> comparer) =>
				this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

			public bool Equals(
#if XUNIT_NULLABLE
				T? x,
				T? y)
#else
				T x,
				T y)
#endif
			{
				if (x == null)
					return y == null;

				if (y == null)
					return false;

				return comparer(x, y);
			}

#if XUNIT_NULLABLE
			public int GetHashCode(T? obj)
#else
			public int GetHashCode(T obj)
#endif
			{
#pragma warning disable CA1065  // This method should never be called, and this exception is a way to highlight if it does
				throw AssertEqualityComparer.OperationalFailureException.ForIllegalGetHashCode();
#pragma warning restore CA1065
			}
		}
	}
}
