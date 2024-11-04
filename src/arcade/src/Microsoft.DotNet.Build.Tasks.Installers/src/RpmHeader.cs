// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Installers
{
    internal sealed partial class RpmHeader<TEntryTag>(List<RpmHeader<TEntryTag>.Entry> entries)
        where TEntryTag : struct, Enum
    {

        [StructLayout(LayoutKind.Sequential)]
        private unsafe struct IndexEntry
        {
            public int Tag;
            public RpmHeaderEntryType Type;
            public int Offset;
            public int Count;
        }

        public readonly struct Entry(TEntryTag tag, RpmHeaderEntryType type, object value)
        {
            public Entry(int entryTag, RpmHeaderEntryType type, object value)
                : this((TEntryTag)Enum.ToObject(typeof(TEntryTag), entryTag), type, value)
            {
            }

            public TEntryTag Tag { get; } = tag;
            public RpmHeaderEntryType Type { get; } = type;
            public object Value { get; } = value;
        }

        public List<Entry> Entries { get; } = entries;

        public override string ToString()
        {
            StringBuilder builder = new();
            foreach (Entry entry in Entries)
            {
                builder.AppendLine($"\tTag: {entry.Tag}, Type: {entry.Type}");
                if (entry.Value is string str)
                {
                    builder.AppendLine($"\t\t'{str}'");
                }
                else if (entry.Type == RpmHeaderEntryType.Binary)
                {
                    builder.AppendLine($"\t\t{((ArraySegment<byte>)entry.Value).Count} bytes");
                    builder.AppendLine($"\t\t{BitConverter.ToString(((ArraySegment<byte>)entry.Value).Array!, ((ArraySegment<byte>)entry.Value).Offset, ((ArraySegment<byte>)entry.Value).Count)}");
                }
                else if (entry.Value is IEnumerable collection)
                {
                    foreach (object item in collection)
                    {
                        builder.AppendLine($"\t\t- '{item}'");
                    }
                }
                else
                {
                    builder.AppendLine($"\t\t{entry.Value}");
                }
            }
            return builder.ToString();
        }

        private static IndexEntry ReadIndexEntry(ReadOnlySpan<byte> bytes)
        {
            return new IndexEntry()
            {
                Tag = BinaryPrimitives.ReadInt32BigEndian(bytes),
                Type = (RpmHeaderEntryType)BinaryPrimitives.ReadInt32BigEndian(bytes.Slice(4)),
                Offset = BinaryPrimitives.ReadInt32BigEndian(bytes.Slice(8)),
                Count = BinaryPrimitives.ReadInt32BigEndian(bytes.Slice(12))
            };
        }

        private static void WriteIndexEntry(Span<byte> bytes, IndexEntry entry)
        {
            BinaryPrimitives.WriteInt32BigEndian(bytes, entry.Tag);
            BinaryPrimitives.WriteInt32BigEndian(bytes.Slice(4), (int)entry.Type);
            BinaryPrimitives.WriteInt32BigEndian(bytes.Slice(8), entry.Offset);
            BinaryPrimitives.WriteInt32BigEndian(bytes.Slice(12), entry.Count);
        }

        public static unsafe RpmHeader<TEntryTag> Read(Stream stream, TEntryTag immutableRegionTag)
        {
            if (!stream.ReadExactly(3).SequenceEqual((ReadOnlySpan<byte>)[0x8e, 0xad, 0xe8 ]))
            {
                throw new InvalidDataException("Invalid RPM header magic");
            }

            byte version = (byte)stream.ReadByte();

            if (version != 1)
            {
                throw new InvalidDataException("Unsupported RPM header version");
            }

            if (!stream.ReadExactly(4).SequenceEqual((ReadOnlySpan<byte>)[ 0x00, 0x00, 0x00, 0x00 ]))
            {
                throw new InvalidDataException("Invalid RPM header reserved bytes");
            }

            int numIndexEntries = BinaryPrimitives.ReadInt32BigEndian(stream.ReadExactly(4));
            int numHeaderBytes = BinaryPrimitives.ReadInt32BigEndian(stream.ReadExactly(4));

            byte[] indexBytes = new byte[sizeof(IndexEntry) * numIndexEntries];
            stream.ReadExactly(indexBytes, 0, indexBytes.Length);
            Span<byte> indexBytesSpan = indexBytes;
            IndexEntry[] indexEntries = new IndexEntry[numIndexEntries];
            for (int i = 0; i < numIndexEntries; i++)
            {
                indexEntries[i] = ReadIndexEntry(indexBytesSpan.Slice(i * sizeof(IndexEntry)));
            }

            byte[] store = new byte[numHeaderBytes];
            stream.ReadExactly(store, 0, store.Length);

            List<Entry> entries = [];

            foreach (var entry in indexEntries)
            {
                // Check the "immutable tag" and validate it if it exists.
                // We don't persist this tag as we need to generate it during emit.
                if (entry.Tag.Equals(Convert.ToInt32(immutableRegionTag)))
                {
                    IndexEntry indexEntry = ReadIndexEntry(store.AsSpan().Slice(entry.Offset));
                    if (entry.Tag != indexEntry.Tag
                        || !indexEntry.Type.Equals(RpmHeaderEntryType.Binary)
                        || indexEntry.Count != 16)
                    {
                        // Don't validate the size of the immutable region as it may not match the size of the header
                        // (i.e. if the package has been signed)
                        throw new InvalidOperationException("Invalid immutable region tag");
                    }
                    continue;
                }

                if (entry.Type is RpmHeaderEntryType.Binary)
                {
                    entries.Add(new Entry(entry.Tag, entry.Type, new ArraySegment<byte>(store, entry.Offset, entry.Count)));
                }
                else if (entry.Type is RpmHeaderEntryType.Null)
                {
                    throw new InvalidOperationException("Null entry should not be present in RPM header");
                }
                else if (entry.Type is RpmHeaderEntryType.String or RpmHeaderEntryType.I18NString)
                {
                    int offset = entry.Offset;
                    int nullTerminatorIndex = Array.IndexOf(store, (byte)0, offset);
                    if (nullTerminatorIndex == -1)
                    {
                        throw new InvalidOperationException("Invalid RPM header entry string (no null terminator found)");
                    }
                    int length = nullTerminatorIndex - offset;
                    entries.Add(new Entry(entry.Tag, entry.Type, Encoding.UTF8.GetString(store, entry.Offset, length)));
                }
                else if (entry.Type is RpmHeaderEntryType.StringArray)
                {
                    int offset = entry.Offset;
                    string[] strings = new string[entry.Count];
                    for (int i = 0; i < entry.Count; i++)
                    {
                        int nullTerminatorIndex = Array.IndexOf(store, (byte)0, offset);
                        if (nullTerminatorIndex == -1)
                        {
                            throw new InvalidOperationException("Invalid RPM header entry string (no null terminator found)");
                        }
                        int length = nullTerminatorIndex - offset;
                        strings[i] = Encoding.UTF8.GetString(store, offset, length);
                        offset += length + 1;
                    }
                    entries.Add(new Entry(entry.Tag, entry.Type, strings));
                }
                else
                {
                    Array contents = Array.CreateInstance(entry.Type switch
                    {
                        RpmHeaderEntryType.Char => typeof(char),
                        RpmHeaderEntryType.Int8 => typeof(byte),
                        RpmHeaderEntryType.Int16 => typeof(short),
                        RpmHeaderEntryType.Int32 => typeof(int),
                        RpmHeaderEntryType.Int64 => typeof(long),
                        _ => throw new InvalidOperationException("Invalid RPM header entry type")
                    }, entry.Count);

                    int offset = entry.Offset;

                    for (int i = 0; i < entry.Count; i++)
                    {
                        switch (entry.Type)
                        {
                            case RpmHeaderEntryType.Char:
                                contents.SetValue((char)store[offset], i);
                                offset += 1;
                                break;
                            case RpmHeaderEntryType.Int8:
                                contents.SetValue(store[offset], i);
                                offset += 1;
                                break;
                            case RpmHeaderEntryType.Int16:
                                contents.SetValue(BinaryPrimitives.ReadInt16BigEndian(store.AsSpan().Slice(offset.AlignUp(2), 2)), i);
                                offset += 2;
                                break;

                            case RpmHeaderEntryType.Int32:
                                contents.SetValue(BinaryPrimitives.ReadInt32BigEndian(store.AsSpan().Slice(offset.AlignUp(4), 4)), i);
                                offset += 4;
                                break;

                            case RpmHeaderEntryType.Int64:
                                contents.SetValue(BinaryPrimitives.ReadInt64BigEndian(store.AsSpan().Slice(offset.AlignUp(8), 8)), i);
                                offset += 8;
                                break;
                        }
                    }

                    entries.Add(new Entry(entry.Tag, entry.Type, contents));
                }
            }

            return new RpmHeader<TEntryTag>(entries);
        }

        public void WriteTo(Stream stream, TEntryTag immutableRegionTag)
        {
            stream.Write([0x8e, 0xad, 0xe8]); // magic
            stream.WriteByte(1); // version
            stream.Write([0, 0, 0, 0]); // reserved
            byte[] numIndexEntries = new byte[4];
            // Add 1 for the immutable region tag
            BinaryPrimitives.WriteInt32BigEndian(numIndexEntries, Entries.Count + 1);
            stream.Write(numIndexEntries);

            MemoryStream indexStream = new();

            using MemoryStream storeStream = new();

            byte[] indexInfoBytes = new byte[16];

            foreach (var entry in Entries)
            {
                BinaryPrimitives.WriteInt32BigEndian(indexInfoBytes, Convert.ToInt32(entry.Tag));
                BinaryPrimitives.WriteInt32BigEndian(indexInfoBytes.AsSpan(4), (int)entry.Type);
                if (entry.Type is RpmHeaderEntryType.Binary)
                {
                    BinaryPrimitives.WriteInt32BigEndian(indexInfoBytes.AsSpan(8), (int)storeStream.Length);

                    ArraySegment<byte> binary = (ArraySegment<byte>)entry.Value;
                    storeStream.Write(binary.Array!, binary.Offset, binary.Count);
                    
                    BinaryPrimitives.WriteInt32BigEndian(indexInfoBytes.AsSpan(12), binary.Count);
                }
                else if (entry.Type is RpmHeaderEntryType.Null)
                {
                    throw new InvalidOperationException("Null entry should not be present in RPM header");
                }
                else if (entry.Type is RpmHeaderEntryType.String or RpmHeaderEntryType.I18NString)
                {
                    BinaryPrimitives.WriteInt32BigEndian(indexInfoBytes.AsSpan(8), (int)storeStream.Length);
                    BinaryPrimitives.WriteInt32BigEndian(indexInfoBytes.AsSpan(12), 1);

                    byte[] bytes = Encoding.UTF8.GetBytes((string)entry.Value);
                    storeStream.Write(bytes);
                    storeStream.WriteByte(0);

                }
                else if (entry.Type is RpmHeaderEntryType.StringArray)
                {
                    BinaryPrimitives.WriteInt32BigEndian(indexInfoBytes.AsSpan(8), (int)storeStream.Length);

                    string[] strings = (string[])entry.Value;

                    BinaryPrimitives.WriteInt32BigEndian(indexInfoBytes.AsSpan(12), strings.Length);

                    foreach (string str in strings)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(str);
                        storeStream.Write(bytes);
                        storeStream.WriteByte(0);
                    }
                }
                else
                {
                    int alignment = entry.Type switch
                    {
                        RpmHeaderEntryType.Char => 1,
                        RpmHeaderEntryType.Int8 => 1,
                        RpmHeaderEntryType.Int16 => 2,
                        RpmHeaderEntryType.Int32 => 4,
                        RpmHeaderEntryType.Int64 => 8,
                        _ => throw new InvalidOperationException("Invalid RPM header entry type")
                    };

                    storeStream.AlignWriteTo(alignment);
                    BinaryPrimitives.WriteInt32BigEndian(indexInfoBytes.AsSpan(8), (int)storeStream.Length);

                    Array contents = (Array)entry.Value;

                    BinaryPrimitives.WriteInt32BigEndian(indexInfoBytes.AsSpan(12), contents.Length);

                    byte[] tempBeBytes = new byte[8];
                    for (int i = 0; i < contents.Length; i++)
                    {
                        switch (entry.Type)
                        {
                            case RpmHeaderEntryType.Char:
                                storeStream.WriteByte((byte)(char)contents.GetValue(i)!);
                                break;
                            case RpmHeaderEntryType.Int8:
                                storeStream.WriteByte((byte)contents.GetValue(i)!);
                                break;
                            case RpmHeaderEntryType.Int16:
                                BinaryPrimitives.WriteInt16BigEndian(tempBeBytes, (short)contents.GetValue(i)!);
                                storeStream.Write(tempBeBytes, 0, 2);
                                break;
                            case RpmHeaderEntryType.Int32:
                                BinaryPrimitives.WriteInt32BigEndian(tempBeBytes, (int)contents.GetValue(i)!);
                                storeStream.Write(tempBeBytes, 0, 4);
                                break;
                            case RpmHeaderEntryType.Int64:
                                BinaryPrimitives.WriteInt64BigEndian(tempBeBytes, (long)contents.GetValue(i)!);
                                storeStream.Write(tempBeBytes, 0, 8);
                                break;
                        }
                    }
                }

                indexStream.Write(indexInfoBytes);
            }

            // Now that we've written all of the index entries, we can write the immutable region tags.
            byte[] immutableRegionIndexEntryBytes = new byte[16];
            IndexEntry immutableRegionIndexEntry = new()
            {
                Tag = Convert.ToInt32(immutableRegionTag),
                Type = RpmHeaderEntryType.Binary,
                Offset = (int)storeStream.Position,
                Count = 16
            };

            WriteIndexEntry(immutableRegionIndexEntryBytes, immutableRegionIndexEntry);

            IndexEntry immutableRegionTrailer = new()
            {
                Tag = Convert.ToInt32(immutableRegionTag),
                Type = RpmHeaderEntryType.Binary,
                Offset = -(Entries.Count + 1) * Unsafe.SizeOf<IndexEntry>(),
                Count = 16
            };

            WriteIndexEntry(indexInfoBytes, immutableRegionTrailer);
            storeStream.Write(indexInfoBytes);

            // Now that we've written the whole index and store, we can write the size of the store
            byte[] numHeaderBytes = new byte[4];
            BinaryPrimitives.WriteInt32BigEndian(numHeaderBytes, (int)storeStream.Length);
            stream.Write(numHeaderBytes);

            // Now write the immutable region index entry
            // at the start of the index.
            stream.Write(immutableRegionIndexEntryBytes);

            // Now write the rest of the index index and the entire store.
            indexStream.Position = 0;
            storeStream.Position = 0;
            indexStream.CopyTo(stream);
            storeStream.CopyTo(stream);
        }
    }
}
