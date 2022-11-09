// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace NuGet.VisualStudio.Common
{
    /// <summary> Wrapper around DTE functions. </summary>
    internal class VisualStudioShell : IVisualStudioShell
    {
        private AsyncLazy<EnvDTE.DTE> _dte;

        // Keeps a reference to BuildEvents so that our event handler won't get disconnected because of GC.
        private EnvDTE.BuildEvents _buildEvents;
        private EnvDTE.SolutionEvents _solutionEvents;

        /// <summary> Creates a new instance of <see cref="VisualStudioShell"/>. </summary>
        /// <param name="asyncServiceProvider"> Async service provider. </param>
        internal VisualStudioShell(IAsyncServiceProvider asyncServiceProvider)
        {
            Verify.ArgumentIsNotNull(asyncServiceProvider, nameof(asyncServiceProvider));
            _dte = new AsyncLazy<EnvDTE.DTE>(() => asyncServiceProvider.GetDTEAsync(), NuGetUIThreadHelper.JoinableTaskFactory);
        }

        async Task IVisualStudioShell.SubscribeToBuildBeginAsync(Action onBuildBegin)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();
            _buildEvents = dte.Events.BuildEvents;
            _buildEvents.OnBuildBegin += (scope, action) => onBuildBegin();
        }

        async Task IVisualStudioShell.SubscribeToAfterClosingAsync(Action afterClosing)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = await _dte.GetValueAsync();
            _solutionEvents = dte.Events.SolutionEvents;
            _solutionEvents.AfterClosing += () => afterClosing();
        }

        async Task<object> IVisualStudioShell.GetPropertyValueAsync(string category, string page, string propertyName)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = await _dte.GetValueAsync();
            var properties = dte.get_Properties(category, page);
            return properties.Item(propertyName).Value;
        }
    }
}
