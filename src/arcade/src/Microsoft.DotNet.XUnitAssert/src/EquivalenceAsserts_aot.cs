#if XUNIT_AOT

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Xunit
{
	partial class Assert
	{
		/// <summary>
		/// Assert.Equivalent is not supported in Native AOT due to reflection requirements.
		/// </summary>
		[Obsolete("Assert.Equivalent is not supported in Native AOT", error: true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void Equivalent(
#if XUNIT_NULLABLE
			object? expected,
			object? actual,
#else
			object expected,
			object actual,
#endif
			bool strict = false) =>
				throw new NotSupportedException("Assert.Equivalent is not supported in Native AOT");

		/// <summary>
		/// Assert.EquivalentWithExclusions is not supported in Native AOT due to reflection requirements.
		/// </summary>
		[Obsolete("Assert.EquivalentWithExclusions is not supported in Native AOT", error: true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void EquivalentWithExclusions<T>(
#if XUNIT_NULLABLE
			object? expected,
#else
			object expected,
#endif
			T actual,
#if XUNIT_NULLABLE
			params Expression<Func<T, object?>>[] exclusionExpressions) =>
#else
			params Expression<Func<T, object>>[] exclusionExpressions) =>
#endif
				throw new NotSupportedException("Assert.Equivalent is not supported in Native AOT");

		/// <summary>
		/// Assert.EquivalentWithExclusions is not supported in Native AOT due to reflection requirements.
		/// </summary>
		[Obsolete("Assert.EquivalentWithExclusions is not supported in Native AOT", error: true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void EquivalentWithExclusions<T>(
#if XUNIT_NULLABLE
			object? expected,
#else
			object expected,
#endif
			T actual,
			bool strict,
#if XUNIT_NULLABLE
			params Expression<Func<T, object?>>[] exclusionExpressions) =>
#else
			params Expression<Func<T, object>>[] exclusionExpressions) =>
#endif
				throw new NotSupportedException("Assert.Equivalent is not supported in Native AOT");

		/// <summary>
		/// Assert.EquivalentWithExclusions is not supported in Native AOT due to reflection requirements.
		/// </summary>
		[Obsolete("Assert.EquivalentWithExclusions is not supported in Native AOT", error: true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void EquivalentWithExclusions(
#if XUNIT_NULLABLE
			object? expected,
			object? actual,
#else
			object expected,
			object actual,
#endif
			params string[] exclusionExpressions) =>
				throw new NotSupportedException("Assert.Equivalent is not supported in Native AOT");

		/// <summary>
		/// Assert.EquivalentWithExclusions is not supported in Native AOT due to reflection requirements.
		/// </summary>
		[Obsolete("Assert.EquivalentWithExclusions is not supported in Native AOT", error: true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void EquivalentWithExclusions(
#if XUNIT_NULLABLE
			object? expected,
			object? actual,
#else
			object expected,
			object actual,
#endif
			bool strict,
			params string[] exclusionExpressions) =>
				throw new NotSupportedException("Assert.Equivalent is not supported in Native AOT");
	}
}

#endif  // XUNIT_AOT
