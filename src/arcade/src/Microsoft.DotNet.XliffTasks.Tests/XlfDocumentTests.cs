// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using XliffTasks.Model;
using Xunit;

namespace XliffTasks.Tests
{
    public class XlfDocumentTests
    {
        [Fact]
        public void LoadNewInitializesNewDocumentWithCorrectContent()
        {
            XlfDocument xliffDocument = new();
            xliffDocument.LoadNew("es");

            StringWriter writer = new();
            xliffDocument.Save(writer);

            string expected =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""es"" original=""_"">
    <body />
  </file>
</xliff>";

            AssertEx.EqualIgnoringLineEndings(expected, writer.ToString());
        }

        [Fact]
        public void UpdateBehavesCorrectlyAsSourceDocumentEvolves()
        {
            // dev authors new resx with no corresponding xliff
            string resx =
@"<root>
  <data name=""Hello"">
    <value>Hello!</value>
    <comment>Greeting</comment>
  </data>
  <data name=""Goodbye"">
    <value>Goodbye!</value>
  </data>
  <data name=""Apple"">
   <value>Apple</value>
   <comment>Tasty</comment>
  </data>
</root>";

            string xliff =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <target state=""new"">Apple</target>
        <note>Tasty</note>
      </trans-unit>
      <trans-unit id=""Goodbye"">
        <source>Goodbye!</source>
        <target state=""new"">Goodbye!</target>
        <note />
      </trans-unit>
      <trans-unit id=""Hello"">
        <source>Hello!</source>
        <target state=""new"">Hello!</target>
        <note>Greeting</note>
      </trans-unit>
    </body>
  </file>
</xliff>";
            AssertEx.EqualIgnoringLineEndings(xliff, Update(xliff: "", resx: resx));

            // loc team translates
            string xliffAfterFirstTranslation =
 @"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <target state=""translated"">Apple</target>
        <note>Tasty</note>
      </trans-unit>
      <trans-unit id=""Goodbye"">
        <source>Goodbye!</source>
        <target state=""translated"">Au revoir!</target>
        <note />
      </trans-unit>
      <trans-unit id=""Hello"">
        <source>Hello!</source>
        <target state=""translated"">Bonjour!</target>
        <note>Greeting</note>
      </trans-unit>
    </body>
  </file>
</xliff>";

            // dev makes some changes
            string resxAfterFirstModification =
@"<root>
  <data name=""HelloWorld"">
    <value>Hello World!</value>
  </data>
  <data name=""Goodbye"" xml:space=""preserve"">
    <value>Goodbye World!</value>
  </data>
  <data name=""Apple"">
    <value>Apple</value>
    <comment>This is referring to fruit.</comment>
  </data>
  <data name=""Banana"">
    <value>Banana</value>
  </data>
</root>";

            string xliffAfterApplyingResxModification =
 @"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <target state=""needs-review-translation"">Apple</target>
        <note>This is referring to fruit.</note>
      </trans-unit>
      <trans-unit id=""Banana"">
        <source>Banana</source>
        <target state=""new"">Banana</target>
        <note />
      </trans-unit>
      <trans-unit id=""Goodbye"">
        <source>Goodbye World!</source>
        <target state=""needs-review-translation"">Au revoir!</target>
        <note />
      </trans-unit>
      <trans-unit id=""HelloWorld"">
        <source>Hello World!</source>
        <target state=""new"">Hello World!</target>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";

            AssertEx.EqualIgnoringLineEndings(
               xliffAfterApplyingResxModification,
               Update(xliff: xliffAfterFirstTranslation, resx: resxAfterFirstModification));
        }

        [Fact]
        public void UpdateBehavesCorrectlyWhenTargetIsMissing()
        {
            string initialXliff =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <note />
      </trans-unit>
      <trans-unit id=""Banana"">
        <source>Banana</source>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";

            string resxWithModifications =
@"<root>
  <data name=""Apple"">
   <value>Better apples</value>
  </data>
  <data name=""Banana"">
    <value>Banana</value>
  </data>
</root>";

            string xliffAfterUpdate =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Better apples</source>
        <target state=""new"">Better apples</target>
        <note />
      </trans-unit>
      <trans-unit id=""Banana"">
        <source>Banana</source>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";

            AssertEx.EqualIgnoringLineEndings(
                expected: xliffAfterUpdate,
                actual: Update(initialXliff, resxWithModifications));
        }

        [Fact]
        public void UpdateBehavesCorrectlyWhenNoteIsMissing()
        {
            string initialXliff =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <target state=""new"">Apple</target>
      </trans-unit>
      <trans-unit id=""Banana"">
        <source>Banana</source>
        <target state=""new"">Banana</target>
      </trans-unit>
    </body>
  </file>
</xliff>";

            string resxWithModifications =
@"<root>
  <data name=""Apple"">
   <value>Apple</value>
   <comment>Make sure they're green apples.</comment>
  </data>
  <data name=""Banana"">
    <value>Banana</value>
  </data>
</root>";

            string xliffAfterUpdate =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <target state=""new"">Apple</target>
        <note>Make sure they're green apples.</note>
      </trans-unit>
      <trans-unit id=""Banana"">
        <source>Banana</source>
        <target state=""new"">Banana</target>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";

            AssertEx.EqualIgnoringLineEndings(
                expected: xliffAfterUpdate,
                actual: Update(initialXliff, resxWithModifications));
        }

        [Fact]
        public void UpdateBehavesCorrectlyWhenTargetStateIsMissing()
        {
            string initialXliff =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <target>Apple</target>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";

            string resxWithModifications =
@"<root>
  <data name=""Apple"">
   <value>Better apples</value>
  </data>
</root>";

            string xliffAfterUpdate =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Better apples</source>
        <target state=""new"">Better apples</target>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";

            AssertEx.EqualIgnoringLineEndings(
                expected: xliffAfterUpdate,
                actual: Update(initialXliff, resxWithModifications));
        }

        [Fact]
        public void NewItemThatShouldBeLastEndsUpLast()
        {
            // Dev has just added "Zucchini" item to RESX
            string resx =
@"<root>
  <data name=""Hello"">
    <value>Hello!</value>
    <comment>Greeting</comment>
  </data>
  <data name=""Goodbye"">
    <value>Goodbye!</value>
  </data>
  <data name=""Apple"">
   <value>Apple</value>
   <comment>Tasty</comment>
  </data>
  <data name=""Zucchini"">
    <value>Zucchini</value>
    <comment>My children won't eat it.</comment>
  </data>
</root>";

            string xliffBeforeUpdate =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <target state=""new"">Apple</target>
        <note>Tasty</note>
      </trans-unit>
      <trans-unit id=""Goodbye"">
        <source>Goodbye!</source>
        <target state=""new"">Goodbye!</target>
        <note />
      </trans-unit>
      <trans-unit id=""Hello"">
        <source>Hello!</source>
        <target state=""new"">Hello!</target>
        <note>Greeting</note>
      </trans-unit>
    </body>
  </file>
</xliff>";

            string xliffAfterUpdate =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <target state=""new"">Apple</target>
        <note>Tasty</note>
      </trans-unit>
      <trans-unit id=""Goodbye"">
        <source>Goodbye!</source>
        <target state=""new"">Goodbye!</target>
        <note />
      </trans-unit>
      <trans-unit id=""Hello"">
        <source>Hello!</source>
        <target state=""new"">Hello!</target>
        <note>Greeting</note>
      </trans-unit>
      <trans-unit id=""Zucchini"">
        <source>Zucchini</source>
        <target state=""new"">Zucchini</target>
        <note>My children won't eat it.</note>
      </trans-unit>
    </body>
  </file>
</xliff>";

            AssertEx.EqualIgnoringLineEndings(
                xliffAfterUpdate,
                Update(xliff: xliffBeforeUpdate, resx: resx));

        }

        [Fact]
        public void CheckSorting()
        {
            string xliff =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Gamma"">
        <source>Hello!</source>
        <target state=""new"">Hello!</target>
        <note>Greeting</note>
      </trans-unit>
      <trans-unit id=""Beta"">
        <source>Goodbye!</source>
        <target state=""new"">Goodbye!</target>
        <note />
      </trans-unit>
      <trans-unit id=""Alpha"">
        <source>Apple</source>
        <target state=""new"">Apple</target>
        <note>Tasty</note>
      </trans-unit>
    </body>
  </file>
</xliff>";

            string xliffAfterSorting =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Alpha"">
        <source>Apple</source>
        <target state=""new"">Apple</target>
        <note>Tasty</note>
      </trans-unit>
      <trans-unit id=""Beta"">
        <source>Goodbye!</source>
        <target state=""new"">Goodbye!</target>
        <note />
      </trans-unit>
      <trans-unit id=""Gamma"">
        <source>Hello!</source>
        <target state=""new"">Hello!</target>
        <note>Greeting</note>
      </trans-unit>
    </body>
  </file>
</xliff>";

            AssertEx.EqualIgnoringLineEndings(
                xliffAfterSorting,
                Sort(xliff));
        }

        [Fact]
        public void UntranslatedResourceCount_Zero()
        {
            string xliff =
 @"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <target state=""translated"">Apple</target>
        <note>Tasty</note>
      </trans-unit>
      <trans-unit id=""Goodbye"">
        <source>Goodbye!</source>
        <target state=""translated"">Au revoir!</target>
        <note />
      </trans-unit>
      <trans-unit id=""Hello"">
        <source>Hello!</source>
        <target state=""translated"">Bonjour!</target>
        <note>Greeting</note>
      </trans-unit>
    </body>
  </file>
</xliff>";

            ISet<string> untranslatedResources = UntranslatedResources(xliff);
            Assert.Empty(untranslatedResources);
        }

        [Fact]
        public void UntranslatedResourceCount_Two()
        {
            string xliff =
 @"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Apple"">
        <source>Apple</source>
        <target state=""translated"">Apple</target>
        <note>Tasty</note>
      </trans-unit>
      <trans-unit id=""Goodbye"">
        <source>Goodbye!</source>
        <target state=""new"">Goodbye!</target>
        <note />
      </trans-unit>
      <trans-unit id=""Hello"">
        <source>Hello!</source>
        <target state=""needs-review-translation"">Hallo!</target>
        <note>Greeting</note>
      </trans-unit>
    </body>
  </file>
</xliff>";

            ISet<string> untranslatedResources = UntranslatedResources(xliff);
            Assert.Contains("Goodbye", untranslatedResources, StringComparer.Ordinal);
            Assert.Contains("Hello", untranslatedResources, StringComparer.Ordinal);
            Assert.DoesNotContain("Apple", untranslatedResources, StringComparer.Ordinal);
        }

        [Fact]
        public void ResetTranslationOnMismatchedPlaceholders()
        {
            // Dev has just added additional placeholders to items Alpha and Beta.
            // Gamma already had a placeholder and is not being changed.

            string resx =
@"<root>
  <data name=""Alpha"">
    <value>Alpha {0}</value>
  </data>
  <data name=""Beta"">
    <value>Beta {0} {1}</value>
  </data>
  <data name=""Gamma"">
    <value>Gamma {0}</value>
  </data>
</root>";

            string xliffBeforeUpdate =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Alpha"">
        <source>Alpha</source>
        <target state=""translated"">Translated Alpha</target>
        <note />
      </trans-unit>
      <trans-unit id=""Beta"">
        <source>Beta {0}</source>
        <target state=""translated"">Translated Beta {0}</target>
        <note />
      </trans-unit>
      <trans-unit id=""Gamma"">
        <source>Gamma {0}</source>
        <target state=""translated"">Translated Gamma {0}</target>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";

            string xliffAfterUpdate =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Alpha"">
        <source>Alpha {0}</source>
        <target state=""new"">Alpha {0}</target>
        <note />
      </trans-unit>
      <trans-unit id=""Beta"">
        <source>Beta {0} {1}</source>
        <target state=""new"">Beta {0} {1}</target>
        <note />
      </trans-unit>
      <trans-unit id=""Gamma"">
        <source>Gamma {0}</source>
        <target state=""translated"">Translated Gamma {0}</target>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";

            AssertEx.EqualIgnoringLineEndings(
                xliffAfterUpdate,
                Update(xliff: xliffBeforeUpdate, resx: resx));

        }

        [Fact]
        public void ValidationReportsNoErrorsOnDocumentWithNoContent()
        {
            XlfDocument document = new();
            List<XmlSchemaException> validationErrors = GetValidationErrors(document);

            Assert.Empty(validationErrors);
        }

        [Fact]
        public void ValidationReportsNoErrorsOnNewDocument()
        {
            XlfDocument document = new();
            document.LoadNew("cs");
            List<XmlSchemaException> validationErrors = GetValidationErrors(document);

            Assert.Empty(validationErrors);
        }

        [Fact]
        public void ValidationReportsErrorsOnMissingSourceElement()
        {
            string xliffText =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.resx"">
    <body>
      <trans-unit id=""Alpha"">
        <target state=""new"">Alpha {0}</target>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";
            XlfDocument document = new();
            document.Load(new StringReader(xliffText));

            List<XmlSchemaException> validationErrors = GetValidationErrors(document);

            Assert.Collection(validationErrors,
                new Action<XmlSchemaException>[]
                {
                    e => Assert.Equal(expected: "The element 'trans-unit' in namespace 'urn:oasis:names:tc:xliff:document:1.2' has invalid child element 'target' in namespace 'urn:oasis:names:tc:xliff:document:1.2'. List of possible elements expected: 'source' in namespace 'urn:oasis:names:tc:xliff:document:1.2'.", actual: e.Message)
                });
        }

        private static List<XmlSchemaException> GetValidationErrors(XlfDocument document)
        {
            List<XmlSchemaException> validationErrors = new();
            void exceptionHandler(XmlSchemaException e) => validationErrors.Add(e);
            document.Validate(exceptionHandler);
            return validationErrors;
        }

        private static string Sort(string xliff)
        {
            XlfDocument xliffDocument = new();
            xliffDocument.Load(new StringReader(xliff));

            xliffDocument.Sort();

            StringWriter writer = new();
            xliffDocument.Save(writer);
            return writer.ToString();
        }

        private static string Update(string xliff, string resx)
        {
            XlfDocument xliffDocument = new();

            if (string.IsNullOrEmpty(xliff))
            {
                xliffDocument.LoadNew("fr");
            }
            else
            {
                xliffDocument.Load(new StringReader(xliff));
            }

            ResxDocument resxDocument = new();
            resxDocument.Load(new StringReader(resx));
            xliffDocument.Update(resxDocument, "test.resx");

            StringWriter writer = new();
            xliffDocument.Save(writer);
            return writer.ToString();
        }

        private static ISet<string> UntranslatedResources(string xliff)
        {
            XlfDocument xliffDocument = new();
            xliffDocument.Load(new StringReader(xliff));

            return xliffDocument.GetUntranslatedResourceIDs();
        }
    }
}
