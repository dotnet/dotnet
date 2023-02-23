// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Test.Common;

public abstract partial class TestBase
{
    private class TestErrorReporter : IErrorReporter
    {
        private readonly IRazorLogger _logger;

        public TestErrorReporter(IRazorLogger logger)
        {
            _logger = logger;
        }

        public void ReportError(Exception exception)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _logger.LogException(exception);
        }

        public void ReportError(Exception exception, IProjectSnapshot? project)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _logger.LogException(exception);
        }

        public void ReportError(Exception exception, Project workspaceProject)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _logger.LogException(exception);
        }
    }
}
