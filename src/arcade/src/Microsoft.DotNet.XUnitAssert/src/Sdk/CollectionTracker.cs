#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8601
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8605
#pragma warning disable CS8618
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// Base class for generic <see cref="CollectionTracker{T}"/>, which also includes some public
	/// static functionality.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	abstract class CollectionTracker : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionTracker"/> class.
		/// </summary>
		/// <param name="innerEnumerable"></param>
		/// <exception cref="ArgumentNullException"></exception>
		protected CollectionTracker(IEnumerable innerEnumerable)
		{
#if NET6_0_OR_GREATER
			ArgumentNullException.ThrowIfNull(innerEnumerable);
#else
			if (innerEnumerable == null)
				throw new ArgumentNullException(nameof(innerEnumerable));
#endif

			InnerEnumerable = innerEnumerable;
		}

		static MethodInfo openGenericCompareTypedSetsMethod =
			typeof(CollectionTracker)
				.GetRuntimeMethods()
				.Single(m => m.Name == nameof(CompareTypedSets));

		/// <summary>
		/// Gets the inner enumerable that this collection track is wrapping. This is mostly
		/// provided for simplifying other APIs which require both the tracker and the collection
		/// (for example, <see cref="AreCollectionsEqual"/>).
		/// </summary>
		protected internal IEnumerable InnerEnumerable { get; protected set; }

		/// <summary>
		/// Determine if two enumerable collections are equal. It contains logic that varies depending
		/// on the collection type (supporting arrays, dictionaries, sets, and generic enumerables).
		/// </summary>
		/// <param name="x">First value to compare</param>
		/// <param name="y">Second value to comare</param>
		/// <param name="itemComparer">The comparer used for individual item comparisons</param>
		/// <param name="isDefaultItemComparer">Pass <c>true</c> if the <paramref name="itemComparer"/> is the default item
		/// comparer from <see cref="AssertEqualityComparer{T}"/>; pass <c>false</c>, otherwise.</param>
		/// <param name="mismatchedIndex">The output mismatched item index when the collections are not equal</param>
		/// <returns>Returns <c>true</c> if the collections are equal; <c>false</c>, otherwise.</returns>
		public static bool AreCollectionsEqual(
#if XUNIT_NULLABLE
			CollectionTracker? x,
			CollectionTracker? y,
#else
			CollectionTracker x,
			CollectionTracker y,
#endif
			IEqualityComparer itemComparer,
			bool isDefaultItemComparer,
			out int? mismatchedIndex)
		{
			Assert.GuardArgumentNotNull(nameof(itemComparer), itemComparer);

			mismatchedIndex = null;

			return
#if XUNIT_AOT
				CheckIfDictionariesAreEqual(x, y, itemComparer) ??
#else
				CheckIfDictionariesAreEqual(x, y) ??
#endif
				CheckIfSetsAreEqual(x, y, isDefaultItemComparer ? null : itemComparer) ??
				CheckIfArraysAreEqual(x, y, itemComparer, isDefaultItemComparer, out mismatchedIndex) ??
				CheckIfEnumerablesAreEqual(x, y, itemComparer, isDefaultItemComparer, out mismatchedIndex);
		}

		static bool? CheckIfArraysAreEqual(
#if XUNIT_NULLABLE
			CollectionTracker? x,
			CollectionTracker? y,
#else
			CollectionTracker x,
			CollectionTracker y,
#endif
			IEqualityComparer itemComparer,
			bool isDefaultItemComparer,
			out int? mismatchedIndex)
		{
			mismatchedIndex = null;

			if (x == null || y == null)
				return null;

			var expectedArray = x.InnerEnumerable as Array;
			var actualArray = y.InnerEnumerable as Array;

			if (expectedArray == null || actualArray == null)
				return null;

			// If we have single-dimensional zero-based arrays, then we delegate to the enumerable
			// version, since that's uses the trackers and gets us the mismatch pointer.
			if (expectedArray.Rank == 1 && expectedArray.GetLowerBound(0) == 0 &&
				actualArray.Rank == 1 && actualArray.GetLowerBound(0) == 0)
				return CheckIfEnumerablesAreEqual(x, y, itemComparer, isDefaultItemComparer, out mismatchedIndex);

			if (expectedArray.Rank != actualArray.Rank)
				return false;

			// Differing bounds, aka object[2,1] vs. object[1,2]
			// You can also have non-zero-based arrays, so we don't just check lengths
			for (var rank = 0; rank < expectedArray.Rank; rank++)
				if (expectedArray.GetLowerBound(rank) != actualArray.GetLowerBound(rank) || expectedArray.GetUpperBound(rank) != actualArray.GetUpperBound(rank))
					return false;

			// Enumeration will flatten everything identically, so just enumerate at this point
			var expectedEnumerator = x.GetSafeEnumerator();
			var actualEnumerator = y.GetSafeEnumerator();

			while (true)
			{
				var hasExpected = expectedEnumerator.MoveNext();
				var hasActual = actualEnumerator.MoveNext();

				if (!hasExpected || !hasActual)
					return hasExpected == hasActual;

				if (!itemComparer.Equals(expectedEnumerator.Current, actualEnumerator.Current))
					return false;
			}
		}

		static bool? CheckIfDictionariesAreEqual(
#if XUNIT_NULLABLE
			CollectionTracker? x,
			CollectionTracker? y
#else
			CollectionTracker x,
			CollectionTracker y
#endif
#if XUNIT_AOT
			, IEqualityComparer itemComparer)
#else
		)
#endif
		{
			if (x == null || y == null)
				return null;

			var dictionaryX = x.InnerEnumerable as IDictionary;
			var dictionaryY = y.InnerEnumerable as IDictionary;

			if (dictionaryX == null || dictionaryY == null)
				return null;

			if (dictionaryX.Count != dictionaryY.Count)
				return false;

			var dictionaryYKeys = new HashSet<object>(dictionaryY.Keys.Cast<object>());

#if !XUNIT_AOT
			// We don't pass along the itemComparer from AreCollectionsEqual because we aren't directly
			// comparing the KeyValuePair<> objects. Instead we rely on Contains() on the dictionary to
			// match up keys, and then create type-appropriate comparers for the values.
#endif
			foreach (var key in dictionaryX.Keys.Cast<object>())
			{
				if (!dictionaryYKeys.Contains(key))
					return false;

				var valueX = dictionaryX[key];
				var valueY = dictionaryY[key];

				if (valueX == null)
				{
					if (valueY != null)
						return false;
				}
				else if (valueY == null)
					return false;
				else
				{
					var valueXType = valueX.GetType();
					var valueYType = valueY.GetType();

#if XUNIT_AOT
					var comparer = itemComparer;
#else
					var comparer = AssertEqualityComparer.GetDefaultComparer(valueXType == valueYType ? valueXType : typeof(object));
#endif
					if (!comparer.Equals(valueX, valueY))
						return false;
				}

				dictionaryYKeys.Remove(key);
			}

			return dictionaryYKeys.Count == 0;
		}

		static bool CheckIfEnumerablesAreEqual(
#if XUNIT_NULLABLE
			CollectionTracker? x,
			CollectionTracker? y,
#else
			CollectionTracker x,
			CollectionTracker y,
#endif
			IEqualityComparer itemComparer,
			bool isDefaultItemComparer,
			out int? mismatchIndex)
		{
			mismatchIndex = null;

			if (x == null)
				return y == null;
			if (y == null)
				return false;

			var enumeratorX = x.GetSafeEnumerator();
			var enumeratorY = y.GetSafeEnumerator();

			mismatchIndex = 0;

			while (true)
			{
				var hasNextX = enumeratorX.MoveNext();
				var hasNextY = enumeratorY.MoveNext();

				if (!hasNextX || !hasNextY)
				{
					if (hasNextX == hasNextY)
					{
						mismatchIndex = null;
						return true;
					}

					return false;
				}

				var xCurrent = enumeratorX.Current;
				var yCurrent = enumeratorY.Current;

				using (var xCurrentTracker = isDefaultItemComparer ? xCurrent.AsNonStringTracker() : null)
				using (var yCurrentTracker = isDefaultItemComparer ? yCurrent.AsNonStringTracker() : null)
				{
					if (xCurrentTracker != null && yCurrentTracker != null)
					{
						int? _;
						var innerCompare = AreCollectionsEqual(xCurrentTracker, yCurrentTracker, AssertEqualityComparer<object>.DefaultInnerComparer, true, out _);
						if (innerCompare == false)
							return false;
					}
					else if (!itemComparer.Equals(xCurrent, yCurrent))
						return false;

					mismatchIndex++;
				}
			}
		}

		static bool? CheckIfSetsAreEqual(
#if XUNIT_NULLABLE
			CollectionTracker? x,
			CollectionTracker? y,
			IEqualityComparer? itemComparer)
#else
			CollectionTracker x,
			CollectionTracker y,
			IEqualityComparer itemComparer)
#endif
		{
			if (x == null || y == null)
				return null;

			var elementTypeX = ArgumentFormatter.GetSetElementType(x.InnerEnumerable);
			var elementTypeY = ArgumentFormatter.GetSetElementType(y.InnerEnumerable);

			if (elementTypeX == null || elementTypeY == null)
				return null;

			if (elementTypeX != elementTypeY)
				return false;

#if XUNIT_AOT
			// Can't use MakeGenericType in AOT
			return CompareUntypedSets(x.InnerEnumerable, y.InnerEnumerable);
#else
			var genericCompareMethod = openGenericCompareTypedSetsMethod.MakeGenericMethod(elementTypeX);
#if XUNIT_NULLABLE
			return (bool)genericCompareMethod.Invoke(null, new object?[] { x.InnerEnumerable, y.InnerEnumerable, itemComparer })!;
#else
			return (bool)genericCompareMethod.Invoke(null, new object[] { x.InnerEnumerable, y.InnerEnumerable, itemComparer });
#endif
#endif // XUNIT_AOT
		}

		static bool CompareUntypedSets(
			IEnumerable enumX,
			IEnumerable enumY)
		{
			var setX = new HashSet<object>(enumX.Cast<object>());
			var setY = new HashSet<object>(enumY.Cast<object>());

			return setX.SetEquals(setY);
		}

		static bool CompareTypedSets<T>(
			ISet<T> setX,
			ISet<T> setY,
#if XUNIT_NULLABLE
			IEqualityComparer<T>? itemComparer)
#else
			IEqualityComparer<T> itemComparer)
#endif
		{
			if (setX.Count != setY.Count)
				return false;

			if (itemComparer != null)
			{
				setX = new HashSet<T>(setX, itemComparer);
				setY = new HashSet<T>(setY, itemComparer);
			}

			return setX.SetEquals(setY);
		}

		/// <inheritdoc/>
		public abstract void Dispose();

		/// <summary>
		/// Formats the collection when you have a mismatched index. The formatted result will be the section of the
		/// collection surrounded by the mismatched item.
		/// </summary>
		/// <param name="mismatchedIndex">The index of the mismatched item</param>
		/// <param name="pointerIndent">How many spaces into the output value the pointed-to item begins at</param>
		/// <param name="depth">The optional printing depth (1 indicates a top-level value)</param>
		/// <returns>The formatted collection</returns>
		public abstract string FormatIndexedMismatch(
			int? mismatchedIndex,
			out int? pointerIndent,
			int depth = 1);

		/// <summary>
		/// Formats the collection when you have a mismatched index. The formatted result will be the section of the
		/// collection from <paramref name="startIndex"/> to <paramref name="endIndex"/>. These indices are usually
		/// obtained by calling <see cref="GetMismatchExtents"/>.
		/// </summary>
		/// <param name="startIndex">The start index of the collection to print</param>
		/// <param name="endIndex">The end index of the collection to print</param>
		/// <param name="mismatchedIndex">The mismatched item index</param>
		/// <param name="pointerIndent">How many spaces into the output value the pointed-to item begins at</param>
		/// <param name="depth">The optional printing depth (1 indicates a top-level value)</param>
		/// <returns>The formatted collection</returns>
		public abstract string FormatIndexedMismatch(
			int startIndex,
			int endIndex,
			int? mismatchedIndex,
			out int? pointerIndent,
			int depth = 1);

		/// <summary>
		/// Formats the beginning part of the collection.
		/// </summary>
		/// <param name="depth">The optional printing depth (1 indicates a top-level value)</param>
		/// <returns>The formatted collection</returns>
		public abstract string FormatStart(int depth = 1);

		/// <summary>
		/// Gets the extents to print when you find a mismatched index, in the form of
		/// a <paramref name="startIndex"/> and <paramref name="endIndex"/>. If the mismatched
		/// index is <c>null</c>, the extents will start at index 0.
		/// </summary>
		/// <param name="mismatchedIndex">The mismatched item index</param>
		/// <param name="startIndex">The start index that should be used for printing</param>
		/// <param name="endIndex">The end index that should be used for printing</param>
		public abstract void GetMismatchExtents(
			int? mismatchedIndex,
			out int startIndex,
			out int endIndex);

		/// <summary>
		/// Gets a safe version of <see cref="IEnumerator"/> that prevents double enumeration and does all
		/// the necessary tracking required for collection formatting. Should should be the same value
		/// returned by <see cref="CollectionTracker{T}.GetEnumerator"/>, except non-generic.
		/// </summary>
		protected internal abstract IEnumerator GetSafeEnumerator();

		/// <summary>
		/// Gets the full name of the type of the element at the given index, if known.
		/// Since this uses the item cache produced by enumeration, it may return <c>null</c>
		/// when we haven't enumerated enough to see the given element, or if we enumerated
		/// so much that the item has left the cache, or if the item at the given index
		/// is <c>null</c>. It will also return <c>null</c> when the <paramref name="index"/>
		/// is <c>null</c>.
		/// </summary>
		/// <param name="index">The item index</param>
#if XUNIT_NULLABLE
		public abstract string? TypeAt(int? index);
#else
		public abstract string TypeAt(int? index);
#endif

		/// <summary>
		/// Wraps an untyped enumerable in an object-based <see cref="CollectionTracker{T}"/>.
		/// </summary>
		/// <param name="enumerable">The untyped enumerable to wrap</param>
		public static CollectionTracker<object> Wrap(IEnumerable enumerable) =>
			new CollectionTracker<object>(enumerable, enumerable.Cast<object>());
	}

	/// <summary>
	/// A utility class that can be used to wrap enumerables to prevent double enumeration.
	/// It offers the ability to safely print parts of the collection when failures are
	/// encountered, as well as some static versions of the printing functionality.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	sealed class CollectionTracker<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T> : CollectionTracker, IEnumerable<T>
	{
		const int MAX_ENUMERABLE_LENGTH_HALF = ArgumentFormatter.MAX_ENUMERABLE_LENGTH / 2;

		readonly IEnumerable<T> collection;
#pragma warning disable CA2213 // We move disposal to DisposeInternal, due to https://github.com/xunit/xunit/issues/2762
#if XUNIT_NULLABLE
		Enumerator? enumerator;
#else
		Enumerator enumerator;
#endif
#pragma warning restore CA2213

		/// <summary>
		/// INTERNAL CONSTRUCTOR. DO NOT CALL.
		/// </summary>
		internal CollectionTracker(
			IEnumerable collection,
			IEnumerable<T> castCollection) :
				base(collection)
		{
#if NET6_0_OR_GREATER
			ArgumentNullException.ThrowIfNull(castCollection);
#else
			if (castCollection == null)
				throw new ArgumentNullException(nameof(castCollection));
#endif

			this.collection = castCollection;
		}

		CollectionTracker(IEnumerable<T> collection) :
			base(collection)
		{
			this.collection = collection;
		}

		/// <summary>
		/// Gets the number of iterations that have happened so far.
		/// </summary>
		public int IterationCount =>
			enumerator == null ? 0 : enumerator.CurrentIndex + 1;

		/// <inheritdoc/>
		public override void Dispose() =>
			enumerator?.DisposeInternal();

		/// <inheritdoc/>
		public override string FormatIndexedMismatch(
			int? mismatchedIndex,
			out int? pointerIndent,
			int depth = 1)
		{
			if (depth == ArgumentFormatter.MAX_DEPTH)
			{
				pointerIndent = 1;
				return ArgumentFormatter.EllipsisInBrackets;
			}

			int startIndex;
			int endIndex;

			GetMismatchExtents(mismatchedIndex, out startIndex, out endIndex);

			return FormatIndexedMismatch(
#if XUNIT_NULLABLE
				enumerator!.CurrentItems,
#else
				enumerator.CurrentItems,
#endif
				() => enumerator.MoveNext(),
				startIndex,
				endIndex,
				mismatchedIndex,
				out pointerIndent,
				depth
			);
		}

		/// <inheritdoc/>
		public override string FormatIndexedMismatch(
			int startIndex,
			int endIndex,
			int? mismatchedIndex,
			out int? pointerIndent,
			int depth = 1)
		{
			if (enumerator == null)
				throw new InvalidOperationException("Called FormatIndexedMismatch with indices without calling GetMismatchExtents first");

			return FormatIndexedMismatch(
				enumerator.CurrentItems,
				() => enumerator.MoveNext(),
				startIndex,
				endIndex,
				mismatchedIndex,
				out pointerIndent,
				depth
			);
		}

#if XUNIT_SPAN
		/// <summary>
		/// Formats a span with a mismatched index.
		/// </summary>
		/// <param name="span">The span to be formatted</param>
		/// <param name="mismatchedIndex">The mismatched index point</param>
		/// <param name="pointerIndent">How many spaces into the output value the pointed-to item begins at</param>
		/// <param name="depth">The optional printing depth (1 indicates a top-level value)</param>
		/// <returns>The formatted span</returns>
		public static string FormatIndexedMismatch(
			ReadOnlySpan<T> span,
			int? mismatchedIndex,
			out int? pointerIndent,
			int depth = 1)
		{
			if (depth == ArgumentFormatter.MAX_DEPTH)
			{
				pointerIndent = 1;
				return ArgumentFormatter.EllipsisInBrackets;
			}

			var startIndex = Math.Max(0, (mismatchedIndex ?? 0) - MAX_ENUMERABLE_LENGTH_HALF);
			var endIndex = Math.Min(span.Length - 1, startIndex + ArgumentFormatter.MAX_ENUMERABLE_LENGTH - 1);
			startIndex = Math.Max(0, endIndex - ArgumentFormatter.MAX_ENUMERABLE_LENGTH + 1);

			var moreItemsPastEndIndex = endIndex < span.Length - 1;
			var items = new Dictionary<int, T>();

			for (var idx = startIndex; idx <= endIndex; ++idx)
				items[idx] = span[idx];

			return FormatIndexedMismatch(
				items,
				() => moreItemsPastEndIndex,
				startIndex,
				endIndex,
				mismatchedIndex,
				out pointerIndent,
				depth
			);
		}
#endif

		static string FormatIndexedMismatch(
			Dictionary<int, T> items,
			Func<bool> moreItemsPastEndIndex,
			int startIndex,
			int endIndex,
			int? mismatchedIndex,
			out int? pointerIndent,
			int depth)
		{
			pointerIndent = null;

			var printedValues = new StringBuilder("[");
			if (startIndex != 0)
				printedValues.Append(ArgumentFormatter.Ellipsis + ", ");

			for (var idx = startIndex; idx <= endIndex; ++idx)
			{
				if (idx != startIndex)
					printedValues.Append(", ");

				if (idx == mismatchedIndex)
					pointerIndent = printedValues.Length;

				printedValues.Append(ArgumentFormatter.Format(items[idx], depth));
			}

			if (moreItemsPastEndIndex())
				printedValues.Append(", " + ArgumentFormatter.Ellipsis);

			printedValues.Append(']');
			return printedValues.ToString();
		}

		/// <inheritdoc/>
		public override string FormatStart(int depth = 1)
		{
			if (depth == ArgumentFormatter.MAX_DEPTH)
				return ArgumentFormatter.EllipsisInBrackets;

			if (enumerator == null)
				enumerator = new Enumerator(collection.GetEnumerator());

			// Ensure we have already seen enough data to format
			while (enumerator.CurrentIndex <= ArgumentFormatter.MAX_ENUMERABLE_LENGTH)
				if (!enumerator.MoveNext())
					break;

			return FormatStart(enumerator.StartItems, enumerator.CurrentIndex, depth);
		}

		/// <summary>
		/// Formats the beginning part of a collection.
		/// </summary>
		/// <param name="collection">The collection to be formatted</param>
		/// <param name="depth">The optional printing depth (1 indicates a top-level value)</param>
		/// <returns>The formatted collection</returns>
		public static string FormatStart(
			IEnumerable<T> collection,
			int depth = 1)
		{
			Assert.GuardArgumentNotNull(nameof(collection), collection);

			if (depth == ArgumentFormatter.MAX_DEPTH)
				return ArgumentFormatter.EllipsisInBrackets;

			var startItems = new List<T>();
			var currentIndex = -1;
			var spanEnumerator = collection.GetEnumerator();

			// Ensure we have already seen enough data to format
			while (currentIndex <= ArgumentFormatter.MAX_ENUMERABLE_LENGTH)
			{
				if (!spanEnumerator.MoveNext())
					break;

				startItems.Add(spanEnumerator.Current);
				++currentIndex;
			}

			return FormatStart(startItems, currentIndex, depth);
		}

#if XUNIT_SPAN
		/// <summary>
		/// Formats the beginning part of a span.
		/// </summary>
		/// <param name="span">The span to be formatted</param>
		/// <param name="depth">The optional printing depth (1 indicates a top-level value)</param>
		/// <returns>The formatted span</returns>
		public static string FormatStart(
			ReadOnlySpan<T> span,
			int depth = 1)
		{
			if (depth == ArgumentFormatter.MAX_DEPTH)
				return ArgumentFormatter.EllipsisInBrackets;

			var startItems = new List<T>();
			var currentIndex = -1;
			var spanEnumerator = span.GetEnumerator();

			// Ensure we have already seen enough data to format
			while (currentIndex <= ArgumentFormatter.MAX_ENUMERABLE_LENGTH)
			{
				if (!spanEnumerator.MoveNext())
					break;

				startItems.Add(spanEnumerator.Current);
				++currentIndex;
			}

			return FormatStart(startItems, currentIndex, depth);
		}
#endif

		static string FormatStart(
			List<T> items,
			int currentIndex,
			int depth)
		{
			var printedValues = new StringBuilder("[");
			var printLength = Math.Min(currentIndex + 1, ArgumentFormatter.MAX_ENUMERABLE_LENGTH);

			for (var idx = 0; idx < printLength; ++idx)
			{
				if (idx != 0)
					printedValues.Append(", ");

				printedValues.Append(ArgumentFormatter.Format(items[idx], depth));
			}

			if (currentIndex >= ArgumentFormatter.MAX_ENUMERABLE_LENGTH)
				printedValues.Append(", " + ArgumentFormatter.Ellipsis);

			printedValues.Append(']');
			return printedValues.ToString();
		}

		/// <inheritdoc/>
		public IEnumerator<T> GetEnumerator()
		{
			if (enumerator != null)
				throw new InvalidOperationException("Multiple enumeration is not supported");

			enumerator = new Enumerator(collection.GetEnumerator());
			return enumerator;
		}

		IEnumerator IEnumerable.GetEnumerator() =>
			GetEnumerator();

		/// <inheritdoc/>
		protected internal override IEnumerator GetSafeEnumerator() =>
			GetEnumerator();

		/// <inheritdoc/>
		public override void GetMismatchExtents(
			int? mismatchedIndex,
			out int startIndex,
			out int endIndex)
		{
			if (enumerator == null)
				enumerator = new Enumerator(collection.GetEnumerator());

			startIndex = Math.Max(0, (mismatchedIndex ?? 0) - MAX_ENUMERABLE_LENGTH_HALF);
			endIndex = startIndex + ArgumentFormatter.MAX_ENUMERABLE_LENGTH - 1;

			// Make sure our window starts with startIndex and ends with endIndex, as appropriate
			while (enumerator.CurrentIndex < endIndex)
				if (!enumerator.MoveNext())
					break;

			endIndex = enumerator.CurrentIndex;
			startIndex = Math.Max(0, endIndex - ArgumentFormatter.MAX_ENUMERABLE_LENGTH + 1);
		}

		/// <inheritdoc/>
#if XUNIT_NULLABLE
		public override string? TypeAt(int? index)
#else
		public override string TypeAt(int? index)
#endif
		{
			if (enumerator == null || !index.HasValue)
				return null;

#if XUNIT_NULLABLE
			T? item;
#else
			T item;
#endif
			if (!enumerator.TryGetCurrentItemAt(index.Value, out item))
				return null;

			return item?.GetType().FullName;
		}

		/// <summary>
		/// Wraps the given collection inside of a <see cref="CollectionTracker{T}"/>.
		/// </summary>
		/// <param name="collection">The collection to be wrapped</param>
		public static CollectionTracker<T> Wrap(IEnumerable<T> collection) =>
			new CollectionTracker<T>(collection);

		sealed class Enumerator : IEnumerator<T>
		{
			int currentItemsLastInsertionIndex = -1;
			T[] currentItemsRingBuffer = new T[ArgumentFormatter.MAX_ENUMERABLE_LENGTH];
			readonly IEnumerator<T> innerEnumerator;

			public Enumerator(IEnumerator<T> innerEnumerator)
			{
				this.innerEnumerator = innerEnumerator;
			}

			public T Current =>
				innerEnumerator.Current;

#if XUNIT_NULLABLE
			object? IEnumerator.Current =>
#else
			object IEnumerator.Current =>
#endif
				Current;

			public int CurrentIndex { get; private set; } = -1;

			public Dictionary<int, T> CurrentItems
			{
				get
				{
					var result = new Dictionary<int, T>();

					if (CurrentIndex > -1)
					{
						var itemIndex = Math.Max(0, CurrentIndex - ArgumentFormatter.MAX_ENUMERABLE_LENGTH + 1);

						var indexInRingBuffer = (currentItemsLastInsertionIndex - CurrentIndex + itemIndex) % ArgumentFormatter.MAX_ENUMERABLE_LENGTH;
						if (indexInRingBuffer < 0)
							indexInRingBuffer += ArgumentFormatter.MAX_ENUMERABLE_LENGTH;

						while (itemIndex <= CurrentIndex)
						{
							result[itemIndex] = currentItemsRingBuffer[indexInRingBuffer];

							++itemIndex;
							indexInRingBuffer = (indexInRingBuffer + 1) % ArgumentFormatter.MAX_ENUMERABLE_LENGTH;
						}
					}

					return result;
				}
			}

			public List<T> StartItems { get; } = new List<T>();

			public void Dispose()
			{ }

			public void DisposeInternal()
			{
				innerEnumerator.Dispose();
			}

			public bool MoveNext()
			{
				if (!innerEnumerator.MoveNext())
					return false;

				CurrentIndex++;
				var current = innerEnumerator.Current;

				// Keep (MAX_ENUMERABLE_LENGTH + 1) items here, so we can
				// print the start of the collection when lengths differ
				if (CurrentIndex <= ArgumentFormatter.MAX_ENUMERABLE_LENGTH)
					StartItems.Add(current);

				// Keep a ring buffer filled with the most recent MAX_ENUMERABLE_LENGTH items
				// so we can print out the items when we've found a bad index
				currentItemsLastInsertionIndex = (currentItemsLastInsertionIndex + 1) % ArgumentFormatter.MAX_ENUMERABLE_LENGTH;
				currentItemsRingBuffer[currentItemsLastInsertionIndex] = current;

				return true;
			}

			public void Reset()
			{
				innerEnumerator.Reset();

				CurrentIndex = -1;
				currentItemsLastInsertionIndex = -1;
				StartItems.Clear();
			}

			public bool TryGetCurrentItemAt(
				int index,
#if XUNIT_NULLABLE
				[MaybeNullWhen(false)] out T item)
#else
				out T item)
#endif
			{
				item = default(T);

				if (index < 0 || index <= CurrentIndex - ArgumentFormatter.MAX_ENUMERABLE_LENGTH || index > CurrentIndex)
					return false;

				var indexInRingBuffer = (currentItemsLastInsertionIndex - CurrentIndex + index) % ArgumentFormatter.MAX_ENUMERABLE_LENGTH;
				if (indexInRingBuffer < 0)
					indexInRingBuffer += ArgumentFormatter.MAX_ENUMERABLE_LENGTH;

				item = currentItemsRingBuffer[indexInRingBuffer];
				return true;
			}
		}
	}
}
