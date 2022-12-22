// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Mappings;

namespace Microsoft.DotNet.AsmDiff.CSV
{
    public interface IDiffCsvColumn
    {
        bool IsVisible { get; }
        string Name { get; }
        string GetValue<TElement>(ElementMapping<TElement> mapping) where TElement : class;
    }
}
