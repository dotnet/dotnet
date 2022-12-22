// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using System.Threading.Tasks;

namespace NuGet.Common
{
    /// <summary>
    /// A generic interface for logging.
    /// </summary>
    public interface ILogger
    {
        void LogDebug(string data);

        void LogVerbose(string data);

        void LogInformation(string data);

        void LogMinimal(string data);

        void LogWarning(string data);

        void LogError(string data);

        void LogInformationSummary(string data);

        void Log(LogLevel level, string data);

        Task LogAsync(LogLevel level, string data);

        void Log(ILogMessage message);

        Task LogAsync(ILogMessage message);
    }
}
