// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public class DefaultOmniSharpProjectSnapshotManagerDispatcher : OmniSharpProjectSnapshotManagerDispatcher
{
    public DefaultOmniSharpProjectSnapshotManagerDispatcher()
    {
        InternalDispatcher = new OmniSharpProjectSnapshotManagerDispatcher();
    }

    public override TaskScheduler DispatcherScheduler => InternalDispatcher.DispatcherScheduler;

    public override void AssertDispatcherThread([CallerMemberName] string caller = null) => InternalDispatcher.AssertDispatcherThread(caller);

    private class OmniSharpProjectSnapshotManagerDispatcher : ProjectSnapshotManagerDispatcherBase
    {
        private const string ThreadName = "Razor." + nameof(OmniSharpProjectSnapshotManagerDispatcher);

        public OmniSharpProjectSnapshotManagerDispatcher() : base(ThreadName)
        {
        }

        public override void LogException(Exception ex)
        {
            // We don't currently have logging mechanisms in place for O#.
        }
    }
}
