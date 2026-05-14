// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
