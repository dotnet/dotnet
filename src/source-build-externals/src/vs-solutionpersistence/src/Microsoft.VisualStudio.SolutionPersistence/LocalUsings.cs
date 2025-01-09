// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK
global using Microsoft.IO;

// Listing the types in System.IO that aren't in Microsoft.IO is a
// little tedious, but this should prevent any need to include the entire
// namespace which might lead to accidental usage of old types.
global using BinaryReader = System.IO.BinaryReader;
global using BinaryWriter = System.IO.BinaryWriter;
global using DirectoryNotFoundException = System.IO.DirectoryNotFoundException;
global using DriveInfo = System.IO.DriveInfo;
global using DriveType = System.IO.DriveType;
global using FileAccess = System.IO.FileAccess;
global using FileAttributes = System.IO.FileAttributes;
global using FileLoadException = System.IO.FileLoadException;
global using FileMode = System.IO.FileMode;
global using FileNotFoundException = System.IO.FileNotFoundException;
global using FileShare = System.IO.FileShare;
global using FileStream = System.IO.FileStream;
global using InvalidDataException = System.IO.InvalidDataException;
global using IOException = System.IO.IOException;
global using MemoryStream = System.IO.MemoryStream;
global using PathTooLongException = System.IO.PathTooLongException;
global using SeekOrigin = System.IO.SeekOrigin;
global using Stream = System.IO.Stream;
global using StreamReader = System.IO.StreamReader;
global using StreamWriter = System.IO.StreamWriter;
global using StringReader = System.IO.StringReader;
global using StringWriter = System.IO.StringWriter;
global using TextReader = System.IO.TextReader;
global using TextWriter = System.IO.TextWriter;
#else
global using System.IO;
#endif

global using System;
global using System.Buffers;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;
global using StringSpan = System.ReadOnlySpan<char>;
