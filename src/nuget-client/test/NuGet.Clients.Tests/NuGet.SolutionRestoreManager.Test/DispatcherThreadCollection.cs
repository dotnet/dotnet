// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Test.Utility.Threading;
using Xunit;

namespace NuGet.SolutionRestoreManager.Test
{
    /// <summary>
    /// Represents a test collection fixture shared among multiple test classes.
    /// Provides access to a JTF instance for running tests.
    /// This class has no code, and is never created. Its purpose is simply
    /// to be the place to apply [CollectionDefinition] and all the
    /// ICollectionFixture<> interfaces.
    /// </summary>
    [CollectionDefinition(CollectionName)]
    public class DispatcherThreadCollection : ICollectionFixture<DispatcherThreadFixture>
    {
        public const string CollectionName = "Dispatcher thread collection";
    }
}
