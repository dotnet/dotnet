// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Frameworks;
using Xunit;

namespace NuGet.Commands.Test
{
    public class MSBuildProjectFrameworkUtilityTests
    {
        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyTargetFramworksParsed()
        {
            // Arrange & Act
            var frameworks = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                targetFrameworks: "net45;netcoreapp1.1");

            // Assert
            Assert.Equal(2, frameworks.Count);
            Assert.Equal(NuGetFramework.Parse("net45"), frameworks[0]);
            Assert.Equal(NuGetFramework.Parse("netcoreapp1.1"), frameworks[1]);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyTargetFramworkParsed()
        {
            // Arrange & Act
            var frameworks = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                targetFrameworks: "net45");

            // Assert
            Assert.Equal(1, frameworks.Count);
            Assert.Equal(NuGetFramework.Parse("net45"), frameworks[0]);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyTargetFramworksUsedInsteadOfTargetFramework()
        {
            // Arrange & Act
            var frameworks = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                targetFrameworks: "net45;netcoreapp1.1",
                targetFramework: "net46");

            // Assert
            Assert.Equal(2, frameworks.Count);
            Assert.Equal(NuGetFramework.Parse("net45"), frameworks[0]);
            Assert.Equal(NuGetFramework.Parse("netcoreapp1.1"), frameworks[1]);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyTargetFrameworkMonikerUsed()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                targetFrameworkMoniker: ".NETFramework,Version=v4.5")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("net45"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyUAPFullVersion()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                targetPlatformIdentifier: "UAP",
                targetPlatformVersion: "10.0.1.2")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("UAP10.0.1.2"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyPlatformIgnoredWithoutVersion()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                targetPlatformIdentifier: "UAP",
                targetFrameworkMoniker: ".NETFramework,Version=v4.5")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("net45"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyUAPFullVersionForJavascript()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.jsproj",
                targetPlatformIdentifier: "UAP",
                targetPlatformVersion: "10.0.1.2")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("UAP10.0.1.2"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyJavascriptDefaultsToWindows()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.jsproj")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("win0.0"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyNETCore45ToWin8()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                targetFrameworkMoniker: ".NETCore,Version=v4.5")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("win8"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyNETCore451ToWin81()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                targetFrameworkMoniker: ".NETCore,Version=v4.5.1")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("win81"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyNETCore50IsUnchanged()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                targetFrameworkMoniker: ".NETCore,Version=v5.0")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("netcore5.0"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_IsXnaWindowsPhoneProjectVerifyReplacement()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                isXnaWindowsPhoneProject: true,
                targetFrameworkMoniker: ".NETFramework,Version=v4.0")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("Silverlight,Version=v4.0,Profile=WindowsPhone71"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_IsManagementPackProject()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                isManagementPackProject: true,
                targetFrameworkMoniker: ".NETFramework,Version=v4.0")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("SCMPInfra, Version=0.0"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyNativeProjectOverMoniker()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.vcxproj",
                targetFrameworkMoniker: ".NETFramework,Version=v4.0")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("native"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyUnknownMonikerIsParsed()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.csproj",
                targetFrameworkMoniker: "Blah,Version=v4.0")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.Parse("Blah,Version=v4.0"), framework);
        }

        [Fact]
        public void MSBuildProjectFrameworkUtility_VerifyFallbackToUnsupported()
        {
            // Arrange & Act
            var framework = GetFrameworks(
                projectFilePath: "/tmp/project.csproj")
                .SingleOrDefault();

            // Assert
            Assert.Equal(NuGetFramework.UnsupportedFramework, framework);
        }

        [Theory]
        [InlineData(".NETFramework,Version=v4.5", "", "", "net45")]
        [InlineData(".NETFramework,Version=4.5", "", "", "net45")]
        [InlineData(".NETFramework,Version=v4.5,Profile=Client", "", "", "net45-client")]
        [InlineData(".NETCoreApp,Version=v5.0", "android,Version=10.0", "", "net5.0-android10.0")]
        [InlineData(".NETCoreApp,Version=v5.0", "ios,Version=21.0", "", "net5.0-ios21.0")]
        [InlineData(".NETCoreApp,Version=v6.0", "ios,Version=21.0", "", "net6.0-ios21.0")]
        [InlineData(null, "ios,Version=21.0", "", "unsupported")]
        [InlineData(".NETCoreApp,Version=v6.0", "UAP,Version=10.0.1.2", "", "uap10.0.1.2")]
        [InlineData(".NETCoreApp,Version=v6.0", "UAP,Version=10.0.1.2", "10.0.1.3", "uap10.0.1.3")]
        [InlineData(".NETCoreApp,Version=v3.0", "android,Version=10.0", "", "netcoreapp3.0")]
        [InlineData(".NETCoreApp,Version=v3.0", "android,Version=10.0", "9.0", "netcoreapp3.0")]
        public void GetProjectFramework_WithCanonicalProperties_Succeeds(
                string targetFrameworkMoniker,
                string targetPlatformMoniker,
                string targetPlatformMinVersion,
                string expectedShortName)
        {
            var nugetFramework = MSBuildProjectFrameworkUtility.GetProjectFramework(
                projectFilePath: @"C:\csproj",
                targetFrameworkMoniker,
                targetPlatformMoniker,
                targetPlatformMinVersion);

            Assert.Equal(expectedShortName, nugetFramework.GetShortFolderName());
        }

        [Theory]
        [InlineData(".NETCoreApp,Version=v6.0", "ios,Version=5.0-preview.3", "")]
        [InlineData(".NETCoreApp,Version=v6.0-preview.3", "ios,Version=5.0", "")]
        [InlineData(".NETCoreApp,Version=v5.0,Profile=NET50CannotHaveProfiles", "android,Version=10", "")]
        public void GetProjectFramework_WithInvalidInput_Throws(
        string targetFrameworkMoniker,
        string targetPlatformMoniker,
        string targetPlatformMinVersion)
        {
            Assert.ThrowsAny<Exception>(() => MSBuildProjectFrameworkUtility.GetProjectFramework(
               projectFilePath: @"C:\csproj",
               targetFrameworkMoniker,
               targetPlatformMoniker,
               targetPlatformMinVersion));
        }

        [Theory]
        [InlineData(@"C:\project.vcxproj", ".NETFramework,Version=v4.5", "Windows,Version=7.0", "", "", null, "native", null)]
        [InlineData(@"C:\project.vcxproj", ".NETFramework,Version=v4.5", "", "", "false", null, "native", null)]
        [InlineData(@"C:\project.vcxproj", ".NETFramework,Version=v4.5", "", "", "NetFx", null, "native", null)]
        [InlineData(@"C:\project.vcxproj", ".NETCoreApp,Version=v5.0", "", "", "NetCore", null, "net5.0", "native")]
        [InlineData(@"C:\project.vcxproj", ".NETCoreApp,Version=v5.0", "Windows,Version=7.0", "", "NetCore", null, "net5.0-windows7.0", "native")]
        [InlineData(@"C:\project.vcxproj", ".NETCoreApp,Version=v5.0", "Windows,Version=7.0", "", "NetCore", "10.0.1234.1", "net5.0-windows10.0.1234.1", "native")]
        [InlineData(@"C:\project.csproj", ".NETCoreApp,Version=v5.0", "", "", "NetCore", null, "net5.0", null)]
        [InlineData(@"C:\project.csproj", ".NETFramework,Version=v4.5", "", "", "NetFramework", null, "net45", null)]
        public void GetProjectFramework_WithCLRSupport_VariousInputs(
               string projectFilePath,
               string targetFrameworkMoniker,
               string targetPlatformMoniker,
               string targetPlatformMinVersion,
               string clrSupport,
               string windowsTargetPlatformMinVersion,
               string expectedPrimaryShortName,
               string expectedSecondaryShortName)
        {
            var nugetFramework = MSBuildProjectFrameworkUtility.GetProjectFramework(
                projectFilePath: projectFilePath,
                targetFrameworkMoniker,
                targetPlatformMoniker,
                targetPlatformMinVersion,
                clrSupport,
                windowsTargetPlatformMinVersion);

            Assert.Equal(expectedPrimaryShortName, nugetFramework.GetShortFolderName());
            if (expectedSecondaryShortName != null)
            {
                Assert.IsAssignableFrom<DualCompatibilityFramework>(nugetFramework);
                var extendedFramework = nugetFramework as DualCompatibilityFramework;
                Assert.Equal(expectedPrimaryShortName, extendedFramework.RootFramework.GetShortFolderName());
                Assert.Equal(expectedSecondaryShortName, extendedFramework.SecondaryFramework.GetShortFolderName());
            }
            else
            {
                Assert.Null(nugetFramework as DualCompatibilityFramework);
            }
        }

        /// <summary>
        /// Test helper
        /// </summary>
        private static List<NuGetFramework> GetFrameworks(
            string projectFilePath,
            string targetFrameworks = "",
            string targetFramework = "",
            string targetFrameworkMoniker = "",
            string targetPlatformIdentifier = "",
            string targetPlatformVersion = "",
            string targetPlatformMinVersion = "",
            bool isXnaWindowsPhoneProject = false,
            bool isManagementPackProject = false)
        {
            return MSBuildProjectFrameworkUtility.GetProjectFrameworks(
                MSBuildProjectFrameworkUtility.GetProjectFrameworkStrings(
                    projectFilePath,
                    targetFrameworks,
                    targetFramework,
                    targetFrameworkMoniker,
                    targetPlatformIdentifier,
                    targetPlatformVersion,
                    targetPlatformMinVersion,
                    isXnaWindowsPhoneProject,
                    isManagementPackProject))
                    .ToList();
        }
    }
}
