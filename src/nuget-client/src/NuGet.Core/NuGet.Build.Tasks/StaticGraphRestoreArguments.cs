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
    [Serializable]
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
        public static StaticGraphRestoreArguments Read(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

            return new StaticGraphRestoreArguments
            {
                GlobalProperties = ReadDictionary(count: reader.ReadInt32()),
                Options = ReadDictionary(count: reader.ReadInt32())
            };

            Dictionary<string, string> ReadDictionary(int count)
            {
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
        public void Write(Stream stream)
        {
            using BinaryWriter writer = new BinaryWriter(stream, Encoding.Default, leaveOpen: true);

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
    }
}
