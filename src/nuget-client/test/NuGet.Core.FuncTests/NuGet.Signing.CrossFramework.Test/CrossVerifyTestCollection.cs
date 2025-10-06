// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Xunit;

namespace NuGet.Signing.CrossFramework.Test
{
    [CollectionDefinition(Name)]
    public class CrossVerifyTestCollection
        : ICollectionFixture<CrossVerifyTestFixture>, ICollectionFixture<X509TrustTestFixture>
    {
        public const string Name = "NuGet Cross Verify Test Collection";
    }
}
