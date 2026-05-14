// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using XliffTasks.Model;
using Xunit;

namespace XliffTasks.Tests
{
    public class UnstructuredDocumentTests
    {
        [Fact]
        public void BasicLoadAndTranslate()
        {
            string source =
@"
Say hello: @@@idhello|Hello@@@<end>
Say goodbye: @@@idgoodbye|Goodbye@@@<end>
";

            Dictionary<string, string> translations = new()
            {
                ["idhello"] = "Bonjour!",
                ["idgoodbye"] = "Au revoir!",
            };

            string expectedTranslation =
@"
Say hello: Bonjour!<end>
Say goodbye: Au revoir!<end>
";

            UnstructuredDocument document = new();
            StringWriter writer = new();
            document.Load(new StringReader(source));
            document.Translate(translations);
            document.Save(writer);

            AssertEx.EqualIgnoringLineEndings(expectedTranslation, writer.ToString());
        }

        [Fact]
        public void SourceEndsWithTranslatableSpan()
        {
            string source = "@@@idhello|Hello@@@";

            Dictionary<string, string> translations = new()
            {
                ["idhello"] = "Bonjour!",
            };

            string expectedTranslation = "Bonjour!";

            UnstructuredDocument document = new();
            StringWriter writer = new();
            document.Load(new StringReader(source));
            document.Translate(translations);
            document.Save(writer);

            AssertEx.EqualIgnoringLineEndings(expectedTranslation, writer.ToString());
        }
    }
}
