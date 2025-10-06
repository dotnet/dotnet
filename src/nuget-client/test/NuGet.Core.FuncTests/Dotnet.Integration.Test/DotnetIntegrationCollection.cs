// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Xunit;

namespace Dotnet.Integration.Test
{
    [CollectionDefinition(Name)]
    public class DotnetIntegrationCollection
        : ICollectionFixture<DotnetIntegrationTestFixture>,
        ICollectionFixture<SignCommandTestFixture>,
        ICollectionFixture<X509TrustTestFixture>
    {
        internal const string Name = "Dotnet Integration Tests";

        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
