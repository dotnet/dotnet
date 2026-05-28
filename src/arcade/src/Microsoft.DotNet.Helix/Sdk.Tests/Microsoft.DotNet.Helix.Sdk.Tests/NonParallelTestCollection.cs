// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace Microsoft.DotNet.Helix.Sdk.Tests
{
    [CollectionDefinition("NonParallel", DisableParallelization = true)]
    public class NonParallelTestCollection { }
}
