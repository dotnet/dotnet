// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.AspNetCore.Razor.Utilities.Shared.Test.TestData;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Utilities.Shared.Test.PooledObjects;

public class PooledArrayBuilderOrderingTests : ImmutableArrayOrderingTestBase
{
    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void ToImmutableOrdered(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        using var builder = new PooledArrayBuilder<int>(capacity: data.Length);
        builder.AddRange(data);

        var sorted = builder.ToImmutableOrdered();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void ToImmutableOrdered_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        using var builder = new PooledArrayBuilder<int>(capacity: data.Length);
        builder.AddRange(data);

        var sorted = builder.ToImmutableOrdered(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void ToImmutableOrderedDescending(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        using var builder = new PooledArrayBuilder<int>(capacity: data.Length);
        builder.AddRange(data);

        var sorted = builder.ToImmutableOrderedDescending();
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void ToImmutableOrderedDescending_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        using var builder = new PooledArrayBuilder<int>(capacity: data.Length);
        builder.AddRange(data);

        var sorted = builder.ToImmutableOrderedDescending(OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void ToImmutableOrderedBy(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        using var builder = new PooledArrayBuilder<ValueHolder<int>>(capacity: data.Length);
        builder.AddRange(data);

        var sorted = builder.ToImmutableOrderedBy(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void ToImmutableOrderedBy_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        using var builder = new PooledArrayBuilder<ValueHolder<int>>(capacity: data.Length);
        builder.AddRange(data);

        var sorted = builder.ToImmutableOrderedBy(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void ToImmutableOrderedByDescending(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        using var builder = new PooledArrayBuilder<ValueHolder<int>>(capacity: data.Length);
        builder.AddRange(data);

        var sorted = builder.ToImmutableOrderedByDescending(static x => x.Value);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void ToImmutableOrderedByDescending_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        using var builder = new PooledArrayBuilder<ValueHolder<int>>(capacity: data.Length);
        builder.AddRange(data);

        var sorted = builder.ToImmutableOrderedByDescending(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
    }

    [Theory]
    [MemberData(nameof(OrderTestData))]
    public void DrainToImmutableOrdered(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builderPool = TestArrayBuilderPool<int>.Create();
        using var builder = new PooledArrayBuilder<int>(capacity: data.Length, builderPool);
        builder.AddRange(data);

        var sorted = builder.DrainToImmutableOrdered();
        AssertEqual(expected, sorted);
        AssertIsDrained(in builder);
    }

    [Theory]
    [MemberData(nameof(OrderTestData_OddBeforeEven))]
    public void DrainToImmutableOrdered_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builderPool = TestArrayBuilderPool<int>.Create();
        using var builder = new PooledArrayBuilder<int>(capacity: data.Length, builderPool);
        builder.AddRange(data);

        var sorted = builder.DrainToImmutableOrdered(OddBeforeEven);
        AssertEqual(expected, sorted);
        AssertIsDrained(in builder);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData))]
    public void DrainToImmutableOrderedDescending(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builderPool = TestArrayBuilderPool<int>.Create();
        using var builder = new PooledArrayBuilder<int>(capacity: data.Length, builderPool);
        builder.AddRange(data);

        var sorted = builder.DrainToImmutableOrderedDescending();
        AssertEqual(expected, sorted);
        AssertIsDrained(in builder);
    }

    [Theory]
    [MemberData(nameof(OrderDescendingTestData_OddBeforeEven))]
    public void DrainToImmutableOrderedDescending_OddBeforeEven(ImmutableArray<int> data, ImmutableArray<int> expected)
    {
        var builderPool = TestArrayBuilderPool<int>.Create();
        using var builder = new PooledArrayBuilder<int>(capacity: data.Length, builderPool);
        builder.AddRange(data);

        var sorted = builder.DrainToImmutableOrderedDescending(OddBeforeEven);
        AssertEqual(expected, sorted);
        AssertIsDrained(in builder);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData))]
    public void DrainToImmutableOrderedBy(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builderPool = TestArrayBuilderPool<ValueHolder<int>>.Create();
        using var builder = new PooledArrayBuilder<ValueHolder<int>>(capacity: data.Length, builderPool);
        builder.AddRange(data);

        var sorted = builder.DrainToImmutableOrderedBy(static x => x.Value);
        AssertEqual(expected, sorted);
        AssertIsDrained(in builder);
    }

    [Theory]
    [MemberData(nameof(OrderByTestData_OddBeforeEven))]
    public void DrainToImmutableOrderedBy_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builderPool = TestArrayBuilderPool<ValueHolder<int>>.Create();
        using var builder = new PooledArrayBuilder<ValueHolder<int>>(capacity: data.Length, builderPool);
        builder.AddRange(data);

        var sorted = builder.DrainToImmutableOrderedBy(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
        AssertIsDrained(in builder);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData))]
    public void DrainToImmutableOrderedByDescending(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builderPool = TestArrayBuilderPool<ValueHolder<int>>.Create();
        using var builder = new PooledArrayBuilder<ValueHolder<int>>(capacity: data.Length, builderPool);
        builder.AddRange(data);

        var sorted = builder.DrainToImmutableOrderedByDescending(static x => x.Value);
        AssertEqual(expected, sorted);
        AssertIsDrained(in builder);
    }

    [Theory]
    [MemberData(nameof(OrderByDescendingTestData_OddBeforeEven))]
    public void DrainToImmutableOrderedByDescending_OddBeforeEven(ImmutableArray<ValueHolder<int>> data, ImmutableArray<ValueHolder<int>> expected)
    {
        var builderPool = TestArrayBuilderPool<ValueHolder<int>>.Create();
        using var builder = new PooledArrayBuilder<ValueHolder<int>>(capacity: data.Length, builderPool);
        builder.AddRange(data);

        var sorted = builder.DrainToImmutableOrderedByDescending(static x => x.Value, OddBeforeEven);
        AssertEqual(expected, sorted);
        AssertIsDrained(in builder);
    }

    private static void AssertIsDrained<T>(ref readonly PooledArrayBuilder<T> builder)
    {
        builder.Validate(static t =>
        {
            Assert.NotNull(t.InnerArrayBuilder);
            Assert.Empty(t.InnerArrayBuilder);

            // After draining, the capacity of the inner array builder should be 0.
            Assert.Equal(0, t.InnerArrayBuilder.Capacity);
        });
    }
}
