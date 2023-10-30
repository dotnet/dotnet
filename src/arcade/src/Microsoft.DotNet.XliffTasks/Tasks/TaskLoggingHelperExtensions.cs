// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Build.Utilities;

namespace XliffTasks.Tasks
{
    internal static class TaskLoggingHelperExtensions
    {
        /// <summary>
        /// Helper method to log MSBuild errors associated with a particular file.
        /// </summary>
        public static void LogErrorInFile(this TaskLoggingHelper log, string file, string message)
        {
            log.LogError(
                subcategory: null,
                errorCode: null,
                helpKeyword: null,
                file: file,
                lineNumber: 0,
                columnNumber: 0,
                endLineNumber: 0,
                endColumnNumber: 0,
                message: message);
        }

        /// <summary>
        /// Helper method to log MSBuild errors associated with a particular file and line.
        /// </summary>
        public static void LogErrorInFile(this TaskLoggingHelper log, string file, int line, string message)
        {
            log.LogError(
                subcategory: null,
                errorCode: null,
                helpKeyword: null,
                file: file,
                lineNumber: line,
                columnNumber: 0,
                endLineNumber: 0,
                endColumnNumber: 0,
                message: message);
        }
    }
}
