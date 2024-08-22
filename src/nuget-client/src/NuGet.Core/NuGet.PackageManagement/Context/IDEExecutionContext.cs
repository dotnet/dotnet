// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement
{
    public class IDEExecutionContext : NuGet.ProjectManagement.ExecutionContext
    {
        public ICommonOperations CommonOperations { get; }

        public IDEExecutionContext(ICommonOperations commonOperations)
        {
            if (commonOperations == null)
            {
                throw new ArgumentNullException(nameof(commonOperations));
            }
            CommonOperations = commonOperations;
        }

        public override async Task OpenFile(string fullPath)
        {
            await CommonOperations.OpenFile(fullPath);
        }

        public async Task SaveExpandedNodeStates(ISolutionManager solutionManager)
        {
            await CommonOperations.SaveSolutionExplorerNodeStates(solutionManager);
        }

        public async Task CollapseAllNodes(ISolutionManager solutionManager)
        {
            await CommonOperations.CollapseAllNodes(solutionManager);
        }

        public PackageIdentity IDEDirectInstall
        {
            get { return DirectInstall; }
            set { DirectInstall = value; }
        }
    }
}
