// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Packaging;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Comprises of the various client context such as logging, fileconflictaction
    /// </summary>
    public interface INuGetProjectContext
    {
        /// <summary>
        /// Logs a message for the given project context
        /// </summary>
        void Log(MessageLevel level, string message, params object[] args);

        /// <summary>
        /// Logs a message for the given project context
        /// </summary>
        void Log(ILogMessage message);

        /// <summary>
        /// Logs an error for the given project context
        /// </summary>
        void ReportError(string message);

        /// <summary>
        /// Logs an error or warning for the given project context
        /// </summary>
        void ReportError(ILogMessage message);

        /// <summary>
        /// Resolves a file conflict for the given project context
        /// </summary>
        FileConflictAction ResolveFileConflict(string message);

        PackageExtractionContext PackageExtractionContext { get; set; }

        ISourceControlManagerProvider SourceControlManagerProvider { get; }

        ExecutionContext ExecutionContext { get; }

        /// <summary>
        /// The original packages.config. This is set by package management
        /// before the actions are executed.
        /// </summary>
        XDocument OriginalPackagesConfig { get; set; }

        NuGetActionType ActionType { get; set; }

        Guid OperationId { get; set; }
    }
}
