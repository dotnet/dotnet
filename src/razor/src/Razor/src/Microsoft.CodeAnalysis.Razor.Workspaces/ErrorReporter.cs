// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

internal sealed class ErrorReporter : IErrorReporter
{
    public static readonly ErrorReporter Instance = new();

    private ErrorReporter()
    {
    }

    public void ReportError(Exception exception)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        // Do nothing.
    }

    public void ReportError(Exception exception, IProjectSnapshot? project)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        // Do nothing.
    }

    public void ReportError(Exception exception, Project workspaceProject)
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        // Do nothing.
    }
}
