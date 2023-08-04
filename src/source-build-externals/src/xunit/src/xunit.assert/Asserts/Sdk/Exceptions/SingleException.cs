#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when the collection did not contain exactly one element.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class SingleException : XunitException
	{
		SingleException(string errorMessage)
			: base(errorMessage)
		{ }

		/// <summary>
		/// Creates an instance of <see cref="SingleException"/> for when the collection didn't contain any of the expected value.
		/// </summary>
#if XUNIT_NULLABLE
		public static Exception Empty(string? expected) =>
#else
		public static Exception Empty(string expected) =>
#endif
			new SingleException(
				string.Format(
					CultureInfo.CurrentCulture,
					"The collection was expected to contain a single element{0}, but it {1}",
					expected == null ? "" : " matching " + expected,
					expected == null ? "was empty." : "contained no matching elements."
				)
			);

		/// <summary>
		/// Creates an instance of <see cref="SingleException"/> for when the collection had too many of the expected items.
		/// </summary>
		/// <returns></returns>
#if XUNIT_NULLABLE
		public static Exception MoreThanOne(int count, string? expected) =>
#else
		public static Exception MoreThanOne(int count, string expected) =>
#endif
			new SingleException(
				string.Format(
					CultureInfo.CurrentCulture,
					"The collection was expected to contain a single element{0}, but it contained {1}{2} elements.",
					expected == null ? "" : " matching " + expected,
					count,
					expected == null ? "" : " matching"
				)
			);
	}
}
