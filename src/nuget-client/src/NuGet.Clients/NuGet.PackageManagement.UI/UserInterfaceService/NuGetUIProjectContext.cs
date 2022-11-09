// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.PackageManagement.VisualStudio;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.UI
{
    public sealed class NuGetUIProjectContext : INuGetProjectContext
    {
        private readonly INuGetUILogger _logger;

        public FileConflictAction FileConflictAction { get; set; }

        public NuGetUIProjectContext(
            ICommonOperations commonOperations,
            INuGetUILogger logger,
            ISourceControlManagerProvider sourceControlManagerProvider)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (sourceControlManagerProvider == null)
            {
                throw new ArgumentNullException(nameof(sourceControlManagerProvider));
            }

            _logger = logger;
            SourceControlManagerProvider = sourceControlManagerProvider;

            if (commonOperations != null)
            {
                ExecutionContext = new IDEExecutionContext(commonOperations);
            }
        }

        public void Log(MessageLevel level, string message, params object[] args)
        {
            _logger.Log(level, message, args);
        }

        public FileConflictAction ShowFileConflictResolution(string message)
        {
            return NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var fileConflictDialog = new FileConflictDialog
                {
                    Question = message
                };

                if (fileConflictDialog.ShowModal() == true)
                {
                    return fileConflictDialog.UserSelection;
                }

                return FileConflictAction.IgnoreAll;
            });
        }

        public FileConflictAction ResolveFileConflict(string message)
        {
            if (FileConflictAction == FileConflictAction.PromptUser)
            {
                var resolution = ShowFileConflictResolution(message);

                if (resolution == FileConflictAction.IgnoreAll
                    ||
                    resolution == FileConflictAction.OverwriteAll)
                {
                    FileConflictAction = resolution;
                }
                return resolution;
            }

            return FileConflictAction;
        }

        public PackageExtractionContext PackageExtractionContext { get; set; }

        public ISourceControlManagerProvider SourceControlManagerProvider { get; }

        public ExecutionContext ExecutionContext { get; }

        public XDocument OriginalPackagesConfig { get; set; }

        public void ReportError(string message)
        {
            _logger.ReportError(new LogMessage(LogLevel.Error, message));
        }

        public void Log(ILogMessage message)
        {
            _logger.Log(message);
        }

        public void ReportError(ILogMessage message)
        {
            _logger.ReportError(message);
        }

        public NuGetActionType ActionType { get; set; }

        public Guid OperationId { get; set; }
    }
}
