// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.ProjectModel;
using Xunit;

namespace NuGet.Commands.Test
{
    public class WarningPropertiesCollectionTests
    {
        [Fact]
        public void WarningPropertiesCollection_ProjectPropertiesWithNoWarn()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };
            var warningPropertiesCollection = new WarningPropertiesCollection(new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors), null, null);

            var suppressedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning");
            var nonSuppressedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1601, "Warning");

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(nonSuppressedMessage));
            Assert.Equal(LogLevel.Warning, nonSuppressedMessage.Level);
        }

        [Fact]
        public void WarningPropertiesCollection_ProjectPropertiesWithWarnAsError()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };
            var warningPropertiesCollection = new WarningPropertiesCollection(new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors), null, null);

            var upgradedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning");
            var nonSuppressedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1601, "Warning");

            // Act && Assert
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(upgradedMessage));
            Assert.Equal(LogLevel.Error, upgradedMessage.Level);
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(nonSuppressedMessage));
            Assert.Equal(LogLevel.Warning, nonSuppressedMessage.Level);
        }

        [Fact]
        public void WarningPropertiesCollection_ProjectPropertiesWithWarnAsErrorAndUndefinedWarningCode()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.Undefined };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };
            var warningPropertiesCollection = new WarningPropertiesCollection(new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors), null, null);

            var nonSuppressedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.Undefined, "Warning");
            var nonSuppressedMessage2 = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1601, "Warning");

            // Act && Assert
            // WarningPropertiesCollection should not Upgrade Warnings with Undefined code.
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(nonSuppressedMessage));
            Assert.Equal(LogLevel.Error, nonSuppressedMessage.Level);
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(nonSuppressedMessage2));
            Assert.Equal(LogLevel.Warning, nonSuppressedMessage2.Level);
        }

        [Fact]
        public void WarningPropertiesCollection_ProjectPropertiesWithAllWarningsAsErrors()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = true;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };
            var warningPropertiesCollection = new WarningPropertiesCollection(new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors), null, null);

            var upgradedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning");
            var upgradedMessage2 = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1601, "Warning");

            // Act && Assert
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(upgradedMessage));
            Assert.Equal(LogLevel.Error, upgradedMessage.Level);
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(upgradedMessage2));
            Assert.Equal(LogLevel.Error, upgradedMessage2.Level);
        }

        [Fact]
        public void WarningPropertiesCollection_ProjectPropertiesWithAllWarningsAsErrorsAndWarningWithUndefinedCode()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = true;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };
            var warningPropertiesCollection = new WarningPropertiesCollection(new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors), null, null);

            var upgradedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.Undefined, "Warning");
            var upgradedMessage2 = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1601, "Warning");

            // Act && Assert
            // WarningPropertiesCollection should not Upgrade Warnings with Undefined code.
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(upgradedMessage));
            Assert.Equal(LogLevel.Warning, upgradedMessage.Level);
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(upgradedMessage2));
            Assert.Equal(LogLevel.Error, upgradedMessage2.Level);
        }

        [Fact]
        public void WarningPropertiesCollection_ProjectPropertiesWithNoWarnAndWarnAsError()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };
            var warningPropertiesCollection = new WarningPropertiesCollection(new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors), null, null);

            var suppressedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning");
            var nonSuppressedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1601, "Warning");

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(nonSuppressedMessage));
            Assert.Equal(LogLevel.Warning, nonSuppressedMessage.Level);
        }

        [Fact]
        public void WarningPropertiesCollection_ProjectPropertiesWithNoWarnAndWarnAsErrorAndAllWarningsAsErrors()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var allWarningsAsErrors = true;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };
            var warningPropertiesCollection = new WarningPropertiesCollection(new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors), null, null);

            var suppressedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning");
            var nonSuppressedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1601, "Warning");

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(nonSuppressedMessage));
            Assert.Equal(LogLevel.Error, nonSuppressedMessage.Level);
        }

        [Fact]
        public void WarningPropertiesCollection_ProjectPropertiesWithWarnAsErrorAndAllWarningsAsErrors()
        {
            // Arrange
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var allWarningsAsErrors = true;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };
            var warningPropertiesCollection = new WarningPropertiesCollection(new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors), null, null);

            var upgradedMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1500, "Warning");
            var upgradedMessage2 = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1601, "Warning");

            // Act && Assert
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(upgradedMessage));
            Assert.Equal(LogLevel.Error, upgradedMessage.Level);
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(upgradedMessage2));
            Assert.Equal(LogLevel.Error, upgradedMessage2.Level);
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_PackagePropertiesWithFrameworkAndWarningWithFramework(string frameworkString)
        {
            // Arrange
            var libraryId = "test_library";
            var targetFramework = NuGetFramework.Parse(frameworkString);

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(null, packageSpecificWarningProperties, null);

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString);
            var nonSuppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId, frameworkString);

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(nonSuppressedMessage));
            Assert.Equal(LogLevel.Warning, nonSuppressedMessage.Level);
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_PackagePropertiesWithATFFrameworkAndWarningWithFramework(string frameworkString)
        {
            // Arrange
            var libraryId = "test_library";
            var targetFramework = new AssetTargetFallbackFramework(NuGetFramework.Parse(frameworkString), new List<NuGetFramework>() { NuGetFramework.AnyFramework });

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(null, packageSpecificWarningProperties, null);

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString);

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_PackagePropertiesWithPTFFrameworkAndWarningWithFramework(string frameworkString)
        {
            // Arrange
            var libraryId = "test_library";
            var targetFramework = new FallbackFramework(NuGetFramework.Parse(frameworkString), new List<NuGetFramework>() { NuGetFramework.AnyFramework });

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(null, packageSpecificWarningProperties, null);

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString);

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_PackagePropertiesWithoutFrameworkAndWarningWithoutFramework(string frameworkString)
        {
            // Arrange
            var libraryId = "test_library";
            var targetFramework = NuGetFramework.Parse(frameworkString);

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(null, packageSpecificWarningProperties, new List<NuGetFramework> { targetFramework });

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId);

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_PackagePropertiesWithoutFrameworkAndWarningWithDifferentFramework(string frameworkString)
        {
            // Arrange
            var libraryId = "test_library";
            var targetFramework = NuGetFramework.Parse(frameworkString);

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(null, packageSpecificWarningProperties, null);

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, "net45");

            // Act && Assert
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_PackagePropertiesWithNoWarnAndProjectProperties(string frameworkString)
        {
            // Arrange
            // Arrange
            var libraryId = "test_library";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                null);

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString);

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_PackagePropertiesAndProjectPropertiesWithNoWarn(string frameworkString)
        {
            // Arrange
            // Arrange
            var libraryId = "test_library";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                null);

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId);

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_PackagePropertiesWithNoWarnAndProjectPropertiesWithWarnAsError(string frameworkString)
        {

            // Arrange
            var libraryId = "test_library";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                new List<NuGetFramework> { targetFramework });

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString);
            var suppressedMessage2 = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId);
            var unaffectedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId, frameworkString);

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage2));
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(unaffectedMessage));
            Assert.Equal(LogLevel.Warning, unaffectedMessage.Level);
            Assert.Equal(1, unaffectedMessage.TargetGraphs.Count);
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_PackagePropertiesWithNoWarnAndProjectPropertiesWithAllWarnAsError(string frameworkString)
        {
            // Arrange
            var libraryId = "test_library";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = true;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                new List<NuGetFramework> { targetFramework });

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString);
            var suppressedMessage2 = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId);
            var upgradedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId, frameworkString);

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage2));
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(upgradedMessage));
            Assert.Equal(LogLevel.Error, upgradedMessage.Level);
            Assert.Equal(1, upgradedMessage.TargetGraphs.Count);
        }


        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_PackagePropertiesWithNoWarnAndProjectPropertiesWithWarnAsErrorAndProjectWithoutTargetFramework(string frameworkString)
        {

            // Arrange
            var libraryId = "test_library";
            var targetFramework = NuGetFramework.Parse(frameworkString);
            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { NuGetLogCode.NU1500 };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                new List<NuGetFramework> { targetFramework });

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, frameworkString);
            var suppressedMessage2 = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId);
            var unaffectedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1601, "Warning", libraryId);

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.Equal(0, suppressedMessage.TargetGraphs.Count);
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage2));
            Assert.Equal(0, suppressedMessage2.TargetGraphs.Count);
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(unaffectedMessage));
            Assert.Equal(0, unaffectedMessage.TargetGraphs.Count);
        }

        [Theory]
        [InlineData("net461", "netcoreapp1.0")]
        [InlineData("net461", "netcoreapp1.1")]
        [InlineData("net461", "netstandard2.0")]
        [InlineData("net461", "netcoreapp2.0")]
        [InlineData("netcoreapp1.0", "netstandard2.0")]
        [InlineData("netcoreapp2.0", "netstandard2.0")]
        public void WarningPropertiesCollection_MessageWithNoTargetGraphAndDependencyWithNoWarnForSomeTfm(string firstFrameworkString, string secondFrameworkString)
        {

            // Arrange
            var libraryId = "test_library";
            var firstTargetFramework = NuGetFramework.Parse(firstFrameworkString);
            var secondTargetFramework = NuGetFramework.Parse(secondFrameworkString);

            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, firstTargetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                new List<NuGetFramework> { firstTargetFramework, secondTargetFramework });

            var nonSuppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId);
            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, firstFrameworkString);

            // Act && Assert
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(nonSuppressedMessage));
            Assert.Equal(0, nonSuppressedMessage.TargetGraphs.Count);
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.Equal(0, suppressedMessage.TargetGraphs.Count);
        }

        [Theory]
        [InlineData("net461", "netcoreapp1.0")]
        [InlineData("net461", "netcoreapp1.1")]
        [InlineData("net461", "netstandard2.0")]
        [InlineData("net461", "netcoreapp2.0")]
        [InlineData("netcoreapp1.0", "netstandard2.0")]
        [InlineData("netcoreapp2.0", "netstandard2.0")]
        public void WarningPropertiesCollection_MessageWithNoTargetGraphAndDependencyWithNoWarnForAllTfm(string firstFrameworkString, string secondFrameworkString)
        {

            // Arrange
            var libraryId = "test_library";
            var firstTargetFramework = NuGetFramework.Parse(firstFrameworkString);
            var secondTargetFramework = NuGetFramework.Parse(secondFrameworkString);

            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, firstTargetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, secondTargetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                new List<NuGetFramework> { firstTargetFramework, secondTargetFramework });

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId);

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.Equal(0, suppressedMessage.TargetGraphs.Count);
        }

        [Theory]
        [InlineData("net461", "netcoreapp1.0")]
        [InlineData("net461", "netcoreapp1.1")]
        [InlineData("net461", "netstandard2.0")]
        [InlineData("net461", "netcoreapp2.0")]
        [InlineData("netcoreapp1.0", "netstandard2.0")]
        [InlineData("netcoreapp2.0", "netstandard2.0")]
        public void WarningPropertiesCollection_MessageWithTargetGraphAndDependencyWithNoWarnForSomeTfm(string firstFrameworkString, string secondFrameworkString)
        {

            // Arrange
            var libraryId = "test_library";
            var firstTargetFramework = NuGetFramework.Parse(firstFrameworkString);
            var secondTargetFramework = NuGetFramework.Parse(secondFrameworkString);

            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, firstTargetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                new List<NuGetFramework> { firstTargetFramework, secondTargetFramework });

            var nonSuppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, new string[] { firstFrameworkString, secondFrameworkString });

            // Act && Assert
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(nonSuppressedMessage));
            Assert.Equal(1, nonSuppressedMessage.TargetGraphs.Count);
        }

        [Theory]
        [InlineData("net461", "netcoreapp1.0")]
        [InlineData("net461", "netcoreapp1.1")]
        [InlineData("net461", "netstandard2.0")]
        [InlineData("net461", "netcoreapp2.0")]
        [InlineData("netcoreapp1.0", "netstandard2.0")]
        [InlineData("netcoreapp2.0", "netstandard2.0")]
        public void WarningPropertiesCollection_MessageWithTargetGraphAndDependencyWithNoWarnForAllTfm(string firstFrameworkString, string secondFrameworkString)
        {
            // Arrange
            var libraryId = "test_library";
            var firstTargetFramework = NuGetFramework.Parse(firstFrameworkString);
            var secondTargetFramework = NuGetFramework.Parse(secondFrameworkString);

            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, firstTargetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, secondTargetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                new List<NuGetFramework> { firstTargetFramework, secondTargetFramework });

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, new string[] { firstFrameworkString, secondFrameworkString });

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.Equal(0, suppressedMessage.TargetGraphs.Count);
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_MessageWithTargetGraphAndDependencyWithNoWarnForAllTfm_2(string frameworkString)
        {
            // Arrange
            var libraryId = "test_library";
            var targetFramework = NuGetFramework.Parse(frameworkString);

            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                new List<NuGetFramework> { targetFramework });

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, new string[] { frameworkString });

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.Equal(0, suppressedMessage.TargetGraphs.Count);
        }

        [Theory]
        [InlineData("net461", "netcoreapp1.0")]
        [InlineData("net461", "netcoreapp1.1")]
        [InlineData("net461", "netstandard2.0")]
        [InlineData("net461", "netcoreapp2.0")]
        [InlineData("netcoreapp1.0", "netstandard2.0")]
        [InlineData("netcoreapp2.0", "netstandard2.0")]
        public void WarningPropertiesCollection_MessageWithTargetGraphAndDependencyWithNoWarnForSomeTfmAndNoProjectFrameworks(string firstFrameworkString, string secondFrameworkString)
        {

            // Arrange
            var libraryId = "test_library";
            var firstTargetFramework = NuGetFramework.Parse(firstFrameworkString);
            var secondTargetFramework = NuGetFramework.Parse(secondFrameworkString);

            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, firstTargetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                null);

            var nonSuppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, new string[] { firstFrameworkString, secondFrameworkString });

            // Act && Assert
            Assert.False(warningPropertiesCollection.ApplyWarningProperties(nonSuppressedMessage));
            Assert.Equal(1, nonSuppressedMessage.TargetGraphs.Count);
        }

        [Theory]
        [InlineData("net461", "netcoreapp1.0")]
        [InlineData("net461", "netcoreapp1.1")]
        [InlineData("net461", "netstandard2.0")]
        [InlineData("net461", "netcoreapp2.0")]
        [InlineData("netcoreapp1.0", "netstandard2.0")]
        [InlineData("netcoreapp2.0", "netstandard2.0")]
        public void WarningPropertiesCollection_MessageWithTargetGraphAndDependencyWithNoWarnForAllTfmAndNoProjectFrameworks(string firstFrameworkString, string secondFrameworkString)
        {
            // Arrange
            var libraryId = "test_library";
            var firstTargetFramework = NuGetFramework.Parse(firstFrameworkString);
            var secondTargetFramework = NuGetFramework.Parse(secondFrameworkString);

            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, firstTargetFramework);
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, secondTargetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                null);

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, new string[] { firstFrameworkString, secondFrameworkString });

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.Equal(0, suppressedMessage.TargetGraphs.Count);
        }

        [Theory]
        [InlineData("net461")]
        [InlineData("netcoreapp1.0")]
        [InlineData("netcoreapp1.1")]
        [InlineData("netstandard2.0")]
        [InlineData("netcoreapp2.0")]
        public void WarningPropertiesCollection_MessageWithTargetGraphAndDependencyWithNoWarnForAllTfmAndNoProjectFrameworks_2(string frameworkString)
        {
            // Arrange
            var libraryId = "test_library";
            var targetFramework = NuGetFramework.Parse(frameworkString);

            var noWarnSet = new HashSet<NuGetLogCode> { };
            var warnAsErrorSet = new HashSet<NuGetLogCode> { };
            var allWarningsAsErrors = false;
            var warningsNotAsErrors = new HashSet<NuGetLogCode>() { };

            var packageSpecificWarningProperties = new PackageSpecificWarningProperties();
            packageSpecificWarningProperties.Add(NuGetLogCode.NU1500, libraryId, targetFramework);

            var warningPropertiesCollection = new WarningPropertiesCollection(
                new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors),
                packageSpecificWarningProperties,
                null);

            var suppressedMessage = RestoreLogMessage.CreateWarning(NuGetLogCode.NU1500, "Warning", libraryId, new string[] { frameworkString });

            // Act && Assert
            Assert.True(warningPropertiesCollection.ApplyWarningProperties(suppressedMessage));
            Assert.Equal(0, suppressedMessage.TargetGraphs.Count);
        }

        [Theory]
        [MemberData(nameof(WarningsNotAsErrorsTestCases))]
        public void ApplyWarningProperties_WithProjectWarningsNotAsErrors(bool allWarningsAsErrors, HashSet<NuGetLogCode> noWarnSet, HashSet<NuGetLogCode> warnAsErrorSet, HashSet<NuGetLogCode> warningsNotAsErrors, LogLevel expectedLogLevel, bool suppressed)
        {
            // Arrange
            var warningPropertiesCollection = new WarningPropertiesCollection(new WarningProperties(warnAsErrorSet, noWarnSet, allWarningsAsErrors, warningsNotAsErrors), null, null);
            var logMessage = new RestoreLogMessage(LogLevel.Warning, NuGetLogCode.NU1503, "Warning");

            // Act && Assert
            bool result = warningPropertiesCollection.ApplyWarningProperties(logMessage);

            // Assert
            result.Should().Be(suppressed);
            logMessage.Level.Should().Be(expectedLogLevel);
        }

        // WarningsAsErrors, NoWarn, WarnAsError, WarnNotAsError, ExpectedLogCode, Supressed
        public static IEnumerable<object[]> WarningsNotAsErrorsTestCases()
        {
            // With no data, nothing get elevated or suppressed
            yield return new object[] { false, new HashSet<NuGetLogCode> { }, new HashSet<NuGetLogCode> { }, new HashSet<NuGetLogCode> { }, LogLevel.Warning, false };
            // NoWarn warning messages are not treated as errors
            yield return new object[] { true, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, new HashSet<NuGetLogCode> { }, new HashSet<NuGetLogCode> { }, LogLevel.Warning, true };
            // NoWarn is preferred over WarningsNotAsErrors
            yield return new object[] { true, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, new HashSet<NuGetLogCode> { }, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, LogLevel.Warning, true };
            // NoWarn is preferred over WarningsAsErrors
            yield return new object[] { false, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, new HashSet<NuGetLogCode> { }, LogLevel.Warning, true };
            // WarningsNotAsError keeps message in warning.
            yield return new object[] { true, new HashSet<NuGetLogCode> { }, new HashSet<NuGetLogCode> { }, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, LogLevel.Warning, false };
            // Warnings as errors takes precedence over warnings not as errors
            yield return new object[] { false, new HashSet<NuGetLogCode> { }, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, LogLevel.Error, false };
            // WarningsNotAsErrors takes precedence over TreatWarningsAsErrors, but WarningsAsErrors takes precedence over WarningsNotAsErrors
            yield return new object[] { true, new HashSet<NuGetLogCode> { }, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, LogLevel.Error, false };
            // WarningsNotAsError config does not apply to the raised warning
            yield return new object[] { true, new HashSet<NuGetLogCode> { }, new HashSet<NuGetLogCode> { }, new HashSet<NuGetLogCode> { NuGetLogCode.NU1504 }, LogLevel.Error, false };
            // Complex scenario
            yield return new object[] { true, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, new HashSet<NuGetLogCode> { NuGetLogCode.NU1503 }, new HashSet<NuGetLogCode> { NuGetLogCode.NU1504 }, LogLevel.Warning, true };
        }
    }
}
