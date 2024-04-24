// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal partial class ProjectConfigurationStateSynchronizer
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal sealed class TestAccessor(ProjectConfigurationStateSynchronizer instance)
    {
        public Task WaitUntilCurrentBatchCompletesAsync()
            => instance._workQueue.WaitUntilCurrentBatchCompletesAsync();
    }
}
