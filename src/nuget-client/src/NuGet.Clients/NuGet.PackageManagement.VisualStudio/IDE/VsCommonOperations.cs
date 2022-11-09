// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(ICommonOperations))]
    internal sealed class VsCommonOperations : ICommonOperations
    {
        private readonly AsyncLazy<EnvDTE.DTE> _dte;
        private IDictionary<string, ISet<VsHierarchyItem>> _expandedNodes;
        private readonly IAsyncServiceProvider _asyncServiceProvider;

        [ImportingConstructor]
        public VsCommonOperations(
            [Import(typeof(SVsServiceProvider))]
            IAsyncServiceProvider asyncServiceProvider)
        {
            Assumes.NotNull(asyncServiceProvider);
            _asyncServiceProvider = asyncServiceProvider;
            _dte = new AsyncLazy<EnvDTE.DTE>(async () =>
            {
                return await asyncServiceProvider.GetDTEAsync();
            }, NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public async Task OpenFile(string fullPath)
        {
            if (fullPath == null)
            {
                throw new ArgumentNullException(nameof(fullPath));
            }

            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            EnvDTE.DTE dte = await _dte.GetValueAsync();

            if (dte.ItemOperations != null
                && File.Exists(fullPath))
            {
                dte.ItemOperations.OpenFile(fullPath);
            }
        }

        public Task SaveSolutionExplorerNodeStates(ISolutionManager solutionManager)
        {
            if (solutionManager == null)
            {
                throw new ArgumentNullException(nameof(solutionManager));
            }

            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                _expandedNodes = await VsHierarchyUtility.GetAllExpandedNodesAsync();

                return Task.CompletedTask;
            });
        }

        public Task CollapseAllNodes(ISolutionManager solutionManager)
        {
            if (solutionManager == null)
            {
                throw new ArgumentNullException(nameof(solutionManager));
            }

            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                await VsHierarchyUtility.CollapseAllNodesAsync(_expandedNodes);

                return Task.CompletedTask;
            });
        }
    }
}
