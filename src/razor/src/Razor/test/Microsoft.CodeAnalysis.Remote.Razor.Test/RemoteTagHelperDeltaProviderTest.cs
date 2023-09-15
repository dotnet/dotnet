// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Utilities;
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
        var delta = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelperChecksums);

        // Assert
        Assert.False(delta.IsDelta);
        Assert.Equal(Project1TagHelperChecksums, delta.Added);
        Assert.Empty(delta.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_Clean_MultiProject()
    {
        // Act
        var delta1 = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelperChecksums);
        var delta2 = _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelperChecksums);

        // Assert
        Assert.False(delta1.IsDelta);
        Assert.Equal(Project1TagHelperChecksums, delta1.Added);
        Assert.Empty(delta1.Removed);
        Assert.False(delta2.IsDelta);
        Assert.Equal(Project2TagHelperChecksums, delta2.Added);
        Assert.Empty(delta2.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_TagHelperRemovedFromProjectOne_InvalidResultId()
    {
        // Arrange
        var tagHelpersWithOneRemoved = ImmutableArray.Create(TagHelper1_Project1.GetChecksum());
        _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelperChecksums);
        _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelperChecksums);

        // Act
        var delta = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1337, tagHelpersWithOneRemoved);

        // Assert
        Assert.False(delta.IsDelta);
        Assert.Equal(tagHelpersWithOneRemoved, delta.Added);
        Assert.Empty(delta.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_TagHelperRemovedFromProjectOne()
    {
        // Arrange
        var tagHelpersWithOneRemoved = ImmutableArray.Create(TagHelper1_Project1.GetChecksum());
        var initialDelta = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelperChecksums);
        _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelperChecksums);

        // Act
        var delta = _provider.GetTagHelpersDelta(Project1Id, initialDelta.ResultId, tagHelpersWithOneRemoved);

        // Assert
        Assert.True(delta.IsDelta);
        Assert.Empty(delta.Added);
        var checksum = Assert.Single(delta.Removed);
        Assert.Equal(TagHelper2_Project1.GetChecksum(), checksum);
    }

    [Fact]
    public void GetTagHelpersDelta_TagHelpersCopiedToProjectOne()
    {
        // Arrange
        var tagHelpers = ImmutableArray.CreateBuilder<Checksum>();
        tagHelpers.AddRange(Project1TagHelperChecksums);
        tagHelpers.AddRange(Project2TagHelperChecksums);
        var initialDelta = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelperChecksums);
        _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelperChecksums);

        // Act
        var delta = _provider.GetTagHelpersDelta(Project1Id, initialDelta.ResultId, tagHelpers.ToImmutableArray());

        // Assert
        Assert.True(delta.IsDelta);
        Assert.Equal<Checksum>(Project2TagHelperChecksums, delta.Added);
        Assert.Empty(delta.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_NoChange()
    {
        // Arrange
        var initialDelta = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelperChecksums);

        // Act
        var delta = _provider.GetTagHelpersDelta(Project1Id, initialDelta.ResultId, Project1TagHelperChecksums);

        // Assert
        Assert.True(delta.IsDelta);
        Assert.Empty(delta.Added);
        Assert.Empty(delta.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_NoChange_MultipleRequests()
    {
        // Arrange
        var project1Delta0 = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelperChecksums);
        var project2Delta0 = _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelperChecksums);

        // Act
        var project2Delta = _provider.GetTagHelpersDelta(Project2Id, project2Delta0.ResultId, Project2TagHelperChecksums);
        var project1Delta1 = _provider.GetTagHelpersDelta(Project1Id, project1Delta0.ResultId, Project1TagHelperChecksums);
        var project1Delta2 = _provider.GetTagHelpersDelta(Project1Id, project1Delta1.ResultId, Project1TagHelperChecksums);

        // Assert
        Assert.True(project1Delta1.IsDelta);
        Assert.Empty(project1Delta1.Added);
        Assert.Empty(project1Delta1.Removed);
        Assert.True(project2Delta.IsDelta);
        Assert.Empty(project2Delta.Added);
        Assert.Empty(project2Delta.Removed);
        Assert.True(project1Delta2.IsDelta);
        Assert.Empty(project1Delta2.Added);
        Assert.Empty(project1Delta2.Removed);
    }

    [Fact]
    public void GetTagHelpersDelta_EndToEnd()
    {
        // Arrange
        var mixedTagHelpers1 = ImmutableArray.Create(TagHelper1_Project1.GetChecksum(), TagHelper1_Project2.GetChecksum());
        var mixedTagHelpers2 = ImmutableArray.Create(TagHelper2_Project1.GetChecksum(), TagHelper2_Project2.GetChecksum());

        var initialDelta1 = _provider.GetTagHelpersDelta(Project1Id, lastResultId: -1, Project1TagHelperChecksums);
        var initialDelta2 = _provider.GetTagHelpersDelta(Project2Id, lastResultId: -1, Project2TagHelperChecksums);

        // Act - 1
        var delta1 = _provider.GetTagHelpersDelta(Project1Id, initialDelta1.ResultId, mixedTagHelpers1);
        var delta2 = _provider.GetTagHelpersDelta(Project2Id, initialDelta2.ResultId, mixedTagHelpers2);

        // Assert - 1
        Assert.True(delta1.IsDelta);
        Assert.Equal(new[] { TagHelper1_Project2.GetChecksum() }, delta1.Added);
        Assert.Equal(new[] { TagHelper2_Project1.GetChecksum() }, delta1.Removed);
        Assert.True(delta2.IsDelta);
        Assert.Equal(new[] { TagHelper2_Project1.GetChecksum() }, delta2.Added);
        Assert.Equal(new[] { TagHelper1_Project2.GetChecksum() }, delta2.Removed);

        // Act - 2 (restore to original state)
        delta1 = _provider.GetTagHelpersDelta(Project1Id, delta1.ResultId, Project1TagHelperChecksums);
        delta2 = _provider.GetTagHelpersDelta(Project2Id, delta2.ResultId, Project2TagHelperChecksums);

        // Assert - 2
        Assert.True(delta1.IsDelta);
        Assert.Equal(new[] { TagHelper2_Project1.GetChecksum() }, delta1.Added);
        Assert.Equal(new[] { TagHelper1_Project2.GetChecksum() }, delta1.Removed);
        Assert.True(delta2.IsDelta);
        Assert.Equal(new[] { TagHelper1_Project2.GetChecksum() }, delta2.Added);
        Assert.Equal(new[] { TagHelper2_Project1.GetChecksum() }, delta2.Removed);

        // Act - 3 (No-op)
        delta1 = _provider.GetTagHelpersDelta(Project1Id, delta1.ResultId, Project1TagHelperChecksums);
        delta2 = _provider.GetTagHelpersDelta(Project2Id, delta2.ResultId, Project2TagHelperChecksums);

        // Assert - 3
        Assert.True(delta1.IsDelta);
        Assert.Empty(delta1.Added);
        Assert.Empty(delta1.Removed);
        Assert.True(delta2.IsDelta);
        Assert.Empty(delta2.Added);
        Assert.Empty(delta2.Removed);
    }
}
