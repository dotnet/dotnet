// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract partial class AbstractRazorProjectInfoDriver
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(AbstractRazorProjectInfoDriver instance)
    {
        public Task WaitUntilCurrentBatchCompletesAsync()
            => instance._workQueue.WaitUntilCurrentBatchCompletesAsync();
    }
}
