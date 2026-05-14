// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Build.Framework;
using Newtonsoft.Json;

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents a base class for defining log items sent over <see cref="Console.Out" />.
    /// </summary>
    public abstract class ConsoleOutLogItem
    {
        /// <summary>
        /// Serialization settings for messages.  These should be set to be as fast as possible and not for friendly
        /// display since no user will ever see them.
        /// </summary>
        protected static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
        };

        /// <summary>
        /// Gets or sets the <see cref="ConsoleOutLogMessageType" /> of the message.
        /// </summary>
        public ConsoleOutLogMessageType MessageType { get; set; }

        /// <summary>
        /// Serializes the current object to JSON.
        /// </summary>
        /// <returns>The current object as a JSON string.</returns>
        public abstract string ToJson();

        /// <summary>
        /// Implicitly converts a <see cref="BuildMessageEventArgs" /> object to a <see cref="ConsoleOutLogItem" /> object.
        /// </summary>
        /// <param name="buildMessageEventArgs">The <see cref="BuildMessageEventArgs" /> object to convert.</param>
        public static implicit operator ConsoleOutLogItem(BuildMessageEventArgs buildMessageEventArgs)
        {
            return new ConsoleOutLogMessage
            {
                Importance = (ConsoleOutLogMessage.MessageImportance)buildMessageEventArgs.Importance,
                Message = buildMessageEventArgs.Message,
                MessageType = ConsoleOutLogMessageType.Message,
            };
        }

        /// <summary>
        /// Implicitly converts a <see cref="BuildWarningEventArgs" /> object to a <see cref="ConsoleOutLogItem" /> object.
        /// </summary>
        /// <param name="buildWarningEventArgs">The <see cref="BuildWarningEventArgs" /> object to convert.</param>
        public static implicit operator ConsoleOutLogItem(BuildWarningEventArgs buildWarningEventArgs)
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
        /// Implicitly converts a <see cref="BuildErrorEventArgs" /> object to a <see cref="ConsoleOutLogItem" /> object.
        /// </summary>
        /// <param name="buildErrorEventArgs">The <see cref="BuildErrorEventArgs" /> object to convert.</param>
        public static implicit operator ConsoleOutLogItem(BuildErrorEventArgs buildErrorEventArgs)
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
