// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Xunit;

namespace NuGet.Commands.Test
{
    public class MSBuildRestoreItemGroupTests
    {
        [Fact]
        public void MSBuildRestoreItemGroupTest_VerifyEmptyCondition()
        {
            // Arrange
            var group = new MSBuildRestoreItemGroup();

            // Act
            var condition = group.Condition;

            // Assert
            Assert.Equal(string.Empty, condition);
        }

        [Fact]
        public void MSBuildRestoreItemGroupTest_SingleCondition()
        {
            // Arrange
            var group = new MSBuildRestoreItemGroup();
            group.Conditions.Add("'$(a)' == 'a'  ");

            // Act
            var condition = group.Condition;

            // Assert
            Assert.Equal(" '$(a)' == 'a' ", condition);
        }

        [Fact]
        public void MSBuildRestoreItemGroupTest_MultipleConditions()
        {
            // Arrange
            var group = new MSBuildRestoreItemGroup();
            group.Conditions.Add("'$(b)' != 'b'  ");
            group.Conditions.Add("    '$(a)' == 'a'  ");

            // Act
            var condition = group.Condition;

            // Assert
            Assert.Equal(" '$(b)' != 'b' AND '$(a)' == 'a' ", condition);
        }
    }
}
