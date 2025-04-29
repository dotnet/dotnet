// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Legacy;
using Microsoft.AspNetCore.Razor.PooledObjects;
using Microsoft.VisualStudio.LegacyEditor.Razor.Indentation;
using Microsoft.VisualStudio.Text;

namespace Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor;

internal static partial class RazorWrapperFactory
{
    private class CodeDocumentWrapper(RazorCodeDocument obj) : Wrapper<RazorCodeDocument>(obj), IRazorCodeDocument
    {
        private string? _csharpGeneratedCode;

        public ImmutableArray<ClassifiedSpan> GetClassifiedSpans()
        {
            var result = Object.GetSyntaxTree().GetClassifiedSpans();

            using var builder = new PooledArrayBuilder<ClassifiedSpan>(capacity: result.Length);

            foreach (var item in result)
            {
                builder.Add(new ClassifiedSpan(
                    ConvertSourceSpan(item.Span),
                    ConvertSourceSpan(item.BlockSpan),
                    (SpanKind)item.SpanKind,
                    (BlockKind)item.BlockKind,
#pragma warning disable CS0618 // Type or member is obsolete
                    (AcceptedCharacters)item.AcceptedCharacters));
#pragma warning restore CS0618 // Type or member is obsolete
            }

            return builder.DrainToImmutable();
        }

        public ImmutableArray<TagHelperSpan> GetTagHelperSpans()
        {
            var result = Object.GetSyntaxTree().GetTagHelperSpans();

            using var builder = new PooledArrayBuilder<TagHelperSpan>(capacity: result.Length);

            foreach (var item in result)
            {
                builder.Add(new TagHelperSpan(
                    ConvertSourceSpan(item.Span),
                    WrapAll(item.Binding.Descriptors, Wrap)));
            }

            return builder.DrainToImmutable();
        }

        public ImmutableArray<RazorSourceMapping> GetSourceMappings()
        {
            var mappings = Object.GetCSharpDocument().SourceMappings;

            return WrapAll<SourceMapping, RazorSourceMapping>(mappings, ConvertSourceMapping);
        }

        public ImmutableArray<IRazorDiagnostic> GetDiagnostics()
        {
            var diagnostics = Object.GetCSharpDocument().Diagnostics;
            return WrapAll(diagnostics, Wrap);
        }

        public IRazorTagHelperDocumentContext GetTagHelperContext()
            => WrapTagHelperDocumentContext(Object.GetTagHelperContext());

        public int? GetDesiredIndentation(ITextSnapshot snapshot, ITextSnapshotLine line, int indentSize, int tabSize)
            => RazorIndentationFacts.GetDesiredIndentation(Object.GetSyntaxTree(), snapshot, line, indentSize, tabSize);

        public string GetGeneratedCode()
            => _csharpGeneratedCode ??=
                InterlockedOperations.Initialize(ref _csharpGeneratedCode, Object.GetCSharpDocument().Text.ToString());
    }
}
