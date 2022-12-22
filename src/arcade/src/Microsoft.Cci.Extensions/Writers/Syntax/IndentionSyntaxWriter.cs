// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace Microsoft.Cci.Writers.Syntax
{
    public class IndentionSyntaxWriter
    {
        private readonly TextWriter _writer;
        private string _indent = "";
        private bool _needToWriteIndent = true;
        private bool _shouldWriteLine = false;

        public IndentionSyntaxWriter(TextWriter writer)
        {
            Contract.Requires(writer != null);
            _writer = writer;
            SpacesInIndent = 2;
        }

        protected void WriteCore(string format, params object[] args)
        {
            if (args.Length > 0)
                WriteCore(string.Format(format, args));
            else
                WriteCore(format);
        }

        protected virtual void WriteCore(string s)
        {
            if (_needToWriteIndent && _indent.Length > 0)
                WriteIndent(_writer, _indent);

            _writer.Write(s);
            _needToWriteIndent = false;
            _shouldWriteLine = true;
        }

        protected virtual void WriteLine(TextWriter writer)
        {
            writer.WriteLine();
        }

        protected virtual void WriteIndent(TextWriter writer, string indent)
        {
            writer.Write(indent);
        }

        public virtual void WriteLine()
        {
            if (!_shouldWriteLine)
                return;

            WriteLine(_writer);
            _needToWriteIndent = true;
            _shouldWriteLine = false;
        }

        public int IndentLevel
        {
            get
            {
                return _indent.Length / SpacesInIndent;
            }
            set
            {
                _indent = new string(' ', value * SpacesInIndent);
            }
        }

        public int SpacesInIndent { get; set; }
    }
}
