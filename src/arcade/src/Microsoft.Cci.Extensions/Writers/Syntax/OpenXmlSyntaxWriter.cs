// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;
using System.Net;

namespace Microsoft.Cci.Writers.Syntax
{
    public class OpenXmlSyntaxWriter : IndentionSyntaxWriter, IStyleSyntaxWriter
    {
        private IDisposable _document;
        private IDisposable _paragraph;
        private readonly StyleHelper _styles;

        public OpenXmlSyntaxWriter(TextWriter writer)
            : base(writer)
        {
            _document = StartDocument();
            _paragraph = StartParagraph();
            _styles = new StyleHelper();
        }

        public void Write(string str)
        {
            WriteText(str);
        }

        public IDisposable StartStyle(SyntaxStyle style, object context)
        {
            IDisposable disposeAction = null;
            switch (style)
            {
                case SyntaxStyle.Added:
                    disposeAction = _styles.SetColor("green");
                    break;

                case SyntaxStyle.Removed:
                    disposeAction = _styles.SetColor("red");
                    break;

                case SyntaxStyle.InheritedMember:
                case SyntaxStyle.InterfaceMember:
                case SyntaxStyle.Comment:
                    disposeAction = _styles.SetColor("gray");
                    break;
                case SyntaxStyle.NotCompatible:
                    disposeAction = _styles.SetBgColor("yellow");
                    break;

                default:
                    throw new NotSupportedException("Style not supported!");
            }

            Contract.Assert(disposeAction != null);
            return new DisposeAction(() => disposeAction.Dispose());
        }

        public void WriteSymbol(string symbol)
        {
            WriteText(symbol);
        }

        public void WriteIdentifier(string id)
        {
            WriteText(id);
        }

        public void WriteKeyword(string keyword)
        {
            WriteText(keyword, "Blue");
        }

        public void WriteTypeName(string typeName)
        {
            WriteText(typeName, "2B91AF");
        }

        protected override void WriteLine(TextWriter writer)
        {
            _paragraph.Dispose();
            writer.WriteLine();
            _paragraph = StartParagraph();
        }

        protected override void WriteIndent(TextWriter writer, string indent)
        {
            writer.Write("<w:r><w:t>{0}</w:t></w:r>", indent);
        }

        public void Dispose()
        {
            if (_paragraph != null)
            {
                _paragraph.Dispose();
                _paragraph = null;
            }

            if (_document != null)
            {
                _document.Dispose();
                _document = null;
            }
        }

        private void WriteRunStyles()
        {
            if (!_styles.HasStyle)
                return;

            WriteCore("<w:rPr>");

            if (_styles.Color != null)
                WriteCore("<w:color w:val='{0}' />", _styles.Color);

            if (_styles.BgColor != null)
                WriteCore("<w:highlight w:val='{0}' />", _styles.BgColor);

            WriteCore("</w:rPr>");
        }

        private void WriteStyleId(string styleId)
        {
            WriteCore(string.Format("<w:rStyle  w:val='{0}' />", styleId));
        }

        private void WriteText(string text, string color = null)
        {
            using (_styles.SetColor(color))
            {
                WriteCore("<w:r>");
                WriteRunStyles();
                WriteCore("<w:t>");
                WriteCore(WebUtility.HtmlEncode(text));
                WriteCore("</w:t>");
                WriteCore("</w:r>");
            }
        }

        private IDisposable StartParagraph()
        {
            WriteCore("<w:p>");
            WriteCore("<w:pPr>");
            WriteCore("<w:pStyle  w:val='Code' />");
            WriteCore("</w:pPr>");

            return new DisposeAction(() => WriteCore("</w:p>"));
        }

        private IDisposable StartDocument()
        {
            // Document Header
            WriteCore(@"<?xml version=""1.0""?>
<?mso-application progid='Word.Document'?>
<w:wordDocument 
   xmlns:w='http://schemas.microsoft.com/office/word/2003/wordml' xml:space='preserve'>");

            // Document Styles
            WriteCore(@"
<w:docPr>
    <w:view w:val='print'/>
</w:docPr>
<w:fonts>
    <w:defaultFonts w:ascii='Consolas' w:hAnsi='Consolas'/>
</w:fonts>
<w:styles>
    <w:style w:type='paragraph' w:styleId='Code'>
      <w:rPr>
        <w:noProof />
      </w:rPr>
    </w:style>
</w:styles>");

            WriteCore("<w:body>");

            return new DisposeAction(() =>
            {
                WriteCore(@"</w:body></w:wordDocument>");
            });
        }
    }
}
