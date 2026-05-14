// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using XliffTasks.Model;
using Xunit;

namespace XliffTasks.Tests
{
    public class JsonDocumentTests
    {
        [Fact]
        public void BasicLoadAndTranslate()
        {
            string source = """
                {
                  "Command1": "Hello!",
                  "Command2": "Goodbye!"
                }
                """;

            Dictionary<string, string> translations = new()
            {
                ["Command1"] = "Bonjour!",
                ["Command2"] = "Au revoir!",
            };

            string expectedTranslation = """
                {
                  "Command1": "Bonjour!",
                  "Command2": "Au revoir!"
                }
                """;

            JsonDocument document = new();
            StringWriter writer = new();
            document.Load(new StringReader(source));
            document.Translate(translations);
            document.Save(writer);

            AssertEx.EqualIgnoringLineEndings(expectedTranslation, writer.ToString());
        }
    }
}
