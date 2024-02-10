// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Shared;

namespace NuGet.ProjectModel
{
    public class AssetsLogMessage : IAssetsLogMessage, IEquatable<IAssetsLogMessage>
    {

        public LogLevel Level { get; }
        public NuGetLogCode Code { get; }
        public string Message { get; }
        public string ProjectPath { get; set; }
        public WarningLevel WarningLevel { get; set; } = WarningLevel.Severe; //setting default to Severe as 0 implies show no warnings
        public string FilePath { get; set; }
        public string LibraryId { get; set; }
        public IReadOnlyList<string> TargetGraphs { get; set; }
        public int StartLineNumber { get; set; }
        public int StartColumnNumber { get; set; }
        public int EndLineNumber { get; set; }
        public int EndColumnNumber { get; set; }

        public static IAssetsLogMessage Create(IRestoreLogMessage logMessage)
        {
            return new AssetsLogMessage(logMessage.Level, logMessage.Code, logMessage.Message)
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

        public AssetsLogMessage(LogLevel logLevel, NuGetLogCode errorCode,
            string errorString, string targetGraph)
        {
            Level = logLevel;
            Code = errorCode;
            Message = errorString;

            if (!string.IsNullOrEmpty(targetGraph))
            {
                TargetGraphs = new List<string>
                {
                    targetGraph
                };
            }
        }

        public AssetsLogMessage(LogLevel logLevel, NuGetLogCode errorCode, string errorString)
            : this(logLevel, errorCode, errorString, string.Empty)
        {
        }

        public bool Equals(IAssetsLogMessage other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Level == other.Level &&
                Code == other.Code &&
                WarningLevel == other.WarningLevel &&
                StartLineNumber == other.StartLineNumber &&
                EndLineNumber == other.EndLineNumber &&
                StartColumnNumber == other.StartColumnNumber &&
                EndColumnNumber == other.EndColumnNumber &&
                StringComparer.Ordinal.Equals(Message, other.Message) &&
                StringComparer.Ordinal.Equals(ProjectPath, other.ProjectPath) &&
                StringComparer.Ordinal.Equals(FilePath, other.FilePath) &&
                StringComparer.Ordinal.Equals(LibraryId, other.LibraryId))
            {
                return TargetGraphs.SequenceEqualWithNullCheck(other.TargetGraphs);
            }

            return false;
        }

        public override bool Equals(object other)
        {
            return Equals(other as IAssetsLogMessage);
        }

        public override int GetHashCode()
        {
            var combiner = new HashCodeCombiner();

            combiner.AddStringIgnoreCase(Message);
            combiner.AddObject((int)Level);
            combiner.AddObject((int)Code);

            return combiner.CombinedHash;
        }
    }
}
