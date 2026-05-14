// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class FactoryExtensionsV3Tests
    {
        [Fact]
        public void GetCoreV3_WhenFactoryIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => FactoryExtensionsV3.GetCoreV3(factory: null));

            Assert.Equal("factory", exception.ParamName);
        }

        [Fact]
        public void GetCoreV3_WithDefaultFactory_ReturnsNonEmptyEnumerable()
        {
            IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders = FactoryExtensionsV3.GetCoreV3(Repository.Provider);

            Assert.NotEmpty(resourceProviders);
        }

        [Fact]
        public void GetCoreV3_WithCustomFactory_ReturnsCustomResult()
        {
            var factory = new CustomProviderFactory();
            IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders = FactoryExtensionsV3.GetCoreV3(factory);

            Assert.Same(factory.ExpectedResult, resourceProviders);
        }

        private sealed class CustomProviderFactory : Repository.ProviderFactory
        {
            internal IEnumerable<Lazy<INuGetResourceProvider>> ExpectedResult { get; }

            internal CustomProviderFactory()
            {
                ExpectedResult = new Lazy<INuGetResourceProvider>[] { };
            }

            public override IEnumerable<Lazy<INuGetResourceProvider>> GetCoreV3()
            {
                return ExpectedResult;
            }
        }
    }
}
