// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;

namespace NuGet.Commands
{
    /// <summary>
    /// Internal extension helpers for NuGet.Commands
    /// </summary>
    internal static class Extensions
    {
        public static ISet<LibraryDependency> GetAllPackageDependencies(this PackageSpec project)
        {
            // Remove non-package dependencies such as framework assembly references.
            return new HashSet<LibraryDependency>(
                project.Dependencies.Concat(project.TargetFrameworks.SelectMany(e => e.Dependencies))
                                    .Where(e => e.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package)));
        }

        public static ISet<LibraryDependency> GetPackageDependenciesForFramework(this PackageSpec project, NuGetFramework framework)
        {
            // Remove non-package dependencies such as framework assembly references.
            return new HashSet<LibraryDependency>(
                project.Dependencies.Concat(project.GetTargetFramework(framework).Dependencies)
                                    .Where(e => e.LibraryRange.TypeConstraintAllows(LibraryDependencyTarget.Package)));
        }

        /// <summary>
        /// Log all messages.
        /// </summary>
        public static void LogMessages(this ILogger logger, IEnumerable<ILogMessage> messages)
        {
            foreach (var message in messages)
            {
                logger.Log(message);
            }
        }

        /// <summary>
        /// Log all messages.
        /// </summary>
        public static Task LogMessagesAsync(this ILogger logger, params ILogMessage[] messages)
        {
            return logger.LogMessagesAsync(messages);
        }

        /// <summary>
        /// Log all messages.
        /// </summary>
        public static async Task LogMessagesAsync(this ILogger logger, IEnumerable<ILogMessage> messages)
        {
            foreach (var message in messages)
            {
                await logger.LogAsync(message);
            }
        }

        /// <summary>
        /// Converts an IAssetsLogMessage into a RestoreLogMessage.
        /// This is needed when an IAssetsLogMessage needs to be logged and loggers do not have visibility to IAssetsLogMessage.
        /// </summary>
        /// <param name="logMessage">IAssetsLogMessage to be converted.</param>
        /// <returns>RestoreLogMessage equivalent to the IAssetsLogMessage.</returns>
        public static RestoreLogMessage AsRestoreLogMessage(this IAssetsLogMessage logMessage)
        {
            return new RestoreLogMessage(logMessage.Level, logMessage.Code, logMessage.Message)
            {
                ProjectPath = logMessage.ProjectPath,
                WarningLevel = logMessage.WarningLevel,
                FilePath = logMessage.FilePath,
                LibraryId = logMessage.LibraryId,
                TargetGraphs = logMessage.TargetGraphs,
                StartLineNumber = logMessage.StartLineNumber,
                StartColumnNumber = logMessage.StartColumnNumber,
                EndLineNumber = logMessage.EndLineNumber,
                EndColumnNumber = logMessage.EndColumnNumber
            };
        }

        /// <summary>
        /// Converts an LogMessage into a RestoreLogMessage.
        /// This is needed when an LogMessage needs to be logged and loggers do not have visibility to LogMessage.
        /// </summary>
        /// <param name="logMessage">LogMessage to be converted.</param>
        /// <returns>RestoreLogMessage equivalent to the LogMessage.</returns>
        public static RestoreLogMessage AsRestoreLogMessage(this LogMessage logMessage)
        {
            return new RestoreLogMessage(logMessage.Level, logMessage.Code, logMessage.Message)
            {
                ProjectPath = logMessage.ProjectPath,
                WarningLevel = logMessage.WarningLevel
            };
        }
    }
}
