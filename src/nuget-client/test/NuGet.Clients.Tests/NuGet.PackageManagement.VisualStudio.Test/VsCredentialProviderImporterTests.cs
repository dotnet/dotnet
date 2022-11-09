// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    // This namespace declaration is on-purpose, to allow for testing the MEF imported VSTS credential provider without actually importing it.
    namespace TeamSystem.NuGetCredentialProvider
    {
        public class VisualStudioAccountProvider : IVsCredentialProvider
        {
            public Task<ICredentials> GetCredentialsAsync(Uri uri, IWebProxy proxy, bool isProxyRequest, bool isRetry, bool nonInteractive, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }

    internal class FailingCredentialProvider : NonFailingCredentialProvider
    {
        public FailingCredentialProvider()
        {
            // simulate a failure at contruction time
            throw new Exception("Exception thrown in constructor of FailingCredentialProvider");
        }
    }

    internal class NonFailingCredentialProvider : IVsCredentialProvider
    {
        public Task<ICredentials> GetCredentialsAsync(Uri uri, IWebProxy proxy, bool isProxyRequest, bool isRetry, bool nonInteractive, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    internal class ThirdPartyCredentialProvider : IVsCredentialProvider
    {
        public Task<ICredentials> GetCredentialsAsync(Uri uri, IWebProxy proxy, bool isProxyRequest, bool isRetry, bool nonInteractive, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class VsCredentialProviderImporterTests
    {
        private readonly StringBuilder _testErrorOutput = new StringBuilder();
        private readonly List<string> _errorMessages = new List<string>();
        private readonly Action<Exception, string> _errorDelegate;

        public VsCredentialProviderImporterTests()
        {
            _errorDelegate = (e, s) => _errorMessages.Add(s);
        }

        private void TestableErrorWriter(string s)
        {
            _testErrorOutput.AppendLine(s);
        }

        private VsCredentialProviderImporter GetTestableImporter()
        {
            var importer = new VsCredentialProviderImporter(
                _errorDelegate,
                initializer: () => { });

            return importer;
        }

        [Fact]
        public void WhenVstsIntializerThrows_ThenExceptionBubblesOut()
        {
            // Arrange
            var exception = new ArgumentException();
            var importer = new VsCredentialProviderImporter(
                _errorDelegate,
                () => { throw exception; });

            // Act & Assert
            var actual = Assert.Throws<ArgumentException>(() => importer.GetProviders());
            Assert.Same(exception, actual);
        }
    }
}
