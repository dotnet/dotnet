// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Moq;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Licenses;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class PackageLicenseUtilitiesTests
    {
        [Theory]
        [InlineData("MIT", 1, new string[] { "MIT" }, false)]
        [InlineData("(MIT)", 3, new string[] { "MIT" }, false)]
        [InlineData("MIT OR Apache-2.0", 3, new string[] { "MIT", "Apache-2.0" }, false)]
        [InlineData("(MIT OR Apache-2.0)", 5, new string[] { "MIT", "Apache-2.0" }, false)]
        [InlineData("(MIT OR RandomLicenseRefStillGetsLinkGenerated)", 6, new string[] { "MIT", "RandomLicenseRefStillGetsLinkGenerated" }, true)]
        [InlineData("((MIT WITH 389-exception) AND Apache-2.0)", 7, new string[] { "MIT", "389-exception", "Apache-2.0" }, false)]
        [InlineData("((( (AMDPLPA) AND BSD-2-Clause)))", 5, new string[] { "AMDPLPA", "BSD-2-Clause" }, false)]
        public void PackageLicenseUtility_GeneratesBasicLink(string license, int partsCount, string[] linkedText, bool hasWarnings)
        {
            // Setup
            var expression = NuGetLicenseExpression.Parse(license);
            IReadOnlyList<string> warnings = null;
            if (hasWarnings)
            {
                warnings = new List<string> { "Random warning" };
            }

            var licenseData = new LicenseMetadata(LicenseType.Expression, license, expression, warnings, LicenseMetadata.EmptyVersion);

            // Act
            var links = PackageLicenseUtilities.GenerateLicenseLinks(licenseData, licenseFileHeader: null, packagePath: null, packageIdentity: null);

            // Assert
            Assert.Equal(partsCount, links.Count);

            var partsWithLinks = new List<LicenseText>();
            foreach (var part in links)
            {
                if (part is LicenseText licenseText)
                {
                    partsWithLinks.Add(licenseText);
                }
            }
            Assert.Equal(linkedText.Count(), partsWithLinks.Count);
            for (var i = 0; i < partsWithLinks.Count; i++)
            {
                Assert.Equal(linkedText[i], partsWithLinks[i].Text);
            }

            Assert.Equal(license, string.Join("", links.Where(e => !(e is WarningText)).Select(e => e.Text)));
            if (hasWarnings)
            {
                Assert.NotNull(links[0] as WarningText);
            }
        }

        [Fact]
        public void PackageLicenseUtility_GeneratesLinkWithHigherVersion()
        {
            var license = "Not so random unparsed license";
            // Setup
            var licenseData = new LicenseMetadata(LicenseType.Expression, license, null, new List<string> { "bad license warning" }, new System.Version(LicenseMetadata.CurrentVersion.Major + 1, 0, 0));

            // Act
            var links = PackageLicenseUtilities.GenerateLicenseLinks(licenseData, licenseFileHeader: null, packagePath: null, packageIdentity: null);

            Assert.True(links[0] is WarningText);
            Assert.DoesNotContain(links, e => e is LicenseText);
        }

        [Fact]
        public void PackageLicenseUtility_GeneratesLegacyLicenseUrlCorrectly()
        {
            // Setup
            var originalUri = "https://nuget.org";
            var uri = new Uri(originalUri);

            // Act
            var links = PackageLicenseUtilities.GenerateLicenseLinks(null, uri, licenseFileHeader: null, packagePath: null, packageIdentity: null);

            var licenseText = links[0] as LicenseText;

            Assert.NotNull(licenseText);
            Assert.Equal(Resources.Text_ViewLicense, licenseText.Text);
            Assert.Equal(uri, licenseText.Link);
        }

        [Fact]
        public void PackageLicenseUtility_UnlicensedGeneratesNoLinksAndAWarning()
        {
            var license = "UNLICENSED";
            NuGetLicenseExpression expression = null;
            var warnings = new List<string>();
            try
            {
                expression = NuGetLicenseExpression.Parse(license);
            }
            catch (NuGetLicenseExpressionParsingException e)
            {
                warnings.Add(e.Message);
            }
            // Setup
            var licenseData = new LicenseMetadata(LicenseType.Expression, license, expression, warnings, LicenseMetadata.CurrentVersion);

            // Act
            var links = PackageLicenseUtilities.GenerateLicenseLinks(licenseData, licenseFileHeader: null, packagePath: null, packageIdentity: null);

            Assert.Equal(links.Count, 2);
            Assert.True(links[0] is WarningText);
            Assert.True(links[1] is FreeText);
        }

        [Fact]
        public void PackageLicenseUtility_BadUnlicensedGeneratesNoLinksAndAWarning()
        {
            var license = "UNLICENSED OR MIT";
            NuGetLicenseExpression expression = null;
            var warnings = new List<string>();
            try
            {
                expression = NuGetLicenseExpression.Parse(license);
            }
            catch (NuGetLicenseExpressionParsingException e)
            {
                warnings.Add(e.Message);
            }
            // Setup
            var licenseData = new LicenseMetadata(LicenseType.Expression, license, expression, warnings, LicenseMetadata.CurrentVersion);

            // Act
            var links = PackageLicenseUtilities.GenerateLicenseLinks(licenseData, licenseFileHeader: null, packagePath: null, packageIdentity: null);

            Assert.Equal(links.Count, 2);
            Assert.True(links[0] is WarningText);
            Assert.True(links[1] is FreeText);
        }

        [Fact]
        public void PackageLicenseUtility_GeneratesLinkForFiles()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Setup
                var packageA1 = new SimpleTestPackageContext("AddLicenseToCache", "1.0.0");
                string licenseFileLocation = "License.txt";
                string licenseFileHeader = "header";
                LicenseMetadata licenseData = new LicenseMetadata(LicenseType.File, licenseFileLocation, null, null, LicenseMetadata.CurrentVersion);
                packageA1.AddFile(licenseFileLocation, StreamLicenseContents);

                PackageIdentity packageIdentity = new PackageIdentity("AddLicenseToCache", NuGetVersion.Parse("1.0.0"));

                // Act
                IReadOnlyList<IText> links = PackageLicenseUtilities.GenerateLicenseLinks(
                    licenseData,
                    licenseFileHeader,
                    pathContext.SolutionRoot,
                    packageIdentity: packageIdentity);

                Assert.Equal(1, links.Count);
                Assert.True(links[0] is LicenseFileText);
                LicenseFileText licenseFileText = links[0] as LicenseFileText;
                Assert.Equal(Resources.Text_ViewLicense, licenseFileText.Text);
                Assert.Equal(Resources.LicenseFile_Loading, ((Run)((Paragraph)licenseFileText.LicenseText.Blocks.AsEnumerable().First()).Inlines.First()).Text);
            }
        }

        private Mock<INuGetTelemetryProvider> _telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

        [Fact]
        public async Task PackageLicenseUtility_GeneratesLinkForFiles_And_CacheIsUpdated()
        {
            using (var pathContext = new SimpleTestPathContext())
            {
                // Setup
                var package = new SimpleTestPackageContext("AddLicenseToCache", "1.0.0");
                string licenseFileLocation = "License.txt";
                string licenseFileHeader = "header";
                LicenseMetadata licenseData = new LicenseMetadata(LicenseType.File, licenseFileLocation, null, null, LicenseMetadata.CurrentVersion);
                package.AddFile(licenseFileLocation, StreamLicenseContents);

                NuGetPackageFileService packageFileService = new NuGetPackageFileService(
                        default(ServiceActivationOptions),
                        Mock.Of<IServiceBroker>(),
                        new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                        _telemetryProvider.Object);

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    pathContext.PackageSource,
                    PackageSaveMode.Defaultv3,
                    package);

                PackageIdentity packageIdentity = new PackageIdentity("AddLicenseToCache", NuGetVersion.Parse("1.0.0"));

                // Act
                IReadOnlyList<IText> links = PackageLicenseUtilities.GenerateLicenseLinks(
                    licenseData,
                    licenseFileHeader,
                    Path.Combine(pathContext.PackageSource, packageIdentity.Id, packageIdentity.Version.ToString(), package.PackageName),
                    packageIdentity);

                Assert.Equal(1, links.Count);
                Assert.True(links[0] is LicenseFileText);
                LicenseFileText licenseFileText = links[0] as LicenseFileText;
                Assert.Equal(Resources.Text_ViewLicense, licenseFileText.Text);
                Assert.Equal(Resources.LicenseFile_Loading, ((Run)((Paragraph)licenseFileText.LicenseText.Blocks.AsEnumerable().First()).Inlines.First()).Text);

                Stream licenseStream = await packageFileService.GetEmbeddedLicenseAsync(packageIdentity, CancellationToken.None);
                Assert.NotNull(licenseStream);
                Assert.Equal(StreamLicenseContents.Length, licenseStream.Length);
            }
        }

        [Fact]
        public void PackageLicenseUtility_GenerateCorrectLink()
        {
            // Setup
            var license = "MIT";
            var expression = NuGetLicenseExpression.Parse(license);
            var licenseData = new LicenseMetadata(LicenseType.Expression, license, expression, null, LicenseMetadata.EmptyVersion);

            // Act
            var links = PackageLicenseUtilities.GenerateLicenseLinks(licenseData, licenseFileHeader: null, packagePath: null, packageIdentity: null);

            // Assert
            Assert.Equal(1, links.Count);
            var licenseText = links[0] as LicenseText;
            Assert.NotNull(licenseText);
            Assert.Equal(license, licenseText.Text);
            Assert.Equal("https://licenses.nuget.org/MIT", licenseText.Link.AbsoluteUri);
        }

        private const string StreamLicenseContents = "I am a license";
    }
}
