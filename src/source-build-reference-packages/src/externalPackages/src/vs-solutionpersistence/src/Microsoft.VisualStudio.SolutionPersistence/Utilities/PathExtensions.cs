// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Utilities;

internal static class PathExtensions
{
    private static readonly bool IsWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;

    /// <summary>
    /// Converts a serialized path that uses backslashes to a model path that uses the platform's directory separator.
    /// This is used by the .sln serializer.
    /// </summary>
    [return: NotNullIfNotNull(nameof(persistencePath))]
    internal static string? ConvertBackslashToModel(string? persistencePath)
    {
        return persistencePath.IsNullOrEmpty() || IsWindows || !persistencePath.Contains('\\') ?
            persistencePath :
            persistencePath.Replace('\\', Path.DirectorySeparatorChar);
    }

    [return: NotNullIfNotNull(nameof(persistencePath))]
    internal static string? ConvertToModel(string? persistencePath)
    {
        char altSlash = IsWindows ? Path.AltDirectorySeparatorChar : '\\';

        return persistencePath.IsNullOrEmpty() || !persistencePath.Contains(altSlash) || IsUri(persistencePath) ?
            persistencePath :
            persistencePath.Replace(altSlash, Path.DirectorySeparatorChar);
    }

    [return: NotNullIfNotNull(nameof(modelPath))]
    internal static string? ConvertModelToBackslashPath(string? modelPath)
    {
        return modelPath is null || IsWindows || !modelPath.Contains(Path.DirectorySeparatorChar) || IsUri(modelPath) ?
            modelPath :
            modelPath.Replace(Path.DirectorySeparatorChar, '\\');
    }

    [return: NotNullIfNotNull(nameof(modelPath))]
    internal static string? ConvertModelToForwardSlashPath(string? modelPath)
    {
        return modelPath is null || !IsWindows || !modelPath.Contains(Path.DirectorySeparatorChar) || IsUri(modelPath) ?
            modelPath :
            modelPath.Replace(Path.DirectorySeparatorChar, '/');
    }

    internal static StringSpan GetStandardDisplayName(string filePath)
    {
        return GetStandardDisplayName(filePath.AsSpan());
    }

    internal static StringSpan GetStandardDisplayName(StringSpan filePath)
    {
        if (filePath.IsEmpty || IsUri(filePath))
        {
            return StringSpan.Empty;
        }

        return Path.GetFileNameWithoutExtension(filePath);
    }

    internal static StringSpan GetExtension(string filePath)
    {
        return GetExtension(filePath.AsSpan());
    }

    internal static StringSpan GetExtension(StringSpan filePath)
    {
        return IsUri(filePath) ? StringSpan.Empty : Path.GetExtension(filePath);
    }

#if NETFRAMEWORK

    private static bool IsUri(string filePath) => IsUri(filePath.AsSpan());

#endif

    private static bool IsUri(StringSpan filePath) => !filePath.IsEmpty && filePath.Contains("://".AsSpan(), StringComparison.Ordinal);
}
