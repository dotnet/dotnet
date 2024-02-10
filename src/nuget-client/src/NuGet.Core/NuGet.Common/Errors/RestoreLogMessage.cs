// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Common
{
    public class RestoreLogMessage : IRestoreLogMessage
    {
        public LogLevel Level { get; set; }
        public NuGetLogCode Code { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Time { get; set; }
        public string? ProjectPath { get; set; }
        public WarningLevel WarningLevel { get; set; } = WarningLevel.Severe; //setting default to Severe as 0 implies show no warnings
        public string? FilePath { get; set; }
        public int StartLineNumber { get; set; }
        public int StartColumnNumber { get; set; }
        public int EndLineNumber { get; set; }
        public int EndColumnNumber { get; set; }
        public string? LibraryId { get; set; }
        public IReadOnlyList<string> TargetGraphs { get; set; }
        public bool ShouldDisplay { get; set; }


        public RestoreLogMessage(LogLevel logLevel, NuGetLogCode errorCode,
            string errorString, string? targetGraph, bool logToInnerLogger)
        {
            Level = logLevel;
            Code = errorCode;
            Message = errorString;
            Time = DateTimeOffset.UtcNow;

            var graphList = new List<string>();

            if (!string.IsNullOrEmpty(targetGraph))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                // .NET BCL is missing annotations on string.IsNullOrEmpty before .NET 3.0
                graphList.Add(targetGraph);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            TargetGraphs = graphList;
            ShouldDisplay = logToInnerLogger;
        }

        public RestoreLogMessage(LogLevel logLevel, NuGetLogCode errorCode,
            string errorString, string? targetGraph)
            : this(logLevel, errorCode, errorString, targetGraph, logToInnerLogger: false)
        {
        }

        public RestoreLogMessage(LogLevel logLevel, NuGetLogCode errorCode, string errorString)
            : this(logLevel, errorCode, errorString, string.Empty, logToInnerLogger: false)
        {
        }

        public RestoreLogMessage(LogLevel logLevel, string errorString)
            : this(logLevel, GetDefaultLogCode(logLevel), errorString, string.Empty, logToInnerLogger: false)
        {
        }

        /// <summary>
        /// Create a log message for a target graph library.
        /// </summary>
        public static RestoreLogMessage CreateWarning(
            NuGetLogCode code,
            string message,
            string? libraryId,
            params string[] targetGraphs)
        {
            return new RestoreLogMessage(LogLevel.Warning, message)
            {
                Code = code,
                LibraryId = libraryId,
                TargetGraphs = targetGraphs
            };
        }

        /// <summary>
        /// Create a warning log message.
        /// </summary>
        public static RestoreLogMessage CreateWarning(
            NuGetLogCode code,
            string message)
        {
            return new RestoreLogMessage(LogLevel.Warning, code, message);
        }

        /// <summary>
        /// Create an error log message.
        /// </summary>
        public static RestoreLogMessage CreateError(
            NuGetLogCode code,
            string message)
        {
            return new RestoreLogMessage(LogLevel.Error, code, message);
        }

        /// <summary>
        /// Create an error log message for a target graph.
        /// </summary>
        public static RestoreLogMessage CreateError(
            NuGetLogCode code,
            string message,
            string? libraryId,
            params string[] targetGraphs)
        {
            return new RestoreLogMessage(LogLevel.Error, message)
            {
                Code = code,
                LibraryId = libraryId,
                TargetGraphs = targetGraphs
            };
        }


        /// <summary>
        /// Get default LogCode based on the log level
        /// </summary>
        private static NuGetLogCode GetDefaultLogCode(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    return NuGetLogCode.NU1000;
                case LogLevel.Warning:
                    return NuGetLogCode.NU1500;
                default:
                    return NuGetLogCode.Undefined;
            }
        }
    }
}
