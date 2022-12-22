// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;

using Xunit;

namespace NuGet.Packaging.Test
{
    public class ManifestFileTest
    {
        [Fact]
        public void ManifestFileReturnsNoValidationResultsIfSourceAndTargetPathAreValid()
        {
            // Arrange
            var manifestFile = new ManifestFile { Source = @"bin\release\MyLib.dll".Replace('\\', Path.DirectorySeparatorChar), Target = @"lib" };

            // Act
            var result = manifestFile.Validate();

            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        public void ManifestFileReturnsNoValidationResultIfSourceContainsWildCardCharacters()
        {
            // Arrange
            var manifestFile = new ManifestFile { Source = @"b?n\**\*.dll".Replace('\\', Path.DirectorySeparatorChar), Target = @"lib" };

            // Act
            var result = manifestFile.Validate();

            // Assert
            Assert.False(result.Any());
        }

        [Fact]
        public void ManifestFileReturnsValidationResultIfSourceContainsInvalidCharacters()
        {
            // Arrange
            var manifestFile = new ManifestFile { Source = @"bin\\|\\*.dll".Replace('\\', Path.DirectorySeparatorChar), Target = @"lib" };

            // Act
            var result = manifestFile.Validate().ToList();

            // Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(@"Source path 'bin\\|\\*.dll' contains invalid characters.".Replace('\\', Path.DirectorySeparatorChar), result.Single());
        }

        [Fact]
        public void ManifestFileReturnsValidationResultIfTargetContainsInvalidCharacters()
        {
            // Arrange
            var manifestFile = new ManifestFile { Source = @"bin\\**\\*.dll".Replace('\\', Path.DirectorySeparatorChar), Target = @"lib\\|\\net40".Replace('\\', Path.DirectorySeparatorChar) };

            // Act
            var result = manifestFile.Validate().ToList();

            // Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(@"Target path 'lib\\|\\net40' contains invalid characters.".Replace('\\', Path.DirectorySeparatorChar), result.Single());
        }

        [Fact]
        public void ManifestFileReturnsValidationResultsIfSourceAndTargetContainsInvalidCharacters()
        {
            // Arrange
            var manifestFile = new ManifestFile { Source = @"bin|\\**\\*.dll".Replace('\\', Path.DirectorySeparatorChar), Target = @"lib\\|\\net40".Replace('\\', Path.DirectorySeparatorChar) };

            // Act
            var result = manifestFile.Validate().ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(@"Source path 'bin|\\**\\*.dll' contains invalid characters.".Replace('\\', Path.DirectorySeparatorChar), result.First());
            Assert.Equal(@"Target path 'lib\\|\\net40' contains invalid characters.".Replace('\\', Path.DirectorySeparatorChar), result.Last());
        }

        [Fact]
        public void ManifestFileReturnsValidationResultsIfTargetPathContainsWildCardCharacters()
        {
            // Arrange
            var manifestFile = new ManifestFile { Source = @"bin\\**\\*.dll".Replace('\\', Path.DirectorySeparatorChar), Target = @"lib\\**\\net40".Replace('\\', Path.DirectorySeparatorChar) };

            // Act
            var result = manifestFile.Validate().ToList();

            // Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(@"Target path 'lib\\**\\net40' contains invalid characters.".Replace('\\', Path.DirectorySeparatorChar), result.Single());
        }
    }
}
