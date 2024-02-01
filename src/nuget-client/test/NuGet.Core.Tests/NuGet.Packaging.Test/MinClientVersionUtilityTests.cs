// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Xml.Linq;
using NuGet.Common;
using NuGet.Versioning;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class MinClientVersionUtilityTests
    {
        [Theory]
        [InlineData("1.0.0", true)]
        [InlineData("4.0.0", true)]
        [InlineData("9.0.0", false)]
        public void MinClientVersionUtility_CheckCompatible(string version, bool expected)
        {
            // Arrange && Act
            var result = MinClientVersionUtility.IsMinClientVersionCompatible(NuGetVersion.Parse(version));

            // Assert
            Assert.Equal(expected, result);
        }

        // Verify an exact match is compatible
        [Fact]
        public void MinClientVersionUtility_CurrentVersionIsCompatible()
        {
            // Arrange && Act
            var result = MinClientVersionUtility.IsMinClientVersionCompatible(MinClientVersionUtility.GetNuGetClientVersion());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void MinClientVersionUtility_ReadFromNuspecNull()
        {
            // Arrange
            var nuspec = GetNuspec();

            // Act
            var result = MinClientVersionUtility.IsMinClientVersionCompatible(nuspec);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void MinClientVersionUtility_ReadFromNuspecCompat()
        {
            // Arrange
            var nuspec = GetNuspec("2.8.6");

            // Act
            var result = MinClientVersionUtility.IsMinClientVersionCompatible(nuspec);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void MinClientVersionUtility_ReadFromNuspecInCompat()
        {
            // Arrange
            var nuspec = GetNuspec("99.0.0");

            // Act
            var result = MinClientVersionUtility.IsMinClientVersionCompatible(nuspec);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void MinClientVersionUtility_ReadFromNuspecInCompatThrows()
        {
            // Arrange
            var nuspec = GetNuspec("99.0.0");

            // Act & Assert
            Assert.Throws<MinClientVersionException>(() => MinClientVersionUtility.VerifyMinClientVersion(nuspec));
        }

        [Fact]
        public void MinClientVersionUtility_ReadFromNuspecInCompatThrowsVerifyLogMessage()
        {
            // Arrange
            var nuspec = GetNuspec("99.0.0");
            ILogMessage logMessage = null;

            // Act
            try
            {
                MinClientVersionUtility.VerifyMinClientVersion(nuspec);
            }
            catch (MinClientVersionException ex)
            {
                logMessage = ex.AsLogMessage();
            }

            // Assert
            Assert.Equal(NuGetLogCode.NU1401, logMessage.Code);
            Assert.Contains("requires NuGet client version", logMessage.Message);
            Assert.Equal(LogLevel.Error, logMessage.Level);
        }

        [Fact]
        public void MinClientVersionUtility_ReadFromNuspecCompatDoesNotThrow()
        {
            // Arrange
            var nuspec = GetNuspec("2.0.0");

            // Act & Assert
            MinClientVersionUtility.VerifyMinClientVersion(nuspec);
        }

        [Fact]
        public void MinClientVersionUtility_ReadFromNuspecNullDoesNotThrow()
        {
            // Arrange
            var nuspec = GetNuspec();

            // Act & Assert
            MinClientVersionUtility.VerifyMinClientVersion(nuspec);
        }

        private static NuspecReader GetNuspec(string version)
        {
            var nuspecXml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata minClientVersion=""{version}"">
                            <id>packageA</id>
                            <version>1.0.0</version>
                            <title />
                            <frameworkAssemblies>
                                <frameworkAssembly assemblyName=""System.Runtime"" />
                            </frameworkAssemblies>
                            <contentFiles>
                                <files include=""cs/net45/config/config.xml"" buildAction=""none"" />
                                <files include=""cs/net45/config/config.xml"" copyToOutput=""true"" flatten=""false"" />
                                <files include=""cs/net45/images/image.jpg"" buildAction=""embeddedresource"" />
                            </contentFiles>
                        </metadata>
                        </package>";

            return new NuspecReader(XDocument.Parse(nuspecXml));
        }

        private static NuspecReader GetNuspec()
        {
            var nuspecXml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <package>
                        <metadata>
                            <id>packageA</id>
                            <version>1.0.0</version>
                            <title />
                            <frameworkAssemblies>
                                <frameworkAssembly assemblyName=""System.Runtime"" />
                            </frameworkAssemblies>
                            <contentFiles>
                                <files include=""cs/net45/config/config.xml"" buildAction=""none"" />
                                <files include=""cs/net45/config/config.xml"" copyToOutput=""true"" flatten=""false"" />
                                <files include=""cs/net45/images/image.jpg"" buildAction=""embeddedresource"" />
                            </contentFiles>
                        </metadata>
                        </package>";

            return new NuspecReader(XDocument.Parse(nuspecXml));
        }
    }
}
