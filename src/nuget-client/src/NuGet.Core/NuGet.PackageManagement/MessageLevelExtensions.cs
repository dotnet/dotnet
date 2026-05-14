// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Common;

namespace NuGet.ProjectManagement
{
    /// <summary> Extension methods for <see cref="MessageLevel"/>. </summary>
    public static class MessageLevelExtensions
    {
        /// <summary> Convert <see cref="MessageLevel"/> to <see cref="LogLevel"/>. </summary>
        /// <param name="messageLevel"> Message level. </param>
        /// <returns> Corresponding log level. </returns>
        public static LogLevel ToLogLevel(this MessageLevel messageLevel)
        {
            switch (messageLevel)
            {
                case MessageLevel.Error: return LogLevel.Error;
                case MessageLevel.Warning: return LogLevel.Warning;
                case MessageLevel.Info: return LogLevel.Information;
                case MessageLevel.Debug: return LogLevel.Debug;
                default: return LogLevel.Minimal;
            }
        }
    }
}
