// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
