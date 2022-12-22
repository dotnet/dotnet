// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace NuGetConsole
{
    /// <summary>
    /// Wrapper console class enabling inline writing into the VS build output window pane.
    /// As the build output is not owned by NuGet but is rather shared with other providers
    /// this implementation doesn't allow certain invasive operations like Clear.
    /// </summary>
    internal sealed class BuildOutputConsole : SharedOutputConsole
    {
        private static Guid BuildWindowPaneGuid = VSConstants.BuildOutput;

        private readonly IVsOutputWindow _vsOutputWindow;
        private readonly AsyncLazy<IVsOutputWindowPane> _outputWindowPane;
        private readonly AsyncLazy<OutputWindowTextWriter> _outputWindowTextWriter;

        public BuildOutputConsole(IVsOutputWindow vsOutputWindow)
        {
            if (vsOutputWindow == null)
            {
                throw new ArgumentNullException(nameof(vsOutputWindow));
            }

            _vsOutputWindow = vsOutputWindow;

            _outputWindowPane = new AsyncLazy<IVsOutputWindowPane>(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                _vsOutputWindow.GetPane(ref BuildWindowPaneGuid, out var outputWindowPane);
                return outputWindowPane;
            },
            NuGetUIThreadHelper.JoinableTaskFactory);

            _outputWindowTextWriter = new AsyncLazy<OutputWindowTextWriter>(async () =>
            {
                var outputWindowPane = await _outputWindowPane.GetValueAsync();
                if (outputWindowPane != null)
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    return new OutputWindowTextWriter(outputWindowPane);
                }
                else
                {
                    return null;
                }
            },
            NuGetUIThreadHelper.JoinableTaskFactory);
        }

        public override async Task ActivateAsync()
        {
            var outputWindowPane = await _outputWindowPane.GetValueAsync();
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            outputWindowPane?.Activate();
        }

        public override Task ClearAsync()
        {
            // It's not our job to clear the build console.
            return Task.CompletedTask;
        }

        public override async Task WriteAsync(string text)
        {
            var outputWindowTextWriter = await _outputWindowTextWriter.GetValueAsync();
            await outputWindowTextWriter?.WriteAsync(text);
        }
    }
}
