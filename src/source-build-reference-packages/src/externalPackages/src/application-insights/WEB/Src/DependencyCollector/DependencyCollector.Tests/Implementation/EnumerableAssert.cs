namespace Microsoft.ApplicationInsights.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A static class collection of assertions that operate on IEnumerable.
    /// </summary>
    public static class EnumerableAssert
    {
        /// <summary>
        /// Determine if the contents of the provided IEnumerable are equal and in the same order.
        /// </summary>
        public static void AreEqual<T>(IEnumerable<T> lhs, IEnumerable<T> rhs)
        {
            if (lhs != rhs)
            {
                Assert.IsNotNull(lhs);
                Assert.IsNotNull(rhs);
                CollectionAssert.AreEqual(lhs.ToArray(), rhs.ToArray());
            }
        }
    }
}
