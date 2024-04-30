// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.VisualStudio.Razor.LanguageClient.ProjectSystem;

internal partial class RazorProjectInfoEndpointPublisher
{
    internal TestAccessor GetTestAccessor()
        => new(this);

    internal sealed class TestAccessor(RazorProjectInfoEndpointPublisher instance)
    {
        /// <summary>
        /// Allows unit tests to imitate ProjectManager.Changed event firing
        /// </summary>
        public void ProjectManager_Changed(object sender, ProjectChangeEventArgs args)
            => instance.ProjectManager_Changed(sender, args);

        /// <summary>
        /// Allows unit tests to enqueue project update directly.
        /// </summary>
        public void EnqueuePublish(IProjectSnapshot projectSnapshot)
             => instance.EnqueuePublish(projectSnapshot);

        /// <summary>
        /// Allows unit tests to wait for all work queue items to be processed.
        /// </summary>
        public Task WaitUntilCurrentBatchCompletesAsync()
            => instance._workQueue.WaitUntilCurrentBatchCompletesAsync();
    }
}
