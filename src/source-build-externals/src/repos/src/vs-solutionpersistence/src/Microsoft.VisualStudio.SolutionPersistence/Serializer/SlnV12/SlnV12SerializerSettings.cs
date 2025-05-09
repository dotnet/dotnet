// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.SlnV12;

/// <summary>
/// Custom settings for the <see cref="SolutionSerializers.SlnFileV12"/> serializer.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SlnV12SerializerSettings"/> struct.
/// Create a new settings with values from the specified settings.
/// </remarks>
/// <param name="settings">The settings to copy.</param>
public readonly struct SlnV12SerializerSettings(SlnV12SerializerSettings settings)
{
    /// <summary>
    /// Gets encoding to use when writing the solution file.
    /// Only ASCII, UTF-8, and UTF-16 are supported.
    /// </summary>
    public Encoding? Encoding { get; init; } = settings.Encoding;
}
