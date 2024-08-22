// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using NuGet.VisualStudio.Common;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.SolutionRestoreManager
{
    internal class NuGetSolutionService : INuGetSolutionService
    {
        [Import]
        internal Lazy<ISolutionRestoreWorker> SolutionRestoreWorker { get; set; }
        private readonly AsyncLazyInitializer _initializer;

        public NuGetSolutionService()
        {
            _initializer = new AsyncLazyInitializer(InitializeAsync, NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public async Task RestoreSolutionAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _initializer.InitializeAsync(cancellationToken);
            await SolutionRestoreWorker.Value.ScheduleRestoreAsync(SolutionRestoreRequest.ByUserCommand(ExplicitRestoreReason.RestoreSolutionPackages), cancellationToken);
        }

        private async Task InitializeAsync()
        {
            var componentModel = await ServiceLocator.GetComponentModelAsync();
            // ensure we satisfy our imports
            componentModel?.DefaultCompositionService.SatisfyImportsOnce(this);
        }

    }
}
