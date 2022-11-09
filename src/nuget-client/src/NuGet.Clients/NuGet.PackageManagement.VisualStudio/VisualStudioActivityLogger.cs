// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using NuGet.Common;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Logger routing messages into VS ActivityLog
    /// </summary>
    [Export, Export("VisualStudioActivityLogger", typeof(ILogger))]
    public sealed class VisualStudioActivityLogger : LegacyLoggerAdapter, ILogger
    {
        private const string LogEntrySource = "NuGet Package Manager";

        public override void LogDebug(string data) => ActivityLog.LogInformation(LogEntrySource, data);

        public override void LogError(string data) => ActivityLog.LogError(LogEntrySource, data);

        public override void LogInformation(string data) => ActivityLog.LogInformation(LogEntrySource, data);

        public override void LogMinimal(string data) => ActivityLog.LogInformation(LogEntrySource, data);

        public override void LogVerbose(string data) => ActivityLog.LogInformation(LogEntrySource, data);

        public override void LogWarning(string data) => ActivityLog.LogWarning(LogEntrySource, data);

        public override void LogInformationSummary(string data) => LogInformation(data);

    }
}
