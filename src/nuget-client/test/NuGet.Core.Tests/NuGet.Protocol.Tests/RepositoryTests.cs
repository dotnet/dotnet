// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public void Factory_Always_ReturnsSameInstance()
        {
            Repository.RepositoryFactory instance0 = Repository.Factory;
            Repository.RepositoryFactory instance1 = Repository.Factory;

            Assert.Same(instance0, instance1);
        }

        [Fact]
        public void Provider_Always_ReturnsSameInstance()
        {
            Repository.ProviderFactory instance0 = Repository.Provider;
            Repository.ProviderFactory instance1 = Repository.Provider;

            Assert.Same(instance0, instance1);
        }

        [Fact]
        public void Provider_WhenSettingNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Repository.Provider = null);

            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void Provider_WhenSetting_SetsFactory()
        {
            // It's important to test robustness that we use the same type as the default.
            var newProviderFactory = new Repository.ProviderFactory();

            Repository.Provider = newProviderFactory;

            Assert.Same(newProviderFactory, Repository.Provider);
        }

        [Fact]
        public void Provider_WhenGetting_ReturnsDefault()
        {
            Repository.ProviderFactory provider = Repository.Provider;

            Assert.Equal(typeof(Repository.ProviderFactory).FullName, provider.GetType().FullName);
        }

        [Fact]
        public void Provider_WithDefaultProvider_ReturnsDefaultResourceProviders()
        {
            IEnumerable<Lazy<INuGetResourceProvider>> resourceProviders = Repository.Provider.GetCoreV3();

            int actualCount = resourceProviders.Count();

            Assert.Equal(47, actualCount);
        }
    }
}
