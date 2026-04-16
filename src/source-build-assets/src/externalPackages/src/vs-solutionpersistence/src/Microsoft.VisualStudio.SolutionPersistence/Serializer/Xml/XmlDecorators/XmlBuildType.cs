// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Child to Configurations that represents a build type (e.g. Debug/Release).
/// </summary>
internal sealed class XmlBuildType(SlnxFile root, XmlElement element) :
    XmlDecorator(root, element, Keyword.BuildType),
    IItemRefDecorator
{
    public Keyword ItemRefAttribute => Keyword.Name;

    internal string Name => this.ItemRef;
}
