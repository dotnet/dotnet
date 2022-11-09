// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Text;
using Microsoft.Css.Parser.Parser;
using Microsoft.Css.Parser.Text;
using Microsoft.Css.Parser.Tokens;
using Microsoft.Css.Parser.TreeItems;

namespace Microsoft.Css.Parser
{
    internal sealed class DebugWriter
    {
        private StringBuilder _sb;
        private int _indent;

        internal string Serialize(ITextProvider textProvider, ParseItem pi, bool dumpClassifications = false)
        {
            _sb = new StringBuilder(pi.Length);
            _indent = 0;

            WriteItem(textProvider, pi, dumpClassifications);

            string s = _sb.ToString();
            _sb = null;

            return s;
        }

        private void WriteItem(ITextProvider textProvider, ParseItem pi, bool dumpClassifications)
        {
            if (pi is ItemName nm)
            {
                WriteName(textProvider, nm);
            }
            else
            {
                if (pi is ComplexItem ci)
                {
                    WriteComplexItem(textProvider, ci, dumpClassifications);
                }
                else
                {
                    WriteTokenItem(textProvider, pi as TokenItem, dumpClassifications);
                }
            }
        }

        private void WriteName(ITextProvider textProvider, ItemName nm)
        {
            _sb.Append(' ', _indent * 4);
            _sb.Append("Namespace    ");

            if (nm.Namespace != null)
            {
                _sb.Append(textProvider.GetText(nm.Namespace.Start, nm.Namespace.Length));
            }

            _sb.Append("\r\n");
            _sb.Append(' ', _indent * 4);
            _sb.Append("Separator    ");

            if (nm.Separator != null)
            {
                _sb.Append(textProvider.GetText(nm.Separator.Start, nm.Separator.Length));
            }

            _sb.Append("\r\n");
            _sb.Append(' ', _indent * 4);
            _sb.Append("Name    ");

            if (nm.Name != null)
            {
                _sb.Append(textProvider.GetText(nm.Name.Start, nm.Name.Length));
            }

            _sb.Append("\r\n");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "baseText")]
        private void WriteTokenItem(ITextProvider textProvider, TokenItem si, bool dumpClassifications)
        {
            _sb.Append(' ', _indent * 4);
            _sb.Append(si.TokenType.ToString());
            WriteParseErrors(si);

            if (dumpClassifications)
            {
                _sb.Append(' ', 4);
                _sb.Append(si.Context.ClassificationName);
            }

            if (si.TokenType != CssTokenType.EndOfFile)
            {
                _sb.Append(' ', 4);
                _sb.Append(textProvider.GetText(si.Start, si.Length));
            }

            _sb.Append("\r\n");
        }

        private void WriteComplexItem(ITextProvider textProvider, ComplexItem ci, bool dumpClassifications)
        {
            Type type = ci.GetType();

            _sb.Append(' ', _indent * 4);
            _sb.Append(type.Name);
            WriteParseErrors(ci);
            _sb.Append("\r\n");

            _indent++;

            foreach (ParseItem p in ci.Children)
            {
                WriteItem(textProvider, p, dumpClassifications);
            }

            if (_indent > 0)
            {
                _indent--;
            }
        }

        private void WriteParseErrors(ParseItem item)
        {
            if (item.HasParseErrors)
            {
                _sb.Append(" (");

                for (int i = 0; i < item.ParseErrors.Count; i++)
                {
                    _sb.Append(item.ParseErrors[i].ErrorType.ToString());
                    _sb.Append(":");
                    _sb.Append(item.ParseErrors[i].Location.ToString());

                    if (i + 1 < item.ParseErrors.Count)
                    {
                        _sb.Append(", ");
                    }
                }

                _sb.Append(")");
            }
        }
    }
}
