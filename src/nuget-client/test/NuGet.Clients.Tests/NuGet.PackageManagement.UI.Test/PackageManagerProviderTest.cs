// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Microsoft.VisualStudio.Utilities;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.UI.Test
{
    public class PackageManagerProviderTest
    {
        [ImportMany(typeof(IVsPackageManagerProvider))]
        public IEnumerable<Lazy<IVsPackageManagerProvider, IOrderable>> PackageManagerProviders { get; set; }

        public PackageManagerProviderTest()
        {
            Init();
        }

        [Fact]
        public void PackageManagerProvider_SimpleSort()
        {
            // Act
            var sorted = PackageManagerProviderUtility.Sort(PackageManagerProviders, 3);

            // Assert
            Assert.Equal("test-version0", sorted[0].PackageManagerId);
            Assert.Equal("test-version1", sorted[1].PackageManagerId);
            Assert.Equal("test-version2", sorted[2].PackageManagerId);
        }

        [Fact]
        public void PackageManagerProvider_DuplicatedManagerId()
        {
            // Act
            var sorted = PackageManagerProviderUtility.Sort(PackageManagerProviders, 3);

            // Assert
            Assert.Equal("testUpdate", sorted[0].PackageManagerName);
            Assert.Equal("test-version1", sorted[1].PackageManagerId);
            Assert.Equal("test-version2", sorted[2].PackageManagerId);
        }

        [Fact]
        public void PackageManagerProvider_LimitedManagerProviders()
        {
            // Act
            var sorted = PackageManagerProviderUtility.Sort(PackageManagerProviders, 1);

            // Assert
            Assert.Equal(1, sorted.Count);
        }

        private void Init()
        {
            var catalog = new AssemblyCatalog
               (Assembly.GetExecutingAssembly());

            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }
}
