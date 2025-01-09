// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Child of a Folder that represents a file in a solution folder.
/// </summary>
internal sealed class XmlFile(SlnxFile root, XmlElement element) :
    XmlDecorator(root, element, Keyword.File),
    IItemRefDecorator
{
    public Keyword ItemRefAttribute => Keyword.Path;

    internal string Path => this.ItemRef;
}
