// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml.XmlDecorators;

/// <summary>
/// Child to a Project that represents a build dependency.
/// </summary>
internal sealed class XmlBuildDependency(SlnxFile root, XmlElement element) :
    XmlDecorator(root, element, Keyword.BuildDependency),
    IItemRefDecorator
{
    public Keyword ItemRefAttribute => Keyword.Project;

    internal string Project => this.ItemRef;
}
