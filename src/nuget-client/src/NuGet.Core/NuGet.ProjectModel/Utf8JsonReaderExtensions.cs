// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json;

namespace NuGet.ProjectModel
{
    internal static class Utf8JsonReaderExtensions
    {
        internal static readonly Utf8JsonStreamLockFileConverter LockFileConverter = new Utf8JsonStreamLockFileConverter();
        internal static readonly Utf8JsonStreamLockFileItemConverter<LockFileItem> LockFileItemConverter = new Utf8JsonStreamLockFileItemConverter<LockFileItem>((string filePath) => new LockFileItem(filePath));
        internal static readonly Utf8JsonStreamLockFileItemConverter<LockFileContentFile> LockFileContentFileConverter = new Utf8JsonStreamLockFileItemConverter<LockFileContentFile>((string filePath) => new LockFileContentFile(filePath));
        internal static readonly Utf8JsonStreamLockFileItemConverter<LockFileRuntimeTarget> LockFileRuntimeTargetConverter = new Utf8JsonStreamLockFileItemConverter<LockFileRuntimeTarget>((string filePath) => new LockFileRuntimeTarget(filePath));
        internal static readonly Utf8JsonStreamLockFileTargetLibraryConverter LockFileTargetLibraryConverter = new Utf8JsonStreamLockFileTargetLibraryConverter();
        internal static readonly Utf8JsonStreamLockFileLibraryConverter LockFileLibraryConverter = new Utf8JsonStreamLockFileLibraryConverter();
        internal static readonly Utf8JsonStreamLockFileTargetConverter LockFileTargetConverter = new Utf8JsonStreamLockFileTargetConverter();
        internal static readonly Utf8JsonStreamProjectFileDependencyGroupConverter ProjectFileDepencencyGroupConverter = new Utf8JsonStreamProjectFileDependencyGroupConverter();
        internal static readonly Utf8JsonStreamIAssetsLogMessageConverter IAssetsLogMessageConverter = new Utf8JsonStreamIAssetsLogMessageConverter();


        internal static string ReadTokenAsString(this ref Utf8JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.True:
                    return bool.TrueString;
                case JsonTokenType.False:
                    return bool.FalseString;
                case JsonTokenType.Number:
                    return reader.ReadNumberAsString();
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.None:
                case JsonTokenType.Null:
                    return null;
                default:
                    throw new InvalidCastException();
            }
        }

        private static string ReadNumberAsString(this ref Utf8JsonReader reader)
        {
            if (reader.TryGetInt64(out long value))
            {
                return value.ToString();
            }
            return reader.GetDouble().ToString();
        }
    }
}
