// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.Cci.Writers.Syntax
{
    public interface ISyntaxWriter : IDisposable
    {
        void Write(string str);
        void WriteSymbol(string symbol);
        void WriteIdentifier(string id);
        void WriteKeyword(string keyword);
        void WriteTypeName(string typeName);
        void WriteLine();
        int IndentLevel { get; set; }
    }

    public static class SyntaxWriterExtensions
    {
        public static void Write(this ISyntaxWriter writer, string str, params object[] args)
        {
            writer.Write(string.Format(str, args));
        }

        public static void WriteLine(this ISyntaxWriter writer, bool force)
        {
            if (force) // Need to make sure the stream isn't empty so that it doesn't ignore the WriteLine
                writer.Write(" ");
            writer.WriteLine();
        }

        public static void WriteSpace(this ISyntaxWriter writer)
        {
            writer.Write(" ");
        }

        public static void WriteList<T>(this ISyntaxWriter writer, IEnumerable<T> list, Action<T> writeItem, string delimiter = ",", bool addSpaceAfterDelimiter = true)
        {
            bool first = true;
            foreach (T t in list)
            {
                if (!first)
                {
                    writer.WriteSymbol(delimiter);
                    if (addSpaceAfterDelimiter)
                        writer.WriteSpace();
                }

                writeItem(t);

                first = false;
            }
        }

        public static void WriteSyntaxToken(this ISyntaxWriter writer, SyntaxToken token)
        {
            switch (token.Type)
            {
                default:
                case SyntaxTokenType.Literal:
                    writer.Write(token.Token); break;
                case SyntaxTokenType.Symbol:
                    writer.WriteSymbol(token.Token); break;
                case SyntaxTokenType.Identifier:
                    writer.WriteIdentifier(token.Token); break;
                case SyntaxTokenType.Keyword:
                    writer.WriteKeyword(token.Token); break;
                case SyntaxTokenType.TypeName:
                    writer.WriteTypeName(token.Token); break;
            }
        }

        public static void WriteSyntaxTokens(this ISyntaxWriter writer, IEnumerable<SyntaxToken> tokens)
        {
            foreach (SyntaxToken token in tokens)
                WriteSyntaxToken(writer, token);
        }

        public static IDisposable StartBraceBlock(this ISyntaxWriter writer)
        {
            return StartBraceBlock(writer, false);
        }

        public static IDisposable StartBraceBlock(this ISyntaxWriter writer, bool onNewLine)
        {
            if (onNewLine)
            {
                writer.WriteLine();
            }
            else
            {
                writer.WriteSpace();
            }

            writer.WriteSymbol("{");
            writer.WriteLine();
            writer.IndentLevel++;
            return new Block(() =>
                {
                    writer.IndentLevel--;
                    writer.WriteSymbol("}");
                    writer.WriteLine();
                });
        }

        private class Block : IDisposable
        {
            private readonly Action _endBlock;

            public Block(Action endBlock)
            {
                _endBlock = endBlock;
            }

            public void Dispose()
            {
                _endBlock();
            }
        }
    }
}
