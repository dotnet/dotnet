#if XUNIT_AOT

#pragma warning disable CA1040 // Avoid empty interfaces
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// This interface is not supported in Native AOT, because interface-based discovery
	/// requires reflection.
	/// </summary>
	[Obsolete("This interface is not supported in Native AOT. Decorating with it is benign and ignored.")]
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	interface IAssertionException
	{ }
}

#endif  // XUNIT_AOT
