// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.Packaging.Signing
{
    /// <summary>
    /// Log message for signature verification.
    /// </summary>
    public class SignatureLog : ILogMessage, IEquatable<SignatureLog>
    {
        public LogLevel Level { get; set; }

        public string Message { get; set; }

        public NuGetLogCode Code { get; set; }

        public WarningLevel WarningLevel { get; set; } = WarningLevel.Severe; //setting default to Severe as 0 implies show no warnings

        public string ProjectPath { get; set; }

        public DateTimeOffset Time { get; set; }

        public string LibraryId { get; set; }

        private SignatureLog(LogLevel level, NuGetLogCode code, string message)
        {
            Level = level;
            Code = code;
            Message = message;
            Time = DateTimeOffset.UtcNow;
        }

        public static SignatureLog MinimalLog(string message)
        {
            // create a log message and make the code undefined to not display the code in any logger
            return new SignatureLog(LogLevel.Minimal, NuGetLogCode.Undefined, message);
        }

        public static SignatureLog InformationLog(string message)
        {
            // create a log message and make the code undefined to not display the code in any logger
            return new SignatureLog(LogLevel.Information, NuGetLogCode.Undefined, message);
        }

        public static SignatureLog DetailedLog(string message)
        {
            // create a log message and make the code undefined to not display the code in any logger
            return new SignatureLog(LogLevel.Verbose, NuGetLogCode.Undefined, message);
        }

        public static SignatureLog DebugLog(string message)
        {
            // create a log message and make the code undefined to not display the code in any logger
            return new SignatureLog(LogLevel.Debug, NuGetLogCode.Undefined, message);
        }

        public static SignatureLog Issue(bool fatal, NuGetLogCode code, string message)
        {
            // A fatal issue should be an error, otherwise just a warning
            var level = fatal ? LogLevel.Error : LogLevel.Warning;

            return new SignatureLog(level, code, message);
        }

        public static SignatureLog Error(NuGetLogCode code, string message)
        {
            return new SignatureLog(LogLevel.Error, code, message);
        }

        public bool Equals(SignatureLog other)
        {
            if (other == null)
            {
                return false;
            }
            else if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Level, other.Level) &&
                Equals(Code, other.Code) &&
                EqualityUtility.EqualsWithNullCheck(LibraryId, other.LibraryId) &&
                EqualityUtility.EqualsWithNullCheck(ProjectPath, other.ProjectPath) &&
                EqualityUtility.EqualsWithNullCheck(Message, other.Message);
        }

        /// <summary>
        /// Converts an SignatureLog into a Restore
        /// This is needed when an SignatureLog needs to be logged and loggers do not have visibility to SignatureLog.
        /// </summary>
        /// <returns>RestoreLogMessage equivalent to the SignatureLog.</returns>
        public RestoreLogMessage AsRestoreLogMessage()
        {
            return new RestoreLogMessage(Level, Code, Message)
            {
                ProjectPath = ProjectPath,
                WarningLevel = WarningLevel,
                LibraryId = LibraryId
            };
        }
    }
}
