// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Serializer.SlnV12;
using Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer;

/// <summary>
/// Solution serializers implemented by this package.
/// </summary>
public static class SolutionSerializers
{
    /// <summary>
    /// Gets the .sln V12 solution serializer.
    /// </summary>
    public static ISolutionSingleFileSerializer<SlnV12SerializerSettings> SlnFileV12 => SlnFileV12Serializer.Instance;

    /// <summary>
    /// Gets the .slnx XML solution serializer.
    /// </summary>
    public static ISolutionSingleFileSerializer<SlnxSerializerSettings> SlnXml => SlnXmlSerializer.Instance;

    /// <summary>
    /// Gets all the solution serializers implemented by this package.
    /// </summary>
    public static IReadOnlyCollection<ISolutionSerializer> Serializers => [SlnFileV12, SlnXml];

    /// <summary>
    /// Finds a serializer that supports opening the given solution moniker.
    /// </summary>
    /// <param name="moniker">A moniker to a solution location.</param>
    /// <returns>A serializer that supports the solution moniker.</returns>
    public static ISolutionSerializer? GetSerializerByMoniker(string moniker)
    {
        foreach (ISolutionSerializer serializer in Serializers)
        {
            if (serializer.IsSupported(moniker))
            {
                return serializer;
            }
        }

        return null;
    }
}
