// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Microsoft.AspNetCore.Razor.Language.CodeGeneration;

public class RuntimeNodeWriter : IntermediateNodeWriter
{
    public virtual string WriteCSharpExpressionMethod { get; set; } = "Write";

    public virtual string WriteHtmlContentMethod { get; set; } = "WriteLiteral";

    public virtual string BeginWriteAttributeMethod { get; set; } = "BeginWriteAttribute";

    public virtual string EndWriteAttributeMethod { get; set; } = "EndWriteAttribute";

    public virtual string WriteAttributeValueMethod { get; set; } = "WriteAttributeValue";

    public virtual string PushWriterMethod { get; set; } = "PushWriter";

    public virtual string PopWriterMethod { get; set; } = "PopWriter";

    public string TemplateTypeName { get; set; } = "Microsoft.AspNetCore.Mvc.Razor.HelperResult";

    public override void WriteUsingDirective(CodeRenderingContext context, UsingDirectiveIntermediateNode node)
    {
        if (node.Source is { FilePath: not null } sourceSpan)
        {
            using (context.CodeWriter.BuildEnhancedLinePragma(sourceSpan, context, suppressLineDefaultAndHidden: true))
            {
                context.CodeWriter.WriteUsing(node.Content, endLine: node.HasExplicitSemicolon);
            }
            if (!node.HasExplicitSemicolon)
            {
                context.CodeWriter.WriteLine(";");
            }
            if (node.AppendLineDefaultAndHidden)
            {
                context.CodeWriter.WriteLine("#line default");
                context.CodeWriter.WriteLine("#line hidden");
            }
        }
        else
        {
            context.CodeWriter.WriteUsing(node.Content);

            if (node.AppendLineDefaultAndHidden)
            {
                context.CodeWriter.WriteLine("#line default");
                context.CodeWriter.WriteLine("#line hidden");
            }
        }
    }

    public override void WriteCSharpExpression(CodeRenderingContext context, CSharpExpressionIntermediateNode node)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }

        context.CodeWriter.WriteStartMethodInvocation(WriteCSharpExpressionMethod);
        context.CodeWriter.WriteLine();
        WriteCSharpChildren(node.Children, context);
        context.CodeWriter.WriteEndMethodInvocation();
    }

    public override void WriteCSharpCode(CodeRenderingContext context, CSharpCodeIntermediateNode node)
    {
        var isWhitespaceStatement = true;
        for (var i = 0; i < node.Children.Count; i++)
        {
            var token = node.Children[i] as IntermediateToken;
            if (token == null || !string.IsNullOrWhiteSpace(token.Content))
            {
                isWhitespaceStatement = false;
                break;
            }
        }

        if (isWhitespaceStatement)
        {
            return;
        }

        WriteCSharpChildren(node.Children, context);
        context.CodeWriter.WriteLine();
    }

    private static void WriteCSharpChildren(IntermediateNodeCollection children, CodeRenderingContext context)
    {
        for (var i = 0; i < children.Count; i++)
        {
            if (children[i] is IntermediateToken token && token.IsCSharp)
            {
                using (context.CodeWriter.BuildEnhancedLinePragma(token.Source, context))
                {
                    context.CodeWriter.Write(token.Content);
                }
            }
            else
            {
                // There may be something else inside the statement like an extension node.
                context.RenderNode(children[i]);
            }
        }
    }

    public override void WriteHtmlAttribute(CodeRenderingContext context, HtmlAttributeIntermediateNode node)
    {
        var valuePieceCount = node
            .Children
            .Count(child =>
                child is HtmlAttributeValueIntermediateNode ||
                child is CSharpExpressionAttributeValueIntermediateNode ||
                child is CSharpCodeAttributeValueIntermediateNode ||
                child is ExtensionIntermediateNode);

        var prefixLocation = node.Source.Value.AbsoluteIndex;
        var suffixLocation = node.Source.Value.AbsoluteIndex + node.Source.Value.Length - node.Suffix.Length;
        context.CodeWriter
            .WriteStartMethodInvocation(BeginWriteAttributeMethod)
            .WriteStringLiteral(node.AttributeName)
            .WriteParameterSeparator()
            .WriteStringLiteral(node.Prefix)
            .WriteParameterSeparator()
            .Write(prefixLocation.ToString(CultureInfo.InvariantCulture))
            .WriteParameterSeparator()
            .WriteStringLiteral(node.Suffix)
            .WriteParameterSeparator()
            .Write(suffixLocation.ToString(CultureInfo.InvariantCulture))
            .WriteParameterSeparator()
            .Write(valuePieceCount.ToString(CultureInfo.InvariantCulture))
            .WriteEndMethodInvocation();

        context.RenderChildren(node);

        context.CodeWriter
            .WriteStartMethodInvocation(EndWriteAttributeMethod)
            .WriteEndMethodInvocation();
    }

    public override void WriteHtmlAttributeValue(CodeRenderingContext context, HtmlAttributeValueIntermediateNode node)
    {
        var prefixLocation = node.Source.Value.AbsoluteIndex;
        var valueLocation = node.Source.Value.AbsoluteIndex + node.Prefix.Length;
        var valueLength = node.Source.Value.Length;
        context.CodeWriter
            .WriteStartMethodInvocation(WriteAttributeValueMethod)
            .WriteStringLiteral(node.Prefix)
            .WriteParameterSeparator()
            .Write(prefixLocation.ToString(CultureInfo.InvariantCulture))
            .WriteParameterSeparator();

        // Write content
        for (var i = 0; i < node.Children.Count; i++)
        {
            if (node.Children[i] is IntermediateToken token && token.IsHtml)
            {
                context.CodeWriter.WriteStringLiteral(token.Content);
            }
            else
            {
                // There may be something else inside the attribute value like an extension node.
                context.RenderNode(node.Children[i]);
            }
        }

        context.CodeWriter
            .WriteParameterSeparator()
            .Write(valueLocation.ToString(CultureInfo.InvariantCulture))
            .WriteParameterSeparator()
            .Write(valueLength.ToString(CultureInfo.InvariantCulture))
            .WriteParameterSeparator()
            .WriteBooleanLiteral(true)
            .WriteEndMethodInvocation();
    }

    public override void WriteCSharpExpressionAttributeValue(CodeRenderingContext context, CSharpExpressionAttributeValueIntermediateNode node)
    {
        var prefixLocation = node.Source.Value.AbsoluteIndex.ToString(CultureInfo.InvariantCulture);
        context.CodeWriter
            .WriteStartMethodInvocation(WriteAttributeValueMethod)
            .WriteStringLiteral(node.Prefix)
            .WriteParameterSeparator()
            .Write(prefixLocation)
            .WriteParameterSeparator();

        WriteCSharpChildren(node.Children, context);

        var valueLocation = node.Source.Value.AbsoluteIndex + node.Prefix.Length;
        var valueLength = node.Source.Value.Length - node.Prefix.Length;
        context.CodeWriter
            .WriteParameterSeparator()
            .Write(valueLocation.ToString(CultureInfo.InvariantCulture))
            .WriteParameterSeparator()
            .Write(valueLength.ToString(CultureInfo.InvariantCulture))
            .WriteParameterSeparator()
            .WriteBooleanLiteral(false)
            .WriteEndMethodInvocation();
    }

    public override void WriteCSharpCodeAttributeValue(CodeRenderingContext context, CSharpCodeAttributeValueIntermediateNode node)
    {
        const string ValueWriterName = "__razor_attribute_value_writer";

        var prefixLocation = node.Source.Value.AbsoluteIndex;
        var valueLocation = node.Source.Value.AbsoluteIndex + node.Prefix.Length;
        var valueLength = node.Source.Value.Length - node.Prefix.Length;
        context.CodeWriter
            .WriteStartMethodInvocation(WriteAttributeValueMethod)
            .WriteStringLiteral(node.Prefix)
            .WriteParameterSeparator()
            .Write(prefixLocation.ToString(CultureInfo.InvariantCulture))
            .WriteParameterSeparator();

        context.CodeWriter.WriteStartNewObject(TemplateTypeName);

        using (context.CodeWriter.BuildAsyncLambda(ValueWriterName))
        {
            BeginWriterScope(context, ValueWriterName);
            WriteCSharpChildren(node.Children, context);
            EndWriterScope(context);
        }

        context.CodeWriter.WriteEndMethodInvocation(false);

        context.CodeWriter
            .WriteParameterSeparator()
            .Write(valueLocation.ToString(CultureInfo.InvariantCulture))
            .WriteParameterSeparator()
            .Write(valueLength.ToString(CultureInfo.InvariantCulture))
            .WriteParameterSeparator()
            .WriteBooleanLiteral(false)
            .WriteEndMethodInvocation();
    }

    public override void WriteHtmlContent(CodeRenderingContext context, HtmlContentIntermediateNode node)
    {
        const int MaxStringLiteralLength = 1024;

        var builder = new StringBuilder();
        for (var i = 0; i < node.Children.Count; i++)
        {
            if (node.Children[i] is IntermediateToken token && token.IsHtml)
            {
                builder.Append(token.Content);
            }
        }

        var content = builder.ToString();

        WriteHtmlLiteral(context, MaxStringLiteralLength, content);
    }

    // Internal for testing
    internal void WriteHtmlLiteral(CodeRenderingContext context, int maxStringLiteralLength, string literal)
    {
        if (literal.Length <= maxStringLiteralLength)
        {
            WriteLiteral(literal);
            return;
        }

        // String is too large, render the string in pieces to avoid Roslyn OOM exceptions at compile time: https://github.com/aspnet/External/issues/54
        var charactersConsumed = 0;
        do
        {
            var charactersRemaining = literal.Length - charactersConsumed;
            var charactersToSubstring = Math.Min(maxStringLiteralLength, charactersRemaining);
            var lastCharBeforeSplitIndex = charactersConsumed + charactersToSubstring - 1;
            var lastCharBeforeSplit = literal[lastCharBeforeSplitIndex];

            if (char.IsHighSurrogate(lastCharBeforeSplit))
            {
                if (charactersRemaining > 1)
                {
                    // Take one less character this iteration. We're attempting to split inbetween a surrogate pair.
                    // This can happen when something like an emoji sits on the barrier between splits; if we were to
                    // split the emoji we'd end up with invalid bytes in our output.
                    charactersToSubstring--;
                }
                else
                {
                    // The user has an invalid file with a partial surrogate a the splitting point.
                    // We'll let the invalid character flow but we'll explode later on.
                }
            }

            var textToRender = literal.Substring(charactersConsumed, charactersToSubstring);

            WriteLiteral(textToRender);

            charactersConsumed += textToRender.Length;
        } while (charactersConsumed < literal.Length);

        void WriteLiteral(string content)
        {
            context.CodeWriter
                .WriteStartMethodInvocation(WriteHtmlContentMethod)
                .WriteStringLiteral(content)
                .WriteEndMethodInvocation();
        }
    }

    public override void BeginWriterScope(CodeRenderingContext context, string writer)
    {
        context.CodeWriter.WriteMethodInvocation(PushWriterMethod, writer);
    }

    public override void EndWriterScope(CodeRenderingContext context)
    {
        context.CodeWriter.WriteMethodInvocation(PopWriterMethod);
    }
}
