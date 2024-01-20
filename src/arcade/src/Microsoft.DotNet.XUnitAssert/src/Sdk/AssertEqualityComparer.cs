#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8605
#pragma warning disable CS8618
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

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit.Sdk
{
	static class AssertEqualityComparer
	{
#if !XUNIT_AOT // not supported on AOT due to MakeGenericType
		static readonly ConcurrentDictionary<Type, IEqualityComparer> cachedDefaultComparers = new ConcurrentDictionary<Type, IEqualityComparer>();
		static readonly ConcurrentDictionary<Type, IEqualityComparer> cachedDefaultInnerComparers = new ConcurrentDictionary<Type, IEqualityComparer>();
#if XUNIT_NULLABLE
		static readonly object?[] singleNullObject = new object?[] { null };
#else
		static readonly object[] singleNullObject = new object[] { null };
#endif

		/// <summary>
		/// Gets the default comparer to be used for the provided <paramref name="type"/> when a custom one
		/// has not been provided. Creates an instance of <see cref="AssertEqualityComparer{T}"/> wrapped
		/// by <see cref="AssertEqualityComparerAdapter{T}"/>.
		/// </summary>
		/// <param name="type">The type to be compared</param>
		internal static IEqualityComparer GetDefaultComparer(Type type) =>
			cachedDefaultComparers.GetOrAdd(type, itemType =>
			{
				var comparerType = typeof(AssertEqualityComparer<>).MakeGenericType(itemType);
				var comparer = Activator.CreateInstance(comparerType, singleNullObject);
				if (comparer == null)
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not create instance of AssertEqualityComparer<{0}>", itemType.FullName ?? itemType.Name));

				var wrapperType = typeof(AssertEqualityComparerAdapter<>).MakeGenericType(itemType);
				var result = Activator.CreateInstance(wrapperType, new object[] { comparer }) as IEqualityComparer;
				if (result == null)
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Could not create instance of AssertEqualityComparerAdapter<{0}>", itemType.FullName ?? itemType.Name));

				return result;
			});

		/// <summary>
		/// Gets the default comparer to be used as an inner comparer for the provided <paramref name="type"/>
		/// when a custom one has not been provided. For non-collections, this defaults to an <see cref="object"/>-based
		/// comparer; for collections, this creates an inner comparer based on the item type in the collection.
		/// </summary>
		/// <param name="type">The type to create an inner comparer for</param>
		internal static IEqualityComparer GetDefaultInnerComparer(Type type) =>
			cachedDefaultInnerComparers.GetOrAdd(type, t =>
			{
				var innerType = typeof(object);

				// string is enumerable, but we don't treat it like a collection
				if (t != typeof(string))
				{
					var enumerableOfT =
						t.GetTypeInfo()
							.ImplementedInterfaces
							.Select(i => i.GetTypeInfo())
							.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

					if (enumerableOfT != null)
						innerType = enumerableOfT.GenericTypeArguments[0];
				}

				return GetDefaultComparer(innerType);
			});
#endif // !XUNIT_AOT

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
	/// Default implementation of <see cref="IEqualityComparer{T}"/> used by the xUnit.net equality assertions.
	/// </summary>
	/// <typeparam name="T">The type that is being compared.</typeparam>
	sealed class AssertEqualityComparer<
			[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces |
				DynamicallyAccessedMemberTypes.PublicFields |
				DynamicallyAccessedMemberTypes.NonPublicFields |
				DynamicallyAccessedMemberTypes.PublicProperties |
				DynamicallyAccessedMemberTypes.NonPublicProperties |
				DynamicallyAccessedMemberTypes.PublicMethods)] T> : IEqualityComparer<T>
	{
#if XUNIT_AOT
		internal static readonly IEqualityComparer DefaultInnerComparer = new AssertEqualityComparerAdapter<object>(new AssertEqualityComparer<object>());
#else
		internal static readonly IEqualityComparer DefaultInnerComparer = AssertEqualityComparer.GetDefaultInnerComparer(typeof(T));
#endif

		static readonly ConcurrentDictionary<Type, TypeInfo> cacheOfIComparableOfT = new ConcurrentDictionary<Type, TypeInfo>();
		static readonly ConcurrentDictionary<Type, TypeInfo> cacheOfIEquatableOfT = new ConcurrentDictionary<Type, TypeInfo>();
		readonly Lazy<IEqualityComparer> innerComparer;
		static readonly Type typeKeyValuePair = typeof(KeyValuePair<,>);

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
			this.innerComparer = new Lazy<IEqualityComparer>(() => innerComparer ?? AssertEqualityComparer<T>.DefaultInnerComparer);
		}

		public IEqualityComparer InnerComparer =>
			innerComparer.Value;

		/// <inheritdoc/>
		public bool Equals(
#if XUNIT_NULLABLE
			[AllowNull] T x,
			[AllowNull] T y)
#else
			T x,
			T y)
#endif
		{
			int? _;

#if XUNIT_FRAMEWORK
			return Equals(x, y, out _);
#else
			using (var xTracker = x.AsNonStringTracker())
			using (var yTracker = y.AsNonStringTracker())
				return Equals(x, xTracker, y, yTracker, out _);
#endif
		}

		internal bool Equals(
#if XUNIT_NULLABLE
			[AllowNull] T x,
#if !XUNIT_FRAMEWORK
			CollectionTracker? xTracker,
#endif
			[AllowNull] T y,
#if !XUNIT_FRAMEWORK
			CollectionTracker? yTracker,
#endif
#else
			T x,
#if !XUNIT_FRAMEWORK
			CollectionTracker xTracker,
#endif
			T y,
#if !XUNIT_FRAMEWORK
			CollectionTracker yTracker,
#endif
#endif
			out int? mismatchedIndex)
		{
			mismatchedIndex = null;

			// Null?
			if (x == null && y == null)
				return true;
			if (x == null || y == null)
				return false;

			// Implements IEquatable<T>?
			var equatable = x as IEquatable<T>;
			if (equatable != null)
				return equatable.Equals(y);

#if !XUNIT_AOT
			var xType = x.GetType();
			var yType = y.GetType();
			var xTypeInfo = xType.GetTypeInfo();

			// Implements IEquatable<typeof(y)>?
			// Not supported on AOT due to MakeGenericType
			if (xType != yType)
			{
				var iequatableY = cacheOfIEquatableOfT.GetOrAdd(yType, (t) => typeof(IEquatable<>).MakeGenericType(t).GetTypeInfo());
				if (iequatableY.IsAssignableFrom(xTypeInfo))
				{
					var equalsMethod = iequatableY.GetDeclaredMethod(nameof(IEquatable<T>.Equals));
					if (equalsMethod == null)
						return false;

#if XUNIT_NULLABLE
					return equalsMethod.Invoke(x, new object[] { y }) is true;
#else
					return (bool)equalsMethod.Invoke(x, new object[] { y });
#endif
				}
			}
#endif // !XUNIT_AOT

#if !XUNIT_FRAMEWORK
			// Special case collections (before IStructuralEquatable because arrays implement that in a way we don't want to call)
			if (xTracker != null && yTracker != null)
				return CollectionTracker.AreCollectionsEqual(xTracker, yTracker, InnerComparer, InnerComparer == DefaultInnerComparer, out mismatchedIndex);
#endif

			// Implements IStructuralEquatable?
			var structuralEquatable = x as IStructuralEquatable;
			if (structuralEquatable != null && structuralEquatable.Equals(y, new TypeErasedEqualityComparer(innerComparer.Value)))
				return true;

			// Implements IComparable<T>?
			var comparableGeneric = x as IComparable<T>;
			if (comparableGeneric != null)
			{
				try
				{
					return comparableGeneric.CompareTo(y) == 0;
				}
				catch
				{
					// Some implementations of IComparable<T>.CompareTo throw exceptions in
					// certain situations, such as if x can't compare against y.
					// If this happens, just swallow up the exception and continue comparing.
				}
			}

#if !XUNIT_AOT
			// Implements IComparable<typeof(y)>?
			// Not supported on AOT due to MakeGenericType
			if (xType != yType)
			{
				var icomparableY = cacheOfIComparableOfT.GetOrAdd(yType, (t) => typeof(IComparable<>).MakeGenericType(t).GetTypeInfo());
				if (icomparableY.IsAssignableFrom(xTypeInfo))
				{
					var compareToMethod = icomparableY.GetDeclaredMethod(nameof(IComparable<T>.CompareTo));
					if (compareToMethod == null)
						return false;

					try
					{
#if XUNIT_NULLABLE
						return compareToMethod.Invoke(x, new object[] { y }) is 0;
#else
						return (int)compareToMethod.Invoke(x, new object[] { y }) == 0;
#endif
					}
					catch
					{
						// Some implementations of IComparable.CompareTo throw exceptions in
						// certain situations, such as if x can't compare against y.
						// If this happens, just swallow up the exception and continue comparing.
					}
				}
			}
#endif // !XUNIT_AOT

			// Implements IComparable?
			var comparable = x as IComparable;
			if (comparable != null)
			{
				try
				{
					return comparable.CompareTo(y) == 0;
				}
				catch
				{
					// Some implementations of IComparable.CompareTo throw exceptions in
					// certain situations, such as if x can't compare against y.
					// If this happens, just swallow up the exception and continue comparing.
				}
			}

			// Special case KeyValuePair<K,V>
			if (typeof(T).IsConstructedGenericType &&
				typeof(T).GetGenericTypeDefinition() == typeKeyValuePair)
			{
#if XUNIT_AOT
				var xKey = typeof(T).GetRuntimeProperty("Key")?.GetValue(x);
				var yKey = typeof(T).GetRuntimeProperty("Key")?.GetValue(y);
#else
				var xKey = xType.GetRuntimeProperty("Key")?.GetValue(x);
				var yKey = yType.GetRuntimeProperty("Key")?.GetValue(y);
#endif

				if (xKey == null)
				{
					if (yKey != null)
						return false;
				}
				else
				{
					var xKeyType = xKey.GetType();
					var yKeyType = yKey?.GetType();

#if XUNIT_AOT
					var keyComparer = innerComparer.Value;
#else
					var keyComparer = AssertEqualityComparer.GetDefaultComparer(xKeyType == yKeyType ? xKeyType : typeof(object));
#endif
					if (!keyComparer.Equals(xKey, yKey))
						return false;
				}

#if XUNIT_AOT
				var xValue = typeof(T).GetRuntimeProperty("Value")?.GetValue(x);
				var yValue = typeof(T).GetRuntimeProperty("Value")?.GetValue(y);
#else
				var xValue = xType.GetRuntimeProperty("Value")?.GetValue(x);
				var yValue = yType.GetRuntimeProperty("Value")?.GetValue(y);
#endif

				if (xValue == null)
					return yValue == null;

				var xValueType = xValue.GetType();
				var yValueType = yValue?.GetType();

#if XUNIT_AOT
				var valueComparer = innerComparer.Value;
#else
				var valueComparer = AssertEqualityComparer.GetDefaultComparer(xValueType == yValueType ? xValueType : typeof(object));
#endif
				return valueComparer.Equals(xValue, yValue);
			}

			// Last case, rely on object.Equals
			return object.Equals(x, y);
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

#if XUNIT_NULLABLE
		sealed class FuncEqualityComparer : IEqualityComparer<T?>
#else
		sealed class FuncEqualityComparer : IEqualityComparer<T>
#endif
		{
			readonly Func<T, T, bool> comparer;

			public FuncEqualityComparer(Func<T, T, bool> comparer)
			{
#if NET6_0_OR_GREATER
				ArgumentNullException.ThrowIfNull(comparer);
#else
				if (comparer == null)
					throw new ArgumentNullException(nameof(comparer));
#endif

				this.comparer = comparer;
			}

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
				throw AssertEqualityComparer.OperationalFailureException.ForIllegalGetHashCode();
			}
		}

		sealed class TypeErasedEqualityComparer : IEqualityComparer
		{
			readonly IEqualityComparer innerComparer;

			public TypeErasedEqualityComparer(IEqualityComparer innerComparer)
			{
				this.innerComparer = innerComparer;
			}

#if !XUNIT_AOT
#if XUNIT_NULLABLE
			static MethodInfo? s_equalsMethod;
#else
			static MethodInfo s_equalsMethod;
#endif
#endif // XUNIT_AOT

			public new bool Equals(
#if XUNIT_NULLABLE
				object? x,
				object? y)
#else
				object x,
				object y)
#endif
			{
				if (x == null)
					return y == null;
				if (y == null)
					return false;

#if XUNIT_AOT
				// Can't use MakeGenericType, have to use object
				return EqualsGeneric(x, y);
#else
				// Delegate checking of whether two objects are equal to AssertEqualityComparer.
				// To get the best result out of AssertEqualityComparer, we attempt to specialize the
				// comparer for the objects that we are checking.
				// If the objects are the same, great! If not, assume they are objects.
				// This is more naive than the C# compiler which tries to see if they share any interfaces
				// etc. but that's likely overkill here as AssertEqualityComparer<object> is smart enough.
				Type objectType = x.GetType() == y.GetType() ? x.GetType() : typeof(object);

				// Lazily initialize and cache the EqualsGeneric<U> method.
				if (s_equalsMethod == null)
				{
					s_equalsMethod = typeof(TypeErasedEqualityComparer).GetTypeInfo().GetDeclaredMethod(nameof(EqualsGeneric));
					if (s_equalsMethod == null)
						return false;
				}

#if XUNIT_NULLABLE
				return s_equalsMethod.MakeGenericMethod(objectType).Invoke(this, new object[] { x, y }) is true;
#else
				return (bool)s_equalsMethod.MakeGenericMethod(objectType).Invoke(this, new object[] { x, y });
#endif // XUNIT_NULLABLE
#endif // XUNIT_AOT
			}

			bool EqualsGeneric<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.Interfaces
					| DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] U>(
				U x,
				U y) =>
					new AssertEqualityComparer<U>(innerComparer: innerComparer).Equals(x, y);

			public int GetHashCode(object obj) =>
				GuardArgumentNotNull(nameof(obj), obj).GetHashCode();
		}

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
	}
}
