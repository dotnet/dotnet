// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.VisualStudio.Razor.Discovery;

internal sealed partial class ProjectBuildDetector
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(ProjectBuildDetector instance)
    {
        public JoinableTask InitializeTask => instance._initializeTask;
        public Task? OnProjectBuiltTask => instance._projectBuiltTask;

        public Task OnProjectBuiltAsync(IVsHierarchy projectHierarchy, CancellationToken cancellationToken)
            => instance.OnProjectBuiltAsync(projectHierarchy, cancellationToken);
    }
}
