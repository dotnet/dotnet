// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common;

public abstract partial class ToolingTestBase
{
    private class TestErrorReporter : IErrorReporter
    {
        private readonly ILogger _logger;

        public TestErrorReporter(ILogger logger)
        {
            _logger = logger;
        }

        public void ReportError(Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _logger.LogError(exception, message: $"[null]");
        }

        public void ReportError(Exception exception, IProjectSnapshot? project)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _logger.LogError(exception, message: $"[null]");
        }

        public void ReportError(Exception exception, Project workspaceProject)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _logger.LogError(exception, message: $"[null]");
        }
    }
}
