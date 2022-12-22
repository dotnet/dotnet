// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class WindowsEmbeddedSignatureVerifierTests
    {
        private readonly WindowsEmbeddedSignatureVerifier _verifier;

        public WindowsEmbeddedSignatureVerifierTests()
        {
            _verifier = new WindowsEmbeddedSignatureVerifier();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IsValid_ThrowsForNullOrEmpty(string filePath)
        {
            var expectedMessage = "Argument cannot be null or empty.";
            var expectedParam = "filePath";
            var exception = Assert.Throws<ArgumentException>(() => _verifier.IsValid(filePath));

            Assert.Contains(expectedMessage, exception.Message);
            Assert.Equal(expectedParam, exception.ParamName);
            //Remove the expected message from the exception message, the rest part should have param info.
            //Background of this change: System.ArgumentException(string message, string paramName) used to generate two lines of message before, but changed to generate one line
            //in PR: https://github.com/dotnet/coreclr/pull/25185/files#diff-0365d5690376ef849bf908dfc225b8e8
            var paramPart = exception.Message.Substring(exception.Message.IndexOf(expectedMessage) + expectedMessage.Length);
            Assert.Contains(expectedParam, paramPart);
        }

        [PlatformFact(Platform.Windows)]
        public void IsValid_ReturnsTrueForValidFile()
        {
            var filePath = Path.Combine(Environment.GetEnvironmentVariable("WINDIR"), "System32", "wintrust.dll");

            var actualResult = _verifier.IsValid(filePath);

            Assert.True(actualResult);
        }

        [PlatformFact(Platform.Windows)]
        public void IsValid_ReturnsFalseForNonExistentFilePath()
        {
            var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var actualResult = _verifier.IsValid(filePath);

            Assert.False(actualResult);
        }

        [PlatformFact(Platform.Windows)]
        public void IsValid_ReturnsFalseForInvalidFile()
        {
            using (var testDirectory = TestDirectory.Create())
            {
                // This is the build output from a default class library project.
                // There is nothing special about it; any unsigned DLL would work just as well.
                // This is a better test than an arbitrary file (e.g.:  file.txt) because
                // this is a portable executable (PE) file.  Authenticode signatures are
                // expected in PE files.
                var fileName = "DefaultClassLibrary.dll";
                var resourceName = $"NuGet.Protocol.Tests.compiler.resources.{fileName}";
                var filePath = Path.Combine(testDirectory.Path, fileName);

                ResourceTestUtility.CopyResourceToFile(resourceName, GetType(), filePath);

                var actualResult = _verifier.IsValid(filePath);

                Assert.False(actualResult);
            }
        }
    }
}
