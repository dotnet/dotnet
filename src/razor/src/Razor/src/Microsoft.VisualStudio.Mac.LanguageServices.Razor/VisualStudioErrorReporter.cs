// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using MonoDevelop.Core;

namespace Microsoft.VisualStudio.Mac.LanguageServices.Razor;

[Export(typeof(ErrorReporter))]
internal class VisualStudioErrorReporter : ErrorReporter
{
    public override void ReportError(Exception exception)
    {
        if (exception is null)
        {
            Debug.Fail("Null exceptions should not be reported.");
            return;
        }

        LoggingService.LogError(
            SR.RazorLanguageServiceGeneralError,
            exception);
    }

    public override void ReportError(Exception exception, Project project)
    {
        if (exception is null)
        {
            Debug.Fail("Null exceptions should not be reported.");
            return;
        }

        LoggingService.LogError(
            SR.FormatRazorLanguageServiceProjectError(project?.Name),
            exception);
    }

    public override void ReportError(Exception exception, ProjectSnapshot? project)
    {
        if (exception is null)
        {
            Debug.Fail("Null exceptions should not be reported.");
            return;
        }

        LoggingService.LogError(
            SR.FormatRazorLanguageServiceProjectSnapshotError(project?.FilePath, exception),
            exception);
    }
}
