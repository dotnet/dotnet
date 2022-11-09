// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Sdk.TestFramework;
using Xunit;

namespace NuGet.VisualStudio.Implementation.Test
{
    /// <summary>
    /// Defines the "MockedVS" xunit test collection.
    /// </summary>
    [CollectionDefinition(CollectionName)]
    public class MockedVs : ICollectionFixture<GlobalServiceProvider>, ICollectionFixture<MefHostingFixture>
    {
        /// <summary>
        /// The name of the xunit test collection.
        /// </summary>
        public const string CollectionName = "MockedVS";
    }
}
