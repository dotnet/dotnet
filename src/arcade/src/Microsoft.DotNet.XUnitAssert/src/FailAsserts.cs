#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8625
#endif

using Xunit.Sdk;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit
{
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class Assert
	{
		/// <summary>
		/// Indicates that the test should immediately fail.
		/// </summary>
		/// <param name="message">The optional failure message</param>
#if XUNIT_NULLABLE
		[DoesNotReturn]
		public static void Fail(string? message = null)
#else
		public static void Fail(string message = null)
#endif
		{
			throw FailException.ForFailure(message);
		}
	}
}
