// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents a log message that contains a file path to embed in an MSBuild binary log.
    /// </summary>
    public sealed class ConsoleOutLogEmbedInBinlog
        : ConsoleOutLogItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleOutLogEmbedInBinlog" /> class.
        /// </summary>
        public ConsoleOutLogEmbedInBinlog()
        {
            MessageType = ConsoleOutLogMessageType.EmbedInBinlog;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleOutLogEmbedInBinlog" /> class with the specified full path to the file to embed in the MSBuild binary log.
        /// </summary>
        /// <param name="path">The full path to the file to embed in the MSBuild binary log.</param>
        public ConsoleOutLogEmbedInBinlog(string path)
            : this()
        {
            Path = path;
        }

        /// <summary>
        /// Gets or sets the full path to the file to embed in the MSBuild binary log.
        /// </summary>
        public string Path { get; set; }

        public override string ToJson() => JsonConvert.SerializeObject(this, SerializerSettings);
    }
}
