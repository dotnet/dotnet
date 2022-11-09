// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Build.Framework;
using Newtonsoft.Json;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents a log message that can be serialized and sent across a text stream like <see cref="Console.Out" />.
    /// </summary>
    public sealed class ConsoleOutLogMessage
    {
        /// <summary>
        /// Serialization settings for messages.  These should be set to be as fast as possible and not for friendly
        /// display since no user will ever see them.
        /// </summary>
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
        };

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

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        public ConsoleOutLogMessageType MessageType { get; set; }

        /// <inheritdoc cref="BuildMessageEventArgs.ProjectFile" />
        public string ProjectFile { get; set; }

        /// <inheritdoc cref="BuildEventArgs.SenderName" />
        public string SenderName { get; set; }

        /// <inheritdoc cref="BuildMessageEventArgs.Subcategory" />
        public string Subcategory { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, SerializerSettings);
        }

        /// <summary>
        /// Implicitly converts a <see cref="BuildMessageEventArgs" /> object to a <see cref="ConsoleOutLogMessage" /> object.
        /// </summary>
        /// <param name="buildMessageEventArgs">The <see cref="BuildMessageEventArgs" /> object to convert.</param>
        public static implicit operator ConsoleOutLogMessage(BuildMessageEventArgs buildMessageEventArgs)
        {
            return new ConsoleOutLogMessage
            {
                Importance = buildMessageEventArgs.Importance,
                Message = buildMessageEventArgs.Message,
                MessageType = ConsoleOutLogMessageType.Message,
            };
        }

        /// <summary>
        /// Implicitly converts a <see cref="BuildWarningEventArgs" /> object to a <see cref="ConsoleOutLogMessage" /> object.
        /// </summary>
        /// <param name="buildWarningEventArgs">The <see cref="BuildWarningEventArgs" /> object to convert.</param>
        public static implicit operator ConsoleOutLogMessage(BuildWarningEventArgs buildWarningEventArgs)
        {
            return new ConsoleOutLogMessage
            {
                Code = buildWarningEventArgs.Code,
                ColumnNumber = buildWarningEventArgs.ColumnNumber,
                EndColumnNumber = buildWarningEventArgs.EndColumnNumber,
                EndLineNumber = buildWarningEventArgs.EndLineNumber,
                File = buildWarningEventArgs.File,
                HelpKeyword = buildWarningEventArgs.HelpKeyword,
                LineNumber = buildWarningEventArgs.LineNumber,
                Message = buildWarningEventArgs.Message,
                MessageType = ConsoleOutLogMessageType.Warning,
                ProjectFile = buildWarningEventArgs.ProjectFile,
                SenderName = buildWarningEventArgs.SenderName,
                Subcategory = buildWarningEventArgs.Subcategory
            };
        }

        /// <summary>
        /// Implicitly converts a <see cref="BuildErrorEventArgs" /> object to a <see cref="ConsoleOutLogMessage" /> object.
        /// </summary>
        /// <param name="buildErrorEventArgs">The <see cref="BuildErrorEventArgs" /> object to convert.</param>
        public static implicit operator ConsoleOutLogMessage(BuildErrorEventArgs buildErrorEventArgs)
        {
            return new ConsoleOutLogMessage
            {
                Code = buildErrorEventArgs.Code,
                ColumnNumber = buildErrorEventArgs.ColumnNumber,
                EndColumnNumber = buildErrorEventArgs.EndColumnNumber,
                EndLineNumber = buildErrorEventArgs.EndLineNumber,
                File = buildErrorEventArgs.File,
                HelpKeyword = buildErrorEventArgs.HelpKeyword,
                LineNumber = buildErrorEventArgs.LineNumber,
                Message = buildErrorEventArgs.Message,
                MessageType = ConsoleOutLogMessageType.Error,
                ProjectFile = buildErrorEventArgs.ProjectFile,
                SenderName = buildErrorEventArgs.SenderName,
                Subcategory = buildErrorEventArgs.Subcategory,
            };
        }
    }
}
