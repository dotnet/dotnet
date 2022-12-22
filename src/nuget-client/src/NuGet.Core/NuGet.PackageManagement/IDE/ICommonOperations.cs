// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace NuGet.PackageManagement
{
    public interface ICommonOperations
    {
        Task OpenFile(string fullPath);

        Task SaveSolutionExplorerNodeStates(ISolutionManager solutionManager);

        Task CollapseAllNodes(ISolutionManager solutionManager);
    }
}
