// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.Cci.Writers.Syntax
{
    public class TokenSyntaxWriter : ISyntaxWriter
    {
        private List<SyntaxToken> _tokens = new List<SyntaxToken>();
        private const int SpacesInIndent = 2;
        private string _indent = "";
        private bool _needToWriteIndent = true;
        private bool _shouldWriteLine = false;

        public void ClearTokens()
        {
            _tokens.Clear();
            _indent = "";
        }

        public IEnumerable<SyntaxToken> ToTokenList()
        {
            return _tokens.ToArray();
        }

        public void Write(string str)
        {
            Add(SyntaxTokenType.Literal, str);
        }

        public void WriteSymbol(string symbol)
        {
            Add(SyntaxTokenType.Symbol, symbol);
        }

        public void WriteIdentifier(string id)
        {
            Add(SyntaxTokenType.Identifier, id);
        }

        public void WriteKeyword(string keyword)
        {
            Add(SyntaxTokenType.Keyword, keyword);
        }

        public void WriteTypeName(string typeName)
        {
            Add(SyntaxTokenType.TypeName, typeName);
        }

        public void WriteLine()
        {
            if (!_shouldWriteLine)
                return;

            Write(Environment.NewLine);
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

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void WriteToken(SyntaxToken token)
        {
            _tokens.Add(token);
        }

        private void Add(SyntaxTokenType token, string s)
        {
            if (_needToWriteIndent && _indent.Length > 0)
                WriteToken(new SyntaxToken(SyntaxTokenType.Literal, _indent));

            WriteToken(new SyntaxToken(token, s));
            _needToWriteIndent = false;
            _shouldWriteLine = true;
        }
    }
}
