// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Xunit;

namespace NuGet.Packaging.Test
{
    [CollectionDefinition(Name)]
    public sealed class SigningTestsCollection
        : ICollectionFixture<CertificatesFixture>, ICollectionFixture<X509TrustTestFixture>
    {
        internal const string Name = "Signing tests collection";

        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
