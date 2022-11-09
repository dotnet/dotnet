// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.VisualStudio.SolutionExplorer.Models
{
    /// <summary>
    /// Models a diagnostic found in the assets file. Immutable.
    /// </summary>
    internal readonly struct AssetsFileLogMessage
    {
        public AssetsFileLogMessage(IAssetsLogMessage logMessage)
        {
            Code = logMessage.Code;
            Level = logMessage.Level;
            WarningLevel = logMessage.WarningLevel;
            Message = logMessage.Message;
            LibraryName = logMessage.LibraryId;
        }

        public NuGetLogCode Code { get; }
        public LogLevel Level { get; }
        public WarningLevel WarningLevel { get; }
        public string Message { get; }
        public string LibraryName { get; }

        public bool Equals(IAssetsLogMessage other)
        {
            return other.Code == Code
                && other.Level == Level
                && other.WarningLevel == WarningLevel
                && other.Message == Message
                && other.LibraryId == LibraryName;
        }

        public override string ToString() => $"{Level} {Code} ({LibraryName}) {Message}";
    }
}
