// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NuGet.ProjectModel
{
    public class FileFormatException : Exception
    {
        public FileFormatException(string message)
            : base(message)
        {
        }

        public FileFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public string Path { get; private set; }
        public int Line { get; private set; }
        public int Column { get; private set; }

        private FileFormatException WithFilePath(string path)
        {
            Path = path;

            return this;
        }

        private FileFormatException WithLineInfo(JsonReaderException exception)
        {
            Line = exception.LinePosition;
            Column = exception.LineNumber;

            return this;
        }

        private FileFormatException WithLineInfo(int line, int column)
        {
            Line = line;
            Column = column;

            return this;
        }

        private FileFormatException WithLineInfo(IJsonLineInfo lineInfo)
        {
            Line = lineInfo.LineNumber;
            Column = lineInfo.LinePosition;

            return this;
        }

        public static FileFormatException Create(Exception exception, JToken value, string path)
        {
            var lineInfo = (IJsonLineInfo)value;

            var message = string.Format(CultureInfo.CurrentCulture,
                Strings.Log_ErrorReadingProjectJsonWithLocation,
                path,
                lineInfo.LineNumber,
                lineInfo.LinePosition,
                exception.Message);

            var ex = new FileFormatException(message, exception);

            return ex.WithFilePath(path).WithLineInfo(lineInfo);
        }

        internal static FileFormatException Create(Exception exception, int line, int column, string path)
        {
            var message = string.Format(CultureInfo.CurrentCulture,
                Strings.Log_ErrorReadingProjectJsonWithLocation,
                path,
                line,
                column,
                exception.Message);

            var ex = new FileFormatException(message, exception);

            return ex.WithFilePath(path).WithLineInfo(line, column);
        }

        internal static FileFormatException Create(Exception exception, string path)
        {
            var message = string.Format(CultureInfo.CurrentCulture,
                Strings.Log_ErrorReadingProjectJson,
                path,
                exception.Message);

            var ex = new FileFormatException(message, exception);

            return ex.WithFilePath(path);
        }

        public static FileFormatException Create(string message, JToken value, string path)
        {
            var lineInfo = (IJsonLineInfo)value;

            var ex = new FileFormatException(message);

            return ex.WithFilePath(path).WithLineInfo(lineInfo);
        }

        internal static FileFormatException Create(string message, int line, int column, string path)
        {
            var ex = new FileFormatException(message);

            return ex.WithFilePath(path).WithLineInfo(line, column);
        }

        internal static FileFormatException Create(JsonReaderException exception, string path)
        {
            string message;
            message = string.Format(CultureInfo.CurrentCulture,
                Strings.Log_ErrorReadingProjectJsonWithLocation,
                path, exception.LineNumber,
                exception.LinePosition,
                exception.Message);

            return new FileFormatException(message, exception)
                .WithFilePath(path)
                .WithLineInfo(exception);
        }

        internal static FileFormatException Create(string message, string path)
        {
            return new FileFormatException(message)
                .WithFilePath(path);
        }
    }
}
