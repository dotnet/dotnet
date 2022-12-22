// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Configuration;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Context for Package Management
    /// </summary>
    public class PackageManagementContext
    {
        public PackageManagementContext(
            ISourceRepositoryProvider sourceRepositoryProvider,
            ISolutionManager solutionManager,
            NuGet.Configuration.ISettings settings,
            ISourceControlManagerProvider sourceControlManagerProvider,
            ICommonOperations commonOperations)
        {
            SourceRepositoryProvider = sourceRepositoryProvider;
            VsSolutionManager = solutionManager;
            Settings = settings;
            SourceControlManagerProvider = sourceControlManagerProvider;
            CommonOperations = commonOperations;
        }

        /// <summary>
        /// Source repository provider
        /// </summary>
        public ISourceRepositoryProvider SourceRepositoryProvider { get; private set; }

        /// <summary>
        /// VS solution manager
        /// </summary>
        public ISolutionManager VsSolutionManager { get; private set; }

        /// <summary>
        /// NuGet config settings
        /// </summary>
        public NuGet.Configuration.ISettings Settings { get; private set; }

        /// <summary>
        /// SourceControlManager provider
        /// </summary>
        public ISourceControlManagerProvider SourceControlManagerProvider { get; private set; }

        /// <summary>
        /// CommonOperations to openfile, and so on
        /// </summary>
        public ICommonOperations CommonOperations { get; private set; }
    }
}
