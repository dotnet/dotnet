// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Test.Utility.Signing;
using Xunit;

namespace NuGet.Commands.FuncTest
{
    [CollectionDefinition(Name)]
    public sealed class TestCollection : ICollectionFixture<X509TrustTestFixture>
    {
        internal const string Name = "NuGet Commands Tests";

        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
