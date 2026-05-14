// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class V2FeedPackageInfoTests
    {
        [Fact]
        public void DependencySet_SimpleParser()
        {
            // Arrange
            var testPackage = CreateTestPackageInfoForDependencyGroup("Microsoft.Data.OData:5.6.3:aspnetcore50|Microsoft.Data.Services.Client:5.6.3:aspnetcore50|System.Spatial:5.6.3:aspnetcore50|System.Collections:4.0.10-beta-22231:aspnetcore50|System.Collections.Concurrent:4.0.0-beta-22231:aspnetcore50|System.Collections.Specialized:4.0.0-beta-22231:aspnetcore50|System.Diagnostics.Debug:4.0.10-beta-22231:aspnetcore50|System.Diagnostics.Tools:4.0.0-beta-22231:aspnetcore50|System.Diagnostics.TraceSource:4.0.0-beta-22231:aspnetcore50|System.Diagnostics.Tracing:4.0.10-beta-22231:aspnetcore50|System.Dynamic.Runtime:4.0.0-beta-22231:aspnetcore50|System.Globalization:4.0.10-beta-22231:aspnetcore50|System.IO:4.0.10-beta-22231:aspnetcore50|System.IO.FileSystem:4.0.0-beta-22231:aspnetcore50|System.IO.FileSystem.Primitives:4.0.0-beta-22231:aspnetcore50|System.Linq:4.0.0-beta-22231:aspnetcore50|System.Linq.Expressions:4.0.0-beta-22231:aspnetcore50|System.Linq.Queryable:4.0.0-beta-22231:aspnetcore50|System.Net.Http:4.0.0-beta-22231:aspnetcore50|System.Net.Primitives:4.0.10-beta-22231:aspnetcore50|System.Reflection:4.0.10-beta-22231:aspnetcore50|System.Reflection.Extensions:4.0.0-beta-22231:aspnetcore50|System.Reflection.TypeExtensions:4.0.0-beta-22231:aspnetcore50|System.Runtime:4.0.20-beta-22231:aspnetcore50|System.Runtime.Extensions:4.0.10-beta-22231:aspnetcore50|System.Runtime.InteropServices:4.0.20-beta-22231:aspnetcore50|System.Runtime.Serialization.Primitives:4.0.0-beta-22231:aspnetcore50|System.Runtime.Serialization.Xml:4.0.10-beta-22231:aspnetcore50|System.Security.Cryptography.Encoding:4.0.0-beta-22231:aspnetcore50|System.Security.Cryptography.Encryption:4.0.0-beta-22231:aspnetcore50|System.Security.Cryptography.Hashing:4.0.0-beta-22231:aspnetcore50|System.Security.Cryptography.Hashing.Algorithms:4.0.0-beta-22231:aspnetcore50|System.Text.Encoding:4.0.10-beta-22231:aspnetcore50|System.Text.Encoding.Extensions:4.0.10-beta-22231:aspnetcore50|System.Text.RegularExpressions:4.0.10-beta-22231:aspnetcore50|System.Threading:4.0.0-beta-22231:aspnetcore50|System.Threading.Tasks:4.0.10-beta-22231:aspnetcore50|System.Threading.Thread:4.0.0-beta-22231:aspnetcore50|System.Threading.ThreadPool:4.0.10-beta-22231:aspnetcore50|System.Threading.Timer:4.0.0-beta-22231:aspnetcore50|System.Xml.ReaderWriter:4.0.10-beta-22231:aspnetcore50|System.Xml.XDocument:4.0.0-beta-22231:aspnetcore50|System.Xml.XmlSerializer:4.0.0-beta-22231:aspnetcore50|Microsoft.Data.OData:5.6.3:aspnet50|Microsoft.Data.Services.Client:5.6.3:aspnet50|System.Spatial:5.6.3:aspnet50|Microsoft.Data.OData:5.6.2:net40-Client|Newtonsoft.Json:5.0.8:net40-Client|Microsoft.Data.Services.Client:5.6.2:net40-Client|Microsoft.WindowsAzure.ConfigurationManager:1.8.0.0:net40-Client|Microsoft.Data.OData:5.6.2:win80|Microsoft.Data.OData:5.6.2:wpa|Microsoft.Data.OData:5.6.2:wp80|Newtonsoft.Json:5.0.8:wp80");

            // Act
            var dependencySet = testPackage.DependencySets;

            // Assert
            Assert.Equal(6, dependencySet.Count);
            Assert.True(dependencySet[0].TargetFramework.Equals(NuGetFramework.Parse("aspnetcore50")));
            Assert.True(dependencySet[1].TargetFramework.Equals(NuGetFramework.Parse("aspnet50")));
            Assert.True(dependencySet[2].TargetFramework.Equals(NuGetFramework.Parse("net40-Client")));
            Assert.True(dependencySet[3].TargetFramework.Equals(NuGetFramework.Parse("win80")));
            Assert.True(dependencySet[4].TargetFramework.Equals(NuGetFramework.Parse("wpa")));
            Assert.True(dependencySet[5].TargetFramework.Equals(NuGetFramework.Parse("wp80")));
        }

        [Fact]
        public void DependencySet_WithoutanyFramework()
        {
            // Arrange
            var testPackage = CreateTestPackageInfoForDependencyGroup("xunit.core:[2.0.0-rc1-build2826]:|xunit.assert:[2.0.0-rc1-build2826]:");

            // Act
            var dependencySet = testPackage.DependencySets;
            var packages = dependencySet[0].Packages.ToList();

            // Assert
            Assert.Equal(1, dependencySet.Count);
            Assert.True(dependencySet[0].TargetFramework.IsAny);
            Assert.Equal("xunit.core", packages[0].Id);
            Assert.Equal("xunit.assert", packages[1].Id);
        }

        [Fact]
        public void DependencySet_WithoutOneFramework()
        {
            // Arrange
            var testPackage = CreateTestPackageInfoForDependencyGroup("xunit.core:[2.0.0-rc1-build2826]:aspnet50|xunit.assert:[2.0.0-rc1-build2826]:");

            // Act
            var dependencySet = testPackage.DependencySets;
            var packages1 = dependencySet[0].Packages.ToList();
            var packages2 = dependencySet[1].Packages.ToList();

            // Assert
            Assert.Equal(2, dependencySet.Count);
            Assert.True(dependencySet[0].TargetFramework.Equals(NuGetFramework.Parse("aspnet50")));
            Assert.True(dependencySet[1].TargetFramework.IsAny);
            Assert.Equal("xunit.core", packages1[0].Id);
            Assert.Equal("xunit.assert", packages2[0].Id);
        }

        [Fact]
        public void DependencySet_WithoutVersion()
        {
            // Arrange
            var testPackage = CreateTestPackageInfoForDependencyGroup("xunit.core::aspnet50|xunit.assert:[2.0.0-rc1-build2826]:");

            // Act
            var dependencySet = testPackage.DependencySets;
            var packages1 = dependencySet[0].Packages.ToList();
            var packages2 = dependencySet[1].Packages.ToList();

            // Assert
            Assert.Equal(2, dependencySet.Count);
            Assert.True(dependencySet[0].TargetFramework.Equals(NuGetFramework.Parse("aspnet50")));
            Assert.True(dependencySet[1].TargetFramework.IsAny);
            Assert.Equal("xunit.core", packages1[0].Id);
            Assert.True(packages1[0].VersionRange.Equals(VersionRange.All));
            Assert.Equal("xunit.assert", packages2[0].Id);
        }

        [Fact]
        public void DependencySet_NoDependency()
        {
            // Arrange
            var testPackage = CreateTestPackageInfoForDependencyGroup("");

            // Act
            var dependencySet = testPackage.DependencySets;

            // Assert
            Assert.Equal(0, dependencySet.Count);
        }

        [Fact]
        public void DependencySet_WithUnsupportFramework()
        {
            // Arrange
            var testPackage = CreateTestPackageInfoForDependencyGroup("xunit.core::futureFramework|xunit.assert:[2.0.0-rc1-build2826]:");

            // Act
            var dependencySet = testPackage.DependencySets;
            var packages1 = dependencySet[0].Packages.ToList();
            var packages2 = dependencySet[1].Packages.ToList();

            // Assert
            Assert.Equal(2, dependencySet.Count);
            Assert.True(dependencySet[0].TargetFramework.IsUnsupported);
            Assert.True(dependencySet[1].TargetFramework.IsAny);
            Assert.Equal("xunit.core", packages1[0].Id);
            Assert.True(packages1[0].VersionRange.Equals(VersionRange.All));
            Assert.Equal("xunit.assert", packages2[0].Id);
        }

        [Fact]
        public void DependencySet_WithoutId()
        {
            // Arrange
            var testPackage = CreateTestPackageInfoForDependencyGroup(":[2.0.0-rc1-build2826]:aspnet50|xunit.assert:[2.0.0-rc1-build2826]:");

            // Act
            var dependencySet = testPackage.DependencySets;
            var packages = dependencySet[1].Packages.ToList();

            // Assert
            Assert.Equal(2, dependencySet.Count);
            Assert.Equal(0, dependencySet[0].Packages.Count());
            Assert.True(dependencySet[1].TargetFramework.IsAny);
            Assert.Equal("xunit.assert", packages[0].Id);
        }

        private V2FeedPackageInfo CreateTestPackageInfoForDependencyGroup(string dependencies)
        {
            return new V2FeedPackageInfo(new PackageIdentity("test", NuGetVersion.Parse("1.0.0")),
                                         "title", "summary", "description", Enumerable.Empty<string>(), Enumerable.Empty<string>(),
                                         "iconUrl", "licenseUrl", "projectUrl", "reportAbuseUrl", "galleryDetailsUrl", "tags", null, null, null, dependencies,
                                         false, "downloadUrl", "0", "packageHash", "packageHashAlgorithm", new NuGetVersion("3.0"));
        }
    }
}
