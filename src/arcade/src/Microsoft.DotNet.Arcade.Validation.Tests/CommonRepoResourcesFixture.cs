// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Xunit;

namespace Validation.Tests
{
    public class CommonRepoResourcesFixture : IAsyncLifetime
    {
        public RepoResources CommonResources { get; private set; }

        public async Task InitializeAsync()
        {
            CommonResources = await RepoResources.Create(useIsolatedRoots: false);
        }

        public Task DisposeAsync()
        {
            CommonResources?.Dispose();
            return Task.CompletedTask;
        }
    }
}
