// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Razor.IntegrationTests.InProcess;

namespace Microsoft.VisualStudio.Extensibility.Testing;

internal partial class WorkspaceInProcess
{
    public Task WaitForAsyncOperationsAsync(string featuresToWaitFor, CancellationToken cancellationToken)
        => WaitForAsyncOperationsAsync(featuresToWaitFor, waitForWorkspaceFirst: true, cancellationToken);

    public async Task WaitForAsyncOperationsAsync(string featuresToWaitFor, bool waitForWorkspaceFirst, CancellationToken cancellationToken)
    {
        if (waitForWorkspaceFirst || featuresToWaitFor == FeatureAttribute.Workspace)
        {
            await WaitForProjectSystemAsync(cancellationToken);
        }

        // TODO: This currently no-ops on the FeaturesToWaitFor portion
        // because we lack any system to wait on it with
    }
}
