// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;

namespace XliffTasks.Model
{
    internal sealed class UnstructuredDocument : TranslatableDocument
    {
        const string TranslatableSpanMarker = "@@@";
        const string TranslatableSpanSeparator = "|";

        private List<string> _fragments = new();
        private List<UnstructuredTranslatableNode> _nodes = new();

        protected override IEnumerable<TranslatableNode> GetTranslatableNodes()
        {
            return _nodes;
        }

        protected override void LoadCore(TextReader reader)
        {
            string text = reader.ReadToEnd();
            int lastSpanEnd = 0;
            int spanStart = text.IndexOf(TranslatableSpanMarker);
            while (spanStart >= 0)
            {
                // the previous span of text is untranslatable and is simply copied
                string plainSpan = text.Substring(lastSpanEnd, spanStart - lastSpanEnd);
                _fragments.Add(plainSpan);

                // next, find the translatable span
                lastSpanEnd = text.IndexOf(TranslatableSpanMarker, spanStart + 1);
                if (lastSpanEnd < 0)
                {
                    throw new InvalidOperationException($"No end of span marker '{TranslatableSpanMarker}' found.");
                }

                lastSpanEnd += TranslatableSpanMarker.Length; // account for the length of the end marker
                int spanLength = lastSpanEnd - spanStart - TranslatableSpanMarker.Length * 2; // trim off the marker start/end length
                string translatableSpan = text.Substring(spanStart + TranslatableSpanMarker.Length, spanLength);
                int separatorIndex = translatableSpan.IndexOf(TranslatableSpanSeparator);
                if (separatorIndex < 0)
                {
                    throw new InvalidOperationException($"No span separator '{TranslatableSpanSeparator}' found.");
                }

                string id = translatableSpan.Substring(0, separatorIndex);
                string source = translatableSpan.Substring(separatorIndex + TranslatableSpanSeparator.Length);

                // keep the original span's text
                _nodes.Add(new UnstructuredTranslatableNode(_fragments, _fragments.Count, id, source));
                _fragments.Add(source);

                spanStart = lastSpanEnd >= text.Length
                    ? -1 // don't search beyond the end of the text
                    : text.IndexOf(TranslatableSpanMarker, lastSpanEnd + 1);
            }

            if (lastSpanEnd < text.Length)
            {
                // add final span
                _fragments.Add(text.Substring(lastSpanEnd));
            }
        }

        protected override void SaveCore(TextWriter writer)
        {
            foreach (string fragment in _fragments)
            {
                writer.Write(fragment);
            }
        }

        private sealed class UnstructuredTranslatableNode : TranslatableNode
        {
            private IList<string> _fragments;
            private int _fragmentIndex;

            public UnstructuredTranslatableNode(IList<string> fragments, int fragmentIndex, string id, string source)
                : base(id, source, null)
            {
                _fragments = fragments;
                _fragmentIndex = fragmentIndex;
            }

            public override void Translate(string translation)
            {
                _fragments[_fragmentIndex] = translation;
            }
        }
    }
}
