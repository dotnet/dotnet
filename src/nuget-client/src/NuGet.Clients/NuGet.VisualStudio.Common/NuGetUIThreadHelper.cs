// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;

namespace NuGet.VisualStudio
{
    public static class NuGetUIThreadHelper
    {
        /// <summary>
        /// Initially it will be null and will be initialized to CPS JTF when there is CPS
        /// based project is being created.
        /// </summary>
        private static Lazy<JoinableTaskFactory> LazyJoinableTaskFactory;

        /// <summary>
        /// Returns the static instance of JoinableTaskFactory set by SetJoinableTaskFactoryFromService.
        /// If this has not been set yet the shell JTF will be used.
        /// During MEF composition some components will immediately call into the thread helper before
        /// it can be initialized. For this reason we need to fall back to the default shell JTF
        /// to provide basic threading support.
        /// </summary>
        public static JoinableTaskFactory JoinableTaskFactory
        {
            get
            {
                return LazyJoinableTaskFactory?.Value ?? GetThreadHelperJoinableTaskFactorySafe();
            }
        }

        public static void SetCustomJoinableTaskFactory(JoinableTaskFactory joinableTaskFactory)
        {
            Assumes.Present(joinableTaskFactory);

            // This is really just a test-hook
            LazyJoinableTaskFactory = new Lazy<JoinableTaskFactory>(() => joinableTaskFactory);
        }

        private static JoinableTaskFactory GetThreadHelperJoinableTaskFactorySafe()
        {
            // Static getter ThreadHelper.JoinableTaskContext, throws NullReferenceException if VsTaskLibraryHelper.ServiceInstance is null
            // And, ThreadHelper.JoinableTaskContext is simply 'ThreadHelper.JoinableTaskContext?.Factory'. Hence, this helper
            return VsTaskLibraryHelper.ServiceInstance != null ? ThreadHelper.JoinableTaskFactory : null;
        }
    }
}
