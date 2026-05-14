// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

/// <summary>
/// Initializes a new instance of the <see cref="SlnXmlModelExtension"/> class.
/// </summary>
[method: SetsRequiredMembers]
internal sealed class SlnXmlModelExtension(ISolutionSerializer serializer, SlnxSerializerSettings settings)
    : ISerializerModelExtension<SlnxSerializerSettings>
{
    [SetsRequiredMembers]
    internal SlnXmlModelExtension(ISolutionSerializer serializer, SlnxSerializerSettings settings, SlnxFile root)
        : this(serializer, settings)
    {
        this.Root = root;
    }

    /// <inheritdoc/>
    public required ISolutionSerializer Serializer { get; init; } = serializer;

    /// <inheritdoc/>
    public required SlnxSerializerSettings Settings { get; init; } = settings;

    /// <inheritdoc/>
    public bool Tarnished => this.Root?.Tarnished ?? false;

    internal SlnxFile? Root { get; init; }

    internal string? SolutionFileFullPath => this.Root?.FullPath;

    internal Version? Version => this.Root?.FileVersion;
}
