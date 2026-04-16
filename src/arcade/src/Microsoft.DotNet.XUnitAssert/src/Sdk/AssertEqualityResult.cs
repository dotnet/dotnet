#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0290 // Use primary constructor

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8601
#pragma warning disable CS8618
#pragma warning disable CS8625
#pragma warning disable CS8765
#pragma warning disable CS8767
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// Indicates the result of comparing two values for equality. Includes success/failure information, as well
	/// as indices where the values differ, if the values are indexed (e.g., collections or strings).
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class AssertEqualityResult : IEquatable<AssertEqualityResult>
	{
		AssertEqualityResult(
			bool equal,
#if XUNIT_NULLABLE
			object? x,
			object? y,
#else
			object x,
			object y,
#endif
			int? mismatchIndexX = null,
			int? mismatchIndexY = null,
#if XUNIT_NULLABLE
			Exception? exception = null,
			AssertEqualityResult? innerResult = null)
#else
			Exception exception = null,
			AssertEqualityResult innerResult = null)
#endif
		{
			Equal = equal;
			X = x;
			Y = y;
			Exception = exception;
			InnerResult = innerResult;
			MismatchIndexX = mismatchIndexX;
			MismatchIndexY = mismatchIndexY;
		}

		/// <summary>
		/// Returns <see langword="true"/> if the values were equal; <see langword="false"/>, otherwise.
		/// </summary>
		public bool Equal { get; }

		/// <summary>
		/// Returns the exception that caused the failure, if it was based on an exception.
		/// </summary>
#if XUNIT_NULLABLE
		public Exception? Exception { get; }
#else
		public Exception Exception { get; }
#endif

		/// <summary>
		/// Returns the comparer result for any inner comparison that caused this result
		/// to fail; returns <see langword="null"/> if there was no inner comparison.
		/// </summary>
		/// <remarks>
		/// If this value is set, then it generally indicates that this comparison was a
		/// failed collection comparison, and the inner result indicates the specific
		/// item comparison that caused the failure.
		/// </remarks>
#if XUNIT_NULLABLE
		public AssertEqualityResult? InnerResult { get; }
#else
		public AssertEqualityResult InnerResult { get; }
#endif

		/// <summary>
		/// Returns the index of the mismatch for the <c>X</c> value, if the comparison
		/// failed on a specific index.
		/// </summary>
		public int? MismatchIndexX { get; }

		/// <summary>
		/// Returns the index of the mismatch for the <c>Y</c> value, if the comparison
		/// failed on a specific index.
		/// </summary>
		public int? MismatchIndexY { get; }

		/// <summary>
		/// The left-hand value in the comparison
		/// </summary>
#if XUNIT_NULLABLE
		public object? X { get; }
#else
		public object X { get; }
#endif

		/// <summary>
		/// The right-hand value in the comparison
		/// </summary>
#if XUNIT_NULLABLE
		public object? Y { get; }
#else
		public object Y { get; }
#endif

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>Returns <see langword="true"/> if the values are equal; <see langword="false"/>, otherwise.</returns>
#if XUNIT_NULLABLE
		public override bool Equals(object? obj) =>
#else
		public override bool Equals(object obj) =>
#endif
			obj is AssertEqualityResult other && Equals(other);

		/// <summary>
		/// Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="other">The object to compare with the current object.</param>
		/// <returns>Returns <see langword="true"/> if the values are equal; <see langword="false"/>, otherwise.</returns>
#if XUNIT_NULLABLE
		public bool Equals(AssertEqualityResult? other)
#else
		public bool Equals(AssertEqualityResult other)
#endif
		{
			if (other is null)
				return false;

			return
				Equal.Equals(other) &&
				X?.Equals(other.X) != false &&
				Y?.Equals(other.Y) != false &&
				InnerResult?.Equals(other.InnerResult) != false &&
				MismatchIndexX.Equals(other.MismatchIndexY) &&
				MismatchIndexY.Equals(other.MismatchIndexY);
		}

		/// <summary>
		/// Creates an instance of <see cref="AssertEqualityResult"/> where the values were
		/// not equal, and there is a single mismatch index (for example, when comparing two
		/// collections).
		/// </summary>
		/// <param name="x">The left-hand value in the comparison</param>
		/// <param name="y">The right-hand value in the comparison</param>
		/// <param name="mismatchIndex">The mismatch index for both <c>X</c> and <c>Y</c> values</param>
		/// <param name="exception">The optional exception that was thrown to cause the failure</param>
		/// <param name="innerResult">The optional inner result that caused the equality failure</param>
		public static AssertEqualityResult ForMismatch(
#if XUNIT_NULLABLE
			object? x,
			object? y,
#else
			object x,
			object y,
#endif
			int mismatchIndex,
#if XUNIT_NULLABLE
			Exception? exception = null,
			AssertEqualityResult? innerResult = null) =>
#else
			Exception exception = null,
			AssertEqualityResult innerResult = null) =>
#endif
				new AssertEqualityResult(false, x, y, mismatchIndex, mismatchIndex, exception, innerResult);

		/// <summary>
		/// Creates an instance of <see cref="AssertEqualityResult"/> where the values were
		/// not equal, and there are separate mismatch indices (for example, when comparing two
		/// strings under special circumstances).
		/// </summary>
		/// <param name="x">The left-hand value in the comparison</param>
		/// <param name="y">The right-hand value in the comparison</param>
		/// <param name="mismatchIndexX">The mismatch index for the <c>X</c> value</param>
		/// <param name="mismatchIndexY">The mismatch index for the <c>Y</c> value</param>
		/// <param name="exception">The optional exception that was thrown to cause the failure</param>
		/// <param name="innerResult">The optional inner result that caused the equality failure</param>
		public static AssertEqualityResult ForMismatch(
#if XUNIT_NULLABLE
			object? x,
			object? y,
#else
			object x,
			object y,
#endif
			int mismatchIndexX,
			int mismatchIndexY,
#if XUNIT_NULLABLE
			Exception? exception = null,
			AssertEqualityResult? innerResult = null) =>
#else
			Exception exception = null,
			AssertEqualityResult innerResult = null) =>
#endif
				new AssertEqualityResult(false, x, y, mismatchIndexX, mismatchIndexY, exception, innerResult);

		/// <summary>
		/// Creates an instance of <see cref="AssertEqualityResult"/>.
		/// </summary>
		/// <param name="equal">A flag which indicates whether the values were equal</param>
		/// <param name="x">The left-hand value in the comparison</param>
		/// <param name="y">The right-hand value in the comparison</param>
		/// <param name="exception">The optional exception that was thrown to cause the failure</param>
		/// <param name="innerResult">The optional inner result that caused the equality failure</param>
		public static AssertEqualityResult ForResult(
			bool equal,
#if XUNIT_NULLABLE
			object? x,
			object? y,
			Exception? exception = null,
			AssertEqualityResult? innerResult = null) =>
#else
			object x,
			object y,
			Exception exception = null,
			AssertEqualityResult innerResult = null) =>
#endif
				new AssertEqualityResult(equal, x, y, exception: exception, innerResult: innerResult);

		/// <summary>
		/// Gets a hash code for the object, to be used in hashed containers.
		/// </summary>
		public override int GetHashCode() =>
			(Equal, MismatchIndexX, MismatchIndexY).GetHashCode();

		/// <summary>
		/// Determines whether two instances of <see cref="AssertEqualityResult"/> are equal.
		/// </summary>
		/// <param name="left">The first value</param>
		/// <param name="right">The second value</param>
		/// <returns>Returns <see langword="true"/> if the values are equal; <see langword="false"/>, otherwise.</returns>
		public static bool operator ==(
#if XUNIT_NULLABLE
			AssertEqualityResult? left,
			AssertEqualityResult? right) =>
#else
			AssertEqualityResult left,
			AssertEqualityResult right) =>
#endif
				left?.Equals(right) == true;

		/// <summary>
		/// Determines whether two instances of <see cref="AssertEqualityResult"/> are not equal.
		/// </summary>
		/// <param name="left">The first value</param>
		/// <param name="right">The second value</param>
		/// <returns>Returns <see langword="true"/> if the values are not equal; <see langword="false"/>, otherwise.</returns>
		public static bool operator !=(
#if XUNIT_NULLABLE
			AssertEqualityResult? left,
			AssertEqualityResult? right) =>
#else
			AssertEqualityResult left,
			AssertEqualityResult right) =>
#endif
				left?.Equals(right) == false;
	}
}
