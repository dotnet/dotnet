// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using FluentAssertions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class CheckForDuplicateNuGetItemsTaskTests
    {
        [Fact]
        public void Execute_WithoutDuplicateItems_ReturnsTrue()
        {
            // Arrange
            var buildEngine = new TestBuildEngine();

            var packageX = new TaskItem
            {
                ItemSpec = "x"
            };
            packageX.SetMetadata("Version", "[1.0.0]");

            var packageY = new TaskItem
            {
                ItemSpec = "y"
            };
            packageY.SetMetadata("Version", "2.0.0");

            var items = new ITaskItem[]
            {
                packageX,
                packageY,
            };

            var task = new CheckForDuplicateNuGetItemsTask()
            {
                BuildEngine = buildEngine,
                ItemName = "PackageReference",
                Items = items,
                LogCode = "NU1504"
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeTrue();
            task.DeduplicatedItems.Length.Should().Be(0);
            buildEngine.TestLogger.Messages.Should().HaveCount(0);
        }

        [Fact]
        public void Execute_WithDuplicateItems_LogsWarning_ReturnsTrue()
        {
            // Arrange
            var buildEngine = new TestBuildEngine();

            var packageX1 = new TaskItem
            {
                ItemSpec = "x"
            };
            packageX1.SetMetadata("Version", "[1.0.0]");

            var packageX2 = new TaskItem
            {
                ItemSpec = "x",
            };
            packageX2.SetMetadata("Version", "2.0.0");

            var items = new ITaskItem[]
            {
                packageX1,
                packageX2,
            };

            var task = new CheckForDuplicateNuGetItemsTask()
            {
                BuildEngine = buildEngine,
                ItemName = "PackageReference",
                Items = items,
                LogCode = "NU1504"
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeTrue();
            task.DeduplicatedItems.Length.Should().Be(1);
            buildEngine.TestLogger.WarningMessages.Should().HaveCount(1);
        }

        [Fact]
        public void Execute_WithDuplicateItems_SelectsFirstItem()
        {
            // Arrange
            var buildEngine = new TestBuildEngine();

            var packageX1 = new TaskItem
            {
                ItemSpec = "x"
            };
            packageX1.SetMetadata("Version", "[1.0.0]");

            var packageX2 = new TaskItem
            {
                ItemSpec = "x",
            };
            packageX2.SetMetadata("Version", "2.0.0");

            var items = new ITaskItem[]
            {
                packageX1,
                packageX2,
            };

            var task = new CheckForDuplicateNuGetItemsTask()
            {
                BuildEngine = buildEngine,
                ItemName = "PackageReference",
                Items = items,
                LogCode = "NU1504"
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeTrue();
            buildEngine.TestLogger.WarningMessages.Should().HaveCount(1);
            task.DeduplicatedItems.Length.Should().Be(1);
            var item = task.DeduplicatedItems.Single();
            item.ItemSpec.Should().Be("x");
            item.GetMetadata("Version").Should().Be("[1.0.0]");
        }

        [Fact]
        public void Execute_WithMultipleDuplicateItems_DeduplicatesAllItems()
        {
            // Arrange
            var buildEngine = new TestBuildEngine();

            var packageX1 = new TaskItem
            {
                ItemSpec = "x"
            };
            packageX1.SetMetadata("Version", "[1.0.0]");

            var packageX2 = new TaskItem
            {
                ItemSpec = "x",
            };
            packageX2.SetMetadata("Version", "2.0.0");

            var packageY1 = new TaskItem
            {
                ItemSpec = "y",
            };
            packageY1.SetMetadata("Version", "1.0.0");

            var packageZ1 = new TaskItem
            {
                ItemSpec = "z",
            };
            packageZ1.SetMetadata("Version", "2.0.0");

            var packageZ2 = new TaskItem
            {
                ItemSpec = "z",
            };
            packageZ2.SetMetadata("Version", "3.0.0");
            var items = new ITaskItem[]
            {
                packageX1,
                packageX2,
                packageY1,
                packageZ1,
                packageZ2,
            };

            var task = new CheckForDuplicateNuGetItemsTask()
            {
                BuildEngine = buildEngine,
                ItemName = "PackageReference",
                Items = items,
                LogCode = "NU1504"
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeTrue();
            buildEngine.TestLogger.WarningMessages.Should().HaveCount(1);
            task.DeduplicatedItems.Length.Should().Be(3);

            var dedupItems = task.DeduplicatedItems;
            dedupItems[0].ItemSpec.Should().Be("x");
            dedupItems[0].GetMetadata("Version").Should().Be("[1.0.0]");

            dedupItems[1].ItemSpec.Should().Be("y");
            dedupItems[1].GetMetadata("Version").Should().Be("1.0.0");

            dedupItems[2].ItemSpec.Should().Be("z");
            dedupItems[2].GetMetadata("Version").Should().Be("2.0.0");
        }

        [Fact]
        public void Execute_WithNoWarn_SuppressesWarning()
        {
            // Arrange
            var buildEngine = new TestBuildEngine();

            var packageX1 = new TaskItem
            {
                ItemSpec = "x"
            };
            packageX1.SetMetadata("Version", "[1.0.0]");

            var packageX2 = new TaskItem
            {
                ItemSpec = "x",
            };
            packageX2.SetMetadata("Version", "2.0.0");

            var items = new ITaskItem[]
            {
                packageX1,
                packageX2,
            };

            var task = new CheckForDuplicateNuGetItemsTask()
            {
                BuildEngine = buildEngine,
                ItemName = "PackageReference",
                Items = items,
                LogCode = "NU1504",
                NoWarn = "1234;5678;NU1504"
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeTrue();
            task.DeduplicatedItems.Length.Should().Be(1);
            buildEngine.TestLogger.WarningMessages.Should().HaveCount(0);
        }

        [Fact]
        public void Execute_WithTreatWarningsAsErrors_ReturnsFalse()
        {
            // Arrange
            var buildEngine = new TestBuildEngine();

            var packageX1 = new TaskItem
            {
                ItemSpec = "x"
            };
            packageX1.SetMetadata("Version", "[1.0.0]");

            var packageX2 = new TaskItem
            {
                ItemSpec = "x",
            };
            packageX2.SetMetadata("Version", "2.0.0");

            var items = new ITaskItem[]
            {
                packageX1,
                packageX2,
            };

            var task = new CheckForDuplicateNuGetItemsTask()
            {
                BuildEngine = buildEngine,
                ItemName = "PackageReference",
                Items = items,
                LogCode = "NU1504",
                NoWarn = "1234;5678",
                TreatWarningsAsErrors = "true"
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeFalse();
            task.DeduplicatedItems.Length.Should().Be(1);
            buildEngine.TestLogger.WarningMessages.Should().HaveCount(0);
            buildEngine.TestLogger.ErrorMessages.Should().HaveCount(1);
        }

        [Fact]
        public void Execute_WithTreatWarningsAsErrorsAndSuppressedWarning_ReturnsTrue()
        {
            // Arrange
            var buildEngine = new TestBuildEngine();

            var packageX1 = new TaskItem
            {
                ItemSpec = "x"
            };
            packageX1.SetMetadata("Version", "[1.0.0]");

            var packageX2 = new TaskItem
            {
                ItemSpec = "x",
            };
            packageX2.SetMetadata("Version", "2.0.0");

            var items = new ITaskItem[]
            {
                packageX1,
                packageX2,
            };

            var task = new CheckForDuplicateNuGetItemsTask()
            {
                BuildEngine = buildEngine,
                ItemName = "PackageReference",
                Items = items,
                LogCode = "NU1504",
                NoWarn = "1234;5678;NU1504",
                TreatWarningsAsErrors = "true"
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeTrue();
            task.DeduplicatedItems.Length.Should().Be(1);
            buildEngine.TestLogger.WarningMessages.Should().HaveCount(0);
            buildEngine.TestLogger.ErrorMessages.Should().HaveCount(0);
        }

        [Fact]
        public void Execute_WithWarningsAsErrors_ReturnsFalse()
        {
            // Arrange
            var buildEngine = new TestBuildEngine();

            var packageX1 = new TaskItem
            {
                ItemSpec = "x"
            };
            packageX1.SetMetadata("Version", "[1.0.0]");

            var packageX2 = new TaskItem
            {
                ItemSpec = "x",
            };
            packageX2.SetMetadata("Version", "2.0.0");

            var items = new ITaskItem[]
            {
                packageX1,
                packageX2,
            };

            var task = new CheckForDuplicateNuGetItemsTask()
            {
                BuildEngine = buildEngine,
                ItemName = "PackageReference",
                Items = items,
                LogCode = "NU1504",
                NoWarn = "1234;5678",
                TreatWarningsAsErrors = "false",
                WarningsAsErrors = "NU1504"
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeFalse();
            task.DeduplicatedItems.Length.Should().Be(1);
            buildEngine.TestLogger.WarningMessages.Should().HaveCount(0);
            buildEngine.TestLogger.ErrorMessages.Should().HaveCount(1);
        }

        [Fact]
        public void Execute_WithWarningsNotAsErrors_RaisesWarning()
        {
            // Arrange
            var buildEngine = new TestBuildEngine();

            var packageX1 = new TaskItem
            {
                ItemSpec = "x"
            };
            packageX1.SetMetadata("Version", "[1.0.0]");

            var packageX2 = new TaskItem
            {
                ItemSpec = "x",
            };
            packageX2.SetMetadata("Version", "2.0.0");

            var items = new ITaskItem[]
            {
                packageX1,
                packageX2,
            };

            var task = new CheckForDuplicateNuGetItemsTask()
            {
                BuildEngine = buildEngine,
                ItemName = "PackageReference",
                Items = items,
                LogCode = "NU1504",
                NoWarn = "1234;5678;NU1505",
                TreatWarningsAsErrors = "true",
                WarningsNotAsErrors = "NU1504",
            };

            // Act
            var result = task.Execute();

            // Assert
            result.Should().BeTrue();
            task.DeduplicatedItems.Length.Should().Be(1);
            buildEngine.TestLogger.WarningMessages.Should().HaveCount(1);
            buildEngine.TestLogger.ErrorMessages.Should().HaveCount(0);
        }
    }
}
