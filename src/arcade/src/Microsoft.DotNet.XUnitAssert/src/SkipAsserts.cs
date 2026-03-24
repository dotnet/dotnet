#pragma warning disable CA1052 // Static holder types should be static

#if XUNIT_NULLABLE
#nullable enable
#endif

using Xunit.Sdk;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit
{
	partial class Assert
	{
		/// <summary>
		/// Skips the current test. Used when determining whether a test should be skipped
		/// happens at runtime rather than at discovery time.
		/// </summary>
		/// <param name="reason">The message to indicate why the test was skipped</param>
#if XUNIT_NULLABLE
		[DoesNotReturn]
#endif
		public static void Skip(string reason)
		{
			GuardArgumentNotNull(nameof(reason), reason);

			throw SkipException.ForSkip(reason);
		}

		/// <summary>
		/// Will skip the current test unless <paramref name="condition"/> evaluates to <see langword="true"/>.
		/// </summary>
		/// <param name="condition">When <see langword="true"/>, the test will continue to run; when <see langword="false"/>,
		/// the test will be skipped</param>
		/// <param name="reason">The message to indicate why the test was skipped</param>
		public static void SkipUnless(
#if XUNIT_NULLABLE
			[DoesNotReturnIf(false)] bool condition,
#else
			bool condition,
#endif
			string reason)
		{
			GuardArgumentNotNull(nameof(reason), reason);

			if (!condition)
				throw SkipException.ForSkip(reason);
		}

		/// <summary>
		/// Will skip the current test when <paramref name="condition"/> evaluates to <see langword="true"/>.
		/// </summary>
		/// <param name="condition">When <see langword="true"/>, the test will be skipped; when <see langword="false"/>,
		/// the test will continue to run</param>
		/// <param name="reason">The message to indicate why the test was skipped</param>
		public static void SkipWhen(
#if XUNIT_NULLABLE
			[DoesNotReturnIf(true)] bool condition,
#else
			bool condition,
#endif
			string reason)
		{
			GuardArgumentNotNull(nameof(reason), reason);

			if (condition)
				throw SkipException.ForSkip(reason);
		}
	}
}
