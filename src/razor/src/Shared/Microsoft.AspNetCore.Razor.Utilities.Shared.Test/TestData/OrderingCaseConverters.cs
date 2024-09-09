// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test.TestData;

public interface IOrderingCaseConverter<TOrderCollection, TOrderByCollection>
    where TOrderCollection : IEnumerable<int>
    where TOrderByCollection : IEnumerable<ValueHolder<int>>
{
    TOrderCollection ConvertOrderCase(ImmutableArray<int> data);
    TOrderByCollection ConvertOrderByCase(ImmutableArray<ValueHolder<int>> data);
}

public static class OrderingCaseConverters
{
    public sealed class Enumerable : IOrderingCaseConverter<IEnumerable<int>, IEnumerable<ValueHolder<int>>>
    {
        public IEnumerable<int> ConvertOrderCase(ImmutableArray<int> data) => data;
        public IEnumerable<ValueHolder<int>> ConvertOrderByCase(ImmutableArray<ValueHolder<int>> data) => data;
    }

    public sealed class ImmutableArray : IOrderingCaseConverter<ImmutableArray<int>, ImmutableArray<ValueHolder<int>>>
    {
        public ImmutableArray<int> ConvertOrderCase(ImmutableArray<int> data) => data;
        public ImmutableArray<ValueHolder<int>> ConvertOrderByCase(ImmutableArray<ValueHolder<int>> data) => data;
    }

    public sealed class ReadOnlyList : IOrderingCaseConverter<IReadOnlyList<int>, IReadOnlyList<ValueHolder<int>>>
    {
        public IReadOnlyList<int> ConvertOrderCase(ImmutableArray<int> data) => data;
        public IReadOnlyList<ValueHolder<int>> ConvertOrderByCase(ImmutableArray<ValueHolder<int>> data) => data;
    }
}
