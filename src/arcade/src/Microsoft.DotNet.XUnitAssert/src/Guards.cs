#pragma warning disable CA1052 // Static holder types should be static

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit
{
	partial class Assert
	{
		/// <summary/>
#if XUNIT_NULLABLE
		[return: NotNull]
#endif
		internal static T GuardArgumentNotNull<T>(
			string argName,
#if XUNIT_NULLABLE
			[NotNull] T? argValue)
#else
			T argValue)
#endif
		{
			if (argValue == null)
				throw new ArgumentNullException(argName.TrimStart('@'));

			return argValue;
		}
	}
}
