// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language;

public class MetadataCollectionTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(32)]
    [InlineData(64)]
    public void CreateAndCompareCollections(int size)
    {
        var pairs = new List<KeyValuePair<string, string?>>();

        for (var i = 0; i < size; i++)
        {
            var key = i.ToString(CultureInfo.InvariantCulture);
            var value = (int.MaxValue - i).ToString(CultureInfo.InvariantCulture);

            pairs.Add(new(key, value));
        }

        var collection1 = MetadataCollection.Create(pairs.ToArray());
        var collection2 = MetadataCollection.Create(pairs.ToArray().Reverse().ToArray());

        Assert.Equal(collection1, collection2);

        foreach (var (key, value) in pairs)
        {
            Assert.True(collection1.TryGetValue(key, out var value1));
            Assert.Equal(value, value1);

            Assert.True(collection2.TryGetValue(key, out var value2));
            Assert.Equal(value, value2);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(32)]
    [InlineData(64)]
    public void EnumeratorReturnsAllItemsInCollection(int size)
    {
        var pairs = new List<KeyValuePair<string, string?>>();
        var map = new Dictionary<string, bool>();

        for (var i = 0; i < size; i++)
        {
            var key = i.ToString(CultureInfo.InvariantCulture);
            var value = (int.MaxValue - i).ToString(CultureInfo.InvariantCulture);

            pairs.Add(new(key, value));
            map.Add(key, false);
        }

        Assert.True(map.All(kvp => kvp.Value == false));

        var collection = MetadataCollection.Create(pairs);

        foreach (var (key, _) in collection)
        {
            map[key] = true;
        }

        Assert.True(map.All(kvp => kvp.Value == true));

        var enumerator = collection.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var (key, _) = enumerator.Current;
            map[key] = false;
        }

        Assert.True(map.All(kvp => kvp.Value == false));

        // Verify that reset works.
        enumerator.Reset();

        while (enumerator.MoveNext())
        {
            var (key, _) = enumerator.Current;
            map[key] = true;
        }

        Assert.True(map.All(kvp => kvp.Value == true));
    }

    [Fact]
    public void CreateThrowsOnDuplicateKeys()
    {
        var one = new KeyValuePair<string, string?>("Key1", "Value1");
        var two = new KeyValuePair<string, string?>("Key2", "Value2");
        var three = new KeyValuePair<string, string?>("Key3", "Value3");
        var four = new KeyValuePair<string, string?>("Key4", "Value4");

        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(one, one));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(new[] { one, one }));

        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(one, one, three));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(new[] { one, one, three }));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(one, three, three));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(new[] { one, three, three }));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(one, two, one));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(new[] { one, two, one }));

        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(one, one, three, four));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(new[] { one, one, three, four }));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(one, two, one, four));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(new[] { one, two, one, four }));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(one, two, three, one));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(new[] { one, two, three, one }));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(one, two, two, four));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(new[] { one, two, two, four }));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(one, two, three, two));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(new[] { one, one, three, two }));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(one, two, three, three));
        Assert.Throws<ArgumentException>(() => MetadataCollection.Create(new[] { one, one, three, three }));
    }

    [Fact]
    public void HashCodesAreSameRegardlessOfOrdering()
    {
        var one = new KeyValuePair<string, string?>("Key1", "Value1");
        var two = new KeyValuePair<string, string?>("Key2", "Value2");
        var three = new KeyValuePair<string, string?>("Key3", "Value3");
        var four = new KeyValuePair<string, string?>("Key4", "Value4");

        Assert.Equal(
            MetadataCollection.Create(one, two).GetHashCode(),
            MetadataCollection.Create(two, one).GetHashCode());

        Assert.Equal(
            MetadataCollection.Create(one, two, three).GetHashCode(),
            MetadataCollection.Create(two, one, three).GetHashCode());

        Assert.Equal(
            MetadataCollection.Create(one, two, three).GetHashCode(),
            MetadataCollection.Create(three, two, one).GetHashCode());

        Assert.Equal(
            MetadataCollection.Create(one, two, three).GetHashCode(),
            MetadataCollection.Create(one, three, two).GetHashCode());

        Assert.Equal(
            MetadataCollection.Create(one, two, three).GetHashCode(),
            MetadataCollection.Create(two, three, one).GetHashCode());

        Assert.Equal(
            MetadataCollection.Create(one, two, three).GetHashCode(),
            MetadataCollection.Create(three, one, two).GetHashCode());

        Assert.Equal(
            MetadataCollection.Create(one, two, three, four).GetHashCode(),
            MetadataCollection.Create(four, three, two, one).GetHashCode());
    }
}
