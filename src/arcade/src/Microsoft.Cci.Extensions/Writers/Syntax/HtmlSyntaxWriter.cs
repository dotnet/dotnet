// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using Microsoft.Cci.Differs;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Cci.Writers.Syntax
{
    public class HtmlSyntaxWriter : IndentionSyntaxWriter, IStyleSyntaxWriter, IReviewCommentWriter
    {
        private volatile bool _hasColor = false;

        private Action _writeColor;
        private Action _writeBackground;

        public HtmlSyntaxWriter(TextWriter writer)
            : base(writer)
        {
            WriteCore("<pre>");
        }

        public bool StrikeOutRemoved { get; set; }

        public IDisposable StartStyle(SyntaxStyle style, object context)
        {
            IDisposable disposeAction = null;
            switch (style)
            {
                case SyntaxStyle.Added:
                    disposeAction = WriteColor("green");
                    break;

                case SyntaxStyle.Removed:
                    string extraStyle = this.StrikeOutRemoved ? " text-decoration:line-through;" : "";
                    disposeAction = WriteColor("red", extraStyle);
                    break;

                case SyntaxStyle.InheritedMember:
                case SyntaxStyle.InterfaceMember:
                case SyntaxStyle.Comment:
                    disposeAction = WriteColor("gray");
                    break;
                case SyntaxStyle.NotCompatible:
                    disposeAction = WriteBackground("yellow", context);
                    break;

                default:
                    throw new NotSupportedException("Style not supported!");
            }

            Contract.Assert(disposeAction != null);
            return new DisposeAction(() => disposeAction.Dispose());
        }

        public void Write(string str)
        {
            WriteEncoded(str);
        }

        public void WriteSymbol(string symbol)
        {
            WriteEncoded(symbol);
        }

        public void WriteIdentifier(string id)
        {
            WriteEncoded(id);
        }

        public void WriteKeyword(string keyword)
        {
            using (WriteColor("blue"))
                WriteEncoded(keyword);
        }

        public void WriteTypeName(string typeName)
        {
            using (WriteColor("#2B91AF"))
                WriteEncoded(typeName);
        }

        public void WriteReviewComment(string author, string text)
        {
            var comment = "> " + author + ": " + text;
            using (WriteColor("gray", "font-weight: bold;padding-left: 20px;"))
                WriteEncoded(comment);
        }

        protected override void WriteCore(string s)
        {
            if (_writeColor != null)
            {
                // Need to not get into an infinite loop
                Action write = _writeColor;
                _writeColor = null;
                write();
            }

            if (_writeBackground != null)
            {
                Action write = _writeBackground;
                _writeBackground = null;
                write();
            }

            base.WriteCore(s);
        }

        public void Dispose()
        {
            WriteCore("</pre>");
        }

        private IDisposable WriteColor(string color, string extraStyle = "")
        {
            if (_writeColor != null || _hasColor)
                return new DisposeAction(() => { });

            _hasColor = true;
            _writeColor = () =>
                WriteCore(string.Format("<span style=\" color: {0};{1}\">", color, extraStyle));

            return new DisposeAction(() =>
            {
                _hasColor = false;
                // Only write if we wrote the beginning tag
                if (_writeColor == null)
                    WriteCore("</span>");

                _writeColor = null;
            });
        }

        private IDisposable WriteBackground(string color, object context)
        {
            string tooltip = "";

            IEnumerable<IncompatibleDifference> differences = context as IEnumerable<IncompatibleDifference>;
            if (context != null)
            {
                tooltip = string.Join(", ", differences.Select(d => d.Message));
            }

            _writeBackground = () =>
                WriteCore(string.Format("<span style=\"background-color:{0}\" title=\"{1}\">", color, tooltip));

            return new DisposeAction(() =>
            {
                // Only write if we wrote the beginning tag
                if (_writeBackground == null)
                    WriteCore("</span>");

                _writeBackground = null;
            });
        }

        private void WriteEncoded(string s)
        {
            WriteCore(WebUtility.HtmlEncode(s));
        }
    }
}
