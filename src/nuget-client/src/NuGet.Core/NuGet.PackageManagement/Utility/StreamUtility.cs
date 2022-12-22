// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NuGet.ProjectManagement
{
    public static class StreamUtility
    {
        public static Stream StreamFromString(string content)
        {
            return StreamFromString(content, Encoding.UTF8);
        }

        public static Stream StreamFromString(string content, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(content));
        }

        /// <summary>
        /// Compare the content of the two streams of data, ingoring the content within the
        /// NUGET: BEGIN LICENSE TEXT and NUGET: END LICENSE TEXCT markers.
        /// </summary>
        /// <param name="stream">First stream</param>
        /// <param name="otherStream">Second stream which MUST be a seekable stream.</param>
        /// <returns>true if the two streams are considered equal.</returns>
        public static bool ContentEquals(Stream stream, Stream otherStream)
        {
            Debug.Assert(otherStream.CanSeek);

            var isBinaryFile = IsBinary(otherStream);
            otherStream.Seek(0, SeekOrigin.Begin);

            return isBinaryFile ? CompareBinary(stream, otherStream) : CompareText(stream, otherStream);
        }

        public static bool IsBinary(Stream stream)
        {
            // Quick and dirty trick to check if a stream represents binary content.
            // We read the first 30 bytes. If there's a character 0 in those bytes, 
            // we assume this is a binary file. 
            var a = new byte[30];
            var bytesRead = stream.Read(a, 0, 30);
            var byteZeroIndex = Array.FindIndex(a, 0, bytesRead, d => d == 0);
            return byteZeroIndex >= 0;
        }

        private static bool CompareText(Stream stream, Stream otherStream)
        {
            var lines = ReadStreamLines(stream);
            var otherLines = ReadStreamLines(otherStream);

            // IMPORTANT: this comparison has to be case-sensitive, hence Ordinal instead of OrdinalIgnoreCase
            return lines.SequenceEqual(otherLines, StringComparer.Ordinal);
        }

        /// <summary>
        /// Read the specified stream and return all lines, but ignoring those within the
        /// NUGET: BEGIN LICENSE TEXT and NUGET: END LICENSE TEXT markers, case-insenstively.
        /// </summary>
        private static IEnumerable<string> ReadStreamLines(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var hasSeenBeginLine = false;

                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine();

                    if (line.IndexOf(Constants.EndIgnoreMarker, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        hasSeenBeginLine = false;
                    }
                    else if (line.IndexOf(Constants.BeginIgnoreMarker, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        hasSeenBeginLine = true;
                    }
                    else if (!hasSeenBeginLine)
                    {
                        // the current line is not within the marker lines.
                        yield return line;
                    }
                }
            }
        }

        private static bool CompareBinary(Stream stream, Stream otherStream)
        {
            if (stream.CanSeek
                && otherStream.CanSeek)
            {
                if (stream.Length != otherStream.Length)
                {
                    return false;
                }
            }

            var buffer = new byte[4 * 1024];
            var otherBuffer = new byte[4 * 1024];

            var bytesRead = 0;
            do
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    var otherBytesRead = otherStream.Read(otherBuffer, 0, bytesRead);
                    if (bytesRead != otherBytesRead)
                    {
                        return false;
                    }

                    for (var i = 0; i < bytesRead; i++)
                    {
                        if (buffer[i] != otherBuffer[i])
                        {
                            return false;
                        }
                    }
                }
            }
            while (bytesRead > 0);

            return true;
        }
    }
}
