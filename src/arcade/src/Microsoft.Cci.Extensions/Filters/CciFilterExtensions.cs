// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.Cci.Filters
{
    public static partial class CciFilterExtensions
    {
        public static ICciFilter And(this ICciFilter left, ICciFilter right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new AndFilter(left, right);
        }

        public static ICciFilter Or(this ICciFilter left, ICciFilter right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));

            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new OrFilter(left, right);
        }

        public static ICciFilter Not(this ICciFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return new NegatedFilter(filter);
        }
    }
}
