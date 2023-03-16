// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents a log message, warning, or error that can be serialized and sent across a text stream like <see cref="Console.Out" />.
    /// </summary>
    public sealed class ConsoleOutLogMessage : ConsoleOutLogItem
    {
        /// <inheritdoc cref="BuildMessageEventArgs.Code" />
        public string Code { get; set; }

        /// <inheritdoc cref="BuildMessageEventArgs.ColumnNumber" />
        public int ColumnNumber { get; set; }

        /// <inheritdoc cref="BuildMessageEventArgs.EndColumnNumber" />
        public int EndColumnNumber { get; set; }

        /// <inheritdoc cref="BuildMessageEventArgs.EndLineNumber" />
        public int EndLineNumber { get; set; }

        /// <inheritdoc cref="BuildMessageEventArgs.File" />
        public string File { get; set; }

        /// <inheritdoc cref="BuildEventArgs.HelpKeyword" />
        public string HelpKeyword { get; set; }

        /// <inheritdoc cref="BuildMessageEventArgs.Importance" />
        public MessageImportance Importance { get; set; }

        /// <inheritdoc cref="BuildMessageEventArgs.LineNumber" />
        public int LineNumber { get; set; }

        /// <inheritdoc cref="BuildEventArgs.Message" />
        public string Message { get; set; }

        /// <inheritdoc cref="BuildMessageEventArgs.ProjectFile" />
        public string ProjectFile { get; set; }

        /// <inheritdoc cref="BuildEventArgs.SenderName" />
        public string SenderName { get; set; }

        /// <inheritdoc cref="BuildMessageEventArgs.Subcategory" />
        public string Subcategory { get; set; }

        public override string ToJson() => JsonConvert.SerializeObject(this, SerializerSettings);

        /// <summary>
        /// Represents the message importance.  This is a copy of <see cref="MessageImportance" /> because in some code paths we need to log a message before MSBuild assemblies have been loaded.
        /// </summary>
        public enum MessageImportance
        {
            High,
            Normal,
            Low
        }
    }
}
