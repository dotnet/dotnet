#if XUNIT_NULLABLE
#nullable enable
#endif

using System.Collections.Generic;
using Xunit.Sdk;

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
		/// Verifies that a set is a proper subset of another set.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="expectedSuperset">The expected superset</param>
		/// <param name="actual">The set expected to be a proper subset</param>
		/// <exception cref="ContainsException">Thrown when the actual set is not a proper subset of the expected set</exception>
#if XUNIT_NULLABLE
		public static void ProperSubset<T>(ISet<T> expectedSuperset, ISet<T>? actual)
#else
		public static void ProperSubset<T>(ISet<T> expectedSuperset, ISet<T> actual)
#endif
		{
			GuardArgumentNotNull(nameof(expectedSuperset), expectedSuperset);

			if (actual == null || !actual.IsProperSubsetOf(expectedSuperset))
				throw new ProperSubsetException(expectedSuperset, actual);
		}

		/// <summary>
		/// Verifies that a set is a proper superset of another set.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="expectedSubset">The expected subset</param>
		/// <param name="actual">The set expected to be a proper superset</param>
		/// <exception cref="ContainsException">Thrown when the actual set is not a proper superset of the expected set</exception>
#if XUNIT_NULLABLE
		public static void ProperSuperset<T>(ISet<T> expectedSubset, ISet<T>? actual)
#else
		public static void ProperSuperset<T>(ISet<T> expectedSubset, ISet<T> actual)
#endif
		{
			GuardArgumentNotNull(nameof(expectedSubset), expectedSubset);

			if (actual == null || !actual.IsProperSupersetOf(expectedSubset))
				throw new ProperSupersetException(expectedSubset, actual);
		}

		/// <summary>
		/// Verifies that a set is a subset of another set.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="expectedSuperset">The expected superset</param>
		/// <param name="actual">The set expected to be a subset</param>
		/// <exception cref="ContainsException">Thrown when the actual set is not a subset of the expected set</exception>
#if XUNIT_NULLABLE
		public static void Subset<T>(ISet<T> expectedSuperset, ISet<T>? actual)
#else
		public static void Subset<T>(ISet<T> expectedSuperset, ISet<T> actual)
#endif
		{
			GuardArgumentNotNull(nameof(expectedSuperset), expectedSuperset);

			if (actual == null || !actual.IsSubsetOf(expectedSuperset))
				throw new SubsetException(expectedSuperset, actual);
		}

		/// <summary>
		/// Verifies that a set is a superset of another set.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="expectedSubset">The expected subset</param>
		/// <param name="actual">The set expected to be a superset</param>
		/// <exception cref="ContainsException">Thrown when the actual set is not a superset of the expected set</exception>
#if XUNIT_NULLABLE
		public static void Superset<T>(ISet<T> expectedSubset, ISet<T>? actual)
#else
		public static void Superset<T>(ISet<T> expectedSubset, ISet<T> actual)
#endif
		{
			GuardArgumentNotNull(nameof(expectedSubset), expectedSubset);

			if (actual == null || !actual.IsSupersetOf(expectedSubset))
				throw new SupersetException(expectedSubset, actual);
		}
	}
}
