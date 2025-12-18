// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Protocol;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test.Models.Package
{
    public class VulnerableCapabilityBaseTests
    {
        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        public void IsVulnerable_VariousVulnerabilities_ReturnsExpected(int severity, bool expected)
        {
            // Arrange
            List<PackageVulnerabilityMetadataContextInfo> vulnerabilities = severity > 0
                ? [new(new Uri("http://example.com"), severity)]
                : [];
            var vulnerableCapability = new TestVulnerableCapability(vulnerabilities);

            // Act
            var result = vulnerableCapability.IsVulnerable;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new int[] { (int)PackageVulnerabilitySeverity.Low, (int)PackageVulnerabilitySeverity.High }, PackageVulnerabilitySeverity.High)]
        [InlineData(new int[] { (int)PackageVulnerabilitySeverity.Critical, (int)PackageVulnerabilitySeverity.Unknown }, PackageVulnerabilitySeverity.Critical)]
        [InlineData(new int[] { -2 }, PackageVulnerabilitySeverity.Unknown)]
        public void VulnerabilityMaxSeverity_VariousVulnerabilities_ReturnsExpected(int[] severities, PackageVulnerabilitySeverity expected)
        {
            // Arrange
            var vulnerabilities = severities.Select(severity => new PackageVulnerabilityMetadataContextInfo(new Uri("http://example.com"), severity)).ToList();
            var vulnerableCapability = new TestVulnerableCapability(vulnerabilities);

            // Act
            var result = vulnerableCapability.VulnerabilityMaxSeverity;

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void VulnerabilityMaxSeverity_EmptyVulnerabilitiesList_ReturnsUnknown()
        {
            // Arrange
            var vulnerableCapability = new TestVulnerableCapability([]);

            // Act & Assert
            Assert.Equal(PackageVulnerabilitySeverity.Unknown, vulnerableCapability.VulnerabilityMaxSeverity);
        }

        [Fact]
        public void VulnerabilityMaxSeverity_NullVulnerabilitiesList_ReturnsUnknown()
        {
            // Arrange
            var vulnerableCapability = new TestVulnerableCapability(null);

            // Act & Assert
            Assert.Equal(PackageVulnerabilitySeverity.Unknown, vulnerableCapability.VulnerabilityMaxSeverity);
        }

        [Fact]
        public void Constructor_OrdersVulnerabilitiesBySeverity_DescendingOrder()
        {
            // Arrange
            List<PackageVulnerabilityMetadataContextInfo> vulnerabilities =
            [
                new(new Uri("http://example.com/high"), (int)PackageVulnerabilitySeverity.High),
                new(new Uri("http://example.com/low"), (int)PackageVulnerabilitySeverity.Low),
                new(new Uri("http://example.com/moderate"), (int)PackageVulnerabilitySeverity.Moderate),
                new(new Uri("http://example.com/critical"), (int)PackageVulnerabilitySeverity.Critical)
            ];

            // Act
            var vulnerableCapability = new TestVulnerableCapability(vulnerabilities);
            var orderedVulnerabilities = vulnerableCapability.Vulnerabilities.ToList();

            // Assert
            Assert.Collection(orderedVulnerabilities,
                item => Assert.Equal((int)PackageVulnerabilitySeverity.Critical, item.Severity),
                item => Assert.Equal((int)PackageVulnerabilitySeverity.High, item.Severity),
                item => Assert.Equal((int)PackageVulnerabilitySeverity.Moderate, item.Severity),
                item => Assert.Equal((int)PackageVulnerabilitySeverity.Low, item.Severity)
            );
        }
    }
}
