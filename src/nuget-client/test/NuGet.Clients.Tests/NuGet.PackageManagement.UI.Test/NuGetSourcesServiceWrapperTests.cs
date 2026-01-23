// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio.Internal.Contracts;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class NuGetSourcesServiceWrapperTests : IDisposable
    {
        private readonly NuGetSourcesServiceWrapper _wrapper;

        public NuGetSourcesServiceWrapperTests()
        {
            _wrapper = new NuGetSourcesServiceWrapper();
        }

        public void Dispose()
        {
            _wrapper.Dispose();
        }

        [Fact]
        public void Service_Always_ReturnsNonNullValue()
        {
            Assert.NotNull(_wrapper.Service);

            using (_wrapper.Swap(newService: null))
            {
                Assert.NotNull(_wrapper.Service);
            }
        }

        [Fact]
        public void Swap_WhenNewServiceIsNull_ReturnsPreviousService()
        {
            using (INuGetSourcesService previousService = _wrapper.Swap(newService: null))
            {
                Assert.NotNull(previousService);
            }
        }

        [Fact]
        public void Swap_WhenNewServiceIsNonNull_ReturnsPreviousService()
        {
            using (INuGetSourcesService expectedResult = _wrapper.Service)
            using (INuGetSourcesService actualResult = _wrapper.Swap(new TestNuGetSourcesService()))
            {
                Assert.Same(expectedResult, actualResult);
            }
        }

        [Fact]
        public async Task GetActivePackageSourceNameAsync_Always_ReturnsActivePackageSourceName()
        {
            var service = new TestNuGetSourcesService();

            using (_wrapper.Swap(service))
            {
                const string expectedResult = "a";

                service.ActivePackageSourceName = expectedResult;

                string actualResult = await _wrapper.GetActivePackageSourceNameAsync(CancellationToken.None);

                Assert.Equal(expectedResult, actualResult);
            }
        }

        private sealed class TestNuGetSourcesService : INuGetSourcesService
        {
            internal string ActivePackageSourceName { get; set; }
            internal IReadOnlyList<PackageSourceContextInfo> PackageSources { get; set; }

            public void Dispose()
            {
            }

            public ValueTask<string> GetActivePackageSourceNameAsync(CancellationToken cancellationToken)
            {
                return new ValueTask<string>(ActivePackageSourceName);
            }

            public ValueTask SavePackageSourceContextInfosAsync(IReadOnlyList<PackageSourceContextInfo> sources, CancellationToken cancellationToken)
            {
                return new ValueTask();
            }

            public ValueTask<IReadOnlyList<PackageSourceContextInfo>> GetPackageSourcesAsync(CancellationToken cancellationToken)
            {
                return new ValueTask<IReadOnlyList<PackageSourceContextInfo>>(PackageSources);
            }

            public IReadOnlyList<SourceRepository> GetEnabledAuditSources()
            {
                throw new NotImplementedException();
            }
        }
    }
}
