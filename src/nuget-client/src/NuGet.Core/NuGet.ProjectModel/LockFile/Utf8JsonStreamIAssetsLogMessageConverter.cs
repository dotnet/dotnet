// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using NuGet.Common;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// A <see cref="IUtf8JsonStreamReaderConverter{T}"/> to allow read JSON into <see cref="IAssetsLogMessage"/>
    /// </summary>
    /// <example>
    /// {
    ///     "code": "<see cref="NuGetLogCode"/>",
    ///     "level": "<see cref="Common.LogLevel"/>",
    ///     "message": "test log message",
    ///     "warningLevel": <see cref="WarningLevel"/>,
    ///     "filePath": "C:\a\file\path.txt",
    ///     "startLineNumber": 1,
    ///     "startColumnNumber": 2,
    ///     "endLineNumber": 10,
    ///     "endcolumnNumber": 20,
    ///     "libraryId": "libraryId",
    ///     "targetGraphs": [
    ///         "targetGraph1"
    ///     ]
    /// }
    /// </example>
    internal class Utf8JsonStreamIAssetsLogMessageConverter : IUtf8JsonStreamReaderConverter<IAssetsLogMessage>
    {
        private static readonly byte[] LevelPropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.LEVEL);
        private static readonly byte[] CodePropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.CODE);
        private static readonly byte[] WarningLevelPropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.WARNING_LEVEL);
        private static readonly byte[] FilePathPropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.FILE_PATH);
        private static readonly byte[] StartLineNumberPropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.START_LINE_NUMBER);
        private static readonly byte[] StartColumnNumberPropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.START_COLUMN_NUMBER);
        private static readonly byte[] EndLineNumberPropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.END_LINE_NUMBER);
        private static readonly byte[] EndColumnNumberPropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.END_COLUMN_NUMBER);
        private static readonly byte[] MessagePropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.MESSAGE);
        private static readonly byte[] LibraryIdPropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.LIBRARY_ID);
        private static readonly byte[] TargetGraphsPropertyName = Encoding.UTF8.GetBytes(LogMessageProperties.TARGET_GRAPHS);

        public IAssetsLogMessage Read(ref Utf8JsonStreamReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject, found " + reader.TokenType);
            }

            var isValid = true;
            LogLevel level = default;
            NuGetLogCode code = default;
            //matching default warning level when AssetLogMessage object is created
            WarningLevel warningLevel = WarningLevel.Severe;
            string message = default;
            string filePath = default;
            int startLineNumber = default;
            int startColNumber = default;
            int endLineNumber = default;
            int endColNumber = default;
            string libraryId = default;
            IReadOnlyList<string> targetGraphs = null;

            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                if (!isValid)
                {
                    reader.Skip();
                }
                if (reader.ValueTextEquals(LevelPropertyName))
                {
                    var levelString = reader.ReadNextTokenAsString();
                    isValid &= Enum.TryParse(levelString, out level);
                }
                else if (reader.ValueTextEquals(CodePropertyName))
                {
                    var codeString = reader.ReadNextTokenAsString();
                    isValid &= Enum.TryParse(codeString, out code);
                }
                else if (reader.ValueTextEquals(WarningLevelPropertyName))
                {
                    reader.Read();
                    warningLevel = (WarningLevel)Enum.ToObject(typeof(WarningLevel), reader.GetInt32());
                }
                else if (reader.ValueTextEquals(FilePathPropertyName))
                {
                    filePath = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(StartLineNumberPropertyName))
                {
                    reader.Read();
                    startLineNumber = reader.GetInt32();
                }
                else if (reader.ValueTextEquals(StartColumnNumberPropertyName))
                {
                    reader.Read();
                    startColNumber = reader.GetInt32();
                }
                else if (reader.ValueTextEquals(EndLineNumberPropertyName))
                {
                    reader.Read();
                    endLineNumber = reader.GetInt32();
                }
                else if (reader.ValueTextEquals(EndColumnNumberPropertyName))
                {
                    reader.Read();
                    endColNumber = reader.GetInt32();
                }
                else if (reader.ValueTextEquals(MessagePropertyName))
                {
                    message = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(LibraryIdPropertyName))
                {
                    libraryId = reader.ReadNextTokenAsString();
                }
                else if (reader.ValueTextEquals(TargetGraphsPropertyName))
                {
                    reader.Read();
                    targetGraphs = (List<string>)reader.ReadStringArrayAsIList();
                }
                else
                {
                    reader.Skip();
                }
            }
            if (isValid)
            {
                var assetLogMessage = new AssetsLogMessage(level, code, message)
                {
                    TargetGraphs = targetGraphs ?? Array.Empty<string>(),
                    FilePath = filePath,
                    EndColumnNumber = endColNumber,
                    EndLineNumber = endLineNumber,
                    LibraryId = libraryId,
                    StartColumnNumber = startColNumber,
                    StartLineNumber = startLineNumber,
                    WarningLevel = warningLevel
                };
                return assetLogMessage;
            }
            return null;
        }
    }
}
