// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using Xunit;

namespace NuGet.Packaging.Test
{
    public class PackageIdValidatorTest
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ValidatePackageIdInvalidIdThrows(bool useRestrictedSet)
        {
            // Arrange
            string packageId = "  Invalid  . Woo   .";

            // Act & Assert
            ExceptionAssert.ThrowsArgumentException(() => PackageIdValidator.ValidatePackageId(packageId, useRestrictedSet), "The package ID '  Invalid  . Woo   .' contains invalid characters. Examples of valid package IDs include 'MyPackage' and 'MyPackage.Sample'.");
        }

        [Fact]
        public void EmptyIsNotValid()
        {
            // Arrange
            string packageId = "";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("Package\n")]
        [InlineData("Package.Name\n")]
        [InlineData("Package\n\n")]
        [InlineData("Package\r\n")]
        [InlineData("Package\r")]
        [InlineData("Package\t")]
        [InlineData("Package ")]
        [InlineData(" Package")]
        [InlineData("\nPackage")]
        [InlineData("Pack\nage")]
        public void PackageIdWithWhitespaceIsInvalid(string packageId)
        {
            Assert.False(PackageIdValidator.IsValidPackageId(packageId));
            Assert.False(PackageIdValidator.IsValidPackageId(packageId, useRestrictedCharacterSet: false));
            Assert.False(PackageIdValidator.IsValidPackageId(packageId, useRestrictedCharacterSet: true));
            Assert.Throws<ArgumentException>(() => PackageIdValidator.ValidatePackageId(packageId, useRestrictedSet: false));
            Assert.Throws<ArgumentException>(() => PackageIdValidator.ValidatePackageId(packageId, useRestrictedSet: true));
        }

        [Fact]
        public void NullThrowsException()
        {
            // Arrange
            string packageId = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(paramName: "packageId",
                testCode: () => PackageIdValidator.IsValidPackageId(packageId));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ValidatePackageId_Null_ThrowsArgumentNullException(bool useRestrictedSet)
        {
            Assert.Throws<ArgumentNullException>(
                paramName: "packageId",
                testCode: () => PackageIdValidator.ValidatePackageId(null, useRestrictedSet));
        }

        [Fact]
        public void AlphaNumericIsValid()
        {
            // Arrange
            string packageId = "42This1Is4You";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void MultipleDotSeparatorsAllowed()
        {
            // Arrange
            string packageId = "I.Like.Writing.Unit.Tests";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void NumbersAndWordsDotSeparatedAllowd()
        {
            // Arrange
            string packageId = "1.2.3.4.Uno.Dos.Tres.Cuatro";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void UnderscoreDotAndDashSeparatorsAreValid()
        {
            // Arrange
            string packageId = "Nu_Get.Core-IsCool";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void NonAlphaNumericUnderscoreDotDashIsInvalid()
        {
            // Arrange
            string packageId = "ILike*Asterisks";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ConsecutiveSeparatorsNotAllowed()
        {
            // Arrange
            string packageId = "I_.Like.-Separators";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void StartingWithSeparatorsNotAllowed()
        {
            // Arrange
            string packageId = "-StartWithSeparator";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void EndingWithSeparatorsNotAllowed()
        {
            // Arrange
            string packageId = "StartWithSeparator.";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void DotToolsIsNotAllowed()
        {
            // Arrange
            string packageId = ".tools";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValidPackageId_PackageIdWithTwoUnderscores_Success()
        {
            // Arrange
            string packageId = "Hello__World";

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValidPackageId_LongPackageIdWithUnMatchedCharAtTheEnd_Fails()
        {
            // Arrange
            string packageId = string.Concat(new string('_', 100), "!");

            // Act
            bool isValid = PackageIdValidator.IsValidPackageId(packageId);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(101)]
        [InlineData(102)]
        [InlineData(200)]
        public void ValidatePackageId_Unrestricted_AllowsLongIds(int idTestLength)
        {
            // Arrange
            string packageId = new string('d', idTestLength);

            // Act && Assert
            PackageIdValidator.ValidatePackageId(packageId, useRestrictedSet: false);
            Assert.True(PackageIdValidator.IsValidPackageId(packageId));
            Assert.True(PackageIdValidator.IsValidPackageId(packageId, useRestrictedCharacterSet: false));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ValidatePackageId_ExactMaxLength_DoesNotThrow(bool useRestrictedSet)
        {
            string packageId = new string('d', 100);

            PackageIdValidator.ValidatePackageId(packageId, useRestrictedSet);
        }

        [Theory]
        [InlineData(101)]
        [InlineData(200)]
        public void ValidatePackageId_Restricted_ExceedingMaxLengthThrows(int idTestLength)
        {
            string packageId = new string('d', idTestLength);

            ExceptionAssert.ThrowsArgumentException(
                () => PackageIdValidator.ValidatePackageId(packageId, useRestrictedSet: true),
                "Id must not exceed 100 characters.");
        }

        [Fact]
        public void ValidatePackageId_Restricted_RejectsNonAsciiCharacters()
        {
            string packageId = "Cont\u00F6so";

            PackageIdValidator.ValidatePackageId(packageId, useRestrictedSet: false);
            Assert.Throws<ArgumentException>(() => PackageIdValidator.ValidatePackageId(packageId, useRestrictedSet: true));
        }

        [Theory]
        [InlineData(101)]
        [InlineData(200)]
        public void ValidatePackageId_Obsolete_ExceedingMaxLengthThrows(int idTestLength)
        {
            string packageId = new string('d', idTestLength);

#pragma warning disable CS0618 // Verify compatibility of the obsolete overload.
            ExceptionAssert.ThrowsArgumentException(
                () => PackageIdValidator.ValidatePackageId(packageId),
                "Id must not exceed 100 characters.");
#pragma warning restore CS0618
        }

        [Fact]
        public void ValidatePackageId_Obsolete_AllowsNonAsciiCharacters()
        {
#pragma warning disable CS0618 // Verify compatibility of the obsolete overload.
            PackageIdValidator.ValidatePackageId("Cont\u00F6so");
#pragma warning restore CS0618
        }

        [Fact]
        public void ValidatePackageId_Obsolete_InvalidIdThrows()
        {
#pragma warning disable CS0618 // Verify compatibility of the obsolete overload.
            ExceptionAssert.ThrowsArgumentException(
                () => PackageIdValidator.ValidatePackageId("Invalid ID"),
                "The package ID 'Invalid ID' contains invalid characters. Examples of valid package IDs include 'MyPackage' and 'MyPackage.Sample'.");
#pragma warning restore CS0618
        }

        [Fact]
        public void ValidatePackageId_Obsolete_NullThrowsNullReferenceException()
        {
#pragma warning disable CS0618 // Verify compatibility of the obsolete overload.
            Assert.Throws<NullReferenceException>(() => PackageIdValidator.ValidatePackageId(null));
#pragma warning restore CS0618
        }

        [Theory]
        [InlineData("Contoso.Utilities")]
        [InlineData("My.Package-1.0")]
        [InlineData("A")]
        [InlineData("_internal")]
        [InlineData("1.2.3")]
        [InlineData("NuGet.Core")]
        [InlineData("Microsoft.Extensions.Logging")]
        public void IsValidPackageId_Restricted_ValidIds_ReturnsTrue(string packageId)
        {
            // Act
            bool result = PackageIdValidator.IsValidPackageId(packageId, useRestrictedCharacterSet: true);

            // Assert
            Assert.True(result, $"Expected '{packageId}' to be a valid restricted package ID.");
        }

        [Theory]
        [InlineData("Contöso.Utilities")]          // non-ASCII ö
        [InlineData("Contoso.Ütil")]               // non-ASCII Ü
        [InlineData("\u0421ontoso.Utilities")]      // Cyrillic С homoglyph
        [InlineData("Paquet.Français")]             // non-ASCII ç
        [InlineData("Paquete.Español")]             // non-ASCII ñ
        [InlineData("パッケージ")]                   // Japanese characters
        public void IsValidPackageId_Restricted_NonAsciiIds_ReturnsFalse(string packageId)
        {
            // Act
            bool result = PackageIdValidator.IsValidPackageId(packageId, useRestrictedCharacterSet: true);

            // Assert
            Assert.False(result, $"Expected '{packageId}' to be an invalid restricted package ID.");
        }

        [Fact]
        public void IsValidPackageId_Restricted_ExceedsMaxLength_ReturnsFalse()
        {
            // Arrange
            string packageId = new string('A', 101);

            // Act
            bool result = PackageIdValidator.IsValidPackageId(packageId, useRestrictedCharacterSet: true);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidPackageId_Restricted_ExactMaxLength_ReturnsTrue()
        {
            // Arrange
            string packageId = "A" + new string('b', 99);

            // Act
            bool result = PackageIdValidator.IsValidPackageId(packageId, useRestrictedCharacterSet: true);

            // Assert
            Assert.True(result);
        }
    }
}
