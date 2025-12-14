// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NuGet.Commands.Restore;
using NuGet.Commands.Restore.Utility;
using Xunit;

namespace NuGet.Commands.Test.RestoreCommandTests.Utility;

public class ProjectItemIdentityComparerTests
{
    [Fact]
    public void Distinct_WithDifferentPackageNames_DoesNotDeduplicate()
    {
        // Arrange
        var item1 = new TestItem("packagea", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            { { "Version", "1.0.0" } }
        );
        var item2 = new TestItem("packageb", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            { { "Version", "1.0.0" } }
        );
        List<IItem> items = [item1, item2];

        // Act
        var result = items.Distinct(ProjectItemIdentityComparer.Default).ToList();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public void Distinct_WithSamePackageNameDifferentVersions_Deduplicates()
    {
        // Arrange
        var item1 = new TestItem("packagea", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            { { "Version", "1.0.0" } }
        );
        var item2 = new TestItem("packagea", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            { { "Version", "2.0.0" } }
        );
        List<IItem> items = [item1, item2];

        // Act
        var result = items.Distinct(ProjectItemIdentityComparer.Default).ToList();

        // Assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public void Distinct_WithDuplicateMixedCasePackageName_Deduplicates()
    {
        // Arrange
        var item1 = new TestItem("packagea", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            { { "Version", "1.0.0" } }
        );
        var item2 = new TestItem("PackageA", new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            { { "Version", "1.0.0" } }
        );
        List<IItem> items = [item1, item2];

        // Act
        var result = items.Distinct(ProjectItemIdentityComparer.Default).ToList();

        // Assert
        result.Should().HaveCount(1);
    }

    private class TestItem : IItem
    {
        private readonly IReadOnlyDictionary<string, string> _metadata;

        public TestItem(string identity, IReadOnlyDictionary<string, string> metadata)
        {
            Identity = identity;
            _metadata = metadata;
        }
        public string Identity { get; }

        public string GetMetadata(string name)
        {
            if (_metadata.TryGetValue(name, out string? value))
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
            }
            return null!;
        }
    }
}
