#pragma warning disable CA1040 // Avoid empty interfaces
#pragma warning disable CA1200 // Avoid using cref tags with a prefix
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
#pragma warning disable IDE0161 // Convert to file-scoped namespace

#if XUNIT_NULLABLE
#nullable enable
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// This is a marker interface implemented by all built-in assertion exceptions so that
	/// test failures can be marked with <see cref="F:Xunit.Sdk.FailureCause.Assertion"/>.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	interface IAssertionException
	{ }
}
