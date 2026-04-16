#if XUNIT_AOT

#pragma warning disable CA1031 // Do not catch general exception types

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#pragma warning disable CS8767
#endif

using System;
using System.Collections;
using System.Collections.Immutable;

namespace Xunit.Sdk
{
	partial class AssertEqualityComparer
	{
		// Create a new instance each call (matching the non-AOT/reflection path behavior)
		// rather than caching in a static field. A cached static field causes a circular
		// static initialization dependency: the field initializer triggers
		// AssertEqualityComparer<object>'s static initializer, which reads the field back
		// via GetDefaultInnerComparer before it has been assigned. On Mono/WASM, this
		// causes DefaultInnerComparer to be permanently null, leading to
		// NullReferenceException when comparing value types via IStructuralEquatable.
		internal static IEqualityComparer GetDefaultComparer(Type _) =>
			new AssertEqualityComparerAdapter<object>(new AssertEqualityComparer<object>());

		internal static IEqualityComparer GetDefaultInnerComparer(Type _) =>
			new AssertEqualityComparerAdapter<object>(new AssertEqualityComparer<object>());
	}

	partial class AssertEqualityComparer<T>
	{
		public AssertEqualityResult Equals(
#if XUNIT_NULLABLE
			T? x,
			CollectionTracker? xTracker,
			T? y,
			CollectionTracker? yTracker)
#else
			T x,
			CollectionTracker xTracker,
			T y,
			CollectionTracker yTracker)
#endif
		{
			// Null?
			if (x == null && y == null)
				return AssertEqualityResult.ForResult(true, x, y);
			if (x == null || y == null)
				return AssertEqualityResult.ForResult(false, x, y);

			// If you point at the same thing, you're equal
			if (ReferenceEquals(x, y))
				return AssertEqualityResult.ForResult(true, x, y);

			// We want the inequality indices for strings
			if (x is string xString && y is string yString)
				return StringAssertEqualityComparer.Equivalent(xString, yString);

			var xType = x.GetType();
			var yType = y.GetType();

			// ImmutableArray<T> defines IEquatable<ImmutableArray<T>> in a way that isn't consistent with the
			// needs of an assertion library. https://github.com/xunit/xunit/issues/3137
			if (!xType.IsGenericType || xType.GetGenericTypeDefinition() != typeof(ImmutableArray<>))
			{
				// Implements IEquatable<T>?
				if (x is IEquatable<T> equatable)
					return AssertEqualityResult.ForResult(equatable.Equals(y), x, y);
			}

			// Special case collections (before IStructuralEquatable because arrays implement that in a way we don't want to call)
			if (xTracker != null && yTracker != null)
				return CollectionTracker.AreCollectionsEqual(xTracker, yTracker, InnerComparer, InnerComparer == DefaultInnerComparer);

			// Implements IStructuralEquatable?
			if (x is IStructuralEquatable structuralEquatable && structuralEquatable.Equals(y, innerComparer.Value))
				return AssertEqualityResult.ForResult(true, x, y);

			// Implements IComparable<T>?
			if (x is IComparable<T> comparableGeneric)
				try
				{
					return AssertEqualityResult.ForResult(comparableGeneric.CompareTo(y) == 0, x, y);
				}
				catch
				{
					// Some implementations of IComparable<T>.CompareTo throw exceptions in
					// certain situations, such as if x can't compare against y.
					// If this happens, just swallow up the exception and continue comparing.
				}

			// Implements IComparable?
			if (x is IComparable comparable)
				try
				{
					return AssertEqualityResult.ForResult(comparable.CompareTo(y) == 0, x, y);
				}
				catch
				{
					// Some implementations of IComparable.CompareTo throw exceptions in
					// certain situations, such as if x can't compare against y.
					// If this happens, just swallow up the exception and continue comparing.
				}

			// Last case, rely on object.Equals
			return AssertEqualityResult.ForResult(object.Equals(x, y), x, y);
		}
	}
}

#endif  // XUNIT_AOT
