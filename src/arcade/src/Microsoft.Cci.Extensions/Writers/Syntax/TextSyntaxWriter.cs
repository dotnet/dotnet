// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Text;

namespace Microsoft.Cci.Writers.Syntax
{
    public class TextSyntaxWriter : IndentionSyntaxWriter, IStyleSyntaxWriter
    {
        public TextSyntaxWriter(TextWriter writer)
            : base(writer)
        {
        }

        public IDisposable StartStyle(SyntaxStyle style, object context)
        {
            IDisposable disposeAction = null;
            switch (style)
            {
                case SyntaxStyle.Added:
                    disposeAction = WriteVersion("2");
                    break;

                case SyntaxStyle.Removed:
                    disposeAction = WriteVersion("1");
                    break;

                case SyntaxStyle.InterfaceMember:
                case SyntaxStyle.InheritedMember:
                case SyntaxStyle.Comment:
                    disposeAction = null;
                    break;

                case SyntaxStyle.NotCompatible:
                    disposeAction = null;
                    break;

                default:
                    throw new NotSupportedException("Style not supported!");
            }

            if (disposeAction == null)
                return new DisposeAction(() => { });

            return new DisposeAction(() => disposeAction.Dispose());
        }

        public void Write(string str)
        {
            WriteCore(str);
        }

        public void WriteSymbol(string symbol)
        {
            WriteCore(symbol);
        }

        public void WriteKeyword(string keyword)
        {
            WriteCore(keyword);
        }

        public void WriteIdentifier(string id)
        {
            WriteCore(id);
        }

        public void WriteTypeName(string typeName)
        {
            WriteCore(typeName);
        }

        private IDisposable WriteVersion(string version)
        {
            Write("[" + version + " ");
            return new DisposeAction(() => Write(" " + version + "]"));
        }

        public void Dispose()
        {
        }
    }
}
