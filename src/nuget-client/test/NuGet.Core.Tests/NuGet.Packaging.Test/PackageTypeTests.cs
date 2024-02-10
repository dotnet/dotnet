// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Xunit;

namespace NuGet.Packaging.Core.Test
{
    public class PackageTypeTests
    {
        [Fact]
        public void PackageType_Equals_WithDifferentName()
        {
            // Arrange
            var a = new PackageType("Foo", new Version(1, 0));
            var b = new PackageType("Bar", new Version(1, 0));

            // Act & Assert
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void PackageType_Equals_WithDifferentVersion()
        {
            // Arrange
            var a = new PackageType("Foo", new Version(1, 0));
            var b = new PackageType("Foo", new Version(2, 0));

            // Act & Assert
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void PackageType_Equals_DifferentNameCase()
        {
            // Arrange
            var a = new PackageType("FOO", new Version(1, 0));
            var b = new PackageType("foo", new Version(1, 0));

            // Act & Assert
            Assert.Equal(a, b);
        }

        [Fact]
        public void PackageType_Equals_SameNameAndVersion()
        {
            // Arrange
            var a = new PackageType("foo", new Version(1, 0));
            var b = new PackageType("foo", new Version(1, 0));

            // Act & Assert
            Assert.Equal(a, b);
        }

        [Fact]
        public void PackageType_Equals_Same()
        {
            // Arrange
            var a = new PackageType("foo", new Version(1, 0));
            var b = a;

            // Act & Assert
            Assert.Equal(a, b);
        }

        [Fact]
        public void PackageType_Equals_OtherType()
        {
            // Arrange
            var a = new PackageType("foo", new Version(1, 0));

            // Act & Assert
            Assert.False(a.Equals("foo"));
        }

        [Fact]
        public void PackageType_Equals_Null()
        {
            // Arrange
            var a = new PackageType("foo", new Version(1, 0));

            // Act & Assert
            Assert.False(a.Equals(null));
        }

        [Fact]
        public void PackageType_GetHashCode_Equal()
        {
            // Arrange
            var a = new PackageType("foo", new Version(1, 0));
            var b = new PackageType("foo", new Version(1, 0));

            // Act
            var aHashCode = a.GetHashCode();
            var bHashCode = b.GetHashCode();

            // Assert
            Assert.Equal(aHashCode, bHashCode);
        }

        [Fact]
        public void PackageType_GetHashCode_DifferentCase()
        {
            // Arrange
            var a = new PackageType("foo", new Version(1, 0));
            var b = new PackageType("FOO", new Version(1, 0));

            // Act
            var aHashCode = a.GetHashCode();
            var bHashCode = b.GetHashCode();

            // Assert
            Assert.Equal(aHashCode, bHashCode);
        }

        [Fact]
        public void PackageType_GetHashCode_Different()
        {
            // Arrange
            var a = new PackageType("foo", new Version(1, 0));
            var b = new PackageType("bar", new Version(1, 0));

            // Act
            var aHashCode = a.GetHashCode();
            var bHashCode = b.GetHashCode();

            // Assert
            Assert.NotEqual(aHashCode, bHashCode);
        }

        [Fact]
        public void PackageType_InHashSet()
        {
            // Arrange
            var inSet = new PackageType("a", new Version(1, 0));
            var notInSet = new PackageType("b", new Version(1, 0));
            var set = new HashSet<PackageType>
            {
                new PackageType("a", new Version(1, 0)),
                new PackageType("b", new Version(2, 0)),
                new PackageType("c", new Version(1, 0)),
                new PackageType("d", new Version(1, 0))
            };

            // Act & Assert
            Assert.Contains(inSet, set);
            Assert.DoesNotContain(notInSet, set);
        }
    }
}
