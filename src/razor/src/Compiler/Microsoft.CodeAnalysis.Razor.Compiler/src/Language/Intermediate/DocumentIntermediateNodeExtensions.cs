// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Razor.Language.Intermediate;

public static class DocumentIntermediateNodeExtensions
{
    public static ClassDeclarationIntermediateNode FindPrimaryClass(this DocumentIntermediateNode node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return FindNode<ClassDeclarationIntermediateNode>(node, static n => n.IsPrimaryClass);
    }

    public static MethodDeclarationIntermediateNode FindPrimaryMethod(this DocumentIntermediateNode node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return FindNode<MethodDeclarationIntermediateNode>(node, static n => n.IsPrimaryMethod);
    }

    public static NamespaceDeclarationIntermediateNode FindPrimaryNamespace(this DocumentIntermediateNode node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        return FindNode<NamespaceDeclarationIntermediateNode>(node, static n => n.IsPrimaryNamespace);
    }

    public static IReadOnlyList<IntermediateNodeReference> FindDirectiveReferences(this DocumentIntermediateNode node, DirectiveDescriptor directive)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (directive == null)
        {
            throw new ArgumentNullException(nameof(directive));
        }

        var visitor = new DirectiveVisitor(directive);
        visitor.Visit(node);
        return visitor.Directives;
    }

    public static IReadOnlyList<IntermediateNodeReference> FindDescendantReferences<TNode>(this DocumentIntermediateNode document)
        where TNode : IntermediateNode
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        var visitor = new ReferenceVisitor<TNode>();
        visitor.Visit(document);
        return visitor.References;
    }

    private static T FindNode<T>(IntermediateNode node, Func<T, bool> predicate)
        where T : IntermediateNode
    {
        if (node is T target && predicate(target))
        {
            return target;
        }

        foreach (var child in node.Children)
        {
            var result = FindNode<T>(child, predicate);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private class DirectiveVisitor : IntermediateNodeWalker
    {
        private readonly DirectiveDescriptor _directive;

        public DirectiveVisitor(DirectiveDescriptor directive)
        {
            _directive = directive;
        }

        public List<IntermediateNodeReference> Directives = new List<IntermediateNodeReference>();

        public override void VisitDirective(DirectiveIntermediateNode node)
        {
            if (_directive == node.Directive)
            {
                Directives.Add(new IntermediateNodeReference(Parent, node));
            }

            base.VisitDirective(node);
        }
    }

    private class ReferenceVisitor<TNode> : IntermediateNodeWalker
        where TNode : IntermediateNode
    {
        public List<IntermediateNodeReference> References = new List<IntermediateNodeReference>();

        public override void VisitDefault(IntermediateNode node)
        {
            base.VisitDefault(node);

            // Use a post-order traversal because references are used to replace nodes, and thus
            // change the parent nodes.
            //
            // This ensures that we always operate on the leaf nodes first.
            if (node is TNode)
            {
                References.Add(new IntermediateNodeReference(Parent, node));
            }
        }
    }
}
