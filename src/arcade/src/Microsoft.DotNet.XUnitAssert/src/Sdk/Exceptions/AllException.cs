#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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

		/// <summary>
		/// Creates a new instance of the <see cref="AllException"/> class to be thrown when one or
		/// more items failed during <see cref="Assert.All{T}(IEnumerable{T}, Action{T})"/>
		/// or <see cref="Assert.All{T}(IEnumerable{T}, Action{T, int})"/>,
		/// <see cref="Assert.AllAsync{T}(IEnumerable{T}, Func{T, Task})"/>,
		/// or <see cref="Assert.AllAsync{T}(IEnumerable{T}, Func{T, int, Task})"/>.
		/// </summary>
		/// <param name="totalItems">The total number of items in the collection</param>
		/// <param name="errors">The list of failures (as index, value, and exception)</param>
		public static AllException ForFailures(
			int totalItems,
			IReadOnlyList<Tuple<int, string, Exception>> errors)
		{
			Assert.GuardArgumentNotNull(nameof(errors), errors);

			var maxItemIndexLength = errors.Max(x => x.Item1).ToString(CultureInfo.CurrentCulture).Length + 4; // "[#]: "
			var indexSpaces = new string(' ', maxItemIndexLength);
			var maxWrapIndent = maxItemIndexLength + 7; // "Item:  " and "Error: "
			var wrapSpaces = Environment.NewLine + new string(' ', maxWrapIndent);

			var message =
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.All() Failure: {0} out of {1} items in the collection did not pass.{2}{3}",
					errors.Count,
					totalItems,
					Environment.NewLine,
					string.Join(
						Environment.NewLine,
						errors.Select(error =>
							string.Format(
								CultureInfo.CurrentCulture,
								"{0}Item:  {1}{2}{3}Error: {4}",
								string.Format(CultureInfo.CurrentCulture, "[{0}]:", error.Item1).PadRight(maxItemIndexLength),
#if NETCOREAPP2_0_OR_GREATER
								error.Item2.Replace(Environment.NewLine, wrapSpaces, StringComparison.Ordinal),
#else
								error.Item2.Replace(Environment.NewLine, wrapSpaces),
#endif
								Environment.NewLine,
								indexSpaces,
#if NETCOREAPP2_0_OR_GREATER
								error.Item3.Message.Replace(Environment.NewLine, wrapSpaces, StringComparison.Ordinal)
#else
								error.Item3.Message.Replace(Environment.NewLine, wrapSpaces)
#endif
							)
						)
					)
				);

			return new AllException(message);
		}
	}
}
