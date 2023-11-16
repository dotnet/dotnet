// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.ServiceHub.Framework.Services;
using Moq;
using NuGet.PackageManagement.Telemetry;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Test.Utility;
using NuGet.Versioning;
using NuGet.VisualStudio.Telemetry;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    using ExceptionUtility = global::Test.Utility.ExceptionUtility;

    [UseCulture("en-US")] // We are asserting exception messages in English
    public class NuGetPackageFileServiceTests
    {
        private Mock<INuGetTelemetryProvider> _telemetryProvider = new Mock<INuGetTelemetryProvider>(MockBehavior.Strict);

        [Fact]
        public void Constructor_WhenServiceBrokerIsNull_Throws()
        {
            Exception exception = Assert.ThrowsAny<Exception>(
                () => new NuGetPackageFileService(
                    default(ServiceActivationOptions),
                    serviceBroker: null,
                    new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                    _telemetryProvider.Object));

            ExceptionUtility.AssertMicrosoftAssumesException(exception);
        }

        [Fact]
        public void Constructor_WhenAuthorizationServiceClientIsNull_Throws()
        {
            Exception exception = Assert.ThrowsAny<Exception>(
                () => new NuGetPackageFileService(
                    default(ServiceActivationOptions),
                    Mock.Of<IServiceBroker>(),
                    authorizationServiceClient: null,
                    _telemetryProvider.Object));

            ExceptionUtility.AssertMicrosoftAssumesException(exception);
        }

        [Fact]
        public async Task AddIconToCache_WhenAdded_IsFound()
        {
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                string fileContents = "testFileContents";
                string filePath = Path.Combine(testDirectory.Path, "testFile.txt");
                File.WriteAllText(filePath, fileContents);
                var packageFileService = new NuGetPackageFileService(
                        default(ServiceActivationOptions),
                        Mock.Of<IServiceBroker>(),
                        new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                        _telemetryProvider.Object);
                var packageIdentity = new PackageIdentity("AddIconToCache_WhenAdded_IsFound", NuGetVersion.Parse("1.0.0"));
                NuGetPackageFileService.AddIconToCache(packageIdentity, new Uri(filePath));

                Stream iconStream = await packageFileService.GetPackageIconAsync(packageIdentity, CancellationToken.None);

                Assert.NotNull(iconStream);
                Assert.Equal(fileContents.Length, iconStream.Length);
            }
        }

        [Fact]
        public async Task GetPackageIconAsync_EmbeddedFromFallbackFolder_HasOnlyReadAccess()
        {
            // Arrange
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                string fileContents = "testFileContents";
                string filePath = Path.Combine(testDirectory.Path, "testFile.txt");

                File.WriteAllText(filePath, fileContents);

                var packageFileService = new NuGetPackageFileService(
                        default(ServiceActivationOptions),
                        Mock.Of<IServiceBroker>(),
                        new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                        _telemetryProvider.Object);
                var packageIdentity = new PackageIdentity(nameof(GetPackageIconAsync_EmbeddedFromFallbackFolder_HasOnlyReadAccess), NuGetVersion.Parse("1.0.0"));

                // Add a fragment to consider this an embedded icon.
                // Note: file:// is required for Uri to parse the Fragment (possibly a Uri bug).
                var uri = new Uri("file://" + filePath + "#testFile.txt");

                NuGetPackageFileService.AddIconToCache(packageIdentity, uri);


                // Act
                Stream iconStream = await packageFileService.GetPackageIconAsync(packageIdentity, CancellationToken.None);

                // Assert
                Assert.NotNull(iconStream);
                Assert.True(iconStream.CanRead);
                Assert.False(iconStream.CanWrite);

                Assert.Equal(fileContents.Length, iconStream.Length);
            }
        }

        [Fact]
        public async Task GetPackageIconAsync_EmbeddedFromFallbackFolder_DoesNotLockFile()
        {
            //Arrange
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                string fileContents = "testFileContents";
                string filePath = Path.Combine(testDirectory.Path, "testFile.txt");

                File.WriteAllText(filePath, fileContents);

                var packageFileService = new NuGetPackageFileService(
                        default(ServiceActivationOptions),
                        Mock.Of<IServiceBroker>(),
                        new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                        _telemetryProvider.Object);
                var packageIdentity = new PackageIdentity(nameof(GetPackageIconAsync_EmbeddedFromFallbackFolder_DoesNotLockFile), NuGetVersion.Parse("1.0.0"));

                // Add a fragment to consider this an embedded icon.
                // Note: file:// is required for Uri to parse the Fragment (possibly a Uri bug).
                var uri = new Uri("file://" + filePath + "#testFile.txt");

                NuGetPackageFileService.AddIconToCache(packageIdentity, uri);

                FileStream fileNotLocked = File.Open(uri.LocalPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

                // Act
                // System.IO.IOException would occur in the Act if we're locking the icon file.
                Stream iconStream = await packageFileService.GetPackageIconAsync(packageIdentity, CancellationToken.None);
                fileNotLocked.Close();
                FileStream fileNotLocked2 = File.Open(uri.LocalPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

                // Assert
                Assert.NotNull(iconStream);
                Assert.True(iconStream.CanRead);
                Assert.False(iconStream.CanWrite);

                Assert.Equal(fileContents.Length, iconStream.Length);
            }
        }

        [Fact]
        public async Task GetPackageIconAsync_EmbeddedFromFallbackFolder_CanOpenReadOnlyFile()
        {
            // Arrange
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                string fileContents = "testFileContents";

                string pathAndFileReadOnly = Path.Combine(testDirectory.Path, "testFile.txt");
                File.WriteAllText(pathAndFileReadOnly, fileContents);
                // Set as a read-only file.
                FileInfo fileInfo = new FileInfo(pathAndFileReadOnly);
                fileInfo.IsReadOnly = true;

                var packageFileService = new NuGetPackageFileService(
                        default(ServiceActivationOptions),
                        Mock.Of<IServiceBroker>(),
                        new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                        _telemetryProvider.Object);
                var packageIdentity = new PackageIdentity(nameof(GetPackageIconAsync_EmbeddedFromFallbackFolder_DoesNotLockFile), NuGetVersion.Parse("1.0.0"));

                // Add a fragment to consider this an embedded icon.
                // Note: file:// is required for Uri to parse the Fragment (possibly a Uri bug).
                var uri = new Uri("file://" + pathAndFileReadOnly + "#testFile.txt");

                NuGetPackageFileService.AddIconToCache(packageIdentity, uri);

                // Act
                // System.UnauthorizedAccessException would occur in the Act if we're requiring Write access on the Read-Only file.
                Stream iconStream = await packageFileService.GetPackageIconAsync(packageIdentity, CancellationToken.None);

                // Assert
                Assert.NotNull(iconStream);
                Assert.True(iconStream.CanRead);
                Assert.False(iconStream.CanWrite);

                Assert.Equal(fileContents.Length, iconStream.Length);
            }
        }

        [Fact]
        public async Task AddIconToCache_WhenAddedTwice_UsesSecond()
        {
            using (TestDirectory testDirectory = TestDirectory.Create())
            {
                string fileContents = "testFileContents";
                string filePath = Path.Combine(testDirectory.Path, "testFile.txt");
                File.WriteAllText(filePath, fileContents);

                string fileContents2 = fileContents + fileContents;
                string filePath2 = Path.Combine(testDirectory.Path, "testFile2.txt");
                File.WriteAllText(filePath2, fileContents2);

                var packageFileService = new NuGetPackageFileService(
                        default(ServiceActivationOptions),
                        Mock.Of<IServiceBroker>(),
                        new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                        _telemetryProvider.Object);
                var packageIdentity = new PackageIdentity("AddIconToCache_WhenAddedTwice_UsesSecond", NuGetVersion.Parse("1.0.0"));
                NuGetPackageFileService.AddIconToCache(packageIdentity, new Uri(filePath));
                NuGetPackageFileService.AddIconToCache(packageIdentity, new Uri(filePath2));

                Stream iconStream = await packageFileService.GetPackageIconAsync(packageIdentity, CancellationToken.None);

                Assert.NotNull(iconStream);
                Assert.Equal(fileContents2.Length, iconStream.Length);
            }
        }

        [Fact]
        public async Task AddIconToCache_WhenMissing_IsNotFound()
        {
            _telemetryProvider.Setup(t => t.PostFaultAsync(It.IsAny<CacheMissException>(), typeof(NuGetPackageFileService).FullName, nameof(NuGetPackageFileService.GetPackageIconAsync), It.IsAny<IDictionary<string, object>>())).Returns(Task.CompletedTask);
            var packageFileService = new NuGetPackageFileService(
                    default(ServiceActivationOptions),
                    Mock.Of<IServiceBroker>(),
                    new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                    _telemetryProvider.Object);
            var packageIdentity = new PackageIdentity("AddIconToCache_WhenMissing_IsNotFound", NuGetVersion.Parse("1.0.0"));
            Stream iconStream = await packageFileService.GetPackageIconAsync(packageIdentity, CancellationToken.None);

            _telemetryProvider.Verify(t => t.PostFaultAsync(It.IsAny<CacheMissException>(), typeof(NuGetPackageFileService).FullName, nameof(NuGetPackageFileService.GetPackageIconAsync), It.IsAny<IDictionary<string, object>>()));
            _telemetryProvider.VerifyNoOtherCalls();
            Assert.Null(iconStream);
        }

        [Theory]
        [InlineData("icon.png", "icon.png", "icon.png", "")]
        public async Task AddLicenseToCache_WhenAdded_IsFound(
            string iconElement,
            string iconFileLocation,
            string fileSourceElement,
            string fileTargetElement)
        {
            using (TestDirectory testDir = TestDirectory.Create())
            {
                // Create decoy nuget package
                var zipPath = Path.Combine(testDir.Path, "file.nupkg");
                CreateDummyPackage(
                    zipPath: zipPath,
                    iconName: iconElement,
                    iconFile: iconFileLocation,
                    iconFileSourceElement: fileSourceElement,
                    iconFileTargetElement: fileTargetElement);

                // prepare test
                var builder = new UriBuilder(new Uri(zipPath, UriKind.Absolute))
                {
                    Fragment = iconElement
                };

                var packageFileService = new NuGetPackageFileService(
                        default(ServiceActivationOptions),
                        Mock.Of<IServiceBroker>(),
                        new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                        _telemetryProvider.Object);
                var packageIdentity = new PackageIdentity("AddLicenseToCache_WhenAdded_IsFound", NuGetVersion.Parse("1.0.0"));
                NuGetPackageFileService.AddLicenseToCache(packageIdentity, builder.Uri);

                Stream licenseStream = await packageFileService.GetEmbeddedLicenseAsync(packageIdentity, CancellationToken.None);

                Assert.NotNull(licenseStream);
                Assert.Equal(StreamContents.Length, licenseStream.Length);
            }
        }

        [Theory]
        [InlineData("icon.png", "icon.png", "icon.png", "")]
        public async Task AddLicenseToCache_WhenAddedTwice_UsesSecond(
            string iconElement,
            string iconFileLocation,
            string fileSourceElement,
            string fileTargetElement)
        {
            using (TestDirectory testDir = TestDirectory.Create())
            {
                // Create decoy nuget package
                var zipPath = Path.Combine(testDir.Path, "file.nupkg");
                CreateDummyPackage(
                    zipPath: zipPath,
                    iconName: iconElement,
                    iconFile: iconFileLocation,
                    iconFileSourceElement: fileSourceElement,
                    iconFileTargetElement: fileTargetElement);

                // prepare test
                var builder = new UriBuilder(new Uri(zipPath, UriKind.Absolute))
                {
                    Fragment = iconElement
                };

                var packageFileService = new NuGetPackageFileService(
                        default(ServiceActivationOptions),
                        Mock.Of<IServiceBroker>(),
                        new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                        _telemetryProvider.Object);
                var packageIdentity = new PackageIdentity("AddLicenseToCache_WhenAdded_IsFound", NuGetVersion.Parse("1.0.0"));
                NuGetPackageFileService.AddLicenseToCache(packageIdentity, new Uri(builder.Uri.ToString() + "more"));
                NuGetPackageFileService.AddLicenseToCache(packageIdentity, builder.Uri);

                Stream licenseStream = await packageFileService.GetEmbeddedLicenseAsync(packageIdentity, CancellationToken.None);

                Assert.NotNull(licenseStream);
                Assert.Equal(StreamContents.Length, licenseStream.Length);
            }
        }

        [Fact]
        public async Task AddLicenseToCache_WhenMissing_IsNotFound()
        {
            _telemetryProvider.Setup(t => t.PostFaultAsync(It.IsAny<CacheMissException>(), typeof(NuGetPackageFileService).FullName, nameof(NuGetPackageFileService.GetEmbeddedLicenseAsync), It.IsAny<IDictionary<string, object>>())).Returns(Task.CompletedTask);
            var packageFileService = new NuGetPackageFileService(
                    default(ServiceActivationOptions),
                    Mock.Of<IServiceBroker>(),
                    new AuthorizationServiceClient(Mock.Of<IAuthorizationService>()),
                    _telemetryProvider.Object);
            var packageIdentity = new PackageIdentity("AddLicenseToCache_WhenMissing_IsNotFound", NuGetVersion.Parse("1.0.0"));
            Stream licenseStream = await packageFileService.GetEmbeddedLicenseAsync(packageIdentity, CancellationToken.None);

            _telemetryProvider.Verify(t => t.PostFaultAsync(It.IsAny<CacheMissException>(), typeof(NuGetPackageFileService).FullName, nameof(NuGetPackageFileService.GetEmbeddedLicenseAsync), It.IsAny<IDictionary<string, object>>()));
            _telemetryProvider.VerifyNoOtherCalls();
            Assert.Null(licenseStream);
        }

        /// <summary>
        /// Creates a NuGet package with .nupkg extension and with a PNG image named "icon.png"
        /// </summary>
        /// <param name="path">Path to NuGet package</param>
        /// <param name="iconName">Icon filename with .png extension</param>
        private static void CreateDummyPackage(
            string zipPath,
            string iconName = "icon.png",
            string iconFile = "icon.png",
            string iconFileSourceElement = "icon.png",
            string iconFileTargetElement = "")
        {
            var dir = Path.GetDirectoryName(zipPath);
            var holdDir = "pkg";
            var folderPath = Path.Combine(dir, holdDir);

            // base dir
            Directory.CreateDirectory(folderPath);

            // create nuspec
            var nuspec = NuspecBuilder.Create()
                .WithIcon(iconName)
                .WithFile(iconFileSourceElement, iconFileTargetElement);

            // create png image
            var iconPath = Path.Combine(folderPath, iconFile);
            var iconDir = Path.GetDirectoryName(iconPath);
            Directory.CreateDirectory(iconDir);

            File.WriteAllText(iconPath, StreamContents);

            // Create nuget package
            using (var nuspecStream = new MemoryStream())
            using (FileStream nupkgStream = File.Create(zipPath))
            using (var writer = new StreamWriter(nuspecStream))
            {
                nuspec.Write(writer);
                writer.Flush();
                nuspecStream.Position = 0;
                var pkgBuilder = new PackageBuilder(stream: nuspecStream, basePath: folderPath);
                pkgBuilder.Save(nupkgStream);
            }
        }

        private static string StreamContents = "I am an image";
    }
}
