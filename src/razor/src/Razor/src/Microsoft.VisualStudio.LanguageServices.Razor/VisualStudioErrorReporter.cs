// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.VisualStudio.LanguageServices.Razor;

[Export(typeof(IErrorReporter))]
[method: ImportingConstructor]
internal class VisualStudioErrorReporter(SVsServiceProvider serviceProvider) : IErrorReporter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void ReportError(Exception exception)
    {
        if (exception is null)
        {
            return;
        }

        var activityLog = GetActivityLog();
        if (activityLog != null)
        {
            var hr = activityLog.LogEntry(
                (uint)__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR,
                "Razor Language Services",
                $"Error encountered:{Environment.NewLine}{exception}");
            ErrorHandler.ThrowOnFailure(hr);
        }
    }

    public void ReportError(Exception exception, IProjectSnapshot? project)
    {
        var activityLog = GetActivityLog();
        if (activityLog is not null)
        {
            var hr = activityLog.LogEntry(
                (uint)__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR,
                "Razor Language Services",
                $"Error encountered from project '{project?.FilePath}':{Environment.NewLine}{exception}");
            ErrorHandler.ThrowOnFailure(hr);
        }
    }

    public void ReportError(Exception exception, Project workspaceProject)
    {
        var activityLog = GetActivityLog();
        if (activityLog is not null)
        {
            var hr = activityLog.LogEntry(
                (uint)__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR,
                "Razor Language Services",
                $"Error encountered from project '{workspaceProject?.Name}' '{workspaceProject?.FilePath}':{Environment.NewLine}{exception}");
            ErrorHandler.ThrowOnFailure(hr);
        }
    }

    private IVsActivityLog? GetActivityLog()
    {
        return _serviceProvider.GetService(typeof(SVsActivityLog)) as IVsActivityLog;
    }
}
