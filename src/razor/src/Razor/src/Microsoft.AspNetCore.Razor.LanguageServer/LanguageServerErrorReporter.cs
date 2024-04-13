// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class LanguageServerErrorReporter : IErrorReporter
{
    private readonly ILogger _logger;

    public LanguageServerErrorReporter(ILoggerFactory loggerFactory)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _logger = loggerFactory.GetOrCreateLogger<LanguageServerErrorReporter>();
    }

    public void ReportError(Exception exception)
        => _logger.LogError(exception, $"Error thrown from LanguageServer");

    public void ReportError(Exception exception, IProjectSnapshot? project)
        => _logger.LogError(exception, $"Error thrown from project {project?.FilePath}");

    public void ReportError(Exception exception, Project workspaceProject)
        => _logger.LogError(exception, $"Error thrown from project {workspaceProject.FilePath}");
}
