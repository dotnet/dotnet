// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Utilities;
using NuGet.Commands;
using NuGet.Configuration;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    [Export(typeof(INuGetUIFactory))]
    internal sealed class NuGetUIFactory : INuGetUIFactory
    {
        [Import]
        private ICommonOperations CommonOperations { get; set; }

        [Import]
        private Lazy<IDeleteOnRestartManager> DeleteOnRestartManager { get; set; }

        [Import]
        private Lazy<INuGetLockService> LockService { get; set; }

        [Import]
        private Lazy<IOptionsPageActivator> OptionsPageActivator { get; set; }

        [Import]
        private INuGetUILogger OutputConsoleLogger { get; set; }

        [Import]
        private Lazy<IPackageRestoreManager> PackageRestoreManager { get; set; }

        [Export(typeof(INuGetProjectContext))]
        private NuGetUIProjectContext ProjectContext { get; }

        [Import]
        private Lazy<ISettings> Settings { get; set; }

        [Import]
        private IVsSolutionManager SolutionManager { get; set; }

        [Import]
        private SolutionUserOptions SolutionUserOptions { get; set; }

        [Import]
        private Lazy<ISourceRepositoryProvider> SourceRepositoryProvider { get; set; }

        [Import]
        private Lazy<IRestoreProgressReporter> RestoreProgressReporter { get; set; }

        [ImportingConstructor]
        public NuGetUIFactory(
            ICommonOperations commonOperations,
            INuGetUILogger logger,
            ISourceControlManagerProvider sourceControlManagerProvider)
        {
            ProjectContext = new NuGetUIProjectContext(
                commonOperations,
                logger,
                sourceControlManagerProvider);
        }

        /// <summary>
        /// Returns the UI for the project or given set of projects.
        /// </summary>
        public async ValueTask<INuGetUI> CreateAsync(
            IServiceBroker serviceBroker,
            params IProjectContextInfo[] projects)
        {
            if (serviceBroker is null)
            {
                throw new ArgumentNullException(nameof(serviceBroker));
            }

            var adapterLogger = new LoggerAdapter(ProjectContext);

            ProjectContext.PackageExtractionContext = new PackageExtractionContext(
                PackageSaveMode.Defaultv2,
                PackageExtractionBehavior.XmlDocFileSaveMode,
                ClientPolicyContext.GetClientPolicy(Settings.Value, adapterLogger),
                adapterLogger);

            return await NuGetUI.CreateAsync(
                serviceBroker,
                CommonOperations,
                ProjectContext,
                SourceRepositoryProvider.Value,
                Settings.Value,
                SolutionManager,
                PackageRestoreManager.Value,
                OptionsPageActivator.Value,
                SolutionUserOptions,
                DeleteOnRestartManager.Value,
                SolutionUserOptions,
                LockService.Value,
                OutputConsoleLogger,
                RestoreProgressReporter.Value,
                CancellationToken.None,
                projects);
        }
    }
}
