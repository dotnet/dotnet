// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Workspace.VSIntegration.UI;

namespace NuGet.VisualStudio.OnlineEnvironment.Client
{
    /// <summary>
    /// Extends the Solution Explorer in online environment scenarios by adding command handlers
    /// </summary>
    [ExportNodeExtender(OnlineEnvironment.LiveShareSolutionView)]
    internal sealed class NuGetNodeExtender : INodeExtender
    {
        /// <summary>
        /// The shared command handler for all nodes NuGet cares about.
        /// </summary>
        private readonly IWorkspaceCommandHandler _commandHandler;

        [ImportingConstructor]
        public NuGetNodeExtender(JoinableTaskContext taskContext) :
            this(taskContext, AsyncServiceProvider.GlobalProvider)
        {
        }

        public NuGetNodeExtender(
            JoinableTaskContext taskContext,
            IAsyncServiceProvider asyncServiceProvider)
        {
            _commandHandler = new NuGetWorkspaceCommandHandler(taskContext, asyncServiceProvider);
        }

        public IChildrenSource ProvideChildren(WorkspaceVisualNodeBase parentNode)
        {
            return null;
        }

        /// <summary>
        /// Provides our <see cref="IWorkspaceCommandHandler"/> for nodes representing
        /// managed projects.
        /// </summary>
        public IWorkspaceCommandHandler ProvideCommandHandler(WorkspaceVisualNodeBase parentNode)
        {
            return _commandHandler;
        }
    }
}
