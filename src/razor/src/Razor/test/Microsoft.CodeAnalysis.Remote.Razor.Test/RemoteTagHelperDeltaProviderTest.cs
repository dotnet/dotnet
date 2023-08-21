// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Remote.Razor.Test;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.CodeAnalysis.Remote.Razor;

public class RemoteTagHelperDeltaProviderTest(ITestOutputHelper testOutput) : TagHelperDescriptorTestBase(testOutput)
{
    private readonly RemoteTagHelperDeltaProvider _provider = new();

    [Fact]
    public void GetTagHelpersDelta_Clean_SingleProject()
    {
        // Act
        var delta = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelpers);

        // Assert
        Assert.False(delta.Delta);
        Assert.Equal(Project1TagHelpers, delta.Added);
        Assert.Empty(delta.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_Clean_MultiProject()
    {
        // Act
        var delta1 = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelpers);
        var delta2 = _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelpers);

        // Assert
        Assert.False(delta1.Delta);
        Assert.Equal(Project1TagHelpers, delta1.Added);
        Assert.Empty(delta1.Removed);
        Assert.False(delta2.Delta);
        Assert.Equal(Project2TagHelpers, delta2.Added);
        Assert.Empty(delta2.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_TagHelperRemovedFromProjectOne_InvalidResultId()
    {
        // Arrange
        var tagHelpersWithOneRemoved = ImmutableArray.Create(TagHelper1_Project1);
        _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelpers);
        _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelpers);

        // Act
        var delta = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1337, tagHelpersWithOneRemoved);

        // Assert
        Assert.False(delta.Delta);
        Assert.Equal(tagHelpersWithOneRemoved, delta.Added);
        Assert.Empty(delta.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_TagHelperRemovedFromProjectOne()
    {
        // Arrange
        var tagHelpersWithOneRemoved = ImmutableArray.Create(TagHelper1_Project1);
        var initialDelta = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelpers);
        _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelpers);

        // Act
        var delta = _provider.GetTagHelpersDelta(Project1Id, initialDelta.ResultId, tagHelpersWithOneRemoved);

        // Assert
        Assert.True(delta.Delta);
        Assert.Empty(delta.Added);
        var tagHelper = Assert.Single(delta.Removed);
        Assert.Equal(TagHelper2_Project1, tagHelper);
    }

    [Fact]
    public void GetTagHelpersDelta_TagHelpersCopiedToProjectOne()
    {
        // Arrange
        var tagHelpers = ImmutableArray.CreateBuilder<TagHelperDescriptor>();
        tagHelpers.AddRange(Project1TagHelpers);
        tagHelpers.AddRange(Project2TagHelpers);
        var initialDelta = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelpers);
        _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelpers);

        // Act
        var delta = _provider.GetTagHelpersDelta(Project1Id, initialDelta.ResultId, tagHelpers.ToImmutableArray());

        // Assert
        Assert.True(delta.Delta);
        Assert.Equal(Project2TagHelpers, delta.Added, TagHelperDescriptorComparer.Default);
        Assert.Empty(delta.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_NoChange()
    {
        // Arrange
        var initialDelta = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelpers);

        // Act
        var delta = _provider.GetTagHelpersDelta(Project1Id, initialDelta.ResultId, Project1TagHelpers);

        // Assert
        Assert.True(delta.Delta);
        Assert.Empty(delta.Added);
        Assert.Empty(delta.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_NoChange_MultipleRequests()
    {
        // Arrange
        var project1Delta0 = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelpers);
        var project2Delta0 = _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelpers);

        // Act
        var project2Delta = _provider.GetTagHelpersDelta(Project2Id, project2Delta0.ResultId, Project2TagHelpers);
        var project1Delta1 = _provider.GetTagHelpersDelta(Project1Id, project1Delta0.ResultId, Project1TagHelpers);
        var project1Delta2 = _provider.GetTagHelpersDelta(Project1Id, project1Delta1.ResultId, Project1TagHelpers);

        // Assert
        Assert.True(project1Delta1.Delta);
        Assert.Empty(project1Delta1.Added);
        Assert.Empty(project1Delta1.Removed);
        Assert.True(project2Delta.Delta);
        Assert.Empty(project2Delta.Added);
        Assert.Empty(project2Delta.Removed);
        Assert.True(project1Delta2.Delta);
        Assert.Empty(project1Delta2.Added);
        Assert.Empty(project1Delta2.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_EndToEnd()
    {
        // Arrange
        var mixedTagHelpers1 = ImmutableArray.Create(TagHelper1_Project1, TagHelper1_Project2);
        var mixedTagHelpers2 = ImmutableArray.Create(TagHelper2_Project1, TagHelper2_Project2);

        var initialDelta1 = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelpers);
        var initialDelta2 = _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelpers);

        // Act - 1
        var delta1 = _provider.GetTagHelpersDelta(Project1Id, initialDelta1.ResultId, mixedTagHelpers1);
        var delta2 = _provider.GetTagHelpersDelta(Project2Id, initialDelta2.ResultId, mixedTagHelpers2);

        // Assert - 1
        Assert.True(delta1.Delta);
        Assert.Equal(new[] { TagHelper1_Project2 }, delta1.Added);
        Assert.Equal(new[] { TagHelper2_Project1 }, delta1.Removed);
        Assert.True(delta2.Delta);
        Assert.Equal(new[] { TagHelper2_Project1 }, delta2.Added);
        Assert.Equal(new[] { TagHelper1_Project2 }, delta2.Removed);

        // Act - 2 (restore to original state)
        delta1 = _provider.GetTagHelpersDelta(Project1Id, delta1.ResultId, Project1TagHelpers);
        delta2 = _provider.GetTagHelpersDelta(Project2Id, delta2.ResultId, Project2TagHelpers);

        // Assert - 2
        Assert.True(delta1.Delta);
        Assert.Equal(new[] { TagHelper2_Project1 }, delta1.Added);
        Assert.Equal(new[] { TagHelper1_Project2 }, delta1.Removed);
        Assert.True(delta2.Delta);
        Assert.Equal(new[] { TagHelper1_Project2 }, delta2.Added);
        Assert.Equal(new[] { TagHelper2_Project1 }, delta2.Removed);

        // Act - 3 (No-op)
        delta1 = _provider.GetTagHelpersDelta(Project1Id, delta1.ResultId, Project1TagHelpers);
        delta2 = _provider.GetTagHelpersDelta(Project2Id, delta2.ResultId, Project2TagHelpers);

        // Assert - 3
        Assert.True(delta1.Delta);
        Assert.Empty(delta1.Added);
        Assert.Empty(delta1.Removed);
        Assert.True(delta2.Delta);
        Assert.Empty(delta2.Added);
        Assert.Empty(delta2.Removed);
    }
}
