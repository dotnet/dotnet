// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;

namespace NuGet.PackageManagement
{
    public static class LogUtility
    {
        public static MessageLevel LogLevelToMessageLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    return MessageLevel.Error;

                case LogLevel.Warning:
                    return MessageLevel.Warning;

                case LogLevel.Information:
                case LogLevel.Minimal:
                    return MessageLevel.Info;

                default:
                    return MessageLevel.Debug;
            }
        }

        /// <summary>
        /// Converts an IAssetsLogMessage into a RestoreLogMessage.
        /// This is needed when an IAssetsLogMessage needs to be logged and loggers do not have visibility to IAssetsLogMessage.
        /// </summary>
        /// <param name="logMessage">IAssetsLogMessage to be converted.</param>
        /// <returns>RestoreLogMessage equivalent to the IAssetsLogMessage.</returns>
        internal static RestoreLogMessage AsRestoreLogMessage(this IAssetsLogMessage logMessage)
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
    }
}
