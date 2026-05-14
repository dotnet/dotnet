// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Internal.NuGet.Testing.SignedPackages;
using Xunit;

namespace NuGet.MSSigning.Extensions.FuncTest.Commands
{
    [CollectionDefinition(Name)]
    public class MSSignCommandTestCollection
        : ICollectionFixture<MSSignCommandTestFixture>, ICollectionFixture<X509TrustTestFixture>
    {
        public const string Name = "MSSign Command Test Collection";

        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
