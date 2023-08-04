#if XUNIT_NULLABLE
#nullable enable
using System.Diagnostics.CodeAnalysis;
#endif

using System;

namespace Xunit
{
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class Assert
	{
		/// <summary/>
#if XUNIT_NULLABLE
		internal static void GuardArgumentNotNull(string argName, [NotNull] object? argValue)
#else
		internal static void GuardArgumentNotNull(string argName, object argValue)
#endif
		{
			if (argValue == null)
				throw new ArgumentNullException(argName);
		}
	}
}
