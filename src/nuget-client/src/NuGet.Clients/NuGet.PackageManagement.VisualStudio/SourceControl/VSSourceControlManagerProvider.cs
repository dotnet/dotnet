// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(ISourceControlManagerProvider))]
    internal sealed class VsSourceControlManagerProvider : ISourceControlManagerProvider
    {
        private const string TfsProviderName = "{4CA58AB2-18FA-4F8D-95D4-32DDF27D184C}";
        private readonly IAsyncServiceProvider _asyncServiceProvider;
        private readonly Configuration.ISettings _settings;
        private readonly AsyncLazy<EnvDTE.DTE> _dte;

        [ImportingConstructor]
        public VsSourceControlManagerProvider(
            [Import(typeof(SVsServiceProvider))]
            IAsyncServiceProvider asyncServiceProvider,
            Configuration.ISettings vsSettings)
        {
            Assumes.NotNull(asyncServiceProvider);
            _asyncServiceProvider = asyncServiceProvider;
            Assumes.Present(vsSettings);

            _settings = vsSettings;
            _dte = new AsyncLazy<EnvDTE.DTE>(async () =>
            {
                return await asyncServiceProvider.GetDTEAsync();
            }, NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public SourceControlManager GetSourceControlManager()
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                EnvDTE.DTE dte = await _dte.GetValueAsync();

                if (dte != null)
                {
                    var sourceControl = (SourceControl2)dte.SourceControl;
                    if (sourceControl != null)
                    {
                        SourceControlBindings sourceControlBindings = null;
                        try
                        {
                            // Get the binding for this solution
                            sourceControlBindings = sourceControl.GetBindings(dte.Solution.FullName);
                        }
                        catch (NotImplementedException)
                        {
                            // Some source control providers don't bother to implement this.
                            // TFS might be the only one using it
                        }

                        if (sourceControlBindings == null
                            || string.IsNullOrEmpty(sourceControlBindings.ProviderName)
                            ||
                            !sourceControlBindings.ProviderName.Equals(TfsProviderName, StringComparison.OrdinalIgnoreCase))
                        {
                            // Return null, if the Source control provider is not TFS
                            return null;
                        }

                        return new DefaultTFSSourceControlManager(_settings, sourceControlBindings);
                    }
                }

                return null;
            });
        }
    }
}
