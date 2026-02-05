// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.ProjectModel
{
    internal static class Utf8JsonStreamLockFileConverters
    {
        internal static readonly Utf8JsonStreamLockFileConverter LockFileConverter = new Utf8JsonStreamLockFileConverter();
        internal static readonly Utf8JsonStreamLockFileItemConverter<LockFileItem> LockFileItemConverter = new Utf8JsonStreamLockFileItemConverter<LockFileItem>((string filePath) => new LockFileItem(filePath));
        internal static readonly Utf8JsonStreamLockFileItemConverter<LockFileContentFile> LockFileContentFileConverter = new Utf8JsonStreamLockFileItemConverter<LockFileContentFile>((string filePath) => new LockFileContentFile(filePath));
        internal static readonly Utf8JsonStreamLockFileItemConverter<LockFileRuntimeTarget> LockFileRuntimeTargetConverter = new Utf8JsonStreamLockFileItemConverter<LockFileRuntimeTarget>((string filePath) => new LockFileRuntimeTarget(filePath));
        internal static readonly Utf8JsonStreamLockFileTargetLibraryConverter LockFileTargetLibraryConverter = new Utf8JsonStreamLockFileTargetLibraryConverter();
        internal static readonly Utf8JsonStreamLockFileLibraryConverter LockFileLibraryConverter = new Utf8JsonStreamLockFileLibraryConverter();
        internal static readonly Utf8JsonStreamLockFileTargetConverter LockFileTargetConverter = new Utf8JsonStreamLockFileTargetConverter();
        internal static readonly Utf8JsonStreamLockFileTargetConverterV4 LockFileTargetConverterV4 = new Utf8JsonStreamLockFileTargetConverterV4();
        internal static readonly Utf8JsonStreamProjectFileDependencyGroupConverter ProjectFileDepencencyGroupConverter = new Utf8JsonStreamProjectFileDependencyGroupConverter();
        internal static readonly Utf8JsonStreamIAssetsLogMessageConverter IAssetsLogMessageConverter = new Utf8JsonStreamIAssetsLogMessageConverter();
    }
}
