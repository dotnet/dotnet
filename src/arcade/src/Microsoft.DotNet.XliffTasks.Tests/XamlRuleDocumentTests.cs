// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.IO;
using XliffTasks.Model;
using Xunit;

namespace XliffTasks.Tests
{
    public class XamlRuleTranslationTests
    {
      [Fact]
        public void BasicLoadAndTranslate()
        {
            string source =
@"<Rule Name=""MyRule""
        DisplayName=""My rule display name""
        PageTemplate=""generic""
        Description=""My rule description""
        xmlns=""http://schemas.microsoft.com/build/2009/properties"" xmlns:xliff=""https://github.com/dotnet/xliff-tasks"" xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
        mc:Ignorable=""xliff"">
  <!-- DisplayName: My rule display name comment -->
  <!-- Description: My rule description comment -->
  <Rule.Categories>
    <Category Name=""MyCategory"" DisplayName=""My category display name"">
      <!-- DisplayName: My category display name comment -->
    </Category>
  </Rule.Categories>
  <EnumProperty Name=""MyEnumProperty"" DisplayName=""My enum property display name"" Category=""MyCategory"" Description=""Specifies the source file will be copied to the output directory."">
    <!-- DisplayName: My enum property display name comment -->
    <!-- Description: My enum property description comment -->
    <EnumValue Name=""First"" DisplayName=""Do the first thing"">
      <!-- DisplayName: My first item comment -->
    </EnumValue>
    <EnumValue Name=""Second"" DisplayName=""Do the second thing"" />
    <EnumValue Name=""Third"" DisplayName=""Do the third thing"" />
  </EnumProperty>
  <BoolProperty Name=""MyBoolProperty"" Description=""My bool property description."" />
  <StringProperty Name=""MyStringProperty"">
    <StringProperty.Metadata>
      <NameValuePair Name=""SearchTerms"" Value=""My;Search;Terms"" TranslatableProp1=""tr1"" TranslatableProp2=""tr2"" NonTranslatableProp3=""same"" xliff:LocalizedProperties=""TranslatableProp1;TranslatableProp2"">
        <!-- Value: My search terms comment -->
      </NameValuePair>
      <NameValuePair Name=""TypeDescriptorText"" Value=""Custom symbols"" xliff:LocalizedProperties=""Value"">
        <!-- Value: My type descriptor text comment -->
      </NameValuePair>
    </StringProperty.Metadata>
  </StringProperty>
</Rule>";

            string expectedXlf =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.xaml"">
    <body>
      <trans-unit id=""BoolProperty|MyBoolProperty|Description"">
        <source>My bool property description.</source>
        <target state=""new"">My bool property description.</target>
        <note />
      </trans-unit>
      <trans-unit id=""Category|MyCategory|DisplayName"">
        <source>My category display name</source>
        <target state=""new"">My category display name</target>
        <note>My category display name comment</note>
      </trans-unit>
      <trans-unit id=""EnumProperty|MyEnumProperty|Description"">
        <source>Specifies the source file will be copied to the output directory.</source>
        <target state=""new"">Specifies the source file will be copied to the output directory.</target>
        <note>My enum property description comment</note>
      </trans-unit>
      <trans-unit id=""EnumProperty|MyEnumProperty|DisplayName"">
        <source>My enum property display name</source>
        <target state=""new"">My enum property display name</target>
        <note>My enum property display name comment</note>
      </trans-unit>
      <trans-unit id=""EnumValue|MyEnumProperty.First|DisplayName"">
        <source>Do the first thing</source>
        <target state=""new"">Do the first thing</target>
        <note>My first item comment</note>
      </trans-unit>
      <trans-unit id=""EnumValue|MyEnumProperty.Second|DisplayName"">
        <source>Do the second thing</source>
        <target state=""new"">Do the second thing</target>
        <note />
      </trans-unit>
      <trans-unit id=""EnumValue|MyEnumProperty.Third|DisplayName"">
        <source>Do the third thing</source>
        <target state=""new"">Do the third thing</target>
        <note />
      </trans-unit>
      <trans-unit id=""Rule|MyRule|Description"">
        <source>My rule description</source>
        <target state=""new"">My rule description</target>
        <note>My rule description comment</note>
      </trans-unit>
      <trans-unit id=""Rule|MyRule|DisplayName"">
        <source>My rule display name</source>
        <target state=""new"">My rule display name</target>
        <note>My rule display name comment</note>
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|Metadata|SearchTerms"">
        <source>My;Search;Terms</source>
        <target state=""new"">My;Search;Terms</target>
        <note>My search terms comment</note>
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp1"">
        <source>tr1</source>
        <target state=""new"">tr1</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp2"">
        <source>tr2</source>
        <target state=""new"">tr2</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|Metadata|TypeDescriptorText|Value"">
        <source>Custom symbols</source>
        <target state=""new"">Custom symbols</target>
        <note>My type descriptor text comment</note>
      </trans-unit>
    </body>
  </file>
</xliff>";

            Dictionary<string, string> translations = new()
            {
                ["Rule|MyRule|DisplayName"] = "AAA",
                ["Rule|MyRule|Description"] = "BBB",
                ["Category|MyCategory|DisplayName"] = "CCC",
                ["EnumProperty|MyEnumProperty|DisplayName"] = "DDD",
                ["EnumProperty|MyEnumProperty|Description"] = "EEE",
                ["EnumValue|MyEnumProperty.First|DisplayName"] = "FFF",
                ["EnumValue|MyEnumProperty.Second|DisplayName"] = "GGG",
                ["EnumValue|MyEnumProperty.Third|DisplayName"] = "HHH",
                ["BoolProperty|MyBoolProperty|Description"] = "III",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms"] = "JJJ",
                ["StringProperty|MyStringProperty|Metadata|TypeDescriptorText|Value"] = "NNN",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp1"] = "LLL",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp2"] = "MMM",
            };

            string expectedTranslation =
@"<Rule Name=""MyRule"" DisplayName=""AAA"" PageTemplate=""generic"" Description=""BBB"" xmlns=""http://schemas.microsoft.com/build/2009/properties"" xmlns:xliff=""https://github.com/dotnet/xliff-tasks"" xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006"" mc:Ignorable=""xliff"">
  <!-- DisplayName: My rule display name comment -->
  <!-- Description: My rule description comment -->
  <Rule.Categories>
    <Category Name=""MyCategory"" DisplayName=""CCC"">
      <!-- DisplayName: My category display name comment -->
    </Category>
  </Rule.Categories>
  <EnumProperty Name=""MyEnumProperty"" DisplayName=""DDD"" Category=""MyCategory"" Description=""EEE"">
    <!-- DisplayName: My enum property display name comment -->
    <!-- Description: My enum property description comment -->
    <EnumValue Name=""First"" DisplayName=""FFF"">
      <!-- DisplayName: My first item comment -->
    </EnumValue>
    <EnumValue Name=""Second"" DisplayName=""GGG"" />
    <EnumValue Name=""Third"" DisplayName=""HHH"" />
  </EnumProperty>
  <BoolProperty Name=""MyBoolProperty"" Description=""III"" />
  <StringProperty Name=""MyStringProperty"">
    <StringProperty.Metadata>
      <NameValuePair Name=""SearchTerms"" Value=""JJJ"" TranslatableProp1=""LLL"" TranslatableProp2=""MMM"" NonTranslatableProp3=""same"" xliff:LocalizedProperties=""TranslatableProp1;TranslatableProp2"">
        <!-- Value: My search terms comment -->
      </NameValuePair>
      <NameValuePair Name=""TypeDescriptorText"" Value=""NNN"" xliff:LocalizedProperties=""Value"">
        <!-- Value: My type descriptor text comment -->
      </NameValuePair>
    </StringProperty.Metadata>
  </StringProperty>
</Rule>";

            RunXamlTranslationTest(source, translations, expectedTranslation, expectedXlf);
        }

        
        /* the purpose of this test is to ensure that deeply nested translations translate as expected, as well as attributes declared as a nested element:
          such as:
          <A Name="MyName">
            <A.Description>MyName Description</A.Description>
            <B>
              <C>
                <D>
                  <NameValuePair Name="SearchTerms" TranslatableProp1="this-is-translated" xliff:LocalizedProperties="TranslatableProp1">
                    <NameValuePair.Value>My Search Terms</NameValuePair.Value> 
                </D>
                ....
    
        */
        [Fact]
        public void LoadAndTranslateWithDeeplyNestedTranslationAndElementAttributeSyntax()
        {
            string source =
@"<Rule Name=""MyRule""
        DisplayName=""My rule display name""
        PageTemplate=""generic""
        Description=""My rule description""
        xmlns=""http://schemas.microsoft.com/build/2009/properties"" xmlns:xliff=""https://github.com/dotnet/xliff-tasks"" xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
        mc:Ignorable=""xliff"">
  <!-- DisplayName: My rule display name comment -->
  <!-- Description: My rule description comment -->
  <Rule.Categories>
    <Category Name=""MyCategory"" Description=""My category description"">
      <Category.DisplayName>My category display name</Category.DisplayName>
      <!-- DisplayName: My category display name comment -->
    </Category>
  </Rule.Categories>
  <StringProperty Name=""MyStringProperty"" DisplayName=""MyStringProperty display name"">
    <StringProperty.Description>MyStringProperty description</StringProperty.Description>
    <StringProperty.ValueEditors>
      <ValueEditor EditorType=""MultiStringSelector"">
        <ValueEditor.Metadata>
          <NameValuePair Name=""TypeDescriptorText"" Value=""Custom symbols"" xliff:LocalizedProperties=""Value"" />
          <NameValuePair Name=""SearchTerms"">
            <NameValuePair.Value>My search terms</NameValuePair.Value>
          </NameValuePair>

          <TestElement Name=""TranslatableElement"" LocalizablePropC=""LocalizableC attribute"" xliff:LocalizedProperties=""LocalizablePropA;LocalizablePropB;LocalizablePropC"">
            <TestElement.LocalizablePropA>LocalizableA descendent prop</TestElement.LocalizablePropA>
            <TestElement.LocalizablePropB>LocalizableB descendent prop</TestElement.LocalizablePropB>
          </TestElement>

          <NameValuePair Name=""ShouldDisplayEvaluatedPreview"" Value=""True"" />
        </ValueEditor.Metadata>
      </ValueEditor>
    </StringProperty.ValueEditors>
  </StringProperty>
</Rule>";

            string expectedXlf =
@"<xliff xmlns=""urn:oasis:names:tc:xliff:document:1.2"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" version=""1.2"" xsi:schemaLocation=""urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2-transitional.xsd"">
  <file datatype=""xml"" source-language=""en"" target-language=""fr"" original=""test.xaml"">
    <body>
      <trans-unit id=""Category|MyCategory|Description"">
        <source>My category description</source>
        <target state=""new"">My category description</target>
        <note />
      </trans-unit>
      <trans-unit id=""Category|MyCategory|DisplayName"">
        <source>My category display name</source>
        <target state=""new"">My category display name</target>
        <note />
      </trans-unit>
      <trans-unit id=""Rule|MyRule|Description"">
        <source>My rule description</source>
        <target state=""new"">My rule description</target>
        <note>My rule description comment</note>
      </trans-unit>
      <trans-unit id=""Rule|MyRule|DisplayName"">
        <source>My rule display name</source>
        <target state=""new"">My rule display name</target>
        <note>My rule display name comment</note>
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|Description"">
        <source>MyStringProperty description</source>
        <target state=""new"">MyStringProperty description</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|DisplayName"">
        <source>MyStringProperty display name</source>
        <target state=""new"">MyStringProperty display name</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|ValueEditors|ValueEditor|Metadata|Metadata|LocalizablePropA"">
        <source>LocalizableA descendent prop</source>
        <target state=""new"">LocalizableA descendent prop</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|ValueEditors|ValueEditor|Metadata|Metadata|LocalizablePropB"">
        <source>LocalizableB descendent prop</source>
        <target state=""new"">LocalizableB descendent prop</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|ValueEditors|ValueEditor|Metadata|SearchTerms"">
        <source>My search terms</source>
        <target state=""new"">My search terms</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|ValueEditors|ValueEditor|Metadata|TranslatableElement|LocalizablePropC"">
        <source>LocalizableC attribute</source>
        <target state=""new"">LocalizableC attribute</target>
        <note />
      </trans-unit>
      <trans-unit id=""StringProperty|MyStringProperty|ValueEditors|ValueEditor|Metadata|TypeDescriptorText|Value"">
        <source>Custom symbols</source>
        <target state=""new"">Custom symbols</target>
        <note />
      </trans-unit>
    </body>
  </file>
</xliff>";

            Dictionary<string, string> translations = new()
            {
                ["Rule|MyRule|DisplayName"] = "AAA",
                ["Rule|MyRule|Description"] = "BBB",
                ["Category|MyCategory|DisplayName"] = "CCC",
                ["EnumProperty|MyEnumProperty|DisplayName"] = "DDD",
                ["EnumProperty|MyEnumProperty|Description"] = "EEE",
                ["EnumValue|MyEnumProperty.First|DisplayName"] = "FFF",
                ["EnumValue|MyEnumProperty.Second|DisplayName"] = "GGG",
                ["EnumValue|MyEnumProperty.Third|DisplayName"] = "HHH",
                ["BoolProperty|MyBoolProperty|Description"] = "III",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms"] = "JJJ",
                ["StringProperty|MyStringProperty|Metadata|TypeDescriptorText|Value"] = "NNN",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp1"] = "LLL",
                ["StringProperty|MyStringProperty|Metadata|SearchTerms|TranslatableProp2"] = "MMM",
            };

            string expectedTranslation =
@"<Rule Name=""MyRule"" DisplayName=""AAA"" PageTemplate=""generic"" Description=""BBB"" xmlns=""http://schemas.microsoft.com/build/2009/properties"" xmlns:xliff=""https://github.com/dotnet/xliff-tasks"" xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006"" mc:Ignorable=""xliff"">
  <!-- DisplayName: My rule display name comment -->
  <!-- Description: My rule description comment -->
  <Rule.Categories>
    <Category Name=""MyCategory"" Description=""My category description"">
      <Category.DisplayName>CCC</Category.DisplayName>
      <!-- DisplayName: My category display name comment -->
    </Category>
  </Rule.Categories>
  <StringProperty Name=""MyStringProperty"" DisplayName=""MyStringProperty display name"">
    <StringProperty.Description>MyStringProperty description</StringProperty.Description>
    <StringProperty.ValueEditors>
      <ValueEditor EditorType=""MultiStringSelector"">
        <ValueEditor.Metadata>
          <NameValuePair Name=""TypeDescriptorText"" Value=""Custom symbols"" xliff:LocalizedProperties=""Value"" />
          <NameValuePair Name=""SearchTerms"">
            <NameValuePair.Value>My search terms</NameValuePair.Value>
          </NameValuePair>
          <TestElement Name=""TranslatableElement"" LocalizablePropC=""LocalizableC attribute"" xliff:LocalizedProperties=""LocalizablePropA;LocalizablePropB;LocalizablePropC"">
            <TestElement.LocalizablePropA>LocalizableA descendent prop</TestElement.LocalizablePropA>
            <TestElement.LocalizablePropB>LocalizableB descendent prop</TestElement.LocalizablePropB>
          </TestElement>
          <NameValuePair Name=""ShouldDisplayEvaluatedPreview"" Value=""True"" />
        </ValueEditor.Metadata>
      </ValueEditor>
    </StringProperty.ValueEditors>
  </StringProperty>
</Rule>";

            RunXamlTranslationTest(source, translations, expectedTranslation, expectedXlf);
        }
        
        private static void RunXamlTranslationTest(string source, Dictionary<string, string> translations, string expectedTranslation, string expectedXlf)
        {
          XamlRuleDocument document = new();
          StringWriter writer = new();
          document.Load(new StringReader(source));

          XlfDocument xliffDocument = new();
          xliffDocument.LoadNew("fr");
          xliffDocument.Update(document, "test.xaml");

          document.Translate(translations);
          document.Save(writer);

          AssertEx.EqualIgnoringLineEndings(expectedTranslation, writer.ToString());

          StringWriter xliffWriter = new();
          xliffDocument.Save(xliffWriter);

          AssertEx.EqualIgnoringLineEndings(expectedXlf, xliffWriter.ToString());
        }
    }
}
