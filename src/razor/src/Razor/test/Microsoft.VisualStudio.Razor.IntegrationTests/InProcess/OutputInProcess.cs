// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Razor.IntegrationTests.Extensions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Microsoft.VisualStudio.Extensibility.Testing;

[TestService]
internal partial class OutputInProcess
{
    private const string RazorPaneName = "Razor Language Server Client";

    public async Task<bool> HasErrorsAsync(CancellationToken cancellationToken)
    {
        var content = await GetRazorOutputPaneContentAsync(cancellationToken);

        return content is null || content.Contains("Error");
    }

    /// <summary>
    /// This method returns the current content of the "Razor Language Server Client" output pane.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The contents of the RLSC output pane.</returns>
    public async Task<string?> GetRazorOutputPaneContentAsync(CancellationToken cancellationToken)
    {
        var outputPaneTextView = await GetOutputPaneTextViewAsync(RazorPaneName, cancellationToken);

        if (outputPaneTextView is null)
        {
            return null;
        }

        return await outputPaneTextView.GetContentAsync(JoinableTaskFactory, cancellationToken);
    }

    private async Task<IVsTextView?> GetOutputPaneTextViewAsync(string paneName, CancellationToken cancellationToken)
    {
        var sVSOutputWindow = await TestServices.Shell.GetRequiredGlobalServiceAsync<SVsOutputWindow, IVsOutputWindow>(cancellationToken);
        var extensibleObject = await TestServices.Shell.GetRequiredGlobalServiceAsync<SVsOutputWindow, IVsExtensibleObject>(cancellationToken);

        // The null propName gives use the OutputWindow object
        ErrorHandler.ThrowOnFailure(extensibleObject.GetAutomationObject(pszPropName: null, out var outputWindowObj));
        var outputWindow = (EnvDTE.OutputWindow)outputWindowObj;

        // This is a public entry point to COutputWindow::GetPaneByName
        EnvDTE.OutputWindowPane? pane = null;
        try
        {
            pane = outputWindow.OutputWindowPanes.Item(paneName);
        }
        catch (ArgumentException)
        {
            return null;
        }

        var textView = OutputWindowPaneToIVsTextView(pane, sVSOutputWindow);

        return textView;

        static IVsTextView OutputWindowPaneToIVsTextView(EnvDTE.OutputWindowPane outputWindowPane, IVsOutputWindow sVsOutputWindow)
        {
            var guid = Guid.Parse(outputWindowPane.Guid);
            ErrorHandler.ThrowOnFailure(sVsOutputWindow.GetPane(guid, out var result));

            if (result is not IVsTextView textView)
            {
                throw new InvalidOperationException($"{nameof(IVsOutputWindowPane)} should implement {nameof(IVsTextView)}");
            }

            return textView;
        }
    }
}
