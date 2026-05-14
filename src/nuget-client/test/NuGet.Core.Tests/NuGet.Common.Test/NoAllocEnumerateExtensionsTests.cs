// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using Moq;
using Xunit;

namespace NuGet.Common.Test;

public class NoAllocEnumerateExtensionsTests
{
    #region IEnumerable

    [Fact]
    public void NoAllocEnumerate_IEnumerable_List()
    {
        ValidateIEnumerable(new List<int> { 0, 1, 2, 3 });
    }

    [Fact]
    public void NoAllocEnumerate_IEnumerable_HashSet()
    {
        var testSet = new HashSet<int> { 0, 1, 2, 3 };
        var newSet = new HashSet<int>(testSet.Count);
        foreach (int i in testSet.NoAllocEnumerate())
        {
            // ensure we got something that was actually in the original set
            Assert.Contains(i, testSet);

            // ensure we don't get duplicates
            Assert.True(newSet.Add(i));
        }

        // If every item we get from the enumerator exists in the source set
        // and we didn't add any duplicates, we know we enumerated over the same set
        // of items if the counts are equal.
        Assert.Equal(testSet.Count, newSet.Count);
    }

    [Fact]
    public void NoAllocEnumerate_IEnumerable_ImmutableList()
    {
        ValidateIEnumerable(ImmutableList.Create(0, 1, 2, 3));
    }

    [Fact]
    public void NoAllocEnumerate_IEnumerable_ImmutableArray()
    {
        ValidateIEnumerable(ImmutableArray.Create(0, 1, 2, 3));
    }

    [Fact]
    public void NoAllocEnumerate_IEnumerable_Fallback()
    {
        int[] array = { 0, 1, 2, 3 };

        var mock = new Mock<IEnumerable<int>>(MockBehavior.Strict);

        mock.Setup(o => o.GetEnumerator())
            .Returns(((IEnumerable<int>)array).GetEnumerator());

        ValidateIEnumerable(mock.Object);

        mock.Verify();
    }

    [Fact]
    public void NoAllocEnumerate_IEnumerable_ICollection_OptimizedForEmpty()
    {
        var mock = new Mock<ICollection<int>>(MockBehavior.Strict);

        mock.SetupGet(o => o.Count).Returns(0);

        // NOTE because the source is empty, GetEnumerator should not be called at all.

        foreach (int i in mock.Object.NoAllocEnumerate())
        {

        }

        mock.Verify();
    }

    private static void ValidateIEnumerable(IEnumerable<int> collection)
    {
        List<int> actual = new();

        foreach (var item in collection.NoAllocEnumerate())
        {
            actual.Add(item);
        }

        Assert.Equal(4, actual.Count);
        Assert.Equal(0, actual[0]);
        Assert.Equal(1, actual[1]);
        Assert.Equal(2, actual[2]);
        Assert.Equal(3, actual[3]);
    }

    #endregion

    #region IList

    [Fact]
    public void NoAllocEnumerate_IList_List()
    {
        ValidateIList(new List<int> { 0, 1, 2, 3 });
    }

    [Fact]
    public void NoAllocEnumerate_IList_ImmutableList()
    {
        ValidateIList(ImmutableList.Create(0, 1, 2, 3));
    }

    [Fact]
    public void NoAllocEnumerate_IList_ImmutableArray()
    {
        ValidateIList(ImmutableArray.Create(0, 1, 2, 3));
    }

    [Fact]
    public void NoAllocEnumerate_IList_Mocked()
    {
        int[] array = { 0, 1, 2, 3 };

        Mock<IList<int>> mock = new(MockBehavior.Strict);

        mock.SetupGet(o => o.Count)
            .Returns(array.Length);

        mock.SetupGet(o => o[0]).Returns(0);
        mock.SetupGet(o => o[1]).Returns(1);
        mock.SetupGet(o => o[2]).Returns(2);
        mock.SetupGet(o => o[3]).Returns(3);

        ValidateIList(mock.Object);

        mock.Verify();
    }

    [Fact]
    public void NoAllocEnumerate_IList_OptimizedForEmpty()
    {
        Mock<IList<int>> mock = new(MockBehavior.Strict);

        mock.SetupGet(o => o.Count).Returns(0);

        // NOTE because the source is empty, GetEnumerator should not be called at all.

        foreach (int i in mock.Object.NoAllocEnumerate())
        {

        }

        mock.Verify();
    }

    private static void ValidateIList(IList<int> collection)
    {
        List<int> actual = new();

        foreach (var item in collection.NoAllocEnumerate())
        {
            actual.Add(item);
        }

        Assert.Equal(4, actual.Count);
        Assert.Equal(0, actual[0]);
        Assert.Equal(1, actual[1]);
        Assert.Equal(2, actual[2]);
        Assert.Equal(3, actual[3]);
    }

    #endregion

    #region IDictionary

    [Fact]
    public void NoAllocEnumerate_IDictionary_Dictionary()
    {
        ValidateIDictionary(new Dictionary<int, int>() { [0] = 0, [1] = 1, [2] = 2, [3] = 3 });
    }

    [Fact]
    public void NoAllocEnumerate_IDictionary_ImmutableDictionary()
    {
        ValidateIDictionary(ImmutableDictionary<int, int>.Empty.Add(0, 0).Add(1, 1).Add(2, 2).Add(3, 3));
    }

    [Fact]
    public void NoAllocEnumerate_IDictionary_Fallback()
    {
        KeyValuePair<int, int>[] array =
        {
            new KeyValuePair<int, int>(0, 0),
            new KeyValuePair<int, int>(1, 1),
            new KeyValuePair<int, int>(2, 2),
            new KeyValuePair<int, int>(3, 3)
        };

        Mock<IDictionary<int, int>> mock = new(MockBehavior.Strict);

        mock.SetupGet(o => o.Count)
            .Returns(array.Length);
        mock.Setup(o => o.GetEnumerator())
            .Returns(((IEnumerable<KeyValuePair<int, int>>)array).GetEnumerator());

        ValidateIDictionary(mock.Object);

        mock.Verify();
    }

    [Fact]
    public void NoAllocEnumerate_IDictionary_OptimizedForEmpty()
    {
        Mock<IList<int>> mock = new(MockBehavior.Strict);

        mock.SetupGet(o => o.Count).Returns(0);

        // NOTE because the source is empty, GetEnumerator should not be called at all.

        foreach (int i in mock.Object.NoAllocEnumerate())
        {

        }

        mock.Verify();
    }

    private static void ValidateIDictionary(IDictionary<int, int> dictionary)
    {
        var actual = new List<KeyValuePair<int, int>>();

        foreach (var pair in dictionary.NoAllocEnumerate())
        {
            actual.Add(pair);
        }

        Assert.Equal(4, actual.Count);
        Assert.Equal(new KeyValuePair<int, int>(0, 0), actual[0]);
        Assert.Equal(new KeyValuePair<int, int>(1, 1), actual[1]);
        Assert.Equal(new KeyValuePair<int, int>(2, 2), actual[2]);
        Assert.Equal(new KeyValuePair<int, int>(3, 3), actual[3]);
    }

    #endregion
}
