﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions;

public static class ModelDirective
{
    public static readonly DirectiveDescriptor Directive = DirectiveDescriptor.CreateDirective(
        "model",
        DirectiveKind.SingleLine,
        builder =>
        {
            builder.AddTypeToken(RazorExtensionsResources.ModelDirective_TypeToken_Name, RazorExtensionsResources.ModelDirective_TypeToken_Description);
            builder.Usage = DirectiveUsage.FileScopedSinglyOccurring;
            builder.Description = RazorExtensionsResources.ModelDirective_Description;
        });

    public static RazorProjectEngineBuilder Register(RazorProjectEngineBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.AddDirective(Directive);
        builder.Features.Add(new Pass());
        return builder;
    }

    public static IntermediateToken GetModelType(DocumentIntermediateNode document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        var visitor = new Visitor();
        return GetModelType(document, visitor);
    }

    private static IntermediateToken GetModelType(DocumentIntermediateNode document, Visitor visitor)
    {
        visitor.Visit(document);

        for (var i = visitor.ModelDirectives.Count - 1; i >= 0; i--)
        {
            var directive = visitor.ModelDirectives[i];

            var tokens = directive.Tokens.ToArray();
            if (tokens.Length >= 1)
            {
                return IntermediateToken.CreateCSharpToken(tokens[0].Content, tokens[0].Source);
            }
        }

        if (document.DocumentKind == RazorPageDocumentClassifierPass.RazorPageDocumentKind)
        {
            return IntermediateToken.CreateCSharpToken(visitor.Class.ClassName);
        }
        else
        {
            return IntermediateToken.CreateCSharpToken("dynamic");
        }
    }

    internal class Pass : IntermediateNodePassBase, IRazorDirectiveClassifierPass
    {
        // Runs after the @inherits directive
        public override int Order => 5;

        protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
        {
            if (documentNode.DocumentKind != RazorPageDocumentClassifierPass.RazorPageDocumentKind &&
               documentNode.DocumentKind != MvcViewDocumentClassifierPass.MvcViewDocumentKind)
            {
                // Not a MVC file. Skip.
                return;
            }

            var visitor = new Visitor();
            var modelType = GetModelType(documentNode, visitor);

            if (documentNode.Options.DesignTime)
            {
                // Alias the TModel token to a known type.
                // This allows design time compilation to succeed for Razor files where the token isn't replaced.
                var typeName = $"global::{typeof(object).FullName}";
                var usingNode = new UsingDirectiveIntermediateNode()
                {
                    Content = $"TModel = {typeName}"
                };

                visitor.Namespace?.Children.Insert(0, usingNode);
                modelType.Source = null;
            }

            if (visitor.Class?.BaseType is BaseTypeWithModel { ModelType: not null } existingBaseType)
            {
                existingBaseType.ModelType = modelType;
            }
        }
    }

    private class Visitor : IntermediateNodeWalker
    {
        public NamespaceDeclarationIntermediateNode Namespace { get; private set; }

        public ClassDeclarationIntermediateNode Class { get; private set; }

        public IList<DirectiveIntermediateNode> ModelDirectives { get; } = new List<DirectiveIntermediateNode>();

        public override void VisitNamespaceDeclaration(NamespaceDeclarationIntermediateNode node)
        {
            if (Namespace == null)
            {
                Namespace = node;
            }

            base.VisitNamespaceDeclaration(node);
        }

        public override void VisitClassDeclaration(ClassDeclarationIntermediateNode node)
        {
            if (Class == null)
            {
                Class = node;
            }

            base.VisitClassDeclaration(node);
        }

        public override void VisitDirective(DirectiveIntermediateNode node)
        {
            if (node.Directive == Directive)
            {
                ModelDirectives.Add(node);
            }
        }
    }
}
