#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Collections.Generic;
using System.Linq;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.All fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class AllException : XunitException
	{
		AllException(string message) :
			base(message)
		{ }

#if XUNIT_VALUETASK
		/// <summary>
		/// Creates a new instance of the <see cref="AllException"/> class to be thrown when one or
		/// more items failed during <see cref="Assert.All{T}(IEnumerable{T}, Action{T})"/>,
		/// <see cref="Assert.All{T}(IEnumerable{T}, Action{T, int})"/>,
		/// <see cref="Assert.AllAsync{T}(IEnumerable{T}, Func{T, System.Threading.Tasks.ValueTask})"/>,
		/// or <see cref="Assert.AllAsync{T}(IEnumerable{T}, Func{T, int, System.Threading.Tasks.ValueTask})"/>.
		/// </summary>
		/// <param name="totalItems">The total number of items in the collection</param>
		/// <param name="errors">The list of failures (as index, value, and exception)</param>
#else
		/// <summary>
		/// Creates a new instance of the <see cref="AllException"/> class to be thrown when one or
		/// more items failed during <see cref="Assert.All{T}(IEnumerable{T}, Action{T})"/>
		/// or <see cref="Assert.All{T}(IEnumerable{T}, Action{T, int})"/>.
		/// </summary>
		/// <param name="totalItems">The total number of items in the collection</param>
		/// <param name="errors">The list of failures (as index, value, and exception)</param>
#endif
		public static AllException ForFailures(
			int totalItems,
			IReadOnlyList<Tuple<int, string, Exception>> errors)
		{
			var maxItemIndexLength = errors.Max(x => x.Item1).ToString().Length + 4; // "[#]: "
			var indexSpaces = new string(' ', maxItemIndexLength);
			var maxWrapIndent = maxItemIndexLength + 7; // "Item:  " and "Error: "
			var wrapSpaces = Environment.NewLine + new string(' ', maxWrapIndent);

			var message =
				$"Assert.All() Failure: {errors.Count} out of {totalItems} items in the collection did not pass." + Environment.NewLine +
				string.Join(
					Environment.NewLine,
					errors.Select(error =>
					{
						var indexString = $"[{error.Item1}]:".PadRight(maxItemIndexLength);

						return $"{indexString}Item:  {error.Item2.Replace(Environment.NewLine, wrapSpaces)}" + Environment.NewLine +
							   $"{indexSpaces}Error: {error.Item3.Message.Replace(Environment.NewLine, wrapSpaces)}";
					})
				);

			return new AllException(message);
		}
	}
}
