// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.AspNetCore.Razor.LanguageServer.Extensions;
using Xunit;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Test;

public class RangeExtensionsTests
{
    [Fact]
    public void CompareTo_StartAndEndAreSame_ReturnsZero()
    {
        // Arrange
        var range1 = new Range() { Start = new Position(1, 2), End = new Position(3, 4) };
        var range2 = new Range() { Start = new Position(1, 2), End = new Position(3, 4) };

        // Act
        var result = range1.CompareTo(range2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_StartOfThisRangeIsBeforeOther_ReturnsNegative()
    {
        // Arrange
        var range1 = new Range() { Start = new Position(1, 2), End = new Position(3, 4) };
        var range2 = new Range() { Start = new Position(2, 2), End = new Position(3, 4) };

        // Act
        var result = range1.CompareTo(range2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_EndOfThisRangeIsBeforeOther_ReturnsNegative()
    {
        // Arrange
        var range1 = new Range() { Start = new Position(1, 2), End = new Position(3, 4) };
        var range2 = new Range() { Start = new Position(1, 2), End = new Position(4, 4) };

        // Act
        var result = range1.CompareTo(range2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_StartOfThisRangeIsAfterOther_ReturnsPositive()
    {
        // Arrange
        var range1 = new Range() { Start = new Position(2, 2), End = new Position(3, 4) };
        var range2 = new Range() { Start = new Position(1, 2), End = new Position(3, 4) };

        // Act
        var result = range1.CompareTo(range2);

        // Assert
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareTo_EndOfThisRangeIsAfterOther_ReturnsPositive()
    {
        // Arrange
        var range1 = new Range() { Start = new Position(1, 2), End = new Position(4, 4) };
        var range2 = new Range() { Start = new Position(1, 2), End = new Position(3, 4) };

        // Act
        var result = range1.CompareTo(range2);

        // Assert
        Assert.True(result > 0);
    }
}
