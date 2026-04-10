// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Child to Configurations that represents a platform (e.g. x86/x64).
/// </summary>
internal sealed class XmlPlatform(SlnxFile root, XmlElement element) :
    XmlDecorator(root, element, Keyword.Platform),
    IItemRefDecorator
{
    public Keyword ItemRefAttribute => Keyword.Name;

    internal string Name => this.ItemRef;
}
