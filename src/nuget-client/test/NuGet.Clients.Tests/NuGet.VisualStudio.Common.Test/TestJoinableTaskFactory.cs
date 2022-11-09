// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using Microsoft.VisualStudio.Threading;

#pragma warning disable VSSDK005 // Avoid instantiating JoinableTaskContext

namespace NuGet.VisualStudio.Common.Test
{
    public class TestJoinableTaskFactory
    {
        public TestJoinableTaskFactory()
        {
            var joinableTaskContext = new JoinableTaskContext(Thread.CurrentThread, SynchronizationContext.Current);
            NuGetUIThreadHelper.SetCustomJoinableTaskFactory(joinableTaskContext.Factory);
        }
    }
}
