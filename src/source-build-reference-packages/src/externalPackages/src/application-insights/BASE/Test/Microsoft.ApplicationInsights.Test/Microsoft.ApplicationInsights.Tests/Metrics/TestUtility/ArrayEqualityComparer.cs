using System;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Metrics.TestUtility
{
    /// <summary />
    public class ArrayEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary />
        public bool Equals(T x, T y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var a1 = (object []) (object) x;
            var a2 = (object []) (object) y;
            return TestUtil.AreEqual(a1, a2);
        }

        /// <summary />
        public int GetHashCode(T obj)
        {
            if (obj == null)
            {
                return 0;
            }

            var a = (object[]) (object) obj;

            int hash = 17;
            unchecked
            {
                for (int i = 0; i < a.Length; i++)
                {
                    hash = hash * 31 + (a[i]?.GetHashCode() ?? 0);
                }
            }

            return hash;
        }
    }
}
