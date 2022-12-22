// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class LanguageServerErrorReporter : ErrorReporter
{
    private readonly ILogger _logger;

    public LanguageServerErrorReporter(ILoggerFactory loggerFactory)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _logger = loggerFactory.CreateLogger<LanguageServerErrorReporter>();
    }

    public override void ReportError(Exception exception)
    {
        _logger.LogError(exception, "Error thrown from LanguageServer");
    }

    public override void ReportError(Exception exception, ProjectSnapshot? project)
    {
        _logger.LogError(exception, "Error thrown from project {projectFilePath}", project?.FilePath);
    }

    public override void ReportError(Exception exception, Project workspaceProject)
    {
        _logger.LogError(exception, "Error thrown from project {workspaceProjectFilePath}", workspaceProject.FilePath);
    }
}
