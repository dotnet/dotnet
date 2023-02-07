// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#pragma warning disable CA2227 // Collection properties should be read only

namespace NuGet.Build.Tasks
{
    /// <summary>
    /// Represents arguments to the out-of-proc static graph-based restore which can be written to disk by <see cref="RestoreTaskEx" /> and then read by NuGet.Build.Tasks.Console.
    /// </summary>
    public sealed class StaticGraphRestoreArguments
    {
        /// <summary>
        /// Gets or sets a <see cref="Dictionary{TKey, TValue}" /> representing the global properties.
        /// </summary>

        public Dictionary<string, string> GlobalProperties { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="Dictionary{TKey, TValue}" /> containing option names and values.
        /// </summary>
        public Dictionary<string, string> Options { get; set; }

        /// <summary>
        /// Reads arguments from the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">A <see cref="Stream" /> to read arguments from.</param>
        /// <returns>A <see cref="StaticGraphRestoreArguments" /> object read from the specified stream.</returns>
        public static StaticGraphRestoreArguments Read(Stream stream, Encoding encoding)
        {
            using var reader = new BinaryReader(stream, encoding, leaveOpen: true);

            int count = SkipPreamble(reader, encoding);

            return new StaticGraphRestoreArguments
            {
                GlobalProperties = ReadDictionary(count),
                Options = ReadDictionary()
            };

            Dictionary<string, string> ReadDictionary(int count = -1)
            {
                count = count == -1 ? reader.ReadInt32() : count;

                var dictionary = new Dictionary<string, string>(capacity: count, StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < count; i++)
                {
                    dictionary.Add(
                        key: reader.ReadString(),
                        value: reader.ReadString());
                }

                return dictionary;
            }
        }

        /// <summary>
        /// Writes the current arguments to the specified stream.
        /// </summary>
        /// <param name="stream">A <see cref="Stream" /> to write the arguments to.</param>
        public void Write(StreamWriter streamWriter)
        {
            using BinaryWriter writer = new BinaryWriter(streamWriter.BaseStream, streamWriter.Encoding, leaveOpen: true);

            WriteDictionary(GlobalProperties);
            WriteDictionary(Options);

            void WriteDictionary(Dictionary<string, string> dictionary)
            {
                writer.Write(dictionary.Count);

                foreach (var item in dictionary)
                {
                    writer.Write(item.Key);
                    writer.Write(item.Value);
                }
            }
        }

        /// <summary>Skips the preamble in the specified <see cref="BinaryReader" />if any and returns the value of the first integer in the stream.</summary>
        /// <param name="reader">The <see cref="BinaryReader "/> that contains the preamble to skip.</param>
        /// <param name="encoding">The <see cref="Encoding" /> of the content.</param>
        /// <returns>The first integer in the stream after the preamble if one was found, otherwise -1.</returns>
        /// <remarks>
        /// Preambles are variable length from 2 to 4 bytes.  The first 4 bytes are either:
        ///   -Variable length preamble and any remaining bytes of the actual content
        ///   -No preamble and just the integer representing the number of items in the first dictionary
        ///
        /// Since the preamble could be 3 bytes, that means that the last byte in the buffer will be the first byte of the next segment. So this code
        /// determines how long the preamble is, replaces the remaining bytes at the beginning of the buffer, and copies the next set of bytes.  This
        /// effectively "skips" the preamble by eventually having the buffer contain the first 4 bytes of the content that should actually be read.
        ///
        /// Example stream:
        /// 
        /// |   3 byte preamble  |      4 byte integer       |
        /// |------|------|------|------|------|------|------|
        /// | 0xFF | 0XEF | 0x00 | 0x07 | 0x00 | 0x00 | 0x00 |
        ///
        /// The first 4 bytes are read into the buffer (notice one of the bytes is actually not the preamble):
        /// 
        /// |       4 byte buffer       |
        /// |------|------|------|------|
        /// | 0xFF | 0XEF | 0x00 | 0x07 |
        ///
        /// If the first three bytes match the preamble (0xFF, 0xEF, 0x00), then the array is shifted so that last item becomes the first:
        /// 
        /// |       4 byte buffer       |
        /// |------|------|------|------|
        /// | 0x07 | 0XEF | 0x00 | 0x07 |
        ///
        /// Since only 1 byte was moved up in the buffer, then the next 3 bytes from the stream are read:
        ///
        /// |       4 byte buffer       |
        /// |------|------|------|------|
        /// | 0x07 | 0X00 | 0x00 | 0x00 |
        ///
        /// Now the buffer contains the first integer in the stream and the preamble is skipped.
        /// 
        /// </remarks>
        private static int SkipPreamble(BinaryReader reader, Encoding encoding)
        {
            // Get the preamble from the current encoding which should only be a maximum of 4 bytes
            byte[] preamble = encoding.GetPreamble();

            if (preamble.Length == 0)
            {
                // Return -1 to the caller if there is no preamble for the encoding meaning that the stream is at the beginning of the expected content
                return -1;
            }

            // Create a buffer for the preamble which should be a maximum of 4 bytes
            byte[] buffer = new byte[4];

            // Read the first 4 bytes of the stream which should be either a preamble of 2 to 4 bytes or a 4 byte integer, if less than 4 bytes were read
            // then the buffer contains nothing.
            if (reader.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                return -1;
            }

            int matchingPreambleLength = 0;

            // Loop through the buffer and verify each byte of the preamble.
            for (int i = 0; i < preamble.Length; i++)
            {
                if (buffer[i] != preamble[i])
                {
                    break;
                }

                matchingPreambleLength++;
            }

            // If the first set of bytes were the preamble then it needs to be skipped
            if (matchingPreambleLength == preamble.Length)
            {
                // Copy the bytes after the preamble to the start of the buffer
                Array.Copy(buffer, matchingPreambleLength, buffer, 0, buffer.Length - matchingPreambleLength);

                // Read in the next bytes from the stream into the buffer so it contains the first 4 bytes after the preamble
                reader.Read(buffer, buffer.Length - matchingPreambleLength, matchingPreambleLength);
            }

            // Convert the buffer to an integer
            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
