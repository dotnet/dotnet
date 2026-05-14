// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request to monitor a NuGet process exit.
    /// </summary>
    public sealed class MonitorNuGetProcessExitRequest
    {
        /// <summary>
        /// Gets the process ID.
        /// </summary>
        [JsonRequired]
        public int ProcessId { get; }

        /// <summary>
        /// Initializes a new <see cref="MonitorNuGetProcessExitRequest" /> class.
        /// </summary>
        /// <param name="processId">The process ID.</param>
        [JsonConstructor]
        public MonitorNuGetProcessExitRequest(int processId)
        {
            ProcessId = processId;
        }
    }
}
