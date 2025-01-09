// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

/// <summary>
/// Allows customization of the behavior of the <see cref="SolutionSerializers.SlnXml"/> serializer.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SlnxSerializerSettings"/> struct.
/// Create a new settings with values from the specified settings.
/// </remarks>
/// <param name="settings">The settings to copy.</param>
public readonly struct SlnxSerializerSettings(SlnxSerializerSettings settings)
{
    /// <summary>
    /// Gets a value indicating whether to keep whitespace when writing the solution file.
    /// If this is true, the solution file will be written with the same whitespace as the original file.
    /// Default is true.
    /// </summary>
    public bool? PreserveWhitespace { get; init; } = settings.PreserveWhitespace;

    /// <summary>
    /// Gets the characters to use for indentation when writing the solution file.
    /// Default is two spaces.
    /// </summary>
    public string? IndentChars { get; init; } = settings.IndentChars;

    /// <summary>
    /// Gets the characters to use for new lines when writing the solution file.
    /// Default is the system's new line characters.
    /// </summary>
    public string? NewLine { get; init; } = settings.NewLine;

    /// <summary>
    /// Gets a value indicating whether to remove unneccessary Visual Studio properties from the solution file.
    /// </summary>
    public bool? TrimVisualStudioProperties { get; init; } = settings.TrimVisualStudioProperties;
}
