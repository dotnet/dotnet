// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

#nullable enable

namespace NuGet.DependencyResolver.Core.Tests;

public class TrackerTests
{
    [Fact]
    public void IsDisputed()
    {
        var itemA1 = MakeItem("A", 1);
        var itemA2 = MakeItem("A", 2);
        var itemB1 = MakeItem("B", 1);

        var tracker = new Tracker<string>();

        Assert.False(tracker.IsDisputed(itemA1));
        Assert.False(tracker.IsDisputed(itemA2));
        Assert.False(tracker.IsDisputed(itemB1));

        tracker.Track(itemA1);

        Assert.False(tracker.IsDisputed(itemA1));
        Assert.False(tracker.IsDisputed(itemA2));
        Assert.False(tracker.IsDisputed(itemB1));

        tracker.Track(itemA1);

        Assert.False(tracker.IsDisputed(itemA1));
        Assert.False(tracker.IsDisputed(itemA2));
        Assert.False(tracker.IsDisputed(itemB1));

        tracker.Track(itemA2);

        Assert.True(tracker.IsDisputed(itemA1));
        Assert.True(tracker.IsDisputed(itemA2));
        Assert.False(tracker.IsDisputed(itemB1));

        tracker.Track(itemB1);

        Assert.True(tracker.IsDisputed(itemA1));
        Assert.True(tracker.IsDisputed(itemA2));
        Assert.False(tracker.IsDisputed(itemB1));

        tracker.Clear();

        Assert.False(tracker.IsDisputed(itemA1));
        Assert.False(tracker.IsDisputed(itemA2));
        Assert.False(tracker.IsDisputed(itemB1));
    }

    [Fact]
    public void GetDisputes()
    {
        var itemA1 = MakeItem("A", 1);
        var itemA2 = MakeItem("A", 2);
        var itemB1 = MakeItem("B", 1);

        var tracker = new Tracker<string>();

        Assert.Empty(tracker.GetDisputes(itemA1));
        Assert.Empty(tracker.GetDisputes(itemA2));
        Assert.Empty(tracker.GetDisputes(itemB1));

        tracker.Track(itemA1);

        Validate(itemA1, new[] { itemA1 });
        Validate(itemA2, new[] { itemA1 });
        Assert.Empty(tracker.GetDisputes(itemB1));

        tracker.Track(itemA2);

        Validate(itemA1, new[] { itemA1, itemA2 });
        Validate(itemA2, new[] { itemA1, itemA2 });
        Assert.Empty(tracker.GetDisputes(itemB1));

        tracker.Clear();

        Assert.Empty(tracker.GetDisputes(itemA1));
        Assert.Empty(tracker.GetDisputes(itemA2));
        Assert.Empty(tracker.GetDisputes(itemB1));

        void Validate(GraphItem<string> item, GraphItem<string>[] expected)
        {
            IEnumerable<GraphItem<string>> disputes = tracker.GetDisputes(item);

            Assert.True(new HashSet<GraphItem<string>>(disputes).SetEquals(expected));
        }
    }

    [Fact]
    public void IsAmbiguous()
    {
        var itemA1 = MakeItem("A", 1);
        var itemB1 = MakeItem("B", 1);

        var tracker = new Tracker<string>();

        Assert.False(tracker.IsAmbiguous(itemA1));
        Assert.False(tracker.IsAmbiguous(itemB1));

        tracker.MarkAmbiguous(itemA1);

        Assert.True(tracker.IsAmbiguous(itemA1));
        Assert.False(tracker.IsAmbiguous(itemB1));

        tracker.MarkAmbiguous(itemB1);

        Assert.True(tracker.IsAmbiguous(itemA1));
        Assert.True(tracker.IsAmbiguous(itemB1));

        tracker.Clear();

        Assert.False(tracker.IsAmbiguous(itemA1));
        Assert.False(tracker.IsAmbiguous(itemB1));
    }

    [Fact]
    public void IsBestVersion()
    {
        var itemA1 = MakeItem("A", 1);
        var itemA2 = MakeItem("A", 2);
        var itemA3 = MakeItem("A", 3);
        var itemB1 = MakeItem("B", 1);

        var tracker = new Tracker<string>();

        // NOTE this is strange behavior. An item that was never tracked is reported as the best version.
        // However this existed at the time the tests were written, so may not be safe to change.
        // These tests reflect the current behavior, which might not be the most sensible.

        Assert.True(tracker.IsBestVersion(itemA1));
        Assert.True(tracker.IsBestVersion(itemA2));
        Assert.True(tracker.IsBestVersion(itemA3));
        Assert.True(tracker.IsBestVersion(itemB1));

        tracker.Track(itemA1);

        Assert.True(tracker.IsBestVersion(itemA1));
        Assert.True(tracker.IsBestVersion(itemA2));
        Assert.True(tracker.IsBestVersion(itemA3));
        Assert.True(tracker.IsBestVersion(itemB1));

        tracker.Track(itemA2);

        Assert.False(tracker.IsBestVersion(itemA1));
        Assert.True(tracker.IsBestVersion(itemA2));
        Assert.True(tracker.IsBestVersion(itemA3));
        Assert.True(tracker.IsBestVersion(itemB1));

        tracker.Track(itemA3);

        Assert.False(tracker.IsBestVersion(itemA1));
        Assert.False(tracker.IsBestVersion(itemA2));
        Assert.True(tracker.IsBestVersion(itemA3));
        Assert.True(tracker.IsBestVersion(itemB1));

        tracker.Track(itemB1);

        Assert.False(tracker.IsBestVersion(itemA1));
        Assert.False(tracker.IsBestVersion(itemA2));
        Assert.True(tracker.IsBestVersion(itemA3));
        Assert.True(tracker.IsBestVersion(itemB1));

        tracker.Clear();

        Assert.True(tracker.IsBestVersion(itemA1));
        Assert.True(tracker.IsBestVersion(itemA2));
        Assert.True(tracker.IsBestVersion(itemA3));
        Assert.True(tracker.IsBestVersion(itemB1));
    }

    private static GraphItem<string> MakeItem(string name, int version)
    {
        var key = new LibraryModel.LibraryIdentity()
        {
            Name = name,
            Version = new Versioning.NuGetVersion(version, 0, 0)
        };

        return new GraphItem<string>(key);
    }
}
