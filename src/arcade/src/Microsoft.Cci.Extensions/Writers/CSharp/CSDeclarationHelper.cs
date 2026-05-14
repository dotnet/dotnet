// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Writers.Syntax;

namespace Microsoft.Cci.Writers.CSharp
{
    public class CSDeclarationHelper
    {
        private readonly ICciFilter _filter;
        private readonly bool _forCompilation;
        private readonly bool _includeFakeAttributes;

        private StringBuilder _string;
        private CSDeclarationWriter _stringWriter;

        private TokenSyntaxWriter _tokenizer;
        private CSDeclarationWriter _tokenWriter;

        public CSDeclarationHelper(ICciFilter filter, bool forCompilation = false, bool includePseudoCustomAttributes = false)
        {
            _filter = filter;
            _forCompilation = forCompilation;
            _includeFakeAttributes = includePseudoCustomAttributes;
        }

        public string GetString(IDefinition definition, int indentLevel = -1)
        {
            EnsureStringWriter();

            _string.Clear();

            if (indentLevel != -1)
                _stringWriter.SyntaxtWriter.IndentLevel = indentLevel;

            _stringWriter.WriteDeclaration(definition);

            return _string.ToString();
        }

        public string GetString(ICustomAttribute attribute, int indentLevel = -1)
        {
            EnsureStringWriter();

            _string.Clear();

            if (indentLevel != -1)
                _stringWriter.SyntaxtWriter.IndentLevel = indentLevel;

            _stringWriter.WriteAttribute(attribute);

            return _string.ToString();
        }

        public IEnumerable<SyntaxToken> GetTokenList(IDefinition definition, int indentLevel = -1)
        {
            EnsureTokenWriter();

            _tokenizer.ClearTokens();

            if (indentLevel != -1)
                _tokenizer.IndentLevel = indentLevel;

            _tokenWriter.WriteDeclaration(definition);

            return _tokenizer.ToTokenList();
        }

        public IEnumerable<SyntaxToken> GetTokenList(ICustomAttribute attribute, int indentLevel = -1)
        {
            EnsureTokenWriter();

            _tokenizer.ClearTokens();

            if (indentLevel != -1)
                _tokenizer.IndentLevel = indentLevel;

            _tokenWriter.WriteAttribute(attribute);

            return _tokenizer.ToTokenList();
        }

        private void EnsureStringWriter()
        {
            if (_stringWriter == null)
            {
                _string = new StringBuilder();
                StringWriter sw = new StringWriter(_string);
                TextSyntaxWriter tsw = new TextSyntaxWriter(sw);

                _stringWriter = new CSDeclarationWriter(tsw, _filter, _forCompilation, _includeFakeAttributes);
            }
        }

        private void EnsureTokenWriter()
        {
            if (_tokenWriter == null)
            {
                _tokenizer = new TokenSyntaxWriter();
                _tokenWriter = new CSDeclarationWriter(_tokenizer, _filter, _forCompilation, _includeFakeAttributes);
            }
        }
    }
}
