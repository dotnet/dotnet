#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Single fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class SingleException : XunitException
	{
		SingleException(string errorMessage)
			: base(errorMessage)
		{ }

		/// <summary>
		/// Creates an new instance of the <see cref="SingleException"/> class to be thrown when
		/// the collection didn't contain any values (or didn't contain the expected value).
		/// </summary>
		/// <param name="expected">The expected value (set to <c>null</c> for no expected value)</param>
		/// <param name="collection">The collection</param>
		public static SingleException Empty(
#if XUNIT_NULLABLE
			string? expected,
#else
			string expected,
#endif
			string collection)
		{
			Assert.GuardArgumentNotNull(nameof(collection), collection);

			if (expected == null)
				return new SingleException("Assert.Single() Failure: The collection was empty");

			return new SingleException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.Single() Failure: The collection did not contain any matching items{0}Expected:   {1}{2}Collection: {3}",
					Environment.NewLine,
					expected,
					Environment.NewLine,
					collection
				)
			);
		}

		/// <summary>
		/// Creates an new instance of the <see cref="SingleException"/> class to be thrown when
		/// the collection more than one value (or contained more than one of the expected value).
		/// </summary>
		/// <param name="count">The number of items, or the number of matching items</param>
		/// <param name="expected">The expected value (set to <c>null</c> for no expected value)</param>
		/// <param name="collection">The collection</param>
		/// <param name="matchIndices">The list of indices where matches occurred</param>
		public static SingleException MoreThanOne(
			int count,
#if XUNIT_NULLABLE
			string? expected,
#else
			string expected,
#endif
			string collection,
			ICollection<int> matchIndices)
		{
			Assert.GuardArgumentNotNull(nameof(collection), collection);
			Assert.GuardArgumentNotNull(nameof(matchIndices), matchIndices);

			var message = string.Format(
				CultureInfo.CurrentCulture,
				"Assert.Single() Failure: The collection contained {0} {1}items",
				count,
				expected == null ? "" : "matching "
			);

			if (expected == null)
				message += string.Format(
					CultureInfo.CurrentCulture,
					"{0}Collection: {1}",
					Environment.NewLine,
					collection
				);
			else
				message += string.Format(
					CultureInfo.CurrentCulture,
					"{0}Expected:      {1}{2}Collection:    {3}{4}Match indices: {5}",
					Environment.NewLine,
					expected,
					Environment.NewLine,
					collection,
					Environment.NewLine,
					string.Join(", ", matchIndices)
				);

			return new SingleException(message);
		}
	}
}
