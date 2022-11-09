// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Differs;

namespace Microsoft.Cci.Filters
{
    public interface IDifferenceFilter
    {
        bool Include(Difference difference);
    }
}
