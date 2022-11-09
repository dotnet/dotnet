// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Test.Utility.Signing;
using Xunit;

namespace NuGet.Packaging.FuncTest
{
    [CollectionDefinition(Name)]
    public class SigningTestCollection
        : ICollectionFixture<SigningTestFixture>, ICollectionFixture<X509TrustTestFixture>
    {
        public const string Name = "Signing Functional Test Collection";

        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
