// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Packaging;

namespace NuGet.ProjectManagement
{
    public class EmptyNuGetProjectContext : INuGetProjectContext
    {
        public void Log(MessageLevel level, string message, params object[] args)
        {
            // No-op
        }

        public FileConflictAction ResolveFileConflict(string message)
        {
            return FileConflictAction.IgnoreAll;
        }

        public PackageExtractionContext PackageExtractionContext { get; set; }

        public ISourceControlManagerProvider SourceControlManagerProvider => null;

        public ExecutionContext ExecutionContext => null;

        public XDocument OriginalPackagesConfig { get; set; }

        public void ReportError(string message)
        {
            // No-op
        }

        public void Log(ILogMessage message)
        {
            // No-op
        }

        public void ReportError(ILogMessage message)
        {
            // No-op
        }

        public NuGetActionType ActionType { get; set; }

        public Guid OperationId { get; set; }
    }
}
