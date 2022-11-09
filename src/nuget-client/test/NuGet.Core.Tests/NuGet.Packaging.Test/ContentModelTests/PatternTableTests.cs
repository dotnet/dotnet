// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.ContentModel;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.Client.Test
{
    public class PatternTableTests
    {
        [Fact]
        public void PatternTable_LookupWhenEmpty()
        {
            // Arrange
            var table = new PatternTable();

            // Act
            object obj;
            var b = table.TryLookup("tfm", "any", out obj);

            // Assert
            Assert.False(b);
            Assert.Null(obj);
        }

        [Fact]
        public void PatternTable_LookupWithValue()
        {
            // Arrange
            var dotnet = NuGetFramework.Parse("dotnet");
            var data = new List<PatternTableEntry>()
            {
                new PatternTableEntry("tfm", "any", dotnet)
            };

            var table = new PatternTable(data);

            // Act
            object obj;
            var b = table.TryLookup("tfm", "any", out obj);

            // Assert
            Assert.True(b);
            Assert.Equal(dotnet, obj);
        }

        [Fact]
        public void PatternTable_LookupWithMiss()
        {
            // Arrange
            var data = new List<PatternTableEntry>()
            {
                new PatternTableEntry("tfm", "dotnet", NuGetFramework.Parse("dotnet"))
            };

            var table = new PatternTable(data);

            // Act
            object obj;
            var b = table.TryLookup("tfm", "any", out obj);

            // Assert
            Assert.False(b);
            Assert.Null(obj);
        }
    }
}
