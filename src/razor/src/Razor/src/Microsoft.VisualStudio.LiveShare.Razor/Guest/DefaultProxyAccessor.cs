// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LiveShare.Razor.Guest;

[System.Composition.Shared]
[Export(typeof(ProxyAccessor))]
internal class DefaultProxyAccessor : ProxyAccessor
{
    private readonly LiveShareSessionAccessor _liveShareSessionAccessor;
    private readonly JoinableTaskFactory _joinableTaskFactory;
    private IProjectHierarchyProxy? _projectHierarchyProxy;

    [ImportingConstructor]
    public DefaultProxyAccessor(
        LiveShareSessionAccessor liveShareSessionAccessor,
        JoinableTaskContext joinableTaskContext)
    {
        if (liveShareSessionAccessor is null)
        {
            throw new ArgumentNullException(nameof(liveShareSessionAccessor));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        _liveShareSessionAccessor = liveShareSessionAccessor;
        _joinableTaskFactory = joinableTaskContext.Factory;
    }

    // Testing constructor
#pragma warning disable CS8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.
    private protected DefaultProxyAccessor()
#pragma warning restore CS8618 // Non-nullable variable must contain a non-null value when exiting constructor. Consider declaring it as nullable.
    {
    }

    public override IProjectHierarchyProxy GetProjectHierarchyProxy()
    {
        _projectHierarchyProxy ??= CreateServiceProxy<IProjectHierarchyProxy>();

        return _projectHierarchyProxy;
    }

    // Internal virtual for testing
    internal virtual TProxy CreateServiceProxy<TProxy>() where TProxy : class
    {
        Assumes.NotNull(_liveShareSessionAccessor.Session);
#pragma warning disable VSTHRD110 // Observe result of async calls
        return _joinableTaskFactory.Run(() => _liveShareSessionAccessor.Session.GetRemoteServiceAsync<TProxy>(typeof(TProxy).Name, CancellationToken.None));
#pragma warning restore VSTHRD110 // Observe result of async calls
    }
}
