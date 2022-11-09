// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.Utilities;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Project system factory imlemented as a composite provider chaining calls to other providers.
    /// </summary>
    [Export]
    public sealed class NuGetProjectFactory
    {
        private readonly INuGetProjectProvider[] _providers;
        private readonly IVsProjectThreadingService _threadingService;
        private readonly Common.ILogger _logger;

        [ImportingConstructor]
        public NuGetProjectFactory(
            [ImportMany(typeof(INuGetProjectProvider))]
            IEnumerable<Lazy<INuGetProjectProvider, IOrderable>> providers,
            [Import]
            IVsProjectThreadingService threadingService,
            [Import("VisualStudioActivityLogger")]
            Common.ILogger logger)
        {
            Assumes.Present(providers);
            Assumes.Present(threadingService);
            Assumes.Present(logger);

            _providers = Orderer
                .Order(providers)
                .Select(p => p.Value)
                .ToArray();

            _threadingService = threadingService;
            _logger = logger;
        }

        /// <summary>
        /// Tries to create an instance of <see cref="NuGetProject"/> by calling registered 
        /// providers in predefined order.
        /// </summary>
        /// <param name="vsProjectAdapter">A project adapter.</param>
        /// <param name="context">Project context</param>
        /// <param name="result">New project instance when <code>true</code> is returned. 
        /// Otherwise - <code>null</code>.</param>
        /// <returns><code>true</code> when new project instance has been successfully created.</returns>
        public async Task<NuGetProject> TryCreateNuGetProjectAsync(
            IVsProjectAdapter vsProjectAdapter,
            ProjectProviderContext context)
        {
            Assumes.Present(vsProjectAdapter);
            Assumes.Present(context);

            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            var exceptions = new List<Exception>();
            foreach (var provider in _providers)
            {
                try
                {
                    var nuGetProject = await provider.TryCreateNuGetProjectAsync(
                        vsProjectAdapter,
                        context,
                        forceProjectType: false);

                    if (nuGetProject != null)
                    {
                        return nuGetProject;
                    }
                }
                catch (Exception e)
                {
                    // Ignore failures. If this method returns null, the problem falls 
                    // into one of the other NuGet project types.
                    exceptions.Add(e);
                }
            }

            exceptions.ForEach(e => _logger.LogError(e.ToString()));

            return null;
        }

        /// <summary>
        /// Creates an instance of <see cref="NuGetProject"/> of desired type.
        /// </summary>
        /// <typeparam name="TProject">Type of project to create.</typeparam>
        /// <param name="vsProjectAdapter">Project adapter</param>
        /// <param name="optionalContext">Optional context for project creation. Not all of providers require it.</param>
        /// <returns>Instance of <see cref="NuGetProject"/> or null if failed.</returns>
        /// <remarks>The factory will identify a provider corresponding to given project type and will attempt to force create the project of desired type.</remarks>
        public async Task<TProject> CreateNuGetProjectAsync<TProject>(
            IVsProjectAdapter vsProjectAdapter,
            ProjectProviderContext optionalContext = null)
            where TProject : NuGetProject
        {
            Assumes.Present(vsProjectAdapter);

            await _threadingService.JoinableTaskFactory.SwitchToMainThreadAsync();

            var provider = _providers
                .FirstOrDefault(p => typeof(TProject).TypeHandle.Equals(p.ProjectType));

            if (provider == null)
            {
                return null;
            }

            try
            {
                var nuGetProject = await provider.TryCreateNuGetProjectAsync(
                    vsProjectAdapter,
                    optionalContext,
                    forceProjectType: true);

                if (nuGetProject != null)
                {
                    return nuGetProject as TProject;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return null;
        }
    }
}
