// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using NuGet.Common;
using NuGet.ProjectModel;
using Xunit;

namespace NuGet.Commands.Test
{
    public class WarningPropertiesTests
    {
        [Fact]
        public void Constructor_WithNullNoWarn_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WarningProperties(allWarningsAsErrors: true, noWarn: null, warningsAsErrors: new HashSet<NuGetLogCode>() { }, warningsNotAsErrors: new HashSet<NuGetLogCode>() { }));
        }

        [Fact]
        public void Constructor_WithNullWarningsAsErrors_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WarningProperties(allWarningsAsErrors: true, noWarn: new HashSet<NuGetLogCode>() { }, warningsAsErrors: null, warningsNotAsErrors: new HashSet<NuGetLogCode>() { }));
        }

        [Fact]
        public void Constructor_WithNullWarningsNotAsErrors_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new WarningProperties(allWarningsAsErrors: true, noWarn: new HashSet<NuGetLogCode>() { }, warningsAsErrors: new HashSet<NuGetLogCode>() { }, warningsNotAsErrors: null));
        }

        [Fact]
        public void Equals_WithOutOfOrderNoWarn_ReturnsTrue()
        {
            var left = new WarningProperties(allWarningsAsErrors: true, noWarn: new HashSet<NuGetLogCode>() { NuGetLogCode.NU1500, NuGetLogCode.NU5501 }, warningsAsErrors: new HashSet<NuGetLogCode>() { NuGetLogCode.NU5004 }, warningsNotAsErrors: new HashSet<NuGetLogCode>() { NuGetLogCode.NU5000 });
            var right = new WarningProperties(allWarningsAsErrors: true, noWarn: new HashSet<NuGetLogCode>() { NuGetLogCode.NU5501, NuGetLogCode.NU1500 }, warningsAsErrors: new HashSet<NuGetLogCode>() { NuGetLogCode.NU5004 }, warningsNotAsErrors: new HashSet<NuGetLogCode>() { NuGetLogCode.NU5000 });

            left.Should().Be(right);
        }

        [Fact]
        public void Equals_WithDifferentNoWarn_ReturnsFalse()
        {
            var left = new WarningProperties(allWarningsAsErrors: true, noWarn: new HashSet<NuGetLogCode>() { NuGetLogCode.NU1507, NuGetLogCode.NU5501 }, warningsAsErrors: new HashSet<NuGetLogCode>() { NuGetLogCode.NU5004 }, warningsNotAsErrors: new HashSet<NuGetLogCode>() { NuGetLogCode.NU5000 });
            var right = new WarningProperties(allWarningsAsErrors: true, noWarn: new HashSet<NuGetLogCode>() { NuGetLogCode.NU5501, NuGetLogCode.NU1500 }, warningsAsErrors: new HashSet<NuGetLogCode>() { NuGetLogCode.NU5004 }, warningsNotAsErrors: new HashSet<NuGetLogCode>() { NuGetLogCode.NU5000 });

            left.Should().NotBe(right);
        }

    }
}
