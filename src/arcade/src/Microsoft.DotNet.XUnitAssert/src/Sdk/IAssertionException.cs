#pragma warning disable CA1711  // This is an interface which indicates exceptions, so ending its name with Exception is correct

#if XUNIT_NULLABLE
#nullable enable
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// This is a marker interface implemented by all built-in assertion exceptions so that
	/// test failures can be marked with <see cref="F:Xunit.v3.FailureCause.Assertion"/>.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	interface IAssertionException
	{ }
}
