// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Mappings;

namespace Microsoft.Cci.Differs
{
    public interface IElementDifferenceFactory
    {
        IDifferences GetDiffer<T>(ElementMapping<T> element) where T : class;
    }
}
