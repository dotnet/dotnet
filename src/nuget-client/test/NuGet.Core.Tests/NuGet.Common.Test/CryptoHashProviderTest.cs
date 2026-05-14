// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NuGet.Common.Test
{
    public class CryptoHashProviderTest
    {
        [Fact]
        public void DefaultCryptoHashProviderUsesSHA512()
        {
            // Arrange
            var testBytes = Encoding.UTF8.GetBytes("There is no butter knife");
            var expectedHash = "xy/brd+/mxheBbyBL7i8Oyy62P2ZRteaIkfc4yA8ncH1MYkbDo+XwBcZsOBY2YeaOucrdLJj5odPvozD430w2g==";
            CryptoHashProvider hashProvider = new CryptoHashProvider();

            // Act
            byte[] actualHash = hashProvider.CalculateHash(testBytes);

            // Assert
            Assert.Equal(actualHash, Convert.FromBase64String(expectedHash));
        }

        [Fact]
        public void DefaultCryptoHashProviderUsesSHA512Stream()
        {
            // Arrange
            var testBytes = Encoding.UTF8.GetBytes("There is no butter knife");
            var expectedHash = "xy/brd+/mxheBbyBL7i8Oyy62P2ZRteaIkfc4yA8ncH1MYkbDo+XwBcZsOBY2YeaOucrdLJj5odPvozD430w2g==";
            CryptoHashProvider hashProvider = new CryptoHashProvider();
            var stream = new MemoryStream(testBytes);

            // Act
            byte[] actualHash = hashProvider.CalculateHash(stream);

            // Assert
            Assert.Equal(actualHash, Convert.FromBase64String(expectedHash));
        }

        [Theory]
        [UseCulture("en-US")] // We are asserting exception messages in English
        [InlineData("md5")]
        [InlineData("MD5")]
        [InlineData("SHA1")]
        [InlineData("SHA2561")]
        public void CryptoHashProviderThrowsIfHashAlgorithmIsNotSHA512orSHA256(string hashAlgorithm)
        {
            // Act and Assert
            var ex = Record.Exception(() => new CryptoHashProvider(hashAlgorithm));
            Assert.NotNull(ex);
            var tex = Assert.IsAssignableFrom<ArgumentException>(ex);
            Assert.Equal("hashAlgorithm", tex.ParamName);
            var expectedMessage = string.Format("Hash algorithm '{0}' is unsupported. Supported algorithms include: SHA512 and SHA256.", hashAlgorithm);
            Assert.Contains(expectedMessage, ex.Message);
            //Remove the expected message from the exception message, the rest part should have param info.
            //Background of this change: System.ArgumentException(string message, string paramName) used to generate two lines of message before, but changed to generate one line
            //in PR: https://github.com/dotnet/coreclr/pull/25185/files#diff-0365d5690376ef849bf908dfc225b8e8
            var paramPart = ex.Message.Substring(ex.Message.IndexOf(expectedMessage) + expectedMessage.Length);
            Assert.Contains("Parameter", paramPart);
            Assert.Contains("hashAlgorithm", paramPart);
        }

        [Theory]
        [InlineData("sha512", "xy/brd+/mxheBbyBL7i8Oyy62P2ZRteaIkfc4yA8ncH1MYkbDo+XwBcZsOBY2YeaOucrdLJj5odPvozD430w2g==")]
        [InlineData("SHA512", "xy/brd+/mxheBbyBL7i8Oyy62P2ZRteaIkfc4yA8ncH1MYkbDo+XwBcZsOBY2YeaOucrdLJj5odPvozD430w2g==")]
        [InlineData("sha256", "F7qs6AZmrGdFSsAc/EpRjjIgkhlW8M92djz8ySt48EM=")]
        public void CryptoHashProviderAllowsSHA512orSHA256(string hashAlgorithm, string expectedHash)
        {
            // Arrange
            var testBytes = Encoding.UTF8.GetBytes("There is no butter knife");
            var hashProvider = new CryptoHashProvider(hashAlgorithm);

            // Act
            var result = Convert.ToBase64String(hashProvider.CalculateHash(testBytes));

            // Assert
            Assert.Equal(expectedHash, result);
        }

        [Theory]
        [InlineData("sha512", "xy/brd+/mxheBbyBL7i8Oyy62P2ZRteaIkfc4yA8ncH1MYkbDo+XwBcZsOBY2YeaOucrdLJj5odPvozD430w2g==")]
        [InlineData("SHA512", "xy/brd+/mxheBbyBL7i8Oyy62P2ZRteaIkfc4yA8ncH1MYkbDo+XwBcZsOBY2YeaOucrdLJj5odPvozD430w2g==")]
        [InlineData("sha256", "F7qs6AZmrGdFSsAc/EpRjjIgkhlW8M92djz8ySt48EM=")]
        public void CryptoHashProviderAllowsSHA512orSHA256Stream(string hashAlgorithm, string expectedHash)
        {
            // Arrange
            var testBytes = Encoding.UTF8.GetBytes("There is no butter knife");
            var hashProvider = new CryptoHashProvider(hashAlgorithm);
            var stream = new MemoryStream(testBytes);

            // Act
            var result = Convert.ToBase64String(hashProvider.CalculateHash(stream));

            // Assert
            Assert.Equal(expectedHash, result);
        }

        [Fact]
        public void CryptoHashProviderReturnsTrueIfHashAreEqual()
        {
            // Arrange
            var testBytes = Encoding.UTF8.GetBytes("There is no butter knife");
            var expectedHash = "xy/brd+/mxheBbyBL7i8Oyy62P2ZRteaIkfc4yA8ncH1MYkbDo+XwBcZsOBY2YeaOucrdLJj5odPvozD430w2g==";
            CryptoHashProvider hashProvider = new CryptoHashProvider();

            // Act
            bool result = hashProvider.VerifyHash(testBytes, Convert.FromBase64String(expectedHash));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CryptoHashProviderReturnsFalseIfHashValuesAreUnequal()
        {
            // Arrange
            var testBytes = Encoding.UTF8.GetBytes("There is no butter knife");
            var badBytes = Encoding.UTF8.GetBytes("this is a bad input");
            CryptoHashProvider hashProvider = new CryptoHashProvider();

            // Act
            byte[] testHash = hashProvider.CalculateHash(testBytes);
            byte[] badHash = hashProvider.CalculateHash(badBytes);
            bool result = hashProvider.VerifyHash(testHash, badHash);

            // Assert
            Assert.False(result);
        }

        // Ensures this issue is fixed: http://nuget.codeplex.com/workitem/1489
        [Fact]
        public void CryptoHashProviderIsThreadSafe()
        {
            // Arrange
            var testBytes = Encoding.UTF8.GetBytes("There is no butter knife");
            var expectedHash = "xy/brd+/mxheBbyBL7i8Oyy62P2ZRteaIkfc4yA8ncH1MYkbDo+XwBcZsOBY2YeaOucrdLJj5odPvozD430w2g==";
            CryptoHashProvider hashProvider = new CryptoHashProvider();

            Parallel.For(0, 10000, ignored =>
                {
                    // Act
                    byte[] actualHash = hashProvider.CalculateHash(testBytes);

                    // Assert
                    Assert.Equal(actualHash, Convert.FromBase64String(expectedHash));
                });
        }
    }
}
