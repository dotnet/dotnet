// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using FluentAssertions;
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class RestoreAuditPropertiesTests
    {
        [Fact]
        public void Equals_WithNull_ReturnsFalse()
        {
            // Arrange
            var properties = new RestoreAuditProperties();

            // Act & Assert
            properties.Equals((RestoreAuditProperties?)null).Should().BeFalse();
            properties!.Equals((object?)null).Should().BeFalse();
            (properties == null).Should().BeFalse();
            (null == properties).Should().BeFalse();
            (properties != null).Should().BeTrue();
            (null != properties).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithSameInstance_ReturnsTrue()
        {
            // Arrange
            var properties = new RestoreAuditProperties();

            // Act & Assert
            properties.Equals(properties).Should().BeTrue();
            properties.Equals((object)properties).Should().BeTrue();
#pragma warning disable CS1718 // Comparison made to same variable
            (properties == properties).Should().BeTrue();
            (properties != properties).Should().BeFalse();
#pragma warning restore CS1718 // Comparison made to same variable
        }

        [Fact]
        public void Equals_InstancesWithSameValues_ReturnsTrue()
        {
            // Arrange
            var properties1 = new RestoreAuditProperties()
            {
                AuditLevel = "moderate",
            };
            var properties2 = new RestoreAuditProperties()
            {
                AuditLevel = properties1.AuditLevel,
            };

            // Act & Assert
            properties1.Equals(properties2).Should().BeTrue();
            properties1.Equals((object)properties2).Should().BeTrue();
            (properties1 == properties2).Should().BeTrue();
            (properties1 != properties2).Should().BeFalse();
        }

        [Fact]
        public void Equals_InstancesWithDifferentValues_ReturnsFalse()
        {
            // Arrange
            var properties1 = new RestoreAuditProperties()
            {
                AuditLevel = "moderate",
            };
            var properties2 = new RestoreAuditProperties()
            {
                AuditLevel = "high",
            };

            // Act & Assert
            properties1.Equals(properties2).Should().BeFalse();
            properties1.Equals((object)properties2).Should().BeFalse(); ;
            (properties1 == properties2).Should().BeFalse();
            (properties1 != properties2).Should().BeTrue();
        }

        [Fact]
        public void Clone_ReturnsNewInstanceWithSameValues()
        {
            // Arrange
            var property1 = new RestoreAuditProperties();

            var type = typeof(RestoreAuditProperties);
            var typeProperties = type.GetProperties(System.Reflection.BindingFlags.Public);
            int propertyCount = 0;
            foreach (var property in typeProperties)
            {
                if (property.CanWrite)
                {
                    propertyCount++;
                    property.SetValue(property1, propertyCount.ToString());
                }
            }

            // Act
            var clone = property1.Clone();

            // Assert
            clone.Should().NotBeSameAs(property1);
            clone.Should().Be(property1);
        }
    }
}
