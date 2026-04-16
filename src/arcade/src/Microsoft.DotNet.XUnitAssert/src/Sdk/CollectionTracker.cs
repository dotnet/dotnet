#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable CA1508 // Avoid dead conditional code
#pragma warning disable CA2213 // We move disposal to DisposeInternal, due to https://github.com/xunit/xunit/issues/2762
#pragma warning disable IDE0019 // Use pattern matching
#pragma warning disable IDE0028 // Simplify collection initialization
#pragma warning disable IDE0063 // Use simple 'using' statement
#pragma warning disable IDE0074 // Use compound assignment
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0290 // Use primary constructor
#pragma warning disable IDE0300 // Simplify collection initialization

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8601
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8618
#pragma warning disable CS8625
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	abstract partial class CollectionTracker : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionTracker"/> class.
		/// </summary>
		/// <param name="innerEnumerable"></param>
		/// <exception cref="ArgumentNullException"></exception>
		protected CollectionTracker(IEnumerable innerEnumerable) =>
			InnerEnumerable = innerEnumerable ?? throw new ArgumentNullException(nameof(innerEnumerable));

		/// <summary>
		/// Gets the inner enumerable that this collection track is wrapping. This is mostly
		/// provided for simplifying other APIs which require both the tracker and the collection
		/// (for example, <see cref="AreCollectionsEqual(CollectionTracker?, CollectionTracker?, IEqualityComparer, bool)"/>).
		/// </summary>
		protected internal IEnumerable InnerEnumerable { get; protected set; }

		/// <summary>
		/// Determine if two enumerable collections are equal. It contains logic that varies depending
		/// on the collection type (supporting arrays, dictionaries, sets, and generic enumerables).
		/// </summary>
		/// <param name="x">First value to compare</param>
		/// <param name="y">Second value to comare</param>
		/// <param name="itemComparer">The comparer used for individual item comparisons</param>
		/// <param name="isDefaultItemComparer">Pass <see langword="true"/> if the <paramref name="itemComparer"/> is the default item
		/// comparer from <see cref="AssertEqualityComparer{T}"/>; pass <see langword="false"/>, otherwise.</param>
		/// <returns>Returns <see langword="true"/> if the collections are equal; <see langword="false"/>, otherwise.</returns>
		public static AssertEqualityResult AreCollectionsEqual(
#if XUNIT_NULLABLE
			CollectionTracker? x,
			CollectionTracker? y,
#else
			CollectionTracker x,
			CollectionTracker y,
#endif
			IEqualityComparer itemComparer,
			bool isDefaultItemComparer)
		{
			Assert.GuardArgumentNotNull(nameof(itemComparer), itemComparer);

			try
			{
				return
					CheckIfDictionariesAreEqual(x, y) ??
					CheckIfSetsAreEqual(x, y, isDefaultItemComparer ? null : itemComparer) ??
					CheckIfArraysAreEqual(x, y, itemComparer, isDefaultItemComparer) ??
					CheckIfEnumerablesAreEqual(x, y, itemComparer, isDefaultItemComparer);
			}
			catch (Exception ex)
			{
				return AssertEqualityResult.ForResult(false, x?.InnerEnumerable, y?.InnerEnumerable, ex);
			}
		}

#if XUNIT_NULLABLE
		static AssertEqualityResult? CheckIfArraysAreEqual(
			CollectionTracker? x,
			CollectionTracker? y,
#else
		static AssertEqualityResult CheckIfArraysAreEqual(
			CollectionTracker x,
			CollectionTracker y,
#endif
			IEqualityComparer itemComparer,
			bool isDefaultItemComparer)
		{
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
				return CheckIfEnumerablesAreEqual(x, y, itemComparer, isDefaultItemComparer);

			if (expectedArray.Rank != actualArray.Rank)
				return AssertEqualityResult.ForResult(false, x.InnerEnumerable, y.InnerEnumerable);

			// Differing bounds, aka object[2,1] vs. object[1,2]
			// You can also have non-zero-based arrays, so we don't just check lengths
			for (var rank = 0; rank < expectedArray.Rank; rank++)
				if (expectedArray.GetLowerBound(rank) != actualArray.GetLowerBound(rank) || expectedArray.GetUpperBound(rank) != actualArray.GetUpperBound(rank))
					return AssertEqualityResult.ForResult(false, x.InnerEnumerable, y.InnerEnumerable);

			// Enumeration will flatten everything identically, so just enumerate at this point
			var expectedEnumerator = x.GetSafeEnumerator();
			var actualEnumerator = y.GetSafeEnumerator();

			while (true)
			{
				var hasExpected = expectedEnumerator.MoveNext();
				var hasActual = actualEnumerator.MoveNext();

				if (!hasExpected || !hasActual)
					return AssertEqualityResult.ForResult(hasExpected == hasActual, x.InnerEnumerable, y.InnerEnumerable);

				if (!itemComparer.Equals(expectedEnumerator.Current, actualEnumerator.Current))
					return AssertEqualityResult.ForResult(false, x.InnerEnumerable, y.InnerEnumerable);
			}
		}

#if XUNIT_NULLABLE
		static AssertEqualityResult? CheckIfDictionariesAreEqual(
			CollectionTracker? x,
			CollectionTracker? y)
#else
		static AssertEqualityResult CheckIfDictionariesAreEqual(
			CollectionTracker x,
			CollectionTracker y)
#endif
		{
			if (x == null || y == null)
				return null;

			var dictionaryX = x.InnerEnumerable as IDictionary;
			var dictionaryY = y.InnerEnumerable as IDictionary;

			if (dictionaryX == null || dictionaryY == null)
				return null;

			if (dictionaryX.Count != dictionaryY.Count)
				return AssertEqualityResult.ForResult(false, x.InnerEnumerable, y.InnerEnumerable);

			var dictionaryYKeys = new HashSet<object>(dictionaryY.Keys.Cast<object>());

			// We don't pass along the itemComparer from AreCollectionsEqual because we aren't directly
			// comparing the KeyValuePair<> objects. Instead we rely on Contains() on the dictionary to
			// match up keys, and then create type-appropriate comparers for the values.
			foreach (var key in dictionaryX.Keys.Cast<object>())
			{
				if (!dictionaryYKeys.Contains(key))
					return AssertEqualityResult.ForResult(false, x.InnerEnumerable, y.InnerEnumerable);

				var valueX = dictionaryX[key];
				var valueY = dictionaryY[key];

				if (valueX == null)
				{
					if (valueY != null)
						return AssertEqualityResult.ForResult(false, x.InnerEnumerable, y.InnerEnumerable);
				}
				else if (valueY == null)
					return AssertEqualityResult.ForResult(false, x.InnerEnumerable, y.InnerEnumerable);
				else
				{
					var valueXType = valueX.GetType();
					var valueYType = valueY.GetType();

					var comparer = AssertEqualityComparer.GetDefaultComparer(valueXType == valueYType ? valueXType : typeof(object));
					if (!comparer.Equals(valueX, valueY))
						return AssertEqualityResult.ForResult(false, x.InnerEnumerable, y.InnerEnumerable);
				}

				dictionaryYKeys.Remove(key);
			}

			return AssertEqualityResult.ForResult(dictionaryYKeys.Count == 0, x.InnerEnumerable, y.InnerEnumerable);
		}

		static AssertEqualityResult CheckIfEnumerablesAreEqual(
#if XUNIT_NULLABLE
			CollectionTracker? x,
			CollectionTracker? y,
#else
			CollectionTracker x,
			CollectionTracker y,
#endif
			IEqualityComparer itemComparer,
			bool isDefaultItemComparer)
		{
			if (x == null)
				return AssertEqualityResult.ForResult(y == null, null, y?.InnerEnumerable);
			if (y == null)
				return AssertEqualityResult.ForResult(false, x.InnerEnumerable, null);

			var (comparisonType, equalsMethod) = GetAssertEqualityComparerMetadata(itemComparer);
			var enumeratorX = x.GetSafeEnumerator();
			var enumeratorY = y.GetSafeEnumerator();
			var mismatchIndex = 0;

			while (true)
			{
				var hasNextX = enumeratorX.MoveNext();
				var hasNextY = enumeratorY.MoveNext();

				if (!hasNextX || !hasNextY)
					return hasNextX == hasNextY
						? AssertEqualityResult.ForResult(true, x.InnerEnumerable, y.InnerEnumerable)
						: AssertEqualityResult.ForMismatch(x.InnerEnumerable, y.InnerEnumerable, mismatchIndex);

				var xCurrent = enumeratorX.Current;
				var yCurrent = enumeratorY.Current;

				using (var xCurrentTracker = isDefaultItemComparer ? xCurrent.AsNonStringTracker() : null)
				using (var yCurrentTracker = isDefaultItemComparer ? yCurrent.AsNonStringTracker() : null)
				{
					try
					{
						if (xCurrentTracker != null && yCurrentTracker != null)
						{
							var innerCompare = AreCollectionsEqual(xCurrentTracker, yCurrentTracker, AssertEqualityComparer<object>.DefaultInnerComparer, true);
							if (!innerCompare.Equal)
								return AssertEqualityResult.ForMismatch(x.InnerEnumerable, y.InnerEnumerable, mismatchIndex, innerResult: innerCompare);
						}
						else
						{
							var assertEqualityResult = default(AssertEqualityResult);
							if (comparisonType?.IsAssignableFrom(xCurrent?.GetType()) == true && comparisonType?.IsAssignableFrom(yCurrent?.GetType()) == true)
								assertEqualityResult = equalsMethod?.Invoke(itemComparer, new[] { xCurrent, null, yCurrent, null }) as AssertEqualityResult;

							if (assertEqualityResult != null)
							{
								if (!assertEqualityResult.Equal)
									return AssertEqualityResult.ForMismatch(x.InnerEnumerable, y.InnerEnumerable, mismatchIndex, innerResult: assertEqualityResult);
							}
							else if (!itemComparer.Equals(xCurrent, yCurrent))
								return AssertEqualityResult.ForMismatch(x.InnerEnumerable, y.InnerEnumerable, mismatchIndex);
						}
					}
					catch (Exception ex)
					{
						return AssertEqualityResult.ForMismatch(x.InnerEnumerable, y.InnerEnumerable, mismatchIndex, ex);
					}

					mismatchIndex++;
				}
			}
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
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Override to provide an implementation of <see cref="IDisposable.Dispose"/>.
		/// </summary>
		/// <param name="disposing"></param>
		protected abstract void Dispose(bool disposing);

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
		/// index is <see langword="null"/>, the extents will start at index 0.
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
		/// Since this uses the item cache produced by enumeration, it may return <see langword="null"/>
		/// when we haven't enumerated enough to see the given element, or if we enumerated
		/// so much that the item has left the cache, or if the item at the given index
		/// is <see langword="null"/>. It will also return <see langword="null"/> when the <paramref name="index"/>
		/// is <see langword="null"/>.
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
	sealed class CollectionTracker<T> : CollectionTracker, IEnumerable<T>
	{
		readonly IEnumerable<T> collection;
#if XUNIT_NULLABLE
		BufferedEnumerator? enumerator;
#else
		BufferedEnumerator enumerator;
#endif

		/// <summary>
		/// INTERNAL CONSTRUCTOR. DO NOT CALL.
		/// </summary>
		internal CollectionTracker(
			IEnumerable collection,
			IEnumerable<T> castCollection) :
				base(collection) =>
					this.collection = castCollection ?? throw new ArgumentNullException(nameof(castCollection));

		CollectionTracker(IEnumerable<T> collection) :
			base(collection) =>
				this.collection = collection;

		/// <summary>
		/// Gets the number of iterations that have happened so far.
		/// </summary>
		public int IterationCount =>
			enumerator == null ? 0 : enumerator.CurrentIndex + 1;

		/// <inheritdoc/>
		protected override void Dispose(bool disposing) =>
			enumerator?.DisposeInternal();

		/// <inheritdoc/>
		public override string FormatIndexedMismatch(
			int? mismatchedIndex,
			out int? pointerIndent,
			int depth = 1)
		{
			if (depth > ArgumentFormatter.MaxEnumerableLength)
			{
				pointerIndent = 1;
				return ArgumentFormatter.EllipsisInBrackets;
			}

			GetMismatchExtents(mismatchedIndex, out var startIndex, out var endIndex);

			return FormatIndexedMismatch(
#if XUNIT_NULLABLE
				enumerator!.CurrentItemsIndexer,
#else
				enumerator.CurrentItemsIndexer,
#endif
				enumerator.MoveNext,
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
				enumerator.CurrentItemsIndexer,
				enumerator.MoveNext,
				startIndex,
				endIndex,
				mismatchedIndex,
				out pointerIndent,
				depth
			);
		}

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
			if (depth > ArgumentFormatter.MaxEnumerableLength)
			{
				pointerIndent = 1;
				return ArgumentFormatter.EllipsisInBrackets;
			}

			int startIndex, endIndex;

			if (ArgumentFormatter.MaxEnumerableLength == int.MaxValue)
			{
				startIndex = 0;
				endIndex = span.Length - 1;
			}
			else
			{
				startIndex = Math.Max(0, (mismatchedIndex ?? 0) - ArgumentFormatter.MaxEnumerableLength / 2);
				endIndex = Math.Min(span.Length - 1, startIndex + ArgumentFormatter.MaxEnumerableLength - 1);
				startIndex = Math.Max(0, endIndex - ArgumentFormatter.MaxEnumerableLength + 1);
			}

			var moreItemsPastEndIndex = endIndex < span.Length - 1;
			var items = new Dictionary<int, T>();

			for (var idx = startIndex; idx <= endIndex; ++idx)
				items[idx] = span[idx];

			return FormatIndexedMismatch(
				idx => items[idx],
				() => moreItemsPastEndIndex,
				startIndex,
				endIndex,
				mismatchedIndex,
				out pointerIndent,
				depth
			);
		}

		static string FormatIndexedMismatch(
			Func<int, T> indexer,
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

				printedValues.Append(ArgumentFormatter.Format(indexer(idx), depth));
			}

			if (moreItemsPastEndIndex())
				printedValues.Append(", " + ArgumentFormatter.Ellipsis);

			printedValues.Append(']');
			return printedValues.ToString();
		}

		/// <inheritdoc/>
		public override string FormatStart(int depth = 1)
		{
			if (depth > ArgumentFormatter.MaxEnumerableLength)
				return ArgumentFormatter.EllipsisInBrackets;

			if (enumerator == null)
				enumerator = BufferedEnumerator.Create(collection.GetEnumerator());

			// Ensure we have already seen enough data to format
			while (enumerator.CurrentIndex <= ArgumentFormatter.MaxEnumerableLength)
				if (!enumerator.MoveNext())
					break;

			return FormatStart(enumerator.StartItemsIndexer, enumerator.CurrentIndex, depth);
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

			if (depth > ArgumentFormatter.MaxEnumerableLength)
				return ArgumentFormatter.EllipsisInBrackets;

			var startItems = new List<T>();
			var currentIndex = -1;
			var spanEnumerator = collection.GetEnumerator();

			// Ensure we have already seen enough data to format
			while (currentIndex <= ArgumentFormatter.MaxEnumerableLength)
			{
				if (!spanEnumerator.MoveNext())
					break;

				startItems.Add(spanEnumerator.Current);
				++currentIndex;
			}

			return FormatStart(idx => startItems[idx], currentIndex, depth);
		}

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
			if (depth > ArgumentFormatter.MaxEnumerableLength)
				return ArgumentFormatter.EllipsisInBrackets;

			var startItems = new List<T>();
			var currentIndex = -1;
			var spanEnumerator = span.GetEnumerator();

			// Ensure we have already seen enough data to format
			while (currentIndex <= ArgumentFormatter.MaxEnumerableLength)
			{
				if (!spanEnumerator.MoveNext())
					break;

				startItems.Add(spanEnumerator.Current);
				++currentIndex;
			}

			return FormatStart(idx => startItems[idx], currentIndex, depth);
		}

		static string FormatStart(
			Func<int, T> indexer,
			int currentIndex,
			int depth)
		{
			var printedValues = new StringBuilder("[");
			var printLength = Math.Min(currentIndex + 1, ArgumentFormatter.MaxEnumerableLength);

			for (var idx = 0; idx < printLength; ++idx)
			{
				if (idx != 0)
					printedValues.Append(", ");

				printedValues.Append(ArgumentFormatter.Format(indexer(idx), depth));
			}

			if (currentIndex >= ArgumentFormatter.MaxEnumerableLength)
				printedValues.Append(", " + ArgumentFormatter.Ellipsis);

			printedValues.Append(']');
			return printedValues.ToString();
		}

		/// <inheritdoc/>
		public IEnumerator<T> GetEnumerator()
		{
			if (enumerator != null)
				throw new InvalidOperationException("Multiple enumeration is not supported");

			enumerator = BufferedEnumerator.Create(collection.GetEnumerator());
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
				enumerator = BufferedEnumerator.Create(collection.GetEnumerator());

			if (ArgumentFormatter.MaxEnumerableLength == int.MaxValue)
			{
				startIndex = 0;
				endIndex = int.MaxValue;
			}
			else
			{
				startIndex = Math.Max(0, (mismatchedIndex ?? 0) - ArgumentFormatter.MaxEnumerableLength / 2);
				endIndex = startIndex + ArgumentFormatter.MaxEnumerableLength - 1;
			}

			// Make sure our window starts with startIndex and ends with endIndex, as appropriate
			while (enumerator.CurrentIndex < endIndex)
				if (!enumerator.MoveNext())
					break;

			endIndex = enumerator.CurrentIndex;

			if (ArgumentFormatter.MaxEnumerableLength != int.MaxValue)
				startIndex = Math.Max(0, endIndex - ArgumentFormatter.MaxEnumerableLength + 1);
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

			if (!enumerator.TryGetCurrentItemAt(index.Value, out var item))
				return null;

			return item?.GetType().FullName;
		}

		/// <summary>
		/// Wraps the given collection inside of a <see cref="CollectionTracker{T}"/>.
		/// </summary>
		/// <param name="collection">The collection to be wrapped</param>
		public static CollectionTracker<T> Wrap(IEnumerable<T> collection) =>
			new CollectionTracker<T>(collection);

		abstract class BufferedEnumerator : IEnumerator<T>
		{
			protected BufferedEnumerator(IEnumerator<T> innerEnumerator) =>
				InnerEnumerator = innerEnumerator;

			public T Current =>
				InnerEnumerator.Current;

#if XUNIT_NULLABLE
			object? IEnumerator.Current =>
#else
			object IEnumerator.Current =>
#endif
				Current;

			public int CurrentIndex { get; private set; } = -1;

			public abstract Func<int, T> CurrentItemsIndexer { get; }

			protected IEnumerator<T> InnerEnumerator { get; }

			public abstract Func<int, T> StartItemsIndexer { get; }

			public static BufferedEnumerator Create(IEnumerator<T> innerEnumerator) =>
				ArgumentFormatter.MaxEnumerableLength == int.MaxValue
					? (BufferedEnumerator)new ListBufferedEnumerator(innerEnumerator)
					: new RingBufferedEnumerator(innerEnumerator);

			public void Dispose() { }

			public void DisposeInternal() =>
				InnerEnumerator.Dispose();

			public virtual bool MoveNext()
			{
				if (!InnerEnumerator.MoveNext())
					return false;

				CurrentIndex++;
				return true;
			}

			public virtual void Reset()
			{
				InnerEnumerator.Reset();

				CurrentIndex = -1;
			}

			public abstract bool TryGetCurrentItemAt(
				int index,
#if XUNIT_NULLABLE
				[MaybeNullWhen(false)] out T item);
#else
				out T item);
#endif

			// Used when ArgumentFormatter.MaxEnumerableLength is unlimited (int.MaxValue)
			sealed class ListBufferedEnumerator : BufferedEnumerator
			{
				readonly List<T> buffer = new List<T>();

				public ListBufferedEnumerator(IEnumerator<T> innerEnumerator) :
					base(innerEnumerator)
				{ }

				public override Func<int, T> CurrentItemsIndexer =>
					idx => buffer[idx];

				public override Func<int, T> StartItemsIndexer =>
					idx => buffer[idx];

				public override bool MoveNext()
				{
					if (!base.MoveNext())
						return false;

					buffer.Add(InnerEnumerator.Current);
					return true;
				}

				public override void Reset()
				{
					base.Reset();

					buffer.Clear();
				}

				public override bool TryGetCurrentItemAt(
					int index,
#if XUNIT_NULLABLE
					[MaybeNullWhen(false)] out T item)
#else
					out T item)
#endif
				{
					if (index < 0 || index > CurrentIndex)
					{
						item = default;
						return false;
					}

					item = buffer[index];
					return true;
				}
			}

			// Used when ArgumentFormatter.MaxEnumerableLength is not unlimited
			sealed class RingBufferedEnumerator : BufferedEnumerator
			{
				int currentItemsLastInsertionIndex = -1;
				readonly T[] currentItemsRingBuffer = new T[ArgumentFormatter.MaxEnumerableLength];
				readonly List<T> startItems = new List<T>();

				public override Func<int, T> CurrentItemsIndexer
				{
					get
					{
						var result = new Dictionary<int, T>();

						if (CurrentIndex > -1)
						{
							var itemIndex = Math.Max(0, CurrentIndex - ArgumentFormatter.MaxEnumerableLength + 1);

							var indexInRingBuffer = (currentItemsLastInsertionIndex - CurrentIndex + itemIndex) % ArgumentFormatter.MaxEnumerableLength;
							if (indexInRingBuffer < 0)
								indexInRingBuffer += ArgumentFormatter.MaxEnumerableLength;

							while (itemIndex <= CurrentIndex)
							{
								result[itemIndex] = currentItemsRingBuffer[indexInRingBuffer];

								++itemIndex;
								indexInRingBuffer = (indexInRingBuffer + 1) % ArgumentFormatter.MaxEnumerableLength;
							}
						}

						return idx => result[idx];
					}
				}

				public override Func<int, T> StartItemsIndexer =>
					idx => startItems[idx];

				public RingBufferedEnumerator(IEnumerator<T> innerEnumerator) :
					base(innerEnumerator)
				{ }

				public override bool MoveNext()
				{
					if (!base.MoveNext())
						return false;

					var current = InnerEnumerator.Current;

					// Keep (MAX_ENUMERABLE_LENGTH + 1) items here, so we can
					// print the start of the collection when lengths differ
					if (CurrentIndex <= ArgumentFormatter.MaxEnumerableLength)
						startItems.Add(current);

					// Keep a ring buffer filled with the most recent MAX_ENUMERABLE_LENGTH items
					// so we can print out the items when we've found a bad index
					currentItemsLastInsertionIndex = (currentItemsLastInsertionIndex + 1) % ArgumentFormatter.MaxEnumerableLength;
					currentItemsRingBuffer[currentItemsLastInsertionIndex] = current;

					return true;
				}

				public override void Reset()
				{
					base.Reset();

					currentItemsLastInsertionIndex = -1;
					startItems.Clear();
				}

				public override bool TryGetCurrentItemAt(
					int index,
#if XUNIT_NULLABLE
					[MaybeNullWhen(false)] out T item)
#else
					out T item)
#endif
				{
					if (index < 0 || index <= CurrentIndex - ArgumentFormatter.MaxEnumerableLength || index > CurrentIndex)
					{
						item = default;
						return false;
					}

					var indexInRingBuffer = (currentItemsLastInsertionIndex - CurrentIndex + index) % ArgumentFormatter.MaxEnumerableLength;
					if (indexInRingBuffer < 0)
						indexInRingBuffer += ArgumentFormatter.MaxEnumerableLength;

					item = currentItemsRingBuffer[indexInRingBuffer];
					return true;
				}
			}
		}
	}
}
