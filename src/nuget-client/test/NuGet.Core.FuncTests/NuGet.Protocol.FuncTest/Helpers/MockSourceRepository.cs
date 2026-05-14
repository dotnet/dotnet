// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using Test.Utility;

namespace NuGet.Protocol.FuncTest.Helpers
{
    internal class MockSourceRepository
    {
        /// <summary>Creates an <see cref="SourceRepository"/></summary>
        /// <param name="mockClientHandler">The </param>
        /// <returns>A <see cref="SourceRepository"/> that looks like a remote V3 feed.</returns>
        /// <remarks>This package source is <b>slow</b>. It should only be used to test the implementation of NuGet
        /// Protocol V3 resources in advanced scenarios. Tests that use a SourceRepository should use a faster
        /// mock.</remarks>
        public static SourceRepository Create(MockServerHttpClientHandler httpClientHandler)
        {
            TestHttpHandlerProvider provider = new TestHttpHandlerProvider(() => httpClientHandler);

            var providers = Repository.Provider.GetCoreV3().Concat(new[] { new Lazy<INuGetResourceProvider>(() => provider) });
            var packageSource = new PackageSource(httpClientHandler.BaseAddress.OriginalString, "testSource");
            var source = new SourceRepository(packageSource, providers);

            return source;
        }
    }
}
