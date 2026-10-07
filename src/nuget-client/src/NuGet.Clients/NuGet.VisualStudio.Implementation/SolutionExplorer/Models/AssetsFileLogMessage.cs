// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using NuGet.Common;
using NuGet.ProjectModel;

namespace NuGet.VisualStudio.SolutionExplorer.Models
{
    /// <summary>
    /// Models a diagnostic found in the assets file. Immutable.
    /// </summary>
    internal readonly struct AssetsFileLogMessage
    {
        public AssetsFileLogMessage(string projectFilePath, IAssetsLogMessage logMessage)
        {
            string? libraryId = logMessage.LibraryId;
            if (libraryId == null)
            {
                throw new ArgumentException("The log message must identify a library.", nameof(logMessage));
            }

            Code = logMessage.Code;
            Level = logMessage.Level;
            WarningLevel = logMessage.WarningLevel;
            Message = logMessage.Message;
            LibraryName = NormalizeLibraryName(libraryId, projectFilePath);
        }

        public NuGetLogCode Code { get; }
        public LogLevel Level { get; }
        public WarningLevel WarningLevel { get; }
        public string Message { get; }
        public string LibraryName { get; }

        public bool Equals(IAssetsLogMessage other, string projectFilePath)
        {
            string? libraryId = other.LibraryId;

            return libraryId != null
                && other.Code == Code
                && other.Level == Level
                && other.WarningLevel == WarningLevel
                && other.Message == Message
                && NormalizeLibraryName(libraryId, projectFilePath) == LibraryName;
        }

        private static string NormalizeLibraryName(string libraryName, string projectFilePath)
        {
            // If we have a rooted path for the library in the messages, it is an unresolved project reference.
            // Other identifiers for this item will be a relative path with respect to the project.
            // So, we compute that relative path here such that it will match correctly. This enables us to
            // display diagnostic items beneath unresolved project references in Solution Explorer.
            if (Path.IsPathRooted(libraryName))
            {
                return PathUtility.GetRelativePath(projectFilePath, libraryName);
            }

            return libraryName;
        }

        public override string ToString() => $"{Level} {Code} ({LibraryName}) {Message}";
    }
}
