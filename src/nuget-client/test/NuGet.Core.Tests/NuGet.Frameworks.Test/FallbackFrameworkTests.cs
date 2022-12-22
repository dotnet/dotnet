// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.Test
{
    public class FallbackFrameworkTests
    {
        [Fact]
        public void FallbackFramework_ReferenceEquals()
        {
            // Arrange
            var a = new FallbackFramework(
                NuGetFramework.Parse("net45"),
                new[] { NuGetFramework.Parse("dnxcore50") });

            // Act & Assert
            Assert.Equal(a, a);
        }

        [Fact]
        public void FallbackFramework_Equals()
        {
            // Arrange
            var a = new FallbackFramework(
                NuGetFramework.Parse("net45"),
                new[] { NuGetFramework.Parse("dnxcore50") });
            var b = new FallbackFramework(
                NuGetFramework.Parse("net45"),
                new[] { NuGetFramework.Parse("dnxcore50") });

            // Act & Assert
            Assert.Equal(a, b);
            Assert.Equal(b, a);
        }

        [Fact]
        public void FallbackFramework_DifferentPrimary()
        {
            // Arrange
            var a = new FallbackFramework(
                NuGetFramework.Parse("net45"),
                new[] { NuGetFramework.Parse("dnxcore50") });
            var b = new FallbackFramework(
                NuGetFramework.Parse("net46"),
                new[] { NuGetFramework.Parse("dnxcore50") });

            // Act & Assert
            Assert.NotEqual(a, b);
            Assert.NotEqual(b, a);
        }

        [Fact]
        public void FallbackFramework_SubsetFallbacks()
        {
            // Arrange
            var a = new FallbackFramework(
                NuGetFramework.Parse("net45"),
                new[] { NuGetFramework.Parse("dnxcore50") });
            var b = new FallbackFramework(
                NuGetFramework.Parse("net45"),
                new[] { NuGetFramework.Parse("dnxcore50"), NuGetFramework.Parse("netstandard1.1") });

            // Act & Assert
            Assert.NotEqual(a, b);
            Assert.NotEqual(b, a);
        }

        [Fact]
        public void FallbackFramework_DifferentFallbackOrder()
        {
            // Arrange
            var a = new FallbackFramework(
                NuGetFramework.Parse("netstandardapp1.5"),
                new[] { NuGetFramework.Parse("net40"), NuGetFramework.Parse("net45") });
            var b = new FallbackFramework(
                NuGetFramework.Parse("netstandardapp1.5"),
                new[] { NuGetFramework.Parse("net40"), NuGetFramework.Parse("net46") });

            // Act & Assert
            Assert.NotEqual(a, b);
            Assert.NotEqual(b, a);
        }

        [Fact]
        public void FallbackFramework_DifferentFallbacks()
        {
            // Arrange
            var a = new FallbackFramework(
                NuGetFramework.Parse("net45"),
                new[] { NuGetFramework.Parse("netstandard1.2") });
            var b = new FallbackFramework(
                NuGetFramework.Parse("net45"),
                new[] { NuGetFramework.Parse("win8") });

            // Act & Assert
            Assert.NotEqual(a, b);
            Assert.NotEqual(b, a);
        }

        [Fact]
        public void FallbackFramework_CompareAsNuGetFramework()
        {
            // Arrange
            NuGetFramework a = new FallbackFramework(
                NuGetFramework.Parse("net45"),
                new[] { NuGetFramework.Parse("netstandard1.2") });
            var b = new FallbackFramework(
                NuGetFramework.Parse("net45"),
                new[] { NuGetFramework.Parse("win8") });

            // Act & Assert
            Assert.Equal(a, (NuGetFramework)b);
            Assert.Equal(b, (NuGetFramework)a);
            Assert.Equal((NuGetFramework)a, b);
            Assert.Equal((NuGetFramework)b, a);
        }
    }
}
