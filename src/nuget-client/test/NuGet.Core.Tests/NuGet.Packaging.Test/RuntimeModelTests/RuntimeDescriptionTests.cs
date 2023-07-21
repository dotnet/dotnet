// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System;
using System.Linq;
using Xunit;

namespace NuGet.RuntimeModel;

public sealed class RuntimeDescriptionTests
{
    [Fact]
    public void Merge_DifferentIdsThrows()
    {
        RuntimeDescription l = new(runtimeIdentifier: "rid-left");
        RuntimeDescription r = new(runtimeIdentifier: "rid-right");

        Assert.Throws<InvalidOperationException>(() => RuntimeDescription.Merge(l, r));
    }

    [Fact]
    public void Merge_PreservedRuntimeIdentifier()
    {
        RuntimeDescription l = new(runtimeIdentifier: "rid");
        RuntimeDescription r = new(runtimeIdentifier: "rid");

        var merged = RuntimeDescription.Merge(l, r);

        Assert.Equal("rid", merged.RuntimeIdentifier);
    }

    [Fact]
    public void Merge_PrefersRightImports()
    {
        RuntimeDescription l = new(runtimeIdentifier: "rid", new[] { "InheritedA" }, Array.Empty<RuntimeDependencySet>());
        RuntimeDescription r = new(runtimeIdentifier: "rid", new[] { "InheritedB" }, Array.Empty<RuntimeDependencySet>());

        var merged = RuntimeDescription.Merge(l, r);

        Assert.Equal(new[] { "InheritedA" }, merged.InheritedRuntimes);
    }

    [Fact]
    public void Merge_UsesLeftImportsIfRightEmpty()
    {
        RuntimeDescription l = new(runtimeIdentifier: "rid", Array.Empty<string>(),  Array.Empty<RuntimeDependencySet>());
        RuntimeDescription r = new(runtimeIdentifier: "rid", new[] { "InheritedB" }, Array.Empty<RuntimeDependencySet>());

        var merged = RuntimeDescription.Merge(l, r);

        Assert.Equal(new[] { "InheritedB" }, merged.InheritedRuntimes);
    }

    [Fact]
    public void Merge_EmptyImportsWhenNoneProvided()
    {
        RuntimeDescription l = new(runtimeIdentifier: "rid", Array.Empty<string>(), Array.Empty<RuntimeDependencySet>());
        RuntimeDescription r = new(runtimeIdentifier: "rid", Array.Empty<string>(), Array.Empty<RuntimeDependencySet>());

        var merged = RuntimeDescription.Merge(l, r);

        Assert.Empty(merged.InheritedRuntimes);
    }

    [Fact]
    public void Merge_CombinedDependencySets()
    {
        RuntimeDescription l = new(runtimeIdentifier: "rid", Array.Empty<string>(), new[] { new RuntimeDependencySet("SetA") });
        RuntimeDescription r = new(runtimeIdentifier: "rid", Array.Empty<string>(), new[] { new RuntimeDependencySet("SetB") });

        var merged = RuntimeDescription.Merge(l, r);

        Assert.Collection(
            merged.RuntimeDependencySets,
            o => Assert.Same(l.RuntimeDependencySets.Single().Value, o.Value),
            o => Assert.Same(r.RuntimeDependencySets.Single().Value, o.Value));
    }

    [Fact]
    public void Merge_CombinedDependencySets_IdentifiersDifferByCase()
    {
        RuntimeDescription l = new(runtimeIdentifier: "rid", Array.Empty<string>(), new[] { new RuntimeDependencySet("SetA") });
        RuntimeDescription r = new(runtimeIdentifier: "rid", Array.Empty<string>(), new[] { new RuntimeDependencySet("SETA") });

        var merged = RuntimeDescription.Merge(l, r);

        Assert.Collection(
            merged.RuntimeDependencySets,
            o => Assert.Same(r.RuntimeDependencySets.Single().Value, o.Value));
    }
}
