// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions.Version1_X;

public static class InjectDirective
{
    public static readonly DirectiveDescriptor Directive = DirectiveDescriptor.CreateDirective(
        "inject",
        DirectiveKind.SingleLine,
        builder =>
        {
            builder
                .AddTypeToken(Resources.InjectDirective_TypeToken_Name, Resources.InjectDirective_TypeToken_Description)
                .AddMemberToken(Resources.InjectDirective_MemberToken_Name, Resources.InjectDirective_MemberToken_Description);

            builder.Usage = DirectiveUsage.FileScopedMultipleOccurring;
            builder.Description = Resources.InjectDirective_Description;
        });

    public static RazorProjectEngineBuilder Register(RazorProjectEngineBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.AddDirective(Directive);
        builder.Features.Add(new Pass());
        builder.AddTargetExtension(new InjectTargetExtension());
        return builder;
    }

    internal class Pass : IntermediateNodePassBase, IRazorDirectiveClassifierPass
    {
        // Runs after the @model and @namespace directives
        public override int Order => 10;

        protected override void ExecuteCore(RazorCodeDocument codeDocument, DocumentIntermediateNode documentNode)
        {
            var visitor = new Visitor();
            visitor.Visit(documentNode);
            var modelType = ModelDirective.GetModelType(documentNode);

            var properties = new HashSet<string>(StringComparer.Ordinal);

            for (var i = visitor.Directives.Count - 1; i >= 0; i--)
            {
                var directive = visitor.Directives[i];
                var tokens = directive.Tokens.ToArray();
                if (tokens.Length < 2)
                {
                    continue;
                }

                var typeName = tokens[0].Content;
                var typeSpan = tokens[0].Source;
                var memberName = tokens[1].Content;
                var memberSpan = tokens[1].Source;

                if (!properties.Add(memberName))
                {
                    continue;
                }

                const string tModel = "<TModel>";
                if (typeName.EndsWith(tModel, StringComparison.Ordinal))
                {
                    typeName = typeName[..^tModel.Length] + "<" + modelType + ">";
                    if (typeSpan.HasValue)
                    {
                        typeSpan = new SourceSpan(typeSpan.Value.FilePath, typeSpan.Value.AbsoluteIndex, typeSpan.Value.LineIndex, typeSpan.Value.CharacterIndex, typeSpan.Value.Length - tModel.Length, typeSpan.Value.LineCount, typeSpan.Value.EndCharacterIndex - tModel.Length);
                    }
                }

                var injectNode = new InjectIntermediateNode()
                {
                    TypeName = typeName,
                    MemberName = memberName,
                    TypeSource = typeSpan,
                    MemberSource = memberSpan
                };

                visitor.Class.Children.Add(injectNode);
            }
        }
    }

    private class Visitor : IntermediateNodeWalker
    {
        public ClassDeclarationIntermediateNode Class { get; private set; }

        public IList<DirectiveIntermediateNode> Directives { get; } = new List<DirectiveIntermediateNode>();

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
                Directives.Add(node);
            }
        }
    }
}
