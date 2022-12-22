// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.Cci.Dgml
{
    public static class DgmlExtensions
    {
        public static XDocument GetDgmlDependencyGraph(this IEnumerable<IAssembly> assemblies)
        {
            var materializedAssemblies = assemblies.ToArray();
            var definedNames = materializedAssemblies.Select(a => a.Name.Value);
            var referencedNames = materializedAssemblies.SelectMany(a => a.AssemblyReferences).Select(ar => ar.Name.Value);
            var assemblyNames = definedNames.Concat(referencedNames).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

            Array.Sort(assemblyNames, StringComparer.OrdinalIgnoreCase);

            var nodes = assemblyNames;
            var edges = from a in materializedAssemblies
                        let source = Array.BinarySearch(assemblyNames, a.Name.Value, StringComparer.OrdinalIgnoreCase)
                        from ar in a.AssemblyReferences
                        let target = Array.BinarySearch(assemblyNames, ar.Name.Value, StringComparer.OrdinalIgnoreCase)
                        select Tuple.Create(source, target);

            const string dgmlNsp = @"http://schemas.microsoft.com/vs/2009/dgml";
            var xDocument = new XDocument();
            var xRoot = new XElement(XName.Get("DirectedGraph", dgmlNsp));
            xDocument.Add(xRoot);

            var xNodes = new XElement(XName.Get("Nodes", dgmlNsp));
            xRoot.Add(xNodes);

            for (var i = 0; i < nodes.Length; i++)
            {
                var id = i;
                var node = nodes[i];
                var xNode = new XElement(XName.Get("Node", dgmlNsp),
                    new XAttribute("Id", id),
                    new XAttribute("Label", node));
                xNodes.Add(xNode);
            }

            var xLinks = new XElement(XName.Get("Links", dgmlNsp));
            xRoot.Add(xLinks);

            foreach (var edge in edges)
            {
                var source = edge.Item1;
                var target = edge.Item2;
                var xLink = new XElement(XName.Get("Link", dgmlNsp),
                    new XAttribute("Source", source),
                    new XAttribute("Target", target));
                xLinks.Add(xLink);
            }

            return xDocument;
        }
    }
}
